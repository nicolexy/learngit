using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using CFT.CSOMS.DAL.ActivityModule;
using CFT.CSOMS.DAL.FundModule;
using System.Collections;

namespace CFT.CSOMS.BLL.ActivityModule
{
    public class ActivityService
    {
        /// <summary>
        /// 活动查询。yinhuang 20140613
        /// </summary>
        /// <param name="actId">活动标识符</param>
        /// <param name="uin">账号</param>
        /// <param name="stime">开始时间</param>
        /// <param name="etime">结束时间</param>
        /// <param name="start">记录起始位</param>
        /// <param name="count">记录条数</param>
        public DataSet QueryActivity(int start, int count, params  string[] strParams)
        {
            //string actId, string uin, string stime, string etime, 
            if (strParams == null || strParams.Length < 1)
            {
                throw new ArgumentNullException("strParams");
            }

            string actId = strParams[0];


            if (string.IsNullOrEmpty(actId))
            {
                throw new ArgumentNullException("actId");
            }

            try
            {
                if (actId == "lsk")
                {
                    if (strParams.Length != 4)
                    {
                        throw new Exception("参数个数错误4");
                    }
                    string uin = strParams[1];
                    if (string.IsNullOrEmpty(uin))
                    {
                        throw new ArgumentNullException("uin");
                    }

                    return new ActivityData().QueryLeShuaKaActivity(uin, strParams[2], strParams[3], start, count);
                }
                else if (actId == "lct")
                {
                    #region 理财通活动
                    if (strParams.Length != 4)
                    {
                        throw new Exception("参数个数错误4");
                    }
                    string uin = strParams[1];
                    if (string.IsNullOrEmpty(uin))
                    {
                        throw new ArgumentNullException("uin");
                    }

                    DataSet ds = new ActivityData().QueryLCTActivity(uin, strParams[2], strParams[3], start, count);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        //中奖列
                        ds.Tables[0].Columns.Add("FPriTransId", typeof(String));
                        ds.Tables[0].Columns.Add("FPrizeType", typeof(String));
                        ds.Tables[0].Columns.Add("FPrizeName", typeof(String));
                        ds.Tables[0].Columns.Add("FPrizeExpiredTime", typeof(String));
                        ds.Tables[0].Columns.Add("FStartDate", typeof(String));
                        ds.Tables[0].Columns.Add("FPrizeDesc", typeof(String));
                        ds.Tables[0].Columns.Add("FErrInfo", typeof(String));
                        ds.Tables[0].Columns.Add("FGiveSpid", typeof(String));
                        ds.Tables[0].Columns.Add("FGiveState", typeof(String));
                        ds.Tables[0].Columns.Add("FPrizeMoney", typeof(String));
                        ds.Tables[0].Columns.Add("FGivePosId", typeof(String));
                        ds.Tables[0].Columns.Add("FPrizeTime", typeof(String));
                        ds.Tables[0].Columns.Add("FPrizeModifyTime", typeof(String));

                        ds.Tables[0].Columns.Add("FStateStr", typeof(String));//状态
                        ds.Tables[0].Columns.Add("FMoneyStr", typeof(String));//申购金额
                        ds.Tables[0].Columns.Add("FPrizeMoneyStr", typeof(String));//抽中金额
                        ds.Tables[0].Columns.Add("FUserTypeStr", typeof(String));//用户类型
                        ds.Tables[0].Columns.Add("FActTypeStr", typeof(String));//活动分类
                        ds.Tables[0].Columns.Add("FPrizeTypeStr", typeof(String));//抽中等级
                        ds.Tables[0].Columns.Add("FGiveStateStr", typeof(String));//赠送状态
                        ds.Tables[0].Columns.Add("FspnameStr", typeof(String));//申购基金
                        ds.Tables[0].Columns.Add("FActName", typeof(String));//活动名称

                        Hashtable ht1 = new Hashtable();
                        ht1.Add("10", "初始状态");
                        ht1.Add("20", "参与中/支付中");
                        ht1.Add("30", "支付成功");
                        ht1.Add("40", "用户已参加，被冻结");
                        ht1.Add("41", "参加成功");
                        ht1.Add("50", "资格已使用");

                        Hashtable ht2 = new Hashtable();
                        ht2.Add("0", "空奖");
                        ht2.Add("1", "现金券");
                        ht2.Add("2", "MP直送礼包");
                        ht2.Add("3", "MP类CDKEY");
                        ht2.Add("4", "信用卡打折密码");
                        ht2.Add("5", "财富值");
                        ht2.Add("6", "理财通优惠券");
                        ht2.Add("7", "理财通基金份额");
                        ht2.Add("8", "实物奖品");

                        Hashtable ht3 = new Hashtable();
                        ht3.Add("10", "初始状态");
                        ht3.Add("11", "赠送中");
                        ht3.Add("12", "赠送成功");
                        ht3.Add("13", "赠送失败");
                        ht3.Add("14", "CVK同步成功");

                        Hashtable ht4 = new Hashtable();
                        ht4.Add("0", "体验账户活动");
                        ht4.Add("1", "微信平台抽奖并送理财通红包活动");
                        ht4.Add("2", "微信平台只抽奖类活动");
                        ht4.Add("3", "微信平台领取QQ礼包活动");
                        ht4.Add("4", "手Q平台领取QQ礼包活动");
                        ht4.Add("5", "手Q平台抽奖并送红包活动");
                        ht4.Add("6", "手Q平台只抽奖类活动");

                        FundModule.FundService fundService = new FundModule.FundService();
                        DataTable spDt = null;

                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            dr["FActName"] = GetActNameByActNo(dr["FActionId"].ToString().Trim());

                            //通过uin，actid查中奖信息
                            DataSet ds2 = new ActivityData().QueryLCTPrize(dr["FPayUin"].ToString(), dr["FActionId"].ToString());
                            if (ds2 != null && ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
                            {
                                //赋值
                                DataRow dr2 = ds2.Tables[0].Rows[0];
                                dr["FPriTransId"] = dr2["FPriTransId"].ToString();
                                dr["FPrizeType"] = dr2["FPrizeType"].ToString();
                                dr["FPrizeName"] = dr2["FPrizeName"].ToString();
                                dr["FPrizeExpiredTime"] = dr2["FPrizeExpiredTime"].ToString();
                                dr["FStartDate"] = dr2["FStartDate"].ToString();
                                dr["FPrizeDesc"] = dr2["FPrizeDesc"].ToString();
                                dr["FErrInfo"] = dr2["FErrInfo"].ToString();
                                dr["FGiveSpid"] = dr2["FGiveSpid"].ToString();
                                dr["FGiveState"] = dr2["FGiveState"].ToString();
                                dr["FPrizeMoney"] = dr2["FPrizeMoney"].ToString();
                                dr["FGivePosId"] = dr2["FGivePosId"].ToString();
                                dr["FPrizeTime"] = dr2["FPrizeTime"].ToString();
                                dr["FPrizeModifyTime"] = dr2["FPrizeModifyTime"].ToString();

                            }

                            dr["FUserTypeStr"] = "新用户";
                            try
                            {
                                if (!string.IsNullOrEmpty(dr["FGiveSpid"].ToString()))
                                {
                                    spDt = fundService.QueryFundInfoBySpid(dr["FGiveSpid"].ToString());
                                    if (spDt != null && spDt.Rows.Count > 0)
                                    {
                                        dr["FspnameStr"] = spDt.Rows[0]["Fsp_name"].ToString();
                                    }
                                }
                            }
                            catch
                            {
                                dr["FspnameStr"] = "";
                            }
                        }

                        COMMLIB.CommUtil.FenToYuan_Table(ds.Tables[0], "FMoney", "FMoneyStr");
                        COMMLIB.CommUtil.FenToYuan_Table(ds.Tables[0], "FPrizeMoney", "FPrizeMoneyStr");

                        COMMLIB.CommUtil.DbtypeToPageContent(ds.Tables[0], "FStatus", "FStateStr", ht1);
                        COMMLIB.CommUtil.DbtypeToPageContent(ds.Tables[0], "FPrizeType", "FPrizeTypeStr", ht2);
                        COMMLIB.CommUtil.DbtypeToPageContent(ds.Tables[0], "FGiveState", "FGiveStateStr", ht3);
                        COMMLIB.CommUtil.DbtypeToPageContent(ds.Tables[0], "FActType", "FActTypeStr", ht4);
                    }
                    return ds;

                    #endregion
                }
                else if (actId == "userfbsyk")
                {
                    #region 用户翻倍收益卡
                    if (strParams.Length != 4)
                    {
                        throw new Exception("参数个数错误4");
                    }
                    string uin = strParams[1];
                    if (string.IsNullOrEmpty(uin))
                    {
                        throw new ArgumentNullException("uin");
                    }

                    return new ActivityData().QueryUserFBSYKActivity(uin, strParams[2], strParams[3], start, count);
                    #endregion
                }
                else if (actId == "wxzfact")
                {
                    #region 微信支付活动
                    if (strParams.Length != 4)
                    {
                        throw new Exception("参数个数错误4");
                    }
                    string uin = strParams[1];
                    if (string.IsNullOrEmpty(uin))
                    {
                        throw new ArgumentNullException("uin");
                    }

                    DataSet ds = new ActivityData().QueryWXZFActivity(uin, strParams[2], strParams[3], start, count);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        ds.Tables[0].Columns.Add("FState_str", typeof(String));//资格状态
                        ds.Tables[0].Columns.Add("FPayFee_str", typeof(String));//支付金额
                        ds.Tables[0].Columns.Add("Fstandby2_str", typeof(String));//奖级类型
                        ds.Tables[0].Columns.Add("Fstandby2_str2", typeof(String));//奖品类型
                        ds.Tables[0].Columns.Add("FPrizeDesc_str", typeof(String));//奖品描述

                        ds.Tables[0].Columns.Add("FActName", typeof(String));//活动名称

                        Hashtable ht1 = new Hashtable();
                        ht1.Add("0", "待支付");
                        ht1.Add("1", "已支付");
                        ht1.Add("2", "领取成功");
                        ht1.Add("3", "领取失败");
                        ht1.Add("4", "防刷逻辑过滤");
                        ht1.Add("5", "抽奖成功");
                        ht1.Add("6", "抽奖失败");
                        ht1.Add("7", "卡号/手机已参与");

                        Hashtable ht2 = new Hashtable();
                        ht2.Add("aim_rule_0", "卡号参加过");
                        ht2.Add("aim_rule_1", "手机参加过");
                        ht2.Add("aim_rule_2", "非新用户(a.客服系统查询订单详情 b.根据订单支付账号查询用户注册时间)");
                        ht2.Add("aim_rule_3", "卡号被多个微信账号绑定");

                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            string actid = dr["FActId"].ToString();
                            string tmp = dr["Fstandby2"].ToString();

                            if (actid == "10054")
                            {
                                //理财通活动特殊处理
                                if (tmp == "1")
                                {
                                    dr["Fstandby2_str"] = "5000元";
                                }
                                else if (tmp == "2")
                                {
                                    dr["Fstandby2_str"] = "500元";
                                }
                                else if (tmp == "3")
                                {
                                    dr["Fstandby2_str"] = "5元";
                                }
                                else if (tmp == "4")
                                {
                                    dr["Fstandby2_str"] = "1元";
                                }
                                else
                                {
                                    dr["Fstandby2_str"] = "未知：" + tmp;
                                }

                                dr["FPrizeDesc_str"] = "理财通红包";
                            }
                            else
                            {
                                string[] arr = tmp.Split(new char[] { ',' });
                                if (arr.Length == 2)
                                {
                                    if (arr[0] == "99")
                                    {
                                        dr["Fstandby2_str"] = "防刷奖级";
                                    }

                                    if (arr[1] == "0")
                                    {
                                        dr["Fstandby2_str2"] = "空奖";
                                    }
                                    else if (arr[1] == "1")
                                    {
                                        dr["Fstandby2_str2"] = "现金券";
                                    }
                                    else if (arr[1] == "2")
                                    {
                                        dr["Fstandby2_str2"] = "MP直发礼包";
                                    }
                                    else if (arr[1] == "3")
                                    {
                                        dr["Fstandby2_str2"] = "MP类CDK";
                                    }
                                    else if (arr[1] == "4")
                                    {
                                        dr["Fstandby2_str2"] = "信用卡打折密码";
                                    }
                                    else if (arr[1] == "5")
                                    {
                                        dr["Fstandby2_str2"] = "财富值";
                                    }
                                }

                                //奖级类型处理
                                string prize_desc = dr["FPrizeDesc"].ToString();
                                if (prize_desc == "aim_rule_0")
                                {
                                    dr["FPrizeDesc_str"] = "卡号参加过";

                                }
                                else if (prize_desc == "aim_rule_1")
                                {
                                    dr["FPrizeDesc_str"] = "手机参加过";
                                }
                                else if (prize_desc == "aim_rule_2")
                                {
                                    dr["FPrizeDesc_str"] = "非新用户(a.客服系统查询订单详情 b.根据订单支付账号查询用户注册时间)";
                                }
                                else if (prize_desc == "aim_rule_3")
                                {
                                    dr["FPrizeDesc_str"] = "卡号被多个微信账号绑定";
                                }
                                else
                                {
                                    dr["FPrizeDesc_str"] = "未知：" + prize_desc;
                                }
                            }

                            //活动名称关联查询有的查不到，改成查询后组合结果
                            //查询活动名称列表
                            DataTable dt = QueryActivityList(actid, "", 0, 1);
                            if (dt != null && dt.Rows.Count > 0)
                                dr["FActName"] = dt.Rows[0]["Fact_name"].ToString();
                            else
                                dr["FActName"] = "未知id：" + actid + "请新增";

                        }

                        COMMLIB.CommUtil.DbtypeToPageContent(ds.Tables[0], "FState", "FState_str", ht1);
                        COMMLIB.CommUtil.FenToYuan_Table(ds.Tables[0], "FPayFee", "FPayFee_str");
                    }
                    return ds;

