using Microsoft.AspNetCore.Mvc;

namespace POTUSTwitterPlugin
{
    [Route("weather")]
    // The name of the Views folder must be WeatherWidget
    public class WeatherWidgetController : Controller
    {
        public IActionResult Index()
        {
            return View("Index");
        }
    }
}