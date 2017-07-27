using RedInks.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RedInks.Utils
{
    public class ExecuteProcess : IExecuteProcess
    {
        public async Task<bool> Execute(String BinaryName, String SubDir, IEnumerable<String> arguments) {
            var tcs = new TaskCompletionSource<bool>();
            String ExePath = Path.Combine(SubDir, BinaryName);
            if (File.Exists(ExePath))
            {
                try
                {
                    var process = new Process
                    {
                        StartInfo = {
                            FileName = ExePath,
                            Arguments = String.Join(" ", arguments),
                            UseShellExecute = false,
                            RedirectStandardOutput = true,
                            RedirectStandardError = true
                        },
                        EnableRaisingEvents = true,
                    };

                    //process.OutputDataReceived += new DataReceivedEventHandler(OutputHandler);
                    //process.ErrorDataReceived += new DataReceivedEventHandler(OutputHandler);

                    process.Exited += (sender, args) =>
                    {
                        MessageBox.Show("End Of Process");
                        /*
                         비동기 Synchronization 코드 추가 
                         */
                        tcs.SetResult(true);
                        process.Dispose();
                    };

                    process.Start();
                }
                catch (Exception e) {
                    MessageBox.Show(e.Message);
                }


            } else {
                tcs.SetResult(false);
            }

            return await tcs.Task;
        }
        static void OutputHandler(object sendingProcess, DataReceivedEventArgs outLine)
        {
            //* Do your stuff with the output (write to console/log/StringBuilder)
            Console.WriteLine(outLine.Data);
        }

    }

}
