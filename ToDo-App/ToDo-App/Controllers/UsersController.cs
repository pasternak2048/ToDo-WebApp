using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDo_App.Enums;
using ToDo_App.Models;
using ToDo_App.Models.Abstractions;
using ToDo_App.Repositories;

namespace ToDo_App.Controllers
{
    public class UsersController : Controller
    {
        UnitOfWork unitOfWork;
        private const int _pageSize = 7;
        public UsersController(ToDoContext context)
        {
            unitOfWork = new UnitOfWork(context);
        }

        [Authorize(Roles = "admin")]
        public IActionResult Index(string searchString, UsersSortState sortOrder = UsersSortState.EmailAsc, int page = 1)
        {
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            var items = unitOfWork.Users.GetAll();

            ViewData["EmailSort"] = sortOrder == UsersSortState.EmailAsc ? UsersSortState.EmailDesc : UsersSortState.EmailAsc;
            ViewData["SearchFilter"] = searchString;

            if (!String.IsNullOrEmpty(searchString))
            {
                items = items.Where(s => s.LastName.Contains(searchString)
                                       || s.FirstName.Contains(searchString));
            }

            var count = items.Count();

            items = items.Skip((page - 1) * _pageSize).Take(_pageSize).ToList();

            items = GetSorted(items, sortOrder);

            var pageViewModel = new PageViewModel(count, page, _pageSize);
            var viewModel = new IndexViewModel<User>
            {
                PageViewModel = pageViewModel,
                Items = items
            };

            return View(viewModel);
        }


        [Authorize(Roles = "admin")]
        public IActionResult Details(int? id, int page)
        {
            ViewData["Page"] = page;
            if (id == null)
            {
                return NotFound();
            }

            var user = unitOfWork.Users.Get(id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }


        [HttpGet]
        [Authorize(Roles = "admin")]
        public IActionResult Create(int page)
        {
            ViewData["Page"] = page;
            ViewBag.Roles = new SelectList(unitOfWork.Roles.GetAll(), "Id", "Name");
            return View();
        }


        [HttpPost]
        [Authorize(Roles = "admin")]
        public IActionResult Create([Bind("Id,Email,Password,LastName,FirstName,Address,DateOfRegistration,RoleId")] User user, int page)
        {
            if(unitOfWork.Users.GetAll().Any(x => x.Email == user.Email))
            {
                ModelState.AddModelError("", "Incorrect Email");
                return View(user);
            }

            if (ModelState.IsValid)
            {
                user.DateOfRegistration = DateTimeOffset.Now;
                unitOfWork.Users.Create(user);
                unitOfWork.Save();

                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }



        [Authorize(Roles = "admin, user")]
        public IActionResult Edit(int? id, int page)
        {
            ViewData["Page"] = page;
            ViewBag.Roles = new SelectList(unitOfWork.Roles.GetAll(), "Id", "Name");
            if (id == null)
            {
                return NotFound();
            }

            var user = unitOfWork.Users.Get(id);
            if (user == null || user.Id.ToString() != User.Identity.Name
                && !User.IsInRole("admin"))
            {
                return NotFound();
            }

            return View(user);
        }


        [HttpPost]
        [Authorize(Roles = "admin, user")]
        public IActionResult Edit(int id, [Bind("Id,Email,Password,LastName,FirstName,Address,DateOfRegistration,RoleId")] User user)
        {
            if (id != user.Id || user.Id.ToString() != User.Identity.Name
                && !User.IsInRole("admin"))
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    unitOfWork.Users.Update(user);
                    unitOfWork.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                if(User.IsInRole("admin"))
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
                
            }

            return View(user);
        }



        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id, int page)
        {
            ViewData["Page"] = page;

            if (id == null)
            {
                return NotFound();
            }

            var user = unitOfWork.Users.Get(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }


        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "admin")]
        public IActionResult DeleteConfirmed(int id, int page)
        {
            ViewData["Page"] = page;

            unitOfWork.Users.Delete(id);
            unitOfWork.Save();
            return RedirectToAction("Index", page);
        }



        private bool UserExists(int id)
        {
            return unitOfWork.Users.GetAll().Any();
        }


        private IEnumerable<User> GetSorted(IEnumerable<User> items, UsersSortState sortOrder)
        {
            items = sortOrder switch
            {
                UsersSortState.EmailAsc => items.OrderBy(s => s.Email),
                UsersSortState.EmailDesc => items.OrderByDescending(s => s.Email)
            };

            return items;
        }
    }
}
