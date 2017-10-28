using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _3DProject.Models.data;

namespace _3DProject.Controllers
{
    public class UserController : Controller
    {
        public PartialViewResult GetAUserProducts(string username)
        {
            using (MyDb db = new MyDb())
            {
            var usr = db.UserTable.FirstOrDefault(x => x.UserName == username);
            var model = db.ProductTable.Where(x => x.userID == usr.Id).ToList();

            return PartialView("_useraproducts", model);
            }
        }
        
    }
}