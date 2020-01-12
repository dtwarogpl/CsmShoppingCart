using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CmsShoppingCart.Models;
using Microsoft.EntityFrameworkCore;

namespace CmsShoppingCart.Infrastructure
{
    public class CmsShoppingCartContext :DbContext
    {
        public CmsShoppingCartContext(DbContextOptions<CmsShoppingCartContext> options):base(options)
        {
            
        }

        private DbSet<Page> Pages { get; set; }
    }
}
