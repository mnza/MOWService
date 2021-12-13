using System.Management;
using System.Runtime.Versioning;

namespace JHAppMOWService.Util
{
    /// <summary>
    /// 工具类
    /// </summary>
    /// <remarks>
    /// 2012.12.08 创建.张磊
    /// </remarks>
    internal static class Utils
    {
        /// <summary>
        /// 获取windows服务器磁盘大小
        /// </summary>
        /// <returns></returns>
        [SupportedOSPlatform("windows")]
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
                        long totalSpace = System.Convert.ToInt64(disk["Size"]);
                        long freeSpace = System.Convert.ToInt64(disk["FreeSpace"]);
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
