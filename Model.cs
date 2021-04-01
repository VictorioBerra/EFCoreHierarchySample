using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace EFGetStarted
{
    public class HierarchyContext : DbContext
    {
        public DbSet<Hierarchy> Hierarchy { get; set; }

        // The following configures EF to create a Sqlite database file as `C:\blogging.db`.
        // For Mac or Linux, change this to `/tmp/blogging.db` or any other absolute path.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options
                .UseSqlite(@"Data Source=.\hierarchy.db").LogTo(Console.WriteLine);
    }

    public class Hierarchy
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Hierarchy Parent { get; set; }
        public ICollection<Hierarchy> Children { get; set; }
    }
}
