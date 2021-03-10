using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
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

            if (str.StartsWith("#'")) {
                return new Rtoken(Rtype.Char, str.ToCharArray()[2]);
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

            if (Regex.IsMatch(str, "^#b\\{[0-1]+\\}$")) {
                return new Rtoken(Rtype.Bin, HexCommon.strBinaryToByte(str.Substring(3, str.Length - 4)));
            }

            if (Regex.IsMatch(str, "^#\\{[0-9|a-f|A-F]+\\}$")) {
                return new Rtoken(Rtype.Bin, HexCommon.strHexToByte(str.Substring(2, str.Length - 3)));
            }

            if (Regex.IsMatch(str, "^.+\\(.*\\):$")) {
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
                case Rtype.Bin:
                    return "bin!";
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
                case "bin!":
                    return Rtype.Bin;
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

        public static void ClearCtxForWords(List<Rtoken> blk) {
            foreach (Rtoken itor in blk) {
                if (itor.tp.Equals(Rtype.Word)) {
                    itor.GetWord().ctx = null;
                } else if (itor.tp.Equals(Rtype.Block) || itor.tp.Equals(Rtype.Paren)) {
                    ClearCtxForWords(itor.GetList());
                } else if (itor.tp.Equals(Rtype.CallProc)) {
                    ClearCtxForWords(itor.GetCallProc().args);
                } else if (itor.tp.Equals(Rtype.SetProc)) {
                    ClearCtxForWords(itor.GetSetProc().args);
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
            using (Stream objectStream = new MemoryStream()) {
                //利用 System.Runtime.Serialization序列化与反序列化完成引用对象的复制
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(objectStream, obj);
                objectStream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(objectStream);
            }
        }


        public static List<T> DeepCopyList<T>(List<T> list) {
            List<T> result = new List<T>();
            foreach (T obj in list) {
                using (Stream objectStream = new MemoryStream()) {
                    //利用 System.Runtime.Serialization序列化与反序列化完成引用对象的复制
                    IFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(objectStream, obj);
                    objectStream.Seek(0, SeekOrigin.Begin);
                    result.Add((T)formatter.Deserialize(objectStream));
                }
            }
            return result;
        }


        public static string GetMemory(object o) // 获取引用类型的内存地址方法    
        {
            GCHandle h = GCHandle.Alloc(o, GCHandleType.WeakTrackResurrection);

            IntPtr addr = GCHandle.ToIntPtr(h);

            return "0x" + addr.ToString("X");
        }

        public static T[] ReverseArr<T>(T[] arr) {
            Array.Reverse(arr);
            return arr;
        }

    }



    class HexCommon {

        /// <summary> 
        /// 16进制字符串转字节数组 
        /// 如01 02 ff 0a
        /// </summary> 
        /// <param name="hexString"></param> 
        /// <returns></returns> 
        public static byte[] strHexToByte(string hexString) {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++) {
                string temp = hexString.Substring(i * 2, 2).Trim();
                returnBytes[i] = Convert.ToByte(temp, 16);
            }
            return returnBytes;
        }


        /// <summary>
        /// 16进制字符串转字节数组 
        /// </summary>
        /// <param name="hexString"></param>
        /// <param name="separator">分隔符</param>
        /// <returns></returns>
        public static byte[] strHexToByte(string hexString, char separator) {
            if (separator == char.MinValue) {
                return strHexToByte(hexString);
            }
            string[] hexarray = hexString.Split(separator);
            List<string> temphexarray = new List<string>(hexarray);
            var newhexarray = temphexarray.Where(u => u != string.Empty);

            int len = newhexarray.Count();
            byte[] returnBytes = new byte[len];

            int i = 0;
            foreach (string str in newhexarray) {

                string temp = str.Trim();
                returnBytes[i] = Convert.ToByte(temp, 16);
                i = i + 1;
            }

            return returnBytes;

        }


        /// <summary> 
        /// 字节数组转二进制字符串 
        /// </summary> 
        /// <param name="bytes"></param> 
        /// <returns></returns> 
        public static string byteToBinStr(byte[] bytes) {
            StringBuilder builder = new StringBuilder();
            foreach(var item in bytes) {
                builder.Append(Convert.ToString(item, 2).PadLeft(8, '0'));
            }

            return builder.ToString();
        }

        /// <summary> 
        /// 字节数组转16进制字符串 
        /// </summary> 
        /// <param name="bytes"></param> 
        /// <returns></returns> 
        public static string byteToHexStr(byte[] bytes) {
            string returnStr = "";
            if (bytes != null) {
                for (int i = 0; i < bytes.Length; i++) {
                    returnStr += bytes[i].ToString("x2") + " ";
                }
            }
            return returnStr.Trim();
        }
        /// <summary> 
        /// 字节数组转16进制字符串 (高低位反)
        /// </summary> 
        /// <param name="bytes"></param> 
        /// <returns></returns> 
        public static string byteToHexStrHL(byte[] bytes) {
            string returnStr = "";
            try {
                if (bytes != null) {
                    for (int i = 1; i < bytes.Length;) {
                        returnStr += bytes[i].ToString("x2") + " ";
                        returnStr += bytes[i - 1].ToString("x2") + " ";
                        i = i + 2;
                    }
                }
            } catch (Exception ex) {
            }
            return returnStr.Trim();
        }

        /// <summary> 
        /// 二进制字符串转字节数组 
        /// 如01001111 011111111 
        /// </summary> 
        /// <param name="hexString"></param> 
        /// <returns></returns> 
        public static byte[] strBinaryToByte(string binaryString) {
            binaryString = binaryString.Replace(" ", "");
            int yu = binaryString.Length % 8;
            //将字符串长度变成8的倍数
            if (yu != 0)
                binaryString = binaryString.PadRight(binaryString.Length + 8 - yu);
            StringBuilder builder = new StringBuilder();

            byte[] returnBytes = new byte[binaryString.Length / 8];
            for (int i = 0; i < returnBytes.Length; i++) {
                string temp = binaryString.Substring(i * 8, 8).Trim();
                returnBytes[i] = Convert.ToByte(temp, 2);
            }

            return returnBytes;
        }

        /// <summary> 
        /// 二进制字符串转字节数组 
        /// 如01001111 011111111 
        /// </summary> 
        /// <param name="hexString"></param> 
        /// <param name="separator">分隔符</param>
        /// <returns></returns> 
        public static byte[] strBinaryToByte(string binaryString, char separator) {
            if (separator == char.MinValue) {
                return strBinaryToByte(binaryString);
            }
            string[] binarray = binaryString.Split(separator);
            List<string> tempbinarray = new List<string>(binarray);
            var newbinarray = tempbinarray.Where(u => u != string.Empty);

            int len = newbinarray.Count();
            byte[] returnBytes = new byte[len];
            int i = 0;
            foreach (string str in newbinarray) {

                string temp = str.Trim();
                returnBytes[i] = Convert.ToByte(temp, 2);
                i = i + 1;
            }

            return returnBytes;
        }

        /// <summary>
        /// 二进制字符串转16进制字符串（无分隔符）
        /// </summary>
        /// <param name="binaryString"></param>
        /// <returns></returns>
        public static string strBinaryTostrHex(string binaryString) {
            binaryString = binaryString.Replace(" ", "");
            int yu = binaryString.Length % 8;
            //将字符串长度变成8的倍数
            if (yu != 0)
                binaryString = binaryString.PadRight(binaryString.Length + 8 - yu);
            StringBuilder builder = new StringBuilder();
            int len = binaryString.Length / 8;
            for (int i = 0; i < len; i++) {
                string temp = binaryString.Substring(i * 8, 8).Trim();
                string str = Convert.ToInt32(temp, 2).ToString("x2");
                builder = builder.Append(str);
            }
            return builder.ToString();
        }
        /// <summary>
        /// 二进制字符串转16进制字符串（有分隔符）
        /// </summary>
        /// <param name="binaryString"></param>
        /// <param name="separator">分隔符</param>
        /// <returns></returns>
        public static string strBinaryTostrHex(string binaryString, char separator) {
            if (separator == char.MinValue) {
                return strBinaryTostrHex(binaryString);
            }
            string[] binarray = binaryString.Split(separator);

            StringBuilder builder = new StringBuilder();
            int len = binarray.Length;
            for (int i = 0; i < len; i++) {
                string bin = binarray[i].Trim();
                if (bin == string.Empty) continue;
                string str = Convert.ToInt32(bin, 2).ToString("x2");
                builder = builder.Append(str);
                builder = builder.Append(separator);
            }
            string returnvalue = builder.ToString();
            returnvalue = returnvalue.TrimEnd(separator);
            return returnvalue;
        }


        /// <summary>
        /// 16进制字符串 转换成2进制字符串,此方法用于没有分隔符
        /// 16进制格式"ff 01" 2进制格式 "10101001 11111111"
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public static string strHexTostrBinary(string hexString) {

            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0) {
                hexString = hexString.PadRight(hexString.Length + 1);
            }

            StringBuilder builder = new StringBuilder();
            int len = hexString.Length / 2;
            for (int i = 0; i < len; i++) {
                string hex = hexString.Substring(i * 2, 2).Trim();
                int a = Convert.ToInt32(hex, 16);
                string str = Convert.ToString(a, 2).PadLeft(8, '0');
                builder = builder.Append(str);
            }
            return builder.ToString();
        }

        /// <summary>
        /// 16进制字符串 转换成2进制字符串,此方法用于有分隔符 如空格
        /// 16进制格式"ff 01" 2进制格式 "10101001 11111111"
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public static string strHexTostrBinary(string hexString, char separator) {
            if (separator == char.MinValue)
                return strHexTostrBinary(hexString);

            string[] hexarray = hexString.Split(separator);

            StringBuilder builder = new StringBuilder();
            int len = hexarray.Length;
            for (int i = 0; i < len; i++) {
                string hex = hexarray[i].Trim();
                if (hex == string.Empty) continue;
                int a = Convert.ToInt32(hex, 16);
                string str = Convert.ToString(a, 2).PadLeft(8, '0');
                builder = builder.Append(str);
                builder = builder.Append(separator.ToString());
            }
            string returnvalue = builder.ToString();
            returnvalue = returnvalue.TrimEnd(separator);
            return returnvalue;
        }
    }


}
