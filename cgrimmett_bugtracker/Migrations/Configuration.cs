namespace cgrimmett_bugtracker.Migrations
{
    using cgrimmett_bugtracker.Models;
    using cgrimmett_bugtracker.Models.CodeFirst;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<cgrimmett_bugtracker.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(cgrimmett_bugtracker.Models.ApplicationDbContext context)
        {
            //Role methods
            var roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(context));

            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));



            //Admin
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }

            if (!context.Roles.Any(r => r.Name == "Project Manager"))
            {
                roleManager.Create(new IdentityRole { Name = "Project Manager" });
            }

            if (!context.Roles.Any(r => r.Name == "Developer"))
            {
                roleManager.Create(new IdentityRole { Name = "Developer" });
            }

            if (!context.Roles.Any(r => r.Name == "Submitter"))
            {
                roleManager.Create(new IdentityRole { Name = "Submitter" });
            }

            if (!context.Users.Any(u => u.Email == "cgrimmett407@gmail.com"))
            {
                userManager.Create(new ApplicationUser         //Creating new user for the application using required fields
                {
                    UserName = "cgrimmett407@gmail.com",
                    DisplayName = "Christian Grimmett",
                    Email = "cgrimmett407@gmail.com",
                    FirstName = "Christian",
                    LastName = "Grimmett",
                }, "Chris407!!");
            }

            //Project Managers
            if (!context.Users.Any(u => u.Email == "ewatkins@coderfoundry.com"))
            {
                userManager.Create(new ApplicationUser         //Creating new user for the application using required fields
                {
                    UserName = "ewatkins@coderfoundry.com",
                    DisplayName = "E. Watkins",
                    Email = "ewatkins@coderfoundry.com",
                    FirstName = "Eric",
                    LastName = "Watkins",
                }, "password1!");
            }

            if (!context.Users.Any(u => u.Email == "joeshmoe@gmail.com"))
            {
                userManager.Create(new ApplicationUser         //Creating new user for the application using required fields
                {
                    UserName = "joeshmoe@gmail.com",
                    DisplayName = "Joe S.",
                    Email = "joeshmoe@gmail.com",
                    FirstName = "Joe",
                    LastName = "Shmoe",
                }, "password1!");
            }

            if (!context.Users.Any(u => u.Email == "jane@gmail.com"))
            {
                userManager.Create(new ApplicationUser         //Creating new user for the application using required fields
                {
                    UserName = "jane@gmail.com",
                    DisplayName = "Jane",
                    Email = "jane@gmail.com",
                    FirstName = "Jane",
                    LastName = "Shmoe",
                }, "password1!");
            }

            var adminId = userManager.FindByEmail("cgrimmett407@gmail.com").Id;
            userManager.AddToRole(adminId, "Admin");

            var moderatorId = userManager.FindByEmail("ewatkins@coderfoundry.com").Id;
            userManager.AddToRole(moderatorId, "Project Manager");

            var developerId = userManager.FindByEmail("joeshmoe@gmail.com").Id;
            userManager.AddToRole(developerId, "Developer");

            var submitterId = userManager.FindByEmail("jane@gmail.com").Id;
            userManager.AddToRole(submitterId, "Submitter");


            //Dropdaown Options
            if (!context.TicketPriorities.Any(p => p.Name == "Low"))
            {
                var priority = new TicketPriority();
                priority.Name = "Low";
                context.TicketPriorities.Add(priority);
            }

            if (!context.TicketPriorities.Any(p => p.Name == "Medium"))
            {
                var priority = new TicketPriority();
                priority.Name = "Medium";
                context.TicketPriorities.Add(priority);
            }

            if (!context.TicketPriorities.Any(p => p.Name == "High"))
            {
                var priority = new TicketPriority();
                priority.Name = "High";
                context.TicketPriorities.Add(priority);
            }

            if (!context.TicketPriorities.Any(p => p.Name == "Urgent"))
            {
                var priority = new TicketPriority();
                priority.Name = "Urgent";
                context.TicketPriorities.Add(priority);
            }


            //Status
            if (!context.TicketStatuses.Any(p => p.Name == "Unassigned"))
            {
                var status = new TicketStatus();
                status.Name = "Unassigned";
                context.TicketStatuses.Add(status);
            }

            if (!context.TicketStatuses.Any(p => p.Name == "Assigned"))
            {
                var status = new TicketStatus();
                status.Name = "Assigned";
                context.TicketStatuses.Add(status);
            }

            if (!context.TicketStatuses.Any(p => p.Name == "In Progress"))
            {
                var status = new TicketStatus();
                status.Name = "In Progress";
                context.TicketStatuses.Add(status);
            }

            if (!context.TicketStatuses.Any(p => p.Name == "Complete"))
            {
                var status = new TicketStatus();
                status.Name = "Complete";
                context.TicketStatuses.Add(status);
            }


            //Type
            if (!context.TicketTypes.Any(p => p.Name == "Hardware"))
            {
                var type = new TicketType();
                type.Name = "Hardware";
                context.TicketTypes.Add(type);
            }

            if (!context.TicketTypes.Any(p => p.Name == "Software"))
            {
                var type = new TicketType();
                type.Name = "Software";
                context.TicketTypes.Add(type);
            }

            if (!context.TicketTypes.Any(p => p.Name == "Bug"))
            {
                var type = new TicketType();
                type.Name = "Bug";
                context.TicketTypes.Add(type);
            }

            if (!context.TicketTypes.Any(p => p.Name == "Feature Request"))
            {
                var type = new TicketType();
                type.Name = "Feature Request";
                context.TicketTypes.Add(type);
            }

            if (!context.TicketTypes.Any(p => p.Name == "Change Request"))
            {
                var type = new TicketType();
                type.Name = "Change Request";
                context.TicketTypes.Add(type);
            }

            if (!context.TicketTypes.Any(p => p.Name == "General Question"))
            {
                var type = new TicketType();
                type.Name = "General Question";
                context.TicketTypes.Add(type);
            }

            if (!context.TicketTypes.Any(p => p.Name == "UI/UX"))
            {
                var type = new TicketType();
                type.Name = "UI/UX";
                context.TicketTypes.Add(type);
            }

            if (!context.TicketTypes.Any(p => p.Name == "Cancellation"))
            {
                var type = new TicketType();
                type.Name = "Cancellation";
                context.TicketTypes.Add(type);
            }

            if (!context.TicketTypes.Any(p => p.Name == "Client-Related Issue"))
            {
                var type = new TicketType();
                type.Name = "Client-Related Issue";
                context.TicketTypes.Add(type);
            }

            if (!context.TicketTypes.Any(p => p.Name == "Other"))
            {
                var type = new TicketType();
                type.Name = "Other";
                context.TicketTypes.Add(type);
            }

            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
