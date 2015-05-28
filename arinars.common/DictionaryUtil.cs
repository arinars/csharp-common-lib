using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Reflection;
using System.Collections;
using System.Linq.Expressions;
using System.Globalization;
using System.Diagnostics;
using System.ComponentModel;
using System.Dynamic;
using System.Data;

namespace arinars.common
{
    /// <summary>
    /// 컨버팅 관련 함수 모음 
    /// </summary>
    public class DictionaryUtil
    {
        /// <summary>
        /// 키에 해당하는 값을 리턴한다.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="aSource"></param>
        /// <param name="aKey"></param>
        /// <returns></returns>
        public static TValue GetValue<TKey, TValue>(IDictionary<TKey, TValue> aSource, TKey aKey)
        {
            if (aSource != null)
            {
                TValue lOutObj;
                if (aSource.TryGetValue(aKey, out lOutObj))
                {
                    if (lOutObj != null)
                    {
                        return lOutObj;
                    }
                    else
                    {
                        return default(TValue);
                    }
                }
                else
                {
                    return default(TValue);
                }
            }
            return default(TValue);
        }
    }
}
