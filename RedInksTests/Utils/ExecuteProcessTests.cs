using Microsoft.VisualStudio.TestTools.UnitTesting;
using RedInks.Interfaces;
using RedInks.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedInks.Utils.Tests
{
    [TestClass()]
    public class ExecuteProcessTests
    {
        [TestMethod()]
        public void ExecuteTest()
        {
            Task<bool> result = new ExecuteProcess().Execute(
                "lastactivityview.exe",
                "D:\\powershell-project\\powershell-repo\\MALSCAN\\module\\bin\\Activity",
                new System.Collections.Generic.List<String> { "/sxml", "C:\\gura.xml" });

        }
    }
}