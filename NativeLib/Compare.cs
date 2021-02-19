﻿using System;
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
            Rtype ltype = args[0].tp;
            Rtype rtype = args[1].tp;

            if (ltype.Equals(Rtype.Int)) {
                if (rtype.Equals(Rtype.Int)) {
                    return EqIntInt(args);
                } else if (rtype.Equals(Rtype.Float)) {
                    return EqIntFloat(args);
                }

            } else if (ltype.Equals(Rtype.Float)) {
                if (rtype.Equals(Rtype.Int)) {
                    return EqFloatInt(args);
                } else if (rtype.Equals(Rtype.Float)) {
                    return EqFloatFloat(args);
                }
            }

            return new Rtoken(Rtype.Err, "Error: Types mismatch for native::eq");
        }

        public Rtoken EqIntInt(List<Rtoken> args) {
            int argl = args[0].GetInt();
            int argr = args[1].GetInt();
            return new Rtoken(Rtype.Bool, argl == argr);
        }

        public Rtoken EqIntFloat(List<Rtoken> args) {
            int argl = args[0].GetInt();
            double argr = args[1].GetFloat();
            return new Rtoken(Rtype.Bool, (double)argl == argr);
        }

        public Rtoken EqFloatInt(List<Rtoken> args) {
            double argl = args[0].GetFloat();
            int argr = args[1].GetInt();
            return new Rtoken(Rtype.Bool, argl == (double)argr);
        }

        public Rtoken EqFloatFloat(List<Rtoken> args) {
            double argl = args[0].GetFloat();
            double argr = args[1].GetFloat();
            return new Rtoken(Rtype.Bool, argl == argr);
        }

    }





    class Lt : Rnative {
        public Lt() {
            name = "lt";
            argsLen = 2;
        }

        public override Rtoken Run(List<Rtoken> args, Rtable ctx) {
            Rtype ltype = args[0].tp;
            Rtype rtype = args[1].tp;

            if (ltype.Equals(Rtype.Int)) {
                if (rtype.Equals(Rtype.Int)) {
                    return LtIntInt(args);
                } else if (rtype.Equals(Rtype.Float)) {
                    return LtIntFloat(args);
                }

            } else if (ltype.Equals(Rtype.Float)) {
                if (rtype.Equals(Rtype.Int)) {
                    return LtFloatInt(args);
                } else if (rtype.Equals(Rtype.Float)) {
                    return LtFloatFloat(args);
                }
            }

            return new Rtoken(Rtype.Err, "Error: Types mismatch for native::lt");
        }

        public Rtoken LtIntInt(List<Rtoken> args) {
            int argl = args[0].GetInt();
            int argr = args[1].GetInt();
            return new Rtoken(Rtype.Bool, argl < argr);
        }

        public Rtoken LtIntFloat(List<Rtoken> args) {
            int argl = args[0].GetInt();
            double argr = args[1].GetFloat();
            return new Rtoken(Rtype.Bool, (double)argl < argr);
        }

        public Rtoken LtFloatInt(List<Rtoken> args) {
            double argl = args[0].GetFloat();
            int argr = args[1].GetInt();
            return new Rtoken(Rtype.Bool, argl < (double)argr);
        }

        public Rtoken LtFloatFloat(List<Rtoken> args) {
            double argl = args[0].GetFloat();
            double argr = args[1].GetFloat();
            return new Rtoken(Rtype.Bool, argl < argr);
        }

    }



    class Gt : Rnative {
        public Gt() {
            name = "gt";
            argsLen = 2;
        }

        public override Rtoken Run(List<Rtoken> args, Rtable ctx) {
            Rtype ltype = args[0].tp;
            Rtype rtype = args[1].tp;

            if (ltype.Equals(Rtype.Int)) {
                if (rtype.Equals(Rtype.Int)) {
                    return GtIntInt(args);
                } else if (rtype.Equals(Rtype.Float)) {
                    return GtIntFloat(args);
                }

            } else if (ltype.Equals(Rtype.Float)) {
                if (rtype.Equals(Rtype.Int)) {
                    return GtFloatInt(args);
                } else if (rtype.Equals(Rtype.Float)) {
                    return GtFloatFloat(args);
                }
            }

            return new Rtoken(Rtype.Err, "Error: Types mismatch for native::gt");
        }

        public Rtoken GtIntInt(List<Rtoken> args) {
            int argl = args[0].GetInt();
            int argr = args[1].GetInt();
            return new Rtoken(Rtype.Bool, argl > argr);
        }

        public Rtoken GtIntFloat(List<Rtoken> args) {
            int argl = args[0].GetInt();
            double argr = args[1].GetFloat();
            return new Rtoken(Rtype.Bool, (double)argl > argr);
        }

        public Rtoken GtFloatInt(List<Rtoken> args) {
            double argl = args[0].GetFloat();
            int argr = args[1].GetInt();
            return new Rtoken(Rtype.Bool, argl > (double)argr);
        }

        public Rtoken GtFloatFloat(List<Rtoken> args) {
            double argl = args[0].GetFloat();
            double argr = args[1].GetFloat();
            return new Rtoken(Rtype.Bool, argl > argr);
        }

    }





    class Le : Rnative {
        public Le() {
            name = "le";
            argsLen = 2;
        }

        public override Rtoken Run(List<Rtoken> args, Rtable ctx) {
            Rtype ltype = args[0].tp;
            Rtype rtype = args[1].tp;

            if (ltype.Equals(Rtype.Int)) {
                if (rtype.Equals(Rtype.Int)) {
                    return LeIntInt(args);
                } else if (rtype.Equals(Rtype.Float)) {
                    return LeIntFloat(args);
                }

            } else if (ltype.Equals(Rtype.Float)) {
                if (rtype.Equals(Rtype.Int)) {
                    return LeFloatInt(args);
                } else if (rtype.Equals(Rtype.Float)) {
                    return LeFloatFloat(args);
                }
            }

            return new Rtoken(Rtype.Err, "Error: Types mismatch for native::le");
        }

        public Rtoken LeIntInt(List<Rtoken> args) {
            int argl = args[0].GetInt();
            int argr = args[1].GetInt();
            return new Rtoken(Rtype.Bool, argl < argr);
        }

        public Rtoken LeIntFloat(List<Rtoken> args) {
            int argl = args[0].GetInt();
            double argr = args[1].GetFloat();
            return new Rtoken(Rtype.Bool, (double)argl < argr);
        }

        public Rtoken LeFloatInt(List<Rtoken> args) {
            double argl = args[0].GetFloat();
            int argr = args[1].GetInt();
            return new Rtoken(Rtype.Bool, argl < (double)argr);
        }

        public Rtoken LeFloatFloat(List<Rtoken> args) {
            double argl = args[0].GetFloat();
            double argr = args[1].GetFloat();
            return new Rtoken(Rtype.Bool, argl < argr);
        }

    }





    class Ge : Rnative {
        public Ge() {
            name = "ge";
            argsLen = 2;
        }

        public override Rtoken Run(List<Rtoken> args, Rtable ctx) {
            Rtype ltype = args[0].tp;
            Rtype rtype = args[1].tp;

            if (ltype.Equals(Rtype.Int)) {
                if (rtype.Equals(Rtype.Int)) {
                    return GeIntInt(args);
                } else if (rtype.Equals(Rtype.Float)) {
                    return GeIntFloat(args);
                }

            } else if (ltype.Equals(Rtype.Float)) {
                if (rtype.Equals(Rtype.Int)) {
                    return GeFloatInt(args);
                } else if (rtype.Equals(Rtype.Float)) {
                    return GeFloatFloat(args);
                }
            }

            return new Rtoken(Rtype.Err, "Error: Types mismatch for native::ge");
        }

        public Rtoken GeIntInt(List<Rtoken> args) {
            int argl = args[0].GetInt();
            int argr = args[1].GetInt();
            return new Rtoken(Rtype.Bool, argl > argr);
        }

        public Rtoken GeIntFloat(List<Rtoken> args) {
            int argl = args[0].GetInt();
            double argr = args[1].GetFloat();
            return new Rtoken(Rtype.Bool, (double)argl > argr);
        }

        public Rtoken GeFloatInt(List<Rtoken> args) {
            double argl = args[0].GetFloat();
            int argr = args[1].GetInt();
            return new Rtoken(Rtype.Bool, argl > (double)argr);
        }

        public Rtoken GeFloatFloat(List<Rtoken> args) {
            double argl = args[0].GetFloat();
            double argr = args[1].GetFloat();
            return new Rtoken(Rtype.Bool, argl > argr);
        }

    }


}