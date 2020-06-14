using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XdtPlayground.Monaco.Interop
{
    public class MinimapOptions
    {
        public bool Enabled { get; set; } = true;
        public int MaxColumn { get; set; } = 120;
        public bool RenderCharacters { get; set; } = true;
    }
}
