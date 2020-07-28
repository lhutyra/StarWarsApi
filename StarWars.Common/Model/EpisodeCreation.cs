using System.ComponentModel.DataAnnotations;

namespace StarWars.Common.Model
{
    public class EpisodeDto
    {
        [Required]
        [MinLength(3)]
        public string EpisodeName { get; set; }
    }
}
