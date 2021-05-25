using System.Collections.Generic;

namespace Curso.API.Models
{
    public class ValidaCampoViewModelOutput
    {
        public IEnumerable<string> Erros { get; set; }

        public ValidaCampoViewModelOutput(IEnumerable<string> erros)
        {
            Erros = erros;
        }
    }
}
