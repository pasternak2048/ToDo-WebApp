using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        private const int _pageSize = 8;
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
