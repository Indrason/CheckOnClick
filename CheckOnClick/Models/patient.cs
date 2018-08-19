using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CheckOnClick.Models
{
    public class Patient
    {
        public int PATIENT_ID { get; set; }
        public string PATIENT_NAME { get; set; }
        public string PATIENT_USERNAME { get; set; }
        public string PATIENT_EMAIL { get; set; }
        public string PATIENT_GENDER { get; set; }
        public string PATIENT_CONTACT { get; set; }
        public string PATIENT_DOB { get; set; }
        public DateTime PATIENT_DOB_IN { get; set; }
        public string PATIENT_BLOODGP { get; set; }
        public string PATIENT_CITY { get; set; }
        public string PATIENT_STATE { get; set; }
        public string PATIENT_COUNTRY { get; set; }
        public int PATIENT_PIN { get; set; }
        public string PATIENT_ACNT_DATE { get; set; }
        public string ACTIVE { get; set; }

        public DataSet getPatient()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbconnect"].ConnectionString);
            string getData = "coc_showPatient";

            SqlCommand cmd = new SqlCommand(getData, con);
            con.Open();
            cmd.CommandType = CommandType.StoredProcedure;

            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            adp.Fill(ds);

            return ds;
        }

        // adding new patient through admin panel

        public string createPatient(string patient_name, string patient_username, string patient_email, string patient_gender, string patient_contact, DateTime patient_dob, string patient_bloodgp, string patient_city, string patient_state, string patient_country, int patient_pin)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbconnect"].ConnectionString);
                string addDocQuery = "coc_addPatient";

                SqlCommand cmd = new SqlCommand(addDocQuery, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@txtpatientname", patient_name);
                cmd.Parameters.AddWithValue("@txtpatientusername", patient_username);
                cmd.Parameters.AddWithValue("@txtpatientemail", patient_email);
                cmd.Parameters.AddWithValue("@txtpatientgender", patient_gender);
                cmd.Parameters.AddWithValue("@txtpatientcontact", patient_contact);
                cmd.Parameters.AddWithValue("@txtpatientdob", patient_dob);
                cmd.Parameters.AddWithValue("@txtpatientbloodgp", patient_bloodgp);
                cmd.Parameters.AddWithValue("@txtpatientcity", patient_city);
                cmd.Parameters.AddWithValue("@txtpatientstate", patient_state);
                cmd.Parameters.AddWithValue("@txtpatientcountry", patient_country);
                cmd.Parameters.AddWithValue("@txtpatientpin", patient_pin);

                cmd.Parameters.Add("@txtresult", SqlDbType.VarChar, 100);
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
    }
}