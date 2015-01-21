namespace WinAppDriver
{
    using System;
    using System.Collections.Generic;

    internal class RequestManager
    {
        private static ILogger logger = Logger.GetLogger("WinAppDriver");

        private SessionManager sessionManager;

        private Dictionary<string, List<EndPoint>> endpoints;

        public RequestManager(SessionManager sessionManager)
        {
            this.sessionManager = sessionManager;
            this.endpoints = new Dictionary<string, List<EndPoint>>
            {
                { "GET", new List<EndPoint>() },
                { "POST", new List<EndPoint>() },
                { "DELETE", new List<EndPoint>() },
            };
        }

        public void AddHandler(IHandler handler)
        {
            // TODO register all handlers automatically, with the help of reflection
            foreach (var route in this.GetRoutes(handler))
            {
                this.endpoints[route.Method].Add(new EndPoint(route.Method, route.Pattern, handler));
            }
        }

        public object Handle(string method, string path, string body, out Session session)
        {
            session = null;
            Dictionary<string, string> urlParams = null;

            foreach (var endpoint in this.endpoints[method])
            {
                if (endpoint.IsMatch(method, path, out urlParams))
                {
                    var handler = endpoint.Handler;
                    logger.Debug("A corresponding endpoint found: {0}", handler.GetType().FullName);

                    if (urlParams.ContainsKey("sessionId"))
                    {
                        session = this.sessionManager[urlParams["sessionId"]]; // TODO invalid session?
                    }

                    return endpoint.Handler.Handle(urlParams, body, ref session);
                }
            }

            return null; // TODO throw exception (command not supported)
        }

        private List<RouteAttribute> GetRoutes(object handler)
        {
            var routes = new List<RouteAttribute>();

            Attribute[] attributes = Attribute.GetCustomAttributes(handler.GetType());
            foreach (var attr in attributes)
            {
                if (attr is RouteAttribute)
                {
                    routes.Add((RouteAttribute)attr);
                }
            }

            return routes; // TODO throw exception (programming error)
        }
    }
}