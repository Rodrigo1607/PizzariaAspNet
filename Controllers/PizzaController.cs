﻿using Microsoft.AspNetCore.Mvc;
using Pizzaria1000Video.DTO;
using Pizzaria1000Video.Models;
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

        public async Task<IActionResult> Editar(int id)
        {
            var pizza = await _pizzaService.GetPizzasPorId(id);
            return View(pizza);
        }

        public async Task<IActionResult> Detalhes(int id)
        {
            var pizza = await _pizzaService.GetPizzasPorId(id);
            return View(pizza); 
        }

        [HttpPost]
        public async Task<IActionResult> Editar(PizzaModel pizzaModel, IFormFile? foto)
        {
            if (ModelState.IsValid)
            {
                var pizza = await _pizzaService.EditarPizza(pizzaModel, foto);
                return RedirectToAction("Index", "Pizza");
            }
            else
            {
                return View(pizzaModel);
            }
        }

        public async Task<IActionResult> Remover(int id)
        {

            var pizza = await _pizzaService.RemoverPizza(id);
            return RedirectToAction("Index", "Pizza");

        }

        public async Task<IActionResult> BuscarSugestoes(string termo)
        {
            if (string.IsNullOrEmpty(termo) || termo.Length < 3)
            {
                return Json(new List<PizzaModel>());
            }

            var pizzas = await _pizzaService.GetPizzasFiltro(termo);
            return Json(pizzas);
        }

    }
}
