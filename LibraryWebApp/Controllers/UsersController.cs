using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibraryWebApp.Data;
using LibraryWebApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace LibraryWebApp.Controllers
{
    [Authorize(Roles = "Administrator,Librarian")]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private IPasswordHasher<ApplicationUser> _passwordHasher;

        public UsersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IPasswordHasher<ApplicationUser> passwordHasher)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _passwordHasher = passwordHasher;
        }

        // GET: User
        public IActionResult Index()
        {
            var users = _userManager.Users;
            List<UserViewModel> userList = new List<UserViewModel>();
            foreach(var user in users)
            {
                UserViewModel userViewModel = new UserViewModel
                {
                    User = user,
                    UserId = user.Id,
                    Roles = [.. _userManager.GetRolesAsync(user).Result],
                };
                userList.Add(userViewModel);
            }
            return View(userList);
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(string? id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            UserViewModel userViewModel = new UserViewModel
            {
                User = user,
                UserId = user.Id,
            };

            return View(userViewModel);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id, FirstName, LastName, UserName, Email, DateOfBirth, Password")] ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(string? id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, string FirstName, string LastName, string UserName, string Email, DateOnly DateOfBirth)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    user.FirstName = FirstName;
                    user.LastName = LastName;
                    user.UserName = UserName;
                    user.Email = Email;
                    user.DateOfBirth = DateOfBirth;

                    await _userManager.UpdateAsync(user);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationUserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_userManager.Users, "Id", "Id", user.Id);
            return View(user);
        }

        // GET: Users/EditRole/5
        public async Task<IActionResult> EditRole(string? id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return RedirectToAction("Index");
            }

            UserViewModel userViewModel = new UserViewModel
            {
                User = user,
                UserId = user.Id,
                Roles = [.. _userManager.GetRolesAsync(user).Result],
            };

            return View(userViewModel);
        }

        // POST: Users/EditRole/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRole(string id, List<string> RoleList)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var roles = _roleManager.Roles.ToList();
            List<string> userRoles = [.. _userManager.GetRolesAsync(user).Result];
            if (ModelState.IsValid)
            {
                try
                {
                    foreach (var role in roles)
                    {
                        if(!userRoles.Contains(role.Name) && RoleList.Contains(role.Name))
                        {
                            await _userManager.AddToRoleAsync(user, role.Name);
                        } else if(userRoles.Contains(role.Name) && !RoleList.Contains(role.Name))
                        {
                            await _userManager.RemoveFromRoleAsync(user, role.Name);
                        }
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationUserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_userManager.Users, "Id", "Id", user.Id);
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            try
            {
                await _userManager.DeleteAsync(user);
            }
            catch (Exception ex) {
                throw;
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ApplicationUserExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
