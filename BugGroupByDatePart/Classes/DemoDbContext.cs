using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BugGroupByDatePart.Classes.Models;
using System.Diagnostics;

namespace BugGroupByDatePart.Classes
{
    public class DemoDbContext: IdentityDbContext<CustomUser, IdentityRole, string>
    {
        // Constructor used by .NET Core for DI
        public DemoDbContext(DbContextOptions<DemoDbContext> options) : base(options)
        {
            Debug.WriteLine("DbContextOptions construction with options:");
            Debug.WriteLine(options);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // non exposed entities
            modelBuilder.Entity<MoneyTransferClient>();
            modelBuilder.Entity<MoneyTransferUser>();
            modelBuilder.Entity<Event>();
            modelBuilder.Entity<TransferToUser>();
            modelBuilder.Entity<TransferToClient>();
            modelBuilder.Entity<OrderThings>();
        }


        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Order> Commande { get; set; }
        public virtual DbSet<MoneyTransfer> MoneyTransfers { get; set; }

    }
}
