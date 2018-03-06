using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CheckOnClick.Models
{
    public class Feedback
    {
        public int FDBK_ID { get; set; }

        public int PATIENT_ID { get; set; }

        public string PATIENT_NAME { get; set; }

        public int DOCTOR_ID { get; set; }

        public string DOCTOR_NAME { get; set; }

        public string FBDK_DESC { get; set; }

        public decimal RATING { get; set; }


        public DataSet getFeedback()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbconnect"].ConnectionString);
            string getData = "coc_showFeedback";

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