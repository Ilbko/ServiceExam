using System;
using System.IO;
using System.Threading;

namespace CookieService
{
    //Бесполезно, поскольку локальная служба не может работать с классами. (сетевая может!)
    //Абстрактный класс логгера (Единственное отличие потомков - строки лога и куки)
    public abstract class Logger
    {
        protected FileSystemWatcher watcher;
        protected bool enabled = true;
        public string logPath;
        public string cookiePath;

        public Logger(string cookiePath, string logPath)
        {
            this.watcher = new FileSystemWatcher(cookiePath);
            this.logPath = logPath;
            this.cookiePath = cookiePath;
            watcher.Deleted += Watcher_Deleted;
            watcher.Created += Watcher_Created;
            watcher.Changed += Watcher_Changed;
            watcher.Renamed += Watcher_Renamed;            
        }
        public void Start()
        {
            watcher.EnableRaisingEvents = true;
            while (enabled)
            {
                Thread.Sleep(60000);
            }
        }
        public void Stop()
        {
            watcher.EnableRaisingEvents = false;
            enabled = false;
        }

        protected void Watcher_Renamed(object sender, RenamedEventArgs e)
        {
            string fileEvent = "переименован в " + e.FullPath;
            string filePath = e.OldFullPath;
            RecordEntry(fileEvent, filePath);
        }

        protected void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            string fileEvent = "изменен";
            string filePath = e.FullPath;
            RecordEntry(fileEvent, filePath);
        }

        protected void Watcher_Created(object sender, FileSystemEventArgs e)
        {
            string fileEvent = "создан";
            string filePath = e.FullPath;
            RecordEntry(fileEvent, filePath);
        }

        protected void Watcher_Deleted(object sender, FileSystemEventArgs e)
        {
            string fileEvent = "удален";
            string filePath = e.FullPath;
            RecordEntry(fileEvent, filePath);
        }

        protected void RecordEntry(string fileEvent, string filePath)
        {
            lock (this)
            {
                using (StreamWriter writer = new StreamWriter(logPath, true))
                {
                    writer.WriteLine(String.Format("{0} файл {1} был {2}",
                        DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"), filePath, fileEvent));
                    writer.Flush();
                }
            }
        }
    }

    public class GoogleLogger : Logger
    {     
        public GoogleLogger() : base(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Google\Chrome\User Data\Default",
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\GoogleCookieServiceLog.txt"){}
    }
}
