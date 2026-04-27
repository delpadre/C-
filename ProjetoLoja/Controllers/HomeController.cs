using Microsoft.AspNetCore.Mvc;
using ProjetoLoja.Interfaces;
using ProjetoLoja.Models;
using ProjetoLoja.Repositorios;
using System.Diagnostics;

namespace ProjetoLoja.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProdutoRepositorio _produtoRepositorio;

        public HomeController(ILogger<HomeController> logger, IProdutoRepositorio produtoRepositorio)
        {
            _logger = logger;
            _produtoRepositorio = produtoRepositorio;
        }

        public IActionResult Index()
        {
            // Busca todos os produtos do banco
            var produtos = _produtoRepositorio.ListarTudo();

            // Envia a lista para a View
            return View(produtos);
        }

   

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
