using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _3DProject.Models.data;

namespace _3DProject.Areas.admin.Controllers
{
    public class ManagerController : Controller
    {
       
        // GET: admin/Manager
        [Role("Admin,User")]
        //Kişi rollere sahip değil ise actiona girişi yok
        public ActionResult Index()
        {
            MyDb db = new MyDb();

            List<UserTable> users = db.UserTable.Where(i => i.IsEnabled == true).ToList();
            List<ProductTable> products = db.ProductTable.ToList();

            var model = Tuple.Create(users, products);
            // MyDb db = new MyDb();
            //var uList = db.UserTable.Where(i=>i.IsEnabled == true).ToList();
            //var pList = db.ProductTable.ToList();


            return View(model);
       } 

        [Role("Admin")]
        public ActionResult Add()
        {
            return View();
        }
    }
}