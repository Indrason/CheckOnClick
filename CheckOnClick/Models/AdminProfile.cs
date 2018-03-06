using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;

namespace CheckOnClick.Models
{
    public class AdminProfile
    {
        [DataType(DataType.Text)]
        public int Admin_ID { get; set; }

        [DataType(DataType.Text)]
        public string Admin_FullName { get; set; }

        [DataType(DataType.Text)]
        public string Admin_UserName { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Admin_Email { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string Admin_Contact { get; set; }

        [DisplayFormat(DataFormatString ="mm/dd/yyyy")]
        [DataType(DataType.Date)]
        public DateTime Admin_DOB { get; set; }

        [DataType(DataType.Text)]
        public string Admin_City { get; set; }

        [DataType(DataType.Text)]
        public string Admin_State { get; set; }

        [DataType(DataType.Text)]
        public string Admin_Country { get; set; }

        [DataType(DataType.PostalCode)]
        public int Admin_Pin { get; set; }

        [DataType(DataType.Password)]
        public string Admin_Password { get; set; }

        [DataType(DataType.Password)]
        public string Admin_NewPassword { get; set; }

        [DataType(DataType.Password)]
        public string Admin_RePassword { get; set; }

        // Getting Profile details for the specific admin
        public DataSet getProfileDetails(string _adminUserName)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbconnect"].ConnectionString);
                string getData = "coc_getAdminProfile";

                SqlCommand cmd = new SqlCommand(getData, con);
                con.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@adminUName", _adminUserName);

                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adp.Fill(ds);

                con.Close();
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Updating profile details for the specific admin
        public bool Updating(string _fullname, string _contact, string _city, string _state, string _country, int _pin, string _liveAdmin)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbconnect"].ConnectionString);
                string updateQuery = "coc_updateAdminProfile";
                SqlCommand cmd = new SqlCommand(updateQuery, con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmd.Parameters.AddWithValue("@txtfullname", _fullname);
                cmd.Parameters.AddWithValue("@txtcontact", _contact);
                cmd.Parameters.AddWithValue("@txtcity", _city);
                cmd.Parameters.AddWithValue("@txtstate", _state);
                cmd.Parameters.AddWithValue("@txtcountry", _country);
                cmd.Parameters.AddWithValue("@txtpin", _pin);
                cmd.Parameters.AddWithValue("@txtAdmin", _liveAdmin);

                cmd.ExecuteNonQuery();

                con.Close();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Updating Admin Password

        public string UpdatingPassword(string _password, string _newPassword, string _admin)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbconnect"].ConnectionString);

                string updateQuery = "coc_updateAdminPassword";

                SqlCommand cmd = new SqlCommand(updateQuery, con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@txtPassword", _password);
                cmd.Parameters.AddWithValue("@txtNewPassword", _newPassword);
                cmd.Parameters.AddWithValue("@txtAdmin", _admin);
                cmd.Parameters.Add("@txtStatus", SqlDbType.VarChar, 30);
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


        // Getting details of the staffs (Doctor)
       

    }
}