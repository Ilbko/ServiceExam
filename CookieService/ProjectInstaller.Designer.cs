using System;

namespace CookieService
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.serviceProcessInstaller1 = new System.ServiceProcess.ServiceProcessInstaller();
            this.serviceInstaller1 = new System.ServiceProcess.ServiceInstaller();
            // 
            // serviceProcessInstaller1
            // 
            this.serviceProcessInstaller1.Password = null;
            this.serviceProcessInstaller1.Username = null;
            this.serviceProcessInstaller1.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            // 
            // serviceInstaller1
            // 
            this.serviceInstaller1.ServiceName = "ServiceExam";
            this.serviceInstaller1.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.serviceProcessInstaller1,
            this.serviceInstaller1});

            this.AfterInstall += ProjectInstaller_AfterInstall;
            this.AfterRollback += ProjectInstaller_AfterRollback;
            this.AfterUninstall += ProjectInstaller_AfterUninstall;
        }

        private void ProjectInstaller_AfterUninstall(object sender, System.Configuration.Install.InstallEventArgs e)
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine("Служба деинсталлирована.");
            Console.BackgroundColor = ConsoleColor.Black;
        }

        private void ProjectInstaller_AfterRollback(object sender, System.Configuration.Install.InstallEventArgs e)
        {
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Ошибка при установке службы.");
            Console.BackgroundColor = ConsoleColor.Black;
        }

        private void ProjectInstaller_AfterInstall(object sender, System.Configuration.Install.InstallEventArgs e)
        {
            Console.BackgroundColor = ConsoleColor.Green;
            Console.WriteLine("Служба установлена.");
            Console.BackgroundColor = ConsoleColor.Black;
        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller serviceProcessInstaller1;
        private System.ServiceProcess.ServiceInstaller serviceInstaller1;
    }
}