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
        File,
        Block,
        Paren,
        Object,
        Flow,
        Word,
        Path,
        Prop,
        GetWord,
        LitWord,
        SetWord,
        SetPath,
        Func,
        Native,
        Op,
        Undefined,
    }
}
