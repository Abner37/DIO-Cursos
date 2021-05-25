using Curso.API.Business.Entities;
using Curso.API.Business.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace Curso.API.Infraestruture.Data.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly CursoDbContext _context;


        public UsuarioRepository(CursoDbContext context)
        {
            _context = context;
        }


        public void Adicionar(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
        }
        public void Excluir(Usuario usuario)
        {
            _context.Remove(usuario);
        }

        public void Salvar()
        {
            _context.SaveChanges();
        }

        public Usuario GetUsuario(int codigo)
        {
            return _context.Usuarios.FirstOrDefault(u => u.Codigo == codigo);
        }
        public Usuario GetUsuario(string login, string senha)
        {
            return _context.Usuarios.FirstOrDefault(u => u.Login == login && u.Senha == senha);
        }
        public IList<Usuario> GetUsuarios()
        {
            return _context.Usuarios.ToList();
        }
    }
}
