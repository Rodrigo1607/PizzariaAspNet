using Microsoft.AspNetCore.Mvc;
using Pizzaria1000Video.Servicos.Pizza;

namespace Pizzaria1000Video.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPizzaService _pizzaService;
        public HomeController(IPizzaService pizzaService)
        {
            _pizzaService = pizzaService;
        }

        public async Task<IActionResult> Index(string? pesquisar)
        {

            if (pesquisar == null)
            {
                var pizzas = await _pizzaService.GetPizzas();
                return View(pizzas);
            }
            else
            {
                var pizzas = await _pizzaService.GetPizzasFiltro(pesquisar);
                return View(pizzas);
            }
          
        }

       

        
    }
}
