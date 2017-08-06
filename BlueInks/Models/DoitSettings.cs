using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueInks.Models
{
    public class DoitSettings
    {
        // 기본 디렉터리 경로
        public String Basement;
        // 
        public String BinaryBase;
        public String LogBase;

        public DoitSettings()
        {
            this.Basement = Directory.GetCurrentDirectory();
            this.BinaryBase = Path.Combine(this.Basement, "bin");
            this.LogBase = Path.Combine(this.Basement, "log");

        }

        public String GetExeFilePath(String BinaryFileName)
        {
            return Path.Combine(this.Basement, BinaryFileName);
        }

    }
}
