using AtomicWriter.DataAccess;
using AtomicWriter.Objects;
using MahApps.Metro.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace AtomicWriter
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : MetroWindow
	{
		private SaveObject _saveObject;
		private string _location = @"C:\Tests\test.json";

		public MainWindow()
		{
			InitializeComponent();
			InitSaveObject();
			//SaveCurrentSaveObject();
			InitTestList();
		}

		private void InitTestList()
		{
			if(_saveObject != null)
			{
				TestsList.ItemsSource = _saveObject.Tests.Select(x => x.TestName);
			}
		}

		private void InitSaveObject()
		{
			while(_saveObject == null)
			{
				OpenTestSuite();
			}
			//_saveObject = DataReader.LoadObject(_location);
			//_saveObject = new SaveObject()
			//{
			//	Tests = new List<Test>()
			//	{
			//		new Test()
			//		{
			//			TestName = "Go to google",
			//			Instructions = new List<Instruction>()
			//			{
			//				new Instruction()
			//				{
			//					InstructionType = Instruction.InstructionTypes.GoToUrl,
			//					Payload = "https://www.google.com"
			//				}
			//			}
			//		},
			//		new Test()
			//		{
			//			TestName = "NewTestName"
			//		}
			//	}
			//};
		}

		private void TestsList_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			var selected = ((ListBox)sender).SelectedValue.ToString();
			var editTestWindow = new EditTest(_saveObject.Tests.First(x => x.TestName == selected));

			var result = editTestWindow.ShowDialog();
			if (result == true)
			{
				SaveCurrentSaveObject();
			}
		}

		private void NewTestButton_Click(object sender, RoutedEventArgs e)
		{
			var editTestWindow = new EditTest(new Test());

			var result = editTestWindow.ShowDialog();
			if (result == true)
			{
				_saveObject.Tests.Add(editTestWindow.Test);
				SaveCurrentSaveObject();
			}
		}

		private void SaveCurrentSaveObject()
		{
			DataWriter.Save(_location, _saveObject);
			InitTestList();
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

			_saveObject = DataReader.LoadObject(_location);
			InitTestList();
		}

		private void OpenTestSuiteButton_Click(object sender, RoutedEventArgs e)
		{
			OpenTestSuite();
		}

		private void DeleteTestButton_Click(object sender, RoutedEventArgs e)
		{
			var selectedTest = ((string)TestsList.SelectedItem);
			if(string.IsNullOrEmpty(selectedTest))
			{
				return;
			}

			var result = MessageBox.Show($"Are you sure you would like to delete the test '{selectedTest}'? ", "This operation cannot be undone.", MessageBoxButton.YesNo);

			if(result == MessageBoxResult.Yes)
			{
				_saveObject.Tests.Remove(_saveObject.Tests.First(x => x.TestName == selectedTest));
				SaveCurrentSaveObject();
			}
		}
	}
}
