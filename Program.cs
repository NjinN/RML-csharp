using System;
using System.Collections.Generic;
using RML.Lang;
using RML.NativeLib;
using RML.OpLib;

namespace RML
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");
            //List<string> strs = StrKit.CutStrs("123 \"Hello\" [1 2 \"][\"] ");
            //foreach(string item in strs) {
            //    Console.WriteLine(item);
            //}
            //Console.WriteLine("Hello World!");

            //List<Rtoken> tks = RtokenKit.MakeRtokens("123 \"Hello\" [1 2 \"][\"] ");
            //foreach(Rtoken item in tks) {
            //    item.Echo();
            //}

            Rtable libCtx = new Rtable(Rtable.Type.SYS);
            InitNative.Init(libCtx);
            InitOp.Init(libCtx);

            Rtable usrCtx = new Rtable(Rtable.Type.USR, libCtx);

            Rsolver mainSolver = new Rsolver();

            while (true) {
                Console.Write(">> ");
                string inp = Console.ReadLine();

                mainSolver.InputStr(inp);
                Rtoken result = mainSolver.Eval(usrCtx);

                switch (result.tp) {
                    case Rtype.Nil:
                        Console.WriteLine("");
                        break;

                    case Rtype.Flow:
                        Rflow flow = result.GetFlow();
                        if(null != flow.val) {
                            flow.val.Echo();
                        }
                        break;

                    default:
                        result.Echo();
                        break;
                }

                Console.WriteLine("");

            }




        }
    }
}
