using System.Collections.Generic;

namespace AtomicReader.Objects
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
