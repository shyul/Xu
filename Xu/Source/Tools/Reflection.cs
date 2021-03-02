/// ***************************************************************************
/// Shared Libraries and Quick Utilities
/// GPL 2001-2007, 2014-2021 Xu Li - me@xuli.us
/// 
/// Data Serializations and Reflection
/// 
/// Code Author: Xu Li
/// 
/// ***************************************************************************

using System;
using System.Linq;
using System.Reflection;

namespace Xu
{
    public static class ReflectionTool
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        /*
        public static (bool IsValid, T Result) GetAttribute<T>(this object value) where T : Attribute
        {
            MemberInfo[] memberInfo = value.GetType().GetMember(value.ToString());
            if (memberInfo.Length > 0)
            {
                T res = (T)Attribute.GetCustomAttribute(memberInfo[0], typeof(T));
                if (res != null) return (IsValid: true, Result: res);
            }
            return (IsValid: false, Result: null);
        }*/

        public static TAttribute GetAttribute<TAttribute>(this object value) where TAttribute : Attribute
        {
            MemberInfo[] memberInfo = value.GetType().GetMember(value.ToString()); // memberInfo.FirstOrDefault(m => m.DeclaringType == type).GetCustomAttributes(typeof(T), false);
            if (memberInfo.Length > 0)
            {
                return Attribute.GetCustomAttribute(memberInfo[0], typeof(TAttribute)) as TAttribute;
            }
            return null;
        }

        /*
        public static (bool IsValid, T Result) GetAttribute<T>(this PropertyInfo p) where T : Attribute
        {
            var attrs = p.GetCustomAttributes(true).Where(n => n.GetType() == typeof(T));
            if (attrs.Count() > 0)
                return (IsValid: true, Result: (T)attrs.First());
            else
                return (IsValid: false, Result: null);
        }*/

        public static TAttribute GetAttribute<TAttribute>(this PropertyInfo p) where TAttribute : Attribute
        {
            var attrs = p.GetCustomAttributes(true).Where(n => n.GetType() == typeof(TAttribute));
            if (attrs.Count() > 0)
                return attrs.First() as TAttribute;
            else
                return null;
        }


        public static TValue GetAttribute<TAttribute, TValue>(this Type type, Func<TAttribute, TValue> valueSelector) where TAttribute : Attribute
        {
            if (type.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault() is TAttribute att)
            {
                return valueSelector(att);
            }
            return default;
        }

        /// <summary>
        /// Get an object's name
        /// </summary>
        /// <param name="o"></param>
        /// <param name="IsFull"></param>
        /// <returns></returns>
        public static string GetObjectName(this object o, bool IsFull)
        {
            if (IsFull)
                return o.GetType().FullName;
            else
                return o.GetType().Name;
        }

        /// <summary>
        /// Get object by name from an asmembly
        /// </summary>
        /// <param name="asm"></param>
        /// <param name="TypeName"></param>
        /// <returns></returns>
        public static object GetObject(Assembly asm, string TypeName)
        {
            return asm.CreateInstance(TypeName);
        }

        /// <summary>
        /// Get object by name from current asmembly
        /// </summary>
        /// <param name="ObjectName"></param>
        /// <returns></returns>
        public static object GetObject(string ObjectName)
        {
            return Assembly.GetExecutingAssembly().CreateInstance(ObjectName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T MaxValue<T>() where T : IComparable, IEquatable<T>
        {
            return (T)typeof(T).GetField(nameof(MaxValue)).GetRawConstantValue();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T MinValue<T>() where T : IComparable, IEquatable<T>
        {
            return (T)typeof(T).GetField(nameof(MinValue)).GetRawConstantValue();
        }

        public static T ParseEnum<T>(this string value) where T : struct, IConvertible => (T)Enum.Parse(typeof(T), value.Trim());

        public static T[] ToArray<T>() where T : Enum => Enum.GetValues(typeof(T)) as T[];
    }
}
