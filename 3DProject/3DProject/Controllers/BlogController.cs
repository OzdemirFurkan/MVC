using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _3DProject.Models.data;

namespace _3DProject.Controllers
{
    public class BlogController : Controller
    {
        public class BlogVM
        {
            [Required]
            public string blogName { get; set; }
            public string blogContent { get; set; }
            public string blogImage { get; set; }
            public DateTime blogAddDate { get; set; }
            //public string listPCategory { get; set; }
        }

        [HttpGet]
        public ActionResult AddBlog()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddBlog(BlogVM model, HttpPostedFileBase blogImage)
        {
            Random rName = new Random();
            int rNames = 0;

            var email = HttpContext.User.Identity.Name;

            using (MyDb db = new MyDb())
            {
                var usr = db.UserTable.FirstOrDefault(x => x.EmailAddress == email);

                if (ModelState.IsValid)
                {
                    rNames = rName.Next(0, 100);
                    blogImage.SaveAs(Server.MapPath("~/blog-images/" + rNames + blogImage.FileName));
                    string yol = "~/blog-images/" + rNames + blogImage.FileName;

                    BlogTable b = new BlogTable();
                    b.blogName = model.blogName;
                    b.blogContent = model.blogContent;
                    b.blogImage = yol;
                    b.blogAddDate = DateTime.Now;

                    if (usr != null) b.userID = usr.Id;

                    db.BlogTable.Add(b);
                    db.SaveChanges();
                }
            }
            return Redirect("/Home/ProfileUser");
        }
    }
}