using System;
using System.Collections.Generic;
using System.Text;
using RML.Lang;

namespace RML.NativeLib {
    class Eq : Rnative {
        public Eq() {
            name = "eq";
            argsLen = 2;
        }

        public override Rtoken Run(List<Rtoken> args, Rtable ctx) {

            return (args[0].tp, args[1].tp) switch {
                (Rtype.Int, Rtype.Int) => EqIntInt(args),
                (Rtype.Int, Rtype.Float) => EqIntFloat(args),
                (Rtype.Float, Rtype.Int) => EqFloatInt(args),
                (Rtype.Float, Rtype.Float) => EqFloatFloat(args),
                (Rtype.None, Rtype.None) => new Rtoken(Rtype.Bool, true),
                (Rtype.Bool, Rtype.Bool) => new Rtoken(Rtype.Bool, args[0].GetBool() == args[1].GetBool()),
                (_, _) => EqAnyAny(args)
            };

        }

        public Rtoken EqIntInt(List<Rtoken> args) {
            int argl = args[0].GetInt();
            int argr = args[1].GetInt();
            return new Rtoken(Rtype.Bool, argl == argr);
        }

        public Rtoken EqIntFloat(List<Rtoken> args) {
            int argl = args[0].GetInt();
            decimal argr = args[1].GetFloat();
            return new Rtoken(Rtype.Bool, (decimal)argl == argr);
        }

        public Rtoken EqFloatInt(List<Rtoken> args) {
            decimal argl = args[0].GetFloat();
            int argr = args[1].GetInt();
            return new Rtoken(Rtype.Bool, argl == (decimal)argr);
        }

        public Rtoken EqFloatFloat(List<Rtoken> args) {
            decimal argl = args[0].GetFloat();
            decimal argr = args[1].GetFloat();
            return new Rtoken(Rtype.Bool, argl == argr);
        }

        public Rtoken EqAnyAny(List<Rtoken> args) {
            if (args[0].tp.Equals(args[1].tp)) {
                return new Rtoken(Rtype.Bool, args[0].val.Equals(args[1].val));
            } else {
                return new Rtoken(Rtype.Bool, false);
            }
        }
    }





    class Lt : Rnative {
        public Lt() {
            name = "lt";
            argsLen = 2;
        }

        public override Rtoken Run(List<Rtoken> args, Rtable ctx) {
            return (args[0].tp, args[1].tp) switch {
                (Rtype.Int, Rtype.Int) => LtIntInt(args),
                (Rtype.Int, Rtype.Float) => LtIntFloat(args),
                (Rtype.Float, Rtype.Int) => LtFloatInt(args),
                (Rtype.Float, Rtype.Float) => LtFloatFloat(args),
                (_, _) => ErrorInfo(args)
            };
        }

        public Rtoken LtIntInt(List<Rtoken> args) {
            int argl = args[0].GetInt();
            int argr = args[1].GetInt();
            return new Rtoken(Rtype.Bool, argl < argr);
        }

        public Rtoken LtIntFloat(List<Rtoken> args) {
            int argl = args[0].GetInt();
            decimal argr = args[1].GetFloat();
            return new Rtoken(Rtype.Bool, (decimal)argl < argr);
        }

        public Rtoken LtFloatInt(List<Rtoken> args) {
            decimal argl = args[0].GetFloat();
            int argr = args[1].GetInt();
            return new Rtoken(Rtype.Bool, argl < (decimal)argr);
        }

        public Rtoken LtFloatFloat(List<Rtoken> args) {
            decimal argl = args[0].GetFloat();
            decimal argr = args[1].GetFloat();
            return new Rtoken(Rtype.Bool, argl < argr);
        }

    }



    class Gt : Rnative {
        public Gt() {
            name = "gt";
            argsLen = 2;
        }

        public override Rtoken Run(List<Rtoken> args, Rtable ctx) {
            return (args[0].tp, args[1].tp) switch {
                (Rtype.Int, Rtype.Int) => GtIntInt(args),
                (Rtype.Int, Rtype.Float) => GtIntFloat(args),
                (Rtype.Float, Rtype.Int) => GtFloatInt(args),
                (Rtype.Float, Rtype.Float) => GtFloatFloat(args),
                (_, _) => ErrorInfo(args)
            };
        }

