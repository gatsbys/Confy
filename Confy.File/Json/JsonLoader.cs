using System;
using System.CodeDom;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Confy.Json
{
    public class JsonLoader
    {
        public static T ConvertFromJson<T>(string file, string section = "")
        {
            var retries = 0;
            while (true)
            {
                try
                {
                    var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.None);
                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        var text = reader.ReadToEnd();
                        var parsed = JObject.Parse(text);
                        var camaleonic = ApplyCamaleonTagsIfNeeded(text, parsed);
                        parsed = JObject.Parse(camaleonic);
                        if (string.IsNullOrEmpty(section))
                        {
                            return parsed.ToObject<T>();
                        }
                        var token = parsed.SelectToken(section);
                        var ob = token.ToObject<T>();
                        return ob;
                    }
                }
                catch (Exception ex)
                {

                    Trace.TraceWarning("Unable to load config, retrying...");
                    if (retries > 5)
                        throw;
                    retries++;
                }
            }
        }

        private static string ApplyCamaleonTagsIfNeeded(string source, JObject jObject)
        {
            Regex regex = new Regex("<cam>(.*)</cam>");
            var result = regex.Match(source);
            string camaleonValue = result.Groups[1].ToString();
            var camaleonPath = GetCamaleonPath(camaleonValue);
            JToken value = ExtractJsonValue(jObject, camaleonPath);
            if (value == null) return source;
            return regex.Replace(source, value.Value<string>());
        }
        private static string[] GetCamaleonPath(string camaleonValue)
        {
            return camaleonValue.Split(new[] { "->" }, StringSplitOptions.None);
        }
        private static JToken ExtractJsonValue(JObject jObject, string[] camaleonPath)
        {

            JToken value = jObject;
            if (camaleonPath.Any(x => x != string.Empty))
            {
                value = jObject[camaleonPath[0]];
            }
            else
            {
                return null;
            }
            camaleonPath = camaleonPath.Skip(1).ToArray();
            foreach (var s in camaleonPath)
            {
                if (s.All(x => char.IsNumber(x)))
                {
                    var index = int.Parse(s);
                    var stepValue = ((JArray)value)[index];
                    value = stepValue;
                }
                else
                {
                    value = value[s];
                }
            }
            return value;
        }
    }
}
