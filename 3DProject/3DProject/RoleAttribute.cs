using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _3DProject.Models.data;

namespace _3DProject
{
    public class RoleAttribute : AuthorizeAttribute
    {
        List<string> roles;

        public RoleAttribute(string _roles)
        {
            roles = _roles.Split(',').ToList();
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {

                filterContext.Result = new RedirectResult("/account/unauthorize");
            }
            else
            {
                var email = HttpContext.Current.User.Identity.Name;
                MyDb db = new MyDb();
                var usr = db.UserTable.FirstOrDefault(x => x.EmailAddress == email);

                var usrroles = usr.UserRoleTable.Select(a => a.Name).ToList();
                bool IsConfirm = false;

                //kullanıcının databasedeki rolü sistemde belirtilen role attribute içine yazılan rol isimlerinden biri mi ? eğer uyuşmuyorsa o zaman kişinin yetki yok sayfasına yönlendirilmesi lazım.

                foreach (var allowedRole in roles)
                {
                    foreach (var userRole in usrroles)
                    {
                        if (allowedRole == userRole)
                        {
                            IsConfirm = true;
                            break;
                        }
                    }
                }

                if (!IsConfirm)
                {
                    filterContext.Result = new RedirectResult("/account/unauthorize");
                }
            }
        }
    }
}