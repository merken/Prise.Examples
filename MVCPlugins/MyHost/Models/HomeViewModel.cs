using System.Collections.Generic;

namespace MyHost.Models
{
    public class Feature
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsEnabled { get; set; }
    }

    public class HomeViewModel
    {
        public IEnumerable<Feature> Features { get; set; }
    }
}
