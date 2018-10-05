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
        public IActionResult PostFavorite([FromBody] TeamModelIn teamIn, string token)
        {
            try
            {
                Guid realToken = Guid.Parse(token);
                favoriteLogic.SetSession(realToken);
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
        public IActionResult GetFavoritesForUser(string token)
        {
            Guid realToken = Guid.Parse(token);
            favoriteLogic.SetSession(realToken);
            ICollection<Team> favoriteTeams = favoriteLogic.GetFavoritesFromUser();
            return Ok(favoriteTeams.ToList());
        }

        [HttpGet("FavoriteComments", Name = "GetFavoritesTeamsComents")]
        public IActionResult GetFavoritesTeamsComents(string token)
        {
            Guid realToken = Guid.Parse(token);
            favoriteLogic.SetSession(realToken);
            ICollection<Comment> favoriteTeamsComments = favoriteLogic.GetFavoritesTeamsComments();
            return Ok(favoriteTeamsComments.ToList());
        }
        
    }
}