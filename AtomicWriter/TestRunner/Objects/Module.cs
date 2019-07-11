using System.Collections.Generic;
using AtomicWriter.Objects;

namespace AtomicQA
{
	public class Module
	{
		public string Name { get; set; }
		public List<Requirement> Requirements { get; set; }
	}

	public class Requirement
	{
		public string Title { get; set; }
		public string Details { get; set; }
		public List<Test> TestCases { get; set; }
	}
}