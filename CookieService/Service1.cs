using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
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
            googleLogger = new GoogleLogger();
        }

        protected override void OnStop()
        {
        }
    }
}
