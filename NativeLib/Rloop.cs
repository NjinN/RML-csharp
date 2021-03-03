using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RML.Lang;

namespace RML.NativeLib {
    class Rbreak : Rnative {
        public Rbreak() {
            name = "break";
            argsLen = 0;
        }

        public override Rtoken Run(List<Rtoken> args, Rtable ctx) {
            
            return new Rtoken(Rtype.Flow, new Rflow("break"));
        }
    }

    class Rcontinue : Rnative {
        public Rcontinue() {
            name = "continue";
            argsLen = 0;
        }

        public override Rtoken Run(List<Rtoken> args, Rtable ctx) {

            return new Rtoken(Rtype.Flow, new Rflow("continue"));
        }
    }


    class Rloop : Rnative {
        public Rloop() {
            name = "loop";
            argsLen = 2;
        }

        public override Rtoken Run(List<Rtoken> args, Rtable ctx) {
            if (args[0].tp.Equals(Rtype.Int)) {
                if (args[1].tp.Equals(Rtype.Block)) {
                    int idx = 0;
                    while(idx < args[0].GetInt()) {
                        Rtoken ans = new Rsolver(args[1].GetList()).Eval(ctx);
                        if (ans.tp.Equals(Rtype.Flow)) {
                            if (ans.GetFlow().name.Equals("break")) {
                                break;
                            }else if (ans.GetFlow().name.Equals("continue")) {

                            } else if (ans.GetFlow().name.Equals("pass")) {

                            } else {
                                return ans;
                            }
                        }
                        idx++;
                    }

                    return new Rtoken();

                }else if (args[1].tp.Equals(Rtype.Str)) {
                    int idx = 0;
                    while (idx < args[0].GetInt()) {
                        Rtoken ans = new Rsolver(args[1].GetStr()).Eval(ctx);
                        if (ans.tp.Equals(Rtype.Flow)) {
                            if (ans.GetFlow().name.Equals("break")) {
                                break;
                            } else if (ans.GetFlow().name.Equals("continue")) {

                            } else if (ans.GetFlow().name.Equals("pass")) {

                            } else {
                                return ans;
                            }
                        }
                        idx++;
                    }

                    return new Rtoken();
                }

            }


            return ErrorInfo(args);
        }
    }




    class Repeat : Rnative {
        public Repeat() {
            name = "repeat";
            argsLen = 3;
            quoteList = new List<bool> {true, false, false};
        }

        public override Rtoken Run(List<Rtoken> args, Rtable ctx) {
            if (args[0].tp.Equals(Rtype.Word) && args[1].tp.Equals(Rtype.Int)) {
                Rtable rCtx = new Rtable(Rtable.Type.TMP, ctx);
                rCtx.PutNow(args[0].GetWord().key, new Rtoken(Rtype.Int, 1));

                if (args[2].tp.Equals(Rtype.Block)) {
                    Rtoken cond = rCtx.GetNow(args[0].GetWord().key);
                    RtokenKit.ClearCtxForWordByWords(new List<Rtoken>() { args[0] }, args[2].GetList());

                    while(cond.tp.Equals(Rtype.Int) && cond.GetInt() <= args[1].GetInt()) {
                        Rtoken ans = new Rsolver(args[2].GetList()).Eval(rCtx);

                        if (ans.tp.Equals(Rtype.Flow)) {
                            if (ans.GetFlow().name.Equals("break")) {
                                break;
                            } else if (ans.GetFlow().name.Equals("return")) {
                                return ans;
                            }
                        }

                        cond = rCtx.GetNow(args[0].GetWord().key);
                        if (cond.tp.Equals(Rtype.Int)) {
                            cond.val = (int)cond.val + 1;
                        } else {
                            break;
                        }
                        
                    }

                    return new Rtoken();

                } else if (args[2].tp.Equals(Rtype.Str)) {
                    Rtoken cond = rCtx.GetNow(args[0].GetWord().key);
                    while (cond.tp.Equals(Rtype.Int) && cond.GetInt() <= args[1].GetInt()) {
                        Rtoken ans = new Rsolver(args[2].GetStr()).Eval(rCtx);

                        if (ans.tp.Equals(Rtype.Flow)) {
                            if (ans.GetFlow().name.Equals("break")) {
                                break;
                            } else if (ans.GetFlow().name.Equals("return")) {
                                return ans;
                            }
                        }

                        cond = rCtx.GetNow(args[0].GetWord().key);
                        if (cond.tp.Equals(Rtype.Int)) {
                            cond.val = (int)cond.val + 1;
                        } else {
                            break;
                        }

                    }

                    return new Rtoken();
                }

            }


            return ErrorInfo(args);
        }
    }



