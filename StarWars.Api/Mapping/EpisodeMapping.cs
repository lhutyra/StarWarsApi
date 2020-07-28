using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using StarWars.Api.Model;
using StarWars.Common.Model;
using StarWars.Domain;

namespace StarWars.Api.Mapping
{
    public class EpisodeProfile : Profile
    {
        public EpisodeProfile()
        {
            CreateMap<EpisodeDto, Episode>();

            CreateMap<Episode, EpisodeDto>();
            CreateMap<Episode, EpisodeResult>();
        }
    }
}
