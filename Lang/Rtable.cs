using System;
using System.Collections.Generic;
using System.Text;

namespace RML.Lang {
    class Rtable {
        public enum Type { SYS, USR, TMP};

        public Type tp;
        public Dictionary<string, Rtoken> table;
        public Rtable father;


        public Rtable() {
            tp = Type.USR;
            table = new Dictionary<string, Rtoken>();
            father = null;
        }

        public Rtable(Type t) {
            tp = t;
            table = new Dictionary<string, Rtoken>();
            father = null;
        }

        public Rtable(Type t, Rtable f) {
            tp = t;
            table = new Dictionary<string, Rtoken>();
            father = f;
        }

        public void SetFather(Rtable f) {
            father = f;
        }

        public void PutNow(string k, Rtoken v) {
            if(Renv.threads > 1) {
                lock (this) {
                    if (table.ContainsKey(k)) {
                        table.Remove(k);
                    }
                    table.Add(k, v.Copy());
                }

            } else {
                if (table.ContainsKey(k)) {
                    table.Remove(k);
                }
                table.Add(k, v.Copy());
            }
        }

        public Rtoken GetNow(string k) {
            return table.GetValueOrDefault(k, new Rtoken());
        }

        public void RemoveNow(string k) {
            table.Remove(k);
        }

        public void Put(string k, Rtoken v) {
            Rtable tb = this;
            
            while(!tb.table.ContainsKey(k) && null != tb.father && tb.father.tp != Type.SYS) {
                tb = tb.father;
            }

            tb.PutNow(k, v);
        }

        public Rtoken Get(string k) {
            Rtable tb = this;

            while(!tb.table.ContainsKey(k) && null != tb.father) {
                tb = tb.father;
            }

            return tb.GetNow(k);
        }


        public void Remove(string k) {
            Rtable tb = this;

            while (!tb.table.ContainsKey(k) && null != tb.father) {
                tb = tb.father;
            }

            tb.RemoveNow(k);
        }

    }
}
