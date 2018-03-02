using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CheckOnClick.Controllers
{
    public class DoctorController : Controller
    {
        // GET: Doctor
        public ActionResult Index()
        {
            if (Session["DoctorUserName"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // Appointment Page
        public ActionResult Appointment()
        {
            if (Session["DoctorUserName"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // Profile Page
        public new ActionResult Profile()
        {
            if (Session["DoctorUserName"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // Apointment History Page
        public ActionResult AppointHistory()
        {
            if (Session["DoctorUserName"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // Share Post Page
        public ActionResult SharePost()
        {
            if (Session["DoctorUserName"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
    }
}