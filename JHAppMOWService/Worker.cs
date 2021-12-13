using Newtonsoft.Json;
using System.Diagnostics;
using System.Runtime.InteropServices;

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
            RedisHelper.Instance.InitConnect(Configuration);

            //判断服务器类型
            // ServerBaseInfo serverBaseInfo = new ServerBaseInfo();
            // Server server = new Server();

            //Console.WriteLine("***********服务器整体信息****************");
            //Console.WriteLine($"服务器名称：{ServerBaseInfo1.ServerName}");
            //Console.WriteLine($"服务器IP：{ServerBaseInfo1.Ip}");
            //Console.WriteLine($"系统版本号：{ServerBaseInfo1.Version}");
            //Console.WriteLine($"操作系统：{ServerBaseInfo1.PlatForm}");
            //Console.WriteLine($"核数：{ServerBaseInfo1.ProcessorCount}");
            //Console.WriteLine($".net版本号：{ServerBaseInfo1.CLRVersion}");
            //Console.WriteLine($"磁盘总容量：{ServerBaseInfo1.TotalSpace}GB");
            //Console.WriteLine($"磁盘已使用：{ServerBaseInfo1.UsedSpace}GB");
            //Console.WriteLine($"磁盘未使用：{ServerBaseInfo1.FreeSpace}GB");

            //RedisHelper.Instance.SetStringKey("IP", JsonConvert.SerializeObject(server));
            //windows服务器 显示各个磁盘分区使用情况
            //if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            //{
            //    foreach (Dictionary<string, string> disk in ServerBaseInfo1.DiskInfo)
            //    {
            //        Console.WriteLine($"{disk["Name"]}总空间{disk["totalSpace"]}GB，已使用{disk["usedSpace"]}GB，未使用{disk["freeSpace"]}GB");
            //    }
            //}
            string[] processArray = (string[])Configuration.GetSection("monitorProcess").Get(typeof(string[]));

            while (!stoppingToken.IsCancellationRequested)
            {
                Server server = new Server();
                //bool b = Environment.Is64BitOperatingSystem;
                //Console.WriteLine("***********服务器整体信息****************");

                //Console.WriteLine("系统已运行时间：" + WindowsDeviceMonitor.GetSystemUpTime());
                //Console.WriteLine("物理总内存：" + (double.Parse(WindowsDeviceMonitor.GetPhysicalMemory()) / (1024 * 1024 * 1024)).ToString("F2") + "GB");
                //Console.WriteLine("当前CPU值：" + WindowsDeviceMonitor.getCurrentCpuUsage());
                //Console.WriteLine("当前可用内存：" + WindowsDeviceMonitor.getAvailableRAM());
                //Console.WriteLine("内存使用率：");
                //Console.WriteLine("***********服务信息****************");
                List<ProcessMonitor> monitorList = new List<ProcessMonitor>();
                foreach (string pro in processArray)
                {
                    Process[] pros = Process.GetProcessesByName(pro);
                    if (pros.Length == 0)
                    {
                        _logger.LogInformation($"未找到进程：{pro}");
                        continue;
                    }
                    using (Process myProcess = pros[0])
                    {
                        ProcessMonitor monitor = new ProcessMonitor();
                        myProcess.Refresh();

                        monitor.WorkingSet64 = myProcess.WorkingSet64;
                        monitor.BasePriority = myProcess.BasePriority;
                        //Console.WriteLine($"  Priority class            : {myProcess.PriorityClass}");
                        //Console.WriteLine($"  User processor time       : {myProcess.UserProcessorTime}");
                        //Console.WriteLine($"  Privileged processor time : {myProcess.PrivilegedProcessorTime}");
                        //Console.WriteLine($"  Total processor time      : {myProcess.TotalProcessorTime}");
                        monitor.PagedSystemMemorySize64 = myProcess.PagedSystemMemorySize64;
                        monitor.PagedMemorySize64 = myProcess.PagedMemorySize64;
                        monitor.NonpagedSystemMemorySize64 = myProcess.NonpagedSystemMemorySize64;
                        monitor.PeakVirtualMemorySize64 = myProcess.PeakVirtualMemorySize64;
                        monitor.PeakWorkingSet64 = myProcess.PeakWorkingSet64;
                        monitor.PrivateMemorySize64 = myProcess.PrivateMemorySize64;
                        monitor.ProcessName = myProcess.ProcessName;
                        monitor.VirtualMemorySize64 = myProcess.VirtualMemorySize64;
                        var startTime = DateTime.UtcNow;
                        var startCpuUsage = Process.GetCurrentProcess().TotalProcessorTime;

                        await Task.Delay(500);

                        var endTime = DateTime.UtcNow;
                        var endCpuUsage = Process.GetCurrentProcess().TotalProcessorTime;

                        var cpuUsedMs = (endCpuUsage - startCpuUsage).TotalMilliseconds;
                        var totalMsPassed = (endTime - startTime).TotalMilliseconds;

                        var cpuUsageTotal = cpuUsedMs / (Environment.ProcessorCount * totalMsPassed);

                        monitor.CpuUsage = cpuUsageTotal * 100;
                        if (myProcess.Responding)
                        {
                            monitor.Status = "Running";
                        }
                        else
                        {
                            monitor.Status = "Not Responding";
                        }
                        monitorList.Add(monitor);
                    }
                }
                server.ProcessMonitors = monitorList;
                RedisHelper.Instance.SetStringKey($"{server.BaseInfo.Ip}:{server.BaseInfo.CurrentTime}", JsonConvert.SerializeObject(server));
                //_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}