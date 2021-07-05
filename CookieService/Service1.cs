using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Management;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;

namespace CookieService
{
    public partial class Service1 : ServiceBase
    {
        private string logPath;
        private bool googleLoggerEnabled;
        public Service1()
        {
            InitializeComponent();
            this.CanPauseAndContinue = true;
            this.CanShutdown = true;
            this.CanStop = true;
            this.AutoLog = true;

            this.logPath = Path.GetPathRoot(Environment.SystemDirectory) + @"Users\" + $"{GetUserName()}" + @"\Documents\CookieServiceLog.txt";
            EventLog.WriteEntry(this.logPath);
        }

        protected override void OnStart(string[] args)
        {
            //this.googleLogger = new GoogleLogger();
            //Thread googleLoggerThread = new Thread(new ThreadStart(this.googleLogger.Start));
            //googleLoggerThread.Start();

            try
            {
                Task.Run(() => StartGoogleLogger());
            } catch (Exception e) { EventLog.WriteEntry(e.Message); }
            //this.StartGoogleLogger();
        }
        protected override void OnStop()
        {
            this.googleLoggerEnabled = false;
        }

        private void Watcher_Renamed(object sender, RenamedEventArgs e)
        {
            string fileEvent = "переименован в " + e.FullPath;
            string filePath = e.OldFullPath;
            RecordEntry(fileEvent, filePath, this.logPath);
        }

        private void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            string fileEvent = "изменен";
            string filePath = e.FullPath;
            RecordEntry(fileEvent, filePath, this.logPath);
        }

        private void Watcher_Created(object sender, FileSystemEventArgs e)
        {
            string fileEvent = "создан";
            string filePath = e.FullPath;
            RecordEntry(fileEvent, filePath, this.logPath);
        }

        private void Watcher_Deleted(object sender, FileSystemEventArgs e)
        {
            string fileEvent = "удален";
            string filePath = e.FullPath;
            RecordEntry(fileEvent, filePath, this.logPath);
        }

        protected void RecordEntry(string fileEvent, string filePath, string logPath)
        {
            lock (this)
            {
                try
                {
                    using (StreamWriter writer = new StreamWriter(logPath, true))
                    {
                        EventLog.WriteEntry(logPath);
                        writer.WriteLine(String.Format("{0} файл {1} был {2}",
                            DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"), filePath, fileEvent));
                        writer.Flush();
                    }
                } catch (Exception e) { EventLog.WriteEntry(e.Message); }
            }
        }

        private void StartGoogleLogger()
        {
            lock (this)
            {
                try
                {
                    string cookiePath = Path.GetPathRoot(Environment.SystemDirectory) + @"Users\" + $"{this.GetUserName()}" + @"\AppData\Local\Google\Chrome\User Data\Default";
                    EventLog.WriteEntry(cookiePath);
                    googleLoggerEnabled = true;

                    FileSystemWatcher googleWatcher = new FileSystemWatcher(cookiePath);

                    googleWatcher.Deleted += Watcher_Deleted;
                    googleWatcher.Created += Watcher_Created;
                    googleWatcher.Changed += Watcher_Changed;
                    googleWatcher.Renamed += Watcher_Renamed;

                    googleWatcher.EnableRaisingEvents = true;

                    try
                    {
                        while (googleLoggerEnabled)
                        {
                            Thread.Sleep(3000);
                        }
                    } catch (Exception e) { EventLog.WriteEntry(e.Message); }
                } catch (Exception e) { EventLog.WriteEntry(e.Message); }
            }
        }

        private string GetUserName()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT UserName FROM Win32_ComputerSystem");
            ManagementObjectCollection collection = searcher.Get();

            return collection.Cast<ManagementBaseObject>().First()["UserName"].ToString().Split('\\')[1].Split('-').Last();
        }        
    }
}
