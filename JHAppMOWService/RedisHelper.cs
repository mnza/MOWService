using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JHAppMOWService
{
    /// <summary>
    /// redis帮助类
    /// </summary>
    /// <remarks>
    /// 2021.12.08 张磊.创建
    /// </remarks>

    internal class RedisHelper
    {
        private static readonly object Locker = new object();
        //连接多路复用器
        private ConnectionMultiplexer _redisMultiplexer;
        private IDatabase _db = null;
        private static RedisHelper _instance = null;
        public static RedisHelper Instance 
        { get 
            { 
                if(_instance == null)
                {
                    lock(Locker)
                    {
                        if(_instance == null)
                        {
                            _instance = new RedisHelper();
                        }
                    }
                }
                return _instance;
            } 
        }

        public void InitConnect(IConfiguration configuration)
        {
            try
            {
                //连接redis服务器
                string redisConnection = configuration.GetConnectionString("RedisConnectionString");
                _redisMultiplexer = ConnectionMultiplexer.Connect(redisConnection);
                _db = _redisMultiplexer.GetDatabase();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _redisMultiplexer = null;
                _db = null;
            }
        }
        
        public RedisHelper()
        {

        }

        #region String 
        /// <summary>
        /// 保存单个key value
        /// </summary>
        /// <param name="value">保存的值</param>
        /// <param name="expiry">过期时间</param>
        public bool SetStringKey(string key, string value, TimeSpan? expiry = default(TimeSpan?))
        {
            TimeSpan ts = new TimeSpan(0,5,60);
            return _db.StringSet(key, value,ts);
        }

        /// <summary>
        /// 获取单个key的值
        /// </summary>
        public RedisValue GetStringKey(string key)
        {
            return _db.StringGet(key);
        }


        /// <summary>
        /// 获取一个key的对象
        /// </summary>
        public T GetStringKey<T>(string key)
        {
            if (_db == null)
            {
                return default;
            }
            var value = _db.StringGet(key);
            if (value.IsNullOrEmpty)
            {
                return default;
            }
            return JsonConvert.DeserializeObject<T>(value);
        }

        /// <summary>
        /// 保存一个对象
        /// </summary>
        /// <param name="obj"></param>
        public bool SetStringKey<T>(string key, T obj, TimeSpan? expiry = default(TimeSpan?))
        {
            if (_db == null)
            {
                return false;
            }
            string json = JsonConvert.SerializeObject(obj);
            return _db.StringSet(key, json, expiry);
        }

        #endregion

        public void ListRemove<T>(string key,T value)
        {
            _db.ListRightPush(key, JsonConvert.SerializeObject(value));
        }
    }
}
