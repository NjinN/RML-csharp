﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

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

            if (str.Equals("/") || str.Equals("/=") || str.Equals("%")) {
                return new Rtoken(Rtype.Word, new Rword(str, ctx));
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

            if (str.EndsWith('!')) {
                return new Rtoken(Rtype.Datatype, RtokenKit.Str2Rtype(str));
            }

            if (str.StartsWith('"')) {
                return new Rtoken(Rtype.Str, str.Substring(1, str.Length - 2));
            }

            if (str.StartsWith('/')) {
                return new Rtoken(Rtype.Prop, str.Substring(1, str.Length - 1));
            }

            if (str.StartsWith('%')) {
                return new Rtoken(Rtype.File, str.Substring(1, str.Length - 1));
            }

            if (str.StartsWith(':')) {
                return new Rtoken(Rtype.GetWord, str.Substring(1, str.Length - 1));
            }

            if (str.StartsWith('\'')) {
                return new Rtoken(Rtype.LitWord, str.Substring(1, str.Length - 1));
            }

            if (str.StartsWith('[')) {
                return new Rtoken(Rtype.Block, MakeRtokens(str.Substring(1, str.Length - 2), ctx));
            }

            if (str.StartsWith('(')) {
                return new Rtoken(Rtype.Paren, MakeRtokens(str.Substring(1, str.Length - 2), ctx));
            }

            if (str.StartsWith('{')) {
                Rtable tb = new Rtable(Rtable.Type.USR, ctx);
                Rsolver solver = new Rsolver(str.Substring(1, str.Length - 2));
                solver.isLocal = true;
                solver.Eval(tb);

                return new Rtoken(Rtype.Object, tb);
            }

            if(!str.Equals("/") && !str.Equals("/=") && str.Contains('/')) {
                int i = 0;
                List<string> strs = StrKit.TakePath(str.ToCharArray(), ref i);
                List<Rtoken> list = new List<Rtoken>();
                foreach(var item in strs) {
                    list.Add(MakeRtoken(item, ctx));
                }
                if (str.EndsWith(':')) {
                    return new Rtoken(Rtype.SetPath, list);
                } else {
                    return new Rtoken(Rtype.Path, list);
                }
                
            }

            if(Regex.IsMatch(str, "^.+\\(.*\\):$")) {
                int i = str.IndexOf('(');
                string name = str.Substring(0, i);
                string argsStr = str.Substring(i + 1, str.Length - i - 3);
                RsetProc setProc = new RsetProc(name, MakeRtokens(argsStr, ctx));
                return new Rtoken(Rtype.SetProc, setProc);
            }

            if (Regex.IsMatch(str, "^.+\\(.*\\)$")) {
                int i = str.IndexOf('(');
                string name = str.Substring(0, i);
                string argsStr = str.Substring(i + 1, str.Length - i - 2);
                RcallProc callProc = new RcallProc(name, MakeRtokens(argsStr, ctx));
                return new Rtoken(Rtype.CallProc, callProc);
            }

            if (str.EndsWith(':') && !str.Contains('/')) {
                return new Rtoken(Rtype.SetWord, str.Substring(0, str.Length - 1));
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
                case Rtype.Datatype:
                    return "datatype!";
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
                case Rtype.File:
                    return "file!";
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
                case Rtype.Path:
                    return "path!";
                case Rtype.Prop:
                    return "prop!";
                case Rtype.Proc:
                    return "proc!";
                case Rtype.CallProc:
                    return "call-proc!";
                case Rtype.GetWord:
                    return "get-word!";
                case Rtype.LitWord:
                    return "lit-word!";
                case Rtype.SetWord:
                    return "set-word!";
                case Rtype.SetPath:
                    return "set-path!";
                case Rtype.SetProc:
                    return "set-proc!";
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
                case "datatype!":
                    return Rtype.Datatype;
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
                case "file!":
                    return Rtype.File;
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
                case "path!":
                    return Rtype.Path;
                case "prop!":
                    return Rtype.Prop;
                case "proc!":
                    return Rtype.Proc;
                case "call-proc!":
                    return Rtype.CallProc;
                case "get-word!":
                    return Rtype.GetWord;
                case "lit-word!":
                    return Rtype.LitWord;
                case "set-word!":
                    return Rtype.SetWord;
                case "set-path!":
                    return Rtype.SetPath;
                case "set-proc!":
                    return Rtype.SetProc;
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
                        }else if (itor.tp.Equals(Rtype.CallProc)) {
                            ClearCtxForWordByWords(words, itor.GetCallProc().args);
                        } else if (itor.tp.Equals(Rtype.SetProc)) {
                            ClearCtxForWordByWords(words, itor.GetSetProc().args);
                        }

                    }
                }
            }
        }


        public static List<Rtoken> CopyList(List<Rtoken> source) {
            List<Rtoken> result = new List<Rtoken>();
            foreach(var item in source) {
                result.Add(item.Copy());
            }
            return result;
        }


    }




    class LangUtil {
        public static T DeepCopy<T>(T obj) {
            //如果是字符串或值类型则直接返回
            if (obj is string || obj.GetType().IsValueType) return obj;

            object retval = Activator.CreateInstance(obj.GetType());
            FieldInfo[] fields = obj.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            foreach (FieldInfo field in fields) {
                try { field.SetValue(retval, DeepCopy(field.GetValue(obj))); } catch { }
            }
            return (T)retval;
        }


        public static List<T> DeepCopyList<T>(List<T> list) {
            List<T> result = new List<T>();
            foreach (var obj in list) {
                //如果是字符串或值类型则直接返回
                if (obj is string || obj.GetType().IsValueType) result.Add(obj);

                object retval = Activator.CreateInstance(obj.GetType());
                FieldInfo[] fields = obj.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                foreach (FieldInfo field in fields) {
                    try { field.SetValue(retval, DeepCopy(field.GetValue(obj))); } catch { }
                }
                result.Add((T)retval);
            }
            return result;
        }
    }

}
