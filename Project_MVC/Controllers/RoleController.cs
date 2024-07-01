using Demo.DAL.Entities;
using Demo.PL.Helpers;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
        public async Task<IActionResult> Index(string SearchValue)
        {
            var roles = Enumerable.Empty<IdentityRole>().ToList();
            if (string.IsNullOrEmpty(SearchValue))
            {
                roles.AddRange(_roleManager.Roles);
            }
            else
            {
                var _role = await _roleManager.FindByNameAsync(SearchValue);
                if (_role != null) { roles.Add(_role); }

            }

            return View(roles);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(IdentityRole role)
        {
            if (ModelState.IsValid)
            {
                await _roleManager.CreateAsync(role);
                return RedirectToAction(nameof(Index));
            }
            return View(role);
        }
        public async Task<IActionResult> Details(string id, string ViewName = "Details")
        {
            if (id == null)
                return NotFound();
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
                return NotFound();
            return View(ViewName, role);
        }
        public async Task<IActionResult> Edit(string id)
        {
            return await Details(id, "Edit");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string id, IdentityRole updatedRole)
        {
            if (id != updatedRole.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    var role = await _roleManager.FindByIdAsync(id);
                    role.Name=updatedRole.Name;
                    await _roleManager.UpdateAsync(role);
                    return RedirectToAction(nameof(Index));
                }
                catch (System.Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(updatedRole);
                }
            }
            return View(updatedRole);
        }
        public async Task<IActionResult> Delete(string id)
        {
            return await Details(id, "Delete");
        }
        [HttpPost]
        public async Task<IActionResult> Delete([FromRoute] string id, IdentityRole deletedRole)
        {
            if (id != deletedRole.Id)
                return BadRequest();
            try
            {
                var role = await _roleManager.FindByIdAsync(id);
                await _roleManager.DeleteAsync(role);
                return RedirectToAction(nameof(Index));
            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(deletedRole);
            }
        }

    }
}
