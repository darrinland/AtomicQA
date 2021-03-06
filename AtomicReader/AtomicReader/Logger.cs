﻿using AtomicReader.Objects;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;

namespace AtomicReader
{
	public class Log
	{
		public Test Test;
		public Instruction Instruction;
		public Exception Exception;
		public string ScreenshotLocation;

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
