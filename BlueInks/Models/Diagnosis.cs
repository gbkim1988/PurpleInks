using BlueInks;
using RedInks.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static RedInks.Models.Diagnosis;

namespace RedInks.Models
{

    public class Diagnosis : Notifier
    {
        public enum Status {
            Pending = 1,
            Processing = 2,
            Completed = 3,
            Error = -1
        }

        public enum Judge
        {
            Pending = 1,
            Vulnerable = 2,
            Ok = 3
        }
        /*
            Notifier 를 상속 후 Property 에 대해서 PorertyChange 코드를 심어, 상태 변화시 관측이 가능하도록 지원 
        */
        private Status _DsisStatus;
        public Status DsisStatus {
            get {
                return _DsisStatus;
            }
            set {
                _DsisStatus = value;
                RaisePropertyChanged("DsisStatus");
            }
        }
        private Judge _DsisJudgement;
        public Judge DsisJudgement
        {
            get
            {
                return _DsisJudgement;
            }
            set
            {
                _DsisJudgement = value;
                RaisePropertyChanged("DsisJudgement");
            }
        }
        private String _DsisTitle;
        public String DsisTitle
        {
            get
            {
                return _DsisTitle;
            }
            set
            {
                _DsisTitle = value;
                RaisePropertyChanged("DsisTitle");
            }
        }
        private String _DsisCode;
        public String DsisCode
        {
            get
            {
                return _DsisCode;
            }
            set
            {
                _DsisCode = value;
                RaisePropertyChanged("DsisCode");
            }
        }
        private Dictionary<String, List<String>> _DsisProofs;
        public Dictionary<String, List<String>> DsisProofs {
            get
            {
                return _DsisProofs;
            }
            set
            {
                _DsisProofs = value;
                RaisePropertyChanged("DsisProofs");
            }
        }

        public DoIt Doit;
        public Diagnosis(DoIt doit)
        {
            // 생성자 
            this.DsisStatus = Status.Pending;
            this.DsisJudgement = Judge.Pending;
            this.DsisTitle = doit.Title;
            this.DsisCode = doit.Code;
            this.DsisProofs = null;
            this.Doit = doit;
            doit.AssignCallback((Status status) =>
            {

                this.DsisStatus = status;
            });

        }
        public void ConfigureSetting() {

        }
        public async void DoWork()
        {
            var task = Task.Run(() => this.Doit.Start());
            await task;
        }
    }

    public abstract class DoIt {
        public String Title;
        public String Code;
        public Action<Status> Callback;
        public Status DoItStatus;
        public DoIt(Action<Status> callback = null) {
            this.Title = "";
            this.Code = "";
            this.DoItStatus = Status.Pending;
            this.Callback = callback;
        }

        public void AssignCallback(Action<Status> callback) {
            this.Callback = callback;
        }
        public abstract void ExecuteCommand();
        public abstract bool Available();
        public async Task<bool> Start()
        {

            this.Callback(Status.Processing);
            if (Available())
            {
                ExecuteCommand();
                if (DoItStatus == Status.Completed)
                {
                    Parse();
                    Judge();
                }
                if (this.DoItStatus != Status.Error)
                    this.Callback(Status.Completed);
            }
            else {
                // 진단 불가인 경우 유보 상태로 두는 것이 좋다.
                DoItStatus = Status.Completed;
                // 일단 점검 완료 상태로 전환 시킴
                this.Callback(Status.Completed);
            }
            return true;
        }
        public void NotifyStatusCallback(Status status, String Message) {
            this.DoItStatus = status;
            this.Callback(status);
            // Propagate Messages
            System.Windows.MessageBox.Show(Message);
        }
        public abstract void Parse();

        public abstract void Judge();
    }

    public class MA001 : DoIt {
        public MA001() {
            Title = "브라우저 유해 사이트 접근 이력 확인";
            Code = "MA001";
        }
        public override bool Available()
        {
            return true;
        }
        public override void Parse() {
            MessageBox.Show("Let's get it");
        }
        public override void Judge()
        {
            
        }
        public override void ExecuteCommand()
        {
            ExecuteProcess.ExecuteCallback(Path.Combine(Directory.GetCurrentDirectory(), "bin"), "lastactivityview.exe", new List<String> { "/sxml", "c:\\TESTMODY.xml"}, NotifyStatusCallback);
            /*
                for (int i = 0; i < 1000; i++) {
                    Console.WriteLine("ExecuteCommand - " + i.ToString());
                }
            */
        }
    }

    public class MA002 : DoIt
    {
        public MA002()
        {
            Title = "기본 테스트2";
            Code = "MA-002";
        }
        public override bool Available()
        {
            return true;
        }
        public override void Parse()
        {
        }
        public override void Judge()
        {

        }
        public override void ExecuteCommand()
        {

        }
    }
}
