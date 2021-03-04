using System;
using System.Collections.Generic;
using System.Text;

namespace RML.Lang {
    struct Rsolver {
        public enum Model { STR, BLK};

        public List<string> inpStrs;
        public List<Rtoken> inpBlk;
        public int inpLen;
        public int idx;
        public Model model;
        public Rtoken ansTk;
        public bool isLocal;

        //public Rsolver() { }

        public Rsolver(List<string> strs) {
            inpStrs = strs;
            inpBlk = null;
            inpLen = strs.Count;
            idx = 0;
            model = Model.STR;
            ansTk = new Rtoken();
            isLocal = false;
        }

        public Rsolver(string strs) {
            inpStrs = StrKit.CutStrs(strs);
            inpBlk = null;
            inpLen = inpStrs.Count;
            idx = 0;
            model = Model.STR;
            ansTk = new Rtoken();
            isLocal = false;
        }

        public Rsolver(List<Rtoken> blk) {
            inpStrs = null;
            inpBlk = blk;
            inpLen = blk.Count;
            idx = 0;
            model = Model.BLK;
            ansTk = new Rtoken();
            isLocal = false;
        }

        public Rsolver InputStr(string s) {
            inpStrs = StrKit.CutStrs(s);
            inpBlk = null;
            inpLen = inpStrs.Count;
            idx = 0;
            model = Model.STR;
            ansTk = new Rtoken();
            return this;
        }

        public Rsolver InputBlk(List<Rtoken> blk) {
            inpStrs = null;
            inpBlk = blk;
            inpLen = blk.Count;
            idx = 0;
            model = Model.BLK;
            ansTk = new Rtoken();
            return this;
        }


        public Rtoken Eval(Rtable ctx) {
            
            while(idx < inpLen) {
                EvalOne(ctx, false);
                if(ansTk.tp.Equals(Rtype.Flow)) {
                    if (ansTk.GetFlow().name.Equals("opAns")) {              
                        ansTk = ansTk.GetFlow().val;
                    } else if (ansTk.GetFlow().name.Equals("pass")) {
                    
                    } else {
                        return ansTk;
                    }
                    
                }else if (ansTk.tp.Equals(Rtype.Err)) {
                    return ansTk;
                } 
            }
            return ansTk;
        }

        public List<Rtoken> Reduce(Rtable ctx) {
            List<Rtoken> result = new List<Rtoken>();
            while (idx < inpLen) {
                EvalOne(ctx, false);
                if (ansTk.tp.Equals(Rtype.Flow)) {
                    if (ansTk.GetFlow().name.Equals("opAns")) {
                        if(result.Count > 0) {
                            result.RemoveAt(result.Count - 1);
                        }
                        ansTk = ansTk.GetFlow().val;
                    } else if (ansTk.GetFlow().name.Equals("return ")) {
                        ansTk = ansTk.GetFlow().val;
                    } else {
                        return result;
                    }

                }
                result.Add(ansTk);
            }
            return result;
        }


