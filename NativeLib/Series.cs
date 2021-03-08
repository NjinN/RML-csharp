using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using RML.Lang;

namespace RML.NativeLib {
    [Serializable]
    class Rlen : Rnative {
        public Rlen() {
            name = "len?";
            argsLen = 1;
        }

        public override Rtoken Run(List<Rtoken> args, Rtable ctx) {
            switch (args[0].tp) {
                case Rtype.Str:
                    return new Rtoken(Rtype.Int, args[0].GetStr().Length);
                case Rtype.Block:
                case Rtype.Paren:
                    return new Rtoken(Rtype.Int, args[0].GetList().Count);
                case Rtype.Object:
                    return new Rtoken(Rtype.Int, args[0].GetTable().table.Count);

                default:
                    return ErrorInfo(args);
            }
        }
    }


    [Serializable]
    class Rtake : Rnative {
        public Rtake() {
            name = "take";
            argsLen = 3;
        }

        public override Rtoken Run(List<Rtoken> args, Rtable ctx) {
            return (args[0].tp, args[1].tp, args[2].tp) switch {
                (Rtype.Str, Rtype.Int, Rtype.Int) => TakeStr(args),
                (Rtype.Block, Rtype.Int, Rtype.Int) => TakeBlk(args),
                (Rtype.Paren, Rtype.Int, Rtype.Int) => TakeBlk(args),

                _ => ErrorInfo(args)
            };
        }

        public Rtoken TakeStr(List<Rtoken> args) {
            int at = args[1].GetInt();
            int part = args[2].GetInt();
            if (at < 1) {
                return new Rtoken(Rtype.Err, "Error: Index out of bound for native::_take");
            }
            if (part <= 0) {
                return new Rtoken(Rtype.Err, "Error: error take/part");
            }

            int len = args[0].GetStr().Length;

            if(at > len) {
                return new Rtoken(Rtype.None, 0);
            }
            if(at + part > len + 1) {
                part = len - at + 1;
            }

            if(part == 1) {
                char c = args[0].GetStr().ToCharArray()[at - 1];
                args[0].val = args[0].GetStr().Remove(at - 1, 1);
                return new Rtoken(Rtype.Char, c);
            } else {
                string s = args[0].GetStr().Substring(at - 1, part);
                args[0].val = args[0].GetStr().Remove(at - 1, part);
                return new Rtoken(Rtype.Str, s);
            }

        }


        public Rtoken TakeBlk(List<Rtoken> args) {
            int at = args[1].GetInt();
            int part = args[2].GetInt();
            if (at < 1) {
                return new Rtoken(Rtype.Err, "Error: Index out of bound for native::_take");
            }
            if (part <= 0) {
                return new Rtoken(Rtype.Err, "Error: error take/part");
            }

            int len = args[0].GetList().Count;

            if (at > len) {
                return new Rtoken(Rtype.None, 0);
            }
            if (at + part > len + 1) {
                part = len - at + 1;
            }

            if (part == 1) {
                Rtoken tk = ((List<Rtoken>)args[0].val)[at - 1];
                ((List<Rtoken>)args[0].val).RemoveAt(at - 1);
                return tk;
            } else {
                List<Rtoken> list = ((List<Rtoken>)args[0].val).GetRange(at - 1, part);
                ((List<Rtoken>)args[0].val).RemoveRange(at - 1, part);
                return new Rtoken(args[0].tp, list);
            }

        }

    }



    [Serializable]
    class Rappend : Rnative {
        public Rappend() {
            name = "_append";
            argsLen = 3;
        }

        public override Rtoken Run(List<Rtoken> args, Rtable ctx) {
            return (args[0].tp, args[1].tp, args[2].tp) switch {
                (Rtype.Str, Rtype.Str, Rtype.Bool) => AppendStrStr(args),
                (Rtype.Str, Rtype.Char, Rtype.Bool) => AppendStrChar(args),
                (Rtype.Str, _, Rtype.Bool) => AppendStrAny(args),
                (Rtype.Block, Rtype.Block, Rtype.Bool) => AppendBlkBlk(args),
                (Rtype.Block, Rtype.Paren, Rtype.Bool) => AppendBlkBlk(args),
                (Rtype.Paren, Rtype.Paren, Rtype.Bool) => AppendBlkBlk(args),
                (Rtype.Paren, Rtype.Block, Rtype.Bool) => AppendBlkBlk(args),
                (Rtype.Block, _, Rtype.Bool) => AppendBlkAny(args),
                (Rtype.Paren, _, Rtype.Bool) => AppendBlkAny(args),


                _ => ErrorInfo(args)
            };
        }

