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

namespace AtomicQA
{
	/// <summary>
	/// Interaction logic for Name_Prompt.xaml
	/// </summary>
	public partial class NamePrompt : Window
	{
		public string NewName { get; set; }

		public NamePrompt()
		{
			InitializeComponent();
		}

		public void Save(object sender, RoutedEventArgs e)
		{
			this.DialogResult = true;
			this.NewName = NameInputBox.Text;
			this.Close();
		}
		
		public void Cancel(object sender, RoutedEventArgs e)
		{
			this.DialogResult = false;
			this.Close();
		}
	}
}
