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
			save.Tests.ForEach(test =>
			{
				using (var testRunner = new TestRunner())
				{
					testRunner.RunTest(test);
				}
			});
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
