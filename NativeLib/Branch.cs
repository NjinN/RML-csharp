using System;
using System.Collections.Generic;
using System.Text;
using RML.Lang;

namespace RML.NativeLib {


    class Rif : Rnative {
        public Rif() {
            name = "if";
            argsLen = 2;
        }

        public override Rtoken Run(List<Rtoken> args, Rtable ctx) {
            return (args[0].ToBool(), args[1].tp) switch {
                (true, Rtype.Block) => new Rsolver(args[1].GetList()).Eval(ctx),
                (true, Rtype.Str) => new Rsolver(args[1].GetStr()).Eval(ctx),
                (false, _) => new Rtoken(Rtype.Flow, new Rflow("pass")),
                (_, _) => ErrorInfo(args)
            };
        }
    }

    class Relif : Rnative {
        public Relif() {
            name = "elif";
            argsLen = 3;
        }

        public override Rtoken Run(List<Rtoken> args, Rtable ctx) {
            return args[0].tp switch {
                Rtype.Flow => (args[0].GetFlow().name, args[1].ToBool(), args[2].tp) switch {
                    ("pass", true, Rtype.Block) => new Rsolver(args[2].GetList()).Eval(ctx),
                    ("pass", true, Rtype.Str) => new Rsolver(args[2].GetStr()).Eval(ctx),
                    ("pass", false, _) => new Rtoken(Rtype.Flow, new Rflow("pass")),
                    (_, _, _) => ErrorInfo(args)
                },

                _ => ErrorInfo(args)
            };

        }
    }


    class Relse : Rnative {
        public Relse() {
            name = "else";
            argsLen = 2;
        }

        public override Rtoken Run(List<Rtoken> args, Rtable ctx) {
            return args[0].tp switch {
                Rtype.Flow => (args[0].GetFlow().name, args[1].tp) switch {
                    ("pass", Rtype.Block) => new Rsolver(args[1].GetList()).Eval(ctx),
                    ("pass", Rtype.Str) => new Rsolver(args[1].GetStr()).Eval(ctx),
                    (_, _) => ErrorInfo(args)
                },
                _ => ErrorInfo(args)
            };

        }
    }


    class Reither : Rnative {
        public Reither() {
            name = "either";
            argsLen = 3;
        }

        public override Rtoken Run(List<Rtoken> args, Rtable ctx) {
            return (args[0].ToBool(), args[1].tp, args[2].tp) switch {
                (true, Rtype.Block, _) => new Rsolver(args[1].GetList()).Eval(ctx),
                (true, Rtype.Str, _) => new Rsolver(args[1].GetStr()).Eval(ctx),
                (false, _, Rtype.Block) => new Rsolver(args[2].GetList()).Eval(ctx),
                (false, _, Rtype.Str) => new Rsolver(args[2].GetStr()).Eval(ctx),
                (_, _, _) => ErrorInfo(args)
            };

        }
    }

}
