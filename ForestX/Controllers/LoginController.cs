using DAL;
using DAL.Repo;
using EmailSender;
using Logger;
using LoginModule.Logic;
using LoginModule.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ForestX.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult LogOut()
        {
            LoginLogic login = new LoginLogic();
            login.ClearCookies(Request.Cookies["LoginCookie"]);
            return Redirect("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                LoginLogic login = new LoginLogic();
                LogInDtls dtls = login.VerifyLogin(model);

                if (dtls.Verify)
                {
                    HttpCookie UserName = new HttpCookie("UserName");
                    HttpCookie LoginCookie = new HttpCookie("LoginCookie");
                    LoginCookie.Value = "true";
                    LoginCookie.Expires = DateTime.Now.AddMinutes(10);
                    UserName.Value = dtls.UserName.ToString();
                    UserName.Expires = DateTime.Now.AddMinutes(10);
                    Session["LoginStatus"] = true;
                    Response.Cookies.Add(UserName);
                    Response.Cookies.Add(LoginCookie);

                    return RedirectToAction("List", "Student");                   
                }
                else
                {
                    ViewBag.ErrorMessage = "Invalid Login Credentials";
                    return View();
                }
            }
            return View();
        }

        public ActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateUser(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                LoginLogic logic = new LoginLogic();
                UserRepo user = new UserRepo();

                if (user.CheckEmail(model.Email))
                {
                    model.Password = logic.GenerateHash(model.Password);
                    user.SaveAtLogin(model);
                    EmailBuilder.BuildEmailTemplateToNewUser(model.UserID);
                    string msg = "An account activation request has been sent to your email";
                    return RedirectToAction("CreateUser", "Login", new { msg });
                }
                else
                {
                    string msg = "This email already exist";
                    ViewBag.Msg = msg;
                    return View();
                }
            }

            return View();
        }

        //display the confirm page for new user
        public ActionResult Confirm(Guid id)
        {
            UserRepo objUserRepo = new UserRepo();
            ViewBag.regID = id;

            var userInfo = objUserRepo.GetUser(id);
            ViewBag.NewUser = userInfo.Name;
            return View();
        }

        //update the database for new user
        public JsonResult RegisterConfirm(Guid id)
        {
            UserRepo objUserRepo = new UserRepo();
            objUserRepo.ActivateAccount(id);
            var msg = "Your Email Is Verified!";
            return Json(msg, JsonRequestBehavior.AllowGet);
        }


        //display the forget password page for user\
        public ActionResult ForgetPass()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgetPass(ForgetPassModel model)
        {
            if (ModelState.IsValid)
            {
                UserRepo objUserRepo = new UserRepo();
                if (objUserRepo.CheckEmail(model.Email) != true)
                {
                    var temp = objUserRepo.GetCredentials(model.Email);
                    EmailBuilder.BuildEmailTemplateToForgetPass(temp.UserID, temp.Email);
                    string msg = "We have sent a reset password requiest to your email. Please check your email.";
                    return RedirectToAction("Login", "Login", new { msg });
                }
                else
                {
                    string msg = "This user does not exist";
                    ViewBag.Msg = msg;
                    return View();
                }
            }
            return View();
        }

        //display the reset password page for user to change their password here
        public ActionResult ResetPass(Guid id)
        {
            ViewBag.forgetPassID = id;
            return View();
        }

        //to store the new password and update in database
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPass(Guid id, ResetPassModel model)
        {
            if (ModelState.IsValid)
            {
                UserRepo objUserRepo = new UserRepo();
                LoginLogic logic = new LoginLogic();
                string pass = logic.GenerateHash(model.Password);
                objUserRepo.ChangePassword(id, pass);

                var msg = "Your password has been reset";
                return RedirectToAction("Login", "Login", new { msg });
            }
            return View();
        }
    }
}