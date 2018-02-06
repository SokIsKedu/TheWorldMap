using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TheWorld.Models;
using Microsoft.Extensions.Logging;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TheWorld.Contollers.Api
{

    [Authorize(Roles = "Administrator")]
    [Route("/api/Admin")]
    public class AdminController : Controller
    {

        private ILogger<TripsController> _logger;
        IWorldRepository _repository;
        
        public AdminController(IWorldRepository repository, ILogger<TripsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        // GET: /<controller>/
        public IActionResult GetUsers()
        {
            var results = _repository.GetUsers();
            return Json(results);
        }

        [HttpGet("/api/Admin/getUser/{userName}")]
        public IActionResult GetUser(string userName)
        {
            var results = _repository.GetUser(userName);
            if (results != null)
            {
                return Json(results);
            }
            return BadRequest("Failed to find user");
        }


        [HttpDelete("/api/Admin/delUser/{userName}")]
        public async Task<IActionResult> Delete(string userName)
        {

            try
            {


                _repository.RemoveUser(userName);
                if (await _repository.SaveChangesAsync())
                {
                    return Ok();
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to remove user: {0}", ex);
            }
            return BadRequest("Failed to remove user");

        }
    }
}
