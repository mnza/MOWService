using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
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
    internal static class ServerInfo
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
                    List<Dictionary<string, string>> diskInfoDic = GetDiskInfoForWindows();
                    foreach(Dictionary<string,string> diskInfo in diskInfoDic)
                    {
                        totalSpace += System.Convert.ToInt64(diskInfo["totalSpace"]); 
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

        public static string UsedSpace
        {
            get
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    //windows服务器
                    long totalSpace = 0;
                    List<Dictionary<string, string>> diskInfoDic = GetDiskInfoForWindows();
                    foreach (Dictionary<string, string> diskInfo in diskInfoDic)
                    {
                        totalSpace += System.Convert.ToInt64(diskInfo["totalSpace"]);
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

        public static List<Dictionary<string, string>> GetDiskInfoForWindows()
        {
            List<Dictionary<string, string>> diskInfoDic = new List<Dictionary<string, string>>();
            ManagementClass diskClass = new ManagementClass("Win32_LogicalDisk");
            ManagementObjectCollection disks = diskClass.GetInstances();
            foreach (ManagementObject disk in disks)
            {
                Dictionary<string, string> diskInfo = new Dictionary<string, string>();
                try
                {
                    // 磁盘名称
                    diskInfo["Name"] = disk["Name"].ToString();
                    // 磁盘描述
                    diskInfo["Description"] = disk["Description"].ToString();
                    // 磁盘总容量，可用空间，已用空间
                    if (System.Convert.ToInt64(disk["Size"]) > 0)
                    {
                        long totalSpace = System.Convert.ToInt64(disk["Size"]) / ConstData.GB;
                        long freeSpace = System.Convert.ToInt64(disk["FreeSpace"]) / ConstData.GB;
                        long usedSpace = totalSpace - freeSpace;
                        diskInfo["totalSpace"] = totalSpace.ToString();
                        diskInfo["usedSpace"] = usedSpace.ToString();
                        diskInfo["freeSpace"] = freeSpace.ToString();
                    }
                    diskInfoDic.Add(diskInfo);
                }
                catch (Exception ex)
                {
                    //Throw ex;
                }
            }
            return diskInfoDic;
        }
    }
}
