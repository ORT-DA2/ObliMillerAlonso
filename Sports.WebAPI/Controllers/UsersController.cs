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

namespace Sports.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUserLogic userLogic;

        public UsersController(IUserLogic aUserLogic)
        {
            userLogic = aUserLogic;
        }


        // GET api/values/5
        [HttpGet("{id}", Name = "GetById")]
        public ActionResult<User> Get(int id)
        {
            User user = userLogic.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            UserModel model = MapUserToModel(user);
            return user;
        }

        private UserModel MapUserToModel(User user)
        {
            return new UserModel()
            {
                Id = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email
            };
        }

        // POST api/values
        [HttpPost]
        public IActionResult PostUser([FromBody] User userIn)
        { 
            try
            {
                RequestBodyIsNotNull(userIn);
                userLogic.AddUser(userIn);
                return CreatedAtRoute("GetById", new { id = userIn.Id });
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }


        // PUT api/values
        [HttpPost]
        public IActionResult PutUser(int userId, [FromBody]User newUser)
        {
            try
            {
                RequestBodyIsNotNull(newUser);
                userLogic.UpdateUser(userId, newUser);
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
                throw new ArgumentNullException("Alguno de los parámetros es invalido.");
        }

    }
}