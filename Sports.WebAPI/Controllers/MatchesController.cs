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
using System.Globalization;

namespace Sports.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchesController : ControllerBase
    {
        private ITeamLogic teamLogic;
        private ISportLogic sportLogic;
        private IMatchLogic matchLogic;
        private IMapper mapper;

        public MatchesController(IMatchLogic aMatchLogic, ISportLogic aSportLogic, ITeamLogic aTeamLogic)
        {
            teamLogic = aTeamLogic;
            sportLogic = aSportLogic;
            matchLogic = aMatchLogic;
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
            mapper = new Mapper(config);
        }

        [HttpGet("{id}", Name = "GetMatchById")]
        public IActionResult Get(int id, string token)
        {
            Guid realToken = Guid.Parse(token);
            matchLogic.SetSession(realToken);
            Match match = matchLogic.GetMatchById(id);
            MatchModelOut modelOut = MatchToModelOut(match);
            return Ok(modelOut);
        }

        [HttpGet(Name = "GetAllMatches")]
        public IActionResult GetAll(string token)
        {
            Guid realToken = Guid.Parse(token);
            matchLogic.SetSession(realToken);
            ICollection<Match> matchList = matchLogic.GetAllMatches();
            ICollection<MatchModelOut> matchModels = new List<MatchModelOut>();
            foreach (Match match in matchList)
            {
                MatchModelOut model = MatchToModelOut(match);
                matchModels.Add(model);
            }
            return Ok(matchModels.ToList());
        }

        private MatchModelOut MatchToModelOut(Match match)
        {
            MatchModelOut model = mapper.Map<MatchModelOut>(match);
            model.Date = match.Date.ToString();
            model.LocalId = match.Local.Id;
            model.VisitorId = match.Visitor.Id;
            model.SportId = match.Sport.Id;
            return model;
        }

        [HttpPost(Name = "AddMatch")]
        public IActionResult Post([FromBody] MatchModelIn matchIn, string token)
        {
            Guid realToken = Guid.Parse(token);
            matchLogic.SetSession(realToken);
            Match match = ModelToMatch(matchIn);
            matchLogic.AddMatch(match);
            MatchModelOut modelOut = MatchToModelOut(match);
            return Ok(modelOut);
        }

        private Match ModelToMatch(MatchModelIn matchIn)
        {
            Match match = mapper.Map<Match>(matchIn);
            match.Date = DateTime.ParseExact(matchIn.Date, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
            match.Local = new Team() { Id = matchIn.LocalId };
            match.Visitor = new Team() { Id = matchIn.VisitorId };
            match.Sport = new Sport() { Id = matchIn.SportId };
            return match;
        }

        [HttpPut("{id}", Name = "ModifyTeam")]
        public IActionResult PutTeam(int id, [FromBody] TeamModelIn teamIn, string token)
        {
            Guid realToken = Guid.Parse(token);
            teamLogic.SetSession(realToken);
            Team team = mapper.Map<Team>(teamIn);
            teamLogic.Modify(id, team);
            return Ok("Team modified succesfully");
        }

        [HttpDelete("{id}", Name = "DeleteTeam")]
        public IActionResult DeleteTeam(int id, string token)
        {
            Guid realToken = Guid.Parse(token);
            teamLogic.SetSession(realToken);
            teamLogic.Delete(id);
            return Ok("Team deleted succesfully");
        }
    }
}