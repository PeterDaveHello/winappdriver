﻿namespace WinAppDriver
{
    internal interface IApplication
    {
        string DriverAppID { get; }

        Capabilities Capabilities { get; }

        IPackageInstaller Installer { get; }

        bool IsInstalled();

        void Launch();

        void Activate();

        void Terminate();

        void BackupInitialStates();

        void RestoreInitialStates();

        void Uninstall();
    }
}