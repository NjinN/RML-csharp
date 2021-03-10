using System;
using System.Collections.Generic;
using System.Text;
using RML.Lang;

namespace RML.NativeLib {
    [Serializable]
    class Rquit : Rnative {
        public Rquit() {
            name = "_quit";
            argsLen = 1;
        }

        public override Rtoken Run(List<Rtoken> args, Rtable ctx) {
            if (args[0].tp.Equals(Rtype.Int)){
                Environment.Exit(args[0].GetInt());
            }
            return ErrorInfo(args);
        }
    }


    [Serializable]
    class Rprint : Rnative {
        public Rprint() {
            name = "_print";
            argsLen = 2;
        }

        public override Rtoken Run(List<Rtoken> args, Rtable ctx) {
            if (args[1].ToBool()) {
                if (args[0].tp.Equals(Rtype.Str)) {
                    Console.Write(args[0].OutputStr());
                } else {
                    Console.Write(args[0].ToStr());
                }
            }else {
                args[0].Echo();
            }
            
            return new Rtoken();
        }
    }


    [Serializable]
    class Rask : Rnative {
        public Rask() {
            name = "_ask";
            argsLen = 2;
        }

        public override Rtoken Run(List<Rtoken> args, Rtable ctx) {
            if (args[0].tp.Equals(Rtype.Str)) {
                Console.Write(args[0].GetStr());
                string inp = "";
                if (args[1].ToBool()) {
                    while (true) {
                        //存储用户输入的按键，并且在输入的位置不显示字符
                        ConsoleKeyInfo ck = Console.ReadKey(true);

                        //判断用户是否按下的Enter键
                        if (ck.Key != ConsoleKey.Enter) {
                            if (ck.Key != ConsoleKey.Backspace) {
                                //将用户输入的字符存入字符串中
                                inp += ck.KeyChar.ToString();
                                //将用户输入的字符替换为*
                                Console.Write("*");
                            } else {
                                if(inp.Length > 0) {
                                    inp = inp.Remove(inp.Length - 1);
                                    Console.Write("\b \b");
                                }
                            }
                        } else {
                            Console.WriteLine();
                            break;
                        }
                    }
                } else {
                    inp = Console.ReadLine();
                }

                return new Rtoken(Rtype.Str, inp);

            }

            return ErrorInfo(args);
        }
    }


    [Serializable]
    class Runset : Rnative {
        public Runset() {
            name = "unset";
            argsLen = 1;
            quoteList = new List<bool>() {true };
        }

        public override Rtoken Run(List<Rtoken> args, Rtable ctx) {
            if (args[0].tp.Equals(Rtype.Word)) {
                return new Rtoken(Rtype.Bool, ctx.Remove(args[0].GetWord().key));
            }
            return ErrorInfo(args);
        }
    }


    [Serializable]
    class Rgc : Rnative {
        public Rgc() {
            name = "gc";
            argsLen = 0;
        }

        public override Rtoken Run(List<Rtoken> args, Rtable ctx) {
            GC.Collect();
            return new Rtoken(Rtype.Bool, true);
        }
    }



    [Serializable]
    class Rclear : Rnative {
        public Rclear() {
            name = "clear";
            argsLen = 0;
        }

        public override Rtoken Run(List<Rtoken> args, Rtable ctx) {
            Console.Clear();
            return new Rtoken();
        }
    }


    [Serializable]
    class Cost : Rnative {
        public Cost() {
            name = "cost";
            argsLen = 1;
        }

        public override Rtoken Run(List<Rtoken> args, Rtable ctx) {
            long start = DateTime.Now.ToUniversalTime().Ticks;
            if (args[0].tp.Equals(Rtype.Block)) {
                Rtoken ans = new Rsolver(args[0].GetList()).Eval(ctx);
                if (ans.tp.Equals(Rtype.Err)) {
                    return ans;
                }
                long end = DateTime.Now.ToUniversalTime().Ticks;
                return new Rtoken(Rtype.Float, Convert.ToDecimal(end - start) / 10000000);
            } else if(args[0].tp.Equals(Rtype.Str)) {
                Rtoken ans = new Rsolver(args[0].GetStr()).Eval(ctx);
                if (ans.tp.Equals(Rtype.Err)) {
                    return ans;
                }
                long end = DateTime.Now.ToUniversalTime().Ticks;
                return new Rtoken(Rtype.Float, Convert.ToDecimal(end - start) / 10000000);
            } else {
                return ErrorInfo(args);
            }
        }
    }



}
