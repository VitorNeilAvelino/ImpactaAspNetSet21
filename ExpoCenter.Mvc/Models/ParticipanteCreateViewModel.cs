using ExpoCenter.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ExpoCenter.Mvc.Models
{
    public class ParticipanteCreateViewModel
    {
        public int Id { get; set; }
        
        [Required]
        public string Nome { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "CPF")]
        [StringLength(11, MinimumLength = 11)]
        public string Cpf { get; set; }
        
        [Required]
        [Display(Name = "Nascimento")]
        public DateTime DataNascimento { get; set; }

        public List<EventoGridViewModel> Eventos { get; set; }
    }
}
