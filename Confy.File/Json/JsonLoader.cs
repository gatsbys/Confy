using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Confy.Json
{
    public class JsonLoader
    {
        public static T ConvertFromJson<T>(string file)
        {
            var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using (StreamReader reader = new StreamReader(fileStream))
            {
                T ob = JsonConvert.DeserializeObject<T>(reader.ReadToEnd());
                return ob;
            }
        }

        public static T ConvertFromJson<T>(string file, string section)
        {
            var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using (StreamReader reader = new StreamReader(fileStream))
            {
                var text = reader.ReadToEnd();
                var parsed = JObject.Parse(text);
                var camaleonic = ApplyCamaleonTags(text, parsed);
                parsed = JObject.Parse(camaleonic);
                var token = parsed.SelectToken(section);
                var ob = token.ToObject<T>();
                return ob;
            }
        }

        private static string ApplyCamaleonTags(string source, JObject jObject)
        {
            Regex regex = new Regex("<cam>(.*)</cam>");
            var result = regex.Match(source);
            string camaleonValue = result.Groups[1].ToString();
            var camaleonPath = camaleonValue.Split(new[] { "->" }, StringSplitOptions.None);
            JToken value = jObject;
            if (camaleonPath.Any())
            {
                value = jObject[camaleonPath[0]];
            }
            else
            {
                return source;
            }
            camaleonPath = camaleonPath.Skip(1).ToArray();
            value = camaleonPath.Aggregate(value, (current, s1) => current[s1]);
            return regex.Replace(source, value.Value<string>());
        }
    }
}
