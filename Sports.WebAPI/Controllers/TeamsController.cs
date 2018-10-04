﻿using System;
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
    public class TeamsController : ControllerBase
    {
        private ITeamLogic teamLogic;
        private ISportLogic sportLogic;
        private IMapper mapper;

        public TeamsController(ITeamLogic aTeamLogic, ISportLogic aSportLogic)
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
            teamLogic.SetSession(token);
            Team team = teamLogic.GetTeamById(id);
            if (team == null)
            {
                return NotFound();
            }
            TeamModelOut modelOut = mapper.Map<TeamModelOut>(team);
            return Ok(modelOut);
        }

        [HttpGet(Name = "GetAll")]
        public IActionResult GetAll([FromHeader] Guid token)
        {
            RequestHeaderIsNotNull(token);
            teamLogic.SetSession(token);
            ICollection<Sport> sportList = sportLogic.GetAll();
            if (sportList == null)
            {
                return NotFound();
            }
            ICollection<TeamModelOut> teamModels = new List<TeamModelOut>();
            foreach (Sport sport in sportList)
            {
                foreach (Team team in sport.Teams)
                {
                    TeamModelOut model = mapper.Map<TeamModelOut>(team);
                    model.SportId = sport.Id;
                    teamModels.Add(model);
                }
            }
            return Ok(teamModels.ToList());
        }
        

        private void RequestHeaderIsNotNull(object Object)
        {
            if (Object == null)
                throw new ArgumentNullException("Invalid parameters, check the fields.");
        }

    }
}