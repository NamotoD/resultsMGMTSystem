using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Assignment2.Models;
using System.IO;

namespace Assignment2.Controllers
{
    [Authorize(Roles = "Admin")]
    public class unitsController : Controller
    {
        private createMGMT_DBEntities db = new createMGMT_DBEntities();

        // GET: units
        public ActionResult Index()
        {
            return View(db.units.ToList());
        }

        // GET: units/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            unit unit = db.units.Find(id);
            if (unit == null)
            {
                return HttpNotFound();
            }
            return View(unit);
        }

        // GET: units/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: units/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(unit unit, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (file.ContentLength > 0)
                    {
                        string _FileName = Path.GetFileName(file.FileName);
                        string _path = Path.Combine(Server.MapPath("~/UploadedFiles/UnitOutlines"), _FileName);
                        file.SaveAs(_path);
                    }
                    ViewBag.Message = "File Uploaded Successfully!!";

                    db.units.Add(new unit
                    {
                        unitCode = unit.unitCode,
                        unitTitle = unit.unitTitle,
                        unitCoordinator = unit.unitCoordinator,
                        unitOutline = "UploadedFiles/UnitOutlines/" + file.FileName
                    });
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch
                {
                    ViewBag.Message = "File upload failed!!";
                    return View();
                }

            }

            return View(unit);
        }

        // GET: units/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            unit unit = db.units.Find(id);
            if (unit == null)
            {
                return HttpNotFound();
            }
            return View(unit);
        }

        // POST: units/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(unit unit, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                    if (file != null)
                    {
                        string _FileName = Path.GetFileName(file.FileName);
                        string _path = Path.Combine(Server.MapPath("~/UploadedFiles/UnitOutlines"), _FileName);
                        file.SaveAs(_path);
                    }
                //Go fetch the existing profile from the database
                var currentProfile = db.units.FirstOrDefault(p => p.unitCode == unit.unitCode);
                //Update the database record with the values from your model
                currentProfile.unitTitle = unit.unitTitle;
                currentProfile.unitCoordinator = unit.unitCoordinator;
                if (file != null)
                {
                    currentProfile.unitOutline = "UploadedFiles/UnitOutlines/" + file.FileName;
                }
                //Commit to the database!
                db.SaveChanges();
                ViewBag.success = "Your changes have been saved";
                    return RedirectToAction("Index");
            }

            return View(unit);
        }


        // GET: units/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            unit unit = db.units.Find(id);
            if (unit == null)
            {
                return HttpNotFound();
            }
            return View(unit);
        }

        // POST: units/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            unit unit = db.units.Find(id);
            db.units.Remove(unit);
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
        [HttpPost]
        public JsonResult IsUnitCodeUsed(string unitCode)
        {

            return Json(IsUnitCodeAvailable(unitCode));

        }
        public bool IsUnitCodeAvailable(string unitCode)
        {
            List<unit> unitsInDatabase = (from u in db.units select u).ToList();

            var unitCodeIDId = (from u in unitsInDatabase
                                where u.unitCode.ToUpper() == unitCode.ToUpper()
                                select new { unitCode }
                                ).FirstOrDefault();

            bool available;
            available = (unitCodeIDId != null) ? false : true; //if found in database return false else true
            return available;
        }
        [HttpPost]
        public JsonResult IsUnitTitleUsed(string unitTitle)
        {

            return Json(IsUnitTitleAvailable(unitTitle));

        }
        public bool IsUnitTitleAvailable(string unitTitle)
        {
            List<unit> unitsInDatabase = (from u in db.units select u).ToList();

            var uTitle = (from u in unitsInDatabase
                          where u.unitTitle.ToUpper() == unitTitle.ToUpper()
                          select new { unitTitle }
                          ).FirstOrDefault();

            bool available;
            available = (uTitle != null) ? false : true; //if found in database return false else true
            return available;
        }
    }
}
