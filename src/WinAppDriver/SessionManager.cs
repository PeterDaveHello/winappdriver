namespace WinAppDriver
{
    using System;
    using System.Collections.Generic;

    internal class SessionManager
    {
        private Dictionary<string, Session> sessions;

        public SessionManager()
        {
            this.sessions = new Dictionary<string, Session>();
        }

        public Session this[string id]
        {
            get { return this.sessions[id]; }
        }

        public Session CreateSession(IApplication application, Capabilities capabilities)
        {
            var id = Guid.NewGuid().ToString();
            var session = new Session(id, application, capabilities);
            this.sessions[id] = session;
            return session;
        }

        public void DeleteSession(string id)
        {
            this.sessions.Remove(id);
        }
    }
}