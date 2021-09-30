using GatewayPagamento.Dominio.Entidades;
using System;
using System.Collections.Generic;

namespace GatewayPagamento.WebApi.Models
{
    public class PagamentoViewModel
    {
        public int Id { get; set; }
        public string MascaraCartao { get; set; }
        public string NumeroPedido { get; set; }
        public decimal Valor { get; set; }
        public int Status { get; set; }
        public string MensagemStatus { get; set; }

        internal static IEnumerable<PagamentoViewModel> Mapear(List<Pagamento> pagamentos)
        {
            var viewModels = new List<PagamentoViewModel>();

            foreach (var pagamento in pagamentos)
            {
                viewModels.Add(Mapear(pagamento));
            }

            return viewModels;
        }

        private static PagamentoViewModel Mapear(Pagamento pagamento)
        {
            var viewModel = new PagamentoViewModel();

            viewModel.Id = pagamento.Id;

            string numeroCartao = pagamento.Cartao.Numero;
            viewModel.MascaraCartao = $"{numeroCartao.Substring(0, 6)}...{numeroCartao.Substring(numeroCartao.Length - 4)}";

            viewModel.NumeroPedido = pagamento.NumeroPedido;
            viewModel.Valor = pagamento.Valor;
            viewModel.Status = (int)pagamento.Status;

            return viewModel;
        }
    }
}