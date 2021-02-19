using System;
using System.Collections.Generic;
using System.Text;
using RML.Lang;

namespace RML.NativeLib {
    class Rquit : Rnative {
        public Rquit() {
            name = "quit";
            argsLen = 0;
        }

        public override Rtoken Run(List<Rtoken> args, Rtable ctx) {
            Environment.Exit(0);
            return new Rtoken();
        }
    }


    class Rprint : Rnative {
        public Rprint() {
            name = "print";
            argsLen = 1;
        }

        public override Rtoken Run(List<Rtoken> args, Rtable ctx) {
            args[0].Echo();
            return new Rtoken();
        }
    }


    class Cost : Rnative {
        public Cost() {
            name = "cost";
            argsLen = 1;
        }

        public override Rtoken Run(List<Rtoken> args, Rtable ctx) {
            long start = DateTime.Now.ToUniversalTime().Ticks;
            if (args[0].tp.Equals(Rtype.Block)) {
                new Rsolver(args[0].GetList()).Eval(ctx);
                long end = DateTime.Now.ToUniversalTime().Ticks;
                return new Rtoken(Rtype.Float, Convert.ToDouble(end - start) / 10000000);
            } else if(args[0].tp.Equals(Rtype.Str)) {
                new Rsolver(args[0].GetStr()).Eval(ctx);
                long end = DateTime.Now.ToUniversalTime().Ticks;
                return new Rtoken(Rtype.Float, Convert.ToDouble(end - start) / 10000000);
            } else {
                return new Rtoken(Rtype.Err, "Error: Types mismatch for native::cost");
            }
        }
    }



}
