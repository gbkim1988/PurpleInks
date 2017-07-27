using RedInks.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedInks
{
    public class RedInksEnvironmentService : IEnvironmentService
    {
        public IEnumerable<string> GetCommandLineArguments()
        {
            return Environment.GetCommandLineArgs();
        }
    }
}
