using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace arinars.expansion
{
    public static class IEnumerableExpansion
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="aList">원본 소스</param>
        /// <param name="aSource"></param>
        /// <returns></returns>
        public static IEnumerable<T> GetDeleteItems<T>(this IEnumerable<T> aSource, IEnumerable<T> aInner, string aSourceKey, string aInnerKey)
        {
            PropertyInfo lSourcePI = typeof(T).GetProperty(aSourceKey);
            PropertyInfo lInnerPI = typeof(T).GetProperty(aInnerKey);

            var lUpdateList = aSource.Join(aInner ?? Enumerable.Empty<T>(), x => lSourcePI.GetValue(x, null), y => lInnerPI.GetValue(y, null), (x, y) => x);
            var lDeleteList = aSource.Except(lUpdateList);
            return lDeleteList;
        }

        ///// <summary>
        ///// 타이틀을 추가하여 리턴한다.
        ///// </summary>
        ///// <param name="aList"></param>
        ///// <param name="aTitle"></param>
        ///// <returns></returns>
        //public static IEnumerable<SelectListItem> GetSelectListItemWithTitle(this IEnumerable<SelectListItem> aSource, string aTitle)
        //{
        //    List<SelectListItem> items = aSource.ToList();
        //    items.Insert(0, new SelectListItem() { Text = aTitle, Value = string.Empty, Selected = false });

        //    return items;
        //}
    }
}
