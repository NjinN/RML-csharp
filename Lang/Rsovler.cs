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
        public List<Rtoken> reduceBlk;

        //public Rsolver() { }

        public Rsolver(List<string> strs) {
            inpStrs = strs;
            inpBlk = null;
            inpLen = strs.Count;
            idx = 0;
            model = Model.STR;
            ansTk = new Rtoken();
            reduceBlk = new List<Rtoken>();
        }

        public Rsolver(string strs) {
            inpStrs = StrKit.CutStrs(strs);
            inpBlk = null;
            inpLen = inpStrs.Count;
            idx = 0;
            model = Model.STR;
            ansTk = new Rtoken();
            reduceBlk = new List<Rtoken>();
        }

        public Rsolver(List<Rtoken> blk) {
            inpStrs = null;
            inpBlk = blk;
            inpLen = blk.Count;
            idx = 0;
            model = Model.BLK;
            ansTk = new Rtoken();
            reduceBlk = new List<Rtoken>();
        }

        public void InputStr(string s) {
            inpStrs = StrKit.CutStrs(s);
            inpBlk = null;
            inpLen = inpStrs.Count;
            idx = 0;
            model = Model.STR;
            ansTk = new Rtoken();
            reduceBlk = new List<Rtoken>();
        }

        public void InputBlk(List<Rtoken> blk) {
            inpStrs = null;
            inpBlk = blk;
            inpLen = blk.Count;
            idx = 0;
            model = Model.BLK;
            ansTk = new Rtoken();
            reduceBlk = new List<Rtoken>();
        }


        public Rtoken Eval(Rtable ctx) {
            
            while(idx < inpLen) {
                EvalOne(ctx);
                if(ansTk.tp.Equals(Rtype.Flow)) {
                    if (ansTk.GetFlow().name.Equals("opAns")) {
                        if (reduceBlk.Count > 0) {
                            reduceBlk.RemoveAt(reduceBlk.Count - 1);
                        }
                        ansTk = ansTk.GetFlow().val;
                    }else if (ansTk.GetFlow().name.Equals("pass")) { 
                    
                    } else {
                        return ansTk;
                    }
                    
                } 

                reduceBlk.Add(ansTk);
            }


            return ansTk;
        }


        public Rtoken EvalOne(Rtable ctx) {
            Rtoken currTk;


            if (model.Equals(Model.STR)) {
                currTk = RtokenKit.MakeRtoken(inpStrs[idx], ctx);
            } else {
                currTk = inpBlk[idx];
            }

            idx++;

            currTk = currTk.GetVal(ctx);

            switch (currTk.tp) {
                case Rtype.SetWord:
                    if(idx == inpLen) {
                        return new Rtoken(Rtype.Err, "Error: Incomplete expression!!!");
                    }
                    Rtoken v = EvalOne(ctx);
                    ctx.Put(currTk.GetStr(), v);
                    ansTk = v;
                    return v;

                case Rtype.Func:
                    Rfunc f = currTk.GetFunc();
                    List<Rtoken> fArgs = new List<Rtoken>();
                    EvalN(fArgs, ctx, f.argsLen);
                    if(fArgs.Count < f.argsLen) {
                        return new Rtoken(Rtype.Err, "Error: Incomplete expression!!!");
                    }
                    ansTk = f.Run(fArgs, ctx);
                    return ansTk;

                case Rtype.Native:
                    Rnative nv = currTk.GetNative();
                    List<Rtoken> nArgs = new List<Rtoken>();
                    EvalN(nArgs, ctx, nv.argsLen);
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
                    EvalN(oArgs, ctx, op.argsLen);
                    if(oArgs.Count < op.argsLen) {
                        ansTk = new Rtoken(Rtype.Err, "Error: Incomplete expression!!!");
                        return ansTk;
                    }
                    ansTk = new Rtoken(Rtype.Flow, new Rflow("opAns", op.Run(oArgs, ctx)));
                    return ansTk;

                default:
                    ansTk = currTk;
                    return ansTk;
            }


        }

        
        public void EvalN(List<Rtoken> args, Rtable ctx, int n) {
            while(args.Count < n && idx < inpLen) {
                EvalOne(ctx);
                if (ansTk.tp.Equals(Rtype.Flow) && ansTk.GetFlow().name.Equals("opAns")) {
                    if (args.Count > 0) {
                        args.RemoveAt(args.Count - 1);
                    }
                    ansTk = ansTk.GetFlow().val;
                }

                args.Add(ansTk);
            }
        }


    }
}
