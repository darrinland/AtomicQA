using AtomicWriter.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
		}

		private void SaveButton_Click(object sender, RoutedEventArgs e)
		{
			GetUpdatedValues();
			Window.GetWindow(this).DialogResult = true;
			Window.GetWindow(this).Close();
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
			instructionPanel.Children.Add(new TextBox() {});

			InstructionsList.Children.Add(instructionPanel);

		}
	}
}
