using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Newtonsoft.Json;
using _3DProject.Models.data;
using _3DProject.Service;

namespace _3DProject.Controllers
{
    public class AccountController : Controller
    {

        public ActionResult UnAuthorize()
        {
            return View();
        }

        public class LoginVM
        {
            [Required]
            public string EmailAddress { get; set; }

            [Required]
            public string Password { get; set; }

            public bool RememberMe { get; set; }

        }

        public class RegisterVM
        {
            [Required]
            public string UserName { get; set; }

            [Required]
            [EmailAddress]
            public string EmailAddress { get; set; }

            [Required]
            [MinLength(8)]
            [MaxLength(12)]
            public string Password { get; set; }

            [Required]
            [System.ComponentModel.DataAnnotations.Compare("Password")]
            public string PasswordAgain { get; set; }

        }
        public ActionResult Activate(string code)
        {

            using (MyDb db = new MyDb())
            {
                var user = db.UserTable.FirstOrDefault(x => x.ActivationCode == code);

                if (user != null)
                {
                    user.IsEnabled = true;
                    db.SaveChanges();
                    return Redirect("/home/profileuser");
                }
            }
            return Redirect("/home/profileuser");
        }


        public ActionResult LogOut()
        {
            //Authentication Cookie Siler.
            FormsAuthentication.SignOut();

            return Redirect("/home/index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginVM model)
        {
            var response = Request["g-recaptcha-response"];
            const string secret = "6LcsdzQUAAAAAF-Qfb_TOTD52uzxOL_y1yYlo7__";
            //Kendi Secret keyinizle değiştirin.

            //webclient recaptcha test
            var client = new WebClient();
            var reply =
                client.DownloadString(
                    string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secret, response));

            var captchaResponse = JsonConvert.DeserializeObject<CaptchaResponse>(reply);

            if (ModelState.IsValid)
            {
                using (MyDb db = new MyDb())
                {
                    var control = db.UserTable.FirstOrDefault(x => x.EmailAddress == model.EmailAddress && x.Password == model.Password && x.IsEnabled == true);


                    if (!captchaResponse.Success)
                    {
                        JsonMessageResult j2 = new JsonMessageResult();
                        j2.IsSuccess = true;
                        j2.Message = "Lutfen gerekli dogrulamalari yapiniz";
                        j2.RedirectUrl = "#";

                        return Json(j2);
                    }
                    else
                    {
                        if (control != null)
                        {
                            //kontrolu icerde yapiyorum cunku guvenlik dogrulamasindan gecmeden yanlis sifre bilgisi dondurmesin
                            //oturum aç
                            FormsAuthentication.SetAuthCookie(model.EmailAddress, model.RememberMe);

                            JsonMessageResult j1 = new JsonMessageResult();
                            j1.IsSuccess = true;
                            j1.Message = "Oturumunuz açılıyor";
                            j1.RedirectUrl = "/" + control.UserName;

                            return Json(j1);
                        }
                    }
                }
                JsonMessageResult j = new JsonMessageResult();
                j.IsSuccess = true;
                j.Message = "Kullanıcı adı veya parola hatalı!";
                j.RedirectUrl = "#";

                return Json(j);
            }
            return Json("Böyle bir Kullanıcı veya parola bulunamadı!");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterVM model)
        {
          
            if (ModelState.IsValid)
            {
                    UserTable t = new UserTable();
                    t.EmailAddress = model.EmailAddress;
                    t.UserName = model.UserName;
                    t.Password = model.Password;
                    t.IsEnabled = false;
                    t.ActivationCode = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10);

                    using (MyDb db = new MyDb())
                    {
                        db.UserTable.Add(t);
                        db.SaveChanges();

                        JsonMessageResult j1 = new JsonMessageResult();
                        j1.IsSuccess = true;
                        j1.Message = "Kullanıcı Kaydınız Yapıldı";
                        j1.RedirectUrl = "/home/index";

                        var url = Path.Combine("http://localhost:24584/account/activate/", t.ActivationCode);

                        MailService s = new MailService();
                        s.SendMessage(new MailTemplate
                        {
                            Subject = "Üyelik",
                            To = model.EmailAddress,
                            Message = "<a href=" + url + ">Üyeliği Aktif Et</a>"
                        });

                        return Json(j1);
                    }             
            }
            else
            {
                JsonMessageResult j = new JsonMessageResult();
                j.IsSuccess = true;
                j.Message = "Kullanıcı Kaydınız Yapılamadı.Lütfen tekrar Deneyiniz";
                j.RedirectUrl = "#";

                return Json(j);
            }
        }


        public class JsonMessageResult
        {
            public string Message { get; set; }
            public bool IsSuccess { get; set; }
            public string RedirectUrl { get; set; }
        }


        public class PasswordResetVM
        {
            [Required]
            [MinLength(8)]
            [MaxLength(12)]
            public string Password { get; set; }
            [System.ComponentModel.DataAnnotations.Compare("Password")]
            public string PasswordAgain { get; set; }
            public string ActivationCode { get; set; }

        }

        public class CaptchaResponse
        {

            public bool Success { get; set; }

            public List<string> ErrorCodes { get; set; }
        }

        public ActionResult ResetPassword(string code)
        {
            MyDb db = new MyDb();
            var usr = db.UserTable.FirstOrDefault(x => x.ActivationCode == code);

            if (usr != null)
            {
                ViewBag.Code = usr.ActivationCode;
                return View();
            }

            return Redirect("/home/userprofile");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(PasswordResetVM model)
        {

            var response = Request["g-recaptcha-response"];
            const string secret = "6LcSmTMUAAAAAGzVkAF-BRSXpJD6x_BRZWbDm5N-";
            //Kendi Secret keyinizle değiştirin.

            //webclient recaptcha test
            var client = new WebClient();
            var reply =
                client.DownloadString(
                    string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secret, response));

            var captchaResponse = JsonConvert.DeserializeObject<CaptchaResponse>(reply);

            if (ModelState.IsValid)
            {
                MyDb db = new MyDb();
                var usr = db.UserTable.FirstOrDefault(x => x.ActivationCode == model.ActivationCode);

                if (usr != null)
                {
                    if (!captchaResponse.Success)
                    {
                        ViewBag.Message = "Lütfen güvenliği doğrulayınız.Parola değişikliği gerçekleşti";

                    }
                    else
                    {
                        ViewBag.Message = "Güvenlik başarıyla doğrulanmıştır.";
                        usr.Password = model.Password;
                        db.SaveChanges();
                    }
                }
            }

            return View();
        }


        public ActionResult PasswordReset()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PasswordReset(string Email)
        {
            MyDb db = new MyDb();
            var usr = db.UserTable.FirstOrDefault(x => x.EmailAddress == Email && x.IsEnabled == true);

            var url = Path.Combine("http://localhost:24584/account/resetpassword/", usr.ActivationCode);

            MailTemplate t = new MailTemplate();
            t.To = Email;
            t.Message = "<a href=" + url + ">Parola Resetle</a>";
            t.Subject = "Parola Değişikliği";

            MailService service = new MailService();
            service.SendMessage(t);

            ViewBag.Message = "Lütfen eposta hesabınızı kontrol ediniz";

            return View();
        }

    }
}