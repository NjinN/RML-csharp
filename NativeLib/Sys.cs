using System;
using System.Collections.Generic;
using System.Text;
using RML.Lang;

namespace RML.NativeLib {
    [Serializable]
    class Rquit : Rnative {
        public Rquit() {
            name = "_quit";
            argsLen = 1;
        }

        public override Rtoken Run(List<Rtoken> args, Rtable ctx) {
            if (args[0].tp.Equals(Rtype.Int)){
                Environment.Exit(args[0].GetInt());
            }
            return ErrorInfo(args);
        }
    }


    [Serializable]
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



    [Serializable]
    class Rclear : Rnative {
        public Rclear() {
            name = "clear";
            argsLen = 0;
        }

        public override Rtoken Run(List<Rtoken> args, Rtable ctx) {
            Console.Clear();
            return new Rtoken();
        }
    }


    [Serializable]
    class Cost : Rnative {
        public Cost() {
            name = "cost";
            argsLen = 1;
        }

        public override Rtoken Run(List<Rtoken> args, Rtable ctx) {
            long start = DateTime.Now.ToUniversalTime().Ticks;
            if (args[0].tp.Equals(Rtype.Block)) {
                Rtoken ans = new Rsolver(args[0].GetList()).Eval(ctx);
                if (ans.tp.Equals(Rtype.Err)) {
                    return ans;
                }
                long end = DateTime.Now.ToUniversalTime().Ticks;
                return new Rtoken(Rtype.Float, Convert.ToDecimal(end - start) / 10000000);
            } else if(args[0].tp.Equals(Rtype.Str)) {
                Rtoken ans = new Rsolver(args[0].GetStr()).Eval(ctx);
                if (ans.tp.Equals(Rtype.Err)) {
                    return ans;
                }
                long end = DateTime.Now.ToUniversalTime().Ticks;
                return new Rtoken(Rtype.Float, Convert.ToDecimal(end - start) / 10000000);
            } else {
                return ErrorInfo(args);
            }
        }
    }



}
