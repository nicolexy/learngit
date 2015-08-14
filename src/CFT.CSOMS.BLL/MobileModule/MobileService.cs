using CFT.CSOMS.DAL.MobileModule;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace CFT.CSOMS.BLL.MobileModule
{
    public class MobileService
    {
        #region 手机绑定

        /// <summary>
        /// 解除手机绑定信息
        /// </summary>
        /// <param name="Fuid"></param>
        /// <param name="Msg"></param>
        /// <returns></returns>
        public bool UnbindMsgNotify(string Fuid, out string Msg)
        {
            return new MobileData().UnbindMsgNotify(Fuid, out Msg);
        }

        /// <summary>
        /// 修改绑定信息
        /// </summary>
        /// <param name="QQ"></param>
        /// <returns></returns>
        public bool UpDateBindInfo(string QQ)
        {
            return new MobileData().UpDateBindInfo(QQ);
        }

        /// <summary>
        /// 查询手机绑定信息
        /// </summary>
        /// <param name="QQ"></param>
        /// <returns></returns>
        public DataSet GetMsgNotify(string QQ)
        {
            DataSet ds = new MobileData().GetMsgNotify(QQ);

            if (ds == null || ds.Tables.Count <= 0 || ds.Tables[0].Rows.Count <= 0)
            {
                return null;
            }
            else
            {
                ds.Tables[0].Columns.Add("MobileState", typeof(string));
                ds.Tables[0].Columns.Add("MsgState", typeof(string));
                ds.Tables[0].Columns.Add("MobilePayState", typeof(string));
                ds.Tables[0].Columns.Add("Unbind", typeof(string));

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    try
                    {
                        /*转化为2进制(0为未开通,1为开通)不足7位前面补0,排序从最后一位开始
                          1.是否开通短信提醒
                          2.是否绑定email
                          3.是否绑定qq
                          4.是否激活
                          5.动态验证玛(废弃)
                          6.是否开通手机支付
                          7.是否绑定手机
                         */
                        string Fstatus = Convert.ToString(Convert.ToInt32(dr["Fstatus"]), 2);
                        if (Fstatus.Length < 31)
                        {
                            Fstatus = Fstatus.PadLeft(31, '0');
                        }
                        if (Fstatus.Length != 31)
                        {
                            throw new Exception("记录状态数据异常");
                        }
                        if (Fstatus.Substring(30, 1).ToString() == "0")
                        {
                            dr["MsgState"] = "未开通";
                        }
                        else
                        {
                            dr["MsgState"] = "开通";
                        }
                        if (Fstatus.Substring(25, 1).ToString() == "0")
                        {
                            dr["MobilePayState"] = "未开通";
                        }
                        else
                        {
                            dr["MobilePayState"] = "开通";
                        }
                        if (Fstatus.Substring(24, 1).ToString() == "0")
                        {
                            dr["MobileState"] = "未绑定";
                        }
                        else
                        {
                            dr["MobileState"] = "绑定";
                        }
                        if (Fstatus.Substring(30, 1).ToString() == "0" && Fstatus.Substring(25, 1).ToString() == "0" && Fstatus.Substring(24, 1).ToString() == "0")
                        {
                            dr["Unbind"] = "";
                        }
                        else
                        {
                            //这里有一个解绑权限的问题,需要在页面上再次去判断
                            dr["Unbind"] = "解绑";
                        }
                    }
                    catch
                    {
                        dr["MsgState"] = "Unknown";
                        dr["MobilePayState"] = "Unknown";
                        dr["MobileState"] = "Unknown";
                        dr["Unbind"] = "";
                    }
                }
            }

            return ds;
        }

        #endregion

        #region 手机充值卡查询

        /// <summary>
        /// 手机充值卡记录查询详细函数
        /// </summary>
        /// <param name="flistid">充值单号</param>
        /// <param name="fsupplylist">给供应商订单号</param>
        /// <param name="fcarrdid">充值卡序列号</param>
        /// <returns></returns>
        public DataSet GetFundCardListDetail(string flistid, string fsupplylist, string fcardid)
        {
            DataSet ds = new MobileData().GetFundCardListDetail(flistid, fsupplylist, fcardid);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ds.Tables[0].Columns.Add("Fstate_str", typeof(string));
                ds.Tables[0].Columns.Add("Fsign_str", typeof(string));
                ds.Tables[0].Columns.Add("Fcard_type_str", typeof(string));
                ds.Tables[0].Columns.Add("Fnum_str", typeof(string));

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    dr["Fnum_str"] = !string.IsNullOrEmpty(dr["Fnum"].ToString()) ? MoneyTransfer.FenToYuan(dr["Fnum"].ToString()) : "";

                    string tmp = dr["Fstate"].ToString();
                    if (tmp == "1")
                    {
                        dr["Fstate_str"] = "付款前";
                    }
                    if (tmp == "2")
                    {
                        dr["Fstate_str"] = "付款后";
                    }

                    tmp = dr["Fsign"].ToString();
                    if (tmp == "1")
                    {
                        dr["Fsign_str"] = "销卡成功";
                    }
                    if (tmp == "2")
                    {
                        dr["Fsign_str"] = "销卡失败";
                    }
                    if (tmp == "3")
                    {
                        dr["Fsign_str"] = "初始化";
                    }

                    tmp = dr["Fcard_type"].ToString();
                    if (tmp == "1")
                    {
                        dr["Fcard_type_str"] = "移动卡";
                    }
                    if (tmp == "2")
                    {
                        dr["Fcard_type_str"] = "联通卡";
                    }
                }
            }
            return ds;
        }

        #endregion

    }
}
