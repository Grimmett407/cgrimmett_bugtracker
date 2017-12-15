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
using System.IO;
using System.Data.Entity.Infrastructure;
using System.Reflection;

namespace cgrimmett_bugtracker.Controllers
{
    public class TicketsController : Universal
    {
        const string TicketHistoryPropertyCreated = "Ticket Created";

        const string TicketTitleUpdated = "Ticket Title Updated";
        const string TicketBodyUpdated = "Ticket Body Updated";
        const string TicketStatusUpdated = "Ticket Status Updated";
        const string TicketPriorityUpdated = "Ticket Priority Updated";
        const string TicketTypeUpdated = "Ticket Type Updated";


        private Ticket currentTicket = new Ticket();

        // GET: Tickets
        public ActionResult Index(int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            var tickets = db.Tickets.Include(t => t.AssignedToUser).Include(t => t.AuthorUser).Include(t => t.Project);
            return View(tickets.OrderByDescending(t => t.TicketPriorityId).ToPagedList(pageNumber, pageSize));
        }

        // GET: Tickets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // GET: Tickets/Create
        public ActionResult Create()
        {
            var user = User.Identity.GetUserId();

                ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Title");
            //var projectAssign = db.Projects.Where(p => p.AssignedId == user).OrderBy(p => p.Assigned).ToList();
            //if (projectAssign != null)
            //{
            //}
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name");
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name");
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name");
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Body,CreatedDate,UpdatedDate,ProjectId,TicketTypeId,TicketPriorityId,TicketStatusId,AuthorId")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                //ApplicationUser current
                var user = db.Users.Find(User.Identity.GetUserId());
                var status = db.TicketStatuses.FirstOrDefault(s => s.Name.Equals("Unassigned")) as TicketStatus;
                if (status != null)
                {
                    ticket.TicketStatusId = status.Id;
                }

                ticket.AuthorUserId = user.Id;
                ticket.CreatedDate = System.DateTimeOffset.Now;
                db.Tickets.Add(ticket);
                db.SaveChanges();

                //Creating the "Create" History
                updateHistory("Created", ticket, ticket, "Created", "Created");
                return RedirectToAction("Details", new { id = ticket.Id });
            }

            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            ViewBag.CurrentTicket = ticket;
            if (ticket == null)
            {
                return HttpNotFound();
            }

            var role = db.Roles.SingleOrDefault(u => u.Name == "Developer");
            var usersInRole = db.Users.Where(u => u.Roles.Any(r => (r.RoleId == role.Id)));
            ViewBag.AssignedId = new SelectList(usersInRole, "Id", "FirstName");

            ViewBag.AssignedToUserId = new SelectList(usersInRole, "Id", "FirstName");
            ViewBag.AssignedToUser = new SelectList(db.Users, "Id", "FirstName");
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Title");
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name");
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name");
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name");
            return View(ticket);
        }


        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Body,CreatedDate,UpdatedDate,ProjectId,TicketTypeId,TicketPriorityId,TicketStatusId,AssignedToUserId")] Ticket ticket)
        {
              if (ModelState.IsValid)
                {
                    ticket.UpdatedDate = DateTimeOffset.Now;

                    //Make new TicketHistory
                    //get a non-proxy oldTicket
                    var dbNoTrack = new ApplicationDbContext();
                    ((IObjectContextAdapter)dbNoTrack).ObjectContext.ContextOptions.ProxyCreationEnabled = false;
                    var oldTicket = dbNoTrack.Tickets.Find(ticket.Id);
                    //Check and record changes
                    CheckChanged(oldTicket, ticket);
                    // get rid of non-proxy old ticket
                    dbNoTrack.Dispose();

                    

                    db.Entry(ticket).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Details", new { id = ticket.Id});
                }

            ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "FirstName", ticket.AssignedToUserId);
            ViewBag.AssignedToUser = new SelectList(db.Users, "Id", "FirstName", ticket.AssignedToUser);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Title", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        public void CheckChanged(object first, object second)
        {
            Type firstType = first.GetType();
            foreach (PropertyInfo propertyInfo in firstType.GetProperties())
            {
                if (propertyInfo.CanRead)
                {
                    object firstValue = propertyInfo.GetValue(first, null);
                    object secondValue = propertyInfo.GetValue(second, null);
                    if (firstValue != null || secondValue != null)
                    {
                        if (firstValue == null || secondValue == null || !firstValue.Equals(secondValue))
                        {
                            string firstV = null;
                            string secondV = null;

                            if (firstValue == null)
                                firstV = null;
                            else
                                firstV = firstValue.ToString();

                            if (secondValue == null)
                                secondV = null;
                            else
                                secondV = secondValue.ToString();
                            if (firstV == null || secondV == null || !firstV.Equals(secondV))
                            {
                                if (propertyInfo.Name != "TicketHistories" && propertyInfo.Name != "UpdatedDate" && propertyInfo.Name != "CreatedDate")
                                {
                                    updateHistory(propertyInfo.Name,
                                        first as Ticket,
                                        second as Ticket,
                                        firstV,
                                        secondV
                                        );
                                }
                            }
                        }
                    }
                }
            }
        }

