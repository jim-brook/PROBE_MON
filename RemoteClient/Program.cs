using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpPcap;
using SharpPcap.AirPcap;
using SharpPcap.LibPcap;
using PacketDotNet;
using PacketDotNet.Ieee80211;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;

namespace ProbeMon
{
    class Program
    {
        public static int PortNo { get; set; }
		public static bool BackgroundThreadStop = false;
#if USE_LOCK
		public static object QueueLock = new object();
#else
        public static SemaphoreSlim QueSem = new SemaphoreSlim(1);
#endif
        public static List<RawCapture> PacketQueue = new List<RawCapture>();
		public static string ServerAddress { get; set; }
        public static AirPcapDevice AirPcapDev;
        public static UInt32 ChanNo { get; set; }
        public static bool ChanWritten { get; set; }
        public static OSPlatform SystemPlatform { get; set; }
        public enum OSPlatform
        {
            Windows = 1,
            Linux = 2,
            Unknown = 3
        }
        static void Main(string[] args)
        {
            UInt32 count = 0;
            int PortNumber = 0;
            OSPlatform Platform;
            LibPcapLiveDevice LibCapDevice = null;
            OperatingSystem os = Environment.OSVersion;  
            PlatformID pid = os.Platform;
            switch (pid)
            {
                case PlatformID.Win32NT:
                case PlatformID.Win32S:
                case PlatformID.Win32Windows:
                case PlatformID.WinCE:
                    Platform = OSPlatform.Windows;
                    break;
                case PlatformID.Unix:
                    Platform = OSPlatform.Linux;
                    break;
                default:
                    Platform = OSPlatform.Unknown;
                    break;
            }
            SystemPlatform = Platform;
            if (Platform == OSPlatform.Linux)
            {
                if (args.Length > 1)
                {
                    try
                    {
                        ServerAddress = args[0].ToString();
                    }
                    catch
                    {
                        Console.WriteLine("Bad IP Address Entered");
                        Console.WriteLine("RemoteClient <server ip> <port no>");
                        return;
                    }
                    try
                    {
                        if (int.TryParse(args[1], out PortNumber))
                        {
                            string phony = args[1].ToString();
                            PortNo = PortNumber;
                        }
                        else
                        {
                            Console.WriteLine("Bad Port Number");
                            Console.WriteLine("RemoteClient <server ip> <port no>");
                            return;
                        }
                    }
                    catch
                    {
                        Console.WriteLine("Bad Port Number");
                        Console.WriteLine("RemoteClient <server ip> <port no>");
                        return;
                    }
                }
                else
                {
                    Console.WriteLine("RemoteClient <server ip> <port no>");
                    return;
                }
                LibPcapLiveDeviceList LibCapDevices = LibPcapLiveDeviceList.Instance;
                if (LibCapDevices.Count == 0)
                {
                    Console.WriteLine("No LibCap Devices Found. Re-run as Administrator.\n");
                    return;
                }
                Console.WriteLine("LibPCap device(s) found:");
                foreach (ICaptureDevice dev in LibCapDevices)
                {
                    Console.WriteLine("[{0}] - {1} {2}", count.ToString(), dev.Name, dev.Description);
                    count++;
                }
                Console.WriteLine("");
                Console.Write("Choose a capture device:");
                int DeviceIndex = int.Parse(Console.ReadLine());
                var backgroundThread = new System.Threading.Thread(BackgroundThread);
                backgroundThread.Start();
                LibCapDevice = LibCapDevices[DeviceIndex] as LibPcapLiveDevice;
                LibCapDevice.OnPacketArrival += new SharpPcap.PacketArrivalEventHandler(OnPacketArrival);
                LibCapDevice.Open();
                LibCapDevice.StartCapture();
                Console.Write("\n\nCapturing on {0}\nChannel:{1}\n\n", LibCapDevice.Name, LibCapDevice.Description);
#if CTRL_C
                Console.CancelKeyPress += delegate (object sender, ConsoleCancelEventArgs e)
                {
                    e.Cancel = true;
                    BackgroundThreadStop = true;
                    LibCapDevice.StopCapture();
                    LibCapDevice.Close();
                    //backgroundThread.Abort();
                    Environment.Exit(0);
                    Console.WriteLine("Won't Print, But A Clean Exit?");
                };
#endif
            }
            else if (Platform == OSPlatform.Windows)
            {
                while (Console.KeyAvailable) { Console.ReadKey(true); } //flush console.read()
                uint ChannelNumber;
                try
                {
                    try
                    {
                        ServerAddress = args[0].ToString();
                    }
                    catch
                    {
                        Console.WriteLine("Bad IP Address Entered");
                        Console.WriteLine("RemoteClient <server ip> <port no> <channel no>");
                        return;
                    }
                    try
                    {
                        if (int.TryParse(args[1], out PortNumber))
                        {
                            string phony = args[1].ToString();
                            PortNo = PortNumber;
                        }
                        else
                        {
                            Console.WriteLine("Bad Port Number");
                            Console.WriteLine("RemoteClient <server ip> <port no> <channel no>");
                            return;
                        }
                    }
                    catch
                    {
                        Console.WriteLine("Bad Port Number");
                        Console.WriteLine("RemoteClient <server ip> <port no> <channel no>");
                        return;
                    }
                    if (uint.TryParse(args[2], out ChannelNumber))
                    {
                        ChanNo = ChannelNumber;
                    }
                    else
                    {
                        Console.WriteLine("Bad Channel Number");
                        Console.WriteLine("RemoteClient <server ip> <port no> <channel no>");
                        return;
                    }
                }
                catch
                {
                    Console.WriteLine("Bad Channel Number");
                    Console.WriteLine("RemoteClient <server ip> <port no> <channel no>");
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
                    Console.WriteLine("[{0}] - {1} {2}", count.ToString(), dev.Name, dev.Description);
                    count++;
                }
                Console.WriteLine("");
                Console.Write("Choose a capture device:");
                string Dev = Console.ReadLine();
                int DeviceIndex;
                if (int.TryParse(Dev, out DeviceIndex))
                {
                    if (DeviceIndex >= 0 && DeviceIndex <= count)
                    {
                        //valid number (int)
                    }
                    else
                    {
                        Console.WriteLine("Invalid Device Number: " + DeviceIndex.ToString() + " " + count.ToString());
                    }
                }
                else
                {
                    Console.WriteLine("Invalid Device Number: " + DeviceIndex.ToString() + " " + count.ToString());
                }
                //Console.WriteLine("");
                //Console.Write("Choose a capture device:");
                //int DeviceIndex = int.Parse(Console.ReadLine());


                AirPcapDev = AirPCapDevices[DeviceIndex] as AirPcapDevice;
                //AirPcapDev.Open(DeviceMode.Promiscuous, 0);
                AirPcapDev.Open();
                AirPcapDev.TxPower = 1;
                AirPcapDev.Channel = ChanNo = ChannelNumber;
                //AirPcapDev.AirPcapLinkType = AirPcapLinkTypes._802_11_PLUS_RADIO;
                //AirPcapDev.MacFlags = AirPcapMacFlags.MonitorModeOn;
                AirPcapDev.OnPacketArrival += new SharpPcap.PacketArrivalEventHandler(OnPacketArrival);
                var backgroundThread = new System.Threading.Thread(BackgroundThread);
                backgroundThread.Start();
                AirPcapDev.StartCapture();
                Console.Write("\n\nCapturing on {0}\nChannel:{1}\n\n", AirPcapDev.Description, AirPcapDev.Channel.ToString());
#if CTRL_C
                Console.CancelKeyPress += delegate (object sender, ConsoleCancelEventArgs e)
                {
                    e.Cancel = true;
                    BackgroundThreadStop = true;
                    AirPcapDev.StopCapture();
                    AirPcapDev.Close();
                    backgroundThread.Abort();
                    Environment.Exit(0);
                    Console.WriteLine("Won't Print, But A Clean Exit?");
                };
#endif
            }
            Console.ReadLine();//press enter to exit
            BackgroundThreadStop = true;
            if (Platform == OSPlatform.Linux)
            {                
                LibCapDevice.StopCapture();
                //LibCapDevice.Close();   
            }
            else if (Platform == OSPlatform.Windows)
            {      
                AirPcapDev.StopCapture();
                //AirPcapDev.Close(); 
            }
            Console.WriteLine("Main Thread Exiting....");
            Environment.Exit(0);
        }
        public static void OnPacketArrival(object sender, CaptureEventArgs e)
        {
#if USE_LOCK
            lock (QueueLock)
			{
				PacketQueue.Add(e.Packet);      
			}
            //Console.Write(".");
#else
            QueSem.WaitAsync();
            PacketQueue.Add(e.Packet);
            QueSem.Release();
#endif
        }

