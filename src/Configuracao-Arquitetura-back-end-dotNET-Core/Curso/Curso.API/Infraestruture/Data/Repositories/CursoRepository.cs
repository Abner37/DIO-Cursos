using Curso.API.Business.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Curso.API.Infraestruture.Data.Repositories
{
    public class CursoRepository : ICursoRepository
    {
        private readonly CursoDbContext _context;


        public CursoRepository(CursoDbContext context)
        {
            _context = context;
        }


        public void Adicionar(Business.Entities.Curso curso)
        {
            _context.Cursos.Add(curso);
        }

        public void Commit()
        {
            _context.SaveChanges();
        }


        public IList<Business.Entities.Curso> GetCursos(int codigoUsuario)
        {
            return _context.Cursos.Include(i => i.Usuario).Where(c => c.CodigoUsuario == codigoUsuario).ToList();
        }
    }
}
