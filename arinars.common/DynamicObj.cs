using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;

namespace arinars.common
{
    public class DynamicObj
    {

        /// <summary>
        /// 사전형태로 받아 동적 Object의 프로퍼트릴 추가후 반환한다.
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        public static dynamic GetDynamicObject(Dictionary<string, object> properties)
        {
            var dynamicObject = new ExpandoObject() as IDictionary<string, Object>;
            foreach (var property in properties)
            {
                dynamicObject.Add(property.Key, property.Value);
            }
            return dynamicObject;
        }
    }
}
