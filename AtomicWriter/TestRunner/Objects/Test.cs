using System.Collections.Generic;

namespace AtomicWriter.Objects
{
	public class Test
	{
		public string TestName { get; set; }
		public List<Instruction> Instructions { get; set; }
        public bool IsMolecule { get; set; }
	}
}
