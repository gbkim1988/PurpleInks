using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using WUApiLib;
using System.Management;
using static GreyInks.MainWindow;
using System.Reflection;
using System.Windows;

namespace GreyInks
{
    public class GreyDoIt
    {
        public static GreyDoIt Instance;

        public static void Default()
        {
            GreyDoIt.Instance = new GreyDoIt();
        }

        public static void Do(ObservableCollection<CheckItem> Collections)
        {
            foreach(var item in Collections)
            {
                DoFactory(item);
            }
        }

        public static void DoFactory(CheckItem item)
        {
            switch (item.code)
            {
                case "MA-001":
                    Diagnosis_MA_001(item);
                    break;
                case "MA-002":
                    //Diagnosis_MA_002(item);
                    break;
                case "MA-003":
                    //Diagnosis_MA_003(item);
                    break;
                case "MA-004":
                    Diagnosis_MA_004(item);
                    break;
                case "MA-005":
                    Diagnosis_MA_005(item);
                    break;
                case "MA-006":
                    Diagnosis_MA_006(item);
                    break;
                case "MA-007":
                    Diagnosis_MA_007(item);
                    break;
                case "MA-008":
                    Diagnosis_MA_008(item);
                    break;
                case "MA-009":
                    Diagnosis_MA_009(item);
                    break;
            }
        }

        /// <summary>
        /// 인터넷 익스플로러 취약성 점검
        /// 레지스트리를 통해 인터넷 익스플로러 취약성을 검토
        /// </summary>
        /// <param name="item"></param>
        public static void Diagnosis_MA_001(CheckItem item)
        {
            string value = GreyWnReg.GetRegistryValue("SOFTWARE\\Microsoft\\Internet Explorer", "svcUpdateVersion", GreyWnReg.Hive.LocalMachine);
            int MajorIeVersion;
            if ( int.TryParse(value.Split('.').First(), out MajorIeVersion))
            {
                if(MajorIeVersion >= 11)
                {
                    item.Proofs["IeVersion"] = value;
                    item.Status = Result.Fulfilled;
                }
                else
                {
                    item.Status = Result.Negative;
                }
            }
            else
            {
                item.Status = Result.Negative;
            }

            
            // 진단 시작
        }

