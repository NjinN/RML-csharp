using System;
using System.Collections.Generic;
using System.Text;
using RML.Lang;

namespace RML.NativeLib {
    [Serializable]
    class RparseRule {
        public static int INT_MAX = 2147483647;
        public static int MAX_DEEP = 500;

        public int minTimes;
        public int maxTimes;
        public string ruleStr;
        public Rtoken ruleBlk;
        public Rtoken code;
        public bool isEnd;
        public bool isSkip;
        public string model;
        public bool opposite;

        public RparseRule() {
            minTimes = -1;
            maxTimes = -1;
            ruleStr = "";
            ruleBlk = null;
            code = null;
            isEnd = false;
            isSkip = false;
            model = "";
            opposite = false;
        }

        public void Init() {
            minTimes = -1;
            maxTimes = -1;
            ruleStr = "";
            ruleBlk = null;
            code = null;
            isEnd = false;
            isSkip = false;
            model = "";
            opposite = false;
        }

        public bool IsRuleComplete() {
            return ((minTimes >= 0 && maxTimes >= minTimes) || model.CompareTo("") > 0) && (ruleStr.CompareTo("") > 0 || (null != ruleBlk && ruleBlk.tp.Equals(Rtype.Block)) || isEnd || isSkip );
        }

        public bool IsEmpty() {
            return minTimes == -1 && maxTimes == -1 && ruleStr.Equals("") && null == ruleBlk && null == code && isEnd == false && isSkip == false;
        }

        public void CompleteRuleRange() {
            if(minTimes < 0) {
                minTimes = 1;
            }
            if(maxTimes < minTimes) {
                maxTimes = minTimes;
            }
        }

        public void Echo() {
            Console.WriteLine("Rule:");
            Console.WriteLine("\tminTimes is " + minTimes.ToString());
            Console.WriteLine("\tmaxTimes is " + maxTimes.ToString());
            Console.WriteLine("\truleStr is " + ruleStr);
            Console.WriteLine("\truleBlk is " + ruleBlk.ToStr());
            Console.WriteLine("\tcode is " + code.ToStr());
            Console.WriteLine("\tisEnd is " + isEnd.ToString());
            Console.WriteLine("\tisSkip is " + isSkip.ToString());
            Console.WriteLine("\tmodel is " + model);
            Console.WriteLine("\topposite is " + opposite.ToString());

        }

