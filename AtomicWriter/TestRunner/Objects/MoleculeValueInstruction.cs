using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AtomicWriter.Objects;

namespace TestRunner.Objects
{
    public class MoleculeValueInstruction
    {
        public string moleculeName { get; set; }
        public Test molecule { get; set; }
    }
}
