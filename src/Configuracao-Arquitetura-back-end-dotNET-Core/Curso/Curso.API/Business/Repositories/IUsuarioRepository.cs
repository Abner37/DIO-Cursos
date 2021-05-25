using Curso.API.Business.Entities;
using System.Collections.Generic;

namespace Curso.API.Business.Repositories
{
    public interface IUsuarioRepository
    {
        void Adicionar(Usuario usuario);
        void Excluir(Usuario usuario);

        void Salvar();

        Usuario GetUsuario(int codigo);
        Usuario GetUsuario(string login, string senha);
        IList<Usuario> GetUsuarios();
    }
}
