/*MainWindow.xaml.cs
 *
 * Title: Main Application Window
 *
 * Author: Katherine Bellman
 * Course: NETD 3202
 * Date: October 26th 2021
 *
 */

using System.Windows;
using System.Windows.Controls;

namespace HourlyWorkerPayRoll
{
	public partial class MainWindow
	{
		public MainWindow()
		{
			InitializeComponent();
			LoadSummary();
		}

		//==ALL EVENT HANDLERS===========================================
		#region EVENT HANDLERS

		#region WORKER ENTRY CONTROLS _ EVENT HANDLERS

		private void TextBoxChanged(object sender, TextChangedEventArgs e)
		{
			throw new System.NotImplementedException();
		}

		private void CalculateClick(object sender, RoutedEventArgs e)
		{
			throw new System.NotImplementedException();
		}

		private void ClearClick(object sender, RoutedEventArgs e)
		{
			throw new System.NotImplementedException();
		}
		#endregion

		#region SUMMARY CONTROLS _ EVENT HANDLERS


		private void LoadSummary()
		{

			textBoxTotalPay.Text = HourlyWorkerPay.TotalPay.ToString("C");
			textBoxTotalMessages.Text = HourlyWorkerPay.TotalMessages.ToString();
			textBoxTotalWorkers.Text = HourlyWorkerPay.TotalWorkers.ToString();

			if (HourlyWorkerPay.TotalWorkers != 0)
			{
				textBoxAveragePay.Text = (HourlyWorkerPay.TotalPay / HourlyWorkerPay.TotalWorkers).ToString("C");
			}

		}

		private void ResetSummaryClick(object sender, RoutedEventArgs e)
		{
			throw new System.NotImplementedException();
		}

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



		#endregion
		#endregion
		//=END===ALL EVENT HANDLERS =====================================
		//FORM METHODS ==================================================


		//=END===FORM METHODS ===========================================
	}
}
