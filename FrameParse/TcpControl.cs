using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProbeMon
{
    public class TcpControl
    {
        Launcher Context;
        public TcpControl(Launcher FC)
        {
            Context = FC;
        }
        public void CreateSitePacketList(byte SiteId)
        {
            lock (Context.PacketParser.PacketBytesMasterListLock)
            {
                Context.PacketParser.SiteListArrayInUse[SiteId] = true;
                Context.PacketParser.PacketBytesMasterList.Add(Context.PacketParser.SiteListArray[SiteId]); 
            }
            //Context.PacketParser.PacketBytesMasterListReadyWait.Set();

        }
    }
}