        public (bool, Rtoken) Match(string str, ref int nowIdx, ref int startDeep, Rtable ctx) {
            int matchTimes = 0;
            bool mch = false;
            Rtoken rst = null;
            Rsolver solver = new Rsolver();

            if(isEnd && model.Equals("")){
                if(nowIdx >= str.Length) {
                    if (!opposite) {
                        return (true, null);
                    }else {
                        return (false, null);
                    }
                } else {
                    if (!opposite) {
                        return (false, null);
                    } else {
                        return (true, null);
                    }
                }

            }else if (isSkip) {
                while(matchTimes < maxTimes && nowIdx < str.Length) {
                    if(null != code) {
                        rst = solver.InputBlk(code.GetList()).Eval(ctx);
                        if (rst.tp.Equals(Rtype.Err)) {
                            return (false, rst);
                        }
                    }
                    matchTimes++;
                    nowIdx++;
                }

                if(matchTimes < minTimes) {
                    if (!opposite) {
                        return (false, null);
                    } else {
                        return (true, null);
                    }
                } else {
                    if (!opposite) {
                        return (true, null);
                    } else {
                        return (false, null);
                    }
                }

            }else if(model.CompareTo("") > 0) {
                if (model.Equals("thru")) {
                    model = "";
                    minTimes = 1;
                    maxTimes = 1;
                    while(nowIdx <= str.Length) {
                        (mch, rst) = Match(str, ref nowIdx, ref startDeep, ctx);
                        if (opposite) {
                            mch = !mch;
                        }
                        if (mch) {
                            Init();
                            return (true, null);
                        }
                        nowIdx++;
                    }
                    if (!mch) {
                        return (false, null);
                    }

                    return (true, null);
                
                }else if (model.Equals("to")) {
                    model = "";
                    minTimes = 1;
                    maxTimes = 1;
                    int tempIdx = nowIdx;
                    while(tempIdx <= str.Length) {
                        nowIdx = tempIdx;
                        (mch, rst) = Match(str, ref tempIdx, ref startDeep, ctx);
                        if (opposite) {
                            mch = !mch;
                        }
                        if (mch) {
                            Init();
                            return (true, null);
                        }
                        tempIdx++;

                    }
                    if (!mch) {
                        return (false, null);
                    }
                    return (true, null);

                }

            }else if(ruleStr.CompareTo("") > 0) {
                while(matchTimes < maxTimes && nowIdx < str.Length) {
                    if(nowIdx + ruleStr.Length > str.Length) {
                        if(matchTimes >= minTimes) {
                            return (true, rst);
                        } else {
                            return (false, rst);
                        }
                    } else {
                        if((!opposite && str.Substring(nowIdx, ruleStr.Length).Equals(ruleStr)) || (opposite && !str.Substring(nowIdx, ruleStr.Length).Equals(ruleStr))) {
                            if(null != code) {
                                rst = solver.InputBlk(code.GetList()).Eval(ctx);
                                if (rst.tp.Equals(Rtype.Err)) {
                                    return (false, rst);
                                }
                            }

                            matchTimes++;
                            nowIdx += ruleStr.Length;

                        } else {
                            if(matchTimes >= minTimes) {
                                return (true, rst);
                            } else {
                                return (false, rst);
                            }
                        }

                    }

                }
                if(matchTimes >= minTimes) {
                    return (true, rst);
                } else {
                    return (false, rst);
                }

            }else if(null != ruleBlk && ruleBlk.tp.Equals(Rtype.Block)) {
                while(matchTimes < maxTimes && nowIdx < str.Length) {
                    (mch, rst) = MatchRuleBlk(str, ruleBlk, ref nowIdx, ref startDeep, ctx);
                    if(null != rst && rst.tp.Equals(Rtype.Err)) {
                        return (false, rst);
                    }

                    if (opposite) {
                        mch = !mch;
                    }

                    if (mch) {
                        if(null != code) {
                            rst = solver.InputBlk(code.GetList()).Eval(ctx);
                            if (rst.tp.Equals(Rtype.Err)) {
                                return (false, rst);
                            }
                        }
                        matchTimes++;
                    } else {
                        if(matchTimes >= minTimes) {
                            return (true, rst);
                        } else {
                            return (false, rst);
                        }
                    }
                }

                if(matchTimes >= minTimes) {
                    return (true, rst);
                } else {
                    return (false, rst);
                }

            }

            return (false, new Rtoken(Rtype.Err, "Error parsing rule"));

        }


