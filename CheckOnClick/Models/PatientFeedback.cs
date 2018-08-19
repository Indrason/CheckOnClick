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
    public class PatientFeedback
    {
        [Required(ErrorMessage = "*")]
        [DataType(DataType.Text)]
        [Display(Name = "DoctorName")]
        public string DoctorName { get; set; }
        public string Feedback { get; set; }
        public decimal Rating { get; set; }
        public int DoctorId { get; set; }


        public DataSet GetDoctorInFeedback(string LivePatient)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbconnect"].ConnectionString);
                string getDoc = "coc_getDoctorsInFeedbackPatient";
                SqlCommand cmd = new SqlCommand(getDoc, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PName", LivePatient);
                con.Open();
                DataSet ds = new DataSet();
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(ds);

                con.Close();
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public string FeedbackP(int _DoctorId, string _Feedback, decimal _Rating, string _livePatient)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbconnect"].ConnectionString);
                string updateQuery = "coc_FeedbackPatient";
                SqlCommand cmd = new SqlCommand(updateQuery, con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                cmd.Parameters.AddWithValue("@txtdoctorid", _DoctorId);
                cmd.Parameters.AddWithValue("@txtfeedback", _Feedback);
                cmd.Parameters.AddWithValue("@txtrating", _Rating);
                cmd.Parameters.AddWithValue("@txtPatient", _livePatient);
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
    }

}