        private void updateHistory(string property, Ticket old, Ticket current, string oldProp, string newProp)
        {
            db.TicketHistories.Add(new TicketHistory()
            {
                TicketId = current.Id,
                UserId = User.Identity.GetUserId(),
                Property = property,
                OldValue = oldProp,
                NewValue = newProp,
                ChangeDate = DateTimeOffset.Now,
            });
            db.SaveChanges();
        }

        // GET: Tickets/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [Authorize(Roles = "Admin, Project Manager")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ticket ticket = db.Tickets.Find(id);
            db.Tickets.Remove(ticket);
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

        // GET: Assigned Tickets to user
        public ActionResult AssignedTickets(int? page)
        {
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            var user = db.Users.Find(User.Identity.GetUserId());

            return View(db.Tickets.Where(a => user.Id.Contains(a.AssignedToUserId)).OrderByDescending(p => p.CreatedDate).ToPagedList(pageNumber, pageSize));
        }

        // GET: Find tickets per submitter
        public ActionResult SubmitterTickets(int? page)
        {
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            var user = db.Users.Find(User.Identity.GetUserId());

            return View(db.Tickets.Where(a => user.Id.Contains(a.AuthorUserId)).OrderByDescending(p => p.CreatedDate).ToPagedList(pageNumber, pageSize));
        }

        public ActionResult SearchResults(string search, int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(db.Tickets.Where(i => i.Body.Contains(search) || i.Title.Contains(search) || i.Project.Title.Contains(search) || 
            i.AssignedToUser.FirstName.Contains(search) || i.AssignedToUser.LastName.Contains(search) || i.TicketPriority.Name.Contains(search) ||
            i.TicketType.Name.Contains(search)).OrderByDescending(p => p.CreatedDate).ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        //[RequireHttps]
        [ValidateAntiForgeryToken]
        public ActionResult CommentCreate([Bind(Include = "Id,TicketId,Body,CreatedDate,AuthorId")] TicketComment ticketcomment)
        { // only pass in the bind the attributes that have forms
            var userId = User.Identity.GetUserId();
            if (ModelState.IsValid)
            {
                if (!String.IsNullOrWhiteSpace(ticketcomment.Body))
                {
                   var ticket = db.Tickets.Find(ticketcomment.TicketId);

                    ticketcomment.CreatedDate = System.DateTime.Now;
                    ticketcomment.AuthorUserId = User.Identity.GetUserId();
                    db.TicketComments.Add(ticketcomment);
                    db.SaveChanges();


                    return RedirectToAction("Details", new { id = ticket.Id });
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        //[RequireHttps]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAttachment([Bind(Include = "Id,TicketId,MediaUrl,CreatedDate,Description,AuthorUserId")] TicketAttachment ticketattachment, HttpPostedFileBase image)
        { // only pass in the bind the attributes that have forms
            var userId = User.Identity.GetUserId();
            var ticket = db.Tickets.Find(ticketattachment.TicketId);
            if (ModelState.IsValid)
            {
                if (image != null && image.ContentLength > 0)
                {
                    var ext = Path.GetExtension(image.FileName).ToLower();
                    if (ext != ".png" && ext != ".jpg" && ext != ".jpeg" && ext != ".gif" 
                        && ext != ".bmp" && ext != ".pdf" && ext != ".doc" && ext != ".docx" && ext != ".txt" 
                        && ext != ".html")
                        ModelState.AddModelError("image", "Invaild Format.");
                }
                if (!String.IsNullOrWhiteSpace(ticketattachment.Description))
                {
                    
                    if (image != null)
                    {

                        var filepath = "/Assets/Attachment Imgs/";
                        var absPath = Server.MapPath("~" + filepath);

                        if (ticketattachment.MediaUrl != string.Empty)
                        {
                            ticketattachment.MediaUrl = filepath + image.FileName;
                            image.SaveAs(Path.Combine(absPath, image.FileName));
                        }
                    }
                    ticketattachment.CreatedDate = System.DateTime.Now;
                    ticketattachment.AuthorUserId = User.Identity.GetUserId();
                    db.TicketAttachments.Add(ticketattachment);
                    db.SaveChanges();

                    return RedirectToAction("Details", new { id = ticket.Id });
                }
            }

            return RedirectToAction("Details", new { id = ticket.Id });
        }

        private void HistoryCreate(Ticket ticket)
        {
            TicketHistory history = new TicketHistory();
            var user = User.Identity.GetUserId();
            history.TicketId = ticket.Id;

            if (user != null)
            {
                history.UserId = user;
            }

            history.Property = TicketHistoryPropertyCreated;
            history.OldValue = "";
            history.NewValue = TicketHistoryPropertyCreated;
            history.ChangeDate = DateTimeOffset.Now;
            db.TicketHistories.Add(history);
            db.SaveChanges();

            return;
        }

        private void Notification(Ticket ticket)
        {
            Notification notification = new Notification();
            var user = User.Identity.GetUserId();
            notification.TicketId = ticket.Id;

            if (user != null)
            {
                notification.NotifyUserId = user;
            }

            notification.CreatedDate = DateTimeOffset.Now;
            db.Notifications.Add(notification);
            db.SaveChanges();

            return;
        }


    }
}
