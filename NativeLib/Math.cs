using System;
using System.Collections.Generic;
using System.Text;
using RML.Lang;

namespace RML.NativeLib {
    class Add : Rnative {
        public Add() {
            name = "add";
            argsLen = 2;
        }

        public override Rtoken Run(List<Rtoken> args, Rtable ctx) {
            Rtype ltype = args[0].tp;
            Rtype rtype = args[1].tp;

            if (ltype.Equals(Rtype.Int)) {
                if (rtype.Equals(Rtype.Int)) {
                    return AddIntInt(args);
                } else if (rtype.Equals(Rtype.Float)) {
                    return AddIntFloat(args);
                }

            } else if (ltype.Equals(Rtype.Float)) {
                if (rtype.Equals(Rtype.Int)) {
                    return AddFloatInt(args);
                } else if (rtype.Equals(Rtype.Float)) {
                    return AddFloatFloat(args);
                }
            
            }else if (ltype.Equals(Rtype.Str)) {
                if (rtype.Equals(Rtype.Str)) {
                    return AddStrStr(args);
                }
            }

            return ErrorInfo(args);
        }

        public Rtoken AddIntInt(List<Rtoken> args) {
            int argl = args[0].GetInt();
            int argr = args[1].GetInt();
            return new Rtoken(Rtype.Int, argl + argr);
        }

        public Rtoken AddIntFloat(List<Rtoken> args) {
            int argl = args[0].GetInt();
            decimal argr = args[1].GetFloat();
            return new Rtoken(Rtype.Float, argl + argr);
        }

        public Rtoken AddFloatInt(List<Rtoken> args) {
            decimal argl = args[0].GetFloat();
            int argr = args[1].GetInt();
            return new Rtoken(Rtype.Float, argl + argr);
        }

        public Rtoken AddFloatFloat(List<Rtoken> args) {
            decimal argl = args[0].GetFloat();
            decimal argr = args[1].GetFloat();
            return new Rtoken(Rtype.Float, argl + argr);
        }

        public Rtoken AddStrStr(List<Rtoken> args) {
            string argl = args[0].GetStr();
            string argr = args[1].GetStr();
            return new Rtoken(Rtype.Str, argl + argr);
        }

    }


    class Sub : Rnative {
        public Sub() {
            name = "sub";
            argsLen = 2;
        }

        public override Rtoken Run(List<Rtoken> args, Rtable ctx) {
            Rtype ltype = args[0].tp;
            Rtype rtype = args[1].tp;

            if (ltype.Equals(Rtype.Int)) {
                if (rtype.Equals(Rtype.Int)) {
                    return SubIntInt(args);
                } else if (rtype.Equals(Rtype.Float)) {
                    return SubIntFloat(args);
                }

            } else if (ltype.Equals(Rtype.Float)) {
                if (rtype.Equals(Rtype.Int)) {
                    return SubFloatInt(args);
                } else if (rtype.Equals(Rtype.Float)) {
                    return SubFloatFloat(args);
                }
            }

            return ErrorInfo(args);
        }

        public Rtoken SubIntInt(List<Rtoken> args) {
            int argl = args[0].GetInt();
            int argr = args[1].GetInt();
            return new Rtoken(Rtype.Int, argl - argr);
        }

        public Rtoken SubIntFloat(List<Rtoken> args) {
            int argl = args[0].GetInt();
            decimal argr = args[1].GetFloat();
            return new Rtoken(Rtype.Float, argl - argr);
        }

        public Rtoken SubFloatInt(List<Rtoken> args) {
            decimal argl = args[0].GetFloat();
            int argr = args[1].GetInt();
            return new Rtoken(Rtype.Float, argl - argr);
        }

        public Rtoken SubFloatFloat(List<Rtoken> args) {
            decimal argl = args[0].GetFloat();
            decimal argr = args[1].GetFloat();
            return new Rtoken(Rtype.Float, argl - argr);
        }
    }


    class Mul : Rnative {
        public Mul() {
            name = "mul";
            argsLen = 2;
        }

