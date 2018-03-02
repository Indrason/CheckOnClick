using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CheckOnClick.Models;

namespace CheckOnClick.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            if (Session["AdminUserName"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // Admin Profile
        public new ActionResult Profile()
        {
            if (Session["AdminUserName"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // Getting Admin Profile and display
        public JsonResult GetProfile()
        {
            Models.AdminProfile profile = new AdminProfile();
            string LiveAdmin = Session["AdminUserName"].ToString();
            DataSet data = profile.getProfileDetails(LiveAdmin);

            List<AdminProfile> profileList = new List<AdminProfile>();


            foreach(DataRow dr in data.Tables[0].Rows)
            {
                profileList.Add(new AdminProfile
                {
                    Admin_FullName = dr["Admin_FullName"].ToString(),
                    Admin_UserName = dr["Admin_UserName"].ToString(),
                    Admin_Email = dr["Admin_Email"].ToString(),
                    Admin_Contact = dr["Admin_Contact"].ToString(),
                    Admin_DOB = Convert.ToDateTime(dr["Admin_DOB"]),
                    Admin_City = dr["Admin_City"].ToString(),
                    Admin_State = dr["Admin_State"].ToString(),
                    Admin_Country = dr["Admin_Country"].ToString(),
                    Admin_Pin = Convert.ToInt32(dr["Admin_Pin"])
                });
            }
            
            return Json(profileList, JsonRequestBehavior.AllowGet);
        }

        // Updating Admin Profile
        [HttpPost]
        public ActionResult UpdateProfile(Models.AdminProfile profile)
        {
            string LiveAdmin = Session["AdminUserName"].ToString();
            if(profile.Updating(profile.Admin_FullName, profile.Admin_Contact, profile.Admin_City, profile.Admin_State, profile.Admin_Country, profile.Admin_Pin, LiveAdmin))
            {
                return Json(new { Success = true });
            }
            return Json(new { Error = true });
        }

        // Fresh Appointment - Appointment that is pending
        public ActionResult Appointment()
        {
            if (Session["AdminUserName"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // Appointment History Page
        public ActionResult AppointHistory()
        {
            if (Session["AdminUserName"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // Roles Page
        public ActionResult Roles()
        {
            if (Session["AdminUserName"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // Departments Page
        public ActionResult Departments()
        {
            if (Session["AdminUserName"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // Specializations Page
        public ActionResult Specializations()
        {
            if (Session["AdminUserName"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // Doctors Page
        public ActionResult Doctors()
        {
            if (Session["AdminUserName"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // Patients Page
        public ActionResult Patients()
        {
            if (Session["AdminUserName"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // Feedback Page
        public ActionResult Feedbacks()
        {
            if (Session["AdminUserName"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // List of Query
        public ActionResult Query()
        {
            if (Session["AdminUserName"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // Displaying Roles in the Role Page
        public JsonResult ShowRole()
        {
            Models.Roles role = new Roles();
            DataSet dsRoles = role.getRoles();

            List<Roles> roleList = new List<Roles>();


            foreach (DataRow dr in dsRoles.Tables[0].Rows)
            {
                roleList.Add(new Roles
                {
                    Role_ID = Convert.ToInt16(dr["Role_ID"]),
                    Role_Name = dr["Role_Name"].ToString(),
                    Active = dr["Active"].ToString()
                });
            }
            var data = roleList;
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        // Adding New Role from the Role Page
        [HttpPost]
        public ActionResult AddRole(Models.Roles roles)
        {
            var status = roles.createRole(roles.NewRole_Name);

            return Json(status.ToString(), JsonRequestBehavior.AllowGet);


        }
    }
}