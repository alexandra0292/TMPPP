using DrugShoppingCartMvcUI.Models;
using DrugShoppingCartMvcUI.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DrugShoppingCartMvcUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHomeRepository _homeRepository;

        public HomeController(ILogger<HomeController> logger, IHomeRepository homeRepository)
        {
            _homeRepository = homeRepository;
            _logger = logger;
        }

        public async Task<IActionResult> Index(string sterm="",int genreId=0)
        {
            IEnumerable<Drug> drugs = await _homeRepository.GetDrugs(sterm, genreId);
            IEnumerable<Genre> genres = await _homeRepository.Genres();
            DrugDisplayModel drugModel = new DrugDisplayModel
            {
              Drugs=drugs,
              Genres=genres,
              STerm=sterm,
              CategoryId=genreId
            };
            return View(drugModel);
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
    }
}