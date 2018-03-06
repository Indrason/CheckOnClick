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
    }
}