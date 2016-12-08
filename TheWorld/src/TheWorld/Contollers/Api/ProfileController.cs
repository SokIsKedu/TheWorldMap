using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;

using TheWorld.Models;
using TheWorld.Services;
using TheWorld.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using Microsoft.Net.Http.Headers;

namespace TheWorld.Contollers.Api
{
    [Route("/api/profile")]
    public class ProfileController: Controller
    {
        IWorldRepository _repository;
        private UserManager<WorldUser> _userManager;
        private IHostingEnvironment _environment;
        public ProfileController(IWorldRepository repository,UserManager<WorldUser> userManager, IHostingEnvironment environment)
        {
            _repository = repository;
            _userManager = userManager;
            _environment = environment;
        }

        [HttpGet("")]
        public async Task<IActionResult> Get()
        {

            var user = await GetCurrentUserAsync();
            var userId = user?.Id;
            return Json(user);
        }



        [HttpPost("/api/profile/upload")]
        public async Task<IActionResult> Upload()
        {
            var files = HttpContext.Request.Form.Files;
            var uploads = Path.Combine(_environment.WebRootPath, "img\\profilePic");
            foreach (FormFile file in files)
            {
                if (file.Length > 0)
                {
                    var usr = await GetCurrentUserAsync();
                    var userId = usr?.Id;
                    var fileName = "pic" + usr?.Id + ".jpg";
                    try
                    {
                        using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                            Debug.WriteLine("eina");
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("err");
                    }
                }
                
            }
            return Redirect("/App/Profile");
        }





        




        private Task<WorldUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);
    }
}
