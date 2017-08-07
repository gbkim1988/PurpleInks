using BlogMaster.Async;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace GreyInks
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public enum Result
        {
            Fulfilled = 1,
            Negative = -1,
            Pending = 0
        }
        [Serializable()]
        public class CheckItem : INotifyPropertyChanged
        {
            [System.Xml.Serialization.XmlElement("code")]
            public string code { get; set; }
            [System.Xml.Serialization.XmlElement("title")]
            public string title;
            public string Title {
                get {
                    return title;
                }
                set {
                    title = value;
                    OnPropertyChanged("Title");
                } }
            [System.Xml.Serialization.XmlElement("status")]
            public Result status;
            public Result Status { get { return status; } set { status = value; OnPropertyChanged("Status"); } }
            [System.Xml.Serialization.XmlElement("help")]
            public string help { get; set; }
            [System.Xml.Serialization.XmlElement("proofs")]
            public Dictionary<string, string> proofs;
            public String progress;
            public String Progress {
                get { return progress; }
                set
                {
                    progress = value;
                    OnPropertyChanged("Progress");
                }
            }
            public event PropertyChangedEventHandler PropertyChanged;

            public Dictionary<string, string> Proofs {
                get { return proofs; }
                set {
                    proofs = value;
                    OnPropertyChanged("Proofs");
                } }

            protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                PropertyChangedEventHandler handler = this.PropertyChanged;
                if (handler != null)
                {
                    var e = new PropertyChangedEventArgs(propertyName);
                    handler(this, e);
                }
            }
        }

        public AsyncObservableCollection<CheckItem> CheckItemList { get; set; }
        public Timer InitTimer;
        public void InitializeCheckList()
        {
            /// 모듈을 시작하기 전에 멤버들을 초기화
            
            this.CheckItemList = new AsyncObservableCollection<CheckItem>();
            DiagnosticCheckList.ItemsSource = this.CheckItemList;
            GreyUtils.Instance.ExtractExecutable("CheckList.xml");

            XElement root = XElement.Load("CheckList.xml");

            IEnumerable<XElement> items = from el in root.Elements("Item") select el;

            foreach(var item in items)
            {
                this.CheckItemList.Add(new CheckItem
                {
                    code = (string)item.Attribute("code"),
                    help = item.Element("help").Value,
                    title = (string)item.Attribute("title"),
                    status = Result.Pending,
                    proofs = new Dictionary<string, string>(),
                    Progress = ""
                });
            }
        }
        public MainWindow()
        {
            InitializeComponent();
            GreyUtils.Default();
            GreyCommand.Default();
            InitializeCheckList();

            InitTimer = new Timer();
            InitTimer.Interval = 3000;
            InitTimer.Elapsed += DoItNow;
            InitTimer.AutoReset = false;
            InitTimer.Start();

            //string value = GreyWnReg.GetRegistryValue("SOFTWARE\\Microsoft\\Internet Explorer", "svcUpdateVersion", GreyWnReg.Hive.LocalMachine);
            //MessageBox.Show(value);
            //value = GreyWnReg.GetRegistryValue("SOFTWARE\\Microsoft\\Internet Explorer", "svcKBNumber", GreyWnReg.Hive.LocalMachine);
            //MessageBox.Show(value);
            // SOFTWARE\AhnLab\ASPack\9.0\Option\AVMON\
            // HKEY_LOCAL_MACHINE\SOFTWARE\AhnLab\ASPack\9.0\ServiceStatus
            // 레지스트리의 값이 없는 경우도 평가에 넣어야 함
            //value = GreyWnReg.GetRegistryValue("SOFTWARE\\AhnLab\\ASPack\\9.0\\Option\\AVMON", "sysmonuse", GreyWnReg.Hive.LocalMachine);
            //MessageBox.Show(value);
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

            //var CheckList = ConfigurationManager.AppSettings["Title"];
            //GreyUtils.Instance.ExtractExecutable("CheckList.xml");
            //string[] arg = GreyCommand.GetCommandLine("util1");
            //GreyCommand.ExecutedCallback(Directory.GetCurrentDirectory(), arg[0], arg[1]);
            //string output = GreyCommand.GetOutputFile("util1");
            //var reports = GreyXML.GetXmlOutput(output);
            //MessageBox.Show(reports[0]["time"]);

            /*
            XElement root = XElement.Load("CheckList.xml");

            IEnumerable<XElement> util = from el in root.Elements("Item")
                                         where (string)el.Attribute("code") == "MA-001"
                                         select el;
            IEnumerable<XElement> util1 = from el in root.Descendants("util")
                        where (string)el.Attribute("type") == "util1"
                        select el;
            //where (string)el.Attribute("type") == "util1"
            //select el;

            foreach(XElement el in util)
            {
                MessageBox.Show((string)el.Attribute("title"));
                MessageBox.Show((string)el.Attribute("code"));
                
            }
            // 명령어 옵션 추출
            foreach (var el in util1)
            {
                MessageBox.Show(string.Join(" ", (from xl in el.Elements("param")
                                                  select (string)xl.Attribute("value"))));
            }

            /*
             XElement root = XElement.Load("PurchaseOrders.xml");  
            IEnumerable<XElement> purchaseOrders =  
                from el in root.Elements("PurchaseOrder")  
                where   
                    (from add in el.Elements("Address")  
                    where  
                        (string)add.Attribute("Type") == "Shipping" &&  
                        (string)add.Element("State") == "NY"  
                    select add)  
                    .Any()  
                select el;  
            foreach (XElement el in purchaseOrders)  
                Console.WriteLine((string)el.Attribute("PurchaseOrderNumber"));  
             
             */
            //GreyUtils.Instance.ExtractExecutable("lastactivityview.exe");
            //DiagnosticCheckList.ItemsSource = new List<String>{"fy","sadf" };
        }
        private void DoItNow(object sender, ElapsedEventArgs e)
        {
            GreyDoIt.Default();
            GreyDoIt.Do(this.CheckItemList);
            MessageBox.Show("Fin");
        }

        private static bool IsAdministrator()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
    }
}
