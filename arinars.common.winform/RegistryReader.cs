using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace arinars.common.winform
{
    public class RegistryReader
    {
        public static bool ReadAutoLogin()
        {
            try
            {
                bool lAutoLogin;
                string lValue = (string)Read("SOFTWARE\\Careercare\\LemonAgent\\CurrentVersion\\Login", "AutoLogin");

                bool.TryParse(lValue, out lAutoLogin);

                return lAutoLogin;
            }
            catch
            {
                return false;
            }
        }

        public static string ReadDefaultUserName()
        {
            try
            {
                return (string)Read("SOFTWARE\\Careercare\\LemonAgent\\CurrentVersion\\Login", "DefaultUserName");
            }
            catch
            {
                return null;
            }
        }

        public static string ReadDefaultPassword()
        {
            try
            {
                return (string)Read("SOFTWARE\\Careercare\\LemonAgent\\CurrentVersion\\Login", "DefaultPass");
            }
            catch
            {
                return null;
            }
        }

        private static object Read(string aSubKey, string aName)
        {
            object lValue = null;
            //creates or opens the key provided.Be very careful while playing with 
            //windows registry.
            RegistryKey rekey = Registry.LocalMachine.CreateSubKey
                (aSubKey);

            if (rekey == null)
                lValue = null;
            else
            {
                lValue = rekey.GetValue(aName);
            }
            //close the RegistryKey object
            rekey.Close();

            return lValue;
        }

        /// <summary>
        /// 윈도우 시작시 자동 실행 설정값 가져오기
        /// </summary>
        /// <param name="aKey"></param>
        /// <param name="aChecked"></param>
        public static bool GetAutoStart(string aKey)
        {
            RegistryKey lRegistryKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);

            if (lRegistryKey.GetValue(aKey) != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
