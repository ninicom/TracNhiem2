using Microsoft.AspNetCore.Mvc;

namespace TracNhiem2.Controllers
{
    public class GameController : Controller
    {
        public IActionResult GameStart()
        {
            return View();
        }
        public IActionResult GameRandom()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
