using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Assignment2.Models;

namespace Assignment2.Controllers
{
    [Authorize(Roles = "Results Manager")]
    public class resultsController : Controller
    {
        private createMGMT_DBEntities db = new createMGMT_DBEntities();

        // GET: results
        public ActionResult Index()
        {
            var results = db.results.Include(r => r.student).Include(r => r.unit);
            return View(results.ToList());
        }

        // GET: results/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            result result = db.results.Find(id);
            if (result == null)
            {
                return HttpNotFound();
            }
            return View(result);
        }

        // GET: results/Create
        public ActionResult Create()
        {
            ViewBag.studentID = new SelectList(db.students, "studentID", "studentName");
            ViewBag.unitCode = new SelectList(db.units, "unitCode", "unitTitle");
            return View();
        }

        // POST: results/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "resultID,unitCode,studentID,rYear,rSemester,ass1,ass2,exam")] result result)
        {
            if (ModelState.IsValid)
            {
                db.results.Add(result);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.studentID = new SelectList(db.students, "studentID", "studentName", result.studentID);
            ViewBag.unitCode = new SelectList(db.units, "unitCode", "unitTitle", result.unitCode);
            return View(result);
        }

        // GET: results/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            result result = db.results.Find(id);
            if (result == null)
            {
                return HttpNotFound();
            }
            ViewBag.studentID = new SelectList(db.students, "studentID", "studentName", result.studentID);
            ViewBag.unitCode = new SelectList(db.units, "unitCode", "unitTitle", result.unitCode);
            return View(result);
        }

        // POST: results/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "resultID,unitCode,studentID,rYear,rSemester,ass1,ass2,exam")] result result)
        {
            if (ModelState.IsValid)
            {
                db.Entry(result).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.studentID = new SelectList(db.students, "studentID", "studentName", result.studentID);
            ViewBag.unitCode = new SelectList(db.units, "unitCode", "unitTitle", result.unitCode);
            return View(result);
        }

        // GET: results/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            result result = db.results.Find(id);
            if (result == null)
            {
                return HttpNotFound();
            }
            return View(result);
        }

        // POST: results/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            result result = db.results.Find(id);
            db.results.Remove(result);
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
