using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using StarWars.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StarWars.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StarWarsController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<Character> Get()
        {
            yield return new Character() { Name = "Luke Skywalker", Episodes = new List<Episode>() { new Episode() { EpisodeName = "NEWHOPE" }, new Episode() { EpisodeName = "EMPIRE" }, new Episode() { EpisodeName = "JEDI" } }, Friends = new List<Character>() { new Character() { Name = "Hans Solo" }, new Character() { Name = "Leia Organa" }, new Character() { Name = "C-3PO" }, new Character() { Name = "R2-D2" } } };
            yield return new Character() { Name = "Darth Vader", Episodes = new List<Episode>() { new Episode() { EpisodeName = "NEWHOPE" }, new Episode() { EpisodeName = "EMPIRE" }, new Episode() { EpisodeName = "JEDI" } }, Friends = new List<Character>() { new Character() { Name = "Wilhuff Tarkin" } } };            
        }
    }
}
