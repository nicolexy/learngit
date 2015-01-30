using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using TENCENT.OSS.C2C.Finance.Common.CommLib;
using CFT.CSOMS.DAL.CFTAccount;
using CFT.CSOMS.DAL.Infrastructure;
using CFT.Apollo.CommunicationFramework;
using CFT.CSOMS.COMMLIB;
using CFT.Apollo.Logging;

namespace CFT.CSOMS.DAL.SPOA
{
    /// <summary>
    /// 商户相关查询类
    /// </summary>
    public class MerchantData
    {
        /// <summary>
        /// 查询商户C账户内部ID
        /// </summary>
        /// <param name="spid">商户编号</param>
        /// <returns>C账户qqid</returns>
        public string GetMerchantCFuid(string spid)
        {
            if (string.IsNullOrEmpty(spid))
            {
                throw new Exception("spid不能为空！");
            }
            string msg = "";
            string strSql = "spid=" + spid;
            string qqid = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_MERCHANTINFO, "Fspecial", out msg);

            if (qqid == null || qqid == "")
            {
                msg = "根据商户号获取绑定的C帐号失败 spid:" + spid + " sql:" + strSql + msg;
                throw new Exception(msg); 
            }

            return qqid;
        }

        /// <summary>
        /// 查询商户证书到期备注
        /// </summary>
        /// <param name="crt_etime"></param>
        /// <param name="spid"></param>
        /// <returns></returns>
        public DataSet QueryExpiredCertOperInfo(string crt_etime, string spid)
        {
            if (string.IsNullOrEmpty(spid))
                throw new ArgumentNullException("spid");

            using (var da = MySQLAccessFactory.GetMySQLAccess("MerchantExpiredCert"))
            {
                da.OpenConn();
                string Sql = string.Format("select * from c2c_fmdb.t_operate_MediCertExpire where Fspid='{0}' and Fcrt_etime='{1}'", spid, crt_etime);
                DataSet ds = da.dsGetTotalData(Sql);
                return ds;
            }
        }

        /// <summary>
        /// 查询是否已存在某个商户证书到期备注
        /// </summary>
        /// <param name="crt_etime"></param>
        /// <param name="spid"></param>
        /// <returns></returns>
        public bool QueryExpiredCertExistOrNot(string crt_etime, string spid)
        {
            if (string.IsNullOrEmpty(spid))
                throw new ArgumentNullException("spid");

            using (var da = MySQLAccessFactory.GetMySQLAccess("MerchantExpiredCert"))
            {
                da.OpenConn();
                string Sql = string.Format("select count(1) from c2c_fmdb.t_operate_MediCertExpire where Fspid='{0}' and Fcrt_etime='{1}'", spid, crt_etime);
                if (da.GetOneResult(Sql) == "1")
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// 新增商户证书到期备注
        /// </summary>
        /// <param name="spid"></param>
        /// <param name="memo"></param>
        /// <param name="crt_etime"></param>
        /// <param name="updateUser"></param>
        public void AddExpiredCertOperInfo(string spid, string memo, string crt_etime, string updateUser)
        {
            if (string.IsNullOrEmpty(spid))
                throw new ArgumentNullException("spid");
            using (var da = MySQLAccessFactory.GetMySQLAccess("MerchantExpiredCert"))
            {
                da.OpenConn();

                //string Sql = string.Format("select * from c2c_fmdb.t_operate_MediCertExpire where Fspid='{0}'", spid);
                string Sql = string.Format("insert into c2c_fmdb.t_operate_MediCertExpire(Fspid,Fcrt_etime,FmodifyTime,Fmemo,FupdateUser)"
                    + "values('{0}','{1}','{2}','{3}','{4}')",
                    spid, crt_etime, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), memo, updateUser);
                if (!da.ExecSql(Sql))
                {
                    throw new Exception("编辑出错");
                }
            }
        }
        /// <summary>
        /// 修改商户证书到期备注
        /// </summary>
        /// <param name="spid"></param>
        /// <param name="memo"></param>
        /// <param name="crt_etime"></param>
        /// <param name="updateUser"></param>
        public void UpdateExpiredCertOperInfo(string spid, string memo, string crt_etime, string updateUser)
        {
            if (string.IsNullOrEmpty(spid))
                throw new ArgumentNullException("spid");
                using (var da = MySQLAccessFactory.GetMySQLAccess("MerchantExpiredCert"))
                {
                    da.OpenConn();

                    string Sql = string.Format("update c2c_fmdb.t_operate_MediCertExpire set FmodifyTime='{0}',Fmemo='{1}',FupdateUser='{2}' where Fspid='{3}' and Fcrt_etime='{4}'"
                        ,DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), memo, updateUser,spid,crt_etime);
                    if (!da.ExecSql(Sql))
                    {
                        throw new Exception("编辑出错");
                    }
                }
        }

