using BlueInks.Models;
using BumYoungTools.Async;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using RedInks;
using RedInks.Interfaces;
using RedInks.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Principal;
using System.Windows;
using System.Windows.Input;

namespace BlueInks.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public enum BlueInksMode
        {
            CLI = 0,
            GUI = 1,
            DEBUG = 2,
        }
        public AsyncObservableCollection<Diagnosis> DsisItems { get; set; }
        public ICommand DoDsisCommand { get; set; }
        public Boolean _CanDsisCommandButton;
        public Boolean CanDsisCommandButton {
            get {
                return _CanDsisCommandButton;
            }
            set {
                _CanDsisCommandButton = value;
                RaisePropertyChanged("CanDsisCommandButton");
            } }
        public MainViewModel()
        {
            if (IsInDesignMode)
            {
                // Code runs in Blend --> create design time data.

            }
            else
            {
                // 권한 상승 로직
                if (IsAdministrator() == false)
                {
                    //https://stackoverflow.com/questions/133379/elevating-process-privilege-programmatically
                    // Restart program and run as admin
                    var exeName = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
                    ProcessStartInfo startInfo = new ProcessStartInfo(exeName);
                    startInfo.Verb = "runas";
                    System.Diagnostics.Process.Start(startInfo);
                    Application.Current.Shutdown();
                    return;
                }

                {
                    // 초기화 중에 버튼 활성화 현상을 차단 하기 위해 비활성화
                    CanDsisCommandButton = false;
                }
                
                DsisItems = new AsyncObservableCollection<Diagnosis>();
                InitializeViewModel();
                DoDsisCommand = new RelayCommand(DoDsisActivity);
            }
        }
        private void InitializeViewModel()
        {
            try
            {
                // Try to Load Settings
                BlueInksSettings.Load();
            }
            catch (Exception)
            {
                // If Failed, Load Default Settings
                BlueInksSettings.Default();
                BlueInksSettings.Instance.Save();
            }

            {
                // Button Control 및 기타 Control 활성화
                CanDsisCommandButton = true;
            }
        }

        private void DoDsisActivity() {
            /*
             * 진단 시작 수행 (라이프 사이클 : 시작)
             * 1. 진단 폴더 생성
             * 2. 모든 클래스의 진단 결과를 및 파생 결과를 소산, 진단 폴더 경로를 알고 있어야 함, 초기화 시에 이를 전달하는 구조가 좋을 듯
             * 3. 반복적인 요청을 차단하기 위해서 버튼 비활성화 기능을 추가
            */
            {
                CanDsisCommandButton = false;
            }
            DiagnosisFactory factory = new DiagnosisFactory();
            for (int i = 1; i < 10; i++)
            {
                //MessageBox.Show(String.Format("MA{0:d3}", i));
                var Item = factory.GetInstances(String.Format("MA{0:d3}", i));
                if (Item != null)
                    DsisItems.Add(Item);
            }

            foreach (var item in DsisItems)
            {
                item.DoWork();
            }
        }

        private void EnableActivity()
        {
            // 다시 버튼을 활성화 시키기 위해서 점검이 완료되었는지를 확인하는 로직이 필요함
        }

    private static bool IsAdministrator()
    {
        WindowsIdentity identity = WindowsIdentity.GetCurrent();
        WindowsPrincipal principal = new WindowsPrincipal(identity);
        return principal.IsInRole(WindowsBuiltInRole.Administrator);
    }

    private void ArgumentParser(IEnvironmentService service)
        {
            IEnumerable<String> Args = service.GetCommandLineArguments();
            List<String> ArgList = new List<String>(Args);
            //ArgList[0]
        }
    }
}