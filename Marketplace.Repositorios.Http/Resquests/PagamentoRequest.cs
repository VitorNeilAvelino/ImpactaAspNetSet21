﻿namespace Marketplace.Repositorios.Http.Resquests
{
    public class PagamentoRequest
    {
        public string NumeroCartao { get; set; }
        public string NumeroPedido { get; set; }
        public decimal Valor { get; set; }
    }
}