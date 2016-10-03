using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TheWorld.Models;
using TheWorld.ViewModels;

namespace TheWorld.Contollers.Api
{
    [Route("api/trips")]
    [Authorize]
    public class TripsController: Controller
    {
        private ILogger<TripsController> _logger;
        private IWorldRepository _repository;
        public TripsController(IWorldRepository repository,ILogger<TripsController> logger )
        {
            _logger = logger;
            _repository = repository;
        }
        [HttpGet("")]
        public IActionResult Get()
        {

            var results = Mapper.Map<IEnumerable<TripViewModel>>
                (_repository.GetUserTripsWithStops(User.Identity.Name));
            
            return Json(results);

        }

        [HttpPost("")]
        public async Task<IActionResult> Post([FromBody]TripViewModel trip)
        {
            if (ModelState.IsValid)
            {
                var newTrip = Mapper.Map<Trip>(trip);
                newTrip.UserName = User.Identity.Name;
                _repository.AddTrip(newTrip);

                if (await _repository.SaveChangesAsync())
                {
                    Response.StatusCode = (int)HttpStatusCode.Created;
                    return Json(Mapper.Map<TripViewModel>(newTrip));
                }
                else {
                    return BadRequest("Failed to save changes to the database");
                }
                
            }
            return BadRequest(ModelState);
        }

        [HttpDelete("/api/trips/deltrip/{tripName}")]
        public async Task<IActionResult> Delete(string tripName)
        {
            try
            {
                
                
                _repository.RemoveTrip(tripName, User.Identity.Name);
                if (await _repository.SaveChangesAsync())
                {
                    return Ok();
                }
                
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to remove trip: {0}", ex);
            }
            return BadRequest("Failed to remove stop");

        }


    }
}
