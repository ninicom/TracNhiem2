using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TracNhiem2.Models;

namespace TracNhiem2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Singleplay()
        {
            return RedirectToAction("Index", "Game");
        }

        public IActionResult ChangeName()
        {
            return RedirectToAction("Index", "Player");
        }
        public IActionResult Multiplay()
        {
            return RedirectToAction("index", "RoomQuiz");
        }
        public IActionResult GameSingle()
        {
            return RedirectToAction("Index", "Game");
        }

        public IActionResult GameRandom()
        {
            return RedirectToAction("GameRandom", "Game");
        }
        public IActionResult ModeSingle()
        {
            return View();
        }


    }
}