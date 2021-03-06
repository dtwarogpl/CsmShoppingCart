﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CmsShoppingCart.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CmsShoppingCart.Infrastructure
{
    public class MainMenuViewComponent : ViewComponent
    {

        private readonly CmsShoppingCartContext Context;

        public MainMenuViewComponent(CmsShoppingCartContext context)
        {
            Context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            var pages = await GetPagesAsync();
            return View(pages);
        }

        private Task<List<Page>> GetPagesAsync()
        {
            return Context.Pages.OrderBy(x => x.Sorting)
                          .ToListAsync();
        }
    }
}