        public Rtoken GtIntInt(List<Rtoken> args) {
            int argl = args[0].GetInt();
            int argr = args[1].GetInt();
            return new Rtoken(Rtype.Bool, argl > argr);
        }

        public Rtoken GtIntFloat(List<Rtoken> args) {
            int argl = args[0].GetInt();
            decimal argr = args[1].GetFloat();
            return new Rtoken(Rtype.Bool, (decimal)argl > argr);
        }

        public Rtoken GtFloatInt(List<Rtoken> args) {
            decimal argl = args[0].GetFloat();
            int argr = args[1].GetInt();
            return new Rtoken(Rtype.Bool, argl > (decimal)argr);
        }

        public Rtoken GtFloatFloat(List<Rtoken> args) {
            decimal argl = args[0].GetFloat();
            decimal argr = args[1].GetFloat();
            return new Rtoken(Rtype.Bool, argl > argr);
        }

    }





    class Le : Rnative {
        public Le() {
            name = "le";
            argsLen = 2;
        }

        public override Rtoken Run(List<Rtoken> args, Rtable ctx) {
            return (args[0].tp, args[1].tp) switch {
                (Rtype.Int, Rtype.Int) => LeIntInt(args),
                (Rtype.Int, Rtype.Float) => LeIntFloat(args),
                (Rtype.Float, Rtype.Int) => LeFloatInt(args),
                (Rtype.Float, Rtype.Float) => LeFloatFloat(args),
                (_, _) => ErrorInfo(args)
            };
        }

        public Rtoken LeIntInt(List<Rtoken> args) {
            int argl = args[0].GetInt();
            int argr = args[1].GetInt();
            return new Rtoken(Rtype.Bool, argl < argr);
        }

        public Rtoken LeIntFloat(List<Rtoken> args) {
            int argl = args[0].GetInt();
            decimal argr = args[1].GetFloat();
            return new Rtoken(Rtype.Bool, (decimal)argl < argr);
        }

        public Rtoken LeFloatInt(List<Rtoken> args) {
            decimal argl = args[0].GetFloat();
            int argr = args[1].GetInt();
            return new Rtoken(Rtype.Bool, argl < (decimal)argr);
        }

        public Rtoken LeFloatFloat(List<Rtoken> args) {
            decimal argl = args[0].GetFloat();
            decimal argr = args[1].GetFloat();
            return new Rtoken(Rtype.Bool, argl < argr);
        }

    }





    class Ge : Rnative {
        public Ge() {
            name = "ge";
            argsLen = 2;
        }

        public override Rtoken Run(List<Rtoken> args, Rtable ctx) {
            return (args[0].tp, args[1].tp) switch {
                (Rtype.Int, Rtype.Int) => GeIntInt(args),
                (Rtype.Int, Rtype.Float) => GeIntFloat(args),
                (Rtype.Float, Rtype.Int) => GeFloatInt(args),
                (Rtype.Float, Rtype.Float) => GeFloatFloat(args),
                (_, _) => ErrorInfo(args)
            };
        }

        public Rtoken GeIntInt(List<Rtoken> args) {
            int argl = args[0].GetInt();
            int argr = args[1].GetInt();
            return new Rtoken(Rtype.Bool, argl > argr);
        }

        public Rtoken GeIntFloat(List<Rtoken> args) {
            int argl = args[0].GetInt();
            decimal argr = args[1].GetFloat();
            return new Rtoken(Rtype.Bool, (decimal)argl > argr);
        }

        public Rtoken GeFloatInt(List<Rtoken> args) {
            decimal argl = args[0].GetFloat();
            int argr = args[1].GetInt();
            return new Rtoken(Rtype.Bool, argl > (decimal)argr);
        }

        public Rtoken GeFloatFloat(List<Rtoken> args) {
            decimal argl = args[0].GetFloat();
            decimal argr = args[1].GetFloat();
            return new Rtoken(Rtype.Bool, argl > argr);
        }

    }


}
