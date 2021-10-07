using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ExpoCenter.Mvc.Models
{
    public class ParticipanteViewModel
    {
        private string cpf;

        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }

        [DisplayName("CPF")]
        //[DisplayFormat(DataFormatString = "{0:000.000.000-00}")]
        public string Cpf { get => long.Parse(cpf).ToString(@"000\.000\.000-00"); set => cpf = value; }

        [DataType(DataType.Date)]
        [Display(Name = "Nascimento")]
        public DateTime DataNascimento { get; set; }
    }
}
