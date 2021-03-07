using System;
using System.Collections.Generic;
using RML.Lang;
using RML.NativeLib;
using RML.OpLib;
using RML.ScriptLib;

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

            mainSolver.InputStr(InitScript.script);
            mainSolver.Eval(libCtx);

            Random rd = new Random();

            Console.WriteLine("如梦令 -- " + Ci.sentences[rd.Next(0, Ci.sentences.Length - 1)]);
            Console.WriteLine("RML no-version;\tGratitude to Carl!");

            String inpCode = "";

            while (true) {
                Console.Write(">> ");
                string inp = Console.ReadLine();

                if (inp.Trim().EndsWith('~')) {
                    inpCode += inp.Trim().Substring(0, inp.Length-1);
                    continue;
                }

                inpCode += inp;

                mainSolver.InputStr(inpCode);
                Rtoken result = mainSolver.Eval(usrCtx);

                switch (result.tp) {
                    case Rtype.Nil:
                        break;

                    case Rtype.Flow:
                        Rflow flow = result.GetFlow();
                        if(null != flow.val) {
                            flow.val.Show();
                        }
                        break;

                    default:
                        result.Show();
                        break;
                }

                Console.WriteLine("");
                inpCode = "";

            }




        }
    }
}
