using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sports.WebAPI.Models;
using Sports.Logic.Interface;
using Sports.Domain;
using Sports.Domain.Exceptions;
using Sports.Logic.Exceptions;
using System.Web;
using AutoMapper;

namespace Sports.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SportsController : ControllerBase
    {
        private ITeamLogic teamLogic;
        private ISportLogic sportLogic;
        private IMapper mapper;

        public SportsController(ITeamLogic aTeamLogic, ISportLogic aSportLogic)
        {
            teamLogic = aTeamLogic;
            sportLogic = aSportLogic;
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
            mapper = new Mapper(config);
        }

        [HttpGet("{id}", Name = "GetById")]
        public IActionResult Get(int id, string token)
        {
            Guid realToken = Guid.Parse(token);
            sportLogic.SetSession(realToken);
            Sport sport = sportLogic.GetSportById(id);
            SportModelOut modelOut = mapper.Map<SportModelOut>(sport);
            return Ok(modelOut);
        }

        [HttpGet(Name = "GetAll")]
        public IActionResult GetAll(string token)
        {
            Guid realToken = Guid.Parse(token);
            sportLogic.SetSession(realToken);
            ICollection<Sport> sportList = sportLogic.GetAll();
            ICollection<SportModelOut> teamModels = new List<SportModelOut>();
            return Ok(teamModels.ToList());
        }

        [HttpPost(Name = "AddSport")]
        public IActionResult PostSport([FromBody] SportModelIn sportIn, string token)
        {
            Guid realToken = Guid.Parse(token);
            sportLogic.SetSession(realToken);
            Sport sport = mapper.Map<Sport>(sportIn);
            sportLogic.AddSport(sport);
            SportModelOut modelOut = mapper.Map<SportModelOut>(sport);
            return Ok(modelOut);
        }

        [HttpPut("{id}", Name = "ModifySport")]
        public IActionResult PutSport(int id, [FromBody]  SportModelIn sportIn, string token)
        {
            Guid realToken = Guid.Parse(token);
            sportLogic.SetSession(realToken);
            Sport sport = mapper.Map<Sport>(sportIn);
            sportLogic.ModifySport(sport.Id,sport);
            return Ok("Succesfully modified");
        }

        [HttpDelete("{id}",Name = "DeleteSport")]
        public IActionResult DeleteSport(int id, string token)
        {
            Guid realToken = Guid.Parse(token);
            sportLogic.SetSession(realToken);
            sportLogic.RemoveSport(id);
            return Ok("Succesfully deleted sport");
        }

        [HttpPost("{id}/Teams",Name = "AddTeam")]
        public IActionResult PostTeam(int id, [FromBody] TeamModelIn teamIn, string token)
        {
            Guid realToken = Guid.Parse(token);
            sportLogic.SetSession(realToken);
            Team team = mapper.Map<Team>(teamIn);
            sportLogic.AddTeamToSport(id,team);
            teamLogic.SetPictureFromPath(team.Id, teamIn.ImagePath);
            TeamModelOut modelOut = mapper.Map<TeamModelOut>(team);
            return Ok(modelOut);
        }


        [HttpGet("{id}/Teams", Name = "GetTeamsFromSport")]
        public IActionResult GetTeams(int id, string token)
        {
            Guid realToken = Guid.Parse(token);
            sportLogic.SetSession(realToken);
            ICollection<Team> teams = sportLogic.GetTeamsFromSport(id);
            ICollection<TeamModelOut> teamModels = new List<TeamModelOut>();
            foreach (Team team in teams)
            {
                TeamModelOut model = mapper.Map<TeamModelOut>(team);
                teamModels.Add(model);
            }
            return Ok(teamModels);
        }
    }
}