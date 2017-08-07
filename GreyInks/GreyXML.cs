using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
            /// XML 파일 내의 깨진 인코딩이 존재할 경우 에러를 발생
            /// 보정 작업이 필요함

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

        public static List<Dictionary<string, string>> GetIeCacheOutput(string filename)
        {
            List<Dictionary<string, string>> report = new List<Dictionary<string, string>>();

            using (StreamReader sr = new StreamReader(filename))
            {
                while (sr.Peek() >= 0)
                {
                    //Reads the line, splits on tab and adds the components to the table
                    try {
                        Dictionary<string, string> table = new Dictionary<string, string>();
                        string[] record = sr.ReadLine().Split((char)9);
                        table["filename"] = record.ElementAtOrDefault(0);
                        table["url"] = record.ElementAtOrDefault(2);
                        table["last_accessed"] = record.ElementAtOrDefault(3);
                        report.Add(table);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                        continue;
                    }
                    
                }
            }

            return report;
        }
    }
}
