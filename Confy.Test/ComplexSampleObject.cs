using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Confy.Test
{
    [Serializable]
    public class ComplexSampleObject
    {
        public string ComplexFirstLevelName { get; set; }
        public DateTime TimeStamp { get; set; }
        public SampleSimpleObject SampleSimpleObject { get; set; }
    }
}
