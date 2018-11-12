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
using System.Globalization;
using static System.Net.WebRequestMethods;

namespace Sports.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchesController : ControllerBase
    {
        private ICompetitorLogic competitorLogic;
        private ISportLogic sportLogic;
        private IMatchLogic matchLogic;
        private IFixtureLogic fixtureLogic;
        private ILogLogic logLogic;
        private IMapper mapper;

        public MatchesController(IMatchLogic aMatchLogic, ISportLogic aSportLogic, ICompetitorLogic aCompetitorLogic, IFixtureLogic aFixtureLogic, ILogLogic aLogLogic)
        {
            competitorLogic = aCompetitorLogic;
            sportLogic = aSportLogic;
            matchLogic = aMatchLogic;
            fixtureLogic = aFixtureLogic;
            logLogic = aLogLogic;
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
            mapper = new Mapper(config);
        }

        [HttpGet("{id}", Name = "GetMatchById")]
        public IActionResult Get(int id, [FromHeader] string token)
        {
            try
            {
                Guid realToken = Guid.Parse(token);
                matchLogic.SetSession(realToken);
                Match match = matchLogic.GetMatchById(id);
                MatchModelOut modelOut = mapper.Map<MatchModelOut>(match);
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
                return StatusCode((int)HttpStatusCode.ServiceUnavailable, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet(Name = "GetAllMatches")]
        public IActionResult GetAll([FromHeader] string token)
        {
            try
            {
                Guid realToken = Guid.Parse(token);
                matchLogic.SetSession(realToken);
                ICollection<Match> matchList = matchLogic.GetAllMatches();
                ICollection<MatchModelOut> matchModels = new List<MatchModelOut>();
                foreach (Match match in matchList)
                {
                    MatchModelOut model = mapper.Map<MatchModelOut>(match);
                    matchModels.Add(model);
                }
                return Ok(matchModels.ToList());
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


        [HttpPost(Name = "AddMatch")]
        public IActionResult Post([FromBody] MatchModelIn matchIn, [FromHeader] string token)
        {
            try
            {
                Guid realToken = Guid.Parse(token);
                matchLogic.SetSession(realToken);
                Match match = mapper.Map<Match>(matchIn);
                matchLogic.AddMatch(match);
                MatchModelOut modelOut = mapper.Map<MatchModelOut>(match);
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
                return StatusCode((int)HttpStatusCode.ServiceUnavailable, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPut("{id}", Name = "ModifyMatch")]
        public IActionResult Put(int id, [FromBody] MatchModelIn matchIn, [FromHeader] string token)
        {
            try
            {
                Guid realToken = Guid.Parse(token);
                matchLogic.SetSession(realToken);
                Match match = mapper.Map<Match>(matchIn);
                matchLogic.ModifyMatch(id, match);
                return RedirectToRoute("GetMatchById", new { id = id, token = token });
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

        [HttpDelete("{id}", Name = "DeleteMatch")]
        public IActionResult Delete(int id, [FromHeader] string token)
        {
            try
            {
                Guid realToken = Guid.Parse(token);
                matchLogic.SetSession(realToken);
                matchLogic.DeleteMatch(id);
                return Ok("Match deleted succesfully");
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


        [HttpGet("{id}/comments", Name = "GetAllComments")]
        public IActionResult GetComments(int id, [FromHeader] string token)
        {
            try
            {
                Guid realToken = Guid.Parse(token);
                matchLogic.SetSession(realToken);
                ICollection<Comment> comments = matchLogic.GetAllComments(id);
                ICollection<CommentModelOut> commentModels = new List<CommentModelOut>();
                foreach (Comment comment in comments)
                {
                    CommentModelOut model = mapper.Map<CommentModelOut>(comment);
                    commentModels.Add(model);
                }
                return Ok(commentModels.ToList());

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

        [HttpPost("{id}/comments", Name = "GetAllComments")]
        public IActionResult PostComment(int id, [FromBody] CommentModelIn commentIn, [FromHeader] string token)
        {
            try
            {
                Guid realToken = Guid.Parse(token);
                matchLogic.SetSession(realToken);
                Comment comment = mapper.Map<Comment>(commentIn);
                matchLogic.AddCommentToMatch(id, comment);
                return RedirectToRoute("GetAllComments", new { id = id, token = token });

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
        

        [HttpPost("fixtureImplementations", Name = "AddFixture")]
        public IActionResult PostFixtureImplementation([FromBody]FixturesPathDTO fixtureDTO, [FromHeader] string token)
        {
            try
            {
                Guid realToken = Guid.Parse(token);
                fixtureLogic.SetSession(realToken);
                fixtureLogic.AddFixtureImplementations(fixtureDTO.Path);
                return Ok("Fixtures succesfully added");

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

        [HttpPost("generateFixture", Name = "GenerateFixture")]
        public IActionResult GenerateFixture([FromBody] FixtureDTO fixtureData, [FromHeader] string token)
        {
            try
            {
                Guid realToken = Guid.Parse(token);
                User user =  fixtureLogic.SetSession(realToken);
                ICollection<Sport> sports = new List<Sport>();
                foreach (SportModelIn model in fixtureData.Sports)
                {
                    Sport sport = mapper.Map<Sport>(model);
                    sports.Add(sport);
                }
                DateTime startDate = Convert.ToDateTime(fixtureData.Date);
                fixtureLogic.GenerateFixture(sports, startDate);
                logLogic.AddEntry("Fixture", user.UserName, DateTime.Now);
                return Ok("Fixture succesfully generated");

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

        [HttpPut("nextFixture", Name = "ChangeFixture")]
        public IActionResult NextFixture([FromHeader] string token)
        {
            try
            {
                Guid realToken = Guid.Parse(token);
                fixtureLogic.SetSession(realToken);
                string message = fixtureLogic.ChangeFixtureImplementation();
                return Ok(message);
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