        public override Rtoken Run(List<Rtoken> args, Rtable ctx) {
            Rtype ltype = args[0].tp;
            Rtype rtype = args[1].tp;

            if (ltype.Equals(Rtype.Int)) {
                if (rtype.Equals(Rtype.Int)) {
                    return MulIntInt(args);
                } else if (rtype.Equals(Rtype.Float)) {
                    return MulIntFloat(args);
                }

            } else if (ltype.Equals(Rtype.Float)) {
                if (rtype.Equals(Rtype.Int)) {
                    return MulFloatInt(args);
                } else if (rtype.Equals(Rtype.Float)) {
                    return MulFloatFloat(args);
                }
            }

            return ErrorInfo(args);
        }

        public Rtoken MulIntInt(List<Rtoken> args) {
            int argl = args[0].GetInt();
            int argr = args[1].GetInt();
            return new Rtoken(Rtype.Int, argl * argr);
        }

        public Rtoken MulIntFloat(List<Rtoken> args) {
            int argl = args[0].GetInt();
            decimal argr = args[1].GetFloat();
            return new Rtoken(Rtype.Float, argl * argr);
        }

        public Rtoken MulFloatInt(List<Rtoken> args) {
            decimal argl = args[0].GetFloat();
            int argr = args[1].GetInt();
            return new Rtoken(Rtype.Float, argl * argr);
        }

        public Rtoken MulFloatFloat(List<Rtoken> args) {
            decimal argl = args[0].GetFloat();
            decimal argr = args[1].GetFloat();
            return new Rtoken(Rtype.Float, argl * argr);
        }



    }





    class Div : Rnative {
        public Div() {
            name = "div";
            argsLen = 2;
        }

        public override Rtoken Run(List<Rtoken> args, Rtable ctx) {
            Rtype ltype = args[0].tp;
            Rtype rtype = args[1].tp;

            if (ltype.Equals(Rtype.Int)) {
                if (rtype.Equals(Rtype.Int)) {
                    return DivIntInt(args);
                } else if (rtype.Equals(Rtype.Float)) {
                    return DivIntFloat(args);
                }

            } else if (ltype.Equals(Rtype.Float)) {
                if (rtype.Equals(Rtype.Int)) {
                    return DivFloatInt(args);
                } else if (rtype.Equals(Rtype.Float)) {
                    return DivFloatFloat(args);
                }
            }

            return ErrorInfo(args);
        }

        public Rtoken DivIntInt(List<Rtoken> args) {
            int argl = args[0].GetInt();
            int argr = args[1].GetInt();
            return new Rtoken(Rtype.Float, (decimal)argl / (decimal)argr);
        }

        public Rtoken DivIntFloat(List<Rtoken> args) {
            int argl = args[0].GetInt();
            decimal argr = args[1].GetFloat();
            return new Rtoken(Rtype.Float, argl / argr);
        }

        public Rtoken DivFloatInt(List<Rtoken> args) {
            decimal argl = args[0].GetFloat();
            int argr = args[1].GetInt();
            return new Rtoken(Rtype.Float, argl / argr);
        }

        public Rtoken DivFloatFloat(List<Rtoken> args) {
            decimal argl = args[0].GetFloat();
            decimal argr = args[1].GetFloat();
            return new Rtoken(Rtype.Float, argl / argr);
        }



    }



    class Mod : Rnative {
        public Mod() {
            name = "mod";
            argsLen = 2;
        }

        public override Rtoken Run(List<Rtoken> args, Rtable ctx) {
            Rtype ltype = args[0].tp;
            Rtype rtype = args[1].tp;

            if (ltype.Equals(Rtype.Int)) {
                if (rtype.Equals(Rtype.Int)) {
                    return ModIntInt(args);
                } else if (rtype.Equals(Rtype.Float)) {
                    return ModIntFloat(args);
                }

            } else if (ltype.Equals(Rtype.Float)) {
                if (rtype.Equals(Rtype.Int)) {
                    return ModFloatInt(args);
                } else if (rtype.Equals(Rtype.Float)) {
                    return ModFloatFloat(args);
                }
            }

            return ErrorInfo(args);
        }

        public Rtoken ModIntInt(List<Rtoken> args) {
            int argl = args[0].GetInt();
            int argr = args[1].GetInt();
            return new Rtoken(Rtype.Int, argl % argr);
        }

