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

        public decimal GetFloat() {
            return (decimal)val;
        }

        public string GetStr() {
            return (string)val;
        }

        public List<Rtoken> GetList() {
            return (List<Rtoken>)val;
        }

        public Rtable GetTable() {
            return (Rtable)val;
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
                    sb.Append('[');
                    foreach(Rtoken item in GetList()) {
                        sb.Append(item.ToStr());
                        sb.Append(' ');
                    }
                    if(sb.Length > 1) {
                        sb.Remove(sb.Length - 1, 1);
                    }
                    sb.Append(']');
                    return sb.ToString();
                case Rtype.Paren:
                    sb.Append('(');
                    foreach (Rtoken item in GetList()) {
                        sb.Append(item.ToStr());
                        sb.Append(' ');
                    }
                    if (sb.Length > 1) {
                        sb.Remove(sb.Length - 1, 1);
                    }
                    sb.Append(')');
                    return sb.ToString();
                case Rtype.Object:
                    sb.Append('{');
                    foreach (var item in GetTable().table) {
                        sb.Append(item.Key);
                        sb.Append(": ");
                        sb.Append(item.Value.ToStr());
                        sb.Append(' ');
                    }
                    if (sb.Length > 1) {
                        sb.Remove(sb.Length - 1, 1);
                    }
                    sb.Append('}');
                    return sb.ToString();
                case Rtype.Flow:
                    return "flow::()";
                case Rtype.Word:
                    return GetWord().key;
                case Rtype.Path:
                    foreach (Rtoken item in GetList()) {
                        sb.Append(item.ToStr());
                        sb.Append('/');
                    }
                    if (sb.Length > 1) {
                        sb.Remove(sb.Length - 1, 1);
                    }
                    return sb.ToString();
                case Rtype.LitWord:
                    return "'" + GetStr();
                case Rtype.SetWord:
                    return GetStr() + ":";
                case Rtype.SetPath:
                    foreach (Rtoken item in GetList()) {
                        sb.Append(item.ToStr());
                        sb.Append('/');
                    }
                    if (sb.Length > 1) {
                        sb.Remove(sb.Length - 1, 1);
                    }
                    return sb.ToString();
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
                case Rtype.Path:
                case Rtype.SetPath:
                    return GetPathVal(ctx);

                default:
                    return this;
            }
        }


        public Rtoken GetPathVal(Rtable ctx) {
            List<Rtoken> list = GetList();
            if (!list[0].tp.Equals(Rtype.Word)) {
                return new Rtoken(Rtype.Err, "Error: illegal path! of " + ToStr());
            }
            Rtoken errTk = new Rtoken(Rtype.Err, "Error: illegal path! of " + ToStr());
            Rtoken currTk = null;
            int i = 0;
            do {
                Rtoken index = list[i];
                if (index.tp.Equals(Rtype.Paren)) {
                    index = index.GetVal(ctx);
                }

                if(i == 0) {
                    if (!index.tp.Equals(Rtype.Word)) {
                        return errTk;
                    }
                    currTk = ctx.Get(index.GetWord().key);
                    i++;
                    continue;
                }

                switch (currTk.tp) {
                    case Rtype.Block:
                    case Rtype.Paren:
                        if (index.tp.Equals(Rtype.Int)) {
                            int ii = index.GetInt();
                            if (ii <= 0 || ii > currTk.GetList().Count) {
                                return new Rtoken(Rtype.Err, "Error: index out of bound for " + ToStr());
                            }
                            currTk = currTk.GetList()[index.GetInt() - 1];

                        } else if (index.tp.Equals(Rtype.SetWord)) { 
                            if(StrKit.IsNumberStr(index.GetStr()) == 0 ) {
                                currTk = new Rtoken(Rtype.SetPath, new List<Rtoken>() { currTk,  RtokenKit.MakeRtoken(index.GetStr(), ctx) });
                            } else {
                                return errTk;
                            }

                        } else {
                            return errTk;
                        }
                        break;

                    case Rtype.Object:
                        switch (index.tp) {
                            case Rtype.SetWord:
                                currTk = new Rtoken(Rtype.SetPath, new List<Rtoken>(){currTk, index});
                                break;
                            case Rtype.Word:
                                Rtoken obj = currTk;
                                currTk = currTk.GetTable().GetNow(index.GetWord().key);
                                if(currTk.tp.Equals(Rtype.Func)) {
                                    new Rtoken(Rtype.Path, new List<Rtoken>() {currTk, obj });
                                }
                                break;
                            default:
                                return errTk;
                        }
                        break;

                    default:
                        return errTk;
                }



                i++;
            } while (i < list.Count);

            return currTk;

        }


        public Rtoken Clone() {
            return new Rtoken(tp, val);
        }

        public void Copy(Rtoken tk) {
            tp = tk.tp;
            val = tk.val;
        }

        public Rtoken Copy() {
            switch (tp) {
                case Rtype.Block:
                case Rtype.Paren:
                    List<Rtoken> list = new List<Rtoken>();
                    foreach(Rtoken item in GetList()) {
                        list.Add(item.Clone());
                    }
                    return new Rtoken(tp, list);

                default:
                    return Clone();
            }
        }

        public Rtoken PutList(int idx, Rtoken v) {
            if(idx <= 0) {
                return new Rtoken(Rtype.Err, "Error: index out of bound");
            }

            if(Renv.threads > 1) {
                lock (this) {
                    var list = GetList();
                    if (idx <= list.Count) {
                        list[idx - 1].Copy(v);
                        return list[idx - 1];
                    }

                    int i = list.Count;
                    while (i < idx - 1) {
                        i++;
                        list.Add(new Rtoken(Rtype.None, 0));
                    }
                    list.Add(v);
                    val = list;
                    return list[idx - 1];
                }
            } else {
                var list = GetList();
                if(idx <= list.Count) {
                    list[idx - 1].Copy(v);
                    return list[idx - 1];
                }

                int i = list.Count;
                while(i < idx - 1) {
                    i++;
                    list.Add(new Rtoken(Rtype.None, 0));
                }
                list.Add(v);
                val = list;
                return list[idx-1];
            }

            
        }



    }


}
