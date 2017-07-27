﻿using RedInks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueInks.Models
{
    public class DiagnosisFactory
    {
        /*
            Factory Pattern 을 이용하여, 진단 클래스의 인스턴스 생성을 용이하게 한다.
         */
        public Diagnosis GetInstances(String code) {
            switch(code){
                case "MA001":
                    return new Diagnosis(new MA001());
                case "MA002":
                    return new Diagnosis(new MA002());
                default:
                    return null;
            }
        }
    }
}
