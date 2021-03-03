using System;
using System.Collections.Generic;
using System.Text;
using RML.Lang;

namespace RML.NativeLib {
    class InitNative {
        public static void Init(Rtable ctx) {
            ctx.PutNow("do", new Rtoken(Rtype.Native, new Rdo()));
            ctx.PutNow("reduce", new Rtoken(Rtype.Native, new Rreduce()));
            ctx.PutNow("compose", new Rtoken(Rtype.Native, new Rcompose()));
            ctx.PutNow("type?", new Rtoken(Rtype.Native, new RtypeOf()));
            ctx.PutNow("_copy", new Rtoken(Rtype.Native, new Rcopy()));


            ctx.PutNow("_quit", new Rtoken(Rtype.Native, new Rquit()));
            ctx.PutNow("print", new Rtoken(Rtype.Native, new Rprint()));
            ctx.PutNow("clear", new Rtoken(Rtype.Native, new Rclear()));
            ctx.PutNow("cost", new Rtoken(Rtype.Native, new Cost()));

            ctx.PutNow("add", new Rtoken(Rtype.Native, new Add()));
            ctx.PutNow("sub", new Rtoken(Rtype.Native, new Sub()));
            ctx.PutNow("mul", new Rtoken(Rtype.Native, new Mul()));
            ctx.PutNow("div", new Rtoken(Rtype.Native, new Div()));
            ctx.PutNow("mod", new Rtoken(Rtype.Native, new Mod()));
            ctx.PutNow("add-set", new Rtoken(Rtype.Native, new AddSet()));
            ctx.PutNow("sub-set", new Rtoken(Rtype.Native, new SubSet()));
            ctx.PutNow("mul-set", new Rtoken(Rtype.Native, new MulSet()));
            ctx.PutNow("div-set", new Rtoken(Rtype.Native, new DivSet()));
            ctx.PutNow("mod-set", new Rtoken(Rtype.Native, new ModSet()));

            ctx.PutNow("eq", new Rtoken(Rtype.Native, new Eq()));
            ctx.PutNow("lt", new Rtoken(Rtype.Native, new Lt()));
            ctx.PutNow("gt", new Rtoken(Rtype.Native, new Gt()));
            ctx.PutNow("le", new Rtoken(Rtype.Native, new Le()));
            ctx.PutNow("ge", new Rtoken(Rtype.Native, new Ge()));

            ctx.PutNow("if", new Rtoken(Rtype.Native, new Rif()));
            ctx.PutNow("either", new Rtoken(Rtype.Native, new Reither()));

            ctx.PutNow("func", new Rtoken(Rtype.Native, new Func()));
            ctx.PutNow("return", new Rtoken(Rtype.Native, new Rreturn()));

            ctx.PutNow("break", new Rtoken(Rtype.Native, new Rbreak()));
            ctx.PutNow("continue", new Rtoken(Rtype.Native, new Rcontinue()));
            ctx.PutNow("loop", new Rtoken(Rtype.Native, new Rloop()));
            ctx.PutNow("repeat", new Rtoken(Rtype.Native, new Repeat()));
            ctx.PutNow("for", new Rtoken(Rtype.Native, new Rfor()));

            ctx.PutNow("fork", new Rtoken(Rtype.Native, new Rfork()));
            ctx.PutNow("spawn", new Rtoken(Rtype.Native, new Rspawn()));
            ctx.PutNow("lock", new Rtoken(Rtype.Native, new Rlock()));

            ctx.PutNow("to", new Rtoken(Rtype.Native, new Rto()));

            ctx.PutNow("len?", new Rtoken(Rtype.Native, new Rlen()));

            ctx.PutNow("attempt", new Rtoken(Rtype.Native, new Rattempt()));
            ctx.PutNow("try", new Rtoken(Rtype.Native, new Rtry()));

        }


    }
}
