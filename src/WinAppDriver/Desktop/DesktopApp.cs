﻿namespace WinAppDriver.Desktop
{
    using System;
    using WinAppDriver.UAC;

    internal class DesktopApp : IDesktopApp
    {
        private static ILogger logger = Logger.GetLogger("WinAppDriver");

        private IDriverContext context;

        private Capabilities capabilities;

        private IUACPomptHandler uacHandler;

        private IUtils utils;

        private IPackageInstaller installerCache;

        public DesktopApp(IDriverContext context, Capabilities capabilities, IUACPomptHandler uacHandler, IUtils utils)
        {
            this.context = context;
            this.capabilities = capabilities; // TODO validate capabilities
            this.uacHandler = uacHandler;
            this.utils = utils;
        }

        public string DriverAppID
        {
            get { return "MyDesktopApp"; } // TODO deduce the app ID
        }

        public Capabilities Capabilities
        {
            get { return this.capabilities; }
        }

        public IPackageInstaller Installer
        {
            get
            {
                if (this.installerCache == null)
                {
                    this.installerCache = new DesktopAppInstaller(this.context, this, this.uacHandler, this.utils);
                }

                return this.installerCache;
            }
        }

        public bool IsInstalled()
        {
            return false; // TODO with the help of external commands (exit status?)
        }

        public void Launch()
        {
            // TODO backup or restore initial states if needed.
            this.RestoreInitialStates();
            this.Activate();
        }

        public void Activate()
        {
            this.utils.Execute(this.capabilities.OpenCommand, "Open", false);
        }

        public void Terminate()
        {
            this.utils.Execute(this.capabilities.CloseCommand, "Close", true);
        }

        public void BackupInitialStates()
        {
            throw new NotImplementedException();
        }

        public void RestoreInitialStates()
        {
            this.utils.Execute(this.capabilities.ResetCommand, "Reset", true);
        }

        public void Uninstall()
        {
            this.utils.Execute(this.capabilities.UninstallCommand, "Uninstall", true);
        }
    }
}