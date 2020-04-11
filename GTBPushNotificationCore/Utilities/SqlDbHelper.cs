using GTBPushNotificationCore.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTBPushNotificationCore.Utilities
{
    public static class SqlDbHelper
    {
        public static SqlResModel SelectWithParam(string ConnString, string CommandName, CommandType cmdType, SqlParameter[] param = null)
        {
            string classMeth = "SelectWithParam ";
            DataTable ds = new DataTable();
            var res = new SqlResModel();

            using (SqlConnection con = new SqlConnection(ConnString))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    try
                    {
                        cmd.CommandType = cmdType;
                        cmd.CommandText = CommandName;
                        cmd.Parameters.Clear();
                        if (param != null)
                        {
                            cmd.Parameters.AddRange(param);
                        }
                        
                        if (con.State != ConnectionState.Open)
                        {
                            con.Open();
                        }

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(ds);
                            res.ResponseCode = "00";
                        }

                    }
                    catch (Exception ex)
                    {
                        ErrHandler.Log(classMeth + "Error occured while executing " + CommandName + " " + ex.Message + " " + ex.StackTrace);
                        res.ResponseCode = "99";
                    }

                    cmd.Parameters.Clear();
                }
            }
            res.ResultDataTable = ds;
            return res;
        }

        public static SqlResModel InsertWithParam(string ConnString, string CommandName, CommandType commandType, SqlParameter[] param = null)
        {
            string classMeth = "InsertWithParam";
            int result = 0;
            var res = new SqlResModel();

            using (SqlConnection con = new SqlConnection(ConnString))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    try
                    {
                        cmd.CommandType = commandType;
                        cmd.CommandText = CommandName;
                        if (param != null)
                        {
                            cmd.Parameters.AddRange(param);
                        }

                        if (con.State != ConnectionState.Open)
                        {
                            con.Open();
                        }

                        result = cmd.ExecuteNonQuery();
                        res.ResponseCode = "00";
                    }
                    catch (Exception ex)
                    {
                        ErrHandler.Log(classMeth + "Error occured while executing " + CommandName + " " + ex.Message + " " + ex.StackTrace);
                        result = 0;
                        res.ResponseCode = "99";
                    }
                }
            }

            res.ResultInt = result;
            return res;
        }
        public static SqlResModel UpdateWithParam(string ConnString, string CommandName, CommandType commandType, SqlParameter[] param)
        {
            string classMeth = "UpdateWithParam";
            int result = 0;
            var res = new SqlResModel();

            using (SqlConnection con = new SqlConnection(ConnString))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    try
                    {
                        cmd.CommandType = commandType;
                        cmd.CommandText = CommandName;
                        cmd.Parameters.AddRange(param);

                        if (con.State != ConnectionState.Open)
                        {
                            con.Open();
                        }

                        result = cmd.ExecuteNonQuery();
                        ErrHandler.Log("The response from check Session is " + result);
                        result = Math.Abs(result);
                        ErrHandler.Log("result - " + result);
                        res.ResponseCode = "00";
                    }
                    catch (Exception ex)
                    {
                        ErrHandler.Log(classMeth + "Error occured while executing " + CommandName + " " + ex.Message + " " + ex.StackTrace);
                        result = 0;
                        res.ResponseCode = "99";
                    
                    }
                }
            }

            res.ResultInt = result;
            return res;
        }

    }
}
