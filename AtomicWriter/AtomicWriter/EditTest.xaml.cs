using AtomicWriter.Objects;
using Newtonsoft.Json;
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
            if (Test.Instructions != null)
            {
                Test.Instructions.ForEach(instruction => AddInstruction(instruction));
            }
        }

		private void InstructionTypeChanged(object sender, RoutedEventArgs e)
		{
			var instructionTypeSelection = ((ComboBox)sender);
			var instructionPanel = ((StackPanel)instructionTypeSelection.Parent);
			var locatorSelection = ((ComboBox)instructionPanel.Children[1]);

			if ((Instruction.InstructionTypes)instructionTypeSelection.SelectedItem == Instruction.InstructionTypes.Click)
			{
				locatorSelection.Visibility = Visibility.Visible;
			}
			else
			{
				locatorSelection.Visibility = Visibility.Collapsed;
			}
		}

		private void AddInstruction(Instruction instruction)
		{
			var instructionPanel = new StackPanel()
			{
				Orientation = Orientation.Horizontal
			};

			var instructionTypeSelection = new ComboBox()
			{
				ItemsSource = Instruction.GetInstructionTypes(),
				SelectedItem = instruction.InstructionType
			};
			instructionTypeSelection.SelectionChanged += InstructionTypeChanged;

			instructionPanel.Children.Add(instructionTypeSelection);

			var text = string.Empty;
			Locator locator = null;
			if (instruction.InstructionType == Instruction.InstructionTypes.Click)
			{
				locator = JsonConvert.DeserializeObject<Locator>(instruction.Payload);
				text = locator.Path;
			}
			else
			{
				text = instruction.Payload;
			}

			var locatorSelection = new ComboBox()
			{
				ItemsSource = Locator.GetLocatorTypes(),
				Visibility = instruction.InstructionType == Instruction.InstructionTypes.Click ? Visibility.Visible : Visibility.Collapsed,
				SelectedItem = instruction.InstructionType == Instruction.InstructionTypes.Click ? locator.LocatorType : Locator.LocatorTypes.Id,
			};

			instructionPanel.Children.Add(locatorSelection);

			instructionPanel.Children.Add(new TextBox()
			{
				Text = text
			});


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

			InstructionsList.Children.Add(instructionPanel);
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
						payload = ((TextBox)instructionPanel.Children[2]).Text;
						break;
					case Instruction.InstructionTypes.Click:
						var xpath = ((TextBox)instructionPanel.Children[2]).Text;
						var locatorType = (Locator.LocatorTypes)((ComboBox)(instructionPanel).Children[1]).SelectedValue;

						var locator = new Locator() { LocatorType = locatorType, Path = xpath };
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
			var instructionTypeSelection = new ComboBox()
			{
				ItemsSource = Instruction.GetInstructionTypes()
			};

			instructionPanel.Children.Add(instructionTypeSelection);
			instructionTypeSelection.SelectionChanged += InstructionTypeChanged;

			var locatorSelection = new ComboBox()
			{
				ItemsSource = Locator.GetLocatorTypes(),
				Visibility = Visibility.Collapsed,
				SelectedItem = Locator.LocatorTypes.Id,
			};

			instructionPanel.Children.Add(locatorSelection);
			instructionPanel.Children.Add(new TextBox() { });

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

			InstructionsList.Children.Add(instructionPanel);

		}
	}
}
