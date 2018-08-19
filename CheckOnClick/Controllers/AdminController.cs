using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CheckOnClick.Models;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.Net.Mail;

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
            getTotalCount();
            getAProfilePic();
            return View();
        }

        // Admin Profile
        public new ActionResult Profile()
        {
            if (Session["AdminUserName"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            getTotalCount();
            getAProfilePic();
            return View();
        }

        // Getting Admin Profile and display
        public JsonResult GetProfile()
        {
            Models.AdminProfile profile = new AdminProfile();
            string LiveAdmin = Session["AdminUserName"].ToString();
            DataSet data = profile.getProfileDetails(LiveAdmin);

            List<AdminProfile> profileList = new List<AdminProfile>();


            foreach (DataRow dr in data.Tables[0].Rows)
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
            if (profile.Updating(profile.Admin_FullName, profile.Admin_Contact, profile.Admin_City, profile.Admin_State, profile.Admin_Country, profile.Admin_Pin, LiveAdmin))
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
            getTotalCount();
            getAProfilePic();
            return View();
        }

        // Appointment History Page
        public ActionResult AppointHistory()
        {
            if (Session["AdminUserName"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            getTotalCount();
            getAProfilePic();
            return View();
        }

        // Roles Page
        public ActionResult Roles()
        {
            if (Session["AdminUserName"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            getTotalCount();
            getAProfilePic();
            return View();
        }

        // Departments Page
        public ActionResult Departments()
        {
            if (Session["AdminUserName"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            getTotalCount();
            getAProfilePic();
            return View();
        }

        // Specializations Page
        public ActionResult Specializations()
        {
            if (Session["AdminUserName"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            getTotalCount();
            getAProfilePic();
            return View();
        }

        // Doctors Page
        public ActionResult Doctors()
        {
            if (Session["AdminUserName"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            getTotalCount();
            showSpecDropDown();
            getAProfilePic();
            return View();
        }

        // Slot Details Page
        public ActionResult SlotDetails()
        {
            if (Session["AdminUserName"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            getTotalCount();
            getAProfilePic();
            return View();
        }

        // Patients Page
        public ActionResult Patients()
        {
            if (Session["AdminUserName"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            getTotalCount();
            getAProfilePic();
            return View();
        }

        // Feedback Page
        public ActionResult Feedbacks()
        {
            if (Session["AdminUserName"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            getAProfilePic();
            getTotalCount();
            return View();
        }

        // List of Query
        public ActionResult Query()
        {
            if (Session["AdminUserName"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            getTotalCount();
            getAProfilePic();
            return View();
        }

        //Error Page
        public ActionResult Error()
        {
            if (Session["AdminUserName"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            getTotalCount();
            getAProfilePic();
            return View();
        }

        //Upload Documents
        public ActionResult UploadDoc()
        {
            if (Session["AdminUserName"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            getTotalCount();
            getAProfilePic();
            return View();
        }

        [HttpPost]
        public ActionResult UploadDoc(FormCollection fc)
        {
            string liveAdmin = Session["AdminUserName"].ToString();
            string day = DateTime.Now.ToString("_ddMMyyyy_HHmmss_");
            HttpPostedFileBase file = Request.Files["profile_pic"];
            var filename = Guid.NewGuid().ToString("N");
            var fileext = System.IO.Path.GetExtension(file.FileName);

            if (file.ContentType == "image/jpg" || file.ContentType == "image/png" || file.ContentType == "image/jpeg")
            {
                if (fileext == ".jpg" || fileext == ".png" || fileext == ".jpeg" || fileext == ".JPG" || fileext == ".PNG" || fileext == ".JPEG")
                {
                    file.SaveAs(Server.MapPath("~/UserFiles/profilePic") + "/" + liveAdmin + day + filename + fileext);
                    ViewBag.ImagePath = "/UserFiles/profilePic/" + liveAdmin + day + filename + fileext;
                }
            }

            string role = "admin";
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbconnect"].ConnectionString);
                string saveProfilePic = "coc_updateProfilePic";
                SqlCommand cmd = new SqlCommand(saveProfilePic, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@txtliveUser", liveAdmin);
                cmd.Parameters.AddWithValue("@role", role);
                cmd.Parameters.AddWithValue("@txtImgLink", ViewBag.ImagePath);
                cmd.Parameters.Add("@txtStatus", SqlDbType.VarChar, 30);
                cmd.Parameters["@txtStatus"].Direction = ParameterDirection.Output;

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                ViewBag.PicUpStatus = cmd.Parameters["@txtStatus"].Value.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            getAProfilePic();
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
                    Staff_Qualification = dr["Staff_Qualification"].ToString(),
                    Staff_DOB = dr["Staff_DOB"].ToString(),
                    Staff_Gender = dr["Staff_Gender"].ToString(),
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
            var status = staffs.AddNewDoctor(staffs.Staff_Name, staffs.Staff_Spec_Id, staffs.Staff_Qualification, staffs.Staff_Gender, staffs.Staff_DOB_IN, staffs.Staff_BloodGroup, staffs.Staff_Email, staffs.Staff_Contact, staffs.Staff_City, staffs.Staff_State, staffs.Staff_Country, staffs.Staff_Pin);

            return Json(status.ToString(), JsonRequestBehavior.AllowGet);
        }

        //Display Specification in dropdown in Add Doctor

        public void showSpecDropDown()
        {
            Models.Specialization spec = new Specialization();
            DataSet data = spec.GetSpecDDown();
            List<SelectListItem> speclist = new List<SelectListItem>();

            foreach (DataRow dr in data.Tables[0].Rows)
            {
                speclist.Add(new SelectListItem { Text = dr["SPEC_NAME"].ToString(), Value = dr["SPEC_ID"].ToString() });
            }
            ViewBag.Specs = speclist;
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
                    PATIENT_USERNAME = dr["PATIENT_USERNAME"].ToString(),
                    PATIENT_NAME = dr["PATIENT_NAME"].ToString(),
                    DOCTOR_ID = Convert.ToInt16(dr["DOCTOR_ID"]),
                    DOCTOR_NAME = dr["STAFF_NAME"].ToString(),
                    FBDK_DESC = dr["FBDK_DESC"].ToString(),
                    RATING = Convert.ToDecimal(dr["RATING"]),
                });
            }
            var data = feedbacksList;
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult ShowSlots()
        {
            Models.Slots slots = new Slots();
            DataSet dsSlots = slots.GettingSlotList();

            List<Slots> listSlot = new List<Slots>();

            foreach (DataRow dr in dsSlots.Tables[0].Rows)
            {
                listSlot.Add(new Slots
                {
                    Slot_Id = Convert.ToInt32(dr["SLOT_ID"]),
                    Doctor_Id = Convert.ToInt32(dr["DOCTOR_ID"]),
                    Doctor_Name = dr["DOCTOR_NAME"].ToString(),
                    Slot_From = dr["TIME_FROM"].ToString(),
                    Slot_To = dr["TIME_TO"].ToString(),
                    Active = dr["ACTIVE"].ToString()

                });
            }
            var data = listSlot;
            return Json(new { data = data, }, JsonRequestBehavior.AllowGet);

        }


        public JsonResult ShowDoctorForSlot()
        {
            Models.Staffs slotDoc = new Staffs();
            DataSet ds = slotDoc.GetDoctorForSlot();
            List<SelectListItem> docList = new List<SelectListItem>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                docList.Add(new SelectListItem { Text = dr["STAFF_ID"].ToString() + " : Dr. " + dr["STAFF_NAME"].ToString() + " : " + dr["STAFF_QUALIFICATION"].ToString(), Value = dr["STAFF_ID"].ToString() });
            }

            return Json(docList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveSlots(Models.Slots slts)
        {
            string status = slts.SavingSlotsForDoc(slts.Doctor_Id, slts.Slot_From, slts.Slot_To);

            return Json(status.ToString(), JsonRequestBehavior.AllowGet);
        }

        // Adding  Patient by admin panel
        [HttpPost]
        public ActionResult AddPatient(Models.Patient patients)
        {
            var status = patients.createPatient(patients.PATIENT_NAME, patients.PATIENT_USERNAME, patients.PATIENT_EMAIL, patients.PATIENT_GENDER, patients.PATIENT_CONTACT, patients.PATIENT_DOB_IN, patients.PATIENT_BLOODGP, patients.PATIENT_CITY, patients.PATIENT_STATE, patients.PATIENT_COUNTRY, patients.PATIENT_PIN);

            return Json(status.ToString(), JsonRequestBehavior.AllowGet);
        }

        public string ShowFreshAppointment()
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbconnect"].ConnectionString);
                string getAppointQuery = "coc_getFreshAppointInAdmin";
                SqlCommand cmd = new SqlCommand(getAppointQuery, con);
                cmd.CommandType = CommandType.StoredProcedure;

                DataSet ds = new DataSet();
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(ds);
                con.Open();
                //        cmd.ExecuteNonQuery();
                con.Close();

                string JSONstring = string.Empty;
                JSONstring = JsonConvert.SerializeObject(ds);
                return JSONstring;


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //fetching count of staff members
        [HttpGet]
        public void getTotalCount()
        {

            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbconnect"].ConnectionString);
                string getQuery = "coc_getTotal";
                SqlCommand cmd = new SqlCommand(getQuery, con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@txtGetTotalDoctors", SqlDbType.Int);
                cmd.Parameters["@txtGetTotalDoctors"].Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@txtGetTotalPatients", SqlDbType.Int);
                cmd.Parameters["@txtGetTotalPatients"].Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@txtGetTotalApointments", SqlDbType.Int);
                cmd.Parameters["@txtGetTotalApointments"].Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@txtGetTotalApointmentsDone", SqlDbType.Int);
                cmd.Parameters["@txtGetTotalApointmentsDone"].Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@txtGetTotalApointmentsRemaining", SqlDbType.Int);
                cmd.Parameters["@txtGetTotalApointmentsRemaining"].Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@txtGetTotalVisitors", SqlDbType.Int);
                cmd.Parameters["@txtGetTotalVisitors"].Direction = ParameterDirection.Output;

                /*    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    adp.Fill(ds);
                    */
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();


                ViewBag.getTotalDoctors = Convert.ToInt32(cmd.Parameters["@txtGetTotalDoctors"].Value);
                ViewBag.getTotalPatients = Convert.ToInt32(cmd.Parameters["@txtGetTotalPatients"].Value);
                ViewBag.getTotalAppointments = Convert.ToInt32(cmd.Parameters["@txtGetTotalApointments"].Value);
                ViewBag.getTotalAppointmentsDone = Convert.ToInt32(cmd.Parameters["@txtGetTotalApointmentsDone"].Value);
                ViewBag.getTotalAppointmentsRemaining = Convert.ToInt32(cmd.Parameters["@txtGetTotalApointmentsRemaining"].Value);
                ViewBag.getTotalVisitors = Convert.ToInt32(cmd.Parameters["@txtGetTotalVisitors"].Value);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string FetchAppointHistory()
        {

            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbconnect"].ConnectionString);
                string getAppoint = "coc_getAppointHistoryInAdmin";
                SqlCommand cmd = new SqlCommand(getAppoint, con);

                con.Open();
                DataSet ds = new DataSet();
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(ds);

                con.Close();

                string AppointJSON = string.Empty;
                AppointJSON = JsonConvert.SerializeObject(ds);
                return AppointJSON;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //fetch admin query
        public string FetchAdminQuery()
        {

            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbconnect"].ConnectionString);
                string fetchAdminQuery = "coc_fetchAdminQuery";
                SqlCommand cmd = new SqlCommand(fetchAdminQuery, con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();
                DataSet ds = new DataSet();
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(ds);

                con.Close();

                string fetchAdminQueryJSON = string.Empty;
                fetchAdminQueryJSON = JsonConvert.SerializeObject(ds);
                return fetchAdminQueryJSON;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public void getAProfilePic()
        {
            string LiveAdmin = Session["AdminUserName"].ToString();
            string role = "admin";
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbconnect"].ConnectionString);
                string getProfilePic = "coc_getProfilePic";
                SqlCommand cmd = new SqlCommand(getProfilePic, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@liveUser", LiveAdmin);
                cmd.Parameters.AddWithValue("@role", role);
                cmd.Parameters.Add("@txtImgLink", SqlDbType.VarChar, 200);
                cmd.Parameters["@txtImgLink"].Direction = ParameterDirection.Output;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                ViewBag.GetImgLink = cmd.Parameters["@txtImgLink"].Value.ToString();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string Reply(int Qid, string Qreply)
        {

            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbconnect"].ConnectionString);
                string finalQuery = "coc_ReplyAdminQuery";
                SqlCommand cmd = new SqlCommand(finalQuery, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@txtQid", Qid);
                cmd.Parameters.AddWithValue("@txtQreply", Qreply);
                cmd.Parameters.Add("@txtresult", SqlDbType.VarChar, 50);
                cmd.Parameters["@txtresult"].Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@txtemail", SqlDbType.VarChar, 50);
                cmd.Parameters["@txtemail"].Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@txtquery", SqlDbType.VarChar, 50);
                cmd.Parameters["@txtquery"].Direction = ParameterDirection.Output;
                con.Open();
                DataSet ds = new DataSet();
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(ds);

                con.Close();

                string Email = cmd.Parameters["@txtemail"].Value.ToString();
                string Message = cmd.Parameters["@txtquery"].Value.ToString();

                if (cmd.Parameters["@txtresult"].Value.ToString() == "saved")
                {
                    string toemail = Email;
                    MailMessage mail = new MailMessage();
                    mail.From = new MailAddress("info.coc.2018@gmail.com", "Check On Click");
                    mail.To.Add(new MailAddress(toemail));
                    mail.Subject = "Query Details - Check On Click";
                    mail.Body = "Dear Candidate, Please Find Reply Regarding Your Query :\n\n Query \n :-" + Message + "\n\n Reply :-" + Qreply + " \n Thank you \n \n @Team CoC Info";

                    SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                    smtp.Credentials = new System.Net.NetworkCredential()
                    {
                        UserName = "info.coc.2018@gmail.com",
                        Password = "cocinfo2018"
                    };

                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }

                return cmd.Parameters["@txtresult"].Value.ToString();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}