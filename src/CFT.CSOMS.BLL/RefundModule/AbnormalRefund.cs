using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using CFT.CSOMS.DAL.RefundModule;
using CFT.CSOMS.COMMLIB;
using System.Collections;
using CFT.Apollo.Logging;


namespace CFT.CSOMS.BLL.RefundModule
{
    public class RefundService
    {
        //付款延迟异常查询  子业务类型
        public static readonly Dictionary<string, string> SubTypePay = new Dictionary<string, string>(); //前台页面需要用于生成下拉框
        public static readonly Dictionary<string, string> typeht = new Dictionary<string, string>();
        public static readonly Dictionary<string, string> notifyTypeht = new Dictionary<string, string>();
        public static readonly Dictionary<string, string> notifyStatusht = new Dictionary<string, string>();
        public static readonly Dictionary<string, string> notifyResultht = new Dictionary<string, string>();
        public static readonly Dictionary<string, string> accht = new Dictionary<string, string>();
        public static readonly Dictionary<string, string> errht = new Dictionary<string, string>();
        private static string MESTemplate_id = "2470";
        private static string WXTemplate_id = "";
        

        static RefundService()
        {
            SubTypePay.Add("", "所有");
            //SubTypePay.Add("60", "手Q付款给财付通");
            //SubTypePay.Add("61", "手Q付款给银行卡");
            //SubTypePay.Add("62", "手Q付款给银行卡");
            //SubTypePay.Add("63", "手Q付款给银行卡");
            SubTypePay.Add("71", "微信提现T+0");
            SubTypePay.Add("72", "微信提现T+1");
            SubTypePay.Add("81", "理财通提现T+0");
            SubTypePay.Add("82", "理财通提现T+1");
            SubTypePay.Add("91", "信用卡还款-微信还款");
            SubTypePay.Add("92", "信用卡还款-手Q还款");
            SubTypePay.Add("93", "信用卡还款-主站还款");

            typeht.Add("", "所有");
            typeht.Add("1", "付款");

            notifyTypeht.Add("", "");
            notifyTypeht.Add("1", "微信");
            notifyTypeht.Add("2", "QQ");
            notifyTypeht.Add("3", "Email");
            notifyTypeht.Add("4", "短信");
            notifyTypeht.Add("5", "Tips");
            notifyTypeht.Add("6", "小钱包");

            notifyStatusht.Add("", "所有");
            notifyStatusht.Add("0", "初始状态");
            notifyStatusht.Add("1", "待发送");
            notifyStatusht.Add("2", "发送中");
            notifyStatusht.Add("3", "已发送");

            notifyResultht.Add("", "所有");
            notifyResultht.Add("0", "初始状态");
            notifyResultht.Add("1", "成功");
            notifyResultht.Add("2", "失败");

            accht.Add("", "所有");
            accht.Add("1", "微信");
            accht.Add("2", "QQ");
            accht.Add("3", "Email");
            accht.Add("4", "手机号");

            errht.Add("", "所有");
            errht.Add("P1", "付款延迟");
            errht.Add("P2", "付款回导延迟");
        }

        private int GetBankType(string strBankName)
        {         
            if (string.IsNullOrEmpty(strBankName))
            {
                return -1;
            }
            switch (strBankName.Trim())
            {
                case "招商银行借记卡": //C++：switch语句 只支持实型，string 不支持
                    {
                        return 1001;
                    }
                case "工商银行借记卡":
                    {
                        return 1002;
                    }
                case "建行银行借记卡":
                    {
                        return 1003;
                    }
                case "浦发银行借记卡":
                    {
                        return 1004;
                    }
                case "农业银行借记卡":
                    {
                        return 1005;
                    }
                case "民生银行借记卡":
                    {
                        return 1006;
                    }
                case "兴业银行借记卡":
                    {
                        return 1009;
                    }
                case "平安银行借记卡":
                    {
                        return 1010;
                    }
                case "交通银行借记卡":
                    {
                        return 1020;
                    }
                case "中国银行借记卡":
                    {
                        return 1026;
                    }
                case "光大银行借记卡":
                    {
                        return 1022;
                    }
                case "广发银行借记卡":
                    {
                        return 1027;
                    }
                case "中信银行借记卡":
                    {
                        return 1044;
                    }
                default:
                    break;
            }
            return -1;

        }
        public DataSet RequestRefundData(string strUid, string strBank, string strFSPID, string strBeginDate, string strEndDate, int iCheck, int iTrade,string strOldID,string strOperater,string strMin,string strMax) 
        {
            try
            {
                return new AbnormalRefundData().RequestRefundData(strUid, strBank, strFSPID, strBeginDate, strEndDate, iCheck, iTrade, strOldID, strOperater, strMin, strMax);
            }
            catch (Exception ex)
            {
                throw new Exception("查询审批数据异常" + ex.Message);
            }
            return null;
            
        }

