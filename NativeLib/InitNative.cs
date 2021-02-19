using System;
using System.Collections.Generic;
using System.Text;
using RML.Lang;

namespace RML.NativeLib {
    class InitNative {
        public static void Init(Rtable ctx) {
            ctx.PutNow("quit", new Rtoken(Rtype.Native, new Rquit()));
            ctx.PutNow("q", new Rtoken(Rtype.Native, new Rquit()));
            ctx.PutNow("print", new Rtoken(Rtype.Native, new Rprint()));
            ctx.PutNow("cost", new Rtoken(Rtype.Native, new Cost()));

            ctx.PutNow("add", new Rtoken(Rtype.Native, new Add()));
            ctx.PutNow("sub", new Rtoken(Rtype.Native, new Sub()));
            ctx.PutNow("mul", new Rtoken(Rtype.Native, new Mul()));
            ctx.PutNow("div", new Rtoken(Rtype.Native, new Div()));

            ctx.PutNow("eq", new Rtoken(Rtype.Native, new Eq()));
            ctx.PutNow("lt", new Rtoken(Rtype.Native, new Lt()));
            ctx.PutNow("gt", new Rtoken(Rtype.Native, new Gt()));
            ctx.PutNow("le", new Rtoken(Rtype.Native, new Le()));
            ctx.PutNow("ge", new Rtoken(Rtype.Native, new Ge()));

            ctx.PutNow("if", new Rtoken(Rtype.Native, new Rif()));
            ctx.PutNow("either", new Rtoken(Rtype.Native, new Reither()));

            ctx.PutNow("func", new Rtoken(Rtype.Native, new Func()));

        }


    }
}
