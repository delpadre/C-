using ProjetoLoja.Models;

namespace ProjetoLoja.Interfaces
{
    public interface IProdutoRepositorio
    {
        //Promete que haverá uma função para devolver uma lista de todos os produtos cadastrados.
        IEnumerable<ProdutoViewModel> ListarTudo();

        //? significa que ela pode devolver o produto ou "nada" (nulo), caso ele não exista.
        ProdutoViewModel? ObterId(int id);

        void CadastrarProduto(ProdutoViewModel produto);

        void EditarProduto(ProdutoViewModel produto);

        void ExcluirProduto(int id);
    }
}
