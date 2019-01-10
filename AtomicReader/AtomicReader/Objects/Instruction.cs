namespace AtomicReader.Objects
{
	public class Instruction
	{
		public InstructionTypes InstructionType { get; set; }
		public string Payload { get; set; }

		public enum InstructionTypes
		{
			GoToUrl,
			Click,
            Type
		}
	}
}
