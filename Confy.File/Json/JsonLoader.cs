using System.IO;
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
                var parsed = JObject.Parse(reader.ReadToEnd());
                var token = parsed.SelectToken(section);
                var ob = token.ToObject<T>();
                return ob;
            }
        }
    }
}