        public Rtoken ModIntFloat(List<Rtoken> args) {
            int argl = args[0].GetInt();
            decimal argr = args[1].GetFloat();
            return new Rtoken(Rtype.Float, argl % argr);
        }

        public Rtoken ModFloatInt(List<Rtoken> args) {
            decimal argl = args[0].GetFloat();
            int argr = args[1].GetInt();
            return new Rtoken(Rtype.Float, argl % argr);
        }

        public Rtoken ModFloatFloat(List<Rtoken> args) {
            decimal argl = args[0].GetFloat();
            decimal argr = args[1].GetFloat();
            return new Rtoken(Rtype.Float, argl % argr);
        }



    }


    class AddSet : Rnative {
        public AddSet() {
            name = "add-set";
            argsLen = 2;
        }

        public override Rtoken Run(List<Rtoken> args, Rtable ctx) {
            Rtype ltype = args[0].tp;
            Rtype rtype = args[1].tp;

            if (ltype.Equals(Rtype.Int)) {
                if (rtype.Equals(Rtype.Int)) {
                    return AddSetIntInt(args);
                } else if (rtype.Equals(Rtype.Float)) {
                    return AddSetIntFloat(args);
                }

            } else if (ltype.Equals(Rtype.Float)) {
                if (rtype.Equals(Rtype.Int)) {
                    return AddSetFloatInt(args);
                } else if (rtype.Equals(Rtype.Float)) {
                    return AddSetFloatFloat(args);
                }

            } else if (ltype.Equals(Rtype.Str)) {
                if (rtype.Equals(Rtype.Str)) {
                    return AddStrStr(args);
                }
            }

            return ErrorInfo(args);
        }

        public Rtoken AddSetIntInt(List<Rtoken> args) {
            if(Renv.threads > 1) {
                lock (args[0]) {
                    args[0].val = (int)args[0].val + (int)args[1].val;
                }
            } else {
                args[0].val = (int)args[0].val + (int)args[1].val;
            }
            return args[0];
        }

        public Rtoken AddSetIntFloat(List<Rtoken> args) {
            if (Renv.threads > 1) {
                lock (args[0]) {
                    args[0].tp = Rtype.Float;
                    args[0].val = args[0].GetInt() + (decimal)args[1].val;
                }
            } else {
                args[0].tp = Rtype.Float;
                args[0].val = args[0].GetInt() + (decimal)args[1].val;
            }
            return args[0];
        }

        public Rtoken AddSetFloatInt(List<Rtoken> args) {
            if (Renv.threads > 1) {
                lock (args[0]) {
                    args[0].val = (decimal)args[0].val + args[1].GetInt();
                }
            } else {
                args[0].val = (decimal)args[0].val + args[1].GetInt();
            }
            return args[0];
        }

        public Rtoken AddSetFloatFloat(List<Rtoken> args) {
            if (Renv.threads > 1) {
                lock (args[0]) {
                    args[0].val = (decimal)args[0].val + (decimal)args[1].val;
                }
            } else {
                args[0].val = (decimal)args[0].val + (decimal)args[1].val;
            }
            return args[0];
        }

        public Rtoken AddStrStr(List<Rtoken> args) {
            if (Renv.threads > 1) {
                lock (args[0]) {
                    args[0].val = args[0].GetStr() + args[1].GetStr();
                }
            } else {
                args[0].val = args[0].GetStr() + args[1].GetStr();
            }
            return args[0];
        }

    }





    class SubSet : Rnative {
        public SubSet() {
            name = "sub-set";
            argsLen = 2;
        }

        public override Rtoken Run(List<Rtoken> args, Rtable ctx) {
            Rtype ltype = args[0].tp;
            Rtype rtype = args[1].tp;

            if (ltype.Equals(Rtype.Int)) {
                if (rtype.Equals(Rtype.Int)) {
                    return SubSetIntInt(args);
                } else if (rtype.Equals(Rtype.Float)) {
                    return SubSetIntFloat(args);
                }

            } else if (ltype.Equals(Rtype.Float)) {
                if (rtype.Equals(Rtype.Int)) {
                    return SubSetFloatInt(args);
                } else if (rtype.Equals(Rtype.Float)) {
                    return SubSetFloatFloat(args);
                }

            }

            return ErrorInfo(args);
        }

