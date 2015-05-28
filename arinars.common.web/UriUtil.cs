using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using System.Web;

namespace arinars.common.web
{
    public sealed class UriUtil
    {
        public static string Combine(params string[] aUrls)
        {
            if (aUrls != null && aUrls.Count() > 0)
            {
                Uri lUri = new Uri(aUrls.First());
                for (int i = 1; i < aUrls.Count() ; i++)
                {
                    string lPartUrl = aUrls[i];
                    if (i != (aUrls.Count() - 1) && lPartUrl.EndsWith("/") == false)
                    {
                        lPartUrl = lPartUrl + "/";
                    }
                    lUri = new Uri(lUri, lPartUrl);
                }
                return lUri.ToString();
            }
            return null;
        }

        public static RouteValueDictionary GetRouteValues(Uri aUrl)
        {
            if (aUrl == null)
            {
                return null;
            }
            var lNameValueCollection = HttpUtility.ParseQueryString(aUrl.Query);
            IDictionary<string, string> lRouteValueDic = lNameValueCollection.Cast<string>().ToDictionary(p => p, p => lNameValueCollection[p]);
            RouteValueDictionary lQuery = new RouteValueDictionary();
            foreach (var lItem in lRouteValueDic)
            {
                lQuery.Add(lItem.Key, lItem.Value);
            }
            return lQuery;
        }

        public static RouteValueDictionary GetRouteValues(Uri aUrl, object aParam)
        {
            RouteValueDictionary lDic = GetRouteValues(aUrl) ?? new RouteValueDictionary();

            var lParamDic = ReflectionUtil.ConverToDictionary<object>(aParam);
            foreach(KeyValuePair<string, object> lParam in (Dictionary<string, object>)lParamDic) {
                lDic.Add(lParam.Key, lParam.Value);
            }
            return lDic;
        }
    }
}
