using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace arinars.common.winform
{
    public class RegistryWriter
    {
        /// <summary>
        /// 자동 로그인 설정
        /// </summary>
        /// <param name="aUsername"></param>
        /// <param name="aPassword"></param>
        public static void WriteDefaultLogin(string aUsername, string aPassword)
        {
            //creates or opens the key provided.Be very careful while playing with 
            //windows registry.
            RegistryKey rekey = Registry.LocalMachine.CreateSubKey
                ("SOFTWARE\\Careercare\\LemonAgent\\CurrentVersion\\Login");

            if (rekey == null)
                System.Windows.Forms.MessageBox.Show
                ("There has been an error while trying to write to windows registry");
            else
            {
                //these are our hero like values here
                //simply use your RegistryKey objects SetValue method to set these keys
                rekey.SetValue("AutoLogin", true);
                rekey.SetValue("DefaultUserName", aUsername);
                rekey.SetValue("DefaultPass", aPassword);
            }
            //close the RegistryKey object
            rekey.Close();
        }

        /// <summary>
        /// 자동 로그인 취소
        /// </summary>
        public static void RemoveDefaultLogin()
        {
            RegistryKey rekey = Registry.LocalMachine.CreateSubKey
                ("SOFTWARE\\Careercare\\LemonAgent\\CurrentVersion\\Login");
            if (rekey == null)
                System.Windows.Forms.MessageBox.Show("Registry write error");
            else
            {
                //deleting the values,
                // first parameter is the Name of the value, 
                //second is a boolean flag,
                // indication wether the method should 
                //raise an exception if the specified 
                //value is NOT present in the 
                //registry. We set it to false, 
                //because we 'know' its there... we just added it :P
                rekey.DeleteValue("DefaultUserName", false);
                rekey.DeleteValue("DefaultPass", false);
                rekey.DeleteValue("AutoLogin", false);
            }
            //close the registry object
            rekey.Close();
        }


        /// <summary>
        /// 윈도우 시작시 자동 실행 설정
        /// </summary>
        /// <param name="aKey"></param>
        /// <param name="aChecked"></param>
        public static void SetAutoStart(string aKey, bool aChecked)
        {
            RegistryKey lRegistryKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);

            if (aChecked)
            {
                if (lRegistryKey.GetValue(aKey) == null)
                {
                    lRegistryKey.SetValue(aKey, System.Windows.Forms.Application.ExecutablePath.ToString());
                }
            }
            else
            {
                lRegistryKey.DeleteValue(aKey, false);
            }
        }
    }
}