        public string RequestItemState(string strRefundId)
        {
            return new AbnormalRefundData().RequestItemState(strRefundId);
        }

        public bool UpdateRefundData(string[] strOldId, string strIdentity, string strBankAccNoOld, string strUserEmail, string strNewBankAccNo, string strBankUserName,
            string strReason, string strImgCommitment, string strImgIdentity, string strImgBankWater, string strImgCancellation, string strBankName, string strOperater, int nInitBankID, int nNewBankID,
            int nUserFalg, int nCardType, int nState, out string outMsg)
        {
            try
            {
                return new AbnormalRefundData().UpdateRefundData(strOldId, strIdentity, strBankAccNoOld, strUserEmail, strNewBankAccNo, strBankUserName, strReason, strImgCommitment, strImgIdentity,
                    strImgBankWater, strImgCancellation, strBankName,strOperater, nInitBankID, nNewBankID, nUserFalg, nCardType, nState, out outMsg);
            }
            catch (Exception ex)
            {
                throw new Exception("存入数据异常：" + ex.Message);
            }
            return false;
            
        }

        public DataSet RequestDetailsData(string strRefundId, out string outMsg)
        {
            try
            {
                return new AbnormalRefundData().RequestDetailsData(strRefundId, out outMsg);
            }
            catch (Exception ex)
            {
                throw new Exception("请求审批详细数据异常：" + ex.Message);
            }
            return null;
        }

        public void SetRefundCheckState(int nState, string strOldId)
        {
            try
            {
                new AbnormalRefundData().SetRefundCheckState(nState, strOldId);
            }
            catch (Exception ex)
            {
                throw new Exception("更新审批状态出错：" + ex.Message);
            }
        }
        public void SetAbnormalRefundListID(string strCheckId, string strOldId)
        {
            try
            {
                new AbnormalRefundData().SetAbnormalRefundListID(strCheckId, strOldId);
            }
            catch (Exception ex)
            {
                throw new Exception("更新审批ID出错：" + ex.Message);
            }
        }
        //SetAbnormalRefundListID
        public string GetBankCardBindInformation(string listid, out string msg)
        {
            return new AbnormalRefundData().GetBankCardBindInformation(listid, out msg);
        }

        
        public void DeleteAbnormalRefundRecord(string strOldId)
        {
            try
            {
                new AbnormalRefundData().DeleteAbnormalRefundRecord(strOldId);
            }
            catch (Exception ex)
            {
                throw new Exception("作废审批记录出错：" + ex.Message);
            }
            
        }

       //查询审批编号
        public string QueryAbnormalRefundCheckID(string strOldId, ref string strHisCheckID)
        {
            try
            {
                return new AbnormalRefundData().QueryAbnormalRefundCheckID(strOldId, ref strHisCheckID);
            }
            catch (Exception ex)
            {
                throw new Exception("作废审批记录出错：" + ex.Message);
            }
        }

        public DataSet CheckFZWCheckMemo(string strOldId)
        {
             try
            {
                return  new AbnormalRefundData().CheckFZWCheckMemo(strOldId);
            }
            catch (Exception ex)
            {
                throw new Exception("查询退款失败财务转客服处理信息失败：" + ex.Message);
            }
        }

        public void RequestInfoChange(string strTxt, string strOperator)
        {
            try
            {
                 new AbnormalRefundData().RequestInfoChange(strTxt,strOperator);
            }
            catch (Exception ex)
            {
                throw new Exception("RequestInfoChange：" + ex.Message);
            }
        }

