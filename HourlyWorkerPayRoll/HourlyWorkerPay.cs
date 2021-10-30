// HourlyWorkerPay.cs
//         Title: IncInc Payroll (Piecework)
// Last Modified: October 29th 2021
//    Written By: Katherine Bellman
// Adapted from PieceworkWorker by Kyle Chapman, September 2019
// 
// This is a class representing individual worker objects. Each stores
// their own name and number of messages and the class methods allow for
// calculation of the worker's pay and for updating of shared summary
// values. Name and messages are received as strings.
// This is being used as part of a piecework payroll application.


using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace HourlyWorkerPayRoll
{
	class HourlyWorkerPay
	{

		#region "VARIABLE DECLARATIONS"

		// Instance variables
		//private string employeeName;
		private string employeeFName;
		private string employeeLName;
		private int employeeMessages;
		private decimal employeePay;

		private bool isValid = true;

		// Shared class variables
		private static int overallNumberOfEmployees;
		private static int overallMessages;
		private static decimal overallPayroll;
		private static decimal overallAverage;

		// Exceptions - MAIN
		public const string NameParameter = "name";
		public const string MessagesParameter = "messages";


		#endregion

		#region "CONSTRUCTORS"

		/// <summary>
		/// HourlyWorker constructor: accepts a worker's name and number of
		/// messages, sets and calculates values as appropriate.
		/// </summary>
		/// <param name="firstNameValue">the worker's first name</param>
		/// <param name="lastNameValue">the worker's last name</param>
		/// <param name="messageValue">a worker's number of messages sent</param>
		public HourlyWorkerPay(string firstNameValue, string lastNameValue, string messagesValue)
		{
			// Validate and set the worker's name
			this.FirstName = firstNameValue;
			this.LastName = lastNameValue;
			// Validate Validate and set the worker's number of messages
			this.Messages = messagesValue;
			// Calculcate the worker's pay and update all summary values
			FindPay();
		}

		/// <summary>
		/// HourlyWorker constructor: empty constructor used strictly for inheritance and instantiation
		/// </summary>
		public HourlyWorkerPay()
		{

		}

		#endregion

		#region "CLASS METHODS"

		/// <summary>
		/// Currently called in the constructor, the FindPay() method is
		/// used to calculate a worker's pay using threshold values to
		/// change how much a worker is paid per message. This also updates
		/// all summary values.
		/// </summary>
		private void FindPay()
		{
			//Array of message volumes for finding correct pay rate
			double[] messagesSent = { 1249, 2499, 3749, 4999, 15000 };
			double[] messagesPayRate = { 0.02, 0.024, 0.028, 0.034, 0.04 };

			double returnRate = 0.0;
			double sentInput = 0;
			double result;

			try
			{
				//convert messages data type to work with method varibales
				if (!double.TryParse(this.Messages, out sentInput))
				{
					isValid = false;
				}
				//Only run through this block if TryParse was success
				//prevents updating values on failed parse
				else
				{
					//Add messages sent to overall total messages
					overallMessages += employeeMessages;

					//loop through to arrays to find correct range and return the associated rate from second array
					for (int counter = 0; counter < messagesSent.Length; counter++)
					{
						//compare to find coresponding range for messages sent
						if (employeeMessages >= messagesSent[counter])
						{
							//return the rate that is at the same index as coresponding range
							returnRate = messagesPayRate[counter];
						}
					}

					//calculate inputed worker total pay
					result = returnRate * sentInput;
					//convert calculation result  to decimal to pass to employeePay 
					employeePay = Convert.ToDecimal(result);

					//increase the number of Employees by 1
					overallNumberOfEmployees++;

					//Add current worker pay to overall payroll value
					overallPayroll += employeePay;
				}
			}
			catch (ArgumentException)
			{
				throw new ArgumentException("Message sent value must be a positive whole number", MessagesParameter);
			}


		}

		#endregion

		#region "PROPERTY PROCEDURES"

		public string FirstName
		{
			get { return employeeFName; }
			set
			{
				if (value.Trim() == string.Empty)
				{
					//Should name textbox be empty
					throw new ArgumentNullException(NameParameter, "Worker name can not be empty!");
				}

				//checks input for non alphabetic entry
				if (!Regex.IsMatch(value.Trim(), @"^[a-zA-Z]+$"))
				{
					//When no alphabetic characters are found within input field, inform user of error
					throw new ArgumentException("Worker name can only have alphabetical characters.", NameParameter);
				}
			}
		}


		public string LastName
		{
			get { return employeeLName; }
			set
			{
				if (value.Trim() == string.Empty)
				{
					//Should name textbox be empty
					throw new ArgumentNullException(NameParameter, "Worker name can not be empty!");
				}

				//checks input for non alphabetic entry
				if (!Regex.IsMatch(value.Trim(), @"^[a-zA-Z]+$"))
				{
					//When no alphabetic characters are found within input field, inform user of error
					throw new ArgumentException("Worker name can only have alphabetical characters.", NameParameter);
				}
			}
		}

		/// <summary>
		/// Gets and sets the number of messages sent by a worker
		/// </summary>
		/// <exception cref="ArgumentException"></exception>
		/// <returns>an employee's number of messages</returns>
		public string Messages
		{
			get { return employeeMessages.ToString(); }
			set
			{
				try
				{
					//Prevent non-numerical values from being passed to messages
					if (!int.TryParse(value, out var temp))
					{
						//if not able to convert input to integer, it get's caught by formatexception but processed by argument exception
						throw new ArgumentException(MessagesParameter, "Messages can only contain numerical values");
					}

					if (!(temp >= 15000))
					{
						employeeMessages = temp;
					}
					else
					{
						throw new ArgumentException("Message sent value can NOT exceed or be equal to 15000",
							MessagesParameter);
					}
				}
				catch (ArgumentNullException)
				{
					//Catching null entries
					throw new ArgumentNullException(MessagesParameter, "Messages can not be zero value.");

				}
				catch (ArgumentException)
				{
					// if input is NOT containing a valid number
					throw new ArgumentException("Message sent value must be a positive whole number",
						MessagesParameter);

				}



				//employeeMessages = Convert.ToInteger(value); // This line is dangerous and should probably never appear in your code. Can you explain why? Post about it on the Q&A board and I'll give you a stock.

				/* It is "dangerous" because it is attempting to convert a string attribute of an object to an integer.
				 Due to the nature of the attribute for the class, there is no way for the program to perform the conversion in this way, at least within the accessor/mutator of the attribute.A better method would be to convert it with an separate class method. OR use a try parse first to ensure the contents are numeric before attempting. Even then Convert.ToInt32() is a better option. */

			}
		}

		/// <summary>
		/// Gets the entered worker's pay
		/// </summary>
		/// <returns>a worker's pay</returns>
		public decimal Pay
		{
			get { return employeePay; }
		}

		/// <summary>
		/// Gets the overall total pay among all workers
		/// </summary>
		/// <returns>the overall total pay among all workers</returns>
		public static decimal TotalPay
		{
			get { return overallPayroll; }
		}

		/// <summary>
		/// Gets the overall number of workers
		/// </summary>
		/// <returns>the overall number of workers</returns>
		public static int TotalWorkers
		{
			get { return overallNumberOfEmployees; }
		}

		/// <summary>
		/// Gets the overall number of messages sent
		/// </summary>
		/// <returns>the overall number of messages sent</returns>
		public static int TotalMessages
		{
			get { return overallMessages; }
		}

		/// <summary>
		/// Calculates and returns an average pay among all workers
		/// </summary>
		/// <returns>the average pay among all workers</returns>
		public static decimal AveragePay
		{
			get
			{

				try
				{

					//Calculation for average total pay for all workers, rounded to 2 decimal places
					overallAverage = Math.Round(Convert.ToDecimal(overallNumberOfEmployees) / overallPayroll, 2);
				}
				//Incase something get's past the condition from before
				//make sure it will not take zero or empty
				catch (DivideByZeroException)
				{
					//pass stored value to overallAverage as no average to be calculated
					overallAverage = overallPayroll;

					//rerturn only payroll in current state (aka. only has current worker pay stored as value)
					return overallAverage;
				}
				return overallAverage;
			}
		}

		/// <summary>
		/// Method to reset all attributes
		/// for summary form
		/// </summary>
		public static void ResetSummary()
		{
			overallPayroll = 0;
			overallAverage = 0;
			overallMessages = 0;
			overallNumberOfEmployees = 0;
		}

		public static DataGrid ShowDataGrid()
		{
			DataGrid toGrid = new DataGrid();
			var fromTable = DataAccess.GetEmployeeList();

			toGrid.ItemsSource = fromTable.AsDataView();
			return toGrid;
		}

		#endregion

	}
}
