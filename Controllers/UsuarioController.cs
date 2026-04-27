using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using ProjetoLoja.Interfaces;
using ProjetoLoja.Models;
using System.Security.Claims;

namespace ProjetoLoja.Controllers
{
    public class UsuarioController : Controller
    {
        //criar o acesso ao banco de dados de usuarios
        private readonly IUsuarioRepositorio _usuarioRepositorio;

        //construtor que prepara a estrutura do banco de dados
        public UsuarioController(IUsuarioRepositorio usuarioRepositorio)
        {
            //guarda as informações que criamos para ser usada depois
            _usuarioRepositorio = usuarioRepositorio;

        }

        [HttpGet]
        public IActionResult CriarConta() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CriarConta(LoginViewModel usuario)
        {
            if (ModelState.IsValid)
            {
                // O repositório já cuida da criptografia
                _usuarioRepositorio.CriarConta(usuario);
                return RedirectToAction("Login");
            }
            return View(usuario);
        }



        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult>Login(LoginViewModel usermodel)
        {
            //verifica se o email e senha foram digitados corretamente
            if(!ModelState.IsValid) return View(usermodel);

            //Pergunta ao banco de dados se existe alguem com email e senha
            var usuario = _usuarioRepositorio.Validar(usermodel.Email, usermodel.Senha);

            if(usuario != null)
            {
                //dados do cracha de identificação
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, usuario.Nome),
                    new Claim(ClaimTypes.Email, usuario.Email),
                    new Claim("NivelAcesso", usuario.Nivel),
                    new Claim("UsuarioId", usuario.Id.ToString())
                };
                // cria a identidade oficial do usuario com base nos dados acima
                var identidade = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                //espera para logar criando o cookie de segurança no navegador
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(identidade),
                    //indica que a sessão se encerra ao fechar o navegador
                    new AuthenticationProperties { IsPersistent = false });
            }
            // Se chegou aqui, é porque o usuário é nulo (Login falhou)
            ModelState.AddModelError(string.Empty, "E-mail ou senha inválidos.");
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> Sair()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }

    }
}
