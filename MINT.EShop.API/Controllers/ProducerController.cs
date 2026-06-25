using Microsoft.AspNetCore.Mvc;

namespace MINT.EShop.API.Controllers
{
    public class ProducerController(ICategoryService producerService, ILogger<ProducerController> logger) : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
