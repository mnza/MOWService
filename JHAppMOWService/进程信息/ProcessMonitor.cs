namespace JHAppMOWService
{
    /// <summary>
    /// 进程监控信息类
    /// </summary>
    /// <remarks>
    /// 2021.12.08 创建.张磊
    /// </remarks>
    public class ProcessMonitor
    {
        /// <summary>
        /// 物理内存
        /// </summary>
        public long WorkingSet64 { get; set; }
        /// <summary>
        /// 优先级
        /// </summary>
        public int BasePriority { get; set; }
        /// <summary>
        /// 分页缓冲池
        /// </summary>
        public long PagedSystemMemorySize64 { get; set; }
        /// <summary>
        /// 提交大小
        /// </summary>
        public long PagedMemorySize64 { get; set; }
        /// <summary>
        /// 非分页缓冲池
        /// </summary>
        public long NonpagedSystemMemorySize64 { get; set; }
        /// <summary>
        /// 最大虚拟内存量
        /// </summary>
        public long PeakVirtualMemorySize64 { get; set; }
        /// <summary>
        /// 最大物理内存量
        /// </summary>
        public long PeakWorkingSet64 { get; set; }
        /// <summary>
        /// 专用内存量
        /// </summary>
        public long PrivateMemorySize64 { get; set; }
        /// <summary>
        /// 进程名称
        /// </summary>
        public string ProcessName { get; set; }
        /// <summary>
        /// 虚拟内存大小
        /// </summary>
        public long VirtualMemorySize64 { get; set; }
        /// <summary>
        /// 进程状态
        /// </summary>
        public string Status { get; set; }
        
        public Double CpuUsage { get; set; }    


    }
}
