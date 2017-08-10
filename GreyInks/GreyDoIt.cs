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
using System.Diagnostics;
using NATUPNPLib;
using NETCONLib;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using ShootBlues;
using System.Text;
using System.ServiceProcess;

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
                    Diagnosis_MA_002(item);
                    break;
                case "MA-003":
                    Diagnosis_MA_003(item);
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
            var reports2 = GreyXML.GetIeCacheOutput(output2);
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
            var reports2 = GreyXML.GetIeCacheOutput(output2);

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
            int count = 0;
            // Reflection 을 이용해서 dll 을 로드할 시에 x86 또는 x64 인지를 잘 확인        
            // 전략적으로는 32 비트가 유용하다. 일단 해보자.
            /*
            UpdateSessionClass uSession = new UpdateSessionClass();
            IUpdateSearcher uSearcher = uSession.CreateUpdateSearcher();
            ISearchResult uResult = uSearcher.Search("IsInstalled=0 and Type='Software'");
            
            foreach (IUpdate update in uResult.Updates)
            {
                count += 1;
                item.Proofs.Add(update.Title, update.Description);
            }
            */
            if (count > 0)
            {
                item.Status = Result.Negative;
            }else
            {
                item.Status = Result.Fulfilled;
            }

            // 실행 파일 획득
            GreyUtils.Instance.ExtractExecutable("WinUpdates.vbe");

            Process scriptProc = new Process();
            scriptProc.StartInfo.FileName = @"cscript";
            scriptProc.StartInfo.WorkingDirectory = Directory.GetCurrentDirectory();
            scriptProc.StartInfo.Arguments = "//Nologo WinUpdates.vbe";
            scriptProc.StartInfo.WindowStyle = ProcessWindowStyle.Normal; //prevent console window from popping up
            scriptProc.StartInfo.Verb = "runas"; // 
            scriptProc.Start();
            scriptProc.Close();

            
            // 직접 업데이트를 수행하면 오래걸리기 때문에 
            // wuauclt.exe /updatenow 명령을 실행시키고 다음단계로 넘어간다.
            /*
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
            // 상당히 시간이 많이 걸리는 작업
            installer.RunWizard("윈도우 업데이트");
            
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
            }*/
        }
        /// <summary>
        /// 보안프로그램 설치 현황을 확인
        /// </summary>
        /// <param name="item"></param>
        public static void Diagnosis_MA_005(CheckItem item)
        {
            int Count = 0;
            try
            {
                string MainKey = @"SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\Uninstall";
                List<string> InstalledSoftware = new List<string>();
                // 진단 시작
                string[] subkeys = GreyWnReg.GetSubKeyNames(MainKey, GreyWnReg.Hive.LocalMachine);
                
                foreach (var key in subkeys)
                {
                    string value = GreyWnReg.GetRegistryValue(string.Join("\\", MainKey, key), "DisplayName", GreyWnReg.Hive.LocalMachine);
                    if (value != null)
                        InstalledSoftware.Add(value);
                }

                MainKey = @"SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall";
                string[] subkeys2 = GreyWnReg.GetSubKeyNames(MainKey, GreyWnReg.Hive.LocalMachine);

                foreach (var key in subkeys2)
                {
                    string value = GreyWnReg.GetRegistryValue(string.Join("\\", MainKey, key), "DisplayName", GreyWnReg.Hive.LocalMachine);
                    if (value != null)
                        InstalledSoftware.Add(value);
                }

                // APC 체크 
                IEnumerable<string> results = InstalledSoftware.Where(x => x != null && x.ToLower().Contains("ahn") && x.ToLower().Contains("policy") && x.ToLower().Contains("agent"));
                if (results.Count() > 0)
                {
                    foreach (var elem in results)
                    {
                        if (elem != null)
                        {
                            try
                            {
                                item.Proofs.Add(elem, "설치");
                            }
                            catch (Exception)
                            {

                            }

                        }

                    }

                }
                else
                {
                    try
                    {
                        item.Proofs.Add("APC Agent", "미설치");
                        Count += 1;
                    }
                    catch (Exception)
                    {

                    }

                }

                // 매체제어 체크 체크 
                Process[] localAll = Process.GetProcesses();
                // 서비스 목록 출력
                ServiceController[] scServices = ServiceController.GetServices();

                IEnumerable<Process> sdpa = localAll.Where(x => x.ProcessName.ToLower().Contains("SDPA".ToLower()));

                if (sdpa.Count() > 0)
                {
                    foreach (var elem in sdpa)
                    {

                        if (elem != null)
                        {
                            try
                            {
                                item.Proofs.Add(elem.ProcessName, "설치");
                            }
                            catch (Exception) { }

                        }
                    }
                }
                else
                {

                    try
                    {
                        item.Proofs.Add("매체 제어(SDPA)", "미설치");
                        Count += 1;
                    }
                    catch (Exception) { }
                }

                IEnumerable<Process> n5client = localAll.Where(x => x != null & x.ProcessName.ToLower().Contains("n5client".ToLower()));

                if (n5client.Count() > 0)
                {
                    foreach (var elem in n5client)
                    {

                        if (elem != null)
                        {
                            try
                            {
                                item.Proofs.Add(elem.ProcessName, "설치");
                            }
                            catch (Exception) { }

                        }
                    }
                }
                else
                {

                    try
                    {
                        item.Proofs.Add("NetClient", "미설치");
                        Count += 1;
                    }
                    catch (Exception) { }
                }

                IEnumerable<Process> edpa = localAll.Where(x => x != null & x.ProcessName.ToLower().Contains("edpa".ToLower()));
                //scServices.Where(x => x != null & x.DisplayName )
                if (edpa.Count() > 0)
                {
                    foreach (var elem in edpa)
                    {

                        if (elem != null)
                        {
                            try
                            {
                                item.Proofs.Add(elem.ProcessName, "설치");
                            }
                            catch (Exception) { }

                        }
                    }
                }
                else
                {

                    try
                    {
                        item.Proofs.Add("데이터 유출 방지프로그램(DLP)", "미설치");
                        Count += 1;
                    }
                    catch (Exception) { }
                }

                IEnumerable<string> backoffice = InstalledSoftware.Where(x => x != null && x.ToLower().Contains("BACKOFFICE"));
                if (backoffice.Count() > 0)
                {
                    foreach (var elem in backoffice)
                    {
                        if (elem != null)
                        {
                            try
                            {
                                item.Proofs.Add(elem, "설치");
                                Count += 1;
                            }
                            catch (Exception)
                            {

                            }

                        }

                    }

                }
                else
                {
                    try
                    {
                        item.Proofs.Add("BackOffice", "미설치");
                    }
                    catch (Exception)
                    {

                    }

                }


                IEnumerable<string> MiPlatform = InstalledSoftware.Where(x => x != null && x.ToLower().Contains("MiPlatform"));
                if (MiPlatform.Count() > 0)
                {
                    foreach (var elem in MiPlatform)
                    {
                        if (elem != null)
                        {
                            try
                            {
                                item.Proofs.Add(elem, "설치");
                                Count += 1;
                            }
                            catch (Exception)
                            {

                            }

                        }

                    }

                }
                else
                {
                    try
                    {
                        item.Proofs.Add("송장 출력 프로그램", "미설치");
                    }
                    catch (Exception)
                    {

                    }

                }
            }catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
            

            if(Count > 0)
            {
                item.Status = Result.Negative;
            }
            else
            {
                item.Status = Result.Fulfilled;
            }

        }
        /// <summary>
        /// 방화벽 예외 프로그램 등록 현황 확인
        /// </summary>
        /// <param name="item"></param>
        public static void Diagnosis_MA_006(CheckItem item)
        {
            
            string[] arg = GreyCommand.GetCommandLine("MA-006","util1");
            GreyUtils.Instance.ExtractExecutable(arg[0]);
            GreyCommand.ExecutedCallback(Directory.GetCurrentDirectory(), arg[0], arg[1]);
            string output = GreyCommand.GetOutputFile("MA-006", "util1");
            var reports = GreyXML.GetXmlOutput(output);

            string[] arg2 = GreyCommand.GetCommandLine("MA-006", "util2");
            GreyUtils.Instance.ExtractExecutable(arg2[0]);

            List<string> Dupless = new List<string>();
            int count = 0;
            string[] extension = { "BAT", "BIN", "CMD", "COM", "CPL", "EXE", "GADGET", "INF1", "INS", "INX", "ISU", "JOB", "JSE", "LNK", "MSC", "MSI", "MSP", "MST", "PAF", "PIF", "PS1", "REG", "RGS", "SCR", "SCT", "SHB", "SHS", "U3P", "VB", "VBE", "VBS", "VBSCRIPT", "WS", "WSF", "WSH" };
            foreach (var elem in reports)
            {
                string filePath = elem["path"];
                if (!Dupless.Contains(filePath))
                {
                    Dupless.Add(filePath);
                    if(extension.Where(x => filePath.ToLower().EndsWith(x.ToLower())).Count() > 0)
                    {
                        if (File.Exists(filePath))
                        {
                            if (IsSigned(filePath) == -2146762496)
                            {
                                count += 1;
                                string hash = "";
                                try
                                {
                                    using (var sha256 = SHA256.Create())
                                    {
                                        using (var stream = File.OpenRead(filePath))
                                        {
                                            byte[] hashValue = sha256.ComputeHash(stream);
                                            hash = BitConverter.ToString(hashValue).Replace("-", String.Empty);
                                        }
                                    }

                                }
                                catch (Exception) { }

                                try
                                {
                                    item.Proofs.Add(hash, filePath);
                                }
                                catch (Exception) { }

                            }

                        }
                    }
                }
            }

            if (count > 0) {
                item.Status = Result.Negative;
            } else
            {
                item.Status = Result.Fulfilled;
            }

            

        }
        public static void Diagnosis_MA_007(CheckItem item)
        {
            // 진단 시작
            // 우선 생략
            item.Status = Result.Fulfilled;
        }
        public static void Diagnosis_MA_008(CheckItem item)
        {
            // 진단 시작
            // HKEY_LOCAL_MACHINE\SOFTWARE\AhnLab\ASPack\9.0\Option\AVMON
            string value = GreyWnReg.GetRegistryValueEx("SOFTWARE\\AhnLab\\ASPack\\9.0\\Option\\AVMON", "sysmonuse", GreyWnReg.Hive.LocalMachine);
            item.Proofs.Add("sysmonuse", value);
            if ( value != "1")
            {
                item.Status = Result.Negative;                
            }
            value = GreyWnReg.GetRegistryValueEx("SOFTWARE\\AhnLab\\ASPack\\9.0\\ServiceStatus", "AvMon", GreyWnReg.Hive.LocalMachine);
            item.Proofs.Add("AvMon", value);
            if (value != "1")
            {
                item.Status = Result.Negative;
            }
            
            if(item.Status != Result.Negative)
            {
                item.Status = Result.Fulfilled;
            }
        }
        public static void Diagnosis_MA_009(CheckItem item)
        {
            // 진단 시작
        }
        public static int IsSigned(string filePath)
        {
            if (filePath == null)
                throw new ArgumentNullException(nameof(filePath));

            var file = new WINTRUST_FILE_INFO();
            file.cbStruct = Marshal.SizeOf(typeof(WINTRUST_FILE_INFO));
            file.pcwszFilePath = filePath;

            var data = new WINTRUST_DATA();
            data.cbStruct = Marshal.SizeOf(typeof(WINTRUST_DATA));
            data.dwUIChoice = WTD_UI_NONE;
            data.dwUnionChoice = WTD_CHOICE_FILE;
            data.fdwRevocationChecks = WTD_REVOKE_NONE;
            data.pFile = Marshal.AllocHGlobal(file.cbStruct);
            Marshal.StructureToPtr(file, data.pFile, false);

            int hr;
            uint reason;
            try
            {
                hr = WinVerifyTrust(INVALID_HANDLE_VALUE, WINTRUST_ACTION_GENERIC_VERIFY_V2, ref data);
                reason = GetLastError();
            }
            finally
            {
                Marshal.FreeHGlobal(data.pFile);
            }

            return hr;
        }

        [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct WINTRUST_FILE_INFO
        {
            public int cbStruct;
            public string pcwszFilePath;
            public IntPtr hFile;
            public IntPtr pgKnownSubject;
        }

        [StructLayoutAttribute(LayoutKind.Sequential)]
        private struct WINTRUST_DATA
        {
            public int cbStruct;
            public IntPtr pPolicyCallbackData;
            public IntPtr pSIPClientData;
            public int dwUIChoice;
            public int fdwRevocationChecks;
            public int dwUnionChoice;
            public IntPtr pFile;
            public int dwStateAction;
            public IntPtr hWVTStateData;
            public IntPtr pwszURLReference;
            public int dwProvFlags;
            public int dwUIContext;
            public IntPtr pSignatureSettings;
        }

        private const int WTD_UI_NONE = 2;
        private const int WTD_REVOKE_NONE = 0;
        private const int WTD_CHOICE_FILE = 1;
        private static readonly IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);
        private static readonly Guid WINTRUST_ACTION_GENERIC_VERIFY_V2 = new Guid("{00AAC56B-CD44-11d0-8CC2-00C04FC295EE}");

        [DllImport("wintrust.dll")]
        private static extern int WinVerifyTrust(IntPtr hwnd, [MarshalAs(UnmanagedType.LPStruct)] Guid pgActionID, ref WINTRUST_DATA pWVTData);
        [DllImport("kernel32.dll")]
        public static extern uint GetLastError();
    }
}
