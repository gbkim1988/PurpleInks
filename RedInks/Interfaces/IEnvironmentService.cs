using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedInks.Interfaces
{
    public interface IEnvironmentService
    {
        IEnumerable<String> GetCommandLineArguments();
    }
}