        /*
        /////////////////
        public bool IsCheckOperator(string strUser, int nState)
        {
            try
            {
               return new AbnormalRefundData().IsCheckOperator(strUser, nState);
            }
            catch (Exception ex)
            {
                throw new Exception("权限判断出错：" + ex.Message);
            }
        }

        public bool SetKFCheckUserInfo(string strUserInfo, string strUserLevel, string strOperator, int nType)
        {
            try
            {
                return new AbnormalRefundData().SetKFCheckUserInfo(strUserInfo, strUserLevel, strOperator, nType);
            }
            catch (Exception ex)
            {
                throw new Exception("设置审人组信息：" + ex.Message);
            }
        }

        public DataTable ReadKFCheckInfo(string strRefundId, int nType)
        {
            try
            {
                return new AbnormalRefundData().ReadKFCheckInfo(strRefundId, nType);
            }
            catch (Exception ex)
            {
                throw new Exception("读取日志：" + ex.Message);
            }
        }

        public bool SetRefundCheckState(string strRefundId, string strUser, int nState, string strMemo, int nResult)
        {
            try
            {
                return new AbnormalRefundData().SetRefundCheckState(strRefundId, strUser, nState,strMemo,nResult);
            }
            catch (Exception ex)
            {
                throw new Exception("设置审批结果出错：" + ex.Message);
            }
        }
         * */

        public DataSet QueryPaymenAbnormal(string sTime,string eTime, string batchID, string packageID,
            string listid, string type, string subTypePay, string notityStatus, string notityResult, string bankType, string errorType,
            string accType, int start, int max)
        {
            if (string.IsNullOrEmpty(sTime) || string.IsNullOrEmpty(eTime))
            {
                throw new ArgumentNullException("日期为空！");
            }
            if (string.IsNullOrEmpty(batchID))
            {
                throw new ArgumentNullException("批次为空！");
            }

            string product="";string business_type="";
            if (!string.IsNullOrEmpty(subTypePay))//子业务类型
            {
                if (subTypePay.Length != 2)
                    throw new Exception("子业务类型不对");

                product = subTypePay.Substring(0, 1);
                business_type = subTypePay.Substring(1, 1);
            }

           DataSet ds=  new AbnormalRefundData().QueryPaymenAbnormal(sTime,eTime, batchID, packageID,
                listid, type,product, business_type, notityStatus,  notityResult,bankType, errorType,accType, start, max);
           if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
           {
               ds.Tables[0].Columns.Add("Ftype_str", typeof(String));
               ds.Tables[0].Columns.Add("subType", typeof(String));
               ds.Tables[0].Columns.Add("subType_str", typeof(String));
              
               ds.Tables[0].Columns.Add("Faccount_type_str", typeof(String));
               ds.Tables[0].Columns.Add("Ferror_type_str", typeof(String));
               ds.Tables[0].Columns.Add("Fnotify_type_str", typeof(String));
               ds.Tables[0].Columns.Add("Fnotify_result_str", typeof(String));
               ds.Tables[0].Columns.Add("Fnotify_status_str", typeof(String));
               ds.Tables[0].Columns.Add("Fnotify_history_str", typeof(String));

               COMMLIB.CommUtil.DbtypeToPageContent(ds.Tables[0], "Ftype", "Ftype_str", typeht);
               COMMLIB.CommUtil.DbtypeToPageContent(ds.Tables[0], "Fnotify_type", "Fnotify_type_str", notifyTypeht);
               COMMLIB.CommUtil.DbtypeToPageContent(ds.Tables[0], "Fnotify_result", "Fnotify_result_str", notifyResultht);
               COMMLIB.CommUtil.DbtypeToPageContent(ds.Tables[0], "Fnotify_status", "Fnotify_status_str", notifyStatusht);
               COMMLIB.CommUtil.DbtypeToPageContent(ds.Tables[0], "Faccount_type", "Faccount_type_str", accht);
               COMMLIB.CommUtil.DbtypeToPageContent(ds.Tables[0], "Ferror_type", "Ferror_type_str", errht);

               foreach (DataRow dr in ds.Tables[0].Rows)
               {
                   dr["subType"] = dr["Fproduct"].ToString() + dr["Fbusiness_type"].ToString();
                   dr["Fnotify_history_str"] = gethis(notifyTypeht, dr["Fnotify_history"].ToString());
               }
               COMMLIB.CommUtil.DbtypeToPageContent(ds.Tables[0], "subType", "subType_str", SubTypePay);
           }

             return ds;
        }

