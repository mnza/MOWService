using JHAppMOWService.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

namespace JHAppMOWService
{
    /// <summary>
    /// 服务器信息类
    /// </summary>
    public class Server
    {
        /// <summary>
        /// 服务器基本信息
        /// </summary>
        public ServerBaseInfo BaseInfo
        {
            get
            {
                return new ServerBaseInfo();
            }
        }
       
        public List<ProcessMonitor> ProcessMonitors { get; set; }

}
}
