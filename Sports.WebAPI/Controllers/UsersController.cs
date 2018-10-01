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
            return user;
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody] User userIn)
        { 
            try
            {
                userLogic.AddUser(userIn);
                var addedUser = new UserModel() { Id = userIn.Id, UserName = userIn.UserName, FirstName = userIn.FirstName, LastName = userIn.LastName, Email = userIn.Email };
                return CreatedAtRoute("GetById", new { id = addedUser.Id }, addedUser);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }
        
    }
}