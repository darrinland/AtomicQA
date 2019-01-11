using AtomicWriter.DataAccess;
using AtomicWriter.Objects;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;

namespace AtomicWriter
{
	/// <summary>
	/// Interaction logic for Dashboard.xaml
	/// </summary>
	public partial class Dashboard : MetroWindow
	{
		private static string _logLocation = string.Empty;

		public Dashboard()
		{
			InitializeComponent();
		}

		private void OpenLogButton_Click(object sender, RoutedEventArgs e)
		{
			do
			{
				GetLogFile();
			} while (String.IsNullOrEmpty(_logLocation));

			PopulateLogList();
		}

		private void OpenScreenshot_Click(object sender, RoutedEventArgs e)
		{
			var location = ((TextBlock)((System.Windows.Controls.Button)sender).Content).Text;
			System.Diagnostics.Process.Start(location);
		}

		private void PopulateLogList()
		{
			var logs = DataReader.LoadObject<List<Log>>(_logLocation);
			if (logs.Count > 0)
			{
				LogListMessage.Visibility = Visibility.Visible;
				LogListMessage.Text = $"{logs.Count} Errors Found";

				foreach (var log in logs)
				{
					var logPanel = new StackPanel()
					{
						Orientation = System.Windows.Controls.Orientation.Horizontal,
						HorizontalAlignment = System.Windows.HorizontalAlignment.Center
					};
					logPanel.Children.Add(new TextBlock() { Text = log.Test.TestName, FontWeight = FontWeights.Bold, Margin = new Thickness(5) });
					logPanel.Children.Add(new TextBlock() { Text = log.Instruction.InstructionType.ToString() + " " + log.Instruction.Payload.ToString(), Margin = new Thickness(5) });
					logPanel.Children.Add(new TextBlock() { Text = log.Exception.Message, Margin = new Thickness(5) });
					var screenshotLocation = new System.Windows.Controls.Button()
					{
						Content = new TextBlock()
						{
							Text = log.ScreenshotLocation
						},
						Margin = new Thickness(5)
					};
					screenshotLocation.Click += OpenScreenshot_Click;
					logPanel.Children.Add(screenshotLocation);
					LogContents.Children.Add(logPanel);
				}
			}
			else
			{
				LogListMessage.Visibility = Visibility.Visible;
				LogListMessage.Text = "No Errors Found";
			}
		}

		private static void GetLogFile()
		{
			var b = new OpenFileDialog
			{
				FileName = _logLocation
			};

			if (b.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				_logLocation = b.FileName;
			}
		}
	}
}
