using System;

namespace AspNet.Capitulo02.Razor.Models
{
    public class ComentarioViewModel
    {
        public DateTime Data { get; set; }
        public string Comentarista { get; set; }
        public string Conteudo { get; set; }
    }
}