using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SpravaSmluv.Models;

namespace SpravaSmluv.Data
{
    public class SpravaSmluvContext : DbContext
    {
        public SpravaSmluvContext (DbContextOptions<SpravaSmluvContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ContractAdvisor>().HasKey(ca => new { ca.ContractId, ca.AdvisorId });
            modelBuilder.Entity<ContractAdvisor>().HasOne(ca => ca.Contract).WithMany(c => c.ContractAdvisors).HasForeignKey(ca => ca.ContractId);
            modelBuilder.Entity<ContractAdvisor>().HasOne(ca => ca.Advisor).WithMany(a => a.ContractAdvisors).HasForeignKey(ca => ca.AdvisorId);
        }

        public DbSet<Contract> Contracts { get; set; }
        public DbSet<Advisor> Advisors { get; set; }
        public DbSet<ContractAdvisor> ContractAdvisors { get; set; }
        public DbSet<SpravaSmluv.Models.Client> Client { get; set; }
    }
}
