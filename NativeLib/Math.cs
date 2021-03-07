using System;
using System.Collections.Generic;
using System.Text;
using RML.Lang;

namespace RML.NativeLib {
    [Serializable]
    class Add : Rnative {
        public Add() {
            name = "add";
            argsLen = 2;
        }

        public override Rtoken Run(List<Rtoken> args, Rtable ctx) {
            return (args[0].tp, args[1].tp) switch {
                (Rtype.Int, Rtype.Int) =>  AddIntInt(args) ,
                (Rtype.Int, Rtype.Float) => AddIntFloat(args),
                (Rtype.Float, Rtype.Int) => AddFloatInt(args),
                (Rtype.Float, Rtype.Float) => AddFloatFloat(args),
                (Rtype.Str, Rtype.Str) => AddStrStr(args),
                (_, _) => ErrorInfo(args)
            };
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
            return (args[0].tp, args[1].tp) switch {
                (Rtype.Int, Rtype.Int) => SubIntInt(args),
                (Rtype.Int, Rtype.Float) => SubIntFloat(args),
                (Rtype.Float, Rtype.Int) => SubFloatInt(args),
                (Rtype.Float, Rtype.Float) => SubFloatFloat(args),
                (_, _) => ErrorInfo(args)
            };

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
            return (args[0].tp, args[1].tp) switch {
                (Rtype.Int, Rtype.Int) => MulIntInt(args),
                (Rtype.Int, Rtype.Float) => MulIntFloat(args),
                (Rtype.Float, Rtype.Int) => MulFloatInt(args),
                (Rtype.Float, Rtype.Float) => MulFloatFloat(args),
                (_, _) => ErrorInfo(args)
            };
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
            return (args[0].tp, args[1].tp) switch {
                (Rtype.Int, Rtype.Int) => DivIntInt(args),
                (Rtype.Int, Rtype.Float) => DivIntFloat(args),
                (Rtype.Float, Rtype.Int) => DivFloatInt(args),
                (Rtype.Float, Rtype.Float) => DivFloatFloat(args),
                (_, _) => ErrorInfo(args)
            };
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
            return (args[0].tp, args[1].tp) switch {
                (Rtype.Int, Rtype.Int) => ModIntInt(args),
                (Rtype.Int, Rtype.Float) => ModIntFloat(args),
                (Rtype.Float, Rtype.Int) => ModFloatInt(args),
                (Rtype.Float, Rtype.Float) => ModFloatFloat(args),
                (_, _) => ErrorInfo(args)
            };
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
            return (args[0].tp, args[1].tp) switch {
                (Rtype.Int, Rtype.Int) => AddSetIntInt(args),
                (Rtype.Int, Rtype.Float) => AddSetIntFloat(args),
                (Rtype.Float, Rtype.Int) => AddSetFloatInt(args),
                (Rtype.Float, Rtype.Float) => AddSetFloatFloat(args),
                (Rtype.Str, Rtype.Str) => AddSetStrStr(args),
                (_, _) => ErrorInfo(args)
            };
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

        public Rtoken AddSetStrStr(List<Rtoken> args) {
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
            return (args[0].tp, args[1].tp) switch {
                (Rtype.Int, Rtype.Int) => SubSetIntInt(args),
                (Rtype.Int, Rtype.Float) => SubSetIntFloat(args),
                (Rtype.Float, Rtype.Int) => SubSetFloatInt(args),
                (Rtype.Float, Rtype.Float) => SubSetFloatFloat(args),
                (_, _) => ErrorInfo(args)
            };
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
            return (args[0].tp, args[1].tp) switch {
                (Rtype.Int, Rtype.Int) => MulSetIntInt(args),
                (Rtype.Int, Rtype.Float) => MulSetIntFloat(args),
                (Rtype.Float, Rtype.Int) => MulSetFloatInt(args),
                (Rtype.Float, Rtype.Float) => MulSetFloatFloat(args),
                (_, _) => ErrorInfo(args)
            };
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
            return (args[0].tp, args[1].tp) switch {
                (Rtype.Int, Rtype.Int) => DivSetIntInt(args),
                (Rtype.Int, Rtype.Float) => DivSetIntFloat(args),
                (Rtype.Float, Rtype.Int) => DivSetFloatInt(args),
                (Rtype.Float, Rtype.Float) => DivSetFloatFloat(args),
                (_, _) => ErrorInfo(args)
            };
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
            return (args[0].tp, args[1].tp) switch {
                (Rtype.Int, Rtype.Int) => ModSetIntInt(args),
                (Rtype.Int, Rtype.Float) => ModSetIntFloat(args),
                (Rtype.Float, Rtype.Int) => ModSetFloatInt(args),
                (Rtype.Float, Rtype.Float) => ModSetFloatFloat(args),
                (_, _) => ErrorInfo(args)
            };
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
