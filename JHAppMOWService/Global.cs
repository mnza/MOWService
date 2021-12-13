using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JHAppMOWService
{
    /// <summary>
    /// 全局变量类
    /// </summary>
    /// <remarks>
    /// 2021.12.07 张磊.创建
    /// </remarks>
    internal class Global
    {
        private static string _lockFlag = "GlobalLock";
        private static Global _instance;
        public static Global Instance
        {
            get
            {
                lock (_lockFlag)
                {
                    if (_instance == null)
                    {
                        _instance = new Global();
                    }
                    return _instance;
                }

            }
        }

        public const int KB = 1024;
        public const int MB = 1024 * 1024;
        public const int GB = 1024 * 1024 * 1024;

    }
}
