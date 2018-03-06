using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;

namespace CheckOnClick.Models
{
    public class Specialization
    {
        public int SPEC_ID { get; set; }

        public string SPEC_Name { get; set; }

        public string Active { get; set; }

        public string NewSPEC_Name { get; set; }

        public DataSet getspecialization()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbconnect"].ConnectionString);
            string getData = "coc_showSpec";

            SqlCommand cmd = new SqlCommand(getData, con);
            con.Open();
            cmd.CommandType = CommandType.StoredProcedure;

            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            adp.Fill(ds);

            return ds;
        }

        public string createSpec(string _newSpec)
        {

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbconnect"].ConnectionString);
            string addSpecQuery = "coc_addspec";

            SqlCommand cmd = new SqlCommand(addSpecQuery, con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@txtSpecName", _newSpec);

            cmd.Parameters.Add("@txtStatus", SqlDbType.VarChar, 50);
            cmd.Parameters["@txtStatus"].Direction = ParameterDirection.Output;

            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

            return cmd.Parameters["@txtStatus"].Value.ToString();
        }

    }
}
