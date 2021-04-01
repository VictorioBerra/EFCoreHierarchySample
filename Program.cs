using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EFGetStarted
{
    internal class Program
    {
        private static void Main()
        {
            using (var db = new HierarchyContext())
            {
                // Note: This sample requires the database to be created before running.

                // Create
                Console.WriteLine("Inserting a new hierarchy");
                db.Add(new Hierarchy
                {
                    Name = "A",
                    Children = new List<Hierarchy>
                    {
                        new Hierarchy
                        {
                            Name = "B"
                        }
                    }
                });
                db.SaveChanges();

                // Read
                Console.WriteLine("Querying for a hierarchy");
                var hierarchys = db.Hierarchy
                    .Include(x => x.Children)
                    .ToList();
            }
        }
    }
}
