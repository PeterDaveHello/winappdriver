﻿namespace WinAppDriver.UI
{
    using System;
    using System.Windows;
    using System.Windows.Automation;

    internal interface IElement
    {
        string UIFramework { get; }

        string ID { get; }

        IntPtr Handle { get; }

        string TypeName { get; }

        string Name { get; }

        string ClassName { get; }

        string Help { get; }

        bool Focusable { get; }

        bool Focused { get; }

        bool Visible { get; }

        bool Enabled { get; }

        bool Selected { get; }

        bool Protected { get; }

        int X { get; }

        int Y { get; }

        int Width { get; }

        int Height { get; }
    }
}