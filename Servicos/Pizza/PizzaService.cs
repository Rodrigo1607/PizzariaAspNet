using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Pizzaria1000Video.Data;
using Pizzaria1000Video.DTO;
using Pizzaria1000Video.Models;

namespace Pizzaria1000Video.Servicos.Pizza
{
    public class PizzaService : IPizzaService
    {
        private readonly AppDbContext _context;
        private readonly string _sistema;

        //ter acesso ao wwwroot
        public PizzaService(AppDbContext context, IWebHostEnvironment sistema)
        {
            _context = context;
            _sistema = sistema.WebRootPath;
        }



        public string GeraCaminhoArquivo(IFormFile foto)
        {                       //Guid retorna uma junção decaracteres únicos
            var codigounico = Guid.NewGuid().ToString();
            //nomear aquivo                             s/espaço - Minúsculo                 extensão.                                 
            var nomeCaminhoImagem = foto.FileName.Replace(" ", "").ToLower() + codigounico + ".png";
            //criar o caminho onde vai salvar
            var caminhoParaSalvarImagem = _sistema + "\\imagem\\";

            if (!Directory.Exists(caminhoParaSalvarImagem))
            {
                Directory.CreateDirectory(caminhoParaSalvarImagem);

            }
            using (var stream = File.Create(caminhoParaSalvarImagem + nomeCaminhoImagem))
            {
                foto.CopyToAsync(stream).Wait();
            }
            return nomeCaminhoImagem;
        }
        public async Task<PizzaModel> CriarPizza(PizzaCriacaoDTO pizzaCriacaoDTO, IFormFile foto)
        {
            try
            {
                var nomeCaminhoImagem = GeraCaminhoArquivo(foto);
                var pizza = new PizzaModel
                {
                    Sabor = pizzaCriacaoDTO.Sabor,
                    Descricao = pizzaCriacaoDTO.Descricao,
                    Valor = pizzaCriacaoDTO.Valor,
                    Capa = nomeCaminhoImagem
                };
                _context.Add(pizza);
                await _context.SaveChangesAsync();

                return pizza;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<PizzaModel>> GetPizzas()
        {
            return await _context.Pizzas.ToListAsync();
        }

        public async Task<PizzaModel> GetPizzasPorId(int id)
        {
            try
            {
                return await _context.Pizzas.FirstOrDefaultAsync(pizza => pizza.Id == id);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PizzaModel> EditarPizza(PizzaModel pizza, IFormFile? foto)
        {
            try
            {
                var pizzaBanco = await _context.Pizzas.AsNoTracking().FirstOrDefaultAsync(pizzaBD => pizzaBD.Id == pizza.Id);
                var nomeCaminhoImagem = "";

                if (foto != null)
                {
                    string caminhoCapaExistente = _sistema + "\\imagem\\" + pizzaBanco.Capa;
                    if (File.Exists(caminhoCapaExistente))
                    {
                        File.Delete(caminhoCapaExistente);
                    }

                    nomeCaminhoImagem = GeraCaminhoArquivo(foto);
                }
                pizzaBanco.Sabor = pizza.Sabor;
                pizzaBanco.Descricao = pizza.Descricao;
                pizzaBanco.Valor = pizza.Valor;

                if (nomeCaminhoImagem != "")
                {
                    pizzaBanco.Capa = nomeCaminhoImagem;
                }
                else
                {
                    pizzaBanco.Capa = pizza.Capa;
                }


                _context.Update(pizzaBanco);
                await _context.SaveChangesAsync();

                return pizza;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PizzaModel>RemoverPizza(int id)
        {
            try
            {
                var pizza = await _context.Pizzas.FirstOrDefaultAsync(pizzaBanco => pizzaBanco.Id == id);
                 _context.Remove(pizza);
                await _context.SaveChangesAsync();
                return pizza;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<PizzaModel>> GetPizzasFiltro(string? pesquisar)
        {
            try
            {
                var pizza = await _context.Pizzas.Where(pizzaBanco => pizzaBanco.Sabor.Contains(pesquisar)).ToListAsync();
                return pizza;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
