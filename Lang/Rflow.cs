using System;
using System.Collections.Generic;
using System.Text;

namespace RML.Lang {
    class Rflow {
        public string name;
        public Rtoken val;


        public Rflow() { }

        public Rflow(string n) {
            name = n;
        }

        public Rflow(string n, Rtoken v) {
            name = n;
            val = v;
        }

    }
}
