using System;
using System.Collections.Generic;
using System.Text;

namespace RML.Lang {

    [Serializable]
    class RcallProc {
        public string name = "";
        public List<Rtoken> args = new List<Rtoken>();

        public RcallProc(string n, List<Rtoken> a) {
            name = n;
            args = a;
        }

        public Rtoken Call(Rtable ctx) {
            Rsolver solver = new Rsolver();
            List<Rtoken> actArgs = solver.InputBlk(args).Reduce(ctx);


            Rtoken p = ctx.Get(name);

            if (!p.tp.Equals(Rtype.Proc)) {
                return new Rtoken(Rtype.Err, "Error: No such proc! named " + name);
            }

            Rproc proc = p.GetProc();
            RprocEnity procEnity;

            if(proc.typeMap.Count == 0) {
                int count = actArgs.Count;
                procEnity = proc.countMap.GetValueOrDefault(count, null);
                
            } else {
                string typeStr = "";
                foreach(var item in actArgs) {
                    typeStr += RtokenKit.Rtype2Str(item.tp);
                }

                procEnity = proc.typeMap.GetValueOrDefault(typeStr, null);
                if(null == procEnity) {
                    int count = actArgs.Count;
                    procEnity = proc.countMap.GetValueOrDefault(count, null);
                }

            }

            if(null == procEnity) {
                return new Rtoken(Rtype.Err, "Error: No such proc! named " + name + " for " + ToStr());
            }

            return procEnity.Run(actArgs, ctx);
        }


        public string ToStr() {
            StringBuilder sb = new StringBuilder();
            sb.Append(name);
            sb.Append('(');
            foreach (Rtoken item in args) {
                sb.Append(item.ToStr());
                sb.Append(' ');
            }
            if (sb.Length > 1) {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append(')');
            return sb.ToString();

        }

    }

    
}
