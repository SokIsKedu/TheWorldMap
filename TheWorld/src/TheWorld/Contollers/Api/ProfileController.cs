﻿using System;
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
using Newtonsoft.Json.Linq;

namespace TheWorld.Contollers.Api
{
    [Route("/api/profile")]
    public class ProfileController: Controller
    {
        private IWorldRepository _repository;
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
            // Something wrong with getting the user.
            var user = await GetCurrentUserAsync();
            var userId = user?.Id;
            return Json(user);
        }

        [HttpPost("")]
        public IActionResult Post([FromBody]JToken usr)
        {
            WorldUser pim = usr.ToObject<WorldUser>();
            _repository.UpdateUser(pim);
            return Ok();
        }

        [HttpPost("/api/profile/upload")]
        public async Task<IActionResult> Upload(ICollection<IFormFile> files)
        {
            Console.WriteLine(_environment.WebRootPath);
            string uploads = _environment.WebRootPath + "\\img\\profilePic";
            Console.WriteLine(_environment.WebRootPath + "\\img\\profilePic");
            foreach (var file in files)
            {
                if (file.Length > 0 && file.ContentType.StartsWith("image"))
                {
                    var usr = await GetCurrentUserAsync();
                    Console.WriteLine(usr.GetType());
                    var userId = usr?.Id;
                    var fileName = "pic" + usr?.Id + ".jpg";
                    try
                    {
                        using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }
                        usr.ProfilePic = fileName;
                        _repository.UpdateUser(usr);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Data);
                    }
                }
            }
            return Redirect("/App/Profile");
        }

        private Task<WorldUser> GetCurrentUserAsync()
        {
            var a = _userManager.GetUserAsync(HttpContext.User);
            return a;
        }
    }
}
