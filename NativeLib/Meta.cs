using System;
using System.Collections.Generic;
using System.Text;
using RML.Lang;

namespace RML.NativeLib {
    [Serializable]
    class Rdo : Rnative {
        public Rdo() {
            name = "_do";
            argsLen = 2;
        }

        public override Rtoken Run(List<Rtoken> args, Rtable ctx) {
            bool with = false;
            if (args[1].tp.Equals(Rtype.Object)) {
                ctx = args[1].GetTable();
                with = true;
            } else {
                if (!args[1].tp.Equals(Rtype.None)) {
                    return ErrorInfo(args);
                }
            }

            if (args[0].tp.Equals(Rtype.Block)) {
                if (with) {
                    List<Rtoken> wordList = new List<Rtoken>();
                    foreach(var k in ctx.table.Keys) {
                        wordList.Add(new Rtoken(Rtype.Word, new Rword(k)));
                    }
                    RtokenKit.ClearCtxForWordByWords(wordList, args[0].GetList());
                }

                return new Rsolver(args[0].GetList()).Eval(ctx);
            }else if (args[0].tp.Equals(Rtype.Str)) {
                return new Rsolver(args[0].GetStr()).Eval(ctx);
            }
            return ErrorInfo(args);
        }
    }


    [Serializable]
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


    [Serializable]
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


    [Serializable]
    class RtypeOf : Rnative {
        public RtypeOf() {
            name = "type?";
            argsLen = 1;
        }

        public override Rtoken Run(List<Rtoken> args, Rtable ctx) {
            return new Rtoken(Rtype.Datatype, args[0].tp);
        }
    }


    [Serializable]
    class Rcopy : Rnative {
        public Rcopy() {
            name = "_copy";
            argsLen = 2;
        }

        public override Rtoken Run(List<Rtoken> args, Rtable ctx) {
            if (args[1].tp.Equals(Rtype.Bool)) {
                if (args[1].GetBool()) {
                    return args[0].CopyDeep();
                } else {
                    return args[0].Copy();
                }
            }

            return ErrorInfo(args);
        }
    }


    [Serializable]
    class Rkeep : Rnative {
        public Rkeep() {
            name = "keep";
            argsLen = 1;
        }

        public override Rtoken Run(List<Rtoken> args, Rtable ctx) {
            Rtoken result = ctx.GetNow("__collect_result__");
            if (result.tp.Equals(Rtype.Nil)) {
                result = new Rtoken(Rtype.Block, new List<Rtoken>());
                ctx.PutNow("__collect_result__", result);
            }
            result.GetList().Add(args[0]);
            return args[0];
        }
    }

    [Serializable]
    class Rcollect : Rnative {
        public Rcollect() {
            name = "_collect";
            argsLen = 2;
        }

        public override Rtoken Run(List<Rtoken> args, Rtable ctx) {
            bool with = false;
            if (args[1].tp.Equals(Rtype.Object)) {
                ctx = args[1].GetTable();
                with = true;
            } else {
                if (!args[1].tp.Equals(Rtype.None)) {
                    return ErrorInfo(args);
                }
            }

            Rtable cctx = new Rtable(Rtable.Type.TMP, ctx);
            cctx.PutNow("keep", new Rtoken(Rtype.Native, new Rkeep()));
            Rtoken result = new Rtoken(Rtype.Block, new List<Rtoken>());
            cctx.PutNow("__collect_result__", result);

            if (args[0].tp.Equals(Rtype.Block)) {
                List<Rtoken> wordList = new List<Rtoken>();
                wordList.Add(new Rtoken(Rtype.Word, new Rword("keep")));
                wordList.Add(new Rtoken(Rtype.Word, new Rword("__collect_result__")));

                if (with) {
                    foreach (var k in ctx.table.Keys) {
                        wordList.Add(new Rtoken(Rtype.Word, new Rword(k)));
                    }
                }
                RtokenKit.ClearCtxForWordByWords(wordList, args[0].GetList());

                new Rsolver(args[0].GetList()).Eval(cctx);
            } else if (args[0].tp.Equals(Rtype.Str)) {
                new Rsolver(args[0].GetStr()).Eval(cctx);
            }
            cctx.RemoveNow("__collect_result__");
            return result;
        }
    }


}
