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
                hkey = Registry.LocalMachine;
                // Key 가 없는 경우 Null Reference Error 가 발생 
                hkey = hkey.OpenSubKey(path);
                if(hkey == null)
                {
                    return null;
                }
                else
                {
                    return (string)hkey.GetValue(key);
                }
                

            }else
            {
                return null;
            }
        }

    }
}
    