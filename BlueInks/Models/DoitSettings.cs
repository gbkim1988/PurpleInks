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
        // 실행 프로그램 이름
        public String BinaryName;
        // 기본 디렉터리 경로
        public String Basement;
        public String FullExePath;
        // 
        public DoitSettings(String ExeFile = null)
        {
            this.Basement = Path.Combine(Directory.GetCurrentDirectory(), "bin");
            this.BinaryName = ExeFile;
            this.FullExePath = Path.Combine(this.Basement, this.BinaryName);
        }
    }
}
