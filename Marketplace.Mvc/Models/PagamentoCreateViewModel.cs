using Marketplace.Repositorios.Http.Resquests;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Marketplace.Mvc.Models
{
    public class PagamentoCreateViewModel
    {
        [Required]
        [Display(Name = "Cartão")]
        public string NumeroCartao { get; set; }

        [Required]
        [DisplayName("Pedido")]
        public string NumeroPedido { get; set; }
        
        [Required]
        public decimal Valor { get; set; }

        internal static PagamentoRequest Mapear(PagamentoCreateViewModel viewModel)
        {
            var request = new PagamentoRequest();

            request.NumeroCartao = viewModel.NumeroCartao;
            request.NumeroPedido = viewModel.NumeroPedido;
            request.Valor = viewModel.Valor;

            return request;
        }
    }
}