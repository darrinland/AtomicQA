using AtomicReader.Objects;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtomicReader
{
	public class Log
	{
		private readonly Test _test;
		private readonly Instruction _instruction;
		private readonly Exception _exception;
		private readonly Screenshot _screenshot;

		public Log(Test test, Instruction instruction, Exception exception, Screenshot screenshot)
		{
			_test = test;
			_instruction = instruction;
			_exception = exception;
			_screenshot = screenshot;
		}
	}

	public class Logger
	{
		private List<Log> _logs = new List<Log>() { };

		public void AddLog(Log log)
		{
			_logs.Add(log);
		}
	}
}
