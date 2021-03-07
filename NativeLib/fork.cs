using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RML.Lang;

namespace RML.NativeLib {

    [Serializable]
    class Rnot : Rnative {
        public Rnot() {
            name = "not";
            argsLen = 1;
        }

        public override Rtoken Run(List<Rtoken> args, Rtable ctx) {
            return new Rtoken(Rtype.Bool, !args[0].ToBool());
        }
    }


    [Serializable]
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


    [Serializable]
    class Rspawn : Rnative {
        public Rspawn() {
            name = "_spawn";
            argsLen = 3;
        }

        public override Rtoken Run(List<Rtoken> args, Rtable ctx) {
            if (args[0].tp.Equals(Rtype.Block) && args[1].tp.Equals(Rtype.Bool) && args[2].tp.Equals(Rtype.Int)) {
                foreach (Rtoken item in args[0].GetList()) {
                    if (!item.tp.Equals(Rtype.Block)) {
                        return new Rtoken(Rtype.Err, "Error: spawn code must be block!");
                    }
                }

                List<Task> taskList = new List<Task>();

                foreach (Rtoken item in args[0].GetList()) {
                    Task t = new Task(delegate () {
                        Renv.threads++;
                        try {
                            new Rsolver(item.GetList()).Eval(ctx);
                        } finally {
                            Renv.threads--;
                        }

                    });
                    taskList.Add(t);
                    t.Start();
                }
                if (args[1].GetBool()) {
                    if(args[2].GetInt() > 0) {
                        Task.WaitAll(taskList.ToArray(), args[2].GetInt());
                    } else {
                        Task.WaitAll(taskList.ToArray());
                    }
                }
                

                return new Rtoken();
            }

            return ErrorInfo(args);
        }
    }


    [Serializable]
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
