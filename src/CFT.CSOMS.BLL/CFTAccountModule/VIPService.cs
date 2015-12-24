using CFT.CSOMS.DAL.CFTAccount;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace CFT.CSOMS.BLL.CFTAccountModule
{
    public class VIPService
    {
        public DataTable QueryVipInfo(string uin)
        {
            try
            {
                DataSet ds = new AccountData().QueryVipInfo(uin);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ds.Tables[0].Columns.Add("vipflag_str", typeof(String));
                    ds.Tables[0].Columns.Add("subid_str", typeof(String));
                    DataRow dr = ds.Tables[0].Rows[0];
                    dr.BeginEdit();
                    string vipflag = dr["vipflag"].ToString();
                    if (vipflag == "0")
                        dr["vipflag_str"] = "非会员";
                    else if (vipflag == "1")
                        dr["vipflag_str"] = "普通会员";
                    else if (vipflag == "2")
                        dr["vipflag_str"] = "VIP会员";
                    else if (vipflag == "4")
                        dr["vipflag_str"] = "连续1个月不做任务的普通会员（同非会员）";
                    else
                        dr["vipflag_str"] = "Unknown";

                    string subid = dr["subid"].ToString();
                    if (subid == "0")
                        dr["subid_str"] = "无支付方式";
                    else if (subid == "1")
                        dr["subid_str"] = "手机支付";
                    else if (subid == "2")
                        dr["subid_str"] = "个人帐户支付";
                    else if (subid == "3")
                        dr["subid_str"] = "Vnet支付";
                    else if (subid == "4")
                        dr["subid_str"] = "PHS小灵通支付";
                    else if (subid == "5")
                        dr["subid_str"] = "银行支付";
                    else if (subid == "100")
                        dr["subid_str"] = "Q币卡支付";
                    else if (subid == "101")
                        dr["subid_str"] = "声讯支付";
                    else if (subid == "102")
                        dr["subid_str"] = "ESALES支付";
                    else if (subid == "103")
                        dr["subid_str"] = "他人赠送";
                    else if (subid == "104")
                        dr["subid_str"] = "索要";
                    else if (subid == "105")
                        dr["subid_str"] = "公司活动赠送";
                    else if (subid == "106")
                        dr["subid_str"] = "免费（用于公免）";
                    else if (subid == "107")
                        dr["subid_str"] = "电信卡";
                    else if (subid == "108")
                        dr["subid_str"] = "缴费卡支付";
                    else if (subid == "109")
                        dr["subid_str"] = "积分";
                    else if (subid == "110")
                        dr["subid_str"] = "广州收费易";
                    else if (subid == "111")
                        dr["subid_str"] = "Q点";
                    else if (subid == "112")
                        dr["subid_str"] = "EPAY";
                    else if (subid == "113")
                        dr["subid_str"] = "MPAY";
                    else if (subid == "114")
                        dr["subid_str"] = "声讯预付费（活动接口）";
                    else if (subid == "115")
                        dr["subid_str"] = "PPW_PAIPAI 拍拍渠道";
                    else if (subid == "116")
                        dr["subid_str"] = "赠送索回";
                    else if (subid == "117")
                        dr["subid_str"] = "声讯预付费  2006.10,移动联通";
                    else if (subid == "118")
                        dr["subid_str"] = "Q币不足Q点支付";
                    else if (subid == "119")
                        dr["subid_str"] = "Q点不足Q币支付";
                    else if (subid == "120")
                        dr["subid_str"] = "移动do分离";
                    else if (subid == "121")
                        dr["subid_str"] = "tenpay";
                    else if (subid == "122")
                        dr["subid_str"] = "手机声讯渠道";
                    else if (subid == "123")
                        dr["subid_str"] = "统一帐户渠道";
                    else
                        dr["subid_str"] = "Unknown";
                    dr.EndEdit();

                    return ds.Tables[0];
                }
                return null;
            }
            catch (Exception ex)
            {              
                log4net.LogManager.GetLogger("查询会员信息异常: " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 财付通会员账号基本信息查询
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public DataSet QueryCFTMember(string account)
        {
            return new AccountData().QueryCFTMember(account);
        }

        /// <summary>
        /// 财付通会员账号高级信息查询
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public DataSet QueryCFTMemberAdvanced(string account)
        {
            return new AccountData().QueryCFTMemberAdvanced(account);
        }

        /// <summary>
        /// 财付值流水查询
        /// </summary>
        /// <param name="account"></param>
        /// <param name="order"></param>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        //public DataSet QueryTurnover(string account, string order, string begin, string end)
        //{
        //    return new AccountData().QueryTurnover(account, order, begin, end);
        //}

        public DataSet GetFreeFlowInfo(string cftNo)
        {
            return new AccountData().GetFreeFlowInfo(cftNo);
        }

    }
}
