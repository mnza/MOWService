using JHAppMOWService.Util;
using System.Diagnostics;
using System.Management;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace JHAppMOWService
{
    public class ServerBaseInfo
    {
        /// <summary>
        /// 服务器名
        /// </summary>
        public string ServerName
        {
            get { return Environment.MachineName; }
        }
        /// <summary>
        /// 版本号
        /// </summary>
        public string Version
        {
            get { return Environment.OSVersion.VersionString; }
        }
        /// <summary>
        /// 服务器平台
        /// </summary>
        public string PlatForm
        {
            get { return RuntimeInformation.OSDescription; }
        }
        /// <summary>
        /// CPU数量
        /// </summary>
        public string ProcessorCount
        {
            get { return Environment.ProcessorCount.ToString(); }
        }
        public string CLRVersion
        {
            get { return Environment.Version.ToString(); }
        }
        /// <summary>
        /// 磁盘总容量
        /// </summary>
        public string TotalSpace
        {
            get
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    //windows服务器
                    long totalSpace = 0;
                    foreach (Dictionary<string, string> disk in Utils.GetDiskInfoForWindows())
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
        public string UsedSpace
        {
            get
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    //windows服务器
                    long totalSpace = 0;
                    foreach (Dictionary<string, string> disk in Utils.GetDiskInfoForWindows())
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
        public string FreeSpace
        {
            get
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    //windows服务器
                    long totalSpace = 0;
                    foreach (Dictionary<string, string> disk in Utils.GetDiskInfoForWindows())
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
        /// <summary>
        /// 服务器IP
        /// </summary>
        public string Ip
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
            }
        }
        /// <summary>
        /// 时间戳
        /// </summary>
        public long CurrentTime
        {
            get
            {
                TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
                return Convert.ToInt64(ts.TotalSeconds);
            }
        }
        /*
         PerformanceCounter: 表示 Windows NT 性能计数器组件。
         */
        //只支持windows系统
        [SupportedOSPlatform("windows")]
        private readonly PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        [SupportedOSPlatform("windows")]
        private readonly PerformanceCounter ramCounter = new PerformanceCounter("Memory", "Available MBytes");
        [SupportedOSPlatform("windows")]
        private readonly PerformanceCounter uptime = new PerformanceCounter("System", "System Up Time");
        /// <summary>
        /// 系统运行时间
        /// </summary>
        /// <returns></returns>
        public TimeSpan SystemUpTime
        {
            get
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    //windows服务器
                    uptime.NextValue();
                    TimeSpan ts = TimeSpan.FromSeconds(uptime.NextValue());
                    return ts;
                }
                else
                {
                    return TimeSpan.Zero;
                }
            }
        }
        /// <summary>
        /// 物理总内存
        /// </summary>
        /// <returns></returns>
        public string PhysicalMemory
        {
            get
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    //windows服务器
                    string str = "";
                    ManagementObjectSearcher objCS = new("SELECT * FROM Win32_ComputerSystem");
                    foreach (ManagementObject objMgmt in objCS.Get())
                    {
                        str = objMgmt["totalphysicalmemory"].ToString();
                    }
                    return str;
                }
                else
                {
                    return "";
                }
            }
        }
        /// <summary>
        /// cpu使用率
        /// </summary>
        /// <returns></returns>
        public string CurrentCpuUsage
        {
            get
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    return cpuCounter.NextValue() + "%";
                }
                else
                {
                    return "";
                }
            }
        }
        /// <summary>
        ///  当前可用内存
        /// </summary>
        /// <returns></returns>
        public string AvailableRAM
        {
            get
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    return ramCounter.NextValue().ToString();
                }
                else
                {
                    return "";
                }
            }
        }
    }
}
