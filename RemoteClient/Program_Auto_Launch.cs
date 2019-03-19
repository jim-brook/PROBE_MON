using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpPcap;
using SharpPcap.AirPcap;
using PacketDotNet;
using PacketDotNet.Ieee80211;
//using System.ServiceModel;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;

namespace ProbeMon
{
    class Program
    {
        public static AirPcapDevice AirPcapDev;
        public static bool BackgroundThreadStop = false;
        public static object QueueLock = new object();
        public static UInt32 ChanNo { get; set; }
        public static bool ChanWritten { get; set; }
        public static List<RawCapture> PacketQueue = new List<RawCapture>();
        static void Main(string[] args)
        {
            AirPcapDev = null;
            ChanWritten = false;
            var backgroundThread = new System.Threading.Thread(BackgroundThread);
            backgroundThread.Start();
            Console.CancelKeyPress += delegate(object sender, ConsoleCancelEventArgs e)
            {
                e.Cancel = true;
                BackgroundThreadStop = true;
                backgroundThread.Abort();
                Environment.Exit(0);
                Console.WriteLine("Won't Print, But A Clean Exit?");
            };
#if USE_OLD_HANDLER
            Console.CancelKeyPress += delegate(object sender, ConsoleCancelEventArgs e)
            {
                e.Cancel = true;
                BackgroundThreadStop = true;
                Console.WriteLine(AirPcapDev.Description.ToString());                
                Console.WriteLine("StopCapture()....");
                AirPcapDev.StopCapture();
                Console.WriteLine("....Capture Stopped\n\rClose()....");
                System.Threading.Thread.Sleep(1000);
                AirPcapDev.Close();
                Console.WriteLine("....Closed"); 
                Environment.Exit(0);                               
                Console.WriteLine("Won't Print, But A Clean Exit?");
            };
#endif
            UInt32 count = 0;
            UInt32 ChannelNumber;
            if (args.Length != 0)
            {
                if (args[0] != null)
                {
                    try
                    {
                        ChannelNumber = Convert.ToUInt32(args[0]);
                    }
                    catch
                    {
                        Console.WriteLine("Invalid Channel! Please Enter Numeric only.");
                        return;
                    }
                }
                else
                {
                    ChannelNumber = 1;
                }
            }
            else
            {
                ChannelNumber = 1;
            }
            if (args[1] == null)
            {
                Console.WriteLine("AirPCap Device Not Selected");
                return;
            }
            AirPcapDeviceList AirPCapDevices = AirPcapDeviceList.Instance;
            if (AirPCapDevices.Count == 0)
            {
                Console.WriteLine("No AirPCap Devices Found. Re-run as Administrator.\n");
                return;
            }
            Console.WriteLine("AirPCap device(s) found:");
            foreach (AirPcapDevice dev in AirPCapDevices)
            {
                count++;
            }
            int DeviceIndex;
            if (int.TryParse(args[1], out DeviceIndex))
            {
                if (DeviceIndex >= 0 && DeviceIndex <= count)
                {
                    //valid number (int)
                }
                else
                {
                    Console.WriteLine("Invalid Device Number");
                }
            }
            else
            {
                Console.WriteLine("Invalid Device Number");
            }
            //Console.WriteLine("");
            //Console.Write("Choose a capture device:");
            //int DeviceIndex = int.Parse(Console.ReadLine());


            AirPcapDev = AirPCapDevices[DeviceIndex] as AirPcapDevice;
            //AirPcapDev.Open(DeviceMode.Promiscuous, 0);
            AirPcapDev.Open();
            AirPcapDev.Channel = ChanNo = ChannelNumber;
            //AirPcapDev.AirPcapLinkType = AirPcapLinkTypes._802_11_PLUS_RADIO;
            //AirPcapDev.MacFlags = AirPcapMacFlags.MonitorModeOn;
            AirPcapDev.OnPacketArrival += new SharpPcap.PacketArrivalEventHandler(OnPacketArrival);
            //AirPcapDev.MinToCopy = 1;
            //AirPcapDev.KernelBufferSize = 4096000;  
            AirPcapDev.StartCapture(); 
            Console.Write("\n\nCapturing on {0}\nChannel:{1}\tMonitor Mode {2}\n\n", AirPcapDev.Description, AirPcapDev.Channel.ToString(), AirPcapDev.MacFlags.ToString());
        }
        public static void OnPacketArrival(object sender, CaptureEventArgs e)
        {         
            lock (QueueLock)
            {
                PacketQueue.Add(e.Packet);      
            }
            //Console.Write(".");

        }

