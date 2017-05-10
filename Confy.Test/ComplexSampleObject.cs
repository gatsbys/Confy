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

    public class BeatCoreConfiguration
    {
        public List<LogstashDefinition> LogstashDefinitions { get; set; } = new List<LogstashDefinition>();
        public List<ESDefinition> ESDefinitions { get; set; } = new List<ESDefinition>();

    }

    public class LogstashDefinition
    {
        public string LogstashServer { get; set; }
        public int LogstashPort { get; set; }
        public string Type { get; set; }
        public string Index { get; set; }
    }

    public class ESDefinition
    {

        public string ElasticSearchServer { get; set; }
        public int ElasticSearchPort { get; set; }
        public string ElasticSearchUserName { get; set; }
        public string ElastisSearchPassword { get; set; }
        public string Index { get; set; }
        public string Type { get; set; }
    }
}
