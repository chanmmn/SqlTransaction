using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConAppTransaction
{
    class Program
    {
        static void Main(string[] args)
        {
            InsertProduct();
        }

        public static void InsertProduct()
        {
           
           string strConn = "Server=.;Initial Catalog=northwind;Persist Security Info=False;User ID=username; " +
                "Password=password;";
            SqlConnection conn = new SqlConnection(strConn);
            string sqlInsert = "INSERT INTO [dbo].[Products] ([ProductName] ,[SupplierID] ,[CategoryID] ,[Discontinued]) " +
                        "VALUES ('Food Food', 1, 1, 1)";
            string sqlInsert1 = "INSERT INTO [dbo].[Products] ([ProductName] ,[SupplierID] ,[CategoryID] ,[Discontinued]) " +
                        "VALUES ('Drink Drink', 1, 10, 1)";
            conn.Open();
            SqlTransaction sqlTran = conn.BeginTransaction();

            // Enlist a command in the current transaction.
            SqlCommand command = conn.CreateCommand();
            command.Transaction = sqlTran;

            try
            {
                // Execute two separate commands.
                command.CommandText = sqlInsert;
                command.ExecuteNonQuery();
                command.CommandText = sqlInsert1;
                command.ExecuteNonQuery();

                // Commit the transaction.
                sqlTran.Commit();
                Console.WriteLine("Both records were written to database.");
            }
            catch (Exception ex)
            {
                // Handle the exception if the transaction fails to commit.
                Console.WriteLine(ex.Message);

                try
                {
                    // Attempt to roll back the transaction.
                    sqlTran.Rollback();
                }
                catch (Exception exRollback)
                {
                    // Throws an InvalidOperationException if the connection
                    // is closed or the transaction has already been rolled
                    // back on the server.
                    Console.WriteLine(exRollback.Message);
                }
            }
            conn.Close();
        }
    }
}
