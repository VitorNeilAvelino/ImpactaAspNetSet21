using GatewayPagamento.Dominio.Entidades;
using GatewayPagamento.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GatewayPagamento.Dominio.Servicos
{
    public class PagamentoServico
    {
        private readonly ICartaoRepositorio cartaoRepositorio;
        private readonly IPagamentoRepositorio pagamentoRepositorio;

        public PagamentoServico(ICartaoRepositorio cartaoRepositorio, IPagamentoRepositorio pagamentoRepositorio)
        {
            this.cartaoRepositorio = cartaoRepositorio;
            this.pagamentoRepositorio = pagamentoRepositorio;
        }

        public StatusPagamento Inserir(Pagamento pagamento)
        {
            var cartao = cartaoRepositorio.Selecionar(pagamento.Cartao.Numero);

            if (cartao == null)
            {
                return StatusPagamento.CartaoInexistente;
            }

            var pagamentosExistentes = pagamentoRepositorio.Selecionar(p => p.NumeroPedido == pagamento.NumeroPedido);

            if (pagamentosExistentes.Any())
            {
                return StatusPagamento.PedidoJaPago;
            }

            if (pagamento.Valor > cartao.Limite)
            {
                return StatusPagamento.SaldoIndisponivel;
            }

            pagamento.Status = StatusPagamento.PagamentoOK;

            pagamentoRepositorio.Inserir(pagamento);

            return StatusPagamento.PagamentoOK;
        }
    }
}
