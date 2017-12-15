using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
    
using cgrimmett_bugtracker.Models.CodeFirst;

namespace cgrimmett_bugtracker.Models
{
        public class ProjectAssignHelper
        {
            private UserManager<ApplicationUser> userManager =
                new UserManager<ApplicationUser>(new UserStore<ApplicationUser>
                    (new ApplicationDbContext()));
            private ApplicationDbContext db = new ApplicationDbContext();

            public bool IsUserOnProject(string userId, int projectId)
            {
                var project = db.Projects.Find(projectId);
                var userBool = project.Users.Any(u => u.Id == userId); // access user through project
                return userBool;
            }

            public void AddUserToProject(string userId, int projectId)
            {
                var user = db.Users.Find(userId);
                var project = db.Projects.Find(projectId);
                project.Users.Add(user); // marrying project with the user
                db.SaveChanges(); // save the change to the database
            }

            public void RemoveUserFromProject(string userId, int projectId)
            {
                var user = db.Users.Find(userId);
                var project = db.Projects.Find(projectId);
                project.Users.Remove(user);
                db.SaveChanges();
            }

            public List<Project> ListUserProjects(string userId) // List<Project> not ICollection<ApplicationUser>, virtual properties makes life a lot easier because it gives access to all the tables
            {
                var user = db.Users.Find(userId);
                return user.Projects.ToList();
            }

            public ICollection<ApplicationUser> ListUsersOnProject(int projectId)
            {
                var project = db.Projects.Find(projectId);
                return project.Users.ToList();
            }

            public ICollection<ApplicationUser> ListUsersNotOnProject(int projectId)
            {
                return db.Users.Where(u => u.Projects.All(p => p.Id != projectId)).ToList();
            }
        }
    }















    //public class ProjectAssignHelper
    //{
    //    private ApplicationDbContext db;

    //    public ProjectAssignHelper(ApplicationDbContext context)
    //    {
    //        //this.userManager = new UserManager<ApplicationUser>(
    //        //    new UserStore<ApplicationUser>(context));
    //        //this.roleManager = new RoleManager<IdentityRole>(
    //        //    new RoleStore<IdentityRole>(context));
    //        this.db = context;
    //    }

    //    public bool IsProjectOnUser(int projectId, string userId)
    //    {
    //        var project = db.Projects.Find(projectId);
    //        var userCheck = project.Users.Any(p => p.Id == userId);
    //        return (userCheck);
    //    }

    //    //public string GetUserInRole(string userId)
    //    //{
    //    //    return userManager.GetRoles(userId).First();
    //    //}

    //    //public IList<string> ListAssignedUsers(int projectId)
    //    public ICollection<ApplicationUser> ListAssignedUsers(int projectId)
    //    {
    //        Project project = db.Projects.Find(projectId);
    //        var userList = project.Users.ToList();
    //        return (userList);
    //    }

    //    public bool AddProjectToUser(int projectId, string userId)
    //    {
    //        Project project = db.Projects.Find(projectId);
    //        ApplicationUser user = db.Users.Find(userId);

    //        project.Users.Add(user);

    //        try
    //        {
    //            var userAdded = db.SaveChanges();

    //            if (userAdded != 0)
    //            {
    //                return true;
    //            }
    //            else
    //            {
    //                return false;
    //            }
    //        }
    //        catch
    //        {
    //            return false;
    //        }

    //        //var roles = userManager.GetRoles(userId);
    //        //foreach(var role in roles)
    //        //{
    //        //    RemoveUserFromRole(userId, role);
    //        //}
    //        //var result = userManager.AddToRole(projectId, userId);
    //        //return result.Succeeded;
    //    }

    //    public bool RemoveProjectFromUser(int projectId, string userId)
    //    {
    //        Project project = db.Projects.Find(projectId);
    //        ApplicationUser user = db.Users.Find(userId);

    //        var result = project.Users.Remove(user);

    //        try
    //        {
    //            var userRemoved = db.SaveChanges();

    //            if (userRemoved != 0)
    //            {
    //                return true;
    //            }
    //            else
    //            {
    //                return false;
    //            }
    //        }
    //        catch
    //        {
    //            return false;
    //        }

    //        //var result = userManager.RemoveFromRole(projectId, userId);
    //        //return result.Succeeded;
    //    }

    //NEEDED?
    //public IList<ApplicationUser> UsersInRole(string roleName)
    //{
    //    var userIDs = roleManager.FindByName(roleName).Users.Select(r => r.UserId);
    //    return userManager.Users.Where(u => userIDs.Contains(u.Id)).ToList();
    //    //Select(u => new UserDropDownViewModel { Id = u.Id, Name = u.DisplayName }).
    //}

    //NEEDED?
    //public IList<ApplicationUser> UsersNotInRole(string roleName)
    //{
    //    var userIDs = System.Web.Security.Roles.GetUsersInRole(roleName);
    //    return userManager.Users.Where(u => !userIDs.Contains(u.Id)).ToList();
    //    //Select(u => new UserDropDownViewModel { Id = u.Id, Name = u.DisplayName }).
    //}

    //public IList<string> ListUserRoles(string userId)
    //{
    //    return manager.GetRoles(userId);
    //}

    //public bool AddUserToRole(string userId, string roleName)
    //{
    //    var result = manager.AddToRole(userId, roleName);
    //    return result.Succeeded;
    //}

    //public bool RemoveUserFromRole(string userId, string roleName)
    //{
    //    var result = manager.RemoveUserFromRole(string userId, string roleName);
    //    return result.Succeeded;
    //}
//}
//}
