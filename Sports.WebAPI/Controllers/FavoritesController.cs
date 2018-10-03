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
    [Route("api/favorites")]
    [ApiController]
    public class FavoritesController : ControllerBase
    {
        private IFavoriteLogic favoriteLogic;
        private IUserLogic userLogic;
        private IMapper mapper;

        public FavoritesController(IUserLogic auserLogic, IFavoriteLogic aFavoriteLogic, IMapper aMapper)
        {
            favoriteLogic = aFavoriteLogic;
            userLogic = auserLogic;
            mapper = aMapper;
        }

        [HttpPost]
        public IActionResult PostFavorite([FromBody] UserModelIn userIn, [FromBody] TeamModelIn teamIn, [FromHeader] Guid token)
        {
            try
            {
                RequestHeaderIsNotNull(token);
                userLogic.SetSession(token);
                favoriteLogic.SetSession(token);
                RequestBodyIsNotNull(userIn);
                RequestBodyIsNotNull(teamIn);
                User user = mapper.Map<User>(userIn);
                Team team = mapper.Map<Team>(teamIn);
                favoriteLogic.AddFavoriteTeam(user, team);
                return Ok("Favorite added successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("{id}", Name = "GetFavoritesForUser")]
        public IActionResult GetFavoritesForUser(int id, [FromHeader] Guid token)
        {
            RequestHeaderIsNotNull(token);
            favoriteLogic.SetSession(token);
            ICollection<Team> favoriteTeams = favoriteLogic.GetFavoritesFromUser(id);
            if(favoriteTeams.Count == 0)
            {
                return NotFound();
            }
            return Ok(favoriteTeams.ToList());
        }

        [HttpGet("{id}", Name = "GetFavoritesTeamsComents")]
        public IActionResult GetFavoritesTeamsComents(User user, [FromHeader] Guid token)
        {
            RequestHeaderIsNotNull(token);
            favoriteLogic.SetSession(token);
            ICollection<Comment> favoriteTeamsComments = favoriteLogic.GetFavoritesTeamsComments(user);
            if (favoriteTeamsComments.Count == 0)
            {
                return NotFound();
            }
            return Ok(favoriteTeamsComments.ToList());
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