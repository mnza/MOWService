using System.Diagnostics;
using System.Management;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace JHAppMOWService
{
    /// <summary>
    /// 服务器硬件信息基类
    /// </summary>
    public class Device
    {
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
                if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
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
                if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    return ramCounter.NextValue() + "MB";
                }else
                {
                    return "";
                }
            }
        }
    }
}
