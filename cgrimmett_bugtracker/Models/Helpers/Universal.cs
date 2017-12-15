using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace cgrimmett_bugtracker.Models.Helpers
{
    public class Universal : Controller
    {
        public ApplicationDbContext db = new ApplicationDbContext();

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            if (User.Identity.IsAuthenticated)
            {
                var user = db.Users.Find(User.Identity.GetUserId()); // variables accessible for each view

                ViewBag.FirstName = user.FirstName;
                ViewBag.LastName = user.LastName;
                ViewBag.DisplayName = user.DisplayName;
                ViewBag.FullName = user.FirstName + " " + user.LastName;

            }
            ViewBag.TicketTotal = db.Tickets.Count();
        }
    }
}