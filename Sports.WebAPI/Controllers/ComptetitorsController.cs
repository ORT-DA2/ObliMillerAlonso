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
using Sports.Logic.Interface.Exceptions;
using Sports.Repository.Interface.Exceptions;
using System.Web;
using AutoMapper;

namespace Sports.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompetitorsController : ControllerBase
    {
        private ICompetitorLogic competitorLogic;
        private ISportLogic sportLogic;
        private IMapper mapper;

        public CompetitorsController(ICompetitorLogic aCompetitorLogic, ISportLogic aSportLogic)
        {
            competitorLogic = aCompetitorLogic;
            sportLogic = aSportLogic;
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
            mapper = new Mapper(config);
        }

        [HttpGet("{id}", Name = "GetCompetitorById")]
        public IActionResult Get(int id, [FromHeader] string token)
        {
            try
            {
                Guid realToken = Guid.Parse(token);
                competitorLogic.SetSession(realToken);
                Competitor competitor = competitorLogic.GetCompetitorById(id);
                CompetitorModelOut modelOut = mapper.Map<CompetitorModelOut>(competitor);
                return Ok(modelOut);
            }
            catch (UnauthorizedException ex)
            {
                return Unauthorized();
            }
            catch (DomainException ex)
            {
                return UnprocessableEntity(ex.Message);
            }
            catch (LogicException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnknownDataAccessException ex)
            {
                return StatusCode((int)HttpStatusCode.ServiceUnavailable, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        
        [HttpPut("{id}", Name = "ModifyCompetitor")]
        public IActionResult PutCompetitor(int id, [FromBody] CompetitorModelIn competitorIn, [FromHeader] string token)
        {
            try
            {
                Guid realToken = Guid.Parse(token);
                competitorLogic.SetSession(realToken);
                Competitor competitor = mapper.Map<Competitor>(competitorIn);
                competitorLogic.Modify(id, competitor);
                return Ok("Modificado");
                //return RedirectToRoute("GetCompetitorById", new { id = id, token = token });
            }
            catch (UnauthorizedException ex)
            {
                return Unauthorized();
            }
            catch (DomainException ex)
            {
                return UnprocessableEntity(ex.Message);
            }
            catch (LogicException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnknownDataAccessException ex)
            {
                return StatusCode((int)HttpStatusCode.ServiceUnavailable, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id}", Name = "DeleteCompetitor")]
        public IActionResult DeleteCompetitor(int id, [FromHeader] string token)
        {
            try
            {
                Guid realToken = Guid.Parse(token);
                competitorLogic.SetSession(realToken);
                competitorLogic.Delete(id);
                return Ok("Competitor deleted succesfully");
            }
            catch (UnauthorizedException ex)
            {
                return Unauthorized();
            }
            catch (DomainException ex)
            {
                return UnprocessableEntity(ex.Message);
            }
            catch (LogicException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnknownDataAccessException ex)
            {
                return StatusCode((int)HttpStatusCode.ServiceUnavailable, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        [HttpGet(Name = "GetAllCompetitors")]
        public IActionResult GetAllCompetitors([FromHeader] string token, [FromHeader] string name, [FromHeader] string order)
        {
            try
            {
                Guid realToken = Guid.Parse(token);
                competitorLogic.SetSession(realToken);
                sportLogic.SetSession(realToken);
                ICollection<Competitor> competitorList = competitorLogic.GetFilteredCompetitors(name,order);
                ICollection<CompetitorModelOut> competitorModels = new List<CompetitorModelOut>();
                foreach (Competitor competitor in competitorList)
                {
                    CompetitorModelOut model = mapper.Map<CompetitorModelOut>(competitor);
                    competitorModels.Add(model);
                }
                return Ok(competitorModels.ToList());
            }
            catch (UnauthorizedException ex)
            {
                return Unauthorized();
            }
            catch (DomainException ex)
            {
                return UnprocessableEntity(ex.Message);
            }
            catch (LogicException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnknownDataAccessException ex)
            {
                return StatusCode((int)HttpStatusCode.ServiceUnavailable, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }


    }
}