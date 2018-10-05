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
    public class SportsController : ControllerBase
    {
        private ITeamLogic teamLogic;
        private ISportLogic sportLogic;
        private IMapper mapper;

        public SportsController(ITeamLogic aTeamLogic, ISportLogic aSportLogic)
        {
            teamLogic = aTeamLogic;
            sportLogic = aSportLogic;
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
            mapper = new Mapper(config);
        }

        [HttpGet("{id}", Name = "GetById")]
        public IActionResult Get(int id, [FromHeader] Guid token)
        {
            RequestHeaderIsNotNull(token);
            sportLogic.SetSession(token);
            Sport sport = sportLogic.GetSportById(id);
            SportModelOut modelOut = mapper.Map<SportModelOut>(sport);
            return Ok(modelOut);
        }

        [HttpGet(Name = "GetAll")]
        public IActionResult GetAll([FromHeader] Guid token)
        {
            RequestHeaderIsNotNull(token);
            sportLogic.SetSession(token);
            ICollection<Sport> sportList = sportLogic.GetAll();
            ICollection<SportModelOut> teamModels = new List<SportModelOut>();
            return Ok(teamModels.ToList());
        }
        

        private void RequestHeaderIsNotNull(object Object)
        {
            if (Object == null)
                throw new ArgumentNullException("Invalid parameters, check the fields.");
        }

    }
}