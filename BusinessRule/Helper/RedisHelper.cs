using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using StackExchange.Redis;
using Newtonsoft.Json;

namespace BusinessRule.Helper
{
    public class RedisHelper
    {
        private readonly IConnectionMultiplexer connection;
        private readonly IDatabase _database;
        private readonly IServer _server;

        public RedisHelper()
        {
            connection = ConnectionMultiplexer.Connect("192.168.1.12:6379,ssl=false,password=Redis123!@#");
            _database = connection.GetDatabase();

            EndPoint[] endPoint = connection.GetEndPoints();
            _server = connection.GetServer(endPoint.First());
        }

        public string GetKeyStringValue(string key)
        {
            //RedisValue redisValue = new RedisValue();
            //if (connection.IsConnected)
            //{
            //    redisValue = _database.StringGet(key);
            //} else
            //{
            //    redisValue = _second_database.StringGet(key);
            //}
            RedisValue redisValue = _database.StringGet(key);
            return redisValue.IsNullOrEmpty ? string.Empty : redisValue.ToString();
        }
        public T GetClassStringKeyValue<T>(string key)
        {
            try
            {
                //RedisValue redisValue = new RedisValue();

                //if (connection.IsConnected)
                //{
                //    redisValue = _database.StringGet(key);
                //}
                //else
                //{
                //    redisValue = _second_database.StringGet(key);
                //}

                RedisValue redisValue = _database.StringGet(key);
                if (redisValue.IsNullOrEmpty)
                {
                    return default(T);
                }

                return JsonConvert.DeserializeObject<T>(redisValue.ToString());
            }
            catch
            {
                return default(T);
            }
        }

        public void SetKeyStringValue(string key, string value, TimeSpan expiredTime)
        {
            _database.StringSet(key, value, expiredTime);
        }
        public void SetClassKeyStringValue<T>(string key, T value, TimeSpan expiredTime)
        {
            string value2 = JsonConvert.SerializeObject(value);
            _database.StringSet(key, value2, expiredTime);
        }

        public void DeleteKeyStringValue(string key)
        {
            _database.KeyDelete(key);
        }
        public void DeleteKeyMatchPattern(string pattern)
        {
            foreach (RedisKey item in _server.Keys(2, pattern, 250, 0L))
            {
                _database.KeyDelete(item);
            }
        }
    }
}