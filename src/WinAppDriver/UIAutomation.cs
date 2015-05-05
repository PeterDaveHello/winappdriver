namespace WinAppDriver
{
    using System.Collections.Generic;
    using System.IO;
    using System.Windows.Automation;
    using System.Xml;
    using System.Xml.XPath;

    internal class UIAutomation : IUIAutomation
    {
        public bool TryGetFocusedWindowOrRoot(out AutomationElement window)
        {
            window = AutomationElement.RootElement;

            var walker = TreeWalker.ContentViewWalker;
            var parent = AutomationElement.FocusedElement;
            while (parent != null)
            {
                if (parent == AutomationElement.RootElement)
                {
                    return false;
                }
                else if (parent.Current.ControlType == ControlType.Window)
                {
                    window = parent;
                    return true;
                }
                else
                {
                    parent = walker.GetParent(parent);
                }
            }

            return false;
        }

        public string DumpXml(AutomationElement start)
        {
            return this.DumpXmlImpl(start, null);
        }

        public string DumpXml(AutomationElement start, out IList<AutomationElement> elements)
        {
            elements = new List<AutomationElement>();
            return this.DumpXmlImpl(start, elements);
        }

        public AutomationElement FindFirstByXPath(AutomationElement start, string xpath)
        {
            IList<AutomationElement> nodes;
            string xml = this.DumpXml(start, out nodes);

            var doc = new XPathDocument(new StringReader(xml));
            XPathNavigator node = doc.CreateNavigator().SelectSingleNode(xpath);
            if (node == null)
            {
                return null;
            }
            else
            {
                var index = int.Parse(node.GetAttribute("_index_", string.Empty));
                return nodes[index];
            }
        }

        public IList<AutomationElement> FindAllByXPath(AutomationElement start, string xpath)
        {
            IList<AutomationElement> elements;
            string xml = this.DumpXml(start, out elements);

            var doc = new XPathDocument(new StringReader(xml));
            XPathNodeIterator nodes = doc.CreateNavigator().Select(xpath);

            var results = new List<AutomationElement>();
            foreach (XPathNavigator node in nodes)
            {
                var index = int.Parse(node.GetAttribute("_index_", string.Empty));
                results.Add(elements[index]);
            }

            return results;
        }

        private string DumpXmlImpl(AutomationElement start, IList<AutomationElement> elements)
        {
            var control = new PropertyCondition(AutomationElement.IsControlElementProperty, true);
            var visible = new PropertyCondition(AutomationElement.IsOffscreenProperty, false);
            var walker = new TreeWalker(new AndCondition(control, visible));

            var stringWriter = new StringWriter();
            XmlWriter writer = new XmlTextWriter(stringWriter);

            writer.WriteStartDocument();
            writer.WriteStartElement("WinAppDriver");
            this.WalkTree(start, walker, writer, elements);
            writer.WriteEndDocument();

            return stringWriter.ToString();
        }

        private void WalkTree(AutomationElement parent, TreeWalker walker, XmlWriter writer, IList<AutomationElement> elements)
        {
            var info = parent.Current;
            writer.WriteStartElement(info.ControlType.ProgrammaticName);
            if (elements != null)
            {
                writer.WriteAttributeString("_index_", elements.Count.ToString());
                elements.Add(parent);
            }

            writer.WriteAttributeString("id", info.AutomationId);
            writer.WriteAttributeString("name", info.Name);
            writer.WriteAttributeString("class", info.ClassName);

            var child = walker.GetFirstChild(parent);
            while (child != null)
            {
                this.WalkTree(child, walker, writer, elements);
                child = walker.GetNextSibling(child);
            }

            writer.WriteEndElement();
        }
    }
}