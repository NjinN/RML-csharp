using System;
using System.Collections.Generic;
using System.Text;

namespace RML.Lang {



    abstract class Rnative{
        public string name;
        public int argsLen;

        public abstract Rtoken Run(List<Rtoken> args, Rtable ctx);
    
    }
}
