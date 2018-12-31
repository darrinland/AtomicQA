using System.Collections.Generic;

namespace AtomicWriter.Objects
{
	public class SaveObject
	{
		public List<Test> Tests { get; set; }

		public SaveObject()
		{
			Tests = new List<Test>() { };
		}
	}
}
