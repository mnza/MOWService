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
            Console.WriteLine("***********������������Ϣ****************");
            Console.WriteLine($"���������ƣ�{ServerInfo.ServerName}");
            Console.WriteLine("�汾�ţ�" + ServerInfo.Version);
            Console.WriteLine("����ϵͳ��" + ServerInfo.PlatForm);
            Console.WriteLine("������" + ServerInfo.ProcessorCount);
            Console.WriteLine(".net�汾�ţ�" + ServerInfo.CLRVersion);
            Console.WriteLine("������������" + ServerInfo.TotalSpace);
            string[] monitorArray = (string[])Configuration.GetSection("monitorProcess").Get(typeof(string[]));

            while (!stoppingToken.IsCancellationRequested)
            {
                bool b = Environment.Is64BitOperatingSystem;
                Console.WriteLine("***********������������Ϣ****************");
                
                Console.WriteLine("ϵͳ������ʱ�䣺" + WindowsDeviceMonitor.GetSystemUpTime());
                Console.WriteLine("�������ڴ棺" + (double.Parse(WindowsDeviceMonitor.GetPhysicalMemory()) / (1024 * 1024 * 1024)).ToString("F2") + "GB");
                Console.WriteLine("��ǰCPUֵ��" + WindowsDeviceMonitor.getCurrentCpuUsage());
                Console.WriteLine("��ǰ�����ڴ棺" + WindowsDeviceMonitor.getAvailableRAM());
                Console.WriteLine("�ڴ�ʹ���ʣ�" );
                Console.WriteLine("***********������Ϣ****************");
                foreach (string monitor in monitorArray)
                {
                    Process[] pros = Process.GetProcessesByName(monitor);
                    if (pros.Length == 0)
                    {
                        _logger.LogInformation($"δ�ҵ����̣�{monitor}");
                        continue;
                    }
                    using (Process myProcess = pros[0])
                    {

                        myProcess.Refresh();
                        Console.WriteLine();
                        Console.WriteLine($"{myProcess} -");
                        Console.WriteLine("-------------------------------------");

                        Console.WriteLine($"�����ڴ�: {myProcess.WorkingSet64 / 1024}K");
                        Console.WriteLine($"���ȼ�: {myProcess.BasePriority}");
                        //Console.WriteLine($"  Priority class            : {myProcess.PriorityClass}");
                        //Console.WriteLine($"  User processor time       : {myProcess.UserProcessorTime}");
                        //Console.WriteLine($"  Privileged processor time : {myProcess.PrivilegedProcessorTime}");
                        //Console.WriteLine($"  Total processor time      : {myProcess.TotalProcessorTime}");
                        Console.WriteLine($"��ҳ�����: {myProcess.PagedSystemMemorySize64/ConstData.KB}K");
                        Console.WriteLine($"�ύ��С: {myProcess.PagedMemorySize64/ ConstData.KB}K");
                        Console.WriteLine($"�Ƿ�ҳ����أ�{myProcess.NonpagedSystemMemorySize64 / ConstData.KB}K");
                        Console.WriteLine($"��������ڴ�����{myProcess.PeakVirtualMemorySize64 / ConstData.KB}K");
                        Console.WriteLine($"��������ڴ�����{myProcess.PeakWorkingSet64 / ConstData.KB}K");
                        Console.WriteLine($"ר���ڴ�����{myProcess.PrivateMemorySize64 / ConstData.KB}K");
                        Console.WriteLine($"���̵����ƣ�{myProcess.ProcessName }");
                        Console.WriteLine($"�����ڴ��С��{myProcess.VirtualMemorySize64 / ConstData.KB}K");

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