        public Rtoken SubSetIntInt(List<Rtoken> args) {
            if (Renv.threads > 1) {
                lock (args[0]) {
                    args[0].val = (int)args[0].val - (int)args[1].val;
                }
            } else {
                args[0].val = (int)args[0].val - (int)args[1].val;
            }
            return args[0];
        }

        public Rtoken SubSetIntFloat(List<Rtoken> args) {
            if (Renv.threads > 1) {
                lock (args[0]) {
                    args[0].tp = Rtype.Float;
                    args[0].val = args[0].GetInt() - (decimal)args[1].val;
                }
            } else {
                args[0].tp = Rtype.Float;
                args[0].val = args[0].GetInt() - (decimal)args[1].val;
            }
            return args[0];
        }

        public Rtoken SubSetFloatInt(List<Rtoken> args) {
            if (Renv.threads > 1) {
                lock (args[0]) {
                    args[0].val = (decimal)args[0].val - args[1].GetInt();
                }
            } else {
                args[0].val = (decimal)args[0].val - args[1].GetInt();
            }
            return args[0];
        }

        public Rtoken SubSetFloatFloat(List<Rtoken> args) {
            if (Renv.threads > 1) {
                lock (args[0]) {
                    args[0].val = (decimal)args[0].val - (decimal)args[1].val;
                }
            } else {
                args[0].val = (decimal)args[0].val - (decimal)args[1].val;
            }
            return args[0];
        }

    }



    class MulSet : Rnative {
        public MulSet() {
            name = "mul-set";
            argsLen = 2;
        }

        public override Rtoken Run(List<Rtoken> args, Rtable ctx) {
            Rtype ltype = args[0].tp;
            Rtype rtype = args[1].tp;

            if (ltype.Equals(Rtype.Int)) {
                if (rtype.Equals(Rtype.Int)) {
                    return MulSetIntInt(args);
                } else if (rtype.Equals(Rtype.Float)) {
                    return MulSetIntFloat(args);
                }

            } else if (ltype.Equals(Rtype.Float)) {
                if (rtype.Equals(Rtype.Int)) {
                    return MulSetFloatInt(args);
                } else if (rtype.Equals(Rtype.Float)) {
                    return MulSetFloatFloat(args);
                }

            }

            return ErrorInfo(args);
        }

        public Rtoken MulSetIntInt(List<Rtoken> args) {
            if (Renv.threads > 1) {
                lock (args[0]) {
                    args[0].val = (int)args[0].val * (int)args[1].val;
                }
            } else {
                args[0].val = (int)args[0].val * (int)args[1].val;
            }
            return args[0];
        }

        public Rtoken MulSetIntFloat(List<Rtoken> args) {
            if (Renv.threads > 1) {
                lock (args[0]) {
                    args[0].tp = Rtype.Float;
                    args[0].val = args[0].GetInt() * (decimal)args[1].val;
                }
            } else {
                args[0].tp = Rtype.Float;
                args[0].val = args[0].GetInt() * (decimal)args[1].val;
            }
            return args[0];
        }

        public Rtoken MulSetFloatInt(List<Rtoken> args) {
            if (Renv.threads > 1) {
                lock (args[0]) {
                    args[0].val = (decimal)args[0].val * args[1].GetInt();
                }
            } else {
                args[0].val = (decimal)args[0].val * args[1].GetInt();
            }
            return args[0];
        }

        public Rtoken MulSetFloatFloat(List<Rtoken> args) {
            if (Renv.threads > 1) {
                lock (args[0]) {
                    args[0].val = (decimal)args[0].val * (decimal)args[1].val;
                }
            } else {
                args[0].val = (decimal)args[0].val * (decimal)args[1].val;
            }
            return args[0];
        }

    }




    class DivSet : Rnative {
        public DivSet() {
            name = "div-set";
            argsLen = 2;
        }

        public override Rtoken Run(List<Rtoken> args, Rtable ctx) {
            Rtype ltype = args[0].tp;
            Rtype rtype = args[1].tp;

            if (ltype.Equals(Rtype.Int)) {
                if (rtype.Equals(Rtype.Int)) {
                    return DivSetIntInt(args);
                } else if (rtype.Equals(Rtype.Float)) {
                    return DivSetIntFloat(args);
                }

            } else if (ltype.Equals(Rtype.Float)) {
                if (rtype.Equals(Rtype.Int)) {
                    return DivSetFloatInt(args);
                } else if (rtype.Equals(Rtype.Float)) {
                    return DivSetFloatFloat(args);
                }

            }

            return ErrorInfo(args);
        }

