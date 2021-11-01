/*MainWindow.xaml.cs
 *
 * Title: Main Application 
 *
 * Author: Katherine Bellman
 * Course: NETD 3202
 * Date: October 26th 2021
 *
 * Modified: October 31st 2021
 *
 * Description: Main application functionality file
 */

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace HourlyWorkerPayRoll
{
	public partial class MainWindow
	{
		static bool StartUpIsFalse = true;
		//Obtains system time and interval data for clock
		System.Windows.Threading.DispatcherTimer Timer = new();
		public MainWindow()
		{
			InitializeComponent();
			//set to false = is startup
			StartUpIsFalse = false;
			//Start the dynamic clock on status bar
			//// Clock functionality reference :https://www.c-sharpcorner.com/blogs/digital-clock-in-wpf1
			Timer.Tick += new EventHandler(DisplayClock);
			Timer.Interval = new TimeSpan(00, 00, 01);
			Timer.Start();

			// Change current status
			StatusBarUpdate("Application started...Clock loaded.");
		}

		//==ALL EVENT HANDLERS===========================================
		#region EVENT HANDLERS

		#region WORKER ENTRY CONTROLS _ EVENT HANDLERS

		/// <summary>
		/// Removes error setting for textbox
		/// and empties error message for associated label
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void TextBoxChanged(object sender, TextChangedEventArgs e)
		{
			RemoveRedAngryError((TextBox)sender);

			//check which sent changed event to change label
			if ((TextBox)sender == textBoxWorkerNameFirst || (TextBox)sender == textBoxWorkerNameLast)
			{
				if ((TextBox)sender == textBoxWorkerNameFirst)
				{
					labelNameError.Content = string.Empty;
				}
				else
				{
					labelNameError.Content = string.Empty;
				}
			}
			else
			{
				labelMessagesError.Content = string.Empty;
			}
		}

		/// <summary>
		/// Creates a new HourlyWorkerPay object from input values
		/// confirms that Pay is valid, then outputs calculated
		/// values from created object within the class, into their
		/// corresponding textbox
		/// and disables/focuses certain controls 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CalculateClick(object sender, RoutedEventArgs e)
		{
			ClearAllAngryRedErrors();

			try
			{
				//create a new HourlyWorkerPay object and apply textbox inputs to attributes
				HourlyWorkerPay worker = new(textBoxWorkerNameFirst.Text, textBoxWorkerNameLast.Text, textBoxMessagesSent.Text);

				//Should worker have 0 as pay, it is not valid
				//Error message will have alredy been reported to user
				if (worker.Pay == 0)
				{
					textBoxWorkerNameFirst.Focus();
					textBoxWorkerNameFirst.SelectAll();
					textBoxWorkerNameLast.Focus();
					textBoxWorkerNameLast.SelectAll();
				}
				//Show attributes of current worker to user, as input is valid
				else
				{
					//output strings of calculated attribute into coresponding textbox with valid output format
					textBoxWorkerTotalPay.Text = worker.Pay.ToString("C");
					StatusBarUpdate($"Worker {worker.FirstName + worker.LastName} with {worker.Messages} messages, with {worker.Pay}pay was successfully added.");

				}

				//Disable calculate button & input fields
				buttonCalculate.IsEnabled = false;
				textBoxWorkerNameFirst.IsEnabled = false;
				textBoxWorkerNameLast.IsEnabled = false;
				textBoxMessagesSent.IsEnabled = false;

				//Clear ANGRY RED errors
				ClearAllAngryRedErrors();

				//apply focus to clear button
				buttonClear.Focus();
			}
			//To catch argumentexceptions for responses to predicted issues within HourlyWorkerPay class
			catch (ArgumentException error)
			{
				switch (error.ParamName)
				{
					//Check other parameters
					case HourlyWorkerPay.NameParameter:
						labelNameError.Content = error.Message;
						ShowRedAngryError(textBoxWorkerNameFirst);
						ShowRedAngryError(textBoxWorkerNameLast);
						StatusBarUpdate("Error: Check Worker Name fields.");
						break;
					case HourlyWorkerPay.MessagesParameter:
						labelMessagesError.Content = error.Message;
						ShowRedAngryError(textBoxMessagesSent);
						StatusBarUpdate("Error: Check Messages Sent field.");
						break;
				}
			}
		}

		/// <summary>
		/// Calls SetDefaults Method
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ClearClick(object sender, RoutedEventArgs e)
		{
			SetDefaults();
		}
		#endregion

		#region SUMMARY CONTROLS _ EVENT HANDLERS



		/// <summary>
		/// Event handler for Summary Reset button
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ResetSummaryClick(object sender, RoutedEventArgs e)
		{
			//clear textboxes
			textBoxTotalPay.Clear();
			textBoxAveragePay.Clear();
			textBoxTotalMessages.Clear();
			textBoxTotalWorkers.Clear();

			//Update textboxes with default values
			textBoxTotalPay.Text = "$0.00";
			textBoxAveragePay.Text = "$0.00";
			textBoxTotalMessages.Text = "0";
			textBoxTotalWorkers.Text = "0";

			StatusBarUpdate("Summary tab view values have been reset.");
		}

		#region EMPLOYEE LIST TAB EVENT HANDLERS
		//Space holder for Employee list tab
		#endregion
		#endregion

		#region APPLICATION LEVEL EVENT HANDLERS

		/// <summary>
		/// Closes the current application window
		/// when button clicked (event triggred)
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CloseClick(object sender, RoutedEventArgs e)
		{
			Close();
		}

		/// <summary>
		/// Gets the time of day
		/// outputs time as specified format
		/// uses Ticks to track time intervals for each part of
		/// the clock display output & updates it as
		/// time elapses
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="eventArgs"></param>
		private void DisplayClock(object sender, EventArgs eventArgs)
		{
			//sets the current time to a varriable (Reference: link below, for TimeOfDay rather than without it)
			var time = DateTime.Now.TimeOfDay;

			// reference for obtaining 12h time format rather than 24 h format: https://stackoverflow.com/questions/10123426/how-to-convert-24-hour-format-timespan-to-12-hour-format-timespan
			//concatenates the time from DateTime.Now and sends it to Content output for statusBarClock item
			statusBarClock.Content = new DateTime(time.Ticks).ToString("hh:mm:ss tt");
		}

		/// <summary>
		/// When a selected tab is changed, it will generate
		/// the data for the selected tab or other functional actions
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void TabChanged(object sender, SelectionChangedEventArgs e)
		{

			//When Entry tab is accessed, focus set to clear button
			if (Equals(tabControlInterface.SelectedItem, tabPayrollEntry))
			{
				buttonClear.Focus();
				//Do not show status update at start unless...
				if (StartUpIsFalse == true)
				{
					StatusBarUpdate("Viewing Payroll Entry Tab");
				}

				//while in on tab, change tab background brush
				tabPayrollEntry.Background = Brushes.AliceBlue;
				//When not on tab, change tab background brush 
				if (!tabPayrollEntry.IsSelected) { tabPayrollEntry.Background = Brushes.LightSteelBlue; }
			}

			//When Summary tab is accessed, database values populate read-only output fields
			else if (Equals(tabControlInterface.SelectedItem, tabSummary))
			{
				textBoxTotalWorkers.Text = HourlyWorkerPay.TotalWorkers.ToString("####");
				textBoxTotalMessages.Text = HourlyWorkerPay.TotalMessages.ToString();
				textBoxTotalPay.Text = HourlyWorkerPay.TotalPay.ToString("C");
				textBoxAveragePay.Text = HourlyWorkerPay.AveragePay.ToString("C");
				StatusBarUpdate("Viewing Summary tab.");

				//Set startup value to true aka. app is beyond initialization
				StartUpIsFalse = true;

				//while in on tab, change tab background brush
				tabSummary.Background = Brushes.AliceBlue;
				//When not on tab, change tab background brush 
				if (!tabSummary.IsSelected) { tabSummary.Background = Brushes.LightSteelBlue; }
			}

			//When Employee list tab is accessed, datagrid view obtains data from the database
			else if (Equals(tabControlInterface.SelectedItem, tabEmployeeList))
			{
				dataGridEmployeeList.ItemsSource = DataAccess.GetEmployeeList().DefaultView;

				//Set startup value to true aka. app is beyond initialization
				StartUpIsFalse = true;

				//while in on tab, change tab background brush
				tabEmployeeList.Background = Brushes.AliceBlue;
				//When not on tab, change tab background brush 
				if (!tabSummary.IsSelected) { tabSummary.Background = Brushes.LightSteelBlue; }
			}
		}



		#endregion
		#endregion
		//=END===ALL EVENT HANDLERS =====================================
		//FORM METHODS ==================================================
		#region FORM METHODS ======================================

		/// <summary>
		/// Resets all defaults for application
		/// when called.
		/// </summary>
		private void SetDefaults()
		{
			//Clear input fields & clear worker Total pay output textbox
			textBoxWorkerNameFirst.Clear();
			textBoxWorkerNameLast.Clear();
			textBoxMessagesSent.Clear();

			textBoxWorkerTotalPay.Text = "$0.00";

			//Re-enable disabled controls
			buttonCalculate.IsEnabled = true;
			textBoxWorkerNameFirst.IsEnabled = true;
			textBoxWorkerNameLast.IsEnabled = true;
			textBoxMessagesSent.IsEnabled = true;

			//Reset all error lables
			ClearAllAngryRedErrors();

			//Set focus to first user input textbox
			textBoxWorkerNameFirst.Focus();

			StatusBarUpdate("Entry tab cleared, ready for new entry.");
		}

		/// <summary>
		/// Changes colour setting for textbox
		/// selects input and obtains focus
		/// </summary>
		/// <param name="textBoxInError"></param>
		private void ShowRedAngryError(TextBox textBoxInError)
		{
			//Change colour settings for textbox
			textBoxInError.Background = Brushes.Tomato;
			textBoxInError.BorderBrush = Brushes.Crimson;

			//Select current input and set focus
			textBoxInError.SelectAll();
			textBoxInError.Focus();
		}


		/// <summary>
		/// Clears textbox colour settings back to
		/// defaults
		/// </summary>
		/// <param name="textBoxToClear"></param>
		private void RemoveRedAngryError(TextBox textBoxToClear)
		{
			textBoxToClear.Background = Brushes.White;
			textBoxToClear.BorderBrush = Brushes.DarkGray;
		}


		/// <summary>
		/// Clears all the ANGRY RED errors
		/// from input textboxes and labels
		/// </summary>
		private void ClearAllAngryRedErrors()
		{
			//Remove error colour settings from textboxes
			RemoveRedAngryError(textBoxWorkerNameFirst);
			RemoveRedAngryError(textBoxWorkerNameLast);
			RemoveRedAngryError(textBoxMessagesSent);

			//clear error message labels
			labelNameError.Content = string.Empty;
			labelMessagesError.Content = string.Empty;

		}


		/// <summary>
		/// When called, it changes the status label
		/// within status bar to passed status message
		/// </summary>
		/// <param name="status">Application status</param>
		private void StatusBarUpdate(string status)
		{
			//for changing the text of the label on the status bar of applicaiton
			labelStatus.Content = status;
		}


		#endregion //===============================================
		//=END===FORM METHODS ===========================================
	}
}
