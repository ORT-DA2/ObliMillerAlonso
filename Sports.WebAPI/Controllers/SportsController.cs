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
        private ICompetitorLogic competitorLogic;
        private ISportLogic sportLogic;
        private IMatchLogic matchLogic;
        private IMapper mapper;

        public SportsController(ICompetitorLogic aCompetitorLogic, ISportLogic aSportLogic, IMatchLogic aMatchLogic)
        {
            competitorLogic = aCompetitorLogic;
            sportLogic = aSportLogic;
            matchLogic = aMatchLogic;
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
            mapper = new Mapper(config);
        }

        [HttpGet("{id}", Name = "GetSportById")]
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
                return Unauthorized();
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
                return StatusCode((int)HttpStatusCode.ServiceUnavailable, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
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
                ICollection<SportModelOut> competitorModels = new List<SportModelOut>();
                foreach (Sport sport in sportList)
                {
                    SportModelOut modelOut = mapper.Map<SportModelOut>(sport);
                    competitorModels.Add(modelOut);
                }
                return Ok(competitorModels.ToList());
            }
            catch (UnauthorizedException ex)
            {
                return Unauthorized();
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
                return StatusCode((int)HttpStatusCode.ServiceUnavailable, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
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
                return Unauthorized();
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
                return StatusCode((int)HttpStatusCode.ServiceUnavailable, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
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
                sportLogic.ModifySport(id, sport);
                SportModelOut modelOut = mapper.Map<SportModelOut>(sport);
                return RedirectToRoute("GetSportById", new { id = id, token = token });
            }
            catch (UnauthorizedException ex)
            {
                return Unauthorized();
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
                return StatusCode((int)HttpStatusCode.ServiceUnavailable, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
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
                return Unauthorized();
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
                return StatusCode((int)HttpStatusCode.ServiceUnavailable, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost("{id}/competitors", Name = "AddCompetitor")]
        public IActionResult PostCompetitor(int id, [FromBody] CompetitorModelIn competitorIn, [FromHeader] string token)
        {
            try
            {
                Guid realToken = Guid.Parse(token);
                sportLogic.SetSession(realToken);
                competitorLogic.SetSession(realToken);
                Competitor competitor = mapper.Map<Competitor>(competitorIn);
                sportLogic.AddCompetitorToSport(id, competitor);
                CompetitorModelOut modelOut = mapper.Map<CompetitorModelOut>(competitor);
                return Ok(modelOut);
            }
            catch (UnauthorizedException ex)
            {
                return Unauthorized();
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
                return StatusCode((int)HttpStatusCode.ServiceUnavailable, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        [HttpGet("{id}/competitors", Name = "GetCompetitorsFromSport")]
        public IActionResult GetCompetitors(int id, [FromHeader] string token)
        {
            try
            {
                Guid realToken = Guid.Parse(token);
                sportLogic.SetSession(realToken);
                ICollection<Competitor> competitors = sportLogic.GetCompetitorsFromSport(id);
                ICollection<CompetitorModelOut> competitorModels = new List<CompetitorModelOut>();
                foreach (Competitor competitor in competitors)
                {
                    CompetitorModelOut model = mapper.Map<CompetitorModelOut>(competitor);
                    competitorModels.Add(model);
                }
                return Ok(competitorModels);
            }
            catch (UnauthorizedException ex)
            {
                return Unauthorized();
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
                return StatusCode((int)HttpStatusCode.ServiceUnavailable, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}/ranking", Name = "GetRanking")]
        public IActionResult GetRanking(int id, [FromHeader] string token)
        {
            try
            {
                Guid realToken = Guid.Parse(token);
                matchLogic.SetSession(realToken);
                ICollection<CompetitorScore> ranking = matchLogic.GenerateRanking(id);
                ICollection<CompetitorScoreModelOut> rankingModels = new List<CompetitorScoreModelOut>();
                foreach (CompetitorScore competitor in ranking)
                {
                    CompetitorScoreModelOut model = mapper.Map<CompetitorScoreModelOut>(competitor);
                    rankingModels.Add(model);
                }
                return Ok(rankingModels.ToList());

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
                return StatusCode((int)HttpStatusCode.ServiceUnavailable, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}