        public Rtoken AppendStrStr(List<Rtoken> args) {
            if (args[2].GetBool()) {
                args[0].val = args[0].GetStr() + args[1].ToStr();
            } else {
                args[0].val = args[0].GetStr() + args[1].GetStr();
            }
            return args[0];
        }

        public Rtoken AppendStrChar(List<Rtoken> args) {
            if (args[2].GetBool()) {
                args[0].val = args[0].GetStr() + args[1].ToStr();
            } else {
                args[0].val = args[0].GetStr() + args[1].GetChar();
            }
            return args[0];
        }

        public Rtoken AppendStrAny(List<Rtoken> args) {
            args[0].val = args[0].GetStr() + args[1].ToStr();
            return args[0];
        }


        public Rtoken AppendBlkBlk(List<Rtoken> args) {
            if (args[2].GetBool()) {
                ((List<Rtoken>)args[0].val).Add(args[1]);
            } else {
                ((List<Rtoken>)args[0].val).AddRange(args[1].GetList());
            }
            return args[0];
        }


        public Rtoken AppendBlkAny(List<Rtoken> args) {
            ((List<Rtoken>)args[0].val).Add(args[1]);
            return args[0];
        }


    }



    [Serializable]
    class Rinsert : Rnative {
        public Rinsert() {
            name = "_insert";
            argsLen = 4;
        }

        public override Rtoken Run(List<Rtoken> args, Rtable ctx) {
            if (!args[2].tp.Equals(Rtype.Int) || !args[3].tp.Equals(Rtype.Bool)) {
                return ErrorInfo(args);
            }

            int at = args[2].GetInt();
            if(at < 1) {
                return new Rtoken(Rtype.Err, "Error: Index out of bound for native::_insert");
            }

            return (args[0].tp, args[1].tp) switch {
                (Rtype.Str, Rtype.Str) => InsertStrStr(args, at),
                (Rtype.Str, Rtype.Char) => InsertStrChar(args, at),
                (Rtype.Str, _) => InsertStrAny(args, at),
                (Rtype.Block, Rtype.Block) => InsertBlkBlk(args, at),
                (Rtype.Block, Rtype.Paren) => InsertBlkBlk(args, at),
                (Rtype.Paren, Rtype.Paren) => InsertBlkBlk(args, at),
                (Rtype.Paren, Rtype.Block) => InsertBlkBlk(args, at),
                (Rtype.Block, _) => InsertBlkAny(args, at),
                (Rtype.Paren, _) => InsertBlkAny(args, at),


                _ => ErrorInfo(args)
            };
        }

        public Rtoken InsertStrStr(List<Rtoken> args, int at) {
            if(at > args[0].GetStr().Length + 1) {
                return new Rtoken(Rtype.Err, "Error: Index out of bound for native::_insert");
            }
            if (args[3].GetBool()) {
                args[0].val = args[0].GetStr().Insert(at - 1, args[1].ToStr());
            } else {
                args[0].val = args[0].GetStr().Insert(at - 1, args[1].GetStr());
            }
            return args[0];
        }

        public Rtoken InsertStrChar(List<Rtoken> args, int at) {
            if (at > args[0].GetStr().Length + 1) {
                return new Rtoken(Rtype.Err, "Error: Index out of bound for native::_insert");
            }
            if (args[3].GetBool()) {
                args[0].val = args[0].GetStr().Insert(at - 1, args[1].ToStr());
            } else {
                args[0].val = args[0].GetStr().Insert(at - 1, args[1].GetChar().ToString());
            }
            return args[0];
        }

        public Rtoken InsertStrAny(List<Rtoken> args, int at) {
            if (at > args[0].GetStr().Length + 1) {
                return new Rtoken(Rtype.Err, "Error: Index out of bound for native::_insert");
            }
            args[0].val = args[0].GetStr().Insert(at - 1, args[1].ToStr());
            return args[0];
        }


        public Rtoken InsertBlkBlk(List<Rtoken> args, int at) {
            if (at > args[0].GetList().Count + 1) {
                return new Rtoken(Rtype.Err, "Error: Index out of bound for native::_insert");
            }
            if (args[3].GetBool()) {
                ((List<Rtoken>)args[0].val).Insert(at - 1, args[1]);
            } else {
                ((List<Rtoken>)args[0].val).InsertRange(at - 1, args[1].GetList());
            }
            return args[0];
        }


