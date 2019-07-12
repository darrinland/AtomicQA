using AtomicWriter.DataAccess;
using AtomicWriter.Objects;
using MahApps.Metro.Controls;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TestRunner;

namespace AtomicWriter
{
	/// <summary>
	/// Interaction logic for TestRunner.xaml
	/// </summary>
	public partial class TestsRunner : MetroWindow
	{
		private string _location;
		private SaveObject _saveObject;
		private static string _logLocation = @"C:/Tests/log.json";

		public TestsRunner()
		{
			InitializeComponent();
		}

		private void BackToWelcomeScreenButton_Click(object sender, RoutedEventArgs e)
		{
			var projectSelection = new WelcomeScreen();
			projectSelection.Show();
			this.Close();
		}

		private void OpenTestSuite()
		{
			var b = new System.Windows.Forms.OpenFileDialog
			{
				FileName = _location,
				InitialDirectory = @"C:/Tests/"
			};

			if (b.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				_location = b.FileName;
			}
			else
			{
				return;
			}

			_saveObject = DataReader.LoadObject<SaveObject>(_location);
		}

		private void OpenTestSuiteButton_Click(object sender, RoutedEventArgs e)
		{
			OpenTestSuite();
			if(_saveObject != null)
			{
				TestsFoundLabel.Text = $"{_saveObject.Tests.Count} Tests Found";
				TestsFoundLabel.Visibility = Visibility.Visible;

				RunTestsButton.Visibility = Visibility.Visible;
			}
			
		}

		private void RunTestsButton_Click(object sender, RoutedEventArgs e)
		{
			var thread = new Thread(x =>
			{
				RunTestFile();
			});
			thread.Start();
		}

		private void RunTestFile()
		{
			var logger = new Logger();
			_saveObject.Tests.ForEach(test =>
            {
                using (var testRunner = new TestRunner.TestRunner(logger))
				{
					testRunner.RunTest(test, _saveObject.Tests);
				}
            });

			DataWriter.Save(_logLocation, logger.GetLogs());
		}
	}
}
