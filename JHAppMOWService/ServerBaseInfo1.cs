using JHAppMOWService.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

namespace JHAppMOWService
{
    /// <summary>
    /// 服务器基本信息类
    /// </summary>
    /// <remarks>
    /// 2021.12.07 创建.张磊
    ///</remarks>
    public static class ServerBaseInfo1
    {
        /// <summary>
        /// 服务器名
        /// </summary>
        public static string ServerName
        {
            get { return Environment.MachineName; }
        }
        /// <summary>
        /// 版本号
        /// </summary>
        public static string Version
        {
            get { return Environment.OSVersion.VersionString; }
        }
        /// <summary>
        /// 服务器平台
        /// </summary>
        public static string PlatForm
        {
            get { return RuntimeInformation.OSDescription; }
        }
        /// <summary>
        /// CPU数量
        /// </summary>
        public static string ProcessorCount
        {
            get { return Environment.ProcessorCount.ToString(); }
        }
        public static string CLRVersion
        {
            get { return Environment.Version.ToString(); }
        }
        /// <summary>
        /// 磁盘总容量
        /// </summary>
        public static string TotalSpace
        {
            get
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    //windows服务器
                    long totalSpace = 0;
                    foreach (Dictionary<string, string> disk in DiskInfo)
                    {
                        totalSpace += System.Convert.ToInt64(disk["totalSpace"]);
                    }
                    return totalSpace.ToString();

                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    //linux服务器
                    return "";
                }
                return "未知的服务器类型！";

            }
        }
        /// <summary>
        /// 磁盘已使用
        /// </summary>
        public static string UsedSpace
        {
            get
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    //windows服务器
                    long totalSpace = 0;
                    foreach (Dictionary<string, string> disk in DiskInfo)
                    {
                        totalSpace += System.Convert.ToInt64(disk["usedSpace"]);
                    }
                    return totalSpace.ToString();

                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    //linux服务器
                    return "";
                }
                return "未知的服务器类型！";
            }
        }
        /// <summary>
        /// 磁盘未使用空间
        /// </summary>
        public static string FreeSpace
        {
            get
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    //windows服务器
                    long totalSpace = 0;
                    foreach (Dictionary<string, string> disk in DiskInfo)
                    {
                        totalSpace += System.Convert.ToInt64(disk["freeSpace"]);
                    }
                    return totalSpace.ToString();

                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    //linux服务器
                    return "";
                }
                return "未知的服务器类型！";
            }
        }

        private static List<Dictionary<string, string>> _diskInfo = new List<Dictionary<string, string>>();
        /// <summary>
        /// windows磁盘信息
        /// </summary>
        [SupportedOSPlatform("windows")]
        public static List<Dictionary<string, string>> DiskInfo
        {
            get
            {
                if (_diskInfo.Count == 0)
                {
                    _diskInfo = Utils.GetDiskInfoForWindows();
                }
                return _diskInfo;
            }
        }

        public static string Ip
        {
            get
            {
                string ip = "";

                //linux下获取IP地址，没有测试，windows测试通过
                NetworkInterface[] it = NetworkInterface.GetAllNetworkInterfaces();

                foreach (NetworkInterface net in it)
                {
                    IPInterfaceProperties ipi = net.GetIPProperties();
                    foreach (UnicastIPAddressInformation t in ipi.UnicastAddresses)
                    {
                        if (t.Address.AddressFamily == AddressFamily.InterNetwork && t.IsDnsEligible)
                        {
                            ip = t.Address.ToString();
                        }

                    }
                }

                return ip;
                //ipi.UnicastAddresses[0].Address.;
                //return NetworkInterface.GetAllNetworkInterfaces()
                //.Select(p => p.GetIPProperties())
                //.SelectMany(p => p.UnicastAddresses)
                //.FirstOrDefault(p => p.Address.AddressFamily == AddressFamily.InterNetwork && !IPAddress.IsLoopback(p.Address) )?.Address.ToString();
                //获取本机可用的IP地址
                //return  Dns.GetHostAddresses(Dns.GetHostName()).Where(x => x.AddressFamily == AddressFamily.InterNetwork).Select(x => x.ToString()).ToList().FirstOrDefault();
            }
        }

    }
}
