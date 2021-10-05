using Microsoft.VisualStudio.TestTools.UnitTesting;
using GatewayPagamento.Dominio.Entidades;

namespace GatewayPagamento.Repositorios.SqlServer.CodeFirst.Tests
{
    [TestClass()]
    public class GatewayPagamentoDbContextTests
    {
        private readonly GatewayPagamentoDbContext contexto = new GatewayPagamentoDbContext();

        [TestMethod()]
        public void InserirCartaoTeste()
        {
            var cartao = new Cartao { Limite = 1000, Numero = "1234123412341234" };
            
            contexto.Cartoes.Add(cartao);
            contexto.Pagamentos.Add(new Pagamento {Cartao = cartao, NumeroPedido = "1841", Status = StatusPagamento.PagamentoOK, Valor = 41 });

            contexto.SaveChanges();
        }
    }
}