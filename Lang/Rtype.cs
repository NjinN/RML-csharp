using System;
using System.Collections.Generic;
using System.Text;

namespace RML.Lang {
    enum Rtype {
        Nil,
        None,
        Err,
        Datatype,
        Bool,
        Byte,
        Char,
        Int,
        Float,
        Str,
        Bin,
        File,
        Block,
        Paren,
        Object,
        Flow,
        Word,
        Path,
        Prop,
        Proc,
        CallProc,
        GetWord,
        LitWord,
        SetWord,
        SetPath,
        SetProc,
        Func,
        Native,
        Op,
        Undefined,
    }
}
