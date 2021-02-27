using System;
using System.Collections.Generic;
using System.Text;

namespace RML.Lang {



    abstract class Rnative{
        public string name;
        public int argsLen;

        public abstract Rtoken Run(List<Rtoken> args, Rtable ctx);
    
        public Rtoken ErrorInfo(List<Rtoken> args) {
            string typeStr = "";
            foreach(Rtoken item in args) {
                typeStr += RtokenKit.Rtype2Str(item.tp) + '-';
            }
            
            typeStr = typeStr.TrimEnd('-');
            
            return new Rtoken(Rtype.Err, "Error: Types of " + typeStr + " mismatch for native::" + name);
        }
    }
}
