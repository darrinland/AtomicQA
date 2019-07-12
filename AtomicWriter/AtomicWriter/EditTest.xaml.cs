using AtomicWriter.Objects;
using TestRunner.Objects;
using MahApps.Metro.Controls;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TestRunner.Objects;

namespace AtomicWriter
{
	/// <summary>
	/// Interaction logic for EditTest.xaml
	/// </summary>

	public partial class EditTest : MetroWindow
	{
		public Test Test { get; set; }
        public List<Test> Molecules { get; set; }

		public EditTest(Test test, List<Test> molecules)
		{
			InitializeComponent();
			Test = test;
            Molecules = molecules;
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

        private ComboBox GetMoleculeComboBox()
        {
            var comboBox = new ComboBox();
            Molecules.ForEach(molecule => { comboBox.Items.Add(new TextBlock() { Text = molecule.TestName, DataContext = molecule }); });
            return comboBox;
        }

		private void InstructionTypeChanged(object sender, RoutedEventArgs e)
		{
			var instructionTypeSelection = ((ComboBox)sender);
			StackPanel instructionPanel = ((StackPanel)instructionTypeSelection.Parent);
			var locatorSelection = ((ComboBox)instructionPanel.Children[1]);
            var textInput = ((TextBox)instructionPanel.Children[3]);
            var keySelection = ((ComboBox)instructionPanel.Children[4]);
            var moleculeSelection = ((ComboBox) instructionPanel.Children[5]);

            var selectedInstruction = (Instruction.InstructionTypes)instructionTypeSelection.SelectedItem;
            var displayLocatorSelection = selectedInstruction == Instruction.InstructionTypes.Click || selectedInstruction == Instruction.InstructionTypes.InputText || selectedInstruction == Instruction.InstructionTypes.AssertElementExists || selectedInstruction == Instruction.InstructionTypes.AssertValue ||  selectedInstruction == Instruction.InstructionTypes.SendKeys;

            locatorSelection.Visibility = displayLocatorSelection ? Visibility.Visible : Visibility.Collapsed;

			if (selectedInstruction == Instruction.InstructionTypes.InputText || selectedInstruction == Instruction.InstructionTypes.AssertValue)
			{
				textInput.Visibility = Visibility.Visible;
			}
			else
			{
				textInput.Visibility = Visibility.Collapsed;
			}

            keySelection.Visibility = selectedInstruction == Instruction.InstructionTypes.SendKeys ? Visibility.Visible : Visibility.Collapsed;
            moleculeSelection.Visibility = selectedInstruction == Instruction.InstructionTypes.Molecule ? Visibility.Visible : Visibility.Collapsed;
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
            else if (instruction.InstructionType == Instruction.InstructionTypes.AssertValue)
            {
                var assert = JsonConvert.DeserializeObject<AssertValueInstruction>(instruction.Payload);
                locator = assert.Locator;
                text = locator.Path;
                inputText = assert.ExpectedValue;
            }
            else if (instruction.InstructionType == Instruction.InstructionTypes.AssertElementExists)
            {
                var assert = JsonConvert.DeserializeObject<AssertElementExistsInstruction>(instruction.Payload);
                locator = assert.Locator;
                text = locator.Path;
            }
            else if (instruction.InstructionType == Instruction.InstructionTypes.SendKeys)
			{
				var sendKeyInstruction = JsonConvert.DeserializeObject<SendKeyInstruction>(instruction.Payload);
				locator = sendKeyInstruction.Locator;
				text = locator.Path;
				keySelection = sendKeyInstruction.Key;
			}
            //else if (instruction.InstructionType == Instruction.InstructionTypes.Molecule)
            //{
            //    //var moleculeInstruction = JsonConvert.DeserializeObject<MoleculeValueInstruction>(instruction.Payload);
            //}
            else if (instruction.InstructionType == Instruction.InstructionTypes.WaitTime)
            {
                var waitTimeInstruction = JsonConvert.DeserializeObject<WaitTimeInstruction>(instruction.Payload);
                text = waitTimeInstruction.waitTime;
            }
            else 
            {
				text = instruction.Payload;
			}

            var displayLocatorSelection = instruction.InstructionType == Instruction.InstructionTypes.Click ||
                                          instruction.InstructionType == Instruction.InstructionTypes.AssertValue ||
                                          instruction.InstructionType == Instruction.InstructionTypes.InputText ||
                                          instruction.InstructionType == Instruction.InstructionTypes.SendKeys ||
                                          instruction.InstructionType == Instruction.InstructionTypes.AssertElementExists;
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
				Visibility = instruction.InstructionType == Instruction.InstructionTypes.InputText || instruction.InstructionType == Instruction.InstructionTypes.AssertValue ? Visibility.Visible : Visibility.Collapsed,
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
					case Instruction.InstructionTypes.AssertValue:
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
                    case Instruction.InstructionTypes.AssertElementExists:
                        xpath = ((TextBox)instructionPanel.Children[2]).Text;
                        locatorType = (Locator.LocatorTypes)((ComboBox)(instructionPanel).Children[1]).SelectedValue;
                        locator = new Locator() { LocatorType = locatorType, Path = xpath };
                        AssertElementExistsInstruction assertElement = new AssertElementExistsInstruction()
                        {
                            Locator = locator,
                        };
                        payload = JsonConvert.SerializeObject(assertElement);
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
                    case Instruction.InstructionTypes.Molecule:
                        var text = ((ComboBox)instructionPanel.Children[5]).Text;
                        payload = text;
                        break;
                    case Instruction.InstructionTypes.WaitTime:
                        text = ((TextBox)instructionPanel.Children[2]).Text;
                        WaitTimeInstruction waitTimeInstruction = new WaitTimeInstruction()
                        {
                            waitTime = text,
                        };
                        payload = JsonConvert.SerializeObject(waitTimeInstruction);
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
            Test.IsMolecule = false;
			GetUpdatedValues();
			Window.GetWindow(this).DialogResult = true;
			Window.GetWindow(this).Close();
		}

        private void SaveAsMolecule_OnClick(object sender, RoutedEventArgs e)
        {
            Test.IsMolecule = true;
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

            var moleculeSelection = GetMoleculeComboBox();
            instructionPanel.Children.Add((moleculeSelection));

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
