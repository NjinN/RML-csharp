using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RML.Lang;

namespace RML.NativeLib {
    class Rfork : Rnative {
        public Rfork() {
            name = "fork";
            argsLen = 1;
        }

        public override Rtoken Run(List<Rtoken> args, Rtable ctx) {
            if (args[0].tp.Equals(Rtype.Block)) {
                Task t = new Task(delegate () {
                    Renv.threads++;
                    try {
                        new Rsolver(args[0].GetList()).Eval(ctx);
                    } finally {
                        Renv.threads--;
                    }

                });

                t.Start();
                return new Rtoken();
            }

            return ErrorInfo(args);
        }
    }



    class Rspawn : Rnative {
        public Rspawn() {
            name = "spawn";
            argsLen = 1;
        }

        public override Rtoken Run(List<Rtoken> args, Rtable ctx) {
            if (args[0].tp.Equals(Rtype.Block)) {
                foreach (Rtoken item in args[0].GetList()) {
                    if (!item.tp.Equals(Rtype.Block)) {
                        return new Rtoken(Rtype.Err, "Error: spawn code must be block!");
                    }
                }

                foreach (Rtoken item in args[0].GetList()) {
                    new Task(delegate () {
                        Renv.threads++;
                        try {
                            new Rsolver(item.GetList()).Eval(ctx);
                        } finally {
                            Renv.threads--;
                        }

                    }).Start();
                }

                return new Rtoken();
            }

            return ErrorInfo(args);
        }
    }


    class Rlock : Rnative {
        public Rlock() {
            name = "lock";
            argsLen = 2;
        }

        public override Rtoken Run(List<Rtoken> args, Rtable ctx) {
            if (args[1].tp.Equals(Rtype.Block)) {
                lock (args[0]) {
                    new Rsolver(args[1].GetList()).Eval(ctx);
                }
                return new Rtoken();
            }

            return ErrorInfo(args);
        }
    }

}
