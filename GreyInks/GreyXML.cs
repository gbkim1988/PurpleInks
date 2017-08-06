using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GreyInks
{
    public class GreyXML
    {
        public static List<Dictionary<string, string>> GetXmlOutput(string filename)
        {
            List<Dictionary<string, string>> report = new List<Dictionary<string, string>>();
            
            XElement root = XElement.Load(filename);
            foreach (var item in root.Elements("item"))
            {
                Dictionary<string, string> table = new Dictionary<string, string>();
                table["time"] = (string)item.Element("action_time");
                table["dscr"] = (string)item.Element("description");
                table["file"] = (string)item.Element("filename");
                table["path"] = (string)item.Element("full_path");
                table["info"] = (string)item.Element("more_information");
                table["ext"] = (string)item.Element("file_extension");
                report.Add(table);
            }

            return report;
        }

        public static List<Dictionary<string, string>> GetChormeCacheXmlOutput(string filename)
        {
            List<Dictionary<string, string>> report = new List<Dictionary<string, string>>();

            XElement root = XElement.Load(filename);
            foreach (var item in root.Elements("item"))
            {
                Dictionary<string, string> table = new Dictionary<string, string>();
                table["filename"] = (string)item.Element("filename");
                table["last_accessed"] = (string)item.Element("last_accessed");
                table["url"] = (string)item.Element("url");
                report.Add(table);
            }

            return report;
        }
    }
}
