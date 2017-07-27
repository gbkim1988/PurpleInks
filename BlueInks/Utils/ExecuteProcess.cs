using RedInks.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static RedInks.Models.Diagnosis;

namespace RedInks.Utils
{
    public class ExecuteProcess : IExecuteProcess
    {
        public static void ExecuteCallback(String basement, String executable, IEnumerable<String> arguments, Action<Status, String> NotifyComplete)
        {
            // 비동기 내에서 실행하므로, 굳이 비동기를 사용할 필요가 없음
            // lastactiveview.exe 와 같은 실행 프로세스는 실행이 종료될 때까지 기다린 다음에 종료된 후에 xml, 등의 파일을 파싱하는 단계로 진행
            String ExePath = Path.Combine(basement, executable);
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
                            RedirectStandardOutput = false,
                            RedirectStandardError = false
                        },
                        EnableRaisingEvents = true,
                    };

                    process.Exited += (sender, args) =>
                    {
                        try
                        {
                            NotifyComplete(Status.Completed, String.Format("프로세스 실행 {0} 정상 종료", ExePath));
                            process.Dispose();
                        }
                        catch(Exception e) {
                            NotifyComplete(Status.Completed, String.Format("프로세스 실행 {0} 오류 : {1}", ExePath, e.Message));
                            process.Dispose();
                        }
                        
                    };
                    // 프로세스 시작
                    process.Start();
                    process.WaitForExit();
                }
                catch (Exception e)
                {
                    NotifyComplete(Status.Error, String.Format("프로세스 실행 {0} 오류 - 에러메시지 : {1}", executable, e.Message));
                }

                

            }
            else {
                NotifyComplete(Status.Error, String.Format("프로세스 실행 {0} 오류 - bin 폴더에 실행파일이 존재하지 않음", executable));
            }

        }

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
