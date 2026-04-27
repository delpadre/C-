using MySql.Data.MySqlClient;
using ProjetoLoja.Interfaces;
using ProjetoLoja.Models;

namespace ProjetoLoja.Repositorios
{
    public class ProdutoRepositorio : IProdutoRepositorio
    {
        private readonly string _connectionString;

        public ProdutoRepositorio(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("Conexao");
        }

        public void CadastrarProduto(ProdutoViewModel produto)
        {
            
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();
            var cmd = new MySqlCommand("INSERT INTO Produtos (Nome,Preco) VALUES(@nome, @preco)", conn);
            cmd.Parameters.AddWithValue("@nome", produto.Nome);
            cmd.Parameters.AddWithValue("@preco", produto.Preco);
            cmd.ExecuteNonQuery();
        }
        

        public IEnumerable<ProdutoViewModel> ListarTudo()
        {
            var lista = new List<ProdutoViewModel>();

            using var conn = new MySqlConnection(_connectionString);

            conn.Open();

            var cmd = new MySqlCommand("SELECT * FROM Produtos", conn);

            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                lista.Add(new ProdutoViewModel
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Nome = reader["Nome"].ToString()!,
                    Preco = Convert.ToDecimal(reader["Preco"]),
                });
            }
            return lista;

        }

        public ProdutoViewModel? ObterId(int id)
        {
            using var conn = new MySqlConnection(_connectionString);

            conn.Open();

            var cmd = new MySqlCommand("SELECT * FROM Produtos WHERE Id=@id", conn);
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return new ProdutoViewModel
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Nome = reader["Nome"].ToString()!,
                    Preco = Convert.ToDecimal(reader["Preco"]),
                };
            }
            return null;
        }

        public void EditarProduto(ProdutoViewModel prodmodel)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();
            //Comando para alterar os dados de um produto que já existe usando o ID dele como referência.
            var cmd = new MySqlCommand("UPDATE Produtos SET Nome = @nome,Preco = @preco WHERE Id= @id", conn);
            //Passa o Nome,Preço e que o usuário quer editar para o comando SQL.
            cmd.Parameters.AddWithValue("@nome", prodmodel.Nome);
            cmd.Parameters.AddWithValue("@preco", prodmodel.Preco);
            cmd.Parameters.AddWithValue("@id", prodmodel.Id);
            ////Executa o comando. Como é um cadastro (não estamos lendo nada), usamos "NonQuery" (não-consulta).
            cmd.ExecuteNonQuery();
        }
        public void ExcluirProduto(int id)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();
            //Comando para remover permanentemente a linha do produto no banco de dados.
            var cmd = new MySqlCommand("DELETE FROM Produtos WHERE Id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }

    }
}
