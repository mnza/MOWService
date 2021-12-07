using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

namespace JHAppMOWService
{
    /// <summary>
    /// windows设备监视类
    /// </summary>
    /// <remarks>
    /// 2021.12.07：创建.张磊
    /// </remarks>
    [SupportedOSPlatform("windows")]
    public class WindowsDeviceMonitor
    {
        /*
         PerformanceCounter: 表示 Windows NT 性能计数器组件。
         */
        //只支持windows系统
        static readonly PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        static readonly PerformanceCounter ramCounter = new PerformanceCounter("Memory", "Available MBytes");
        static readonly PerformanceCounter uptime = new PerformanceCounter("System", "System Up Time");


        public static bool GetInternetAvilable()
        {
            bool networkUp = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
            return networkUp;
        }
        /// <summary>
        /// 系统运行时间
        /// </summary>
        public static TimeSpan GetSystemUpTime()
        {
            uptime.NextValue();
            TimeSpan ts = TimeSpan.FromSeconds(uptime.NextValue());
            return ts;
        }
        /// <summary>
        /// 物理总内存
        /// </summary>
        public static string GetPhysicalMemory()
        {
            string str = "";
            ManagementObjectSearcher objCS = new("SELECT * FROM Win32_ComputerSystem");
            foreach (ManagementObject objMgmt in objCS.Get())
            {
                str = objMgmt["totalphysicalmemory"].ToString();
            }
            return str;
        }
        /// <summary>
        /// cpu使用率
        /// </summary>
        public static string getCurrentCpuUsage()
        {
            return cpuCounter.NextValue() + "%";
        }
        /// <summary>
        /// 当前可用内存
        /// </summary>
        public static string getAvailableRAM()
        {
            return ramCounter.NextValue() + "MB";
        }
    }
}
