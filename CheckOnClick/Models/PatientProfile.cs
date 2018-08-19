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
    public class PatientProfile
    {
        [Required(ErrorMessage = "*")]
        [DataType(DataType.Text)]
        [Display(Name = "Patient_Name")]
        public string Patient_Name { get; set; }

        [Required(ErrorMessage = "*")]
        [DataType(DataType.Text)]
        [Display(Name = "Patient_UserName")]
        public string Patient_UserName { get; set; }

        [Required(ErrorMessage = "*")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Patient_Email")]
        public string Patient_Email { get; set; }

        [Required(ErrorMessage = "*")]
        [DataType(DataType.Password)]
        [Display(Name = "PatientPassword")]
        public string Patient_Password { get; set; }

        [Required(ErrorMessage = "*")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Contact Number")]
        public string Patient_Contact { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Patient_DOB")]
        public string Patient_DOB { get; set; }

        [Required(ErrorMessage = "*")]
        [DataType(DataType.Text)]
        [Display(Name = "Patient_BloodGP")]
        public string Patient_BloodGP { get; set; }

        [Required(ErrorMessage = "*")]
        [DataType(DataType.Text)]
        [Display(Name = "Patient_City")]
        public string Patient_City { get; set; }

        [Required(ErrorMessage = "*")]
        [DataType(DataType.Text)]
        [Display(Name = "Patient_State")]
        public string Patient_State { get; set; }


        [Required(ErrorMessage = "*")]
        [DataType(DataType.Text)]
        [Display(Name = "Patient_Country")]
        public string Patient_Country { get; set; }


        [Required(ErrorMessage = "*")]
        [DataType(DataType.PostalCode)]
        [Display(Name = "Patient_Pin")]
        public int Patient_Pin { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Patient_Gender")]
        public string Patient_Gender { get; set; }

        [DataType(DataType.Password)]
        public string Patient_NewPassword { get; set; }

        [DataType(DataType.Password)]
        public string Patient_RePassword { get; set; }

        public string Qname { get; set; }
        public string Qemail { get; set; }
        public string Qcontact { get; set; }

        public DataSet getProfileDetails(string _PatientUserName)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbconnect"].ConnectionString);
            string getData = "coc_getpatientProfile";

            SqlCommand cmd = new SqlCommand(getData, con);
            con.Open();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PName", _PatientUserName);

            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();

            adp.Fill(ds);

            return ds;
        }

        public bool Updating(string _Patient_Name, string _Contact, string _DOB, string _BloodGP, string _City, string _State, string _Country, int _Pincode, string _livePatient)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbconnect"].ConnectionString);
                string updateQuery = "coc_updatePatientProfile";
                SqlCommand cmd = new SqlCommand(updateQuery, con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmd.Parameters.AddWithValue("@txtpatientname", _Patient_Name);
                cmd.Parameters.AddWithValue("@txtcontact", _Contact);
                cmd.Parameters.AddWithValue("@txtDOB", _DOB);
                cmd.Parameters.AddWithValue("@txtBloodGP", _BloodGP);
                cmd.Parameters.AddWithValue("@txtcity", _City);
                cmd.Parameters.AddWithValue("@txtstate", _State);
                cmd.Parameters.AddWithValue("@txtcountry", _Country);
                cmd.Parameters.AddWithValue("@txtpincode", _Pincode);
                cmd.Parameters.AddWithValue("@txtPatient", _livePatient);

                cmd.ExecuteNonQuery();

                con.Close();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //Updating patient password
        public string UpdatingPassword(string _password, string _newPassword, string _patient)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbconnect"].ConnectionString);

                string updateQuery = "coc_updatePatientPassword";

                SqlCommand cmd = new SqlCommand(updateQuery, con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@txtPassword", _password);
                cmd.Parameters.AddWithValue("@txtNewPassword", _newPassword);
                cmd.Parameters.AddWithValue("@txtPatient", _patient);
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


    }
}