using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bank.Core.Entity;

namespace Bank.Infrastructure.Data
{
    public class BankDbContext : DbContext
    {

        public BankDbContext(DbContextOptions<BankDbContext> options)
        : base(options)
        {
        }

        public DbSet<User> USER { get; set; }

        public DbSet<RoleType> RoleTypes { get; set; }

        public DbSet<RefMaster> REF_MASTER { get; set; }

    }
}
