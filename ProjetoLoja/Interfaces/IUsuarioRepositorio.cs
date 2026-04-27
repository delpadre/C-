using ProjetoLoja.Models;

namespace ProjetoLoja.Interfaces
{
    public interface IUsuarioRepositorio
    {
        LoginViewModel Validar(string email, string senha);
        // Contrato para salvar um novo usuário no banco

        // contrato para cadastrar um novo usuario
        void CriarConta(LoginViewModel usuario);
    }
}