        public Rtoken InsertBlkAny(List<Rtoken> args, int at) {
            if (at > args[0].GetList().Count + 1) {
                return new Rtoken(Rtype.Err, "Error: Index out of bound for native::_insert");
            }
            ((List<Rtoken>)args[0].val).Insert(at - 1, args[1]);
            return args[0];
        }


    }




    [Serializable]
    class Rat : Rnative {
        public Rat() {
            name = "_at";
            argsLen = 2;
        }

        public override Rtoken Run(List<Rtoken> args, Rtable ctx) {
            if (!args[1].tp.Equals(Rtype.Int)) {
                return ErrorInfo(args);
            }

            int at = args[1].GetInt();
            
            try {
                switch (args[0].tp) {
                    case Rtype.Str:
                        return new Rtoken(Rtype.Str, args[0].GetStr().Substring(at-1));
                    case Rtype.Block:
                    case Rtype.Paren:
                        return new Rtoken(args[0].tp, args[0].GetList().GetRange(at - 1, args[0].GetList().Count - at + 1));

                    default:
                        return ErrorInfo(args);
                }
            }catch(Exception e) {
                return new Rtoken(Rtype.Err, "Error: " + e.Message);
            }
            
        }
    }


    [Serializable]
    class Rindex : Rnative {
        public Rindex() {
            name = "_index";
            argsLen = 4;
        }

        public override Rtoken Run(List<Rtoken> args, Rtable ctx) {
            if (!args[2].tp.Equals(Rtype.Int) || !args[3].tp.Equals(Rtype.Bool)) {
                return ErrorInfo(args);
            }

            if(args[2].GetInt() < 1) {
                return new Rtoken(Rtype.Err, "Error: Index out of bound for native::_index");
            }

            return (args[0].tp, args[1].tp, args[3].GetBool()) switch {
                (Rtype.Str, Rtype.Char, false) => new Rtoken(Rtype.Int, IndexStrChar(args)),
                (Rtype.Str, Rtype.Str, false) => new Rtoken(Rtype.Int, IndexStrStr(args)),
                (Rtype.Block, Rtype.Block, false) => new Rtoken(Rtype.Int, IndexBlkBlk(args)),
                (Rtype.Block, _, false) => new Rtoken(Rtype.Int, IndexBlkAny(args)),

                (Rtype.Str, Rtype.Char, true) => new Rtoken(Rtype.Int, IndexStrCharLast(args)),
                (Rtype.Str, Rtype.Str, true) => new Rtoken(Rtype.Int, IndexStrStrLast(args)),
                (Rtype.Block, Rtype.Block, true) => new Rtoken(Rtype.Int, IndexBlkBlkLast(args)),
                (Rtype.Block, _, true) => new Rtoken(Rtype.Int, IndexBlkAnyLast(args)),


                _ => ErrorInfo(args)
            };

        }

        public static int IndexStrChar(List<Rtoken> args) {
            int i = args[2].GetInt() - 1;
            char[] cs = args[0].GetStr().ToCharArray();
            while (i < cs.Length) {
                if (cs[i].Equals(args[1].GetChar())) {
                    return i + 1;
                }
                i++;
            }
            return 0;
        }

        public static int IndexStrStr(List<Rtoken> args) {
            int i = args[2].GetInt() - 1;
            string sStr = args[0].GetStr();
            string tStr = args[1].GetStr();
            if (tStr.Length > sStr.Length) {
                return 0;
            }

            while (i < sStr.Length && (i + tStr.Length) <= sStr.Length) {
                if(sStr.Substring(i, tStr.Length).Equals(tStr)) {
                    return i + 1;
                }
                i++;
            }
            return 0;
        }

        public static int IndexBlkAny(List<Rtoken> args) {
            int i = args[2].GetInt() - 1;
            List<Rtoken> list = args[0].GetList();
            while(i < list.Count) {
                if (list[i].Eq(args[1])) {
                    return i + 1;
                }
                i++;
            }
            return 0;
        }
        public static int IndexBlkBlk(List<Rtoken> args) {
            int i = args[2].GetInt() - 1;
            List<Rtoken> sList = args[0].GetList();
            List<Rtoken> tList = args[1].GetList();
            if(tList.Count > sList.Count) {
                return 0;
            }

            while (i < sList.Count && (i + tList.Count) <= sList.Count) {
                int j = 0;
                bool match = true;
                while(j < tList.Count) {
                    if (!sList[i + j].Eq(tList[j])){
                        match = false;
                        break;
                    }
                    j++;
                }
                if (match) {
                    return i + 1;
                }

                i++;
            }
            return 0;
        }




