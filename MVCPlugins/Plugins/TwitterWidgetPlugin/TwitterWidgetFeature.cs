using Contract;
using Prise.Plugin;

namespace TwitterWidgetPlugin
{
    [Plugin(PluginType = typeof(IMVCFeature))]
    [MVCFeatureDescription(Description = "This feature will add the '/twitter' widget to the current MVC Host.")]
    public class TwitterWidgetFeature : IMVCFeature
    {
        // Nothing to do here, just some feature discovery happening...
    }
}

