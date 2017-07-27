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
                // ���� ��� ����
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
                    // �ʱ�ȭ �߿� ��ư Ȱ��ȭ ������ ���� �ϱ� ���� ��Ȱ��ȭ
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
                // Button Control �� ��Ÿ Control Ȱ��ȭ
                CanDsisCommandButton = true;
            }
        }

        private void DoDsisActivity() {
            /*
             * ���� ���� ���� (������ ����Ŭ : ����)
             * 1. ���� ���� ����
             * 2. ��� Ŭ������ ���� ����� �� �Ļ� ����� �һ�, ���� ���� ��θ� �˰� �־�� ��, �ʱ�ȭ �ÿ� �̸� �����ϴ� ������ ���� ��
             * 3. �ݺ����� ��û�� �����ϱ� ���ؼ� ��ư ��Ȱ��ȭ ����� �߰�
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
            // �ٽ� ��ư�� Ȱ��ȭ ��Ű�� ���ؼ� ������ �Ϸ�Ǿ������� Ȯ���ϴ� ������ �ʿ���
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