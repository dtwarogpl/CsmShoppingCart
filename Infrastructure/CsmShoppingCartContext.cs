using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CmsShoppingCart.Infrastructure
{
    public class CsmShoppingCartContext :DbContext
    {
        public CsmShoppingCartContext(DbContextOptions<CsmShoppingCartContext> options):base(options)
        {
            
        }
    }
}