        public static (bool, Rtoken) MatchRuleBlk(string str, Rtoken blk, ref int nowIdx, ref int startDeep, Rtable ctx) {
            Rsolver solver = new Rsolver();
            startDeep++;
            if(startDeep > MAX_DEEP) {
                return (false, new Rtoken(Rtype.Err, "Parse too Deep"));  
            }

            Rtoken rst;
            bool mch;
            RparseRule rule = new RparseRule();
            if (IsOrRules(blk)) {
                Rtoken rules = SplitOrRules(blk);
                int tempIdx = 0;
                foreach(var item in rules.GetList()) {
                    tempIdx = nowIdx;
                    (mch, rst) = MatchRuleBlk(str, item, ref tempIdx, ref startDeep, ctx);
                    if(null != rst && rst.tp.Equals(Rtype.Err)) {
                        startDeep--;
                        return (false, rst);
                    }
                    if (mch) {
                        nowIdx = tempIdx;
                        startDeep--;
                        return (true, null);
                    }
                }
                startDeep--;
                return (false, null);

            } else {
                int blkIdx = 0;
                while(blkIdx < blk.GetList().Count) {
                    Rtoken nowRule = blk.GetList()[blkIdx];

                    if (nowRule.tp.Equals(Rtype.Word)) {
                        if (nowRule.GetWord().key.Equals("copy")) {
                            if(blkIdx < blk.GetList().Count - 1 && blk.GetList()[blkIdx + 1].tp.Equals(Rtype.Word)) {
                                int startIdx = nowIdx;
                                string word = blk.GetList()[blkIdx + 1].GetWord().key;
                                blkIdx += 2;
                                GetNextRule(rule, blk, ref blkIdx, ctx);
                                if (rule.IsRuleComplete()) {
                                    (mch, rst) = rule.Match(str, ref nowIdx, ref startDeep, ctx);
                                    if(!mch || (null != rst && rst.tp.Equals(Rtype.Err))) {
                                        if(null != rule.ruleBlk) {
                                            startDeep--;
                                        }
                                        return (false, rst);
                                    } else {
                                        Copy(str, startIdx, nowIdx, word, ctx);
                                        if(blkIdx < blk.GetList().Count && blk.GetList()[blkIdx].tp.Equals(Rtype.Paren)) {
                                            rst = solver.InputBlk(blk.GetList()[blkIdx].GetList()).Eval(ctx);
                                            if (rst.tp.Equals(Rtype.Err)) {
                                                return (false, rst);
                                            }
                                            blkIdx++;
                                        }
                                        continue;
                                    }

                                } else {
                                    return (false, new Rtoken(Rtype.Err, "Error parsing rule"));
                                }

                            } else {
                                return (false, new Rtoken(Rtype.Err, "Error parsing rule"));
                            }

                        }
                    }

                    if (!rule.IsRuleComplete()) {
                        GetNextRule(rule, blk, ref blkIdx, ctx);
                    }

                    if (rule.IsRuleComplete()) {
                        if(blkIdx < blk.GetList().Count && blk.GetList()[blkIdx].tp.Equals(Rtype.Paren)) {
                            rule.code = blk.GetList()[blkIdx];
                            blkIdx++;
                        }

                        (mch, rst) = rule.Match(str, ref nowIdx, ref startDeep, ctx);
                        if(!mch || (null != rst && rst.tp.Equals(Rtype.Err))){
                            startDeep--;
                            return (false, rst);
                        }
                        rule.Init();

                    } else {
                        return (false, new Rtoken(Rtype.Err, "Error parsing rule"));
                    }


                }

                startDeep--;
                if(startDeep == 0) {
                    if(nowIdx == str.Length && rule.IsEmpty()) {
                        return (true, null);
                    } else {
                        return (false, null);
                    }
                } else {
                    return (rule.IsEmpty(), null);
                }
            }
        }


        public static void GetNextRule(RparseRule rule, Rtoken blk, ref int blkIdx, Rtable ctx) {
            while(blkIdx < blk.GetList().Count) {
                Rtoken nowRule = blk.GetList()[blkIdx];
                if (nowRule.tp.Equals(Rtype.Int)) {
                    if(rule.minTimes < 0) {
                        rule.minTimes = nowRule.GetInt();
                    } else {
                        rule.maxTimes = nowRule.GetInt();
                    }

                }else if (nowRule.tp.Equals(Rtype.Str)) {
                    rule.ruleStr = nowRule.GetStr();
                    rule.CompleteRuleRange();
                    blkIdx++;
                    return;
                }else if (nowRule.tp.Equals(Rtype.Block)) {
                    rule.ruleBlk = nowRule;
                    rule.CompleteRuleRange();
                    blkIdx++;
                    return;
                }else if (nowRule.tp.Equals(Rtype.Word)) {
                    if (nowRule.GetWord().key.Equals("end")) {
                        rule.isEnd = true;
                        rule.CompleteRuleRange();
                        blkIdx++;
                        return;
                    }else if (nowRule.GetWord().key.Equals("skip")) {
                        rule.isSkip = true;
                        rule.CompleteRuleRange();
                        blkIdx++;
                        return;
                    }else if (nowRule.GetWord().key.Equals("some")) {
                        rule.minTimes = 1;
                        rule.maxTimes = INT_MAX;
                        blkIdx++;
                        continue;
                    }else if (nowRule.GetWord().key.Equals("any")) {
                        rule.minTimes = 0;
                        rule.maxTimes = INT_MAX;
                        blkIdx++;
                        continue;
                    }else if (nowRule.GetWord().key.Equals("opt")) {
                        rule.minTimes = 0;
                        rule.maxTimes = 1;
                        blkIdx++;
                        continue;
                    }else if (nowRule.GetWord().key.Equals("not")) {
                        rule.opposite = true;
                        blkIdx++;
                        continue;
                    }else if (nowRule.GetWord().key.Equals("thru")) {
                        rule.model = "thru";
                        blkIdx++;
                        continue;
                    }else if (nowRule.GetWord().key.Equals("to")) {
                        rule.model = "to";
                        blkIdx++;
                        continue;
                    }

                    Rtoken tempTk = nowRule.GetVal(ctx);
                    if(null == tempTk || tempTk.tp.Equals(Rtype.Nil) || tempTk.tp.Equals(Rtype.Err)) {
                        rule.Init();
                        return;
                    }

                    if (tempTk.tp.Equals(Rtype.Int)) {
                        if(rule.minTimes < 0) {
                            rule.minTimes = tempTk.GetInt();
                        } else {
                            rule.maxTimes = tempTk.GetInt();
                        }
                    } else if (nowRule.tp.Equals(Rtype.Str)) {
                        rule.ruleStr = tempTk.GetStr();
                        rule.CompleteRuleRange();
                        blkIdx++;
                        return;
                    } else if (nowRule.tp.Equals(Rtype.Block)) {
                        rule.ruleBlk = tempTk;
                        rule.CompleteRuleRange();
                        blkIdx++;
                        return;
                    } else {
                        rule.Init();
                        return;
                    }


                }

                blkIdx++;
            }

        }


