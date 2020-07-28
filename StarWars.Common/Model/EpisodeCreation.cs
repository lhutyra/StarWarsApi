using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StarWars.Api.Model
{
    public class EpisodeCreation
    {
        [Required]
        [MinLength(3)]
        public string EpisodeName { get; set; }
    }
}
