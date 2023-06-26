//using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebApplicationIdeoDigital.Models
{
    public class APIDbContext : DbContext
    {
        public APIDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Invoice> Invoices { get; set; }
    }
}
