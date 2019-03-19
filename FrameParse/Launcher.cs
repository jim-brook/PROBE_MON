using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using SharpPcap;
using SharpPcap.AirPcap;
using System.Net.NetworkInformation;
using System.Runtime.Serialization.Formatters.Binary;

namespace ProbeMon
{
    public class Launcher
    {
        public FrameControl Context;
        [Serializable]
        public struct StationAlarms
        {
            public uint SiteId;
            public bool InRange;
            public bool RadioTapRSSIHigh;
            public bool RadioTapRSSILow;
            public sbyte RadioTapRSSIValueHigh;
            public sbyte RadioTapRSSIValueLow;
            public String StationAddress;
        }
        [Serializable]
        public struct SSIDAlarms
        {
            public bool StationRequest;
            public string SSID;
            public uint SiteId;
        }
        public bool ProgramIsClosing;

        public TcpControl SiteComms;
        public TcpIPC IpcComm;
        public PacketParsing PacketParser;
        public AirPcapDeviceList AirPCapDevices;
        public Monitor IncomingPacketMonitor;
        public StationWatchList WatchList;
        public Alarms ActiveAlarms;
        public volatile List<string> StationWatchList = new List<string>();
        public volatile List<string> SSIDWatchList = new List<string>();
        public volatile List<StationAlarms> StationAlarmsList = new List<StationAlarms>();
        public volatile List<SSIDAlarms> SSIDAlarmsList = new List<SSIDAlarms>();
        public bool WriteFramesToSql;
        public volatile sbyte MaxRSSIAlarm;
        public int TcpPort;
        public int DevIndex;
        public int Chn;
        public Launcher(FrameControl FC, int DeviceIndex, int Channel, int PortNumber)
        {
            Context = FC;
            TcpPort = PortNumber;
            DevIndex = DeviceIndex;
            Chn = Channel;
            LoadClasses();
        }
        public void LoadClasses()
        {
            ProgramIsClosing = false;
            if (DevIndex == -1)
            {
                SiteComms = new TcpControl(this);
            }
            IpcComm = new TcpIPC(this);
            SiteComms = new TcpControl(this);
            IncomingPacketMonitor = new Monitor(this);
            PacketParser = new PacketParsing(this);
            WatchList = new StationWatchList(this);
            ActiveAlarms = new Alarms(this);
            WriteFramesToSql = true;
            MaxRSSIAlarm = 0;
            ActiveAlarms.Show();
            IncomingPacketMonitor.Show();
            WatchList.Show();
            PacketParser.Show();
            WatchList.Hide();
            PacketParser.Hide();
        }
        public void Unload()
        {
            ProgramIsClosing = true;
            if (DevIndex == -1)
            {
                //wait objects etc form TcpControl
            }
            PacketParser.PacketBytesMasterListReadyWait.Set();
            IncomingPacketMonitor.AlarmCheckReadyWait.Set();
            PacketParser.USBPacketBytesReadyWait.Set();
            PacketParser.PacketDataReadyWait.Set();
            PacketParser.PacketMetaDataReadyWait.Set();
            IncomingPacketMonitor.IncomingFrameReadyWait.Set();
            WatchList.UpdateUIWait.Set();
            SaveAddressWatchList();
            SaveSSIDWatchList();
            SaveStationAddressAlarmList();
            SaveSSIDAlarmList();
        }
        public void SaveSSIDAlarmList()
        {
            try
            {
                FileStream stream = new FileStream(@"SSIDAlarms.dat", FileMode.Create);
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, SSIDAlarmsList);
                stream.Close();
            }
            catch (Exception Ex)
            {

            }
        }
        public void SaveStationAddressAlarmList()
        {
            try
            {
                FileStream stream = new FileStream(@"AddrAlarms.dat", FileMode.Create);
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, StationAlarmsList);
                stream.Close();
            }
            catch (Exception Ex)
            {

            }
        }
        public void ReadSSIDAlarmList()
        {
            try
            {
                FileStream stream = new FileStream(@"SSIDAlarms.dat", FileMode.Open);
                BinaryFormatter formatter = new BinaryFormatter();
                SSIDAlarmsList = (List<SSIDAlarms>)formatter.Deserialize(stream);
                stream.Close();
            }
            catch (Exception Ex)
            {

            }
        }
        public void ReadStationAddressAlarmList()
        {
            try
            {
                FileStream stream = new FileStream(@"AddrAlarms.dat", FileMode.Open);
                BinaryFormatter formatter = new BinaryFormatter();
                StationAlarmsList = (List<StationAlarms>)formatter.Deserialize(stream);
                stream.Close();
            }
            catch (Exception Ex)
            {

            }
        }
        public void SaveAddressWatchList()
        {
            try
            {
                FileStream stream = new FileStream(@"AddrWatch.dat", FileMode.Create);
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, StationWatchList);
                stream.Close();
            }
            catch (Exception Ex)
            {

            }
        }
        public void ReadAddressWatchList()
        {
            try
            {
                FileStream stream = new FileStream(@"AddrWatch.dat", FileMode.Open);
                BinaryFormatter formatter = new BinaryFormatter();
                StationWatchList = (List<string>)formatter.Deserialize(stream);
                stream.Close();
            }
            catch (Exception Ex)
            {

            }
        }
        public void SaveSSIDWatchList()
        {
            try
            {
                FileStream stream = new FileStream(@"SSIDWatch.dat", FileMode.Create);
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, SSIDWatchList);
                stream.Close();
            }
            catch (Exception Ex)
            {

            }
        }
        public void ReadSSIDWatchList()
        {
            try
            {
                FileStream stream = new FileStream(@"SSIDWatch.dat", FileMode.Open);
                BinaryFormatter formatter = new BinaryFormatter();
                SSIDWatchList = (List<string>)formatter.Deserialize(stream);
                stream.Close();
            }
            catch (Exception Ex)
            {

            }
        }
        public void BeginCapture()
        {
            ProcessStartInfo ProcToStart = new ProcessStartInfo();
            IncomingPacketMonitor.Show();
            string args = Chn.ToString() + " " + DevIndex.ToString() + " " + TcpPort.ToString();
            ProcToStart.Arguments = args;
            ProcToStart.FileName = "CaptureConsole.exe";
            Process.Start(ProcToStart);
            System.Threading.Thread.Sleep(750);


        }
    }
}

