﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExpoCenter.Dominio.Entidades;
using ExpoCenter.Repositorios.SqlServer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace ExpoCenter.WebApi.Controllers
{
    [ApiController]    
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PagamentosController : ControllerBase
    {
        private readonly ExpoCenterDbContext _context;

        public PagamentosController(ExpoCenterDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "Agente")]
        public async Task<ActionResult<IEnumerable<Pagamento>>> GetPagamentos()
        {
            return await _context.Pagamentos.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Pagamento>> GetPagamento(int id)
        {
            var pagamento = await _context.Pagamentos.FindAsync(id);

            if (pagamento == null)
            {
                return NotFound();
            }

            return pagamento;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPagamento(int id, Pagamento pagamento)
        {
            if (id != pagamento.Id)
            {
                return BadRequest();
            }

            _context.Entry(pagamento).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PagamentoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Pagamento>> PostPagamento(Pagamento pagamento)
        {
            _context.Pagamentos.Add(pagamento);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPagamento", new { id = pagamento.Id }, pagamento);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePagamento(int id)
        {
            var pagamento = await _context.Pagamentos.FindAsync(id);
            if (pagamento == null)
            {
                return NotFound();
            }

            _context.Pagamentos.Remove(pagamento);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PagamentoExists(int id)
        {
            return _context.Pagamentos.Any(e => e.Id == id);
        }
    }
}