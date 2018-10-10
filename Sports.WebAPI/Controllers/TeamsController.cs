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

        [HttpGet("{id}", Name = "GetTeamById")]
        public IActionResult Get(int id, [FromHeader] string token)
        {
            try
            {
                Guid realToken = Guid.Parse(token);
                teamLogic.SetSession(realToken);
                Team team = teamLogic.GetTeamById(id);
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
        
        [HttpPut("{id}", Name = "ModifyTeam")]
        public IActionResult PutTeam(int id, [FromBody] TeamModelIn teamIn, [FromHeader] string token)
        {
            try
            {
                Guid realToken = Guid.Parse(token);
                teamLogic.SetSession(realToken);
                Team team = mapper.Map<Team>(teamIn);
                teamLogic.Modify(id, team);
                teamLogic.SetPictureFromPath(id, teamIn.ImagePath);
                return RedirectToRoute("GetTeamById", new { id = id, token = token });
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

        [HttpDelete("{id}", Name = "DeleteTeam")]
        public IActionResult DeleteTeam(int id, [FromHeader] string token)
        {
            try
            {
                Guid realToken = Guid.Parse(token);
                teamLogic.SetSession(realToken);
                teamLogic.Delete(id);
                return Ok("Team deleted succesfully");
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


        [HttpGet(Name = "GetAllTeams")]
        public IActionResult GetAllTeams([FromHeader] string token, [FromHeader] string name, [FromHeader] string order)
        {
            try
            {
                Guid realToken = Guid.Parse(token);
                teamLogic.SetSession(realToken);
                sportLogic.SetSession(realToken);
                ICollection<Team> teamList = teamLogic.GetFilteredTeams(name,order);
                ICollection<TeamModelOut> teamModels = new List<TeamModelOut>();
                foreach (Team team in teamList)
                {
                    TeamModelOut model = mapper.Map<TeamModelOut>(team);
                    teamModels.Add(model);
                }
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


    }
}