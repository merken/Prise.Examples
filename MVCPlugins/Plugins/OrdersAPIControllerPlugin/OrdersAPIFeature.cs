using Contract;
using Prise.Plugin;

namespace OrdersAPIControllerPlugin
{
    [Plugin(PluginType =typeof(IMVCFeature))]
    [MVCFeatureDescription(Description = "This feature will add the 'api/orders' API Controller to the current MVC Host. This controller retrieves orders data from Azure Table Storage.")]
    public class OrdersAPIFeature : IMVCFeature
    {
    }
}
