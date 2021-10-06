﻿using System;
using System.Collections.Generic;

namespace ExpoCenter.Dominio.Entidades
{
    public class Participante
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Cpf { get; set; }
        public DateTime DataNascimento { get; set; }
        public List<Evento> Eventos { get; set; }
    }
}