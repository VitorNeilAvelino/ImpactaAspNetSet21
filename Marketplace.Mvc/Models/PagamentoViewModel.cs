using Marketplace.Repositorios.Http.Responses;
using System.Collections.Generic;
using System.ComponentModel;

namespace Marketplace.Mvc.Models
{
    public class PagamentoViewModel
    {
        public int Id { get; set; }
        
        [DisplayName("Máscara")]
        public string MascaraCartao { get; set; }
        
        [DisplayName("Número do pedido")]
        public string NumeroPedido { get; set; }
        
        public decimal Valor { get; set; }

        internal static List<PagamentoViewModel> Mapear(List<PagamentoResponse> pagamentos)
        {
            var viewModels = new List<PagamentoViewModel>();

            foreach (var pagamento in pagamentos)
            {
                viewModels.Add(Mapear(pagamento));
            }

            return viewModels;
        }

        private static PagamentoViewModel Mapear(PagamentoResponse pagamento)
        {
            var viewModel = new PagamentoViewModel();

            viewModel.MascaraCartao = pagamento.MascaraCartao;
            viewModel.Id = pagamento.Id;
            //viewModel.Data = pagamento.Data;
            viewModel.NumeroPedido = pagamento.NumeroPedido;
            viewModel.Valor = pagamento.Valor;

            return viewModel;
        }
    }
}