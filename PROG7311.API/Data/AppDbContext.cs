using Microsoft.EntityFrameworkCore;
using PROG7311_POE.Models;
using System;
using System.Collections.Generic;

namespace PROG7311_POE.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<ServiceRequest> ServiceRequests { get; set; }
    }
}
