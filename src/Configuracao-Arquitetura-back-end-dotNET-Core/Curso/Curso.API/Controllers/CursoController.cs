using Curso.API.Business.Repositories;
using Curso.API.Configurations;
using Curso.API.Filters;
using Curso.API.Models;
using Curso.API.Models.Cursos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Swashbuckle.AspNetCore.Annotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Curso.API.Controllers
{
    [Route("api/v1/cursos")]
    [Authorize]
    [ApiController]
    public class CursoController : ControllerBase
    {
        private readonly ICursoRepository _cursoRepository;
        private readonly IConfiguration _configuration;
        private readonly IAuthenticationService _authentication;


        public CursoController(
            ICursoRepository cursoRepository,
            IConfiguration configuration,
            IAuthenticationService authentication)
        {
            _cursoRepository = cursoRepository;
            _configuration = configuration;
            _authentication = authentication;
        }


        /// <summary>
        /// Este serviço permite cadastrar curso para o usuário autenticado.
        /// </summary>
        /// <returns>Retorna status 201 e dados do curso do usuário.</returns>
        [HttpPost]
        [Route("")]
        [ValidacaoModelStateCustomizado]
        [SwaggerResponse(201, "Sucesso ao cadastrar um curso.", typeof(CursoViewModelInput))]
        [SwaggerResponse(400, "Campos obrigatórios.", typeof(ValidaCampoViewModelOutput))]
        [SwaggerResponse(401, "Não autorizado.")]
        [SwaggerResponse(500, "Erro interno.", typeof(ErroGenericoViewModel))]
        public async Task<ActionResult> Cadastrar(CursoViewModelInput cursoModelView)
        {
            var curso = new Business.Entities.Curso();
            curso.Nome = cursoModelView.Nome;
            curso.Descricao = cursoModelView.Descricao;
            curso.CodigoUsuario = int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

            _cursoRepository.Adicionar(curso);
            _cursoRepository.Commit();

            return Created("", cursoModelView);
        }


        /// <summary>
        /// Este serviço permite obter todos os cursos ativos do usuário.
        /// </summary>
        /// <returns>Retorna status ok e dados do curso do usuário.</returns>
        [HttpGet]
        [Route("")]
        [SwaggerResponse(200, "Sucesso ao obter os cursos.", typeof(CursoViewModelOutput))]
        [SwaggerResponse(401, "Não autorizado.")]
        [SwaggerResponse(500, "Erro interno.", typeof(ErroGenericoViewModel))]
        public async Task<ActionResult> ListarCursos()
        {
            var identifier = User?.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var codigoUsuario = int.Parse(identifier);

            var cursos = _cursoRepository.GetCursos(codigoUsuario).Select(c =>
                new CursoViewModelOutput
                {
                    Nome = c.Nome,
                    Descricao = c.Descricao,
                    Login = c.Usuario.Login
                }
            );

            return Ok(cursos);
        }
    }
}
