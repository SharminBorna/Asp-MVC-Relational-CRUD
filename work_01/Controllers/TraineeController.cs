using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using work_01.Models;
using work_01.ViewModels;

namespace work_01.Controllers
{
    [Authorize]
    public class TraineeController : Controller
    {
        private TraineeDBContext db = new TraineeDBContext();

        // GET: Trainee
        [AllowAnonymous]
        public ActionResult Index(string search = "")
        {
            var trainees = db.Trainees.Include("Course").Select(x => new TraineeVM
            {
                TraineeId = x.TraineeId,
                TraineeName = x.TraineeName,
                JoinDate = x.JoinDate,
                cName = x.Course.Name,
                PicturePath = x.PicturePath
            });
            if (!string.IsNullOrEmpty(search))
            {
                trainees = trainees.Where(x => x.TraineeName.ToLower().StartsWith(search.ToLower()));
            }
            return View(trainees);
        }
        public ActionResult Create()
        {
            ViewBag.Course = new SelectList(db.Courses, "CourseId", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TraineeVM tvm)
        {
            if (ModelState.IsValid)
            {
                if (tvm.Picture != null)
                {
                    string filepath = Path.Combine("~/Images", Guid.NewGuid().ToString() + Path.GetExtension(tvm.Picture.FileName));
                    tvm.Picture.SaveAs(Server.MapPath(filepath));

                    Trainee trainee = new Trainee
                    {
                        TraineeName = tvm.TraineeName,
                        JoinDate = tvm.JoinDate,
                        CourseId = tvm.CourseId,
                        PicturePath = filepath
                    };
                    db.Trainees.Add(trainee);
                    db.SaveChanges();
                    return PartialView("_success");
                }
            }
            ViewBag.Course = new SelectList(db.Courses, "CourseId", "Name");
            return PartialView("_error");
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trainee trainee = db.Trainees.Find(id);
            if (trainee == null)
            {
                return HttpNotFound();
            }
            TraineeVM tvm = new TraineeVM
            {
                TraineeId = trainee.TraineeId,
                TraineeName = trainee.TraineeName,
                JoinDate = trainee.JoinDate,
                CourseId = trainee.CourseId,
                PicturePath = trainee.PicturePath
            };
            ViewBag.Course = new SelectList(db.Courses, "CourseId", "Name", tvm.CourseId);
            return View(tvm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TraineeVM tvm)
        {
            if (ModelState.IsValid)
            {
                string filepath = tvm.PicturePath;
                if (tvm.Picture != null)
                {
                    filepath = Path.Combine("~/Images", Guid.NewGuid().ToString() + Path.GetExtension(tvm.Picture.FileName));
                    tvm.Picture.SaveAs(Server.MapPath(filepath));

                    Trainee trainee = new Trainee
                    {
                        TraineeId = tvm.TraineeId,
                        TraineeName = tvm.TraineeName,
                        JoinDate = tvm.JoinDate,
                        CourseId = tvm.CourseId,
                        PicturePath = filepath
                    };
                    db.Entry(trainee).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    Trainee trainee = new Trainee
                    {
                        TraineeId = tvm.TraineeId,
                        TraineeName = tvm.TraineeName,
                        JoinDate = tvm.JoinDate,
                        CourseId = tvm.CourseId,
                        PicturePath = filepath
                    };
                    db.Entry(trainee).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            ViewBag.Course = new SelectList(db.Courses, "CourseId", "Name", tvm.CourseId);
            return View(tvm);
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trainee trainee = db.Trainees.Find(id);
            if (trainee == null)
            {
                return HttpNotFound();
            }
            return View(trainee);
        }   
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Trainee trainee = db.Trainees.Find(id);

            string file_name = trainee.PicturePath;
            string path = Server.MapPath(file_name);
            FileInfo file = new FileInfo(path);
            if (file.Exists)//check file exsit or not  
            {
                file.Delete();
            }
            db.Trainees.Remove(trainee);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
