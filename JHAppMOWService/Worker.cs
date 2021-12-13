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

            //�жϷ���������
            // ServerBaseInfo serverBaseInfo = new ServerBaseInfo();
            // Server server = new Server();

            //Console.WriteLine("***********������������Ϣ****************");
            //Console.WriteLine($"���������ƣ�{ServerBaseInfo1.ServerName}");
            //Console.WriteLine($"������IP��{ServerBaseInfo1.Ip}");
            //Console.WriteLine($"ϵͳ�汾�ţ�{ServerBaseInfo1.Version}");
            //Console.WriteLine($"����ϵͳ��{ServerBaseInfo1.PlatForm}");
            //Console.WriteLine($"������{ServerBaseInfo1.ProcessorCount}");
            //Console.WriteLine($".net�汾�ţ�{ServerBaseInfo1.CLRVersion}");
            //Console.WriteLine($"������������{ServerBaseInfo1.TotalSpace}GB");
            //Console.WriteLine($"������ʹ�ã�{ServerBaseInfo1.UsedSpace}GB");
            //Console.WriteLine($"����δʹ�ã�{ServerBaseInfo1.FreeSpace}GB");

            //RedisHelper.Instance.SetStringKey("IP", JsonConvert.SerializeObject(server));
            //windows������ ��ʾ�������̷���ʹ�����
            //if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            //{
            //    foreach (Dictionary<string, string> disk in ServerBaseInfo1.DiskInfo)
            //    {
            //        Console.WriteLine($"{disk["Name"]}�ܿռ�{disk["totalSpace"]}GB����ʹ��{disk["usedSpace"]}GB��δʹ��{disk["freeSpace"]}GB");
            //    }
            //}
            string[] processArray = (string[])Configuration.GetSection("monitorProcess").Get(typeof(string[]));

            while (!stoppingToken.IsCancellationRequested)
            {
                Server server = new Server();
                //bool b = Environment.Is64BitOperatingSystem;
                //Console.WriteLine("***********������������Ϣ****************");

                //Console.WriteLine("ϵͳ������ʱ�䣺" + WindowsDeviceMonitor.GetSystemUpTime());
                //Console.WriteLine("�������ڴ棺" + (double.Parse(WindowsDeviceMonitor.GetPhysicalMemory()) / (1024 * 1024 * 1024)).ToString("F2") + "GB");
                //Console.WriteLine("��ǰCPUֵ��" + WindowsDeviceMonitor.getCurrentCpuUsage());
                //Console.WriteLine("��ǰ�����ڴ棺" + WindowsDeviceMonitor.getAvailableRAM());
                //Console.WriteLine("�ڴ�ʹ���ʣ�");
                //Console.WriteLine("***********������Ϣ****************");
                List<ProcessMonitor> monitorList = new List<ProcessMonitor>();
                foreach (string pro in processArray)
                {
                    Process[] pros = Process.GetProcessesByName(pro);
                    if (pros.Length == 0)
                    {
                        _logger.LogInformation($"δ�ҵ����̣�{pro}");
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