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

namespace arinars.common
{
    /// <summary>
    /// Reflection 관련 함수 모음 
    /// </summary>
    public class ReflectionUtil
    {
        /// <summary>
        /// 특정 오브젝트의 모든 필드값을 가져온다. 
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static IEnumerable<FieldInfo> GetAllFields<T>(T t) {

            if (t == null)
                return Enumerable.Empty<FieldInfo>();

            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            return t.GetType().GetFields(flags);

            
        }

        /// <summary>
        /// 특정 오브젝트를 Dictionary 로 변환한다. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="aModel"></param>
        /// <returns></returns>
        public static IDictionary ConverToDictionary<T>(T aModel) {

            var myDict = aModel.GetType()
                               .GetProperties()
                               .Select(pi => new { Name = pi.Name, Value = pi.GetValue(aModel, null) })
                               .Union(
                                   aModel.GetType()
                                   .GetFields()
                                   .Select(fi => new { Name = fi.Name, Value = fi.GetValue(aModel) })
                                )
                               .ToDictionary(ks => ks.Name, vs => vs.Value);

            return myDict;
        }

        /// <summary>
        /// 프로퍼티 정보를 가져온다.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="source"></param>
        /// <param name="propertyLambda"></param>
        /// <returns></returns>
        public static PropertyInfo GetPropertyInfo<TSource, TProperty>(TSource source, Expression<Func<TSource, TProperty>> propertyLambda)
        {
            Type type = typeof(TSource);

            MemberExpression member = propertyLambda.Body as MemberExpression;
            if (member == null)
                throw new ArgumentException(string.Format("Expression '{0}' referts to a method, not a property.", propertyLambda.ToString()));

            PropertyInfo propInfo = member.Member as PropertyInfo;
            if (propInfo == null)
                throw new ArgumentException(string.Format("Expression '{0}' refers to a field, not a property", propertyLambda, propertyLambda.ToString()));

            if (type != propInfo.ReflectedType && !type.IsSubclassOf(propInfo.ReflectedType))
                throw new ArgumentException(string.Format("Expression '{0}' refers to a property that is not from type {1}.", propertyLambda.ToString(), type));

            return propInfo;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expr"></param>
        /// <returns></returns>
        public static string GetMemberName<T>(Expression<Func<T, object>> expr)
        {
            System.Linq.Expressions.Expression body = ((LambdaExpression)expr).Body;
            MemberExpression memberExpression = body as MemberExpression;
            if (memberExpression == null)
            {
                memberExpression = (MemberExpression)((UnaryExpression)body).Operand;
            }
            return memberExpression.Member.Name;
        }

        public static string GetDisplayName<T>(Expression<Func<T, object>> lPropertyExpression)
        {
            var memberInfo = GetPropertyInformation(lPropertyExpression.Body);
            if (memberInfo == null)
            {
                throw new ArgumentException(
                    "No property reference expression was found.",
                    "propertyExpression");
            }

            var attr = memberInfo.GetAttribute<DisplayNameAttribute>(false);
            if (attr == null)
            {
                return memberInfo.Name;
            }

            return attr.DisplayName;
        }

        public static string GetDisplayName(Type aType)
        {
            if (aType == null)
            {
                throw new ArgumentException(
                    "No property reference expression was found.",
                    "propertyExpression");
            }

            var attr = aType.GetAttribute<DisplayNameAttribute>(false);
            if (attr == null)
            {
                return aType.Name;
            }

            return attr.DisplayName;
        }

        public static MemberInfo GetPropertyInformation(Expression propertyExpression)
        {
            Debug.Assert(propertyExpression != null, "propertyExpression != null");
            MemberExpression memberExpr = propertyExpression as MemberExpression;
            if (memberExpr == null)
            {
                UnaryExpression unaryExpr = propertyExpression as UnaryExpression;
                if (unaryExpr != null && unaryExpr.NodeType == ExpressionType.Convert)
                {
                    memberExpr = unaryExpr.Operand as MemberExpression;
                }
            }
            if (memberExpr != null && memberExpr.Member.MemberType == MemberTypes.Property)
            {
                return memberExpr.Member;
            }
            return null;
        }

        /// <summary>
        /// 현재 도메인의 메인 어셈블리를 가져온다.
        /// </summary>
        /// <returns></returns>
        public static Assembly getCurrentMainAssembly()
        {
            string[] lAssemblyNameArr = AppDomain.CurrentDomain.BaseDirectory.Split('\\');
            string lAssemblyName = lAssemblyNameArr[lAssemblyNameArr.Count() - 2]; // 베이스디렉토리에서 마지막 디렉토리명(프로젝트명)을 가져온다.
            System.Reflection.Assembly lAssembly = AppDomain.CurrentDomain.GetAssemblies().Where(x => x.GetName().Name == lAssemblyName).FirstOrDefault();
            return lAssembly;
        }
    }

    public static class MemberInfoExpansion
    {
        public static T GetAttribute<T>(this MemberInfo member, bool isRequired) where T : Attribute
        {
            var attribute = member.GetCustomAttributes(typeof(T), false).SingleOrDefault();

            if (attribute == null && isRequired)
            {
                throw new ArgumentException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "The {0} attribute must be defined on member {1}",
                        typeof(T).Name,
                        member.Name));
            }

            return (T)attribute;
        }
    }
}
