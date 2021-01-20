using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ToDo_App.Models;
using ToDo_App.Repositories;

namespace ToDo_App.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IStringLocalizer<HomeController> _localizer;
        UnitOfWork unitOfWork;

        public HomeController(IStringLocalizer<HomeController> localizer, ILogger<HomeController> logger, ToDoContext context)
        {
            _logger = logger;
            _localizer = localizer;
            unitOfWork = new UnitOfWork(context);
        }

        public IActionResult Index()
        {
            ViewData["WelcomeMessage"] = _localizer["WelcomeMessage"];
            ViewData["LastName"] = _localizer["LastName"];
            ViewData["FirstName"] = _localizer["FirstName"];
            ViewData["Email"] = _localizer["Email"];
            ViewData["Address"] = _localizer["Address"];
            ViewData["DateOfRegistration"] = _localizer["DateOfRegistration"];
            ViewData["Role"] = _localizer["Role"];
            ViewData["EditButton"] = _localizer["EditButton"];

            string test = _localizer["WelcomeMessage"];
            User currentUser = unitOfWork.Users.GetAll().Where(x => x.Id.ToString() == this.User.Identity.Name).First();
            return View(currentUser);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
