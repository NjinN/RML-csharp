using System;
using System.Collections.Generic;
using System.Text;
using RML.Lang;

namespace RML.NativeLib {
    class Rdo : Rnative {
        public Rdo() {
            name = "do";
            argsLen = 1;
        }

        public override Rtoken Run(List<Rtoken> args, Rtable ctx) {
            if (args[0].tp.Equals(Rtype.Block)) {
                return new Rsolver(args[0].GetList()).Eval(ctx);
            }else if (args[0].tp.Equals(Rtype.Str)) {
                return new Rsolver(args[0].GetStr()).Eval(ctx);
            }
            return ErrorInfo(args);
        }
    }


    class Rreduce : Rnative {
        public Rreduce() {
            name = "do";
            argsLen = 1;
        }

        public override Rtoken Run(List<Rtoken> args, Rtable ctx) {
            if (args[0].tp.Equals(Rtype.Block)) {
                List<Rtoken> list =  new Rsolver(args[0].GetList()).Reduce(ctx);
                return new Rtoken(Rtype.Block, list);
            } else if (args[0].tp.Equals(Rtype.Str)) {
                List<Rtoken> list = new Rsolver(args[0].GetStr()).Reduce(ctx);
                return new Rtoken(Rtype.Block, list);
            }
            return ErrorInfo(args);
        }
    }


    class Rcompose : Rnative {
        public Rcompose() {
            name = "compose";
            argsLen = 1;
        }

        public override Rtoken Run(List<Rtoken> args, Rtable ctx) {
            if (args[0].tp.Equals(Rtype.Block)) {
                return new Rtoken(Rtype.Block, Rsolver.ComposeDeep(args[0].GetList(), ctx));
            }
            return ErrorInfo(args);
        }
    }

}
