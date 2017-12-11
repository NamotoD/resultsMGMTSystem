using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Assignment2.Models;
using System.Data.Entity.Infrastructure;
using System.IO;

namespace Assignment2.Controllers
{
    [HandleError(ExceptionType = typeof(DbUpdateException), View = "Error")]
    public class studentsController : Controller
    {
        private createMGMT_DBEntities db = new createMGMT_DBEntities();

        // GET: students
        public ActionResult Index()
        {
            return View(db.students.ToList());
        }

        // GET: students/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            student student = db.students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: students/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(student student, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (file.ContentLength > 0)
                    {
                        string _FileName = Path.GetFileName(file.FileName);
                        string _path = Path.Combine(Server.MapPath("~/UploadedFiles/StudentPhotos"), _FileName);
                        file.SaveAs(_path);
                    }
                    ViewBag.Message = "File Uploaded Successfully!!";

                    db.students.Add(new student
                    {
                        studentID = student.studentID,
                        studentName = student.studentName,
                        studentPhoto = "UploadedFiles/StudentPhotos/" + file.FileName
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

            return View(student);
        }

        // GET: students/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            student student = db.students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(student student, HttpPostedFileBase file)
        {
            //Console.WriteLine("student.studentName" + student.studentName);

            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    string _FileName = Path.GetFileName(file.FileName);
                    string _path = Path.Combine(Server.MapPath("~/UploadedFiles/StudentPhotos"), _FileName);
                    file.SaveAs(_path);
                }
                //Go fetch the existing profile from the database
                var currentProfile = db.students.FirstOrDefault(p => p.studentID == student.studentID);
                //Update the database record with the values from your model
                currentProfile.studentName = student.studentName;
                if (file != null)
                {
                    currentProfile.studentPhoto = "UploadedFiles/StudentPhotos/" + file.FileName;
                }
                //Commit to the database!
                db.SaveChanges();
                ViewBag.success = "Your changes have been saved";
                return RedirectToAction("Index");
            }

            return View(student);
        }

        // GET: students/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            student student = db.students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            try
            {
                student student = db.students.Find(id);
                db.students.Remove(student);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "EmployeeInfo", "Create"));
            }
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
        public JsonResult IsStudentIDUsed(string studentID)
        {

            return Json(IsStudentIDAvailable(studentID));

        }
        public bool IsStudentIDAvailable(string studentID)
        {
            List<student> studentsInDatabase = (from s in db.students select s).ToList();

            var sID = (from s in studentsInDatabase
                          where s.studentID.ToUpper() == studentID.ToUpper()
                          select new { studentID }
                          ).FirstOrDefault();

            bool available;
            available = (sID != null) ? false : true; //if found in database return false else true
            return available;
        }
    }
}