        private static void BackgroundThread()
        {
            TcpClient client = new TcpClient();
            client.ReceiveTimeout = 2000;
            client.SendTimeout = 4000;
            client.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 48009));
            Console.WriteLine("Connected.");
            UInt64 FrameCounter = 0;

#if USE_UDP
            IPEndPoint localpt = new IPEndPoint(IPAddress.Loopback, 48008);
            UdpClient uClient = new UdpClient();
            uClient.ExclusiveAddressUse = false;
            uClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            uClient.Client.Bind(localpt);

            uClient.Connect(localpt);
#endif
            while (!BackgroundThreadStop)
            {
                bool shouldSleep = true;
                lock (QueueLock)
                {
                    if (PacketQueue.Count != 0)
                    {
                        shouldSleep = false;
                    }
                }

                if (shouldSleep)
                {
                    System.Threading.Thread.Sleep(250);
                }
                else // should process the queue
                {
                    if (ChanWritten == false)
                    {
                        AirPcapDev.Channel = ChanNo;
                        ChanWritten = true;
                    }
                    List<RawCapture> ourQueue;
                    lock (QueueLock)
                    {
                        // swap queues, giving the capture callback a new one
                        ourQueue = PacketQueue;
                        PacketQueue = new List<RawCapture>();
                    }
    
                    foreach (var packet in ourQueue)
                    {
   
                        bool FrameFound = false;
                        Packet ParsedPacket = Packet.ParsePacket(packet.LinkLayerType, packet.Data);
                        if (ParsedPacket.PayloadPacket == null) { Console.WriteLine("NULL Payload"); continue; }
                        MacFrame macFrame = ParsedPacket.PayloadPacket as MacFrame;
                        //Console.WriteLine(packet.LinkLayerType.ToString());
                        if (packet.LinkLayerType == PacketDotNet.LinkLayers.Ieee80211_Radio)
                        {
                            switch (macFrame.FrameControl.SubType)
                            {
                                case PacketDotNet.Ieee80211.FrameControlField.FrameSubTypes.ManagementProbeRequest:
                                    FrameFound = true;
                                    break;
                                case PacketDotNet.Ieee80211.FrameControlField.FrameSubTypes.ManagementProbeResponse:
                                    FrameFound = true;
                                    break;
                                case PacketDotNet.Ieee80211.FrameControlField.FrameSubTypes.ManagementAssociationResponse:
                                    FrameFound = true;
                                    break;
                                case PacketDotNet.Ieee80211.FrameControlField.FrameSubTypes.ManagementAssociationRequest:
                                    FrameFound = true;
                                    break;
                                case PacketDotNet.Ieee80211.FrameControlField.FrameSubTypes.ManagementReassociationRequest:
                                    FrameFound = true;
                                    break;

                                default:
                                    //Console.WriteLine(macFrame.ToString());
                                    break;


                            }
                        }
                        
                        if (FrameFound)
                        {



                            //Console.WriteLine(macFrame.ToString());
                            try
                            {

                                BinaryFormatter bf = new BinaryFormatter();
                                NetworkStream ns = client.GetStream();
                                bf.Serialize(ns, packet.Data);
                                FrameCounter++;
                                Console.WriteLine("TX: " + FrameCounter.ToString());
#if USE_UDP
                                uClient.Send(packet.Data, packet.Data.Length);
#endif
                            }
                            catch (Exception Ex)
                            {                                
                                Console.WriteLine(Ex.ToString());
                            }
#if USE_UDP
                            catch (SocketException se)
                            {
                                Console.WriteLine("uClient.Send : {0}", se.ToString());
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("uClient.Send : {0}", e.ToString());
                            }
#endif
                        }

                    }

                }
 
            }
            client.Close();            
#if USE_UDP
            uClient.Close();
#endif
            Console.WriteLine("Background Thread Exited....");
        }
    }
}
