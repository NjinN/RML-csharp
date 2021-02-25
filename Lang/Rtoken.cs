using System;
using System.Collections.Generic;
using System.Text;

namespace RML.Lang {
    class Rtoken {
        public Rtype    tp;
        public Object  val;

        public Rtoken() {
            tp = Rtype.Nil;
            val = 0;
        }

        public Rtoken(Rtype t, dynamic v) {
            tp = t;
            val = v;
        }


        public bool GetBool() {
            return (bool)val;
        }

        public byte GetByte() {
            return (byte)val;
        }

        public char GetChar() {
            return (char)val;
        }


        public int GetInt() {
            return (int)val;
        }

        public double GetFloat() {
            return (double)val;
        }

        public string GetStr() {
            return (string)val;
        }

        public List<Rtoken> GetList() {
            return (List<Rtoken>)val;
        }

        public Rword GetWord() {
            return (Rword)val;
        }

        public Rnative GetNative() {
            return (Rnative)val;
        }

        public Rflow GetFlow() {
            return (Rflow)val;
        }

        public Rfunc GetFunc() {
            return (Rfunc)val;
        }


        public string ToStr() {
            StringBuilder sb = new StringBuilder();
            switch (tp) {
                case Rtype.Nil:
                    return "nil";
                case Rtype.None:
                    return "none";
                case Rtype.Err:
                    return GetStr();
                case Rtype.Bool:
                    return GetBool().ToString().ToLower();
                case Rtype.Byte:
                    return GetByte().ToString();
                case Rtype.Char:
                    return GetChar().ToString();
                case Rtype.Int:
                    return GetInt().ToString();
                case Rtype.Float:
                    return GetFloat().ToString();
                case Rtype.Str:
                    return '"' + GetStr() + '"';
                case Rtype.Block: { }
                    sb.Append("[");
                    foreach(Rtoken item in GetList()) {
                        sb.Append(item.ToStr());
                        sb.Append(" ");
                    }
                    if(sb.Length > 1) {
                        sb.Remove(sb.Length - 1, 1);
                    }
                    sb.Append("]");
                    return sb.ToString();
                case Rtype.Paren:
                    sb.Append("(");
                    foreach (Rtoken item in GetList()) {
                        sb.Append(item.ToStr());
                        sb.Append(" ");
                    }
                    if (sb.Length > 1) {
                        sb.Remove(sb.Length - 1, 1);
                    }
                    sb.Append(")");
                    return sb.ToString();
                case Rtype.Flow:
                    return "flow::()";
                case Rtype.Word:
                    return GetWord().key;
                case Rtype.LitWord:
                    return "'" + GetStr();
                case Rtype.SetWord:
                    return GetStr() + ":";
                case Rtype.Func:
                    return "Function";
                case Rtype.Native:
                    return "Native";
                case Rtype.Op:
                    return "Op";



                default:
                    return "undefined-token!!!";
            }
        }

        public void Echo() {
            if (tp.Equals(Rtype.Str)) {
                string str = ToStr();
                Console.WriteLine(str.Substring(1, str.Length - 2));
            } else {
                Console.WriteLine(ToStr());
            }
        }


        public Rtoken GetVal(Rtable ctx) {
            switch (tp) {
                case Rtype.Word:
                    Rword w = GetWord();
                    Rtoken v;
                    if(null == w.ctx) {
                        v = ctx.Get(w.key);
                    } else {
                        v = w.ctx.Get(w.key);
                    }

                    if (v.tp.Equals(Rtype.Nil)) {
                        return new Rtoken(Rtype.None, 0);
                    } else {
                        return v;
                    }

                case Rtype.LitWord:
                    return new Rtoken(Rtype.Word, new Rword(GetStr(), ctx));

                case Rtype.Paren:
                    return new Rsolver(GetList()).Eval(ctx);

                default:
                    return this;
            }
        }

    }
}
