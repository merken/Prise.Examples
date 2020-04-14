using System.Collections.Generic;

namespace MyHost2.Models
{
    public class Feature
    {
        public string Name { get; set; }
        public bool IsEnabled { get; set; }
    }

    public class HomeViewModel
    {
        public IEnumerable<Feature> Features { get; set; }
    }
}
