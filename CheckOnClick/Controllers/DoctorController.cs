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
            return View();
        }

        public ActionResult Appointment()
        {
            return View();
        }

        public new ActionResult Profile()
        {
            return View();
        }

        public ActionResult AppointHistory()
        {
            return View();
        }

        public ActionResult SharePost()
        {
            return View();
        }
    }
}