using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace atomapp.api.Infrastructure
{
    public class AppSettings
    {
        public string VoskPath { get; set; }
        public string TempPath { get; set; }
        public string GrammarsPath { get; set; }
        public string ExamplesPath { get; set; }
    }
}
