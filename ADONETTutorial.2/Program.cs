using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace ADONETTutorial
{
    class Program
    {
        static void Main(string[] args)
        {
            var sqlConnStringBuilder = new SqlConnectionStringBuilder
            {
                ApplicationName = "Sql Example",
                DataSource = "localhost",
                UserID = "sa",
                Password = "Pa$$w0rd1",
                InitialCatalog = "SqlExamples"
            };
            var sqlConn = new SqlConnection(sqlConnStringBuilder.ConnectionString);

            var sqlCmd = new SqlCommand
            {
                Connection = sqlConn,
                CommandType = CommandType.Text,
                CommandText = "select * from Table_1 where id = @id"
            };

            sqlConn.Open();

            SqlParameter idParam = new SqlParameter("@Id", SqlDbType.Int);
            idParam.Value = 1;

            sqlCmd.Parameters.Add(idParam);

            SqlDataReader sqlRdr = sqlCmd.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dataTable = sqlRdr.GetSchemaTable();

            var listOfArray = new List<Array>();

            while (sqlRdr.Read())
            {
                Object[] values = new Object[sqlRdr.FieldCount];
                int fieldCount = sqlRdr.GetValues(values);

                listOfArray.Add(values);

                int v = sqlRdr.GetInt32(0);
                string v1 = sqlRdr.GetString(1);
                int v2 = sqlRdr.GetOrdinal("Id");
                
                Console.WriteLine($"{sqlRdr[0]}, {sqlRdr[1]}");
            }

            sqlRdr.Close();

        }

        private static void GetStoredProcedureParamaters()
        {
            var sqlConnStringBuilder = new SqlConnectionStringBuilder
            {
                ApplicationName = "Sql Example",
                DataSource = "localhost",
                UserID = "sa",
                Password = "Pa$$w0rd1",
                InitialCatalog = "SqlExamples"
            };
            var sqlConn = new SqlConnection(sqlConnStringBuilder.ConnectionString);

            var sqlCmd = new SqlCommand
            {
                Connection = sqlConn,
                CommandType = CommandType.StoredProcedure,
                CommandText = "get_all_Table_1"
            };

            sqlConn.Open();

            SqlCommandBuilder.DeriveParameters(sqlCmd);

            foreach (SqlParameter parameter in sqlCmd.Parameters)
            {
                Console.WriteLine($"Type: {parameter.SqlDbType}, TypeName: {parameter.ParameterName}, Direction: {parameter.Direction}");
            }
        }

        private static SqlDataReader SqlCommandObjectExample()
        {
            var sqlConnStringBuilder = new SqlConnectionStringBuilder
            {
                ApplicationName = "Sql Example",
                DataSource = "localhost",
                UserID = "sa",
                Password = "Pa$$w0rd1",
                InitialCatalog = "SqlExamples"
            };
            var sqlConn = new SqlConnection(sqlConnStringBuilder.ConnectionString);
            var sqlCmd = new SqlCommand
            {
                Connection = sqlConn,
                CommandType = CommandType.StoredProcedure,
                CommandText = "get_all_Table_1"
            };

            sqlConn.InfoMessage += SqlConn_InfoMessage;
            sqlCmd.StatementCompleted += SqlCmd_StatementCompleted;

            sqlConn.Open();

            SqlParameter idParam = new SqlParameter("@Id", SqlDbType.Int);
            idParam.Value = 1;
            idParam.Direction = ParameterDirection.Input;

            SqlParameter countParam = new SqlParameter("@count", SqlDbType.Int);
            countParam.Direction = ParameterDirection.Output;

            SqlParameter returnParam = new SqlParameter("@return_value", SqlDbType.Int);
            returnParam.Direction = ParameterDirection.ReturnValue;

            sqlCmd.Parameters.Add(idParam);
            sqlCmd.Parameters.Add(countParam);
            sqlCmd.Parameters.Add(returnParam);

            SqlDataReader sqlRdr = sqlCmd.ExecuteReader(CommandBehavior.CloseConnection);

            //while (sqlRdr.Read())
            //{
            //    Console.WriteLine($"{sqlRdr[0]}, {sqlRdr[1]}");
            //}

            //sqlRdr.Close();

            //Console.WriteLine($"Return: {returnParam.Value}, output: {countParam.Value}");

            return sqlRdr;
        }

        private static void SqlConn_InfoMessage(object sender, SqlInfoMessageEventArgs e)
        {
            Console.WriteLine($"SqlConn_InfoMessage: {e.Message}");
        }

        private static void SqlCmd_StatementCompleted(object sender, StatementCompletedEventArgs e)
        {
            Console.WriteLine($"SqlCmd_StatementCompleted: {e.RecordCount}");
        }
    }
}
