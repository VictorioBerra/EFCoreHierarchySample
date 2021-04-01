using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Terminal.Gui;

namespace EFGetStarted
{
    internal class Program
    {
        private static void Main()
        {
            Application.Init();
            var top = Application.Top;

            using var db = new HierarchyContext();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            // Creates the top-level window to show
            var win = new Window("EF Hierarchy")
            {
                X = 0,
                Y = 1, // Leave one row for the toplevel menu

                // By using Dim.Fill(), it will automatically resize without manual intervention
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };

            top.Add(win);

            // Creates a menubar, the item "New" has a help menu.
            var menu = new MenuBar(new MenuBarItem[] {
                new MenuBarItem ("_File", new MenuItem [] {
                    new MenuItem ("_Quit", "", () => { if (Quit ()) top.Running = false; })
                }),
            });

            top.Add(menu);

            static bool Quit()
            {
                var n = MessageBox.Query(50, 7, "Quit EF Hierarchy", "Are you sure you want to quit?", "Yes", "No");
                return n == 0;
            }

            // Create
            if (!db.Hierarchy.Any())
            {
                Console.WriteLine("Inserting a new hierarchy");
                db.Add(new Hierarchy
                {
                    Name = "test user 1",
                    Children = new List<Hierarchy>
                    {
                        new Hierarchy
                        {
                            Name = "test user 2",
                            Children = new List<Hierarchy>
                            {
                                new Hierarchy
                                {
                                    Name = "test user 3",
                                    Children = new List<Hierarchy>
                                    {
                                        new Hierarchy
                                        {
                                            Name = "test user 4"
                                        }
                                    }
                                }
                            }
                        },
                        new Hierarchy
                        {
                            Name = "test user 5",
                        }
                    }
                });
                db.SaveChanges();
            }

            // Read
            Console.WriteLine("Querying for a hierarchy");
            var hierarchys = db.Hierarchy
                .Include(x => x.Children)
                .ToList();

            var query = db.Hierarchy
                .Include(x => x.Children)
                .ToQueryString();

            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("Id", typeof(int)));
            dt.Columns.Add(new DataColumn("User", typeof(string)));
            dt.Columns.Add(new DataColumn("Parent", typeof(string)));

            foreach (var hierarchy in hierarchys)
            {
                dt.Rows.Add(hierarchy.Id, hierarchy.Name, hierarchy.Parent?.Name);
            }

            var table = new TableView()
            {

                Width = Dim.Fill(),
                Height = Dim.Fill()
            };

            table.Table = dt;

            win.Add(table);

            Application.Run();
        }
    }
}
