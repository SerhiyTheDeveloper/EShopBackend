using Microsoft.AspNetCore.Mvc;

namespace MINT.EShop.API.Controllers
{
    public class CategoryController(ICategoryService categoryService, ILogger<CategoryController> logger) : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
