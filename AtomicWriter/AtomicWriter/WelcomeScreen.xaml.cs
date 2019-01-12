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
	/// Interaction logic for Window1.xaml
	/// </summary>
	public partial class WelcomeScreen : MetroWindow
	{
		public WelcomeScreen()
		{
			InitializeComponent();
		}

		private void EditorButton_Click(object sender, RoutedEventArgs e)
		{
			var testEditor = new TestEditor();
			testEditor.Show();
			this.Close();
		}

		private void DashboardButton_Click(object sender, RoutedEventArgs e)
		{
			var dashboard = new Dashboard();
			dashboard.Show();
			this.Close();
		}

		private void RunnerButton_Click(object sender, RoutedEventArgs e)
		{
			var runner = new TestsRunner();
			runner.Show();
			this.Close();
		}
	}
}
