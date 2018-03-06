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

        // Getting the details of Staffs (Doctors)
        [HttpGet]
        public JsonResult ShowDoctors()
        {
            Models.Staffs staffs = new Staffs();
            DataSet dsDoctor = staffs.getDoctor();

            List<Staffs> staffList = new List<Staffs>();

            foreach (DataRow dr in dsDoctor.Tables[0].Rows)
            {
                staffList.Add(new Staffs
                {
                    Staff_ID = Convert.ToInt32(dr["Staff_ID"]),
                    Staff_Name = dr["Staff_Name"].ToString(),
                    Staff_Spec = dr["Spec_Name"].ToString(),
                    Staff_DOB = dr["Staff_DOB"].ToString(),
                    Staff_BloodGroup = dr["Staff_BloodGp"].ToString(),
                    Staff_Email = dr["Staff_Email"].ToString(),
                    Staff_Contact = dr["Staff_Contact"].ToString(),
                    Staff_City = dr["Staff_City"].ToString(),
                    Staff_State = dr["Staff_State"].ToString(),
                    Staff_Country = dr["Staff_Country"].ToString(),
                    Staff_Pin = Convert.ToInt32(dr["Staff_Pin"]),
                    Staff_Acnt_Date = dr["Staff_Acnt_Date"].ToString(),
                    Active = dr["Active"].ToString()
                });
            }
            var data = staffList;
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        // Adding  Doctor
        [HttpPost]
        public ActionResult AddDoctor(Models.Staffs staffs)
        {
            var status = staffs.AddNewDoctor(staffs.Staff_Name, staffs.Staff_Spec, staffs.Staff_DOB, staffs.Staff_BloodGroup, staffs.Staff_Email, staffs.Staff_Contact, staffs.Staff_City, staffs.Staff_State, staffs.Staff_Country, staffs.Staff_Pin);

            return Json(status.ToString(), JsonRequestBehavior.AllowGet);
        }

        //Display Specification in dropdown in Add Doctor

        public void showSpecDropDown()
        {

        }

        //Updating Admin Password
        public JsonResult UpdateAdminPassword(Models.AdminProfile password)
        {
            string LiveAdmin = Session["AdminUserName"].ToString();
            var passwordStatus = password.UpdatingPassword(password.Admin_Password, password.Admin_NewPassword, LiveAdmin);

            return Json(passwordStatus.ToString(), JsonRequestBehavior.AllowGet);
        }

        // Displaying list of specialization in admin
        public JsonResult ShowSpec()
        {
            Models.Specialization spec = new Specialization();
            //   string LiveAdmin = Session["AdminUserName"].ToString();
            DataSet dsSpecialization = spec.getspecialization();

            List<Specialization> specList = new List<Specialization>();


            foreach (DataRow dr in dsSpecialization.Tables[0].Rows)
            {
                specList.Add(new Specialization
                {
                    SPEC_ID = Convert.ToInt16(dr["SPEC_ID"]),
                    SPEC_Name = dr["SPEC_Name"].ToString(),
                    Active = dr["Active"].ToString()
                });
            }
            var data = specList;
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        // Adding new Specialization by the admin
        [HttpPost]
        public ActionResult AddSpec(Models.Specialization spec)
        {
            var status = spec.createSpec(spec.NewSPEC_Name);

            return Json(status.ToString(), JsonRequestBehavior.AllowGet);


        }

        // Displaying list of Patients in the Admin Panel
        public JsonResult ShowPatient()
        {
            Models.Patient Patients = new Patient();
            //   string LiveAdmin = Session["AdminUserName"].ToString();
            DataSet dspatient = Patients.getPatient();

            List<Patient> patientsList = new List<Patient>();


            foreach (DataRow dr in dspatient.Tables[0].Rows)
            {
                patientsList.Add(new Patient
                {
                    PATIENT_ID = Convert.ToInt32(dr["PATIENT_ID"]),
                    PATIENT_NAME = dr["PATIENT_NAME"].ToString(),
                    PATIENT_USERNAME = dr["PATIENT_USERNAME"].ToString(),
                    PATIENT_EMAIL = dr["PATIENT_EMAIL"].ToString(),
                    PATIENT_GENDER = dr["PATIENT_GENDER"].ToString(),
                    PATIENT_CONTACT = dr["PATIENT_CONTACT"].ToString(),
                    PATIENT_DOB = dr["PATIENT_DOB"].ToString(),
                    PATIENT_BLOODGP = dr["PATIENT_BLOODGP"].ToString(),
                    PATIENT_CITY = dr["PATIENT_CITY"].ToString(),
                    PATIENT_STATE = dr["PATIENT_STATE"].ToString(),
                    PATIENT_COUNTRY = dr["PATIENT_COUNTRY"].ToString(),
                    PATIENT_PIN = Convert.ToInt32(dr["PATIENT_PIN"]),
                    PATIENT_ACNT_DATE = dr["PATIENT_ACNT_DATE"].ToString(),
                    ACTIVE = dr["Active"].ToString()
                });
            }
            var data = patientsList;
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        // Display list of Feedbacks in the Admin
        public JsonResult ShowFeedback()
        {
            Models.Feedback feedbacks = new Feedback();
            //   string LiveAdmin = Session["AdminUserName"].ToString();
            DataSet dsFeedback = feedbacks.getFeedback();

            List<Feedback> feedbacksList = new List<Feedback>();


            foreach (DataRow dr in dsFeedback.Tables[0].Rows)
            {
                feedbacksList.Add(new Feedback
                {
                    FDBK_ID = Convert.ToInt16(dr["FDBK_ID"]),
                    PATIENT_ID = Convert.ToInt16(dr["PATIENT_ID"]),
                    PATIENT_NAME = dr["PATIENT_NAME"].ToString(),
                    DOCTOR_ID = Convert.ToInt16(dr["DOCTOR_ID"]),
                    DOCTOR_NAME = dr["DOCTOR_NAME"].ToString(),
                    FBDK_DESC = dr["FBDK_DESC"].ToString(),
                    RATING = Convert.ToDecimal(dr["RATING"]),
                });
            }
            var data = feedbacksList;
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }
    }
}