using System;
using System.Collections.Generic;

namespace Curso.API.Business.Repositories
{
    public interface ICursoRepository
    {
        void Adicionar(Entities.Curso curso);
        void Commit();

        IList<Entities.Curso> GetCursos(int codigoUsuario);
    }
}
