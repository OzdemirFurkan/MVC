using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _3DProject.Filters;
using _3DProject.Models.data;

namespace _3DProject.Controllers
{
    public class HomeController : Controller
    {

        public List<SelectListItem> listPCategory()
        {
            MyDb db = new MyDb();
            List<SelectListItem> listPCategory = new List<SelectListItem>();

            var PCategories = db.ProductCategoryTable.ToList();

            PCategories.ForEach(x =>
            {
                SelectListItem item1 = new SelectListItem();
                item1.Text = x.categoryName;
                item1.Value = x.pcategoryID.ToString();
                item1.Selected = x.pcategoryID == 1 ? true : false;

                listPCategory.Add(item1);
            });

            return listPCategory;
        }

        // GET: Home
        public ActionResult Index(string username)
        {
            MyDb db = new MyDb();

            ViewBag.UserInfo = HttpContext.User.Identity.IsAuthenticated;

            var email = HttpContext.User.Identity.Name;
            var usr = db.UserTable.FirstOrDefault(x => x.EmailAddress == email);
            List<ProductTable> products = db.ProductTable.OrderByDescending(x => x.productAddDate).ToList();
            List<BlogTable> blogs = db.BlogTable.ToList();

            var model = Tuple.Create(products, blogs);

            //SOR
            if(HttpContext.User.Identity.IsAuthenticated)
            {
                var control = db.PictureTable.FirstOrDefault(x => x.pictureID == usr.pictureID);
             
                if (control != null)
                    ViewBag.Uimage = control.picturePath;
                else { ViewBag.Uimage = "/images/default-user-photo.png"; }
            }
            return View(model);
        }

        public ActionResult Blog()
        {
            MyDb db = new MyDb();

            var model = db.BlogTable.OrderByDescending(x => x.blogAddDate).ToList();

            return View(model);
        }

        [Authorize]
        public ActionResult ProfileUser(string username)
        {
            var email = HttpContext.User.Identity.Name;

            MyDb db = new MyDb();

            var getProductList = db.ProductTable.ToList();

            List<ProductTable> plist = new List<ProductTable>(getProductList);

            var model = db.UserTable.FirstOrDefault(x => x.EmailAddress == email);

            //SOR
            var usr = db.UserTable.FirstOrDefault(x => x.EmailAddress == email);
            var control = db.PictureTable.FirstOrDefault(x => x.pictureID == usr.pictureID);
            if (control != null)
                ViewBag.Uimage = control.picturePath;
            else { ViewBag.Uimage = "/images/default-user-photo.png"; }


            ViewBag.Email = email;
            ViewBag.Name = model.UserName;

            ViewBag.PCategories = listPCategory();

            if (model.ProductTable.Any(x => x.userID == model.Id))
            {
                ViewBag.Level = "Yeni Fotografci";
            }
            else
            {
                ViewBag.Level = "Yeni Kullanici";
            }


            if (model.pictureID == null)
            {
                ViewBag.Photo = "/images/default-user-photo.png";
            }
            else
            {
                ViewBag.Photo = model.PictureTable.picturePath;
            }
            if (model.kapakID == null)
            {
                ViewBag.KPhoto = "/kapak-images/default-kapak.jpg";
            }
            else
            {
                ViewBag.KPhoto = model.KapakPictureTable.kapakPath;
            }
            return View(model);
        }

        public ActionResult ProductDetail(string productname)
        {
            MyDb db = new MyDb();

            var model = db.ProductTable.FirstOrDefault(x => x.productName == productname);

            @ViewBag.name = productname;
            @ViewBag.image = model.productImage;

            return View(model);
        }

        public ActionResult UserDetail(string username)
        {
            MyDb db = new MyDb();
            var ident = HttpContext.User.Identity.Name;
            var identity = db.UserTable.FirstOrDefault(x => x.EmailAddress == ident);

            var model = db.UserTable.FirstOrDefault(x => x.UserName == username);

          

            if (identity != null && identity.UserName == username)
            {
                return Redirect("/{username}");
            }
            else
            {
                ViewBag.uName = model.UserName;
                ViewBag.uKphoto = model.KapakPictureTable.kapakPath;
                ViewBag.uLevel = "Yeni Kullanici";

                if (model.pictureID != null)
                {
                    ViewBag.uPhoto = model.PictureTable.picturePath;
                }
                else
                {
                    ViewBag.uPhoto = "/images/default-user-photo.png";
                };

                return View(model);
            }
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase pfile)
        {
            MyDb db = new MyDb();

            var email = HttpContext.User.Identity.Name;
            var model = db.UserTable.FirstOrDefault(x => x.EmailAddress == email);
           
            try
            {
                pfile.SaveAs(Server.MapPath("~/images/" + pfile.FileName));
                string yol = "/images/" + pfile.FileName;

                PictureTable r = new PictureTable();

                r.pictureName = pfile.FileName;
                r.picturePath = yol;
                model.pictureID = r.pictureID; // sonradan degistim test etmedim

                db.PictureTable.Add(r);
                db.SaveChanges();
                return Redirect("/Home/ProfileUser");
            }
            catch (Exception)
            {
                AccountController.JsonMessageResult j3 = new AccountController.JsonMessageResult();
                j3.IsSuccess = true;
                j3.Message = "Lutfen bir profil resmi yukleyiniz";
                j3.RedirectUrl = "#";

                return Json(j3);
            }
        }

        [HttpPost]
        //[Log]
        public ActionResult KapakUpload(HttpPostedFileBase file)
        {
            MyDb db = new MyDb();

            var email = HttpContext.User.Identity.Name;
            var model = db.UserTable.FirstOrDefault(x => x.EmailAddress == email);
            try
            {
                file.SaveAs(Server.MapPath("~/kapak-images/" + file.FileName));
                string yol = "/kapak-images/" + file.FileName;

                KapakPictureTable kp = new KapakPictureTable();

                kp.kapakPath = yol;
                model.kapakID = kp.kapakID;

                db.KapakPictureTable.Add(kp);
                db.SaveChanges();

                return Redirect("/Home/ProfileUser");
            }
            catch (Exception)
            {
                AccountController.JsonMessageResult j3 = new AccountController.JsonMessageResult();
                j3.IsSuccess = true;
                j3.Message = "Lutfen bir profil resmi yukleyiniz";
                j3.RedirectUrl = "#";

                return Json(j3);
            }
        }


    }
}