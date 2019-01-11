using AtomicReader.Objects;
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
		public Screenshot Screenshot;

		public Log(Test test, Instruction instruction, Exception exception, Screenshot screenshot)
		{
			Test = test;
			Instruction = instruction;
			Exception = exception;
			Screenshot = screenshot;
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
