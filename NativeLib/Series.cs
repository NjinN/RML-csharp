using System;
using System.Collections.Generic;
using System.Text;
using RML.Lang;

namespace RML.NativeLib {
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


}
