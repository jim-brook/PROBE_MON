#define DISP_RX
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;

namespace ProbeMon
{

    public class TcpIPC
    {
        public Thread IncoimingPacketThread;
        public Thread DeserializedFramesThread;
        public object DeserializedFramesListLock = new object();
        public List<byte[]> DeserializedFrames = new List<byte[]>();
        public EventWaitHandle DeserializedFramesReadyWait;
        FrameControl Context;
        int Port;
        FrameControl.CaptureType Src;
        public TcpIPC(FrameControl FC, FrameControl.CaptureType CapSrc, int PortNumber)
        {
            Context = FC;
            Port = PortNumber;
            Src = CapSrc;
            DeserializedFramesReadyWait = new EventWaitHandle(false, EventResetMode.AutoReset);
            DeserializedFramesThread = new Thread(QueueUpFrames);
            DeserializedFramesThread.Start();
            IncoimingPacketThread = new Thread(NewPacketReady);
            IncoimingPacketThread.Start();

        }
        public void QueueUpFrames()
        {
            while (Context.ProgramIsClosing == false)
            {//wait obj add
                DeserializedFramesReadyWait.WaitOne();
                if (Context.ProgramIsClosing == true) { return; }
                List<byte[]> TempList;
                lock (DeserializedFramesListLock)
                {
                    TempList = DeserializedFrames.ToList();
                    DeserializedFrames.Clear();
                }
                foreach (byte[] ReceiveBytes in TempList)
                {
                    FrameControl.IncomingPacketDescriptor FrameDesc = new FrameControl.IncomingPacketDescriptor();
                    FrameDesc.PortNo = Port;
                    FrameDesc.PacketSource = Src;
                    FrameDesc.RawPacket = ReceiveBytes;
                    lock (Context.PacketParser.PacketBytesQueueLock)
                    {
                        Context.PacketParser.PacketBytesQueue.Add(FrameDesc);
                    }
                }
                Context.PacketParser.PacketBytesReadyWait.Set();
            }
        }
        private void NewPacketReady()
        {

            UInt64 FrameCounter;
            while (Context.ProgramIsClosing == false)
            {
                Socket client = null;
                TcpListener listener = new TcpListener(IPAddress.Any, Port);

                listener.Start();
                Console.WriteLine("IPC Server started.");
                while (!listener.Pending())
                {
                    System.Threading.Thread.Sleep(250);
                    if(Context.ProgramIsClosing) 
                    {
                        if (listener != null)
                        {
                            listener.Stop();
                            listener = null;
                        }
                        if (client != null)
                        {
                            client.Close();
                            client.Dispose();
                            client = null;
                        } 
                        return;
                    }
                }
                client = listener.AcceptSocket();
                Console.WriteLine("Accepted IPC client {0}.\n", client.RemoteEndPoint);
                FrameCounter = 0;
                while (Context.ProgramIsClosing == false)
                {       
                    using (NetworkStream ns = new NetworkStream(client))
                    {
                        BinaryFormatter bf = new BinaryFormatter();
                        try
                        {
                            byte[] ReceiveBytes = (byte[])bf.Deserialize(ns);
                            lock (DeserializedFramesListLock)
                            {
                                DeserializedFrames.Add(ReceiveBytes);
                            }                            
                        }
                        catch (Exception Ex)
                        {
                            Console.WriteLine("Error deserializing in tcpIPC.cs", Ex.Message.ToString());
                            Console.WriteLine("\n\r\n\r\n\r\n\r----->IPC Client Disconnect\n\r");
                            ns.Dispose();
                            break;
                        }

                        FrameCounter++;
                        DeserializedFramesReadyWait.Set();

#if DISP_RX
                        Console.WriteLine("RX: " + FrameCounter.ToString());
#endif
                        ns.Dispose();
                    }

                }
                if (listener != null)
                {
                    listener.Stop();
                    listener = null;
                }
                if (client != null)
                {
                    client.Close();
                    client.Dispose();
                    client = null;
                }
            }
        }
    }

}
