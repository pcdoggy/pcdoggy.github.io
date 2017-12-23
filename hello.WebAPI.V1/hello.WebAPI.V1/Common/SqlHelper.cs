using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace hello.WebAPI.V1.Common
{
    public class SqlHelper
    {
        /// <summary>
        /// 读取config连接设置生成sql连接
        /// </summary>
        /// <returns></returns>
        static public SqlConnection GetConnection()
        {
            SqlConnection sqlcon = null;
            try
            {
                string connectstring = System.Configuration.ConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString;
                sqlcon = new System.Data.SqlClient.SqlConnection(connectstring);
            }
            catch
            {
                sqlcon = null;
            }
            return sqlcon;
        }

        /*一般sql语句执行*/
        /// <summary>
        /// 执行查询语句并返回结果集
        /// </summary>
        /// <param name="sqlcon">连接</param>
        /// <param name="CommandText">SQL语句文本</param>
        /// <param name="ResultDataTableName">要返回的结果集名称</param>
        /// <returns>结果数据集</returns>
        static public DataSet SqlQuery(SqlConnection sqlcon, string CommandText)
        {
            DataSet datas = new DataSet();
            using (SqlCommand sqlcom = new SqlCommand())
            {
                try
                {
                    sqlcom.Connection = sqlcon;
                    sqlcom.CommandText = CommandText;
                    sqlcom.CommandTimeout = 300;
                    if (sqlcon.State != ConnectionState.Open)
                    {
                        sqlcon.Open();
                    }
                    SqlDataAdapter sqlad = new SqlDataAdapter(sqlcom);
                    sqlad.Fill(datas);
                }
                catch (Exception ex)
                {
                    datas = null;
                    //调用日志类的写日志的方法，将错误信息传入  
                    Log.WriteLog(ex.Message);
                }
            }
            return datas;
        }

        /// <summary>
        /// 执行查询语句并返回结果集
        /// </summary>
        /// <param name="sqlcon">连接</param>
        /// <param name="CommandText">SQL语句文本</param>
        /// <param name="ResultDataTableName">要返回的结果集名称</param>
        /// <param name="sqltran">事务</param>
        /// <returns>结果数据集</returns>
        static public DataSet SqlQuery(SqlConnection sqlcon, string CommandText, SqlTransaction sqltran)
        {
            DataSet datas = new DataSet();
            using (SqlCommand sqlcom = new SqlCommand())
            {
                try
                {
                    sqlcom.Connection = sqlcon;
                    sqlcom.CommandText = CommandText;
                    sqlcom.CommandTimeout = 300;
                    sqlcom.Transaction = sqltran;
                    if (sqlcon.State != ConnectionState.Open)
                    {
                        sqlcon.Open();
                    }
                    SqlDataAdapter sqlad = new SqlDataAdapter(sqlcom);
                    sqlad.Fill(datas);
                }
                catch (Exception ex)
                {
                    datas = null;
                    //调用日志类的写日志的方法，将错误信息传入  
                    Log.WriteLog(ex.Message);
                }
            }
            return datas;
        }

        /// <summary>
        /// 执行无返回结果集的语句
        /// </summary>
        /// <param name="sqlcon">连接</param>
        /// <param name="CommandText">SQL语句文本</param>
        /// <returns>语句影响的行数</returns>
        static public int SqlNonQuery(SqlConnection sqlcon, string CommandText)
        {
            int rowcount = -99;//语句影响的行数
            using (SqlCommand sqlcom = new SqlCommand())
            {
                try
                {
                    sqlcom.Connection = sqlcon;
                    sqlcom.CommandText = CommandText;
                    sqlcom.CommandTimeout = 300;
                    if (sqlcon.State != ConnectionState.Open)
                    {
                        sqlcon.Open();
                    }
                    rowcount = sqlcom.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    rowcount = -99;
                    //调用日志类的写日志的方法，将错误信息传入  
                    Log.WriteLog(ex.Message);
                }
            }
            return rowcount;
        }

        /// <summary>
        /// 执行无返回结果集的语句
        /// </summary>
        /// <param name="sqlcon">连接</param>
        /// <param name="CommandText">SQL语句文本</param>
        /// <param name="sqltran">事务</param>
        /// <returns>语句影响的行数</returns>
        /// 
        static public int SqlNonQuery(SqlConnection sqlcon, string CommandText, SqlTransaction sqltran)
        {
            int rowcount = -99;//语句影响的行数
            using (SqlCommand sqlcom = new SqlCommand())
            {
                try
                {
                    sqlcom.Connection = sqlcon;
                    sqlcom.CommandText = CommandText;
                    sqlcom.CommandTimeout = 300;
                    sqlcom.Transaction = sqltran;
                    if (sqlcon.State != ConnectionState.Open)
                    {
                        sqlcon.Open();
                    }
                    rowcount = sqlcom.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    rowcount = -99;
                    //调用日志类的写日志的方法，将错误信息传入  
                    Log.WriteLog(ex.Message);
                }
            }
            return rowcount;
        }

        /*存储过程调用*/
        /// <summary>
        /// 传入参数值执行存储过程并返回结果集
        /// </summary>
        /// <param name="sqlcon">连接</param>
        /// <param name="ProcedureName">存储过程名称</param>
        /// <param name="ParameterValue">一个键值对集合,键为参数名,值为该参数的值</param>
        /// <returns>结果数据集</returns>
        static public DataSet SqlQuery(SqlConnection sqlcon, string ProcedureName, Dictionary<string, object> ParameterValue)
        {
            DataSet datas = new DataSet();
            using (SqlCommand sqlcom = new SqlCommand())
            {
                try
                {
                    sqlcom.Connection = sqlcon;
                    sqlcom.CommandText = ProcedureName;
                    sqlcom.CommandTimeout = 300;
                    sqlcom.CommandType = CommandType.StoredProcedure;
                    foreach (KeyValuePair<string, object> par in ParameterValue)
                    {
                        SqlParameter sqlpar;
                        if (par.Value != null)
                        {
                            sqlpar = new SqlParameter(par.Key, par.Value);
                        }
                        else
                        {
                            sqlpar = new SqlParameter(par.Key, DBNull.Value);
                        }
                        sqlcom.Parameters.Add(sqlpar);
                    }

                    if (sqlcon.State != ConnectionState.Open)
                    {
                        sqlcon.Open();
                    }
                    SqlDataAdapter sqlad = new SqlDataAdapter(sqlcom);
                    sqlad.Fill(datas);
                }
                catch (Exception ex)
                {
                    datas = null;
                    //调用日志类的写日志的方法，将错误信息传入  
                    Log.WriteLog(ex.Message);
                }
            }
            return datas;
        }

        /// <summary>
        /// 传入参数值执行存储过程并返回结果集
        /// </summary>
        /// <param name="sqlcon">连接</param>
        /// <param name="ProcedureName">存储过程名称</param>
        /// <param name="ParameterValue">一个键值对集合,键为参数名,值为该参数的值</param>
        /// <param name="sqltran">事务</param>
        /// <returns>结果数据集</returns>
        static public DataSet SqlQuery(SqlConnection sqlcon, string ProcedureName, Dictionary<string, object> ParameterValue, SqlTransaction sqltran)
        {
            DataSet datas = new DataSet();
            using (SqlCommand sqlcom = new SqlCommand())
            {
                try
                {
                    sqlcom.Connection = sqlcon;
                    sqlcom.CommandText = ProcedureName;
                    sqlcom.CommandTimeout = 300;
                    sqlcom.Transaction = sqltran;
                    sqlcom.CommandType = CommandType.StoredProcedure;
                    foreach (KeyValuePair<string, object> par in ParameterValue)
                    {
                        SqlParameter sqlpar;
                        if (par.Value != null)
                        {
                            sqlpar = new SqlParameter(par.Key, par.Value);
                        }
                        else
                        {
                            sqlpar = new SqlParameter(par.Key, DBNull.Value);
                        }
                        sqlcom.Parameters.Add(sqlpar);
                    }

                    if (sqlcon.State != ConnectionState.Open)
                    {
                        sqlcon.Open();
                    }
                    SqlDataAdapter sqlad = new SqlDataAdapter(sqlcom);
                    sqlad.Fill(datas);
                }
                catch (Exception ex)
                {
                    datas = null;
                    //调用日志类的写日志的方法，将错误信息传入  
                    Log.WriteLog(ex.Message);
                }
            }
            return datas;
        }

        /// <summary>
        /// 执行无返回结果集的存储过程
        /// </summary>
        /// <param name="sqlcon">连接</param>
        /// <param name="ProcedureName">存储过程名称</param>
        /// <param name="ParameterValue">一个键值对集合,键为参数名,值为该参数的值</param>
        /// <returns>语句影响的行数</returns>
        static public int SqlNonQuery(SqlConnection sqlcon, string ProcedureName, Dictionary<string, object> ParameterValue)
        {
            int rowcount = -99;//语句影响的行数
            using (SqlCommand sqlcom = new SqlCommand())
            {
                try
                {
                    sqlcom.Connection = sqlcon;
                    sqlcom.CommandText = ProcedureName;
                    sqlcom.CommandTimeout = 300;
                    sqlcom.CommandType = CommandType.StoredProcedure;
                    foreach (KeyValuePair<string, object> par in ParameterValue)
                    {
                        SqlParameter sqlpar;
                        if (par.Value != null)
                        {
                            sqlpar = new SqlParameter(par.Key, par.Value);
                        }
                        else
                        {
                            sqlpar = new SqlParameter(par.Key, DBNull.Value);
                        }
                        sqlcom.Parameters.Add(sqlpar);
                    }

                    if (sqlcon.State != ConnectionState.Open)
                    {
                        sqlcon.Open();
                    }
                    rowcount = sqlcom.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    rowcount = -99;
                    //调用日志类的写日志的方法，将错误信息传入  
                    Log.WriteLog(ex.Message);
                }
            }

            return rowcount;
        }

        /// <summary>
        /// 执行无返回结果集的存储过程
        /// </summary>
        /// <param name="sqlcon">连接</param>
        /// <param name="ProcedureName">存储过程名称</param>
        /// <param name="ParameterValue">一个键值对集合,键为参数名,值为该参数的值</param>
        /// <param name="sqltran">事务</param>
        /// <returns>语句影响的行数</returns>
        static public int SqlNonQuery(SqlConnection sqlcon, string ProcedureName, Dictionary<string, object> ParameterValue, SqlTransaction sqltran)
        {
            int rowcount = -99;//语句影响的行数
            using (SqlCommand sqlcom = new SqlCommand())
            {
                try
                {
                    sqlcom.Connection = sqlcon;
                    sqlcom.CommandText = ProcedureName;
                    sqlcom.CommandTimeout = 300;
                    sqlcom.Transaction = sqltran;
                    sqlcom.CommandType = CommandType.StoredProcedure;
                    foreach (KeyValuePair<string, object> par in ParameterValue)
                    {
                        SqlParameter sqlpar;
                        if (par.Value != null)
                        {
                            sqlpar = new SqlParameter(par.Key, par.Value);
                        }
                        else
                        {
                            sqlpar = new SqlParameter(par.Key, DBNull.Value);
                        }
                        sqlcom.Parameters.Add(sqlpar);
                    }

                    if (sqlcon.State != ConnectionState.Open)
                    {
                        sqlcon.Open();
                    }
                    rowcount = sqlcom.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    rowcount = -99;
                    //调用日志类的写日志的方法，将错误信息传入  
                    Log.WriteLog(ex.Message);
                }
            }

            return rowcount;
        }

        static public int SqlRollback(SqlTransaction sqltran)
        {
            int resault = 1;
            try
            {
                sqltran.Rollback();
            }
            catch (Exception ex)
            {
                resault = -99;
                //调用日志类的写日志的方法，将错误信息传入  
                Log.WriteLog(ex.Message);
            }
            return resault;
        }

        static public int SqlCommit(SqlTransaction sqltran)
        {
            int resault = 1;
            try
            {
                sqltran.Commit();
            }
            catch (Exception ex)
            {
                resault = -99;
                //调用日志类的写日志的方法，将错误信息传入  
                Log.WriteLog(ex.Message);
            }
            return resault;
        }
    }
}