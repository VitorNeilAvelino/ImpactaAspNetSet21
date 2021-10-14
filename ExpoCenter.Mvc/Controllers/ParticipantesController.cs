using AutoMapper;
using ExpoCenter.Dominio.Entidades;
using ExpoCenter.Mvc.Models;
using ExpoCenter.Repositorios.SqlServer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExpoCenter.Mvc.Controllers
{
    public class ParticipantesController : Controller
    {
        private readonly ExpoCenterDbContext dbContext;// = new ExpoCenterDbContext();
        private readonly IMapper mapper;

        public ParticipantesController(ExpoCenterDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public ActionResult Index()
        {
            return View(mapper.Map<List<ParticipanteIndexViewModel>>(dbContext.Participantes));            
        }

        // GET: ParticipantesController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ParticipantesController/Create
        public ActionResult Create()
        {
            var viewModel = new ParticipanteCreateViewModel();

            viewModel.Eventos = mapper.Map<List<EventoGridViewModel>>(dbContext.Eventos);
            
            return View(viewModel);
        }

        // POST: ParticipantesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ParticipanteCreateViewModel viewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(ModelState);
                }

                var participante = mapper.Map<Participante>(viewModel);

                participante.Eventos = new List<Evento>();

                foreach (var evento in viewModel.Eventos.Where(e => e.Selecionado))
                {
                    participante.Eventos.Add(dbContext.Eventos.Single(e => e.Id == evento.Id));
                }

                dbContext.Participantes.Add(participante);
                dbContext.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View("Error");
            }
        }

        // GET: ParticipantesController/Edit/5
        public ActionResult Edit(int id)
        {
            //var participante = dbContext.Participantes.Include(p => p.Eventos).SingleOrDefault(p => p.Id == id);
            var participante = dbContext.Participantes.Find(id);

            if (participante == null)
            {
                return NotFound();
            }

            var viewModel = mapper.Map<ParticipanteCreateViewModel>(participante);

            viewModel.Eventos = mapper.Map<List<EventoGridViewModel>>(dbContext.Eventos
                .Where(e => e.Data > DateTime.Now)
                .ToList());

            foreach (var evento in participante.Eventos)
            {
                viewModel.Eventos.Single(e => e.Id == evento.Id).Selecionado = true;
            }

            return View(viewModel);
        }

        // POST: ParticipantesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ParticipanteCreateViewModel viewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(ModelState);
                }

                var participante = dbContext.Participantes.Find(viewModel.Id);

                if (participante == null)
                {
                    return NotFound();
                }

                dbContext.Entry(participante).CurrentValues.SetValues(viewModel);

                foreach (var evento in viewModel.Eventos)
                {
                    if (evento.Selecionado)
                    {
                        if (participante.Eventos.Any(e => e.Id == evento.Id)) continue;
                        
                        participante.Eventos.Add(dbContext.Eventos.Find(evento.Id));
                    }
                    else
                    {
                        participante.Eventos.Remove(dbContext.Eventos.Find(evento.Id));
                    }
                }

                dbContext.Update(participante);
                dbContext.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqlException sqlException)
                {
                    switch (sqlException.Message)
                    {
                        case string mensagem when mensagem.Contains("IX_Participante_Cpf"):
                            ModelState.AddModelError("", $"O CPF {viewModel.Cpf} já está cadastrado.");
                            break;

                        case string mensagem when mensagem.Contains("IX_Participante_Email"):
                            ModelState.AddModelError("", $"O e-mail {viewModel.Email} já está cadastrado.");
                            break;
                    }

                    if (!ModelState.IsValid)
                    {
                        return View(viewModel);
                    }
                }

                throw;
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        // GET: ParticipantesController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ParticipantesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
