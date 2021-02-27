using System;
using System.Collections.Generic;
using System.Text;
using RML.Lang;

namespace RML.NativeLib {
    class Func : Rnative {
        public Func() {
            name = "func";
            argsLen = 2;
        }

        public override Rtoken Run(List<Rtoken> args, Rtable ctx) {
            Rtype type0 = args[0].tp;
            Rtype type1 = args[1].tp;

            if(type0.Equals(Rtype.Block) && type1.Equals(Rtype.Block)) {
                List<Rtoken> argsBlk = args[0].GetList();
                List<Rtoken> codeBlk = args[1].GetList();

                RtokenKit.ClearCtxForWordByWords(argsBlk, codeBlk);

                return new Rtoken(Rtype.Func, new Rfunc(argsBlk.Count, argsBlk, codeBlk));

            }

            return ErrorInfo(args);
        }
    }



    class Rreturn : Rnative {
        public Rreturn() {
            name = "return";
            argsLen = 1;
        }

        public override Rtoken Run(List<Rtoken> args, Rtable ctx) {
            return new Rtoken(Rtype.Flow, new Rflow("return", args[0]));
        }
    }
}
