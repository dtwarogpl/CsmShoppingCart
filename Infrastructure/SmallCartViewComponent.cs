using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CmsShoppingCart.Models;
using Microsoft.AspNetCore.Mvc;

namespace CmsShoppingCart.Infrastructure
{
    public class SmallCartViewComponent: ViewComponent

    
    {
        public IViewComponentResult Invoke()
        {
            List<CartItem> Cart = HttpContext.Session.GetJson<List<CartItem>>("Cart");
            SmallCartViewModel smallCartViewModel;
            if(Cart == null ||
               Cart.Count == 0)
            {
                smallCartViewModel = null;
            }
            else
            {
                smallCartViewModel = new SmallCartViewModel
                                     {
                                             NumberofItems = Cart.Sum(x => x.Quantity), TotalAmount = Cart.Sum(x => x.Quantity * x.Price)
                                     };
            }

            return View(smallCartViewModel);
        }
    }
}
