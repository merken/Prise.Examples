using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyHost.Models
{
    public class LanguageInfo
    {
        public string Name { get; set; }
        public string Culture { get; set; }
        public string Description { get; set; }
        public LanguageInfo[] Dialects { get; set; }
    }
}
