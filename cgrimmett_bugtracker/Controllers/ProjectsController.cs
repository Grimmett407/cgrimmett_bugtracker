using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using cgrimmett_bugtracker.Models;
using cgrimmett_bugtracker.Models.CodeFirst;
using cgrimmett_bugtracker.Models.Helpers;
using PagedList;
using PagedList.Mvc;
using Microsoft.AspNet.Identity;

namespace cgrimmett_bugtracker.Controllers
{
    public class ProjectsController : Universal
    {

        // GET: Projects
        public ActionResult Index(int? page, int? id)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            ProjectUserViewModels uservm = new ProjectUserViewModels();

            //Find project based on project id
            var project = db.Projects.Find(id);
            //Find user for this project id
            //var assignmentList = db.ProjectUser.Where(a => a.ProjectId == project.Id).ToList();
            //var i = 0;
            //foreach(var assignment in assignmentList)
            //{
            //    var user = db.Users.Find(assignment.UserId);
            //    uservm.SelectedUsers[i] = user.DisplayName;
            //    i++;
            //}

            //uservm.SelectedUsers = assignmentList.ListAssignedUsers(id).Select(u => u.Id).ToArray();
            //uservm.Users = new MultiSelectList(db.Users.Where(u => (u.DisplayName != "N/A" && u.DisplayName != "(Remove Assigned User)")).OrderBy(u => u.FirstName), "Id", "DisplayName", uservm.SelectedUsers);


            uservm.AssignProject = project;

            return View(db.Projects.OrderByDescending(p => p.CreatedDate).ToPagedList(pageNumber, pageSize));
        }

        // GET: Projects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // GET: Projects/Create
        public ActionResult Create()
        {

            var role = db.Roles.SingleOrDefault(u => u.Name == "Project Manager");
            var usersInRole = db.Users.Where(u => u.Roles.Any(r => (r.RoleId == role.Id)));
            ViewBag.AssignedId = new SelectList(usersInRole, "Id", "FirstName");
            ViewBag.AssignedToUser = new SelectList(db.Users, "Id", "FirstName");
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles ="Admin, Project Manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Body,CreatedDate,UpdatedDate,IsActive,AuthorId,Assigned,AssignedId")] Project project)
        {
            if (ModelState.IsValid)
            {
                var user = User.Identity.GetUserId();
                project.AuthorId = user;
                project.CreatedDate = System.DateTimeOffset.Now;
                db.Projects.Add(project);
                db.SaveChanges();
                return RedirectToAction("Details", new { id = project.Id });
            }
            ViewBag.Assigned = new SelectList(db.Users, "Id", "FirstName", project.Assigned);
            ViewBag.AssignedId = new SelectList(db.Users, "Id", "FirstName", project.AssignedId);
            return View(project);
        }

        // GET: Projects/Edit/5
        [Authorize(Roles = "Admin, Project Manager")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            var user = User.Identity.GetUserId();

            ViewBag.AssignedToUser = new SelectList(db.Users, "Id", "FirstName");

            var role = db.Roles.SingleOrDefault(u => u.Name == "Project Manager");
            var usersInRole = db.Users.Where(u => u.Roles.Any(r => (r.RoleId == role.Id)));
            ViewBag.AssignedId = new SelectList(usersInRole, "Id", "FirstName");

            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin, Project Manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Body,CreatedDate,UpdatedDate,IsActive,AuthorId, AssignedId, Assigned")] Project project)
        {
            if (ModelState.IsValid)
            {
                project.UpdatedDate = System.DateTimeOffset.Now;
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = project.Id});
            }
            ViewBag.AssignedToUser = new SelectList(db.Users, "Id", "FirstName", project.Assigned);
            ViewBag.AssignedId = new SelectList(db.Users, "Id", "FirstName", project.AssignedId);
            return View(project);
        }

        // GET: Projects/Delete/5
        [Authorize(Roles = "Admin, Project Manager")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Delete/5
        [Authorize(Roles = "Admin, Project Manager")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = db.Projects.Find(id);
            db.Projects.Remove(project);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        //GET: Your Projects
        public ActionResult AssignedProjects(int? page)
        {
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            var user = db.Users.Find(User.Identity.GetUserId());

            return View(db.Projects.Where(a => user.Id.Contains(a.AssignedId)).OrderByDescending(p => p.CreatedDate).ToPagedList(pageNumber, pageSize));
        }

        // GET: EditProjectUsers
        [Authorize(Roles = "Admin, Project Manager")]
        public ActionResult ProjectUser(int? id)
        {
            var project = db.Projects.Find(id);
            ProjectUserViewModels projectuserVM = new ProjectUserViewModels();
            projectuserVM.AssignProject = project;
            projectuserVM.AssignProjectId = Convert.ToInt32(project.Id);
            projectuserVM.SelectedUsers = project.Users.Select(u => u.Id).ToArray();
            projectuserVM.Users = new MultiSelectList(db.Users.ToList(), "Id", "FirstName", projectuserVM.SelectedUsers);
            // MultiSelectList parameters: collection of objects used for select list (user for us), inside user table we have diff properties that we submit by like Id or FirstName, display is FirstNames, highlighted selected users
            return View(projectuserVM); // how does this view return work? View(model) for example: passes in an object
        }

        // POST: EditProjectUsers
        [HttpPost]
        [Authorize(Roles = "Admin, Project Manager")]
        public ActionResult ProjectUser(ProjectUserViewModels model, int? id)
        {
            // submit assign and it goes through here, we need to go through users assigned in database and put in the users selected
            ProjectAssignHelper projectassignhelper = new ProjectAssignHelper();
            //var project = db.Projects.Find(model.AssignProject.Id); No need for this since we don't need to find project in db by Id
            foreach (var userId in db.Users.Select(r => r.Id).ToList()) // remove all users from project
            {
                projectassignhelper.RemoveUserFromProject(userId, model.AssignProjectId);
            }
            foreach (var userId in model.SelectedUsers) // add back the ones you want
            {
                projectassignhelper.AddUserToProject(userId, model.AssignProjectId);

                ProjectUser projectuser = new ProjectUser();
                projectuser.ProjectId = model.AssignProjectId;
                projectuser.UserId = userId;
                db.ProjectUser.Add(projectuser);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}
