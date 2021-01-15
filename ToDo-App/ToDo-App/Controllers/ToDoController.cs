using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDo_App.Models;
using ToDo_App.Repositories;

namespace ToDo_App.Controllers
{
    public class ToDoController : Controller
    {
        UnitOfWork unitOfWork;

        public ToDoController(ToDoContext context)
        {
            unitOfWork = new UnitOfWork(context);
        }

        [Authorize(Roles = "admin, user")]
        public async Task<IActionResult> Index()
        {
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;

            if (currentUser.IsInRole("user"))
            {
                var shoppingContext = unitOfWork.ToDos.GetAll().Where(i => i.User.Email == User.Identity.Name);
                return View(shoppingContext.ToList());
            }
            else
            {
                var shoppingContext = unitOfWork.ToDos.GetAll();
                return View(shoppingContext.ToList());
            }
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todo = unitOfWork.ToDos.Get(id);
                
            if (todo == null)
            {
                return NotFound();
            }

            return View(todo);
        }



        [HttpGet]
        [Authorize(Roles = "admin, user")]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [Authorize(Roles = "admin, user")]
        public async Task<IActionResult> Create([Bind("Id,TaskName,TaskDescription,Deadline,UserId")] ToDo todo)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.ToDos.Create(todo);
                unitOfWork.Save();

                return RedirectToAction(nameof(Index));
            }

            return View(todo);
        }





    }
}
