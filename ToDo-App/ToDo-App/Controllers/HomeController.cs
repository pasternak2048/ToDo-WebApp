using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        UnitOfWork unityOfWork;
        public HomeController(ILogger<HomeController> logger, ToDoContext context)
        {
            _logger = logger;
            unityOfWork = new UnitOfWork(context);
        }

        public IActionResult Index()
        {
            User currentUser = unityOfWork.Users.GetAll().Where(x => x.Email == this.User.Identity.Name).First();
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
