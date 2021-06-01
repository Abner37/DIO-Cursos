using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetMVC.Models
{
    public class Produto
    {
        public int Id { get; set; }
        
        [Display(Name = "Descrição")]
        [Required(ErrorMessage = "Este campo é obrigatório.")]
        public string Descricao { get; set; }
        
        [Required(ErrorMessage = "Este campo é obrigatório.")]
        public int Quantidade { get; set; }

        [Display(Name = "Categoria")]
        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; }
    }
}
