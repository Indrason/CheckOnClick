using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CheckOnClick.Models
{
    public class DoctorProfile
    {

        [Required(ErrorMessage = "*")]
        [DataType(DataType.Text)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "*")]
        [DataType(DataType.Text)]
        [Display(Name = "Specification")]
        public string Specification { get; set; }

        [Required]
        [Display(Name = "Dateofbirth")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Dateofbirth { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Dateofbirth")]
        public string DOBdoctor { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "BloodGroup")]
        [DataType(DataType.Text)]
        public string BloodGroup { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Gender")]
        [DataType(DataType.Text)]
        public string Gender { get; set; }

        [Required(ErrorMessage = "*")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "*")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Contact")]
        public string Contact { get; set; }


        [Display(Name = "Role")]
        [DataType(DataType.Text)]
        public string Role { get; set; }


        [Required(ErrorMessage = "*")]
        [DataType(DataType.Text)]
        [Display(Name = "Country")]
        public string Country { get; set; }

        [Required(ErrorMessage = "*")]
        [DataType(DataType.Text)]
        [Display(Name = "State")]
        public string State { get; set; }

        [Required(ErrorMessage = "*")]
        [DataType(DataType.Text)]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required(ErrorMessage = "*")]
        [DataType(DataType.PostalCode)]
        [Display(Name = "Pincode")]
        public string Pincode { get; set; }


        public DataSet getTodayRemindershow(string _docUserName)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbconnect"].ConnectionString);
            string getData = "coc_showTodayReminders";

            SqlCommand cmd = new SqlCommand(getData, con);
            con.Open();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@docName", _docUserName);

            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();

            adp.Fill(ds);


            return ds;

        }


        public DataSet getProfileDetails(string _docUserName)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbconnect"].ConnectionString);
            string getData = "coc_getDocProfile";

            SqlCommand cmd = new SqlCommand(getData, con);
            con.Open();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@docName", _docUserName);

            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            adp.Fill(ds);

            return ds;
        }


        public bool Updating(string _Contact, string _City, string _State, string _Country, string _Pincode, string liveDoc)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbconnect"].ConnectionString);
                string updateQuery = "coc_updateDoctorProfile";
                SqlCommand cmd = new SqlCommand(updateQuery, con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmd.Parameters.AddWithValue("@txtcontact", _Contact);
                cmd.Parameters.AddWithValue("@txtcity", _City);
                cmd.Parameters.AddWithValue("@txtstate", _State);
                cmd.Parameters.AddWithValue("@txtcountry", _Country);
                cmd.Parameters.AddWithValue("@txtpincode", _Pincode);
                cmd.Parameters.AddWithValue("@docName", liveDoc);

                cmd.ExecuteNonQuery();

                con.Close();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }





        //get Appointdetails
        public int APPOINT_ID { get; set; }

        public string PATIENT_NAME { get; set; }

        public string SCHEDULE_TIME { get; set; }

        public string SCHEDULE_DATE { get; set; }

        public string BOOKED_DATE { get; set; }


        public string APPOINT_STATUS { get; set; }

        public string MEDICINE { get; set; }



        public string APPOINT_DESC { get; set; }


        //get appointHistory

        public string PRESCRIPTION { get; set; }

        public string CANCEL_REASON { get; set; }



        public DataSet getappointHistory(string _docUserName)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbconnect"].ConnectionString);
            string getData = "coc_getDocapphistory";

            SqlCommand cmd = new SqlCommand(getData, con);
            con.Open();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@docName", _docUserName);

            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();

            adp.Fill(ds);


            return ds;

        }


        [DataType(DataType.Password)]
        public string Doc_CurrentPassword { get; set; }

        [DataType(DataType.Password)]
        public string Doc_NewPassword { get; set; }

        [DataType(DataType.Password)]
        public string Doc_RePassword { get; set; }



        public string UpdatingDocPassword(string _curpassword, string _newPassword, string _admin)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbconnect"].ConnectionString);

                string updateQuery = "coc_updateDoctorPassword";

                SqlCommand cmd = new SqlCommand(updateQuery, con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@txtcurPass", _curpassword);
                cmd.Parameters.AddWithValue("@txtnewPass", _newPassword);
                cmd.Parameters.AddWithValue("@txtDoctor", _admin);
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



        [Required(ErrorMessage = "*")]
        [DataType(DataType.Text)]
        [Display(Name = "DoctorName")]
        public string DoctorName { get; set; }
        public string Post { get; set; }

    /*
        public string SharepostP(string _DoctorName, string _Post, string _liveDoc)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbconnect"].ConnectionString);
                string updateQuery = "coc_Doctorpost";
                SqlCommand cmd = new SqlCommand(updateQuery, con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                cmd.Parameters.AddWithValue("@txtdoctorname", _DoctorName);
                cmd.Parameters.AddWithValue("@txtPost", _Post);
                cmd.Parameters.AddWithValue("@txtDoctor", _liveDoc);
                cmd.Parameters.Add("@txtstatus", SqlDbType.VarChar, 30);
                cmd.Parameters["@txtstatus"].Direction = ParameterDirection.Output;


                cmd.ExecuteNonQuery();

                con.Close();

                return cmd.Parameters["@txtstatus"].Value.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        */
    }

}
