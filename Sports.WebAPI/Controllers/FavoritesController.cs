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
        private IMapper mapper;

        public FavoritesController(IFavoriteLogic aFavoriteLogic, IMapper aMapper)
        {
            favoriteLogic = aFavoriteLogic;
            mapper = aMapper;
        }
    }
}