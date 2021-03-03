using System;
using System.Collections.Generic;
using System.Text;
using RML.Lang;

namespace RML.NativeLib {
    class Rto : Rnative {
        public Rto() {
            name = "to";
            argsLen = 2;
        }

        public override Rtoken Run(List<Rtoken> args, Rtable ctx) {
            if (!args[0].tp.Equals(Rtype.Datatype)) {
                return ErrorInfo(args);
            }

            if (args[0].tp.Equals(args[1].tp)) {
                return args[1].CopyDeep();
            }

            try {
                return (args[0].GetRtype(), args[1].tp) switch {
                    (Rtype.Err, _) => new Rtoken(Rtype.Err, args[1].OutputStr()),
                    (Rtype.Datatype, _) => new Rtoken(Rtype.Datatype, RtokenKit.Str2Rtype(args[1].OutputStr())),
                    (Rtype.Bool, _) => new Rtoken(Rtype.Bool, args[1].ToBool()),
                    (Rtype.Byte, _) => new Rtoken(Rtype.Byte, (byte)args[1].val),
                    (Rtype.Char, _) => new Rtoken(Rtype.Char, (char)args[1].val),
                    (Rtype.Int, Rtype.Float) => new Rtoken(Rtype.Int, (int)args[1].GetFloat()),
                    (Rtype.Int, _) => new Rtoken(Rtype.Int, (int)args[1].val),
                    (Rtype.Float, Rtype.Int) => new Rtoken(Rtype.Float, (decimal)args[1].GetInt()),
                    (Rtype.Float, _) => new Rtoken(Rtype.Float, (decimal)args[1].val),
                    (Rtype.Str, _) => new Rtoken(Rtype.Str, args[1].OutputStr()),
                    (Rtype.File, _) => new Rtoken(Rtype.File, args[1].OutputStr()),
                    (Rtype.Block, Rtype.Paren) => new Rtoken(Rtype.Block, LangUtil.DeepCopyList(args[1].GetList())),
                    (Rtype.Block, Rtype.Path) => new Rtoken(Rtype.Block, LangUtil.DeepCopyList(args[1].GetList())),
                    (Rtype.Paren, Rtype.Block) => new Rtoken(Rtype.Paren, LangUtil.DeepCopyList(args[1].GetList())),
                    (Rtype.Paren, Rtype.Path) => new Rtoken(Rtype.Paren, LangUtil.DeepCopyList(args[1].GetList())),
                    (Rtype.Path, Rtype.Block) => new Rtoken(Rtype.Path, LangUtil.DeepCopyList(args[1].GetList())),
                    (Rtype.Path, Rtype.Paren) => new Rtoken(Rtype.Path, LangUtil.DeepCopyList(args[1].GetList())),
                    (Rtype.Word, _) => new Rtoken(Rtype.Word, new Rword(args[1].OutputStr(), ctx)),
                    (Rtype.GetWord, _) => new Rtoken(Rtype.GetWord, args[1].OutputStr()),
                    (Rtype.LitWord, _) => new Rtoken(Rtype.LitWord, args[1].OutputStr()),
                    (Rtype.SetWord, _) => new Rtoken(Rtype.SetWord, args[1].OutputStr()),
                    (Rtype.Path, Rtype.SetPath) => new Rtoken(Rtype.Path, LangUtil.DeepCopyList(args[1].GetList())),
                    (Rtype.SetPath, Rtype.Path) => new Rtoken(Rtype.SetPath, LangUtil.DeepCopyList(args[1].GetList())),


                    (_, _) => ErrorInfo(args)
                };
            }catch(Exception e) {
                return new Rtoken(Rtype.Err, e.Message);
            }



        }
    }



}
