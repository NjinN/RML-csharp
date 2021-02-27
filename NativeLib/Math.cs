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
            return new Rtoken(Rtype.Float, (decimal)argl + argr);
        }

        public Rtoken AddFloatInt(List<Rtoken> args) {
            decimal argl = args[0].GetFloat();
            int argr = args[1].GetInt();
            return new Rtoken(Rtype.Float, argl + (decimal)argr);
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
            return new Rtoken(Rtype.Float, (decimal)argl - argr);
        }

        public Rtoken SubFloatInt(List<Rtoken> args) {
            decimal argl = args[0].GetFloat();
            int argr = args[1].GetInt();
            return new Rtoken(Rtype.Float, argl - (decimal)argr);
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
            return new Rtoken(Rtype.Float, (decimal)argl * argr);
        }

        public Rtoken MulFloatInt(List<Rtoken> args) {
            decimal argl = args[0].GetFloat();
            int argr = args[1].GetInt();
            return new Rtoken(Rtype.Float, argl * (decimal)argr);
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
            return new Rtoken(Rtype.Float, (decimal)argl / argr);
        }

        public Rtoken DivFloatInt(List<Rtoken> args) {
            decimal argl = args[0].GetFloat();
            int argr = args[1].GetInt();
            return new Rtoken(Rtype.Float, argl / (decimal)argr);
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
            return new Rtoken(Rtype.Float, (decimal)argl % argr);
        }

        public Rtoken ModFloatInt(List<Rtoken> args) {
            decimal argl = args[0].GetFloat();
            int argr = args[1].GetInt();
            return new Rtoken(Rtype.Float, argl % (decimal)argr);
        }

        public Rtoken ModFloatFloat(List<Rtoken> args) {
            decimal argl = args[0].GetFloat();
            decimal argr = args[1].GetFloat();
            return new Rtoken(Rtype.Float, argl % argr);
        }



    }






}
