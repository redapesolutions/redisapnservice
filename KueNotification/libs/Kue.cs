using Newtonsoft.Json;
using PushNotification;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KueNotification.libs
{
    public class Kue
    {
        private static ConnectionMultiplexer redis;
        private static APNPushNotify apnService;
        private string ip = ConfigurationManager.AppSettings["RedisIP"];

        public Kue() {
            redis = ConnectionMultiplexer.Connect(ip);
            apnService = new APNPushNotify("", true, null);
        }

        public Kue(string password, bool production, string path)
        {
            redis = ConnectionMultiplexer.Connect(ip);
            apnService = new APNPushNotify(password, production, path);
        }

        public ConnectionMultiplexer getConnection()
        {
            return redis;
        }

        public void subscribe(string subsribeKey)
        {
            ISubscriber sub = redis.GetSubscriber();
            
            sub.Subscribe(subsribeKey, (channel, message) =>
            {
                var tokens = (JsonConvert.DeserializeObject<Message>(message)).tokens;
                var payload = (JsonConvert.DeserializeObject<PushNotification.Model.Payload>(message));

                apnService.SendMultipleNotifications(tokens, payload.message, payload);
            });
        }
    }

    class Message {
        public List<string> tokens { get; set; }
    }
}
