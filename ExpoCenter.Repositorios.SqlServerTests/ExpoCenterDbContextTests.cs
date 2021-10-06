using Microsoft.VisualStudio.TestTools.UnitTesting;
using ExpoCenter.Repositorios.SqlServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using ExpoCenter.Dominio.Entidades;

namespace ExpoCenter.Repositorios.SqlServer.Tests
{
    [TestClass]
    public class ExpoCenterDbContextTests
    {
        private readonly ExpoCenterDbContext dbContext;// = new ExpoCenterDbContext();

        public ExpoCenterDbContextTests()
        {
            var dbContextOptions = new DbContextOptionsBuilder<ExpoCenterDbContext>()
                .UseSqlServer(new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ExpoCenter;Integrated Security=True"))
                .LogTo(ExibirQuery, LogLevel.Information)
                .Options;

            dbContext = new ExpoCenterDbContext(dbContextOptions);
        }

        private void ExibirQuery(string query)
        {
            Console.WriteLine(query);
        }

        [TestMethod()]
        public void InserirEventoTeste()
        {
            var evento = new Evento();
            evento.Data = DateTime.Now;
            evento.Descricao = "Expo Dubai 2020";
            evento.Local = "Dubai";
            evento.Preco = 12.32m;

            dbContext.Eventos.Add(evento);
            dbContext.SaveChanges();
        }

        [TestMethod]
        public void InserirParticipanteTeste()
        {
            var participante = new Participante();
            participante.Cpf = "12345678910";
            participante.DataNascimento = new DateTime(2000, 1, 1);
            participante.Email = "avelino.vitor@gmail.com";
            participante.Nome = "Vítor";

            // var evento = new Evento();

            participante.Eventos = new List<Evento> { dbContext.Eventos.Single(e => e.Descricao == "Expo Dubai 2020") };

            dbContext.Participantes.Add(participante);
            dbContext.SaveChanges();
        }

        [TestMethod]
        public void SelecionarParticipantesTeste()
        {
            foreach (var participante in dbContext.Participantes.OrderBy(p => p.Nome))
            {
                Console.WriteLine(participante.Nome);
            }
        }
    }
}