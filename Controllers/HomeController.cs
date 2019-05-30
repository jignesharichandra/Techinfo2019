using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Techinfo2019.Models;
using System.Globalization;
using System.Web;
using System.Data;

namespace Techinfo2019.Controllers
{
    public class HomeController : Controller
    {
        private DEMO1Entities db = new  DEMO1Entities();

        public ActionResult Index(string searchString, string brand)
        {
            //ViewBag.CurrentFilter = searchString;


            List<string> genreList = new List<string>();
            var genreQuery = from g in db.Mobiles
                             orderby g.Brand
                             select g.Brand;
            genreList.AddRange(genreQuery.Distinct());
            ViewBag.brand = new SelectList(genreList);

             



            var mobiles = from m in db.Mobiles
                          select m;
            if (!String.IsNullOrEmpty(brand))
            {
                mobiles = mobiles.Where(x => x.MobileName == brand);
            }
            if (!String.IsNullOrEmpty(searchString))
            {
                mobiles = mobiles.Where(x => x.MobileName.Contains(searchString));
            }
            
            return View(mobiles);
        }


        public ActionResult _TableRow(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mobile mobile = db.Mobiles.Find(id);
            if (mobile == null)
            {
                return HttpNotFound();
            }
            return PartialView(mobile);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "slNo,MobileName,Brand,Price,url,Description,Cheer")]Mobile mobile)

        {

            if (mobile.url == null)
            {
                mobile.url = "https://www.shareicon.net/data/2015/10/20/130824_exit_256x256.png";
            }

            if (mobile.Cheer == null)
            {
                mobile.Cheer = 0;
            }

            if (ModelState.IsValid)
            {
                db.Mobiles.Add(mobile);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(mobile);
        }

        public ActionResult Cheer(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mobile mob = db.Mobiles.Find(id);
            if (mob == null)
            {
                return HttpNotFound();
            }

            int currentCheers = (int)mob.Cheer;
            mob.Cheer = currentCheers + 1;
            if (ModelState.IsValid)
            {
                db.Entry(mob).State = EntityState.Modified;
                db.SaveChanges();

            }
            mob = db.Mobiles.Find(id);
            return PartialView("_TableRow", mob);
        }

        //public ActionResult Cheer(int id)
        //{
        //    Mobile Mobile = db.Mobiles.Find(id);
        //    int? currentCheers =  Mobile.Cheer;
        //    Mobile.Cheer = currentCheers + 1;
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(Mobile).State = EntityState.Modified;
        //        db.SaveChanges();
        //    }

        //    return PartialView("_TableRow", Mobile);
        //}

        public ActionResult Edit(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            Mobile mobile = db.Mobiles.Find(id);


            if (mobile == null)
            {
                return HttpNotFound();
            }

            return View(mobile);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "slNo,MobileName,Brand,Price,url,Description,Cheer")]Mobile mobile)
        {
            if (mobile.url == null)
            {
                mobile.url = "https://www.shareicon.net/data/2015/10/20/130824_exit_256x256.png";
            }

            if (ModelState.IsValid)
            {
                db.Entry(mobile).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(mobile);
        }


       

         

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Mobile mobile = db.Mobiles.Find(id);
            if (mobile == null)
            {
                return HttpNotFound();
            }


            return View(mobile);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mobile mobile = db.Mobiles.Find(id);

            if (mobile == null)
            {
                return HttpNotFound();
            }
            return View(mobile);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Mobile mobile = db.Mobiles.Find(id);
            db.Mobiles.Remove(mobile);
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

        //public ActionResult About()
        //{
        //    ViewBag.Message = "Your application description page.";

        //    return View();
        //}

        //public ActionResult Contact()
        //{
        //    ViewBag.Message = "Your contact page.";

        //    return View();
        //}
    }



}