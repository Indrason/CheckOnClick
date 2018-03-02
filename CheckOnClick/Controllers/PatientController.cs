using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CheckOnClick.Controllers
{
    public class PatientController : Controller
    {
        // GET: Patient
        public ActionResult Index()
        {
            if (Session["PatientUserName"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // Appointment Page
        public ActionResult Appointment()
        {
            if (Session["PatientUserName"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // Profile Page
        public new ActionResult Profile()
        {
            if (Session["PatientUserName"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // Appointment History Page
        public ActionResult AppointHistory()
        {
            if (Session["PatientUserName"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // Feedback Page
        public ActionResult Feedback()
        {
            if (Session["PatientUserName"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
    }
}