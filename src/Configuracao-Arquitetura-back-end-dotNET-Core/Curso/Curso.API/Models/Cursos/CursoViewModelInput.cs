using System.ComponentModel.DataAnnotations;

namespace Curso.API.Models.Cursos
{
    public class CursoViewModelInput
    {
        [Required(ErrorMessage = "O nome é obrigatório!")]
        public string Nome { get; set; }
        

        [Required(ErrorMessage = "A descrição é obrigatória!")]
        public string Descricao { get; set; }
    }
}
