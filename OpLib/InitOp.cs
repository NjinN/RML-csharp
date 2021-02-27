using System;
using System.Collections.Generic;
using System.Text;
using RML.Lang;
using RML.NativeLib;

namespace RML.OpLib {
    class InitOp {
        public static void Init(Rtable ctx) {
            ctx.PutNow("+", new Rtoken(Rtype.Op, new Add()));
            ctx.PutNow("-", new Rtoken(Rtype.Op, new Sub()));
            ctx.PutNow("*", new Rtoken(Rtype.Op, new Mul()));
            ctx.PutNow("/", new Rtoken(Rtype.Op, new Div()));
            ctx.PutNow("%", new Rtoken(Rtype.Op, new Mod()));

            ctx.PutNow("=", new Rtoken(Rtype.Op, new Eq()));
            ctx.PutNow("<", new Rtoken(Rtype.Op, new Lt()));
            ctx.PutNow(">", new Rtoken(Rtype.Op, new Gt()));
            ctx.PutNow("<=", new Rtoken(Rtype.Op, new Le()));
            ctx.PutNow(">=", new Rtoken(Rtype.Op, new Ge()));

            ctx.PutNow("elif", new Rtoken(Rtype.Op, new Relif()));
            ctx.PutNow("else", new Rtoken(Rtype.Op, new Relse()));
        }


    }
}
