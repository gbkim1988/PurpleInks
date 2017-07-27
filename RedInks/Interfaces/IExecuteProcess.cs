using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedInks.Interfaces
{
    public interface IExecuteProcess
    {
        Task<bool> Execute(String path, String rootDir, IEnumerable<String> args);
    }
}
