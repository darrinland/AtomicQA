using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AtomicWriter.Objects
{
    public class SendKeyInstruction
    {
        public Locator Locator { get; set; }
        public Keys Key { get; set; }

        public static List<Keys> GetSendKeyTypes()
        {
            return new List<Keys>()
            {
                Keys.Enter,
                Keys.Tab,
            };
        }
    }
}
