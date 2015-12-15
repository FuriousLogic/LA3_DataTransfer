using System;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace LA3DataTransfer
{
    public static class CoreFunctions
    {
        public static void ExecuteSQL(string sql)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["LA3Access"].ConnectionString;
                using (var conn = new OleDbConnection(connectionString))
                {
                    conn.Open();
                    var comm = new OleDbCommand(sql, conn) { CommandType = CommandType.Text };
                    comm.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
            }
        }

        public static int SaveRecord(string sql)
        {
            const int rv = 0;
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["LA3Access"].ConnectionString;
                using (var conn = new OleDbConnection(connectionString))
                {
                    conn.Open();
                    var cmGetID = new OleDbCommand("SELECT @@IDENTITY", conn);
                    var comm = new OleDbCommand(sql, conn) { CommandType = CommandType.Text };
                    comm.ExecuteNonQuery();

                    var ds = new DataSet();
                    var adapt = new OleDbDataAdapter(cmGetID);
                    adapt.Fill(ds);
                    adapt.Dispose();
                    cmGetID.Dispose();

                    return int.Parse(ds.Tables[0].Rows[0][0].ToString());

                }
            }
            catch (Exception)
            {
            }
            return rv;
        }

        public static DataSet GetDatasetFromAccess(string sql, string mdbFilePath)
        {
            var rv = new DataSet();

            try
            {
                string connectionString = string.Format(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0}; Persist Security Info=False;", mdbFilePath);
                using (var conn = new OleDbConnection(connectionString))
                {
                    conn.Open();
                    var adapt = new OleDbDataAdapter(sql, conn);
                    adapt.Fill(rv);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return rv;
        }
    }
}