        public static int IndexStrCharLast(List<Rtoken> args) {
            char[] cs = args[0].GetStr().ToCharArray();
            int i = cs.Length - 1;
            while (i > args[2].GetInt() - 1 && i > 0) {
                if (cs[i].Equals(args[1].GetChar())) {
                    return i + 1;
                }
                i--;
            }
            return 0;
        }

        public static int IndexStrStrLast(List<Rtoken> args) {
            string sStr = args[0].GetStr();
            string tStr = args[1].GetStr();
            if (tStr.Length > sStr.Length) {
                return 0;
            }
            int i = sStr.Length - tStr.Length - 1;
            while (i > args[2].GetInt() - 1 && i > 0) {
                if (sStr.Substring(i, tStr.Length).Equals(tStr)) {
                    return i + 1;
                }
                i--;
            }
            return 0;
        }

        public static int IndexBlkAnyLast(List<Rtoken> args) {
            List<Rtoken> list = args[0].GetList();
            int i = list.Count - 1;
            while (i > args[2].GetInt() - 1 && i > 0) {
                if (list[i].Eq(args[1])) {
                    return i + 1;
                }
                i--;
            }
            return 0;
        }
        public static int IndexBlkBlkLast(List<Rtoken> args) {
            List<Rtoken> sList = args[0].GetList();
            List<Rtoken> tList = args[1].GetList();
            if (tList.Count > sList.Count) {
                return 0;
            }
            int i = sList.Count - tList.Count - 1;
            while (i > args[2].GetInt() - 1 && i > 0) {
                int j = 0;
                bool match = true;
                while (j < tList.Count) {
                    if (!sList[i + j].Eq(tList[j])) {
                        match = false;
                        break;
                    }
                    j++;
                }
                if (match) {
                    return i + 1;
                }

                i--;
            }
            return 0;
        }


    }



    [Serializable]
    class Rfind : Rnative {
        public Rfind() {
            name = "_find";
            argsLen = 4;
        }

        public override Rtoken Run(List<Rtoken> args, Rtable ctx) {
            if (!args[2].tp.Equals(Rtype.Int) || !args[3].tp.Equals(Rtype.Bool)) {
                return ErrorInfo(args);
            }

            if (args[2].GetInt() < 1) {
                return new Rtoken(Rtype.Err, "Error: Index out of bound for native::_find");
            }

            if (args[0].tp.Equals(Rtype.Str)) {
                return FindStr(args);
            }else if (args[0].tp.Equals(Rtype.Block)) {
                return FindBlk(args);
            } else {
                return ErrorInfo(args);
            }

        }

        public Rtoken FindStr(List<Rtoken> args) {
            int i = 0;
            bool last = args[3].GetBool();
            if (last) {
                if (args[1].tp.Equals(Rtype.Char)) {
                    i = Rindex.IndexStrCharLast(args);
                }else if (args[1].tp.Equals(Rtype.Str)) {
                    i = Rindex.IndexStrStrLast(args);
                }

            } else {
                if (args[1].tp.Equals(Rtype.Char)) {
                    i = Rindex.IndexStrChar(args);
                } else if (args[1].tp.Equals(Rtype.Str)) {
                    i = Rindex.IndexStrStr(args);
                 }
            }

            if(i == 0) {
                return new Rtoken(Rtype.Str, "");
            }
            return new Rtoken(Rtype.Str, args[0].GetStr().Substring(i - 1));
        }


        public Rtoken FindBlk(List<Rtoken> args) {
            int i = 0;
            bool last = args[3].GetBool();
            if (last) {
                if (args[1].tp.Equals(Rtype.Block)) {
                    i = Rindex.IndexBlkBlkLast(args);
                } else {
                    i = Rindex.IndexBlkAnyLast(args);
                }

            } else {
                if (args[1].tp.Equals(Rtype.Block)) {
                    i = Rindex.IndexBlkBlk(args);
                } else  {
                    i = Rindex.IndexBlkBlk(args);
                }
            }

            if (i == 0) {
                return new Rtoken(Rtype.Block, new List<Rtoken>());
            }
            return new Rtoken(Rtype.Block, args[0].GetList().GetRange(i - 1, args[0].GetList().Count - i + 1));
        }

    }






    [Serializable]
    class Rreplace : Rnative {
        public Rreplace() {
            name = "_replace";
            argsLen = 7;
        }

