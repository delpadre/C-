using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjetoLoja.Interfaces;
using ProjetoLoja.Models;

namespace ProjetoLoja.Controllers
{
    public class ProdutoController : Controller
    {
        private readonly IProdutoRepositorio _produtoRepositorio;

        public ProdutoController(IProdutoRepositorio produtoRepositorio)
        {
            _produtoRepositorio = produtoRepositorio;
        }


        public IActionResult Index()
        {
            var produtos = _produtoRepositorio.ListarTudo();
            return View(produtos);
        }

        [HttpGet]
        public IActionResult CadastrarProduto()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CadastrarProduto(ProdutoViewModel prodmodel)
        {
            if (!ModelState.IsValid) return View(prodmodel);

            var produto = new ProdutoViewModel
            {
                Nome = prodmodel.Nome,
                Preco = prodmodel.Preco,
            };

            _produtoRepositorio.CadastrarProduto(produto);
            return RedirectToAction(nameof(Index));
        }

        //Abre a página de edição buscando o produto pelo número de identificação (ID).
        [HttpGet]
        public IActionResult EditarProduto(int id)
        {
            //Busca no banco: "Cadê o produto número X?".
            var produto = _produtoRepositorio.ObterId(id);
            //Se o produto não existir, mostra erro 404.
            if (produto == null) return NotFound();

            //Cria um novo objeto de produto com o Nome e Preço que você editou.
            var viewModel = new ProdutoViewModel
            {
                Id = produto.Id,
                Nome = produto.Nome,
                Preco = produto.Preco,
            };
            //Envia os dados do produto para preencher o formulário na tela.
            return View(viewModel);
        }

        [HttpPost]
        //Só permite salvar a alteração se o usuário estiver logado.
        [Authorize]
        //datanotation-Um selo de segurança para evitar que hackers enviem dados falsos.
        [ValidateAntiForgeryToken]
        public IActionResult EditarProduto(int id, ProdutoViewModel prodmodel)
        {
            // Garante que o ID da URL é o mesmo do produto que está sendo salvo
            if (id != prodmodel.Id) return BadRequest();

            ////Se você esqueceu de preencher algo obrigatório, ele para e mostra os erros.
            if (ModelState.IsValid)
            {
                //Cria um novo objeto de produto com o Nome e Preço que você editou.
                var produto = new ProdutoViewModel
                {
                    Id = prodmodel.Id,
                    Nome = prodmodel.Nome,
                    Preco = prodmodel.Preco,
                };
                //Avisa o banco: "Atualize as informações desse produto".
                _produtoRepositorio.EditarProduto(produto);
            }
            //Redireciona o usuário de volta para a lista (Index) após salvar
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        //Só permite salvar a alteração se o usuário estiver logado.
        [Authorize]
        //datanotation-Um selo de segurança para evitar que hackers enviem dados falsos.
        [ValidateAntiForgeryToken]

        //Recebe o ID do produto que deve ser apagado.
        public IActionResult ExcluirProduto(int id)
        {
            //Diz ao banco: "Pode apagar esse registro".
            _produtoRepositorio.ExcluirProduto(id);
            //manda de volta para a lista atualizada.
            return RedirectToAction(nameof(Index));
        }
    }
}
