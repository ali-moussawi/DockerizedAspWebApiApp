using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.Generic;

namespace Subscriptionapi.Models
{
    public class ApplicationDbContext : DbContext
    {



        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {



        }

        public override void Dispose()
        {
            base.Dispose();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Subscription> Subscription { get; set; }


    }
}