                    #endregion
                }
                else if (actId == "xyk")
                {
                    #region 心意卡活动
                    if (strParams.Length != 5)
                    {
                        throw new Exception("参数个数错误4");
                    }
                    string uin = strParams[1];
                    if (string.IsNullOrEmpty(uin))
                    {
                        throw new ArgumentNullException("uin");
                    }

                    string sendCount = "0";
                    DataSet ds = new ActivityData().QueryXYKActivity(uin, strParams[2], strParams[3], strParams[4], start, count, out sendCount);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        if (strParams[4] == "send")
                        {
                            ds.Tables[0].Columns.Add("FSendCount", typeof(String));//总量
                            ds.Tables[0].Columns.Add("totalamountStr", typeof(String));//支付金额

                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                dr["FSendCount"] = sendCount;
                            }

                            COMMLIB.CommUtil.FenToYuan_Table(ds.Tables[0], "totalamount", "totalamountStr");
                        }
                        else if (strParams[4] == "rec")
                        {
                            ds.Tables[0].Columns.Add("usestateStr", typeof(String));//使用状态

                            Hashtable ht1 = new Hashtable();
                            ht1.Add("1", "初始化");
                            ht1.Add("2", "下单中");
                            ht1.Add("3", "下单成功");
                            ht1.Add("4", "下单失败");
                            ht1.Add("5", "已使用");
                            ht1.Add("6", "作废");

                            COMMLIB.CommUtil.DbtypeToPageContent(ds.Tables[0], "usestate", "usestateStr", ht1);
                        }

                    }

                    return ds;

                    #endregion
                }
                else
                {
                    throw new Exception("不支持的活动ID：" + actId);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("QueryActivity Error：" + ex.Message);
            }
        }

        /// <summary>
        /// 心意卡发送详情查询。yinhuang 20140616
        /// </summary>
        /// <param name="sendId">发送单号</param>
        public DataSet QueryXYKSendDetail(string sendId)
        {
            if (string.IsNullOrEmpty(sendId))
            {
                throw new ArgumentNullException("sendId");
            }

            DataSet ds = new ActivityData().QueryXYKSendDetail(sendId);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ds.Tables[0].Columns.Add("statusStr", typeof(String));//使用状态

                Hashtable ht1 = new Hashtable();
                ht1.Add("1", "初始化");
                ht1.Add("2", "领取中");
                ht1.Add("3", "领取成功");
                ht1.Add("4", "转赠成功");
                ht1.Add("5", "已使用");

                COMMLIB.CommUtil.DbtypeToPageContent(ds.Tables[0], "status", "statusStr", ht1);
            }
            return ds;
        }

        /// <summary>
        /// 添加新活动
        /// </summary>
        /// <param name="actNo"></param>
        /// <param name="actName"></param>
        /// <param name="opeUin"></param>
        public void AddLctActivity(string actNo, string actName, string opeUin)
        {
            if (string.IsNullOrEmpty(actNo))
            {
                throw new ArgumentNullException("actNo");
            }
            new ActivityData().AddLctActivity(actNo, actName, opeUin);
        }

        public DataTable QueryActivityList(string actNo, string actName, int offset, int limit)
        {
            return new ActivityData().QueryActivityList(actNo, actName, offset, limit);
        }

        public void DelLctActivity(int fid)
        {
            new ActivityData().DelLctActivity(fid);
        }

        //通过FUin找到用户参加活动的渠道号
        public string GetChannelIDByFUid(string FUid)
        {
            return new ActivityData().GetChannelIDByFUId(FUid);
        }

        public DataSet QueryHandQActivity(string strListID, string strBegingTime, string strEndTime, int nStart, int nCount)
        {
            return new ActivityData().QueryHandQActivity(strListID, strBegingTime, strEndTime, nStart,nCount);
        }

        /// <summary>
        /// 获取活动号对应的活动名称
        /// 主要用于理财通活动查询功能使用
        /// </summary>
        /// <param name="act_no"></param>
        /// <returns></returns>
        public string GetActNameByActNo(string act_no)
        {
            DataTable dt = new ActivityService().QueryActivityList(act_no, "", 0, 1);
            if (dt != null && dt.Rows.Count > 0)
            {
                return dt.Rows[0]["Fact_name"].ToString().Trim();
            }
            else
                return "未知" + act_no;
        }
    }
}
