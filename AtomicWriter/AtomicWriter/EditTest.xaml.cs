using AtomicWriter.Objects;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace AtomicWriter
{
	/// <summary>
	/// Interaction logic for EditTest.xaml
	/// </summary>

	public partial class EditTest : Window
	{
		public Test Test { get; set; }

		public EditTest(Test test)
		{
			InitializeComponent();
			Test = test;
			SetTestValues();
		}

		private void SetTestValues()
		{
			TestName.Text = Test.TestName;
		}

		private void GetUpdatedValues()
		{
			Test.TestName = TestName.Text;

			var instructions = InstructionsList.Children;
			Test.Instructions = new List<Instruction>() { };
			foreach (StackPanel instructionPanel in instructions)
			{
				ComboBox instructionTypeComboBox = (ComboBox)(instructionPanel).Children[0];
				Instruction.InstructionTypes selectedInstructionType = (Instruction.InstructionTypes)instructionTypeComboBox.SelectedValue;
				string payload = string.Empty;

				switch (selectedInstructionType)
				{
					case Instruction.InstructionTypes.GoToUrl:
						payload = ((TextBox)instructionPanel.Children[1]).Text;
						break;
					case Instruction.InstructionTypes.Click:
						var xpath = ((TextBox)instructionPanel.Children[1]).Text;
						var locator = new Locator() { LocatorType = Locator.LocatorTypes.XPath, Path = xpath };
						payload = Newtonsoft.Json.JsonConvert.SerializeObject(locator);
						break;
					default:
						MessageBox.Show("Error matching InstructionType");
						break;
				}

				Test.Instructions.Add(new Instruction()
				{
					InstructionType = selectedInstructionType,
					Payload = payload,
				});
			}
		}

		private void SaveButton_Click(object sender, RoutedEventArgs e)
		{
			GetUpdatedValues();
			Window.GetWindow(this).DialogResult = true;
			Window.GetWindow(this).Close();
		}

        private void DeleteInstructionButton_Click(object sender, RoutedEventArgs e)
        {
            var parentPanel = ((StackPanel)((Button)sender).Parent);
            InstructionsList.Children.Remove(parentPanel);
        }

		private void AddInstructionButton_Click(object sender, RoutedEventArgs e)
		{
			var instructionPanel = new StackPanel()
			{
				Orientation = Orientation.Horizontal
			};
			instructionPanel.Children.Add(new ComboBox()
			{
				ItemsSource = Instruction.GetInstructionTypes()
			});
			instructionPanel.Children.Add(new TextBox() { });

			InstructionsList.Children.Add(instructionPanel);

            var deleteInstructionButton = new Button()
            {
                Width = 50,
                Content = new ContentControl()
                {
                    Content = "Delete",
                },
            };
            deleteInstructionButton.Click += new RoutedEventHandler(DeleteInstructionButton_Click);
            instructionPanel.Children.Add(deleteInstructionButton);
		}
	}
}