        private static void BackgroundThread()
        {
			TcpClient client = new TcpClient();
			client.ReceiveTimeout = 9000;
			client.SendTimeout = 9000;
			IPAddress Addr = IPAddress.Parse (ServerAddress);
			try
			{
                client.Connect(new IPEndPoint(Addr, PortNo));
			}
			catch(Exception Ex)
			{
				Console.WriteLine("Server Unreachable");
                Console.WriteLine(Ex.ToString());
                BackgroundThreadStop = true;
				return;
			}
			Console.WriteLine("Connected.");
			UInt64 FrameCounter = 0;

	
			while (!BackgroundThreadStop)
			{
				bool shouldSleep = true;
#if USE_LOCK
				lock (QueueLock)
				{
					if (PacketQueue.Count != 0)
					{
						shouldSleep = false;
					}
				}
#else
                QueSem.WaitAsync();
                if (PacketQueue.Count != 0)
                {
                    shouldSleep = false;
                }
                QueSem.Release();
#endif
                if (shouldSleep)
				{
					System.Threading.Thread.Sleep(250);
					if(BackgroundThreadStop)
					{
						client.Close ();
                        Console.WriteLine("Background Thread Exited....return");
                        return;		
					}
				}
				else // should proceabemsss the queue
				{
                    if ((ChanWritten == false) && (SystemPlatform == OSPlatform.Windows))
                    {
                        AirPcapDev.Channel = ChanNo;
                        ChanWritten = true;
                    }
                    
					List<RawCapture> ourQueue;
#if USE_LOCK
                    lock (QueueLock)
                    {
                        // swap queues, giving the capture callback a new one
                        ourQueue = PacketQueue;
                        PacketQueue = new List<RawCapture>();
                    }
#else
                    QueSem.WaitAsync();
                    // swap queues, giving the capture callback a new one
                    ourQueue = PacketQueue;
                    QueSem.Release();
                    PacketQueue = new List<RawCapture>();
#endif

                    foreach (var packet in ourQueue)
					{

						bool FrameFound = false;
						Packet ParsedPacket = Packet.ParsePacket(packet.LinkLayerType, packet.Data);
						if (ParsedPacket.PayloadPacket == null) { Console.WriteLine("NULL Payload"); continue; }
						MacFrame macFrame = ParsedPacket.PayloadPacket as MacFrame;
						//Console.WriteLine(packet.LinkLayerType.ToString());
						if (packet.LinkLayerType == PacketDotNet.LinkLayers.Ieee80211_Radio)
						{
                            if (macFrame.FrameControl.Type == FrameControlField.FrameTypes.Management)
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
                            else if (macFrame.FrameControl.Type == FrameControlField.FrameTypes.Control)
                            {
                                switch (macFrame.FrameControl.SubType)
                                {
                                    case PacketDotNet.Ieee80211.FrameControlField.FrameSubTypes.ControlCTS:
                                        FrameFound = true;
                                        break;
                                    default:
                                        //Console.WriteLine(macFrame.ToString());
                                        break;
                                }
                            }
						}

                        NetworkStream ns = null;
                        BinaryFormatter bf = null;
						if (FrameFound)
						{
							//Console.WriteLine(macFrame.ToString());
							try
							{
								bf = new BinaryFormatter();
								ns = client.GetStream();
								bf.Serialize(ns, packet.Data);
								FrameCounter++;
								Console.WriteLine("TX: " + FrameCounter.ToString());	
							}
							catch (System.IO.IOException Ex)
							{
                                Console.WriteLine("Retry Attempted " + Ex.Message.ToString());
                                try
                                {
                                    Console.WriteLine("TX: Retry");
                                    client.Close();
                                    client = new TcpClient();
                                    client.Connect(new IPEndPoint(Addr, PortNo));
                                    bf = new BinaryFormatter();
                                    ns = client.GetStream();
                                    bf.Serialize(ns, packet.Data);
                                    FrameCounter++;
                                    Console.WriteLine("TX: " + FrameCounter.ToString());
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("Server Unreachable");
                                    BackgroundThreadStop = true;
                                    if (BackgroundThreadStop)
                                    {
                                        client.Close();
                                    }
                                    Console.WriteLine("Retry Failed! " + ex.Message.ToString());
                                    return;

                                }
                            }

                        }
					}
				}
			}
			client.Close();  
			Console.WriteLine("Background Thread Exited....");
		}       
    }
}
