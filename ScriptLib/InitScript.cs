using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RML.ScriptLib {
    class InitScript {
        public static string script = @"

quit: func [/code code] [
    if code [ _quit code]
    _quit 0
]

q: :quit

copy: func [source /deep] [
    _copy source deep
]











        








    ";

    }
}
