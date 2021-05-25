using Curso.API.Models.Usuarios;
using System;

namespace Curso.API.Configurations
{
    public interface IAuthenticationService
    {
        string TokenGenerate(UsuarioViewModelOutput usuario);
    }
}
