﻿namespace WinAppDriver
{
    internal interface IApplication
    {
        bool IsInstalled();

        void Activate();

        void Terminate();

        void BackupInitialStates();

        void RestoreInitialStates();
    }
}