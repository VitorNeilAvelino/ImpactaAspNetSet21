﻿using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ExpoCenter.Mvc.Models
{
    public class EventoViewModel
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [DisplayName("Descrição")]
        public string Descricao { get; set; }
        
        [Required]
        public DateTime Data { get; set; }

        [Required]
        public string Local { get; set; }

        [Required]
        [DisplayName("Preço")]
        [DataType(DataType.Currency)]
        public decimal Preco { get; set; }
    }
}