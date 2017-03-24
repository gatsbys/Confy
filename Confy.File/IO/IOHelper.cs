using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Confy.File.IO
{
    public class IOHelper
    {
        public static DateTime GetLastWriteDateUtc(string file)
        {
            return System.IO.File.GetLastWriteTimeUtc(file);
        }
    }
}
