using Contract;
using Prise.Plugin;

namespace ProductsAPIControllerPlugin
{
    [Plugin(PluginType = typeof(IMVCFeature))]
    [MVCFeatureDescription(Description = "This feature will add the 'api/products' API Controller to the current MVC Host. This API gets data from SQL Server via EF Core.")]
    public class ProductsControllerFeature : IMVCFeature
    {
    }
}
