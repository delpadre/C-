using MySql.Data.MySqlClient;
using ProjetoLoja.Interfaces;
using ProjetoLoja.Models;

namespace ProjetoLoja.Repositorio
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        //Variavel que guada o endereço para entrar no banco de dados
        private readonly string _connectionString;

        public UsuarioRepositorio(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("Conexao");
        }

        public void CriarConta(LoginViewModel usuario)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                /// Criptogrando senha antes de enviar para o banco de dados 
                string senhahash = BCrypt.Net.BCrypt.HashPassword(usuario.Senha);

                var sql = "INSERT INTO Usuarios (Nome, Email, Senha, Nivel) VALUES (@nome, @email, @senha, @nivel)";

                var cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@nome", usuario.Nome);
                cmd.Parameters.AddWithValue("@email", usuario.Email);
                cmd.Parameters.AddWithValue("@senha", senhahash);
                cmd.Parameters.AddWithValue("@nivel","Operador");

                cmd.ExecuteNonQuery();

            }
            
        }


        public LoginViewModel? Validar(string email, string senha)
        {
            using var con = new MySqlConnection(_connectionString);
            con.Open();
            var cmd = new MySqlCommand("SELECT * FROM Usuarios WHERE Email =@email",con);
            cmd.Parameters.AddWithValue("@email", email);

            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                string senhaDoBanco = reader["Senha"].ToString() ?? "";

                // Verifica se a senha digitada bate com o Hash do banco
                if (BCrypt.Net.BCrypt.Verify(senha, senhaDoBanco))
                {
                    return new LoginViewModel
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Nome = reader["Nome"].ToString()!,
                        Email = reader["Email"].ToString()!,
                        Nivel = reader["Nivel"].ToString()!,
                    };
                }

            }
            return null;
        }
    }
}
