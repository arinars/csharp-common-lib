using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace arinars.common
{
    public class SecurityUtil
    {
        /// <summary>
        /// Returns the MD5 sum of the given string. The output always has 32 digits.
        /// </summary>
        /// <param name="aMsg">string the string to calculate the MD5 sum for.</param>
        /// <returns>MD5 sum of the given string.</returns>
        public static string GetMD5(string aMsg)
        {
            MD5CryptoServiceProvider lMD = null;
            try
            {
                lMD = new MD5CryptoServiceProvider();
                byte[] lBufSource = Encoding.UTF8.GetBytes(aMsg);
                byte[] lBufTarget = lMD.ComputeHash(lBufSource);
                StringBuilder lSB = new StringBuilder();
                foreach (byte lB in lBufTarget)
                {
                    lSB.Append(lB.ToString("x2").ToLower());
                }
                return lSB.ToString();
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Returns the SHA1 sum of the given string. The output always has 32 digits.
        /// </summary>
        /// <param name="aMsg">string the string to calculate the SHA1 sum for.</param>
        /// <returns>SHA1 sum of the given string.</returns>
        public static string GetSHA1(string aMsg)
        {
            SHA1CryptoServiceProvider lSHA1 = null;
            try
            {
                lSHA1 = new SHA1CryptoServiceProvider();
                byte[] lBufSource = Encoding.UTF8.GetBytes(aMsg);
                byte[] lBufTarget = lSHA1.ComputeHash(lBufSource);
                StringBuilder lSB = new StringBuilder();
                foreach (byte lB in lBufTarget)
                {
                    lSB.Append(lB.ToString("x2").ToLower());
                }
                return lSB.ToString();
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
