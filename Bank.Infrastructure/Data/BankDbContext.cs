using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bank.Core.Model;

namespace Bank.Infrastructure.Data
{
    public class BankDbContext : DbContext
    {

        public BankDbContext(DbContextOptions<BankDbContext> options)
        : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

    }
}
