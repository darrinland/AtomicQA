﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtomicReader.Objects
{
    public class AssertValueInstruction
    {
        public Locator Locator { get; set; }
        public string ExpectedValue { get; set; }
    }
}
