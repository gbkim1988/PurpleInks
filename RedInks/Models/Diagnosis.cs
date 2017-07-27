using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedInks.Models
{

    public class Diagnosis
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

        public Status DsisStatus;
        public Judge DsisJudgement;
        public String DsisTitle;
        public String DsisCode;

        public Diagnosis(DoIt doit)
        {
            // 생성자 
            DsisStatus = Status.Pending;
            DsisJudgement = Judge.Pending;
            DsisTitle = doit.Title;
            DsisCode = doit.Code;
        }
        public void Initialize()
        {

        }
    }

    public abstract class DoIt {
        public String Title;
        public String Code;
        public DoIt() {
            Title = "기본 테스트1";
            Code = "MA-001";
        }
        public abstract void ExecuteCommand();
        public async Task<bool> Start()
        {

            ExecuteCommand();
            Parse();
            Judge();

            return true;
        }

        public abstract void Parse();


        public abstract void Judge();
    }

    public class MA001 : DoIt {
        public MA001() {
            Title = "기본 테스트2";
            Code = "MA-002";
        }
        public override void Parse() {
        }
        public override void Judge()
        {
            
        }
        public override void ExecuteCommand()
        {
            
        }
    }
}