        public static void UpdatePaymenAbnormal(string query, Dictionary<string, string> dicCondition, Dictionary<string, string> dicData, ArrayList listids = null)
        {
            #region 条件限制
            if (string.IsNullOrEmpty(dicCondition["sTime"]))
                throw new ArgumentNullException("sTime");
            if (string.IsNullOrEmpty(dicCondition["notityStatus"]) || dicCondition["notityStatus"] == "1" || dicCondition["notityStatus"] == "2")//待发送、发送中及所有状态不允许发送通知
            {
                LogHelper.LogInfo("待发送及所有状态不允许发送通知");
            }
            #endregion

            #region 写入数据限制
            if (string.IsNullOrEmpty(dicData["notify_type"]))
                throw new ArgumentNullException("notify_type");
            if (string.IsNullOrEmpty(dicData["notify_detail"]))
                throw new ArgumentNullException("notify_detail");
            if (string.IsNullOrEmpty(dicData["pre_arrival_time"]))
                throw new ArgumentNullException("pre_arrival_time");

            if (dicData["notify_type"] == "1")//微信
            {
                dicData.Add("template_id", WXTemplate_id);
            }
            else
            {
                dicData.Add("template_id", MESTemplate_id);
            }
            #endregion

            #region 发送通知
            if (query == "listid")
            {
                new AbnormalRefundData().UpdatePaymenAbnormalByListid(dicCondition["sTime"], dicData, listids);
                LogHelper.LogInfo("根据选定单号发送通知成功！");
            }
            else if (query == "condition")
            {
                #region 子业务类型拆成两个数据库字段
                string subTypePay = dicCondition["subTypePay"];
                if (subTypePay.Length == 2)
                {
                    dicCondition.Add("product", subTypePay.Substring(0, 1));
                    dicCondition.Add("business_type", subTypePay.Substring(1, 1));
                }
                else if (subTypePay == "")
                {
                    dicCondition.Add("product", "");
                    dicCondition.Add("business_type", "");
                }
                else
                    LogHelper.LogInfo("子业务类型不正确：" + subTypePay);
                #endregion

                int updateNum = 0;
                int total = 0;
                do
                {
                    updateNum = new AbnormalRefundData().UpdatePaymenAbnormalByQuery(dicCondition, dicData);
                    total += updateNum;
                } while (updateNum > 0);

                LogHelper.LogInfo("根据条件全部发送通知条数：" + total);
            }
            #endregion
        }



        public void UpdatePaymenAbnormalByListid(string sTime, string notityStatus, Dictionary<string, string> dicData, ArrayList listids)
        {
            try
            {
                Dictionary<string, string> dicCondition = new Dictionary<string, string>();
                dicCondition.Add("sTime", sTime);
                dicCondition.Add("notityStatus", notityStatus);
                UpdatePaymenAbnormal("listid", dicCondition, dicData, listids);
            }
            catch (Exception ex)
            {
                LogHelper.LogInfo("根据单号发送通知异常：" + ex);
            }
            finally
            {
                GC.Collect();
            }
        }

        public void UpdatePaymenAbnormalByQuery(Dictionary<string, string> dicCondition, Dictionary<string, string> dicData)
        {
            try
            {
                UpdatePaymenAbnormal("condition", dicCondition, dicData);
            }
            catch (Exception ex)
            {
                LogHelper.LogInfo("根据查询条件全部发送通知异常：" + ex);
            }
            finally
            {
                GC.Collect();
            }
        }

        /// <summary>
        /// 历史消息通知解析
        /// </summary>
        /// <param name="notifyTypeht">编码对应表</param>
        /// <param name="tmp">消息编码串</param>
        /// <returns></returns>
        private static string gethis(Dictionary<string, string> notifyTypeht, string tmp)
        {
            string NotifyHis = "";
            try
            {
                char[] notifyArray = tmp.ToArray();
                foreach (char c in notifyArray)
                {
                    NotifyHis += " " + notifyTypeht[c.ToString()];
                }
                return NotifyHis;
            }
            catch
            {
               return tmp;
            }
        }

    }
}
