using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Web;

namespace arinars.common.web
{
    /// <summary>
    /// 비교 관련 함수 유틸리티
    /// </summary>
    public class WebCompareUtil
    {
        /// <summary>
        /// QueryString 비교.
        /// <paramref name="aQueryString1"/> 와 <paramref name="aQueryString2"/> 가 일치하는지 검사
        /// </summary>
        /// <param name="aQueryString1"></param>
        /// <param name="aQueryString2"></param>
        /// <returns></returns>
        public static bool CompareQueryString(NameValueCollection aQueryString1, string aQueryString2)
        {
            if (aQueryString1 == null)
            {
                aQueryString1 = new NameValueCollection();
            }
            if (aQueryString2 == null)
            {
                aQueryString2 = string.Empty;
            }
            NameValueCollection lnvc2 = HttpUtility.ParseQueryString(aQueryString2);

            return CompareUtil.CompareQueryString(aQueryString1, lnvc2);
        }

        /// <summary>
        /// QueryString 비교.
        /// <paramref name="aQueryString2"/> 가 <paramref name="aQueryString1"/>에 포함되는지 검사
        /// </summary>
        /// <param name="aQueryString1"></param>
        /// <param name="aQueryString2"></param>
        /// <returns></returns>
        public static bool ContainsQueryString(NameValueCollection aSource, string aValue)
        {
            if (aSource == null)
            {
                aSource = new NameValueCollection();
            }
            if (aValue == null)
            {
                aValue = string.Empty;
            }
            NameValueCollection lValue = HttpUtility.ParseQueryString(aValue);

            return CompareUtil.ContainsQueryString(aSource, lValue);
        }
    }
}
