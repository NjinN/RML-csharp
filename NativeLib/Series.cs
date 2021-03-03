using System;
using System.Collections.Generic;
using System.Text;
using RML.Lang;

namespace RML.NativeLib {
    class Rlen : Rnative {
        public Rlen() {
            name = "len?";
            argsLen = 1;
        }

        public override Rtoken Run(List<Rtoken> args, Rtable ctx) {
            switch (args[0].tp) {
                case Rtype.Str:
                    return new Rtoken(Rtype.Int, args[0].GetStr().Length);
                case Rtype.Block:
                case Rtype.Paren:
                    return new Rtoken(Rtype.Int, args[0].GetList().Count);
                case Rtype.Object:
                    return new Rtoken(Rtype.Int, args[0].GetTable().table.Count);

                default:
                    return ErrorInfo(args);
            }
        }
    }


   



}
