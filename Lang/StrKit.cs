using System;
using System.Collections.Generic;
using System.Text;

namespace RML.Lang {
    class StrKit {
        public static List<string> CutStrs(string s) {
            List<string> result = new List<string>();
            int idx = 0;
            char[] cs = s.ToCharArray();
            while (idx < s.Length) {
                if (!char.IsWhiteSpace(cs[idx])) {
                    if (cs[idx].Equals('"')) {
                        result.Add(TakeStr(cs, ref idx));
                    }else if(cs[idx].Equals('[')) {
                        result.Add(TakeBlock(cs, ref idx));
                    }else if (cs[idx].Equals('(')) {
                        result.Add(TakeParen(cs, ref idx));
                    }else if (cs[idx].Equals('{')) {
                        result.Add(TakeObject(cs, ref idx));
                    } else {
                        result.Add(TakeWord(cs, ref idx));
                    }
                }

                idx++;
            }

            return result;
        }

        public static string TakeWord(char[] cs, ref int idx) {
            int start = idx;
            while (idx < cs.Length) {
                if (char.IsWhiteSpace(cs[idx])) {
                    break;
                }else if (cs[idx].Equals('/')) {
                    idx = start;
                    TakePath(cs, ref idx);
                    return new string(cs[start..idx]);
                }

                idx++;
            }
            return new string(cs[start..idx]);
        }

        public static string TakeStr(char[] cs, ref int idx) {
            int start = idx;
            idx++;
            while (idx < cs.Length) {
                if (cs[idx].Equals('^')) {
                    idx++;
                } else if (cs[idx].Equals('"')) {
                    idx++;
                    break;
                }
                idx++;
            }
            return new string(cs[start..idx]);
        }

        public static string TakeBlock(char[] cs, ref int idx) {
            int start = idx;
            int floor = 0;
            while (idx < cs.Length) {
                if (cs[idx].Equals('"')) {
                    TakeStr(cs, ref idx);
                } else if (cs[idx].Equals('[')) {
                    floor++;
                } else if (cs[idx].Equals(']')) {
                    floor--;
                }

                idx++;
                if (floor == 0) {
                    break;
                }

            }
            return new string(cs[start..idx]);

        }

        public static string TakeParen(char[] cs, ref int idx) {
            int start = idx;
            int floor = 0;
            while (idx < cs.Length) {
                if (cs[idx].Equals('"')) {
                    TakeStr(cs, ref idx);
                } else if (cs[idx].Equals('(')) {
                    floor++;
                } else if (cs[idx].Equals(')')) {
                    floor--;
                }

                idx++;
                if (floor == 0) {
                    break;
                }

            }
            return new string(cs[start..idx]);

        }


        public static string TakeObject(char[] cs, ref int idx) {
            int start = idx;
            int floor = 0;
            while (idx < cs.Length) {
                if (cs[idx].Equals('"')) {
                    TakeStr(cs, ref idx);
                } else if (cs[idx].Equals('{')) {
                    floor++;
                } else if (cs[idx].Equals('}')) {
                    floor--;
                }

                idx++;
                if (floor == 0) {
                    break;
                }

            }
            return new string(cs[start..idx]);

        }


        public static List<string> TakePath(char[] cs, ref int idx) {
            List<string> result = new List<string>();
            do {
                if (cs[idx].Equals('/')) {
                    idx++;
                }

                if (cs[idx].Equals('"')) {
                    result.Add(TakeStr(cs, ref idx));
                    continue;
                }else if (cs[idx].Equals('(')) {
                    result.Add(TakeParen(cs, ref idx));
                    continue;
                }

                int start = idx;
                while(idx < cs.Length && !cs[idx].Equals('/') && !char.IsWhiteSpace(cs[idx])) {
                    idx++;
                }
                result.Add(new string(cs[start..idx]));

            } while (idx < cs.Length && cs[idx].Equals('/'));

            return result;
        }



        public static int IsNumberStr(string s) {
            char[] cs = s.ToCharArray();
            int idx = 0;
            int dot = 0;

            if(s.Length == 0) {
                return -1;
            }
            if(s == "-") {
                return -1;
            }

            if (s.StartsWith('-')) {
                idx = 1;
            }

            while(idx < cs.Length) {
                if (cs[idx].Equals('.')) {
                    dot++;
                }else if (!char.IsNumber(cs[idx])) {
                    return -1;
                }

                idx++;
            }

            return dot;
        }


    }

}
