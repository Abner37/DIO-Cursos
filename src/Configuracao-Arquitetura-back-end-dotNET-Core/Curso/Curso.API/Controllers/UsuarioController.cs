using Curso.API.Business.Entities;
using Curso.API.Business.Repositories;
using Curso.API.Configurations;
using Curso.API.Filters;
using Curso.API.Models;
using Curso.API.Models.Usuarios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Swashbuckle.AspNetCore.Annotations;
using System.Linq;
using System.Security.Claims;

namespace Curso.API.Controllers
{
    [Route("api/v1/usuario")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IConfiguration _configuration;
        private readonly IAuthenticationService _authentication;


        public UsuarioController(
            IUsuarioRepository usuarioRepository,
            IConfiguration configuration,
            IAuthenticationService authentication)
        {
            _usuarioRepository = usuarioRepository;
            _configuration = configuration;
            _authentication = authentication;
        }


        /// <summary>
        /// Este serviço permite se logar na API, para realizar requisições que necessitem de autenticação.
        /// </summary>
        /// <returns>Retorna status ok, dados do usuário e token para autenticação.</returns>
        [HttpPost]
        [Route("logar")]
        [ValidacaoModelStateCustomizado]
        [SwaggerResponse(statusCode: 200, description: "Sucesso ao autenticar.", type: typeof(UsuarioViewModelOutput))]
        [SwaggerResponse(statusCode: 400, description: "Campos obrigatórios.", type: typeof(ValidaCampoViewModelOutput))]
        [SwaggerResponse(statusCode: 404, description: "Usuário não encontrado.", type: typeof(ErroGenericoViewModel))]
        [SwaggerResponse(statusCode: 500, description: "Erro interno.", type: typeof(ErroGenericoViewModel))]
        public IActionResult Logar(LoginViewModelInput input)
        {
            var usuario = _usuarioRepository.GetUsuario(input.Login, input.Senha);

            if (usuario == null)
            {
                return NotFound(new ErroGenericoViewModel { Mensagem = "Usuário não encontrado! Login ou senha incorretos!" });
            }

            var output = new UsuarioViewModelOutput()
            {
                Codigo = usuario.Codigo,
                Login = usuario.Login,
                Email = usuario.Email
            };

            var token = _authentication.TokenGenerate(output);

            return Ok(new 
            {
                Token = token,
                Usuario = output
            });
        }


        /// <summary>
        /// Este serviço permite se cadastrar na API, para efetuar login e gerenciar cursos.
        /// </summary>
        /// <returns>Retorna status created e dados do usuário.</returns>
        [HttpPost]
        [Route("registrar")]
        [ValidacaoModelStateCustomizado]
        [SwaggerResponse(201, "Sucesso no cadastro.", typeof(UsuarioViewModelOutput))]
        [SwaggerResponse(400, "Campos obrigatórios.", typeof(ValidaCampoViewModelOutput))]
        [SwaggerResponse(500, "Erro interno.", typeof(ErroGenericoViewModel))]
        public IActionResult Cadastrar(RegistroViewModelInput input)
        {
            var usuario = new Usuario
            {
                Login = input.Login,
                Email = input.Email,
                Senha = input.Senha
            };

            _usuarioRepository.Adicionar(usuario);
            _usuarioRepository.Salvar();

            var output = new UsuarioViewModelOutput
            {
                Codigo = usuario.Codigo,
                Login = usuario.Login,
                Email = usuario.Email
            };

            return Created("", output);
        }


        /// <summary>
        /// Este serviço permite atualizar o cadastro do usuário na API.
        /// </summary>
        /// <returns>Retorna status ok e os dados do usuário.</returns>
        [HttpPut]
        [Route("atualizar")]
        [Authorize]
        [ValidacaoModelStateCustomizado]
        [SwaggerResponse(200, "Sucesso na atualização do cadastro.", typeof(UsuarioViewModelOutput))]
        [SwaggerResponse(400, "Campos obrigatórios.", typeof(ValidaCampoViewModelOutput))]
        [SwaggerResponse(401, "Não autorizado.")]
        [SwaggerResponse(404, "Usuário não encontrado.", typeof(ErroGenericoViewModel))]
        [SwaggerResponse(500, "Erro interno.", typeof(ErroGenericoViewModel))]
        public IActionResult AtualizarCadastro(CadastroViewModel input)
        {
            var identifier = User?.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var codigoUsuario = int.Parse(identifier);

            var dadoSalvos = _usuarioRepository.GetUsuario(codigoUsuario);

            if (dadoSalvos == null)
            {
                return NotFound(new ErroGenericoViewModel { Mensagem = "Usuário não encontrado!" });
            }

            dadoSalvos.Login = input.Login;
            dadoSalvos.Email = input.Email;

            _usuarioRepository.Salvar();

            var output = new UsuarioViewModelOutput
            {
                Codigo = dadoSalvos.Codigo,
                Login = dadoSalvos.Login,
                Email = dadoSalvos.Email
            };

            return Ok(output);
        }


        /// <summary>
        /// Este serviço permite excluir o cadastro do usuário na API.
        /// </summary>
        /// <returns>Retorna status ok.</returns>
        [HttpDelete]
        [Route("excluir")]
        [Authorize]
        [SwaggerResponse(200, "Sucesso na exclusão do cadastro.")]
        [SwaggerResponse(401, "Não autorizado.")]
        [SwaggerResponse(404, "Usuário não encontrado.", typeof(ErroGenericoViewModel))]
        [SwaggerResponse(500, "Erro interno.", typeof(ErroGenericoViewModel))]
        public IActionResult ExcluirCadastro()
        {
            var identifier = User?.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var codigoUsuario = int.Parse(identifier); 
            
            var dadoSalvos = _usuarioRepository.GetUsuario(codigoUsuario);

            if (dadoSalvos == null)
            {
                return NotFound(new ErroGenericoViewModel { Mensagem = "Usuário não encontrado!" });
            }

            _usuarioRepository.Excluir(dadoSalvos);
            _usuarioRepository.Salvar();

            return Ok();
        }


        /// <summary>
        /// Este serviço permite listar os usuários cadastrados na API.
        /// </summary>
        /// <returns>Retorna status ok com a lista dos usuários.</returns>
        [HttpGet]
        [Route("listar")]
        [SwaggerResponse(200, "Sucesso ao obter os usuários.", typeof(CadastroViewModel))]
        [SwaggerResponse(500, "Erro interno.", typeof(ErroGenericoViewModel))]
        public IActionResult ListarUsuarios()
        {
            var usuarios = _usuarioRepository.GetUsuarios().Select(u =>
                new CadastroViewModel
                {
                    Login = u.Login,
                    Email = u.Email
                });

            return Ok(usuarios);
        }
    }
}
