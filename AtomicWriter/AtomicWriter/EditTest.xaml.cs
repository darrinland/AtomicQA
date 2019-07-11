﻿using AtomicWriter.Objects;
using MahApps.Metro.Controls;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace AtomicWriter
{
	/// <summary>
	/// Interaction logic for EditTest.xaml
	/// </summary>

	public partial class EditTest : MetroWindow
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
			StackPanel instructionPanel = ((StackPanel)instructionTypeSelection.Parent);
			var locatorSelection = ((ComboBox)instructionPanel.Children[1]);
			var textInput = ((TextBox)instructionPanel.Children[3]);
			var keySelection = ((ComboBox)instructionPanel.Children[4]);

			var selectedInstruction = (Instruction.InstructionTypes)instructionTypeSelection.SelectedItem;
			var displayLocatorSelection = selectedInstruction == Instruction.InstructionTypes.Click || selectedInstruction == Instruction.InstructionTypes.InputText || selectedInstruction == Instruction.InstructionTypes.Assert || selectedInstruction == Instruction.InstructionTypes.SendKeys;
			if (displayLocatorSelection)
			{
				locatorSelection.Visibility = Visibility.Visible;
			}
			else
			{
				locatorSelection.Visibility = Visibility.Collapsed;
			}

			if (selectedInstruction == Instruction.InstructionTypes.InputText || selectedInstruction == Instruction.InstructionTypes.Assert)
			{
				textInput.Visibility = Visibility.Visible;
			}
			else
			{
				textInput.Visibility = Visibility.Collapsed;
			}

			if (selectedInstruction == Instruction.InstructionTypes.SendKeys)
			{
				keySelection.Visibility = Visibility.Visible;
			}
			else
			{
				keySelection.Visibility = Visibility.Collapsed;
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
			var inputText = string.Empty;
			System.Windows.Forms.Keys keySelection = 0;

			if (instruction.InstructionType == Instruction.InstructionTypes.Click)
			{
				locator = JsonConvert.DeserializeObject<Locator>(instruction.Payload);
				text = locator.Path;
			}
			else if (instruction.InstructionType == Instruction.InstructionTypes.InputText)
			{
				var inputTextType = JsonConvert.DeserializeObject<TextInputInstruction>(instruction.Payload);
				locator = inputTextType.Locator;
				text = locator.Path;
				inputText = inputTextType.Text;
			}
			else if (instruction.InstructionType == Instruction.InstructionTypes.Assert)
			{
				var assert = JsonConvert.DeserializeObject<AssertValueInstruction>(instruction.Payload);
				locator = assert.Locator;
				text = locator.Path;
				inputText = assert.ExpectedValue;
			}
			else if (instruction.InstructionType == Instruction.InstructionTypes.SendKeys)
			{
				var sendKeyInstruction = JsonConvert.DeserializeObject<SendKeyInstruction>(instruction.Payload);
				locator = sendKeyInstruction.Locator;
				text = locator.Path;
				keySelection = sendKeyInstruction.Key;
			}
			else
			{
				text = instruction.Payload;
			}

			var displayLocatorSelection = instruction.InstructionType == Instruction.InstructionTypes.Click ||
										  instruction.InstructionType == Instruction.InstructionTypes.Assert ||
										  instruction.InstructionType == Instruction.InstructionTypes.InputText ||
										  instruction.InstructionType == Instruction.InstructionTypes.SendKeys;
			var locatorSelection = new ComboBox()
			{
				ItemsSource = Locator.GetLocatorTypes(),
				Visibility = displayLocatorSelection ? Visibility.Visible : Visibility.Collapsed,
				SelectedItem = displayLocatorSelection ? locator.LocatorType : Locator.LocatorTypes.Id,
			};
			instructionPanel.Children.Add(locatorSelection);
			instructionPanel.Children.Add(new TextBox()
			{
				Text = text
			});

			var typedTextInput = new TextBox()
			{
				Visibility = instruction.InstructionType == Instruction.InstructionTypes.InputText || instruction.InstructionType == Instruction.InstructionTypes.Assert ? Visibility.Visible : Visibility.Collapsed,
				Text = inputText,
			};
			instructionPanel.Children.Add(typedTextInput);

			var sendKeySelection = new ComboBox()
			{
				ItemsSource = SendKeyInstruction.GetSendKeyTypes(),
				Visibility = instruction.InstructionType == Instruction.InstructionTypes.SendKeys ? Visibility.Visible : Visibility.Collapsed,
				SelectedItem = keySelection,
			};
			instructionPanel.Children.Add(sendKeySelection);

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
					case Instruction.InstructionTypes.InputText:
						xpath = ((TextBox)instructionPanel.Children[2]).Text;
						var textInput = ((TextBox)instructionPanel.Children[3]).Text;
						locatorType = (Locator.LocatorTypes)((ComboBox)(instructionPanel).Children[1]).SelectedValue;
						locator = new Locator() { LocatorType = locatorType, Path = xpath };
						TextInputInstruction input = new TextInputInstruction()
						{
							Locator = locator,
							Text = textInput,
						};
						payload = JsonConvert.SerializeObject(input);
						break;
					case Instruction.InstructionTypes.Assert:
						xpath = ((TextBox)instructionPanel.Children[2]).Text;
						var expectedValue = ((TextBox)instructionPanel.Children[3]).Text;
						locatorType = (Locator.LocatorTypes)((ComboBox)(instructionPanel).Children[1]).SelectedValue;
						locator = new Locator() { LocatorType = locatorType, Path = xpath };
						AssertValueInstruction assertValue = new AssertValueInstruction()
						{
							Locator = locator,
							ExpectedValue = expectedValue,
						};
						payload = JsonConvert.SerializeObject(assertValue);
						break;
					case Instruction.InstructionTypes.SendKeys:
						xpath = ((TextBox)instructionPanel.Children[2]).Text;
						locatorType = (Locator.LocatorTypes)((ComboBox)(instructionPanel).Children[1]).SelectedValue;
						locator = new Locator() { LocatorType = locatorType, Path = xpath };
						var keySelection = (System.Windows.Forms.Keys)((ComboBox)(instructionPanel).Children[4]).SelectedValue;
						SendKeyInstruction sendKeyInstruction = new SendKeyInstruction()
						{
							Locator = locator,
							Key = keySelection,
						};
						payload = JsonConvert.SerializeObject(sendKeyInstruction);
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

			var textInput = new TextBox()
			{
				Visibility = Visibility.Collapsed
			};
			instructionPanel.Children.Add(textInput);

			var keySelection = new ComboBox()
			{
				ItemsSource = SendKeyInstruction.GetSendKeyTypes(),
				Visibility = Visibility.Collapsed,
				SelectedItem = Locator.LocatorTypes.Id,
			};
			instructionPanel.Children.Add(keySelection);

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
