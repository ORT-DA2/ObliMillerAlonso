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
using Newtonsoft.Json.Linq;

namespace Sports.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUserLogic userLogic;
        private IFavoriteLogic favoriteLogic;
        private ISessionLogic sessionLogic;
        private ILogLogic logLogic;
        private IMapper mapper;

        public UsersController(IUserLogic aUserLogic, ISessionLogic aSessionLogic, IFavoriteLogic aFavoriteLogic, ILogLogic aLogLogic)
        {
            favoriteLogic = aFavoriteLogic;
            userLogic = aUserLogic;
            sessionLogic = aSessionLogic;
            logLogic = aLogLogic;
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
            mapper = new Mapper(config);
        }

        [HttpGet("{id}", Name = "GetUserById")]
        public IActionResult GetUser(int id, [FromHeader] string token)
        {
            try
            {
                Guid realToken = Guid.Parse(token);
                userLogic.SetSession(realToken);
                User user = userLogic.GetUserById(id);
                UserFullModelOut modelOut = mapper.Map<UserFullModelOut>(user);
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


        [HttpGet("current", Name = "GetUserByToken")]
        public IActionResult GetCurrentUser([FromHeader] string token)
        {
            try
            {
                Guid realToken = Guid.Parse(token);
                User user = sessionLogic.GetUserFromToken(realToken);
                UserFullModelOut modelOut = mapper.Map<UserFullModelOut>(user);
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

        [HttpGet(Name = "GetUsersAll")]
        public IActionResult GetAllUsers([FromHeader] string token)
        {
            try
            {
                Guid realToken = Guid.Parse(token);
                userLogic.SetSession(realToken);
                ICollection<User> userList = userLogic.GetAll();
                ICollection<UserFullModelOut> userModels = new List<UserFullModelOut>();
                foreach (User user in userList)
                {
                    UserFullModelOut model = mapper.Map<UserFullModelOut>(user);
                    userModels.Add(model);
                }
                return Ok(userModels.ToList());
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

        [HttpPost(Name = "CreateUser")]
        public IActionResult PostUser([FromBody] UserModelIn userIn, [FromHeader] string token)
        {
            try
            {
                Guid realToken = Guid.Parse(token);
                userLogic.SetSession(realToken);
                User user = mapper.Map<User>(userIn);
                userLogic.AddUser(user);
                UserModelOut modelOut = mapper.Map<UserModelOut>(user);
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

        [HttpPut("{userId}", Name = "ModifyUser")]
        public IActionResult PutUser(int userId, [FromBody]UserModelIn newUser, [FromHeader] string token)
        {
            try
            {
                Guid realToken = Guid.Parse(token);
                userLogic.SetSession(realToken);
                User user = mapper.Map<User>(newUser);
                userLogic.UpdateUser(userId, user);
                UserModelOut modelOut = mapper.Map<UserModelOut>(user);
                return Ok("Modificado");
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

        [HttpDelete("{userId}", Name = "DeleteUser")]
        public IActionResult DeleteUser(int userId, [FromHeader] string token)
        {
            try
            {
                Guid realToken = Guid.Parse(token);
                userLogic.SetSession(realToken);
                userLogic.RemoveUser(userId);
                return Ok("User deleted");
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
        
        [HttpPost("login", Name = "LoginUser")]
        public IActionResult Login([FromBody]LoginDTO modelIn)
        {
            try
            {
                Guid token = sessionLogic.LogInUser(modelIn.Username, modelIn.Password);
                logLogic.AddEntry("Login", modelIn.Username, DateTime.Now);
                string tokenString = token.ToString();
                TokenModelOut modelOut = new TokenModelOut() { Token = tokenString };
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
        [HttpDelete("logout", Name = "LogoutUser")]
        public IActionResult Logout([FromHeader] string token)
        {
            try
            {
                Guid realToken = Guid.Parse(token);
                sessionLogic.LogoutByToken(realToken);
                return Ok("Succesfully logged out");
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


        [HttpPost("favoriteCompetitors", Name = "SetFavoriteCompetitor")]
        public IActionResult PostFavorite([FromBody] CompetitorModelIn competitorIn, [FromHeader] string token)
        {
            try
            {
                Guid realToken = Guid.Parse(token);
                favoriteLogic.SetSession(realToken);
                Competitor competitor = mapper.Map<Competitor>(competitorIn);
                favoriteLogic.AddFavoriteCompetitor(competitor);

                return Ok("Modificado");
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

        [HttpGet("favoriteCompetitors", Name = "GetFavoritesCompetitors")]
        public IActionResult GetFavoriteCompetitors([FromHeader] string token)
        {
            try
            {
                Guid realToken = Guid.Parse(token);
                favoriteLogic.SetSession(realToken);
                ICollection<Competitor> favoriteCompetitors = favoriteLogic.GetFavoritesFromUser();
                ICollection<CompetitorModelOut> competitorModels = new List<CompetitorModelOut>();
                foreach (Competitor competitor in favoriteCompetitors)
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

        [HttpGet("favoriteComments", Name = "GetFavoritesCompetitorsComents")]
        public IActionResult GetFavoritesCompetitorsComents([FromHeader] string token)
        {
            try
            {
                Guid realToken = Guid.Parse(token);
                favoriteLogic.SetSession(realToken);
                ICollection<Comment> favoriteCompetitorsComments = favoriteLogic.GetFavoritesCompetitorsComments();
                ICollection<CommentModelOut> commentModels = new List<CommentModelOut>();
                foreach (Comment comment in favoriteCompetitorsComments)
                {
                    CommentModelOut model = mapper.Map<CommentModelOut>(comment);
                    commentModels.Add(model);
                }
                return Ok(commentModels.ToList());
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

        [HttpPost("log", Name = "LogBetweenDates")]
        public IActionResult LogBetweenDates([FromHeader] string token, [FromBody] LogDatesDTO dates)
        {
            try
            {
                IActionResult result = GetCurrentUser(token);
                var code = result as ObjectResult;
                if(code.StatusCode == 200)
                {
                    var okResult = result as ObjectResult;
                    var modelOut = okResult.Value as UserFullModelOut;
                    if (modelOut.isAdmin)
                    {
                        return Ok(logLogic.GetBetweenDates(mapper.Map<DateTime>(dates.StartDate), mapper.Map<DateTime>(dates.FinishDate)));
                    }
                    return Unauthorized();
                }
                return result;
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
    }
}