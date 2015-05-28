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
    public class ConvertUtil
    {
        /// <summary>
        /// 소스모델을 타겟모델의 리스트 형태로 변환한다.
        /// </summary>
        /// <typeparam name="T1">소스모델 타입</typeparam>
        /// <typeparam name="T2">타겟모델 타입</typeparam>
        /// <param name="aModel">소스모델 인스턴스 </param>
        /// <param name="aNameOfTargetKey">타겟모델의 외래키 명칭 예) 공통코드 : NO, 전자결재 : NO_Draft</param>
        /// <param name="aValueOfModelKey">타겟모델의 외래키  </param>
        /// <returns></returns>
        public static IList<T2> ToListOfTargetModel<T1, T2>(T1 aModel, string aNameOfTargetKey, object aValueOfModelKey)
        {


            IEnumerable<FieldInfo> fields = ReflectionUtil.GetAllFields<T1>(aModel);  // 소스모델의 모든 필드를 가져온다. 
           
            IList<T2> lFieldList = new List<T2>();  // 리턴용 리스트 객체 변수

            // 소스모델 필드에 값이 존재할경우 -> 딕셔너리로 데이터및 필드가공 -> 가공된 딕셔너리를 타겟 모델로 변경  
            foreach (FieldInfo f in fields)
            {
              
                // 휴가취소일들 필드는 제외...
                if (f.Name != "mExp_NO_PersonnalHolidays" && (string)f.GetValue(aModel) != "" && (string)f.GetValue(aModel) != null)
                {
                    Dictionary<string, object> ObjDic = new Dictionary<string, object>();
                    ObjDic.Add(aNameOfTargetKey, aValueOfModelKey);
                    ObjDic.Add("Name_Key", f.Name.Replace(">k__BackingField", "").Replace("<", ""));
                    ObjDic.Add("Ref_Val", (string)f.GetValue(aModel));


                    T2 lReturnObj = DictionaryToObject<T2>(ObjDic); // 가공된 딕셔너리를 타겟 모델로 변경 

                    lFieldList.Add(lReturnObj);
                }
            }


            // 사전을 오브젝트로 변환? 
            return lFieldList;
        }

        /// <summary>
        /// 딕셔너리를 오브젝트로 변환한다. 딕셔너리에 주어진 각 키값과 매칭되는값들을 제너릭에 존재하는 프로퍼티에셋팅한다.
        /// </summary>
        /// <typeparam name="T">타겟 모델 타입</typeparam>
        /// <param name="dict">변환대상 딕셔너리</param>
        /// <returns></returns>
        public static T DictionaryToObject<T>(IDictionary<string, object> dict)
        {
            var t = Activator.CreateInstance<T>();                          // 타입을 통해 인스턴스 생성 
            PropertyInfo[] properties = t.GetType().GetProperties();        // 프로퍼티를 가져온다. 

            foreach (PropertyInfo property in properties)                   // 프로퍼티의 수만큼 루프를 실행한다. 
            {
                // 사전과 모델간의 동일 키값이 없다면.. 반복문 종료 
                if (!dict.Any(x => x.Key.Equals(property.Name, StringComparison.InvariantCultureIgnoreCase)))
                    continue;

                // 키밸류셋의 아이템 생성 
                KeyValuePair<string, object> item = dict.First(x => x.Key.Equals(property.Name, StringComparison.InvariantCultureIgnoreCase));

                // Find which property type (int, string, double? etc) the CURRENT property is...
                // 현재 변수의 타입을 가져온다 
                Type tPropertyType = t.GetType().GetProperty(property.Name).PropertyType;

                // Fix nullables...
                // nullable 가능여부에따른 타입 재정의 
                Type newT = Nullable.GetUnderlyingType(tPropertyType) ?? tPropertyType;

                // ...and change the type
                // 키밸류셋을 -> 타겟모델의 변수형으로 변경한다. 
                object newA = Convert.ChangeType(item.Value, newT);
                // 모델의 해당 프로퍼티에 변경된 타입을 삽입한다.
                t.GetType().GetProperty(property.Name).SetValue(t, newA, null);
            }
            return t;


        }



        /// <summary>
        /// 특정모델의 리스트를 다이나믹 객체 리스트로 변경 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="aModel"></param>
        /// <param name="addProp"></param>
        /// <returns></returns>
        public static dynamic ToDynamicObjectList<TSource>(IList<TSource> aModel, Dictionary<string, object> addProp = null)
        {

            List<ExpandoObject> lRtnList = new List<ExpandoObject>();

            foreach (TSource d in aModel)
            {
                var Dobj = ToDynamicObject<TSource>(d, addProp);
                lRtnList.Add(Dobj);
            }

            return lRtnList;
        }


        /// <summary>
        /// 특정 모델을 다이나믹 객체로 변경 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="aModel"></param>
        /// <returns></returns>
        public static dynamic ToDynamicObject<TSource>(TSource aModel, Dictionary<string, object> addProp = null)
        {

            IEnumerable<FieldInfo> fields = ReflectionUtil.GetAllFields<TSource>(aModel);  // 소스모델의 모든 필드를 가져온다.            
            Dictionary<string, object> ObjDic = new Dictionary<string, object>();      // 소스모델의 필드 Key,Val 추출 위한 사전 변수

            // 추가 프로퍼티 속성이 존재한다면, 동적객체에 할당한다.
            if (addProp != null)
            {
                string[] keys = addProp.Keys.ToArray(); // 사전의 키를 배열로 추출                
                foreach (string key in keys)
                {          // 사전의 키/값을 동적객체 프로퍼티에 추가  
                    ObjDic.Add(key, addProp[key]);
                }
            }

            // 소스모델 필드에 값이 존재할경우 -> 딕셔너리로 데이터및 필드가공 -> 가공된 딕셔너리를 타겟 모델로 변경  
            foreach (FieldInfo f in fields)
            {
                ObjDic.Add(f.Name.Replace(">k__BackingField", "").Replace("<", ""), f.GetValue(aModel));
            }

            dynamic DObj = DynamicObj.GetDynamicObject(ObjDic);

            return DObj;
        }

        public static string ToWonString(string aNumStr)
        {
            return ToWonString(Convert.ToInt64(aNumStr));
        }

        public static string ToWonString(Int32 aNumStr)
        {
            return string.Format("{0:n0}", aNumStr);
        }

        public static string ToWonString(Int64 aNumStr)
        {
            return string.Format("{0:n0}", aNumStr);
        }



        /// <summary>
        /// 제너릭 리스트를 데이터 테이블로 교체합니다.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static DataTable ConvertToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties =
               TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;

        }

        public static string DateTimeToString(DateTime aDateTime)
        {
            string lResult = "";
            if (aDateTime >= DateTime.Today)
            {
                TimeSpan t = DateTime.Now - aDateTime;
                if (t.Hours > 0)
                {
                    lResult += t.Hours.ToString() + "시간";
                }
                if (t.Minutes > 0)
                {
                    if(t.Hours > 0) lResult += " ";
                    lResult += t.Minutes.ToString() + "분";
                }
                if(t.Hours == 0 && t.Minutes == 0) {
                    lResult += "방금";
                }
            }
            else if (aDateTime >= DateTime.Today.AddDays(-1))
            {
                lResult += aDateTime.ToString("어제 tt hh:mm");
            }
            else if (aDateTime.Year == DateTime.Now.Year)
            {
                lResult += aDateTime.ToString("M월 d일 tt hh:mm");
            }
            else
            {
                lResult += aDateTime.ToString("yyyy년 M월 d일 tt hh:mm");
            }
            return lResult;
        }

        public static string DateTimeToString(string aDateTime)
        {
            DateTime lDateTime;
            if (DateTime.TryParse(aDateTime, out lDateTime))
            {
                return DateTimeToString(lDateTime);
            }
            else
            {
                return string.Empty;
            }
            
        }
    }
}
