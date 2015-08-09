﻿namespace WinAppDriver.Desktop
{
    using System.Collections.Generic;
    using System.Diagnostics;

    internal interface IDesktopApp : IApplication
    {
        Process TriggerCustomAction(string command, IDictionary<string, string> envs);

        Process TriggerCustomAction(string command, IDictionary<string, string> envs, out int waitExitCode);
    }
}