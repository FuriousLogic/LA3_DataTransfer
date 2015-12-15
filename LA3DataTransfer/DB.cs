using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.Configuration;

namespace LA3DataTransfer
{
    class StagingDB
    {
        public static int ClearTables()
        {
            //Generated Function, don't alter
            int returnValue = -1;
            string dbConnection = ConfigurationManager.ConnectionStrings["LA3_Staging"].ToString();
            try
            {
                using (SqlConnection conn = new SqlConnection(dbConnection))
                {
                    SqlCommand comm = new SqlCommand("ClearTables", conn);
                    comm.CommandType = CommandType.StoredProcedure;

                    conn.Open();
                    returnValue = comm.ExecuteNonQuery();
                    conn.Close();
                    comm.Dispose();

                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                throw new System.Exception("Error accessing stored procedure 'ClearTables'");
            }
            return returnValue;

        }
        public static DataSet GetDataset(string SQL)
        {
            DataSet rv = new DataSet();
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["LA3_Staging"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand comm = new SqlCommand(SQL, conn);
                    comm.CommandTimeout = 360;
                    comm.CommandType = CommandType.Text;
                    SqlDataAdapter adapt = new SqlDataAdapter(comm);
                    adapt.Fill(rv);
                    adapt.Dispose();
                    comm.Dispose();
                }
            }
            catch (Exception)
            {
            }
            return rv;
        }
        public static int ExecuteSQL(string SQL)
        {
            int rv = 0;
            string dbConnection = ConfigurationManager.ConnectionStrings["LA3_Staging"].ToString();
            try
            {
                using (SqlConnection conn = new SqlConnection(dbConnection))
                {
                    SqlCommand comm = new SqlCommand(SQL, conn);
                    comm.CommandType = CommandType.Text;
                    conn.Open();
                    rv = comm.ExecuteNonQuery();
                    conn.Close();
                    comm.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new System.Exception("Error accessing stored procedure 'dcClearData'");
            }
            return rv;
        }
    }
}
