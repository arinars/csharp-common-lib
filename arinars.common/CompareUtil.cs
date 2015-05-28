using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;

namespace arinars.common
{
    /// <summary>
    /// 비교 관련 함수 유틸리티
    /// </summary>
    public class CompareUtil
    {
        /// <summary>
        /// 2개의 NameValueCollection이 같은지 비교 (QueryString 비교)
        /// </summary>
        /// <param name="nvc1"></param>
        /// <param name="nvc2"></param>
        /// <returns></returns>
        public static bool CompareQueryString(NameValueCollection nvc1, NameValueCollection nvc2)
        {
            return nvc1.AllKeys.OrderBy(key => key)
                               .SequenceEqual(nvc2.AllKeys.OrderBy(key => key))
                && nvc1.AllKeys.All(key => nvc1[key] == nvc2[key]);
        }

        /// <summary>
        /// 전체 NameValueCollection에 일부 NameValueCollection이 포함되는지 검사
        /// </summary>
        /// <param name="AllNvc">전체 컬렉션</param>
        /// <param name="InNvc">일부 컬렉션</param>
        /// <returns></returns>
        public static bool ContainsQueryString(NameValueCollection aSource, NameValueCollection aValue)
        {
            bool lContained = !aValue.AllKeys.Except(aSource.AllKeys).Any();
            return lContained && aValue.AllKeys.All(key => aValue[key] == aSource[key]);
        }
    }
}
