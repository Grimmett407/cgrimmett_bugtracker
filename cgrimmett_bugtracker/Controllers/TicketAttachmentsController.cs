﻿using System;
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

namespace cgrimmett_bugtracker.Controllers
{
    public class TicketAttachmentsController : Universal
    {



        // GET: TicketAttachments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketAttachment ticketAttachment = db.TicketAttachments.Find(id);
            if (ticketAttachment == null)
            {
                return HttpNotFound();
            }
            return View(ticketAttachment);
        }

        // POST: TicketAttachments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TicketId,CreatedDate,MediaUrl,FilePath,FileName,AuthorId,Description")] TicketAttachment ticketAttachment, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticketAttachment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "Tickets", new { id = ticketAttachment.TicketId });
            }

            return RedirectToAction("Details", "Tickets", new { id = ticketAttachment.TicketId });
        }

        // GET: TicketAttachments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketAttachment ticketAttachment = db.TicketAttachments.Find(id);
            if (ticketAttachment == null)
            {
                return HttpNotFound();
            }
            return View(ticketAttachment);
        }

        // POST: TicketAttachments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TicketAttachment ticketAttachment = db.TicketAttachments.Find(id);
            db.TicketAttachments.Remove(ticketAttachment);
            db.SaveChanges();
            return RedirectToAction("Details", "Tickets", new { id = ticketAttachment.TicketId });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}




//// GET: TicketAttachments
//public ActionResult Index()
//{
//    return View(db.TicketAttachments.ToList());
//}

//// GET: TicketAttachments/Details/5
//public ActionResult Details(int? id)
//{
//    if (id == null)
//    {
//        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//    }
//    TicketAttachment ticketAttachment = db.TicketAttachments.Find(id);
//    if (ticketAttachment == null)
//    {
//        return HttpNotFound();
//    }
//    return View(ticketAttachment);
//}

//// GET: TicketAttachments/Create
//public ActionResult Create()
//{
//    return View();
//}

//// POST: TicketAttachments/Create
//// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
//[HttpPost]
//[ValidateAntiForgeryToken]
//public ActionResult Create([Bind(Include = "Id,TicketId,CreatedDate,MediaUrl,FilePath,FileName,AuthorId")] TicketAttachment ticketAttachment)
//{
//    if (ModelState.IsValid)
//    {
//        db.TicketAttachments.Add(ticketAttachment);
//        db.SaveChanges();
//        return RedirectToAction("Index");
//    }

//    return View(ticketAttachment);
//}