using AtomicWriter.Objects;
using MahApps.Metro.Controls;
using System;
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

		public MainWindow()
		{
			InitializeComponent();
			InitSaveObject();
			InitTestList();
		}

		private void InitTestList()
		{
			TestsList.ItemsSource = _saveObject.Tests.Select(x => x.TestName);
		}

		private void InitSaveObject()
		{
			_saveObject = new SaveObject()
			{
				Tests = new List<Test>()
				{
					new Test()
					{
						TestName = "Go to google",
						Instructions = new List<Instruction>()
						{
							new Instruction()
							{
								InstructionType = Instruction.InstructionTypes.GoToUrl,
								Payload = "https://www.google.com"
							}
						}
					},
					new Test()
					{
						TestName = "NewTestName"
					}
				}
			};
		}

		private void TestsList_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			var selected = ((ListBox)sender).SelectedValue.ToString();
			var editTestWindow = new EditTest(_saveObject.Tests.First(x => x.TestName == selected));

			var result = editTestWindow.ShowDialog();
			if (result == true)
			{
				MessageBox.Show(editTestWindow.Test.TestName);
			}
		}
		
		private void NewTestButton_Click(object sender, RoutedEventArgs e)
		{
			var editTestWindow = new EditTest(new Test());

			var result = editTestWindow.ShowDialog();
			if (result == true)
			{
				MessageBox.Show(editTestWindow.Test.TestName);
			}
		}
	}
}
