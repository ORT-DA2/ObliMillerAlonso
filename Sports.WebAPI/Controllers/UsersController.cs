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
    public class UsersController : ControllerBase
    {
        private IUserLogic userLogic;
        private ISessionLogic sessionLogic;
        private IMapper mapper;

        public UsersController(IUserLogic aUserLogic, ISessionLogic aSessionLogic)
        {
            userLogic = aUserLogic;
            sessionLogic = aSessionLogic;
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
            mapper = new Mapper(config);
        }

        [HttpGet("{id}", Name = "GetById")]
        public IActionResult Get(int id, string token)
        {
            Guid realToken = Guid.Parse(token);
            userLogic.SetSession(realToken);
            User user = userLogic.GetUserById(id);
            UserModelOut modelOut = mapper.Map<UserModelOut>(user);
            return Ok(modelOut);
        }

        [HttpGet(Name = "GetAll")]
        public IActionResult GetAll(string token)
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

        [HttpPost]
        public IActionResult PostUser([FromBody] UserModelIn userIn, string token)
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
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPut]
        public IActionResult PutUser([FromHeader] int userId, [FromBody]UserModelIn newUser, string token)
        {
            try
            {
                Guid realToken = Guid.Parse(token);
                userLogic.SetSession(realToken);
                User user = mapper.Map<User>(newUser);
                userLogic.UpdateUser(userId, user);
                return Ok("User modified correctly.");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

        }

        [HttpDelete]
        public IActionResult DeleteUser([FromHeader] int userId, string token)
        {
            try
            {
                Guid realToken = Guid.Parse(token);
                userLogic.SetSession(realToken);
                userLogic.RemoveUser(userId);
                return Ok("User deleted");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

        }
        

        public IActionResult Login([FromBody]LoginModel modelIn)
        {
            try
            {
                Guid token = sessionLogic.LogInUser(modelIn.Username,modelIn.Password);
                return Ok(token.ToString());
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        public IActionResult Logout(string token)
        {
            try
            {
                Guid realToken = Guid.Parse(token);
                sessionLogic.LogoutByToken(realToken);
                return Ok("Succesfully logged out");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}