using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace MicroS_Common
{
    public static class Extensions
    {
        public static string Underscore(this string value)
            => string.Concat(value.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString()));

        public static TModel GetOptions<TModel>(this IConfiguration configuration, string section) where TModel : new()
        {
            var model = new TModel();
            configuration.GetSection(section).Bind(model);

            return model;
        }
        public static bool TryGetOptions<TModel>(this IConfiguration configuration, string section,out TModel model) where TModel : new()
        {
            try
            {
                model = new TModel();
                configuration.GetSection(section).Bind(model);

                return true;
            }
            catch
            {
                model =default(TModel);
                return false;
            }
        }

        /// <summary>
        /// Deserialize an Xml string into Object T
        /// </summary>
        /// <typeparam name="T">Type Param</typeparam>
        /// <param name="input">The string to deserialize</param>
        /// <returns></returns>
        public static T Deserialize<T>(this string input) where T : class
        {
            XmlSerializer ser = new XmlSerializer(typeof(T));

            using StringReader sr = new StringReader(input);
            return (T)ser.Deserialize(sr);
        }

        /// <summary>
        /// Deserialize an Xml string into Object T
        /// </summary>
        /// <typeparam name="T">Type Param</typeparam>
        /// <param name="input">The stream to deserialize</param>
        /// <returns></returns>
        public static T Deserialize<T>(this Stream input) where T : class
        {
            XmlSerializer ser = new XmlSerializer(typeof(T));
            return (T)ser.Deserialize(input);

        }

        /// <summary>
        /// Deserialize an Xml string into Object T
        /// </summary>
        /// <typeparam name="T">Type Param</typeparam>
        /// <param name="input">The bytes to deserialize</param>
        /// <returns></returns>
        public static T Deserialize<T>(this byte[] input) where T : class
        {
            try
            {
                XmlSerializer ser = new XmlSerializer(typeof(T));
                using MemoryStream ms = new MemoryStream(input);
                return (T)ser.Deserialize(ms);
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public static string GetEnvironmentVariableValue(this string key)
        {
            var value = Environment.GetEnvironmentVariable(key);

            return string.IsNullOrWhiteSpace(value) ? null : value;
        }
    }
}
