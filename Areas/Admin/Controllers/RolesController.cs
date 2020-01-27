using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CmsShoppingCart.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CmsShoppingCart.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class RolesController : Controller
    {

        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            this._roleManager = roleManager;
            this._userManager = userManager;
        }

        // GET /admin/roles
        public IActionResult Index() => View(_roleManager.Roles);
           
        // GET /admin/roles/create
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        // POST /admin/roles/create
        public async Task<IActionResult> Create([MinLength(2),Required] string name)
        {
            if(ModelState.IsValid)
            {
                IdentityResult result = await _roleManager.CreateAsync(new IdentityRole(name));
                if(result.Succeeded)
                {
                    TempData["Success"] = "The role has been created";
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach(IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("",error.Description);
                    }
                }
            }
            
            
                ModelState.AddModelError("","Minimum length is 2");
                return View();
            
        }

        
        // GET /admin/roles/edit/5
        public async Task<IActionResult> Edit(string id)
        {
            IdentityRole role = await _roleManager.FindByIdAsync(id);

            List<AppUser> members= new List<AppUser>();
            List<AppUser> noneMembers= new List<AppUser>();

            foreach(var user in _userManager.Users)
            {
                var list = await _userManager.IsInRoleAsync(user, role.Name)? members : noneMembers;
                list.Add(user);
            }

            return View(new RoleEdit() {Role = role, Members = members, NoneMembers = noneMembers});
        }

        // POST /admin/roles/edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RoleEdit roleEdit)
        {
            IdentityResult result;

            foreach(string userId in roleEdit.AddIds ?? new string[]{})
            {
                AppUser user = await _userManager.FindByIdAsync(userId);
                result = await _userManager.AddToRoleAsync(user, roleEdit.RoleName);
            }


            foreach (string userId in roleEdit.DeleteIds ?? new string[] { })
            {
                AppUser user = await _userManager.FindByIdAsync(userId);
                result = await _userManager.RemoveFromRoleAsync(user, roleEdit.RoleName);
            }

            return Redirect(
                    Request.Headers["Referer"]
                           .ToString());
        }

    }
}