using Microsoft.AspNetCore.Mvc;
using Pizzaria1000Video.DTO;
using Pizzaria1000Video.Servicos.Pizza;

namespace Pizzaria1000Video.Controllers
{
    public class PizzaController : Controller
    {
        private readonly IPizzaService _pizzaService;
        public PizzaController(IPizzaService pizzaService)
        {
            _pizzaService = pizzaService;
        }
        public async Task<IActionResult> Index()
        {
            var pizzas = await _pizzaService.GetPizzas(); 
            return View(pizzas);
        }

        public IActionResult Cadastrar()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Cadastrar(PizzaCriacaoDTO pizzaCriacaoDTO, IFormFile foto)
        {
            if (ModelState.IsValid)
            {
                var pizza = await _pizzaService.CriarPizza(pizzaCriacaoDTO, foto);
                return RedirectToAction("Index", "Pizza");
            }
            else
            {
                return View(pizzaCriacaoDTO);
            }
        }


    }
}