        public Rtoken EvalOne(Rtable ctx, bool quote) {
            Rtoken currTk;


            if (model.Equals(Model.STR)) {
                currTk = RtokenKit.MakeRtoken(inpStrs[idx], ctx);
            } else {
                currTk = inpBlk[idx];
            }

            idx++;

            if (quote) {
                ansTk = currTk;
                return ansTk;
            }

            currTk = currTk.GetVal(ctx);

            switch (currTk.tp) {
                case Rtype.SetWord:
                    if(idx == inpLen) {
                        return new Rtoken(Rtype.Err, "Error: Incomplete expression!!!");
                    }
                    Rtoken v = EvalOne(ctx, false);
                    if (isLocal) {
                        ctx.PutNow(currTk.GetStr(), v);
                    } else {
                        ctx.Put(currTk.GetStr(), v);
                    }
                    ansTk = v;
                    return v;
                case Rtype.SetPath:
                    List<Rtoken> list = currTk.GetList();

                    Rtoken value = EvalOne(ctx, false);
                    switch (list[0].tp) {
                        case Rtype.Block:
                            list[0].PutList(list[1].GetInt(), value);
                            break;
                        case Rtype.Object:
                            list[0].GetTable().PutNow(list[1].GetStr(), value);
                            break;
                        default:
                            ansTk = new Rtoken(Rtype.Err, "Error: Illegal  expression!!!");
                            return ansTk;
                    }
                    ansTk = value;
                    return ansTk;
                case Rtype.Func:
                    Rfunc f = currTk.GetFunc();
                    List<Rtoken> fArgs = new List<Rtoken>();
                    EvalN(fArgs, ctx, f.argsLen, null);
                    if(fArgs.Count < f.argsLen) {
                        ansTk = new Rtoken(Rtype.Err, "Error: Incomplete expression!!!");
                        return ansTk;
                    }
                    ansTk = f.Run(fArgs, ctx);
                    if(ansTk.tp.Equals(Rtype.Flow) && ansTk.GetFlow().name.Equals("return")) {
                        ansTk = ansTk.GetFlow().val;
                    }
                    return ansTk;

                case Rtype.Prop:
                    List<Rtoken> fwpList = currTk.GetList();
                    Rfunc fwp = fwpList[0].GetFunc();
                    Rtable fwpCtx = fwpList[1].GetTable();
                    List<Rtoken> pList = new List<Rtoken>(fwpList.ToArray()[2..]);
                    List<bool> fwpQuoteList = fwp.GetQuoteListWithProps(pList);
                    List<Rtoken> fwpArgs = new List<Rtoken>();
                    EvalN(fwpArgs, ctx, fwpQuoteList.Count, fwpQuoteList);
                    if (fwpArgs.Count < fwpQuoteList.Count) {
                        ansTk = new Rtoken(Rtype.Err, "Error: Incomplete expression!!!");
                        return ansTk;
                    }
                    ansTk = fwp.RunWithProps(fwpArgs, fwpCtx, pList);
                    if (ansTk.tp.Equals(Rtype.Flow) && ansTk.GetFlow().name.Equals("return")) {
                        ansTk = ansTk.GetFlow().val;
                    }
                    return ansTk;

                case Rtype.Native:
                    Rnative nv = currTk.GetNative();
                    List<Rtoken> nArgs = new List<Rtoken>();
                    EvalN(nArgs, ctx, nv.argsLen, nv.quoteList);
                    if(nArgs.Count < nv.argsLen) {
                        ansTk = new Rtoken(Rtype.Err, "Error: Incomplete expression!!!");
                        return ansTk;
                    }
                    ansTk = nv.Run(nArgs, ctx);
                    return ansTk;

                case Rtype.Op:
                    Rnative op = currTk.GetNative();
                    List<Rtoken> oArgs = new List<Rtoken>();
                    oArgs.Add(ansTk);
                    EvalN(oArgs, ctx, op.argsLen, op.quoteList);
                    if(oArgs.Count < op.argsLen) {
                        ansTk = new Rtoken(Rtype.Err, "Error: Incomplete expression!!!");
                        return ansTk;
                    }
                    ansTk = new Rtoken(Rtype.Flow, new Rflow("opAns", op.Run(oArgs, ctx)));
                    return ansTk;

                case Rtype.GetWord:
                    ansTk = new Rtoken(Rtype.Word, new Rword(currTk.GetStr(), ctx)).GetVal(ctx);
                    return ansTk;

                default:
                    ansTk = currTk;
                    return ansTk;
            }


        }

        
        public void EvalN(List<Rtoken> args, Rtable ctx, int n, List<bool> quoteList) {
            while(args.Count < n && idx < inpLen) {
                if(null == quoteList) {
                    EvalOne(ctx, false);
                } else {
                    EvalOne(ctx, quoteList[args.Count]);
                }
                
                if (ansTk.tp.Equals(Rtype.Flow) && ansTk.GetFlow().name.Equals("opAns")) {
                    if (args.Count > 0) {
                        args.RemoveAt(args.Count - 1);
                    }
                    ansTk = ansTk.GetFlow().val;
                }

                args.Add(ansTk);
            }
        }



        public static List<Rtoken> Compose(List<Rtoken> blk, Rtable ctx) {
            List<Rtoken> result = new List<Rtoken>();
            foreach(Rtoken item in blk) {
                if (item.tp.Equals(Rtype.Paren)) {
                    result.Add(new Rsolver(item.GetList()).Eval(ctx));
                } else {
                    result.Add(item);
                }
            }

            return result;
        }


        public static List<Rtoken> ComposeDeep(List<Rtoken> blk, Rtable ctx) {
            List<Rtoken> result = new List<Rtoken>();
            foreach (Rtoken item in blk) {
                if (item.tp.Equals(Rtype.Paren)) {
                    result.Add(new Rsolver(item.GetList()).Eval(ctx));
                } else if (item.tp.Equals(Rtype.Block)) {
                    List<Rtoken> list = ComposeDeep(item.GetList(), ctx);
                    result.Add(new Rtoken(Rtype.Block, list));
                } else {
                    result.Add(item);
                }
            }

            return result;

        }



    }
}
