//#define DEBUG_TIMER
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;

namespace ProbeMon
{
    public class UDPRxSeq
    {
       

        FrameControl Context;
        public IPEndPoint mRxIpEndPoint;
        public UdpClient mUDPRx;
        public UdpState mRxState;
        public int mUDPPortNumber;

        public UDPRxSeq(FrameControl FC)
        {
            Context = FC;
            InitSocket(); 
        }

        public void InitSocket()
        {
            mRxState = new UdpState();
            IPEndPoint localpt = new IPEndPoint(IPAddress.Loopback, 48008);
            mRxIpEndPoint = new IPEndPoint(IPAddress.Loopback, 48008);
            mUDPRx = new UdpClient();
            mUDPRx.ExclusiveAddressUse = false;
            mUDPRx.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            mUDPRx.Client.Bind(localpt);

            IPEndPoint inEndPoint = new IPEndPoint(IPAddress.Any, 0);
        }
        public void StartUDP(Object stateInfo)
        {
            mRxState.ipEndPoint = mRxIpEndPoint;
            mRxState.udpClient = mUDPRx;
            mUDPRx.BeginReceive(new AsyncCallback(ReceiveCallback), mRxState);
        }
        public void ReceiveCallback(IAsyncResult ar)
        {
            UdpClient UdpRx = (UdpClient)((UdpState)(ar.AsyncState)).udpClient;
            IPEndPoint IpEndPoint = (IPEndPoint)((UdpState)(ar.AsyncState)).ipEndPoint;
            if (Context.ProgramIsClosing)
            {
                return;
            }

            Byte[] ReceiveBytes = UdpRx.EndReceive(ar, ref IpEndPoint);
            lock (Context.PacketParser.PacketBytesQueueLock)
            {
                Context.PacketParser.PacketBytesQueue.Add(ReceiveBytes); 
            }
            Context.PacketParser.PacketBytesReadyWait.Set();
            if (!ThreadPool.QueueUserWorkItem(new WaitCallback(StartUDP)))
            {
                return;
            }
        }

        public string ByteToStr(byte[] NetworkBuffer)
        {
            
            return Encoding.ASCII.GetString(NetworkBuffer, 0, NetworkBuffer.Length);  
        }

        public class UdpState
        {
            public UdpState() { }
            public IPEndPoint ipEndPoint;
            public UdpClient udpClient;
        }
    }
}
