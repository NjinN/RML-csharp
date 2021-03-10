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

print: func [t /inline] [
    _print t inline
]

ask: func [msg /hide] [
    _ask msg hide
]

do: func [blk /with with] [
    _do blk with
]

copy: func [source /deep] [
    _copy source deep
]

collect: func [blk /with with] [
    _collect blk with
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

at*: func [s idx] [
    _at s idx
]
        
at: func [s idx] [
    _at copy/deep s idx
]

index: func [s t /at at /last] [
    if at = none [at: 1]
    _index s t at last
]

find*: func [s t /at at /last] [
    if at = none [at: 1]
    _find s t at last
]

find: func [s t /at at /last] [
    if at = none [at: 1]
    _find copy/deep s t at last
]

replace*: func [s t n /at at /times times /last /all /only] [
    if at = none [at: 1]
    if times = none [times: 1]
    if all [times: 99999999]
    _replace s t n at last times only
]

replace: func [s t n /at at /times times /last /all /only] [
    if at = none [at: 1]
    if times = none [times: 1]
    if all [times: 99999999]
    _replace copy/deep s t n at last times only
]

read: func [t /str /local tp] [
    tp: bin!
    if str [tp: str!]
    _read t tp
]

write: func [t d /append] [
    _write t d append
]

fcopy: func [f1 f2 /rewrite] [
    if rewrite = none [rewrite: false]
    _fcopy f1 f2 rewrite
]

fls: func [/dir dir] [
    if dir = none [dir: fcwd]
    _fls dir
]




    ";

    }
}
