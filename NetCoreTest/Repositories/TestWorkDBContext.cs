using Microsoft.EntityFrameworkCore;
using NetCoreTestProject.DataModel;
using NetCoreTestProject.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetCoreTestProject.Repositories
{
    public class TestWorkDBContext : DbContext, IUnitOfWork
    {
        public DbSet<TestWork> TestWorkModel { get; set; }

        public TestWorkDBContext(DbContextOptions<TestWorkDBContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TestWork>();
        }

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = await base.SaveChangesAsync();
            return true;
        }
    }
}
