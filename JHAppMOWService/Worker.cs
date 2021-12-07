using System.Diagnostics;

namespace JHAppMOWService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration Configuration;

        public Worker(ILogger<Worker> logger, IConfiguration configuration)
        {
            _logger = logger;
            Configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("***********服务器整体信息****************");
            Console.WriteLine($"服务器名称：{ServerInfo.ServerName}");
            Console.WriteLine("版本号：" + ServerInfo.Version);
            Console.WriteLine("操作系统：" + ServerInfo.PlatForm);
            Console.WriteLine("核数：" + ServerInfo.ProcessorCount);
            Console.WriteLine(".net版本号：" + ServerInfo.CLRVersion);
            Console.WriteLine("磁盘总容量：" + ServerInfo.TotalSpace);
            string[] monitorArray = (string[])Configuration.GetSection("monitorProcess").Get(typeof(string[]));

            while (!stoppingToken.IsCancellationRequested)
            {
                bool b = Environment.Is64BitOperatingSystem;
                Console.WriteLine("***********服务器整体信息****************");
                
                Console.WriteLine("系统已运行时间：" + WindowsDeviceMonitor.GetSystemUpTime());
                Console.WriteLine("物理总内存：" + (double.Parse(WindowsDeviceMonitor.GetPhysicalMemory()) / (1024 * 1024 * 1024)).ToString("F2") + "GB");
                Console.WriteLine("当前CPU值：" + WindowsDeviceMonitor.getCurrentCpuUsage());
                Console.WriteLine("当前可用内存：" + WindowsDeviceMonitor.getAvailableRAM());
                Console.WriteLine("内存使用率：" );
                Console.WriteLine("***********服务信息****************");
                foreach (string monitor in monitorArray)
                {
                    Process[] pros = Process.GetProcessesByName(monitor);
                    if (pros.Length == 0)
                    {
                        _logger.LogInformation($"未找到进程：{monitor}");
                        continue;
                    }
                    using (Process myProcess = pros[0])
                    {

                        myProcess.Refresh();
                        Console.WriteLine();
                        Console.WriteLine($"{myProcess} -");
                        Console.WriteLine("-------------------------------------");

                        Console.WriteLine($"物理内存: {myProcess.WorkingSet64 / 1024}K");
                        Console.WriteLine($"优先级: {myProcess.BasePriority}");
                        //Console.WriteLine($"  Priority class            : {myProcess.PriorityClass}");
                        //Console.WriteLine($"  User processor time       : {myProcess.UserProcessorTime}");
                        //Console.WriteLine($"  Privileged processor time : {myProcess.PrivilegedProcessorTime}");
                        //Console.WriteLine($"  Total processor time      : {myProcess.TotalProcessorTime}");
                        Console.WriteLine($"分页缓冲池: {myProcess.PagedSystemMemorySize64/ConstData.KB}K");
                        Console.WriteLine($"提交大小: {myProcess.PagedMemorySize64/ ConstData.KB}K");
                        Console.WriteLine($"非分页缓冲池：{myProcess.NonpagedSystemMemorySize64 / ConstData.KB}K");
                        Console.WriteLine($"最大虚拟内存量：{myProcess.PeakVirtualMemorySize64 / ConstData.KB}K");
                        Console.WriteLine($"最大物理内存量：{myProcess.PeakWorkingSet64 / ConstData.KB}K");
                        Console.WriteLine($"专用内存量：{myProcess.PrivateMemorySize64 / ConstData.KB}K");
                        Console.WriteLine($"进程的名称：{myProcess.ProcessName }");
                        Console.WriteLine($"虚拟内存大小：{myProcess.VirtualMemorySize64 / ConstData.KB}K");

                        if (myProcess.Responding)
                        {
                            Console.WriteLine("Status = Running");
                        }
                        else
                        {
                            Console.WriteLine("Status = Not Responding");
                        }
                    }
                }
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1022200, stoppingToken);
            }
        }
    }
}