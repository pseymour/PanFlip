namespace PanFlip
{
    using System.Configuration.Install;

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
            try
            {
                if (disposing)
                {
                    if (components != null)
                    {
                        components.Dispose();
                    }

                    if (this.serviceInstaller != null)
                    {
                        this.serviceInstaller.Dispose();
                    }

                    if (this.serviceProcessInstaller != null)
                    {
                        this.serviceProcessInstaller.Dispose();
                    }
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.serviceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.serviceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // serviceProcessInstaller
            // 
            this.serviceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalService;
            this.serviceProcessInstaller.Password = null;
            this.serviceProcessInstaller.Username = null;
            // 
            // serviceInstaller
            // 
            this.serviceInstaller.Description = "Enables users toggle the state of the Palo Alto GlobalProtect VPN Client.";
            this.serviceInstaller.DisplayName = "PanFlip";
            this.serviceInstaller.ServiceName = "PanFlip";
            this.serviceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            this.serviceInstaller.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.serviceInstaller_AfterInstall);
            this.serviceInstaller.BeforeUninstall += new System.Configuration.Install.InstallEventHandler(this.serviceInstaller_BeforeUninstall);
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.serviceProcessInstaller,
            this.serviceInstaller});

        }

        #endregion

        /// <summary>
        /// Installs an executable containing classes that extend ServiceBase. This
        /// class is called by installation utilities, such as InstallUtil.exe, when
        /// installing a service application.
        /// </summary>
        private System.ServiceProcess.ServiceProcessInstaller serviceProcessInstaller;

        /// <summary>
        /// Installs a class that extends ServiceBase to implement a service. This
        /// class is called by the install utility when installing a service application.
        /// </summary>
        private System.ServiceProcess.ServiceInstaller serviceInstaller;
    }
}