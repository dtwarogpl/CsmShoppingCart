﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CmsShoppingCart.Infrastructure;
using CmsShoppingCart.Models;
using Microsoft.AspNetCore.Mvc;

namespace CmsShoppingCart.Controllers
{
    public class CartController : Controller
    {

        private CmsShoppingCartContext _context;

        public CartController(CmsShoppingCartContext context)
        {
            _context = context;
        }
        // GET /cart
        public IActionResult Index()
        {
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            CartViewModel cartVM = new CartViewModel()
                                   {

                                           CartItems = cart,
                                           GrandTotal = cart.Sum(x=>x.Total)
            };
            return View(cartVM);
        }

        // GET /cart/add/5
        public async Task<IActionResult> Add(int id)
        {
            Product product =await _context.Products.FindAsync(id);

            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            CartItem cartItem = cart
                                    .FirstOrDefault(x => x.ProductId == id);
            if(cartItem == null)
            {
                cart.Add(new CartItem(product));
            }
            else
            {
                cartItem.Quantity+=1; 

            }

            HttpContext.Session.SetJson("Cart", cart);


            if (HttpContext.Request.Headers["X-Requested-With"]!= "XMLHttpRequest")
            {
                return RedirectToAction("Index");
            }

            return ViewComponent("SmallCart");


        }

        // GET /cart/decrease/5
        public IActionResult Decrease(int id)
        {
           
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart");

            CartItem cartItem = cart
                    .FirstOrDefault(x => x.ProductId == id);
            if(cartItem.Quantity > 1)
            {
                cartItem.Quantity--;

            }
            else { cart.RemoveAll(x => x.ProductId == id); }
            if(cart.Count==0)
            {
                HttpContext.Session.Remove("Cart");
            }
            else
            {
                HttpContext.Session.SetJson("Cart", cart);
            }

            return RedirectToAction("Index");
        }

        // GET /cart/remove/5
        public IActionResult Remove(int id)
        {

            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart");

            CartItem cartItem = cart
                    .FirstOrDefault(x => x.ProductId == id);
            cart.RemoveAll(x => x.ProductId == id);


            if (cart.Count == 0)
            {
                HttpContext.Session.Remove("Cart");
            }
            else
            {
                HttpContext.Session.SetJson("Cart", cart);
            }

            return RedirectToAction("Index");
        }


        // GET /cart/clear
        public IActionResult Clear()
        {
            HttpContext.Session.Remove("Cart");
            return Redirect(Request.Headers["Referer"].ToString());
        }   
    }
}