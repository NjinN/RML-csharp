using System;
using System.Collections.Generic;
using System.Text;

namespace RML.Lang {
    class Rfunc {
        public int argsLen;
        public List<Rtoken> args;
        public List<Rtoken> code;

        public Rfunc(int l, List<Rtoken> a, List<Rtoken> c) {
            argsLen = l;
            args = a;
            code = c;
        }

        public Rtoken Run(List<Rtoken> actArgs, Rtable ctx) {
            Rtable fCtx = new Rtable(Rtable.Type.TMP, ctx);

            int i = 0;
            while(i < argsLen) {
                fCtx.PutNow(args[i].GetWord().key, actArgs[i]);
                i++;
            }

            return new Rsolver(code).Eval(fCtx);
        }

    }
}
