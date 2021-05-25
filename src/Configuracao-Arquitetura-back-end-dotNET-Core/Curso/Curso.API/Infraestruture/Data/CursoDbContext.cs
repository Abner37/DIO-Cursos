using Curso.API.Business.Entities;
using Curso.API.Infraestruture.Data.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Curso.API.Infraestruture.Data
{
    public class CursoDbContext : DbContext
    {
        public CursoDbContext(DbContextOptions<CursoDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CursoMapping());
            modelBuilder.ApplyConfiguration(new UsuarioMapping());
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Usuario> Usuarios {get; set;}
        public DbSet<Business.Entities.Curso> Cursos {get; set; }
    }
}
