using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CmsShoppingCart.Infrastructure;
using CmsShoppingCart.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CmsShoppingCart.Controllers
{
    public class ProductsController : Controller
    {

        private readonly CmsShoppingCartContext _context;

        public ProductsController(CmsShoppingCartContext context)
        {
            this._context = context;
        }

        // GET admin/products/
        public async Task<IActionResult> Index(int p = 1)
        {

            var pagesize = 6;
            var prducts = _context.Products.OrderBy(x => x.Id)
                                  .Skip((p - 1) * pagesize)
                                  .Take(pagesize);

            ViewBag.PageNumber = p;
            ViewBag.PageRange = pagesize;
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)_context.Products.Count() / pagesize);

            return View(await prducts.ToListAsync());
        } 
        
        // GET products/category/
        public async Task<IActionResult> ProductsbyCategory(string categoryslug,int p = 1)
        {
            Category category =await _context.Categories.Where(x=>x.Slug == categoryslug)
                                        .FirstOrDefaultAsync();

            if(category == null) return RedirectToAction("Index");
            
            var pagesize = 6;
            var prducts = _context.Products.OrderBy(x => x.Id).Where(x=>x.CategoryId== category.Id)
                                  .Skip((p - 1) * pagesize)
                                  .Take(pagesize);

            ViewBag.PageNumber = p;
            ViewBag.PageRange = pagesize;
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)_context.Products.Count(x => x.CategoryId == category.Id) / pagesize);
            ViewBag.CategoryName = category.Name;
            ViewBag.CategorySlug = category.Slug;
            return View(await prducts.ToListAsync());
        }

    }
}