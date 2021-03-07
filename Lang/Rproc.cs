using System;
using System.Collections.Generic;
using System.Text;

namespace RML.Lang {

    [Serializable]
    class Rproc {
        public Dictionary<string, RprocEnity> typeMap;
        public Dictionary<int, RprocEnity> countMap;

        public Rproc() {
            typeMap = new Dictionary<string, RprocEnity>();
            countMap = new Dictionary<int, RprocEnity>();
        }

        public void PutByInt(int count, RprocEnity enity) {
            if (countMap.ContainsKey(count)) {
                countMap.Remove(count);
            }
            countMap.Add(count, enity);
        }

        public void PutByType(string t, RprocEnity enity) {
            if (typeMap.ContainsKey(t)) {
                typeMap.Remove(t);
            }
            typeMap.Add(t, enity);
        }





    }


    [Serializable]
    class RprocEnity {
        public List<Rtoken> args;
        public List<Rtoken> code;
        public List<Rtoken> localList;


        public RprocEnity(List<Rtoken> a, List<Rtoken> c, List<Rtoken> ls) {
           
            args = a;
            code = c;
            localList = ls;
        }

        public Rtoken Run(List<Rtoken> actArgs, Rtable ctx) {
            Rtable fCtx = new Rtable(Rtable.Type.TMP, ctx);

            int i = 0;
            while(i < args.Count) {
                fCtx.PutNow(args[i].GetWord().key, actArgs[i]);
                i++;
            }

            foreach(var item in localList) {
                fCtx.PutNow(item.GetWord().key, new Rtoken(Rtype.None, 0));
            }

            return new Rsolver(code).Eval(fCtx);
        }

     

    }
}
