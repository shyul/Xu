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
using System.Collections.Generic;
using System.Linq;

namespace Xu
{
    public static class CollectionTool
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">Key's type</typeparam>
        /// <typeparam name="T2">Value't type</typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool CheckAdd<T, T2>(this IDictionary<T, T2> dict, T key, T2 value)
        {
            lock (dict)
                if (!dict.ContainsKey(key))
                {
                    dict.Add(key, value);
                    return true;
                }
                else
                    return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static bool CheckAdd<T>(this ICollection<T> list, T item)
        {
            lock (list)
                if (!list.Contains(item))
                {
                    list.Add(item);
                    return true;
                }
                else
                    return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="list2"></param>
        /// <returns></returns>
        public static bool CheckAdd<T>(this ICollection<T> list, IEnumerable<T> list2)
        {
            bool hasAdded = false;

            lock (list)
                lock (list2)
                    foreach (T item in list2)
                    {
                        if (!list.Contains(item))
                        {
                            list.Add(item);
                            hasAdded = true;
                        }
                    }

            return hasAdded;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static bool CheckRemove<T>(this ICollection<T> list, T item)
        {
            lock (list)
                if (list.Contains(item))
                {
                    list.Remove(item);
                    return true;
                }
                else
                    return false;
        }

        /// <summary>
        /// Refer 1: https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/generics/constraints-on-type-parameters
        /// where T : IDependable where T2 : IDependable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="parent"></param>
        /// <param name="child"></param>
        /// <param name="recursive"></param>
        /// <returns></returns>
        public static bool CheckRemove<T>(this T parent, T child, bool recursive = false) where T : IDependable
        {
            if (parent.Children.Contains(child))
                lock (parent)
                {
                    if (recursive)
                        foreach (var grandchild in child.Children)
                            child.CheckRemove(grandchild, true);

                    if (recursive || child.Children.Count == 0)
                    {
                        parent.Children.Remove(child);
                        child = default;
                        return true;
                    }
                }

            return false;
        }

        /// <summary>
        /// Recursively get all members of an IDependable object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="current"></param>
        /// <param name="child"></param>
        /// <returns></returns>
        public static List<IDependable> GetMembers(this IDependable current)
        {
            List<IDependable> members = new() { current };
            foreach (var child in current.Children)
            {
                members.CheckAdd(child);
                members.CheckAdd(child.GetMembers());
            }
            return members;
        }

        public static List<IDependable> GetChildren(this IDependable current)
        {
            List<IDependable> children = new();
            foreach (var child in current.Children)
            {
                children.CheckAdd(child);
                children.CheckAdd(child.GetChildren());
            }
            return children;
        }

        public static List<IDependable> GetParents(this IDependable current)
        {
            List<IDependable> parents = new();
            foreach (var parent in current.Parents)
            {
                parents.CheckAdd(parent);
                parents.CheckAdd(parent.GetParents());
            }
            return parents;
        }

        public static void AddChild(this IDependable current, IDependable newChild)
        {
            current.Children.CheckAdd(newChild);
            newChild.Parents.CheckAdd(current);
        }

        public static void RemoveChild(this IDependable current, IDependable oldChild)
        {
            current.Children.CheckRemove(oldChild);
            oldChild.Parents.CheckAdd(current);
        }

        public static bool CheckRemove<T>(this T parent, bool recursive = false) where T : IDependable
        {
            foreach (var child in parent.Children)
                parent.CheckRemove(child, recursive);

            return (parent.Children.Count == 0);
        }

        public static IEnumerable<T> SelectType<T, T2>(this IEnumerable<T2> source) where T : class
        {
            return source.Where(n => n is T).Select(n => n as T);
        }

        public static List<(T, T)> SelectPair<T>(this IEnumerable<T> source) where T : IEquatable<T>
        {
            List<(T, T)> res = new();

            for (int i = 0; i < source.Count(); i++)
            {
                source.Skip(i + 1).Select(n => (source.ElementAt(i), n)).ToList().ForEach(n => { if (!res.Contains(n) && !n.Item1.Equals(n.Item2)) res.Add(n); });
            }

            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <param name="value"></param>
        /// <param name="sep"></param>
        public static void FromString(this ICollection<string> list, string value, char sep = Xu.TextTool.ValueSeparator)
        {
            string[] res = value.Trim().Split(sep);

            list.Clear();
            if (res.Length > 0)
            {
                foreach (string s in res)
                {
                    if (!list.Contains(s)) list.Add(s);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="sep"></param>
        /// <returns></returns>
        public static string ToString<T>(this IEnumerable<T> list, char sep = Xu.TextTool.ValueSeparator) => ToString<T>(list, new string(new char[] { sep }));

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="sep"></param>
        /// <returns></returns>
        public static string ToString<T>(this IEnumerable<T> list, string sep) => string.Join(sep, list);

        public static string ToStringWithIndex<T>(this ICollection<T> list)
        {
            string s = string.Empty;
            for (int i = 0; i < list.Count; i++)
            {
                s += "(" + i.ToString() + ")\"" + list.ElementAt(i) + "\"-";
            }

            if (s.Length > 0) return s.TrimEnd('-');
            else return "Empty String Array";
        }

        public static T[] SubArray<T>(this T[] data, int index, int length)
        {
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }
    }
}
