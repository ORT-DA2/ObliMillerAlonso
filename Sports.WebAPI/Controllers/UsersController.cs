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
        private IMapper mapper;

        public UsersController(IUserLogic aUserLogic, ISessionLogic aSessionLogic, IFavoriteLogic aFavoriteLogic)
        {
            favoriteLogic = aFavoriteLogic;
            userLogic = aUserLogic;
            sessionLogic = aSessionLogic;
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

        [HttpGet(Name = "GetUsersAll")]
        public IActionResult GetAllUsers([FromHeader] string token)
        {
            try
            {
                Guid realToken = Guid.Parse(token);
                userLogic.SetSession(realToken);
                ICollection<User> userList = userLogic.GetAll();
                ICollection<UserModelOut> userModels = new List<UserModelOut>();
                foreach (User user in userList)
                {
                    UserModelOut model = mapper.Map<UserModelOut>(user);
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
                return RedirectToRoute("GetUserById", new { id = userId, token = token });
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


        [HttpPost("favoriteTeams", Name = "SetFavoriteTeam")]
        public IActionResult PostFavorite([FromBody] TeamModelIn teamIn, [FromHeader] string token)
        {
            try
            {
                Guid realToken = Guid.Parse(token);
                favoriteLogic.SetSession(realToken);
                Team team = mapper.Map<Team>(teamIn);
                favoriteLogic.AddFavoriteTeam(team);
                return RedirectToRoute("GetFavoritesTeams", new {token = token });
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

        [HttpGet("favoriteTeams", Name = "GetFavoritesTeams")]
        public IActionResult GetFavoriteTeams([FromHeader] string token)
        {
            try
            {
                Guid realToken = Guid.Parse(token);
                favoriteLogic.SetSession(realToken);
                ICollection<Team> favoriteTeams = favoriteLogic.GetFavoritesFromUser();
                ICollection<TeamModelOut> teamModels = new List<TeamModelOut>();
                foreach (Team team in favoriteTeams)
                {
                    TeamModelOut model = mapper.Map<TeamModelOut>(team);
                    teamModels.Add(model);
                }
                return Ok(teamModels);
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

        [HttpGet("favoriteComments", Name = "GetFavoritesTeamsComents")]
        public IActionResult GetFavoritesTeamsComents([FromHeader] string token)
        {
            try
            {
                Guid realToken = Guid.Parse(token);
                favoriteLogic.SetSession(realToken);
                ICollection<Comment> favoriteTeamsComments = favoriteLogic.GetFavoritesTeamsComments();
                ICollection<CommentModelOut> commentModels = new List<CommentModelOut>();
                foreach (Comment comment in favoriteTeamsComments)
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
    }
}