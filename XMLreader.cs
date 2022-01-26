using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;

namespace CustomActorToolkit
{
    public static class XMLreader
    {
        //   XmlTextReader reader = new XmlTextReader ("ActorNames.xml")

        public static SongItem[] getXMLItems(String XMLFile, String XMLTag)
        {
            XmlDocument doc = new XmlDocument();
            var fileName = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), @"XML/" + XMLFile + ".xml");
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            doc.Load(fs);
            XmlNodeList nodes = doc.SelectNodes("Table/" + XMLTag);
            int incr = 0;
            SongItem[] output = new SongItem[nodes.Count];
            if (nodes != null)
                foreach (XmlNode node in nodes)
                {
                    XmlAttributeCollection nodeAtt = node.Attributes;
                    output[incr] = new SongItem();
                    output[incr].Text = nodeAtt["Key"].Value + " - " + node.InnerText;
                    output[incr].Value = Convert.ToInt64(nodeAtt["Key"].Value, 16);
                    incr++;
                }
            return output;
        }

        public static XmlNodeList getXMLNodes(String XMLFile, String XMLTag)
        {
            XmlDocument doc = new XmlDocument();
            var fileName = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), @"XML/" + XMLFile + ".xml");
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            doc.Load(fs);
            XmlNodeList nodes = doc.SelectNodes("Table/" + XMLTag);
            return nodes;
        }


    }
}
