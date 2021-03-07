using System;
using System.Collections.Generic;
using System.Text;

namespace RML.Lang {

    [Serializable]
    class Rword {
        public string key;
        public Rtable ctx;


        public Rword() { }

        public Rword(string k) {
            key = k;
            ctx = null;
        }

        public Rword(string k, Rtable c) {
            key = k;
            ctx = c;
        }
    }
}
