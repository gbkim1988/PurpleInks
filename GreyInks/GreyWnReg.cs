using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace GreyInks
{
    public class GreyWnReg
    {
        /// Windows 환경의 Registry 환경을 분석하여 
        /// 사용자가 접근하기 용이한 API를 제공
        public enum Hive
        {
            LocalMachine = 0,
            CurrentUser = 1
        }
        public static string GetRegistryValue(string path, string key, Hive hive)
        {
            RegistryKey hkey;
            if (hive == Hive.CurrentUser)
            {
                hkey = Registry.CurrentUser;

                hkey = hkey.OpenSubKey(path);
                if (hkey == null)
                {
                    return null;
                }
                else
                {
                    return (string)hkey.GetValue(key);
                }

            }
            else if( hive == Hive.LocalMachine)
            {
                //hkey = Registry.LocalMachine;
                // Key 가 없는 경우 Null Reference Error 가 발생 
                //hkey = hkey.OpenSubKey(path);
                // 플랫폼에 종속적인 코드가 투입되야 할 것으로 생각됨
                using (var hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
                {
                    hkey = hklm.OpenSubKey(path);
                    if (hkey == null)
                    {
                        return null;
                    }
                    else
                    {
                        return (string)hkey.GetValue(key).ToString();
                    }
                }

            }else
            {
                return null;
            }
        }

        public static string [] GetSubKeyNames(string path, Hive hive)
        {
            RegistryKey hkey;
            if (hive == Hive.CurrentUser)
            {
                hkey = Registry.CurrentUser;
                hkey = hkey.OpenSubKey(path, false);

                if (hkey == null)
                {
                    return null;
                }
                else
                {
                    return hkey.GetSubKeyNames();
                }

            }
            else if (hive == Hive.LocalMachine)
            {
                hkey = Registry.LocalMachine;
                using (var hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
                {
                    hkey = hklm.OpenSubKey(path);
                }
                //hkey = hkey.OpenSubKey(path, false);
                if (hkey == null)
                {
                    return null;
                }
                else
                {
                    return hkey.GetSubKeyNames();
                }


            }
            else
            {
                return null;
            }
        }

    }
}
    