    class Rfor : Rnative {
        public Rfor() {
            name = "for";
            argsLen = 5;
            quoteList = new List<bool> { true, false, false, false, false };
        }

        public override Rtoken Run(List<Rtoken> args, Rtable ctx) {
            return (args[0].tp, args[1].tp, args[2].tp, args[3].tp, args[4].tp) switch {
                (Rtype.Word, Rtype.Int, Rtype.Int, Rtype.Int, Rtype.Block) => ForIntIntInt(args, ctx),
                (Rtype.Word, Rtype.Float, Rtype.Float, Rtype.Float, Rtype.Block) => ForFloatFloatFloat(args, ctx),


                _ => ErrorInfo(args)
            };

    
        }


        Rtoken ForIntIntInt(List<Rtoken> args, Rtable ctx) {
            Rtable fctx = new Rtable(Rtable.Type.TMP, ctx);
            string condKey = args[0].GetWord().key;
            Rtoken cond = args[1].Copy();
            fctx.PutNow(condKey, cond);
            cond = fctx.GetNow(condKey);
            List<Rtoken> code = RtokenKit.CopyList(args[4].GetList());

            RtokenKit.ClearCtxForWordByWords(new List<Rtoken>() { args[0] }, code);

            Rsolver fsolver = new Rsolver();

            while(cond.tp.Equals(Rtype.Int) && cond.GetInt() <= args[2].GetInt()) {
                fsolver.InputBlk(code);
                Rtoken ans = fsolver.Eval(fctx);
                
                if (ans.tp.Equals(Rtype.Err)) {
                    return ans;
                }else if (ans.tp.Equals(Rtype.Flow)) {
                    if (ans.GetFlow().name.Equals("return")) {
                        return ans;
                    }else if (ans.GetFlow().name.Equals("break")) {
                        break;
                    }
                }

                if (cond.tp.Equals(Rtype.Int)) {
                    cond.val = cond.GetInt() + args[3].GetInt();
                } else {
                    break;
                }
            }

            return new Rtoken();
        }


        Rtoken ForFloatFloatFloat(List<Rtoken> args, Rtable ctx) {
            Rtable fctx = new Rtable(Rtable.Type.TMP, ctx);
            string condKey = args[0].GetWord().key;
            Rtoken cond = args[1].Copy();
            fctx.PutNow(condKey, cond);
            cond = fctx.GetNow(condKey);
            List<Rtoken> code = RtokenKit.CopyList(args[4].GetList());

            RtokenKit.ClearCtxForWordByWords(new List<Rtoken>() { args[0] }, code);

            Rsolver fsolver = new Rsolver();

            while (cond.tp.Equals(Rtype.Float) && cond.GetFloat() <= args[2].GetFloat()) {
                fsolver.InputBlk(code);
                Rtoken ans = fsolver.Eval(fctx);

                if (ans.tp.Equals(Rtype.Err)) {
                    return ans;
                } else if (ans.tp.Equals(Rtype.Flow)) {
                    if (ans.GetFlow().name.Equals("return")) {
                        return ans;
                    } else if (ans.GetFlow().name.Equals("break")) {
                        break;
                    }
                }

                if (cond.tp.Equals(Rtype.Float)) {
                    cond.val = cond.GetFloat() + args[3].GetFloat();
                } else {
                    break;
                }
            }

            return new Rtoken();
        }

    }

}
