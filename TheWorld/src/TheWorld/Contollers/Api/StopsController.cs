using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheWorld.Models;
using TheWorld.Services;
using TheWorld.ViewModels;

namespace TheWorld.Contollers.Api
{
    [Authorize]
    [Route("/api/trips/{tripName}/stops")]
    public class StopsController : Controller
    {
        IWorldRepository _repository;
        ILogger<StopsController> _logger;
        private GeoCoordsService _coordsService;

        public StopsController(IWorldRepository repository, ILogger<StopsController> logger, GeoCoordsService coordsService)
        {
            _logger = logger;
            _repository = repository;
            _coordsService = coordsService;
        }


        [HttpGet("")]
        public IActionResult Get(string tripName, string username)
        {
            try
            {
                var trip = _repository.GetTripByName(tripName, User.Identity.Name);

                return Ok(Mapper.Map<IEnumerable<StopViewModel>>(trip.Stops.OrderBy(s => s.Order).ToList()));
            }
            catch(Exception ex) {
                _logger.LogError($"Failed to get stops: {0}", ex);
            }
            return BadRequest("Failed to get stops");
        }
        


        [HttpPost("/api/trips/{tripName}/stops/delstop")]
        public async Task<IActionResult> Delete(string tripName, [FromBody]StopViewModel stop)
        {
            try
            {
                // if (ModelState.IsValid)
                // {
                var newStop = Mapper.Map<Stop>(stop); 
                _repository.RemoveStop(newStop, tripName, User.Identity.Name);
                    if (await _repository.SaveChangesAsync())
                    {
                        return Ok();
                    }
              //  }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to remove stop: {0}", ex);
            }
            return BadRequest("Failed to remove stop");

        }





        [HttpPost("")]
        public async Task<IActionResult> Post(string tripName, [FromBody]StopViewModel vm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newStop = Mapper.Map<Stop>(vm);

                    var result = await _coordsService.GetCoordsAsync(newStop.Name);
                    if (!result.Success)
                    {
                        _logger.LogError(result.Message);
                    }
                    else
                    {
                        newStop.Latitude = result.Latitude;
                        newStop.Longitude = result.Longitude;
                    }

                    _repository.AddStop(tripName, newStop, User.Identity.Name);
                    if (await _repository.SaveChangesAsync())
                    {
                        return Created($"/api/trips/{tripName}/stops/{newStop.Name}", Mapper.Map<StopViewModel>(newStop));
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to save new stop: {0}", ex);
            }
            return BadRequest("Failed to save new stop");
        }
    }
}