        //查询商户联系信息
        public DataSet QuerySPContactInfo(string spid)
        {
            string msg = "";
            string service_name = "query_sp_contact_info_service";
            string req = "";

           
            try
            {
                req = "spid=" + spid.Trim();

                DataSet ds = CommQuery.GetOneTableFromICE(req, "QUERY_SP_CONTACT_INFO", service_name, false, out msg);
                if (msg != "")
                    throw new Exception(msg);
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception("查询商户联系信息异常！" + ex.Message);
            }
        }

        /// <summary>
        /// 修改商户联系信息
        /// </summary>
        /// <param name="aci">商户联系信息類</param>
        /// <returns></returns>
        public bool InsertOrUpdateSPContactInfo(SPContact aci, string uid, string ip)
        {
            string servicename="ui_common_update_service";
            try
            {
                //修改客服联系邮箱
                if (!UpdateServerEmail(aci, uid, ip)) return false;

                string inmsg = "spid=" + aci.spid;
                inmsg += "&CMD=" + CommQuery.ICEEncode("MOD_SP_CONTACT_INFO");

                inmsg += "&role=1|2|3|4|5|6|7";
                inmsg += "&name1=" + aci.name1;
                inmsg += "&tele1=" + aci.tele1;
                inmsg += "&mobile1=" + aci.mobile1;
                inmsg += "&standbya1=" + aci.standbya1;
                //inmsg += "&email1=" + aci;   //联系人的email  spoa不用修改，其它系统提交修改

                inmsg += "&name2=" + aci.name2;
                inmsg += "&tele2=" + aci.tele2;
                inmsg += "&qqnum2=" + aci.qqnum2;
                inmsg += "&email2=" + aci.email2;

                inmsg += "&name3=" + aci.name3;
                inmsg += "&tele3=" + aci.tele3;
                inmsg += "&qqnum3=" + aci.qqnum3;
                inmsg += "&email3=" + aci.email3;

                inmsg += "&name4=" + aci.name4;
                inmsg += "&tele4=" + aci.tele4;
                inmsg += "&qqnum4=" + aci.qqnum4;
                inmsg += "&email4=" + aci.email4;

                inmsg += "&name5=" + aci.name5;
                inmsg += "&tele5=" + aci.tele5;
                inmsg += "&qqnum5=" + aci.qqnum5;
                inmsg += "&email5=" + aci.email5;

                inmsg += "&name6=" + aci.name6;
                inmsg += "&tele6=" + aci.tele6;
                inmsg += "&qqnum6=" + aci.qqnum6;
                inmsg += "&email6=" + aci.email6;

                inmsg += "&name7=" + aci.name7;
                inmsg += "&tele7=" + aci.tele7;
                inmsg += "&qqnum7=" + aci.qqnum7;
                inmsg += "&email7=" + aci.email7;
                string reply;
                short result;
                string msg;
                if (ICEAccessFactory.ICEMiddleInvoke("ui_common_update_service", inmsg, true, out reply, out result, out msg))
                {
                    if (reply.IndexOf("result=0") == -1)
                    {
                        throw new Exception("修改商户联系信息时ICE接口返回失败：reply=" + reply);
                    }
                    else
                    {
                        return true;
                    }

                }
                else
                {
                    throw new Exception("修改商户联系信息ICE接口失败：result=" + result);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("修改商户联系信息异常！" + ex.Message);
            }
        }

        //查询商户联系信息
        public bool UpdateServerEmail(SPContact aci,string uid,string ip)
        {
            try
            {
                string inmsg = "spid=" + CommQuery.ICEEncode(aci.spid);
                inmsg += "&CMD=" + CommQuery.ICEEncode("FINANCE_MERINFO_STANDBY_XX");
                inmsg += "&standby6=" + CommQuery.ICEEncode((aci.email5 == "" ? aci.email1 : aci.email5).ToString());
                inmsg += "&client_ip=" + CommQuery.ICEEncode(ip);
                inmsg += "&operator_id=" + CommQuery.ICEEncode(uid);
                inmsg += "&module=" +"修改外卡商户客服联系邮箱";
                inmsg += "&function=" + CommQuery.ICEEncode("modify_kf_email");
                inmsg += "&modify_time=" + CommQuery.ICEEncode(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                string reply;
                short result;
                string msg;
                if (ICEAccessFactory.ICEMiddleInvoke("ui_common_update_service", inmsg,true,out reply, out result, out msg))
                {
                    if (reply.IndexOf("result=0") == -1)
                    {
                        throw new Exception("客服联系邮箱时ICE接口返回失败：reply=" + reply);
                    }
                    else
                    {
                        return true;
                    }

                }
                else
                {
                    throw new Exception("修改客服联系邮箱ICE接口失败：result=" + result);
                }

            }
            catch (Exception ex)
            {
                throw new Exception("修改客服联系邮箱异常！" + ex.Message);
            }
        }

		 //查询商户证书统计
        public DataTable queryMerchantCertStat(int s_time, int e_time, string spid, string year)
        {
            string msg = "";
            string service_name = "spm_queryCertStat_service";
            string req = "";

            try
            {
                DataSet ds = null;
                req = "timeStart=" + s_time + "&timeEnd=" + e_time + "&spid=" + spid + "&year=" + year;
               // req = "timeStart=1405356884&timeEnd=1405439684&spid=1203487901&year=2014";测试例子
                ds = CommRes.GetOneTableFromICE(service_name, req, false, out msg);
                //    ds = CommQuery.GetOneTableFromICE(req, "", service_name, false, out msg);
                if (msg != "")
                    throw new Exception("Service处理失败！" + msg);
                return ds.Tables[0];
            }
            catch (Exception e)
            {
                throw new Exception("查询余额支付功能关闭与否Service处理失败！" + msg);
            }
        }

        public DataSet QueryContract(string vendorName, string customerName, string contractNo,
                                    string startCreatedTime, string endCreatedTime,
                                    string startArchiveDay, string endArchiveDay,
                                    string startBeginDate, string endBeginDate,
                                    string startEndDate, string endEndDate,
                                    int start, int max)
        {
            try
            {
                string msg = "";
                string appid = System.Configuration.ConfigurationManager.AppSettings["ContractPlatformAppid"].Trim();

                ContractObject.BaseRequest baseRequest = new ContractObject.BaseRequest();
                baseRequest.AppId = appid;
                baseRequest.BatchId = "";

                ContractObject.RequestClass request = new ContractObject.RequestClass();
                request.StartRow = start.ToString();
                request.RowCount = max.ToString();
                request.OrderBy = "ContractNo";
                request.SortOption = "DESC";

                ContractObject.ConditionClass condition = new ContractObject.ConditionClass();
                condition.ContractNo = contractNo;
                condition.VendorName = vendorName;
                condition.CustomerName = customerName;
                condition.StartCreatedTime = startCreatedTime;
                condition.EndCreatedTime = endCreatedTime;
                condition.StartArchiveDay = startArchiveDay;
                condition.EndArchiveDay = endArchiveDay;
                condition.StartBeginDate = startBeginDate;
                condition.EndBeginDate = endBeginDate;
                condition.StartEndDate = startEndDate;
                condition.EndEndDate = endEndDate;
                //开放平台基础部[14139]
                //微信产品部[9747] 
                //在线支付部[1233]
                condition.DeptID = "14139;9747;1233";

                request.condition = condition;

                // string url = "http://10.12.189.116/CSP/Rest/CSPService/Json/QueryContract";
                string server = System.Configuration.ConfigurationManager.AppSettings["ContractPlatformServer"].Trim();
                string url = server + "/CSP/Rest/CSPService/Json/QueryContract";
                string requestStr = CommUtil.ObjToJson<ContractObject.RequestClass>(request);
                baseRequest.Request = requestStr;
                string req = CommUtil.ObjToJson<ContractObject.BaseRequest>(baseRequest);

                //返回结果
                string ret = CommUtil.SendHttpPost(url, req, true, true, out msg, "application/json; charset=UTF-8");
                if (msg != "")
                    throw new Exception("请求接口异常："+msg);
                DataSet ds = MerchantHandle.JsonTurnDSForContactQuery(ret, out msg);
                if (msg != "")
                    throw new Exception(msg);
                return ds;
            }
            catch (Exception err)
            {
                throw new Exception("查询接口异常："+err.Message);
            }
        }

        /// <summary>
        /// 合同状态查询
        /// </summary>
        /// <param name="contractid">合同号</param>
        /// <returns></returns>
        public string GetContractState(string contractid)
        {
            try
            {
                string msg = "";
                string appid = System.Configuration.ConfigurationManager.AppSettings["ContractPlatformAppid"].Trim();


                // string url = "http://10.12.189.116/CSP/Rest/CSPService/Json/QueryContract";
                string server = System.Configuration.ConfigurationManager.AppSettings["ContractPlatformServer"].Trim();
                string url = server + "/CSP/Rest/CSPService/Get/GetContractState/"+appid+"/?contractid=" + contractid;

                LogHelper.LogInfo("GetContractState send req:" + url);
                string ret = commRes.GetFromCGI(url, "utf-8", out msg);
                LogHelper.LogInfo("GetContractState return:" + ret);
                
                if (msg != "")
                    throw new Exception(msg);
                //获取成功返回{"Result":"OK","State":"已归档","Msg":""}，
                //获取不成功返回{"Result":"ERROR","State":"","Msg":"XXX"}
                string result = ret.Substring(11, ret.IndexOf(",\"State\"") - 12);
                if (result == "OK")
                {
                    int resultn = ret.IndexOf("\"State\":\"") + 9;
                    string state = ret.Substring(resultn, ret.IndexOf("\",\"Msg") - resultn);
                    return state;
                }
                else
                    return "";
            }
            catch (Exception err)
            {
                throw new Exception("查询接口异常：" + err.Message);
            }
        }

        /// <summary>
        /// 过期证书信息查询
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="spid"></param>
        /// <returns></returns>
        public DataSet QueryCertExpiredInfo(string spid,string startTime, string endTime )
        {
            if (string.IsNullOrEmpty(spid))
                throw new ArgumentNullException("spid");
            string year = DateTime.Parse(startTime).ToString("yyyy");
            using (var da = MySQLAccessFactory.GetMySQLAccess("CertExpired"))
            {
                da.OpenConn();
                string Sql = string.Format("select FSpid,FCertNum,FOldCertClinkInt from c2c_db_inc.t_spm_certExpiredStatInfo_" + year + " where Fspid='{0}' and FCreateTime between '{1}' and '{2}'", spid, startTime, endTime);
                DataSet ds = da.dsGetTotalData(Sql);
                return ds;
            }
        }

        /// <summary>
        /// 过期证书统计信息查询
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="spid"></param>
        /// <returns></returns>
        public DataSet QueryCgiInfo(string spid,string startTime, string endTime, int start, int max)
        {
            if (string.IsNullOrEmpty(spid))
                throw new ArgumentNullException("spid");
            string year = DateTime.Parse(startTime).ToString("yyyy");
            using (var da = MySQLAccessFactory.GetMySQLAccess("CertExpired"))
            {
                da.OpenConn();
                string Sql = string.Format("select FSpid,FModifyTime,FCertValidTimeEnd,FMixed,FCgiName,FClientHostIp,FCgiClkInt from c2c_db_inc.t_spm_certExpiredInfo_" + year + " where Fspid='{0}' and FCreateTime between '{1}' and '{2}' limit {3},{4}", spid, startTime, endTime, start, max);
                DataSet ds = da.dsGetTotalData(Sql);
                return ds;
            }
        }

    }

    #region 商户联系信息類
    public class SPContact
    {
        public string spid;
        public string name1;
        public string standbya1;
        public string tele1;
        public string mobile1;
        public string email1;

        public string name2;
        public string tele2;
        public string qqnum2;
        public string email2;

        public string name3;
        public string tele3;
        public string qqnum3;
        public string email3;

        public string name4;
        public string tele4;
        public string qqnum4;
        public string email4;

        public string name5;
        public string tele5;
        public string qqnum5;
        public string email5;

        public string name6;
        public string tele6;
        public string qqnum6;
        public string email6;

        public string name7;
        public string tele7;
        public string qqnum7;
        public string email7;

    }
    #endregion
}
