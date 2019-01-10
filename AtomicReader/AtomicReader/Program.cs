using AtomicReader.DataAccess;
using System;
using System.Windows.Forms;

namespace AtomicReader
{
	class Program
	{
		private static string _location = @"C:/Tests/test.json";

		static void Main(string[] args)
		{
			RunTestFile();
		}

		private static void RunTestFile()
		{
			var save = DataReader.LoadObject(_location);
			var logger = new Logger();
			save.Tests.ForEach(test =>
			{
				using (var testRunner = new TestRunner(logger))
				{
					testRunner.RunTest(test);
				}
			});

			// TODO: Need to save logs somewhere for use in dashboard
		}

		[STAThread]
		private static void GetTestFile()
		{
			var b = new OpenFileDialog
			{
				FileName = _location
			};

			if (b.ShowDialog() == DialogResult.OK)
			{
				_location = b.FileName;
			}
		}
	}
}
