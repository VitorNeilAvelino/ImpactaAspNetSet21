using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ExpoCenter.Dominio.Entidades;
using ExpoCenter.Repositorios.SqlServer;
using AutoMapper;
using ExpoCenter.Mvc.Models;

namespace ExpoCenter.Mvc.Controllers
{
    public class EventosController : Controller
    {
        private readonly ExpoCenterDbContext _context;
        private readonly IMapper mapper;

        public EventosController(ExpoCenterDbContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }

        // GET: Eventos
        public async Task<ActionResult> Index()
        {
            var eventos = await _context.Eventos
                            .OrderBy(e => e.Data)
                            .ThenBy(e => e.Descricao)
                            .ToListAsync();

            return View(mapper.Map<List<EventoViewModel>>(eventos));
        }

        // GET: Eventos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evento = await _context.Eventos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (evento == null)
            {
                return NotFound();
            }

            return View(mapper.Map<EventoViewModel>(evento));
        }

        // GET: Eventos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Eventos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventoViewModel evento)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mapper.Map<Evento>(evento));
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(evento);
        }

        // GET: Eventos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evento = await _context.Eventos.FindAsync(id);

            if (evento == null)
            {
                return NotFound();
            }

            return View(mapper.Map<EventoViewModel>(evento));
        }

        // POST: Eventos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EventoViewModel evento)
        {
            if (id != evento.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mapper.Map<Evento>(evento));
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventoExists(evento.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            return View(evento);
        }

        // GET: Eventos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evento = await _context.Eventos.SingleOrDefaultAsync(m => m.Id == id);

            if (evento == null)
            {
                return NotFound();
            }

            return View(mapper.Map<EventoViewModel>(evento));
        }

        // POST: Eventos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var evento = await _context.Eventos.FindAsync(id);

            _context.Eventos.Remove(evento);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool EventoExists(int id)
        {
            return _context.Eventos.Any(e => e.Id == id);
        }

        public ActionResult Participantes(int id)
        {
            var evento = _context.Eventos.Find(id);

            var viewModel = mapper.Map<EventoViewModel>(evento);

            viewModel.Participantes = mapper.Map<List<ParticipanteGridViewModel>>(_context.Participantes);

            foreach (var participante in evento.Participantes)
            {
                viewModel.Participantes.Single(p => p.Id == participante.Id).Selecionado = true;
            }

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Participantes(EventoViewModel viewModel)
        {
            var evento = _context.Eventos.Find(viewModel.Id);

            foreach (var participante in viewModel.Participantes)
            {
                if (participante.Selecionado)
                {
                    if (evento.Participantes.Any(p => p.Id == participante.Id)) continue;

                    evento.Participantes.Add(_context.Participantes.Find(participante.Id));
                }
                else
                {
                    evento.Participantes.Remove(_context.Participantes.Find(participante.Id));
                }
            }

            _context.Update(evento);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}