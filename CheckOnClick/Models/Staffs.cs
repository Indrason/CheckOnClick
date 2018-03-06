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
    public class Staffs
    {
        [DataType(DataType.Text)]
        public int Staff_ID { get; set; }

        [DataType(DataType.Text)]
        public string Staff_Name { get; set; }

        [DataType(DataType.Text)]
        public string Staff_Spec { get; set; }

        [DataType(DataType.Date)]
        public string Staff_DOB { get; set; }

        [DataType(DataType.Text)]
        public string Staff_BloodGroup { get; set; }

        [DataType(DataType.Text)]
        public string Staff_Email { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string Staff_Contact { get; set; }

        [DataType(DataType.Text)]
        public string Staff_City { get; set; }

        [DataType(DataType.Text)]
        public string Staff_State { get; set; }

        [DataType(DataType.Text)]
        public string Staff_Country { get; set; }

        [DataType(DataType.PostalCode)]
        public int Staff_Pin { get; set; }

        [DataType(DataType.DateTime)]
        public string Staff_Acnt_Date { get; set; }

        public string Active { get; set; }

        // Getting Doctors

        public DataSet getDoctor()
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbconnect"].ConnectionString);
                string getData = "coc_showDoctors";

                SqlCommand cmd = new SqlCommand(getData, con);
                con.Open();
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adp.Fill(ds);

                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Adding New Doctor
        public string AddNewDoctor(string _doc_name, string _doc_spec, string _doc_dob, string _doc_bloodgp, string _doc_email, string _doc_contact, string _doc_city, string _doc_state, string _doc_country, int _doc_pin)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbconnect"].ConnectionString);
                string addDocQuery = "coc_AddDoctor";

                SqlCommand cmd = new SqlCommand(addDocQuery, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@txtDocName", _doc_name);
                cmd.Parameters.AddWithValue("@txtDocSpec", _doc_spec);
                cmd.Parameters.AddWithValue("@txtDocDob", _doc_dob);
                cmd.Parameters.AddWithValue("@txtDocBloodGp", _doc_bloodgp);
                cmd.Parameters.AddWithValue("@txtDocEmail", _doc_email);
                cmd.Parameters.AddWithValue("@txtDocContact", _doc_contact);
                cmd.Parameters.AddWithValue("@txtDocCity", _doc_city);
                cmd.Parameters.AddWithValue("@txtDocState", _doc_state);
                cmd.Parameters.AddWithValue("@txtDocCountry", _doc_country);
                cmd.Parameters.AddWithValue("@txtDocPin", _doc_pin);

                cmd.Parameters.Add("@txtresult", SqlDbType.VarChar,100);
                cmd.Parameters["@txtresult"].Direction = ParameterDirection.Output;

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                return cmd.Parameters["@txtresult"].Value.ToString();
                
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetSpecDDown()
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbconnect"].ConnectionString);
                string getData = "coc_showDoctors";

                SqlCommand cmd = new SqlCommand(getData, con);
                con.Open();
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adp.Fill(ds);

                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}