﻿using AutoMapper;
using ExpoCenter.Dominio.Entidades;
using ExpoCenter.Mvc.Models;

namespace ExpoCenter.Mvc.Mappings
{
    public class ViewModelProfile : Profile
    {
        public ViewModelProfile()
        {
            CreateMap<Participante, ParticipanteIndexViewModel>().ReverseMap();
            CreateMap<Participante, ParticipanteCreateViewModel>().ReverseMap();
            CreateMap<Participante, ParticipanteGridViewModel>().ReverseMap();

            CreateMap<Evento, EventoViewModel>().ReverseMap();
            CreateMap<Evento, EventoGridViewModel>().ReverseMap();
        }
    }
}