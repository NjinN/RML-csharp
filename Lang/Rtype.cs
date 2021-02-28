using System;
using System.Collections.Generic;
using System.Text;

namespace RML.Lang {
    enum Rtype {
        Nil,
        None,
        Err,
        Bool,
        Byte,
        Char,
        Int,
        Float,
        Str,
        Block,
        Paren,
        Object,
        Flow,
        Word,
        Path,
        LitWord,
        SetWord,
        SetPath,
        Func,
        Native,
        Op,
        Undefined,
    }
}
