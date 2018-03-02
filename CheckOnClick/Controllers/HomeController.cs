using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CheckOnClick.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        // Logging into the System
        [HttpPost]
        public ActionResult Login(Models.LoginSignup login)
        {
            var role_type = login.CheckLogin(login.UserName, login.Password);
            if (role_type == "admin")
            {
                Session["AdminUserName"] = login.UserName;
                return new RedirectResult(@"~\Admin\");
            }
            else if (role_type == "patient")
            {
                Session["PatientUserName"] = login.UserName;
                return new RedirectResult(@"~\Patient\");
            }
            else if (role_type == "doctor")
            {
                Session["DoctorUserName"] = login.UserName;
                return new RedirectResult(@"~\Doctor\");
            }
            else
            {
                TempData["Message"] = "UserName or Password is incorrect";
                return RedirectToAction("Index", "Home");
            }
        }

        // Creating new Account for the Patient
        [HttpPost]
        public ActionResult CreateAccount(Models.LoginSignup signup)
        {

            var status = signup.CreatingNewAccount(signup.FullName, signup.PaUserName, signup.Email, signup.Contact, signup.Gender, signup.PaPassword);

            return Json(status.ToString(), JsonRequestBehavior.AllowGet);



        }

        // Logout Function
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}