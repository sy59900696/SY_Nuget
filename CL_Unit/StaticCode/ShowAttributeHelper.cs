using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StaticCode
{
    /// <summary>
    /// 用作给datagridview显示时，控制是否显示及显示标题。
    /// </summary>
    public static class ShowAttributeHelper
    {
        /// <summary>
        /// Cache Data
        /// </summary>
        private static readonly Dictionary<string, string> Cache = new Dictionary<string, string>();

        /// <summary>
        /// 获取CustomAttribute Value
        /// </summary>
        /// <typeparam name="T">Attribute的子类型</typeparam>
        /// <param name="sourceType">头部标有CustomAttribute类的类型</param>
        /// <param name="attributeValueAction">取Attribute具体哪个属性值的匿名函数</param>
        /// <returns>返回Attribute的值，没有则返回null</returns>
        public static string GetCustomAttributeValue<T>(this Type sourceType, Func<T, string> attributeValueAction) where T : Attribute
        {
            return GetAttributeValue(sourceType, attributeValueAction, null);
        }

        /// <summary>
        /// 获取CustomAttribute Value
        /// </summary>
        /// <typeparam name="T">Attribute的子类型</typeparam>
        /// <param name="sourceType">头部标有CustomAttribute类的类型</param>
        /// <param name="attributeValueAction">取Attribute具体哪个属性值的匿名函数</param>
        /// <param name="name">field name或property name</param>
        /// <returns>返回Attribute的值，没有则返回null</returns>
        public static string GetCustomAttributeValue<T>(this Type sourceType, Func<T, string> attributeValueAction, string name) where T : Attribute
        {
            return GetAttributeValue(sourceType, attributeValueAction, name);
        }

        /// <summary>
        /// 没用到。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sourceType"></param>
        /// <param name="attributeValueAction"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Type GetCustomAttributeValue<T>(this Type sourceType, Func<T, Type> attributeValueAction, string name) where T : Attribute
        {
            var key = BuildKey(sourceType, name);
            //if (!Cache.ContainsKey(key))
            //{
            //    CacheAttributeValue(sourceType, attributeValueAction, name);
            //}

            var value = GetValue(sourceType, attributeValueAction, name);
            return value;
        }

        private static string GetAttributeValue<T>(Type sourceType, Func<T, string> attributeValueAction, string name) where T : Attribute
        {
            var key = BuildKey(sourceType, name);
            if (!Cache.ContainsKey(key))
            {
                CacheAttributeValue(sourceType, attributeValueAction, name);
            }

            return Cache[key];
        }

        /// <summary>
        /// 缓存Attribute Value
        /// </summary>
        private static void CacheAttributeValue<T>(Type type, Func<T, string> attributeValueAction, string name)
        {
            var key = BuildKey(type, name);

            var value = GetValue(type, attributeValueAction, name);

            lock (key + "_attributeValueLockKey")
            {
                if (!Cache.ContainsKey(key))
                {
                    Cache[key] = value;
                }
            }
        }

        private static string GetValue<T>(Type type, Func<T, string> attributeValueAction, string name)
        {
            object attribute = null;
            if (string.IsNullOrEmpty(name))
            {
                attribute =
                    type.GetCustomAttributes(typeof(T), false).FirstOrDefault();
            }
            else
            {
                var propertyInfo = type.GetProperty(name);
                if (propertyInfo != null)
                {
                    attribute =
                        propertyInfo.GetCustomAttributes(typeof(T), false).FirstOrDefault();
                }

                var fieldInfo = type.GetField(name);
                if (fieldInfo != null)
                {
                    attribute = fieldInfo.GetCustomAttributes(typeof(T), false).FirstOrDefault();
                }
            }

            return attribute == null ? null : attributeValueAction((T)attribute);
        }
         

        private static Type GetValue<T>(Type type, Func<T, Type> attributeValueAction, string name)
        {
            object attribute = null;
            if (string.IsNullOrEmpty(name))
            {
                attribute =
                    type.GetCustomAttributes(typeof(T), false).FirstOrDefault();
            }
            else
            {
                var propertyInfo = type.GetProperty(name);
                if (propertyInfo != null)
                {
                    attribute =
                        propertyInfo.GetCustomAttributes(typeof(T), false).FirstOrDefault();
                }

                var fieldInfo = type.GetField(name);
                if (fieldInfo != null)
                {
                    attribute = fieldInfo.GetCustomAttributes(typeof(T), false).FirstOrDefault();
                }
            }

            return attribute == null ? null : attributeValueAction((T)attribute);
        }

        /// <summary>
        /// 缓存Collection Name Key
        /// </summary>
        private static string BuildKey(Type type, string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return type.FullName;
            }

            return type.FullName + "." + name;
        }

    }

    /// <summary>
    /// 用作展示
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public sealed class ShowAttribute : Attribute
    {
        private readonly string _name;
        private Type _type;
        private string _sDisplay;
        private string _sPer;

        /// <summary>
        /// 如果在字段上标注特性,如：[Show("创建时间")]，用语句typeof(Opening)...(x => x.Name, "Createtime")可获取到【创建时间】
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// 备用
        /// </summary>
        public string Display
        {
            get { return _sDisplay; }
            set { _sDisplay = value; }
        }

        /// <summary>
        /// 展示时要缩减倍数。如此处为1000，则当实际输入为2100时，应该展示2.1
        /// </summary>
        public string sPer
        {
            get { return _sPer; }
            set { _sPer = value; }
        }

        /// <summary>
        /// 备用
        /// </summary>
        public Type MyType
        {
            get { return _type;  }
            set { _type = value; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ShowAttribute()
        {

        }

        /// <summary>
        /// 构造函数, 无用
        /// </summary>
        /// <param name="name"></param>
        public ShowAttribute(string name)
        {
            _name = name;
        } 
    }

}