        /// <summary>
        /// 말버타이징 위험 노출 점검
        /// </summary>
        /// <param name="item"></param>
        public static void Diagnosis_MA_002(CheckItem item)
        {
            // Code 를 통해서 우선 검토
            
            string[] arg = GreyCommand.GetCommandLine("MA-002", "util1");
            GreyUtils.Instance.ExtractExecutable(arg[0]);
            GreyCommand.ExecutedCallback(Directory.GetCurrentDirectory(), arg[0], arg[1]);
            string output = GreyCommand.GetOutputFile("MA-002", "util1");
            var reports = GreyXML.GetChormeCacheXmlOutput(output);

            string[] arg2 = GreyCommand.GetCommandLine("MA-002", "util2");
            GreyUtils.Instance.ExtractExecutable(arg2[0]);
            GreyCommand.ExecutedCallback(Directory.GetCurrentDirectory(), arg2[0], arg2[1]);
            string output2 = GreyCommand.GetOutputFile("MA-002", "util2");
            var reports2 = GreyXML.GetChormeCacheXmlOutput(output2);
            reports.Concat(reports2);
            int count = 0;
            int progress = 0;
            int total = reports.Count;
            List<string> DuplessHost = new List<string>();
            foreach(var elem in reports)
            {
                string host;
                try {
                    host = (new Uri(elem["url"])).Host;
                    progress += 1;
                    item.Progress = "(" + ((int)((float)progress/(float)total * 100)).ToString() + " %) ";
                } catch(Exception) {
                    continue;
                } finally
                {

                }

                if (DuplessHost.Contains(host))
                    continue;
                else
                    DuplessHost.Add(host);
                try
                {
                    IPHostEntry ip = Dns.GetHostEntry(host);
                    try
                    {
                        item.Proofs[host] = "유효한 도메인 입니다.";
                    }
                    catch (Exception) { }
                    
                }
                catch (Exception)
                {
                    try
                    {
                        item.Proofs[host] = "유효한 도메인이 아닙니다.";
                        count++;
                    }
                    catch (Exception) { }
                }
            }
            item.Progress = "";
            if (count > 0)
            {
                item.Status = Result.Negative;
            }
            else
            {
                item.Status = Result.Fulfilled;
            }
        }
        /// <summary>
        /// 유해 사이트 노출 점검
        /// </summary>
        /// <param name="item"></param>
        public static void Diagnosis_MA_003(CheckItem item)
        {
            string output = GreyCommand.GetOutputFile("MA-002", "util1");
            var reports = GreyXML.GetChormeCacheXmlOutput(output);

            string output2 = GreyCommand.GetOutputFile("MA-002", "util2");
            var reports2 = GreyXML.GetChormeCacheXmlOutput(output2);

            reports.Concat(reports2);
            List<string> blacklist = new List<string>();
            int count = 0;
            foreach(var elem in reports)
            {
                string host;
                try
                {
                    host = (new Uri(elem["url"])).Host;
                    if (blacklist.Contains(host))
                    {
                        item.Proofs.Add(host, "블랙리스트 도메인 접근 확인");
                        count += 1;
                    }
                }
                catch (Exception)
                {
                    continue;
                }
            }
            if (count > 0)
            {
                item.Status = Result.Negative;
            }
            else
            {
                item.Status = Result.Fulfilled;
            }
            // 진단 시작
        }
        /// <summary>
        /// 윈도우 업데이트 현황 점검
        /// </summary>
        /// <param name="item"></param>
        public static void Diagnosis_MA_004(CheckItem item)
        {
            // 진단 시작
            /*
             * 1) wuapi.dll 을 리소스에 추가 
             * 2) 동적으로 dll 을 로드 
             * 3) API 호출
             * 4) 업데이트 설치
             * 5) 종료
             * 참고 : http://techforum4u.com/entry.php/11-Install-Windows-Update-Using-C
             */
            
            // Reflection 을 이용해서 dll 을 로드할 시에 x86 또는 x64 인지를 잘 확인        
            // 전략적으로는 32 비트가 유용하다. 일단 해보자.
            UpdateSessionClass uSession = new UpdateSessionClass();
            IUpdateSearcher uSearcher = uSession.CreateUpdateSearcher();
            ISearchResult uResult = uSearcher.Search("IsInstalled=0 and Type='Software'");
            foreach (IUpdate update in uResult.Updates)
            {
                Console.WriteLine(update.Title);
            }

            UpdateDownloader downloader = uSession.CreateUpdateDownloader();
            downloader.Updates = uResult.Updates;
            downloader.Download();

            UpdateCollection updatesToInstall = new UpdateCollection();
            foreach (IUpdate update in uResult.Updates)
            {
                if (update.IsDownloaded)
                    updatesToInstall.Add(update);
            }

            IUpdateInstaller installer = uSession.CreateUpdateInstaller();
            installer.Updates = updatesToInstall;

            IInstallationResult installationRes = installer.Install();

            for (int i = 0; i < updatesToInstall.Count; i++)
            {
                if (installationRes.GetUpdateResult(i).HResult == 0)
                {
                    Console.WriteLine("Installed : " + updatesToInstall[i].Title);
                }
                else
                {
                    Console.WriteLine("Failed : " + updatesToInstall[i].Title);
                }
            }
        }
        public static void Diagnosis_MA_005(CheckItem item)
        {
            // 진단 시작
        }
        public static void Diagnosis_MA_006(CheckItem item)
        {
            // 진단 시작
        }
        public static void Diagnosis_MA_007(CheckItem item)
        {
            // 진단 시작
        }
        public static void Diagnosis_MA_008(CheckItem item)
        {
            // 진단 시작
        }
        public static void Diagnosis_MA_009(CheckItem item)
        {
            // 진단 시작
        }

    }
}
