using Microsoft.EntityFrameworkCore;
using Promotion.Models;

namespace Promotion.Data
{
    public class StoreDBContext : DbContext
    {
        public StoreDBContext(DbContextOptions<StoreDBContext> opt): base(opt)
        {

        }

        public virtual DbSet<Store> Stores { get; set; }

    }
}