        public override Rtoken Run(List<Rtoken> args, Rtable ctx) {
            if (!args[3].tp.Equals(Rtype.Int) || !args[4].tp.Equals(Rtype.Bool) || !args[5].tp.Equals(Rtype.Int) || !args[6].tp.Equals(Rtype.Bool)) {
                return ErrorInfo(args);
            }

            if (args[3].GetInt() < 1) {
                return new Rtoken(Rtype.Err, "Error: Index out of bound for native::_replace");
            }

            return (args[0].tp, args[1].tp) switch {
                (Rtype.Str, _) => ReplaceStr(args),
                (Rtype.Block, _) => ReplaceBlk(args),

                _ => ErrorInfo(args)
            };

        }

        public Rtoken ReplaceStr(List<Rtoken> args) {
            int i = 0;
            int times = args[5].GetInt();
            bool only = args[6].GetBool();

            do {
                if (args[1].tp.Equals(Rtype.Char)) {
                    i = Rindex.IndexStrChar(new List<Rtoken>() { args[0], args[1], args[3], args[4] });
                    if(i > 0) {
                        Regex r = new Regex(args[1].GetChar().ToString());
                        if (only) {
                            args[0].val = r.Replace(args[0].GetStr(), args[2].ToStr(), 1, i - 1);
                        } else {
                            if (args[2].tp.Equals(Rtype.Char)) {
                                args[0].val = r.Replace(args[0].GetStr(), args[2].GetChar().ToString(), 1, i - 1);
                            } else if (args[2].tp.Equals(Rtype.Str)) {
                                args[0].val = r.Replace(args[0].GetStr(), args[2].GetStr(), 1, i - 1);
                            } else {
                                args[0].val = r.Replace(args[0].GetStr(), args[2].ToStr(), 1, i - 1);
                            }
                        }
                            
                    }
                } else if (args[1].tp.Equals(Rtype.Str)) {
                    i = Rindex.IndexStrStr(new List<Rtoken>() { args[0], args[1], args[3], args[4] });
                    if (i > 0) {
                        Regex r = new Regex(args[1].GetStr());
                        if (only) {
                            args[0].val = r.Replace(args[0].GetStr(), args[2].ToStr(), 1, i - 1);
                        } else {
                            if (args[2].tp.Equals(Rtype.Char)) {
                                args[0].val = r.Replace(args[0].GetStr(), args[2].GetChar().ToString(), 1, i - 1);
                            } else if (args[2].tp.Equals(Rtype.Str)) {
                                args[0].val = r.Replace(args[0].GetStr(), args[2].GetStr(), 1, i - 1);
                            } else {
                                args[0].val = r.Replace(args[0].GetStr(), args[2].ToStr(), 1, i - 1);
                            }
                        }

                    }
                }

                times--;
            } while (i > 0 && times > 0);

            return args[0];
        }


        public Rtoken ReplaceBlk(List<Rtoken> args) {
            int i = 0;
            int times = args[5].GetInt();
            bool only = args[6].GetBool();

            do {
                if (args[1].tp.Equals(Rtype.Block)) {
                    i = Rindex.IndexBlkBlk(new List<Rtoken>() { args[0], args[1], args[3], args[4] });
                    if (i > 0) {
                        if (only) {
                            args[0].GetList().RemoveRange(i - 1, args[1].GetList().Count);
                            args[0].GetList().Insert(i - 1, args[2]);
                        } else {
                            if (args[2].tp.Equals(Rtype.Block)) {
                                args[0].GetList().RemoveRange(i - 1, args[1].GetList().Count);
                                args[0].GetList().InsertRange(i - 1, args[2].GetList());
                            } else {
                                args[0].GetList().RemoveRange(i - 1, args[1].GetList().Count);
                                args[0].GetList().Insert(i - 1, args[2]);
                            }
                        }

                    }
                } else {
                    i = Rindex.IndexBlkAny(new List<Rtoken>() { args[0], args[1], args[3], args[4] });
                    if (i > 0) {
                        if (only) {
                            args[0].GetList().RemoveRange(i - 1, args[1].GetList().Count);
                            args[0].GetList().Insert(i - 1, args[2]);
                        } else {
                            if (args[2].tp.Equals(Rtype.Block)) {
                                args[0].GetList().RemoveRange(i - 1, args[1].GetList().Count);
                                args[0].GetList().InsertRange(i - 1, args[2].GetList());
                            } else {
                                args[0].GetList().RemoveRange(i - 1, args[1].GetList().Count);
                                args[0].GetList().Insert(i - 1, args[2]);
                            }
                        }

                    }
                }

                times--;
            } while (i > 0 && times > 0);


            return args[0];
        }

    }


}
