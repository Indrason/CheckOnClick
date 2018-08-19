using CheckOnClick.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Net.Mail;
using System.Collections.Specialized;

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
            string LivePatient = Session["PatientUserName"].ToString();
            getName();
            getTotalPCount();
            getProfilePic();
            return View();
        }

        public string Index2()
        {
            try
            {
                SqlConnection Con = new SqlConnection();
                string path = ConfigurationManager.ConnectionStrings["dbconnect"].ConnectionString;
                Con.ConnectionString = path;
                //   DataTable dt = new DataTable();
                string getData = "coc_DoctorSharedPosts";
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand(getData, Con);
                Con.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(ds);
                string PostList = string.Empty;
                PostList = JsonConvert.SerializeObject(ds);
                return PostList;
            }
            catch (Exception)
            {

                throw;
            }
        }

        // Appointment Page
        public ActionResult Appointment()
        {
            if (Session["PatientUserName"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            getName();
            getTotalPCount();
            showSpecDropDown();
            getProfilePic();
            return View();
        }

        // Profile Page
        public new ActionResult Profile()
        {
            if (Session["PatientUserName"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            getName();
            getTotalPCount();
            getProfilePic();
            return View();
        }

        // Appointment History Page
        public ActionResult AppointHistory()
        {
            if (Session["PatientUserName"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                string LivePatient = Session["PatientUserName"].ToString();
                SqlConnection Con = new SqlConnection();
                string path = ConfigurationManager.ConnectionStrings["dbconnect"].ConnectionString;
                Con.ConnectionString = path;
                DataTable dt = new DataTable();
                try
                {
                    string getData = "coc_getPatientAppointmentHistory";

                    SqlCommand cmd = new SqlCommand(getData, Con);
                    Con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PName", LivePatient);
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(dt);
                }
                catch (Exception)
                {

                    throw;
                }
                getName();
                getTotalPCount();
                getProfilePic();
                return View(dt);
            }
        }

        // Feedback Page
        public ActionResult Feedback()
        {
            if (Session["PatientUserName"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            getName();
            getTotalPCount();
            getProfilePic();
            return View();
        }

        //AnyQuery Page
        public ActionResult AnyQuery()
        {
            if (Session["PatientUserName"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            getName();
            getTotalPCount();
            getProfilePic();
            return View();
        }

        //Error Page
        public ActionResult Error()
        {
            if (Session["PatientUserName"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            getName();
            getTotalPCount();
            getProfilePic();
            return View();
        }

        //Upload Documents
        public ActionResult UploadDoc()
        {
            if (Session["PatientUserName"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            getName();
            getTotalPCount();
            getProfilePic();
            return View();
        }

        public void getName()
        {
            string LivePatient = Session["PatientUserName"].ToString();
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbconnect"].ConnectionString);
                string getNameQuery = "coc_getPatientNameInMenu";

                SqlCommand cmd = new SqlCommand(getNameQuery, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@txtPatientUsername", LivePatient);
                cmd.Parameters.Add("@PatientName", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                ViewBag.PatientName = cmd.Parameters["@PatientName"].Value.ToString();

            }
            catch (Exception ex)
            {
                throw ex;
                //RedirectToAction("Error", "Patient");
            }
        }

        [HttpPost]
        public ActionResult UploadDoc(FormCollection fc)
        {
            getName();
            string livePatient = Session["PatientUserName"].ToString();
            string day = DateTime.Now.ToString("_ddMMyyyy_HHmmss_");
            HttpPostedFileBase file = Request.Files["profile_pic"];
            var filename = Guid.NewGuid().ToString("N");
            var fileext = System.IO.Path.GetExtension(file.FileName);

            if (file.ContentType == "image/jpg" || file.ContentType == "image/png" || file.ContentType == "image/jpeg")
            {
                if (fileext == ".jpg" || fileext == ".png" || fileext == ".jpeg" || fileext == ".JPG" || fileext == ".PNG" || fileext == ".JPEG")
                {
                    file.SaveAs(Server.MapPath("~/UserFiles/profilePic") + "/" + livePatient + day + filename + fileext);
                    ViewBag.ImagePath = "/UserFiles/profilePic/" + livePatient + day + filename + fileext;
                }
            }

            string role = "patient";
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbconnect"].ConnectionString);
                string saveProfilePic = "coc_updateProfilePic";
                SqlCommand cmd = new SqlCommand(saveProfilePic, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@txtliveUser", livePatient);
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
                //RedirectToAction("Error", "Patient");
            }

            getProfilePic();
            return View();
        }

        //Update patient profile
        public JsonResult GetProfile()
        {
            // Session["AdminUserName"] = "Indra123";
            Models.PatientProfile profile = new PatientProfile();
            string LivePatient = Session["PatientUserName"].ToString();
            DataSet data = profile.getProfileDetails(LivePatient);

            List<PatientProfile> profileList = new List<PatientProfile>();


            foreach (DataRow dr in data.Tables[0].Rows)
            {
                profileList.Add(new PatientProfile
                {
                    Patient_Name = dr["Patient_Name"].ToString(),
                    Patient_UserName = dr["Patient_Username"].ToString(),
                    Patient_Email = dr["Patient_Email"].ToString(),
                    Patient_Password = dr["Patient_Password"].ToString(),
                    Patient_Contact = dr["Patient_Contact"].ToString(),
                    Patient_DOB = dr["Patient_DOB"].ToString(),
                    Patient_BloodGP = dr["Patient_BloodGP"].ToString(),
                    Patient_Gender = dr["Patient_Gender"].ToString(),
                    Patient_City = dr["Patient_City"].ToString(),
                    Patient_State = dr["Patient_State"].ToString(),
                    Patient_Country = dr["Patient_Country"].ToString(),
                    Patient_Pin = Convert.ToInt32(dr["Patient_Pin"])
                });
            }

            return Json(profileList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateProfile(Models.PatientProfile profile)
        {
            string livePatient = Session["PatientUsername"].ToString();
            if (profile.Updating(profile.Patient_Name, profile.Patient_Contact, profile.Patient_DOB, profile.Patient_BloodGP, profile.Patient_City, profile.Patient_State, profile.Patient_Country, profile.Patient_Pin, livePatient))
            {
                return Json(new { Success = true });
            }
            return Json(new { Error = true });
        }

        //update patient password
        public JsonResult UpdatePatientPassword(Models.PatientProfile password)
        {
            string LivePatient = Session["PatientUserName"].ToString();
            var passwordStatus = password.UpdatingPassword(password.Patient_Password, password.Patient_NewPassword, LivePatient);

            return Json(passwordStatus.ToString(), JsonRequestBehavior.AllowGet);
        }

        //Inserting Feedback into the databsase
        public JsonResult FeedbackPatient(Models.PatientFeedback fdbk)
        {
            string LivePatient = Session["PatientUserName"].ToString();
            string status = fdbk.FeedbackP(fdbk.DoctorId, fdbk.Feedback, fdbk.Rating, LivePatient);

            return Json(status.ToString(), JsonRequestBehavior.AllowGet);
        }

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

        public JsonResult LoadDoctor(int Spec_Id)
        {
            Models.Staffs doc = new Staffs();
            DataSet data = doc.GetDoctorInAppoint(Spec_Id);
            List<SelectListItem> docList = new List<SelectListItem>();

            foreach (DataRow dr in data.Tables[0].Rows)
            {
                docList.Add(new SelectListItem { Text = dr["STAFF_ID"].ToString() + " : Dr. " + dr["STAFF_NAME"].ToString() + " : " + dr["STAFF_QUALIFICATION"].ToString(), Value = dr["STAFF_ID"].ToString() });
            }
            return Json(docList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadSlots(int Doc_Id)
        {
            Models.Slots slot = new Slots();
            DataSet dsSlot = slot.LoadSlotsForAppoint(Doc_Id);

            List<Slots> list = new List<Slots>();
            foreach (DataRow dr in dsSlot.Tables[0].Rows)
            {
                list.Add(new Slots
                {
                    Slot_Id = Convert.ToInt32(dr["SLOT_ID"]),
                    Slot_From = dr["TIME_FROM"].ToString(),
                    Slot_To = dr["TIME_TO"].ToString()

                });
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveAppointment(string AppointDate, int SpecId, int DocId, string AppointSlot, string PatientDesc)
        {
            string LivePatient = Session["PatientUserName"].ToString();
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbconnect"].ConnectionString);
                string saveAppointQuery = "coc_bookAppointment";
                SqlCommand cmd = new SqlCommand(saveAppointQuery, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@txtAppointDate", AppointDate);
                cmd.Parameters.AddWithValue("@txtSpecId", SpecId);
                cmd.Parameters.AddWithValue("@txtDocId", DocId);
                cmd.Parameters.AddWithValue("@txtAppointSlot", AppointSlot);
                cmd.Parameters.AddWithValue("@txtPatientDesc", PatientDesc);
                cmd.Parameters.AddWithValue("@txtPatientUserName", LivePatient);
                cmd.Parameters.Add("@txtStatus", SqlDbType.VarChar, 30);
                cmd.Parameters["@txtStatus"].Direction = ParameterDirection.Output;

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                return Json(cmd.Parameters["@txtStatus"].Value.ToString(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
                //RedirectToAction("Error", "Patient");
            }
        }

        //Checking the time is already book or not
        public JsonResult BookedSlots(string AppointDate, string Doc_Id)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbconnect"].ConnectionString);
                string getBookQuery = "coc_getBookDetails";

                SqlCommand cmd = new SqlCommand(getBookQuery, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@txtAppointDate", AppointDate);
                cmd.Parameters.AddWithValue("@txtDocId", Doc_Id);
                con.Open();
                DataSet ds = new DataSet();
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(ds);

                con.Close();

                List<Slots> list = new List<Slots>();

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    list.Add(new Slots
                    {
                        AppointDate = dr["SCHEDULE_DATE"].ToString(),
                        AppointTime = dr["SCHEDULE_TIME"].ToString()
                    });

                };

                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
                //RedirectToAction("Error", "Patient");
            }
        }


        // Getting patient details in Query
        public JsonResult GetPatientInQuery()
        {
            string LivePatient = Session["PatientUserName"].ToString();
            Models.PatientProfile patient = new PatientProfile();
            DataSet ds = patient.getProfileDetails(LivePatient);

            List<PatientProfile> list = new List<PatientProfile>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                list.Add(new PatientProfile
                {
                    Qname = dr["Patient_Name"].ToString(),
                    Qemail = dr["Patient_Email"].ToString(),
                    Qcontact = dr["Patient_Contact"].ToString()
                });
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        // Patient Feedback
        public JsonResult FeedbackPat()
        {
            Models.PatientFeedback doc = new PatientFeedback();
            string LivePatient = Session["PatientUserName"].ToString();
            DataSet data = doc.GetDoctorInFeedback(LivePatient);
            List<SelectListItem> docList = new List<SelectListItem>();

            foreach (DataRow dr in data.Tables[0].Rows)
            {
                docList.Add(new SelectListItem { Text = dr["DOCTOR_ID"].ToString() + " : Dr. " + dr["STAFF_NAME"].ToString(), Value = dr["DOCTOR_ID"].ToString() });
            }
            return Json(docList, JsonRequestBehavior.AllowGet);
        }

        //fetching count of Patient members
        [HttpGet]
        public void getTotalPCount()
        {

            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbconnect"].ConnectionString);
                string getQuery = "coc_getPTotal";
                SqlCommand cmd = new SqlCommand(getQuery, con);
                cmd.CommandType = CommandType.StoredProcedure;
                string LivePatient = Session["PatientUserName"].ToString();
                cmd.Parameters.AddWithValue("@txtpatientusername", LivePatient);

                cmd.Parameters.Add("@txtGetTodayApointment", SqlDbType.Int);
                cmd.Parameters["@txtGetTodayApointment"].Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@txtGetTotalFeedbackCount", SqlDbType.Int);
                cmd.Parameters["@txtGetTotalFeedbackCount"].Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@txtGetTotalApointmentsBooked", SqlDbType.Int);
                cmd.Parameters["@txtGetTotalApointmentsBooked"].Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@txtGetTotalApointmentsDone", SqlDbType.Int);
                cmd.Parameters["@txtGetTotalApointmentsDone"].Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@txtGetTotalApointmentsRemaining", SqlDbType.Int);
                cmd.Parameters["@txtGetTotalApointmentsRemaining"].Direction = ParameterDirection.Output;
                
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();


                ViewBag.GetTodayApointment = Convert.ToInt32(cmd.Parameters["@txtGetTodayApointment"].Value);
                ViewBag.GetTotalFeedbackCount = Convert.ToInt32(cmd.Parameters["@txtGetTotalFeedbackCount"].Value);
                ViewBag.GetTotalApointmentsBooked = Convert.ToInt32(cmd.Parameters["@txtGetTotalApointmentsBooked"].Value);
                ViewBag.GetTotalApointmentsDone = Convert.ToInt32(cmd.Parameters["@txtGetTotalApointmentsDone"].Value);
                ViewBag.GetTotalApointmentsRemaining = Convert.ToInt32(cmd.Parameters["@txtGetTotalApointmentsRemaining"].Value);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // fetching Query record for patients

        public string FetchQueryRecord()
        {
            string LivePatient = Session["PatientUserName"].ToString();
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbconnect"].ConnectionString);
                string getquery = "coc_fetchPatientQuery";
                SqlCommand cmd = new SqlCommand(getquery, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@txtpatientName", LivePatient);

                con.Open();
                DataSet ds = new DataSet();
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(ds);

                con.Close();

                string QueryJSON = string.Empty;
                QueryJSON = JsonConvert.SerializeObject(ds);
                return QueryJSON;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string SendQuery(string Qtarget, string Qname, string Qemail, string Qcontact, string Qquery)
        {
            var LivePatient = Session["PatientUserName"].ToString();
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbconnect"].ConnectionString);
                string finalQuery = "coc_sendquery";
                SqlCommand cmd = new SqlCommand(finalQuery, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@txtusername", LivePatient);
                cmd.Parameters.AddWithValue("@txttarget", Qtarget);
                cmd.Parameters.AddWithValue("@txtname", Qname);
                cmd.Parameters.AddWithValue("@txtemail", Qemail);
                cmd.Parameters.AddWithValue("@txtcontact", Qcontact);
                cmd.Parameters.AddWithValue("@txtquery", Qquery);

                cmd.Parameters.Add("@txtresult", SqlDbType.VarChar, 20);
                cmd.Parameters["@txtresult"].Direction = ParameterDirection.Output;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                return cmd.Parameters["@txtresult"].Value.ToString();

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpGet]
        public void getProfilePic()
        {
            string LivePatient = Session["PatientUserName"].ToString();
            string role = "patient";
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbconnect"].ConnectionString);
                string getProfilePic = "coc_getProfilePic";
                SqlCommand cmd = new SqlCommand(getProfilePic, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@liveUser", LivePatient);
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

        // Sending email
        public void SendEmail(string Email, string Subject, string Body)
        {
            string toemail = Email;
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("info.coc.2018@gmail.com", "Check On Click");
            mail.To.Add(new MailAddress(toemail));
            mail.Subject = Subject;
            mail.Body = Body;

            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new System.Net.NetworkCredential()
            {
                UserName = "info.coc.2018@gmail.com",
                Password = "cocinfo2018"
            };

            smtp.EnableSsl = true;
            smtp.Send(mail);
        }

        // Sending mobile message
        public void SendMessage(string msg, string Contact)
        {
            String message = HttpUtility.UrlEncode(msg);
            using (var wb = new WebClient())
            {
                byte[] response = wb.UploadValues("https://api.textlocal.in/send/", new NameValueCollection()
                {
                {"apikey" , "LUcZ25zh5Ew-UEWplbTTIn5ljSYbTvQWOdUkDyFYHD"},
                {"numbers" , Contact},
                {"message" , message},
                {"sender" , "TXTLCL"}
                });
                string result = System.Text.Encoding.UTF8.GetString(response);

            }
        }
    }
}