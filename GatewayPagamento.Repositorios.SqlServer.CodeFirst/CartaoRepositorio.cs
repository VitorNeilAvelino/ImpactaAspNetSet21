using GatewayPagamento.Dominio.Entidades;
using System.Linq;

namespace GatewayPagamento.Repositorios.SqlServer.CodeFirst
{
    public class CartaoRepositorio
    {
        public Cartao Selecionar(string numeroCartao)
        {
            using (var contexto = new GatewayPagamentoDbContext())
            {
                return contexto.Cartoes.SingleOrDefault(c => c.Numero == numeroCartao);
            }
        }
    }
}