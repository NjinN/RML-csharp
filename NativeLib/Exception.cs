using System;
using System.Collections.Generic;
using System.Text;
using RML.Lang;

namespace RML.NativeLib {
    class Rattempt : Rnative {
        public Rattempt() {
            name = "attempt";
            argsLen = 1;
        }

        public override Rtoken Run(List<Rtoken> args, Rtable ctx) {
            Rtoken ans;
            if (args[0].tp.Equals(Rtype.Block)) {
                ans = new Rsolver(args[0].GetList()).Eval(ctx);
                if (ans.tp.Equals(Rtype.Err)) {
                    return new Rtoken(Rtype.None, 0);
                } else {
                    return ans;
                }
            }else if (args[0].tp.Equals(Rtype.Str)) {
                ans = new Rsolver(args[0].GetStr()).Eval(ctx);
                if (ans.tp.Equals(Rtype.Err)) {
                    return new Rtoken(Rtype.None, 0);
                } else {
                    return ans;
                }
            }

            return ErrorInfo(args);
        }

    }


    class Rtry : Rnative {
        public Rtry() {
            name = "try";
            argsLen = 1;
        }

        public override Rtoken Run(List<Rtoken> args, Rtable ctx) {
            Rtoken ans;
            if (args[0].tp.Equals(Rtype.Block)) {
                ans = new Rsolver(args[0].GetList()).Eval(ctx);
                if (ans.tp.Equals(Rtype.Err)) {
                    return new Rtoken(Rtype.Flow, new Rflow("pass", ans));
                } else {
                    return ans;
                }
            } else if (args[0].tp.Equals(Rtype.Str)) {
                ans = new Rsolver(args[0].GetStr()).Eval(ctx);
                if (ans.tp.Equals(Rtype.Err)) {
                    return new Rtoken(Rtype.Flow, new Rflow("pass", ans));
                } else {
                    return ans;
                }
            }

            return ErrorInfo(args);
        }

    }


    class Rcatch : Rnative {
        public Rcatch() {
            name = "catch";
            argsLen = 3;
            quoteList = new List<bool> {false, true, false };
        }

        public override Rtoken Run(List<Rtoken> args, Rtable ctx) {
            if (args[0].tp.Equals(Rtype.Flow)) {
                Rflow flow = args[0].GetFlow();
                if(flow.name.Equals("pass") && null != flow.val && flow.val.tp.Equals(Rtype.Err)) {
                    if (args[1].tp.Equals(Rtype.Word) && (args[2].tp.Equals(Rtype.Block) || args[2].tp.Equals(Rtype.Str))) {
                        Rtable cctx = new Rtable(Rtable.Type.TMP, ctx);
                        cctx.PutNow(args[1].GetWord().key, flow.val);
                        if (args[2].tp.Equals(Rtype.Block)) {
                            return new Rsolver(args[2].GetList()).Eval(cctx);
                        } else {
                            return new Rsolver(args[2].GetStr()).Eval(cctx);
                        }
                    }

                    return ErrorInfo(args);
                }
            }

            return new Rtoken();
            
        }

    }



}
