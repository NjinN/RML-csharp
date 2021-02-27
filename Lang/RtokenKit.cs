using System;
using System.Collections.Generic;
using System.Text;

namespace RML.Lang {
    class RtokenKit {

        public static Rtoken MakeRtoken(string s, Rtable ctx) {
            string str = s.Trim();
            int len = str.Length;

            if (string.IsNullOrWhiteSpace(str)) {
                return new Rtoken(Rtype.Undefined, 0);
            }

            if (str.Equals("nil")) {
                return new Rtoken();
            }

            if (str.Equals("none")) {
                return new Rtoken(Rtype.None, 0);
            }

            if (str.Equals("false")) {
                return new Rtoken(Rtype.Bool, false);
            }

            if (str.Equals("true")) {
                return new Rtoken(Rtype.Bool, true);
            }

            if (str.Length ==4 && str.StartsWith("#'") && str.EndsWith("'")) {
                return new Rtoken(Rtype.Char, str.ToCharArray()[2]);
            }

            if (StrKit.IsNumberStr(str) == 0) {
                return new Rtoken(Rtype.Int, Convert.ToInt32(str));
            }

            if (StrKit.IsNumberStr(str) == 1) {
                return new Rtoken(Rtype.Float, Convert.ToDecimal(str));
            }

            if (str.StartsWith('"')) {
                return new Rtoken(Rtype.Str, str.Substring(1, str.Length - 2));
            }

            if (str.StartsWith('\'')) {
                return new Rtoken(Rtype.LitWord, str.Substring(1, str.Length - 1));
            }

            if (str.EndsWith(':')) {
                return new Rtoken(Rtype.SetWord, str.Substring(0, str.Length - 1));
            }

            if (str.StartsWith('[')) {
                return new Rtoken(Rtype.Block, MakeRtokens(str.Substring(1, str.Length - 2), ctx));
            }

            if (str.StartsWith('(')) {
                return new Rtoken(Rtype.Paren, MakeRtokens(str.Substring(1, str.Length - 2), ctx));
            }


            return new Rtoken(Rtype.Word, new Rword(s, ctx));
        }


        public static List<Rtoken> MakeRtokens(String s, Rtable ctx) {
            List<Rtoken> result = new List<Rtoken>();
            List<string> strs = StrKit.CutStrs(s);
            foreach(string item in strs) {
                result.Add(MakeRtoken(item, ctx));
            }
            return result;
        }


        public static string Rtype2Str(Rtype tp) {
            switch (tp) {
                case Rtype.Nil:
                    return "nil!";
                case Rtype.None:
                    return "none!";
                case Rtype.Err:
                    return "err!";
                case Rtype.Bool:
                    return "bool!";
                case Rtype.Byte:
                    return "byte!";
                case Rtype.Char:
                    return "char!";
                case Rtype.Int:
                    return "int!";
                case Rtype.Float:
                    return "float!";
                case Rtype.Str:
                    return "str!";
                case Rtype.Block:
                    return "block!";
                case Rtype.Paren:
                    return "paren!";
                case Rtype.Object:
                    return "object!";
                case Rtype.Flow:
                    return "flow!";
                case Rtype.Word: 
                    return "word!";
                case Rtype.LitWord:
                    return "lit-word!";
                case Rtype.SetWord:
                    return "set-word!";
                case Rtype.Func:
                    return "func!";
                case Rtype.Native:
                    return "native!";
                case Rtype.Op:
                    return "op!";


                default:
                    return "undefined!";
            }
        }


        public static Rtype Str2Rtype(string s) {
            switch (s) {
                case "nil!":
                    return Rtype.Nil;
                case "none!":
                    return Rtype.None;
                case "err!":
                    return Rtype.Err;
                case "bool!":
                    return Rtype.Bool;
                case "byte!":
                    return Rtype.Byte;
                case "char!":
                    return Rtype.Char;
                case "int!":
                    return Rtype.Int;
                case "float!":
                    return Rtype.Float;
                case "str!":
                    return Rtype.Str;
                case "block!":
                    return Rtype.Block;
                case "paren!":
                    return Rtype.Paren;
                case "object!":
                    return Rtype.Object;
                case "flow!":
                    return Rtype.Flow;
                case "word!":
                    return Rtype.Word;
                case "lit-word!":
                    return Rtype.LitWord;
                case "set-word!":
                    return Rtype.SetWord;
                case "func!":
                    return Rtype.Func;
                case "native!":
                    return Rtype.Native;
                case "op!":
                    return Rtype.Op;


                default:
                    return Rtype.Undefined;
            }
        }


        public static void ClearCtxForWordByWords(List<Rtoken> words, List<Rtoken> blk) {
            foreach(Rtoken item in words) {
                if (item.tp.Equals(Rtype.Word)) {
                    foreach(Rtoken itor in blk) {
                        if (itor.tp.Equals(Rtype.Word)){
                            if (itor.GetWord().key.Equals(item.GetWord().key)) {
                                itor.GetWord().ctx = null;
                            }
                        }else if(itor.tp.Equals(Rtype.Block) || itor.tp.Equals(Rtype.Paren)) {
                            ClearCtxForWordByWords(words, itor.GetList());
                        }
                    }
                }
            
            }
        }

    }




}