        public Rtoken DivSetIntInt(List<Rtoken> args) {
            if (Renv.threads > 1) {
                lock (args[0]) {
                    args[0].tp = Rtype.Float;
                    args[0].val = (decimal)args[0].GetInt() / (decimal)args[1].GetInt();
                }
            } else {
                args[0].tp = Rtype.Float;
                args[0].val = (decimal)args[0].GetInt() / (decimal)args[1].GetInt();
            }
            return args[0];
        }

        public Rtoken DivSetIntFloat(List<Rtoken> args) {
            if (Renv.threads > 1) {
                lock (args[0]) {
                    args[0].tp = Rtype.Float;
                    args[0].val = args[0].GetInt() / (decimal)args[1].val;
                }
            } else {
                args[0].tp = Rtype.Float;
                args[0].val = args[0].GetInt() / (decimal)args[1].val;
            }
            return args[0];
        }

        public Rtoken DivSetFloatInt(List<Rtoken> args) {
            if (Renv.threads > 1) {
                lock (args[0]) {
                    args[0].val = (decimal)args[0].val / args[1].GetInt();
                }
            } else {
                args[0].val = (decimal)args[0].val / args[1].GetInt();
            }
            return args[0];
        }

        public Rtoken DivSetFloatFloat(List<Rtoken> args) {
            if (Renv.threads > 1) {
                lock (args[0]) {
                    args[0].val = (decimal)args[0].val / (decimal)args[1].val;
                }
            } else {
                args[0].val = (decimal)args[0].val / (decimal)args[1].val;
            }
            return args[0];
        }

    }




    class ModSet : Rnative {
        public ModSet() {
            name = "mod-set";
            argsLen = 2;
        }

        public override Rtoken Run(List<Rtoken> args, Rtable ctx) {
            Rtype ltype = args[0].tp;
            Rtype rtype = args[1].tp;

            if (ltype.Equals(Rtype.Int)) {
                if (rtype.Equals(Rtype.Int)) {
                    return ModSetIntInt(args);
                } else if (rtype.Equals(Rtype.Float)) {
                    return ModSetIntFloat(args);
                }

            } else if (ltype.Equals(Rtype.Float)) {
                if (rtype.Equals(Rtype.Int)) {
                    return ModSetFloatInt(args);
                } else if (rtype.Equals(Rtype.Float)) {
                    return ModSetFloatFloat(args);
                }

            }

            return ErrorInfo(args);
        }

        public Rtoken ModSetIntInt(List<Rtoken> args) {
            if (Renv.threads > 1) {
                lock (args[0]) {              
                    args[0].val = (int)args[0].val % (int)args[1].val;
                }
            } else {
                args[0].val = (int)args[0].val % (int)args[1].val;
            }
            return args[0];
        }

        public Rtoken ModSetIntFloat(List<Rtoken> args) {
            if (Renv.threads > 1) {
                lock (args[0]) {
                    args[0].tp = Rtype.Float;
                    args[0].val = args[0].GetInt() % (decimal)args[1].val;
                }
            } else {
                args[0].tp = Rtype.Float;
                args[0].val = args[0].GetInt() % (decimal)args[1].val;
            }
            return args[0];
        }

        public Rtoken ModSetFloatInt(List<Rtoken> args) {
            if (Renv.threads > 1) {
                lock (args[0]) {
                    args[0].val = (decimal)args[0].val % args[1].GetInt();
                }
            } else {
                args[0].val = (decimal)args[0].val % args[1].GetInt();
            }
            return args[0];
        }

        public Rtoken ModSetFloatFloat(List<Rtoken> args) {
            if (Renv.threads > 1) {
                lock (args[0]) {
                    args[0].val = (decimal)args[0].val % (decimal)args[1].val;
                }
            } else {
                args[0].val = (decimal)args[0].val % (decimal)args[1].val;
            }
            return args[0];
        }

    }



}
