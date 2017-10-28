using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _3DProject.Filters;
using _3DProject.Models.data;

namespace _3DProject.Controllers
{  
    public class ProductController : Controller
    {
        public class ProductVM
        {
            [Required]
            public string productName { get; set; }
            public string productDescription { get; set; }
            public string productImage { get; set; }
            public DateTime productAddDate { get; set; }
            public string listPCategory { get; set; }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddProduct(ProductVM model, HttpPostedFileBase productFile) //
        {
            Random rName = new Random();
            int rNames = 0;
            var email = HttpContext.User.Identity.Name;


            using (MyDb db = new MyDb()) //sor
            {
                var usr = db.UserTable.FirstOrDefault(x => x.EmailAddress == email);

                if (ModelState.IsValid)
                {
                   rNames = rName.Next(0, 100);
                productFile.SaveAs(Server.MapPath("~/product-images/" + rNames + productFile.FileName));
                   string yol = "/product-images/" + rNames + productFile.FileName ;

                    ProductTable p = new ProductTable();
                    p.productName = model.productName; //Modellernden gelenleri veritabanini isliyorum
                    p.productDescription = model.productDescription;
                    p.productImage = yol;
                    p.productAddDate = DateTime.Now;
                    //p.productCategoryID = model.listPCategory;

                    if (usr != null)
                        p.userID = usr.Id; //Authenticate olmus kullanicinin emailinden yakalayip idisini cekiyorum ve veritabanina ekliyorum
                    //p.productLike = 0;

                    db.ProductTable.Add(p);
                    db.SaveChanges();
                }
            }
            return Redirect("/Home/ProfileUser");
        }
        public PartialViewResult GetUserProducts()
        {
            MyDb db= new MyDb();
            var email = HttpContext.User.Identity.Name;

            var usr = db.UserTable.FirstOrDefault(x => x.EmailAddress == email);
            var model = db.ProductTable.Where(x=>x.userID == usr.Id).ToList();

            return PartialView("_userproducts",model);
        }

    }
}