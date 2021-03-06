﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CmsShoppingCart.Infrastructure;
using CmsShoppingCart.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CmsShoppingCart.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class CategoriesController : Controller
    {

        private readonly CmsShoppingCartContext _context;

        public CategoriesController(CmsShoppingCartContext context)
        {
            _context = context;
        }

   
        // GET admin/categories/
        public async Task<IActionResult> Index()
        {
            return View(await _context.Categories.OrderBy(x=>x.Sorting).ToListAsync());
        }

        public IActionResult Create() => View();


        [HttpPost]
        [ValidateAntiForgeryToken]
        // POST admin/categories/create
        public async Task<IActionResult> Create(Category category)
        {

            if (ModelState.IsValid)
            {
                category.Slug = category.Name.ToLower()
                                .Replace(" ", "-");
                category.Sorting = 100;

                var slug = await _context.Categories.FirstOrDefaultAsync(x => x.Slug == category.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "The category already exists");
                    return View(category);
                }

                _context.Add(category);
                await _context.SaveChangesAsync();

                TempData["Success"] = "The category has been created";

                return RedirectToAction("Index");
            }

            return View(category);

        }


        //GET /admin/pages/edit/id
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) { return NotFound(); }
            return View(category);

        }

        //POST /admin/categories/edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category category)
        {

            if (ModelState.IsValid)
            {
                category.Slug = category.Name .ToLower()
                                          .Replace(" ", "-");


                var slug = await _context.Pages.Where(x => x.Id != id).FirstOrDefaultAsync(x => x.Slug == category.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "The page category exists");
                    return View(category);
                }

                _context.Update(category);
                await _context.SaveChangesAsync();

                TempData["Success"] = "The page has been edited";

                return RedirectToAction("Edit", new {id});
            }

            return View(category);

        }

        //GET /admin/pages/delete/id
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) { TempData["Error"] = "The category does not exist"; }
            else
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();

                TempData["success"] = "The category has been removed";

            }

            return RedirectToAction("Index");


        }


        //POST /admin/categories/reorder
        [HttpPost]
        public async Task<IActionResult> Reorder(int[] id)
        {

            int count = 1;

            foreach (var categoryId in id)
            {
                Category category = await _context.Categories.FindAsync(categoryId);
                category.Sorting = count;
                _context.Update(category);
                await _context.SaveChangesAsync();
                count++;
            }

            return Ok();

        }

    }
}