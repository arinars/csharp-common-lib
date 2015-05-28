using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace arinars.common.web
{
    public class CookieUtil
    {
        private static object mSinglTon = new object();


        /// <summary>
        /// 쿠키 저장 (직렬화를 통한 데이터 저장)
        /// </summary>
        /// <param name="aName"></param>
        public static void SetGlobal(string aName, HttpResponse aResponse, dynamic aValue, int aExpires = 120)
        {
            HttpCookie GlobalVar = new HttpCookie("GlobalVar");
            GlobalVar.Values[aName] = JsonConvert.SerializeObject(aValue);
            GlobalVar.Expires = DateTime.Now.AddMinutes(aExpires);
            aResponse.Cookies.Add(GlobalVar);    // Add Cookie
        }

        /// <summary>
        /// 특정 값을 가져온다. 상황에 따라서 동적 변경 가능하다.
        /// </summary>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="aKey"></param>
        /// <param name="aRequest"></param>
        /// <param name="aResponse"></param>
        /// <param name="aParam"></param>
        /// <returns></returns>
        public static TReturn GetGlobal<TReturn>(string aKey, HttpRequest aRequest, dynamic aParam)
        {
            TReturn lReturn;

            if (aRequest.Cookies["GlobalVar"] != null)
            {
                lReturn = JsonConvert.DeserializeObject<TReturn>(aRequest.Cookies["GlobalVar"][aKey]);
            }
            else
            {
                lReturn = default(TReturn);
            }


            return lReturn;
        }

        public static void ClearGlobal(HttpRequest aRequest, HttpResponse aResponse)
        {
            if (aRequest.Cookies["GlobalVar"] != null)
            {
                HttpCookie myCookie = new HttpCookie("GlobalVar");
                myCookie.Expires = DateTime.Now.AddDays(-1d);
                aResponse.Cookies.Add(myCookie);
            }
        }

    }
}
