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
    public class TeamsController : ControllerBase
    {
        private ITeamLogic teamLogic;
        private ISportLogic sportLogic;
        private IMapper mapper;

        public TeamsController(ITeamLogic aTeamLogic, ISportLogic aSportLogic)
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
            teamLogic.SetSession(realToken);
            Team team = teamLogic.GetTeamById(id);
            if (team == null)
            {
                return NotFound();
            }
            TeamModelOut modelOut = mapper.Map<TeamModelOut>(team);
            return Ok(modelOut);
        }

        [HttpGet(Name = "GetAll")]
        public IActionResult GetAll(string token)
        {
            Guid realToken = Guid.Parse(token);
            teamLogic.SetSession(realToken);
            ICollection<Sport> sportList = sportLogic.GetAll();
            if (sportList == null)
            {
                return NotFound();
            }
            ICollection<TeamModelOut> teamModels = new List<TeamModelOut>();
            foreach (Sport sport in sportList)
            {
                foreach (Team team in sport.Teams)
                {
                    TeamModelOut model = mapper.Map<TeamModelOut>(team);
                    model.SportId = sport.Id;
                    teamModels.Add(model);
                }
            }
            return Ok(teamModels.ToList());
        }

        [HttpPut("{id}", Name = "AddTeam")]
        public IActionResult PutTeam(int id, [FromBody] TeamModelIn teamIn, string token)
        {
            Guid realToken = Guid.Parse(token);
            teamLogic.SetSession(realToken);
            Team team = mapper.Map<Team>(teamIn);
            teamLogic.Modify(id, team);
            TeamModelOut modelOut = mapper.Map<TeamModelOut>(team);
            return Ok(modelOut);
        }

    }
}