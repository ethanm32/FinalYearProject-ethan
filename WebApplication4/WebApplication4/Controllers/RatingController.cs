using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication4.DataContext;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public class RatingController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Rating
        public ActionResult Index(string name)
        {
            ViewBag.Name = name;
            return View(db.RatingObj.ToList());
        }

        // GET: Rating/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RatingModel ratingModel = db.RatingObj.Find(id);
            if (ratingModel == null)
            {
                return HttpNotFound();
            }
            return View(ratingModel);
        }

        // GET: Rating/Create
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult StartPage()
        {
            return View(db.RatingObj.ToList());
        }

        // POST: Rating/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "songid,songname,rating,review")] RatingModel ratingModel)
        {
            if (ModelState.IsValid)
            {
                db.RatingObj.Add(ratingModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ratingModel);
        }

        // GET: Rating/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RatingModel ratingModel = db.RatingObj.Find(id);
            if (ratingModel == null)
            {
                return HttpNotFound();
            }
            return View(ratingModel);
        }

        // POST: Rating/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "songid,songname,rating,review")] RatingModel ratingModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ratingModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ratingModel);
        }

        // GET: Rating/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RatingModel ratingModel = db.RatingObj.Find(id);
            if (ratingModel == null)
            {
                return HttpNotFound();
            }
            return View(ratingModel);
        }

        // POST: Rating/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RatingModel ratingModel = db.RatingObj.Find(id);
            db.RatingObj.Remove(ratingModel);
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
    }
}
