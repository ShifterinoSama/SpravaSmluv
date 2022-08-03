using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SpravaSmluv.Models;

namespace SpravaSmluv.Data
{
    public class ContractManagmentContext : DbContext
    {
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<Advisor> Advisors { get; set; }        
        public DbSet<Client> Clients { get; set; }

        public ContractManagmentContext (DbContextOptions<ContractManagmentContext> options)
            : base(options)
        {
        }

        public ContractManagmentContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Client>()
                .HasMany(cl => cl.Contracts)
                .WithOne(co => co.Client);

            modelBuilder.Entity<Contract>()
                .HasOne(co => co.Client)
                .WithMany(cl => cl.Contracts)
                .HasForeignKey("ClientId")
                .IsRequired();

            modelBuilder.Entity<Contract>()
                .HasOne(co => co.ContractManager)
                .WithMany()
                .HasForeignKey("ContractManagerId")
                .IsRequired()
                .HasConstraintName("FK_CONTRACT_ADVISOR_CONTRACT_MANAGER")
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Contract>()
                .HasMany(co => co.Advisors)
                .WithMany(ad => ad.Contracts);
            //.UsingEntity(ca => ca.ToTable("ContractAdvisors"));
            modelBuilder.Entity<Contract>()
                .Property(co => co.EvidenceNumber)
                .ValueGeneratedOnAdd();



        }

        
        
        
    }
}