        public static bool IsOrRules(Rtoken blk) {

            int idx = 0;
            while(idx < blk.GetList().Count) {
                Rtoken item = blk.GetList()[idx];
                if(item.tp.Equals(Rtype.Word) && (item.GetWord().key.Equals("|") || item.GetWord().key.Equals("or")) && idx > 0 &&  idx < blk.GetList().Count - 1) {
                    return true;
                }
                idx++;
            }
            return false;
        }

        public static Rtoken SplitOrRules(Rtoken blk) {
            Rtoken result = new Rtoken(Rtype.Block, new List<Rtoken>());

            int startIdx = 0;
            int nowIdx = 0;

            while (nowIdx < blk.GetList().Count) {
                Rtoken nowItem = blk.GetList()[nowIdx];

                if (nowItem.tp.Equals(Rtype.Word) && (nowItem.GetWord().key.Equals("|") || nowItem.GetWord().key.Equals("or")) && nowIdx > startIdx) {
                    Rtoken temp = new Rtoken(Rtype.Block, new List<Rtoken>());
                    temp.GetList().AddRange(blk.GetList().GetRange(startIdx, nowIdx - startIdx));

                    result.GetList().Add(temp);
                    startIdx = nowIdx + 1;

                } else if (nowIdx == blk.GetList().Count - 1 && nowIdx > 0 && nowIdx >= startIdx) {
                    Rtoken temp = new Rtoken(Rtype.Block, new List<Rtoken>());
                    temp.GetList().AddRange(blk.GetList().GetRange(startIdx, nowIdx - startIdx + 1));

                    result.GetList().Add(temp);

                }
                nowIdx++;
            }
            return result;
        }


        public static void Copy(string str, int startIdx, int endIdx, string word, Rtable ctx) {
            ctx.PutNow(word, new Rtoken(Rtype.Str, str.Substring(startIdx, endIdx - startIdx)));
        }

    }



    [Serializable]
    class Rparse : Rnative {
        public Rparse() {
            name = "parse";
            argsLen = 2;
        }

        public override Rtoken Run(List<Rtoken> args, Rtable ctx) {
            if (args[0].tp.Equals(Rtype.Str) && args[1].tp.Equals(Rtype.Block)) {
                int nowIdx = 0;
                int startDeep = 0;
                RtokenKit.ClearCtxForWords(args[1].GetList());
                var (mch, rst) = RparseRule.MatchRuleBlk(args[0].GetStr(), args[1], ref nowIdx, ref startDeep, ctx);
                if(null != rst && rst.tp.Equals(Rtype.Err)) {
                    return rst;
                }

                return new Rtoken(Rtype.Bool, mch);
            }

            return ErrorInfo(args);

        }

    }




}
