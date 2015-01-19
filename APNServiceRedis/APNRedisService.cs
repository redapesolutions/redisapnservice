using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using KueNotification.libs;

namespace APNServiceRedis
{
    public partial class APNRedisService : ServiceBase
    {
        private System.Diagnostics.EventLog eventlog;
        private string LOG_NAME, SOURCE_NAME = "source", subscribeKey, keyPath, password;
        private bool isProduction;

        public APNRedisService()
        {
            InitializeComponent();
            LOG_NAME = this.GetType().Name;

            eventlog = new System.Diagnostics.EventLog();
            if (!System.Diagnostics.EventLog.SourceExists(SOURCE_NAME))
            {
                System.Diagnostics.EventLog.CreateEventSource(
                    SOURCE_NAME, LOG_NAME);
            }
            eventlog.Source = SOURCE_NAME;
            eventlog.Log = LOG_NAME;
            subscribeKey = ConfigurationManager.AppSettings["SubscribeKey"];
            keyPath = ConfigurationManager.AppSettings["KeyPath"];
            isProduction = Boolean.Parse(ConfigurationManager.AppSettings["APNProduction"]);

            // get password from machine environment variable
            password = Environment.GetEnvironmentVariable("APNPassword", EnvironmentVariableTarget.Machine);
        }

        protected override void OnStart(string[] args)
        {
            Kue kue = new Kue(password, isProduction, keyPath);
            kue.subscribe(subscribeKey);
            eventlog.WriteEntry("starting up apn redis");
        }

        protected override void OnStop()
        {
            eventlog.WriteEntry("stopping..");
        }


    }
}
