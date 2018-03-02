using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;

namespace CheckOnClick.Models
{
    public class Roles
    {
        [DataType(DataType.Text)]
        public int Role_ID { get; set; }

        [DataType(DataType.Text)]
        public string Role_Name { get; set; }

        [DataType(DataType.Text)]
        public string Active { get; set; }

        [DataType(DataType.Text)]
        public string NewRole_Name { get; set; }

        // Getting Roles and display in the Admin Role Page
        public DataSet getRoles()
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbconnect"].ConnectionString);
                string getData = "coc_showRole";

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

        // Creating new Roles by the Admin
        public string createRole(string _newRole)
        {
            try
            {

                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbconnect"].ConnectionString);
                string addRoleQuery = "coc_addRole";

                SqlCommand cmd = new SqlCommand(addRoleQuery, con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@txtRoleName", _newRole);

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