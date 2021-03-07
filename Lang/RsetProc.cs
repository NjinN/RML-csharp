using System;
using System.Collections.Generic;
using System.Text;

using RML.NativeLib;

namespace RML.Lang {

    [Serializable]
    class RsetProc {
        public string name = "";
        public List<Rtoken> args = new List<Rtoken>();

        public RsetProc(string n, List<Rtoken> a) {
            name = n;
            args = a;
        }


        public Rtoken Set(Rtoken code, Rtable ctx) {
            List<Rtoken> actArgs = new Rcompose().Run(new List<Rtoken>() { new Rtoken(Rtype.Block, args) }, ctx).GetList();

            List<Rtoken> aList = new List<Rtoken>();
            List<Rtoken> lList = new List<Rtoken>();

            int i = 0;
            while(i < actArgs.Count) {
                if (actArgs[i].tp.Equals(Rtype.Prop)) {
                    if (!actArgs[i].GetStr().Equals("local")) {
                        return new Rtoken(Rtype.Err, "Error: " + actArgs[i].ToStr() + " is not supported for def proc!");
                    }
                    i++;
                    while (i < actArgs.Count) {
                        if (!actArgs[i].tp.Equals(Rtype.Word)) {
                            return new Rtoken(Rtype.Err, "Error: " + actArgs[i].ToStr() + " is not supported for def local var");
                        }
                        lList.Add(actArgs[i]);
                        i++;
                    }
                    break;
                }

                aList.Add(actArgs[i]);
                i++;
            }

            int model = 1;
            List<Rtoken> wordList = new List<Rtoken>();
            List<Rtoken> typeList = new List<Rtoken>();

            i = 0;
            while(i < aList.Count) {
                if (!aList[i].tp.Equals(Rtype.Word)){
                    model = 0;
                    break;
                }
                wordList.Add(aList[i]);
                i++;
            }

            

            if(model == 0 && aList.Count % 2 == 0) {
                wordList.Clear();
                model = 2;
                i = 0;
                while(i < aList.Count) {
                    if(!aList[i].tp.Equals(Rtype.Word) || !aList[i + 1].tp.Equals(Rtype.Datatype)) {
                        model = 0;
                        break;
                    }
                    wordList.Add(aList[i]);
                    typeList.Add(aList[i + 1]);
                    i += 2;
                }
            }

            if(model == 0) {
                return new Rtoken(Rtype.Err, "Error: error args for def proc!");
            }

            List<Rtoken> clearList = new List<Rtoken>();
            clearList.AddRange(wordList);
            clearList.AddRange(lList);
            RtokenKit.ClearCtxForWordByWords(clearList, code.GetList());
            RprocEnity enity = new RprocEnity(wordList, code.GetList(), lList);

            Rtoken p = ctx.Get(name);
            Rproc proc;
            if (p.tp.Equals(Rtype.Proc)) {
                proc = p.GetProc();
            } else {
                proc = new Rproc();
                ctx.Put(name, new Rtoken(Rtype.Proc, proc));
                p = ctx.Get(name);
            }

            if(model == 1) {
                proc.PutByInt(aList.Count, enity);
            }else if(model == 2) {
                string typeStr = "";
                foreach(var item in typeList) {
                    typeStr += item.ToStr();
                }
                proc.PutByType(typeStr, enity);
            }

            return p;

        }



    }

    
}
