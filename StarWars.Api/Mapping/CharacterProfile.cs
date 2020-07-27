﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using StarWars.Api.Model;
using StarWars.Domain;

namespace StarWars.Api.Mapping
{
    public class CharacterProfile : Profile
    {
        public CharacterProfile()
        {
            CreateMap<Character, CharacterResult>();

            CreateMap<Episode, EpisodeCreation>();
            CreateMap<Character, CharacterResult>()
                .ForMember(dest => dest.Episodes, opt => opt.MapFrom(src =>
                    src.CharacterEpisodes.Select(f => f.Episode.EpisodeName)));
        }
    }
}
