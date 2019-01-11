using AtomicReader.Objects;
using Newtonsoft.Json;
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
                case Instruction.InstructionTypes.Type:
                    ExecuteInput(instruction.Payload);
                    break;
                case Instruction.InstructionTypes.Assert:
                    ExecuteAssertValue(instruction.Payload);
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
			var locator = JsonConvert.DeserializeObject<Locator>(payload);
			_driver.WaitToClick(Locator.GetByLocator(locator.LocatorType, locator.Path));
		}

        private void ExecuteInput(string payload)
        {
            var typedTextInstruction = JsonConvert.DeserializeObject<TypedTextInput>(payload);
            var locator = typedTextInstruction.Locator;
            _driver.Input(Locator.GetByLocator(locator.LocatorType, locator.Path), typedTextInstruction.Text);
        }

        private void ExecuteAssertValue(string payload)
        {
            var assertValueInstruction = JsonConvert.DeserializeObject<AssertValue>(payload);
            var locator = assertValueInstruction.Locator;
            var expectedValue = assertValueInstruction.ExpectedValue;
            var actualValue =_driver.WaitToGetText(Locator.GetByLocator(locator.LocatorType, locator.Path));
            if (expectedValue != actualValue)
            {
                throw new Exception("Expected Value (" + expectedValue + ") does not match Actual Value (" + actualValue + ").");
            }
        }
	}
}
