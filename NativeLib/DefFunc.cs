using System;
using System.Collections.Generic;
using System.Text;
using RML.Lang;

namespace RML.NativeLib {
    class Func : Rnative {
        public Func() {
            name = "func";
            argsLen = 2;
        }

        public override Rtoken Run(List<Rtoken> args, Rtable ctx) {
            Rtype type0 = args[0].tp;
            Rtype type1 = args[1].tp;

            if(type0.Equals(Rtype.Block) && type1.Equals(Rtype.Block)) {
                List<Rtoken> argsBlk = args[0].GetList();
                List<Rtoken> codeBlk = args[1].GetList();

                List<Rtoken> aList = new List<Rtoken>();
                List<Rtoken> pList = new List<Rtoken>();
                List<bool> qList = new List<bool>();
                List<Rtoken> localList = new List<Rtoken>();

                int i = 0;
                while( i < argsBlk.Count && (argsBlk[i].tp.Equals(Rtype.Word) || argsBlk[i].tp.Equals(Rtype.GetWord))) {
                    aList.Add(argsBlk[i].Clone());
                    if (argsBlk[i].tp.Equals(Rtype.Word)) {
                        qList.Add(false);
                    } else {
                        qList.Add(true);
                    }
                    i++;
                }

                while(i < argsBlk.Count) {
                    
                    if (!argsBlk[i].tp.Equals(Rtype.Prop)) {
                        return new Rtoken(Rtype.Err, "Error: " + argsBlk[i].ToStr() + " is no supported for def func!");
                    }

                    if (argsBlk[i].GetStr().Equals("local")) {
                        i++;
                        while(i < argsBlk.Count) {
                            if (!argsBlk[i].tp.Equals(Rtype.Word)) {
                                return new Rtoken(Rtype.Err, "Error: " + argsBlk[i].ToStr() + " is no supported for def /local");
                            }
                            localList.Add(argsBlk[i].Copy());
                            i++;
                        }

                        break;
                    }

                    pList.Add(argsBlk[i].Clone());
                    
                    if(i == argsBlk.Count - 1 || argsBlk[i+1].tp.Equals(Rtype.Prop)){
                        pList.Add(null);
                    } else {
                        if(argsBlk[i+1].tp.Equals(Rtype.Word) || argsBlk[i + 1].tp.Equals(Rtype.GetWord)) {
                            pList.Add(argsBlk[i + 1].Clone());
                        } else {
                            return new Rtoken(Rtype.Err, "Error: " + argsBlk[i].ToStr() + " is no supported for def func!");
                        }
                        i++;
                    }

                    i++;
                }

                List<Rtoken> clearList = new List<Rtoken>();
                clearList.AddRange(aList);
                clearList.AddRange(localList);
                foreach(var item in pList) {
                    if(null != item) {
                        if (item.tp.Equals(Rtype.Word)) {
                            clearList.Add(new Rtoken(Rtype.Word, new Rword(item.GetWord().key, null)));
                        } else {
                            clearList.Add(new Rtoken(Rtype.Word, new Rword(item.GetStr(), null)));
                        }
                    }
                }

                RtokenKit.ClearCtxForWordByWords(clearList, codeBlk);

                return new Rtoken(Rtype.Func, new Rfunc(aList.Count, aList, codeBlk, qList, pList, localList));

            }

            return ErrorInfo(args);
        }
    }



    class Rreturn : Rnative {
        public Rreturn() {
            name = "return";
            argsLen = 1;
        }

        public override Rtoken Run(List<Rtoken> args, Rtable ctx) {
            return new Rtoken(Rtype.Flow, new Rflow("return", args[0]));
        }
    }
}
