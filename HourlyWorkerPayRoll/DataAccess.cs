// DataAccess.cs
//         Title: DataAccess - Data Access Layer for Piecework Payroll
// Last Modified: October 26th 2021
//    Written By: Katherine Bellman
// Based on code samples provided by Kyle Chapman
// 
// This is a module with a set of classes allowing for interaction between
// Piecework Worker data objects and a database.

using System;
using System.Configuration;
// See this StackOverflow answer: https://stackoverflow.com/a/54472192
using System.Data;
using System.Data.SqlClient;

namespace HourlyWorkerPayRoll
{
	internal class DataAccess
	{

		#region "Connection String"

		/// <summary>
		/// Return connection string
		/// </summary>
		/// <returns>Connection string</returns>
		private static string GetConnectionString()
		{
			/* Somehow, we need to have a working connection string. 
             * This is best done by adding an App.config file to your project by
             * using Add -> New Item... -> Application Configuration file.
             * For further details, refer to the Week 5 slides and/or the
             * class recording on the subject of connection strings (Week 6/7),
             * as well as https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/connection-strings-and-configuration-files .
             * Other options may be viable. */

			//return "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=" +
			//	   "C:\\Users\\Katherine Bellman\\Source\\Repos\\HourlyWorkerPayroll\\HourlyWorkerPayroll" +
			//	   "\\WorkerDatabase.mdf;Integrated Security=True;Connect Timeout=30";

			// The following is better - no absolute/fixed path - but it won't actually work while we're debugging. You can try a variant on this.
			//return "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=" +
			//    Directory.GetCurrentDirectory() +
			//    "\\WorkerDatabase.mdf;Integrated Security=True;Connect Timeout=30";

			string returnValue = null;

			// Look for myConnectionString in the connectionStrings section.
			ConnectionStringSettings myConnectionString = ConfigurationManager.ConnectionStrings[1];

			//// If found, return the connection string.
			if (myConnectionString != null)
				returnValue = myConnectionString.ConnectionString;

			return returnValue;
		}

		#endregion

		#region "Methods"

		/// <summary>
		/// Function that returns all workers in the database as a DataTable for display
		/// </summary>
		/// <returns>a DataTable containing all workers in the database</returns>
		internal static DataTable GetEmployeeList()
		{
			// Declare the SQL connection, SQL command, and SQL adapter
			SqlConnection dbConnection = new SqlConnection(GetConnectionString());
			SqlCommand command = new SqlCommand("SELECT * FROM [Entries]", dbConnection);
			SqlDataAdapter adapter = new SqlDataAdapter(command);

			// Declare a DataTable object that will hold the return value
			DataTable employeeTable = new DataTable();

			// Try to connect to the database, and use the adapter to fill the table
			try
			{
				dbConnection.Open();
				adapter.Fill(employeeTable);
			}
			catch (Exception ex)
			{
				// If there is an error, re-throw the exception to be handled by the presentation tier.
				// (You could also just do error messaging here but that's not as nice.)
				throw ex;
			}
			finally
			{
				adapter.Dispose();
				dbConnection.Close();
			}

			// Return the populated DataTable
			return employeeTable;
		}

		/// <summary>
		/// Function to add a new worker to the worker database
		/// </summary>
		/// <param name="insertWorker">a worker object to be inserted</param>
		/// <returns>true if successful</returns>
		internal static bool InsertNewRecord(HourlyWorkerPay insertWorker)
		{
			// Create return value
			bool returnValue = false;

			// Declare the SQL connection
			SqlConnection dbConnection = new SqlConnection(GetConnectionString());

			// Create new SQL command and assign it paramaters
			SqlCommand command = new SqlCommand("INSERT INTO Entries VALUES(@firstName, @lastName, @messages, @pay, @entryDate)", dbConnection);

			// TO DO The next two lines assume workers only have 1 name. Read your requirements carefully!
			//TODO: string of name will need to be split for entry into SQL
			command.Parameters.AddWithValue("@firstName", insertWorker.FirstName);
			command.Parameters.AddWithValue("@lastName", insertWorker.LastName);
			command.Parameters.AddWithValue("@messages", insertWorker.Messages);
			command.Parameters.AddWithValue("@pay", insertWorker.Pay);
			// TO DO This line assumes the PieceworkWorker class has no Date property. Careful!
			command.Parameters.AddWithValue("@entryDate", DateTime.Now);

			// The above SQL command is the same as the following:
			// SqlCommand command = new SqlCommand("INSERT INTO tblEntries VALUES(" + insertWorker.Name + ", " + insertWorker.Name + ", " + insertWorker.Messages + ", " + insertWorker.Pay + ", " + DateTime.Now + ")", dbConnection);
			// Your choice if you think this version is nicer!

			// Try to insert the new record, return result
			try
			{
				dbConnection.Open();
				returnValue = (command.ExecuteNonQuery() == 1);
			}
			catch (Exception ex)
			{
				// If there is an error, re-throw the exception to be handled by the presentation tier.
				// (You could also just do error messaging here but that's not as nice.)
				throw ex;
			}
			finally
			{
				dbConnection.Close();
			}

			// Return the true if this worked, false if it failed
			return returnValue;
		}

		/// <summary>
		/// Returns a total number of employees from the database.
		/// </summary>
		/// <returns>total employees, as a string</returns>
		internal static string GetTotalEmployees()
		{
			// Declare the SQL connection and the SQL command
			SqlConnection dbConnection = new SqlConnection(GetConnectionString());
			SqlCommand command = new SqlCommand("SELECT COUNT(EntryId) FROM Entries", dbConnection);

			// Try to open a connection to the database and read the total. Return result.
			try
			{
				dbConnection.Open();
				return command.ExecuteScalar().ToString();
			}
			catch (Exception ex)
			{
				// If there is an error, re-throw the exception to be handled by the presentation tier.
				// (You could also just do error messaging here but that's not as nice.)
				throw ex;
			}
			finally
			{
				dbConnection.Close();
			}
		}

		/// <summary>
		/// Returns a total number of messages from the database.
		/// </summary>
		/// <returns>total messages, as a string</returns>
		internal static string GetTotalMessages()
		{
			// Declare the SQL connection and the SQL command
			SqlConnection dbConnection = new SqlConnection(GetConnectionString());
			SqlCommand command = new SqlCommand("SELECT SUM(Messages) FROM Entries", dbConnection);

			// Try to open a connection to the database and read the total. Return result.
			try
			{
				dbConnection.Open();
				return command.ExecuteScalar().ToString();
			}
			catch (Exception ex)
			{
				// If there is an error, re-throw the exception to be handled by the presentation tier.
				// (You could also just do error messaging here but that's not as nice.)
				throw ex;
			}
			finally
			{
				dbConnection.Close();
			}
		}

		/// <summary>
		/// Returns a total pay among all employees from the database.
		/// </summary>
		/// <returns>total pay, as a string</returns>
		internal static string GetTotalPay()
		{
			// Declare the SQL connection and the SQL command
			SqlConnection dbConnection = new SqlConnection(GetConnectionString());
			SqlCommand command = new SqlCommand("SELECT SUM(Pay) FROM Entries", dbConnection);

			// Try to open a connection to the database and read the total. Return result.
			try
			{
				dbConnection.Open();
				return command.ExecuteScalar().ToString();
			}
			catch (Exception ex)
			{
				// If there is an error, re-throw the exception to be handled by the presentation tier.
				// (You could also just do error messaging here but that's not as nice.)
				throw ex;
			}
			finally
			{
				dbConnection.Close();
			}
		}

		#endregion

	}
}
