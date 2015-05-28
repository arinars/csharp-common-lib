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
using System.Web.Mvc;

namespace arinars.common.web
{
    /// <summary>
    /// 컨버팅 관련 함수 모음 
    /// </summary>
    public class WebConvertUtil
    {
        /// <summary>
        /// 2차원 배열의 Array를 인자로 DropdownList 형태로 변환하여 리턴한다. 
        /// </summary>
        /// <param name="aArray"></param>
        /// <returns></returns>
        public static IList<SelectListItem> ArrayToDropdownList(string [,] aArray){
        
            // 사전형으로 선 변환
            Dictionary<string, string> Dict = Enumerable
                                                        .Range(0, aArray.GetLength(0))
                                                        .ToDictionary(i => aArray[i, 0], i => aArray[i, 1]);
            // 원하는 형태에 맞게 재가공 
            List<SelectListItem> lReturnlist = Dict.Select(p => new SelectListItem { Value = p.Key, Text = p.Value }).ToList();

            return lReturnlist;
            
        }

        /// <summary>
        /// 스트링 배열로 키와 값이 동일한 SelectListItem 열거자를 생성한다.
        /// </summary>
        /// <param name="aArray"></param>
        /// <param name="aTitle"></param>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> ToSelectListItems(Array aArray, string aTitle = "")
        {
            if(!string.IsNullOrWhiteSpace(aTitle)) {
                yield return new SelectListItem { Text = aTitle };
            }
            foreach (var item in aArray)
            {
                var lItemStr = Convert.ToString(item);
                yield return new SelectListItem { Text = lItemStr, Value = lItemStr};
            }
        }

		/// <summary>
		/// 스트링 배열로 키와 값이 동일한 SelectListItem 열거자를 생성한다.
		/// </summary>
		/// <param name="aArray"></param>
		/// <param name="aTitle"></param>
		/// <returns></returns>
		public static IEnumerable<SelectListItem> ToSelectListItems<T>(IEnumerable<T> aArray, string aKeyProperty, string aValueProperty, string aTitle = "")
		{
			PropertyInfo lKeyProp = typeof(T).GetProperty(aKeyProperty);
			PropertyInfo lValueProp = typeof(T).GetProperty(aValueProperty);
			
			if (!string.IsNullOrWhiteSpace(aTitle))
			{
				yield return new SelectListItem { Text = aTitle };
			}
			foreach (var item in aArray)
			{
				object lKey = lKeyProp.GetValue(item, null);
				object lValue = lValueProp.GetValue(item, null);
				yield return new SelectListItem { Text = Convert.ToString(lValue), Value = Convert.ToString(lKey) };
			}
		}
    }
}
