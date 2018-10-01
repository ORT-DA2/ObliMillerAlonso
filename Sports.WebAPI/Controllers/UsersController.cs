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
        private IMapper mapper;

        public UsersController(IUserLogic aUserLogic, IMapper aMapper)
        {
            userLogic = aUserLogic;
            mapper = aMapper;
        }


        // GET api/values/5
        [HttpGet("{id}", Name = "GetById")]
        public IActionResult Get(int id)
        {
            User user = userLogic.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            UserModelOut modelOut = mapper.Map<UserModelOut>(user);
            return Ok(modelOut);
        }


        // GET api/values
        [HttpGet(Name = "GetAll")]
        public IActionResult GetAll()
        {
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

        // POST api/values
        [HttpPost]
        public IActionResult PostUser([FromBody] UserModelIn userIn)
        { 
            try
            {
                RequestBodyIsNotNull(userIn);
                User user = mapper.Map<User>(userIn);
                userLogic.AddUser(user);
                UserModelOut modelOut = mapper.Map<UserModelOut>(user);
                return Ok(modelOut);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }


        // PUT api/values
        [HttpPost]
        public IActionResult PutUser(int userId, [FromBody]UserModelIn newUser)
        {
            try
            {
                RequestBodyIsNotNull(newUser);
                User user = mapper.Map<User>(newUser);
                userLogic.UpdateUser(userId, user);
                return Ok("Usuario modificado");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        // DELETE api/values
        [HttpPost]
        public IActionResult DeleteUser(int userId)
        {
            try
            {
                userLogic.RemoveUser(userId);
                return Ok("Usuario eliminado");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        private void RequestBodyIsNotNull(object Object)
        {
            if (Object == null)
                throw new ArgumentNullException("Alguno de los parámetros es invalido. Corrija bien los campos");
        }


    }
}