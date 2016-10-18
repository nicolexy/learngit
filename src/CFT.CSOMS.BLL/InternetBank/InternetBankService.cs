using CFT.Apollo.Logging;
using CFT.CSOMS.BLL.WechatPay;
using CFT.CSOMS.DAL.Infrastructure;
using CFT.CSOMS.DAL.InternetBank;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using TENCENT.OSS.C2C.Finance.Common.CommLib;

namespace CFT.CSOMS.BLL.InternetBank
{
    public  class InternetBankService
    {
        public bool AddRefundInfo(string FOrderId, int FRefund_type, string FSam_no, string FRecycle_user, string FSubmit_user, string FRefund_amount, string memo)
        {
            if (string.IsNullOrEmpty(FOrderId)) 
            {
                throw new ArgumentNullException("FOrderId");
            }
            try
            {
                List<int> refundIdList = new List<int>();
                DataSet ds = new InternetBankService().GetRefundByFrefundId(0, "", "", 0, 0);
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow item in ds.Tables[0].Rows)
                        {
                            refundIdList.Add(Convert.ToInt32(item["Frefund_id"]));
                        }
                    }
                }
                if (!string.IsNullOrEmpty(FOrderId) && FOrderId.Length >= 10)
                {
                    if (!refundIdList.Contains(Convert.ToInt32(FOrderId.Substring(0, 10))))
                    {
                        LogHelper.LogError("该商家的订单不允许走网银退款。");
                        throw new Exception("该商家的订单不允许走网银退款。");
                    }
                }

                //DataTable wx_dt = new WechatPayService().QueryWxTrans(FOrderId); //查询微信转账业务
                //if (wx_dt != null && wx_dt.Rows.Count > 0)
                //{
                //    string wxTradeId = PublicRes.objectToString(wx_dt, "wx_trade_id");//子账户关联订单号
                //    if (wxTradeId.Contains("mkt") || wxTradeId.Contains("wxp"))
                //    {
                //        LogHelper.LogError("当前订单为微信大单，禁止录入！");
                //        throw new Exception("当前订单为微信大单，禁止录入！");
                //    }
                //}
            }
            catch (Exception ex)
            {
                loger.err("AddRefundInfo", ex.Message+ex.StackTrace);
                return false;
            }
            return  new CFT.CSOMS.DAL.InternetBank.InternetBankData().AddRefundInfo(FOrderId, FRefund_type, FSam_no, FRecycle_user, FSubmit_user, FRefund_amount,  memo);
        }

        /// <summary>
        /// 添加 退款商户名单 
        /// </summary>
        /// <param name="Frefund_id">商户号码</param>
        /// <param name="Frefund_name">商户名称</param>
        /// <param name="Fcreate_by">创建人</param>
        /// <param name="Fmodify_by">修改人</param>
        /// <returns></returns>
        public int AddRefundList(int Frefund_id, string Frefund_name, string Fcreate_by)
        {
            return new InternetBankData().AddRefundList(Frefund_id, Frefund_name, Fcreate_by);
        }

        /// <summary>
        /// 删除  退款商户名单 
        /// </summary>
        /// <param name="Fid">自增ID</param>
        /// <returns></returns>
        public bool DelRefundByFid(int Fid)
        {
            return new InternetBankData().DelRefundByFid(Fid);
        }

        /// <summary>
        /// 修改 退款商户名单 
        /// </summary>
        /// <param name="Frefund_id">商户号码</param>
        /// <param name="Frefund_name">商户名称</param>
        /// <param name="Fmodify_by">修改人</param>
        /// <param name="Fid">自增ID</param>
        /// <returns></returns>
        public int EditRefundByFid(int Frefund_id, int Old_Frefund_id,string Frefund_name, string Fmodify_by, int Fid)
        {
            return new InternetBankData().EditRefundByFid(Frefund_id,Old_Frefund_id, Frefund_name, Fmodify_by, Fid);
        }

        /// <summary>
        /// 查询 退款商户名单 
        /// </summary>
        /// <param name="Frefund_id">商户号码</param>
        /// <param name="sTime"></param>
        /// <param name="eTime"></param>
        /// <returns></returns>
        public DataSet GetRefundByFrefundId(int Frefund_id, string sTime, string eTime, int start, int max)
        {
            return new InternetBankData().GetRefundByFrefundId(Frefund_id, sTime, eTime, start, max);
        }

        public int GetRefundCount(int Frefund_id, string sTime, string eTime)
        {
            return new InternetBankData().GetRefundCount(Frefund_id, sTime, eTime);
        }
    }
}
