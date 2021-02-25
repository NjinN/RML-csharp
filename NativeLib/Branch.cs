using System;
using System.Collections.Generic;
using System.Text;
using RML.Lang;

namespace RML.NativeLib {
    class Rif : Rnative {
        public Rif() {
            name = "if";
            argsLen = 2;
        }

        public override Rtoken Run(List<Rtoken> args, Rtable ctx) {
            Rtype type0 = args[0].tp;
            Rtype type1 = args[1].tp;

            if (type0.Equals(Rtype.Bool)) {
                if (type1.Equals(Rtype.Block)) {
                    if (args[0].GetBool()) {
                        return new Rsolver(args[1].GetList()).Eval(ctx);
                    } else {
                        return new Rtoken(Rtype.Flow, new Rflow("pass"));
                    }

                }else if (type1.Equals(Rtype.Str)) {
                    if (args[0].GetBool()) {
                        return new Rsolver(args[1].GetStr()).Eval(ctx);
                    } else {
                        return new Rtoken(Rtype.Flow, new Rflow("pass"));
                    }
                }
            }


            return new Rtoken(Rtype.Err, "Error: Types mismatch for native::if");
        }
    }

    class Relif : Rnative {
        public Relif() {
            name = "elif";
            argsLen = 3;
        }

        public override Rtoken Run(List<Rtoken> args, Rtable ctx) {
            Rtype type0 = args[0].tp;
            Rtype type1 = args[1].tp;
            Rtype type2 = args[2].tp;

            if (type0.Equals(Rtype.Flow)) {

            } else {
                return new Rtoken();
            }


            if (type1.Equals(Rtype.Bool)) {
                if (type2.Equals(Rtype.Block)) {
                    if (args[1].GetBool()) {
                        return new Rsolver(args[2].GetList()).Eval(ctx);
                    } else {
                        return new Rtoken(Rtype.Flow, new Rflow("pass"));
                    }

                } else if (type2.Equals(Rtype.Str)) {
                    if (args[1].GetBool()) {
                        return new Rsolver(args[2].GetStr()).Eval(ctx);
                    } else {
                        return new Rtoken(Rtype.Flow, new Rflow("pass"));
                    }
                }
            }


            return new Rtoken(Rtype.Err, "Error: Types mismatch for native::elif");
        }
    }


    class Relse : Rnative {
        public Relse() {
            name = "else";
            argsLen = 2;
        }

        public override Rtoken Run(List<Rtoken> args, Rtable ctx) {
            Rtype type0 = args[0].tp;
            Rtype type1 = args[1].tp;


            if (type0.Equals(Rtype.Flow)) {
                if (args[0].GetFlow().name.Equals("pass")) {
                    if (type1.Equals(Rtype.Block)) {
                        return new Rsolver(args[1].GetList()).Eval(ctx);
                    } else if (type1.Equals(Rtype.Str)) {
                        return new Rsolver(args[1].GetStr()).Eval(ctx);
                    }
                }

            } else {
                return new Rtoken();
            }

            return new Rtoken(Rtype.Err, "Error: Types mismatch for native::else");
        }
    }


    class Reither : Rnative {
        public Reither() {
            name = "either";
            argsLen = 3;
        }

        public override Rtoken Run(List<Rtoken> args, Rtable ctx) {
            Rtype type0 = args[0].tp;
            Rtype type1 = args[1].tp;
            Rtype type2 = args[2].tp;

            if (type0.Equals(Rtype.Bool)) {
                if (type1.Equals(Rtype.Block) && type2.Equals(Rtype.Block)) {
                    if (args[0].GetBool()) {
                        return new Rsolver(args[1].GetList()).Eval(ctx);
                    } else {
                        return new Rsolver(args[2].GetList()).Eval(ctx);
                    }

                } else if (type1.Equals(Rtype.Str) && type2.Equals(Rtype.Block)) {
                    if (args[0].GetBool()) {
                        return new Rsolver(args[1].GetStr()).Eval(ctx);
                    } else {
                        return new Rsolver(args[2].GetStr()).Eval(ctx);
                    }
                }
            }


            return new Rtoken(Rtype.Err, "Error: Types mismatch for native::either");
        }
    }

}
