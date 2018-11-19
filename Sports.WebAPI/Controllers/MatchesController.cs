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


        [HttpGet("bySport",Name = "GetAllMatchesBySport")]
        public IActionResult GetAllBySport([FromHeader] string token)
        {
            try
            {
                Guid realToken = Guid.Parse(token);
                matchLogic.SetSession(realToken);
                ICollection<Match> matchList = matchLogic.GetAllMatches();
                ICollection<MatchSimpleModelOut> matchModels = mapper.Map<ICollection<MatchSimpleModelOut>>(matchList);
                ICollection<SportMatchModelOut> sportList = new List<SportMatchModelOut>();
                foreach (MatchSimpleModelOut match in matchModels)
                {
                    SportMatchModelOut sportModel = sportList.Where(s => s.Id == match.Sport.Id).FirstOrDefault();
                    if (sportModel == null)
                    {
                        sportModel = new SportMatchModelOut()
                        {
                            Id = match.Sport.Id,
                            Name = match.Sport.Name,
                            Matches = new List<MatchSimpleModelOut>()
                        };
                        sportList.Add(sportModel);
                    }
                    sportModel.Matches.Add(match);
                }
                return Ok(sportList.ToList());
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

        [HttpGet("bySport/{id}", Name = "GetAllMatchesBySportId")]
        public IActionResult GetAllBySportId(int id, [FromHeader] string token)
        {
            try
            {
                Guid realToken = Guid.Parse(token);
                matchLogic.SetSession(realToken);
                ICollection<Match> matchList = matchLogic.GetAllMatches();
                ICollection<MatchSimpleModelOut> matchModels = mapper.Map<ICollection<MatchSimpleModelOut>>(matchList);
                ICollection<SportMatchModelOut> sportList = new List<SportMatchModelOut>();
                foreach (MatchSimpleModelOut match in matchModels)
                {
                    SportMatchModelOut sportModel = sportList.Where(s => s.Id == match.Sport.Id).FirstOrDefault();
                    if (sportModel == null)
                    {
                        sportModel = new SportMatchModelOut()
                        {
                            Id = match.Sport.Id,
                            Name = match.Sport.Name,
                            Matches = new List<MatchSimpleModelOut>()
                        };
                        sportList.Add(sportModel);
                    }
                    sportModel.Matches.Add(match);
                }
                return Ok(sportList.Where(s=>s.Id==id).ToList());
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

        [HttpGet("byCompetitor/{id}", Name = "GetAllMatchesByCompetitorId")]
        public IActionResult GetAllByCompetitorId(int id, [FromHeader] string token)
        {
            try
            {
                Guid realToken = Guid.Parse(token);
                matchLogic.SetSession(realToken);
                competitorLogic.SetSession(realToken);
                Competitor competitor = competitorLogic.GetCompetitorById(id);
                ICollection<Match> matchList = matchLogic.GetAllMatchesForCompetitor(competitor);
                ICollection<MatchSimpleModelOut> matchModels = mapper.Map<ICollection<MatchSimpleModelOut>>(matchList);
                ICollection<SportMatchModelOut> sportList = new List<SportMatchModelOut>();
                foreach (MatchSimpleModelOut match in matchModels)
                {
                    SportMatchModelOut sportModel = sportList.Where(s => s.Id == match.Sport.Id).FirstOrDefault();
                    if (sportModel == null)
                    {
                        sportModel = new SportMatchModelOut()
                        {
                            Id = match.Sport.Id,
                            Name = match.Sport.Name,
                            Matches = new List<MatchSimpleModelOut>()
                        };
                        sportList.Add(sportModel);
                    }
                    sportModel.Matches.Add(match);
                }
                return Ok(sportList.ToList());
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
                return Ok("Modificado");
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
                return Ok(comment.Id);

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
                DateTime startDate = Convert.ToDateTime(fixtureData.Date);
                fixtureLogic.GenerateFixture(fixtureData.Pos, fixtureData.SportId, startDate);
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

        [HttpGet("fixtures", Name = "ChangeFixture")]
        public IActionResult GetFixtureImplementations([FromHeader] string token)
        {
            try
            {
                Guid realToken = Guid.Parse(token);
                fixtureLogic.SetSession(realToken);
                ICollection<string> message = fixtureLogic.RefreshFixtureImplementations();
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