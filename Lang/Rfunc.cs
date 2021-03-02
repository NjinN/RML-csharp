using System;
using System.Collections.Generic;
using System.Text;

namespace RML.Lang {
    class Rfunc {
        public int argsLen;
        public List<Rtoken> args;
        public List<Rtoken> code;
        public List<bool> quotes;
        public List<Rtoken> props;
        public List<Rtoken> localList;
        public List<string> desc;


        public Rfunc(int l, List<Rtoken> a, List<Rtoken> c, List<bool> q, List<Rtoken> p, List<Rtoken> ls) {
            argsLen = l;
            args = a;
            code = c;
            quotes = q;
            props = p;
            localList = ls;
        }

        public Rtoken Run(List<Rtoken> actArgs, Rtable ctx) {
            Rtable fCtx = new Rtable(Rtable.Type.TMP, ctx);

            int i = 0;
            while(i < argsLen) {
                fCtx.PutNow(args[i].GetWord().key, actArgs[i]);
                i++;
            }

            int x = 0;
            while (x < props.Count) {
                if (!fCtx.table.ContainsKey(props[x].GetStr())) {
                    if (null == props[x + 1]) {
                        fCtx.PutNow(props[x].GetStr(), new Rtoken(Rtype.Bool, false));
                    } else {
                        if (props[x + 1].tp.Equals(Rtype.Word)) {
                            fCtx.PutNow(props[x + 1].GetWord().key, new Rtoken(Rtype.None, 0));
                        } else {
                            fCtx.PutNow(props[x + 1].GetStr(), new Rtoken(Rtype.None, 0));
                        }
                    }
                }
                x += 2;
            }

            foreach(var item in localList) {
                fCtx.PutNow(item.GetWord().key, new Rtoken(Rtype.None, 0));
            }

            return new Rsolver(code).Eval(fCtx);
        }

        public Rtoken RunWithProps(List<Rtoken> actArgs, Rtable ctx, List<Rtoken> ps) {
            Rtable fCtx = new Rtable(Rtable.Type.TMP, ctx);

            int i = 0;
            while (i < argsLen) {
                fCtx.PutNow(args[i].GetWord().key, actArgs[i]);
                i++;
            }

            foreach(var p in ps) {
                int j = 0;
                bool match = false;
                while(j < props.Count) {
                    if (props[j].GetStr().Equals(p.GetStr())) {
                        if(null == props[j + 1]) {
                            fCtx.PutNow(p.GetStr(), new Rtoken(Rtype.Bool, true));
                        } else {
                            if (props[j + 1].tp.Equals(Rtype.Word)) {
                                fCtx.PutNow(props[j + 1].GetWord().key, actArgs[i]);
                            } else {
                                fCtx.PutNow(props[j + 1].GetStr(), actArgs[i]);
                            }
                            i++;
                        }

                        match = true;
                    }
                    j += 2;
                }

                if (!match) {
                    return new Rtoken(Rtype.Err, "Error: error prop " + p.ToStr());
                }

            }

            int x = 0;
            while(x < props.Count) {
                if (!fCtx.table.ContainsKey(props[x].GetStr())) {
                    if(null == props[x + 1]) {
                        fCtx.PutNow(props[x].GetStr(), new Rtoken(Rtype.Bool, false));
                    } else {
                        if (props[x + 1].tp.Equals(Rtype.Word)) {
                            fCtx.PutNow(props[x + 1].GetWord().key, new Rtoken(Rtype.None, 0));
                        } else {
                            fCtx.PutNow(props[x + 1].GetStr(), new Rtoken(Rtype.None, 0));
                        }
                    }
                }
                x += 2;
            }

            

            return new Rsolver(code).Eval(fCtx);
        }


        public List<bool> GetQuoteListWithProps(List<Rtoken> propList) {
            List<bool> quoteList = new List<bool>();
            quoteList.AddRange(quotes);

            foreach(var item in propList) {
                int i = 0;
                while(i < props.Count) {
                    if (props[i].GetStr().Equals(item.GetStr())) {
                        if(null == props[i + 1]) {
                            
                        }else if (props[i + 1].tp.Equals(Rtype.GetWord)) {
                            quoteList.Add(true);
                        } else {
                            quoteList.Add(false);
                        }
                        break;
                    }

                    i += 2;
                }
            }

            return quoteList;
        }

    }
}
