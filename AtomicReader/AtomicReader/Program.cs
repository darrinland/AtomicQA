using AtomicReader.DataAccess;
using AtomicReader.Objects;
using System;
using System.Windows.Forms;

namespace AtomicReader
{
	class Program
	{
		private static SeleniumBase.SeleniumBase _driver;

		[STAThread]
		static void Main(string[] args)
		{
			Console.WriteLine("FileLocation:");
			var location = @"C:/Tests/test.json";

			var b = new OpenFileDialog
			{
				FileName = location
			};

			if (b.ShowDialog() == DialogResult.OK)
			{
				location = b.FileName;
			}

			//do
			//{
			//	location = Console.ReadLine();
			//} while (!DataReader.FileExists(location));

			//var save = new SaveObject()
			//{
			//	Tests = new List<Test>()
			//	{
			//		new Test()
			//		{
			//			Instructions = new List<Instruction>()
			//			{
			//				new Instruction()
			//				{
			//					Payload = "https://www.google.com"
			//				}
			//			}
			//		}
			//	}
			//};
			//var writer = new DataWriter();
			//writer.Save(location, save);

			var save = DataReader.LoadObject(location);
			save.Tests.ForEach(test =>
			{
				RunTest(test);
			});
		}

		public static void RunTest(Test test)
		{
			_driver = new SeleniumBase.SeleniumBase();
			test.Instructions.ForEach(instruction =>
			{
				ExecuteInstruction(instruction);
			});
			_driver.Dispose();
		}

		public static void ExecuteGoToUrl(string url)
		{
			_driver.NavigateToUrl(url);
		}

		public static void ExecuteInstruction(Instruction instruction)
		{
			switch (instruction.InstructionType)
			{
				case Instruction.InstructionTypes.GoToUrl:
					ExecuteGoToUrl(instruction.Payload);
					break;
				default:
					Console.Write("InstructionType not Recognized");
					break;
			}
		}
	}
}
