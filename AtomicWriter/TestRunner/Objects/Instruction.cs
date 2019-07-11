using System.Collections.Generic;

namespace AtomicWriter.Objects
{
	public class Instruction
	{
		public InstructionTypes InstructionType { get; set; }
		public string Payload { get; set; }

		public enum InstructionTypes
		{
			GoToUrl,
			Click,
            InputText,
            AssertValue,
            SendKeys,
            AssertElementExists,
        }

		public static List<InstructionTypes> GetInstructionTypes()
		{
			return new List<InstructionTypes>()
			{
				InstructionTypes.GoToUrl,
				InstructionTypes.Click,
                InstructionTypes.InputText,
                InstructionTypes.AssertValue,
                InstructionTypes.SendKeys,
                InstructionTypes.AssertElementExists,
            };
		}
	}
}
