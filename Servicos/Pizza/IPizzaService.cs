using Pizzaria1000Video.DTO;
using Pizzaria1000Video.Models;

namespace Pizzaria1000Video.Servicos.Pizza
{
    public interface IPizzaService
    {
        Task <PizzaModel> CriarPizza (PizzaCriacaoDTO pizzaCriacaoDTO, IFormFile foto);
        Task<List<PizzaModel>> GetPizzas();
        Task<PizzaModel> GetPizzasPorId(int id);
        Task<PizzaModel> EditarPizza(PizzaModel pizza, IFormFile? foto);

    }
}
