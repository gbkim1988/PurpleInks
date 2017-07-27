using BlueInks.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BlueInks.Models
{
    public class DiagnosisDirectory
    {
        private static String DsisEntryDirectory = Directory.GetCurrentDirectory();

        public static String GetEntryDirectory(String FolderName) {

            return Path.Combine(Directory.GetCurrentDirectory(), FolderName);
        }

        public static String GetDiagnosisDirectory()
        {
            // 유효한 아이피를 찾아낸다.
            // 해당 작업은 비동기로 진행해야 하는가? 그렇지 않으면 사용자의 양해를 구하고 아이피를 구하는 작업을 진행해야 하는가?
            String IPAddress = String.Join("_", NetworkUtils.GetValidIPAddress().Split('.'));
            String timestamp = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            String Hostname = Dns.GetHostName();
            return String.Join("_", new String[] { IPAddress, Hostname, timestamp });
        }

        public static String GetBinaryDirectory()
        {
            return Path.Combine(Directory.GetCurrentDirectory(), "bin");
        }
    }
}
