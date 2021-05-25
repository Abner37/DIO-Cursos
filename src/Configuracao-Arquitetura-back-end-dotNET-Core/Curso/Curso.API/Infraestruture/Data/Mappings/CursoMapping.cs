using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Curso.API.Infraestruture.Data.Mappings
{
    public class CursoMapping : IEntityTypeConfiguration<Business.Entities.Curso>
    {
        public void Configure(EntityTypeBuilder<Business.Entities.Curso> builder)
        {
            builder.ToTable("TB_CURSO");
            builder.HasKey(pk => pk.Codigo);
            builder.Property(p => p.Codigo).ValueGeneratedOnAdd();
            builder.Property(p => p.Nome);
            builder.Property(p => p.Descricao);
            builder.HasOne(p => p.Usuario).WithMany().HasForeignKey(fk => fk.CodigoUsuario);
        }
    }
}
