using KueNotification.libs;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using PushNotification;

namespace KueNotification
{
    class Program
    {
        static void Main(string[] args)
        {
            
            var subscribeKey = ConfigurationManager.AppSettings["SubscribeKey"];
            string keyPath = ConfigurationManager.AppSettings["KeyPath"];
            bool isProduction = Boolean.Parse(ConfigurationManager.AppSettings["APNProduction"]);

            // get password from machine environment variable
            string password = Environment.GetEnvironmentVariable("APNPassword", EnvironmentVariableTarget.Machine);

            Kue kue = new Kue(password, isProduction, keyPath);

            // subscribe to redis
            kue.subscribe(subscribeKey);
 
            Console.ReadLine();
        }
    }
}
