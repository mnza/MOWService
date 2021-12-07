using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JHAppMOWService
{
    /// <summary>
    /// 磁盘信息类
    /// </summary>
    /// <remarks>
    /// 2021.12.07 创建.张磊
    /// 用于存放磁盘信息（单位GB）
    /// </remarks>
    internal class DiskInfo
    {
        /// <summary>
        /// 磁盘总大小
        /// </summary>
        public static long TotalSize { get; set; }
        /// <summary>
        /// 磁盘已使用大小
        /// </summary>
        public static long UsedSize { get; set; }
        /// <summary>
        /// 磁盘未使用大小
        /// </summary>
        public static long FreeSize { get; set; }

    }
}
