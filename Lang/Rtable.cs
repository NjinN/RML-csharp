﻿using System;
using System.Collections.Generic;
using System.Text;

namespace RML.Lang {
    [Serializable]
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
                    Rtoken tk = table.GetValueOrDefault(k, null);
                    if(null == tk) {
                        table.Add(k, v);
                    } else {
                        tk.Copy(v);
                    }
                }

            } else {
                Rtoken tk = table.GetValueOrDefault(k, null);
                if (null == tk) {
                    table.Add(k, v);
                } else {
                    tk.Copy(v);
                }
            }
        }

        public Rtoken GetNow(string k) {
            return table.GetValueOrDefault(k, new Rtoken());
        }

        public bool RemoveNow(string k) {
            return table.Remove(k);
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


        public bool Remove(string k) {
            Rtable tb = this;

            while (!tb.table.ContainsKey(k) && null != tb.father) {
                tb = tb.father;
            }

            return tb.RemoveNow(k);
        }

        public Rtable CopyDeep() {
            Rtable result = new Rtable();
            result.tp = tp;
            result.father = father;

            foreach(var (k, v) in table) {
                result.table.Add(k, v.CopyDeep());
            }

            return result;
        }
    }
}
