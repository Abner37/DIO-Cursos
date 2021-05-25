using System.ComponentModel.DataAnnotations;

namespace Curso.API.Models.Usuarios
{
    public class CadastroViewModel
    {
        [Required(ErrorMessage = "O login é obrigatório")]
        public string Login { get; set; }

        [Required(ErrorMessage = "O e-mail é obrigatório")]
        public string Email { get; set; }
    }
}
