﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BlueInksTest
{
    public class GreyCommand
    {
        /// <summary>
        /// XML 파일의 정보를 읽어와 명령문을 생성
        /// </summary>
        private static string XMLFile = "CheckList.xml";

        public static string [] GetCommandLine(string type)
        {
            XElement root = XElement.Load(XMLFile);

            XElement UtilNode = (from el in root.Descendants("util")
                                             where (string)el.Attribute("type") == type
                                             select el).First();
            string BinaryName = (string)UtilNode.Attribute("name");
            string param = string.Join(" ", (from el in UtilNode.Elements("param") select (string)el.Attribute("value")));

            return new string [] { BinaryName, param};
        }

        public static void ExecutedCallback(string basement, string binary, string argument)
        {
            String ExePath = Path.Combine(basement, binary);
            if (File.Exists(ExePath))
            {
                try
                {
                    var process = new Process
                    {
                        StartInfo = {
                            FileName = ExePath,
                            Arguments = String.Join(" ", argument),
                            UseShellExecute = false,
                            RedirectStandardOutput = false,
                            RedirectStandardError = false
                        },
                        EnableRaisingEvents = true,
                    };

                    // 프로세스 시작
                    process.Start();
                    process.WaitForExit();
                }
                catch (Exception e)
                {

                }
            }
        }

        public static GreyCommand Instance { get; private set; }
        public static void Default()
        {
            GreyCommand.Instance = new GreyCommand();
        }
    }
}
