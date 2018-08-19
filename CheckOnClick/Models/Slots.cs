using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CheckOnClick.Models
{
    public class Slots
    {
        public int Slot_Id { get; set; }

        public int Doctor_Id { get; set; }

        public string Doctor_Name { get; set; }

        public string Slot_From { get; set; }

        public string Slot_To { get; set; }

        public string Active { get; set; }

        public string AppointDate { get; set; }
        public string AppointTime { get; set; }


        // Getting all the slot list in Admin Portal
        public DataSet GettingSlotList()
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbconnect"].ConnectionString);
                string getSlot = "coc_showSlots";

                SqlCommand cmd = new SqlCommand(getSlot, con);
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

        public string SavingSlotsForDoc(int _doc_id, string _slot_from, string _slot_to)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbconnect"].ConnectionString);
                string saveSlotQuery = "coc_addSlots";
                SqlCommand cmd = new SqlCommand(saveSlotQuery, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@txtDocId", _doc_id);
                cmd.Parameters.AddWithValue("@txtSlotFrom", _slot_from);
                cmd.Parameters.AddWithValue("@txtSlotTo", _slot_to);
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

        public DataSet LoadSlotsForAppoint(int _doc_id)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbconnect"].ConnectionString);
                string loadSlotQuery = "coc_getSlotInAppoint";
                SqlCommand cmd = new SqlCommand(loadSlotQuery, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@txtDocId", _doc_id);
                con.Open();
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
    }
}