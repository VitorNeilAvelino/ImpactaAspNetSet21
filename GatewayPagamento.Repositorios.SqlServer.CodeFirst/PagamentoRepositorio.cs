using GatewayPagamento.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GatewayPagamento.Repositorios.SqlServer.CodeFirst
{
    public class PagamentoRepositorio
    {
        public List<Pagamento> Selecionar(string numeroCartao)
        {
            using (var contexto = new GatewayPagamentoDbContext())
            {
                return contexto.Pagamentos
                    .Where(p => p.Cartao.Numero == numeroCartao)
                    .ToList();
            }
        }

        public List<Pagamento> Selecionar(Func<Pagamento, bool> condicao)
        {
            using (var contexto = new GatewayPagamentoDbContext())
            {
                return contexto.Pagamentos
                    .Where(condicao)
                    .ToList();
            }
        }

        public void Inserir(Pagamento pagamento)
        {
            using (var contexto = new GatewayPagamentoDbContext())
            {
                contexto.Pagamentos.Add(pagamento);
                contexto.SaveChanges();
            }
        }
    }
}
