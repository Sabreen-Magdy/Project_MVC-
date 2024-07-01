using Demo.DAL.Entities;
using Demo.PL.Helpers;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UserController(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<IActionResult> Index(string SearchValue)
        {
            var users = Enumerable.Empty<ApplicationUser>().ToList();
            if (string.IsNullOrEmpty(SearchValue))
            {
                users.AddRange(_userManager.Users);
            }
            else
            {
                var _user = await _userManager.FindByEmailAsync(SearchValue);
                if (_user != null) { users.Add(_user); }
                
            }

            return View(users);
        }
        public async Task<IActionResult> Details(string id, string ViewName = "Details")
        {
            if (id == null)
                return NotFound();
            var user = await _userManager.FindByIdAsync(id); 
            if (user == null)
                return NotFound();
            return View(ViewName, user);
        }
        public async Task<IActionResult> Edit(string id)
        {
            return await Details(id, "Edit");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string id, ApplicationUser updatedUser)
        {
            if (id != updatedUser.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.FindByIdAsync(id);
                    user.UserName = updatedUser.UserName;
                    user.PhoneNumber = updatedUser.PhoneNumber;
                    await _userManager.UpdateAsync(user);
                    return RedirectToAction(nameof(Index));
                }
                catch (System.Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(updatedUser);
                }
            }
            return View(updatedUser);
        }
        public async Task<IActionResult> Delete(string id)
        {
            return await Details(id, "Delete");
        }
        [HttpPost]
        public async Task<IActionResult> Delete([FromRoute] string id, ApplicationUser deletedUser)
        {
            if (id != deletedUser.Id)
                return BadRequest();
            try
            {
                var user=await _userManager.FindByIdAsync(id);
                await _userManager.DeleteAsync(user);
                return RedirectToAction(nameof(Index));
            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(deletedUser);
            }
        }


    }
}
