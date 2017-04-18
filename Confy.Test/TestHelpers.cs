using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Confy.Test
{
    public class TestHelpers
    {
        public static T DeepClone<T>(T obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
            }
        }

        public static void ModifyConfig()
        {
            var path = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory())) +
                       @"\Config_ComplexSectionConfig.json";
            var content = @"{
                              'SAMPLE': {

                              },
                              'NO-SAMPLE' : {
                                'ComplexFirstLevelName': 'New Complex Name',
                                'TimeStamp': '10/04/2017 05:20:00',
                                'SampleSimpleObject': {
                                  'Name': 'New Second Level Name',
                                  'Age': '30'
                                }
                              }
                            }
                            ";
            System.IO.File.WriteAllText(path,content);
        }

        public static void SetConfigAsDef()
        {
            var path = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory())) +
                       @"\Config_ComplexSectionConfig.json";
            var content = @"{
                              'SAMPLE': {

                              },
                              'NO-SAMPLE' : {
                                'ComplexFirstLevelName': 'Complex Name',
                                'TimeStamp': '10/04/2016 05:20:00',
                                'SampleSimpleObject': {
                                  'Name': 'Second Level Name',
                                  'Age': '50'
                                }
                              }
                            }
                            ";
            System.IO.File.WriteAllText(path, content);
        }
    }
}
