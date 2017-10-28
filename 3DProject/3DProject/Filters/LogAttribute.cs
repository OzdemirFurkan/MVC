using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _3DProject.Models.data;

namespace _3DProject.Filters
{
    public class LogAttribute :FilterAttribute,IActionFilter,IExceptionFilter
    {
        protected DateTime start_time;

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            MyDb db = new MyDb();
            LogVM.Loglar.Add(new LogTable
           {
               Controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName,
               Action = filterContext.ActionDescriptor.ActionName,
               IslemTarihi = DateTime.Now,
               Tip = "Sonra",
               UserName = filterContext.HttpContext.User.Identity.Name.ToString()
        });
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            LogVM.Loglar.Add(new LogTable
            {
                Controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName,
                Action = filterContext.ActionDescriptor.ActionName,
                IslemTarihi = DateTime.Now,
                Tip = "Once",
                UserName = filterContext.HttpContext.User.Identity.Name.ToString()       
        });
         
    }

        public void OnException(ExceptionContext filterContext)
        {
            throw new NotImplementedException();
        }
    }
}