using AtomicReader.Objects;
using System;

namespace AtomicReader
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
					case Instruction.InstructionTypes.Type:
						ExecuteInput(instruction.Payload);
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
					_logger.AddLog(new Log(test, instruction, e, null));
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
			var locator = Newtonsoft.Json.JsonConvert.DeserializeObject<Locator>(payload);
			_driver.WaitToClick(Locator.GetByLocator(locator.LocatorType, locator.Path));
		}

		private void ExecuteInput(string payload)
		{
			var typedTextInstruction = Newtonsoft.Json.JsonConvert.DeserializeObject<TypedTextInput>(payload);
			var locator = typedTextInstruction.Locator;
			_driver.Input(Locator.GetByLocator(locator.LocatorType, locator.Path), typedTextInstruction.Text);
		}
	}
}
