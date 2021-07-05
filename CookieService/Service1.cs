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
        private bool loggerEnabled = true;
        public Service1()
        {
            InitializeComponent();
            this.CanPauseAndContinue = true;
            this.CanShutdown = true;
            this.CanStop = true;
            this.AutoLog = true;

            //Путь к файлу логов
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
                //Task.Run(() => StartGoogleLogger());
                //Task.Run(() => StartOperaLogger());
                //Task googleTask = new Task(() => StartGoogleLogger());
                //Task operaTask = new Task(() => StartOperaLogger());

                //Task.WhenAll(googleTask, operaTask);
                Task.Run(() => StartLoggers());
            } catch (Exception e) { EventLog.WriteEntry(e.Message); }
            //this.StartGoogleLogger();
        }
        protected override void OnStop()
        {
            this.loggerEnabled = false;
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

        private void StartLoggers()
        {
            lock (this)
            {
                try
                {
                    //Массив файловых наблюдателей. Каждый из них следит за изменениями в папке с куки разных браузеров. 
                    FileSystemWatcher[] fileWatchers = new FileSystemWatcher[3] { new FileSystemWatcher(Path.GetPathRoot(Environment.SystemDirectory) + @"Users\" + $"{this.GetUserName()}" + @"\AppData\Local\Google\Chrome\User Data\Default"), 
                    new FileSystemWatcher(Path.GetPathRoot(Environment.SystemDirectory) + @"Users\" + $"{this.GetUserName()}" + @"\AppData\Roaming\Opera Software\Opera Stable"),
                    new FileSystemWatcher(Directory.GetFiles(Path.GetPathRoot(Environment.SystemDirectory) + @"Users\" + $"{GetUserName()}" + @"\AppData\Roaming\Mozilla\Firefox\Profiles", "cookies.sqlite", SearchOption.AllDirectories)[0].Replace(@"\cookies.sqlite", string.Empty))};

                    EventLog.WriteEntry("fileWatchers done");
                    loggerEnabled = true;

                    //Подписка на события
                    foreach (FileSystemWatcher fileWatcher in fileWatchers)
                    {
                        fileWatcher.Deleted += Watcher_Deleted;
                        fileWatcher.Created += Watcher_Created;
                        fileWatcher.Changed += Watcher_Changed;
                        fileWatcher.Renamed += Watcher_Renamed;

                        fileWatcher.EnableRaisingEvents = true;
                    }

                    while (loggerEnabled)
                    {
                        Thread.Sleep(1000);
                    }
                }
                catch (Exception e) { EventLog.WriteEntry(e.Message); }
            }        
        }

        //private void StartGoogleLogger()
        //{
        //    lock (this)
        //    {
        //        try
        //        {
        //            string cookiePath = Path.GetPathRoot(Environment.SystemDirectory) + @"Users\" + $"{this.GetUserName()}" + @"\AppData\Local\Google\Chrome\User Data\Default";
        //            EventLog.WriteEntry(cookiePath);
        //            loggerEnabled = true;

        //            FileSystemWatcher googleWatcher = new FileSystemWatcher(cookiePath);

        //            googleWatcher.Deleted += Watcher_Deleted;
        //            googleWatcher.Created += Watcher_Created;
        //            googleWatcher.Changed += Watcher_Changed;
        //            googleWatcher.Renamed += Watcher_Renamed;

        //            googleWatcher.EnableRaisingEvents = true;

        //            try
        //            {
        //                while (loggerEnabled)
        //                {
        //                    Thread.Sleep(1000);
        //                }
        //            } catch (Exception e) { EventLog.WriteEntry(e.Message); }
        //        } catch (Exception e) { EventLog.WriteEntry(e.Message); }
        //    }
        //}

        private string GetUserName()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT UserName FROM Win32_ComputerSystem");
            ManagementObjectCollection collection = searcher.Get();

            return collection.Cast<ManagementBaseObject>().First()["UserName"].ToString().Split('\\')[1].Split('-').Last();
        }        
    }
}
