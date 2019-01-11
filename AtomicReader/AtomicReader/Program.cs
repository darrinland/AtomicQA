using AtomicReader.DataAccess;
using AtomicReader.Objects;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AtomicReader
{
	class Program
	{
		private static string _location = @"C:/Tests/test.json";
		private static string _logLocation = @"C:/Tests/log.json";

		static void Main(string[] args)
		{
			RunTestFile();
		}

		private static void RunTestFile()
		{
			var save = DataReader.LoadObject<SaveObject>(_location);
			var logger = new Logger();
			save.Tests.ForEach(test =>
			{
				using (var testRunner = new TestRunner(logger))
				{
					testRunner.RunTest(test);
				}
			});

			DataWriter.Save(_logLocation, logger.GetLogs());
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
