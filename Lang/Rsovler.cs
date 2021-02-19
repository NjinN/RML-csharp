using System;
using System.Collections.Generic;
using System.Text;

namespace RML.Lang {
    class Rsolver {
        public enum Model { STR, BLK};

        public List<string> inpStrs;
        public List<Rtoken> inpBlk;
        public int inpLen;
        public int idx;
        public Model model;
        public Rtoken ansTk;
        public List<Rtoken> reduceBlk;

        public Rsolver() { }

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
                ansTk = EvalOne(ctx);
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
                    return v;

                case Rtype.Func:
                    Rfunc f = currTk.GetFunc();
                    List<Rtoken> fArgs = new List<Rtoken>();
                    EvalN(fArgs, ctx, f.argsLen);
                    if(fArgs.Count < f.argsLen) {
                        return new Rtoken(Rtype.Err, "Error: Incomplete expression!!!");
                    }
                    return f.Run(fArgs, ctx);

                case Rtype.Native:
                    Rnative nv = currTk.GetNative();
                    List<Rtoken> nArgs = new List<Rtoken>();
                    EvalN(nArgs, ctx, nv.argsLen);
                    if(nArgs.Count < nv.argsLen) {
                        return new Rtoken(Rtype.Err, "Error: Incomplete expression!!!");
                    }
                    return nv.Run(nArgs, ctx);

                case Rtype.Op:
                    if(reduceBlk.Count == 0) {
                        return new Rtoken(Rtype.Err, "Error: Incomplete expression!!!");
                    }
                    reduceBlk.RemoveAt(reduceBlk.Count - 1);

                    Rnative op = currTk.GetNative();
                    List<Rtoken> oArgs = new List<Rtoken>();
                    oArgs.Add(ansTk);
                    EvalN(oArgs, ctx, op.argsLen);
                    if(oArgs.Count < op.argsLen) {
                        return new Rtoken(Rtype.Err, "Error: Incomplete expression!!!");
                    }
                    return op.Run(oArgs, ctx);

                default:
                    return currTk;
            }


        }

        
        public void EvalN(List<Rtoken> args, Rtable ctx, int n) {
            while(args.Count < n && idx < inpLen) {
                args.Add(EvalOne(ctx));
            }
        }


    }
}
