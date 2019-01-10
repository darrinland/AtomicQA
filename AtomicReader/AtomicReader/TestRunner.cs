using AtomicReader.Objects;
using OpenQA.Selenium;
using System;

namespace AtomicReader
{
	public class TestRunner : IDisposable
	{
		private SeleniumBase.SeleniumBase _driver;

		public TestRunner()
		{
			_driver = new SeleniumBase.SeleniumBase();
		}

		public void Dispose()
		{
			_driver.Dispose();
		}

		public void RunTest(Test test)
		{
			test.Instructions.ForEach(instruction =>
			{
				ExecuteInstruction(instruction);
			});
		}
		
		private void ExecuteInstruction(Instruction instruction)
		{
			switch (instruction.InstructionType)
			{
				case Instruction.InstructionTypes.GoToUrl:
					ExecuteGoToUrl(instruction.Payload);
					break;
				case Instruction.InstructionTypes.Click:
					ExecuteClick(instruction.Payload);
					break;
				default:
					Console.Write("InstructionType not Recognized");
					break;
			}
		}

		private void ExecuteGoToUrl(string url)
		{
			_driver.NavigateToUrl(url);
		}

		private void ExecuteClick(string payload)
		{
			var locator = Newtonsoft.Json.JsonConvert.DeserializeObject<Locator>(payload);
			_driver.WaitToClick(Locator.GetByLocator(locator.LocatorType, locator.Path));
		}
	}
}
