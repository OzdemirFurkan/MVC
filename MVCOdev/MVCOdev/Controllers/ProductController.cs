using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCOdev.Data;

namespace MVCOdev.Controllers
{
    public class ProductController : Controller
    {
        public RedirectResult Delete(int id)
        {
            odevDbEntities db = new odevDbEntities();
            var data = db.Urunlers.Find(id);

            db.Urunlers.Remove(data);
            db.SaveChanges();

            return Redirect("/Product/List");
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost][ValidateAntiForgeryToken]
        public ActionResult Add(Urunler model)
        {
            if (ModelState.IsValid)
            {
                odevDbEntities db = new odevDbEntities();
                model.urunAd = SlugService.Slug(model.urunAd);
                db.Urunlers.Add(model);

                bool IsSuccess = db.SaveChanges() > 0 ? true : false;
                ViewBag.Message = IsSuccess == true ? "Basarili" : "Yeniden Deneyiniz";

            }
            return View();
        }

        public ActionResult Show(string nameSlug)
        {
            odevDbEntities db = new odevDbEntities();
            var urun = db.Urunlers.FirstOrDefault(x => x.nameSlug == nameSlug);

            return View(urun);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            odevDbEntities db = new odevDbEntities();
            var data = db.Urunlers.Find(id);

            return View(data);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Urunler model)
        {
            if (ModelState.IsValid)
            {
                odevDbEntities db = new odevDbEntities();
                var data = db.Urunlers.FirstOrDefault(x => x.urunID == model.urunID);

                data.urunAd = model.urunAd;
                data.urunFiyat = model.urunFiyat;

                bool IsSuccess = db.SaveChanges() > 0 ? false : true;
                ViewBag.Message = IsSuccess == true ? "Guncellendi" : "Tekrar Deneyiniz...";
            }

            return View(model);
        }


        public ActionResult List()
        {
            odevDbEntities db = new odevDbEntities();
            var urun = db.Urunlers.ToList();

            return View(urun);
        }

        public ActionResult Detail(int id)
        {
            odevDbEntities db = new odevDbEntities();
            var urun = db.Urunlers.Find(id);

            return View(urun);
        }


    }
}