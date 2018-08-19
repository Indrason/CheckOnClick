using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace CheckOnClick.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            string ipAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(ipAddress))
            {
                ipAddress = Request.ServerVariables["REMOTE_ADDR"];
            }
            ViewBag.IPAddress = ipAddress;

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbconnect"].ConnectionString);
            string guestQuery = "coc_InsertIP";
            SqlCommand cmd = new SqlCommand(guestQuery, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@txtip", ipAddress);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

            showSpecDropDown();
            getTotalCount();
            return View();

        }

        public ActionResult BookAppointment()
        {
            showSpecDropDown();
            return View();
        }

        public void showSpecDropDown()
        {
            Models.Specialization spec = new Models.Specialization();
            DataSet data = spec.GetSpecDDown();
            List<SelectListItem> speclist = new List<SelectListItem>();

            foreach (DataRow dr in data.Tables[0].Rows)
            {
                speclist.Add(new SelectListItem { Text = dr["SPEC_NAME"].ToString(), Value = dr["SPEC_ID"].ToString() });
            }
            ViewBag.Specs = speclist;
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
        public ActionResult CreateAccount(string FullName, string Email, string Gender, string Contact, string PaPassword)
        {
            Models.LoginSignup signup = new Models.LoginSignup();
            var status = signup.CreatingNewAccount(FullName, Email, Contact, Gender, PaPassword);

            return Json(status.ToString(), JsonRequestBehavior.AllowGet);



        }

        // Logout Function
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
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

        public string ChangePassword(string Email)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbconnect"].ConnectionString);
                string finalQuery = "coc_changepassword";
                SqlCommand cmd = new SqlCommand(finalQuery, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@txtEmail", Email);

                cmd.Parameters.Add("@txtresult", SqlDbType.VarChar, 20);
                cmd.Parameters["@txtresult"].Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@txtusername", SqlDbType.VarChar, 20);
                cmd.Parameters["@txtusername"].Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@txtpassword", SqlDbType.VarChar, 20);
                cmd.Parameters["@txtpassword"].Direction = ParameterDirection.Output;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();


                string username = cmd.Parameters["@txtusername"].Value.ToString();
                string password = cmd.Parameters["@txtpassword"].Value.ToString();

                if (cmd.Parameters["@txtresult"].Value.ToString() == "saved")
                {
                    string toemail = Email;
                    MailMessage mail = new MailMessage();
                    mail.From = new MailAddress("info.coc.2018@gmail.com", "Check On Click");
                    mail.To.Add(new MailAddress(toemail));
                    mail.Subject = "Login Details - Check On Click";
                    mail.Body = "Dear User, Please Use these Credentials to Login in to your Account :\n Username :-" + username + "\n Password :-" + password + " \n Thank you \n \n @Team CoC Info";

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


        //sending guest query to the query table in the database
        public string sendGuestQuery(string name, string email, string contactNumber, string message)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbconnect"].ConnectionString);
                string guestQuery = "coc_saveGuestQuery";
                SqlCommand cmd = new SqlCommand(guestQuery, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@txtname", name);
                cmd.Parameters.AddWithValue("@txtemail", email);
                cmd.Parameters.AddWithValue("@txtcontactNumber", contactNumber);
                cmd.Parameters.AddWithValue("@txtmessage", message);
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

        public void getRandom()
        {
            Random generator = new Random();
            int r = generator.Next(100000, 1000000);

            ViewBag.Num = r;
            Session["OTP"] = r;
        }

        // Checking the new user is already exists or not
        public string CheckNewUser(string Email, string Contact)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbconnect"].ConnectionString);
                string query = "coc_checkUser";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@txtEmail", Email);
                cmd.Parameters.AddWithValue("@txtContact", Contact);
                cmd.Parameters.Add("@txtStatus", SqlDbType.VarChar, 40);
                cmd.Parameters["@txtStatus"].Direction = ParameterDirection.Output;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                return cmd.Parameters["@txtStatus"].Value.ToString();

            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public string SendOneTimePassword(string Email, string Contact)

        {
            string status = CheckNewUser(Email, Contact);
            if (status == "new")
            {
                getRandom();

                string msg = "Welcome to Check On Click, Please verify your account using the six digit OTP : " + ViewBag.Num + " \n Thank you";

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


                string toemail = Email;
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("info.coc.2018@gmail.com", "Check On Click");
                mail.To.Add(new MailAddress(toemail));
                mail.Subject = "Verify New Account - Check On Click";
                mail.Body = "Welocme to Check On Click, Please verify your account using the six digit OTP : " + ViewBag.Num + " \n Thank you";

                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.Credentials = new System.Net.NetworkCredential()
                {
                    UserName = "info.coc.2018@gmail.com",
                    Password = "cocinfo2018"
                };

                smtp.EnableSsl = true;
                smtp.Send(mail);
                /*        try
                        {
                            smtp.Send(mail);

                        }
                        catch(SmtpFailedRecipientException ex)
                        {
                            return "wrong";
                        }*/
                return "sent";

            }
            else if (status == "email")
            {
                return "email";
            }
            else if (status == "contact")
            {
                return "contact";
            }
            else
            {
                return "failed";
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

        // Saving guest or user appointment along with creating new account
        public string SaveNewAppointment(string AppointDate, int SpecId, int DocId, string AppointSlot, string PatientDesc, string NewName, string NewEmail, string NewContact, string NewGender, string NewPass, string UserOTP)
        {
            if (UserOTP == Session["OTP"].ToString())
            {
                try
                {
                    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbconnect"].ConnectionString);
                    string appointQuery = "coc_bookAppointmentWithNewAccount";
                    SqlCommand cmd = new SqlCommand(appointQuery, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@txtAppointDate", AppointDate);
                    cmd.Parameters.AddWithValue("@txtSpecId", SpecId);
                    cmd.Parameters.AddWithValue("@txtDocId", DocId);
                    cmd.Parameters.AddWithValue("@txtAppointSlot", AppointSlot);
                    cmd.Parameters.AddWithValue("@txtPatientDesc", PatientDesc);
                    cmd.Parameters.AddWithValue("@txtNewName", NewName);
                    cmd.Parameters.AddWithValue("@txtNewEmail", NewEmail);
                    cmd.Parameters.AddWithValue("@txtNewContact", NewContact);
                    cmd.Parameters.AddWithValue("@txtNewGender", NewGender);
                    cmd.Parameters.AddWithValue("@txtNewPass", NewPass);

                    cmd.Parameters.Add("@txtStatus", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
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
            else
            {
                return "notMatched";
            }
        }
 
    }
}