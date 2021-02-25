using System;
using System.Collections.Generic;
using System.Text;

namespace RML.Lang {
    class StrKit {
        public unsafe static List<string> CutStrs(string s) {
            List<string> result = new List<string>();
            int idx = 0;
            char[] cs = s.ToCharArray();
            while (idx < s.Length) {
                if (!char.IsWhiteSpace(cs[idx])) {
                    if (cs[idx].Equals('"')) {
                        result.Add(TakeStr(cs, &idx));
                    }else if(cs[idx].Equals('[')) {
                        result.Add(TakeBlock(cs, &idx));
                    }else if (cs[idx].Equals('(')) {
                        result.Add(TakeParen(cs, &idx));
                    }else if (cs[idx].Equals('{')) {
                        result.Add(TakeObject(cs, &idx));
                    } else {
                        result.Add(TakeWord(cs, &idx));
                    }
                }

                idx++;
            }

            return result;
        }

        public unsafe static string TakeWord(char[] cs, int* idx) {
            int start = *idx;
            while (*idx < cs.Length) {
                if (char.IsWhiteSpace(cs[*idx])) {
                    break;
                }

                (*idx)++;
            }
            return new string(cs[start..(*idx)]);
        }

        public unsafe static string TakeStr(char[] cs, int* idx) {
            int start = *idx;
            (*idx)++;
            while (*idx < cs.Length) {
                if (cs[*idx].Equals('^')) {
                    (*idx)++;
                } else if (cs[*idx].Equals('"')) {
                    (*idx)++;
                    break;
                }
                (*idx)++;
            }
            return new string(cs[start..(*idx)]);
        }

        public unsafe static string TakeBlock(char[] cs, int* idx) {
            int start = *idx;
            int floor = 0;
            while (*idx < cs.Length) {
                if (cs[*idx].Equals('"')) {
                    TakeStr(cs, idx);
                } else if (cs[*idx].Equals('[')) {
                    floor++;
                } else if (cs[*idx].Equals(']')) {
                    floor--;
                }

                (*idx)++;
                if (floor == 0) {
                    break;
                }

            }
            return new string(cs[start..(*idx)]);

        }

        public unsafe static string TakeParen(char[] cs, int* idx) {
            int start = *idx;
            int floor = 0;
            while (*idx < cs.Length) {
                if (cs[*idx].Equals('"')) {
                    TakeStr(cs, idx);
                } else if (cs[*idx].Equals('(')) {
                    floor++;
                } else if (cs[*idx].Equals(')')) {
                    floor--;
                }

                (*idx)++;
                if (floor == 0) {
                    break;
                }

            }
            return new string(cs[start..(*idx)]);

        }


        public unsafe static string TakeObject(char[] cs, int* idx) {
            int start = *idx;
            int floor = 0;
            while (*idx < cs.Length) {
                if (cs[*idx].Equals('"')) {
                    TakeStr(cs, idx);
                } else if (cs[*idx].Equals('{')) {
                    floor++;
                } else if (cs[*idx].Equals('}')) {
                    floor--;
                }

                (*idx)++;
                if (floor == 0) {
                    break;
                }

            }
            return new string(cs[start..(*idx)]);

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
