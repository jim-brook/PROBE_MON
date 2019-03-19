using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpPcap;
using SharpPcap.AirPcap;
using PacketDotNet;
using PacketDotNet.Ieee80211;
using System.Collections;
using System.Data.SqlClient;


namespace ProbeMon
{
    public partial class PacketParsing : Form
    {
        public FrameControl Context;
        public List<FrameControl.IncomingPacketDescriptor> PacketBytesQueue = new List<FrameControl.IncomingPacketDescriptor>();
        public object PacketBytesQueueLock = new object();
        public Queue PacketMetaDataQueue;
        public object PacketMetaDataQueueLock = new object();
        public Queue RadioPacketsQueue;
        public object RadioPacketsQueueLock = new object();  
        public EventWaitHandle PacketBytesReadyWait;
        public EventWaitHandle PacketDataReadyWait;
        public EventWaitHandle PacketMetaDataReadyWait;
        public Thread ProcessPacketBytesThread;
        public Thread ProcessPacketDataThread;
        public Thread ProcessPacketMetaDataThread;
        public string SQLConnectionString;
        public SqlConnection Connection;

        public PacketParsing(FrameControl FC)
        {
            InitializeComponent();
            Context = FC;
            PacketMetaDataQueue = new Queue();
            RadioPacketsQueue = new Queue(); 
            PacketBytesReadyWait = new EventWaitHandle(false, EventResetMode.AutoReset);
            PacketDataReadyWait = new EventWaitHandle(false, EventResetMode.AutoReset);
            PacketMetaDataReadyWait = new EventWaitHandle(false, EventResetMode.AutoReset);
            ProcessPacketMetaDataThread = new Thread(MetaDataToSql);
            ProcessPacketMetaDataThread.Start();
            ProcessPacketDataThread = new Thread(GetPacketMetaData);
            ProcessPacketDataThread.Start();
            ProcessPacketBytesThread = new Thread(AddPackets);
            ProcessPacketBytesThread.Start();
        }
        public void AddPackets()
        {
            while (Context.ProgramIsClosing == false)
            {                
                PacketBytesReadyWait.WaitOne();
                lock (PacketBytesQueueLock)
                {
                    foreach(FrameControl.IncomingPacketDescriptor FrameDesc in PacketBytesQueue)
                    {
                        Packet packet = Packet.ParsePacket(LinkLayers.Ieee80211_Radio, FrameDesc.RawPacket);
                        FrameControl.IncomingPacketDescriptor FrameDescriptor = new FrameControl.IncomingPacketDescriptor();
                        FrameDescriptor.packet = packet;
                        FrameDescriptor.PacketSource = FrameDesc.PacketSource;
                        FrameDescriptor.PortNo = FrameDesc.PortNo;
                        lock (RadioPacketsQueueLock)
                        {
                            RadioPacketsQueue.Enqueue(FrameDescriptor);                            
                        }

                    }
                    PacketBytesQueue.Clear();
                    PacketDataReadyWait.Set();
                }
            }
        }
        public void MetaDataExtract(FrameControl.IncomingPacketDescriptor FrameDesc)
        {
            Packet packet = FrameDesc.packet;
            if (packet.PayloadPacket == null) { Console.WriteLine("NULL Payload"); return; }
            int count = 0;
            char[] SSID = new char[32];
            bool FrameFound = false;
            RadioPlusMAC HeaderMetaData = new RadioPlusMAC();
            RadioPacket RadioHeader = packet as RadioPacket;
            MacFrame MACFrame = packet.PayloadPacket as MacFrame;
            ProbeRequestFrame PRframe = null;
            ProbeResponseFrame PRrespframe = null;
            AuthenticationFrame AuthFrame = null;
            AssociationRequestFrame AssocReqFrame = null;
            AssociationResponseFrame AssocResFrame = null;
            ReassociationRequestFrame ReAssocReqFrame = null;
            CtsFrame CtrlCTS = null;
            Console.WriteLine(MACFrame.ToString());
            if (MACFrame.FrameControl.Type == FrameControlField.FrameTypes.Management)
            {
                switch (MACFrame.FrameControl.SubType)
                {
                    case PacketDotNet.Ieee80211.FrameControlField.FrameSubTypes.ManagementProbeRequest:
                        PRframe = (ProbeRequestFrame)packet.PayloadPacket;
                        FrameFound = true;
                        break;
                    case PacketDotNet.Ieee80211.FrameControlField.FrameSubTypes.ManagementProbeResponse:
                        PRrespframe = (ProbeResponseFrame)packet.PayloadPacket;
                        FrameFound = true;
                        break;
                    case PacketDotNet.Ieee80211.FrameControlField.FrameSubTypes.ManagementAssociationResponse:
                        AssocResFrame = (AssociationResponseFrame)packet.PayloadPacket;
                        FrameFound = true;
                        break;
                    case PacketDotNet.Ieee80211.FrameControlField.FrameSubTypes.ManagementAssociationRequest:
                        AssocReqFrame = (AssociationRequestFrame)packet.PayloadPacket;
                        FrameFound = true;
                        break;
                    case PacketDotNet.Ieee80211.FrameControlField.FrameSubTypes.ManagementReassociationRequest:
                        ReAssocReqFrame = (ReassociationRequestFrame)packet.PayloadPacket;
                        FrameFound = true;
                        break;

                    default:
                        //Console.WriteLine(MACFrame.ToString());
                        FrameFound = false;
                        break;
                }
            }
            else if (MACFrame.FrameControl.Type == FrameControlField.FrameTypes.Control)
            {
                switch (MACFrame.FrameControl.SubType)
                {
                    case PacketDotNet.Ieee80211.FrameControlField.FrameSubTypes.ControlCTS:
                        CtrlCTS = (CtsFrame)packet.PayloadPacket;
                        FrameFound = true;
                        break;
                    default:
                        FrameFound = false;
                        //Console.WriteLine(macFrame.ToString());
                        break;
                }
            }
            if (CtrlCTS != null)
            {
                HeaderMetaData.DestinationAddr = CtrlCTS.ReceiverAddress.GetAddressBytes();
                HeaderMetaData.PropDstAddr = CtrlCTS.ReceiverAddress;
                HeaderMetaData.PropSrcAddr = CtrlCTS.ReceiverAddress;
                HeaderMetaData.SourceAddr = CtrlCTS.ReceiverAddress.GetAddressBytes();
                HeaderMetaData.BSSID = CtrlCTS.ReceiverAddress.GetAddressBytes();
                HeaderMetaData.PropBSSID = CtrlCTS.ReceiverAddress;

                HeaderMetaData.PropRadioTapAntNoiseDB = RadioHeader[RadioTapType.DbAntennaNoise] as DbAntennaNoiseRadioTapField;
                if (HeaderMetaData.PropRadioTapAntNoiseDB != null) { HeaderMetaData.RadioTapAntNoiseDB = HeaderMetaData.PropRadioTapAntNoiseDB.AntennaNoisedB; }//tinyint

                HeaderMetaData.PropRadioTapAntSignalDB = RadioHeader[RadioTapType.DbAntennaSignal] as DbAntennaSignalRadioTapField;
                if (HeaderMetaData.PropRadioTapAntSignalDB != null) { HeaderMetaData.RadioTapAntSignalDB = HeaderMetaData.PropRadioTapAntSignalDB.SignalStrengthdB; }//tinyint

                HeaderMetaData.PropRadioTapAntNoiseDBM = RadioHeader[RadioTapType.DbmAntennaNoise] as DbmAntennaNoiseRadioTapField;
                if (HeaderMetaData.PropRadioTapAntNoiseDBM != null) { HeaderMetaData.RadioTapAntNoiseDBM = HeaderMetaData.PropRadioTapAntNoiseDBM.AntennaNoisedBm; }//tinyint

                HeaderMetaData.PropRadioTapRssi = RadioHeader[RadioTapType.DbmAntennaSignal] as DbmAntennaSignalRadioTapField;
                if (HeaderMetaData.PropRadioTapRssi != null) { HeaderMetaData.RadioTapRSSI = HeaderMetaData.PropRadioTapRssi.AntennaSignalDbm; } //tinyint

                HeaderMetaData.PropRadioTapTxPowerDBM = RadioHeader[RadioTapType.DbmTxPower] as DbmTxPowerRadioTapField;
                if (HeaderMetaData.PropRadioTapTxPowerDBM != null) { HeaderMetaData.RadioTapTxPowerDBM = HeaderMetaData.PropRadioTapTxPowerDBM.TxPowerdBm; }//tinyint

                HeaderMetaData.PropRadioTapAntenna = RadioHeader[RadioTapType.Antenna] as AntennaRadioTapField;
                if (HeaderMetaData.PropRadioTapAntenna != null) { HeaderMetaData.RadioTapAntenna = HeaderMetaData.PropRadioTapAntenna.Antenna; }//tinyint

                HeaderMetaData.PropRadioTapChannel = RadioHeader[RadioTapType.Channel] as ChannelRadioTapField;
                if (HeaderMetaData.PropRadioTapChannel != null) { HeaderMetaData.RadioTapChannel = HeaderMetaData.PropRadioTapChannel.Channel; }//int


                if (AssocResFrame != null)
                {
                    HeaderMetaData.PropSrcAddr = AssocResFrame.SourceAddress;
                    HeaderMetaData.SourceAddr = AssocResFrame.SourceAddress.GetAddressBytes();
                    HeaderMetaData.PropDstAddr = AssocResFrame.DestinationAddress;
                    HeaderMetaData.DestinationAddr = AssocResFrame.DestinationAddress.GetAddressBytes();
                    HeaderMetaData.PropBSSID = AssocResFrame.BssId;
                    HeaderMetaData.BSSID = AssocResFrame.BssId.GetAddressBytes();//binary(6)
                    HeaderMetaData.PropRadioTapAntNoiseDB = RadioHeader[RadioTapType.DbAntennaNoise] as DbAntennaNoiseRadioTapField;
                    if (HeaderMetaData.PropRadioTapAntNoiseDB != null) { HeaderMetaData.RadioTapAntNoiseDB = HeaderMetaData.PropRadioTapAntNoiseDB.AntennaNoisedB; }//tinyint

                    HeaderMetaData.PropRadioTapAntSignalDB = RadioHeader[RadioTapType.DbAntennaSignal] as DbAntennaSignalRadioTapField;
                    if (HeaderMetaData.PropRadioTapAntSignalDB != null) { HeaderMetaData.RadioTapAntSignalDB = HeaderMetaData.PropRadioTapAntSignalDB.SignalStrengthdB; }//tinyint

                    HeaderMetaData.PropRadioTapAntNoiseDBM = RadioHeader[RadioTapType.DbmAntennaNoise] as DbmAntennaNoiseRadioTapField;
                    if (HeaderMetaData.PropRadioTapAntNoiseDBM != null) { HeaderMetaData.RadioTapAntNoiseDBM = HeaderMetaData.PropRadioTapAntNoiseDBM.AntennaNoisedBm; }//tinyint

                    HeaderMetaData.PropRadioTapRssi = RadioHeader[RadioTapType.DbmAntennaSignal] as DbmAntennaSignalRadioTapField;
                    if (HeaderMetaData.PropRadioTapRssi != null) { HeaderMetaData.RadioTapRSSI = HeaderMetaData.PropRadioTapRssi.AntennaSignalDbm; } //tinyint

                    HeaderMetaData.PropRadioTapTxPowerDBM = RadioHeader[RadioTapType.DbmTxPower] as DbmTxPowerRadioTapField;
                    if (HeaderMetaData.PropRadioTapTxPowerDBM != null) { HeaderMetaData.RadioTapTxPowerDBM = HeaderMetaData.PropRadioTapTxPowerDBM.TxPowerdBm; }//tinyint

                    HeaderMetaData.PropRadioTapAntenna = RadioHeader[RadioTapType.Antenna] as AntennaRadioTapField;
                    if (HeaderMetaData.PropRadioTapAntenna != null) { HeaderMetaData.RadioTapAntenna = HeaderMetaData.PropRadioTapAntenna.Antenna; }//tinyint

                    HeaderMetaData.PropRadioTapChannel = RadioHeader[RadioTapType.Channel] as ChannelRadioTapField;
                    if (HeaderMetaData.PropRadioTapChannel != null) { HeaderMetaData.RadioTapChannel = HeaderMetaData.PropRadioTapChannel.Channel; }//int
                }
                if (AssocReqFrame != null)
                {
                    HeaderMetaData.PropSrcAddr = AssocReqFrame.SourceAddress;
                    HeaderMetaData.SourceAddr = AssocReqFrame.SourceAddress.GetAddressBytes();
                    HeaderMetaData.PropDstAddr = AssocReqFrame.DestinationAddress;
                    HeaderMetaData.DestinationAddr = AssocReqFrame.DestinationAddress.GetAddressBytes();
                    HeaderMetaData.PropBSSID = AssocReqFrame.BssId;
                    HeaderMetaData.BSSID = AssocReqFrame.BssId.GetAddressBytes();//binary(6)
                    HeaderMetaData.PropRadioTapAntNoiseDB = RadioHeader[RadioTapType.DbAntennaNoise] as DbAntennaNoiseRadioTapField;
                    if (HeaderMetaData.PropRadioTapAntNoiseDB != null) { HeaderMetaData.RadioTapAntNoiseDB = HeaderMetaData.PropRadioTapAntNoiseDB.AntennaNoisedB; }//tinyint

                    HeaderMetaData.PropRadioTapAntSignalDB = RadioHeader[RadioTapType.DbAntennaSignal] as DbAntennaSignalRadioTapField;
                    if (HeaderMetaData.PropRadioTapAntSignalDB != null) { HeaderMetaData.RadioTapAntSignalDB = HeaderMetaData.PropRadioTapAntSignalDB.SignalStrengthdB; }//tinyint

                    HeaderMetaData.PropRadioTapAntNoiseDBM = RadioHeader[RadioTapType.DbmAntennaNoise] as DbmAntennaNoiseRadioTapField;
                    if (HeaderMetaData.PropRadioTapAntNoiseDBM != null) { HeaderMetaData.RadioTapAntNoiseDBM = HeaderMetaData.PropRadioTapAntNoiseDBM.AntennaNoisedBm; }//tinyint

                    HeaderMetaData.PropRadioTapRssi = RadioHeader[RadioTapType.DbmAntennaSignal] as DbmAntennaSignalRadioTapField;
                    if (HeaderMetaData.PropRadioTapRssi != null) { HeaderMetaData.RadioTapRSSI = HeaderMetaData.PropRadioTapRssi.AntennaSignalDbm; } //tinyint

                    HeaderMetaData.PropRadioTapTxPowerDBM = RadioHeader[RadioTapType.DbmTxPower] as DbmTxPowerRadioTapField;
                    if (HeaderMetaData.PropRadioTapTxPowerDBM != null) { HeaderMetaData.RadioTapTxPowerDBM = HeaderMetaData.PropRadioTapTxPowerDBM.TxPowerdBm; }//tinyint

                    HeaderMetaData.PropRadioTapAntenna = RadioHeader[RadioTapType.Antenna] as AntennaRadioTapField;
                    if (HeaderMetaData.PropRadioTapAntenna != null) { HeaderMetaData.RadioTapAntenna = HeaderMetaData.PropRadioTapAntenna.Antenna; }//tinyint

                    HeaderMetaData.PropRadioTapChannel = RadioHeader[RadioTapType.Channel] as ChannelRadioTapField;
                    if (HeaderMetaData.PropRadioTapChannel != null) { HeaderMetaData.RadioTapChannel = HeaderMetaData.PropRadioTapChannel.Channel; }//int
                }
                if (ReAssocReqFrame != null)
                {
                    HeaderMetaData.PropSrcAddr = ReAssocReqFrame.SourceAddress;
                    HeaderMetaData.SourceAddr = ReAssocReqFrame.SourceAddress.GetAddressBytes();
                    HeaderMetaData.PropDstAddr = ReAssocReqFrame.DestinationAddress;
                    HeaderMetaData.DestinationAddr = ReAssocReqFrame.DestinationAddress.GetAddressBytes();
                    HeaderMetaData.PropBSSID = ReAssocReqFrame.BssId;
                    HeaderMetaData.BSSID = ReAssocReqFrame.BssId.GetAddressBytes();//binary(6)
                    HeaderMetaData.PropRadioTapAntNoiseDB = RadioHeader[RadioTapType.DbAntennaNoise] as DbAntennaNoiseRadioTapField;
                    if (HeaderMetaData.PropRadioTapAntNoiseDB != null) { HeaderMetaData.RadioTapAntNoiseDB = HeaderMetaData.PropRadioTapAntNoiseDB.AntennaNoisedB; }//tinyint

                    HeaderMetaData.PropRadioTapAntSignalDB = RadioHeader[RadioTapType.DbAntennaSignal] as DbAntennaSignalRadioTapField;
                    if (HeaderMetaData.PropRadioTapAntSignalDB != null) { HeaderMetaData.RadioTapAntSignalDB = HeaderMetaData.PropRadioTapAntSignalDB.SignalStrengthdB; }//tinyint

                    HeaderMetaData.PropRadioTapAntNoiseDBM = RadioHeader[RadioTapType.DbmAntennaNoise] as DbmAntennaNoiseRadioTapField;
                    if (HeaderMetaData.PropRadioTapAntNoiseDBM != null) { HeaderMetaData.RadioTapAntNoiseDBM = HeaderMetaData.PropRadioTapAntNoiseDBM.AntennaNoisedBm; }//tinyint

                    HeaderMetaData.PropRadioTapRssi = RadioHeader[RadioTapType.DbmAntennaSignal] as DbmAntennaSignalRadioTapField;
                    if (HeaderMetaData.PropRadioTapRssi != null) { HeaderMetaData.RadioTapRSSI = HeaderMetaData.PropRadioTapRssi.AntennaSignalDbm; } //tinyint

                    HeaderMetaData.PropRadioTapTxPowerDBM = RadioHeader[RadioTapType.DbmTxPower] as DbmTxPowerRadioTapField;
                    if (HeaderMetaData.PropRadioTapTxPowerDBM != null) { HeaderMetaData.RadioTapTxPowerDBM = HeaderMetaData.PropRadioTapTxPowerDBM.TxPowerdBm; }//tinyint

                    HeaderMetaData.PropRadioTapAntenna = RadioHeader[RadioTapType.Antenna] as AntennaRadioTapField;
                    if (HeaderMetaData.PropRadioTapAntenna != null) { HeaderMetaData.RadioTapAntenna = HeaderMetaData.PropRadioTapAntenna.Antenna; }//tinyint

                    HeaderMetaData.PropRadioTapChannel = RadioHeader[RadioTapType.Channel] as ChannelRadioTapField;
                    if (HeaderMetaData.PropRadioTapChannel != null) { HeaderMetaData.RadioTapChannel = HeaderMetaData.PropRadioTapChannel.Channel; }//int
                }
                if (AuthFrame != null)
                {
                    HeaderMetaData.PropSrcAddr = AuthFrame.SourceAddress;
                    HeaderMetaData.SourceAddr = AuthFrame.SourceAddress.GetAddressBytes();
                    HeaderMetaData.PropDstAddr = AuthFrame.DestinationAddress;
                    HeaderMetaData.DestinationAddr = AuthFrame.DestinationAddress.GetAddressBytes();
                    HeaderMetaData.PropBSSID = AuthFrame.BssId;
                    HeaderMetaData.BSSID = AuthFrame.BssId.GetAddressBytes();//binary(6)
                    HeaderMetaData.PropRadioTapAntNoiseDB = RadioHeader[RadioTapType.DbAntennaNoise] as DbAntennaNoiseRadioTapField;
                    if (HeaderMetaData.PropRadioTapAntNoiseDB != null) { HeaderMetaData.RadioTapAntNoiseDB = HeaderMetaData.PropRadioTapAntNoiseDB.AntennaNoisedB; }//tinyint

                    HeaderMetaData.PropRadioTapAntSignalDB = RadioHeader[RadioTapType.DbAntennaSignal] as DbAntennaSignalRadioTapField;
                    if (HeaderMetaData.PropRadioTapAntSignalDB != null) { HeaderMetaData.RadioTapAntSignalDB = HeaderMetaData.PropRadioTapAntSignalDB.SignalStrengthdB; }//tinyint

                    HeaderMetaData.PropRadioTapAntNoiseDBM = RadioHeader[RadioTapType.DbmAntennaNoise] as DbmAntennaNoiseRadioTapField;
                    if (HeaderMetaData.PropRadioTapAntNoiseDBM != null) { HeaderMetaData.RadioTapAntNoiseDBM = HeaderMetaData.PropRadioTapAntNoiseDBM.AntennaNoisedBm; }//tinyint

                    HeaderMetaData.PropRadioTapRssi = RadioHeader[RadioTapType.DbmAntennaSignal] as DbmAntennaSignalRadioTapField;
                    if (HeaderMetaData.PropRadioTapRssi != null) { HeaderMetaData.RadioTapRSSI = HeaderMetaData.PropRadioTapRssi.AntennaSignalDbm; } //tinyint

                    HeaderMetaData.PropRadioTapTxPowerDBM = RadioHeader[RadioTapType.DbmTxPower] as DbmTxPowerRadioTapField;
                    if (HeaderMetaData.PropRadioTapTxPowerDBM != null) { HeaderMetaData.RadioTapTxPowerDBM = HeaderMetaData.PropRadioTapTxPowerDBM.TxPowerdBm; }//tinyint

                    HeaderMetaData.PropRadioTapAntenna = RadioHeader[RadioTapType.Antenna] as AntennaRadioTapField;
                    if (HeaderMetaData.PropRadioTapAntenna != null) { HeaderMetaData.RadioTapAntenna = HeaderMetaData.PropRadioTapAntenna.Antenna; }//tinyint

                    HeaderMetaData.PropRadioTapChannel = RadioHeader[RadioTapType.Channel] as ChannelRadioTapField;
                    if (HeaderMetaData.PropRadioTapChannel != null) { HeaderMetaData.RadioTapChannel = HeaderMetaData.PropRadioTapChannel.Channel; }//int
                }
                if (PRrespframe != null)
                {
                    HeaderMetaData.PropSrcAddr = PRrespframe.SourceAddress;
                    HeaderMetaData.SourceAddr = PRrespframe.SourceAddress.GetAddressBytes();
                    HeaderMetaData.PropDstAddr = PRrespframe.DestinationAddress;
                    HeaderMetaData.DestinationAddr = PRrespframe.DestinationAddress.GetAddressBytes();
                    HeaderMetaData.PropBSSID = PRrespframe.BssId;
                    HeaderMetaData.BSSID = PRrespframe.BssId.GetAddressBytes();//binary(6)
                    HeaderMetaData.PropRadioTapAntNoiseDB = RadioHeader[RadioTapType.DbAntennaNoise] as DbAntennaNoiseRadioTapField;
                    if (HeaderMetaData.PropRadioTapAntNoiseDB != null) { HeaderMetaData.RadioTapAntNoiseDB = HeaderMetaData.PropRadioTapAntNoiseDB.AntennaNoisedB; }//tinyint

                    HeaderMetaData.PropRadioTapAntSignalDB = RadioHeader[RadioTapType.DbAntennaSignal] as DbAntennaSignalRadioTapField;
                    if (HeaderMetaData.PropRadioTapAntSignalDB != null) { HeaderMetaData.RadioTapAntSignalDB = HeaderMetaData.PropRadioTapAntSignalDB.SignalStrengthdB; }//tinyint

                    HeaderMetaData.PropRadioTapAntNoiseDBM = RadioHeader[RadioTapType.DbmAntennaNoise] as DbmAntennaNoiseRadioTapField;
                    if (HeaderMetaData.PropRadioTapAntNoiseDBM != null) { HeaderMetaData.RadioTapAntNoiseDBM = HeaderMetaData.PropRadioTapAntNoiseDBM.AntennaNoisedBm; }//tinyint

                    HeaderMetaData.PropRadioTapRssi = RadioHeader[RadioTapType.DbmAntennaSignal] as DbmAntennaSignalRadioTapField;
                    if (HeaderMetaData.PropRadioTapRssi != null) { HeaderMetaData.RadioTapRSSI = HeaderMetaData.PropRadioTapRssi.AntennaSignalDbm; } //tinyint

                    HeaderMetaData.PropRadioTapTxPowerDBM = RadioHeader[RadioTapType.DbmTxPower] as DbmTxPowerRadioTapField;
                    if (HeaderMetaData.PropRadioTapTxPowerDBM != null) { HeaderMetaData.RadioTapTxPowerDBM = HeaderMetaData.PropRadioTapTxPowerDBM.TxPowerdBm; }//tinyint

                    HeaderMetaData.PropRadioTapAntenna = RadioHeader[RadioTapType.Antenna] as AntennaRadioTapField;
                    if (HeaderMetaData.PropRadioTapAntenna != null) { HeaderMetaData.RadioTapAntenna = HeaderMetaData.PropRadioTapAntenna.Antenna; }//tinyint

                    HeaderMetaData.PropRadioTapChannel = RadioHeader[RadioTapType.Channel] as ChannelRadioTapField;
                    if (HeaderMetaData.PropRadioTapChannel != null) { HeaderMetaData.RadioTapChannel = HeaderMetaData.PropRadioTapChannel.Channel; }//int

                    //LINUX pcap frames the SSID is (count+61-5)
                    if ((FrameDesc.PacketSource == FrameControl.CaptureType.LocalDevice) || (FrameDesc.PacketSource == FrameControl.CaptureType.RemoteWinDevice))
                    {
                        PacketDotNet.Ieee80211.InformationElementList InfoElementList = PRrespframe.InformationElements;
                        foreach (PacketDotNet.Ieee80211.InformationElement InfoElement in InfoElementList)
                        {
                            if (InfoElement.Id == InformationElement.ElementId.ServiceSetIdentity)
                            {
                                if (InfoElement.ValueLength > 0)
                                {
                                    char[] ssid = new char[InfoElement.Value.Length];
                                    Array.Copy(InfoElement.Value, ssid, InfoElement.Value.Length);
                                    string ServiceSID = new string(ssid);
                                    HeaderMetaData.PropProbeRequestSSID = ServiceSID;
                                    char TermTest = HeaderMetaData.PropProbeRequestSSID.ElementAt(0);
                                    if (TermTest == '\0')
                                    {
                                        HeaderMetaData.PropProbeRequestSSID = string.Empty;
                                    }
                                }
                                else
                                {
                                    HeaderMetaData.PropProbeRequestSSID = string.Empty;
                                }
                            }
                        }
#if FIXED_OFFSET_PARSE
                    for (count = 0; count < 32; count++)
                    {
                        if ((count + 61) >= MACFrame.BytesHighPerformance.Bytes.Length) { break; }
                        SSID[count] = Convert.ToChar(MACFrame.BytesHighPerformance.Bytes[count + 61]);
                        if ((MACFrame.BytesHighPerformance.Bytes[count + 61] < 32) || (MACFrame.BytesHighPerformance.Bytes[count + 61] > 126))
                        {
                            SSID[count] = '\n';
                            break;
                        }
                    }
#endif
                    }
                    else if (FrameDesc.PacketSource == FrameControl.CaptureType.RemoteLinuxDevice)//many variations in mmgmnt frame payloads + headers
                    {
                        PacketDotNet.Ieee80211.InformationElementList InfoElementList = PRrespframe.InformationElements;
                        foreach (PacketDotNet.Ieee80211.InformationElement InfoElement in InfoElementList)
                        {
                            if (InfoElement.Id == InformationElement.ElementId.ServiceSetIdentity)
                            {
                                if (InfoElement.ValueLength > 0)
                                {
                                    char[] ssid = new char[InfoElement.Value.Length];
                                    Array.Copy(InfoElement.Value, ssid, InfoElement.Value.Length);
                                    string ServiceSID = new string(ssid);
                                    HeaderMetaData.PropProbeRequestSSID = ServiceSID;
                                    char TermTest = HeaderMetaData.PropProbeRequestSSID.ElementAt(0);
                                    if (TermTest == '\0')
                                    {
                                        HeaderMetaData.PropProbeRequestSSID = string.Empty;
                                    }
                                }
                                else
                                {
                                    HeaderMetaData.PropProbeRequestSSID = string.Empty;
                                }
                            }
                        }
#if FIXED_OFFSET_PARSE
                    for (count = 0; count < 32; count++)
                    {
                        if ((count + 56) >= MACFrame.BytesHighPerformance.Bytes.Length) { break; }
                        SSID[count] = Convert.ToChar(MACFrame.BytesHighPerformance.Bytes[count + 56]);
                        if ((MACFrame.BytesHighPerformance.Bytes[count + 56] < 32) || (MACFrame.BytesHighPerformance.Bytes[count + 56] > 126))
                        {
                            SSID[count] = '\n';
                            break;
                        }
                    }
#endif
                    }

                    Array.Resize(ref SSID, count);
                    if (count > 0)
                    {
                        HeaderMetaData.PropProbeRequestSSID = new string(SSID);
                    }

                }
                if (PRframe != null)
                {
                    HeaderMetaData.PropSrcAddr = PRframe.SourceAddress;
                    HeaderMetaData.SourceAddr = PRframe.SourceAddress.GetAddressBytes();
                    HeaderMetaData.PropDstAddr = PRframe.DestinationAddress;
                    HeaderMetaData.DestinationAddr = PRframe.DestinationAddress.GetAddressBytes();
                    HeaderMetaData.PropBSSID = PRframe.BssId;
                    HeaderMetaData.BSSID = PRframe.BssId.GetAddressBytes();//binary(6)
                    HeaderMetaData.PropRadioTapAntNoiseDB = RadioHeader[RadioTapType.DbAntennaNoise] as DbAntennaNoiseRadioTapField;
                    if (HeaderMetaData.PropRadioTapAntNoiseDB != null) { HeaderMetaData.RadioTapAntNoiseDB = HeaderMetaData.PropRadioTapAntNoiseDB.AntennaNoisedB; }//tinyint

                    HeaderMetaData.PropRadioTapAntSignalDB = RadioHeader[RadioTapType.DbAntennaSignal] as DbAntennaSignalRadioTapField;
                    if (HeaderMetaData.PropRadioTapAntSignalDB != null) { HeaderMetaData.RadioTapAntSignalDB = HeaderMetaData.PropRadioTapAntSignalDB.SignalStrengthdB; }//tinyint

                    HeaderMetaData.PropRadioTapAntNoiseDBM = RadioHeader[RadioTapType.DbmAntennaNoise] as DbmAntennaNoiseRadioTapField;
                    if (HeaderMetaData.PropRadioTapAntNoiseDBM != null) { HeaderMetaData.RadioTapAntNoiseDBM = HeaderMetaData.PropRadioTapAntNoiseDBM.AntennaNoisedBm; }//tinyint

                    HeaderMetaData.PropRadioTapRssi = RadioHeader[RadioTapType.DbmAntennaSignal] as DbmAntennaSignalRadioTapField;
                    if (HeaderMetaData.PropRadioTapRssi != null) { HeaderMetaData.RadioTapRSSI = HeaderMetaData.PropRadioTapRssi.AntennaSignalDbm; } //tinyint

                    HeaderMetaData.PropRadioTapTxPowerDBM = RadioHeader[RadioTapType.DbmTxPower] as DbmTxPowerRadioTapField;
                    if (HeaderMetaData.PropRadioTapTxPowerDBM != null) { HeaderMetaData.RadioTapTxPowerDBM = HeaderMetaData.PropRadioTapTxPowerDBM.TxPowerdBm; }//tinyint

                    HeaderMetaData.PropRadioTapAntenna = RadioHeader[RadioTapType.Antenna] as AntennaRadioTapField;
                    if (HeaderMetaData.PropRadioTapAntenna != null) { HeaderMetaData.RadioTapAntenna = HeaderMetaData.PropRadioTapAntenna.Antenna; }//tinyint

                    HeaderMetaData.PropRadioTapChannel = RadioHeader[RadioTapType.Channel] as ChannelRadioTapField;
                    if (HeaderMetaData.PropRadioTapChannel != null) { HeaderMetaData.RadioTapChannel = HeaderMetaData.PropRadioTapChannel.Channel; }//int
                    if ((FrameDesc.PacketSource == FrameControl.CaptureType.LocalDevice) || (FrameDesc.PacketSource == FrameControl.CaptureType.RemoteWinDevice))
                    {//There are diffs in the packet headers etc between win-aipcap and linux lipcap. linux hdrs are smaller
                        PacketDotNet.Ieee80211.InformationElementList InfoElementList = PRframe.InformationElements;
                        foreach (PacketDotNet.Ieee80211.InformationElement InfoElement in InfoElementList)
                        {
                            if (InfoElement.Id == InformationElement.ElementId.ServiceSetIdentity)
                            {
                                if (InfoElement.ValueLength > 0)
                                {
                                    char[] ssid = new char[InfoElement.Value.Length];
                                    Array.Copy(InfoElement.Value, ssid, InfoElement.Value.Length);
                                    string ServiceSID = new string(ssid);
                                    HeaderMetaData.PropProbeRequestSSID = ServiceSID;
                                    char TermTest = HeaderMetaData.PropProbeRequestSSID.ElementAt(0);
                                    if (TermTest == '\0')
                                    {
                                        HeaderMetaData.PropProbeRequestSSID = string.Empty;
                                    }
                                }
                                else
                                {
                                    HeaderMetaData.PropProbeRequestSSID = string.Empty;
                                }
                            }
                        }
#if FIXED_OFFSET_PARSE
                    for (count = 0; count < 32; count++)
                    {
                        if ((count + 49) >= MACFrame.BytesHighPerformance.Bytes.Length) { break; } //Some short packets cause index out of bound excptions
                        SSID[count] = Convert.ToChar(MACFrame.BytesHighPerformance.Bytes[count + 49]);
                        if ((MACFrame.BytesHighPerformance.Bytes[count + 49] < 32) || (MACFrame.BytesHighPerformance.Bytes[count + 49] > 126))
                        {
                            SSID[count] = '\n';
                            break;
                        }
                    }
#endif

                    }
                    else if (FrameDesc.PacketSource == FrameControl.CaptureType.RemoteLinuxDevice)//many variations in mmgmnt frame payloads + headers
                    {
                        PacketDotNet.Ieee80211.InformationElementList InfoElementList = PRframe.InformationElements;
                        foreach (PacketDotNet.Ieee80211.InformationElement InfoElement in InfoElementList)
                        {
                            if (InfoElement.Id == InformationElement.ElementId.ServiceSetIdentity)
                            {
                                if (InfoElement.ValueLength > 0)
                                {
                                    char[] ssid = new char[InfoElement.Value.Length];
                                    Array.Copy(InfoElement.Value, ssid, InfoElement.Value.Length);
                                    string ServiceSID = new string(ssid);
                                    HeaderMetaData.PropProbeRequestSSID = ServiceSID;
                                    char TermTest = HeaderMetaData.PropProbeRequestSSID.ElementAt(0);
                                    if (TermTest == '\0')
                                    {
                                        HeaderMetaData.PropProbeRequestSSID = string.Empty;
                                    }
                                }
                                else
                                {
                                    HeaderMetaData.PropProbeRequestSSID = string.Empty;
                                }
                            }
#if FIXED_OFFSET_PARSE
                    for (count = 0; count < 32; count++)
                    {
                        if ((count + 44) >= MACFrame.BytesHighPerformance.Bytes.Length) { break; } //Some short packets cause index out of bound excptions
                        SSID[count] = Convert.ToChar(MACFrame.BytesHighPerformance.Bytes[count + 44]);
                        if ((MACFrame.BytesHighPerformance.Bytes[count + 44] < 32) || (MACFrame.BytesHighPerformance.Bytes[count + 44] > 126))
                        {
                            SSID[count] = '\n';
                            break;
                        }
                    }
#endif
                        }
                    }
                    Array.Resize(ref SSID, count);
                    if (count > 0)
                    {
                        HeaderMetaData.PropProbeRequestSSID = new string(SSID);
                    }
                }
                if (FrameFound)
                {
                    HeaderMetaData.SourceLoc = FrameDesc.PortNo;
                    HeaderMetaData.PropFrameType = MACFrame.FrameControl.SubType;
                    HeaderMetaData.FrameType = (byte)HeaderMetaData.PropFrameType;
                    HeaderMetaData.TimeStamp = DateTime.Now;
                    HeaderMetaData.FirstSeen = DateTime.Now;
                    HeaderMetaData.LastSeen = DateTime.Now;
                    HeaderMetaData.strSrcAddr = HeaderMetaData.PropSrcAddr.ToString();
                    HeaderMetaData.strDstAddr = HeaderMetaData.PropDstAddr.ToString();
                    //Console.WriteLine(HeaderMetaData.FrameType.ToString());
                    lock (PacketMetaDataQueueLock)
                    {
                        PacketMetaDataQueue.Enqueue(HeaderMetaData);
                    }

                    PacketMetaDataReadyWait.Set();

                }



            }
        }
        public void GetPacketMetaData()
        {

            while (Context.ProgramIsClosing == false)
            {
                PacketDataReadyWait.WaitOne();
                int counter = 0;   
                lock (RadioPacketsQueueLock)
                {
                    int QueueCount = RadioPacketsQueue.Count;
                    for (counter = 0; counter < QueueCount; counter++)
                    {
                        FrameControl.IncomingPacketDescriptor FrameDesc = new FrameControl.IncomingPacketDescriptor();
                        FrameDesc = (FrameControl.IncomingPacketDescriptor)RadioPacketsQueue.Dequeue();
                        MetaDataExtract(FrameDesc);
                    }
                }
            }
        }
        public void SendMetaDataToLists()
        {
            int PacketQueueCount;
            int counter;
            PacketQueueCount = PacketMetaDataQueue.Count;
            for (counter = 0; counter < PacketQueueCount; counter++)
            {
                RxInfo MetaData = new RxInfo();
                RadioPlusMAC RplusM = new RadioPlusMAC();
                lock (PacketMetaDataQueueLock)
                {
                    RplusM = (RadioPlusMAC)PacketMetaDataQueue.Dequeue();
                }
                if (RplusM == null) { break; }
                lock (Context.IncomingPacketMonitor.IncoimingFrameListLock)
                {
                    Context.IncomingPacketMonitor.IncomingFrameList.Add(RplusM);
                }
                Context.IncomingPacketMonitor.IncomingFrameReadyWait.Set();
            }
        }
        public void MetaDataToSql()
        {
            int PacketQueueCount;
            int counter;

            while (Context.ProgramIsClosing == false)
            {
                int RetryCount = 0;
                PacketMetaDataReadyWait.WaitOne();
                if (!Context.WriteFramesToSql)
                {
                    SendMetaDataToLists();
                    continue;
                }
  
                SQLConnectionString = "Data Source=" + "200.200.200.66" + '\\' + "SQL2012EXPRESS" + ";Initial Catalog=Wireless;" + "Persist Security Info=True;User ID=sa;Password=" + "SurfaceDetection";
                Connection = new SqlConnection(SQLConnectionString);
                RxInfoDataContext MetaDataContext = new RxInfoDataContext(Connection);
              
                PacketQueueCount = PacketMetaDataQueue.Count;
                for (counter = 0; counter < PacketQueueCount; counter++)
                {
                    RxInfo MetaData = new RxInfo();
                    RadioPlusMAC RplusM = new RadioPlusMAC();
                    lock (PacketMetaDataQueueLock)
                    {
                        RplusM = (RadioPlusMAC)PacketMetaDataQueue.Dequeue();
                    }
                    if (RplusM == null) { break; }
                    lock (Context.IncomingPacketMonitor.IncoimingFrameListLock)
                    {
                        Context.IncomingPacketMonitor.IncomingFrameList.Add(RplusM);
                    }
                    Context.IncomingPacketMonitor.IncomingFrameReadyWait.Set();

                    if (PacketDotNet.Ieee80211.FrameControlField.FrameSubTypes.ManagementProbeResponse == RplusM.PropFrameType) //Too many of these to write SQL during peak hours.
                    {
                        continue;
                    }
                    //copy the structs RplusM to MetaData here
                    MetaData.SourceAddr = RplusM.SourceAddr;
                    MetaData.DestinationAddr = RplusM.DestinationAddr;
                    MetaData.BSSID = RplusM.BSSID;
                    MetaData.FrameType = RplusM.FrameType;
                    MetaData.RadioTapAntNoiseDB = RplusM.RadioTapAntNoiseDB;
                    MetaData.RadioTapAntSignalDB = RplusM.RadioTapAntSignalDB;
                    MetaData.RadioTapAntNoiseDBM = (byte)RplusM.RadioTapAntNoiseDBM;
                    MetaData.RadioTapRSSI = (byte)RplusM.RadioTapRSSI;
                    MetaData.RadioTapTxPowerDBM = (byte)RplusM.RadioTapTxPowerDBM;
                    MetaData.RadioTapAntenna = RplusM.RadioTapAntenna;
                    MetaData.RadioTapChannel = RplusM.RadioTapChannel;
                    MetaData.ProbeRequestSSID = RplusM.PropProbeRequestSSID;
                    MetaData.TimeStamp = RplusM.TimeStamp;


                RetrySend:
                    try
                    {
                        MetaDataContext.RxInfos.InsertOnSubmit(MetaData);
                        MetaDataContext.SubmitChanges();
                        Console.WriteLine(RplusM.ToString());
                    }
                    catch (Exception Ex)
                    {
                        RetryCount++;
                        if (RetryCount < 4)
                        {
                            goto RetrySend;
                        }
                        var res = System.Windows.Forms.MessageBox.Show("Can't Write To SQL!\n" + Ex, "Retry?", System.Windows.Forms.MessageBoxButtons.YesNo);
                        if (res == System.Windows.Forms.DialogResult.Yes)
                        {
                            goto RetrySend;
                        }
                        else
                        {
                            System.Windows.Forms.MessageBox.Show("Fata Error", "FATAL SQL ERROR", System.Windows.Forms.MessageBoxButtons.OK);
                            MetaDataContext.Dispose();
                            return;
                        }
                    }                    
                }
                MetaDataContext.Dispose();
            }
        }
    }
    public class RadioPlusMAC
    {
        public PacketDotNet.Ieee80211.FrameControlField.FrameSubTypes PropFrameType { get; set; }
        public byte FrameType { get; set; }
        public System.Net.NetworkInformation.PhysicalAddress PropSrcAddr { get; set; }
        public string strSrcAddr { get; set; }
        public byte[] SourceAddr { get; set; }
        public System.Net.NetworkInformation.PhysicalAddress PropDstAddr { get; set; }
        public string strDstAddr { get; set; }
        public byte[] DestinationAddr { get; set; }
        public System.Net.NetworkInformation.PhysicalAddress PropBSSID { get; set; }
        public byte[] BSSID { get; set; }        
        public DbAntennaNoiseRadioTapField PropRadioTapAntNoiseDB { get; set; }
        public byte RadioTapAntNoiseDB { get; set; }
        public DbAntennaSignalRadioTapField PropRadioTapAntSignalDB { get; set; }
        public byte RadioTapAntSignalDB { get; set; }
        public DbmAntennaNoiseRadioTapField PropRadioTapAntNoiseDBM { get; set; }
        public sbyte RadioTapAntNoiseDBM { get; set; }
        public DbmAntennaSignalRadioTapField PropRadioTapRssi { get; set; }
        public sbyte RadioTapRSSI { get; set; }
        public DbmTxPowerRadioTapField PropRadioTapTxPowerDBM { get; set; }
        public sbyte RadioTapTxPowerDBM { get; set; }
        public AntennaRadioTapField PropRadioTapAntenna { get; set; }
        public byte RadioTapAntenna { get; set; }
        public ChannelRadioTapField PropRadioTapChannel { get; set; }
        public int RadioTapChannel { get; set; }
        public string PropProbeRequestSSID { get; set; }
        public DateTime TimeStamp { get; set; }
        public int Count { get; set; }
        public DateTime FirstSeen { get; set; }
        public DateTime LastSeen { get; set; }
        public int SourceLoc { get; set; }
        public string Tag { get; set; }
        public override string ToString()
        {
            StringBuilder SB = new StringBuilder();
            SB.Clear();
            SB.Append(PropFrameType.ToString() + " ");
            SB.Append(PropSrcAddr.ToString() + " ");
            SB.Append(PropDstAddr.ToString() + " ");
            SB.Append(PropBSSID.ToString() + " ");
            SB.Append(PropRadioTapChannel.ToString() + " ");
            SB.Append(PropRadioTapRssi.ToString() + " ");
            if (PropProbeRequestSSID != null)
            {
                SB.Append(PropProbeRequestSSID.ToString() + " ");
            }
            return SB.ToString();

        }
    }
}
