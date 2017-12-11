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
    //[Authorize] // using filters
    public class reportsController : Controller
    {
        private createMGMT_DBEntities db = new createMGMT_DBEntities();

        // GET: reports
        public async System.Threading.Tasks.Task<ActionResult> Index(string unit, string student, string semester, string year, string sortType)
        {

            ViewBag.sortByStudentID = String.IsNullOrEmpty(sortType) ? "studentID_desc" : "";
            ViewBag.sortByUnitCode = sortType == "unitCode_asc" ? "unitCode_desc" : "unitCode_asc";
            ViewBag.sortByUnitScore = sortType == "unitScore_asc" ? "unitScore_desc" : "unitScore_asc";
            var UnitLst = new List<string>();

            var UnitLstQry = from d in db.results
                             orderby d.unitCode
                             select d.unitCode;

            UnitLst.AddRange(UnitLstQry.Distinct());
            ViewBag.unit = new SelectList(UnitLst);

            var StudentLst = new List<string>();

            var StudentLstQry = from d in db.results
                                orderby d.studentID
                                select d.studentID;

            StudentLst.AddRange(StudentLstQry.Distinct());
            ViewBag.student = new SelectList(StudentLst);

            var SemesterLst = new List<string>();

            var SemesterLstQry = from d in db.results
                                 orderby d.rSemester
                                 select d.rSemester;

            SemesterLst.AddRange(SemesterLstQry.Distinct());
            ViewBag.semester = new SelectList(SemesterLst);

            var results = db.results.Include(r => r.student).Include(r => r.unit);

            if (!String.IsNullOrEmpty(year))
            {
                results = results.Where(r => r.rYear.Contains(year));
            }

            if (!string.IsNullOrEmpty(unit))
            {
                results = results.Where(x => x.unitCode == unit);
            }

            if (!string.IsNullOrEmpty(student))
            {
                results = results.Where(x => x.studentID == student);
            }

            if (!string.IsNullOrEmpty(semester))
            {
                results = results.Where(x => x.rSemester == semester);
            }

            switch (sortType)
            {
                case "studentID_desc":
                    results = results.OrderByDescending(r => r.studentID);
                    break;
                case "unitCode_asc":
                    results = results.OrderBy(r => r.unitCode);
                    break;
                case "unitCode_desc":
                    results = results.OrderByDescending(r => r.unitCode);
                    break;
                case "unitScore_asc":
                    results = results.OrderBy(r => r.unitScore);
                    break;
                case "unitScore_desc":
                    results = results.OrderByDescending(r => r.unitScore);
                    break;
                default:
                    results = results.OrderBy(r => r.studentID);
                    break;
            }
            var rowCount = results.Count();
            ViewBag.rowCount = rowCount;
            double? averageScore = results.Average(x => x.unitScore);
            double doubleAverageScore = averageScore ?? 0;
            ViewBag.averageScore = Math.Round(doubleAverageScore, 0);

            string grade = "";
            if (averageScore >= 80) { grade = "HD"; }
            else if (averageScore >= 70) { grade = "D"; }
            else if (averageScore >= 60) { grade = "CR"; }
            else if (averageScore >= 50) { grade = "Pass"; }
            else { grade = "N"; }
            ViewBag.grade = grade;

            return View(await results.AsNoTracking().ToListAsync());
        }

        // GET: reports/Details/5
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

        // GET: reports/Create
        public ActionResult Create()
        {
            ViewBag.studentID = new SelectList(db.students, "studentID", "studentName");
            ViewBag.unitCode = new SelectList(db.units, "unitCode", "unitTitle");
            return View();
        }

        // POST: reports/Create
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

        // GET: reports/Edit/5
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

        // POST: reports/Edit/5
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

        // GET: reports/Delete/5
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

        // POST: reports/Delete/5
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
