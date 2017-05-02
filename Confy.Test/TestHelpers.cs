using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Confy.Test
{
    public class TestHelpers
    {
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
                                  'Age': '30',
                                  'CamaleonicSample' : '<cam>NO-SAMPLE->TimeStamp</cam>'
                                }
                              }
                            }
                            ";
            System.IO.File.WriteAllText(path, content);
        }
        public static void ModifyConfigBlockingMode()
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
                                  'Age': '30',
                                  'CamaleonicSample' : '<cam>NO-SAMPLE->TimeStamp</cam>'
                                }
                              }
                            }
                            ";
            var bytes = Encoding.UTF8.GetBytes(content);

            using (FileStream fs =
         System.IO.File.Open(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
            using (FileStream fs =
        System.IO.File.Open(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
            {
                Thread.Sleep(TimeSpan.FromSeconds(20));
            }


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
                                  'Age': '50',
                                  'CamaleonicSample' : '<cam>NO-SAMPLE->TimeStamp</cam>'
                                }
                              }
                            }
                            ";
            System.IO.File.WriteAllText(path, content);
        }
    }
}
