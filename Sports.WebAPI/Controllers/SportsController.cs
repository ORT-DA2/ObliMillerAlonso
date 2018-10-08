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
using Sports.Logic.Interface.Exceptions;
using Sports.Repository.Interface.Exceptions;
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

        [HttpGet("{id}", Name = "GetSportsById")]
        public IActionResult Get(int id, [FromHeader] string token)
        {
            try
            {
                Guid realToken = Guid.Parse(token);
                sportLogic.SetSession(realToken);
                Sport sport = sportLogic.GetSportById(id);
                SportModelOut modelOut = mapper.Map<SportModelOut>(sport);
                return Ok(modelOut);
            }
            catch (UnauthorizedException ex)
            {
                return StatusCode(401, ex.Message);
            }
            catch (DomainException ex)
            {
                return UnprocessableEntity(ex.Message);
            }
            catch (LogicException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnknownDataAccessException ex)
            {
                return StatusCode(503, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet(Name = "GetAllSports")]
        public IActionResult GetAll([FromHeader] string token)
        {
            try
            {
                Guid realToken = Guid.Parse(token);
                sportLogic.SetSession(realToken);
                ICollection<Sport> sportList = sportLogic.GetAll();
                ICollection<SportModelOut> teamModels = new List<SportModelOut>();
                return Ok(teamModels.ToList());
            }
            catch (UnauthorizedException ex)
            {
                return StatusCode(401, ex.Message);
            }
            catch (DomainException ex)
            {
                return UnprocessableEntity(ex.Message);
            }
            catch (LogicException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnknownDataAccessException ex)
            {
                return StatusCode(503, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost(Name = "AddSport")]
        public IActionResult PostSport([FromBody] SportModelIn sportIn, [FromHeader] string token)
        {
            try
            {
                Guid realToken = Guid.Parse(token);
                sportLogic.SetSession(realToken);
                Sport sport = mapper.Map<Sport>(sportIn);
                sportLogic.AddSport(sport);
                SportModelOut modelOut = mapper.Map<SportModelOut>(sport);
                return Ok(modelOut);
            }
            catch (UnauthorizedException ex)
            {
                return StatusCode(401, ex.Message);
            }
            catch (DomainException ex)
            {
                return UnprocessableEntity(ex.Message);
            }
            catch (LogicException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnknownDataAccessException ex)
            {
                return StatusCode(503, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}", Name = "ModifySport")]
        public IActionResult PutSport(int id, [FromBody]  SportModelIn sportIn, [FromHeader] string token)
        {
            try
            {
                Guid realToken = Guid.Parse(token);
                sportLogic.SetSession(realToken);
                Sport sport = mapper.Map<Sport>(sportIn);
                sportLogic.ModifySport(sport.Id, sport);
                return Ok("Succesfully modified");
            }
            catch (UnauthorizedException ex)
            {
                return StatusCode(401, ex.Message);
            }
            catch (DomainException ex)
            {
                return UnprocessableEntity(ex.Message);
            }
            catch (LogicException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnknownDataAccessException ex)
            {
                return StatusCode(503, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}", Name = "DeleteSport")]
        public IActionResult DeleteSport(int id, [FromHeader] string token)
        {
            try
            {
                Guid realToken = Guid.Parse(token);
                sportLogic.SetSession(realToken);
                sportLogic.RemoveSport(id);
                return Ok("Succesfully deleted sport");
            }
            catch (UnauthorizedException ex)
            {
                return StatusCode(401, ex.Message);
            }
            catch (DomainException ex)
            {
                return UnprocessableEntity(ex.Message);
            }
            catch (LogicException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnknownDataAccessException ex)
            {
                return StatusCode(503, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("{id}/Teams", Name = "AddTeam")]
        public IActionResult PostTeam(int id, [FromBody] TeamModelIn teamIn, [FromHeader] string token)
        {
            try
            {
                Guid realToken = Guid.Parse(token);
                sportLogic.SetSession(realToken);
                Team team = mapper.Map<Team>(teamIn);
                sportLogic.AddTeamToSport(id, team);
                teamLogic.SetPictureFromPath(team.Id, teamIn.ImagePath);
                TeamModelOut modelOut = mapper.Map<TeamModelOut>(team);
                return Ok(modelOut);
            }
            catch (UnauthorizedException ex)
            {
                return StatusCode(401, ex.Message);
            }
            catch (DomainException ex)
            {
                return UnprocessableEntity(ex.Message);
            }
            catch (LogicException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnknownDataAccessException ex)
            {
                return StatusCode(503, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("{id}/Teams", Name = "GetTeamsFromSport")]
        public IActionResult GetTeams(int id, [FromHeader] string token)
        {
            try
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
            catch (UnauthorizedException ex)
            {
                return StatusCode(401, ex.Message);
            }
            catch (DomainException ex)
            {
                return UnprocessableEntity(ex.Message);
            }
            catch (LogicException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnknownDataAccessException ex)
            {
                return StatusCode(503, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}