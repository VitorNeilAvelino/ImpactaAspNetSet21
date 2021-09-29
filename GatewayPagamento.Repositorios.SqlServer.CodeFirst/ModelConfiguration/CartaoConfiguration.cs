using GatewayPagamento.Dominio.Entidades;
using System.Data.Entity.ModelConfiguration;

namespace GatewayPagamento.Repositorios.SqlServer.CodeFirst.ModelConfiguration
{
    internal class CartaoConfiguration : EntityTypeConfiguration<Cartao>
    {
        public CartaoConfiguration()
        {
            Property(c => c.Numero)
                .IsRequired()
                .HasMaxLength(20);

            Property(c => c.Limite)
                .IsRequired();
                //.HasPrecision(11, 2);
        }
    }
}