using Demo.DAL.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Contexts
{
    public class CompanyDbContext:IdentityDbContext<ApplicationUser>
    {
        public CompanyDbContext(DbContextOptions<CompanyDbContext> options):base(options) 
        {
            
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        // => optionsBuilder.UseSqlServer("Server=DESKTOP-NPBIAJ7\\SQLEXPRESS01;Database=CompanyDB;Trusted_Connction=true;");
        public DbSet<Department> departments { get; set; }
        public DbSet<Employee> employees { get; set; }

    }
}
