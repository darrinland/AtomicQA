﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AtomicWriter.Objects;

namespace TestRunner.Objects
{
    public class AssertElementExistsInstruction
    {
        public Locator Locator { get; set; }
        public string ExpectedValue { get; set; }
    }
}
