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

                    return new Rtoken(Rtype.None, 0);

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

                    return new Rtoken(Rtype.None, 0);
                }

            }


            return ErrorInfo(args);
        }
    }




    class Repeat : Rnative {
        public Repeat() {
            name = "repeat";
            argsLen = 3;
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

                    return new Rtoken(Rtype.None, 0);

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

                    return new Rtoken(Rtype.None, 0);
                }

            }


            return ErrorInfo(args);
        }
    }


}
