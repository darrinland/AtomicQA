using AtomicWriter.Objects;
using Newtonsoft.Json;
using SeleniumBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestRunner.Objects;

namespace TestRunner
{
	public class TestRunner : IDisposable
	{
		private SeleniumBase.SeleniumBase _driver;
		private readonly Logger _logger;

		public TestRunner(Logger logger)
		{
			_driver = new SeleniumBase.SeleniumBase();
			_logger = logger;
		}

		public void Dispose()
		{
			_driver.Dispose();
		}

		public void RunTest(Test test)
		{
			try
			{
				test.Instructions.ForEach(instruction =>
				{
					ExecuteInstruction(test, instruction);
				});
			}
			catch
			{
				return;
			}
		}

		private void ExecuteInstruction(Test test, Instruction instruction)
		{
			try
			{
				switch (instruction.InstructionType)
				{
					case Instruction.InstructionTypes.GoToUrl:
						ExecuteGoToUrl(instruction.Payload);
						break;
					case Instruction.InstructionTypes.Click:
						ExecuteClick(instruction.Payload);
						break;
					case Instruction.InstructionTypes.InputText:
						ExecuteInput(instruction.Payload);
						break;
					case Instruction.InstructionTypes.SendKeys:
						ExecuteSendKeys(instruction.Payload);
						break;
                    case Instruction.InstructionTypes.AssertValue:
                        ExecuteAssertValue(instruction.Payload);
                        break;
                    case Instruction.InstructionTypes.AssertElementExists:
                        ExecuteAssertElementExists(instruction.Payload);
                        break;
                    default:
						Console.Write("InstructionType not Recognized");
						break;
				}
			}
			catch (Exception e)
			{
				try
				{
					_logger.AddLog(new Log(test, instruction, e, _driver.GetScreenshot()));
				}
				catch
				{
					_logger.AddLog(new Log(test, instruction, e, ""));
				}
				finally
				{
					throw e;
				}
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
			var typedTextInstruction = Newtonsoft.Json.JsonConvert.DeserializeObject<TextInputInstruction>(payload);
			var locator = typedTextInstruction.Locator;
			_driver.WaitToInput(Locator.GetByLocator(locator.LocatorType, locator.Path), typedTextInstruction.Text);
		}

		private void ExecuteSendKeys(string payload)
		{
			var sendKeysInstruction = JsonConvert.DeserializeObject<SendKeyInstruction>(payload);
			var locator = sendKeysInstruction.Locator;
			_driver.SendKeys(Locator.GetByLocator(locator.LocatorType, locator.Path), sendKeysInstruction.Key.ToString());
		}

		private void ExecuteAssertValue(string payload)
		{
			var assertValueInstruction = JsonConvert.DeserializeObject<AssertValueInstruction>(payload);
			var locator = assertValueInstruction.Locator;
			var expectedValue = assertValueInstruction.ExpectedValue;
			var actualValue = _driver.WaitToGetText(Locator.GetByLocator(locator.LocatorType, locator.Path));
			if (expectedValue != actualValue)
			{
				throw new Exception("Expected Value(" + expectedValue + ") does not match Actual Value(" + actualValue + ").");
			}
        }
        private void ExecuteAssertElementExists(string payload)
        {
            var assertElementExistsInstruction = JsonConvert.DeserializeObject<AssertElementExistsInstruction>(payload);
            var locator = assertElementExistsInstruction.Locator;
            try
            {
                _driver.WaitFor(Locator.GetByLocator(locator.LocatorType, locator.Path));
            }
            catch
            {
                throw new Exception("Unable to find element By." + locator.LocatorType + " (" + locator.Path + ")");
            }
        }
    }
}
