namespace WinAppDriver
{
    using System;
    using SystemWrapper.Windows.Input;
    using WinAppDriver.Handlers;
    using WinAppDriver.UAC;
    using WinAppDriver.WinUserWrapper;

    internal class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            var sessionMgr = new SessionManager();
            var requestMgr = InitRequestManager(sessionMgr);

            var driver = new Server(requestMgr);
            driver.Start();
        }

        private static RequestManager InitRequestManager(SessionManager sessionManager)
        {
            var context = new DriverContext();
            var manager = new RequestManager(sessionManager);
            var utils = new Utils();
            var winUserWrap = new WinUserWrap();
            var keyboard = new Keyboard(new KeyboardWrap(), new KeyInteropWrap(), winUserWrap);
            var uiAutomation = new UIAutomation();
            var uacHandler = new UACPromptHandler(uiAutomation, keyboard);

            manager.AddHandler(new ClickElementHandler());
            manager.AddHandler(new DeleteSessionHandler(sessionManager));
            manager.AddHandler(new FindElementHandler(uiAutomation));
            manager.AddHandler(new FindElementsHandler(uiAutomation));
            manager.AddHandler(new GetElementTextHandler());
            manager.AddHandler(new GetCurrentWindowHandler(uiAutomation));
            manager.AddHandler(new GetSourceHandler(uiAutomation));
            manager.AddHandler(new GetWindowsHandler(uiAutomation));
            manager.AddHandler(new NewSessionHandler(context, sessionManager, uacHandler, utils));
            manager.AddHandler(new ScreenshotHandler());
            manager.AddHandler(new SendKeysHandler(keyboard));
            manager.AddHandler(new SetElementValueHandler());
            manager.AddHandler(new SwitchToWindowHandler(uiAutomation, winUserWrap, keyboard));

            return manager;
        }
    }
}