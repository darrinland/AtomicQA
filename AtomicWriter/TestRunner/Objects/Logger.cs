using Newtonsoft.Json;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;

namespace AtomicWriter.Objects
{
	public class Log
	{
		public Test Test;
		public Instruction Instruction;
		public Exception Exception;
		public string ScreenshotLocation;

		[JsonConstructor]
		public Log(Test test, Instruction instruction, Exception exception, string screenshotLocation)
		{
			Test = test;
			Instruction = instruction;
			Exception = exception;
			ScreenshotLocation = screenshotLocation;
		}

		public Log(Test test, Instruction instruction, Exception exception, Screenshot screenshot)
		{
			Test = test;
			Instruction = instruction;
			Exception = exception;
			ScreenshotLocation = GenerateScreenshot(screenshot, test);
		}

		private string GenerateScreenshot(Screenshot screenshot, Test test)
		{
			var fileLocation = $"C:\\Tests\\{test.TestName}.png";
			screenshot.SaveAsFile($"C:\\Tests\\{test.TestName}.png", ScreenshotImageFormat.Png);
			return fileLocation;
		}
	}

	public class Logger
	{
		private List<Log> _logs = new List<Log>() { };

		public void AddLog(Log log)
		{
			_logs.Add(log);
		}

		public List<Log> GetLogs()
		{
			return _logs;
		}
	}
}
