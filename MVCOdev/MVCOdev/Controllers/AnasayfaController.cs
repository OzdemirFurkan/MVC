using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCOdev.Data;

namespace MVCOdev.Controllers
{
    public class AnasayfaController : Controller
    {

        public ActionResult index()
        {
            odevDbEntities db = new odevDbEntities();
            var product = db.Urunlers.ToList();

            return View(product);
        }
    }
}