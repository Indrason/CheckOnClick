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
    public class LoginSignup
    {
        [Required(ErrorMessage = "Please enter your username")]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please enter your username")]
        [Display(Name = "UserName")]
        public string UserNameForget { get; set; }

        [Required(ErrorMessage = "Please enter your password")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Text)]
        public string FullName { get; set; }

        [Display(Name = "UserName")]
        [StringLength(5, ErrorMessage = "Long Uname")]
        public string PaUserName { get; set; }

        [Required(ErrorMessage = "*")]
        [DataType(DataType.Password)]
        [Display(Name = "Password1")]
        public string Password1 { get; set; }

        [Required(ErrorMessage = "*")]
        [DataType(DataType.Password)]
        [Display(Name = "Password2")]
        public string Password2 { get; set; }


        [Display(Name = "Gender")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "*")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "*")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email Address")]
        public string Email1 { get; set; }

        [Required(ErrorMessage = "*")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Contact Number")]
        public string Contact { get; set; }

        [DataType(DataType.Date)]
        public DateTime DOB { get; set; }

        [DataType(DataType.Text)]
        public string BloodGroup { get; set; }

        [DataType(DataType.Text)]
        public string City { get; set; }

        [DataType(DataType.Text)]
        public string State { get; set; }

        [DataType(DataType.Text)]
        public string Country { get; set; }

        [DataType(DataType.PostalCode)]
        public int Pin { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string PaPassword { get; set; }

        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }


        // Logging into the system
        public string CheckLogin(string _userName, string _password)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbconnect"].ConnectionString);

                string query = "coc_Login";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@txtusername", _userName);
                cmd.Parameters.AddWithValue("@txtpassword", _password);

                cmd.Parameters.Add("@txtrole", SqlDbType.VarChar, 20);
                cmd.Parameters["@txtrole"].Direction = ParameterDirection.Output;

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                return cmd.Parameters["@txtRole"].Value.ToString();


               
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Creating new account for the patient
        public string CreatingNewAccount(string _fullname, string _email, string _contact, string _gender, string _password)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbconnect"].ConnectionString);
                string signupQuery = "coc_createAccount";

                SqlCommand cmd = new SqlCommand(signupQuery, con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@txtfullname", _fullname);
                cmd.Parameters.AddWithValue("@txtusername", _contact);
                cmd.Parameters.AddWithValue("@txtemail", _email);
                cmd.Parameters.AddWithValue("@txtgender", _gender);
                cmd.Parameters.AddWithValue("@txtcontact", _contact);
                cmd.Parameters.AddWithValue("@txtpassword", _password);

                cmd.Parameters.Add("@txtStatus", SqlDbType.VarChar, 50);
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