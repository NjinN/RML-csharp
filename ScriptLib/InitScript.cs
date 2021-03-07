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

spawn: func [blk /wait /timeout timeout] [
    if timeout = none [timeout: 0]
    _spawn blk wait timeout
]

take*: func [s /at at /part part] [
    if at = none [at: 1]
    if part = none [part: 1]
    _take s at part
]

take: func [s /at at /part part] [
    if at = none [at: 1]
    if part = none [part: 1]
    _take copy/deep s at part
]

append*: func [s t /only] [
    _append s t only
]

append: func [s t /only] [
    _append copy/deep s t only
]

insert*: func [s t /at at /only] [
    if at = none [at: 1]
    _insert s t at only
]

insert: func [s t /at at /only] [
    if at = none [at: 1]
    _insert copy/deep s t at only
]


        








    ";

    }
}
