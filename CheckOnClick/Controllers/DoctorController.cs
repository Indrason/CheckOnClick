using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CheckOnClick.Models;
using Newtonsoft.Json;

using DHTMLX.Scheduler;
using DHTMLX.Common;
using DHTMLX.Scheduler.Data;
using DHTMLX.Scheduler.Controls;


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


            var scheduler = new DHXScheduler(this);

            //scheduler.Config.event_duration = 60;



            scheduler.InitialView = "day";
            scheduler.Config.first_hour = 9;
            scheduler.Config.last_hour = 18;
            scheduler.LoadData = true;
            scheduler.EnableDataprocessor = true;



            getTotalDCount();
            getDProfilePic();

            return View(scheduler);
        }


        public ActionResult Appointment()
        {
            if (Session["DoctorUserName"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            getTotalDCount();
            getDProfilePic();
            return View();
        }

        public ActionResult AppointHistory()
        {
            if (Session["DoctorUserName"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            getTotalDCount();
            getDProfilePic();
            return View();
        }

        [HttpGet]
        public ActionResult ReplyQuery()
        {
            if (Session["DoctorUserName"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            getTotalDCount();
            getDProfilePic();
            return View();
        }

        [HttpGet]
        public ActionResult SharePost()
        {
            if (Session["DoctorUserName"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            getTotalDCount();
            getDProfilePic();
            return View();
        }

        public new ActionResult Profile()
        {
            if (Session["DoctorUserName"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            getTotalDCount();
            getDProfilePic();
            return View();
        }

        //Error Page
        public ActionResult Error()
        {
            if (Session["DoctorUserName"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            getTotalDCount();
            getDProfilePic();
            return View();
        }

        //Upload Documents
        public ActionResult UploadDoc()
        {
            if (Session["DoctorUserName"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            getTotalDCount();
            getDProfilePic();
            return View();
        }

        [HttpPost]
        public ActionResult UploadDoc(FormCollection fc)
        {
            string liveDoctor = Session["DoctorUserName"].ToString();
            string day = DateTime.Now.ToString("_ddMMyyyy_HHmmss_");
            HttpPostedFileBase file = Request.Files["profile_pic"];
            var filename = Guid.NewGuid().ToString("N");
            var fileext = System.IO.Path.GetExtension(file.FileName);

            if (file.ContentType == "image/jpg" || file.ContentType == "image/png" || file.ContentType == "image/jpeg")
            {
                if (fileext == ".jpg" || fileext == ".png" || fileext == ".jpeg" || fileext == ".JPG" || fileext == ".PNG" || fileext == ".JPEG")
                {
                    file.SaveAs(Server.MapPath("~/UserFiles/profilePic") + "/" + liveDoctor + day + filename + fileext);
                    ViewBag.ImagePath = "/UserFiles/profilePic/" + liveDoctor + day + filename + fileext;
                }
            }

            string role = "doctor";
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbconnect"].ConnectionString);
                string saveProfilePic = "coc_updateProfilePic";
                SqlCommand cmd = new SqlCommand(saveProfilePic, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@txtliveUser", liveDoctor);
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

            getDProfilePic();
            return View();
        }




        //Reminder calendar display

        public ContentResult Data()
        {
            int livedoc = Convert.ToInt16(Session["DoctorUserName"]);

            var data = new SchedulerAjaxData(new DocreminderDataContext().Docreminders.Where(ev => ev.USERNAME == livedoc));

            return (data);
        }

        //Reminder calendar for saving data

        public ContentResult Save(int? id, FormCollection actionValues)
        {
            actionValues.Add("USERNAME", Session["DoctorUserName"].ToString());

            var action = new DataAction(actionValues);

            try
            {
                var changedEvent = (Docreminder)DHXEventsHelper.Bind(typeof(Docreminder), actionValues);

                var data = new DocreminderDataContext();

                switch (action.Type)
                {
                    case DataActionTypes.Insert:
                        data.Docreminders.InsertOnSubmit(changedEvent);
                        break;
                    case DataActionTypes.Delete:
                        changedEvent = data.Docreminders.SingleOrDefault(ev => ev.id == action.SourceId);
                        data.Docreminders.DeleteOnSubmit(changedEvent);
                        break;
                    default:// "update"                          
                        var docreminderToUpdate = data.Docreminders.SingleOrDefault(ev => ev.id == action.SourceId);
                        DHXEventsHelper.Update(docreminderToUpdate, changedEvent, new List<string>() { "id" });
                        break;
                }
                data.SubmitChanges();
                action.TargetId = changedEvent.id;

            }

            catch
            {
                action.Type = DataActionTypes.Error;
            }
            return (ContentResult)new AjaxSaveResponse(action);
        }

        //displaying Today reminders
        public string getTodayReminders()
        {

            CheckOnClick.Models.DoctorProfile getTodayReminder = new CheckOnClick.Models.DoctorProfile();

            string LiveDoc = Session["DoctorUserName"].ToString();

            DataSet data = getTodayReminder.getTodayRemindershow(LiveDoc);


            string JSONstring = string.Empty;
            JSONstring = JsonConvert.SerializeObject(data);
            return JSONstring;

        }

        public ActionResult appointmenthistory()
        {


            CheckOnClick.Models.DoctorProfile History = new CheckOnClick.Models.DoctorProfile();

            string LiveDoc = Session["DoctorUserName"].ToString();

            DataSet data = History.getappointHistory(LiveDoc);



            List<DoctorProfile> HistoryList = new List<DoctorProfile>();


            foreach (DataRow dr in data.Tables[0].Rows)
            {
                HistoryList.Add(new DoctorProfile
                {
                    APPOINT_ID = Convert.ToInt32(dr["APPOINT_ID"]),
                    PATIENT_NAME = dr["PATIENT_NAME"].ToString(),
                    SCHEDULE_DATE = dr["SCHEDULE_DATE"].ToString(),
                    SCHEDULE_TIME = dr["SCHEDULE_TIME"].ToString(),
                    APPOINT_STATUS = dr["APPOINT_STATUS"].ToString(),
                    BOOKED_DATE = dr["BOOKED_DATE"].ToString(),
                    APPOINT_DESC = dr["APPOINT_DESC"].ToString(),
                    PRESCRIPTION = dr["PRESCRIPTION"].ToString(),
                    MEDICINE = dr["MEDICINE"].ToString(),
                    CANCEL_REASON = dr["CANCEL_REASON"].ToString(),
                });
            }
            var display = HistoryList;
            return Json(new { data = display }, JsonRequestBehavior.AllowGet);
        }




        public JsonResult GetProfile()
        {
            CheckOnClick.Models.DoctorProfile profile = new DoctorProfile();
            // string LiveAdmin = Session["AdminUserName"].ToString();
            string LiveDoc = Session["DoctorUserName"].ToString();
            //string LiveDoc = "jnp";
            DataSet data = profile.getProfileDetails(LiveDoc);

            List<DoctorProfile> profileList = new List<DoctorProfile>();


            foreach (DataRow dr in data.Tables[0].Rows)
            {
                profileList.Add(new DoctorProfile
                {
                    Name = dr["STAFF_NAME"].ToString(),
                    Specification = dr["SPEC_NAME"].ToString(),
                    BloodGroup = dr["STAFF_BLOODGP"].ToString(),
                    Email = dr["STAFF_EMAIL"].ToString(),
                    Contact = dr["STAFF_CONTACT"].ToString(),
                    DOBdoctor = dr["STAFF_DOB"].ToString(),
                    City = dr["STAFF_CITY"].ToString(),
                    State = dr["STAFF_STATE"].ToString(),
                    Country = dr["STAFF_COUNTRY"].ToString(),
                    Gender = dr["STAFF_GENDER"].ToString(),
                    Pincode = Convert.ToString(dr["STAFF_PIN"])

                });
            }

            return Json(profileList, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult UpdateProfile(CheckOnClick.Models.DoctorProfile profile)
        {
            string liveDoc = Session["DoctorUserName"].ToString();
            if (profile.Updating(profile.Contact, profile.City, profile.State, profile.Country, profile.Pincode, liveDoc))
            {
                return Json(new { Success = true });
            }
            return Json(new { Error = true });
        }
        /*    [HttpPost]
            public JsonResult Sharepost(CheckOnClick.Models.DoctorProfile pst)
            {
                string liveDoc = Session["DoctorUserName"].ToString();
                string status = pst.SharepostP(pst.DoctorName, pst.Post, liveDoc);

                return Json(status.ToString(), JsonRequestBehavior.AllowGet);
            }*/
        public JsonResult UpdateDocPassword(Models.DoctorProfile password)
        {
            string LiveDoc = Session["DoctorUserName"].ToString();
            var passwordStatus = password.UpdatingDocPassword(password.Doc_CurrentPassword, password.Doc_NewPassword, LiveDoc);

            return Json(passwordStatus.ToString(), JsonRequestBehavior.AllowGet);
        }


        public string PendingAppoint()
        {
            string LiveDoctor = Session["DoctorUserName"].ToString();
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbconnect"].ConnectionString);
                string getAppoint = "coc_getDocappointment";
                SqlCommand cmd = new SqlCommand(getAppoint, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@docName", LiveDoctor);

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


        //Confirm the appointment by doctor
        public string ConfirmBook(int AppointId)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbconnect"].ConnectionString);
                string query = "coc_confirmAppointment";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@txtAppoint_Id", AppointId);
                cmd.Parameters.Add("@txtStatus", SqlDbType.VarChar, 30);
                cmd.Parameters["@txtStatus"].Direction = ParameterDirection.Output;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                return cmd.Parameters["@txtStatus"].Value.ToString();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Fetch Patient Details For Final Appointment
        public string FetchPatDetailsForFinalAppointment(int AppointId)
        {
            string LiveDoctor = Session["DoctorUserName"].ToString();
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbconnect"].ConnectionString);
                string fetchQuery = "coc_fetchDetailForFinalAppointment";
                SqlCommand cmd = new SqlCommand(fetchQuery, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@txtAppointId", AppointId);
                cmd.Parameters.AddWithValue("@txtDoctor", LiveDoctor);

                DataSet ds = new DataSet();
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                con.Open();
                adp.Fill(ds);
                con.Close();

                string JSONdata = JsonConvert.SerializeObject(ds);

                return JSONdata;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Save Final Appointment 
        public string SaveFinalAppointment(int appointID, string patPrescription, string patMedicine)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbconnect"].ConnectionString);
                string finalQuery = "coc_saveFinalAppointment";
                SqlCommand cmd = new SqlCommand(finalQuery, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@txtAppointID", appointID);
                cmd.Parameters.AddWithValue("@txtPrescription", patPrescription);
                cmd.Parameters.AddWithValue("@txtMedicine", patMedicine);
                cmd.Parameters.Add("@txtStatus", SqlDbType.VarChar, 30);
                cmd.Parameters["@txtStatus"].Direction = ParameterDirection.Output;

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                return cmd.Parameters["@txtStatus"].Value.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Cancel Appointment before confirmation
        public string CancelAppointment(int Appoint_Id)
        {
            string user = "doctor";

            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbconnect"].ConnectionString);
                string query = "coc_CancelAppointB4Confirm";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@txtAppointId", Appoint_Id);
                cmd.Parameters.AddWithValue("@txtUser", user);
                cmd.Parameters.Add("@txtResult", SqlDbType.VarChar, 30).Direction = ParameterDirection.Output;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                return cmd.Parameters["@txtResult"].Value.ToString();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        //Cancel Appointment After Confirmation
        public string CancelAppointmentAfterConfirm(int AppointId, string Reason)
        {
            string user = "doctor";
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbconnect"].ConnectionString);
                string queryCancel = "coc_CancelAppointAfterConfirm";
                SqlCommand cmd = new SqlCommand(queryCancel, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@txtAppointID", AppointId);
                cmd.Parameters.AddWithValue("@txtReason", Reason);
                cmd.Parameters.AddWithValue("@txtUser", user);
                cmd.Parameters.Add("@txtResult", SqlDbType.VarChar, 30).Direction = ParameterDirection.Output;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                return cmd.Parameters["@txtResult"].Value.ToString();

            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        //fetching count of Doctor members
        [HttpGet]
        public void getTotalDCount()
        {

            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbconnect"].ConnectionString);
                string getQuery = "coc_getTotalDCount";
                SqlCommand cmd = new SqlCommand(getQuery, con);
                cmd.CommandType = CommandType.StoredProcedure;
                string LiveDoc = Session["DoctorUserName"].ToString();
                cmd.Parameters.AddWithValue("@txtDocName", LiveDoc);


                cmd.Parameters.Add("@txtGetDTodayTotalApointment", SqlDbType.Int);
                cmd.Parameters["@txtGetDTodayTotalApointment"].Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@txtGetDTotalApointment", SqlDbType.Int);
                cmd.Parameters["@txtGetDTotalApointment"].Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@txtGetDTotalApointmentDone", SqlDbType.Int);
                cmd.Parameters["@txtGetDTotalApointmentDone"].Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@txtGetDTotalApointmentRemaining", SqlDbType.Int);
                cmd.Parameters["@txtGetDTotalApointmentRemaining"].Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@txtGetTotalApointmentToApprove", SqlDbType.Int);
                cmd.Parameters["@txtGetTotalApointmentToApprove"].Direction = ParameterDirection.Output;

                /*    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    adp.Fill(ds);
                    */
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();


                ViewBag.GetDTodayTotalApointment = Convert.ToInt32(cmd.Parameters["@txtGetDTodayTotalApointment"].Value);
                ViewBag.GetDTotalApointment = Convert.ToInt32(cmd.Parameters["@txtGetDTotalApointment"].Value);
                ViewBag.GetDTotalApointmentDones = Convert.ToInt32(cmd.Parameters["@txtGetDTotalApointmentDone"].Value);
                ViewBag.GetDTotalApointmentRemaining = Convert.ToInt32(cmd.Parameters["@txtGetDTotalApointmentRemaining"].Value);
                ViewBag.GetTotalApointmentToApprove = Convert.ToInt32(cmd.Parameters["@txtGetTotalApointmentToApprove"].Value);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // fetch query record for doctor
        public string FetchQueryRecord()
        {
            string LiveDoc = Session["DoctorUserName"].ToString();
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbconnect"].ConnectionString);
                string finalQuery = "coc_fetchDoctorQuery";
                SqlCommand cmd = new SqlCommand(finalQuery, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@targetName", "18012");
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

        // fetch query record for doctor
        public string Reply(int Qid, string Qreply)
        {

            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbconnect"].ConnectionString);
                string finalQuery = "coc_ReplyQuery";
                SqlCommand cmd = new SqlCommand(finalQuery, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@txtQid", Qid);
                cmd.Parameters.AddWithValue("@txtreply", Qreply);
                cmd.Parameters.Add("@txtresult", SqlDbType.VarChar, 500);
                cmd.Parameters["@txtresult"].Direction = ParameterDirection.Output;
                con.Open();
                DataSet ds = new DataSet();
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(ds);

                con.Close();


                return cmd.Parameters["@txtresult"].Value.ToString();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public void getDProfilePic()
        {
            string LiveDoctor = Session["DoctorUserName"].ToString();
            string role = "doctor";
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbconnect"].ConnectionString);
                string getProfilePic = "coc_getProfilePic";
                SqlCommand cmd = new SqlCommand(getProfilePic, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@liveUser", LiveDoctor);
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


        // Saving Share Post from the doctor
        //    [HttpPost]
        public JsonResult SavePost(string postTitle, string postBody, string tags)
        {
            string liveDoctor = Session["DoctorUserName"].ToString();
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbconnect"].ConnectionString);
                string savePost = "coc_saveDoctorPost";
                SqlCommand cmd = new SqlCommand(savePost, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@txtTitle", postTitle);
                cmd.Parameters.AddWithValue("@txtBody", postBody);
                cmd.Parameters.AddWithValue("@txtTags", tags);
                cmd.Parameters.AddWithValue("@txtDoctor", liveDoctor);
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
            }
        }


        //Get list of Feedback
        public string GetPatFeedback()
        {
            string LiveDoctor = Session["DoctorUserName"].ToString();
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbconnect"].ConnectionString);
                string getpostQuery = "coc_getPatientFeedback";
                SqlCommand cmd = new SqlCommand(getpostQuery, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@txtDoc", LiveDoctor);
                DataSet ds = new DataSet();
                con.Open();
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(ds);
                con.Close();

                string PostList = string.Empty;
                PostList = JsonConvert.SerializeObject(ds);
                return PostList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Getting detail history for patient at Appointment Page
        public string ShowAppointDetailsHistory(int AppointId)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbconnect"].ConnectionString);
                string detail = "coc_getPatientDetailsInDocAppointment";
                SqlCommand cmd = new SqlCommand(detail, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@txtAppointId", AppointId);
                con.Open();
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adp.Fill(ds);
                con.Close();

                string DetailData = string.Empty;
                DetailData = JsonConvert.SerializeObject(ds);

                return DetailData;
            }
            catch (Exception)
            {
                throw;
            }
        }

        // Cancel the Apointment if the Patient did not turn up on the Booked Date
        public string CancelAppointNoPatient(int AppointId)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbconnect"].ConnectionString);
                string cancelQuery = "coc_CancelAppointNoPatient";
                SqlCommand cmd = new SqlCommand(cancelQuery, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@txtAppointID", AppointId);
                cmd.Parameters.Add("@txtStatus", SqlDbType.VarChar, 30);
                cmd.Parameters["@txtStatus"].Direction = ParameterDirection.Output;

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                return cmd.Parameters["@txtStatus"].Value.ToString();
            }
            catch(Exception)
            {
                throw;
            }
        }
    }
}