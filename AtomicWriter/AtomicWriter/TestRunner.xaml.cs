using MahApps.Metro.Controls;
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
	/// Interaction logic for TestRunner.xaml
	/// </summary>
	public partial class TestRunner : MetroWindow
	{
		public TestRunner()
		{
			InitializeComponent();
		}

		private void BackToWelcomeScreenButton_Click(object sender, RoutedEventArgs e)
		{
			var projectSelection = new WelcomeScreen();
			projectSelection.Show();
			this.Close();
		}
	}
}
