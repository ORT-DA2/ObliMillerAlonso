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
    public class FavoritesController : ControllerBase
    {
        private IFavoriteLogic favoriteLogic;
        private IUserLogic userLogic;
        private IMapper mapper;

        public FavoritesController(IUserLogic auserLogic, IFavoriteLogic aFavoriteLogic)
        {
            favoriteLogic = aFavoriteLogic;
            userLogic = auserLogic;
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
            mapper = new Mapper(config);
        }

        [HttpPost]
        public IActionResult PostFavorite([FromBody] TeamModelIn teamIn, [FromHeader] Guid token)
        {
            try
            {
                RequestHeaderIsNotNull(token);
                userLogic.SetSession(token);
                favoriteLogic.SetSession(token);
                RequestBodyIsNotNull(teamIn);
                Team team = mapper.Map<Team>(teamIn);
                favoriteLogic.AddFavoriteTeam(team);
                return Ok("Favorite added successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("FavoriteTeams",Name = "GetFavoritesForUser")]
        public IActionResult GetFavoritesForUser( [FromHeader] Guid token)
        {
            RequestHeaderIsNotNull(token);
            favoriteLogic.SetSession(token);
            ICollection<Team> favoriteTeams = favoriteLogic.GetFavoritesFromUser();
            if(favoriteTeams.Count == 0)
            {
                return NotFound();
            }
            return Ok(favoriteTeams.ToList());
        }

        [HttpGet("FavoriteComments", Name = "GetFavoritesTeamsComents")]
        public IActionResult GetFavoritesTeamsComents([FromHeader] Guid token)
        {
            RequestHeaderIsNotNull(token);
            favoriteLogic.SetSession(token);
            ICollection<Comment> favoriteTeamsComments = favoriteLogic.GetFavoritesTeamsComments();
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