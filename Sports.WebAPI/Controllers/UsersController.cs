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
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUserLogic userLogic;
        private IMapper mapper;

        public UsersController(IUserLogic aUserLogic)
        {
            userLogic = aUserLogic;
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
            mapper = new Mapper(config);
        }

        [HttpGet("{id}", Name = "GetById")]
        public IActionResult Get(int id, [FromHeader] Guid token)
        {
            RequestHeaderIsNotNull(token);
            userLogic.SetSession(token);
            User user = userLogic.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            UserModelOut modelOut = mapper.Map<UserModelOut>(user);
            return Ok(modelOut);
        }

        [HttpGet(Name = "GetAll")]
        public IActionResult GetAll([FromHeader] Guid token)
        {
            RequestHeaderIsNotNull(token);
            userLogic.SetSession(token);
            ICollection<User> userList = userLogic.GetAll();
            if (userList == null)
            {
                return NotFound();
            }
            ICollection<UserModelOut> userModels = new List<UserModelOut>();
            foreach (User user in userList)
            {
                UserModelOut model = mapper.Map<UserModelOut>(user);
                userModels.Add(model);
            }
            return Ok(userModels.ToList());
        }

        [HttpPost]
        public IActionResult PostUser([FromBody] UserModelIn userIn, [FromHeader] Guid token)
        {
            try
            {
                RequestHeaderIsNotNull(token);
                userLogic.SetSession(token);
                RequestBodyIsNotNull(userIn);
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
        public IActionResult PutUser([FromHeader] int userId, [FromBody]UserModelIn newUser, [FromHeader] Guid token)
        {
            try
            {
                RequestHeaderIsNotNull(token);
                userLogic.SetSession(token);
                RequestBodyIsNotNull(newUser);
                User user = mapper.Map<User>(newUser);
                userLogic.UpdateUser(userId, user);
                return Ok("User modified correctly.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpDelete]
        public IActionResult DeleteUser([FromHeader] int userId, [FromHeader] Guid token)
        {
            try
            {
                RequestHeaderIsNotNull(token);
                userLogic.SetSession(token);
                userLogic.RemoveUser(userId);
                return Ok("User deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        private void RequestBodyIsNotNull(object Object)
        {
            if (Object == null)
                throw new ArgumentNullException("Invalid parameters, check the fields.");
        }

        private void RequestHeaderIsNotNull(object Object)
        {
            if (Object == null)
                throw new ArgumentNullException("Invalid parameters, check the fields.");
        }

    }
}