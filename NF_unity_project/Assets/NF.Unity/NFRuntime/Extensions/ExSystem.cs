using System;
using UnityEngine;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine.Assertions;
using System.Linq;

namespace NFRuntime.Extensions
{
    public static class ExSystem
    {
        private static readonly System.Random Rand = new System.Random();

        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        public static string ToUnixPath(this string fpath)
        {
            // "a\\b.txt" => "a/b.txt"
            return fpath.Replace('\\', '/');
        }

        public static string ToAssetPath(this string fpath)
        {
            // "C:/UnityProj/Assets/a.txt" => "Assets/a.txt";
            return ("Assets/" + fpath.ToUnixPath().Substring(Application.dataPath.ToUnixPath().Length + 1)).ToUnixPath();
        }

        public static string ToBundleName(this string fpath, string root_dir)
        {
            return fpath.ToUnixPath().Substring(root_dir.ToUnixPath().Length + 1).ToUnixPath();
        }

        public static T ToEnum<T>(this string value)
        {
            try
            {
                return (T)Enum.Parse(typeof(T), value, ignoreCase: false);
            }
            catch
            {
                return default(T);
            }
        }

        public static Type GetMemberType(this MemberInfo info)
        {
            switch (info.MemberType)
            {
                case MemberTypes.Event:
                    var e = info as EventInfo;
                    return e.EventHandlerType;

                case MemberTypes.Field:
                    var f = info as FieldInfo;
                    return f.FieldType;

                case MemberTypes.Method:
                    var m = info as MethodInfo;
                    return m.ReturnType;

                case MemberTypes.Property:
                    var p = info as PropertyInfo;
                    return p.PropertyType;

                case MemberTypes.Constructor:
                case MemberTypes.TypeInfo:
                case MemberTypes.Custom:
                case MemberTypes.NestedType:
                case MemberTypes.All:
                default:
                    return null;
            }
        }

        public static void SetValue(this MemberInfo info, object obj, object value)
        {
            switch (info.MemberType)
            {
                case MemberTypes.Field:
                    var f = (info as FieldInfo);
                    f.SetValue(obj, value);
                    break;

                case MemberTypes.Property:
                    var p = (info as PropertyInfo);
                    p.SetValue(obj, value, null);
                    break;

                case MemberTypes.Constructor:
                case MemberTypes.Method:
                case MemberTypes.Event:
                case MemberTypes.TypeInfo:
                case MemberTypes.Custom:
                case MemberTypes.NestedType:
                case MemberTypes.All:
                default:
                    break;
            }
        }

        public static bool HasFlag(this Enum self, Enum flag)
        {
            var selfValue = Convert.ToInt32(self);
            var flagValue = Convert.ToInt32(flag);

            return (selfValue & flagValue) == flagValue;
        }

        public static IEnumerable<T> Randomize<T>(this IEnumerable<T> source)
        {
            return Randomize(source, Rand);
        }

        public static IEnumerable<T> Randomize<T>(this IEnumerable<T> source, System.Random rand)
        {
            return source.OrderBy(item => rand.Next());
        }

        public static IList<List<T>> SplitList<T>(this IList<T> listToSplit, int countToTake)
        {
            IList<List<T>> splitList = new List<List<T>>();
            var countToSkip = 0;

            do
            {
                splitList.Add(listToSplit.Skip(countToSkip).Take(countToTake).ToList());
                countToSkip += countToTake;
            }
            while (countToSkip < listToSplit.Count);

            return splitList;
        }

        public static List<T> ShiftListRight<T>(this List<T> lst, int count)
        {
            var ret = new List<T>();
            ret.AddRange(lst.Skip(count).Take(lst.Count - count));
            ret.AddRange(lst.Take(count));
            Assert.AreEqual(lst.Count, ret.Count, $"count is equal {lst.Count} | {ret.Count}");

            return ret;
        }

        public static void ForEachWithIndex<T>(this IEnumerable<T> enumerable, Action<T, int> handler)
        {
            var idx = 0;
            foreach (var item in enumerable)
            {
                handler(item, idx++);
            }
        }
    }
}