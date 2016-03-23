using CFT.CSOMS.DAL.CFTAccount;
using CFT.CSOMS.DAL.Infrastructure;
using CFT.CSOMS.DAL.MobileModule;
using CFT.CSOMS.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TENCENT.OSS.C2C.Finance.BankLib;

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

        public string GetMsgNotifyByPhoneNumber(string phoneNumber)
        {
            return new MobileData().GetMsgNotifyByPhoneNumber(phoneNumber);
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

        #region 手机号码清理

        /// <summary>
        /// 查询手机绑定次数
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> QueryMobileBoundNumber(string mobile)
        {
            return new MobileData().QueryMobileBoundNumber(mobile);
        }

        /// <summary>
        /// 查询手机绑定清理日记
        /// </summary>
        /// <param name="mobile">手机号码</param>
        /// <returns></returns>
        public DataSet QueryClearMobileNumberLog(string mobile)
        {
            ArrayList keyNameList = new ArrayList()
            {
                  "Fsubmit_user",   //当前操作用户
                 // "FUser_type",     //用户属性
                  "FCreate_time",   //操作时间
                  "FMobile",        //手机号码
                  "MobileBindCount_Old",    //清理前绑定次数
              };
            return PublicRes.QueryKFLog("ClearMobileNumberLog", "Mobile", mobile, keyNameList);
        }

        /// <summary>
        /// 手机绑定清零
        /// </summary>
        /// <param name="mobile">手机号码</param>
        /// <returns></returns>
        public bool ClearMobileBoundNumber(string mobile, string keyName, string keyValue, string operat_user, string obj_id, Param[] param)
        {
            var bol = new MobileData().ClearMobileBoundNumber(mobile);
            if (bol)
            {
                PublicRes.WirteKFLog(obj_id, "ClearMobileNumberLog", keyName, keyValue, operat_user, param);//写日志
            }
            return bol;
        }

        #endregion

        #region 

        /// <summary>
        /// 获取用户充值卡充值返回结果列表
        /// </summary>
        /// <param name="buyerUin">买家uin</param>
        /// <param name="dealId">业务订单ID，用户可以订单详情页面查看到。可选参数</param>
        /// <param name="startTime">订单开始时间戳。可选参数</param>
        /// <param name="endTime">订单结束时间，可选参数</param>
        /// <param name="operatorName">操作人员rtx英文名。必填参数</param>
        /// <param name="pageNum">从0页开始</param>
        /// <param name="pageSize">每页大小</param>
        /// <returns></returns>
        public UserMobileRechargeOrderResult GetRechargeList(string requestServerUrl, string buyerUin, string dealId, int startTime, int endTime, string operatorName, int pageNum, int
             pageSize, string auth_cm_com_session_key, ref string msg)
        {
            UserMobileRechargeOrderResult rechargeList = null;
            msg ="ok";

            if (string.IsNullOrEmpty(buyerUin.Trim()) && string.IsNullOrEmpty(dealId))
            {
                msg = "传递必填参数userQQ 和 dealId为空";
                return rechargeList;
            }

            var requestHttpAddress = string.Empty;

            StringBuilder requestParams =new StringBuilder();
            //buyerUin={1}&operator={2}&dealid={3}&startTime={4}&endTime={5}&pageNum={6}&pageSize={7}";
            if (!string.IsNullOrEmpty(operatorName))
            {
                requestParams.Append("operator=" + operatorName);
            }
            if (!string.IsNullOrEmpty(buyerUin))
            {
                requestParams.Append("&buyerUin=" + buyerUin);
            }
            if (!string.IsNullOrEmpty(dealId))
            {
                requestParams.Append("&dealId=" + dealId);
            }
            if (startTime>0)
            {
                requestParams.Append("&startTime=" + startTime);
            }
            if (endTime > 0)
            {
                requestParams.Append("&endTime=" + endTime);
            }
            if (pageNum >= 0)
            {
                requestParams.Append("&pageNum=" + pageNum);
            }
            if (pageSize > 0)
            {
                requestParams.Append("&pageSize=" + pageSize);
            }

                requestHttpAddress = requestServerUrl + requestParams.ToString();

            //md5签名，大小写不敏感。必填参数。
           //string sign = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(account + internalID + ID + IDType + md5Key, "md5");
            using (WebClient client = new WebClient())
            {
                var reqCookies = string.Format("userName={0}; user_name={0}; auth_cm_com_session_key={1}",operatorName,auth_cm_com_session_key);
                client.Headers.Add(HttpRequestHeader.Cookie, reqCookies);

                Apollo.Logging.LogHelper.LogInfo(string.Format("CFT.CSOMS.BLL.MobileModule.MobileService  GetRechargeList,请求URL={0},获取请求Cookie={1}",requestHttpAddress,  reqCookies));

                Task<Stream> data=null;
                try
                {
                    data = Task.Factory.StartNew(() => client.OpenRead(requestHttpAddress));
                }
                catch (Exception ef)
                {
                    Apollo.Logging.LogHelper.LogInfo(string.Format("CFT.CSOMS.BLL.MobileModule.MobileService  GetRechargeList,获取数据请求异常:{0}", ef.ToString()));
                }

                if (data.Result != null)
                {
                    using (StreamReader reader = new StreamReader(data.Result, Encoding.UTF8))
                    {
                        string result = reader.ReadToEnd();

                        Apollo.Logging.LogHelper.LogInfo(string.Format("CFT.CSOMS.BLL.MobileModule.MobileService  GetRechargeList,返回信息:{0}", result));

                        try
                        {
                            rechargeList = Newtonsoft.Json.JsonConvert.DeserializeObject<UserMobileRechargeOrderResult>(result);
                        }
                        catch (Exception ef)
                        {
                            msg = ef.Message;
                            Apollo.Logging.LogHelper.LogError(string.Format("CFT.CSOMS.BLL.MobileModule.MobileService  GetRechargeList,请求URL={0},返回信息:{1},数据json转对象异常：{2}。", requestHttpAddress, result, ef.ToString()));
                        }
                    }
                }
            }

              return rechargeList;
        }

        #endregion
    }
}
