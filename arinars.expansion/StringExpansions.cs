using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace arinars.common.expansion
{
	public static class StringExpansions
	{
		/// <summary>
		/// 오른쪽에서 문자열 잘라내기
		/// </summary>
		public static string right(this string value, int length)
		{
			return value.Substring(value.Length - length);
		}

        /// <summary>
        /// 특정 스트링을 nullable 타입으로 변경한다.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="valueAsString"></param>
        /// <returns></returns>
        public static T? GetValueOrNull<T>(this string valueAsString) where T : struct
        {
            if (string.IsNullOrEmpty(valueAsString))
                return null;
            return (T)Convert.ChangeType(valueAsString, typeof(T));
        }

        /// <summary>
        /// nullable int32로 변경 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static int? ToNullableInt32(this string s)
        {
            int i;
            if (Int32.TryParse(s, out i)) return i;
            return null;
        }
	}
}
