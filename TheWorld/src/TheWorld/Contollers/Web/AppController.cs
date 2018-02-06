using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using TheWorld.ViewModels;
using TheWorld.Services;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using TheWorld.Models;
//using Microsoft.VisualStudio.Web.CodeGeneration;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;


// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TheWorld.Contollers.Web
{
    
    public class AppController : Controller
    {
        private IMailService _mailService;
        private IConfigurationRoot _config;
        public IWorldRepository _repository { get; set; }
        public ILogger _logger;
        public AppController(IMailService mailService,IConfigurationRoot config, IWorldRepository repository, ILogger<AppController> logger)
        {
            _mailService = mailService;
            _config = config;
            _repository = repository;
            _logger = logger;
        }
        [AllowAnonymous]
        public IActionResult Index()
        {         
                return View();    
        }

        [Authorize]
        public IActionResult Trips()
        {
            return View();
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Admin()
        {
            return View();
        }

        public IActionResult Profile()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Contact()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Contact(ContactViewModel model)
        {
            
            if (ModelState.IsValid)
            {                
                _mailService.SendMail("et88568@gmail.com", model.Email, " Subject", model.Message);
                ModelState.Clear();
                ViewBag.UserMessage = "Message Sent";
            }
            else {
                ViewBag.UserMessage = "Unable to send";
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                Debug.WriteLine(errors);
            }
            return View();
            }
        [AllowAnonymous]
        public IActionResult About()
        {
            Console.WriteLine("==============================================================================");
            Console.WriteLine(DateTime.Now);
            return View();
        }
    }
}
