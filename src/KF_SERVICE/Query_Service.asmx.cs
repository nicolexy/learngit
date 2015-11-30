using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.EnterpriseServices;
using System.Configuration;
using TENCENT.OSS.CFT.KF.DataAccess;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.C2C.Finance.DataAccess;
using TENCENT.OSS.C2C.Finance.Common;
using TENCENT.OSS.C2C.Finance.Common.CommLib;
using TENCENT.OSS.C2C.Finance.BankLib;
using System.Xml;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using CFT.CSOMS.BLL.SPOA;
using CFT.CSOMS.BLL.FundModule;
using CFT.CSOMS.BLL.FreezeModule;
using CFT.CSOMS.COMMLIB;
using SunLibrary;
using CFT.CSOMS.BLL.TradeModule;
using CFT.CSOMS.BLL.CFTAccountModule;

namespace TENCENT.OSS.CFT.KF.KF_Service
{
    /// <summary>
    /// C2C交易平台（财务后台）WebService类
    /// </summary>

    [WebService(Namespace = "http://Tencent.com/OSS/C2C/Finance/Query_WebService")]
    public class Query_Service : System.Web.Services.WebService
    {
        public Finance_Header myHeader;

        public Query_Service()
        {
            //CODEGEN: 该调用是 ASP.NET Web 服务设计器所必需的
            InitializeComponent();
        }

        #region 组件设计器生成的代码

        //Web 服务设计器所必需的
        private IContainer components = null;

        /// <summary>
        /// 设计器支持所需的方法 - 不要使用代码编辑器修改
        /// 此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
        }

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion

        #region 子帐户查询
        [WebMethod(Description = "得到子帐户详细信息")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet GetChildrenInfo(string u_QQID, string Fcurtype)
        {
            string fuid = PublicRes.ConvertToFuid(u_QQID);
            if (fuid == null)
                fuid = "0";

            ICEAccess ice = null;
            if (Fcurtype != "1")
            {
                ice = new ICEAccess(PublicRes.ICEServerIP3, PublicRes.ICEPort3);
            }
            else
            {
                ice = new ICEAccess(PublicRes.ICEServerIP, PublicRes.ICEPort);
            }

            try
            {
                ice.OpenConn();
                string strwhere = "where=" + ICEAccess.URLEncode("fuid=" + fuid + "&");
                strwhere += ICEAccess.URLEncode("fcurtype=" + Fcurtype + "&");
                string strResp = "";

                DataTable dt = ice.InvokeQuery_GetDataTable(YWSourceType.用户资源, YWCommandCode.查询用户信息, fuid, strwhere, out strResp);

                if (dt == null || dt.Rows.Count == 0)
                    return null;

                ice.CloseConn();
                DataSet ds = new DataSet();
                ds.Tables.Add(dt);
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                ice.Dispose();
            }

        }

        #region 账户销户记录，判断是否存在未完成交易
        [WebMethod(Description = "判断是否存在未完成交易")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public bool LogOnUsercheckOrder(string u_QQID, string Fcurtype)
        {
            try
            {
                string errMsg = "";
                string fuid = PublicRes.ConvertToFuid(u_QQID);
                if (fuid == null)
                    fuid = "0";
                string strSql = "uid=" + fuid;
                //查询买家是否有未完成交易
                DataTable dtbuy = CommQuery.GetTableFromICE(strSql, CommQuery.QUERY_UNFINISHTRADE_BUY, out errMsg);
                //查询卖家是否有未完成交易
                DataTable dtsale = CommQuery.GetTableFromICE(strSql, CommQuery.QUERY_UNFINISHTRADE_SALE, out errMsg);
                if ((dtbuy != null && dtbuy.Rows.Count > 0) || (dtsale != null && dtsale.Rows.Count > 0))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }        
        }
        #endregion

        #region 账户销户记录，系统自动执行注销结果
        [WebMethod(Description = "系统自动执行注销结果")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public bool LogOnUserDeleteUser(string qqid, string reason, string user, string userIP, out string Msg)
        {
            try
            {
                Msg = null;
                string inmsg1 = "&uin=" + qqid;
                inmsg1 += "&client_ip=" + myHeader.UserIP;
                inmsg1 += "&memo=" + ICEAccess.ICEEncode(reason);
                inmsg1 += "&op_type=3";
                inmsg1 += "&watch_word=" + PublicRes.GetWatchWord("upay_delete_user_service");

                string reply = "";
                string msg = "";
                short result = -1;

                if (commRes.middleInvoke("upay_delete_user_service", inmsg1, true, out reply, out result, out msg))
                {
                    if (result != 0)
                    {
                        Msg = "销户接口upay_delete_user_service返回失败：result=" + result + ",msg=" + msg;
                        return false;
                    }
                    else
                    {
                        if (reply.IndexOf("result=0") > -1)//销户成功
                        {

                        }
                        else
                        {
                            Msg = "销户接口upay_delete_user_service返回失败：reply=" + reply;
                            return false;
                        }
                    }
                }
                else
                {
                    Msg = "upay_delete_user_service接口失败：result=" + result + "，msg=" + msg + "&reply=" + reply;
                    return false;
                }

                //插入历史备份数据库
                string nowTime = PublicRes.strNowTimeStander;
                MySqlAccess fmda = new MySqlAccess(PublicRes.GetConnString("ht"));
                try
                {
                    userIP = myHeader.UserIP;
                    string insertStr = "insert into c2c_fmdb.t_logon_history (fqqid,fquid,freason,handid,handip,flastMOdifyTime) values ('"
                        + qqid + "','" + PublicRes.ConvertToFuid(qqid) + "','" + commRes.replaceSqlStr(reason) + "','" + user + "','" + userIP + "','" + nowTime + "')";
                    fmda.OpenConn();
                    if (!fmda.ExecSql(insertStr))
                    {                    //da.RollBack();

                        Msg = "销户时插入历史备份数据库出错。";
                        commRes.sendLog4Log("FinanceManage.LogOnUser", Msg);
                        return false;
                    }
                    return true;
                }
                finally
                {
                    fmda.Dispose();
                }
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }

        }
        #endregion

        #region 账户销户记录，判断是否一点通用户
        [WebMethod(Description = "判断是否一点通用户")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public bool LogOnUserCheckYDT(string qqid, string Fcurtype)
        {
            //调用快捷支付-一点通业务中查询函数来确定是否开通一点通
            DataSet ds = GetBankCardBindList_New(qqid, "", "", "", "", "", "", "", "", "", int.Parse(Fcurtype), true, 2, "", 0, 5);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            return false;
        }
        #endregion


        [WebMethod(Description = "子帐户资金流水查询函数", MessageName = "GetChildrenBankRollList2")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet GetChildrenBankRollList(string u_QQID, DateTime u_BeginTime, DateTime u_EndTime, string Fcurtype, int istr, int imax)
        {
            return GetChildrenBankRollList(u_QQID, u_BeginTime, u_EndTime, Fcurtype, istr, imax, 0, string.Empty);
        }

        [WebMethod(Description = "子帐户资金流水查询函数")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet GetChildrenBankRollList(string u_QQID, DateTime u_BeginTime, DateTime u_EndTime, string Fcurtype, int istr, int imax, int Ftype, string Fmemo)
        {
            try
            {
                string fuid = PublicRes.ConvertToFuid(u_QQID);
                if (fuid == null || fuid == "" || fuid == "0")
                    return null;

                //reqid=124 是uid(180000000 - 1999999999)
                //reqid=117 是uid < 180000000的
                string reqid = "124";
                if (Int64.Parse(fuid) < 180000000)
                    reqid = "117";
                string inmsg = "fields=begin_time:" + PublicRes.ICEEncode(u_BeginTime.ToString("yyyy-MM-dd HH:mm:ss"));
                inmsg += "|end_time:" + PublicRes.ICEEncode(u_EndTime.ToString("yyyy-MM-dd HH:mm:ss"));
                inmsg += "|uid:" + fuid;
                inmsg += "|cur_type:" + Fcurtype;
                if (Ftype != 0)
                    inmsg += "|type:" + Ftype;
                if (!string.IsNullOrEmpty(Fmemo))
                    inmsg += "|memo:" + Fmemo;
                inmsg += "&flag=2";   //写死2,原因问小牛
                int start = istr - 1;
                inmsg += "&offset=" + start.ToString();  //从0开始的
                inmsg += "&limit=" + imax;
                inmsg += "&reqid=" + reqid;

                string reply;
                short sresult;
                string msg;

                if (TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.middleInvoke("common_simquery_service", inmsg, false, out reply, out sresult, out msg))
                {
                    if (sresult != 0)
                    {
                        throw new Exception("common_simquery_service接口失败：result=" + sresult + "，msg=" + "&reply=" + reply);
                    }
                    else
                    {
                        if (reply.StartsWith("result=0&res_info=ok"))
                        {
                            DataSet ds = new DataSet();
                            DataTable dt = new DataTable();

                            dt.Columns.Add("Flistid", typeof(string));
                            dt.Columns.Add("Ftype", typeof(string));
                            dt.Columns.Add("Fspid", typeof(string));
                            dt.Columns.Add("Fbalance", typeof(string));
                            dt.Columns.Add("Fpaynum", typeof(string));
                            dt.Columns.Add("Fbank_type", typeof(string));
                            dt.Columns.Add("Fsubject", typeof(string));
                            dt.Columns.Add("Faction_type", typeof(string));
                            dt.Columns.Add("Fmemo", typeof(string));
                            dt.Columns.Add("Fcreate_time", typeof(string));
                            dt.Columns.Add("Ffromid", typeof(string));
                            dt.Columns.Add("Fvs_qqid", typeof(string));
                            dt.Columns.Add("Ffrom_name", typeof(string));
                            dt.Columns.Add("Fpaynum1", typeof(string));
                            dt.Columns.Add("Fpaynum2", typeof(string));
                            dt.Columns.Add("FbalanceNum", typeof(string));

                            dt.Columns.Add("total", typeof(string));
                            dt.Columns.Add("Fcurtype", typeof(string));
                            dt.Columns.Add("Fexplain", typeof(string));
                            dt.Columns.Add("FBKid", typeof(string));
                            dt.Columns.Add("Fuid", typeof(string));
                            dt.Columns.Add("Fqqid", typeof(string));
                            dt.Columns.Add("Ftrue_name", typeof(string));
                            dt.Columns.Add("Ffrom_uid", typeof(string));
                            dt.Columns.Add("Fprove", typeof(string));
                            dt.Columns.Add("Fapplyid", typeof(string));
                            dt.Columns.Add("Fip", typeof(string));
                            dt.Columns.Add("Fmodify_time_acc", typeof(string));
                            dt.Columns.Add("Fmodify_time", typeof(string));
                            dt.Columns.Add("Fcon", typeof(string));

                            XmlDocument param = new XmlDocument();
                            param.LoadXml(reply.Replace("result=0&res_info=ok&rec_info=", ""));

                            XmlNodeList tmpElement = param.GetElementsByTagName("record");
                            if (tmpElement != null && tmpElement.Count > 0)
                            {
                                for (int i = 0; i < tmpElement.Count; i++)
                                {
                                    DataRow dr = dt.NewRow();

                                    foreach (XmlNode node in tmpElement[i].ChildNodes)
                                    {
                                        string strvalue = node.InnerText.Trim();
                                        if (node.Name.Trim() == "Flistid")
                                        {
                                            dr["Flistid"] = strvalue;
                                        }
                                        else if (node.Name.Trim() == "Ffromid")
                                        {
                                            dr["Ffromid"] = strvalue;
                                        }
                                        else if (node.Name.Trim() == "Ftype")
                                        {
                                            dr["Ftype"] = strvalue;
                                        }
                                        else if (node.Name.Trim() == "Fspid")
                                        {
                                            dr["Fspid"] = strvalue;
                                        }
                                        else if (node.Name.Trim() == "Fbalance")
                                        {
                                            dr["Fbalance"] = strvalue;
                                        }
                                        else if (node.Name.Trim() == "Fpaynum")
                                        {
                                            dr["Fpaynum"] = strvalue;
                                        }
                                        else if (node.Name.Trim() == "Fbank_type")
                                        {
                                            dr["Fbank_type"] = strvalue;
                                        }
                                        else if (node.Name.Trim() == "Fsubject")
                                        {
                                            dr["Fsubject"] = strvalue;
                                        }
                                        else if (node.Name.Trim() == "Faction_type")
                                        {
                                            dr["Faction_type"] = strvalue;
                                        }
                                        else if (node.Name.Trim() == "Fmemo")
                                        {
                                            dr["Fmemo"] = strvalue;
                                        }
                                        else if (node.Name.Trim() == "Fcreate_time")
                                        {
                                            dr["Fcreate_time"] = strvalue;
                                        }
                                        else if (node.Name.Trim() == "Ffrom_name")
                                        {
                                            dr["Ffrom_name"] = strvalue;
                                        }
                                        else if (node.Name.Trim() == "Fcon")
                                        {
                                            dr["Fcon"] = strvalue;
                                        }
                                    }

                                    if (dr["Ftype"].ToString() == "1")
                                    {
                                        dr["Fpaynum1"] = MoneyTransfer.FenToYuan(dr["Fpaynum"].ToString());
                                        dr["FbalanceNum"] = MoneyTransfer.FenToYuan(dr["Fbalance"].ToString());
                                        dr["Fvs_qqid"] = "";
                                    }
                                    else
                                    {
                                        dr["Fpaynum2"] = MoneyTransfer.FenToYuan(dr["Fpaynum"].ToString());
                                        dr["FbalanceNum"] = MoneyTransfer.FenToYuan(dr["Fbalance"].ToString());
                                        dr["Fspid"] = "";
                                    }
                                    dr["total"] = "1000";
                                    dr["FBKid"] = "";
                                    dr["Fuid"] = fuid;
                                    dr["Fqqid"] = u_QQID;
                                    dr["Ftrue_name"] = "";
                                    dr["Ffrom_uid"] = "";
                                    dr["Fprove"] = "";
                                    dr["Fapplyid"] = "";
                                    dr["Fip"] = "";
                                    dr["Fcurtype"] = Fcurtype;
                                    dr["Fexplain"] = "";
                                    dr["Fvs_qqid"] = "";
                                    dr["Fmodify_time_acc"] = dr["Fcreate_time"];
                                    dr["Fmodify_time_acc"] = dr["Fcreate_time"];
                                    dt.Rows.Add(dr);
                                }
                            }
                            ds.Tables.Add(dt);
                            return ds;
                        }
                        else
                        {
                            throw new Exception("common_simquery_service接口失败：result=" + sresult + "，msg=" + msg + "&reply=" + reply);
                        }
                    }
                }
                else
                {
                    throw new Exception("common_simquery_service接口失败：result=" + sresult + "，msg=" + msg + "&reply=" + reply);
                }

            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }

        private string QueryTradeFundInfo(string spId, string listid)
        {
            string duoFund = "";
            var tradeFund = new FundService().QueryTradeFundInfo(spId, listid);
            if (tradeFund != null && tradeFund.Rows.Count > 0)
            {
                string fundName = tradeFund.Rows[0]["Ffund_name"].ToString();
                string tmp = tradeFund.Rows[0]["Fpur_type"].ToString();
                if (tmp == "11")
                    duoFund = "(" + fundName + "转入)";
                if (tmp == "12")
                    duoFund = "(转出至" + fundName + ")";
            }
            return duoFund;
        }

        [WebMethod(Description = "子帐户资金流水查询函数(整合了页面逻辑)")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet GetChildrenBankRollListEx(string qqId, DateTime beginTime, DateTime endTime, string spId, int pageIndex, int pageMax, int fType, string fMemo)
        {
            try
            {
                int start = pageMax * (pageIndex - 1);
                if (string.IsNullOrEmpty(spId))
                    throw new Exception(string.Format("无法同时查询所有基金的流水信息，请选择指定的基金"));

                var fundInfo = FundService.GetAllFundInfo().Where(i => i.SPId == spId);

                if (fundInfo.Count() < 1)
                    throw new Exception(string.Format("找不到{0}对应的基金信息", spId));

                var bankRollList = GetChildrenBankRollList(qqId, beginTime, endTime, fundInfo.First().CurrencyType.ToString(), start + 1, pageMax, fType, fMemo);

                if (bankRollList.Tables != null && bankRollList.Tables.Count > 0)
                {
                    bankRollList.Tables[0].Columns.Add("FpaynumText", typeof(string));
                    bankRollList.Tables[0].Columns.Add("FbalanceText", typeof(string));
                    bankRollList.Tables[0].Columns.Add("FtypeText", typeof(string));
                    bankRollList.Tables[0].Columns.Add("FmemoText", typeof(string));
                    bankRollList.Tables[0].Columns.Add("FconStr", typeof(string));

                    foreach (DataRow dr in bankRollList.Tables[0].Rows)
                    {
                        switch (dr["Ftype"].ToString())
                        {
                            case "1":
                                dr["FtypeText"] = "入";
                                break;
                            case "2":
                                dr["FtypeText"] = "出";
                                break;
                            case "3":
                                dr["FtypeText"] = "冻结";
                                break;
                            case "4":
                                dr["FtypeText"] = "解冻";
                                break;
                            default:
                                dr["FtypeText"] = dr["Ftype"].ToString();
                                break;
                        }

                        switch (dr["Fmemo"].ToString())
                        {
                            case "余额宝子账户提现":
                                dr["FmemoText"] = "提现";
                                break;
                            default:
                                dr["FmemoText"] = dr["Fmemo"].ToString();
                                break;
                        }

                        string duoFund = "";
                        string listid = dr["Flistid"].ToString();
                        if (dr["FmemoText"].ToString().Equals("基金申购"))
                        {
                            if (new FundService().IfAnewBoughtFund(dr["Flistid"].ToString(), dr["Fcreate_time"].ToString()))
                            {
                                dr["FmemoText"] = "重新申购";
                            }

                            duoFund = QueryTradeFundInfo(spId, listid);//查询多基金转换
                            dr["FmemoText"] += duoFund;
                        }

                        if (dr["FmemoText"].ToString().Equals("提现"))
                        {
                            duoFund = QueryTradeFundInfo(spId, listid.Substring(listid.Length - 18));//查询多基金转换
                            dr["FmemoText"] += duoFund;
                        }

                        dr["FpaynumText"] = MoneyTransfer.FenToYuan(dr["Fpaynum"].ToString());
                        dr["FbalanceText"] = MoneyTransfer.FenToYuan(dr["Fbalance"].ToString());
                        dr["FconStr"] = MoneyTransfer.FenToYuan(dr["Fcon"].ToString());
                    }

                    return bankRollList;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("获取账户流水异常:{0}", ex.Message));
            }
            return null;
        }

        [WebMethod(Description = "子帐户交易单查询函数")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet GetChildrenFlistList(string Flistid, string Fcurtype)
        {
            string fuid = "0";
            ICEAccess ice = new ICEAccess(PublicRes.ICEServerIP3, PublicRes.ICEPort3);
            try
            {
                ice.OpenConn();
                string strwhere = "where=" + ICEAccess.URLEncode("flistid=" + Flistid + "&");
                strwhere += ICEAccess.URLEncode("fcurtype=" + Fcurtype + "&");
                string strResp = "";

                DataTable dt = ice.InvokeQuery_GetDataTable(YWSourceType.交易单资源, YWCommandCode.查询交易单信息, fuid, strwhere, out strResp);
                ice.CloseConn();

                if (dt == null || dt.Rows.Count == 0)
                    return null;

                DataSet ds = new DataSet();
                ds.Tables.Add(dt);
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception("后台查询失败:" + ex.Message);
            }
            finally
            {
                ice.Dispose();
            }
        }

        [WebMethod(Description = "子帐户冻结或解冻函数")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public void ChildrenFreezeOrUnfreeze(string u_QQID, string Fcurtype, string UpdateFstate)
        {
            string strResp = "";
            string fuid = PublicRes.ConvertToFuid(u_QQID);
            if (fuid == null)
                throw new LogicException("查询fuid无记录" + strResp);

            ICEAccess ice = new ICEAccess(PublicRes.ICEServerIP3, PublicRes.ICEPort3);
            try
            {
                ice.OpenConn();
                string strwhere = "where=" + ICEAccess.URLEncode("fuid=" + fuid + "&");
                strwhere += ICEAccess.URLEncode("fcurtype=" + Fcurtype + "&");
                string strUpdate = "data=" + ICEAccess.URLEncode("fstate=" + ICEAccess.URLEncode(UpdateFstate));
                strUpdate += ICEAccess.URLEncode("&fmodify_time=" + PublicRes.strNowTimeStander);

                if (ice.InvokeQuery_Exec(YWSourceType.用户资源, YWCommandCode.修改用户信息, fuid, strwhere + "&" + strUpdate, out strResp) != 0)
                {
                    throw new Exception("修改用户信息时出错！" + strResp);
                }

                ice.CloseConn();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                ice.Dispose();
            }
        }

        #endregion

        #region 系统公告
        [WebMethod(Description = "获取系统公告数据")]
        public DataSet GetSysBulletin(string listtype, out string msg)
        {
            msg = "";
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("INC"));
            try
            {
                da.OpenConn();
                DataSet ds = da.dsGetTotalData("select * from c2c_db_inc.t_bulletin_info where FState=1 and Flist_state=1 and FSysID="
                    + listtype + " order by FOrder desc");

                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];

                    //下面三个需要处理.
                    dt.Columns.Add("FIsNewName", typeof(System.String));

                    foreach (DataRow dr in dt.Rows)
                    {
                        dr.BeginEdit();

                        string tmp = QueryInfo.GetString(dr["FIsNew"]);
                        if (tmp == "1")
                            dr["FIsNewName"] = "new";

                        tmp = QueryInfo.GetString(dr["FStandBy1"]);
                        if (tmp == "1")
                            dr["FTitle"] = "<font color=red>" + dr["FTitle"].ToString() + "</font>";

                        dr.EndEdit();
                    }
                }

                return ds;
            }
            catch (Exception err)
            {
                msg = err.Message;
                return null;
            }
            finally
            {
                da.Dispose();
            }
        }

        [WebMethod(Description = "获取系统公告标题")]
        public string GetSysBulletinTitleById(int id, out string msg)
        {
            string result = string.Empty;
            msg = "";
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("INC"));
            try
            {
                da.OpenConn();
                result = da.GetOneResult(@"
                    select Ftitle from c2c_db_inc.t_bulletin_info 
                        where FState=1 and Flist_state=1 and FID=" + id);
            }
            catch (Exception err)
            {
                msg = err.Message + err.StackTrace;
            }
            finally
            {
                da.Dispose();
            }
            return result;
        }

        [WebMethod(Description = "获取银行维护公告数据")]
        public DataSet GetSysBankBulletin(string banktype, out string msg)
        {
            msg = "";
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("INC"));
            try
            {
                da.OpenConn();
                string strsql = "select * from c2c_db_inc.t_bankbulletin_info_all  ";
                string strwhere = "where 1=1  ";
                if (banktype != "" && banktype != null)
                {
                    strwhere += "  and  Fbanktype=" + banktype + "";
                }
                strwhere += "  order by fid desc ";
                DataSet ds = da.dsGetTotalData(strsql + strwhere);

                return ds;
            }
            catch (Exception err)
            {
                msg = err.Message;

                log4net.ILog log = log4net.LogManager.GetLogger("QueryService.T_BANKBULLETIN_INFO");
                if (log.IsErrorEnabled)
                    log.Error(banktype, err);

                return null;
            }
            finally
            {
                da.Dispose();
            }
        }

        [WebMethod(Description = "获取银行接口数据")]
        public DataSet GetSysBankInterface(string busineType, string banktype, out string msg)
        {
            msg = "";
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("INC"));
            try
            {
                da.OpenConn();
                string strsql = "select * from c2c_db_inc.t_bankbulletin_type_all  ";
                string strwhere = "where 1=1  ";
                if (busineType != "" && busineType != null)
                {
                    strwhere += "  and  Fbusinetype=" + int.Parse(busineType) + "";
                }
                if (banktype != "" && banktype != null)
                {
                    strwhere += "  and  Fbanktype=" + banktype + "";
                }
                strwhere += "  order by fid desc ";
                DataSet ds = da.dsGetTotalData(strsql + strwhere);

                return ds;
            }
            catch (Exception err)
            {
                msg = err.Message;
                return null;
            }
            finally
            {
                da.Dispose();
            }
        }

        [WebMethod(Description = "查询银行接口信息ById")]
        public T_BANKBULLETIN_TYPE_ALL QueryBankInterfaceById(string fid, out string msg)
        {
            msg = "";
            MySqlAccess dazw = new MySqlAccess(PublicRes.GetConnString("INC"));
            try
            {
                if (fid == null || fid == "")
                {
                    msg = "查询的ID不能为空！";
                    return null;
                }
                dazw.OpenConn();
                string strSql = "select * from  c2c_db_inc.t_bankbulletin_type_all  where Fid='" + fid + "' ";
                DataTable dt = dazw.GetTable(strSql);

                T_BANKBULLETIN_TYPE_ALL um = new T_BANKBULLETIN_TYPE_ALL();
                um.LoadFromDB(dt.Rows[0]);

                return um;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return null;
            }
            finally
            {
                dazw.Dispose();
            }
        }

        [WebMethod(Description = "查询银行接口信息ByObjid")]
        public T_BANKBULLETIN_TYPE_ALL QueryBankInterfaceByObjid(string objid, string checkType, out string msg)
        {

            msg = "";
            try
            {
                DataTable dt = GetCheckInfo(objid, checkType, out msg);
                if (dt == null)
                {
                    return null;
                }
                T_BANKBULLETIN_TYPE_ALL um = new T_BANKBULLETIN_TYPE_ALL();
                um.LoadFromParamDB(dt);
                return um;

            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return null;
            }
        }

        [WebMethod(Description = "查询新增或修改银行公告时间段是否已存在")]
        public bool IsRepeatedTime(string Fid, string Fbanktype, string Fbusinetype, string Fstartime, string Fendtime, out string msg)
        {
            msg = "";
            MySqlAccess dazw = new MySqlAccess(PublicRes.GetConnString("INC"));
            try
            {
                dazw.OpenConn();
                string strSql = "select * from  c2c_db_inc.t_bankbulletin_type_all  where Fbanktype='" + Fbanktype + "'and Fbusinetype=" + Fbusinetype;
                if (Fid != "")
                    strSql += " and Fid<>" + int.Parse(Fid);
                DataTable dt = dazw.GetTable(strSql);
                DateTime start = DateTime.Parse(Fstartime);//新增公告开始时间
                DateTime end = DateTime.Parse(Fendtime);//新增公告结束时间
                if (dt == null || dt.Rows.Count == 0)
                    return false;
                else
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        DateTime dbStart = DateTime.Parse(dr["Fstartime"].ToString());
                        DateTime dbEnd = DateTime.Parse(dr["Fendtime"].ToString());
                        if (end <= dbStart || dbEnd <= start)
                        {

                        }
                        else
                        {
                            msg = "与数据库公告时间段重复！" + dr["Fstartime"].ToString() + "--" + dr["Fendtime"].ToString();
                            return true;
                        }
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return true;
            }
            finally
            {
                dazw.Dispose();
            }
        }

        [WebMethod(Description = "查询银行维护公告信息ById")]
        public T_BANKBULLETIN_INFO_ALL QueryBankBulletinById(string fid, out string msg)
        {
            msg = "";
            MySqlAccess dazw = new MySqlAccess(PublicRes.GetConnString("INC"));
            try
            {
                if (fid == null || fid == "")
                {
                    msg = "查询的ID不能为空！";
                    return null;
                }

                dazw.OpenConn();
                string strSql = "select * from  c2c_db_inc.t_bankbulletin_info_all  where Fid='" + fid + "' ";
                DataTable dt = dazw.GetTable(strSql);

                T_BANKBULLETIN_INFO_ALL um = new T_BANKBULLETIN_INFO_ALL();
                um.LoadFromDB(dt.Rows[0]);
                return um;

            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return null;
            }
            finally
            {
                dazw.Dispose();
            }
        }

        [WebMethod(Description = "查询银行维护公告信息ByObjid")]
        public T_BANKBULLETIN_INFO_ALL QueryBankBulletinInfoByObjid(string objid, string checkType, out string msg)
        {
            msg = "";
            try
            {
                DataTable dt = GetCheckInfo(objid, checkType, out msg);
                if (dt == null)
                {
                    return null;
                }
                T_BANKBULLETIN_INFO_ALL um = new T_BANKBULLETIN_INFO_ALL();
                um.LoadFromParamDB(dt);
                return um;

            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return null;
            }
        }

        private DataTable GetCheckInfo(string objid, string checkType, out string msg)
        {
            msg = "";
            MySqlAccess daht = new MySqlAccess(PublicRes.GetConnString("HT"));
            try
            {
                if (objid == null || objid == "")
                {
                    msg = "审批单ID不能为空！";
                    return null;
                }

                daht.OpenConn();
                string strSql = "select Fid from c2c_fmdb.t_check_main where fobjid='" + objid + "' and fcheckType='" + checkType + "' ";
                DataTable dt_main = daht.GetTable(strSql);
                if (dt_main == null || dt_main.Rows.Count != 1)
                {
                    msg = "审批单任务单ID" + objid + "对应的记录数不唯一";
                    return null;
                }

                strSql = "select * from c2c_fmdb.t_check_param where fcheckid=" + dt_main.Rows[0]["Fid"].ToString() + "";
                DataTable dt_param = daht.GetTable(strSql);
                if (dt_param == null || dt_param.Rows.Count == 0)
                {
                    msg = "审批单ID" + dt_main.Rows[0]["Fid"].ToString() + "对应的参数为空";
                    return null;
                }

                return dt_param;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return null;
            }
            finally
            {
                daht.Dispose();
            }
        }

        [WebMethod(Description = "获取生活缴费维护公告数据")]
        public DataSet GetUtility_charge(string servicecode, out string msg)
        {
            msg = "";
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("UC"));
            try
            {
                da.OpenConn();
                //string strsql="select * from public_utility_charge.t_puc_new_service_info  ";
                string strsql = "select A.*,B.FProvince_name,B.FCity_name,B.FArea_name from public_utility_charge_platform.t_charge_resources_info A, public_utility_charge_platform.t_charge_area_info B  ";
                string strwhere = "where B.FArea_id=A.FArea_id  ";
                if (servicecode != "" && servicecode != null)
                {
                    //strwhere+="  and  Fservicecode="+servicecode+"";
                    strwhere += "  and  A.FResourcesId=" + servicecode + "";
                }
                DataSet ds = da.dsGetTotalData(strsql + strwhere);

                return ds;
            }
            catch (Exception err)
            {
                msg = err.Message;

                log4net.ILog log = log4net.LogManager.GetLogger("QueryService.GetUtility_charge");
                if (log.IsErrorEnabled)
                    log.Error(servicecode, err);

                return null;
            }
            finally
            {
                da.Dispose();
            }
        }

        [WebMethod(Description = "查询生活缴费维护公告信息ById")]
        public T_PUCNEWSERVICE_INFO QueryUCBulletinById(string fid, string uctype, out string msg)
        {
            msg = "";
            MySqlAccess dazw = new MySqlAccess(PublicRes.GetConnString("UC"));
            try
            {
                if (fid == null || fid == "")
                {
                    msg = "查询的ID不能为空！";
                    return null;
                }

                dazw.OpenConn();                
                string strSql = "select FResourcesId as Fservicecode,'8' as Fuctype,Frepair_limit_tips as Ftips,Frepair_limit_starttime as Fstartime,Frepair_limit_endtime as Fendtime from  public_utility_charge_platform.t_charge_resources_info  where FResourcesId='" + fid + "'";
                DataTable dt = dazw.GetTable(strSql);
                T_PUCNEWSERVICE_INFO um = new T_PUCNEWSERVICE_INFO();
                um.LoadFromDB(dt.Rows[0]);

                return um;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return null;
            }
            finally
            {
                dazw.Dispose();
            }
        }

        [WebMethod(Description = "查询生活缴费维护公告信息ByObjid")]
        public T_PUCNEWSERVICE_INFO QueryUCBulletinInfoByObjid(string objid, string checkType, out string msg)
        {
            msg = "";
            try
            {
                DataTable dt = GetCheckInfo(objid, checkType, out msg);
                if (dt == null)
                {
                    return null;
                }
                T_PUCNEWSERVICE_INFO um = new T_PUCNEWSERVICE_INFO();
                um.LoadFromParamDB(dt);

                return um;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return null;
            }
        }

        [WebMethod(Description = "系统公告发布函数")]
        public bool SysBulletinIssue(string sysid, out string msg)
        {
            msg = "";
            try
            {
                return SysBulletinClass.Issue(sysid, out msg);
            }
            catch (Exception err)
            {
                msg = err.Message;
                return false;
            }
        }

        [WebMethod(Description = "系统公告向前调整函数")]
        public bool SysBulletinGoPrior(string fid, string UserIP, out string msg)
        {
            msg = "";
            try
            {
                return SysBulletinClass.GoPrior(fid, UserIP, out msg);
            }
            catch (Exception err)
            {
                msg = err.Message;
                return false;
            }
        }

        [WebMethod(Description = "系统公告向后调整函数")]
        public bool SysBulletinGoNext(string fid, string UserIP, out string msg)
        {
            msg = "";
            try
            {
                return SysBulletinClass.GoNext(fid, UserIP, out msg);
            }
            catch (Exception err)
            {
                msg = err.Message;
                return false;
            }
        }

        [WebMethod(Description = "系统公告删除函数")]
        public bool SysBulletinDel(string fid, string UserIP, out string msg)
        {
            msg = "";
            try
            {
                return SysBulletinClass.Del(fid, UserIP, out msg);
            }
            catch (Exception err)
            {
                msg = err.Message;
                return false;
            }
        }

        [WebMethod(Description = "系统公告调整到历史函数")]
        public bool SysBulletinGoHistory(string fid, string UserIP, out string msg)
        {
            msg = "";
            try
            {
                return SysBulletinClass.GoHistory(fid, UserIP, out msg);
            }
            catch (Exception err)
            {
                msg = err.Message;
                return false;
            }
        }

        [WebMethod(Description = "获取系统公告函数")]
        public SysBulletinClass GetOneSysBulletin(string fid, out string msg)
        {
            msg = "";
            try
            {
                return new SysBulletinClass(Int32.Parse(fid));
            }
            catch (Exception err)
            {
                msg = err.Message;
                return null;
            }
        }

        [WebMethod(Description = "创建系统公告函数")]
        public bool AddOneSysBulletin(SysBulletinClass sc, string UserIP, out string msg)
        {
            msg = "";
            try
            {
                return sc.CreateNew(UserIP, out msg);
            }
            catch (Exception err)
            {
                msg = err.Message;
                return false;
            }
        }

        [WebMethod(Description = "创建系统公告函数")]
        public T_BANKBULLETIN_INFO GetB()
        {
            return new T_BANKBULLETIN_INFO();
        }

        [WebMethod(Description = "修改系统公告函数")]
        public bool ChangeOneSysBulletin(SysBulletinClass sc, string UserIP, out string msg)
        {
            msg = "";
            try
            {
                return sc.Change(UserIP, out msg);
            }
            catch (Exception err)
            {
                msg = err.Message;
                return false;
            }
        }
        #endregion

        #region 以下是公告2期
        [WebMethod(Description = "银行公告查询 利用接口查询  不分页 不使用了")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet QueryBankBulletinNotUse(int businesstype, int op_support_flag, int banktype, string bulletin_id)
        {
            string msg = "";
            try
            {
                string service_name = "bank_channel_bulletin_query_service";//接口名
                string req = "";

                DataSet ds = null;
                req = "businesstype=" + businesstype;
                if (op_support_flag != 0)
                    req += "&op_support_flag=" + op_support_flag;
                if (banktype != 0)
                    req += "&banktype=" + banktype;
                if (bulletin_id != "")
                    req += "&bulletin_id=" + bulletin_id;
                int offset = 0;
                req += "&limit=50&channeltype=1";//limit=50先设定每次查询50条，因为怕网络一次传不了那么多数据

                int totalNum = 0;

                ds = CommQuery.GetXmlToDataSetFromICE(req + "&offset=0", "", service_name, out msg, false);
                if (msg != "")
                {
                    throw new Exception("Service处理失败！" + msg);
                }

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count < 50)
                    {
                        return ds;
                    }
                    else
                    {
                        totalNum = CommQuery.GetTotalNumFromICE(req + "&offset=0", "", service_name, out msg, false);//总记录数
                        //需多次查询合并结果
                        int index = 1;
                        do
                        {
                            offset = 50 * index - 1;
                            DataSet ds2 = CommQuery.GetXmlToDataSetFromICE(req + "&offset=" + offset, "", service_name, out msg, false);
                            index++;
                            ds = ToOneDataset(ds, ds2);//将两个库的数据合并到一个库
                        } while (50 * index < totalNum);
                    }
                }
                return ds;
            }
            catch (Exception e)
            {
                throw new Exception("Service处理失败！" + e.Message);
            }
        }

        [WebMethod(Description = "银行公告查询 利用接口查询")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet QueryBankBulletin(int businesstype, int op_support_flag, int banktype, string bulletin_id, int limit, int offset)
        {
            string msg = "";
            try
            {
                string service_name = "bank_channel_bulletin_query_service";//接口名
                string req = "";

                DataSet ds = null;
                req = "businesstype=" + businesstype;
                if (op_support_flag != 0)
                    req += "&op_support_flag=" + op_support_flag;
                if (banktype != 0)
                    req += "&banktype=" + banktype;
                if (bulletin_id != "")
                    req += "&bulletin_id=" + bulletin_id;
                req += "&limit=" + limit + "&channeltype=1&offset=" + offset;//chengzi测试接口最多limit=8，因为怕网络一次传不了那么多数据

                ds = CommQuery.GetXmlToDataSetFromICE(req, "", service_name, out msg, false);
                if (msg != "")
                {
                    throw new Exception("Service处理失败！" + msg);
                }
                return ds;
            }
            catch (Exception e)
            {
                throw new Exception("Service处理失败！" + e.Message);
            }
        }

        //将两个表结构一致的dataset合并到一个dataset
        private static DataSet ToOneDataset(DataSet ds, DataSet ds2)
        {
            DataSet dsAll = new DataSet();

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                dsAll.Tables.Add(ds.Tables[0].Copy());
                if (ds2 != null && ds2.Tables.Count > 0)
                {//分库表不为null
                    foreach (DataTable tbl in ds2.Tables)
                        if (tbl.Rows.Count > 0)//分库表不为null
                        {
                            foreach (DataRow dr in tbl.Rows)
                            {
                                dsAll.Tables[0].ImportRow(dr);//将记录加入到一个表里
                            }
                        }
                }
            }
            else
            {
                if (ds2 != null && ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
                    dsAll.Tables.Add(ds2.Tables[0].Copy());
            }
            return dsAll;
        }

        [WebMethod(Description = "将接口返回公告信息转换成公告类")]
        public T_BANKBULLETIN_INFO TurnBankBulletinClass(DataSet ds)
        {
            try
            {
                DataTable dt = new DataTable();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    dt = ds.Tables[0];

                    T_BANKBULLETIN_INFO um = new T_BANKBULLETIN_INFO();
                    um.LoadFromDB(dt.Rows[0]);
                    return um;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw new Exception("公告信息转换成公告类Service处理失败！" + ex.Message);
                return null;
            }
        }

        [WebMethod(Description = "查询银行接口信息ByObjid 公告2期")]
        public T_BANKBULLETIN_INFO QueryBankBulletinByObjid(string objid, string checkType, out string msg)
        {
            msg = "";
            try
            {
                DataTable dt = GetCheckInfo(objid, checkType, out msg);
                if (dt == null)
                {
                    return null;
                }
                T_BANKBULLETIN_INFO um = new T_BANKBULLETIN_INFO();
                um.LoadFromParamDB(dt);
                return um;

            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return null;
            }
        }
        #endregion

        #region 银行卡查询

        [WebMethod(Description = "查询银行卡信息")]  //根据银行卡号查订单号
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet Getfbank_orderList(string fpay_acc, string Date, int istr, int imax)
        {
            try
            {
                if (string.IsNullOrEmpty(Date))
                {
                    throw new Exception("日期不能为空！");
                }
                string zwskDate = "20130331";
                try
                {
                    zwskDate = ConfigurationManager.AppSettings["ZWSKDate"];
                }
                catch
                {
                    zwskDate = "20130331";
                }
                if (string.IsNullOrEmpty(zwskDate))
                {
                    zwskDate = "20130331";
                }

                BankDataClass cuser = new BankDataClass(fpay_acc, Date);
                DataSet ds = null;

                DateTime d1 = DateTime.ParseExact(Date, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
                DateTime d2 = DateTime.ParseExact(zwskDate, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
                if (d1.CompareTo(d2) >= 0)
                {
                    ds = cuser.GetResultX(istr, imax, "ZWSK");
                }
                else
                {
                    ds = cuser.GetResultX(istr, imax, "ZW");  //查老数据
                }

                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region “中行信用卡”和“兴业信用卡”银行卡查询

        [WebMethod(Description = "'中行信用卡'和'兴业信用卡'查询银行卡信息")]  //根据银行卡号查订单号
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet Getfbank_twoBank_orderList(string fpay_acc, DateTime Date)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("ZWNEWTABLE"));
            MySqlAccess da2 = new MySqlAccess(PublicRes.GetConnString("ZWOLDTABLE"));
            DataSet ds = new DataSet();
            try
            {
                da.OpenConn();
                da2.OpenConn();
                string date = Date.ToString("yyyyMM");
                string begintime = Date.ToString("yyyy-MM-dd HH:mm:ss");
                string endtime = Date.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss");
                string serBankaccno = BankLib.BankIOX.Encrypt(fpay_acc);//银行卡加密
                // 先使用新表查询
                string strSql = "select  Fcard_no as fpay_acc ,Fbill_no as fbank_order,Famount as Famt,Fbiz_type from c2c_db_pos.t_bank_pos_" + date.Substring(0, 6) + " where Fcard_no='" + serBankaccno + "'and Fcreate_time between '" + begintime + "' and '" + endtime + "'";

                ds = da.dsGetTotalData(strSql);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)//查新表
                {
                    ds.Tables[0].Columns.Add("Fbiz_type_str", typeof(String));//业务状态
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        dr["fpay_acc"] = fpay_acc;//页面显示明文卡号
                        if (!(dr["Fbiz_type"] is DBNull))
                        {
                            string tmp = dr["Fbiz_type"].ToString();
                            if (tmp == "10100")
                            {
                                dr["Fbiz_type_str"] = "支付";
                            }
                            else if (tmp == "10200")
                            {
                                dr["Fbiz_type_str"] = "提现";
                            }
                            else if (tmp == "10300")
                            {
                                dr["Fbiz_type_str"] = "退款";
                            }
                            else if (tmp == "10400")
                            {
                                dr["Fbiz_type_str"] = "ATM充值";
                            }
                            else if (tmp == "10500")
                            {
                                dr["Fbiz_type_str"] = "session索引KEY";
                            }
                            else
                            {
                                dr["Fbiz_type_str"] = "未知：" + tmp;
                            }
                        }
                    }
                    return ds;

                }
                else//查旧表
                {
                    strSql = "select  Fbankid as fpay_acc ,FBillNO as fbank_order,FAmount as Famt from c2c_db_pos.t_pos_water where Fbankid='" + serBankaccno + "'and FModifyTime between '" + begintime + "' and '" + endtime + "' ";
                    // ds = QueryInfo.GetTable(strSql, istr, imax, "ZWOLDTABLE");
                    ds = da2.dsGetTotalData(strSql);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)//查新表
                    {
                        ds.Tables[0].Columns.Add("Fbiz_type_str", typeof(String));//业务状态
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            dr["fpay_acc"] = fpay_acc;//页面显示明文卡号
                        }
                    }
                }
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                da.Dispose();
                da2.Dispose();
            }
        }
        #endregion

        #region 同步订单状态
        [WebMethod(Description = "同步订单状态")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public bool Synchro_State(string transaction_id, out string msg)
        {
            try
            {
                string inmsg = "transaction_id=" + transaction_id;
                inmsg += "&cur_type=1";
                inmsg += "&business_type=1";

                string reply;
                short sresult;

                if (TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.middleInvoke("medi_sync_order_service", inmsg, true, out reply, out sresult, out msg))
                {
                    if (sresult != 0)
                    {
                        msg = "medi_sync_order_service接口失败：result=" + sresult + "，msg=" + "&reply=" + reply;
                        return false;
                    }
                    else
                    {
                        if (reply.StartsWith("result=0"))
                        {
                            return true;
                        }
                        else
                        {
                            msg = "medi_sync_order_service接口失败：result=" + sresult + "，msg=" + msg + "&reply=" + reply;
                            return false;
                        }
                    }
                }
                else
                {
                    msg = "medi_sync_order_service接口失败：result=" + sresult + "，msg=" + msg + "&reply=" + reply;
                    return false;
                }

            }
            catch (Exception err)
            {
                msg = err.Message;
                return false;
            }
        }
        #endregion

        #region 二次登录密码
        [WebMethod(Description = "是否设置了二次登录函数")]
        public bool IsSecondPasseword(string qqid)
        {
            try
            {
                if (qqid == null || qqid.Trim().Length < 3)
                {
                    throw new LogicException("帐号不能为空");
                    return false;
                }

                string errMsg = "";
                string strSql = "uin=" + qqid;
                strSql += "&sign=1";
                strSql += "&login_type=1";
                string Flogin_passwd = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_RELATION, "Flogin_passwd", out errMsg);

                if (Flogin_passwd == null)
                {
                    throw new LogicException("CommQuery.QUERY_RELATION查询Flogin_passwd出错");
                }

                if (Flogin_passwd == "")
                    return false;
                else
                    return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        [WebMethod(Description = "是否设置了二次登录函数")]
        public bool SuspendSecondPasseword(string qqid, out string msg)
        {
            try
            {
                string inmsg = "uin=" + qqid;
                inmsg += "&op_type=3";
                inmsg += "&cmd=5";
                inmsg += "&watch_word=3188d76a6a9c63b32f60cdeff47b3618";   //md5("watch_word")
                string reply;
                short sresult;

                if (TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.middleInvoke("tlp_setting_service", inmsg, true, out reply, out sresult, out msg))
                {
                    if (sresult != 0)
                    {
                        msg = "tlp_setting_service接口失败：result=" + sresult + "，msg=" + "&reply=" + reply;
                        return false;
                    }
                    else
                    {
                        if (reply.StartsWith("result=0"))
                        {
                            return true;
                        }
                        else
                        {
                            msg = "tlp_setting_service接口失败：result=" + sresult + "，msg=" + msg + "&reply=" + reply;
                            return false;
                        }
                    }
                }
                else
                {
                    msg = "tlp_setting_service接口失败：result=" + sresult + "，msg=" + msg + "&reply=" + reply;
                    return false;
                }

            }
            catch (Exception err)
            {
                msg = err.Message;
                return false;
            }
        }
        #endregion

        #region PNR签约
        [WebMethod(Description = "获取C帐号PNR签约商户函数")]
        public DataSet GetPNRSpid(string qqid)
        {
            string Sql = "select Fpayee from direct_pay.t_dpay_agent_pay_whitelist where Fpayer='" + qqid + "' and Flstate=0";

            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("UK"));
            try
            {
                da.OpenConn();
                return da.dsGetTotalData(Sql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                da.Dispose();
            }
        }
        #endregion

        //通过类型查询返回字段表信息
        public static DataSet QueryDicInfoByType(string type, out string Msg)
        {
            Msg = "";
            try
            {
                //先查询出总笔数
                string icesql = "type=" + type;
                string count = CommQuery.GetOneResultFromICE(icesql, CommQuery.QUERY_DIC_COUNT, "acount", out Msg);
                if (count == null || count == "" || count == "0")
                {
                    return null;
                }
                int allCount = Convert.ToInt32(count);
                if (allCount <= 0)
                {
                    return null;
                }

                DataTable dt_all = new DataTable();
                dt_all.Columns.Add("Fno", System.Type.GetType("System.String"));
                dt_all.Columns.Add("FType", System.Type.GetType("System.String"));
                dt_all.Columns.Add("Fvalue", System.Type.GetType("System.String"));
                dt_all.Columns.Add("Fmemo", System.Type.GetType("System.String"));
                dt_all.Columns.Add("Fsymbol", System.Type.GetType("System.String"));

                string strSqlTmp = "type=" + type;
                int limitStart = 0;
                int onceCount = 20;//一次返回笔数

                while (allCount > limitStart)
                {
                    string strSqlLimit = "&strlimit=limit " + limitStart + "," + onceCount;
                    string strSql = strSqlTmp + strSqlLimit;
                    DataTable dt_one = CommQuery.GetTableFromICE(strSql, CommQuery.QUERY_DIC, out Msg);

                    if (dt_one != null && dt_one.Rows.Count > 0)
                    {
                        foreach (DataRow dr2 in dt_one.Rows)
                        {
                            string fno = dr2["Fno"].ToString();
                            string FType = dr2["FType"].ToString();
                            string Fvalue = dr2["Fvalue"].ToString();
                            string Fmemo = dr2["Fmemo"].ToString();
                            string symbol = dr2["Fsymbol"].ToString();

                            DataRow drNew = dt_all.NewRow();
                            drNew["Fno"] = fno;
                            drNew["FType"] = FType;
                            drNew["Fvalue"] = Fvalue;
                            drNew["Fmemo"] = Fmemo;
                            drNew["Fsymbol"] = symbol;
                            dt_all.Rows.Add(drNew);
                        }

                    }

                    limitStart = limitStart + onceCount;
                }
                DataSet ds = new DataSet();
                ds.Tables.Add(dt_all);

                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [WebMethod(Description = "是否为快速交易用户判断函数")]
        public bool IsFastPayUser(string qqid)
        {
            try
            {
                if (qqid == null || qqid.Trim().Length < 3)
                {
                    throw new LogicException("帐号不能为空");
                    return false;
                }

                string errMsg = "";
                string strSql = "uin=" + qqid;
                string sign = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_RELATION, "Fsign", out errMsg);

                if (sign == null)
                {
                    throw new LogicException("CommQuery.QUERY_RELATION查询FSign出错");
                }

                if (sign == "2")
                    return true;
                else
                    return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /*
        2002  CR_CONNECTION_ERROR     连接数据库错误
        1149  ER_SYNTAX_ERROR  					语法错误
        1146  ER_NO_SUCH_TABLE    				没有此用户
        1046  ER_NO_DB_ERROR  			数据库不存在
        */
        [WebMethod(Description = "查询用户帐户表")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet GetUserAccount(string u_QQID, int fcurtype, int istr, int imax)
        {
            if (myHeader == null)
            {
                throw new Exception("不正确的调用方法！");
            }

            string strUserID = myHeader.UserName;
            string strPassword = myHeader.UserPassword;
            string strIP = myHeader.UserIP;
            string strRightCode = "GetUserAccount";
            int sign = 0;
            string detail, actionType;
            actionType = "查询用户帐户表";

            try
            {
                string fuid = PublicRes.ConvertToFuid(u_QQID);
                sign = 1;

                if (fuid == null)
                    fuid = "0";

                string femail = "";
                string fmobile = "";
                string fatt_id = "";
                string ftrueName = "";
                string fz_amt = ""; //分账冻结金额 yinhuang 2014/1/8

                ICEAccess ice = new ICEAccess(PublicRes.ICEServerIP, PublicRes.ICEPort);
                //MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("AP"));
                try
                {
                    string errMsg = "";
                    string strSql = "uid=" + fuid;

                    fatt_id = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_USERATT, "Fatt_id", out errMsg);

                    fatt_id = QueryInfo.GetString(fatt_id);

                    DataTable dt_userInfo = CommQuery.GetTableFromICE(strSql, CommQuery.QUERY_USERINFO, out errMsg);
                    if (dt_userInfo != null && dt_userInfo.Rows.Count == 1)
                    {
                        femail = dt_userInfo.Rows[0]["Femail"].ToString();
                        fmobile = dt_userInfo.Rows[0]["Fmobile"].ToString();
                        ftrueName = dt_userInfo.Rows[0]["FtrueName"].ToString();

                        string fusertype = QueryInfo.GetString(dt_userInfo.Rows[0]["Fuser_type"]);
                        if (fusertype == "2")//公司类型
                        {
                            ftrueName = dt_userInfo.Rows[0]["Fcompany_name"].ToString();
                        }
                    }

                    ice.OpenConn();
                    string strwhere = "where=" + ICEAccess.URLEncode("fuid=" + fuid + "&");
                    strwhere += ICEAccess.URLEncode("fcurtype=" + fcurtype + "&");

                    string strResp = "";
                    DataTable dt = ice.InvokeQuery_GetDataTable(YWSourceType.用户资源, YWCommandCode.查询用户信息, fuid, strwhere, out strResp);
                    if (dt == null || dt.Rows.Count == 0)
                        throw new LogicException("调用ICE查询T_user无记录" + strResp);

                    ice.CloseConn();

                    //da.OpenConn();
                    //string sql = "select * from app_platform.t_account_freeze where Fuin = '" + u_QQID + "'";
                    //DataTable dt2 = da.GetTable(sql);
                    //if (dt2 != null && dt2.Rows.Count > 0)
                    //{
                    //    fz_amt = dt2.Rows[0]["Famount"].ToString();
                    //}
                    fz_amt = new SettleService().getAmount(fuid, "uid");
                    dt.Columns.Add("Femail", typeof(System.String));
                    dt.Columns.Add("Fmobile", typeof(System.String));
                    dt.Columns.Add("Att_id", typeof(System.String));
                    dt.Columns.Add("UserRealName2", typeof(System.String));
                    dt.Columns.Add("Ffz_amt", typeof(System.String));
                    dt.Rows[0]["Femail"] = femail;
                    dt.Rows[0]["Fmobile"] = fmobile;
                    dt.Rows[0]["Att_id"] = fatt_id;
                    dt.Rows[0]["UserRealName2"] = ftrueName;
                    dt.Rows[0]["Ffz_amt"] = fz_amt;

                    DataSet ds = new DataSet();
                    ds.Tables.Add(dt);

                    return ds;
                }
                finally
                {
                    ice.Dispose();
                    //da.Dispose();
                }
            }
            catch (Exception e)
            {
                sign = 0;
                return null;
            }
            finally
            {
                PublicRes.writeSysLog(strUserID, strIP, "query", actionType, sign, u_QQID, "用户");
            }
        }

        [WebMethod(Description = "通过微信查询用户帐户表")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet GetUserAccountFromWechat(string u_QQID, int istr, int imax)
        {
            if (myHeader == null)
            {
                throw new Exception("不正确的调用方法！");
            }
            string strUserID = myHeader.UserName;
            string strIP = myHeader.UserIP;
            int sign = 0;
            string actionType = "查询用户帐户表";
            string errMsg = "";

            try
            {
                string fuid = PublicRes.ConvertToFuid(u_QQID);
                string strReq = "uid=" + fuid;
                sign = 1;
                var task= System.Threading.Tasks.Task<DataTable>.Factory.StartNew(() =>
                {
                    //使用新线程查询另一个接口   加速页面响应
                    using (ICEAccess ice = new ICEAccess(PublicRes.ICEServerIP, PublicRes.ICEPort))
                    {
                        ice.OpenConn();
                        string strwhere = "where=" + ICEAccess.URLEncode("fuid=" + fuid + "&") + ICEAccess.URLEncode("fcurtype=" + 1 + "&");  
                        string strResp = "";
                        return ice.InvokeQuery_GetDataTable(YWSourceType.用户资源, YWCommandCode.查询用户信息, fuid, strwhere, out strResp);
                    }  
                });
                var ds=CommQuery.GetDataSetFromICE(strReq, CommQuery.QUERY_USERINFO, out errMsg);
                DataTable dt1 = task.Result;
                if (ds != null && ds.Tables.Count > 0 && dt1!=null)
                {
                    var dt = ds.Tables[0];
                    if (dt.Rows.Count > 0 && dt1.Rows.Count > 0)
                    {
                        dt.Columns.Add("Fbalance"); 
                        dt.Columns.Add("Fcon");
                        var row = dt.Rows[0];
                        var row1 = dt1.Rows[0];
                        row["Fbalance"] = row1["Fbalance"];
                        row["Fcon"] = row1["Fcon"];
                    }
                }
                return ds;
            }
            catch (Exception e)
            {
                sign = 0;
                return null;
            }
            finally
            {
                PublicRes.writeSysLog(strUserID, strIP, "query", actionType, sign, u_QQID, "用户");
            }
        }

        [WebMethod(Description = "查询注销用户帐户表")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet GetUserAccountCancel(string fuid, int fcurtype, int istr, int imax)
        {
            if (myHeader == null)
            {
                throw new Exception("不正确的调用方法！");
            }
            string strUserID = myHeader.UserName;
            string strPassword = myHeader.UserPassword;
            string strIP = myHeader.UserIP;
            string strRightCode = "GetUserAccount";

            int sign = 0;
            string detail, actionType;
            actionType = "查询用户帐户表";

            try
            {
                Q_USER cuser = new Q_USER(fuid, fcurtype);

                sign = 1;
                //furion 20090611 已改写，这个SQL从任何数据库连接都可执行。
                return cuser.GetResultX(istr, imax, "HT");
            }
            catch (Exception e)
            {
                sign = 0;
                return null;
            }
        }

        [WebMethod(Description = "查询用户帐户表核心库")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet GetUserAccountMain(string u_QQID, int istr, int imax)
        {
            try
            {
                string fuid = PublicRes.ConvertToFuid(u_QQID);
                Q_USER cuser = new Q_USER(fuid, 1);
                //furion 20090611 已改写，这个SQL从任何数据库连接都可执行。
                return cuser.GetResultX(istr, imax, "ZJB");
            }
            catch (Exception e)
            {
                return null;
            }
        }

        [WebMethod(Description = "查询用户商家工具按钮表")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet GetUserButtonInfo(string u_QQID, int istr, int imax)
        {
            if (myHeader == null)
            {
                throw new Exception("不正确的调用方法！");
            }
            string strUserID = myHeader.UserName;
            string strIP = myHeader.UserIP;
            string strRightCode = "GetUserAccount";

            int sign = 0;
            string detail, actionType;
            actionType = "查询用户商家工具按钮表";

            try
            {
                Q_BUTTONINFO cuser = new Q_BUTTONINFO(u_QQID, istr, imax);
                sign = 1;
                //furion 20090611 此表移入订单库了
                return cuser.GetResultX("ZJB");
            }
            catch (Exception e)
            {
                sign = 0;
                throw new Exception("用户商家工具按钮不存在或者未注册！(" + e.Message.ToString().Replace("'", "’") + ")");
                return null;
            }
            finally
            {
                PublicRes.writeSysLog(strUserID, strIP, "query", actionType, sign, u_QQID, "商家工具按钮");
            }
        }

        [WebMethod(Description = "查询所有商户帐户信息，edwardzheng 20051110")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public System.Data.DataSet GetUserMedInfoList()
        {
            RightAndLog rl = new RightAndLog();
            try
            {
                if (myHeader == null)
                {
                    throw new Exception("不正确的调用方法！");
                }

                rl.actionType = "查询商户帐户信息";
                rl.ID = "";
                rl.OperID = myHeader.OperID;
                rl.sign = 1;
                rl.strRightCode = "GetUserMedInfo";
                rl.RightString = myHeader.RightString;
                rl.SzKey = myHeader.SzKey;
                rl.type = "查询";
                rl.UserID = myHeader.UserName;
                rl.UserIP = myHeader.UserIP;
  	
                Q_USER_MED cuser = new Q_USER_MED();   //spid 中介账户

                //furion 20090611 此函数再无客户端调用。
                return cuser.GetResultX();
            }
            catch (LogicException err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                throw;
            }
            catch (Exception err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                throw new LogicException("Service处理失败！");
            }
            finally
            {
                rl.WriteLog();
            }
        }

        [WebMethod(Description = "查询商户帐户信息")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public T_USER_MED GetUserMedInfo(string SPID, int istr, int imax)
        {
            try
            {
                Q_USER_MED cuser = new Q_USER_MED(SPID);   //spid 中介账户
                return cuser.GetResult();
            }
            catch (Exception ex)
            {
                throw new Exception("Service处理失败！" + PublicRes.replaceMStr(ex.Message));
            }
        }

        [WebMethod(Description = "修改商户登录密码")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public bool ResetMediPasswd(string spid, out string Password)
        {
            try
            {
                T_USER_MED um = new T_USER_MED();
                return um.UpdatePasswd(spid, out Password);
            }
            catch (Exception ex)
            {
                throw new Exception("Service处理失败！" + PublicRes.replaceMStr(ex.Message));
            }
        }

        [WebMethod(Description = "查询用户资料表")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet GetUserInfo(string u_QQID, int istr, int imax)
        {
            if (myHeader == null)
            {
                throw new Exception("不正确的调用方法！");
            }
            string strUserID = myHeader.UserName;
            string strIP = myHeader.UserIP;
            string strRightCode = "GetUserInfo";

            int sign = 0; string detail, actionType, signStr;
            actionType = "查询用户资料表";
            try
            {
                // TODO: 1客户信息资料外移
                sign = 1;
                string fuid = PublicRes.ConvertToFuid(u_QQID);
                Q_USER_INFO cuser = new Q_USER_INFO(fuid);
         
                return cuser.GetResultX(istr, imax, "ZW");
            }
            catch (Exception e)
            {
                sign = 0;
                throw new Exception("该用户不存在！");
                return null;
            }
            finally
            {
                PublicRes.writeSysLog(strUserID, strIP, "query", actionType, sign, u_QQID, "用户");
            }
        }

        [WebMethod(Description = "查询用户绑定银行帐户表")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet GetUserBankAccount(string u_QQID, int istr, int imax)
        {
            if (myHeader == null)
            {
                throw new Exception("不正确的调用方法！");
            }
            string strUserID = myHeader.UserName;
            string strIP = myHeader.UserIP;
            string strRightCode = "GetUserBankAccount";
            int sign = 0;
            string detail, actionType, signStr;
            actionType = "查询用户绑定银行账户表";

            try
            {
                sign = 1;
                // TODO: 1客户信息资料外移
                Q_BANK_USER cuser = new Q_BANK_USER(u_QQID);
                return cuser.GetResultX_ICE();
            }
            catch (Exception e)
            {
                sign = 0;
                throw new Exception("用户绑定银行帐户不存在！");
                return null;
            }
            finally
            {
                PublicRes.writeSysLog(strUserID, strIP, "query", actionType, sign, u_QQID, "用户");
            }
        }

        [WebMethod(Description = "批量查询用户绑定银行帐户表")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet GetBatchUserBankAccount(string u_QQID)
        {
            try
            {
                // TODO: 1客户信息资料外移
                Q_BANK_USER cuser = new Q_BANK_USER(u_QQID, true);

                DataSet ds = cuser.GetResultX_ICE();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ds.Tables[0].Columns.Add("Ftruename", typeof(String));
                    ds.Tables[0].Columns.Add("Fcompany_name", typeof(String));
                    ds.Tables[0].Columns.Add("Fqqid", typeof(String));
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        dr.BeginEdit();
                        string errMsg = "";
                        string fuid = PublicRes.ConvertToFuid(u_QQID);
                        string fqqid = PublicRes.Uid2QQ(fuid);
                        string strSql = "uid=" + fuid;
                        string ftruename = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_USERINFO, "Ftruename", out errMsg);
                        string fcompany_name = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_USERINFO, "Fcompany_name", out errMsg);

                        dr["Ftruename"] = ftruename;
                        dr["Fcompany_name"] = fcompany_name;
                        dr["Fqqid"] = fqqid;

                        dr.EndEdit();
                    }
                }
                return ds;
            }
            catch (Exception e)
            {
                throw new Exception("此用户不存在");
                return null;
            }
        }


        [WebMethod(Description = "查询交易单表")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet GetPayList(string u_ID, int u_IDType, DateTime u_BeginTime, DateTime u_EndTime, int istr, int imax)
        {
            ICEAccess ice = new ICEAccess(PublicRes.ICEServerIP, PublicRes.ICEPort);
            try
            {
                //已修改 furion V30_FURION核心查询需改动 type=1和2时,由t_tran_list改为查询t_order
                //0买家交易单，9卖家交易单，1，通过交易单查询，2通过给银行订单号查询，4，通过订单查询
                Q_PAY_LIST cuser = new Q_PAY_LIST(u_ID, u_IDType, u_BeginTime, u_EndTime, istr, imax);
              
                if (cuser.ICESQL == "")
                {
                    if (u_IDType == 0 || u_IDType == 9 || u_IDType == 10 || u_IDType == 13)
                    {
                        //改成调用relay接口 v_yqyqguo 2015-5-9
                        return new TradeService().Q_PAY_LIST(u_ID, u_IDType, u_BeginTime, u_EndTime, istr, imax);
                    }
                    else
                        return cuser.GetResultX("BSB");
                    //现在要查核心交易单，而不是用订单替代。
                }
                else if (u_IDType == 1)
                {
                    ice.OpenConn();
                    string strwhere = "where=" + ICEAccess.URLEncode("flistid=" + u_ID.Trim() + "&");
                    strwhere += ICEAccess.URLEncode("fcurtype=1&");

                    string strResp = "";

                    //3.0接口测试需要 furion 20090708
                    DataTable dt = ice.InvokeQuery_GetDataTable(YWSourceType.交易单资源, YWCommandCode.查询交易单信息, u_ID.Trim(), strwhere, out strResp);

                    ice.CloseConn();

                    if (dt == null || dt.Rows.Count == 0)
                    {
                        return null;
                    }
                    DataSet ds = new DataSet();
                    ds.Tables.Add(dt);
                    return ds;
                }
                else
                {
                    string errMsg = "";
                    DataSet ds = CommQuery.GetDataSetFromICE(cuser.ICESQL, CommQuery.QUERY_ORDER, out errMsg);

                    return ds;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                ice.Dispose();
            }

        }

        [WebMethod(Description = "满减使用查询")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet GetManJianUsingList(string u_ID, int u_IDType, DateTime u_BeginTime, DateTime u_EndTime, string banktype, int istr, int imax)
        {
            try
            {
                string fuid = PublicRes.ConvertToFuid(u_ID);
                string tPayList = PublicRes.GetTName("t_user_order", fuid);             //交易单的表

                string sWhereStr1 = " where fbuy_uid='" + fuid + "' and fcurtype=1 and fstandby6=1 and fcreate_time between '"
                       + u_BeginTime.ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + u_EndTime.ToString("yyyy-MM-dd HH:mm:ss") + "' ";
                if (banktype != null && banktype != "")
                {
                    sWhereStr1 += "and Fbuy_bank_type=" + banktype;
                }

                int iCount = 10000;
                string fstrSql = "Select *," + iCount + " as total,Ftrade_state as Fstate from " + tPayList + sWhereStr1
                    //+ "UNION Select *," + iCount + " as total,Ftrade_state as Fstate from " + tPayList + sWhereStr2  
                       + " ORDER By Fcreate_time DESC limit " + (istr - 1) + "," + imax;
                string connstr = PublicRes.GetConnString("t_user_order_bsb", fuid.Substring(fuid.Length - 2));
                return PublicRes.returnDSAll_Conn(fstrSql, connstr);
            }
            catch (Exception e)
            {
                return null;
            }
        }



        /**
         * 接口有两种查询方式：
         * 1. req_type=27,cre_id, bank_type, uin, mobile, activity_no=1
         * 2. 一点通：req_type=28，uin, cre_id, bank_type,activity_no=1(目前接口系统线上线下配置不一致，都使用activity_no=1)
         * mobile通过快捷支付记录来查询
         * bank_type给定列表，包括一点通与非一点通，查询所有卡类型的满减
         * 页面显示每条返回结果全!(is_exist_0=0)的记录
         **/
        [WebMethod(Description = "满减用户查询")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet ManJianUserList(string u_QQID)
        {        
            #region
            string fuid = "";
            try
            {
                fuid = PublicRes.ConvertToFuid(u_QQID);
                //       fuid = "298849000";
                if (fuid == null || fuid.Trim() == "")
                {
                    throw new Exception("输入的QQ号码有误！");
                }
                string msg = "";
                try
                {
                    string service_name = "exau_limitquery_service";//接口名
                    string req = "";//参数组合
                    string cre_id = "";//证件号
                    List<string> mobileList = new List<string>();//手机列表

                    DataSet dsAll = QueryBankCardForManJian(fuid, 0);//查询全部，为了拿到身份证
                    if (dsAll != null & dsAll.Tables.Count > 0 & dsAll.Tables[0].Rows.Count > 0)
                    {
                        DataTable dt = dsAll.Tables[0];
                        foreach (DataRow dr in dt.Rows)
                        {
                            cre_id = dr["Fcre_id"].ToString();
                            if (cre_id != null && cre_id != "") break;
                        }
                    }
                    else
                    {
                        return null;//查询绑定银行卡记录为空:查这个表为了查到身份证及快捷电话号码
                    }

                    DataSet dsM = QueryBankCardForManJian(fuid, 1);//查询快捷支付，为了拿到手机号码
                    if (dsM != null & dsM.Tables.Count > 0 & dsM.Tables[0].Rows.Count > 0)
                    {
                        DataTable dt = dsM.Tables[0];
                        foreach (DataRow dr in dt.Rows)
                        {
                            string temp = dr["Fmobilephone"].ToString();
                            if (temp != null && temp != "" && !(mobileList.Contains(temp)))
                            {
                                mobileList.Add(temp);
                            }
                        }
                    }

                    Hashtable hashYDT = new Hashtable();//一点通银行卡
                    Hashtable hashKJ = new Hashtable();//非一点通卡
                    hashYDT.Add("2001", "招行一点通");
                    hashYDT.Add("2002", "工行一点通");
                    hashKJ.Add("3001", "兴业信用卡");
                    hashKJ.Add("3002", "中行信用卡");

                    DataSet dsResult = new DataSet();//存放查询接口的数据结果
                    DataTable dtResult = new DataTable();
                    dsResult.Tables.Add(dtResult);
                    //  DataRow drfield = dtResult.NewRow();
                    dtResult.Columns.Add("QQ");
                    dtResult.Columns.Add("cre_id");
                    dtResult.Columns.Add("bank_type");
                    dtResult.Columns.Add("bank_typeName");
                    dtResult.Columns.Add("mobile");
                    dtResult.Columns.Add("cre_idbank_exist");
                    dtResult.Columns.Add("cre_idbank_exist_limit");//身份证、银行类型维度
                    dtResult.Columns.Add("uinbank_exist");
                    dtResult.Columns.Add("uinbank_exist_limit");//QQ、银行类型维度
                    dtResult.Columns.Add("mobilebank_exist");
                    dtResult.Columns.Add("mobilebank_limit");//手机号、银行类型维度

                    //查询非一点通卡满减
                    if (mobileList.Count == 0 || mobileList == null)
                    {
                        mobileList.Add("1");//加一个手机号码1，以查询接口，确保不会漏掉记录
                    }
                    foreach (string mobile in mobileList)
                    {
                        foreach (string str in hashKJ.Keys)
                        {
                            req = "channel_id=1&direct=1&req_type=27&activity_no=1&bank_card_id=1&uin=" + u_QQID + "&cre_id=" + cre_id + "&bank_type=" + str + "&mobile=" + mobile;
                            DataSet dsKJ = CommQuery.GetOneTableFromICE(req, "", service_name, false, out msg);
                            if (dsKJ != null & dsKJ.Tables.Count > 0 & dsKJ.Tables[0].Rows.Count > 0)
                            {
                                DataTable dt = dsKJ.Tables[0];
                                foreach (DataRow dr in dt.Rows)
                                {
                                    DataRow drfield = dtResult.NewRow();
                                    drfield["QQ"] = u_QQID;
                                    drfield["cre_id"] = cre_id;
                                    drfield["bank_type"] = str;
                                    drfield["bank_typeName"] = hashKJ[str];
                                    drfield["mobile"] = mobile;
                                    drfield["cre_idbank_exist"] = dr["is_exist_0"];
                                    drfield["cre_idbank_exist_limit"] = dr["limit_type_6_0"];
                                    drfield["uinbank_exist"] = dr["is_exist_1"];
                                    drfield["uinbank_exist_limit"] = dr["limit_type_6_1"];
                                    drfield["mobilebank_exist"] = dr["is_exist_3"];
                                    drfield["mobilebank_limit"] = dr["limit_type_6_3"];
                                    dtResult.Rows.Add(drfield);//结果表增加一条记录
                                }
                            }
                        }
                    }

                    //查询一点通卡满减
                    foreach (string str in hashYDT.Keys)
                    {
                        req = "channel_id=1&direct=1&req_type=28&activity_no=1&uin=" + u_QQID + "&cre_id=" + cre_id + "&bank_type=" + str;
                        DataSet dsYDT = CommQuery.GetOneTableFromICE(req, "", service_name, false, out msg);
                        if (dsYDT != null & dsYDT.Tables.Count > 0 & dsYDT.Tables[0].Rows.Count > 0)
                        {
                            DataTable dt = dsYDT.Tables[0];
                            foreach (DataRow dr in dt.Rows)
                            {
                                DataRow drfield = dtResult.NewRow();
                                drfield["QQ"] = u_QQID;
                                drfield["cre_id"] = cre_id;
                                drfield["bank_type"] = str;
                                drfield["bank_typeName"] = hashYDT[str];
                                drfield["cre_idbank_exist"] = dr["is_exist_0"];
                                drfield["cre_idbank_exist_limit"] = dr["limit_type_6_0"];
                                drfield["uinbank_exist"] = dr["is_exist_1"];
                                drfield["uinbank_exist_limit"] = dr["limit_type_6_1"];
                                dtResult.Rows.Add(drfield);//结果表增加一条记录
                            }
                        }
                    }   
                    return dsResult;
                }
                catch (Exception err)
                {
                    throw new Exception("Service处理失败！" + msg);
                }
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
            #endregion
        }

        [WebMethod(Description = "满减增加一次")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet ManJianAddOne(string uin, string cre_id, string bank_type, string mobile)
        {
            string msg = "";
            string service_name = "exau_limitminus_service";//接口名
            string req = "";
            try
            {
                //测试数据
                //  req = "channel_id=1&direct=1&req_type=28&activity_no=1&uin=2322405969&cre_id=500234197805037308&bank_type=2001&amount=1";
                //  req = "channel_id=1&direct=1&req_type=27&uin=11470931&cre_id=350583198002060053&bank_type=2002&activity_no=1&mobile=18682336225&bank_card_id=1&amount=1";

                DataSet ds = null;
                if (mobile == "&nbsp;" || mobile == "" || mobile == null)
                {
                    req = "channel_id=1&direct=1&amount=1&req_type=28&activity_no=1&uin=" + uin + "&cre_id=" + cre_id + "&bank_type=" + bank_type;
                }
                else
                {
                    req = "channel_id=1&direct=1&amount=1&req_type=27&activity_no=1&bank_card_id=1&uin=" + uin + "&cre_id=" + cre_id + "&bank_type=" + bank_type + "&mobile=" + mobile;
                }
                ds = CommQuery.GetOneTableFromICE(req, "", service_name, true, out msg);
                DataTable dtt = ds.Tables[0];
                foreach (DataRow dr in dtt.Rows)
                {
                    string res_info = dr["res_info"].ToString();
                }
                return ds;
            }
            catch (Exception e)
            {
                throw new Exception("Service处理失败！" + msg);
            }
        }
        private static DataSet QueryBankCardForManJian(string fuid, int queryType)
        {
            MySqlAccess da = null;
            try
            {
                string filter = "(1=1)";
                filter += " and fuid=" + fuid;
                DataSet ds_findUID = null;
                if (queryType == 1)
                {
                    //// 一点通
                    //filter += " and ( (Fbind_type >=1 and Fbind_type<=9) or (Fbind_type >=20 and Fbind_type<=29) or (Fbind_type >=100 and Fbind_type<=119)) ";
                    // 快捷支付
                    filter += " and Fbind_type >=10 and Fbind_type<=19 ";
                }
                da = new MySqlAccess(PublicRes.GetConnString("BD"));
                da.OpenConn();
                string Sql = "select Findex,Fbind_serialno,Fprotocol_no,Fuin,Fuid,Fbank_type,Fbind_flag,Fbind_type,Fbind_status,Fbank_status,right(Fcard_tail,4) as Fcard_tail," +
                    "Fbank_id,Ftruename,Funchain_time_local,Fmodify_time,Fmemo,Fcre_id,Ftelephone,Fmobilephone from " + PublicRes.GetTName("t_user_bind", fuid) + " where " + filter;
                return da.dsGetTotalData(Sql);
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
            finally
            {
                if (da != null)
                    da.Dispose();
            }
        }

        [WebMethod(Description = "查询交易单表")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet GetPayListForChildren(string u_ID, int curType, int u_IDType, DateTime u_BeginTime, DateTime u_EndTime, int istr, int imax)
        {
            ICEAccess ice = new ICEAccess(PublicRes.ICEServerIP3, PublicRes.ICEPort3);
            string strResp = "";
            string errMsg = "";
            try
            {
                Q_PAY_LIST cuser = new Q_PAY_LIST(u_ID, u_IDType, u_BeginTime, u_EndTime, istr, imax);
      
                if (cuser.ICESQL == "")
                {
                    if (u_IDType == 0 || u_IDType == 9 || u_IDType == 10)
                    {
                        //改成调用relay接口 v_yqyqguo 2015-5-9
                        return new TradeService().Q_PAY_LIST(u_ID, u_IDType, u_BeginTime, u_EndTime, istr, imax);
                    }
                    else
                        return cuser.GetResultX("BSB");
                }
                else if (u_IDType == 1)
                {
                    ice.OpenConn();
                    string strwhere = "where=" + ICEAccess.URLEncode("flistid=" + u_ID.Trim() + "&");
                    strwhere += ICEAccess.URLEncode("fcurtype=" + curType + "&");

                    DataTable dt = ice.InvokeQuery_GetDataTable(YWSourceType.交易单资源, YWCommandCode.查询交易单信息, u_ID.Trim(), strwhere, out strResp);

                    ice.CloseConn();

                    if (dt == null || dt.Rows.Count == 0)
                    {
                        throw new Exception("没有记录！strResp:" + strResp);
                    }

                    DataSet ds = new DataSet();
                    ds.Tables.Add(dt);
                    return ds;
                }
                else
                {
                    DataSet ds = CommQuery.GetDataSetFromICE(cuser.ICESQL, CommQuery.QUERY_ORDER, out errMsg);

                    return ds;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "-strResp:" + strResp + "-errMsg:" + errMsg);
            }
            finally
            {
                ice.Dispose();
            }

        }

        [WebMethod(Description = "查询交易单ID")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet GetListidFromUserOrder(string qqid, string uid, int start, int max)
        {
            MySqlAccess da = null;
            try
            {
                string fuid = "";
                if (!string.IsNullOrEmpty(qqid.Trim()))
                    fuid = PublicRes.ConvertToFuid(qqid);
                if (!string.IsNullOrEmpty(uid.Trim()))
                    fuid = uid;
                if (fuid == null || fuid.Length < 3)
                {
                    throw new Exception(fuid + "账号不存在");
                }

                // 测试
                //     fuid = "01212004";

                string connstr = PublicRes.GetConnString("t_user_order_bsb", fuid.Substring(fuid.Length - 2));
                string tPayList = PublicRes.GetTName("t_user_order", fuid); //交易单的表

                string fstrSql = " Select Flistid, Fcreate_time from " + tPayList
                    + " WHERE (Fbuy_uid='" + fuid + "' or Fsale_uid='" + fuid + "') "
                    + "  AND Fcurtype=1 AND ((Ftrade_type=4 and Fmedi_sign<>1 AND Ftrade_state=8) or "
                    + " (Ftrade_type=1 AND Ftrade_state=2) or"
                    + " (Ftrade_type=4 AND (Ftrade_state=2 OR Ftrade_state=3) AND Fmedi_sign=1)) "
                    + " ORDER By Fcreate_time DESC limit " + start + "," + max;

                da = new MySqlAccess(connstr);
                da.OpenConn();
                return da.dsGetTotalData(fstrSql);
            }
            catch (Exception err)
            {
                throw new Exception("查询交易单ID异常：" + err.Message);
            }
            finally
            {
                if (da != null)
                    da.Dispose();
            }
        }

        //furion 20050811 为了查询二期充值信息和二期提现信息而设的查询。
        [WebMethod(Description = "查询交易信息")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet GetPayList_List(string u_ID)
        {
            RightAndLog rl = new RightAndLog();
            try
            {
                if (myHeader == null)
                {
                    throw new Exception("不正确的调用方法！");
                }

                rl.actionType = "查询交易信息";
                rl.ID = u_ID;
                rl.sign = 1;
                rl.strRightCode = "GetPayList";
                rl.type = "查询";

                PublicRes.SetRightAndLog(myHeader, rl);
                if (!rl.CheckRight())
                {
                    throw new LogicException("用户无权执行此操作！");
                }
                Q_PAY_LIST cuser = new Q_PAY_LIST(u_ID);

                string errMsg = "";
                DataSet ds = CommQuery.GetDataSetFromICE(cuser.ICESQL, CommQuery.QUERY_ORDER, out errMsg);

                return ds;
            }
            catch (LogicException err)
            {
                rl.sign = 0;
                throw;
            }
            catch (Exception err)
            {
                rl.sign = 0;
                throw new LogicException("Service处理失败！");
            }
            finally
            {
                rl.WriteLog();
            }

        }

        [WebMethod(Description = "查询用户帐户流水表")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet GetBankRollList(string u_QQID, int fcurtype, DateTime u_BeginTime, DateTime u_EndTime, int istr, int imax)
        {
            if (myHeader == null)
            {
                throw new Exception("不正确的调用方法！");
            }
            string strUserID = myHeader.UserName;
            string strIP = myHeader.UserIP;
            string strRightCode = "GetBankRollList";

            int sign = 0; string detail, actionType, signStr;
            actionType = "查询用户帐户流水";

            try
            {
                //已改动 furion V30_FURION核心查询需改动 资金流水查询待通用接口  //已处理 V30_20090525 资金流水用QQID查询
                //idtype 0买家，1卖家
                sign = 1;
                string fuid = PublicRes.ConvertToFuid(u_QQID);
                DataSet ds;

                int start = istr - 1;
                if (start < 0) start = 0;

                string strWhere = "start_time=" + ICEAccess.ICEEncode(u_BeginTime.ToString("yyyy-MM-dd HH:mm:ss"));
                strWhere += "&end_time=" + ICEAccess.ICEEncode(u_EndTime.ToString("yyyy-MM-dd HH:mm:ss"));
                strWhere += "&uid=" + fuid;
                strWhere += "&curtype=" + fcurtype;
                strWhere += "&lim_start=" + start;
                strWhere += "&lim_count=" + imax;

                string errMsg;

                if (!CommQuery.GetDataFromICE(strWhere, CommQuery.个人资金流水, out errMsg, out ds))
                {
                    throw new LogicException(errMsg);
                }

                if (ds == null)
                    return null;

                return ds;
            }
            catch (Exception e)
            {
                sign = 0;
                throw;
                return null;
            }
            finally
            {
                PublicRes.writeSysLog(strUserID, strIP, "query", actionType, sign, u_QQID, "用户");
            }
        }

        [WebMethod(Description = "查询用户帐户流水表_WithListID")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet GetBankRollList_withID(DateTime u_BeginTime, DateTime u_EndTime, string ListID, int istr, int imax)
        {
            if (myHeader == null)
            {
                throw new Exception("不正确的调用方法！");
            }
            string strUserID = myHeader.UserName;
            string strIP = myHeader.UserIP;
            string strRightCode = "GetBankRollList";

            int sign = 0; string detail, actionType, signStr;
            actionType = "查询用户帐户流水";

            try
            {
                //furion V30_FURION核心查询需改动 等待通用查询接口.  //V30_20090525 资金流水用listid查询 这个要增加多个查询，然后组合结果
                sign = 1;
                Q_BANKROLL_LIST cuser = new Q_BANKROLL_LIST(u_BeginTime, u_EndTime, ListID);
  
                if (cuser.alTables == null || cuser.alTables.Count == 0)
                    return null;

                string onerow = cuser.alTables[0].ToString().Replace("UID=", "");
                string strWhere = "uid=" + onerow;
                strWhere += "&listid=" + ListID;

                DataSet ds;
                string errMsg;

                if (!CommQuery.GetDataFromICE(strWhere, CommQuery.交易单资金流水_个人, out errMsg, out ds))
                {
                    throw new LogicException(errMsg);
                }

                bool havefirstds = true;
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0] == null || ds.Tables[0].Columns.Count == 0)
                {
                    //throw new LogicException("查询买卖家资金流水时有误");
                    //应该是未支付,啥资金流水都没发生.
                    //return null;
                    havefirstds = false;
                }

                //先取出买卖家的一个数据表
                for (int i = 1; i < cuser.alTables.Count; i++)
                {
                    DataSet newds;
                    onerow = cuser.alTables[i].ToString();
                    if (onerow.StartsWith("UID="))
                    {
                        strWhere = "uid=" + onerow.Replace("UID=", "");
                        strWhere += "&listid=" + ListID;

                        if (!CommQuery.GetDataFromICE(strWhere, CommQuery.交易单资金流水_个人, out errMsg, out newds))
                        {
                            throw new LogicException(errMsg);
                        }
                    }
                    else
                    {
                        strWhere = "start_time=" + onerow.Replace("TME=", "").Substring(0, 10);
                        strWhere += "&listid=" + ListID;

                        if (!CommQuery.GetDataFromICE(strWhere, CommQuery.交易单资金流水_商户, out errMsg, out newds))
                        {
                            throw new LogicException(errMsg);
                        }
                    }

                    //把这次得到的表内容合并入ds中。
                    if (newds == null || newds.Tables.Count == 0 || newds.Tables[0] == null || newds.Tables[0].Rows.Count == 0)
                    {
                        continue;
                    }

                    if (havefirstds)
                    {
                        foreach (DataRow dr in newds.Tables[0].Rows)
                        {
                            ds.Tables[0].Rows.Add(dr.ItemArray);
                        }
                    }
                    else
                    {
                        havefirstds = true;
                        ds = newds;
                    }
                }

                //furion 增加银行资金流水表。 20090813
                for (int i = 1; i < cuser.alTables.Count; i++)
                {
                    DataSet newds;
                    onerow = cuser.alTables[i].ToString();
                    if (onerow.StartsWith("UID="))
                    {
                        continue;
                    }
                    else
                    {
                        strWhere = "start_time=" + onerow.Replace("TME=", "").Substring(0, 10);
                        strWhere += "&listid=" + ListID;

                        if (!CommQuery.GetDataFromICE(strWhere, CommQuery.交易单资金流水_银行, out errMsg, out newds))
                        {
                            throw new LogicException(errMsg);
                        }
                    }

                    //把这次得到的表内容合并入ds中。
                    if (newds == null || newds.Tables.Count == 0 || newds.Tables[0] == null || newds.Tables[0].Rows.Count == 0)
                    {
                        continue;
                    }

                    if (havefirstds)
                    {
                        foreach (DataRow dr in newds.Tables[0].Rows)
                        {
                            ds.Tables[0].Rows.Add(dr.ItemArray);
                        }
                    }
                    else
                    {
                        havefirstds = true;
                        ds = newds;
                    }
                }

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0] == null || ds.Tables[0].Columns.Count == 0)
                {
                    return null;
                }

                ds.Tables[0].DefaultView.Sort = "Faction_Type DESC";
                return ds;
            }
            catch (Exception e)
            {
                sign = 0;
                throw new Exception("service发生错误,请联系管理员！" + e.Message);
                return null;
            }
            finally
            {
                PublicRes.writeSysLog(strUserID, strIP, "query", actionType, sign, ListID, "交易单");
            }
        }


        [WebMethod(Description = "查询用户交易流水表")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet GetUserPayList(string u_ID, int u_IDType, DateTime u_BeginTime, DateTime u_EndTime, int istr, int imax)
        {
            if (myHeader == null)
            {
                throw new Exception("不正确的调用方法！");
            }
            string strUserID = myHeader.UserName;
            string strIP = myHeader.UserIP;
            string strRightCode = "GetUserPayList";
            int sign = 0; string detail, actionType, signStr;
            actionType = "查询用户交易流水";

            try
            {
                string fuid = PublicRes.ConvertToFuid(u_ID);
                string strSql = "uid=" + fuid;
                strSql += "&starttime=" + u_BeginTime.ToString("yyyy-MM-dd HH:mm:ss");
                strSql += "&endtime=" + u_EndTime.ToString("yyyy-MM-dd HH:mm:ss");
                strSql += "&limitstart=" + istr;
                strSql += "&limitend=" + imax;

                string errMsg = "";
                DataSet ds = CommQuery.GetDataSetFromICE(strSql, CommQuery.QUERY_USERPAY_U, out errMsg);

                return ds;
            }
            catch (Exception e)
            {
                sign = 0;
                throw new Exception("service发生错误,请联系管理员！");
                return null;
            }
            finally
            {
                PublicRes.writeSysLog(strUserID, strIP, "query", actionType, sign, u_ID, "用户");
            }
        }

        [WebMethod(Description = "查询用户交易流水表_WithListID")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet GetUserPayList_withID(string u_ID, DateTime u_BeginTime, DateTime u_EndTime, string slistID, int istr, int imax)
        {
            if (myHeader == null)
            {
                throw new Exception("不正确的调用方法！");
            }
            string strUserID = myHeader.UserName;
            string strPassword = "";
            string strIP = myHeader.UserIP;
            string strRightCode = "GetUserPayList";

            int sign = 0;
            string detail, actionType, signStr;
            actionType = "查询用户交易流水";

            try
            {
                string errMsg = "";

                string strSql = "listid=" + slistID;
                DataSet dsbuy = CommQuery.GetDataSetFromICE(strSql, CommQuery.QUERY_USERPAY_L, out errMsg);

                return dsbuy;
            }
            catch (Exception e)
            {
                sign = 0;
                throw new Exception("service发生错误,请联系管理员！");
                return null;
            }
            finally
            {
                PublicRes.writeSysLog(strUserID, strIP, "query", actionType, sign, slistID, "交易单");
            }
        }


        [WebMethod(Description = "查询腾讯银行帐户表")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet GetTCBankAccount(string u_ID, int u_IDType, DateTime u_BeginTime, DateTime u_EndTime, int istr, int imax)
        {
            if (myHeader == null)
            {
                throw new Exception("不正确的调用方法！");
            }
            string strUserID = myHeader.UserName;
            string strIP = myHeader.UserIP;
            string strRightCode = "GetTCBankAccount";

            int sign = 0; string detail, actionType, signStr;
            actionType = "查询腾讯银行帐户";

            try
            {
                sign = 1;
                Q_PAY_LIST cuser = new Q_PAY_LIST(u_ID, u_IDType, u_BeginTime, u_EndTime, istr, imax);
              
                if (cuser.ICESQL == "")
                {
                    if (u_IDType == 0 || u_IDType == 9 || u_IDType == 10)
                    {
                        //改成调用relay接口 v_yqyqguo 2015-5-9
                        return new TradeService().Q_PAY_LIST(u_ID, u_IDType, u_BeginTime, u_EndTime, istr, imax);
                    }
                    else
                        return cuser.GetResultX(istr, imax, "BSB");
                    //现在要查核心交易单，而不是用订单替代。
                }
                else
                {
                    string errMsg = "";
                    DataSet ds = CommQuery.GetDataSetFromICE(cuser.ICESQL, CommQuery.QUERY_ORDER, out errMsg);
                    return ds;
                }
            }
            catch (Exception e)
            {
                sign = 0;
                throw new Exception("service发生错误,请联系管理员！");
                return null;
            }
            finally
            {
                PublicRes.writeSysLog(strUserID, strIP, "query", actionType, sign, u_ID, "交易单");
            }
        }


        [WebMethod(Description = "查询腾讯收款记录表")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet GetTCBankRollList(string u_ID, int u_IDType, DateTime u_BeginTime, DateTime u_EndTime, bool isHistory, int istr, int imax)
        {
            if (myHeader == null)
            {
                throw new Exception("不正确的调用方法！");
            }
            string strUserID = myHeader.UserName;
            string strIP = myHeader.UserIP;
            string strRightCode = "GetTCBankRollList";

            int sign = 0;
            string detail, actionType, signStr;
            actionType = "查询腾讯收款记录";

            try
            {
                DataSet ds = null;

                float fnum = float.Parse("0");
                float fnummax = float.Parse("20000000.00");

                if (u_IDType == 0)
                {
                    ds = GetBankRollListByListId(u_ID, "qq", 1, u_BeginTime, u_EndTime, 0, fnum, fnummax, "0000", "0", istr, imax);
                }
                else
                {
                    ds = GetBankRollListByListId(u_ID, "czd", 1, u_BeginTime, u_EndTime, 0, fnum, fnummax, "0000", "0", istr, imax);
                }

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ds.Tables[0].Columns.Add("total");
                    ds.Tables[0].Rows[0]["total"] = ds.Tables[0].Rows.Count;
                }

                return ds;
            }
            catch (Exception e)
            {
                sign = 0;
                throw new Exception("service发生错误,请联系管理员！");
                return null;
            }
            finally
            {
                PublicRes.writeSysLog(strUserID, strIP, "query", actionType, sign, u_ID, "用户");
            }

        }

        [WebMethod(Description = "按充值单号查询")]//新充值单查询
        public DataSet GetBankRollListByListId(string u_ID, string u_QueryType, int fcurtype, DateTime u_BeginTime, DateTime u_EndTime, int fstate,
            float fnum, float fnumMax, string banktype, string sorttype, int iPageStart, int iPageMax)
        {
            string msg = "";
            try
            {
                DataSet ds = null;
                DataSet dsqq = new DataSet();
                DataTable dsdt = new DataTable();
                string strSql = "";

                if (!string.IsNullOrEmpty(u_ID))
                {
                    switch (u_QueryType.ToLower())
                    {
                        case "qq":         //按照QQ号查询，注意使用内部uid
                            string uid = PublicRes.ConvertToFuid(u_ID);
                            strSql = "auid=" + uid;
                            break;
                        case "tobank":     //给银行的订单号
                            strSql = "bank_list=" + u_ID.Trim();
                            break;
                        case "bankback":   //银行返回u
                            strSql = "bank_acc=" + u_ID.Trim();
                            break;
                        case "czd":        //充值单查询
                            strSql = "listid=" + u_ID.Trim();
                            break;
                    }
                }
                
                if (fstate != 0)
                {
                    strSql += "&sign=" + fstate;
                }
                long num = (long)Math.Round(fnum * 100, 0);
                long numMax = (long)Math.Round(fnumMax * 100, 0);


                strSql += "&num_start=" + num + "&num_end=" + numMax;

                if (banktype != "0000" && banktype != "")
                {
                    strSql += "&bank_type=" + banktype;
                }

                if (u_QueryType.ToLower() != "tobank")
                {
                    TimeSpan ts = u_EndTime - u_BeginTime;
                    int sub = ts.Days;
                    bool iscone = false;
                    for (int i = 0; i <= sub; i++)
                    {
                        string truetime = u_EndTime.AddDays(-i).ToString("yyyyMMdd");
                        string querstr = strSql + "&query_day=" + truetime + "&curtype=" + fcurtype + "&strlimit=limit " + iPageStart + "," + iPageMax;
                        ds = CommQuery.GetDataSetFromICE(querstr, CommQuery.QUERY_TCBANKROLL_DAY, out msg);
                        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            if (u_QueryType.ToLower() == "qq" || u_QueryType.ToLower() == "czd")
                            {
                                if (!iscone)
                                {
                                    dsdt = ds.Tables[0].Clone();
                                    iscone = true;
                                }

                                foreach (DataRow dr in ds.Tables[0].Rows)
                                {
                                    dsdt.ImportRow(dr);
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
                else   //按给银行订单号查询
                {
                    strSql += string.Format("&offset=0&limit=10&sp_id=1000000000&MSG_NO=&fronttime_start={0}&fronttime_end={1}", 
                        u_BeginTime.ToString("yyyy-MM-dd HH:mm:ss"), u_EndTime.ToString("yyyy-MM-dd HH:mm:ss"));
                    ds = CommQuery.GetDataSetFromICE_QueryServer(strSql, CommQuery.QUERY_TCBANKROLL_S, out msg);
                }

                if (u_QueryType.ToLower() == "qq" || u_QueryType.ToLower() == "czd")
                {
                    dsqq.Tables.Add(dsdt);
                    return dsqq;
                }

                return ds;
            }
            catch (Exception e)
            {
                throw new Exception("service发生错误,请联系管理员！" + e.Message + msg);
                return null;
            }
        }

        [WebMethod(Description = "充值单按给银行订单号分库分表查询详细函数")]//add rowenwu 20120301
        public DataSet GetFundListDetail_New(string listid, string fbank_list, string fbank_type, DateTime u_BeginTime, DateTime u_EndTime, out string mesgg)
        {
            mesgg = "";
            try
            {
                string strSql = string.Format("listid={0}&bank_list={1}&bank_type={2}&offset=0&limit=1&sp_id=1000000000&MSG_NO=&fronttime_start={3}&fronttime_end={4}",
                    listid, fbank_list, fbank_type, u_BeginTime.ToString("yyyy-MM-dd HH:mm:ss"), u_EndTime.ToString("yyyy-MM-dd HH:mm:ss"));

                DataSet ds = PublicRes.QueyNewCZDataDataSet(strSql, out mesgg);
                return ds;
            }
            catch (Exception e)
            {
                throw new Exception("service发生错误,请联系管理员！" + e.Message);
                return null;
            }
        }

        [WebMethod(Description = "充值单拆分日期比较")]
        public bool IsNewOrderCZData(DateTime payFrontTime)
        {
            return PublicRes.IsNewOrderCZData(payFrontTime);
        }
   
        [WebMethod(Description = "查询退款单表")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet GetRefund(string u_ID, int u_IDType, DateTime u_BeginTime, DateTime u_EndTime, int istr, int imax)
        {
            if (myHeader == null)
            {
                throw new Exception("不正确的调用方法！");
            }
            string strUserID = myHeader.UserName;
            string strIP = myHeader.UserIP;
            string strRightCode = "GetRefund";

            int sign = 0;
            string detail, actionType, signStr;
            actionType = "查询退款单";

            try
            {
                sign = 1;
                Q_REFUND cuser = new Q_REFUND(u_ID, u_IDType, u_BeginTime, u_EndTime);
                return cuser.GetResultX(istr, imax);
            }
            catch (Exception e)
            {
                sign = 0;
                throw new Exception("service发生错误,请联系管理员！");
                return null;
            }
            finally
            {
                PublicRes.writeSysLog(strUserID, strIP, "query", actionType, sign, u_ID, "用户");
            }
        }

        //该函数未被调用
        [WebMethod(Description = "买/卖家查询交易单表")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet GetPayListByType(string u_BuyID, DateTime u_BeginTime, DateTime u_EndTime, int U_Type, int istr, int imax)
        {
            if (myHeader == null)
            {
                throw new Exception("不正确的调用方法！");
            }
            string strUserID = myHeader.UserName;
            string strIP = myHeader.UserIP;
            string strRightCode = "GetPayListByType";
            int sign = 0;
            string detail, actionType, signStr;
            actionType = "查询交易单";

            try
            {
                sign = 1;
                Q_PAY_LIST_BYTYPE cuser = new Q_PAY_LIST_BYTYPE(u_BuyID, u_BeginTime, u_EndTime, U_Type);   
                string connstr = PublicRes.GetConnString("t_user_order_bs", u_BuyID.Substring(u_BuyID.Length - 2));
                return cuser.GetResultX_Conn(istr, imax, connstr);
            }
            catch (Exception e)
            {
                sign = 0;
                throw new Exception("service发生错误,请联系管理员！");
                return null;
            }
            finally
            {
                PublicRes.writeSysLog(strUserID, strIP, "query", actionType, sign, u_BuyID, "用户");
            }
        }

        [WebMethod(Description = "根据银行订单号和日期返回银行交易单号")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public string returnListID(string sFbankAcc, string sDateTime)  //时间格式： 2005-07-02
        {
            try
            {
                DateTime dt_checkTime = PublicRes.ConvertToDateTime(sDateTime);
                string errMsg = "";        
                string strCmd = "bank_acc=" + sFbankAcc + "&query_day=" + dt_checkTime.ToString("yyyyMMdd");  //增加时间参数 andrew 20110322;
                string listID = CommQuery.GetOneResultFromICE(strCmd, CommQuery.QUERY_TCBANKROLL_DAY, "Flistid", out errMsg);
                return listID;
            }
            catch
            {
                throw new Exception("根据银行订单号和日期查询交易单失败！");
            }
        }

        [WebMethod(Description = "检验用户登录")]
        public TCreateSessionReply ValidUser(string LoginUserID, string strPassword, string userIP)
        {
            RightAndLog rl = new RightAndLog();
            try
            {
                rl.actionType = "用户登录";
                rl.ID = strPassword;
                rl.OperID = 0;
                rl.sign = 1;
                rl.strRightCode = "ValidUser";
                rl.RightString = "";
                rl.SzKey = "";
                rl.type = "登录";
                rl.UserID = LoginUserID;
                rl.UserIP = userIP;
                rl.detail = "用户" + LoginUserID + "在" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                    + "时在IP为" + userIP + "的机器进行了尝试登录成功！";

                return UserRight.ValidateUser(LoginUserID, strPassword, PublicRes.f_strServerIP, PublicRes.f_iServerPort);
            }
            catch (LogicException err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);

                rl.detail = "用户" + LoginUserID + "在" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                    + "时在IP为" + userIP + "的机器进行了尝试登录失败！";

                throw;
            }
            catch (Exception err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                rl.detail = "用户" + LoginUserID + "在" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                    + "时在IP为" + userIP + "的机器进行了尝试登录失败！";

                throw new LogicException("Service处理失败！");
            }
            finally
            {
                rl.WriteLog();
            }
        }

        /// <summary>
        /// Ray 20051104新增加按照银行订单号和银行返回的订单号查询 增加参数：u_QueryType
        /// </summary>
        [WebMethod(Description = "充值查询函数")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet GetFundList(string u_ID, string u_QueryType, int fcurtype, DateTime u_BeginTime, DateTime u_EndTime, int fstate,
            float fnum, float fnumMax, string banktype, string sorttype, bool isHistory, int iPageStart, int iPageMax)
        {
            string msg = string.Empty;
            RightAndLog rl = new RightAndLog();
            try
            {
                if (myHeader == null)
                {
                    throw new LogicException("不正确的调用方法！");
                }

                rl.actionType = "充值查询函数";
                rl.ID = u_ID;
                rl.sign = 1;
                rl.strRightCode = "TradeManagement"; //"GetFundList";
                rl.type = "查询";
        
                PublicRes.SetRightAndLog(myHeader, rl);
                if (!rl.CheckRight())
                {
                    throw new LogicException("用户无权执行此操作！");
                }

                FundQueryClass cuser = new FundQueryClass(u_ID, u_QueryType, fcurtype, u_BeginTime, u_EndTime, fstate, fnum, fnumMax, banktype, sorttype, isHistory);

                int start = iPageStart - 1;
                if (start < 0) start = 0;

                string errMsg = "";
                string strSql = cuser.ICESQL + "&strlimit=limit " + start + "," + iPageMax;
                DataSet ds = CommQuery.GetDataSetFromICE(strSql, cuser.ICETYPE, out errMsg);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)  
                {
                    if (u_QueryType.ToLower() == "tobank")   //给银行的订单号
                    {
                        strSql = "bank_list=" + u_ID;

                        if (fstate != 0)
                            strSql += "&sign=" + fstate;

                        long num = (long)Math.Round(fnum * 100, 0);
                        long numMax = (long)Math.Round(fnumMax * 100, 0);


                        strSql += "&num_start=" + num + "&num_end=" + numMax;

                        if (banktype != "0000" && banktype != "")
                        {
                            strSql += "&bank_type=" + banktype;
                        }
                        strSql += string.Format("&offset=0&limit=10&sp_id=1000000000&MSG_NO=&fronttime_start={0}&fronttime_end={1}",
                        u_BeginTime.ToString("yyyy-MM-dd HH:mm:ss"), u_EndTime.ToString("yyyy-MM-dd HH:mm:ss"));
                        ds = CommQuery.GetDataSetFromICE_QueryServer(strSql, CommQuery.QUERY_TCBANKROLL_S, out msg);
                    }
                }

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    PublicRes.GetUserName_Table(ds.Tables[0], "Fauid", "Faname");
                }

                return ds;

            }
            catch (LogicException err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message + " " + msg);
                throw;
            }
            catch (Exception err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message + " " + msg);
                throw new LogicException("Service处理失败！" + err.Message + " " + msg);
            }
            finally
            {
                rl.WriteLog();
            }
        }

        [WebMethod(Description = "充值查询个数函数")]
        public int GetFundListCount(string u_ID, string u_QueryType, int fcurtype, DateTime u_BeginTime, DateTime u_EndTime,
            int fstate, float fnum, float fnumMax, string banktype, bool isHistory)
        {
            try
            {
                FundQueryClass cuser = new FundQueryClass(u_ID, u_QueryType, fcurtype, u_BeginTime, u_EndTime, fstate, fnum, fnumMax, banktype, "0", isHistory);
                return cuser.GetCount("HT");
            }
            catch (Exception e)
            {
                throw new Exception("service发生错误,请联系管理员！");
                return 0;
            }
        }

        [WebMethod(Description = "充值查询详细函数")]
        public DataSet GetFundListDetail(string tdeid, string listid, DateTime u_BeginTime, DateTime u_EndTime, bool oldflag, bool isHistory)
        {
            try
            {
                FundQueryClass cuser = new FundQueryClass(tdeid, listid, u_BeginTime, u_EndTime, oldflag, isHistory);

                string errMsg = "";
                string strSql = cuser.ICESQL;
                DataSet ds = CommQuery.GetDataSetFromICE(strSql, cuser.ICETYPE, out errMsg);

                //yinhuang 2014/2/20 查历史表详情 
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    PublicRes.GetUserName_Table(ds.Tables[0], "Fauid", "Faname");
                }

                return ds;
            }
            catch (Exception e)
            {
                throw new Exception("service发生错误,请联系管理员！" + e.Message);
                return null;
            }
        }

        [WebMethod(Description = "查询充值单历史详细函数")]
        public DataSet GetFundHistoryDetail(string u_ID, string payTime, string listid, string bank_type)
        {
            try
            {
                DataSet ds = null;
                if (string.IsNullOrEmpty(u_ID))
                {
                    return null;
                }
                DateTime date;
                if (string.IsNullOrEmpty(payTime))
                {
                    date = DateTime.Now;
                }
                else
                {
                    date = DateTime.Parse(payTime);
                }
                string str_where = " where Fbank_list='" + u_ID + "'";
                if (!string.IsNullOrEmpty(listid))
                {
                    str_where += " AND Flistid='" + listid + "'";
                }
                if (!string.IsNullOrEmpty(bank_type))
                {
                    str_where += " AND Fbank_type=" + bank_type;
                }

                MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("BANKLISTHIS"));
                da.OpenConn();
                string table = "order1_c2c_order_db_" + u_ID.Substring(u_ID.Length - 2, 2) + "_" + date.Year.ToString() + ".t_tcbankroll_list_" + u_ID.Substring(u_ID.Length - 3, 1);
                string sql = "select * from " + table + " " + str_where;
                ds = da.dsGetTotalData(sql);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    table = "order2_c2c_order_db_" + u_ID.Substring(u_ID.Length - 2, 2) + "_" + date.Year.ToString() + ".t_tcbankroll_list_" + u_ID.Substring(u_ID.Length - 3, 1);
                    sql = "select * from " + table + " " + str_where;
                    ds = da.dsGetTotalData(sql);
                }
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    PublicRes.GetUserName_Table(ds.Tables[0], "Fauid", "Faname");
                }

                return ds;
            }
            catch (Exception e)
            {
                throw new Exception("service发生错误,请联系管理员！" + e.Message);
                return null;
            }
        }

        [WebMethod(Description = "子帐户充值详细查询函数")]
        public DataSet GetFundListDetail_Subacc(string listid, string fcurtype, out string strResp)
        {
            try
            {
                ICEAccess ice = new ICEAccess(PublicRes.ICEServerIPSub, PublicRes.ICEPortSub);
                ice.OpenConn();
                string strwhere = "where=" + ICEAccess.URLEncode("flistid=" + listid.Trim() + "&");
                strwhere += ICEAccess.URLEncode("fbank_list=" + listid + "&");
                strwhere += ICEAccess.URLEncode("fbank_type=1099&");
                strwhere += ICEAccess.URLEncode("fcurtype=" + fcurtype + "&");
                strResp = "";
                DataTable dt2 = ice.InvokeQuery_GetDataTable(YWSourceType.充值单资源, YWCommandCode.查询核心充值单信息, listid.Trim(), strwhere, out strResp);
                ice.CloseConn();
                DataSet ds = null;

                if (dt2 != null && dt2.Rows.Count > 0)
                {
                    PublicRes.GetUserName_Table(dt2, "Fauid", "Faname");
                    ds = new DataSet();
                    ds.Tables.Add(dt2);
                }

                return ds;
            }
            catch (Exception e)
            {
                throw new Exception("service发生错误,请联系管理员！" + e.Message);
                return null;
            }
        }
      
        [WebMethod(Description = "查询投诉商户列表函数")]
        public DataSet GetComplainBussList(string bussId, DateTime u_BeginTime, DateTime u_EndTime, int iPageStart, int iPageMax)
        {
            try
            {
                DataSet ds = null;
                ComplainBussClass cuser = new ComplainBussClass(bussId, u_BeginTime, u_EndTime);
                ds = cuser.GetResultX(iPageStart, iPageMax, "ht");

                return ds;
            }
            catch (Exception err)
            {
                throw new LogicException("Service处理失败！");
            }
        }

        [WebMethod(Description = "查询投诉商户数量函数")]
        public int GetComplainBussCount(string bussId, DateTime u_BeginTime, DateTime u_EndTime)
        {
            try
            {
                ComplainBussClass cuser = new ComplainBussClass(bussId, u_BeginTime, u_EndTime);
                return cuser.GetCount("ht");
            }
            catch (Exception err)
            {
                throw new LogicException("Service处理失败！");
                return 0;
            }
        }

        [WebMethod(Description = "添加投诉商户函数")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public bool AddComplainBuss(ComplainBussClass cbs, out string msg)
        {
            msg = "";
            try
            {
                return cbs.addComplainBuss(out msg);
            }
            catch (Exception err)
            {
                msg = err.Message;
                return false;
            }
        }

        [WebMethod(Description = "更新投诉商户函数")]
        public bool ChangeComplainBuss(ComplainBussClass cbs, out string msg)
        {
            msg = "";
            try
            {
                return cbs.changeComplainBuss(out msg);
            }
            catch (Exception err)
            {
                msg = err.Message;
                return false;
            }
        }

        [WebMethod(Description = "投诉商户详情函数")]
        public ComplainBussClass GetComplainBussDetail(string bussid, out string msg)
        {
            msg = "";
            try
            {
                return new ComplainBussClass(bussid);
            }
            catch (Exception err)
            {
                msg = err.Message;
                return null;
            }
        }

        [WebMethod(Description = "删除投诉商户函数")]
        public void DelComplainBuss(string bussid)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("ht"));

            try
            {
                da.OpenConn();
                string strSql = "delete from c2c_fmdb.t_complain_buss_list where Fbuss_id='" + bussid + "'";
                da.ExecSqlNum(strSql);
            }
            catch (Exception err)
            {
                throw new LogicException(err.Message);
            }
            finally
            {
                da.Dispose();
            }
        }

        [WebMethod(Description = "查询用户投诉列表函数")]
        public DataSet GetUserComplainList(string bussId, string cft_orderid, int comptype, int compstatus, DateTime u_BeginTime, DateTime u_EndTime, int iPageStart, int iPageMax)
        {
            try
            {
                DataSet ds = null;
                UserComplainClass cuser = new UserComplainClass(bussId, cft_orderid, comptype, compstatus, u_BeginTime, u_EndTime);
                ds = cuser.GetResultX(iPageStart, iPageMax, "ht");

                return ds;

            }
            catch (Exception err)
            {
                throw new LogicException("Service处理失败！");
            }
        }

        [WebMethod(Description = "查询用户投诉数量函数")]
        public int GetUserComplainCount(string bussId, string cft_orderid, int comptype, int compstatus, DateTime u_BeginTime, DateTime u_EndTime)
        {
            try
            {
                UserComplainClass cuser = new UserComplainClass(bussId, cft_orderid, comptype, compstatus, u_BeginTime, u_EndTime);
                return cuser.GetCount("ht");
            }
            catch (Exception err)
            {
                throw new LogicException("Service处理失败！");
                return 0;
            }
        }

        [WebMethod(Description = "添加用户投诉函数")]
        public string AddUserComplain(UserComplainClass ucc, out string msg)
        {
            msg = "";
            try
            {
                return ucc.addUserComplain(out msg);
            }
            catch (Exception err)
            {
                msg = err.Message;
                return "";
            }
        }

        [WebMethod(Description = "更新用户投诉函数")]
        public bool ChangeUserComplain(UserComplainClass ucc, out string msg)
        {
            msg = "";
            try
            {
                return ucc.changeUserComplain(out msg);
            }
            catch (Exception err)
            {
                msg = err.Message;
                return false;
            }
        }

        [WebMethod(Description = "用户投诉详情函数")]
        public UserComplainClass GetUserComplainDetail(string listid, out string msg)
        {
            msg = "";
            try
            {
                return new UserComplainClass(int.Parse(listid));
            }
            catch (Exception err)
            {
                msg = err.Message;
                return null;
            }
        }

        [WebMethod(Description = "催办用户投诉函数")]
        public bool RemindUserComplain(UserComplainClass ucc, out string msg)
        {
            msg = "";
            try
            {
                return ucc.remindUserComplain(out msg);
            }
            catch (Exception err)
            {
                msg = err.Message;
                return false;
            }
        }

        [WebMethod(Description = "查询退款登记函数")]
        public DataSet QueryRefundInfo(string coding, string orderId, string stime, string etime, int refundType, int refundState, string tradeState, int iPageStart, int iPageMax)
        {
            string errMsg = "";
            try
            {
                DataSet ds = null;
                DataSet newDs = null;

                RefundInfoClass cuser = new RefundInfoClass(coding, orderId, stime, etime, refundType, refundState, "");
                string msg = "";
                ds = cuser.GetResultX(iPageStart, iPageMax, "ht");
                if (ds != null && ds.Tables.Count > 0)
                {
                    ds.Tables[0].Columns.Add("Ftrade_state_new", typeof(String));
                    ds.Tables[0].Columns.Add("FTrade_Type", typeof(String));

                    newDs = new DataSet();
                    DataTable res_dt = new DataTable();
                    newDs.Tables.Add(res_dt);
                    for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                    {
                        res_dt.Columns.Add(ds.Tables[0].Columns[i].ColumnName);
                    }

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        msg = "";
                        string striceWhere = "listid=" + dr["Forder_id"].ToString();
                        DataTable dt_ice = CommQuery.GetTableFromICE(striceWhere, "query_order_service", out msg);
                        if (msg != "")
                        {
                            errMsg += msg;
                        }
                        if (dt_ice != null && dt_ice.Rows.Count > 0)
                        {
                            dr["FTrade_Type"] = dt_ice.Rows[0]["Ftrade_type"].ToString();
                            object obj = dt_ice.Rows[0]["Ftrade_state"];
                            //通过交易状态查询条件，来过滤记录
                            if (obj != null)
                            {
                                string state = obj.ToString().Trim();
                                dr["Ftrade_state_new"] = state;
                                if (!string.IsNullOrEmpty(tradeState) && tradeState != "0")
                                {
                                    if (state == tradeState)
                                    {
                                        //如果状态相同，表示是需要查询的记录
                                        res_dt.ImportRow(dr);
                                    }
                                }
                                else
                                {
                                    res_dt.ImportRow(dr);
                                }
                            }
                        }
                    }
                    //暂不处理异常
                    //if (errMsg != "") 
                    //{
                    //   throw new Exception(errMsg);
                    //}
                }

                return newDs;

            }
            catch (Exception err)
            {
                throw new LogicException(err.Message + errMsg);
            }
        }

        [WebMethod(Description = "查询退款登记数量函数")]
        public int QueryRefundCount(string coding, string orderId, string stime, string etime, int refundType, int refundState, string tradeState)
        {
            try
            {
                RefundInfoClass cuser = new RefundInfoClass(coding, orderId, stime, etime, refundType, refundState, tradeState);
                return cuser.GetCount("ht");
            }
            catch (Exception err)
            {
                throw new LogicException(err.Message);
                return 0;
            }
        }

        [WebMethod(Description = "添加退款登记函数")]
        public void AddRefundInfo(RefundInfoClass ric)
        {
            string msg = "";
            try
            {
                ric.addRefundInfo(out msg);
            }
            catch (Exception err)
            {
                throw new LogicException(err.Message);
            }
        }

        [WebMethod(Description = "修改退款登记函数")]
        public void ChangeRefundInfo(RefundInfoClass ric)
        {
            string msg = "";
            try
            {
                ric.changeRefundInfo(out msg);
            }
            catch (Exception err)
            {
                throw new LogicException(err.Message);
            }
        }

        [WebMethod(Description = "修改退款登记函数")]
        public void DelRefundInfo(string fid)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("ht"));
            try
            {
                da.OpenConn();
                string strSql = "delete from c2c_fmdb.t_refund_info  where Fid='" + fid + "'";
                da.ExecSqlNum(strSql);
            }
            catch (Exception err)
            {
                throw new LogicException(err.Message);
            }
            finally
            {
                da.Dispose();
            }
        }

        [WebMethod(Description = "退款登记详细函数")]
        public RefundInfoClass GetRefundDetail(string fid)
        {
            try
            {
                return new RefundInfoClass(fid);
            }
            catch (Exception err)
            {
                throw new LogicException(err.Message);
            }
        }

        [WebMethod(Description = "更新提交退款状态函数")]
        public void UpdateSubmitRefundState(string fid, int refundState)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("ht"));
            try
            {
                da.OpenConn();
                string sql = string.Format(@"
                update c2c_fmdb.t_refund_info set Fsubmit_refund={0},Fmodify_time=now() where Fid in ({1})",
                refundState, fid);
                da.ExecSqlNum(sql);
            }
            catch (Exception err)
            {
                throw new LogicException(err.Message);
            }
            finally
            {
                da.Dispose();
            }
        }

        [WebMethod(Description = "查询运通账号信息列表函数")]
        public DataSet GetYTInfoList(string cftNo, string ytNo, string beginTime, string card_type)
        {
            string strReplyInfo;
            short iResult;
            string msg = "";

            try
            {
                DataSet dt = null;
                string ww = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile("CFT_NO_PASS_WATCH_WORD", "md5").ToLower();
                string req = "card_type=" + card_type + "&watch_word=" + ww;//card_type=1运通 2 中行
                //首先要查询uid
                if (cftNo != null && cftNo != "")
                {
                    string uid = PublicRes.ConvertToFuid(cftNo);
                    req += "&uid=" + uid;
                }
                if (ytNo != null && ytNo != "")
                {
                    req += "&card_id=" + ytNo;
                }
                if (beginTime != null && beginTime != "")
                {
                    req += "&expire_date=" + beginTime;
                }
                string service_name = "fcpay_manage_querycard_service"; //uid=299442012&card_type=1&card_id=111&expire_date=111&verify_info=0
                dt = CommQuery.GetOneTableFromICE(req, CommQuery.QUERY_YT_INFO, service_name, out msg);
                if (msg != "")
                    throw new Exception("Service处理失败！" + msg);

                return dt;

            }
            catch (Exception err)
            {
                throw new LogicException("Service处理失败！" + msg);
            }
        }

        [WebMethod(Description = "查询身份证开通卡个数函数")]
        public DataSet GetCertNum(string certNo, string card_type)
        {
            string strReplyInfo;
            short iResult;
            string msg = "";

            try
            {
                DataSet dt = null;
                string ww = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile("CFT_NO_PASS_WATCH_WORD", "md5").ToLower();
                string req = "watch_word=" + ww;

                if (certNo != null && certNo != "")
                {
                    req += "&crenum=" + certNo;
                }
                req += "&card_type=" + card_type;
                string service_name = "fcpay_manage_querycrenum_service"; //uid=299442012&card_type=1&card_id=111&expire_date=111&verify_info=0
                dt = CommQuery.GetOneTableFromICE(req, CommQuery.QUERY_YT_CERT_NUM, service_name, out msg);
                if (msg != "")
                    throw new Exception("Service处理失败！" + msg);
                return dt;

            }
            catch (Exception err)
            {
                throw new LogicException("Service处理失败！" + msg);
            }
        }

        [WebMethod(Description = "查询运通账号冻结列表函数")]
        public DataSet GetYTFreezeList(string cftNo, string ytNo, string tradeNo, string bussOrderNo, string beginTime, string endTime, int iPageStart, int iPageMax, string card_type)
        {
            string msg = "";
            try
            {
                DataSet ds = null;
                string ww = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile("CFT_NO_PASS_WATCH_WORD", "md5").ToLower();
                string req = "watch_word=" + ww + "&offset=" + iPageStart + "&limit=" + iPageMax;

                if (cftNo != null && cftNo != "")
                {
                    string uid = PublicRes.ConvertToFuid(cftNo);
                    req += "&uid=" + uid;
                }
                if (ytNo != null && ytNo != "")
                {
                    req += "&card_id=" + ytNo;
                }
                if (tradeNo != null && tradeNo != "")
                {
                    req += "&listid=" + tradeNo;
                }
                if (bussOrderNo != null && bussOrderNo != "")
                {
                    req += "&tid=" + bussOrderNo;
                }
                if (beginTime != null && beginTime != "")
                {
                    req += "&s_time=" + beginTime;
                }
                if (endTime != null && endTime != "")
                {
                    req += "&e_time=" + endTime;
                }
                req += "&card_type=" + card_type;
                string service_name = "fcpay_manage_queryfreezeorder_service";
                ds = CommQuery.GetXmlToDataSetFromICE(req, CommQuery.QUERY_YT_FREEZE_INFO, service_name, out msg);
                if (msg != "")
                    throw new Exception("Service处理失败！" + msg);
                return ds;

            }
            catch (Exception err)
            {
                throw new LogicException("Service处理失败！" + msg);
            }
        }

        [WebMethod(Description = "查询运通账号交易列表函数")]
        public DataSet GetYTTradeList(string cftNo, string ytNo, string tradeNo, string bussOrderNo, string beginTime, string endTime, int iPageStart, int iPageMax, string card_type)
        {
            string msg = "";
            try
            {
                DataSet ds = null;
                string ww = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile("CFT_NO_PASS_WATCH_WORD", "md5").ToLower();
                string req = "watch_word=" + ww + "&offset=" + iPageStart + "&limit=" + iPageMax;

                if (cftNo != null && cftNo != "")
                {
                    string uid = PublicRes.ConvertToFuid(cftNo);
                    req += "&uid=" + uid;
                }
                if (ytNo != null && ytNo != "")
                {
                    req += "&card_id=" + ytNo;
                }
                if (tradeNo != null && tradeNo != "")
                {
                    req += "&listid=" + tradeNo;
                }
                if (bussOrderNo != null && bussOrderNo != "")
                {
                    req += "&tid=" + bussOrderNo;
                }
                if (beginTime != null && beginTime != "")
                {
                    req += "&s_time=" + beginTime;
                }
                if (endTime != null && endTime != "")
                {
                    req += "&e_time=" + endTime;
                }
                req += "&card_type=" + card_type;
                string service_name = "fcpay_manage_querytranse_service";
                ds = CommQuery.GetXmlToDataSetFromICE(req, CommQuery.QUERY_YT_TRADE_INFO, service_name, out msg);
                if (msg != "")
                    throw new Exception("Service处理失败！" + msg);
                return ds;

            }
            catch (Exception err)
            {
                throw new LogicException("Service处理失败！" + msg);
            }
        }

        [WebMethod(Description = "查询外汇汇率列表函数")]
        public DataSet GetExchangeRateList(string foreType, string issueBank, string beginTime, string endTime, int iPageStart, int iPageMax)
        {
            try
            {
                DataSet ds = null;
                //首先要查询uid
                ExchangeRateQueryClass cuser = new ExchangeRateQueryClass(foreType, issueBank, beginTime, endTime);
                ds = cuser.GetResultX(iPageStart, iPageMax, "FCPAY");

                return ds;

            }
            catch (Exception err)
            {
                LogHelper.LogInfo("查询外汇汇率列表异常：" + err);
                throw new LogicException("Service处理失败！" +err.Message);
            }
        }
          
        [WebMethod(Description = "代扣批次请求")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet BatchWithholdReq(string sp_id, string sp_batchid, string batchid, string fname, int verify_way, int total_count, int faudit_flag, int fcmd)
        {
            string msg = "";
            DataSet ds = null;

            try
            {
                if (myHeader == null)
                {
                    throw new LogicException("不正确的调用方法！");
                }

                string key = "CFT_MANUAL_WATCH_WORD";
                long curr_time = DateTime.Now.Ticks;
                string req = "op_type=3&channel=8192&client_ip=" + myHeader.UserIP + "&oper_id=" + myHeader.OperID + "&timestamp=" + curr_time + "&cmd=" + fcmd;
                string checksign = sp_id + sp_batchid + curr_time + key;
                checksign = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(checksign, "md5").ToLower();

                req += "&sp_id=" + sp_id;
                req += "&sp_batchid=" + sp_batchid;
                req += "&batchid=" + batchid;
                req += "&total_count=" + total_count;
                req += "&fname=" + fname;
                req += "&verify_way=" + verify_way;
                req += "&audit_flag=" + faudit_flag; //是否审核
                req += "&memo=客服操作";
                req += "&checksign=" + checksign;
                string service_name = "cep_contract_batch_mng_service";
                ds = CommQuery.GetOneTableFromICE(req, "FINANCE_OD_BATCH_WITHHOLD_REQ", service_name, out msg);
            }
            catch (Exception err)
            {
                throw new LogicException("Service处理失败！" + msg);
            }

            return ds;
        }

        [WebMethod(Description = "代扣协议库查询")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet QueryProtocolBatch(string sp_id, string sp_name, string beginTime, string endTime, int iPageStart, int iPageMax)
        {
            string msg = "";
            DataSet ds = null;

            try
            {
                if (myHeader == null)
                {
                    throw new LogicException("不正确的调用方法！");
                }

                string strWhere = "";
                string splist = ""; //多个spid的组合

                if (sp_name != null && sp_name != "")
                {
                    //通过商户名称去查询商户号，可能有多个
                    strWhere = "spname=" + sp_name;
                    ds = CommQuery.GetDataSetFromICE(strWhere, "FINANCE_UI_QUERY_MERCHANTINFO", out msg);
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            splist += dr["Fspid"].ToString();
                            splist += ",";
                        }
                    }
                }
                if (splist == "")
                {
                    if (sp_id != null && sp_id != "")
                    {
                        splist = sp_id;
                    }
                }
                else
                {
                    splist = splist.Substring(0, splist.Length - 1);
                }

                if (splist == "")
                {
                    throw new LogicException("缺少参数！");
                }

                strWhere = "sp_id=" + splist + "&start_createtime=" + beginTime + "&end_createtime=" + endTime + "&offset=" + iPageStart + "&limit=" + iPageMax;

                ds = CommQuery.GetDataSetFromICE(strWhere, "FINANCE_QUERY_PROTOCOL_BATCH", out msg);
            }
            catch (Exception err)
            {
                throw new LogicException("Service处理失败！" + msg);
            }

            return ds;
        }

        [WebMethod(Description = "代扣协议明细查询")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet QueryProtocolDetail(string sp_id, string beginTime, string endTime, int iPageStart, int iPageMax)
        {
            string msg = "";
            DataSet ds = null;

            try
            {
                if (myHeader == null)
                {
                    throw new LogicException("不正确的调用方法！");
                }

                if (sp_id == null || sp_id == "")
                {
                    throw new LogicException("缺少参数！");
                }

                string strWhere = "sp_id=" + sp_id + "&start_createtime=" + beginTime + "&end_createtime=" + endTime + "&offset=" + iPageStart + "&limit=" + iPageMax;
                ds = CommQuery.GetDataSetFromICE(strWhere, "FINANCE_QUERY_PROTOCOL_DETAIL", out msg);
                if (ds != null && ds.Tables.Count > 0)
                {
                    ds.Tables[0].Columns.Add("Fbank_area_str", typeof(String));

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        string area = dr["Fbank_area"].ToString();
                        if (area != null && area != "")
                        {
                            dr["Fbank_area_str"] = AreaInfo.GetAreaName_Long(Int32.Parse(area));
                        }
                    }
                }
            }
            catch (Exception err)
            {
                throw new LogicException("Service处理失败！" + msg);
            }

            return ds;
        }

        [WebMethod(Description = "查询kps_mds值")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public string QueryKpsMD5(string sp_id, string m_sp_id, string file_path)
        {
            DataSet ds = null;
            string msg = "";
            string ret = "";

            try
            {
                //先MD5(file)
                //string file_md5_2 = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile("abc", "md5").ToLower();
                System.Security.Cryptography.HashAlgorithm tans = (System.Security.Cryptography.HashAlgorithm)System.Security.Cryptography.MD5.Create();
                FileStream fs = new FileStream(file_path, FileMode.Open);
                byte[] b_ot = tans.ComputeHash(fs);
                fs.Close();

                string file_md5 = BitConverter.ToString(b_ot).Replace("-", "").ToLower();
                string req = "merchant_spid=" + m_sp_id;
                string sp_str = file_md5 + "%26key%3D";
                req += "&sp_str=" + sp_str;

                string service_name = "kps_retmd5_service";
                ds = CommQuery.GetOneTableFromICE(req, "FINANCE_OD_QUERY_KPS_RETMD5", service_name, false, out msg);
                if (ds != null)
                {
                    DataTable dt = ds.Tables[0];
                    if (dt.Rows[0]["result"].ToString().Trim() == "0")
                    {
                        ret = dt.Rows[0]["sp_md5"].ToString();
                    }
                }
            }
            catch (Exception err)
            {
                throw new LogicException("Service处理失败！" + msg);
            }

            return ret;
        }

        [WebMethod(Description = "查询打折密码函数")]
        public DataSet QueryDiscountCode(string cft_no, string cdk_no, string s_status, int iPageStart, int iPageMax)
        {
            DataSet ds = null;
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("cdk"));
            try
            {
                da.OpenConn();
                string sql = "SELECT * FROM db_cdk.t_cdk_record WHERE 1=1 ";
                if (!string.IsNullOrEmpty(cft_no))
                {
                    sql += " AND Fuin='" + cft_no.Trim() + "'";
                }
                if (!string.IsNullOrEmpty(cdk_no))
                {
                    sql += " AND Fcdkeytype='" + cdk_no.Trim() + "'";
                }
                if (s_status != "0000")
                {
                    sql += " AND Fstate='" + s_status + "'";
                }
                sql += " LIMIT " + iPageStart + "," + iPageMax;

                ds = da.dsGetTotalData(sql);
            }
            catch (Exception err)
            {
                throw new LogicException("查询t_cdk_record失败");
            }
            finally
            {
                da.Dispose();
            }
            return ds;
        }

        [WebMethod(Description = "证件号码清理函数")]
        public int ClearCreid(int type, string creid)
        {
            MySqlAccess da = null;
            int ret = 0;
            try
            {
                if (string.IsNullOrEmpty(creid.Trim()))
                {
                    throw new Exception("证件号码不能为空！");
                }

                string tablename = PublicRes.GetTableNameByCreid("creid_statistics", creid.Trim());
                if (type == 0)
                {
                    //普通用户
                    da = new MySqlAccess(PublicRes.GetConnString("statistics"));//统计数据库
                }
                else
                {
                    //微信用户
                    da = new MySqlAccess(PublicRes.GetConnString("comprehensive"));//综合业务数据库
                }
                da.OpenConn();
                string sql = "update " + tablename + " set count=0 where Fcreid='" + creid + "'";

                ret = da.ExecSqlNum(sql);
                LogHelper.LogInfo("证件号码清理执行sql语句:" + sql + ".执行结果:" + ret);
            }
            catch (LogicException err)
            {
                LogHelper.LogInfo("发生异常:" + err.ToString());
                throw;
            }
            catch (Exception e)
            {
                LogHelper.LogInfo("发生异常:" + e.ToString());
                throw new Exception("service发生错误！(" + e.Message + ")");
            }
            finally
            {
                if (da != null) da.Dispose();
            }
            return ret;
        }

        //yinhuang 2013/9/5
        [WebMethod(Description = "新的财付券个数函数")]
        public int GetGwqCount_new(string u_ID, int is_tdeID, string filter)
        {
            try
            {
                GwqQueryClass cuser = new GwqQueryClass(u_ID, is_tdeID, filter);
                return cuser.GetCount("GWQ");
            }
            catch (LogicException err)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new Exception("service发生错误,请联系管理员！(" + e.Message + ")");
                return 0;
            }
        }

        [WebMethod(Description = "新的财付券查询函数")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet GetGwq_new(string u_ID, int is_tdeID, string filter, int iPageStart, int iPageMax)
        {
            RightAndLog rl = new RightAndLog();
            try
            {
                if (myHeader == null)
                {
                    throw new LogicException("不正确的调用方法！");
                }

                GwqQueryClass cuser = new GwqQueryClass(u_ID, is_tdeID, filter);
                return cuser.GetResultX(iPageStart, iPageMax, "GWQ");

            }
            catch (LogicException err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceHtmlStr(err.Message);
                throw;
            }
            catch (Exception err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceHtmlStr(err.Message);
                throw new LogicException("Service处理失败！(" + err.Message + ")");
            }
            finally
            {
                rl.WriteLog();
            }
        }

        [WebMethod(Description = "财付券记录信息")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public T_GWQ GetGwqInfo(string u_ID, string ticket_id)
        {
            RightAndLog rl = new RightAndLog();
            try
            {
                if (myHeader == null)
                {
                    throw new Exception("不正确的调用方法！");
                }
    
                GwqQueryClass cuser = new GwqQueryClass(u_ID, 0);   //spid 中介账户
                return cuser.GetResult(ticket_id);
            }
            catch (LogicException err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceHtmlStr(err.Message);
                throw;
            }
            catch (Exception err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceHtmlStr(err.Message);
                throw new LogicException("Service处理失败！(" + err.Message + ")");
            }
            finally
            {
                rl.WriteLog();
            }

        }

        [WebMethod(Description = "财付券操作日志查询函数")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet GetGwqRoll(string u_ID, string ticket_id)
        {
            RightAndLog rl = new RightAndLog();
            try
            {
                if (myHeader == null)
                {
                    throw new LogicException("不正确的调用方法！");
                }          
                GwqRollQueryClass cuser = new GwqRollQueryClass(u_ID, ticket_id);
                return cuser.GetResultX("GWQ");

            }
            catch (LogicException err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceHtmlStr(err.Message);
                throw;
            }
            catch (Exception err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceHtmlStr(err.Message);
                throw new LogicException("Service处理失败！(" + err.Message + ")");
            }
            finally
            {
                rl.WriteLog();
            }
        }

        //yinhuang 2013/9/22
        [WebMethod(Description = "解冻非主卡函数")]
        public void FreezeNonPrimaryCard(string qqid, int state, string curtype, string banktype, string cardtail)
        {
            string errMsg = "";
            try
            {
                if (string.IsNullOrEmpty(qqid))
                {
                    throw new Exception("冻结非主卡出错！用户账户不能为空。");
                }
                qqid = qqid.Trim();
                string uid = PublicRes.ConvertToFuid(qqid);
                if (uid == null || uid.Length < 3)
                {
                    throw new Exception("冻结非主卡出错！内部ID不正确。");
                }
                string strSql = "uid=" + uid + "&state=" + state;

                if (!string.IsNullOrEmpty(curtype))
                {
                    strSql += "&curtype=" + curtype.Trim();
                }
                if (!string.IsNullOrEmpty(banktype))
                {
                    strSql += "&bank_type=" + banktype.Trim();
                }
                if (!string.IsNullOrEmpty(cardtail))
                {
                    strSql += "&card_tail=" + cardtail.Trim();
                }
                int ret = CommQuery.ExecSqlFromICE(strSql, CommQuery.UPDATE_NON_PRIMIRY_BANK, out errMsg);
                if (ret < 0)
                {
                    throw new Exception(errMsg);
                }
            }
            catch (Exception err)
            {
                throw new LogicException("Service处理失败！" + err.Message);
            }
        }

        #region 信用支付

        [WebMethod(Description = "还款查询函数")]
        public DataSet QueryCreditRefundList(string uin, string s_day, string e_day, int iPageStart, int iPageMax)
        {
            DataSet ds = null;
            string Msg = "";  //调用前置机用
            string errMsg = ""; //解析xml用

            try
            {
                if (uin == null || uin == "")
                {
                    throw new Exception("账号不能为空");
                }
                string uid = PublicRes.ConvertToFuid(uin);
                if (uid == null || uid.Length < 3)
                {
                    throw new Exception(uin + "账号不存在");
                }
                //通过uid查询account_no
                string req_a = "request_type=8009&flag=1&reqid=5002&fields=uid:" + uid;

                DataSet ds_a = CommQuery.GetXmlToDataSetFromICE(req_a, "", "common_query_service", out Msg);
                if (Msg != "")
                {
                    throw new Exception(Msg);
                }
                string acc_no = ""; //贷款账号
                string bank_type = "";

                if (ds_a != null && ds_a.Tables.Count > 0 && ds_a.Tables[0].Rows.Count > 0)
                {
                    acc_no = ds_a.Tables[0].Rows[0]["Faccount_no"].ToString();
                    bank_type = ds_a.Tables[0].Rows[0]["Fbank_type"].ToString();
                }

                if (acc_no == "")
                {
                    throw new Exception("贷款账号为空");
                }

                //通过uid查询uidtoc
                Msg = "";
                req_a = "request_type=8009&flag=1&reqid=5008&fields=uid:" + uid;
                ds_a = CommQuery.GetXmlToDataSetFromICE(req_a, "", "common_query_service", out Msg);
                if (Msg != "")
                {
                    throw new Exception(Msg);
                }
                string uidtoc = "";
                if (ds_a != null && ds_a.Tables.Count > 0 && ds_a.Tables[0].Rows.Count > 0)
                {
                    uidtoc = ds_a.Tables[0].Rows[0]["Fuidtoc"].ToString();
                }
                if (uidtoc == "")
                {
                    throw new Exception("必填参数uidtoc为空");
                }

                /**设置初始值---------start-----------**/
                if (iPageStart < 0)
                {
                    iPageStart = 0;
                }
                if (iPageMax < 1)
                {
                    iPageMax = 10;
                }
                if (string.IsNullOrEmpty(s_day))
                {
                    if (!string.IsNullOrEmpty(e_day))
                    {
                        s_day = DateTime.Parse(e_day).AddDays(-30).ToString("yyyy-MM-dd"); //结束日期的前30天
                    }
                    else
                    {
                        s_day = DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd"); //当前日期的前30天
                    }
                }
                if (string.IsNullOrEmpty(e_day))
                {
                    if (!string.IsNullOrEmpty(s_day))
                    {
                        e_day = DateTime.Parse(s_day).AddDays(30).ToString("yyyy-MM-dd"); //结束日期的后30天
                    }
                    else
                    {
                        e_day = DateTime.Now.ToString("yyyy-MM-dd");
                    }
                }
                /**设置初始值---------end-----------**/

                string qzj_ip = ConfigurationManager.AppSettings["CreditPay_QZJ_IP"];
                string qzj_port = ConfigurationManager.AppSettings["CreditPay_QZJ_PORT"];
                string CFTAccount = ConfigurationManager.AppSettings["CFTAccount"];
                string req = "request_type=2211&ver=1&head_u=&uid=" + uid;

                req += "&acctNo=" + acc_no + "&bank_type=" + bank_type + "&sp_id=" + CFTAccount + "&uidtoc=" + uidtoc;
                req += "&queryBegDate=" + s_day + "&queryEndDate=" + e_day + "&queryBegNum=" + iPageStart + "&queryCnt=" + iPageMax;
                Msg = ""; //重置

                string answer = commRes.GetFromRelay(req, qzj_ip, qzj_port, out Msg);

                if (answer == "")
                {
                    return null;
                }
                if (Msg != "")
                {
                    throw new Exception(Msg);
                }
                //解析xml
                ds = CommQuery.PaseRelayXml(answer, out errMsg);
                if (errMsg != "")
                {
                    throw new Exception(errMsg);
                }
            }
            catch (Exception e)
            {
                throw new LogicException("service处理失败:" + e.Message);
            }

            return ds;
        }

        [WebMethod(Description = "欠款查询函数")]
        public DataSet QueryCreditDebt(string uin)
        {
            DataSet ds = null;
            string Msg = "";  //调用前置机用
            string errMsg = ""; //解析xml用

            try
            {
                if (uin == null || uin == "")
                {
                    throw new Exception("账号不能为空");
                }
                string uid = PublicRes.ConvertToFuid(uin);
                if (uid == null || uid.Length < 3)
                {
                    throw new Exception(uin + "账号不存在");
                }
                //通过uid查询account_no
                string req_a = "request_type=8009&flag=1&reqid=5002&fields=uid:" + uid;

                DataSet ds_a = CommQuery.GetXmlToDataSetFromICE(req_a, "", "common_query_service", out Msg);
                if (Msg != "")
                {
                    throw new Exception(Msg);
                }
                string acc_no = ""; //贷款账号
                string bank_type = "";

                if (ds_a != null && ds_a.Tables.Count > 0 && ds_a.Tables[0].Rows.Count > 0)
                {
                    acc_no = ds_a.Tables[0].Rows[0]["Faccount_no"].ToString();
                    bank_type = ds_a.Tables[0].Rows[0]["Fbank_type"].ToString();
                }
                if (acc_no == "")
                {
                    throw new Exception("贷款账号为空");
                }

                //通过uid查询uidtoc
                Msg = "";
                req_a = "request_type=8009&flag=1&reqid=5008&fields=uid:" + uid;
                ds_a = CommQuery.GetXmlToDataSetFromICE(req_a, "", "common_query_service", out Msg);
                if (Msg != "")
                {
                    throw new Exception(Msg);
                }
                string uidtoc = "";
                if (ds_a != null && ds_a.Tables.Count > 0 && ds_a.Tables[0].Rows.Count > 0)
                {
                    uidtoc = ds_a.Tables[0].Rows[0]["Fuidtoc"].ToString();
                }
                if (uidtoc == "")
                {
                    throw new Exception("必填参数uidtoc为空");
                }

                //md5
                string CFTAccount = ConfigurationManager.AppSettings["CFTAccount"];
                string wxzfAccount = ConfigurationManager.AppSettings["wxzfAccount"];
                string relay_ip = ConfigurationManager.AppSettings["Relay_IP"];
                string relay_port = ConfigurationManager.AppSettings["Relay_PORT"];
                string sign = "account_no=" + acc_no + "&bank_type=" + bank_type + "&uid=" + uid + "&uidtoc=" + uidtoc + "&key=";
                sign = System.Web.HttpUtility.UrlEncode(sign, System.Text.Encoding.GetEncoding("gb2312"));

                Msg = "";
                errMsg = "";

                string req_sign = "request_type=132&ver=1&head_u=&sp_id=" + wxzfAccount + "&merchant_spid=" + wxzfAccount + "&sp_str=" + sign;
                string sign_md5 = commRes.GetFromRelay(req_sign, relay_ip, relay_port, out Msg);

                if (Msg != "")
                {
                    throw new Exception(Msg);
                }
                ds = CommQuery.ParseRelayStr(sign_md5, out errMsg);
                if (errMsg != "")
                {
                    throw new Exception(errMsg);
                }
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    sign_md5 = ds.Tables[0].Rows[0]["sp_md5"].ToString();
                }

                string qzj_ip = ConfigurationManager.AppSettings["CreditPay_QZJ_IP"];
                string qzj_port = ConfigurationManager.AppSettings["CreditPay_QZJ_PORT"];
                string req = "request_type=2209&ver=1&head_u=&uid=" + uid;
                req += "&account_no=" + acc_no + "&bank_type=" + bank_type + "&sp_id=" + CFTAccount + "&uidtoc=" + uidtoc + "&sign=" + sign_md5;

                Msg = ""; //重置
                errMsg = "";

                string answer = commRes.GetFromRelay(req, qzj_ip, qzj_port, out Msg);

                if (answer == "")
                {
                    return null;
                }
                if (Msg != "")
                {
                    throw new Exception(Msg);
                }

                //解析relay str
                ds = CommQuery.ParseRelayStr(answer, out errMsg);
                if (errMsg != "")
                {
                    throw new Exception(errMsg);
                }

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ds.Tables[0].Columns.Add("Ftruename", typeof(String));
                    ds.Tables[0].Columns.Add("Fqqid", typeof(String));
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        dr.BeginEdit();
                        string errMsg2 = "";
                        string strSql = "uid=" + uid;
                        string ftruename = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_USERINFO, "Ftruename", out errMsg2);

                        dr["Ftruename"] = ftruename;
                        dr["Fqqid"] = uin;

                        dr.EndEdit();
                    }
                }
            }
            catch (Exception e)
            {
                throw new LogicException("service处理失败:" + e.Message);
            }

            return ds;
        }

        [WebMethod(Description = "基本信息查询函数")]
        public DataSet QueryCreditUserInfo(string uin)
        {
            DataSet ds = null;
            string Msg = "";  //调用前置机用
            string errMsg = ""; //解析xml用

            try
            {
                if (uin == null || uin == "")
                {
                    throw new Exception("账号不能为空");
                }
                string uid = PublicRes.ConvertToFuid(uin);
                if (uid == null || uid.Length < 3)
                {
                    throw new Exception(uin + "账号不存在");
                }
                //通过uid查询uidtoc
                Msg = "";
                string req_a = "request_type=8009&flag=1&reqid=5008&fields=uid:" + uid;
                DataSet ds_a = CommQuery.GetXmlToDataSetFromICE(req_a, "", "common_query_service", out Msg);
                if (Msg != "")
                {
                    throw new Exception(Msg);
                }
                string uidtoc = "";
                if (ds_a != null && ds_a.Tables.Count > 0 && ds_a.Tables[0].Rows.Count > 0)
                {
                    uidtoc = ds_a.Tables[0].Rows[0]["Fuidtoc"].ToString();
                }

                if (uidtoc == "")
                {
                    throw new Exception("必填参数uidtoc为空");
                }

                //判断预授信和是否激活
                req_a = "request_type=8009&flag=1&reqid=5001&fields=uidtoc:" + uidtoc;
                ds_a = CommQuery.GetXmlToDataSetFromICE(req_a, "", "common_query_service", out Msg);
                //如果是未激活的，需要组装ds .....
                //Fpre_credit_state 2 待激活 4 已经激活
                string creditFlag = "";
                if (Msg != "")
                {
                    throw new Exception(Msg);
                }
                if (ds_a != null && ds_a.Tables.Count > 0 && ds_a.Tables[0].Rows.Count > 0)
                {
                    creditFlag = ds_a.Tables[0].Rows[0]["Fpre_credit_state"].ToString();
                }

                //通过uid查询account_no
                Msg = "";
                req_a = "request_type=8009&flag=1&reqid=5002&fields=uid:" + uid;

                ds_a = CommQuery.GetXmlToDataSetFromICE(req_a, "", "common_query_service", out Msg);
                if (Msg != "")
                {
                    throw new Exception(Msg);
                }
                string acc_no = ""; //贷款账号
                string bank_type = "";  //银行类型
                string credit_result = ""; //征信结果
                string ret_date = "";  //还款日
                string line_expdate = ""; //额度有效期

                if (ds_a != null && ds_a.Tables.Count > 0 && ds_a.Tables[0].Rows.Count > 0)
                {
                    acc_no = ds_a.Tables[0].Rows[0]["Faccount_no"].ToString();
                    bank_type = ds_a.Tables[0].Rows[0]["Fbank_type"].ToString();
                    credit_result = ds_a.Tables[0].Rows[0]["Fcredit_result"].ToString();
                    ret_date = ds_a.Tables[0].Rows[0]["Freturn_date"].ToString();
                    line_expdate = ds_a.Tables[0].Rows[0]["Fline_expdate"].ToString();
                }

                if (string.IsNullOrEmpty(creditFlag) || creditFlag == "2")
                {
                    //如果为空或者未激活
                    ds = new DataSet();
                    DataTable dt = new DataTable();
                    ds.Tables.Add(dt);
                    //需要增加9个字段
                    ds.Tables[0].Columns.Add("baseline_total", typeof(String));//总额度
                    ds.Tables[0].Columns.Add("baseline_rest", typeof(String));//可用额度
                    ds.Tables[0].Columns.Add("baseline_used", typeof(String));//已用额度
                    ds.Tables[0].Columns.Add("tmpline_total", typeof(String));//临时额度
                    ds.Tables[0].Columns.Add("activate_date", typeof(String));//激活日期

                    ds.Tables[0].Columns.Add("bank_acc_status", typeof(String));//账户状态
                    ds.Tables[0].Columns.Add("bank_creline_status", typeof(String));//额度状态
                    DataRow dr = dt.NewRow();
                    dr["bank_acc_status"] = "1N"; //非预授信
                    dt.Rows.Add(dr);
                }
                else
                {
                    if (acc_no == "")
                    {
                        throw new Exception("贷款账号为空");
                    }

                    //=================md5==========start================
                    string relay_ip = ConfigurationManager.AppSettings["Relay_IP"];
                    string relay_port = ConfigurationManager.AppSettings["Relay_PORT"];
                    string CFTAccount = ConfigurationManager.AppSettings["CFTAccount"];
                    string wxzfAccount = ConfigurationManager.AppSettings["wxzfAccount"];

                    string sign = "account_no=" + acc_no + "&bank_type=" + bank_type + "&uid=" + uid + "&uidtoc=" + uidtoc + "&key=";
                    sign = System.Web.HttpUtility.UrlEncode(sign, System.Text.Encoding.GetEncoding("gb2312"));

                    Msg = "";
                    errMsg = "";
                    string req_sign = "request_type=132&ver=1&head_u=&sp_id=" + wxzfAccount + "&merchant_spid=" + wxzfAccount + "&sp_str=" + sign;

                    string sign_md5 = commRes.GetFromRelay(req_sign, relay_ip, relay_port, out Msg);
                    if (Msg != "")
                    {
                        throw new Exception(Msg);
                    }
                    ds = CommQuery.ParseRelayStr(sign_md5, out errMsg);
                    if (errMsg != "")
                    {
                        throw new Exception(errMsg);
                    }
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        sign_md5 = ds.Tables[0].Rows[0]["sp_md5"].ToString();
                    }

                    //======================md5=======================end=====

                    string qzj_ip = ConfigurationManager.AppSettings["CreditPay_QZJ_IP"];
                    string qzj_port = ConfigurationManager.AppSettings["CreditPay_QZJ_PORT"];
                    string req = "request_type=2208&ver=1&head_u=&uid=" + uid;
                    req += "&account_no=" + acc_no + "&bank_type=" + bank_type + "&sp_id=" + CFTAccount + "&uidtoc=" + uidtoc + "&sign=" + sign_md5;

                    Msg = ""; //重置

                    string answer = commRes.GetFromRelay(req, qzj_ip, qzj_port, out Msg);

                    if (answer == "")
                    {
                        return null;
                    }
                    if (Msg != "")
                    {
                        throw new Exception(Msg);
                    }

                    //解析relay str
                    ds = CommQuery.ParseRelayStr(answer, out errMsg);
                    if (errMsg != "")
                    {
                        throw new Exception(errMsg);
                    }
                }

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ds.Tables[0].Columns.Add("Ftruename", typeof(String));
                    ds.Tables[0].Columns.Add("Fcredit", typeof(String));
                    ds.Tables[0].Columns.Add("Femail", typeof(String));
                    ds.Tables[0].Columns.Add("Fmobile", typeof(String));
                    ds.Tables[0].Columns.Add("Fqqid", typeof(String));

                    ds.Tables[0].Columns.Add("Fcredit_result", typeof(String));  //征集结果
                    ds.Tables[0].Columns.Add("Fret_date", typeof(String));  //还款日
                    ds.Tables[0].Columns.Add("Fline_expdate", typeof(String));  //额度有效期
                    ds.Tables[0].Columns.Add("activeFlag", typeof(String));//是否激活

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        dr.BeginEdit();
                        string errMsg2 = "";

                        string strSql = "uid=" + uid;
                        string ftruename = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_USERINFO, "Ftruename", out errMsg2);
                        string fcredit = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_USERINFO, "Fcredit", out errMsg2);
                        string femail = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_USERINFO, "Femail", out errMsg2);
                        string fmobile = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_USERINFO, "Fmobile", out errMsg2);

                        dr["Ftruename"] = ftruename;
                        dr["Fcredit"] = fcredit;
                        dr["Femail"] = femail;
                        dr["Fmobile"] = fmobile;
                        dr["Fqqid"] = uin;

                        dr["Fcredit_result"] = credit_result;
                        dr["Fret_date"] = ret_date;
                        dr["Fline_expdate"] = line_expdate;
                        dr["activeFlag"] = creditFlag;

                        dr.EndEdit();
                    }
                }
            }
            catch (Exception e)
            {
                throw new LogicException("service处理失败:" + e.Message);
            }

            return ds;
        }

        [WebMethod(Description = "账单查询函数")]
        public DataSet QueryCreditBillList(string uin, string month, int iPageStart, int iPageMax)
        {
            DataSet ds = null;
            string Msg = "";  //调用前置机用
            string errMsg = ""; //解析xml用

            try
            {
                if (uin == null || uin == "")
                {
                    throw new Exception("账号不能为空");
                }
                string uid = PublicRes.ConvertToFuid(uin);
                if (uid == null || uid.Length < 3)
                {
                    throw new Exception(uin + "账号不存在");
                }
                //通过uid查询account_no
                string req_a = "request_type=8009&flag=1&reqid=5002&fields=uid:" + uid;

                DataSet ds_a = CommQuery.GetXmlToDataSetFromICE(req_a, "", "common_query_service", out Msg);
                if (Msg != "")
                {
                    throw new Exception(Msg);
                }

                string acc_no = ""; //贷款账号
                string bank_type = "";

                if (ds_a != null && ds_a.Tables.Count > 0 && ds_a.Tables[0].Rows.Count > 0)
                {
                    acc_no = ds_a.Tables[0].Rows[0]["Faccount_no"].ToString();
                    bank_type = ds_a.Tables[0].Rows[0]["Fbank_type"].ToString();
                }
                if (acc_no == "")
                {
                    throw new Exception("贷款账号为空");
                }
                //通过uid查询uidtoc
                Msg = "";
                req_a = "request_type=8009&flag=1&reqid=5008&fields=uid:" + uid;
                ds_a = CommQuery.GetXmlToDataSetFromICE(req_a, "", "common_query_service", out Msg);
                if (Msg != "")
                {
                    throw new Exception(Msg);
                }
                string uidtoc = "";
                if (ds_a != null && ds_a.Tables.Count > 0 && ds_a.Tables[0].Rows.Count > 0)
                {
                    uidtoc = ds_a.Tables[0].Rows[0]["Fuidtoc"].ToString();
                }

                if (uidtoc == "")
                {
                    throw new Exception("必填参数uidtoc为空");
                }

                /**设置初始值---------start-----------**/
                if (iPageStart < 0)
                {
                    iPageStart = 0;
                }
                if (iPageMax < 1)
                {
                    iPageMax = 10;
                }

                /**设置初始值---------end-----------**/

                //=================md5==========start================
                string relay_ip = ConfigurationManager.AppSettings["Relay_IP"];
                string relay_port = ConfigurationManager.AppSettings["Relay_PORT"];
                string CFTAccount = ConfigurationManager.AppSettings["CFTAccount"];
                string wxzfAccount = ConfigurationManager.AppSettings["wxzfAccount"];

                string sign = "account_no=" + acc_no + "&bank_type=" + bank_type + "&uid=" + uid + "&uidtoc=" + uidtoc + "&key=";
                sign = System.Web.HttpUtility.UrlEncode(sign, System.Text.Encoding.GetEncoding("gb2312"));

                Msg = "";
                errMsg = "";
                string req_sign = "request_type=132&ver=1&head_u=&sp_id=" + wxzfAccount + "&merchant_spid=" + wxzfAccount + "&sp_str=" + sign;

                string sign_md5 = commRes.GetFromRelay(req_sign, relay_ip, relay_port, out Msg);
                if (Msg != "")
                {
                    throw new Exception(Msg);
                }
                ds = CommQuery.ParseRelayStr(sign_md5, out errMsg);
                if (errMsg != "")
                {
                    throw new Exception(errMsg);
                }
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    sign_md5 = ds.Tables[0].Rows[0]["sp_md5"].ToString();
                }

                //======================md5=======================end=====

                string qzj_ip = ConfigurationManager.AppSettings["CreditPay_QZJ_IP"];
                string qzj_port = ConfigurationManager.AppSettings["CreditPay_QZJ_PORT"];
                string req = "request_type=2205&ver=1&head_u=&uid=" + uid;
                req += "&account_no=" + acc_no + "&bank_type=" + bank_type + "&sp_id=" + CFTAccount + "&uidtoc=" + uidtoc;
                req += "&offset=" + iPageStart + "&limit=" + iPageMax + "&sign=" + sign_md5;
                Msg = ""; //重置

                string answer = commRes.GetFromRelay(req, qzj_ip, qzj_port, out Msg);

                if (answer == "")
                {
                    return null;
                }
                if (Msg != "")
                {
                    throw new Exception(Msg);
                }

                //解析xml
                ds = CommQuery.PaseRelayXml(answer, out errMsg);
                if (errMsg != "")
                {
                    throw new Exception(errMsg);
                }
            }
            catch (Exception e)
            {
                throw new LogicException("service处理失败:" + e.Message);
            }

            return ds;
        }

        [WebMethod(Description = "账单详情查询函数")]
        public DataSet QueryCreditBillDetail(string uin, string month, int iPageStart, int iPageMax)
        {
            DataSet ds = null;
            string Msg = "";  //调用前置机用
            string errMsg = ""; //解析xml用

            try
            {
                if (uin == null || uin == "")
                {
                    throw new Exception("账号不能为空");
                }

                string uid = PublicRes.ConvertToFuid(uin);
                if (uid == null || uid.Length < 3)
                {
                    throw new Exception(uin + "账号不存在");
                }

                //通过uid查询account_no
                string req_a = "request_type=8009&flag=1&reqid=5002&fields=uid:" + uid;

                DataSet ds_a = CommQuery.GetXmlToDataSetFromICE(req_a, "", "common_query_service", out Msg);
                if (Msg != "")
                {
                    throw new Exception(Msg);
                }

                string acc_no = ""; //贷款账号
                string bank_type = "";

                if (ds_a != null && ds_a.Tables.Count > 0 && ds_a.Tables[0].Rows.Count > 0)
                {
                    acc_no = ds_a.Tables[0].Rows[0]["Faccount_no"].ToString();
                    bank_type = ds_a.Tables[0].Rows[0]["Fbank_type"].ToString();
                }

                if (acc_no == "")
                {
                    throw new Exception("贷款账号为空");
                }

                //通过uid查询uidtoc
                Msg = "";
                req_a = "request_type=8009&flag=1&reqid=5008&fields=uid:" + uid;
                ds_a = CommQuery.GetXmlToDataSetFromICE(req_a, "", "common_query_service", out Msg);
                if (Msg != "")
                {
                    throw new Exception(Msg);
                }
                string uidtoc = "";
                if (ds_a != null && ds_a.Tables.Count > 0 && ds_a.Tables[0].Rows.Count > 0)
                {
                    uidtoc = ds_a.Tables[0].Rows[0]["Fuidtoc"].ToString();
                }

                if (uidtoc == "")
                {
                    throw new Exception("必填参数uidtoc为空");
                }

                /**设置初始值---------start-----------**/
                if (iPageStart < 0)
                {
                    iPageStart = 0;
                }
                if (iPageMax < 1)
                {
                    iPageMax = 10;
                }

                /**设置初始值---------end-----------**/

                //=================md5==========start================
                string relay_ip = ConfigurationManager.AppSettings["Relay_IP"];
                string relay_port = ConfigurationManager.AppSettings["Relay_PORT"];
                string CFTAccount = ConfigurationManager.AppSettings["CFTAccount"];
                string wxzfAccount = ConfigurationManager.AppSettings["wxzfAccount"];

                string sign = "account_no=" + acc_no + "&bank_type=" + bank_type + "&bill_month=" + month + "&uid=" + uid + "&uidtoc=" + uidtoc + "&key=";
                sign = System.Web.HttpUtility.UrlEncode(sign, System.Text.Encoding.GetEncoding("gb2312"));

                Msg = "";
                errMsg = "";
                string req_sign = "request_type=132&ver=1&head_u=&sp_id=" + wxzfAccount + "&merchant_spid=" + wxzfAccount + "&sp_str=" + sign;

                string sign_md5 = commRes.GetFromRelay(req_sign, relay_ip, relay_port, out Msg);
                if (Msg != "")
                {
                    throw new Exception(Msg);
                }
                ds = CommQuery.ParseRelayStr(sign_md5, out errMsg);
                if (errMsg != "")
                {
                    throw new Exception(errMsg);
                }
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    sign_md5 = ds.Tables[0].Rows[0]["sp_md5"].ToString();
                }

                //======================md5=======================end=====

                string qzj_ip = ConfigurationManager.AppSettings["CreditPay_QZJ_IP"];
                string qzj_port = ConfigurationManager.AppSettings["CreditPay_QZJ_PORT"];
                string req = "request_type=2218&ver=1&head_u=&uid=" + uid;
                req += "&account_no=" + acc_no + "&bank_type=" + bank_type + "&sp_id=" + CFTAccount + "&uidtoc=" + uidtoc;
                req += "&offset=" + iPageStart + "&limit=" + iPageMax + "&bill_month=" + month + "&sign=" + sign_md5;
                Msg = ""; //重置

                string answer = commRes.GetFromRelay(req, qzj_ip, qzj_port, out Msg);

                if (answer == "")
                {
                    return null;
                }
                if (Msg != "")
                {
                    throw new Exception(Msg);
                }

                //解析xml
                ds = CommQuery.PaseRelayXml(answer, out errMsg);
                if (errMsg != "")
                {
                    throw new Exception(errMsg);
                }
            }
            catch (Exception e)
            {
                throw new LogicException("service处理失败:" + e.Message);
            }

            return ds;
        }

        [WebMethod(Description = "资金流水查询函数")]
        public DataSet QueryCapitalRoll(string uin, string sDate, string eDate, int iPageStart, int iPageMax)
        {
            DataSet ds = null;
            string Msg = "";  //调用前置机用
            string errMsg = ""; //解析xml用

            try
            {
                if (uin == null || uin == "")
                {
                    throw new Exception("账号不能为空");
                }

                string uid = PublicRes.ConvertToFuid(uin);
                if (uid == null || uid.Length < 3)
                {
                    throw new Exception(uin + "账号不存在");
                }

                //通过uid查询account_no
                string req_a = "request_type=8009&flag=1&reqid=5002&fields=uid:" + uid;

                DataSet ds_a = CommQuery.GetXmlToDataSetFromICE(req_a, "", "common_query_service", out Msg);
                if (Msg != "")
                {
                    throw new Exception(Msg);
                }

                string acc_no = ""; //贷款账号
                string bank_type = "";

                if (ds_a != null && ds_a.Tables.Count > 0 && ds_a.Tables[0].Rows.Count > 0)
                {
                    acc_no = ds_a.Tables[0].Rows[0]["Faccount_no"].ToString();
                    bank_type = ds_a.Tables[0].Rows[0]["Fbank_type"].ToString();
                }

                if (acc_no == "")
                {
                    throw new Exception("贷款账号为空");
                }

                //通过uid查询uidtoc
                Msg = "";
                req_a = "request_type=8009&flag=1&reqid=5008&fields=uid:" + uid;
                ds_a = CommQuery.GetXmlToDataSetFromICE(req_a, "", "common_query_service", out Msg);
                if (Msg != "")
                {
                    throw new Exception(Msg);
                }
                string uidtoc = "";
                if (ds_a != null && ds_a.Tables.Count > 0 && ds_a.Tables[0].Rows.Count > 0)
                {
                    uidtoc = ds_a.Tables[0].Rows[0]["Fuidtoc"].ToString();
                }

                if (uidtoc == "")
                {
                    throw new Exception("必填参数uidtoc为空");
                }

                /**设置初始值---------start-----------**/
                if (iPageStart < 0)
                {
                    iPageStart = 0;
                }
                if (iPageMax < 1)
                {
                    iPageMax = 10;
                }

                string CFTAccount = ConfigurationManager.AppSettings["CFTAccount"];
                string wxzfAccount = ConfigurationManager.AppSettings["wxzfAccount"];
                string qzj_ip = ConfigurationManager.AppSettings["CreditPay_QZJ_IP"];
                string qzj_port = ConfigurationManager.AppSettings["CreditPay_QZJ_PORT"];
                string req = "request_type=2221&ver=1&head_u=&uid=" + uid;
                req += "&acctNo=" + acc_no + "&sp_id=" + CFTAccount + "&bank_type=" + bank_type + "&uidtoc=" + uidtoc + "&queryBegDate=" + sDate + "&queryEndDate=" + eDate;
                req += "&queryBegNum=" + iPageStart + "&queryCnt=" + iPageMax;
                Msg = ""; //重置

                string answer = commRes.GetFromRelay(req, qzj_ip, qzj_port, out Msg);

                if (answer == "")
                {
                    return null;
                }
                if (Msg != "")
                {
                    throw new Exception(Msg);
                }

                ds = CommQuery.ParseRelayPageRow(answer, out errMsg);
                if (errMsg != "")
                {
                    throw new Exception(errMsg);
                }
            }
            catch (Exception e)
            {
                throw new LogicException("service处理失败:" + e.Message);
            }

            return ds;
        }

        #endregion

        [WebMethod(Description = "查询外围订单函数")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet GetSPOrder(string spid, string spcoding)
        {
            RightAndLog rl = new RightAndLog();
            try
            {

                if (myHeader == null)
                {
                    throw new LogicException("不正确的调用方法！");
                }

                rl.actionType = "外围订单查询函数";
                rl.ID = spid;
                rl.OperID = myHeader.OperID;
                rl.sign = 1;
                rl.strRightCode = "";
                rl.RightString = myHeader.RightString;
                rl.SzKey = myHeader.SzKey;
                rl.type = "查询";
                rl.UserID = myHeader.UserName;
                rl.UserIP = myHeader.UserIP;
   
                string errmsg = "";
                string strWhere = "partner_id=" + spid + "&sp_billno=" + spcoding + "&business_type=1";
                DataSet ds = CommQuery.GetDataSetFromICE_OrderServer(strWhere, "sp_order_query_service", true, out errmsg);

                return ds; 
            }
            catch (LogicException err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                throw;
            }
            catch (Exception err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                throw new LogicException("Service处理失败！");
            }
            finally
            {
                rl.WriteLog();
            }
        }

        [WebMethod(Description = "查询活动列表函数")]
        public DataSet QueryActivityList(string act_id)
        {
            DataSet ds = null;
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("activity"));
            try
            {
                string sql = "select FActID,FActName from db_action_info.t_cft_action_info where FActID>'1300000'";//特殊处理,区分普通活动和微信活动
                if (!string.IsNullOrEmpty(act_id))
                {
                    sql += " where FActID=" + act_id;
                }
                sql += " order by FCreateTime desc";
                da.OpenConn();
                ds = da.dsGetTotalData(sql);
            }
            catch (Exception err)
            {
                throw new LogicException("查询活动列表失败" + err.Message);
            }
            finally
            {
                da.Dispose();
            }
            return ds;
        }

        [WebMethod(Description = "查询用户参加的活动函数")]
        public DataSet QueryUserJoinActivity(string cft_no, string beginTime, string endTime, string actId, int iPageStart, int iPageMax)
        {
            DataSet ds = null;
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("activity"));
            try
            {
                if (cft_no == null || cft_no == "")
                {
                    throw new LogicException("财付通账号不能为空！");
                }

                da.OpenConn();
                int len = cft_no.Length;
                string tableName = "db_action_data_" + cft_no.Substring(len - 2) + ".t_cft_action_chance_list_" + cft_no.Substring(len - 3, 1);
                string sql = "select a.*,b.FActId,b.FActName,b.FBeginDate,b.FEndDate,b.FUrl from " + tableName + " a,db_action_info.t_cft_action_info b  where a.FActId=b.FActId AND a.FChanceID IN (select MAX(FChanceID) from " + tableName + " where FUin='" + cft_no + "'";
                if (!string.IsNullOrEmpty(beginTime))
                {
                    sql += " AND FCreateTime>='" + beginTime + "'";
                }
                if (!string.IsNullOrEmpty(endTime))
                {
                    sql += " AND FCreateTime<='" + endTime + "'";
                }
                if (!string.IsNullOrEmpty(actId))
                {
                    sql += " AND FActId='" + actId + "'";
                }
                sql += " GROUP BY FActId,FUin ) LIMIT " + iPageStart + "," + iPageMax;

                ds = da.dsGetTotalData(sql);
            }
            catch (Exception err)
            {
                throw new LogicException("查询用户参加的活动失败" + err.Message);
            }
            finally
            {
                da.Dispose();
            }
            return ds;
        }

        [WebMethod(Description = "查询活动日志函数")]
        public DataSet QueryActivityLogs(string cft_no, string beginTime, string actId, int iPageStart, int iPageMax)
        {
            DataSet ds = null;
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("activity"));
            try
            {
                if (cft_no == null || cft_no == "")
                {
                    throw new LogicException("财付通账号不能为空！");
                }

                da.OpenConn();
                int len = cft_no.Length;
                string tableName = "db_action_data_" + cft_no.Substring(len - 2) + ".t_cft_action_chance_list_" + cft_no.Substring(len - 3, 1);
                string sql = "SELECT * FROM " + tableName + "  WHERE FUin= " + cft_no;
                if (!string.IsNullOrEmpty(beginTime))
                {
                    sql += " AND FCreateTime>='" + beginTime + " 00:00:00'";
                    sql += " AND FCreateTime<='" + beginTime + " 23:59:59'";
                }
                if (!string.IsNullOrEmpty(actId))
                {
                    sql += " AND FActId='" + actId + "'";
                }
                sql += " LIMIT " + iPageStart + "," + iPageMax;

                ds = da.dsGetTotalData(sql);
            }
            catch (Exception err)
            {
                throw new LogicException("查询活动日志失败" + err.Message);
            }
            finally
            {
                da.Dispose();
            }
            return ds;
        }

        [WebMethod(Description = "查询微信支付活动函数")]
        public DataSet QueryWebchatPayActivity(string cft_no, string beginTime, string endTime, string actId, int iPageStart, int iPageMax)
        {
            DataSet ds = null;
            MySqlAccess da = null;
            try
            {
                if (cft_no == null || cft_no == "")
                {
                    throw new LogicException("财付通账号/财付通订单号不能为空！");
                }
                if (string.IsNullOrEmpty(actId))
                {
                    throw new LogicException("活动ID不能为空！");
                }

                //根据actId来判断查什么活动库
                if (actId == "wxzfact")
                {
                    da = new MySqlAccess(PublicRes.GetConnString("wxzfact"));
                }
                else
                {
                    throw new LogicException("活动ID不存在！" + actId);
                }

                da.OpenConn();
                int len = cft_no.Length;
                string tableName = "db_action_wx.t_pay_and_cdkey_" + cft_no.Substring(len - 2);
                string sql = "SELECT a.*,b.FActName FROM " + tableName + " a, db_action_info.t_cft_action_info b  WHERE a.FActId=b.FActId  and a.FTransId= '" + cft_no + "'";
                if (!string.IsNullOrEmpty(beginTime))
                {
                    sql += " AND a.FCreateTime>='" + beginTime + "'";
                }
                if (!string.IsNullOrEmpty(endTime))
                {
                    sql += " AND a.FCreateTime<='" + endTime + "'";
                }

                sql += " LIMIT " + iPageStart + "," + iPageMax;
                ds = da.dsGetTotalData(sql);
            }
            catch (Exception err)
            {
                throw new LogicException("查询微信支付活动失败" + err.Message);
            }
            finally
            {
                if (da != null)
                { da.Dispose(); }
            }
            return ds;
        }

        // 2012/6/11 修改删除查询必填字段beginDate和EndDate，添加查询必填条件limStart和limCount
        [WebMethod(Description = "商户流水查询函数，可按自定义条件查询，edwardzheng20051110")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet GetMediListX(string u_ID, string Fcode, string strBeginTime, string strEndTime, string u_UserFilter,
            string u_OrderBy, int limStart, int limCount)
        {
            RightAndLog rl = new RightAndLog();
            try
            {
                if (myHeader == null)
                {
                    throw new LogicException("不正确的调用方法！");
                }


                rl.actionType = "商户流水查询函数";
                rl.ID = u_ID;
                rl.OperID = myHeader.OperID;
                rl.sign = 1;
                rl.strRightCode = "SPInfoManagement"; //"TradeLogList";
                rl.RightString = myHeader.RightString;
                rl.SzKey = myHeader.SzKey;
                rl.type = "查询";
                rl.UserID = myHeader.UserName;
                rl.UserIP = myHeader.UserIP;
          
                string strSql = "spid=" + u_ID;
                string errMsg = "";
                string fuid = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_MERCHANTINFO, "FuidMiddle", out errMsg);
                if (string.IsNullOrEmpty(fuid))
                {
                    return null;
                }
                string connstr = PublicRes.GetConnString("t_user_order_bsb", fuid.Substring(fuid.Length - 2));
                if (string.IsNullOrEmpty(connstr))
                {
                    return null;
                }
                //2015-5-11 sql转relay                
                MediListQueryClass cuser = new MediListQueryClass(u_ID, Fcode, strBeginTime, strEndTime, u_UserFilter, u_OrderBy, limStart, limCount);   
                return cuser.GetResultX_Conn(connstr);              
            }
            catch (LogicException err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                throw;
            }
            catch (Exception err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                throw new LogicException("Service处理失败！");
            }
            finally
            {
                rl.WriteLog();
            }
        }

        //该函数未被调用
        [WebMethod(Description = "商户流水查询函数")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet GetMediList(string u_ID, string Fcode, DateTime u_BeginTime, DateTime u_EndTime, int iPageStart, int iPageMax)
        {
            RightAndLog rl = new RightAndLog();
            try
            {
                if (myHeader == null)
                {
                    throw new LogicException("不正确的调用方法！");
                }

                rl.actionType = "商户流水查询函数";
                rl.ID = u_ID;       
                rl.sign = 1;
                rl.strRightCode = "GetMediList";   
                rl.type = "查询";
           		
                PublicRes.SetRightAndLog(myHeader, rl);
                if (!rl.CheckRight())
                {
                    throw new LogicException("用户无权执行此操作！");
                }

                string strSql = "spid=" + u_ID;
                string errMsg = "";
                string fuid = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_MERCHANTINFO, "FuidMiddle", out errMsg);
                string connstr = PublicRes.GetConnString("t_user_order_bsb", fuid.Substring(fuid.Length - 2));

                MediListQueryClass cuser = new MediListQueryClass(u_ID, Fcode, u_BeginTime, u_EndTime);	
                return cuser.GetResultX_Conn(iPageStart, iPageMax, connstr);
            }
            catch (LogicException err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                throw;
            }
            catch (Exception err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                throw new LogicException("Service处理失败！");
            }
            finally
            {
                rl.WriteLog();
            }
        }

        //该函数未被调用
        [WebMethod(Description = "商户流水个数函数")]
        public int GetMediListCount(string u_ID, string Fcode, DateTime u_BeginTime, DateTime u_EndTime)
        {
            try
            {
                string strSql = "spid=" + u_ID;
                string errMsg = "";
                string fuid = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_MERCHANTINFO, "FuidMiddle", out errMsg);
                string connstr = PublicRes.GetConnString("t_user_order_bsb", fuid.Substring(fuid.Length - 2));

                MediListQueryClass cuser = new MediListQueryClass(u_ID, Fcode, u_BeginTime, u_EndTime);
                return cuser.GetCount_Conn(connstr);
            }
            catch (LogicException err)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new Exception("service发生错误,请联系管理员！");
                return 0;
            }
        }

        [WebMethod(Description = "快速交易查询详细函数")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet GetQuickTradeListDetail(string listid)
        {
            //已经修改为商户流水详细页面， furion 20050817
            try
            {
                //已改动 furion V30_FURION核心查询需改动 从订单里查
                QuickTradeQueryClass cuser = new QuickTradeQueryClass(listid);
                //furion 现在加入了新的功能，要取得买家的真实姓名。
                DataSet ds = cuser.GetResultX(1, 1, "HT");

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    PublicRes.GetUserName_Table(ds.Tables[0], "Fbuy_uid", "Fbuy_name");
                }

                return ds;
            }
            catch (Exception e)
            {
                throw new Exception("service发生错误,请联系管理员！");
                return null;
            }
        }

        [WebMethod(Description = "风控解冻审核的查询")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet GetFreezeList_2(string qqid, string szBeginDate, string szEndDate, string szStatue,
            string szListID, string szFreezeUser, string szFreezeReason, int iPageStart, int iPageMax)
        {
            RightAndLog rl = new RightAndLog();
            try
            {
                if (myHeader == null)
                {
                    throw new LogicException("不正确的调用方法！");
                }

                rl.actionType = "冻结查询函数";
                rl.ID = "";
                rl.OperID = myHeader.OperID;
                rl.sign = 1;
                rl.strRightCode = "GetFreezeList";
                rl.RightString = myHeader.RightString;
                rl.SzKey = myHeader.SzKey;
                rl.type = "查询";
                rl.UserID = myHeader.UserName;
                rl.UserIP = myHeader.UserIP;
       
                FreezeQueryClass_2 cuser = new FreezeQueryClass_2(qqid, szBeginDate, szEndDate, szStatue, szListID, szFreezeUser, szFreezeReason);
                DataSet ds = cuser.GetResultX(iPageStart, iPageMax, "HT");

                return ds;
            }
            catch (LogicException err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                throw;
            }
            catch (Exception err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                throw new LogicException("Service处理失败！");
            }
            finally
            {
                rl.WriteLog();
            }
        }

        [WebMethod(Description = "风控解冻审核的查询3")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet GetFreezeList_3(string qqid, string szBeginDate, string szEndDate, int iStatue,
            string szListID, string szFreezeUser, string szFreezeReason, int iPageStart, int iPageMax, string orderType, out int AllRecordCount)
        {
            RightAndLog rl = new RightAndLog();
            try
            {
                if (myHeader == null)
                {
                    throw new LogicException("不正确的调用方法！");
                }

                rl.actionType = "冻结查询函数";
                rl.ID = "";
                rl.OperID = myHeader.OperID;
                rl.sign = 1;
                rl.strRightCode = "GetFreezeList";
                rl.RightString = myHeader.RightString;
                rl.SzKey = myHeader.SzKey;
                rl.type = "查询";
                rl.UserID = myHeader.UserName;
                rl.UserIP = myHeader.UserIP;

                CFTUserAppealClass cuser = new CFTUserAppealClass(qqid, szBeginDate, szEndDate, iStatue, 8, "", szFreezeUser, szListID, szFreezeReason, orderType);
                DataSet ds = cuser.GetResultX(iPageStart, iPageMax, "CFT");

                AllRecordCount = cuser.GetCount("CFT");

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    return null;

                ds.Tables[0].Columns.Add("FreezeReason", typeof(string));
                ds.Tables[0].Columns.Add("FreezeUser", typeof(string));
                ds.Tables[0].Columns.Add("isFreezeListHas", typeof(string));
                ds.Tables[0].Columns.Add("Fuincolor", typeof(string));

                long Appeal_FreezeMoney = long.Parse(System.Configuration.ConfigurationManager.AppSettings["Appeal_FreezeMoney"]);

                ICEAccess ice = new ICEAccess(PublicRes.ICEServerIP, PublicRes.ICEPort);
                try
                {
                    ice.OpenConn();
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        try
                        {
                            FreezeQueryClass cuser2 = new FreezeQueryClass(dr["Fuin"].ToString(), 1);

                            DataSet ds2 = cuser2.GetResultX(0, 1, "HT");

                            if (ds2 != null && ds2.Tables.Count != 0 && ds2.Tables[0].Rows.Count != 0)
                            {
                                dr["FreezeReason"] = ds2.Tables[0].Rows[0]["FFreezeReason"].ToString();
                                dr["FreezeUser"] = ds2.Tables[0].Rows[0]["FHandleUserID"].ToString();
                                dr["isFreezeListHas"] = "1";
                            }
                            else
                            {
                                dr["isFreezeListHas"] = "0";
                            }

                            dr["Fuincolor"] = "";
                            string fuid = PublicRes.ConvertToFuid(dr["Fuin"].ToString());
                            string strwhere = "where=" + ICEAccess.URLEncode("fuid=" + fuid + "&");
                            strwhere += ICEAccess.URLEncode("fcurtype=1&");
                            string strResp = "";

                            DataTable dtuser = ice.InvokeQuery_GetDataTable(YWSourceType.用户资源, YWCommandCode.查询用户信息, fuid, strwhere, out strResp);

                            if (dtuser == null || dtuser.Rows.Count == 0)
                            {
                                continue;
                            }

                            long lbalance = long.Parse(dtuser.Rows[0]["fbalance"].ToString());

                            if (lbalance >= Appeal_FreezeMoney)
                            {
                                dr["Fuincolor"] = "BIGMONEY";
                            }
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    ice.CloseConn();
                }
                finally
                {
                    ice.Dispose();
                }

                return ds;
            }
            catch (LogicException err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                throw;
            }
            catch (Exception err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                throw new LogicException("Service处理失败！" + err.Message);
            }
            finally
            {
                rl.WriteLog();
            }
        }

        [WebMethod(Description = "风控解冻审核的查询NEW")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet GetFreezeList_New(string qqid, string szBeginDate, string szEndDate, int ftype, int iStatue,
            string szListID, string szFreezeUser, string szFreezeReason, int iPageStart, int iPageMax, string orderType, string freeze_channel, out int AllRecordCount)
        {
            RightAndLog rl = new RightAndLog();
            try
            {
                if (myHeader == null)
                {
                    throw new LogicException("不正确的调用方法！");
                }

                rl.actionType = "冻结查询函数";
                rl.ID = "";
                rl.OperID = myHeader.OperID;
                rl.sign = 1;
                rl.strRightCode = "GetFreezeList";
                rl.RightString = myHeader.RightString;
                rl.SzKey = myHeader.SzKey;
                rl.type = "查询";
                rl.UserID = myHeader.UserName;
                rl.UserIP = myHeader.UserIP;
        
               // AllRecordCount = 0;
                //DataSet ds = null;

                #region 查询申诉记录列表
                DateTime sdate = DateTime.Parse(szBeginDate);
                DateTime edate = DateTime.Parse(szEndDate);

                Func<DateTime, DateTime, Tuple<DataSet, int>> QueryDBHendle = (s, e) =>
                {
                    string table = string.Format("db_appeal_{0}.t_tenpay_appeal_trans_{1}", s.Year.ToString(), s.Month.ToString("d2"));
                    CFTUserAppealClass cuser = new CFTUserAppealClass(qqid, s.ToString("yyyy-MM-dd"), e.ToString("yyyy-MM-dd"), iStatue, ftype, "", szFreezeUser, szListID, szFreezeReason, orderType, freeze_channel, table);
                    var ds1 = cuser.GetResultX(iPageStart, iPageMax, "fkdj");
                    var count = cuser.GetCount("fkdj");
                    return new Tuple<DataSet, int>(ds1, count);
                };

                var tuple1 = QueryDBHendle(sdate, edate);       // 开始日期      - 结束日期    : 只查询开始日记当月记录
                DataSet ds = tuple1.Item1;
                AllRecordCount = tuple1.Item2;

                if (sdate.Month != edate.Month) //如果跨月 就查询一下结束日期那个库表
                {
                    //如果月份不一致，需要查询2个表 例如：20140110--20140210 则查20140110-20140131;20140201-20140210两个时间段

                    DateTime e_sdate = edate.AddDays(1 - edate.Day);    //  结束日期一号
                    var tuple2 = QueryDBHendle(e_sdate, edate);         //  结束日期一号 - 结束日期
                    AllRecordCount += tuple2.Item2;
                   
                    var ds2 = tuple2.Item1;
                    if (ds2 != null && ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
                    {
                        if (ds != null && ds.Tables.Count > 0)
                        {
                            foreach (DataRow dr in ds2.Tables[0].Rows)
                            {
                                ds.Tables[0].ImportRow(dr);//将记录加入到一个表里
                            }
                        }
                        else
                        {
                            ds = new DataSet();
                            ds.Tables.Add(ds2.Tables[0].Copy());
                        }
                    }
                }
                #endregion

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    return null;

                ds.Tables[0].Columns.Add("FreezeReason", typeof(string));
                ds.Tables[0].Columns.Add("FreezeUser", typeof(string));
                ds.Tables[0].Columns.Add("isFreezeListHas", typeof(string));
                ds.Tables[0].Columns.Add("Fuincolor", typeof(string));

                long Appeal_FreezeMoney = long.Parse(System.Configuration.ConfigurationManager.AppSettings["Appeal_FreezeMoney"]);

                ICEAccess ice = new ICEAccess(PublicRes.ICEServerIP, PublicRes.ICEPort);
                try
                {
                    ice.OpenConn();
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        try
                        {
                            #region 查询冻结日志信息
                            if (ftype == 8 || ftype == 19)
                            {
                                FreezeQueryClass cuser2 = new FreezeQueryClass(dr["Fuin"].ToString(), 1);

                                DataSet ds2 = cuser2.GetResultX(0, 1, "HT");

                                if (ds2 != null && ds2.Tables.Count != 0 && ds2.Tables[0].Rows.Count != 0)
                                {
                                    dr["FreezeReason"] = ds2.Tables[0].Rows[0]["FFreezeReason"].ToString();
                                    dr["FreezeUser"] = ds2.Tables[0].Rows[0]["FHandleUserID"].ToString();
                                    dr["isFreezeListHas"] = "1";
                                }
                                else
                                {
                                    dr["isFreezeListHas"] = "0";
                                }
                            }
                            #endregion

                            #region 添加大额标记
                            dr["Fuincolor"] = "";
                            string fuid = PublicRes.ConvertToFuid(dr["Fuin"].ToString());
                            string strwhere = "where=" + ICEAccess.URLEncode("fuid=" + fuid + "&");
                            strwhere += ICEAccess.URLEncode("fcurtype=1&");
                            string strResp = "";

                            DataTable dtuser = ice.InvokeQuery_GetDataTable(YWSourceType.用户资源, YWCommandCode.查询用户信息, fuid, strwhere, out strResp);

                            if (dtuser == null || dtuser.Rows.Count == 0)
                            {
                                continue;
                            }

                            long lbalance = long.Parse(dtuser.Rows[0]["fbalance"].ToString());

                            if (lbalance >= Appeal_FreezeMoney)
                            {
                                dr["Fuincolor"] = "BIGMONEY";
                            }
                            #endregion
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    ice.CloseConn();
                }
                finally
                {
                    ice.Dispose();
                }

                return ds;
            }
            catch (LogicException err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                throw;
            }
            catch (Exception err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                throw new LogicException("Service处理失败！" + err.Message);
            }
            finally
            {
                rl.WriteLog();
            }
        }

        [WebMethod(Description = "风控解冻审核的查询冻结单详细资料")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet GetFreezeListDetail_2(string fid)
        {
            try
            {
                FreezeQueryClass cuser = new FreezeQueryClass(fid);

                DataSet ds = cuser.GetResultX(1, 1, "HT");

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    return null;

                DataRow dr = ds.Tables[0].Rows[0];

                string memo = dr["FFreezeMemo"].ToString();

                DataSet ds2 = new DataSet();
                ds2.Tables.Add(new DataTable());
                ds2.Tables[0].Columns.Add("SecNo", typeof(string));
                ds2.Tables[0].Columns.Add("HandleResult", typeof(string));

                string[] strMemo = memo.Split(new char[] { '|' });

                for (int i = 0; i < strMemo.Length; i++)
                {
                    ds2.Tables[0].Rows.Add(new object[] { i.ToString(), strMemo[i] });
                }

                return ds2;
            }
            catch (Exception e)
            {
                throw new Exception("service发生错误,请联系管理员！");
                return null;
            }
        }

        [WebMethod(Description = "查询风控冻结处理日志")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet GetFreezeDiary(string fid, string ffreezeID, string handleType, string handleUser,
            string handleResult, string memo, string strBeginDate, string strEndDate, int startIndex, int maxPage)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("HT"));

            string tableName = "c2c_fmdb.t_Freeze_Detail";

            string cmd = "select * from " + tableName + " where (1=1) ";
            if (fid.Trim() != "")
                cmd += " and FID=" + fid;

            if (ffreezeID.Trim() != "")
                cmd += " and FFreezeListID=" + ffreezeID;

            if (handleType.Trim() != "")
                cmd += " and FHandleType in(" + handleType + ")";

            if (handleUser.Trim() != "")
                cmd += " and FHandleUser like '%" + handleUser + "%'";

            if (handleResult.Trim() != "")
                cmd += " and FHandleResult like '%" + handleResult + "%'";

            if (strBeginDate.Trim() != "")
                cmd += " and FCreateDate >='" + strBeginDate.Trim() + "'";

            if (strEndDate.Trim() != "")
                cmd += " and FCreateDate<='" + strEndDate.Trim() + "'";

            if (memo.Trim() != "")
            { }

            cmd += " order by FCreateDate DESC ";

            DataSet ds = new DataSet();

            if (startIndex != -1)
            {
                ds = da.dsGetTableByRange(cmd, startIndex, maxPage);
            }
            else
            {
                ds = da.dsGetTotalData(cmd);
            }

            return ds;
        }


        // 暂时不用
        [WebMethod(Description = "创建风控冻结处理日志")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public bool CreateFreezeDiary(string ffreezeListID, int handleType, string handleUser, string handleResult
            , string memo, string freezedUserName, string emailTo)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("HT"));

            da.OpenConn();
            string tableName = "c2c_fmdb.t_Freeze_Detail";

            DataSet ds = GetFreezeDiary("", ffreezeListID, "", "", "", "", "", "", 1, 1);

            // 记录结单状态和结单人员
            int srcHandleType = 0;
            string srcHandleUser = "";

            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {

            }
            else
            {
                srcHandleType = int.Parse(ds.Tables[0].Rows[0]["FSourceType"].ToString());

                // 结单的日志只允许补充处理结果
                if (srcHandleType == 1 || srcHandleType == 2)
                {
                    if (handleType != 100)
                    {
                        return false;
                    }
                }

                // 作废的日志就不许再操作了
                if (srcHandleType == 3)
                    return false;
            }

            if (handleType == 1 || handleType == 2 || handleType == 3)
            {
                srcHandleType = handleType;
            }

            if (srcHandleType == 1 || srcHandleType == 2 || srcHandleType == 3)
            {
                if (handleType == 1 || handleType == 2 || handleType == 3)
                {
                    srcHandleUser = handleUser;
                }
                else
                {
                    srcHandleUser = ds.Tables[0].Rows[0]["FField3"].ToString();
                }
            }

            if (handleResult.Trim() != "")
            {
                handleResult = PublicRes.replaceMStr(handleResult);
            }


            // 结单和作废操作都必须将FSourceType设置成HandleType，而遇到日志的FSourceType为结单或作废状态的，
            // 在创建新日志的时候都要将SourceType写回
            string sqlCmd = "insert " + tableName + " (FFreezeListID,FSourceType,FCreateDate,FHandleType,FHandleUser,FHandleResult,FMemo,FField3) values ("
                + ffreezeListID + "," + srcHandleType + ",'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," + handleType + ",'"
                + handleUser + "','" + handleResult + "','" + memo + "','" + srcHandleUser + "')";

            // 存储到数据库之后，将handleResult的换行符转换成网页的格式
            handleResult = handleResult.Replace("\n", "<br>");
            handleResult = handleResult.Replace("\r", "<br>");

            if (da.ExecSql(sqlCmd))
            {
                if (handleType == 1 || handleType == 2 || handleType == 3)
                {
                    string sqlCmd2 = "update " + tableName + " set FSourceType=" + handleType + ",FField3='" + handleUser
                        + "' where FFreezeListID=" + ffreezeListID;

                    if (!da.ExecSql(sqlCmd2))
                    {
                        da.RollBack();
                        return false;
                    }
                }

                // 成功更新数据库，则检查是否结单操作并发送邮件

                if (handleType == 1 || handleType == 2)
                {
                    string msg = "";
              
                    try
                    {
                        string str_params = "p_name=" + freezedUserName + "&p_parm1=" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "&p_parm2=" + handleResult + "&p_parm3=" + "" + "&p_parm4=" + "";
                        TENCENT.OSS.C2C.Finance.Common.CommLib.CommMailSend.SendMsg(emailTo, "2033", str_params);
                    }
                    catch (Exception ex)
                    {
                        // 发送邮件失败
                        da.RollBack();
                        return false;
                    }

                }

                return true;
            }
            else
            {
                da.RollBack();
                return false;
            }

            return false;
        }

        [WebMethod(Description = "创建风控冻结处理日志2")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public bool CreateFreezeDiary_2(string ffreezeListID, int handleType, string handleUser, string handleResult
            , string memo, string freezedUserName, string emailTo)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("HT"));
            da.OpenConn();
            string tableName = "c2c_fmdb.t_Freeze_Detail";

            CFTUserAppealClass cuser = new CFTUserAppealClass(int.Parse(ffreezeListID));
            DataSet ds = cuser.GetResultX(0, 1, "CFT");

            // 记录结单状态和结单人员
            int srcHandleType = 0;
            string srcHandleUser = "";
            string freezeSubmitTime = ds.Tables[0].Rows[0]["fsubmittime"].ToString();

            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                return false;
            }
            else
            {
                srcHandleType = int.Parse(ds.Tables[0].Rows[0]["FState"].ToString());
                srcHandleUser = ds.Tables[0].Rows[0]["FCheckUser"].ToString();

                // 结单的日志只允许补充处理结果
                if (srcHandleType == 1 || srcHandleType == 2)
                {
                    if (handleType != 100)
                    {
                        return false;
                    }
                }

                // 作废的日志就不许再操作了
                if (srcHandleType == 7)
                    return false;
            }

            if (handleResult.Trim() != "")
            {
                handleResult = PublicRes.replaceMStr(handleResult);
            }

            if (handleType != 100)
            {
                MySqlAccess da_2 = new MySqlAccess(PublicRes.GetConnString("CFT"));

                da_2.OpenConn();

                string sqlCmd_updateAppeal = "update db_appeal.t_tenpay_appeal_trans set FState=" + handleType
                    + ",Fcomment='风控冻结." + memo + "', FCheckUser='" + handleUser + "',FCheckTime=Now(),"
                    + " FPickTime=now(),FPickUser='" + handleUser + "',"
                    + " FReCheckTime=now(),FRecheckUser='" + handleUser + "'"
                    + " where Fid=" + ffreezeListID;

                try
                {
                    if (!da_2.ExecSql(sqlCmd_updateAppeal))
                    {
                        return false;
                    }
                }
                catch (System.Exception ex)
                {
                    // 记录失败日志
                    return false;
                }
            }

            string sqlCmd = "insert " + tableName + " (FFreezeListID,FCreateDate,FHandleType,FHandleUser,FHandleResult,FMemo) values ("
                + ffreezeListID + ",'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," + handleType + ",'"
                + handleUser + "','" + handleResult + "','" + memo + "')";

            // 存储到数据库之后，将handleResult的换行符转换成网页的格式
            handleResult = handleResult.Replace("\n", "<br>");
            handleResult = handleResult.Replace("\r", "<br>");

            if (da.ExecSql(sqlCmd))
            {
                // 成功更新数据库，则检查是否结单操作并发送邮件

                if (handleType == 1 || handleType == 2)
                {
                    string msg = "";            
                    try
                    {                
                        string str_params = "p_name=" + freezedUserName + "&p_parm1=" + freezeSubmitTime + "&p_parm2=" + handleResult + "&p_parm3=" + "" + "&p_parm4=" + "";
                        TENCENT.OSS.C2C.Finance.Common.CommLib.CommMailSend.SendMsg(emailTo, "2033", str_params);
                    }
                    catch (Exception ex)
                    {
                        // 发送邮件失败
                        //da.RollBack();
                        throw new Exception("发送邮件失败：" + ex.Message);
                        return false;
                    }

                }

                return true;
            }
            else
            {
                da.RollBack();
                return false;
            }

            return false;
        }

        [WebMethod(Description = "创建风控冻结处理日志NEW")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public bool CreateFreezeDiary_NEW(string ffreezeListID, int handleType, string handleUser, string handleResult
            , string memo, string uin, string userPhone, string submitDate, int bt, string userDesc, string zdyBt1, string zdyBt2, string zdyBt3
            , string zdyBt4, string zdyCont1, string zdyCont2, string zdyCont3, string zdyCont4)
        {

            string tableName = "c2c_fmdb.t_Freeze_Detail";

            DateTime date = DateTime.Parse(submitDate);
            int i_m = date.Month;
            string s_m = "";
            if (i_m < 10)
            {
                s_m = "0" + i_m;
            }
            else
            {
                s_m = i_m.ToString();
            }
            string table = "db_appeal_" + date.Year.ToString() + ".t_tenpay_appeal_trans_" + s_m;
            CFTUserAppealClass cuser = new CFTUserAppealClass(ffreezeListID, table);
            DataSet ds = cuser.GetResultX(0, 1, "fkdj");

            // 记录结单状态和结单人员
            int srcHandleType = 0;
            string srcHandleUser = "";
            int reqType = 8; //处理8,19类型的记录
            //string freezeSubmitTime = ds.Tables[0].Rows[0]["fsubmittime"].ToString();

            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                //记录不存在，再查询一下上个月记录 yinhuang
                DateTime d2 = date.AddMonths(-1);
                i_m = d2.Month;
                if (i_m < 10)
                {
                    s_m = "0" + i_m;
                }
                else
                {
                    s_m = i_m.ToString();
                }
                table = "db_appeal_" + d2.Year.ToString() + ".t_tenpay_appeal_trans_" + s_m;
                CFTUserAppealClass cuser2 = new CFTUserAppealClass(ffreezeListID, table);
                ds = cuser2.GetResultX(0, 1, "fkdj");
            }

            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                throw new Exception("记录不存在" + ffreezeListID);
                return false;
            }
            else
            {
                srcHandleType = int.Parse(ds.Tables[0].Rows[0]["FState"].ToString());
                srcHandleUser = ds.Tables[0].Rows[0]["FCheckUser"].ToString();
                reqType = int.Parse(ds.Tables[0].Rows[0]["FType"].ToString());

                if (reqType == 8 || reqType == 19)
                {
                    // 结单的日志只允许补充处理结果
                    if (srcHandleType == 1 || srcHandleType == 2)
                    {
                        if (handleType != 7)//待补充资料可以作废处理
                        {
                            return false;
                        }
                    }

                    // 作废的日志就不许再操作了
                    if (srcHandleType == 7)
                        return false;
                }
                else if (reqType == 11)//特殊找回密码
                {
                    if (!(srcHandleType == 0 || srcHandleType == 12))//除了未处理、已补充资料状态外，不能补充资料操作
                        return false;
                }
            }

            if (handleResult.Trim() != "")
            {
                handleResult = PublicRes.replaceMStr(handleResult);
            }

            if (handleType != 100)
            {
                MySqlAccess da_2 = new MySqlAccess(PublicRes.GetConnString("fkdj"));

                da_2.OpenConn();

                if (reqType == 8 || reqType == 19)
                    memo = "风控冻结." + memo.Replace("风控冻结.","");
                else if (reqType == 11)
                    memo = "特殊找回密码." + memo.Replace("风控冻结.", "");

                string sqlCmd_updateAppeal = "update " + table + " set FState=" + handleType
                    + ",Fcomment='" + memo + "', FCheckUser='" + handleUser + "',FCheckTime=Now(),"
                    + " FPickTime=now(),FPickUser='" + handleUser + "',FStandBy1=" + bt
                    + " ,FReCheckTime=now(),FRecheckUser='" + handleUser + "',FCheckInfo='" + handleResult + "',Fsup_desc1='" + zdyBt1
                    + "',Fsup_desc2='" + zdyBt2 + "',Fsup_desc3='" + zdyBt3 + "',Fsup_desc4='" + zdyBt4 + "',Fsup_tips1='" + zdyCont1
                    + "',Fsup_tips2='" + zdyCont2 + "',Fsup_tips3='" + zdyCont3 + "',Fsup_tips4='" + zdyCont4 + "' "
                    + " where Fid='" + ffreezeListID + "'";

                try
                {
                    if (!da_2.ExecSql(sqlCmd_updateAppeal))
                    {
                        return false;
                    }
                }
                catch (System.Exception ex)
                {
                    // 记录失败日志
                    throw new Exception("更新表错误" + ex.Message);
                    return false;
                }
            }

            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("HT"));

            string sqlCmd = "insert into " + tableName + " (FFreezeListID,FCreateDate,FHandleType,FHandleUser,FHandleResult,FMemo) values ("
                + ffreezeListID + ",'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," + handleType + ",'"
                + handleUser + "','" + handleResult + "','" + userDesc + "')";

            // 存储到数据库之后，将handleResult的换行符转换成网页的格式
            //handleResult = handleResult.Replace("\n", "<br>");
            //handleResult = handleResult.Replace("\r", "<br>");

            try
            {
                da.OpenConn();
                if (da.ExecSql(sqlCmd))
                {
                    // 成功更新数据库，则检查是否结单操作并发送邮件
                    if (reqType == 8 || reqType == 19)
                    {
                        if (handleType == 1)
                        {
                            //结单解冻
                            if (reqType == 19)
                            {
                                //发微信解冻消息
                                if (uin.IndexOf("@wx.tenpay.com") > 0)
                                {
                                    string reqsource = "bus_kf_unfreeze";
                                    string accid = uin.Substring(0, uin.IndexOf("@wx.tenpay.com"));
                                    string templateid = "DeNkYEfSBW7mVQET6QHwnilGWvG8cLssLSyRH0CSDk0";
                                    string cont1 = "你的微信支付账户已排除了安全风险并由保护模式切换至正常模式。";
                                    string cont2 = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                                    string cont3 = "请点击详情查看微信支付安全保障介绍";
                                    string msgtype = "unfreeze";

                                    new FreezeService().SendWechatMsg(reqsource, accid, templateid, cont1, cont2, cont3, msgtype);
                                }
                            }
                            else
                            {
                                string str_params = "http://action.tenpay.com/cuifei/2014/fengkong/unfreeze_suc.shtml?clientuin=$UIN$&clientkey=$KEY$";
                                str_params = "url=" + System.Web.HttpUtility.UrlEncode(str_params, System.Text.Encoding.GetEncoding("gb2312"));
                                TENCENT.OSS.C2C.Finance.Common.CommLib.CommMailSend.SendMsgQQTips(uin, "2236", str_params);
                                TENCENT.OSS.C2C.Finance.Common.CommLib.CommMailSend.SendMessage(userPhone, "2236", "userid=" + uin);
                            }
                        }
                        else if (handleType == 2)
                        {
                            //补充资料
                            if (reqType == 19)
                            {
                                //发微信补填资料消息
                                if (uin.IndexOf("@wx.tenpay.com") > 0)
                                {
                                    string reqsource = "bus_kf_supple";
                                    string accid = uin.Substring(0, uin.IndexOf("@wx.tenpay.com"));
                                    string templateid = "p7DifLpETQvbtDPtRPDSI6x4ufUtnjZXcb6LpVIbZ70";
                                    string cont1 = "你的微信支付账户仍处于保护模式，请点击详情补充恢复微信支付账户所需资料。";
                                    string cont2 = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                                    string cont3 = "";
                                    string msgtype = "supple";

                                    new FreezeService().SendWechatMsg(reqsource, accid, templateid, cont1, cont2, cont3, msgtype);
                                }
                            }
                            else
                            {
                                string str_params = "http://action.tenpay.com/cuifei/2014/fengkong/unfreeze_fail.shtml?clientuin=$UIN$&clientkey=$KEY$";
                                str_params = "url=" + System.Web.HttpUtility.UrlEncode(str_params, System.Text.Encoding.GetEncoding("gb2312"));
                                TENCENT.OSS.C2C.Finance.Common.CommLib.CommMailSend.SendMsgQQTips(uin, "2237", str_params);
                                TENCENT.OSS.C2C.Finance.Common.CommLib.CommMailSend.SendMessage(userPhone, "2237", "user=" + uin);
                            }
                        }
                    }
                    else if (reqType == 11)//特殊找回密码 发tips和短信 模板位申请，需更改下面代码
                    {
                        string str_params = "www.tenpay.com/v2/cs/";
                        str_params = "url=" + System.Web.HttpUtility.UrlEncode(str_params, System.Text.Encoding.GetEncoding("gb2312"));
                        //uin = "466678748";
                        //userPhone = "18718489269";
                        TENCENT.OSS.C2C.Finance.Common.CommLib.CommMailSend.SendMsgQQTips(uin, "2429", str_params);
                        TENCENT.OSS.C2C.Finance.Common.CommLib.CommMailSend.SendMessage(userPhone, "2429", str_params);
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("添加日志失败：" + ex.Message);
            }
            finally
            {
                if (da != null)
                {
                    da.Dispose();
                }
            }

            return true;
        }

        [WebMethod(Description = "每日自动处理冻结状态任务")]
        public bool AutoProcessFreezeStateDaily()
        {
            MySqlAccess da = null;
            MySqlAccess da2 = null;

            try
            {
                string sql1 = "";
                string sql2 = "";
                DateTime date = DateTime.Now.AddDays(-1); //当前日期的前一天
                //根据月份去查询表数据
                int i_m = date.Month;
                int i_y = date.Year;
                string s_m = "";
                string strWhere = " where FType=8 AND FState IN(0,2,10)"; //未处理、待补充、已补充
                string table1 = ""; //要更新的表
                string table2 = ""; //要更新的表

                if (i_m == 1)
                {
                    if (i_y > 2014)
                    {
                        //如果是1月份，则处理上年12月份
                        table1 = "db_appeal_" + (date.Year - 1).ToString() + ".t_tenpay_appeal_trans_12 ";
                        sql1 = "select * from " + table1 + strWhere;
                    }

                    if (i_m < 10)
                    {
                        s_m = "0" + i_m;
                    }
                    else
                    {
                        s_m = i_m.ToString();
                    }
                    table2 = "db_appeal_" + date.Year.ToString() + ".t_tenpay_appeal_trans_" + s_m;
                    sql2 = "select * from " + table2 + strWhere;
                }
                else
                {
                    int i_preMonth = i_m - 1;
                    if (i_preMonth < 10)
                    {
                        s_m = "0" + i_preMonth;
                    }
                    else
                    {
                        s_m = i_preMonth.ToString();
                    }

                    table1 = "db_appeal_" + date.Year.ToString() + ".t_tenpay_appeal_trans_" + s_m;
                    sql1 = "select * from " + table1 + strWhere;

                    if (i_m < 10)
                    {
                        s_m = "0" + i_m;
                    }
                    else
                    {
                        s_m = i_m.ToString();
                    }
                    table2 = "db_appeal_" + date.Year.ToString() + ".t_tenpay_appeal_trans_" + s_m;
                    sql2 = "select * from " + table2 + strWhere;
                }

                DataSet ds = null;
                DataSet ds2 = null;
                da = new MySqlAccess(PublicRes.GetConnString("fkdj"));

                da.OpenConn();
                if (sql1 != "")
                {
                    ds = da.dsGetTotalData(sql1);
                }

                //查询t_freeze_list
                da2 = new MySqlAccess(PublicRes.GetConnString("HT"));

                da2.OpenConn();
                //查询冻结表中状态为处理完成的
                string ht_sql = "select * from c2c_fmdb.t_freeze_list  where FHandleFinish=9 AND  Fid='";

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {

                        string tmp_sql = ht_sql + dr["Fid"].ToString() + "'";
                        ds2 = da2.dsGetTotalData(tmp_sql);
                        if (ds2 != null && ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
                        {
                            //如果存在记录，要更新申诉单状态为7
                            if (table1 != "")
                            {
                                string up_sql = "update" + table1 + " set FState=7 where Fid='" + dr["Fid"].ToString() + "'";
                                try
                                {
                                    da.ExecSql(up_sql);
                                }
                                catch (Exception err)
                                {

                                }
                            }
                        }
                    }
                }
                if (sql2 != "")
                {
                    ds = da.dsGetTotalData(sql2);
                }
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {

                        string tmp_sql = ht_sql + dr["Fid"].ToString() + "'";
                        ds2 = da2.dsGetTotalData(tmp_sql);
                        if (ds2 != null && ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
                        {
                            //如果存在记录，要更新申诉单状态为7
                            if (table2 != "")
                            {
                                string up_sql = "update" + table2 + " set FState=7 where Fid='" + dr["Fid"].ToString() + "'";
                                try
                                {
                                    da.ExecSql(up_sql);
                                }
                                catch (Exception err)
                                {

                                }
                            }
                        }
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                throw new LogicException("Service处理失败" + e.Message);
            }
            finally
            {
                if (da != null)
                {
                    da.Dispose();
                }
                if (da2 != null)
                {
                    da2.Dispose();
                }
            }
        }

        // 暂时不用
        [WebMethod(Description = "更新风控冻结日志")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public bool UpdateFreezeDiary(string fid, string updateType, string handleUser, string handleResult)
        {         
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("HT"));

            da.OpenConn();

            string tableName = "c2c_fmdb.t_Freeze_Detail";

            //100 为添加处理结果
            if (updateType == "100")
            {
                string sqlQueryCmd = "select * from " + tableName + "where FID=" + fid;

                DataSet ds = da.dsGetTableByRange(sqlQueryCmd, 0, 1);

                if (ds == null)
                {
                    throw new Exception("目标日志不存在");
                }

                string newHandleResult = ds.Tables[0].Rows[0]["FHandleResult"].ToString() + " 补充处理结果：" + handleResult;

                string sqlCmd = "update " + tableName + " set FHandleDate='"
                    + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',FHandleResult='" + newHandleResult
                    + "',FHandleUser='" + handleUser + "' where FFreezeListID=" + fid;

                return da.ExecSql(sqlCmd);
            }
            else
            {
                string sqlCmd = "update " + tableName + " set FHandleDate='"
                    + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',FHandleResult='" + handleResult
                    + "',FHandleUser='" + handleUser + "' where FFreezeListID=" + fid;

                return da.ExecSql(sqlCmd);
            }

        }

        [WebMethod(Description = "冻结查询函数")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet GetFreezeList(DateTime u_BeginTime, DateTime u_EndTime, string freezeuser,
            string username, int handletype, int statetype, string qqid, int iPageStart, int iPageMax)
        {
            RightAndLog rl = new RightAndLog();
            try
            {
                if (myHeader == null)
                {
                    throw new LogicException("不正确的调用方法！");
                }

                rl.actionType = "冻结查询函数";
                rl.ID = "";
                rl.OperID = myHeader.OperID;
                rl.sign = 1;
                rl.strRightCode = "GetFreezeList";
                rl.RightString = myHeader.RightString;
                rl.SzKey = myHeader.SzKey;
                rl.type = "查询";
                rl.UserID = myHeader.UserName;
                rl.UserIP = myHeader.UserIP;

                FreezeQueryClass cuser = new FreezeQueryClass(u_BeginTime, u_EndTime, freezeuser, username, handletype, statetype, qqid);
                DataSet ds = cuser.GetResultX(iPageStart, iPageMax, "HT");

                return ds;

            }
            catch (LogicException err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                throw;
            }
            catch (Exception err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                throw new LogicException("Service处理失败！");
            }
            finally
            {
                rl.WriteLog();
            }
        }

        [WebMethod(Description = "冻结处理查询函数")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public FreezeInfo GetExistFreeze(string freezeid, int FFreezeType)
        {
            RightAndLog rl = new RightAndLog();
            try
            {
                if (myHeader == null)
                {
                    throw new LogicException("不正确的调用方法！");
                }

                rl.actionType = "冻结处理查询函数";
                rl.ID = freezeid;
                rl.OperID = myHeader.OperID;
                rl.sign = 1;
                rl.strRightCode = "GetExistFreeze";
                rl.RightString = myHeader.RightString;
                rl.SzKey = myHeader.SzKey;
                rl.type = "查询";
                rl.UserID = myHeader.UserName;
                rl.UserIP = myHeader.UserIP;

                return FreezeInfo.GetExistFreeze(freezeid, FFreezeType);

            }
            catch (LogicException err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                throw;
            }
            catch (Exception err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                throw new LogicException("Service处理失败！");
            }
            finally
            {
                rl.WriteLog();
            }
        }

        [WebMethod(Description = "创建冻结函数")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public void CreateNewFreeze(FreezeInfo fi)
        {
            RightAndLog rl = new RightAndLog();
            try
            {
                if (myHeader == null)
                {
                    throw new LogicException("不正确的调用方法！");
                }

                rl.actionType = "创建冻结函数";
                rl.ID = fi.FFreezeID;
                rl.OperID = myHeader.OperID;
                rl.sign = 1;
                rl.strRightCode = "CreateNewFreeze";
                rl.RightString = myHeader.RightString;
                rl.SzKey = myHeader.SzKey;
                rl.type = "查询";
                rl.UserID = myHeader.UserName;
                rl.UserIP = myHeader.UserIP;

                FreezeInfo.CreateNewFreeze(myHeader, fi);
            }
            catch (LogicException err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                throw;
            }
            catch (Exception err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                throw new LogicException("Service处理失败！");
            }
            finally
            {
                rl.WriteLog();
            }
        }

        [WebMethod(Description = "处理冻结函数")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public void UpdateFreezeInfo(FreezeInfo fi)
        {
            RightAndLog rl = new RightAndLog();
            try
            {
                if (myHeader == null)
                {
                    throw new LogicException("不正确的调用方法！");
                }

                rl.actionType = "处理冻结函数";
                rl.ID = fi.fid;
                rl.OperID = myHeader.OperID;
                rl.sign = 1;
                rl.strRightCode = "UpdateFreezeInfo";
                rl.RightString = myHeader.RightString;
                rl.SzKey = myHeader.SzKey;
                rl.type = "查询";
                rl.UserID = myHeader.UserName;
                rl.UserIP = myHeader.UserIP;

                FreezeInfo.UpdateFreezeInfo(myHeader, fi);
            }
            catch (LogicException err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                throw;
            }
            catch (Exception err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                throw new LogicException("Service处理失败！");
            }
            finally
            {
                rl.WriteLog();
            }
        }

        [WebMethod(Description = "冻结查询个数函数")]
        public int GetFreezeListCount(DateTime u_BeginTime, DateTime u_EndTime, string freezeuser,
            string username, int handletype, int statetype, string qqid)
        {
            try
            {
                FreezeQueryClass cuser = new FreezeQueryClass(u_BeginTime, u_EndTime, freezeuser, username, handletype, statetype, qqid);
                return cuser.GetCount("HT");
            }
            catch (Exception e)
            {
                throw new Exception("service发生错误,请联系管理员！");
                return 0;
            }
        }

        [WebMethod(Description = "冻结查询详细函数")]
        public DataSet GetFreezeListDetail(string tdeid)
        {
            try
            {
                FreezeQueryClass cuser = new FreezeQueryClass(tdeid);
                DataSet ds = cuser.GetResultX(1, 1, "HT");

                return ds;
            }
            catch (Exception e)
            {
                throw new Exception("service发生错误,请联系管理员！");
                return null;
            }
        }

        [WebMethod(Description = "更新冻结备注函数")]
        public void UpdateFreezeListDetail(string fid, string FreezeReason, string UserName, string UserIP)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("HT"));
            try
            {
                da.OpenConn();
                string sql = "update c2c_fmdb.t_freeze_list set FFreezeReason = '" + FreezeReason + "',FHandleUserID='" + UserName + "',FHandleUserIP='" + UserIP + "',FHandleTime=now() where  Fid='" + fid + "'";
                da.ExecSql(sql);
            }
            catch (Exception e)
            {
                throw new Exception("service发生错误,请联系管理员！");
            }
            finally
            {
                da.Dispose();
            }
        }

        // 2012/3/30
        #region		2012/3   ----  2012/6

        [WebMethod(Description = "用户冻结记录查询")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet QueryUserFreezeRecord(string strBeginDate, string strEndDate, string fpayAccount, double freezeFin, string flistID, int iPageStart, int iPageMax)
        {
            RightAndLog rl = new RightAndLog();
            try
            {
                if (myHeader == null)
                {
                    throw new LogicException("不正确的调用方法！");
                }

                rl.actionType = "用户冻结记录查询";
                rl.ID = fpayAccount;
                rl.OperID = myHeader.OperID;
                rl.sign = 1;
                rl.strRightCode = "";
                rl.RightString = myHeader.RightString;
                rl.SzKey = myHeader.SzKey;
                rl.type = "查询";
                rl.UserID = myHeader.UserName;
                rl.UserIP = myHeader.UserIP;
        
                FreezeFinQueryClass fqc = new FreezeFinQueryClass(strBeginDate, strEndDate, fpayAccount, freezeFin, flistID, iPageStart, iPageMax);

                // 这里的数据库的IP地址未确定，需要询问
                DataSet ds = fqc.GetResultX_ICE();

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    return null;

                // 在原来列上修改的话，会出异常，所以只好添加列元素
                ds.Tables[0].Columns.Add("strFreason", typeof(string));
                ds.Tables[0].Columns.Add("strType", typeof(string));
                ds.Tables[0].Columns.Add("strBankName", typeof(string));
                ds.Tables[0].Columns.Add("strSignName", typeof(string));
                ds.Tables[0].Columns.Add("strFcurtype", typeof(string));

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    dr["Fconnum"] = TENCENT.OSS.CFT.KF.Common.MoneyTransfer.FenToYuan(dr["Fconnum"].ToString());
                    dr["Fpaynum"] = TENCENT.OSS.CFT.KF.Common.MoneyTransfer.FenToYuan(dr["Fpaynum"].ToString());
                    dr["Fbalance"] = TENCENT.OSS.CFT.KF.Common.MoneyTransfer.FenToYuan(dr["Fbalance"].ToString());
                    dr["Fcon"] = TENCENT.OSS.CFT.KF.Common.MoneyTransfer.FenToYuan(dr["Fcon"].ToString());
                    dr["strFreason"] = getData.GetSubjectName(dr["Fsubject"].ToString());

                    switch (dr["Ftype"].ToString())
                    {
                        case "3":
                            {
                                dr["strType"] = "冻结"; break;
                            }
                        case "4":
                            {
                                dr["strType"] = "解冻"; break;
                            }
                        default:
                            {
                                dr["strType"] = "不允许的类型(Error)"; break;
                            }
                    }

                    dr["strBankName"] = getData.getBankName(dr["Fbank_type"].ToString());

                    switch (dr["Flist_sign"].ToString())
                    {
                        case "0":
                            {
                                dr["strSignName"] = "正常";
                                break;
                            }
                        case "1":
                            {
                                dr["strSignName"] = "被冲正";
                                break;
                            }
                        case "2":
                            {
                                dr["strSignName"] = "冲正";
                                break;
                            }
                        default:
                            {
                                dr["strSignName"] = null;
                                break;
                            }
                    }

                    switch (dr["Fcurtype"].ToString())
                    {
                        case "1":
                            {
                                dr["strFcurtype"] = "RMB"; break;
                            }
                        case "2":
                            {
                                dr["strFcurtype"] = "基金"; break;
                            }
                        case "3":
                            {
                                dr["strFcurtype"] = "游戏子账户(零钱包)"; break;
                            }
                        case "4":
                            {
                                dr["strFcurtype"] = "彩贝积分"; break;
                            }
                        case "5":
                            {
                                dr["strFcurtype"] = "直通车"; break;
                            }
                        default:
                            {
                                dr["strFcurtype"] = "未知"; break;
                            }
                    }
                }
                return ds;
            }
            catch (LogicException err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                throw;
            }
            catch (Exception err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                throw new LogicException("Service处理失败！");
            }
            finally
            {
                rl.WriteLog();
            }
        }

        [WebMethod(Description = "用户受控资金查询")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet QueryUserControledRecord(string qqid, string strBeginDate, string strEndDate, string cur_type, int iNumStart, int iNumMax)
        {
            RightAndLog rl = new RightAndLog();
            try
            {
                if (myHeader == null)
                {
                    throw new LogicException("不正确的调用方法！");
                }

                rl.actionType = "用户受控资金查询";
                rl.ID = qqid;
                rl.OperID = myHeader.OperID;
                rl.sign = 1;
                rl.strRightCode = "";
                rl.RightString = myHeader.RightString;
                rl.SzKey = myHeader.SzKey;
                rl.type = "查询";
                rl.UserID = myHeader.UserName;
                rl.UserIP = myHeader.UserIP;

                string fuid = PublicRes.ConvertToFuid(qqid);
                // 测试
                //  fuid = "295191000";

                if (fuid == null || fuid.Trim() == "")
                    throw new Exception("帐号不存在！");

                QeuryUserControledFinInfoClass query = new QeuryUserControledFinInfoClass(fuid, strBeginDate, strEndDate, cur_type, iNumStart, iNumMax);

                DataSet ds = query.GetResultX_ICE();

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    return ds;

                ds.Tables[0].Columns.Add("Fcur_typeName", typeof(string));
                ds.Tables[0].Columns.Add("FlstateName", typeof(string));
                ds.Tables[0].Columns.Add("FtypeText", typeof(string));
                ds.Tables[0].Columns.Add("uid", typeof(string));
                ds.Tables[0].Columns.Add("FbalanceStr", typeof(string));

                /*   这里没完全确认好！ */
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    dr["uid"] = fuid;
                    dr["FbalanceStr"] = MoneyTransfer.FenToYuan(dr["Fbalance"].ToString()) + "元";

                    switch (dr["Flstate"].ToString())
                    {
                        case "1":
                            {
                                dr["FlstateName"] = "有效"; break;
                            }
                        default:
                            {
                                dr["FlstateName"] = "无效"; break;
                            }
                    }

                    switch (dr["Fcur_type"].ToString())
                    {
                        case "80":
                            {
                                dr["Fcur_typeName"] = "账户类"; break;
                            }
                        case "81":
                            {
                                dr["Fcur_typeName"] = "账户类"; break;
                            }
                        case "82":
                            {
                                dr["Fcur_typeName"] = "账户类"; break;
                            }
                        case "1002":
                            {
                                dr["Fcur_typeName"] = "不可提现付款类"; break;
                            }
                        case "1003":
                            {
                                dr["Fcur_typeName"] = "不可提现付款类"; break;
                            }
                        case "1006":
                            {
                                dr["Fcur_typeName"] = "不可提现付款类"; break;
                            }
                        case "1007":
                            {
                                dr["Fcur_typeName"] = "定向类"; break;
                            }
                        case "1008":
                            {
                                dr["Fcur_typeName"] = "定向类"; break;
                            }
                        case "1005":
                            {
                                dr["Fcur_typeName"] = "定向类"; break;
                            }
                        default:
                            {
                                // 这个是看表取值，因为目前“不可提现类”值太多，所以暂时使用这个判断
                                if (dr["Fcur_type"].ToString().Length == 5 || dr["Fcur_type"].ToString() == "1004")
                                {
                                    dr["Fcur_typeName"] = "不可提现类";
                                    break;
                                }

                                dr["Fcur_typeName"] = "未知类型"; break;
                            }
                    }

                    string type = dr["Fcur_type"].ToString().Trim();
                    if (type != null && type.Length == 5)//5位数去掉前面1按银行类型解析，其他按下文档解析
                    {
                        type = type.Remove(0, 1);
                        dr["FtypeText"] = BankIO.QueryBankName(type);
                    }
                    else
                    {
                        switch (type)
                        {
                            case "1005":
                                {
                                    dr["FtypeText"] = "优秀员工奖金受控"; break;
                                }
                            case "1007":
                                {
                                    dr["FtypeText"] = "航空专区派送现金受控"; break;
                                }
                            case "1008":
                                {
                                    dr["FtypeText"] = "国航赠送现金受控"; break;
                                }
                            case "1010":
                                {
                                    dr["FtypeText"] = "淘点网充值受控"; break;
                                }
                            case "1011":
                                {
                                    dr["FtypeText"] = "paipai大促"; break;
                                }
                            case "1012":
                                {
                                    dr["FtypeText"] = "运通返现资金受控"; break;
                                }
                            case "1013":
                                {
                                    dr["FtypeText"] = "201306拍拍大促"; break;
                                }
                            case "1014":
                                {
                                    dr["FtypeText"] = "航旅B2C授信金额"; break;
                                }
                            default:
                                {
                                    dr["FtypeText"] = "无效" + type; break;
                                }
                        }
                    }

                }

                return ds;             
            }
            catch (LogicException err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                throw;
            }
            catch (Exception err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                throw new LogicException("Service处理失败！" + err.Message);
            }
            finally
            {
                rl.WriteLog();
            }
        }

        #region  代扣获取银行渠道及编号
        [WebMethod(Description = "代扣获取银行渠道及编号")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet QueryDkInfo_GetBank()
        {
            string strSql = "select *,count(distinct Fbank_sname) from cft_cep_db.t_bank_channel where Fbank_sname<>'' group by Fbank_sname";

            DataSet ds = new DataSet();
            DataTable dt;
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("INCB_NEW"));
            try
            {
                da.OpenConn();
                ds = da.dsGetTotalData(strSql);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    dr["Fbank_sname"] = DKBankName(dr["Fbank_sname"].ToString());
                }
                return ds;
            }
            finally
            {
                da.Dispose();
            }
        }

        #endregion

        //代扣银行渠道获取：通过英文名获取对应的银行渠道
        public static string DKBankName(string englishName)
        {
            string DKBankList = ConfigurationManager.AppSettings["DKBankList"];
            string[] bankEnglishName = DKBankList.Split('|');
            foreach (string oneInfo in bankEnglishName)
            {
                if (oneInfo.StartsWith(englishName + "="))
                {
                    return oneInfo.Replace(englishName + "=", "");
                }
            }

            return "未知银行渠道（" + englishName + "）";
        }

        #region	代扣查询
        [WebMethod(Description = "代扣单笔查询函数")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet QueryDkInfo(string explain, string bankID, string userID, string strBeginDate, string strEndDate, string spid, string spListID
            , string spBatchID, string cep_id, string state, string transaction_id, string bank_type, string service_code, int limStart, int limMax)
        {

            RightAndLog rl = new RightAndLog();
            try
            {
                if (myHeader == null)
                {
                    throw new LogicException("不正确的调用方法！");
                }


                rl.actionType = "代扣单笔查询";
                rl.ID = userID;
                rl.OperID = myHeader.OperID;
                rl.sign = 1;
                rl.strRightCode = "";
                rl.RightString = myHeader.RightString;
                rl.SzKey = myHeader.SzKey;
                rl.type = "查询";
                rl.UserID = myHeader.UserName;
                rl.UserIP = myHeader.UserIP;
   
                QueryDKInfo query = new QueryDKInfo(explain, bankID, userID, strBeginDate, strEndDate, spid, spListID, spBatchID, cep_id, state, transaction_id, bank_type, service_code, limStart, limMax);

                DataSet ds = query.GetResultX_AllAndLimit(limStart, limMax, "INCB_NEW");

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    return null;

                ds.Tables[0].Columns.Add("FstateName", typeof(string));
                ds.Tables[0].Columns.Add("FailedReason", typeof(string));
                ds.Tables[0].Columns.Add("Ftrade_state", typeof(string));
                ds.Tables[0].Columns.Add("Ftrade_stateName", typeof(string));
                ds.Tables[0].Columns.Add("Fbank_typeName", typeof(string));
                ds.Tables[0].Columns.Add("Fservice_codeName", typeof(string));

                DataTable dt;
                dt = QueryDkInfo_GetBank().Tables[0];
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string tmp = "";
                    if (dr["Fbank_type"] != null)
                    {
                        tmp = dr["Fbank_type"].ToString().Trim();
                        if (dt != null)
                        {
                            foreach (DataRow dl in dt.Rows)
                            {
                                if (tmp.Equals(dl["Fbank_type"].ToString()))
                                {
                                    dr["Fbank_typeName"] = dl["Fbank_sname"].ToString(); break;
                                }
                            }
                        }
                    }

                    tmp = dr["Fservice_code"].ToString().Trim();
                    tmp = tmp.Replace(dr["Fspid"].ToString().Trim(), "");
                    if (getData.htService_code.Contains(tmp))
                    {
                        dr["Fservice_codeName"] = getData.htService_code[tmp].ToString();
                    }

                    string strExplain = dr["Fexplain"].ToString().Trim();

                    for (; ; )
                    {
                        if (strExplain.IndexOf("%") > 0)
                        {
                            strExplain = PublicRes.getCgiString2(strExplain);
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (strExplain != "")
                    {
                        if (strExplain.IndexOf("&") > 0)
                            dr["FailedReason"] = strExplain.Substring(0, strExplain.IndexOf("&"));
                        else
                            dr["FailedReason"] = strExplain;
                    }

                    DataSet ds2 = this.GetQueryListDetail(dr["Ftransaction_id"].ToString());

                    if (ds2 != null && ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
                    {
                        dr["Ftrade_state"] = ds2.Tables[0].Rows[0]["Ftrade_state"].ToString();
                    }

                    switch (dr["Fstate"].ToString())
                    {
                        case "1":
                            {
                                dr["FstateName"] = "初始化"; break;
                            }
                        case "2":
                            {
                                dr["FstateName"] = "已审核"; break;
                            }
                        case "3":
                            {
                                dr["FstateName"] = "审核失败"; break;
                            }
                        case "4":
                            {
                                dr["FstateName"] = "待验证"; break;
                            }
                        case "5":
                            {
                                dr["FstateName"] = "已验证"; break;
                            }
                        case "6":
                            {
                                dr["FstateName"] = "发送银行前"; break;
                            }
                        case "7":
                            {
                                dr["FstateName"] = "已发送银行"; break;
                            }
                        case "8":
                            {
                                dr["FstateName"] = "交易成功"; break;
                            }
                        case "9":
                            {
                                dr["FstateName"] = "交易失败"; break;
                            }
                        default:
                            {
                                dr["FstateName"] = "未知" + dr["Fstate"].ToString(); break;
                            }
                    }
                }
                return ds;
            }
            catch (LogicException err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                throw;
            }
            catch (Exception err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                throw new LogicException("Service处理失败！");
            }
            finally
            {
                rl.WriteLog();
            }
        }

        [WebMethodAttribute(Description = "统计代扣单笔查询情况")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet CountDKInfo(string transactionId, string bankType, string bankID, string userID, string strBeginDate, string strEndDate, string spid, string spListID
            , string spBatchID, string state)
        {
            string strWhere = "";
            if (transactionId.Trim() != "")
                strWhere += "&transaction_id=" + transactionId;

            if (bankType.Trim() != "")
                strWhere += "&bank_type=" + bankType;
            if (spid.Trim() != "")
                strWhere += "&sp_id=" + spid;

            if (bankID.Trim() != "")
                strWhere += "&bankacc_no=" + bankID;

            if (userID.Trim() != "")
                strWhere += "&name=" + userID;

            if (spListID.Trim() != "")
                strWhere += "&coding=" + spListID;

            if (spBatchID.Trim() != "")
                strWhere += "&sp_batchid=" + spBatchID;

            if (state.Trim() != "")
            {
                // 这里有待和页面确定
                if (state == "3")
                {
                    strWhere += "&processing=";
                }
                else if (state == "1")
                {
                    strWhere += "&success_state=";
                }
                else if (state == "2")
                {
                    strWhere += "&failed_state=";
                }
            }

            if (strBeginDate != "" && strEndDate != "")
            {
                strWhere += "&stime=" + strBeginDate;
                strWhere += "&etime=" + strEndDate;
            }

            string errMsg = "";

            DataSet ds = CommQuery.GetDataSetFromICE(strWhere, "FINANCE_COUNT_CEPSPLIST", out errMsg);

            return ds;
        }

        [WebMethod(Description = "代扣单笔详细查询函数")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet QueryDkDetail(string cep_id, string strBeginDate, string strEndDate)
        {
            RightAndLog rl = new RightAndLog();
            try
            {
                if (myHeader == null)
                {
                    throw new LogicException("不正确的调用方法！");
                }

                rl.actionType = "代扣单笔详细查询函数";
                rl.ID = cep_id;
                rl.OperID = myHeader.OperID;
                rl.sign = 1;
                rl.strRightCode = "";
                rl.RightString = myHeader.RightString;
                rl.SzKey = myHeader.SzKey;
                rl.type = "查询";
                rl.UserID = myHeader.UserName;
                rl.UserIP = myHeader.UserIP;
   
                QueryDKInfo query = new QueryDKInfo(cep_id, strBeginDate, strEndDate);
                DataSet ds = query.GetResultX_ICE();

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    return null;

                ds.Tables[0].Columns.Add("FstateName", typeof(string));
                ds.Tables[0].Columns.Add("Frcd_typeName", typeof(string));
                ds.Tables[0].Columns.Add("Ftrade_typeName", typeof(string));
                ds.Tables[0].Columns.Add("FchannelName", typeof(string));
                ds.Tables[0].Columns.Add("FailedReason", typeof(string));
                ds.Tables[0].Columns.Add("Fbankacc_attrName", typeof(string));
                ds.Tables[0].Columns.Add("Fbankacc_typeName", typeof(string));
                ds.Tables[0].Columns.Add("Fpay_modeName", typeof(string));
                ds.Tables[0].Columns.Add("Fservice_codeName", typeof(string));

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string tmp = dr["Fservice_code"].ToString().Trim();
                    tmp = tmp.Replace(dr["Fspid"].ToString().Trim(), "");
                    if (getData.htService_code.Contains(tmp))
                    {
                        dr["Fservice_codeName"] = getData.htService_code[tmp].ToString();
                    }

                    string strExplain = dr["Fexplain"].ToString().Trim();

                    for (; ; )
                    {
                        if (strExplain.IndexOf("%") > 0)
                        {
                            strExplain = PublicRes.getCgiString2(strExplain);
                        }
                        else
                        {
                            break;
                        }
                    }

                    dr["FailedReason"] = strExplain;

                    switch (dr["Fbankacc_attr"].ToString())
                    {
                        case "0":
                            {
                                dr["Fbankacc_attrName"] = "公司"; break;
                            }
                        case "1":
                            {
                                dr["Fbankacc_attrName"] = "个人"; break;
                            }
                        default:
                            {
                                dr["Fbankacc_attrName"] = "未知" + dr["Fbankacc_attr"].ToString(); break;
                            }
                    }

                    switch (dr["Frcd_type"].ToString())
                    {
                        case "1":
                            {
                                dr["Frcd_typeName"] = "单笔型"; break;
                            }
                        case "2":
                            {
                                dr["Frcd_typeName"] = "批量型"; break;
                            }
                        default:
                            {
                                dr["Frcd_typeName"] = "未知"; break;
                            }
                    }

                    switch (dr["Fchannel"].ToString())
                    {
                        case "1":
                            {
                                dr["FchannelName"] = "企业版"; break;
                            }
                        case "2":
                            {
                                dr["FchannelName"] = "接口"; break;
                            }
                        default:
                            {
                                dr["FchannelName"] = "未知"; break;
                            }
                    }


                    switch (dr["Fbankacc_type"].ToString())
                    {
                        case "0":
                            {
                                dr["Fbankacc_typeName"] = "银行卡"; break;
                            }
                        case "1":
                            {
                                dr["Fbankacc_typeName"] = "存折"; break;
                            }
                        case "2":
                            {
                                dr["Fbankacc_typeName"] = "信用卡"; break;
                            }
                        default:
                            {
                                dr["Fbankacc_typeName"] = "未知"; break;
                            }
                    }


                    switch (dr["Fpay_type"].ToString())
                    {
                        case "1":
                            {
                                dr["Fpay_modeName"] = "余额"; break;
                            }
                        case "2":
                            {
                                dr["Fpay_modeName"] = "一点通"; break;
                            }
                        case "4":
                            {
                                dr["Fpay_modeName"] = "直接代扣（借记卡）"; break;
                            }
                        case "8":
                            {
                                dr["Fpay_modeName"] = "EPOS支付"; break;
                            }
                        case "16":
                            {
                                dr["Fpay_modeName"] = "快捷代扣"; break;
                            }
                        case "17":
                            {
                                dr["Fpay_modeName"] = "混合"; break;
                            }
                        default:
                            {
                                dr["Fpay_modeName"] = "未知"; break;
                            }
                    }


                    switch (dr["Ftrade_type"].ToString())
                    {
                        case "1":
                            {
                                dr["Ftrade_typeName"] = "c2c"; break;
                            }
                        case "2":
                            {
                                dr["Ftrade_typeName"] = "b2c"; break;
                            }
                        case "3":
                            {
                                dr["Ftrade_typeName"] = "fastpay"; break;
                            }
                        case "4":
                            {
                                dr["Ftrade_typeName"] = "收/付款"; break;
                            }
                        case "5":
                            {
                                dr["Ftrade_typeName"] = "转帐（按钮）"; break;
                            }
                        case "6":
                            {
                                dr["Ftrade_typeName"] = "商户转帐 "; break;
                            }
                        case "7":
                            {
                                dr["Ftrade_typeName"] = "直扣交易"; break;
                            }
                        case "8":
                            {
                                dr["Ftrade_typeName"] = "购物券（发行）类"; break;
                            }
                        case "9":
                            {
                                dr["Ftrade_typeName"] = "C2C转账"; break;
                            }
                        default:
                            {
                                dr["Fbankacc_typeName"] = "未知"; break;
                            }
                    }


                    switch (dr["Fstate"].ToString())
                    {
                        case "1":
                            {
                                dr["FstateName"] = "初始化"; break;
                            }
                        case "2":
                            {
                                dr["FstateName"] = "已审核"; break;
                            }
                        case "3":
                            {
                                dr["FstateName"] = "审核失败"; break;
                            }
                        case "4":
                            {
                                dr["FstateName"] = "待验证"; break;
                            }
                        case "5":
                            {
                                dr["FstateName"] = "已验证"; break;
                            }
                        case "6":
                            {
                                dr["FstateName"] = "发送银行前"; break;
                            }
                        case "7":
                            {
                                dr["FstateName"] = "已发送银行"; break;
                            }
                        case "8":
                            {
                                dr["FstateName"] = "交易成功"; break;
                            }
                        case "9":
                            {
                                dr["FstateName"] = "交易失败"; break;
                            }
                        default:
                            {
                                dr["FstateName"] = "未知" + dr["Fstate"].ToString(); break;
                            }
                    }
                }

                return ds;
            }
            catch (LogicException err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                throw;
            }
            catch (Exception err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                throw new LogicException("Service处理失败！(" + err.Message + ")");
            }
            finally
            {
                rl.WriteLog();
            }
        }

        [WebMethod(Description = "代扣批量查询函数")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet QueryBatchInfo(string strBeginDate, string strEndDate, string spid, string spBatchID, string batchid, string state, int limStart, int limMax)
        {
            RightAndLog rl = new RightAndLog();
            try
            {
                if (myHeader == null)
                {
                    throw new LogicException("不正确的调用方法！");
                }

                rl.actionType = "代扣批量查询函数";
                rl.ID = spid;
                rl.OperID = myHeader.OperID;
                rl.sign = 1;
                rl.strRightCode = "";
                rl.RightString = myHeader.RightString;
                rl.SzKey = myHeader.SzKey;
                rl.type = "查询";
                rl.UserID = myHeader.UserName;
                rl.UserIP = myHeader.UserIP;

                QueryBatchDKInfo query = new QueryBatchDKInfo(strBeginDate, strEndDate, spid, spBatchID, batchid, state, limStart, limMax);
                DataSet ds = query.GetResultX_AllAndLimit(limStart, limMax, "INC_NEW");

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    return null;

                ds.Tables[0].Columns.Add("FstateName", typeof(string));
                ds.Tables[0].Columns.Add("FHandling_Count", typeof(string));
                ds.Tables[0].Columns.Add("FHandling_amount", typeof(string));
                ds.Tables[0].Columns.Add("Fservice_codeName", typeof(string));

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string tmp = dr["Fservice_code"].ToString().Trim();
                    tmp = tmp.Replace(dr["Fspid"].ToString().Trim(), "");
                    if (getData.htService_code.Contains(tmp))
                    {
                        dr["Fservice_codeName"] = getData.htService_code[tmp].ToString();
                    }

                    long handling_count = long.Parse(dr["Ftotal_count"].ToString()) - long.Parse(dr["Fsucpay_count"].ToString())
                        - long.Parse(dr["Ffailpay_count"].ToString());

                    long handling_amount = long.Parse(dr["Ftotal_paynum"].ToString()) - long.Parse(dr["Fsucpay_amount"].ToString())
                        - long.Parse(dr["Ffailpay_amount"].ToString());

                    dr["FHandling_Count"] = handling_count.ToString();
                    dr["FHandling_amount"] = handling_amount.ToString();
                    dr["Fexplain"] = PublicRes.getCgiString(dr["Fexplain"].ToString());

                    switch (dr["Fstate"].ToString())
                    {
                        case "1":
                            {
                                dr["FstateName"] = "批次初始化"; break;
                            }
                        case "2":
                            {
                                dr["FstateName"] = "批次记录失败"; break;
                            }
                        case "3":
                            {
                                dr["FstateName"] = "批次待审批"; break;
                            }
                        case "4":
                            {
                                dr["FstateName"] = "批次取消"; break;
                            }
                        case "5":
                            {
                                dr["FstateName"] = "批次审核失败"; break;
                            }
                        case "6":
                            {
                                dr["FstateName"] = "批次验证中"; break;
                            }
                        case "7":
                            {
                                dr["FstateName"] = "批次处理中"; break;
                            }
                        case "8":
                            {
                                dr["FstateName"] = "批次处理完"; break;
                            }
                        case "9":
                            {
                                dr["FstateName"] = "处理结果全部成功"; break;
                            }
                        case "10":
                            {
                                dr["FstateName"] = "处理结果部分成功"; break;
                            }
                        case "11":
                            {
                                dr["FstateName"] = "处理结果全部失败"; break;
                            }
                        case "12":
                            {
                                dr["FstateName"] = "后台预处理中"; break;
                            }
                        case "13":
                            {
                                dr["FstateName"] = "预处理完成"; break;
                            }
                        default:
                            {
                                dr["FstateName"] = "未知" + dr["Fstate"].ToString(); break;
                            }
                    }
                }

                return ds;
            }
            catch (LogicException err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                throw;
            }
            catch (Exception err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                throw new LogicException("Service处理失败！(" + err.Message + ")");
            }
            finally
            {
                rl.WriteLog();
            }
        }

        [WebMethodAttribute(Description = "统计代扣批量的情况")]
        public DataSet CountBatchInfo(string strBeginDate, string strEndDate, string spid, string spBatchID, string batchid, string state)
        {
            string strWhere = " where Fcreate_time between '" + strBeginDate + "' and '" + strEndDate + "' ";
            if (spid.Trim() != "")
            {
                strWhere += " and Fspid='" + spid + "' ";
            }

            if (spBatchID.Trim() != "")
                strWhere += " and Fsp_batchid='" + spBatchID + "' ";

            if (batchid.Trim() != "")
                strWhere += " and Fbatchid='" + batchid + "' ";

            if (state != "0")
                strWhere += " and Fstate=" + state;

            string strSql = " select count(*),sum(Fsucpay_count),sum(Fsucpay_amount),sum(Ffailpay_count),sum(Ffailpay_amount),"
                + "sum(Ftotal_count-Fsucpay_count-Ffailpay_count),sum(Ftotal_paynum-Fsucpay_amount-Ffailpay_amount) from cft_cep_db.t_batch_record " + strWhere;

            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("INC_NEW"));
            try
            {
                da.OpenConn();
                DataSet ds = da.dsGetTotalData(strSql);
                return ds;
            }
            catch
            {
                return null;
            }
            finally
            {
                da.Dispose();
            }
        }

        [WebMethod(Description = "财付券查询函数")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet GetCepspServiceInfo(string spid, string service_code)
        {
            CepspService query = new CepspService(spid, service_code);

            return query.GetResultX_ICE();
        }

        #endregion

        #endregion

        [WebMethod(Description = "财付券查询函数")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet GetGwq(string u_ID, string filter, int iPageStart, int iPageMax)
        {
            RightAndLog rl = new RightAndLog();
            try
            {
                if (myHeader == null)
                {
                    throw new LogicException("不正确的调用方法！");
                }
                GwqQueryClass cuser = new GwqQueryClass(u_ID, filter);
                return cuser.GetResultX(iPageStart, iPageMax, "GWQ");

            }
            catch (LogicException err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                throw;
            }
            catch (Exception err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                throw new LogicException("Service处理失败！(" + err.Message + ")");
            }
            finally
            {
                rl.WriteLog();
            }
        }

        [WebMethod(Description = "财付券个数函数")]
        public int GetGwqCount(string u_ID, string filter)
        {
            try
            {
                GwqQueryClass cuser = new GwqQueryClass(u_ID, filter);
                return cuser.GetCount("GWQ");
            }
            catch (LogicException err)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new Exception("service发生错误,请联系管理员！(" + e.Message + ")");
                return 0;
            }
        }

        [WebMethod(Description = "返回用户投诉意见信息")]
        public bool getUserReports(int istartIndex, int length, DateTime bgdate, DateTime eddate, string whereStr, out DataSet ds, out string Msg)
        {
            ds = null;
            Msg = null;
            MySqlAccess da = null;

            try
            {
                string bgstrDate = bgdate.ToString("yyyy-MM-dd 00:00:00");
                string edstrDate = eddate.ToString("yyyy-MM-dd 23:59:59");

                string whereCommand = " where dttm >= '" + bgstrDate + "' and dttm <='" + edstrDate + "' ";

                if (whereStr != "" && whereStr.Trim() != "")
                {
                    whereCommand += whereStr;
                }

                string usrreportDB = System.Configuration.ConfigurationManager.AppSettings["usrreportDB"].Trim();
                string dbName = usrreportDB + ".tblsug";

                //此表不再需要。furion 20090610
                da = new MySqlAccess(PublicRes.GetConnString("ywb_30"));
                da.OpenConn();

                string fstrSql_count = "select count(1) from " + dbName + "" + whereCommand;
                int count = Int32.Parse(da.GetOneResult(fstrSql_count));
                string fstrSql = "select *," + count + " as icount from " + dbName + " " + whereCommand + "  order by dttm DESC limit " + istartIndex + "," + length; //查询最新的数据
                ds = da.dsGetTotalData(fstrSql);

                return true;
            }
            catch (Exception e)
            {
                Msg += "返回用户投诉意见信息失败!" + PublicRes.replaceMStr(e.Message);
                PublicRes.WriteFile(Msg);
                return false;
            }
            finally
            {
                da.Dispose();
            }
        }

        [WebMethod(Description = "返回用户敏感信息修改历史")]
        public bool getUserModify(int istartIndex, int length, DateTime bgdate, DateTime eddate, string whereStr, out DataSet ds, out string Msg)
        {
            ds = null;
            Msg = null;
            MySqlAccess da = null;

            try
            {
                string bgstrDate = bgdate.ToString("yyyy-MM-dd 00:00:00");
                string edstrDate = eddate.ToString("yyyy-MM-dd 23:59:59");
                string whereCommand = " where FcommitTime >= '" + bgstrDate + "' and FcommitTime <='" + edstrDate + "' ";

                if (whereStr != "" && whereStr.Trim() != "")
                {
                    whereCommand += whereStr;
                }

                string dbName = "c2c_fmdb.t_mediation";
                da = new MySqlAccess(PublicRes.GetConnString("ht"));
                da.OpenConn();
                string fstrSql_count = "select count(1) from " + dbName + " " + whereCommand;
                int count = Int32.Parse(da.GetOneResult(fstrSql_count));
                string fstrSql = "select *," + count + " as icount from " + dbName + " " + whereCommand + "  order by FID DESC limit " + istartIndex + "," + length; //查询最新的数据
                ds = da.dsGetTotalData(fstrSql);

                return true;
            }
            catch (Exception e)
            {
                Msg += "返回用户历史修改信息失败!" + PublicRes.replaceMStr(e.Message);
                PublicRes.WriteFile(Msg);
                return false;
            }
            finally
            {
                da.Dispose();
            }
        }

        [WebMethod(Description = "查询处理器（处理多种查询)")]
        public bool getQueryData(int istartIndex, int length, DateTime bgDate, DateTime edDate, string whereStr, string queryType, out DataSet ds, out string Msg)
        {
            ds = null;

            if (queryType.ToLower() == "usermodify")
            {
                getUserModify(istartIndex, length, bgDate, edDate, whereStr, out ds, out Msg);
                return true;
            }
            else
            {
                Msg = " 未定义的查询类型！ 请检查！";
                return false;
            }
        }

        [WebMethod(Description = "自助申诉查询函数")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet GetCFTUserAppealList(string fuin, string u_BeginTime, string u_EndTime, int fstate, int ftype, string QQType, string dotype, int iPageStart, int iPageMax, int SortType)
        {
            RightAndLog rl = new RightAndLog();
            try
            {
                if (myHeader == null)
                {
                    throw new LogicException("不正确的调用方法！");
                }

                rl.actionType = "自助申诉查询函数";
                rl.ID = fuin;
                rl.sign = 1;
                rl.strRightCode = "InfoCenter";
                rl.type = "查询";
 
                PublicRes.SetRightAndLog(myHeader, rl);
                if (!rl.CheckRight())
                {
                    throw new LogicException("用户无权执行此操作！");
                }

                CFTUserAppealClass cuser = new CFTUserAppealClass(fuin, u_BeginTime, u_EndTime, fstate, ftype, QQType, dotype, SortType);
                DataSet ds = cuser.GetResultX(iPageStart, iPageMax, "CFTB");

                long Appeal_BigMoney = long.Parse(System.Configuration.ConfigurationManager.AppSettings["Appeal_BigMoney"]);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (ftype == 8)
                    {
                        CFTUserAppealClass.HandleParameter_ForControledFreeze(ds, true);
                    }
                    else
                    {
                        CFTUserAppealClass.HandleParameter(ds, false);
                    }

                    //取出金额后如果超过大金额，打上标记。
                    ds.Tables[0].Columns.Add("Fuincolor", typeof(String));
                    ds.Tables[0].Columns.Add("balance", typeof(String));//金额 echo 20140909

                    ICEAccess ice = new ICEAccess(PublicRes.ICEServerIP, PublicRes.ICEPort);
                    try
                    {
                        ice.OpenConn();
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            try
                            {
                                dr["Fuincolor"] = "";
                                string fuid = PublicRes.ConvertToFuid(dr["Fuin"].ToString());

                                string strwhere = "where=" + ICEAccess.URLEncode("fuid=" + fuid + "&");
                                strwhere += ICEAccess.URLEncode("fcurtype=1&");

                                string strResp = "";

                                DataTable dtuser = ice.InvokeQuery_GetDataTable(YWSourceType.用户资源, YWCommandCode.查询用户信息, fuid, strwhere, out strResp);

                                if (dtuser == null || dtuser.Rows.Count == 0)
                                {
                                    dr["balance"] = "0";
                                    continue;
                                }

                                long lbalance = long.Parse(dtuser.Rows[0]["fbalance"].ToString());
                                dr["balance"] = lbalance.ToString();

                                if (lbalance >= Appeal_BigMoney)
                                {
                                    dr["Fuincolor"] = "BIGMONEY";
                                }
                            }
                            catch
                            {
                                continue;
                            }
                        }

                        ice.CloseConn();
                    }
                    finally
                    {
                        ice.Dispose();
                    }
                }

                return ds;

            }
            catch (LogicException err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                throw;
            }
            catch (Exception err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                throw new LogicException("Service处理失败！");
            }
            finally
            {
                rl.WriteLog();
            }
        }

        [WebMethod(Description = "自助申诉查询函数")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet GetCFTUserAppealListNew(string fuin, string u_BeginTime, string u_EndTime, int fstate, int ftype, string QQType, string dotype, int iPageStart, int iPageMax, int SortType)
        {
            RightAndLog rl = new RightAndLog();
            try
            {
                if (myHeader == null)
                {
                    throw new LogicException("不正确的调用方法！");
                }

                rl.actionType = "自助申诉查询函数";
                rl.ID = fuin;
                rl.sign = 1;
                rl.strRightCode = "InfoCenter";
                rl.type = "查询";
                PublicRes.SetRightAndLog(myHeader, rl);
                if (!rl.CheckRight())
                {
                    throw new LogicException("用户无权执行此操作！");
                }

                CFTUserAppealClass cuser = new CFTUserAppealClass(fuin, u_BeginTime, u_EndTime, fstate, ftype, QQType, dotype, SortType);
                DataSet ds = cuser.GetResultX("CFTB");

                //为了自助申诉查询功能添加
                if (u_EndTime == "" && u_BeginTime == "")
                {
                    u_EndTime = DateTime.Now.ToString();
                    u_BeginTime = "2014-01-01 00:00:00";
                }

                DateTime date = DateTime.Parse(u_BeginTime);
                int yearEnd = DateTime.Parse(u_EndTime).Year;
                int monEnd = DateTime.Parse(u_EndTime).Month;
                List<string> listdb = new List<string>();
                List<string> listtb = new List<string>();
                if (yearEnd >= 2014)//才查分库表
                {
                    while (date.Year < 2014)
                    {
                        date = date.AddMonths(1);
                    }
                    while (!((date.Year == yearEnd && date.Month > monEnd) || (date.Year > yearEnd)))
                    {
                        listdb.Add(date.Year.ToString());
                        listtb.Add(date.Month.ToString());
                        date = date.AddMonths(1);
                    }
                }

                DataSet dsFenResult = new DataSet();
                if (listdb == null || listdb.Count == 0)
                    dsFenResult = null;
                else
                {
                    int index = 0;//计数添加的有数据的数据表
                    for (int i = 0; i < listdb.Count; i++)
                    {
                        string db = listdb[i];
                        string tb = listtb[i];
                        CFTUserAppealClass cuser2 = new CFTUserAppealClass(fuin, u_BeginTime, u_EndTime, fstate, ftype, QQType, dotype, SortType, true, db, tb);//分库分表的查询
                        DataSet dsfen = cuser2.GetResultX("CFTNEW");

                        if (dsfen != null && dsfen.Tables.Count > 0 && dsfen.Tables[0].Rows.Count > 0)
                        {
                            if (index == 0)
                            {
                                dsFenResult.Tables.Add(dsfen.Tables[0].Copy());
                                index++;
                            }
                            else
                            {
                                foreach (DataRow dr in dsfen.Tables[0].Rows)
                                {
                                    dsFenResult.Tables[0].ImportRow(dr);//将记录加入到一个表里
                                }
                            }
                        }
                    }
                }
                //将旧表与分库表数据写入一个表
                DataSet dsAll = NewMethod(ds, dsFenResult);
           
                return dsAll;

            }
            catch (LogicException err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                throw;
            }
            catch (Exception err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                throw new LogicException("Service处理失败！");
            }
            finally
            {
                rl.WriteLog();
            }
        }

        [WebMethod(Description = "自助申诉查询三种类型，在排序后处理用户信息")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet GetCFTUserAppealListFunction(DataSet dsAll)
        {
            RightAndLog rl = new RightAndLog();
            try
            {
                long Appeal_BigMoney = long.Parse(System.Configuration.ConfigurationManager.AppSettings["Appeal_BigMoney"]);

                if (dsAll != null && dsAll.Tables.Count > 0 && dsAll.Tables[0].Rows.Count > 0)
                {
                    CFTUserAppealClass.HandleParameter(dsAll, false);

                    //取出金额后如果超过大金额，打上标记。
                    dsAll.Tables[0].Columns.Add("Fuincolor", typeof(String));
                    dsAll.Tables[0].Columns.Add("balance", typeof(String));//金额 echo 20140909

                    ICEAccess ice = new ICEAccess(PublicRes.ICEServerIP, PublicRes.ICEPort);
                    try
                    {
                        ice.OpenConn();

                        foreach (DataRow dr in dsAll.Tables[0].Rows)
                        {
                            try
                            {
                                dr["Fuincolor"] = "";
                                string fuid = PublicRes.ConvertToFuid(dr["Fuin"].ToString());

                                string strwhere = "where=" + ICEAccess.URLEncode("fuid=" + fuid + "&");
                                strwhere += ICEAccess.URLEncode("fcurtype=1&");

                                string strResp = "";

                                DataTable dtuser = ice.InvokeQuery_GetDataTable(YWSourceType.用户资源, YWCommandCode.查询用户信息, fuid, strwhere, out strResp);

                                if (dtuser == null || dtuser.Rows.Count == 0)
                                {
                                    dr["balance"] = "0";
                                    continue;
                                }

                                long lbalance = long.Parse(dtuser.Rows[0]["fbalance"].ToString());
                                dr["balance"] = lbalance.ToString();

                                if (lbalance >= Appeal_BigMoney)
                                {
                                    dr["Fuincolor"] = "BIGMONEY";
                                }
                            }
                            catch
                            {
                                continue;
                            }
                        }

                        ice.CloseConn();
                    }
                    finally
                    {
                        ice.Dispose();
                    }
                }
                return dsAll;
            }
            catch (LogicException err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                throw;
            }
            catch (Exception err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                throw new LogicException("Service处理失败！");
            }
            finally
            {
                rl.WriteLog();
            }
        }

        private static DataSet NewMethod(DataSet ds, DataSet ds2)
        {
            DataSet dsAll = new DataSet();
            DataSet result = new DataSet();
            DataTable dtAll = new DataTable();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                dsAll.Tables.Add(ds.Tables[0].Copy());
                dsAll.Tables[0].Columns.Add("FidNew", Type.GetType("System.String"));
                //处理Fid,t_tenpay_appeal_trans表fid为int，分库表为varchar
                if (dsAll != null && dsAll.Tables.Count > 0 && dsAll.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsAll.Tables[0].Rows)
                    {
                        dr["FidNew"] = dr["Fid"].ToString();
                    }
                }

                dsAll.Tables[0].Columns.Remove(dsAll.Tables[0].Columns["Fid"]);//删除fid列
                dsAll.Tables[0].Columns["FidNew"].ColumnName = "Fid";//将FidNew列名修改为Fid

                if (ds2 != null && ds2.Tables.Count > 0)
                {//分库表不为null
                    foreach (DataTable tbl in ds2.Tables)
                        if (tbl.Rows.Count > 0)//分库表不为null
                        {
                            foreach (DataRow dr in tbl.Rows)
                            {
                                dsAll.Tables[0].ImportRow(dr);//将记录加入到一个表里
                            }
                        }
                }
            }
            else
            {
                if (ds2 != null && ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
                    dsAll.Tables.Add(ds2.Tables[0].Copy());
           
            }
            return dsAll;
        }

        [WebMethod(Description = "自助申诉直接通过函数")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public void CFTUserAppealPass(string UserIP, bool IsLockList)  //手工执行加上锁定单
        {
            string msg = "";

            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("CFT"));
            MySqlAccess dab = new MySqlAccess(PublicRes.GetConnString("CFTB"));
            MySqlAccess daFen = new MySqlAccess(PublicRes.GetConnString("CFTNEW"));//新分库分表 
            MySqlAccess daIVRFen = new MySqlAccess(PublicRes.GetConnString("IVRNEW"));//IVR新分库分表 

            //DateTime NowFlag = Convert.ToDateTime(PublicRes.strNowTimeStander).AddHours(-1);    //查备机以防万一把时间提前一小时
            DateTime NowFlag = Convert.ToDateTime(PublicRes.strNowTimeStander);//原代码测试注释了，上线需打开
            //  DateTime NowFlag = DateTime.Now.AddMonths(2).AddDays(-12);//用该行测试，2014.1.1
            //   DateTime NowFlag = DateTime.Now;//测试用

            bool Appeal_IVRFlag = System.Configuration.ConfigurationManager.AppSettings["Appeal_IVRFlag"] == "true"; //IVR开关,如果为真的话,使用IVR外呼
            long Appeal_IVRMoney = long.Parse(System.Configuration.ConfigurationManager.AppSettings["Appeal_IVRMoney"]); //IVR金额限额值,申诉用户余额超过后进行IVR外呼
            try
            {
                da.OpenConn();
                dab.OpenConn();
                daFen.OpenConn();
                daIVRFen.OpenConn();

                //旧库高分单
                #region
                string Sql;
                if (IsLockList)  //只执行锁定单
                    Sql = "select Fid,Fuin,Fuid,Ftype,Fstate,FSubmitTime from db_appeal.t_tenpay_appeal_trans where FSubmitTime>='" + NowFlag.AddDays(-3).ToString("yyyy-MM-dd HH:mm:ss")
                        + "' and FSubmitTime<='" + NowFlag.ToString("yyyy-MM-dd HH:mm:ss") + "' and Fstate = 8 and FType in(1,2,5,6,7) and FParameter like '%&AUTO_APPEAL=1%'";
                else
                    Sql = "select Fid,Fuin,Fuid,Ftype,Fstate,FSubmitTime from db_appeal.t_tenpay_appeal_trans where FSubmitTime>='" + NowFlag.AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss")
                        + "' and FSubmitTime<='" + NowFlag.ToString("yyyy-MM-dd HH:mm:ss") + "' and Fstate in(0,3,4,5,6) and FType in(1,2,5,6,7) and FParameter like '%&AUTO_APPEAL=1%'";

                DataSet ds = dab.dsGetTotalData(Sql);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (!IsLockList)
                    {
                        Sql = "update db_appeal.t_tenpay_appeal_trans set FPickUser='system',FPickTime=now(),Fstate=8 where FSubmitTime>='" + NowFlag.AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss")
                            + "' and FSubmitTime<='" + NowFlag.ToString("yyyy-MM-dd HH:mm:ss") + "' and Fstate in(0,3,4,5,6) and FType in(1,2,5,6,7) and FParameter like '%&AUTO_APPEAL=1%'";
                        da.ExecSql(Sql);
                    }

                    //在这个函数里,所有auto_appeal=1的单被置状态为Fstate=8; 					

                    string errMsg = "";
                    int Success = 0;
                    int Fail = 0;
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        try
                        {
                            #region
                            if (Appeal_IVRFlag && dr["Ftype"].ToString() == "5")
                            {
                                string fuid = dr["Fuid"].ToString();
                                //先判断余额好,还是先判断一点通好,看哪个查询快和过滤度高.
                                string icesql = "uid=" + fuid;
                                string acount = CommQuery.GetOneResultFromICE(icesql, "QUERY_USER_BINDKJ", "acount", out errMsg);

                                bool ivrflag = false;
                                if (Int32.Parse(acount) > 0)
                                    ivrflag = true;

                                if (!ivrflag)
                                {
                                    icesql = "uid=" + fuid;
                                    acount = CommQuery.GetOneResultFromICE(icesql, "QUERY_USER_BINDYDT", "acount", out errMsg);

                                    if (Int32.Parse(acount) > 0)
                                        ivrflag = true;
                                }
                                if (!ivrflag)
                                {
                                    //判断余额.
                                    ICEAccess ice = new ICEAccess(PublicRes.ICEServerIP, PublicRes.ICEPort);
                                    try
                                    {
                                        ice.OpenConn();
                                        string strwhere = "where=" + ICEAccess.URLEncode("fuid=" + fuid + "&");
                                        strwhere += ICEAccess.URLEncode("fcurtype=1&");
                                        string strResp = "";

                                        DataTable dtuser = ice.InvokeQuery_GetDataTable(YWSourceType.用户资源, YWCommandCode.查询用户信息, fuid, strwhere, out strResp);

                                        ice.CloseConn();

                                        if (dtuser == null || dtuser.Rows.Count == 0)
                                        {
                                            Fail++;
                                            continue;
                                        }

                                        long lbalance = long.Parse(dtuser.Rows[0]["fbalance"].ToString());

                                        if (lbalance >= Appeal_IVRMoney)
                                        {
                                            ivrflag = true;
                                        }
                                    }
                                    finally
                                    {
                                        ice.Dispose();
                                    }
                                }
                                //判断通过后,插入数据.先不取姓名和金额值,那两个是预留字段.								
                                if (ivrflag)
                                {
                                    string mobile = "";
                                    MySqlAccess damn = new MySqlAccess(PublicRes.GetConnString("MN"));

                                    try
                                    {
                                        damn.OpenConn();
                                        string sql = " select * from msgnotify_" + fuid.Substring(fuid.Length - 3, 2) + ".t_msgnotify_user_" + fuid.Substring(fuid.Length - 1, 1) + " where Fuid = '" + fuid + "'";
                                        DataTable dt = damn.GetTable(sql);

                                        if (dt == null || dt.Rows.Count != 1)
                                        {
                                            //如果不存在记录就continue,应该是可以的.不执行通过策略.
                                            Fail++;
                                            continue;
                                        }

                                        mobile = dt.Rows[0]["Fmobile"].ToString();
                                    }
                                    finally
                                    {
                                        damn.Dispose();
                                    }

                                    string strSql = "select count(*) from db_appeal.t_tenpay_appeal_IVR where FAppealID=" + dr["Fid"].ToString();
                                    if (da.GetOneResult(strSql) == "0")
                                    {
                                        strSql = "Insert db_appeal.t_tenpay_appeal_IVR(FAppealID,FAppealType,Fuin,Fuid,Fmobile,FAppealTime,Fstate,FPickTime,FmodifyTime) values ("
                                            + dr["Fid"].ToString() + ",5,'" + dr["Fuin"].ToString() + "','" + dr["Fuid"].ToString() + "','"
                                            + mobile + "','" + DateTime.Parse(dr["FSubmitTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "',0,now(),now())";

                                        da.ExecSql(strSql);

                                    }
                                    continue;
                                }
                                //判断不通过,不用处理,走入下面的函数.
                            }
                            #endregion

                            string mesg = "";
                            if (CFTUserAppealClass.ConfirmAppeal(int.Parse(dr["fid"].ToString()), "", "system", UserIP, out mesg))
                                Success++;
                            else
                                Fail++;
                        }
                        catch
                        {
                            continue;
                        }
                    }

                    if (Success > 0 || Fail > 0)
                    {
                        loger.err("CFTUserAppealPass", "高分主动审核成功笔数：" + Success + "，失败笔数：" + Fail);
                    }
                }
                #endregion


                //旧库低分单
                #region
                string Sql2;
                if (IsLockList)  //只执行锁定单
                    Sql2 = "select Fid,Fuin,Fuid,Ftype,Fstate,FSubmitTime,FParameter  from db_appeal.t_tenpay_appeal_trans where FSubmitTime>='" + NowFlag.AddDays(-3).ToString("yyyy-MM-dd HH:mm:ss")
                        + "' and FSubmitTime<='" + NowFlag.ToString("yyyy-MM-dd HH:mm:ss") + "' and Fstate = 8 and FType in(1,2,5,6,7) and FParameter like '%&sheetIVRSate=1%' ";
                else
                    //低分单，且之前没有被扫描过
                    Sql2 = "select Fid,Fuin,Fuid,Ftype,Fstate,FSubmitTime,FParameter  from db_appeal.t_tenpay_appeal_trans where FSubmitTime>='" + NowFlag.AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss")
                            + "' and FSubmitTime<='" + NowFlag.ToString("yyyy-MM-dd HH:mm:ss") + "' and Fstate in(0,3,4,5,6) and FType=5 and FParameter not like '%&AUTO_APPEAL=1%' and FParameter not like '%&sheetIVRSate=1%' ";

                DataSet ds2 = dab.dsGetTotalData(Sql2);

                if (ds2 != null && ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
                {

                    //在这个函数里,所有低分单，余额不超过设定值、或开通快捷、或开通一点通被置状态为Fstate=8; 					

                    string errMsg = "";
                    int Success = 0;
                    int Fail = 0;
                    foreach (DataRow dr in ds2.Tables[0].Rows)
                    {
                        if (!IsLockList)
                        {
                            //&sheetIVRSate=1 低分单已通过该函数，在FParameter中增加标记，以防多次扫描
                            //   string Parameter = dr["FParameter"].ToString() + "&sheetIVRSate=1";
                            Sql2 = "update db_appeal.t_tenpay_appeal_trans set FPickUser='system',FPickTime=now(),FParameter=concat(FParameter,'&sheetIVRSate=1') where Fid='" + dr["Fid"].ToString() + "' and FUin='" + dr["Fuin"].ToString() + "'";
                            da.ExecSql(Sql2);
                        }

                        try
                        {
                            #region
                            if (Appeal_IVRFlag && dr["Ftype"].ToString() == "5")
                            {
                                string fuid = dr["Fuid"].ToString();
                                //先判断余额好,还是先判断一点通好,看哪个查询快和过滤度高.
                                string icesql = "uid=" + fuid;
                                string acount = CommQuery.GetOneResultFromICE(icesql, "QUERY_USER_BINDKJ", "acount", out errMsg);
                                // acount = "2";//测试
                                bool ivrflag = false;
                                if (Int32.Parse(acount) > 0)
                                    ivrflag = true;

                                if (!ivrflag)
                                {
                                    icesql = "uid=" + fuid;
                                    acount = CommQuery.GetOneResultFromICE(icesql, "QUERY_USER_BINDYDT", "acount", out errMsg);

                                    if (Int32.Parse(acount) > 0)
                                        ivrflag = true;
                                }
                                if (!ivrflag)
                                {
                                    //判断余额.
                                    ICEAccess ice = new ICEAccess(PublicRes.ICEServerIP, PublicRes.ICEPort);
                                    try
                                    {
                                        ice.OpenConn();
                                        string strwhere = "where=" + ICEAccess.URLEncode("fuid=" + fuid + "&");
                                        strwhere += ICEAccess.URLEncode("fcurtype=1&");

                                        string strResp = "";

                                        DataTable dtuser = ice.InvokeQuery_GetDataTable(YWSourceType.用户资源, YWCommandCode.查询用户信息, fuid, strwhere, out strResp);

                                        ice.CloseConn();

                                        if (dtuser == null || dtuser.Rows.Count == 0)
                                        {
                                            Fail++;
                                            continue;
                                        }

                                        long lbalance = long.Parse(dtuser.Rows[0]["fbalance"].ToString());

                                        if (lbalance >= Appeal_IVRMoney)
                                        {
                                            ivrflag = true;
                                        }
                                    }
                                    finally
                                    {
                                        ice.Dispose();
                                    }
                                }
                                //判断通过后,插入数据.先不取姓名和金额值,那两个是预留字段.								
                                if (ivrflag)
                                {
                                    string mobile = "";
                                    MySqlAccess damn = new MySqlAccess(PublicRes.GetConnString("MN"));

                                    try
                                    {
                                        damn.OpenConn();
                                        string sql = " select * from msgnotify_" + fuid.Substring(fuid.Length - 3, 2) + ".t_msgnotify_user_" + fuid.Substring(fuid.Length - 1, 1) + " where Fuid = '" + fuid + "'";
                                        DataTable dt = damn.GetTable(sql);

                                        if (dt == null || dt.Rows.Count != 1)
                                        {
                                            //如果不存在记录就continue,应该是可以的.不执行通过策略.
                                            Fail++;
                                            continue;
                                        }

                                        mobile = dt.Rows[0]["Fmobile"].ToString();
                                    }
                                    finally
                                    {
                                        damn.Dispose();
                                    }
                                    //在写入t_tenpay_appeal_IVR表之前，将Fstate=8
                                    Sql2 = "update db_appeal.t_tenpay_appeal_trans set FPickUser='system',FPickTime=now(),Fstate=8  where Fid='" + dr["Fid"].ToString() + "' and FUin='" + dr["Fuin"].ToString() + "'";
                                    da.ExecSql(Sql2);

                                    string strSql = "select count(*) from db_appeal.t_tenpay_appeal_IVR where FAppealID=" + dr["Fid"].ToString();
                                    if (da.GetOneResult(strSql) == "0")
                                    {
                                        strSql = "Insert db_appeal.t_tenpay_appeal_IVR(FAppealID,FAppealType,Fuin,Fuid,Fmobile,FAppealTime,Fstate,FPickTime,FmodifyTime) values ("
                                            + dr["Fid"].ToString() + ",5,'" + dr["Fuin"].ToString() + "','" + dr["Fuid"].ToString() + "','"
                                            + mobile + "','" + DateTime.Parse(dr["FSubmitTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "',0,now(),now())";

                                        da.ExecSql(strSql);

                                    }
                                    continue;
                                }
                                //判断不通过,不用处理,走入下面的函数.
                            }
                            #endregion
                        }
                        catch
                        {
                            continue;
                        }
                    }
                }
                #endregion


                //分库分表，Ftype=5类型低分单IVR外呼
                //分库分表数据只有1、5、6三种类型，5类型的低分单需要客服系统处理，要判断有无风险，是否外呼
                #region

                //每次都跑上月与本月的分表
                int year = NowFlag.Year;
                int month = NowFlag.Month;
                string str1 = "";
                string str2 = "";
                string Sql3;
                if (IsLockList)//只执行锁定单
                {
                    string from = "select Fid,Fuin,Fuid,Ftype,Fstate,FSubmitTime from db_appeal_";
                    string where = " where FSubmitTime>='" + NowFlag.AddDays(-3).ToString("yyyy-MM-dd HH:mm:ss")
                       + "' and FSubmitTime<='" + NowFlag.ToString("yyyy-MM-dd HH:mm:ss") + "' and Fstate = 8 and FType=5 and FIVRState=1 ";

                    //查当月分表
                    if (month < 10)//处理不同月份表名
                        str1 = from + year + ".t_tenpay_appeal_trans_" + "0" + month + where;
                    else
                        str1 = from + year + ".t_tenpay_appeal_trans_" + month + where;

                    if (NowFlag.Day < 4)//每月3号内需加查上个月数据
                    {
                        //查上个月分表
                        if (month == 1)//如果现在是一月加查前一年的12月
                            str2 = from + (year - 1) + ".t_tenpay_appeal_trans_" + 12 + where;
                        else
                        {
                            if (month - 1 < 10)
                                str2 = from + year + ".t_tenpay_appeal_trans_" + "0" + (month - 1) + where;
                            else
                                str2 = from + year + ".t_tenpay_appeal_trans_" + (month - 1) + where;
                        }
                    }
                }
                else//低分单，且之前没有被扫描过
                {
                    string from = "select Fid,Fuin,Fuid,Ftype,Fstate,FSubmitTime from db_appeal_";
                    string where = " where FSubmitTime>='" + NowFlag.AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss")
                       + "' and FSubmitTime<='" + NowFlag.ToString("yyyy-MM-dd HH:mm:ss") + "' and Fstate in(0,3,4,5,6) and FType=5 and FAutoAppeal<>1 and FIVRState<>1 ";

                    //查当月分表
                    if (month < 10)//处理不同月份表名
                        str1 = from + year + ".t_tenpay_appeal_trans_" + "0" + month + where;
                    else
                        str1 = from + year + ".t_tenpay_appeal_trans_" + month + where;

                    if (NowFlag.Day == 1)//每月1号内需加查上个月数据
                    {
                        //查上个月分表
                        if (month == 1)//如果现在是一月加查前一年的12月
                            str2 = from + (year - 1) + ".t_tenpay_appeal_trans_" + 12 + where;
                        else
                        {
                            if (month - 1 < 10)
                                str2 = from + year + ".t_tenpay_appeal_trans_" + "0" + (month - 1) + where;
                            else
                                str2 = from + year + ".t_tenpay_appeal_trans_" + (month - 1) + where;
                        }
                    }
                }

                if (str2 != "")
                    Sql3 = str1 + " union " + str2;
                else
                    Sql3 = str1;
                DataSet ds3 = daFen.dsGetTotalData(Sql3);

                if (ds3 != null && ds3.Tables.Count > 0 && ds3.Tables[0].Rows.Count > 0)
                {

                    //在这个函数里,所有低分单，余额不超过设定值、或开通快捷、或开通一点通被置状态为Fstate=8; 					

                    string errMsg = "";

                    int Success = 0;
                    int Fail = 0;
                    foreach (DataRow dr in ds3.Tables[0].Rows)
                    {
                        int yearDB = DateTime.Parse(dr["FSubmitTime"].ToString()).Year;
                        int monthDB = DateTime.Parse(dr["FSubmitTime"].ToString()).Month;

                        if (!IsLockList)
                        {
                            //低分单已通过该函数，将FIVRState=1，以防多次扫描
                            if (monthDB < 10)
                                Sql3 = "update db_appeal_" + yearDB + ".t_tenpay_appeal_trans_" + "0" + monthDB + " set FPickUser='system',FPickTime=now(),FModifyTime=Now(),FIVRState=1 where Fid='" + dr["Fid"].ToString() + "' and FUin='" + dr["Fuin"].ToString() + "'";
                            else
                                Sql3 = "update db_appeal_" + yearDB + ".t_tenpay_appeal_trans_" + monthDB + " set FPickUser='system',FPickTime=now(),FModifyTime=Now(),FIVRState=1 where Fid='" + dr["Fid"].ToString() + "' and FUin='" + dr["Fuin"].ToString() + "'";
                            daFen.ExecSql(Sql3);
                        }

                        try
                        {
                            #region
                            if (Appeal_IVRFlag && dr["Ftype"].ToString() == "5")
                            {
                                string fuid = dr["Fuid"].ToString();
                                //先判断余额好,还是先判断一点通好,看哪个查询快和过滤度高.
                                string icesql = "uid=" + fuid;
                                string acount = CommQuery.GetOneResultFromICE(icesql, "QUERY_USER_BINDKJ", "acount", out errMsg);
                                // acount = "2";//测试
                                bool ivrflag = false;
                                if (Int32.Parse(acount) > 0)
                                    ivrflag = true;

                                if (!ivrflag)
                                {
                                    icesql = "uid=" + fuid;
                                    acount = CommQuery.GetOneResultFromICE(icesql, "QUERY_USER_BINDYDT", "acount", out errMsg);

                                    if (Int32.Parse(acount) > 0)
                                        ivrflag = true;
                                }
                                if (!ivrflag)
                                {
                                    //判断余额.
                                    ICEAccess ice = new ICEAccess(PublicRes.ICEServerIP, PublicRes.ICEPort);
                                    try
                                    {
                                        ice.OpenConn();
                                        string strwhere = "where=" + ICEAccess.URLEncode("fuid=" + fuid + "&");
                                        strwhere += ICEAccess.URLEncode("fcurtype=1&");

                                        string strResp = "";

                                        DataTable dtuser = ice.InvokeQuery_GetDataTable(YWSourceType.用户资源, YWCommandCode.查询用户信息, fuid, strwhere, out strResp);

                                        ice.CloseConn();

                                        if (dtuser == null || dtuser.Rows.Count == 0)
                                        {
                                            Fail++;
                                            continue;
                                        }

                                        long lbalance = long.Parse(dtuser.Rows[0]["fbalance"].ToString());

                                        if (lbalance >= Appeal_IVRMoney)
                                        {
                                            ivrflag = true;
                                        }
                                    }
                                    finally
                                    {
                                        ice.Dispose();
                                    }
                                }
                                //判断通过后,插入数据.先不取姓名和金额值,那两个是预留字段.								
                                if (ivrflag)
                                {
                                    string mobile = "";
                                    MySqlAccess damn = new MySqlAccess(PublicRes.GetConnString("MN"));

                                    try
                                    {
                                        damn.OpenConn();
                                        string sql = " select * from msgnotify_" + fuid.Substring(fuid.Length - 3, 2) + ".t_msgnotify_user_" + fuid.Substring(fuid.Length - 1, 1) + " where Fuid = '" + fuid + "'";
                                        DataTable dt = damn.GetTable(sql);

                                        if (dt == null || dt.Rows.Count != 1)
                                        {
                                            //如果不存在记录就continue,应该是可以的.不执行通过策略.
                                            Fail++;
                                            continue;
                                        }

                                        mobile = dt.Rows[0]["Fmobile"].ToString();
                                    }
                                    finally
                                    {
                                        damn.Dispose();
                                    }
                                    //在写入t_tenpay_appeal_IVR表之前，将Fstate=8
                                    if (monthDB < 10)
                                        Sql3 = "update db_appeal_" + yearDB + ".t_tenpay_appeal_trans_" + "0" + monthDB + " set FPickUser='system',FPickTime=now(),FModifyTime=Now(),Fstate=8  where Fid='" + dr["Fid"].ToString() + "' and FUin='" + dr["Fuin"].ToString() + "'";
                                    else
                                        Sql3 = "update db_appeal_" + yearDB + ".t_tenpay_appeal_trans_" + monthDB + " set FPickUser='system',FPickTime=now(),FModifyTime=Now(),Fstate=8  where Fid='" + dr["Fid"].ToString() + "' and FUin='" + dr["Fuin"].ToString() + "'";
                                    daFen.ExecSql(Sql3);

                                    string strSql;
                                    if (monthDB < 10)
                                        strSql = "select count(*) from db_apeal_IVR_" + yearDB + ".t_tenpay_appeal_IVR_" + "0" + monthDB + "  where FAppealID='" + dr["Fid"].ToString() + "'";
                                    else
                                        strSql = "select count(*) from db_apeal_IVR_" + yearDB + ".t_tenpay_appeal_IVR_" + monthDB + "  where FAppealID='" + dr["Fid"].ToString() + "'";
                                    if (daIVRFen.GetOneResult(strSql) == "0")
                                    {
                                        if (monthDB < 10)
                                            strSql = "Insert  db_apeal_IVR_" + yearDB + ".t_tenpay_appeal_IVR_" + "0" + monthDB + "(FAppealID,FAppealType,Fuin,Fuid,Fmobile,FAppealTime,Fstate,FPickTime,FmodifyTime,FAutoAppeal) values ('"
                                            + dr["Fid"].ToString() + "',5,'" + dr["Fuin"].ToString() + "','" + dr["Fuid"].ToString() + "','"
                                            + mobile + "','" + DateTime.Parse(dr["FSubmitTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "',0,now(),now(),0)";
                                        else
                                            strSql = "Insert  db_apeal_IVR_" + yearDB + ".t_tenpay_appeal_IVR_" + monthDB + "(FAppealID,FAppealType,Fuin,Fuid,Fmobile,FAppealTime,Fstate,FPickTime,FmodifyTime,FAutoAppeal) values ('"
                                            + dr["Fid"].ToString() + "',5,'" + dr["Fuin"].ToString() + "','" + dr["Fuid"].ToString() + "','"
                                            + mobile + "','" + DateTime.Parse(dr["FSubmitTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "',0,now(),now(),0)";
                                        daIVRFen.ExecSql(strSql);
                                    }
                                    continue;
                                }
                                //判断不通过,不用处理,走入人工审批流程
                            }
                            #endregion
                        }
                        catch
                        {
                            continue;
                        }
                    }
                }
                #endregion


                //分库分表，Ftype=5类型高分单IVR外呼
                //分库分表数据只有1、5、6三种类型，5类型的高分单经过前端后都是有风险的，客服系统需外呼
                #region

                //每次都跑上月与本月的分表
                year = NowFlag.Year;
                month = NowFlag.Month;
                str1 = "";
                str2 = "";
                string Sql4;
                if (IsLockList)//只执行锁定单
                {
                    string from = "select Fid,Fuin,Fuid,Ftype,Fstate,FSubmitTime from db_appeal_";
                    string where = " where FSubmitTime>='" + NowFlag.AddDays(-3).ToString("yyyy-MM-dd HH:mm:ss")
                       + "' and FSubmitTime<='" + NowFlag.ToString("yyyy-MM-dd HH:mm:ss") + "' and Fstate = 8 and FType=5 and FAutoAppeal=1 ";

                    //查当月分表
                    if (month < 10)//处理不同月份表名
                        str1 = from + year + ".t_tenpay_appeal_trans_" + "0" + month + where;
                    else
                        str1 = from + year + ".t_tenpay_appeal_trans_" + month + where;

                    if (NowFlag.Day < 4)//每月3号内需加查上个月数据
                    {
                        //查上个月分表
                        if (month == 1)//如果现在是一月加查前一年的12月
                            str2 = from + (year - 1) + ".t_tenpay_appeal_trans_" + 12 + where;
                        else
                        {
                            if (month - 1 < 10)
                                str2 = from + year + ".t_tenpay_appeal_trans_" + "0" + (month - 1) + where;
                            else
                                str2 = from + year + ".t_tenpay_appeal_trans_" + (month - 1) + where;
                        }
                    }
                }
                else//高分单
                {
                    string from = "select Fid,Fuin,Fuid,Ftype,Fstate,FSubmitTime from db_appeal_";
                    string where = " where FSubmitTime>='" + NowFlag.AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss")
                       + "' and FSubmitTime<='" + NowFlag.ToString("yyyy-MM-dd HH:mm:ss") + "' and Fstate in(0,3,4,5,6) and FType=5 and FAutoAppeal=1";

                    //查当月分表
                    if (month < 10)//处理不同月份表名
                        str1 = from + year + ".t_tenpay_appeal_trans_" + "0" + month + where;
                    else
                        str1 = from + year + ".t_tenpay_appeal_trans_" + month + where;

                    if (NowFlag.Day == 1)//每月1号内需加查上个月数据
                    {
                        //查上个月分表
                        if (month == 1)//如果现在是一月加查前一年的12月
                            str2 = from + (year - 1) + ".t_tenpay_appeal_trans_" + 12 + where;
                        else
                        {
                            if (month - 1 < 10)
                                str2 = from + year + ".t_tenpay_appeal_trans_" + "0" + (month - 1) + where;
                            else
                                str2 = from + year + ".t_tenpay_appeal_trans_" + (month - 1) + where;
                        }
                    }
                }
                if (str2 != "")
                    Sql4 = str1 + " union " + str2;
                else
                    Sql4 = str1;

                DataSet ds4 = daFen.dsGetTotalData(Sql4);

                if (ds4 != null && ds4.Tables.Count > 0 && ds4.Tables[0].Rows.Count > 0)
                {

                    //在这个函数里,所有低分单，余额不超过设定值、或开通快捷、或开通一点通被置状态为Fstate=8; 					

                    string errMsg = "";

                    int Success = 0;
                    int Fail = 0;
                    foreach (DataRow dr in ds4.Tables[0].Rows)
                    {
                        int yearDB = DateTime.Parse(dr["FSubmitTime"].ToString()).Year;
                        int monthDB = DateTime.Parse(dr["FSubmitTime"].ToString()).Month;

                        try
                        {
                            #region
                            if (Appeal_IVRFlag && dr["Ftype"].ToString() == "5")
                            {
                                string fuid = dr["Fuid"].ToString();


                                string mobile = "";
                                MySqlAccess damn = new MySqlAccess(PublicRes.GetConnString("MN"));

                                try
                                {
                                    damn.OpenConn();
                                    string sql = " select * from msgnotify_" + fuid.Substring(fuid.Length - 3, 2) + ".t_msgnotify_user_" + fuid.Substring(fuid.Length - 1, 1) + " where Fuid = '" + fuid + "'";
                                    DataTable dt = damn.GetTable(sql);

                                    if (dt == null || dt.Rows.Count != 1)
                                    {
                                        //如果不存在记录就continue,应该是可以的.不执行通过策略.
                                        Fail++;
                                        continue;
                                    }

                                    mobile = dt.Rows[0]["Fmobile"].ToString();
                                }
                                finally
                                {
                                    damn.Dispose();
                                }
                                //在写入t_tenpay_appeal_IVR表之前，将Fstate=8
                                if (monthDB < 10)
                                    Sql3 = "update db_appeal_" + yearDB + ".t_tenpay_appeal_trans_" + "0" + monthDB + " set FPickUser='system',FPickTime=now(),FModifyTime=Now(),Fstate=8  where Fid='" + dr["Fid"].ToString() + "' and FUin='" + dr["Fuin"].ToString() + "'";
                                else
                                    Sql3 = "update db_appeal_" + yearDB + ".t_tenpay_appeal_trans_" + monthDB + " set FPickUser='system',FPickTime=now(),FModifyTime=Now(),Fstate=8  where Fid='" + dr["Fid"].ToString() + "' and FUin='" + dr["Fuin"].ToString() + "'";
                                daFen.ExecSql(Sql3);

                                string strSql;
                                if (monthDB < 10)
                                    strSql = "select count(*) from db_apeal_IVR_" + yearDB + ".t_tenpay_appeal_IVR_" + "0" + monthDB + "  where FAppealID='" + dr["Fid"].ToString() + "'";
                                else
                                    strSql = "select count(*) from db_apeal_IVR_" + yearDB + ".t_tenpay_appeal_IVR_" + monthDB + "  where FAppealID='" + dr["Fid"].ToString() + "'";
                                if (daIVRFen.GetOneResult(strSql) == "0")
                                {
                                    if (monthDB < 10)
                                        strSql = "Insert  db_apeal_IVR_" + yearDB + ".t_tenpay_appeal_IVR_" + "0" + monthDB + "(FAppealID,FAppealType,Fuin,Fuid,Fmobile,FAppealTime,Fstate,FPickTime,FmodifyTime,FAutoAppeal) values ('"
                                        + dr["Fid"].ToString() + "',5,'" + dr["Fuin"].ToString() + "','" + dr["Fuid"].ToString() + "','"
                                        + mobile + "','" + DateTime.Parse(dr["FSubmitTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "',0,now(),now(),1)";
                                    else
                                        strSql = "Insert  db_apeal_IVR_" + yearDB + ".t_tenpay_appeal_IVR_" + monthDB + "(FAppealID,FAppealType,Fuin,Fuid,Fmobile,FAppealTime,Fstate,FPickTime,FmodifyTime,FAutoAppeal) values ('"
                                        + dr["Fid"].ToString() + "',5,'" + dr["Fuin"].ToString() + "','" + dr["Fuid"].ToString() + "','"
                                        + mobile + "','" + DateTime.Parse(dr["FSubmitTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "',0,now(),now(),1)";
                                    daIVRFen.ExecSql(strSql);

                                }
                                continue;

                            }
                            #endregion
                        }
                        catch
                        {
                            continue;
                        }
                    }
                }
                #endregion


            }
            catch (Exception ex)
            {
                loger.err("CFTUserAppealPass", ex.Message);
            }
            finally
            {
                da.Dispose();
                dab.Dispose();
                daFen.Dispose();
                daIVRFen.Dispose();
            }
        }

        [WebMethod(Description = "自助申诉查询个数函数")]
        public int GetCFTUserAppealCount(string fuin, string u_BeginTime, string u_EndTime, int fstate, int ftype, string QQType, string dotype, int SortType)
        {
            try
            {
                CFTUserAppealClass cuser = new CFTUserAppealClass(fuin, u_BeginTime, u_EndTime, fstate, ftype, QQType, dotype, SortType);
                return cuser.GetCount("CFTB");
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("service发生错误,请联系管理员！{0}", e.Message));
                return 0;
            }
        }

        [WebMethod(Description = "自助申诉查询个数函数：三种申诉类型分库分表")]
        public int GetCFTUserAppealCountNew(string fuin, string u_BeginTime, string u_EndTime, int fstate, int ftype, string QQType, string dotype, int SortType)
        {
            try
            {
                int Count = 0;
                CFTUserAppealClass cuser = new CFTUserAppealClass(fuin, u_BeginTime, u_EndTime, fstate, ftype, QQType, dotype, SortType);
                Count = cuser.GetCount("CFTB");

                DateTime date = DateTime.Parse(u_BeginTime);
                int yearEnd = DateTime.Parse(u_EndTime).Year;
                int monEnd = DateTime.Parse(u_EndTime).Month;

                List<string> listdb = new List<string>();
                List<string> listtb = new List<string>();

                if (yearEnd >= 2014)//才查分库表
                {
                    while (date.Year < 2014)
                    {
                        date = date.AddMonths(1);
                    }
                    while (!((date.Year == yearEnd && date.Month > monEnd) || (date.Year > yearEnd)))
                    {
                        listdb.Add(date.Year.ToString());
                        listtb.Add(date.Month.ToString());
                        date = date.AddMonths(1);
                    }
                }

                if (listdb == null || listdb.Count == 0) return Count;
                for (int i = 0; i < listdb.Count; i++)
                {
                    string db = listdb[i];
                    string tb = listtb[i];
                    CFTUserAppealClass cuser2 = new CFTUserAppealClass(fuin, u_BeginTime, u_EndTime, fstate, ftype, QQType, dotype, SortType, true, db, tb);//分库分表的查询
                    Count += cuser2.GetCount("CFTNEW");
                }
                return Count;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("service发生错误,请联系管理员！{0}", ex.Message));
                return 0;
            }
        }

        [WebMethod(Description = "自助申诉查询详细函数")]
        public DataSet GetCFTUserAppealDetail(int fid)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("CFT"));
            try
            {
                CFTUserAppealClass cuser = new CFTUserAppealClass(fid);
                DataSet ds = cuser.GetResultX(0, 1, "CFT");

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    int ftype = int.Parse(ds.Tables[0].Rows[0]["Ftype"].ToString());
                    if (ftype == 8)
                    {
                        CFTUserAppealClass.HandleParameter_ForControledFreeze(ds, true);

                        FreezeQueryClass cuser2 = new FreezeQueryClass(ds.Tables[0].Rows[0]["Fuin"].ToString(), 1);

                        DataSet ds2 = cuser2.GetResultX(0, 1, "HT");

                        ds.Tables[0].Columns.Add("isFreezeListHas", typeof(string));

                        if (ds2 != null && ds2.Tables.Count != 0 && ds2.Tables[0].Rows.Count != 0)
                        {
                            ds.Tables[0].Rows[0]["isFreezeListHas"] = "1";
                        }
                        else
                        {
                            ds.Tables[0].Rows[0]["isFreezeListHas"] = "0";
                        }
                    }
                    else
                    {
                        CFTUserAppealClass.HandleParameter(ds, true);
                    }

                    //如果类型是5:完整注册用户更换关联手机 ,增加上外呼结果. //IVR外呼专用furion
                    ds.Tables[0].Columns.Add("FIVRResult");
                    ds.Tables[0].Rows[0]["FIVRResult"] = "";

                    if (ftype == 5)
                    {
                        da.OpenConn();
                        string strSql = "select * from db_appeal.t_tenpay_appeal_IVR where Fappealid=" + fid;
                        DataTable dtivr = da.GetTable(strSql);

                        if (dtivr != null && dtivr.Rows.Count == 1)
                        {
                            string ivrresult = "呼叫次数:" + dtivr.Rows[0]["Fcallnum"].ToString();
                            string tmp = dtivr.Rows[0]["Fcallresult"].ToString();
                            if (tmp == "1")
                                ivrresult += "$$呼叫状态:用户主动回复1同意";
                            else if (tmp == "2")
                                ivrresult += "$$呼叫状态:用户主动回复2拒绝";
                            else if (tmp == "3")
                                ivrresult += "$$呼叫状态:用户主动回复其它值.";
                            else if (tmp == "4")
                                ivrresult += "$$呼叫状态:用户不接听电话";
                            else if (tmp == "5")
                                ivrresult += "$$呼叫状态:用户主动挂机";
                            else if (tmp == "6")
                                ivrresult += "$$呼叫状态:呼叫无法建立(空号,关机)";
                            else if (tmp == "7")
                                ivrresult += "$$呼叫状态:Ivr主动挂机(超过1分钟用户没有按键)";

                            ivrresult += "$$呼叫结果:" + dtivr.Rows[0]["Fcallmemo"].ToString();
                            ivrresult += "$$原绑定手机号码:" + dtivr.Rows[0]["Fmobile"].ToString();

                            ds.Tables[0].Rows[0]["FIVRResult"] = ivrresult;
                        }
                    }
                }

                return ds;
            }
            catch (LogicException err)
            {
                throw;
            }
            catch (Exception err)
            {
                throw new LogicException("Service处理失败！");
            }
            finally
            {
                da.Dispose();
                System.GC.Collect();
            }
        }

        [WebMethod(Description = "解冻申诉查询详细函数")]
        public DataSet GetCFTUserAppealDetail_New(string fid, string submitDate)
        {
            try
            {
                if (string.IsNullOrEmpty(submitDate))
                {
                    throw new Exception("申诉单提交时间不能为空！");
                }

                #region 查申诉库表
                DateTime date = DateTime.Parse(submitDate);
                int i_m = date.Month;
                string s_m = "";
                if (i_m < 10)
                {
                    s_m = "0" + i_m;
                }
                else
                {
                    s_m = i_m.ToString();
                }
                string table = "db_appeal_" + date.Year.ToString() + ".t_tenpay_appeal_trans_" + s_m;

                CFTUserAppealClass cuser = new CFTUserAppealClass(fid, table);
                DataSet ds = cuser.GetResultX(0, 1, "fkdj");
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    DateTime d2 = date.AddMonths(-1);
                    i_m = d2.Month;
                    if (i_m < 10)
                    {
                        s_m = "0" + i_m;
                    }
                    else
                    {
                        s_m = i_m.ToString();
                    }
                    table = "db_appeal_" + d2.Year.ToString() + ".t_tenpay_appeal_trans_" + s_m;
                    CFTUserAppealClass cuser2 = new CFTUserAppealClass(fid, table);
                    ds = cuser2.GetResultX(0, 1, "fkdj");
                }
                #endregion

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    int ftype = int.Parse(ds.Tables[0].Rows[0]["Ftype"].ToString());

                    #region 字段处理
                    ds.Tables[0].Columns.Add("isFreezeListHas", typeof(string));
                    ds.Tables[0].Columns.Add("FreezeReason", typeof(string));//冻结原因
                    ds.Tables[0].Columns.Add("detail_score", typeof(string)); //得分明细
                    ds.Tables[0].Columns.Add("risk_result", typeof(string)); //风控标记

                    DataRow dr = ds.Tables[0].Rows[0];
                    string risk_result = CFTUserAppealClass.getCgiString(dr["FRiskState"].ToString());
                    if (risk_result == "0")
                        dr["risk_result"] = "";
                    else if (risk_result == "1")
                        dr["risk_result"] = "风控异常单无需人工回访用户";
                    else if (risk_result == "2")
                        dr["risk_result"] = "风控异常单需人工回访用户";
                    else
                        dr["risk_result"] = risk_result;

                    dr["detail_score"] = CFTUserAppealClass.getCgiString(dr["FDetailScore"].ToString());

                    if (dr["detail_score"].ToString() != "")
                    {
                        try
                        {
                            string detail_score = System.Web.HttpUtility.UrlDecode(dr["detail_score"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));

                            if (detail_score.IndexOf("PwdProtection") > -1)
                            {
                                detail_score = detail_score.Replace("PwdProtection", "密保校验得分");
                            }
                            if (detail_score.IndexOf("CertifiedId") > -1)
                            {
                                detail_score = detail_score.Replace("CertifiedId", "证件号校验得分");
                            }
                            if (detail_score.IndexOf("bind_email") > -1)
                            {
                                detail_score = detail_score.Replace("bind_email", "绑定邮箱校验得分");
                            }
                            if (detail_score.IndexOf("bind_mobile") > -1)
                            {
                                detail_score = detail_score.Replace("bind_mobile", "绑定手机校验得分");
                            }
                            if (detail_score.IndexOf("QQReceipt") > -1)
                            {
                                detail_score = detail_score.Replace("QQReceipt", "QQ申诉回执号得分");
                            }
                            if (detail_score.IndexOf("CertifiedBankCard") > -1)
                            {
                                detail_score = detail_score.Replace("CertifiedBankCard", "实名认证银行卡号校验得分");
                            }
                            if (detail_score.IndexOf("CreditCardPayHist") > -1)
                            {
                                detail_score = detail_score.Replace("CreditCardPayHist", "信用卡还款信息校验得分");
                            }
                            if (detail_score.IndexOf("WithdrawHist") > -1) //andrew 20110419
                            {
                                detail_score = detail_score.Replace("WithdrawHist", "提现记录得分");
                            }
                            if (detail_score.IndexOf("MBVerify") > -1) 
                            {
                                detail_score = detail_score.Replace("MBVerify", "安平密保验证得分");
                            }
                            if (detail_score.IndexOf("MBQuery") > -1)
                            {
                                detail_score = detail_score.Replace("MBQuery", "通过安全中心密保得分");
                            }
                            if (detail_score.IndexOf("BindMobile") > -1)
                            {
                                detail_score = detail_score.Replace("BindMobile", "绑定的手机号码得分");
                            }
                            if (detail_score.IndexOf("Mobile") > -1)
                            {
                                detail_score = detail_score.Replace("Mobile", "手机得分");
                            }
                            if (detail_score.IndexOf("Email_QQ") > -1)
                            {
                                detail_score = detail_score.Replace("Email_QQ", "绑定QQ邮箱得分");
                            }
                            if (detail_score.IndexOf("Mobile_New") > -1)
                            {
                                detail_score = detail_score.Replace("Mobile_New", "未注册手机得分");
                            }
                            if (detail_score.IndexOf("QQReceipt_6") > -1)
                            {
                                detail_score = detail_score.Replace("QQReceipt_6", "简化注册用户QQ申诉回执单号得分");
                            }

                            dr["detail_score"] = detail_score;

                        }
                        catch
                        { }
                    }
                    #endregion

                    if (ftype == 8 || ftype == 19)
                    {
                        #region 查冻结日志表
                        FreezeQueryClass cuser2 = new FreezeQueryClass(ds.Tables[0].Rows[0]["Fuin"].ToString(), 1);

                        DataSet ds2 = cuser2.GetResultX(0, 1, "HT");

                        if (ds2 != null && ds2.Tables.Count != 0 && ds2.Tables[0].Rows.Count != 0)
                        {
                            ds.Tables[0].Rows[0]["isFreezeListHas"] = "1";
                            ds.Tables[0].Rows[0]["FreezeReason"] = ds2.Tables[0].Rows[0]["FFreezeReason"].ToString();
                        }
                        else
                        {
                            ds.Tables[0].Rows[0]["isFreezeListHas"] = "0";
                        }

                        #endregion
                    }
                    else if (ftype == 11)//特殊找回支付密码
                    {
                        ds.Tables[0].Rows[0]["isFreezeListHas"] = "";
                        ds.Tables[0].Rows[0]["FreezeReason"] = "";
                    }
                    else
                    {
                        //只处理类型为8,19,11的记录
                        throw new Exception("只处理解冻申诉、特殊找回支付密码，记录类型错误：" + ftype);
                    }
                }

                return ds;
            }
            catch (LogicException err)
            {
                throw;
            }
            catch (Exception err)
            {
                throw new LogicException("Service处理失败！");
            }
        }

        [WebMethod(Description = "自助申诉查询详细函数")]
        public DataSet GetCFTUserAppealDetailByDBTB(string fid, string db, string tb)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("CFT"));
            try
            {
                CFTUserAppealClass cuser = new CFTUserAppealClass(fid, db, tb, "ByDBTB");
                DataSet ds = cuser.GetResultX(0, 1, "CFTNEW");

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    int ftype = int.Parse(ds.Tables[0].Rows[0]["Ftype"].ToString());
                   
                    CFTUserAppealClass.HandleParameterByDBTB(ds, true);//分库表的处理方法，处理结果后的结果与旧表一样
 
                    //如果类型是5:完整注册用户更换关联手机 ,增加上外呼结果. //IVR外呼专用furion
                    ds.Tables[0].Columns.Add("FIVRResult");
                    ds.Tables[0].Rows[0]["FIVRResult"] = "";

                    if (ftype == 5)
                    {
                        da.OpenConn();

                        string dateTime = ds.Tables[0].Rows[0]["FSubmitTime"].ToString();
                        int year = DateTime.Parse(dateTime).Year;
                        int month = DateTime.Parse(dateTime).Month;
                        string dbIVR = "db_apeal_IVR_" + year;
                        string tbIVR = "";
                        if (month < 10)
                            tbIVR = "t_tenpay_appeal_IVR_0" + month;
                        else
                            tbIVR = "t_tenpay_appeal_IVR_" + month;

                        string strSql = "select * from  " + dbIVR + "." + tbIVR + "  where Fappealid='" + fid + "'";//外呼库表需重构
                        DataTable dtivr = da.GetTable(strSql);

                        if (dtivr != null && dtivr.Rows.Count == 1)
                        {
                            string ivrresult = "呼叫次数:" + dtivr.Rows[0]["Fcallnum"].ToString();
                            string tmp = dtivr.Rows[0]["Fcallresult"].ToString();
                            if (tmp == "1")
                                ivrresult += "$$呼叫状态:用户主动回复1同意";
                            else if (tmp == "2")
                                ivrresult += "$$呼叫状态:用户主动回复2拒绝";
                            else if (tmp == "3")
                                ivrresult += "$$呼叫状态:用户主动回复其它值.";
                            else if (tmp == "4")
                                ivrresult += "$$呼叫状态:用户不接听电话";
                            else if (tmp == "5")
                                ivrresult += "$$呼叫状态:用户主动挂机";
                            else if (tmp == "6")
                                ivrresult += "$$呼叫状态:呼叫无法建立(空号,关机)";
                            else if (tmp == "7")
                                ivrresult += "$$呼叫状态:Ivr主动挂机(超过1分钟用户没有按键)";

                            ivrresult += "$$呼叫结果:" + dtivr.Rows[0]["Fcallmemo"].ToString();
                            ivrresult += "$$原绑定手机号码:" + dtivr.Rows[0]["Fmobile"].ToString();

                            ds.Tables[0].Rows[0]["FIVRResult"] = ivrresult;
                        }
                    }
                }

                return ds;
            }
            catch (LogicException err)
            {
                throw;
            }
            catch (Exception err)
            {
                throw new LogicException("Service处理失败！");
            }
            finally
            {
                da.Dispose();
                System.GC.Collect();
            }
        }

        [WebMethod(Description = "自助申诉批量领单函数")]
        public DataSet GetUserAppealLockList(DateTime BeginDate, DateTime EndDate, string fstate, string ftype, string QQType, string username, int Count, int SortType, string dotype)
        {
            return CFTUserAppealClass.GetLockList(BeginDate, EndDate, fstate, ftype, QQType, username, Count, SortType, dotype);
        }

        [WebMethod(Description = "自助申诉批量领单函数:分库表类型")]
        public DataSet GetUserAppealLockListDBTB(DateTime BeginDate, DateTime EndDate, string fstate, string ftype, string QQType, string username, int Count, int SortType, string dotype)
        {
            try
            {
                DataSet ds = new DataSet();
                CFTUserAppealClass cuser = new CFTUserAppealClass(BeginDate, EndDate, fstate, ftype, QQType, username, Count, SortType, dotype);
                ds = cuser.GetResultX("CFT");//查旧表


                DateTime date = BeginDate;
                int yearEnd = EndDate.Year;
                int monEnd = EndDate.Month;
                List<string> listdb = new List<string>();
                List<string> listtb = new List<string>();
                if (yearEnd >= 2014)//才查分库表
                {
                    while (date.Year < 2014)
                    {
                        date = date.AddMonths(1);
                    }
                    while (!((date.Year == yearEnd && date.Month > monEnd) || (date.Year > yearEnd)))
                    {
                        listdb.Add(date.Year.ToString());
                        listtb.Add(date.Month.ToString());
                        date = date.AddMonths(1);
                    }
                }

                DataSet dsFenResult = new DataSet();
                if (listdb == null || listdb.Count == 0)
                    dsFenResult = null;
                else
                {
                    int index = 0;//计数添加的有数据的数据表
                    for (int i = 0; i < listdb.Count; i++)
                    {
                        string db = listdb[i];
                        string tb = listtb[i];
                        CFTUserAppealClass cuser2 = new CFTUserAppealClass(BeginDate, EndDate, fstate, ftype, QQType, username, Count, SortType, dotype, true, db, tb);//分库分表的查询
                        DataSet dsfen = cuser2.GetResultX("CFTNEW");

                        if (dsfen != null && dsfen.Tables.Count > 0 && dsfen.Tables[0].Rows.Count > 0)
                        {
                            if (index == 0)
                            {
                                dsFenResult.Tables.Add(dsfen.Tables[0].Copy());
                                index++;
                            }
                            else
                            {
                                foreach (DataRow dr in dsfen.Tables[0].Rows)
                                {
                                    dsFenResult.Tables[0].ImportRow(dr);//将记录加入到一个表里
                                }
                            }
                        }
                    }
                }

                //将旧表与分库表数据写入一个表
                DataSet dsAll = NewMethod(ds, dsFenResult);

                return dsAll;
            }
            catch (Exception ex)
            {
                throw new Exception("批量领单获取数据出错：" + ex.Message);
            }
        }

        [WebMethod(Description = "自助申诉批量领单函数:分库表类型,内部处理函数")]
        public DataSet GetUserAppealLockListDBTBInnrFun(DataSet ds)
        {
            try
            {
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataSet ds1 = new DataSet();
                    DataTable dt1 = new DataTable();
                    dt1 = ds.Tables[0].Clone();
                    ds1.Tables.Add(dt1);

                    DataSet ds2 = new DataSet();
                    DataTable dt2 = new DataTable();
                    dt2 = ds.Tables[0].Clone();
                    ds2.Tables.Add(dt2);
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (dr["DBName"].ToString() == "")
                        {
                            dt1.ImportRow(dr);
                            string a = dt1.Rows[0]["Fid"].ToString();
                        }
                        else
                        {
                            dt2.ImportRow(dr);
                        }
                    }

                    if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                    {
                        CFTUserAppealClass.HandleParameter(ds1, true);
                    }
                    if (ds2 != null && ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
                    {
                        CFTUserAppealClass.HandleParameterByDBTBList(ds2, true);
                    }

                    //将旧表与分库表数据写入一个表
                    DataSet dsAll = NewMethod(ds1, ds2);
                    // string s = dsAll.Tables[0].Rows[0]["IsPass"].ToString();
                    return dsAll;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("批量领单获处理内部信息出错：" + ex.Message);
            }
        }

        [WebMethod(Description = "自助申诉批量领单函数:分库表类型,领单处理")]
        public DataSet GetUserAppealLockListDBTB2(DataSet ds, string username)
        {
            if (username == null || username == "")
            {
                throw new Exception("批量领单人不允许为空!");
            }

            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("CFT"));
            MySqlAccess da2 = new MySqlAccess(PublicRes.GetConnString("CFTNEW"));
            try
            {
                da.OpenConn();
                da2.OpenConn();
                string strSql = "";

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {

                    string WhereStr = "";
                    string db = "", tb = "";
                       
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        db = dr["DBName"].ToString();
                        tb = dr["tableName"].ToString();
                        if (db == null || db == "" || tb == null || tb == "")//旧表
                        {
                            WhereStr += "'" + dr["Fid"].ToString().Trim() + "',";
                        }
                        else//分库表
                        {
                            strSql = " update " + db + "." + tb + " set FPickUser='" + username + "',FPickTime=now(),FModifyTime=Now(),Fstate=8 where Fid='" + dr["Fid"].ToString().Trim() + "'";
                            da2.ExecSql(strSql);
                        }
                    }

                    if (WhereStr.Length > 0)
                    {
                        WhereStr = WhereStr.Substring(0, WhereStr.Length - 1);

                        strSql = " update db_appeal.t_tenpay_appeal_trans set FPickUser='" + username + "',FPickTime=now(),Fstate=8 where Fid in(" + WhereStr + ")";

                        da.ExecSql(strSql);
                    }
                }
                return ds;
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                da.Dispose();
                da2.Dispose();
            }

        }

        [WebMethod(Description = "自助申诉拒绝函数")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public bool CFTCancelAppeal(int fid, string reason, string OtherReason, string Fcomment, string UserName, string UserIP, out string msg)
        {
            msg = "";

            RightAndLog rl = new RightAndLog();
            try
            {              
                return CFTUserAppealClass.CancelAppeal(fid, reason, OtherReason, Fcomment, UserName, UserIP, out msg);
            }
            catch (LogicException err)
            {
                return false;
            }
            catch (Exception err)
            {
                return false;
            }
            finally
            {
                System.GC.Collect();
            }
        }

        [WebMethod(Description = "自助申诉拒绝函数")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public bool CFTCancelAppealDBTB(string fid, string db, string tb, string reason, string OtherReason, string Fcomment, string UserName, string UserIP, out string msg)
        {
            msg = "";

            RightAndLog rl = new RightAndLog();
            try
            {            
                return CFTUserAppealClass.CancelAppealDBTB(fid, db, tb, reason, OtherReason, Fcomment, UserName, UserIP, out msg);
            }
            catch (LogicException err)
            {
                return false;
            }
            catch (Exception err)
            {
                return false;
            }
            finally
            {
                System.GC.Collect();
            }
        }
     
        [WebMethod(Description = "自助申诉通过函数")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public bool CFTConfirmAppeal(int fid, string Fcomment, string UserName, string UserIP, out string msg)
        {
            msg = "";
            RightAndLog rl = new RightAndLog();
            try
            {             
                return CFTUserAppealClass.ConfirmAppeal(fid, Fcomment, UserName, UserIP, out msg);
            }
            catch (LogicException err)
            {
                return false;
            }
            catch (Exception err)
            {
                return false;
            }
            finally
            {
                System.GC.Collect();
            }
        }

        [WebMethod(Description = "自助申诉通过函数，分库表处理函数")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public bool CFTConfirmAppealDBTB(string fid, string db, string tb, string Fcomment, string UserName, string UserIP, out string msg)
        {
            msg = "";
            RightAndLog rl = new RightAndLog();
            try
            {             
                return CFTUserAppealClass.ConfirmAppealDBTB(fid, db, tb, Fcomment, UserName, UserIP, out msg);
            }
            catch (LogicException err)
            {             
                return false;
            }
            catch (Exception err)
            {             
                return false;
            }
            finally
            {
                System.GC.Collect();
            }
        }

        [WebMethod(Description = "自助申诉删除函数")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public bool CFTDelAppeal(int fid, string Fcomment, string UserName, string UserIP, out string msg)
        {
            msg = "";
            RightAndLog rl = new RightAndLog();
            try
            {             
                return CFTUserAppealClass.DelAppeal(fid, Fcomment, UserName, UserIP, out msg);
            }
            catch (LogicException err)
            {
                return false;
            }
            catch (Exception err)
            {
                return false;
            }
            finally
            {
                System.GC.Collect();
            }
        }

        [WebMethod(Description = "自助申诉删除函数")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public bool CFTDelAppealDBTB(string fid, string db, string tb, string Fcomment, string UserName, string UserIP, out string msg)
        {
            msg = "";
            RightAndLog rl = new RightAndLog();
            try
            {            
                return CFTUserAppealClass.DelAppealDBTB(fid, db, tb, Fcomment, UserName, UserIP, out msg);
            }
            catch (LogicException err)
            {
                return false;
            }
            catch (Exception err)
            {
                return false;
            }
            finally
            {
                System.GC.Collect();
            }
        }

        [WebMethod(Description = "客服统计查询函数")]
        public DataSet GetKFTotalQueryList(string User, DateTime u_BeginTime, DateTime u_EndTime, string OperationType, int iPageStart, int iPageMax)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("CFT"));
            try
            {
                da.OpenConn();

                string Sql = " select sum(SuccessNum) as SuccessNumSum,sum(FailNum) as FailNumSum,sum(DeleteNum) as DeleteNumSum,sum(OtherNum) as OtherNumSum," +
                             "sum(UserClassSuccessNum) as UserClassSuccessNumSum,sum(UserClassFailNum) as UserClassFailNumSum,sum(UserClassOtherNum) as UserClassOtherNumSum " +
                             "from db_appeal.t_tenpay_appeal_kf_total " +
                             "where user='" + User + "' and OperationType = '" + OperationType + "' and operationday between '" + u_BeginTime.ToString("yyyy-MM-dd") + "' and '" + u_EndTime.ToString("yyyy-MM-dd") + "'";

                return da.dsGetTotalData(Sql);
            }
            finally
            {
                da.Dispose();
            }
        }

        [WebMethod(Description = "测试")]
        public bool Test()
        {
            try
            {
                string fuin = HttpUtility.UrlEncode("2202088785", System.Text.Encoding.GetEncoding("GB2312"));
                string ftype = HttpUtility.UrlEncode("1", System.Text.Encoding.GetEncoding("GB2312"));
                string fstate = System.Web.HttpUtility.UrlEncode("1", System.Text.Encoding.GetEncoding("GB2312"));
                string cre_id = HttpUtility.UrlEncode("365245184521584965", System.Text.Encoding.GetEncoding("GB2312"));
                string clear_pps = HttpUtility.UrlEncode("1", System.Text.Encoding.GetEncoding("GB2312"));
                string is_answer = HttpUtility.UrlEncode("Y", System.Text.Encoding.GetEncoding("GB2312"));
                string mobile_no = HttpUtility.UrlEncode("13454824587", System.Text.Encoding.GetEncoding("GB2312"));
                string email = HttpUtility.UrlEncode("bruceliao@tencent.com", System.Text.Encoding.GetEncoding("GB2312"));
                string score = HttpUtility.UrlEncode("80", System.Text.Encoding.GetEncoding("GB2312"));
                string is_pass = HttpUtility.UrlEncode("Y", System.Text.Encoding.GetEncoding("GB2312"));
                string client_ip = HttpUtility.UrlEncode("127.0.0.1", System.Text.Encoding.GetEncoding("GB2312"));
                string reason = HttpUtility.UrlEncode("忘记密码", System.Text.Encoding.GetEncoding("GB2312"));
                string fcomment = HttpUtility.UrlEncode("二次审核,拒绝转审核通过.201110100224735133", System.Text.Encoding.GetEncoding("GB2312")); //备注
                string fcheck_info = HttpUtility.UrlEncode("上传的扫描件与财付通注册资料不符&", System.Text.Encoding.GetEncoding("GB2312"));   //拒绝原因

                string Data = "fuin=" + fuin + "&ftype=" + ftype + "&fstate=" + fstate + "&cre_id=" + cre_id + "&clear_pps=" + clear_pps + "&is_answer=" + is_answer +
                    "&mobile_no=" + mobile_no + "&email=" + email + "score=" + score + "&is_pass=" + is_pass + "&client_ip=" + client_ip +
                    "&reason=" + reason + "&fcomment=" + fcomment + "&fcheck_info=" + fcheck_info;
                Data = HttpUtility.UrlEncode(Data, System.Text.Encoding.GetEncoding("GB2312"));
                Data = "protocol=user_appeal_notify&version=1.0&data=" + Data;

                System.Text.Encoding GB2312 = System.Text.Encoding.GetEncoding("GB2312");
                byte[] sendBytes = GB2312.GetBytes(Data);

                string IP = ConfigurationManager.AppSettings["FK_UDP_IP"].ToString();
                string PORT = ConfigurationManager.AppSettings["FK_UDP_PORT"].ToString();
                TENCENT.OSS.CFT.KF.Common.UDP.GetUDPReplyNoReturn(sendBytes, IP, Int32.Parse(PORT));
                return true;
            }
            catch
            {
                return false;
            }

        }
      
        [WebMethod(Description = "修改QQ查询函数")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet GetChangeQQList(string userid, string qq, int iPageStart, int iPageMax)
        {
            RightAndLog rl = new RightAndLog();
            try
            {
                if (myHeader == null)
                {
                    throw new LogicException("不正确的调用方法！");
                }

                rl.actionType = "修改QQ查询函数";
                rl.ID = qq;
                rl.OperID = myHeader.OperID;
                rl.sign = 1;
                rl.strRightCode = "GetChangeQQList";
                rl.RightString = myHeader.RightString;
                rl.SzKey = myHeader.SzKey;
                rl.type = "查询";
                rl.UserID = myHeader.UserName;
                rl.UserIP = myHeader.UserIP;
   
                ChangeQQQueryClass cuser = new ChangeQQQueryClass(userid, qq);
                return cuser.GetResultX(iPageStart, iPageMax, "HT");

            }
            catch (LogicException err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                throw;
            }
            catch (Exception err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                throw new LogicException("Service处理失败！");
            }
            finally
            {
                rl.WriteLog();
            }
        }

        [WebMethod(Description = "修改QQ查询个数函数")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public int GetChangeQQListCount(string userid, string qq)
        {
            try
            {
                ChangeQQQueryClass cuser = new ChangeQQQueryClass(userid, qq);
                return cuser.GetCount("HT");
            }
            catch (Exception e)
            {
                throw new Exception("service发生错误,请联系管理员！");
                return 0;
            }
        }

        [WebMethod(Description = "查询QQ号码")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public bool GetQQByType(int ntype, string dbName, int istartIndex, int length, string whereStr, out DataSet ds, out string Msg)
        {
            ds = null;
            Msg = null;
            MySqlAccess da = null;

            try
            {
                string whereCommand = "";

                if (whereStr != "" && whereStr.Trim() != "")
                {
                    whereCommand += whereStr;
                }

                var index = int.Parse(dbName.Substring(dbName.Length - 2, 2));
                if (1 == ntype)
                {
                    da = new MySqlAccess(PublicRes.GetConnString("zw" + index));
                    da.OpenConn();
                    string fstrSql_count = "select count(*) from " + dbName + " " + whereCommand;
                    int count = Int32.Parse(da.GetOneResult(fstrSql_count));
                    string fstrSql = "select Fqqid," + count + " as icount from " + dbName + " " + whereCommand + "  order by Fqqid DESC limit " + istartIndex + "," + length; //查询最新的数据
                    ds = da.dsGetTotalData(fstrSql);
                }
                else
                {
                    da = new MySqlAccess(PublicRes.GetConnString("zw" + index));
                    da.OpenConn();
                    string fstrSql_count = "select count(*) from " + dbName + " " + whereCommand;
                    int count = Int32.Parse(da.GetOneResult(fstrSql_count));
                    string fstrSql = "select Fqqid," + count + " as icount from " + dbName + " " + whereCommand + "  order by Fqqid DESC limit " + istartIndex + "," + length; //查询最新的数据
                    ds = da.dsGetTotalData(fstrSql);
                }

                return true;
            }
            catch (Exception e)
            {
                Msg += "查询QQ号码失败" + PublicRes.replaceMStr(e.Message);
                PublicRes.WriteFile(Msg);
                return false;
            }
            finally
            {
                da.Dispose();
            }

        }
       
        /// <summary>
        /// 获取内部ID
        /// </summary>
        /// <param name="qqid"></param>
        /// <param name="Msg"></param>
        /// <returns></returns>
        [WebMethod(Description = "转换QQ号为UID")]
        public string QQ2Uid(string qqid)
        {
            return PublicRes.ConvertToFuid(qqid);
        }

        /// <summary>
        /// 获取内部ID,非正常帐户则返回空
        /// </summary>
        /// <param name="qqid"></param>
        /// <param name="Msg"></param>
        /// <returns></returns>
        [WebMethod(Description = "转换QQ号为UID")]
        public string QQ2UidX(string qqid)
        {
            return PublicRes.ConvertToFuidX(qqid);
        }

        [WebMethod(Description = "订单查询函数")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet GetQueryList(DateTime u_BeginTime, DateTime u_EndTime, string buyqq, string saleqq, string buyqqInnerID, string saleqqInnerID,
            string u_QueryType, string queryvalue, int fstate, int fcurtype, int iPageStart, int iPageMax)
        {
            try
            {
                OrderQueryClassZJ cuser = new OrderQueryClassZJ(u_BeginTime, u_EndTime, buyqq, saleqq, buyqqInnerID, saleqqInnerID, u_QueryType, queryvalue, fstate, fcurtype);

                if (u_QueryType != "FlistID" || queryvalue.Trim() == "")
                {
                    string dbConn = "";
                    if (buyqq.Trim() != "")
                    {
                        dbConn = PublicRes.GetBSBOrderDBString(buyqq, "", 2);
                    }
                    else if (saleqq.Trim() != "")
                    {
                        dbConn = PublicRes.GetBSBOrderDBString(saleqq, "", 2);
                    }
                    else if (buyqqInnerID.Trim() != "")
                    {
                        dbConn = PublicRes.GetBSBOrderDBString("", buyqqInnerID, 2);
                    }
                    else if (saleqqInnerID.Trim() != "")
                    {
                        dbConn = PublicRes.GetBSBOrderDBString("", saleqqInnerID, 2);
                    }
                    else
                    {
                        return null;
                    }
                    DataSet ds = cuser.GetResultX(iPageStart, iPageMax, dbConn);//支持多台DB的查询 20121112

                    DataSet dsForWX = new DataSet();
                    if (!string.IsNullOrEmpty(buyqqInnerID.Trim()))
                    {
                        dsForWX = (new TradeService()).QueryWxBuyOrderByUid(int.Parse(buyqqInnerID.Trim()), u_BeginTime, u_EndTime);//微信买家纬度订单

                        //添加将微信订单数据

                        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            GetResultFormWXOrder(dsForWX, ds);
                        }
                        else
                        {
                            DataTable dt = ds.Tables[0].Clone();
                            DataSet temp = new DataSet();
                            temp.Tables.Add(dt);
                            GetResultFormWXOrder(dsForWX, temp);
                            return temp;
                        }
                    }

                    return ds;
                }
                else
                {
                    string errMsg = "";
                    DataSet ds = CommQuery.GetDataSetFromICE(cuser.ICESQL, CommQuery.QUERY_ORDER, out errMsg);
                    return ds;
                }

            }
            catch (Exception err)
            {
                throw new Exception("查询出错！" + err.Message);
            }
        }

        /// <summary>
        /// 处理返回结果
        /// </summary>
        /// <param name="dsForWX"></param>
        /// <param name="ds"></param>
        private void GetResultFormWXOrder(DataSet dsForWX, DataSet ds)
        {
            if (dsForWX != null && dsForWX.Tables.Count > 0 && dsForWX.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsForWX.Tables[0].Rows)
                {
                    var newRow = ds.Tables[0].NewRow();
                    newRow["Fsale_bank_type"] = dr["Fsale_bank_type"];
                    newRow["Fsale_name"] = dr["Fsale_name"];
                    newRow["Fsaleid"] = dr["Fsaleid"];
                    newRow["Fsale_uid"] = dr["Fsale_uid"];
                    newRow["Fbuy_bankid"] = dr["Fbuy_bankid"];
                    newRow["Fbuy_bank_type"] = dr["Fbuy_bank_type"];
                    newRow["Fbuy_name"] = dr["Fbuy_name"];
                    newRow["Fbuyid"] = dr["Fbuyid"];
                    newRow["Fbuy_uid"] = dr["Fbuy_uid"];
                    newRow["Fpay_type"] = dr["Fpay_type"];
                    newRow["Fspid"] = dr["Fspid"];
                    newRow["Fcoding"] = dr["Fcoding"];
                    newRow["Flistid"] = dr["Flistid"];
                    newRow["Fuser_type"] = dr["Fuser_type"];
                    newRow["Fuid"] = dr["Fuid"];
                    newRow["Fpay_time"] = dr["Fpay_time"];
                    newRow["Fcreate_time"] = dr["Fcreate_time"];
                    newRow["Ffee3"] = dr["Ffee3"];
                    newRow["Ftoken"] = dr["Ftoken"];
                    newRow["Fcash"] = dr["Fcash"];
                    newRow["Fservice"] = dr["Fservice"];
                    newRow["Fprocedure"] = dr["Fprocedure"];
                    newRow["Ffact"] = dr["Ffact"];
                    newRow["Fpaynum"] = dr["Fpaynum"];
                    newRow["Fcarriage"] = dr["Fcarriage"];
                    newRow["Fprice"] = dr["Fprice"];
                    newRow["Flstate"] = dr["Flstate"];
                    newRow["Frefund_state"] = dr["Frefund_state"];
                    newRow["Ftrade_state"] = dr["Ftrade_state"];
                    newRow["Fcurtype"] = dr["Fcurtype"];
                    newRow["Fsale_bankid"] = dr["Fsale_bankid"];
                    newRow["Fstandby2"] = dr["Fstandby2"];
                    newRow["Fstandby1"] = dr["Fstandby1"];
                    newRow["Fgwq_listid"] = dr["Fgwq_listid"];
                    newRow["Fmedi_sign"] = dr["Fmedi_sign"];
                    newRow["Fappeal_sign"] = dr["Fappeal_sign"];
                    newRow["Fpaysale"] = dr["Fpaysale"];
                    newRow["Fpaybuy"] = dr["Fpaybuy"];
                    newRow["Frefund_type"] = dr["Frefund_type"];
                    newRow["Fchannel_id"] = dr["Fchannel_id"];
                    newRow["Ftrade_type"] = dr["Ftrade_type"];
                    newRow["Fmodify_time"] = dr["Fmodify_time"];
                    newRow["Fexplain"] = dr["Fexplain"];
                    newRow["Fmemo"] = dr["Fmemo"];

                    ds.Tables[0].Rows.Add(newRow);
                }
            }
        }

        [WebMethod(Description = "订单查询个数函数")]
        public int GetQueryListCount(DateTime u_BeginTime, DateTime u_EndTime, string buyqq, string saleqq, string buyqqInnerID, string saleqqInnerID,
            string u_QueryType, string queryvalue, int fstate, int fcurtype)
        {
            try
            {
                OrderQueryClassZJ cuser = new OrderQueryClassZJ(u_BeginTime, u_EndTime, buyqq, saleqq, buyqqInnerID, saleqqInnerID, u_QueryType, queryvalue, fstate, fcurtype);
                if (u_QueryType != "FlistID" || queryvalue.Trim() == "")
                {
                    string dbConn = "";
                    if (buyqq.Trim() != "")
                    {
                        dbConn = PublicRes.GetBSBOrderDBString(buyqq, "", 2);
                    }
                    else if (saleqq.Trim() != "")
                    {
                        dbConn = PublicRes.GetBSBOrderDBString(saleqq, "", 2);
                    }
                    else if (buyqqInnerID.Trim() != "")
                    {
                        dbConn = PublicRes.GetBSBOrderDBString("", buyqqInnerID, 2);
                    }
                    else if (saleqqInnerID.Trim() != "")
                    {
                        dbConn = PublicRes.GetBSBOrderDBString("", saleqqInnerID, 2);
                    }
                    else
                    {
                        return 0;
                    }

                    DataSet dsForWX = new DataSet();
                    if (!string.IsNullOrEmpty(buyqqInnerID.Trim()))
                    {
                        dsForWX = (new TradeService()).QueryWxBuyOrderByUid(int.Parse(buyqqInnerID.Trim()), u_BeginTime, u_EndTime);//微信买家纬度订单
                    }

                    int WxCount = 0;
                    if (dsForWX != null && dsForWX.Tables.Count > 0 && dsForWX.Tables[0].Rows.Count > 0)
                    {
                        WxCount = dsForWX.Tables[0].Rows.Count;
                    }

                    return (cuser.GetCount(dbConn) + WxCount);//支持多台DB的查询 20121112
                }
                else
                {
                    return 1;
                }
            }
            catch (Exception e)
            {
                throw new Exception("查询出错！" + e.Message);
                return 0;
            }
        }

        [WebMethod(Description = "订单查询详细函数")]
        public DataSet GetQueryListDetail(string listid)
        {
            try
            {
                OrderQueryClassZJ cuser = new OrderQueryClassZJ(listid);
                string errMsg = "";
                DataSet ds = CommQuery.GetDataSetFromICE(cuser.ICESQL, CommQuery.QUERY_ORDER, out errMsg);

                return ds;
            }
            catch (Exception e)
            {
                throw new Exception("service发生错误,请联系管理员！");
                return null;
            }
        }

        [WebMethod(Description = "查询订单的投诉单信息")]
        public DataSet GetAppealList(string listid)
        {         
            string strSql = "listid=" + listid;
            string errMsg = "";
            DataSet ds = CommQuery.GetDataSetFromICE(strSql, CommQuery.QUERY_APPEAL, out errMsg);

            return ds;
        }

        [WebMethod(Description = "查询订单的交易流水信息")]
        public DataSet GetUserpayList(string listid)
        {  
            try
            {
                string errMsg = "";            
                string strSql = "listid=" + listid;
                DataSet dssale = CommQuery.GetDataSetFromICE(strSql, CommQuery.QUERY_USERPAY_L, out errMsg);
            
                return dssale;     
            }
            catch (Exception e)
            {
                return null;
            }
        }

        [WebMethod(Description = "查询订单的物流单信息")]
        public DataSet GetTransportList(string listid)
        {           
            try
            {              
                string errMsg = "";
                string strSql = "listid=" + listid;
                return CommQuery.GetDataSetFromICE(strSql, CommQuery.QUERY_TRANSPORT, out errMsg);
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                //da.Dispose();
            }
        }

        [WebMethod(Description = "投诉查询函数")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet GetAppealQueryList(DateTime u_BeginTime, DateTime u_EndTime, string buyqq, string saleqq,
            string queryvalue, int fstate, int iPageStart, int iPageMax)
        {
            RightAndLog rl = new RightAndLog();
            try
            {
                if (myHeader == null)
                {
                    throw new LogicException("不正确的调用方法！");
                }

                rl.actionType = "订单查询函数";
                rl.ID = queryvalue;
                rl.OperID = myHeader.OperID;
                rl.sign = 1;
                rl.strRightCode = "GetQueryList";
                rl.RightString = myHeader.RightString;
                rl.SzKey = myHeader.SzKey;
                rl.type = "查询";
                rl.UserID = myHeader.UserName;
                rl.UserIP = myHeader.UserIP;
          
                AppealQueryClass cuser = new AppealQueryClass(u_BeginTime, u_EndTime, buyqq, saleqq, queryvalue, fstate);
          
                int start = iPageStart - 1;
                if (start < 0) start = 0;

                string strSql = cuser.ICESQL;
                strSql += "&strlimit=" + "limit " + start + "," + iPageMax;
                string errMsg = "";
                DataSet ds = CommQuery.GetDataSetFromICE(strSql, CommQuery.QUERY_APPEAL, out errMsg);
               
                return ds;

            }
            catch (LogicException err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceHtmlStr(err.Message);
                throw;
            }
            catch (Exception err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceHtmlStr(err.Message);
                throw new LogicException("Service处理失败！");
            }
            finally
            {
                rl.WriteLog();
            }
        }

        [WebMethod(Description = "投诉查询个数函数")]
        public int GetAppealListCount(DateTime u_BeginTime, DateTime u_EndTime, string buyqq, string saleqq,
            string queryvalue, int fstate)
        {
            try
            {
                AppealQueryClass cuser = new AppealQueryClass(u_BeginTime, u_EndTime, buyqq, saleqq, queryvalue, fstate);
                return cuser.GetCount("HT");
            }
            catch (Exception e)
            {
                throw new Exception("service发生错误,请联系管理员！");
                return 0;
            }
        }

        [WebMethod(Description = "投诉查询详细函数")]
        public DataSet GetAppealListDetail(string appealid)
        {
            try
            {
                AppealQueryClass cuser = new AppealQueryClass(appealid);
                string errMsg = "";
                DataSet ds = CommQuery.GetDataSetFromICE(cuser.ICESQL, CommQuery.QUERY_APPEAL, out errMsg);
               
                return ds;
            }
            catch (Exception e)
            {
                throw new Exception("service发生错误,请联系管理员！");
                return null;
            }
        }

      
        [WebMethod(Description = "实名认证处理详细函数")]
        public DataSet GetUserClassInfo(string qqid)
        {          
            string uid = PublicRes.ConvertToFuid(qqid);
            if (uid == null || uid.Trim() == "")
            {
                throw new Exception("找不到此用户");
            }

            string errMsg = "";
            string strSql = "uid=" + uid;
            return CommQuery.GetDataSetFromICE(strSql, CommQuery.QUERY_USERINFO, out errMsg);
        }

        [WebMethod(Description = "实名认证处理详细临时函数")]
        public DataSet GetUserClassInfoFlag(string qqid, out string errMsg)   //为了测试暂时加个
        {
            errMsg = "";

            string uid = PublicRes.ConvertToFuid(qqid);
            if (uid == null || uid.Trim() == "")
            {
                throw new Exception("找不到此用户");
            }

            string strSql = "uid=" + uid;
            DataSet ds;
            try
            {
                ds = CommQuery.GetDataSetFromICE(strSql, CommQuery.QUERY_USERINFO, out errMsg);
            }
            catch (Exception ex)
            {
                errMsg += uid;
                return null;
            }

            errMsg += uid;
            return ds;
        }


        [WebMethod(Description = "实名认证通过函数")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public bool UserClassConfirm(int flist_id, string UserName, out string msg)
        {
            msg = "";
            RightAndLog rl = new RightAndLog();
            try
            {            
                return UserClassClass.UserClassConfirm(flist_id, "RU", UserName, out msg);
            }
            catch (LogicException err)
            {              
                return false;
            }
            catch (Exception err)
            {               
                return false;
            }
            finally
            {
                System.GC.Collect();
            }
        }

        [WebMethod(Description = "实名认证通过后记录公安局返回的信息")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public void UpdateInfoFromPolice(string flist_id, string infomation)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("RU"));
            da.OpenConn();
            string strSql = string.Format(@" update authen_process_db.t_authening_info set Fpicktime = now(),Fmemo = '{0}'
					 where Flist_id = {1}", infomation, flist_id);
            da.ExecSqlNum(strSql);
        }

        [WebMethod(Description = "实名认证拒绝函数")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public bool UserClassCancel(int flist_id, string reason, string OtherReason, string UserName, out string msg)
        {
            msg = "";
            RightAndLog rl = new RightAndLog();
            try
            {            
                return UserClassClass.UserClassCancel(flist_id, reason, OtherReason, "RU", UserName, out msg);
            }
            catch (LogicException err)
            {   
                return false;
            }
            catch (Exception err)
            {
                return false;
            }
            finally
            {
                System.GC.Collect();
            }
        }

        [WebMethod(Description = "查询删除实名认证日志")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet GetUserClassDeleteList(string Fqqid)
        {
            try
            {
                return UserClassClass.GetDeleteList(Fqqid);
            }
            catch (Exception err)
            {
                return null;
            }
        }

        [WebMethod(Description = "获取退单失败数据记录个数函数")]
        public int GetRefundErrorCount(string batchid, string refundOrder, int orderType, string beginDate, string endDate, int refundType, string bankType,
            int refundPath, int handleType, int errorType, int refundState, string viewOldIds)
        {
            try
            {
                RefundErrorClass cuser = new RefundErrorClass(batchid, refundOrder, orderType, beginDate, endDate, refundType, bankType, refundPath, handleType, errorType, refundState, viewOldIds);

                return cuser.GetCount("ZWTK");
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        [WebMethod(Description = "获取退单失败记录")]
        public DataSet GetRefundErrorList(string batchid, string refundOrder, int orderType, string beginDate, string endDate, int refundType, string bankType,
            int refundPath, int handleType, int errorType, int refundState, bool truebankdata, string viewOldIds, int istart, int imax)
        {
            try
            {
                RefundErrorClass cuser = new RefundErrorClass(batchid, refundOrder, orderType, beginDate, endDate, refundType, bankType, refundPath, handleType, errorType, refundState, viewOldIds);
                DataSet ds = cuser.GetResultX(istart, imax, "ZWTK");

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null)
                {

                    #region 转化操作

                    DataTable dt = ds.Tables[0];
                    if (truebankdata)
                    {
                        PublicRes.GetTureBankList(dt, "fbank_listid");
                    }
                    else
                    {
                        PublicRes.GetTureBankListForView(dt, "fbank_listid");
                    }
                    TransferRefundOtherTable(dt);

                    #endregion

                    int c = ds.Tables[0].Rows.Count;
                    return ds;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception e)
            {
                return null;
            }
        }

        private void TransferRefundOtherTable(DataTable dt)
        {
            TransferRefundTotalTable(dt);

            dt.Columns.Add("FHandleTypeName", typeof(String));
            dt.Columns.Add("FerrorTypeName", typeof(String));
            dt.Columns.Add("FAuthorizeFlagName", typeof(String));

            foreach (DataRow dr in dt.Rows)
            {
                string tmp = dr["FHandleType"].ToString();
                if (tmp == "1")
                {
                    dr["FHandleTypeName"] = "待处理";
                }
                else if (tmp == "2")
                {
                    dr["FHandleTypeName"] = "处理中";
                }
                else if (tmp == "3")
                {
                    dr["FHandleTypeName"] = "处理完成";
                }
                else if (tmp == "4")
                {
                    dr["FHandleTypeName"] = "申请调状态";
                }
                else
                {
                    dr["FHandleTypeName"] = "未知状态" + tmp;
                }

                tmp = dr["FerrorType"].ToString();
                if (tmp == "1")
                {
                    dr["FerrorTypeName"] = "退单失败";
                }
                else if (tmp == "2")
                {
                    dr["FerrorTypeName"] = "手工调失败";
                }
                else if (tmp == "3")
                {
                    dr["FerrorTypeName"] = "系统校验置失败";
                }
                else
                {
                    dr["FerrorTypeName"] = "未知状态" + tmp;
                }

                tmp = dr["FAuthorizeFlag"].ToString();
                if (tmp == "0")
                {
                    dr["FAuthorizeFlagName"] = "未校验";
                }
                else if (tmp == "1")
                {
                    dr["FAuthorizeFlagName"] = "不完备";

                }
                else if (tmp == "2")
                {
                    //手工修改过备注
                    if (CheckRefundParamMake(dr["FOldID"].ToString()))
                    {
                        dr["FAuthorizeFlagName"] = "<FONT color=\"green\">附加信息完备</font>";
                    }
                    else
                    {
                        dr["FAuthorizeFlagName"] = "附加信息完备";
                    }
                }

            }

        }

        private void TransferRefundTotalTable(DataTable dt)
        {
            dt.Columns.Add("FreturnamtName", typeof(String));
            dt.Columns.Add("FamtName", typeof(String));
            dt.Columns.Add("FstateName", typeof(String));
            dt.Columns.Add("FreturnStateName", typeof(String));
            dt.Columns.Add("FrefundTypeName", typeof(String));
            dt.Columns.Add("FrefundPathName", typeof(String));
            dt.Columns.Add("FbanktypeName", typeof(String));
            dt.Columns.Add("FlstateName", typeof(String));
            dt.Columns.Add("FAdjustTypeName", typeof(String));
            dt.Columns.Add("Fbank_typeName", typeof(String));

            foreach (DataRow dr in dt.Rows)
            {
                string tmp = dr["Freturnamt"].ToString();
                dr["FreturnamtName"] = MoneyTransfer.FenToYuan(tmp);

                tmp = dr["Famt"].ToString();
                dr["FamtName"] = MoneyTransfer.FenToYuan(tmp);

                tmp = dr["Fstate"].ToString();
                if (tmp == "0")
                {
                    dr["FstateName"] = "初始状态";
                }
                else if (tmp == "1")
                {
                    dr["FstateName"] = "退单流程中";
                }
                else if (tmp == "2")
                {
                    dr["FstateName"] = "退单成功";
                }
                else if (tmp == "3")
                {
                    dr["FstateName"] = "退单失败";
                }
                else if (tmp == "4")
                {
                    dr["FstateName"] = "退单状态未定";
                }
                else if (tmp == "5")
                {
                    dr["FstateName"] = "手工退单中";
                }
                else if (tmp == "6")
                {
                    dr["FstateName"] = "申请手工处理";
                }
                else if (tmp == "7")
                {
                    dr["FstateName"] = "申请转入代发";
                }
                else if (tmp == "8")
                {
                    dr["FstateName"] = "挂异常处理中";
                }
                else
                {
                    dr["FstateName"] = "未知类型" + tmp;
                }

                tmp = dr["FreturnState"].ToString();

                if (tmp == "1")
                {
                    dr["FreturnStateName"] = "回导前";
                }
                else if (tmp == "2")
                {
                    dr["FreturnStateName"] = "回导后";
                }
                else
                {
                    dr["FreturnStateName"] = "未知类型" + tmp;
                }

                tmp = dr["FrefundType"].ToString();

                if (tmp == "1")
                {
                    dr["FrefundTypeName"] = "商户退单";
                }
                else if (tmp == "2")
                {
                    dr["FrefundTypeName"] = "对帐结果退单";
                }
                else if (tmp == "3")
                {
                    dr["FrefundTypeName"] = "人工录入退单";
                }
                else if (tmp == "4")
                {
                    dr["FrefundTypeName"] = "对帐异常退单";
                }
                else
                {
                    dr["FrefundTypeName"] = "未知类型" + tmp;
                }


                tmp = dr["FrefundPath"].ToString();
                if (tmp == "1")
                {
                    dr["FrefundPathName"] = "网银退单";
                }
                else if (tmp == "2")
                {
                    dr["FrefundPathName"] = "接口退单";
                }
                else if (tmp == "3")
                {
                    dr["FrefundPathName"] = "人工授权";
                }
                else if (tmp == "4")
                {
                    dr["FrefundPathName"] = "转帐退单";
                }
                else if (tmp == "5")
                {
                    dr["FrefundPathName"] = "转入代发";
                }
                else if (tmp == "6")
                {
                    dr["FrefundPathName"] = "付款退款";
                }
                else
                {
                    dr["FrefundPathName"] = "未知类型" + tmp;
                }

                tmp = dr["Flstate"].ToString();
                if (tmp == "1")
                {
                    dr["FlstateName"] = "正常";
                }
                else if (tmp == "2")
                {
                    dr["FlstateName"] = "锁定";
                }
                else if (tmp == "3")
                {
                    dr["FlstateName"] = "作废";
                }
                else
                {
                    dr["FlstateName"] = "未知类型" + tmp;
                }


                tmp = dr["FAdjustType"].ToString();
                if (tmp == "1")
                {
                    dr["FAdjustTypeName"] = "正常";
                }
                else if (tmp == "2")
                {
                    dr["FAdjustTypeName"] = "执行前调整";
                }
                else if (tmp == "3")
                {
                    dr["FAdjustTypeName"] = "执行后调整";
                }
                else
                {
                    dr["FAdjustTypeName"] = "未知类型" + tmp;
                }

            }
        }

        private bool CheckRefundParamMake(string oldId)
        {
            MySqlAccess dazw = new MySqlAccess(PublicRes.GetConnString("ZWTK"));
            try
            {
                dazw.OpenConn();
                string strSql = "Select count(*) from c2c_zwdb.t_refund_param where FOldId='" + oldId + "' and FUserMake=1";
                return Convert.ToInt32(dazw.GetOneResult(strSql)) > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                dazw.Dispose();
            }

        }

        [WebMethod(Description = "退单失败查询详细函数")]
        public DataSet GetRefundErrorDetail(string oldId)
        {
            try
            {
                RefundErrorClass cuser = new RefundErrorClass(oldId);

                DataSet ds = cuser.GetResultX(1, 1, "ZWTK");

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null)
                {
                    #region 转化操作
                    DataTable dt = ds.Tables[0];
                    PublicRes.GetTureBankListForView(dt, "fbank_listid");
                    PublicRes.GetTureBankListForView(dt, "Fbank_backid");
                    TransferRefundOtherTable(dt);
                    #endregion

                    return ds;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                throw new Exception("service发生错误,请联系管理员！");
                return null;
            }
        }


        [WebMethod(Description = "手机充值卡记录查询详细函数")]
        public DataSet GetFundCardListDetail(string flistid, string fsupplylist, string fcarrdid)
        {
            try
            {
                FundCardQueryClass cuser = new FundCardQueryClass(flistid, fsupplylist, fcarrdid);
                int iNum = cuser.GetCount("HD");
                if (iNum == 0)
                    return null;
                return cuser.GetResultX(1, iNum, "HD");
            }
            catch (Exception e)
            {
                throw new Exception("service发生错误,请联系管理员！" + e.Message);
                return null;
            }
        }
 
        #region 分帐查询
        [WebMethod(Description = "商户收支分离付款C帐号查询")]
        public DataSet GetSettleRuleList(string Fspid, int iPageStart, int iPageMax)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("AP"));
            iPageStart = iPageStart - 1;
            if (iPageStart < 0) iPageStart = 0;

            try
            {
                da.OpenConn();
                string sql = "select * from app_platform.t_settle_rule where Fspid = '" + Fspid + "' and Fcharge_method=2 limit " + iPageStart + "," + iPageMax;
                return da.dsGetTotalData(sql);
            }
            catch (Exception err)
            {
                return null;
            }
            finally
            {
                da.Dispose();
            }
        }

        //yinhuang 2013/09/24
        [WebMethod(Description = "分账请求查询")]
        public DataSet GetSettleReqList(string szListid, string reqid)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("AP"));
            try
            {
                if (szListid == null || string.IsNullOrEmpty(szListid.Trim()))
                {
                    throw new Exception("财付通订单号不能为空！");
                }
                szListid = szListid.Trim();

                da.OpenConn();
                string sql = "select * from app_platform_" + szListid.Substring(26, 2) + ".t_settle_request_" + szListid.Substring(25, 1) +
                    " where Flistid='" + szListid + "'";

                if (!string.IsNullOrEmpty(reqid.Trim()))
                {
                    reqid = reqid.Trim();
                    sql += " AND Fsettle_request_id='" + reqid + "'";
                }

                return da.dsGetTotalData(sql);
            }
            catch (Exception err)
            {
                throw new LogicException(err.Message);
            }
            finally
            {
                da.Dispose();
            }
        }

        [WebMethod(Description = "分账请求明细函数")]
        public DataSet GetSettleReqInfo(string szListid)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("AP"));
            try
            {
                if (szListid == null || string.IsNullOrEmpty(szListid.Trim()))
                {
                    throw new Exception("财付通订单号不能为空！");
                }
                szListid = szListid.Trim();
                da.OpenConn();
                string sql = "select * from app_platform_" + szListid.Substring(26, 2) + ".t_settle_list_" + szListid.Substring(25, 1) +
                    " where Flistid='" + szListid + "'";

                return da.dsGetTotalData(sql);
            }
            catch (Exception err)
            {
                throw new LogicException(err.Message);
            }
            finally
            {
                da.Dispose();
            }
        }

        [WebMethod(Description = "调帐查询函数")]
        public DataSet QueryAdjustList(string szListid, string spno, string spid, string adjustTime)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("AP"));
            try
            {
                int t = 0;
                string sql = "select * from app_platform.t_adjust where 1=1 ";
                if (szListid != null && !string.IsNullOrEmpty(szListid.Trim()))
                {
                    t++;
                    szListid = szListid.Trim();
                    sql += " AND  Ftransaction_id='" + szListid + "'";
                }
                if (spno != null && !string.IsNullOrEmpty(spno.Trim()))
                {
                    t++;
                    spno = spno.Trim();
                    sql += " AND  Fsp_bill_no='" + spno + "'";
                }
                if (spid != null && !string.IsNullOrEmpty(spid.Trim()))
                {
                    if (adjustTime == null || string.IsNullOrEmpty(adjustTime.Trim()))
                    {
                        throw new Exception("调帐时间不能为空！");
                    }
                    t++;
                    spid = spid.Trim();
                    sql += " AND  Fspid='" + spid + "' AND DATE_FORMAT(Fadjust_time,'%Y-%m-%d')='" + adjustTime + "'";
                }
                if (t < 1)
                {
                    throw new Exception("请输入查询条件！");
                }

                da.OpenConn();

                return da.dsGetTotalData(sql);
            }
            catch (Exception err)
            {
                throw new LogicException(err.Message);
            }
            finally
            {
                da.Dispose();
            }
        }

        [WebMethod(Description = "分账规则查询函数")]
        public DataSet QuerySettleRuleList(string spid)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("AP"));
            try
            {
                if (spid == null || string.IsNullOrEmpty(spid.Trim()))
                {
                    throw new Exception("商户号不能为空！");
                }
                spid = spid.Trim();
                da.OpenConn();
                string sql = "select * from app_platform.t_settle_rule where Fspid='" + spid + "'";

                return da.dsGetTotalData(sql);
            }
            catch (Exception err)
            {
                throw new LogicException(err.Message);
            }
            finally
            {
                da.Dispose();
            }
        }

        [WebMethod(Description = "商户权限查询函数")]
        public DataSet QuerySpControl(string spid)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("AP"));
            try
            {
                if (spid == null || string.IsNullOrEmpty(spid.Trim()))
                {
                    throw new Exception("商户号不能为空！");
                }
                spid = spid.Trim();
                da.OpenConn();
                string sql = "select * from app_platform.t_sp_control where Fspid='" + spid + "'";

                return da.dsGetTotalData(sql);
            }
            catch (Exception err)
            {
                throw new LogicException(err.Message);
            }
            finally
            {
                da.Dispose();
            }
        }

        [WebMethod(Description = "补差关系查询函数")]
        public DataSet QueryRelationOrderList(string szListid, string subListid)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("AP"));
            try
            {
                int t = 0;
                string sql = "select * from app_platform.t_relation_order where 1=1 ";
                if (szListid != null && !string.IsNullOrEmpty(szListid.Trim()))
                {
                    t++;
                    szListid = szListid.Trim();
                    sql += " AND Ftransaction_id='" + szListid + "'";
                }
                if (subListid != null && !string.IsNullOrEmpty(subListid.Trim()))
                {
                    t++;
                    subListid = subListid.Trim();
                    sql += " AND Fsub_listid='" + subListid + "'";
                }
                if (t < 1)
                {
                    throw new Exception("查询条件不能为空！");
                }

                da.OpenConn();
                DataSet ds = da.dsGetTotalData(sql);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ds.Tables[0].Columns.Add("Fuin", typeof(string));

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        sql = "select * from app_platform.t_trust_limit where Fspid='" + dr["Fspid"].ToString() + "'";
                        DataSet ds2 = da.dsGetTotalData(sql);
                        if (ds2 != null && ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
                        {
                            DataRow dr2 = ds2.Tables[0].Rows[0];
                            dr["Fuin"] = dr2["Fuin"];
                        }
                    }
                }

                return ds;
            }
            catch (Exception err)
            {
                throw new LogicException(err.Message);
            }
            finally
            {
                da.Dispose();
            }
        }

        [WebMethod(Description = "补差明细查询函数")]
        public DataSet QuerySubOrderList(string mergeListid, string listid)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("AP"));
            try
            {
                if (mergeListid == null || string.IsNullOrEmpty(mergeListid.Trim()))
                {
                    throw new Exception("原交易单不能为空！");
                }
                mergeListid = mergeListid.Trim();
                string sql = "select * from app_platform_" + mergeListid.Substring(26, 2) + ".t_sub_order_" + mergeListid.Substring(25, 1) +
                    " where Fmerge_listid='" + mergeListid + "'";

                if (listid != null && !string.IsNullOrEmpty(listid.Trim()))
                {
                    listid = listid.Trim();
                    sql += " AND Flistid='" + listid + "'";
                }

                da.OpenConn();

                return da.dsGetTotalData(sql);
            }
            catch (Exception err)
            {
                throw new LogicException(err.Message);
            }
            finally
            {
                da.Dispose();
            }
        }

        [WebMethod(Description = "补差账户查询函数")]
        public DataSet QueryTrueLimtList(string spid, string qqid)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("AP"));
            try
            {
                int t = 0;
                string sql = "select * from app_platform.t_trust_limit where Ftrust_rule&2=2 ";
                if (spid != null && !string.IsNullOrEmpty(spid.Trim()))
                {
                    t++;
                    spid = spid.Trim();
                    sql += " AND Fspid='" + spid + "'";
                }
                if (qqid != null && !string.IsNullOrEmpty(qqid.Trim()))
                {
                    t++;
                    qqid = qqid.Trim();
                    sql += " AND Fuin='" + qqid + "'";
                }
                if (t < 1)
                {
                    throw new Exception("查询条件不能为空！");
                }

                da.OpenConn();

                return da.dsGetTotalData(sql);
            }
            catch (Exception err)
            {
                throw new LogicException(err.Message);
            }
            finally
            {
                da.Dispose();
            }
        }

        [WebMethod(Description = "分账附加信息查询")]
        public DataSet GetSettleListAppend(string szListid)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("AP"));
            try
            {
                da.OpenConn();
                string sql = "select * from app_platform_" + szListid.Substring(26, 2) + ".t_air_order_append_" + szListid.Substring(25, 1) +
                    " where Flistid='" + szListid + "'";

                return da.dsGetTotalData(sql);
            }
            catch (Exception err)
            {
                throw new LogicException(err.Message);
            }
            finally
            {
                da.Dispose();
            }
        }

        [WebMethod(Description = "分账明细信息查询")]
        public DataSet GetSettleInfoListDetail(string szListid)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("AP"));
            try
            {
                da.OpenConn();
                string sql = "select * from app_platform_" + szListid.Substring(26, 2) + ".t_settle_stat_" + szListid.Substring(25, 1) +
                    " where Flistid='" + szListid + "' and Frole!=3";

                return da.dsGetTotalData(sql);
            }
            catch (Exception err)
            {
                return null;
            }
            finally
            {
                da.Dispose();
            }
        }
        [WebMethod(Description = "分账退款明细查询")]
        public DataSet GetSettleRefundListDetail(string szRefundId, string szListid)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("AP"));
            try
            {
                da.OpenConn();
                string sql = "select * from app_platform_" + szListid.Substring(26, 2) + ".t_settle_refund_" + szListid.Substring(25, 1) +
                                " where Flistid='" + szListid + "' and Fdrawid='" + szRefundId + "'order by Fmodify_time,Foper_type";

                return da.dsGetTotalData(sql);
            }
            catch (Exception err)
            {
                return null;
            }
            finally
            {
                da.Dispose();
            }
        }

        [WebMethod(Description = "代理分账关联关系")]
        public DataSet GetSpAgentRelation(string spid, string agentid, int start, int max)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("ZL"));
            try
            {
                da.OpenConn();
                string sql = "select * from c2c_db.t_agent_relation where ";
                if (spid.Length != 0)
                {
                    sql += " Fspid='" + spid + "'";
                    if (agentid.Length != 0)
                    {
                        sql += " and  Fagentid='" + agentid + "'";
                    }
                }
                else
                {
                    if (agentid.Length != 0)
                    {
                        sql += " Fagentid='" + agentid + "'";
                    }
                    else
                    {
                        return null;
                    }
                }

                sql = sql + " and  Fstate=2 and Fstandby1=1 order by Fmodify_time desc limit " + start + "," + max;

                return da.dsGetTotalData(sql);
            }
            catch (Exception err)
            {
                return null;
            }
            finally
            {
                da.Dispose();
            }
        }

        [WebMethod(Description = "调账订单查询")]
        public DataSet GetAirAdjustList(int iType, string Flistid, string szSpid, string szBeginDate, string szEndDate, int iPageStart, int iPageMax)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("AP"));
            iPageStart = iPageStart - 1;
            if (iPageStart < 0) iPageStart = 0;

            try
            {
                da.OpenConn();
                string sql = "select * from app_platform.t_adjust where ";

                if (iType == 1)
                {
                    sql += " Ftransaction_id='" + Flistid + "'";
                }
                else if (iType == 2)
                {
                    sql += " Fsp_bill_no='" + Flistid + "'";
                }
                else
                {
                    sql += " Fspid='" + szSpid + "' and Fmodify_time>='" + szBeginDate + "' and Fmodify_time<='" + szEndDate + "' order by Fmodify_time desc";
                }

                sql = sql + " limit " + iPageStart + "," + iPageMax;

                return da.dsGetTotalData(sql);
            }
            catch (Exception err)
            {
                return null;
            }
            finally
            {
                da.Dispose();
            }
        }

        [WebMethod(Description = "冻结解冻")]
        public DataSet GetAirFreeze(string szSpid, string szQQid, string szBeginDate, string szEndDate, int iPageStart, int iPageMax)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("AP"));
            iPageStart = iPageStart - 1;
            if (iPageStart < 0) iPageStart = 0;

            try
            {
                da.OpenConn();
                string sql = "select * from app_platform.t_freeze_roll where Fcreate_time>='" + szBeginDate + "' and Fcreate_time<='" + szEndDate + "'";

                if (szSpid != "")
                {
                    sql += " and Fspid='" + szSpid + "'";
                }

                if (szQQid != "")
                {
                    sql += " and Fqqid='" + szQQid + "'";
                }

                sql += " and Frole!=3  order by Fcreate_time desc ";

                sql = sql + " limit " + iPageStart + "," + iPageMax;

                return da.dsGetTotalData(sql);
            }
            catch (Exception err)
            {
                return null;
            }
            finally
            {
                da.Dispose();
            }
        }

        [WebMethod(Description = "分账退款查询")]
        public DataSet GetSettleRefundList(string Flistid, int iQueryType, int iPageStart, int iPageMax)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("AP"));
            iPageStart = iPageStart - 1;
            if (iPageStart < 0) iPageStart = 0;

            try
            {
                da.OpenConn();
                string sql = "select * from app_platform.t_spm_refund where ";

                if (iQueryType == 1)
                {
                    sql += " Ftransaction_id='" + Flistid;
                }
                else
                {
                    sql += " Fdraw_id='" + Flistid;
                }

                sql = sql + "' limit " + iPageStart + "," + iPageMax;

                return da.dsGetTotalData(sql);
            }
            catch (Exception err)
            {
                return null;
            }
            finally
            {
                da.Dispose();
            }
        }
        //bruceliao 20080807
        [WebMethod(Description = "分帐业务查询函数")]
        public DataSet GetSeparateOperationList(string flistid, int start, int max)
        {
            Ice.Communicator ic = null;
            int total = 0;
            AMSManager.AccRollRecord[] rolls;
            int errCode = 0;
            string errInfo = "";
            try
            {

                if (ic != null)
                {
                    ic.destroy();
                    ic = null;
                }

                string serverIP = ConfigurationManager.AppSettings["ICEServerIP2"];
                string serverPort = ConfigurationManager.AppSettings["ICEPort2"];
                string initStr = "AMSManager:tcp -h " + serverIP + " -p " + serverPort;

                String[] args = { "AMSManager" };
                ic = Ice.Util.initialize(ref args);

                Ice.ObjectPrx obj = ic.stringToProxy(initStr);

                AMSManager.IAMSManagerPrx manager = AMSManager.IAMSManagerPrxHelper.checkedCast(obj);
                if (manager == null)
                    throw new ApplicationException("Invalid proxy");
                string spid = flistid.Substring(0, 10);
                manager.QueryTransBankRollList(spid, flistid, start, max, out total, out rolls, out errCode, out errInfo);
                /*
                    string listid;  // 交易单号
                    int type;  // 借贷类型：1-入; 2-出;
                    int subject; // 科目、类别：见数据字典 2.1.13
                    string paynum; // 金额
                    int bankType; // 用户银行的类型
                    int sign; // 流水的标记：0 正常 1 被冲正 2 冲正
                    int actionType; //Faction_type

                    string fromId; // 对方的ID（QQ号码，银行帐号）
                    string fromName; // 对方的名称
                    string balance;  // 账户余额
                    string memo; // 转账说明
                    string modifyTime; // 最后修改时间
                */
                if (total > 0)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("listid", typeof(string));
                    dt.Columns.Add("modifyTime", typeof(string));
                    dt.Columns.Add("subject", typeof(string));
                    dt.Columns.Add("fromId", typeof(string));
                    dt.Columns.Add("fromName", typeof(string));
                    dt.Columns.Add("type", typeof(string));
                    dt.Columns.Add("paynum", typeof(string));
                    for (int i = 0; i < rolls.Length; i++)
                    {
                        DataRow dr = dt.NewRow();
                        dr["listid"] = rolls[i].listid.ToString();
                        dr["modifyTime"] = rolls[i].modifyTime.ToString();
                        dr["subject"] = rolls[i].subject.ToString();
                        dr["fromId"] = rolls[i].fromId.ToString();
                        dr["fromName"] = rolls[i].fromName.ToString();
                        dr["type"] = rolls[i].type.ToString();
                        dr["paynum"] = rolls[i].paynum.ToString();
                        dt.Rows.Add(dr);
                    }

                    DataSet ds = new DataSet();
                    ds.Tables.Add(dt);
                    return ds;
                }
                else
                    return null;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message + errInfo);
            }
            finally
            {
                if (ic != null)
                    ic.destroy();
            }
        }

        //该函数未被调用
        [WebMethod(Description = "分帐业务查询函数详情")]
        public DataSet GetSeparateOperationDetail(DateTime Starttime, DateTime Endtime, string Flistid, string Fuid)
        {
            MySqlAccess daFuid = new MySqlAccess(PublicRes.GetConnString("t_user_order_bsb", Fuid.Substring(Fuid.Length - 2)));
            try
            {
                DataSet ds = new DataSet();
                DataSet ds1 = new DataSet();
                DataSet ds2 = new DataSet();
                DataSet ds3 = new DataSet();

                string errMsg = "";

                string strWhere = "start_time=" + Starttime.ToString("yyyy-MM-dd");
                strWhere += "&listid=" + Flistid;

                if (!CommQuery.GetDataFromICE(strWhere, CommQuery.交易单资金流水_商户, out errMsg, out ds1))
                {
                    throw new Exception(errMsg);
                }
   
                if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                {
                    ds = ds1;
                }

                DateTime NewStart = new DateTime(Starttime.Year, Starttime.Month, 1);
                DateTime NewEnd = new DateTime(Endtime.Year, Endtime.Month, 1);


                while (NewStart <= NewEnd)
                {
                    strWhere = "start_time=" + NewStart.ToString("yyyy-MM-dd");
                    strWhere += "&listid=" + Flistid;

                    NewStart = NewStart.AddMonths(1);

                    if (!CommQuery.GetDataFromICE(strWhere, CommQuery.交易单资金流水_商户, out errMsg, out ds2))
                    {
                        throw new Exception(errMsg);
                    }

                    //把这次得到的表内容合并入ds中。
                    if (ds2 == null || ds2.Tables.Count == 0 || ds2.Tables[0] == null || ds2.Tables[0].Rows.Count == 0)
                    {
                        continue;
                    }

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds2.Tables[0].Rows)
                        {
                            ds.Tables[0].Rows.Add(dr.ItemArray);
                        }
                    }
                    else
                    {
                        ds = ds2;
                    }
                }


                //根据Fuid抓取Flistid,Fcreate_time //**t_user_order需要修改
                DataSet dsFuid = new DataSet();
                daFuid.OpenConn();
                string strSqlFuid = "select Flistid, Fcreate_time from " + PublicRes.GetTName("t_user_order", Fuid) + " where Fcoding='" + Flistid + "' order by Fcreate_time";
                dsFuid = daFuid.dsGetTotalData(strSqlFuid);

                for (int i = 0; i < dsFuid.Tables[0].Rows.Count; i++)
                {
                    strWhere = "start_time=" + DateTime.Parse(dsFuid.Tables[0].Rows[i]["Fcreate_time"].ToString()).ToString("yyyy_MM-dd");
                    strWhere += "&listid=" + dsFuid.Tables[0].Rows[i]["Flistid"].ToString();

                    if (!CommQuery.GetDataFromICE(strWhere, CommQuery.交易单资金流水_商户, out errMsg, out ds3))
                    {
                        throw new Exception(errMsg);
                    }

                    //把这次得到的表内容合并入ds中。
                    if (ds3 == null || ds3.Tables.Count == 0 || ds3.Tables[0] == null || ds3.Tables[0].Rows.Count == 0)
                    {
                        continue;
                    }

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds3.Tables[0].Rows)
                        {
                            ds.Tables[0].Rows.Add(dr.ItemArray);
                        }
                    }
                    else
                    {
                        ds = ds3;
                    }
                }

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        dr.BeginEdit();
                        dr["Fpaynum"] = MoneyTransfer.FenToYuan(dr["Fpaynum"].ToString());
                        dr["Fbalance"] = MoneyTransfer.FenToYuan(dr["Fbalance"].ToString());
                        dr.EndEdit();
                    }

                    ds.Tables[0].DefaultView.Sort = "Fmodify_time ";
                    return ds;
                }
                else
                {
                    return null;
                }              
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                daFuid.Dispose();
            }
        }


        [WebMethod(Description = "分帐订单查询（Flistid）函数")]
        public DataSet GetSeparateListForFlistid(string Flistid, int start, int max)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("AP"));
            if (start < 0) start = 0;

            try
            {
                da.OpenConn();
                string sql = "select Flistid,Fcoding,Fcreate_time as TimeStr,Ftotal_fee as Fpaynum,Fsttl_fee,Frefund_fee,Frf_fee,Fadjustout_fee,Fadjustin_fee,Foffer_fee,Foffer_sign " +
                              "from app_platform_" + Flistid.Substring(26, 2).ToString() + ".t_air_order_append_" + Flistid.Substring(25, 1).ToString() +
                              " where Flistid = '" + Flistid + "' limit " + start + "," + max;
                return da.dsGetTotalData(sql);
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
            finally
            {
                da.Dispose();
            }
        }

        [WebMethod(Description = "分帐订单查询（Fspid）函数")]
        public DataSet GetSeparateListForFspid(string Fspid, DateTime BeginDate, DateTime EndDate, int start, int max)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("AP"));
            if (start < 0) start = 0;

            try
            {
                da.OpenConn();
                string sql = "select distinct Flistid,Fcoding,Fpay_time as TimeStr,Fpaynum,Fsttl_fee,Frefund_fee,Frf_fee,Fadjustout_fee,Fadjustin_fee,Foffer_fee,Foffer_sign " +
                              "from app_platform.t_air_order_" + BeginDate.Month.ToString().PadLeft(2, '0') +
                              " where Fspid = '" + Fspid + "' and Fcreate_time >= '" + BeginDate.ToString("yyyy-MM-dd 00:00:00") +
                              "' and Fcreate_time<= '" + EndDate.ToString("yyyy-MM-dd 23:59:59") + "'" + "and Ftrade_state!=1 and Flstate=2" +
                              " limit " + start + "," + max;
                return da.dsGetTotalData(sql);
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
            finally
            {
                da.Dispose();
            }
        }

        #endregion

        [WebMethod(Description = "代收付费查询函数")]
        public DataSet QueryFeeDataList(string ftype, string istate, string ipaystate, string listid, int start, int max)
        {
            if (listid == null || listid == "")
            {
                throw new Exception("代缴费信息查询失败！请输入交易单号");
            }
            DateTime payTime = DateTime.Now;
            try
            {
                payTime = Convert.ToDateTime(listid.Substring(10, 4) + "-" + listid.Substring(14, 2) + "-" + listid.Substring(16, 2));

            }
            catch
            {
                throw new Exception("查询代缴费信息，时间转换失败:" + listid);
            }

            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("ZW"));

            try
            {
                da.OpenConn();

                string strWhere = " where 1=1 ";

                if (istate != "9")
                {
                    strWhere += " and Fstate=" + istate;
                }
                if (ipaystate != "9")
                {
                    strWhere += " and FPaytype=" + ipaystate;
                }
                if (ftype != "9")
                {
                    strWhere += " and Ftype='" + ftype + "'";
                }

                if (listid != null && listid.Trim() != "")
                {
                    strWhere += " and FListid='" + listid.Trim() + "' ";
                }

                string Sql = "select * from c2c_zwdb.t_sporder_" + payTime.ToString("yyyyMM") + strWhere;
                Sql += " union select * from c2c_zwdb.t_sporder_" + payTime.AddMonths(-1).ToString("yyyyMM") + strWhere;
                Sql += " union select * from c2c_zwdb.t_sporder_" + payTime.AddMonths(1).ToString("yyyyMM") + strWhere + " limit " + start + "," + max;

                return da.dsGetTotalData(Sql);
            }
            catch (Exception err)
            {
                throw new Exception("查询代缴费信息失败！" + err.Message);
            }
            finally
            {
                da.Dispose();
            }
        }

        #region 商户信息查询

        [WebMethod(Description = "直付商户查询函数")]
        public DataSet GetPayBusinessList(string CompanyName, string CompanyID, string URL, string WebName)
        {
            DataSet ds = null;
            try
            {
                ds = new SPOAService().GetOneValueAddedTax(CompanyID);             
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return ds;
        }

        [WebMethod(Description = "查询商户的内部ID")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public bool GetMerchantMidUid(string spid, out string midUid, out string msg)
        {
            msg = "";
            midUid = "";
            try
            {
                if (spid == null || spid == "")
                {
                    msg = "传入的Spid不能为空！";
                    return false;
                }
                if (spid == "0")
                {
                    midUid = "0";
                    return true;
                }

                string ErrMsg;
                string strSql = "spid=" + spid;
                midUid = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_MERCHANTINFO, "Fuidmiddle", out ErrMsg);

                if (midUid == null || midUid.Trim() == "")
                {
                    msg = "对应的内部ID有误";
                    return false;
                }

                return true;

            }
            catch (Exception err)
            {
                msg = err.Message;
                return false;
            }
        }

        [WebMethod(Description = "查询商户保证金账户")]
        public DataSet GetInsureAccount(string spid, string insureUIn, out string msg)
        {
            StringBuilder sbSql = new StringBuilder();

            sbSql.Append("uid=").Append(spid);

            if (insureUIn != null)
                sbSql.Append("&insure_uin=").Append(insureUIn);

            DataSet ds = CommQuery.GetDataSetFromICE(sbSql.ToString(), "QUERY_USER_INSURE", out msg);
            return ds;
        }

        //如果是管理员,则spid=qqid;如果是操作员qqid请传操作员账号
        [WebMethod(Description = "商户号和别名互查函数")]
        public string QuerySpidOrSpalias(string spid, string alias, string qqid)
        {
            string ret = "";
            string Msg = "";
            try
            {
                int flag = 0;
                string pars = "";
                if (!string.IsNullOrEmpty(alias))
                {
                    flag = 1;
                    pars = "flstate:1|alias:" + alias.Trim();
                }
                else if (!string.IsNullOrEmpty(spid))
                {
                    flag = 2;
                    pars = "flstate:1|spid:" + spid.Trim();
                }
                if (!string.IsNullOrEmpty(qqid))
                {
                    //通过别名查商户号，不传qqid，如果qqid=spid，那么是管理员，否则是操作员
                    pars += "|qqid:" + qqid.Trim();
                }

                if (flag == 0)
                {
                    throw new Exception("查询参数为空");
                }
                string req_a = "request_type=4006&flag=2&reqid=2308&offset=0&limit=1&fields=" + pars;

                DataSet ds_a = CommQuery.GetXmlToDataSetFromICE(req_a, "", "common_query_service", out Msg);
                if (Msg != "")
                {
                    throw new Exception(Msg);
                }

                if (ds_a != null && ds_a.Tables.Count > 0 && ds_a.Tables[0].Rows.Count > 0)
                {
                    if (flag == 1)
                    {
                        //别名查商户号
                        ret = ds_a.Tables[0].Rows[0]["Fspid"].ToString();
                    }
                    else if (flag == 2)
                    {
                        //商户号查别名
                        ret = ds_a.Tables[0].Rows[0]["Fstandby7"].ToString();
                    }
                }
            }
            catch (Exception e)
            {
                throw new LogicException("service处理失败：" + e.Message);
            }

            return ret;
        }

        [WebMethod(Description = "拍拍店铺白名单查询函数")]
        public DataSet PaiPaiBMDList(string qq)
        {
            try
            {
                var parameters = Encoding.Default.GetBytes("_dbagent_version_=2&serviceid=paipai_conf&ruleid=paipai_conf_query&uin=" + qq + "\r\n");
                string msg = "";
                string IP = ConfigurationManager.AppSettings["PaiPaiBMD_IP"];
                string PORT = ConfigurationManager.AppSettings["PaiPaiBMD_PORT"];
                string answer = UDP.GetTCPReplyString(parameters, IP, Int32.Parse(PORT), out msg);
                Hashtable paramsHt = new Hashtable();
                DataSet ds = new DataSet();
                DataTable dt = ds.Tables.Add();
                if (answer != "")
                {
                    paramsHt = UDP.tcpParameters(answer);

                    if (paramsHt.Contains("ROWSET") && paramsHt.Contains("TOTALROWS"))//存在这两个参数，说明有记录结果
                    {
                        //解析每一条记录
                        List<string[]> list = new List<string[]>();
                        foreach (String item in Regex.Split((string)paramsHt["ROWDATA"], ";", RegexOptions.IgnoreCase))
                        {
                            if (item != null && item != "")
                            {
                                String[] row = Regex.Split(item, ",", RegexOptions.IgnoreCase);
                                list.Add(row);
                            }
                        }
                        dt.Columns.Add("SalerQQ", typeof(string));
                        dt.Columns.Add("UsePlace", typeof(string));
                        dt.Columns.Add("UseOrNot", typeof(string));
                        dt.Columns.Add("Describe", typeof(string));
                        int size = Int32.Parse((string)paramsHt["TOTALROWS"]);
                        foreach (String[] r in list)
                        {
                            DataRow dr = dt.NewRow();
                            dt.Rows.Add(dr);
                            dr["SalerQQ"] = r[0];
                            dr["UsePlace"] = r[2];
                            dr["UseOrNot"] = r[4];
                            dr["Describe"] = r[5];
                            if (dr["UseOrNot"].ToString() == "1")
                            {
                                dr["UseOrNot"] = "是";
                            }
                            else
                            {
                                dr["UseOrNot"] = "否";
                            }
                        }
                    }
                    return ds;
                }
                else
                {
                    throw new Exception(msg);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [WebMethod(Description = "直付商户查询函数查询单笔")]
        public DataSet GetPayBusinessInfo(string KeyID)
        {
            try
            {
                return new SPOAService().GetSpInfo(null, KeyID, null, null, null, null, 1, 0);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [WebMethod(Description = "根据商户号查直付商户资料")]
        public DataSet GetPayBusinessInfoList(string Fspid)
        {
            try
            {
                return new SPOAService().GetOneValueAddedTax(Fspid);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [WebMethod(Description = "直付商户查询函数根据商户号查询余额和提现类型")]
        public DataSet GetPayBusinessElseInfo(string KeyID)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("HT"));
            ICEAccess ice = new ICEAccess(PublicRes.ICEServerIP, PublicRes.ICEPort);
            try
            {
                da.OpenConn();
       
                string Msg = "";
                string strSql = "spid=" + KeyID;
                string f_strID = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_MERCHANTINFO, "FuidMiddle", out Msg);

                if (f_strID == null || f_strID == "")
                {
                    return null;
                }

                ice.OpenConn();
                string strwhere = "where=" + ICEAccess.URLEncode("fuid=" + f_strID + "&");
                strwhere += ICEAccess.URLEncode("fcurtype=1&");

                string strResp = "";
                DataTable dt = ice.InvokeQuery_GetDataTable(YWSourceType.商户资源, YWCommandCode.查询商户信息, f_strID, strwhere, out strResp);
                if (dt == null || dt.Rows.Count == 0)
                    return null;

                ice.CloseConn();

                string Fbalance = dt.Rows[0]["Fbalance"].ToString();

                if (Fbalance == null || Fbalance.Trim() == "")
                {
                    return null;
                }
                else
                {
                    Fbalance = MoneyTransfer.FenToYuan(Fbalance);
         
                    strSql = "spid=" + KeyID;
                    string Fstandby1 = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_MERCHANTINFO, "Fstandby1", out Msg);

                    strSql = "select '" + Fbalance + "' as Fbalance,'" + Fstandby1 + "' as Fstandby1";
                    return da.dsGetTotalData(strSql);
                }
            }
            finally
            {
                ice.Dispose();
                da.Dispose();
            }
        }

        [WebMethod(Description = "直付商户查询修改联系人资料")]
        public void ModifyPayBusinessInfo(string TableFlag, string KeyID, string ContactUser, string ContactPhone, string ContactMobile, string ContactQQ, string ContactEmail, string CompanyAddress, string Postalcode)
        {
            try
            {           
                new SPOAService().ModifyPayBusinessInfo(TableFlag, KeyID, ContactUser, ContactPhone, ContactMobile, ContactQQ, ContactEmail, CompanyAddress, Postalcode);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        [WebMethod(Description = "中介商户查询函数")]
        public DataSet GetAgencyBusinessList(string Fqqid, string Fdomain, int offset, int qcount)
        {
            DataSet ds = null;
            try
            {            
                ds = new SPOAService().GetAgencyBusinessList(Fqqid, Fdomain, offset, qcount);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ds.Tables[0].Columns.Add("Sflag", typeof(String));//选择,用做详情是查哪个方法

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        //用户账号查询，没有管理员别名
                        dr.BeginEdit();
                        dr["Sflag"] = "1";
                        dr.EndEdit();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return ds;
        }


        [WebMethod(Description = "中介商户查询函数查询单笔")]
        public DataSet GetAgencyBusinessInfo(string Fid)
        {
            try
            {
                return new SPOAService().GetAgencyBusinessInfo(Fid);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [WebMethod(Description = "通过商户号查询中介商户函数")]
        public DataSet QueryAgencyBySpid(string spid)
        {
            DataSet ds = null;
            try
            {
                if (string.IsNullOrEmpty(spid))
                {
                    throw new Exception("商户号不能为空");
                }

                ds = new SPOAService().QueryAgencyBySpid(spid);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ds.Tables[0].Columns.Add("Sflag", typeof(String));//选择,用做详情是查哪个方法
                    ds.Tables[0].Columns.Add("SpAlias", typeof(String));//别名

                    ds.Tables[0].Columns.Add("Fid", typeof(String));//fid 唯一
                    ds.Tables[0].Columns.Add("Fqqid", typeof(String));//账号
                    ds.Tables[0].Columns.Add("Fdomain", typeof(String));//域名
                    ds.Tables[0].Columns.Add("Femail", typeof(String));//email
                    ds.Tables[0].Columns.Add("Fcreate_time", typeof(String));//创建时间

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        string s = QuerySpidOrSpalias(dr["SPID"].ToString(), "", dr["SPID"].ToString()); //通过商户号查管理员别名
                        dr.BeginEdit();
                        dr["SpAlias"] = s;
                        dr["Sflag"] = "2"; //商户号查询

                        dr["Fid"] = dr["ApplyCpInfoID"].ToString();
                        dr["Fqqid"] = dr["QQID"].ToString();
                        dr["Fdomain"] = dr["WWWAdress"].ToString();
                        dr["Femail"] = dr["ContactEmail"].ToString();
                        dr["Fcreate_time"] = "";

                        dr.EndEdit();

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return ds;
        }

        [WebMethod(Description = "通过ID查中介商户详情函数")]
        public DataSet QueryAgencyInfoById(string fid)
        {
            DataSet ds = null;
            try
            {
    
                ds = new SPOAService().QueryAgencyInfoById(fid);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ds.Tables[0].Columns.Add("Fid", typeof(String));//fid 唯一
                    ds.Tables[0].Columns.Add("Fqqid", typeof(String));//账号
                    ds.Tables[0].Columns.Add("Fdomain", typeof(String));//域名
                    ds.Tables[0].Columns.Add("Femail", typeof(String));//email
                    ds.Tables[0].Columns.Add("Ftel", typeof(String));//电话
                    ds.Tables[0].Columns.Add("FMobile", typeof(String));//手机
                    ds.Tables[0].Columns.Add("FName", typeof(String));//姓名
                    ds.Tables[0].Columns.Add("Faddress", typeof(String));//地址
                    ds.Tables[0].Columns.Add("Fpostcode", typeof(String));//邮编
                    ds.Tables[0].Columns.Add("Fmemo", typeof(String));//账户备注
                    ds.Tables[0].Columns.Add("Fmodify_time", typeof(String));//修改时间
                    ds.Tables[0].Columns.Add("Fcheck_id", typeof(String));//审核ID
                    ds.Tables[0].Columns.Add("Fcheck_user", typeof(String));//审核人
                    ds.Tables[0].Columns.Add("Fcheck_time", typeof(String));//审核时间
                    ds.Tables[0].Columns.Add("Fsuggester", typeof(String));//推荐人
                    ds.Tables[0].Columns.Add("Fop_memo", typeof(String));//操作备注
                    ds.Tables[0].Columns.Add("Fcreate_time", typeof(String));//创建时间

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        dr.BeginEdit();
                        dr["Fid"] = "";
                        dr["Fqqid"] = dr["QQID"].ToString();
                        dr["Fdomain"] = dr["WWWAdress"].ToString();
                        dr["Femail"] = dr["ContactEmail"].ToString();
                        dr["Ftel"] = dr["ContactPhone"].ToString();
                        dr["FMobile"] = dr["ContactMobile"].ToString();
                        dr["FName"] = dr["ContactUser"].ToString();
                        dr["Faddress"] = dr["CompanyAddress"].ToString();
                        dr["Fpostcode"] = dr["Postalcode"].ToString();
                        dr["Fmemo"] = "";
                        dr["Fmodify_time"] = "";
                        dr["Fcheck_id"] = dr["CheckUserID"].ToString();
                        dr["Fcheck_user"] = dr["CheckUserName"].ToString();
                        dr["Fcheck_time"] = dr["CheckTime"].ToString();
                        dr["Fsuggester"] = dr["SuggestUser"].ToString();
                        dr["Fop_memo"] = "";
                        dr["Fcreate_time"] = "";

                        dr.EndEdit();

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return ds;
        }

        [WebMethod(Description = "收易付查询函数")]
        public DataSet GetShouFuYiList(string qq)
        {
            try
            {
                return new SPOAService().GetShouFuYiList(qq);   
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [WebMethod(Description = "根据商户号查中介商户资料")]
        public DataSet GetAgencyBusinessInfoList(string Fspid)
        {
            try
            {
                return new SPOAService().GetAgencyBusinessInfoList(Fspid);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [WebMethod(Description = "商户资料函数")]
        public DataSet GetBusinessInfoList(string Fspid)
        {
            try
            {
                string Msg = "";
                string strSql = "spid=" + Fspid;
                string fuid = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_MERCHANTINFO, "FuidMiddle", out Msg);

                if (fuid == null || fuid.Trim() == "")
                {
                    throw new Exception("查找不到指定的记录，请确认你的输入是否正确！");
                }

                strSql = "uid=" + fuid;
                DataSet ds = CommQuery.GetDataSetFromICE(strSql, CommQuery.QUERY_USERINFO, out Msg);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null)
                {
                    ds.Tables[0].Columns.Add("FspidName");
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        string fuser_type = dr["Fuser_type"].ToString();
                        dr.BeginEdit();
                        if (fuser_type == "1")
                            dr["FspidName"] = dr["Fcompany_name"];
                        else
                            dr["FspidName"] = dr["Ftruename"];
                        dr.EndEdit();
                    }
                }

                return ds;
            }
            catch
            {
                return null;
            }
            finally
            {
                //da.Dispose();
            }
        }

        [WebMethod(Description = "中介C帐户资料函数")]
        public DataSet GetBusiness2InfoList(string Fspid)
        {
            try
            {
                string fuid = PublicRes.ConvertToFuid(Fspid);
                if (fuid == null || fuid.Trim() == "")
                {
                    throw new Exception("查找不到指定的记录，请确认你的输入是否正确！");
                }
    
                string errMsg = "";
                string Sql = "uid=" + fuid;
                return CommQuery.GetDataSetFromICE(Sql, CommQuery.QUERY_USERINFO, out errMsg);
            }
            catch
            {
                return null;
            }
            finally
            {
                //da.Dispose();
            }
        }

        [WebMethod(Description = "商户银行绑定函数")]
        public DataSet GetBusinessBankList(string Fspid)
        {
            try
            {  
                string Msg = "";
                string strSql = "spid=" + Fspid;
                string fuid = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_MERCHANTINFO, "Fuid", out Msg);

                if (fuid == null || fuid.Trim() == "")
                {
                    throw new Exception("查找不到指定的记录，请确认你的输入是否正确！");
                }

                strSql = "uid=" + fuid + "&curtype=1";
                return CommQuery.GetDataSetFromICE(strSql, CommQuery.QUERY_BANKUSER, out Msg);
            }
            catch
            {
                return null;
            }
            finally
            {
                //da.Dispose();
            }
        }

        [WebMethod(Description = "得到直付商户Email函数")]
        public DataSet GetBusinessEmail(string Fspid)
        {
            try
            {
                string Msg = "";
                string strSql = "spid=" + Fspid;
                string fuid = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_MERCHANTINFO, "FuidMiddle", out Msg);

                if (fuid == null || fuid.Trim() == "")
                {
                    throw new Exception("查找不到指定的邮箱记录，请确认你的输入是否正确！");
                }

                strSql = "uid=" + fuid;
                return CommQuery.GetDataSetFromICE(strSql, CommQuery.QUERY_USERINFO, out Msg);
            }
            catch
            {
                return null;
            }
            finally
            {
                //da.Dispose();
            }
        }

        [WebMethod(Description = "查询商户修改记录")]
        public DataSet GetHisBusinessList(string Fspid)
        {
            try
            {
                return new SPOAService().GetHisBusinessList(Fspid);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [WebMethod(Description = "商户修改函数")]
        public void SubmitBusinessInfo(string UserName, string Fspid, string OldFspName, string NewFspName, string OldEmail, string NewEmail, string OldAddress, string NewAddress, string ApplyResult, string[] FileInfos)
        {
            try
            {
                new SPOAService().SubmitBusinessInfo(UserName, Fspid, OldFspName, NewFspName, OldEmail, NewEmail, OldAddress, NewAddress, ApplyResult, FileInfos);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [WebMethod(Description = "商户修改证件信息函数")]
        public void SubmitBusinessCreInfo(string spid, string oldCreID, string newCreID, string memo)
        {
            // 2012/7/16

            throw new Exception("UnFinish");
        }

        [WebMethod(Description = "自助和BD商户--商户接入状态函数")]
        public DataSet GetSelfTypeList()
        {
            try
            {
                return new SPOAService().GetSelfTypeList();
            }
            catch
            {
                throw new Exception("'商户接入状态'异常");
            }
        }

        [WebMethod(Description = "自助商户--所属客服函数")]
        public DataSet GetSelfKFList()
        {
            try
            {
                return new SPOAService().GetSelfKFList();
            }
            catch
            {
                throw new Exception("'所属客服人员'异常");
            }
        }

        [WebMethod(Description = "自助和BD商户列表记录数函数")]
        public int GetSelfQueryListCount(string SPID, int? DraftFlag, string CompanyName, int? Flag, string WWWAdress, string Appid, DateTime? ApplyTimeStart, DateTime? ApplyTimeEnd, string BankUserName, string KFCheckUser, string SuggestUser, string MerType)
        {
            try
            {
                return new SPOAService().GetSelfQueryListCount(SPID, DraftFlag, CompanyName, Flag, WWWAdress, Appid, ApplyTimeStart, ApplyTimeEnd, BankUserName, KFCheckUser, SuggestUser, MerType);
            }
            catch
            {
                throw new Exception("列表记录数查询失败");
            }
        }

        [WebMethod(Description = "自助和BD商户列表函数")]
        public DataSet GetSelfQueryList(string SPID, int? DraftFlag, string CompanyName, int? Flag, string WWWAdress, string Appid,
            DateTime? ApplyTimeStart, DateTime? ApplyTimeEnd, string BankUserName, string KFCheckUser, string SuggestUser, string MerType, int topCount, int notInCount)
        {
            try
            {
                return new SPOAService().GetSelfQueryList(SPID, DraftFlag, CompanyName, Flag, WWWAdress, Appid, ApplyTimeStart, ApplyTimeEnd, BankUserName, KFCheckUser, SuggestUser, MerType, topCount, notInCount);
            }
            catch (Exception e)
            {
                throw new Exception("service发生错误,请联系管理员！");
            }
        }

        [WebMethod(Description = "自助和BD商户按主键查询函数")]
        public DataSet GetSelfQueryInfo(string ApplyCpInfoID)
        {
            try
            {
                return new SPOAService().GetSelfQueryInfo(ApplyCpInfoID);
            }
            catch (Exception e)
            {
                throw new Exception("service发生错误,请联系管理员！");
            }
        }

        [WebMethod(Description = "自助商户领单函数")]
        public void CheckTicket(string ApplyCpInfoID, string UserID)
        {
            try
            {
                new SPOAService().CheckTicket(ApplyCpInfoID, UserID);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [WebMethod(Description = "自助商户审核函数")]
        public void ApproveTicket(string ApplyCpInfoID, string UserID, bool Result, string Reason)
        {
            try
            {      
                int Type = 0;
                if (!Result)
                {
                    Type = 8;
                    //datafrom=0  and Fagentid is not null平台下属
                    DataSet ds = new SPOAService().GetApplyCpInfoXByID(ApplyCpInfoID);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 1)
                    {
                        if (ds.Tables[0].Rows[0]["datafrom"].ToString().Trim() == "0" && ds.Tables[0].Rows[0]["datafrom"].ToString().Trim() != "")
                        {
                            AgentCancel(ds.Tables[0].Rows[0]["spid"].ToString());
                        }
                    }
                    else
                    {
                        throw new Exception("该记录不存在!");
                    }
                }
                new SPOAService().ApproveTicket(ApplyCpInfoID, UserID, Type, Reason);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        #endregion

        #region 商户系统域名申请
        [WebMethod(Description = "商户系统域名申请列表记录数函数")]
        public DataSet GetSpidDomainQueryListCount(string filter)
        {
            try
            {
                return new SPOAService().GetSpidDomainQueryListCount(filter);
            }
            catch
            {
                throw new Exception("列表记录数查询失败");
            }
        }

        [WebMethod(Description = "商户系统域名申请列表函数")]
        public DataSet GetSpidDomainQueryList(string Spid, string CompanyName, DateTime? ApplyTimeStart, DateTime? ApplyTimeEnd, int? AmendState, string submitType, int topCount, int notInCount)
        {
            try
            {
                return new SPOAService().GetSpidDomainQueryList(Spid, CompanyName, ApplyTimeStart, ApplyTimeEnd, AmendState, submitType, topCount, notInCount);
            }
            catch (Exception e)
            {
                throw new Exception("service发生错误,请联系管理员！" + e.Message);
            }
        }

        [WebMethod(Description = "商户系统域名申请审核函数")]
        public void ApproveSpidDomain(string Taskid, string UserID, bool Result, string Reason)
        {
            try
            {
                if (UserID == null || UserID == "")
                {
                    throw new Exception("审核人不允许为空!");
                }

                DataSet ds = new SPOAService().GetMspAmendTaskByID(Taskid);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 1)
                {
                    if (ds.Tables[0].Rows[0]["AmendState"].ToString() == "-3")
                    {
                        if (Result)
                        {
                            new SPOAService().UpdateMspAmendTaskByTaskid(Taskid, 0, UserID);
                            CFTUserAppealClass.InputAppealNumber(UserID, "Success", "domain");
                        }
                        else
                        {
                            new SPOAService().UpdateSpidDomainApplyByTaskid(Taskid, Reason);
                            new SPOAService().UpdateMspAmendTaskByTaskid(Taskid, 4, UserID);
                            CFTUserAppealClass.InputAppealNumber(UserID, "Fail", "domain");
                        }
                    }
                    else
                    {
                        throw new Exception("该记录已被审核!");
                    }
                }
                else
                {
                    throw new Exception("该记录不存在!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [WebMethod(Description = "商户系统邮箱申请审核函数")]
        public void ApproveSpidEmail(string Taskid, string UserID, bool Result, string Reason)
        {
            try
            {
                if (UserID == null || UserID == "")
                {
                    throw new Exception("审核人不允许为空!");
                }

                DataSet ds = new SPOAService().GetMspAmendTaskByID(Taskid);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 1)
                {
                    if (ds.Tables[0].Rows[0]["AmendState"].ToString() == "-3")
                    {
                        if (Result)
                        {
                            new SPOAService().UpdateMspAmendTaskByTaskid(Taskid, 0, UserID);
                            CFTUserAppealClass.InputAppealNumber(UserID, "Success", "email");
                        }
                        else
                        {
                            new SPOAService().UpdateMspAmendInfoByTaskid(Taskid, Reason);
                            new SPOAService().UpdateMspAmendTaskByTaskid(Taskid, 4, UserID);
                            CFTUserAppealClass.InputAppealNumber(UserID, "Fail", "email");
                        }
                    }
                    else
                    {
                        throw new Exception("该记录已被审核!");
                    }
                }
                else
                {
                    throw new Exception("该记录不存在!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [WebMethod(Description = "商户系统商户名称申请审核函数")]
        public void ApproveSpidCompanyName(string Taskid, string UserID, bool Result, string Reason)
        {
            try
            {
                if (UserID == null || UserID == "")
                {
                    throw new Exception("审核人不允许为空!");
                }

                DataSet ds = new SPOAService().GetMspAmendTaskByID(Taskid);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 1)
                {
                    if (ds.Tables[0].Rows[0]["AmendState"].ToString() == "-3")
                    {
                        if (Result)
                        {
                            new SPOAService().UpdateMspAmendTaskByTaskid(Taskid, 0, UserID);
                            CFTUserAppealClass.InputAppealNumber(UserID, "Success", "companyname");
                        }
                        else
                        {
                            new SPOAService().UpdateMspAmendInfoByTaskid(Taskid, Reason);
                            new SPOAService().UpdateMspAmendTaskByTaskid(Taskid, 4, UserID);
                            CFTUserAppealClass.InputAppealNumber(UserID, "Fail", "companyname");
                        }
                    }
                    else
                    {
                        throw new Exception("该记录已被审核!");
                    }
                }
                else
                {
                    throw new Exception("该记录不存在!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region 商户营改增
        [WebMethod(Description = "商户系统新申请列表记录数函数")]
        public DataSet GetApplyValueAddedTax(string Spid, string Flags, int topCount, int notInCount)
        {
            try
            {
                return new SPOAService().GetApplyValueAddedTax(Spid, Flags, topCount, notInCount);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [WebMethod(Description = "商户系统新申请记录数函数")]
        public DataSet GetValueAddedTaxDetail(string taskid)
        {
            try
            {
                return new SPOAService().GetValueAddedTaxDetail(taskid);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [WebMethod(Description = "商户系统申请审核通过操作函数")]
        public void ValueAddedTaxApprove(string taskid, string Memo, string imgTaxCert, string imgBizLicenseCert, string imgAuthorizationCert, string UserName)
        {
            try
            {               
                new SPOAService().ValueAddedTaxApprove(taskid, Memo, imgTaxCert, imgBizLicenseCert, imgAuthorizationCert, UserName);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [WebMethod(Description = "商户系统申请审核拒绝操作函数")]
        public void ValueAddedTaxCancel(string taskid, string spid, string Memo, string UserName)
        {
            try
            {
                new SPOAService().ValueAddedTaxCancel(taskid, spid, Memo, UserName);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [WebMethod(Description = "查询商户营改增记录函数")]
        public DataSet GetAllValueAddedTax(string Spid, string CompanyName, int topCount, int notInCount)
        {
            try
            {
                return new SPOAService().GetAllValueAddedTax(Spid, CompanyName, topCount, notInCount);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [WebMethod(Description = "查询单个商户营改增记录函数")]
        public DataSet GetOneValueAddedTax(string Spid)
        {
            try
            {
                return new SPOAService().GetOneValueAddedTax(Spid);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [WebMethod(Description = "商户营改增修改状态函数")]
        public void ValueAddedTaxModify(string Spid, int Flag)
        {
            try
            {               
                new SPOAService().ValueAddedTaxModify(Spid, Flag);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region 平台下属商户审批取消
        //1.合同改成正常,合同终止日期改为T+15（T日：商户申请注销日期）
        //2.商品款和手续费改正常
        //3.取消权限
        [WebMethod(Description = "平台下属商户审批取消")]
        public void AgentCancel(string Fspid)
        {
            MySqlAccess daCS = new MySqlAccess(PublicRes.GetConnString("CS"));
            MySqlAccess daJS = new MySqlAccess(PublicRes.GetConnString("JS"));

            try
            {
                daCS.OpenConn();
                daJS.OpenConn();

                string sql = "update t_feecontract set FStandardStatus = " + (int)FeeContractStatus.正常 + ",FEndDate = adddate(now(),15) where Fspid = '" + Fspid + "' AND now() BETWEEN FStartDate AND FEndDate ";
                daCS.ExecSql(sql);

                sql = "update t_settlement set FRecordStatus = " + (int)FeeRecordStatus.正常 + " where Fspid = '" + Fspid + "' AND (FFeeItem = 4 or FFeeItem = 5) AND now() BETWEEN FStartDate AND FEndDate ";
                daJS.ExecSql(sql);

                string Msg = "";
                string strSql = "spid=" + Fspid;
                string fuid = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_MERCHANTINFO, "FuidMiddle", out Msg);

                strSql = "uid=" + fuid;
                strSql += "&attid=" + 44;
                strSql += "&modify_time=" + CommQuery.ICEEncode(PublicRes.strNowTimeStander);

                if (CommQuery.ExecSqlFromICE(strSql, CommQuery.UPDATE_USERATT, out Msg) < 0)
                {
                    throw new Exception("更新userattinfo失败" + Msg);
                }

                SetRole(Fspid, Fspid, 0, 1);
                SetRole(Fspid, Fspid, 0, 3);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                daCS.Dispose();
                daJS.Dispose();
            }
        }

        public enum FeeContractStatus
        {
            正常 = 1, 暂停 = 2, 取消 = 9, 冻结 = 3
        };
        public enum FeeRecordStatus
        {
            删除 = 0,
            正常 = 1,
            待审核 = 2,
            无效 = 9
        };

        [WebMethod(Description = "权限处理函数")]
        public bool SetRole(string spid, string qq, int newrole, int signorder)
        {
            if (signorder < 1 || signorder > 4)
            {
                throw new LogicException("权限位越界");
            }

            try
            {
                // TODO: 1客户信息资料外移
   
                string STRSQL = "qqid={1}&spid=" + spid;
                STRSQL += "&modify_time=" + PublicRes.strNowTimeStander;
                STRSQL += "&sign" + signorder + "={0}";
                string strSql = "";
                string str = Convert.ToString((long)newrole, 2);
                str = str.PadLeft(32, '0');

                if (spid == qq)
                {
                    // TODO: 1客户信息资料外移
                    //先校验,如果是修改的管理员,则把操作员的管理员为0的权限位也置0

                    string errMsg = "";
                    strSql = "spid=" + spid;
                    DataTable dt = CommQuery.GetTableFromICE(strSql, CommQuery.QUERY_MUSER, out errMsg);

                    if (dt == null || dt.Rows.Count == 0)
                        return false;

                    foreach (DataRow dr in dt.Rows)
                    {
                        string operatorqq = dr["Fqqid"].ToString().Trim();
                        if (operatorqq == spid)
                        {
                            int changedrole = Convert.ToInt32(str, 2);
                            strSql = String.Format(STRSQL, changedrole, spid);             
                            CommQuery.ExecSqlFromICE(strSql, CommQuery.UPDATE_MUSER, out errMsg);
                        }
                        else
                        {
                            strSql = "spid=" + spid + "&qqid=" + operatorqq;
                            long opersign = long.Parse(CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_MUSER, "Fsign" + signorder, out errMsg));
                            string strspsign = Convert.ToString(opersign, 2);
                            strspsign = strspsign.PadLeft(32, '0');

                            for (int i = 0; i <= 31; i++)
                            {
                                if (str[i] == '0')
                                {
                                    strspsign = strspsign.Substring(0, i) + "0" + strspsign.Substring(i + 1, 32 - i - 1);
                                }
                            }

                            int changedrole = Convert.ToInt32(strspsign, 2);
                            strSql = String.Format(STRSQL, changedrole, operatorqq);
               
                            CommQuery.ExecSqlFromICE(strSql, CommQuery.UPDATE_MUSER, out errMsg);
                        }
                    }

                    return true;
                }
                else if (qq.StartsWith(spid) && spid != qq)
                {
                    // TODO: 1客户信息资料外移
                    //如果修改的是操作员,则把管理员为0的权限位也置0

                    string errMsg = "";
                    strSql = "qqid=" + spid + "&spid=" + spid;
                    long spsign = long.Parse(CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_MUSER, "Fsign" + signorder, out errMsg));

                    string strspsign = Convert.ToString(spsign, 2);
                    strspsign = strspsign.PadLeft(32, '0');

                    for (int i = 0; i <= 31; i++)
                    {
                        if (strspsign[i] == '0')
                        {
                            str = str.Substring(0, i) + "0" + str.Substring(i + 1, 32 - i - 1);
                        }
                    }

                    int changedrole = Convert.ToInt32(str, 2);
                    strSql = String.Format(STRSQL, changedrole, qq);

                    int iresult = CommQuery.ExecSqlFromICE(strSql, CommQuery.UPDATE_MUSER, out errMsg);
                    return iresult == 1;
                }
                else
                    return false;

            }
            finally
            {
                //da.Dispose();
            }
        }
        #endregion

        #region  商户注销
        [WebMethod]
        public void BusinessLogout(string Fspid, string UserName, string Reason)
        {
            try
            {               
                if (new SPOAService().BusinessLogout(Fspid, UserName, Reason) != "0")
                {
                    throw new Exception("商户注销申请失败");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region  商户冻结  //原来走SPOA审核流，现在改为客服提交直接生效

        [WebMethod]
        public void BusinessFreeze(string Fspid, string UserName, bool IsFreeze, bool IsFreezePay, bool IsAccLoss, bool IsCloseAgent, string Reason)
        {
            try
            {
                DataSet ds = new SPOAService().GetOneValueAddedTax(Fspid);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    throw new Exception("该商户号不存在!");
                }

                BusinessFreezeSPOA(Fspid, IsFreeze, IsFreezePay, IsAccLoss, IsCloseAgent, UserName, Reason);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [WebMethod(Description = "商户冻结提交记录查询函数")]
        public DataSet QueryBussFreezeList(string spid, string type, string state)
        {
            try
            {              
                return new SPOAService().QueryBussFreezeList(spid, type, state);
            }
            catch (Exception e)
            {
                throw new LogicException("提交记录查询错误：" + e.Message);
            }
        }

        [WebMethod(Description = "查询属组函数")]
        public string GetBDName(string spid)
        {
            try
            {             
                return new SPOAService().GetBDName(spid);
            }
            catch (Exception e)
            {
                throw new LogicException("获取BD错误" + e.Message);
            }
        }

        [WebMethod(Description = "查询网站域名函数")]
        public string GetWWWAddress(string spid)
        {
            try
            {             
                return new SPOAService().GetWWWAddress(spid);
            }
            catch (Exception e)
            {
                throw new LogicException("获取网站域名错误" + e.Message);
            }
        }

        [WebMethod(Description = "查询行业分类函数")]
        public string GetTradeType(string spid)
        {
            try
            {           
                return new SPOAService().GetTradeType(spid);
            }
            catch (Exception e)
            {
                throw new LogicException("获取行业类别错误" + e.Message);
            }
        }

        [WebMethod]//如果结算合同管理中状态改成冻结，所有条款改为冻结
        public void BusinessFreezeSPOA(string Fspid, bool IsFreeze, bool IsFreezePay, bool IsAccLoss, bool IsCloseAgent, string UserName, string Reason)
        {
            PublicRes PR = new PublicRes();
            if (IsFreezePay)
            {//关闭支付
                try
                {                  
                    if (new SPOAService().ClosePay(Fspid, UserName, Reason) != "0")
                    {
                        throw new Exception("关闭支付申请失败!");
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("关闭支付申请失败：" + ex.Message);
                }
            }

            if (IsFreeze)
            {//暂停结算             
                try
                {
                    if (new SPOAService().FreezeSpid(Fspid, UserName, Reason) != "0")
                    {
                        throw new Exception("冻结商户结算失败!");
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("冻结商户成功：记录日志失败，" + ex.Message);
                }
            }
            if (IsAccLoss)
            {//账号挂失51
                try
                {                 
                    if (new SPOAService().LostOfSpid(Fspid, UserName, Reason) != "0")
                    {
                        throw new Exception("账号挂失失败!");
                    }
                }
                catch (Exception e)
                {
                    throw new Exception("账号挂失失败：" + e.Message);
                }
            }
            if (IsCloseAgent)
            {//关闭中介52
                try
                {                 
                    if (new SPOAService().CloseAgency(Fspid, UserName, Reason) != "0")
                    {
                        throw new Exception("关闭中介失败!");
                    }
                }
                catch (Exception e)
                {
                    throw new Exception("关闭中介失败：" + e.Message);
                }
            }
        }

        [WebMethod]//得到权限
        public int GetRole(string spid, string qq, int signorder)
        {
            if (signorder < 1 || signorder > 4)
            {
                throw new Exception("权限位越界");
            }
            string errMsg = "";
            string sql = "qqid=" + qq + "&spid=" + spid;
            string fsign = CommQuery.GetOneResultFromICE(sql, CommQuery.QUERY_MUSER, "Fsign" + signorder, out errMsg);

            if (fsign == null || fsign.Trim() == "")
                return 0;
            else
                return Int32.Parse(fsign);
        }

        #endregion

        #region 风控邮件
        //向结算审核者发邮件
        private bool SetEmailToFKCheck(int maskid, string UserName, string Type)
        {
            try
            {
                //外发邮件
                TENCENT.OSS.C2C.Finance.Common.CommLib.NewMailSend newMail = new TENCENT.OSS.C2C.Finance.Common.CommLib.NewMailSend();
                newMail.SendMail(ConfigurationManager.AppSettings["amendcheck_fk"].ToString().Replace(";", "@tencent.com;"), "", Type + "，请风控审核", GetEmalContentFK(maskid, Type, UserName), true, null);

                return true;
            }
            catch (Exception err)
            {
                return false;
            }

        }

        //获取银行信息修改邮件模板
        private string GetEmalContentFK(int maskid, string Type, string UserName)
        {
            string filename = ConfigurationManager.AppSettings["ServicePath"].Trim();
            if (!filename.EndsWith("\\"))
                filename += "\\email_amend_fk.htm";
            System.IO.StreamReader sr = new System.IO.StreamReader(filename, System.Text.Encoding.GetEncoding("GB2312"));
            try
            {
                string content = sr.ReadToEnd();

                string checkurl = ConfigurationManager.AppSettings["WebUrl"].ToString() + "/spoaweb/self";
                if (Type == "申请关闭商户支付权限")
                    checkurl += "/BusinessFreeze.aspx?mode=check_fk&id=" + maskid;
                else if (Type == "申请关闭商户退款权限")
                    checkurl += "/ShutRefund.aspx?mode=check_fk&id=" + maskid;
                else if (Type == "申请开通商户退款权限")
                    checkurl += "/ApplyRefund.aspx?mode=check_fk&id=" + maskid;
                else
                    checkurl += "/amend_msp_check.aspx?mode=check_fk&id=" + maskid;

                return String.Format(content, UserName, DateTime.Now.ToString("yyyy-MM-dd"), Type, checkurl).Replace("[", "{").Replace("]", "}");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                sr.Close();
            }
        }
        #endregion

        #region 关闭商户退款申请
        [WebMethod]
        public void ShutRefund(string Fspid, string UserName, string Reason)
        {
            try
            {             
                if (new SPOAService().CloseRefund(Fspid, UserName, Reason) != "0")
                {
                    throw new Exception("关闭商户退款申请失败!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [WebMethod]
        public void ApplyRefund(string Fspid, string UserName, string Reason)    //开通退款申请
        {
            try
            {               
                if (new SPOAService().OpenRefund(Fspid, UserName, Reason) != "0")
                {
                    throw new Exception("开通退款申请失败!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region  商户恢复
        [WebMethod]  //只走SPOA的风控和结算审核流程
        public void BusinessResume(string Fspid, string UserName, string Reason)
        {
            try
            {              
                if (new SPOAService().RestoreOfSpid(Fspid, UserName, Reason) != "0")
                {
                    throw new Exception("商户恢复申请失败！");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region 撤销退款
        [WebMethod]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public bool SuspendRefundment(ArrayList al, string UserIP, out string msg)
        {
            msg = "";

            try
            {
                return B2cReturnClass.SuspendRefundment(al, UserIP, out msg);
            }
            catch (Exception err)
            {
                throw new LogicException("Service处理失败！");
            }
        }
        #endregion

        [WebMethod()]
        public string GetUIDFromCerNum(string cerNum)
        {
            try
            {
                string errMsg;
                string strWhere = "vali_type=2&cre_type=1&cre_id=" + cerNum;
                string result = CommQuery.GetOneResultFromICE(strWhere, "fund_queryacc_service", "uid", out errMsg);

                return result;
            }
            catch
            {
                throw new Exception("");
            }
        }

        [WebMethod(Description = "获取B2C退款查询个数函数")]
        public int GetB2cReturnCount(string spid, string begintime, string endtime, int refundtype, int status,
            string tranid, string buyqq, string banktype, int sumtype, string drawid)
        {
            try
            {
                B2cReturnClass cuser = new B2cReturnClass(spid, begintime, endtime, refundtype, status, tranid, buyqq, banktype, sumtype, drawid);
                return 10000;
            }
            catch (Exception e)
            {
                string tmp = e.Message;
                return 0;
            }
        }

        [WebMethod(Description = "获取B2C退款查询列表函数")]
        public DataSet GetB2cReturnList(string spid, string begintime, string endtime, int refundtype, int status,
            string tranid, string buyqq, string banktype, int sumtype, string drawid, int queryTable, int istart, int imax)
        {
            try
            {
                DataSet ds = null;
                if (queryTable == 3)//查询快照表 andrew 20120712
                {
                    B2cReturnClass cuser = new B2cReturnClass(spid, begintime, endtime, refundtype, status, tranid, buyqq, banktype, sumtype, drawid);
                    ds = cuser.GetResultX(istart, imax, "ZWTK");
                }
                else if (queryTable == 1)//商户退款申请表
                {
                    if (spid.Trim() == "" && tranid.Trim() == "" && drawid.Trim() == "")
                    {
                        throw new Exception("商户号、交易单号、退单号不能全部为空！");
                    }

                    if (spid.Trim() == "" && tranid.Trim() == "" && drawid.Trim() != "")//只有drawid
                    {
                        //drawid 不为空，先通过drawId查询listid
                        string comSql = "draw_id=" + drawid;
                        string msg = "";
                        tranid = CommQuery.GetOneResultFromICE(comSql, CommQuery.QUERY_REFUND_RELATION, "Ftransaction_id", out msg);
                      
                        if (tranid == null || tranid.Trim() == "")
                        {
                            throw new Exception("通过退款单号：" + drawid + "未查询到对应的交易单号！");
                        }
                    }

                    if (tranid.Trim() != "")//交易单不为空
                    {
                        B2cReturnClass cuser = new B2cReturnClass(spid, begintime, endtime, refundtype, status, tranid, buyqq, banktype, sumtype, drawid);

                        int start = istart - 1;
                        if (start < 0) start = 0;

                        string strSql = cuser.ICESQL + "&strlimit=limit " + start + "," + imax;
                        string errMsg = "";
                        ds = CommQuery.GetDataSetFromICE(strSql, CommQuery.QUERY_MCH_REFUND, out errMsg);
                  
                    }
                    else
                    {
                        B2cReturnClass cuser = new B2cReturnClass(spid, begintime, endtime, refundtype, status, tranid, buyqq, banktype, sumtype, drawid);
                        int start = istart - 1;
                        if (start < 0) start = 0;

                        string strSql = cuser.ICESQL + "&strlimit=limit " + start + "," + imax;
                        string errMsg = "";
                        ds = CommQuery.GetDataSetFromICE(strSql, CommQuery.QUERY_USER_REFUND, out errMsg);
                    }
                }
                else if (queryTable == 2)//拍拍退款申请表
                {
                    B2cReturnClass cuser = new B2cReturnClass(spid, begintime, endtime, refundtype, status, tranid, buyqq, banktype, sumtype, drawid);

                    int start = istart - 1;
                    if (start < 0) start = 0;

                    string strSql = cuser.ICESQL + "&strlimit=limit " + start + "," + imax;
                    string errMsg = "";
                    ds = CommQuery.GetDataSetFromICE(strSql, CommQuery.QUERY_PAIPAI_REFUND, out errMsg);
                }


                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null)
                {

                    #region 转化操作

                    DataTable dt = ds.Tables[0];
                    TransferB2cReturnTable(dt);

                    #endregion

                    return ds;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private void TransferB2cReturnTable(DataTable dt)
        {
            dt.Columns.Add("Frp_feeName", typeof(String));
            dt.Columns.Add("Frb_feeName", typeof(String));
            dt.Columns.Add("Frefund_typeName", typeof(String));
            dt.Columns.Add("FstatusName", typeof(String));
            dt.Columns.Add("Flist_stateName", typeof(String));
            dt.Columns.Add("Fbank_typeName", typeof(String));
            dt.Columns.Add("FareaName", typeof(String));
            dt.Columns.Add("FcityName", typeof(String));

            foreach (DataRow dr in dt.Rows)
            {
                string tmp = dr["Frp_fee"].ToString();
                dr["Frp_feeName"] = MoneyTransfer.FenToYuan(tmp);

                tmp = dr["Frb_fee"].ToString();
                dr["Frb_feeName"] = MoneyTransfer.FenToYuan(tmp);

                string area = QueryInfo.GetString(dr["Farea"]);
                string city = QueryInfo.GetString(dr["Fcity"]);

                string sumtype = QueryInfo.GetString(dr["fstandby1"]);
                if (sumtype == null || sumtype == "")
                {
                    dr["fstandby1"] = "0";
                }

                if (area != null && area != "" && area != "0" && city != null && city != "" && city != "")
                {
                    try
                    {

                        dr["FareaName"] = AreaInfo.GetAreaName_Long(Int32.Parse(area));
                        dr["FcityName"] = AreaInfo.GetCityName_Long(Int32.Parse(area), city);
                    }
                    catch (Exception ex)
                    {
                        dr["FareaName"] = area;
                        dr["FcityName"] = city;

                    }
                }
                else
                {
                    dr["FareaName"] = "";
                    dr["FcityName"] = "";
                }

                tmp = dr["Frefund_type"].ToString();
                if (tmp == "1")
                {
                    dr["Frefund_typeName"] = "B2C退款";
                }
                else if (tmp == "2")
                {
                    dr["Frefund_typeName"] = "我要付款";
                }
                else if (tmp == "3")
                {
                    dr["Frefund_typeName"] = "银行直接退款";
                }
                else if (tmp == "4")
                {
                    dr["Frefund_typeName"] = "提现退款";
                }
                else if (tmp == "5")
                {
                    dr["Frefund_typeName"] = "信用卡退款";
                }
                else if (tmp == "6")
                {
                    dr["Frefund_typeName"] = "分帐退款";
                }
                else if (tmp == "9")
                {
                    dr["Frefund_typeName"] = "充值单退款";
                }
                else if (tmp == "11")
                {
                    dr["Frefund_typeName"] = "拍拍退单";
                }
                else
                {
                    dr["Frefund_typeName"] = "未知类型" + tmp;
                }


                tmp = dr["Fstatus"].ToString();

                if (tmp == "1")
                {
                    dr["FstatusName"] = "待审批";
                }
                else if (tmp == "2")
                {
                    dr["FstatusName"] = "审批流程中";
                }
                else if (tmp == "3")
                {
                    dr["FstatusName"] = "审批失败";
                }
                else if (tmp == "4")
                {
                    dr["FstatusName"] = "退款成功";
                }
                else if (tmp == "5")
                {
                    dr["FstatusName"] = "退款失败";
                }
                else if (tmp == "6")
                {
                    dr["FstatusName"] = "资料重填";
                }
                else if (tmp == "7")
                {
                    dr["FstatusName"] = "转入代发";
                }
                else if (tmp == "8")
                {
                    dr["FstatusName"] = "暂不处理";
                }
                else if (tmp == "9")
                {
                    dr["FstatusName"] = "退款流程中";
                }
                else if (tmp == "10")
                {
                    dr["FstatusName"] = "转入代发成功";
                }
                else if (tmp == "11")
                {
                    dr["FstatusName"] = "转入代发中";
                }
                else if (tmp == "12")
                {
                    dr["FstatusName"] = "分账回退中";
                }
                else if (tmp == "13")
                {
                    dr["FstatusName"] = "分帐回退成功";
                }
                else
                {
                    dr["FstatusName"] = "未知类型" + tmp;
                }

                tmp = dr["Flist_state"].ToString();

                if (tmp == "1")
                {
                    dr["Flist_stateName"] = "正常";
                }
                else if (tmp == "2")
                {
                    dr["Flist_stateName"] = "作废";
                }
            }
        }


        #region 结算查询
        [WebMethod(Description = "结算查询列表函数")]
        public DataSet SettQuery(string filter)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("JS"));

            try
            {
                DataSet ds = new DataSet();
                da.OpenConn();
                filter = (filter == "") ? "" : (" WHERE " + filter);
                string sql = "SELECT * FROM t_settlement " + filter + " ORDER BY FSpid,FPreDate DESC,FCalculateNo";
                ds = da.dsGetTotalData(sql);

                return ds;
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                da.Dispose();
            }
        }

        [WebMethod(Description = "微信商户结算查询列表函数")]
        public DataSet SettQueryWechat(string filter)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("JSWECHAT"));

            try
            {

                DataSet ds = new DataSet();

                da.OpenConn();

                filter = (filter == "") ? "" : (filter);

                string sql = "SELECT a.Fsettle_id AS FCalculateNo,a.Fspid AS FFeeContract,a.Fspid AS FSpid,a.Ffee_item AS FFeeItem,a.Ftrans_count AS FTransactionCount,a.Ftrans_amount AS FTransactionAmount," +
                    "0 AS FAddCountA,0 AS FAddCountB,a.Fcost_amount AS FCalculateAmount,a.Ffee_amount AS FDueAmount,a.Fincome_id AS FFeeNo,a.Faccount_date AS FPreDate,a.Ftrans_date AS FNextDate,a.Fplatform_spid AS FAgentSPID," +
                    "0 AS FDiscount,0 AS FItem,0 AS FInUid,a.Ftrans_cur_type AS Fcurtype,a.Fstate AS FUseTag,a.Ftrans_spid_ac AS FUid,a.Faccount_date AS FCreateTime,a.Flast_time AS FModify_time,a.Frecord_state AS FRecordStatus,'' AS FuserId,'' AS FPriceFormat," +
                    "b.Ftrans_channel_id AS FChannelNo,b.Ftrans_product_id AS FProductType,c.Fstandard_name AS FFeeStandard,c.Ffee_ceiling AS FMinAmount,c.Ffee_floor AS FMaxAmount,c.Fgrade_ceiling AS FMinGradation,c.Fgrade_floor AS FMaxGradation," +
                    "c.Fgrade_unit AS FGradationTag,c.Fcal_unit AS FCalculateUnit,c.Ffix_fee AS FFixAmount,c.Frate_mole AS FPerMolecule,c.Frate_denom AS FPerDenominator " +
                    " FROM c2c_settle.t_spid_settlement a,c2c_account.t_rule_pattern b,c2c_account.t_fee_standard c WHERE a.Frule_patt_id=b.Frule_patt_id AND a.Fstandard_id=c.Fstandard_id " + filter + " ORDER BY a.Fspid,a.Faccount_date DESC,a.Fsettle_id";

                ds = da.dsGetTotalData(sql);

                return ds;
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                da.Dispose();
            }
        }

        [WebMethod(Description = "结算查询得静态值函数1")]
        public DataSet QueryForSelect1()
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("CS"));

            try
            {
                DataSet ds = new DataSet();
                da.OpenConn();
                string sql = "SELECT FChannelNo,FProductType,FName FROM t_producttype WHERE FRecordStatus=1 ORDER BY FChannelNo,FProductType";
                ds = da.dsGetTotalData(sql);

                return ds;
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                da.Dispose();
            }
        }


        [WebMethod(Description = "结算查询得静态值函数2")]
        public DataSet QueryForSelect2_NOUSE()
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("ZLB"));

            try
            {
                DataSet ds = new DataSet();
                da.OpenConn();
                string sql = "SELECT FSpid,FName FROM c2c_db.t_merchant_info ORDER BY FSpid";
                ds = da.dsGetTotalData(sql);

                return ds;
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                da.Dispose();
            }

        }


        [WebMethod(Description = "结算查询得静态值函数3")]
        public DataSet QueryForSelect3()
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("CS"));

            try
            {
                DataSet ds = new DataSet();
                da.OpenConn();
                string sql = "SELECT FChannelNo,FName FROM t_channel WHERE FRecordStatus=1 ORDER BY FChannelNo";
                ds = da.dsGetTotalData(sql);

                return ds;
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                da.Dispose();
            }
        }

        [WebMethod(Description = "结算查询得静态值函数4")]
        public DataSet QueryForSelect4()
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("CS"));

            try
            {
                DataSet ds = new DataSet();
                da.OpenConn();
                string sql = "SELECT FFeeItem,FName FROM t_feeitem WHERE FRecordStatus=1 ORDER BY FFeeItem";
                ds = da.dsGetTotalData(sql);

                return ds;
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                da.Dispose();
            }
        }


        [WebMethod(Description = "结算查询得静态值函数5")]
        public DataSet QueryForSelect5()
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("CS"));

            try
            {
                DataSet ds = new DataSet();
                da.OpenConn();
                string sql = "SELECT FFeeStandard,FName FROM t_feestandard WHERE FRecordStatus=1 AND FNo=0 ORDER BY FFeeStandard";
                ds = da.dsGetTotalData(sql);

                return ds;
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                da.Dispose();
            }
        }


        [WebMethod(Description = "结算查询得静态值函数6")]
        public DataSet QueryForSelect6(string id)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("ZL"));

            try
            {
                DataSet ds = new DataSet();

                id = id.Replace("'", "''").Replace("\r\n", "' + CHAR(13) + CHAR(10) + '").Replace("\n", "' + CHAR(10) + '");

                string Msg = "";
                string strSql = "spid=" + id;
                ds = CommQuery.GetDataSetFromICE(strSql, CommQuery.QUERY_MERCHANTINFO, out Msg);

                return ds;
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                da.Dispose();
            }
        }

        private int GetUid(string special)
        {
            //furion 20061115 email修改相关
            if (special == null || special.Trim().Length < 3)
                return 0;

            string qqid = special.Trim();
            return Int32.Parse(PublicRes.ConvertToFuid(qqid));         
        }


        private DataSet GetBanks()
        {         
            string Msg = "";
            DataSet ds = QueryDicInfoByType("BANK_TYPE", out Msg);
            return ds;
        }


        private string NameByType(string type)
        {
            return GetBankName(type);          
        }


        [WebMethod(Description = "结算查询得静态值函数7")]
        public string QueryForSelect7(string special)
        {
            string bank = "Unknown";

            int uid = GetUid(special);

            if (uid != 0 && uid.ToString().Length > 2)
            {
               
                string Msg = "";
                string strSql = "uid=" + uid + "&curtype=1";
                DataTable dt = CommQuery.GetTableFromICE(strSql, CommQuery.QUERY_BANKUSER, out Msg);

                if (dt != null && dt.Rows.Count == 1)
                {
                    string FbankID = dt.Rows[0]["Fbankid"].ToString().Trim();

                    string NewFbankID = "";

                    if (FbankID != "" && FbankID.Length > 5)
                    {
                        for (int i = 0; i < FbankID.Length - 5; i++)
                        {
                            NewFbankID += "*";
                        }
                        NewFbankID = NewFbankID + FbankID.Substring(FbankID.Length - 5, 5).ToString();
                    }
                    else
                        NewFbankID = FbankID;

                    bank = NameByType(dt.Rows[0]["Fbank_type"].ToString()) + "  " +
                        dt.Rows[0]["Fbank_name"].ToString() + "  " +
                        NewFbankID;
                }
            }
            return bank;
        }


        [WebMethod(Description = "结算查询得静态值函数8")]
        public DataSet QueryForSelect8(string filter)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("CS"));

            try
            {
                DataSet ds = new DataSet();
                da.OpenConn();
                filter = (filter == "") ? "" : " AND (" + filter + ")";

                string sql = "SELECT * FROM t_feecontract WHERE FRecordStatus=1" + filter + " ORDER BY FFeeContract,FNo,FChannelNo,FProductType";
                ds = da.dsGetTotalData(sql);

                return ds;
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                da.Dispose();
            }
        }

        [WebMethod(Description = "微信商户查合同表")]
        public DataSet QueryContractWechat(string filter)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("JSWECHAT"));

            try
            {
                DataSet ds = new DataSet();
                da.OpenConn();

                filter = (filter == "") ? "" : " AND (" + filter + ")";

                string sql = "SELECT a.Fid AS FNo,a.Ffee_item AS  FFeeItem,a.Fstandard_id AS FFeeStandard,0 AS FDisCount,0 AS FAppendAmount,0 AS FPriorityAmount,a.Fcontract_state AS FStandardStatus,a.Fstart_date AS FStartDate,a.Fend_date AS FEndDate," +
                    "b.Ftrans_product_id AS FProductType,b.Ftrans_channel_id AS FChannelNo, " +
                    "c.Fcyc_type AS FCyc,c.Fcyc_number AS FCycNumber " +
                    " FROM c2c_account.t_spid_contract a,c2c_account.t_rule_pattern b,c2c_account.t_settle_period c WHERE a.Frule_patt_id=b.Frule_patt_id AND a.Fsettle_period=c.Fsettle_period " + filter + " ORDER BY a.Fcontract_id";

                ds = da.dsGetTotalData(sql);

                return ds;
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                da.Dispose();
            }
        }

        [WebMethod(Description = "结算统计查询")]
        public DataSet TranLogIncomeSumQuery(DateTime dateFrom, DateTime dateTo, string filter)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("JSTL"));

            try
            {
                DataSet ds = new DataSet();
                da.OpenConn();

                if (dateTo < dateFrom)
                    dateTo = dateFrom;

                filter = (filter == "") ? "" : (" AND " + filter);
                string sql = "SELECT * FROM c2c_tranlog_{0}.t_incomesum WHERE faccountdate>='" + dateFrom.ToString("yyyy-MM-dd") + "' AND faccountdate<'"
                    + dateTo.AddDays(1).ToString("yyyy-MM-dd") + "'" + filter;

                string s = "";
                int Month = (dateTo.Year * 12 + dateTo.Month) - (dateFrom.Year * 12 + dateFrom.Month) + 1;
                for (int i = 0; i < Month; i++)
                {
                    if (i > 0)
                        s += " UNION ";
                    s += String.Format(sql, dateFrom.AddMonths(i).ToString("yyyyMM"));
                }
                s += " ORDER BY FChannelNo,FSpid,FProductType";

                ds = da.dsGetTotalData(s);

                return ds;
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                da.Dispose();
            }
        }

        #endregion

        #region 结算查询(新)
        // 查询明细  单笔赋值时累加 历史未结算笔数 历史未结算金额(只查商品款的项目)
        //	根据收费实例号查询到已经结算的结算实例号
        [WebMethod(Description = "商户未结算记录")]
        public DataSet GetQuerySettlementList(string Fspid)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("JSTL"));
            try
            {
                da.OpenConn();
                //序号,渠道,结算项目,交易起始日期,交易结束日期,结算笔数,结算金额,费率,实际划账金额,产品名称,产品数量,上级商户号
                string Sql = "SELECT FCalculateNo,FChannelNo,FFeeItem,DATE_ADD(FPreDate, INTERVAL 1 DAY) AS FCurrentDate,FNextDate,FTransactionCount, " +
                    "FTransactionAmount,FFeeStandard,FDueAmount,FProductType,FAddCountA,FAgentSpid,FPerMolecule,FPerDenominator " +
                    "FROM c2c_settlement.t_settlement " +
                    "WHERE FSpid = '" + Fspid + "' AND FTransactionAmount <> 0 AND FFeeItem not in (1,2,3,4) AND FUseTag IN (0,2) " +
                    "ORDER BY FCalculateNo DESC LIMIT 10000";

                return da.dsGetTotalData(Sql);
            }
            catch (Exception err)
            {
                return null;
            }
            finally
            {
                da.Dispose();
            }
        }

        [WebMethod(Description = "微信商户未结算记录")]
        public DataSet QuerySettlementListWechat(string Fspid)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("JSWECHAT"));
            try
            {
                da.OpenConn();
                //序号,渠道,结算项目,交易起始日期,交易结束日期,结算笔数,结算金额,费率,实际划账金额,产品名称,产品数量,上级商户号
                string Sql = "SELECT a.Fsettle_id AS FCalculateNo,a.Ffee_item AS FFeeItem,a.Faccount_date AS FCurrentDate,a.Ftrans_date AS FNextDate,a.Fsettle_trans_count AS FTransactionCount, " +
                    "a.Ftrans_amount AS FTransactionAmount,a.Ffee_amount AS FDueAmount,a.Ftrans_count AS FAddCountA,a.Fplatform_spid AS FAgentSpid,b.Ftrans_product_id AS FProductType," +
                    "b.Ftrans_channel_id AS FChannelNo,c.Fstandard_name AS FFeeStandard,c.Frate_mole AS FPerMolecule,c.Frate_denom AS FPerDenominator " +
                    " FROM c2c_settle.t_spid_settlement a,c2c_account.t_rule_pattern b,c2c_account.t_fee_standard c  " +
                    " WHERE a.Frule_patt_id=b.Frule_patt_id AND a.Fstandard_id=c.Fstandard_id AND  a.Fspid = '" + Fspid + "' AND a.Ftrans_amount <> 0 AND a.Ffee_item not in (1,2,3,4) AND a.Fstate IN (2,3,4) " +
                    " ORDER BY a.Fsettle_id DESC LIMIT 10000";

                return da.dsGetTotalData(Sql);
            }
            catch (Exception err)
            {
                return null;
            }
            finally
            {
                da.Dispose();
            }
        }

        [WebMethod(Description = "商户今天结算记录")] //2015-8-11 改接口 v_yqyqguo
        public DataSet GetQuerySettlementTodayList(string Fspid)    //抓取今天的结算
        {
            //furion 20090611 考虑将来换数据库连接，现在YWB可以修改为拆分数据库，但前提条件是c2c_db.t_tcpay_list也在上面
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("YWB"));
            try
            {
                da.OpenConn();
                string Sql = "SELECT sum( if(Frequest_type in (4,6,9,29),1,0))," +
                    "sum( if(Frequest_type in (4,6,9,29) ,Fnum, 0))," +
                    "sum( if(Frequest_type in (4,6,9,29),0,1))," +
                    "sum( if(Frequest_type in (4,6,9,29) ,0,FNum+FPayBuy )) " +
                    //" FROM c2c_db_paylog.t_paylog_" + DateTime.Today.ToString("yyyyMMdd") +
                    " From c2c_db_00.t_tranlog_0 " +
                    " WHERE FSpid = '" + Fspid + "' AND Ftrade_type <> 1 AND Frequest_type IN(4,6,9,29,12,23,30) AND Flist_sign = 0 " +
                    " AND Fmodify_time >= '" + DateTime.Today.ToString("yyyyMMdd") + "' AND Fmodify_time < '" + DateTime.Today.AddDays(1).ToString("yyyyMMdd") + "'";

                return da.dsGetTotalData(Sql);
            }
            catch (Exception err)
            {
                return null;
            }
            finally
            {
                da.Dispose();
            }
        }

        [WebMethod(Description = "商户已结算记录")]
        public DataSet GetQueryBalanceList(string Fspid, DateTime BeginDate, DateTime EndDate)    //得到已结算记录
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("JSTL"));
            try
            {
                da.OpenConn();       //把划账金额,结算金额,手续费金额三项同时为0的屏蔽掉(客服与其它系统不同)
                string Sql = "SELECT FNo, FCreateTime, IF(FPriType = 10 or FPriType = 100, FTranAmount,FCashAmount + FTranAmount) AS Transfer, " +
                             "IF(FPriType = 10 or FPriType = 100, 0, FIncomeTranAmount) AS Balance,IF(FPriType = 10 or FPriType = 100, 0, FIncomeFeeAmount) AS Poundage,FPriType, " +
                             "CASE  FPriType WHEN '10' THEN '' ELSE '查看' END AS FPriTypeStr " +      //10 不显示链接,其他 显示链接
                             "FROM c2c_settlement.t_draw " +
                             "WHERE FSpid = '" + Fspid + "' " +
                             "AND FCreateTime BETWEEN '" + BeginDate.ToString("yyyyMMdd") + "' AND '" + EndDate.ToString("yyyyMMdd") + "' " +
                             "AND (IF(FPriType = 10  or FPriType = 100, FTranAmount,FCashAmount + FTranAmount) <> 0 OR IF(FPriType = 10  or FPriType = 100, 0, FIncomeTranAmount) <> 0 " +
                             "OR IF(FPriType = 10  or FPriType = 100, 0, FIncomeFeeAmount) <> 0 ) " +
                             "ORDER BY FNo DESC LIMIT 10000";

                return da.dsGetTotalData(Sql);
            }
            catch (Exception err)
            {
                return null;
            }
            finally
            {
                da.Dispose();
            }
        }

        [WebMethod(Description = "微信商户已结算记录")]
        public DataSet QueryBalanceListWechat(string Fspid, DateTime BeginDate, DateTime EndDate)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("JSWECHAT"));
            try
            {
                da.OpenConn();       //把划账金额,结算金额,手续费金额三项同时为0的屏蔽掉(客服与其它系统不同)
                string Sql = "SELECT a.Fdraw_id AS FNo, a.Faccount_date AS FCreateTime, IF(b.Fdraw_type = 10 or b.Fdraw_type = 100, Ftrans_amount,Fdraw_amount + Ftrans_amount) AS Transfer, " +
                             "0 AS Balance,0 AS Poundage,b.Fdraw_type AS FPriType, " +
                             "CASE  b.Fdraw_type WHEN '10' THEN '' ELSE '查看' END AS FPriTypeStr " +      //10 不显示链接,其他 显示链接
                             " FROM c2c_settle.t_spid_draw a,c2c_account.t_settle_rule b " +
                             " WHERE a.Fsettle_rule=b.Fsettle_rule AND a.Fspid = '" + Fspid + "' " +
                             " AND a.Faccount_date BETWEEN '" + BeginDate.ToString("yyyyMMdd") + "' AND '" + EndDate.ToString("yyyyMMdd") + "' " +
                             " AND (IF(b.Fdraw_type = 10  or b.Fdraw_type = 100, a.Ftrans_amount,a.Fdraw_amount + a.Ftrans_amount) <> 0)  " +
                             " ORDER BY a.Fdraw_id DESC LIMIT 10000";

                return da.dsGetTotalData(Sql);
            }
            catch (Exception err)
            {
                return null;
            }
            finally
            {
                da.Dispose();
            }
        }

        [WebMethod(Description = "商户已结算记录的流水")]
        public DataSet GetQueryBalanceDetailList(string Fspid, string FDrawNo)    //得到每笔结算记录的流水
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("JSTL"));
            try
            {
                da.OpenConn();
                string Sql = "SELECT FFeeNo FROM c2c_settlement.t_income WHERE FSpid = '" + Fspid + "' AND FDrawNo = " + FDrawNo;
                DataSet FeeNo = da.dsGetTotalData(Sql);
                if (FeeNo != null && FeeNo.Tables[0].Rows.Count > 0)
                {
                    string FFeeNo = "";

                    foreach (DataRow dr in FeeNo.Tables[0].Rows)
                    {
                        FFeeNo += "," + dr["FFeeNo"].ToString();
                    }

                    if (FFeeNo.StartsWith(","))
                        FFeeNo = FFeeNo.Substring(1, FFeeNo.Length - 1);

                    Sql = "SELECT FCalculateNo,FChannelNo,FFeeItem,DATE_ADD(FPreDate, INTERVAL 1 DAY) AS FCurrentDate,FNextDate,FTransactionCount," +
                          "FTransactionAmount,FFeeStandard,FDueAmount,FProductType,FAddCountA,FAgentSpid,FPerMolecule,FPerDenominator " +
                          "FROM c2c_settlement.t_settlement " +
                          "WHERE FSpid = '" + Fspid + "' AND FFeeNo IN (" + FFeeNo + ") AND FDueAmount <> 0 " +
                          "ORDER BY FCalculateNo  DESC";

                    return da.dsGetTotalData(Sql);
                }
                else
                    return null;
            }
            catch (Exception err)
            {
                return null;
            }
            finally
            {
                da.Dispose();
            }
        }

        [WebMethod(Description = "微信商户已结算记录的流水")]
        public DataSet QueryBalanceDetailListWechat(string Fspid, string FDrawNo)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("JSWECHAT"));
            try
            {
                da.OpenConn();
                string Sql = "SELECT Fincome_id AS FFeeNo FROM c2c_settle.t_spid_income WHERE Fspid = '" + Fspid + "' AND Fson_type=2 AND Fson_id = " + FDrawNo;
                DataSet FeeNo = da.dsGetTotalData(Sql);
                if (FeeNo != null && FeeNo.Tables[0].Rows.Count > 0)
                {
                    string FFeeNo = "";

                    foreach (DataRow dr in FeeNo.Tables[0].Rows)
                    {
                        FFeeNo += "," + dr["FFeeNo"].ToString();
                    }

                    if (FFeeNo.StartsWith(","))
                        FFeeNo = FFeeNo.Substring(1, FFeeNo.Length - 1);

                    Sql = "SELECT a.Fsettle_id AS FCalculateNo,a.Ffee_item AS FFeeItem,a.Faccount_date AS FCurrentDate,a.Ftrans_date AS FNextDate,a.Fsettle_trans_count AS FTransactionCount," +
                          "a.Ftrans_amount AS FTransactionAmount,a.Ffee_amount AS FDueAmount,a.Ftrans_count AS FAddCountA,a.Fplatform_spid AS FAgentSpid,b.Ftrans_product_id AS FProductType," +
                          "b.Ftrans_channel_id AS FChannelNo,c.Fstandard_name AS FFeeStandard,c.Frate_mole AS FPerMolecule,c.Frate_denom AS FPerDenominator " +
                          " FROM c2c_settle.t_spid_settlement a,c2c_account.t_rule_pattern b,c2c_account.t_fee_standard c  " +
                          " WHERE a.Frule_patt_id=b.Frule_patt_id AND a.Fstandard_id=c.Fstandard_id AND  a.Fspid = '" + Fspid + "' AND a.Fincome_id IN (" + FFeeNo + ") AND a.Ffee_amount <> 0 " +
                          " ORDER BY a.Fsettle_id  DESC";

                    return da.dsGetTotalData(Sql);
                }
                else
                    return null;
            }
            catch (Exception err)
            {
                return null;
            }
            finally
            {
                da.Dispose();
            }
        }

        #endregion

        [WebMethod(Description = "查询靓号QQ在回收库中记录")]
        public DataSet GetReclaimRecord(string u_QQID)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("ZL"));
            try
            {
                da.OpenConn();

                string Sql = "SELECT * FROM c2c_db.t_uid_recycle_record WHERE Fqqid = '" + u_QQID + "' AND Frecycle_status = 1";
                return da.dsGetTotalData(Sql);
            }
            catch (Exception err)
            {
                return null;
            }
            finally
            {
                da.Dispose();
            }
        }


        [WebMethod(Description = "查询QQ在回收库中记录")]
        public DataSet GetQQReclaimRecord(string u_QQID)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("UK"));
            try
            {
                da.OpenConn();

                string Sql = "SELECT * FROM c2c_db_" + u_QQID.Substring(u_QQID.Length - 2, 2) + ".ordinary_qq_recycle_" + u_QQID.Substring(u_QQID.Length - 3, 1) + " WHERE Fqqid = '" + u_QQID + "'";
                return da.dsGetTotalData(Sql);
            }
            catch (Exception err)
            {
                return null;
            }
            finally
            {
                da.Dispose();
            }
        }

        [WebMethod(Description = "查询账号的银行卡绑定记录")]
        public DataSet GetBankCardBindList(string u_QQID, string Fbank_type)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("BD"));
            try
            {
                da.OpenConn();
                string fuid = PublicRes.ConvertToFuid(u_QQID);

                if (fuid == null || fuid.Trim() == "")
                {
                    throw new Exception("查找不到指定的记录，请确认你的输入是否正确！");
                }

                string filter = "fuid=" + fuid;
                if (Fbank_type != "")
                    filter += " and Fbank_type=" + Fbank_type;
                // 有一个专门是Fprotocol_no分表的数据表，所以跟据条件判断查哪个表，因为功能目前暂缓，暂不做
                // 2012/5/29 新增查询证件号码项
                string Sql = "select Findex,Fbind_serialno,Fprotocol_no,Fuin,Fuid,Fbank_type,Fbind_flag,Fbind_type,Fbind_status,Fbank_status,right(Fcard_tail,4) as Fcard_tail," +
                             "Fbank_id,Ftruename,Funchain_time_local,Fmodify_time,Fmemo,Fcre_id from " + PublicRes.GetTName("t_user_bind", fuid) + " where " + filter;
                return da.dsGetTotalData(Sql);
            }
            catch (Exception err)
            {
                return null;
            }
            finally
            {
                da.Dispose();
            }
        }

        [WebMethod(Description = "查询特定的银行卡绑定记录")]
        public DataSet GetBankCardBind(string fuid, string Findex, string FBDIndex)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("BD"));
            try
            {
                // 2012/5/29 新增加查询字段Fcre_id
                da.OpenConn();
                string Sql = "";
                if (FBDIndex != null && FBDIndex == "1")
                {
                    Sql = "select Findex,Fbind_serialno,Fprotocol_no,Fuin,Fuid,Fbank_type,Fbind_flag,Fbind_type,Fbind_status," +
                                "Fbank_status,Fcard_tail,Fbank_id,Ftruename,Funchain_time_local,Fmodify_time," +
                                "Fmemo,Fcre_id,Ftelephone,Fmobilephone,Fcreate_time,Fbind_time_local,Fbind_time_bank,Funchain_time_bank,Fcre_type,Fonce_quota,Fday_quota,Fi_character2 & 0x01 as sms_flag from "
                                + PublicRes.GetTName("t_user_bind", fuid) + " where Findex=" + Findex + " and fuid=" + fuid;
                }
                else if (FBDIndex != null && FBDIndex == "2")//该Findex的记录在临时表
                {
                    Sql = "select Findex,Fbind_serialno,Fprotocol_no,Fuin,Fuid,Fbank_type,Fbind_flag,Fbind_type,Fbind_status," +
                                "Fbank_status,Fcard_tail,Fbank_id,Ftruename,Funchain_time_local,Fmodify_time," +
                                "Fmemo,Fcre_id,Ftelephone,Fmobilephone,Fcreate_time,Fbind_time_local,Fbind_time_bank,Funchain_time_bank,Fcre_type,Fonce_quota,Fday_quota,Fi_character2 & 0x01 as sms_flag from c2c_db.t_user_bind_tmp where Findex=" + Findex + " and fuid=" + fuid;
                }
                DataSet set = da.dsGetTotalData(Sql);
                return set;
            }
            catch (Exception err)
            {
                return null;
            }
            finally
            {
                da.Dispose();
            }
        }

        [WebMethod(Description = "查询一点通业务和快捷支付业务的QQ号码")]
        public DataSet GetBankCardBindList_UIN(string Fbank_type, string bankID, string creType, string creID,
            string protocolno, string phoneno, int bindStatue, int limStart, int limCount)
        {
            MySqlAccess da = null;
            try
            {
                //string bankID_Encode = PublicRes.BankIDEncode_ForBankCardUnbind(bankID);
                string bankID_Encode = PublicRes.EncryptZerosPadding(bankID);

                // 如果fuid为空则查询c2c_db.t_bind_relation
                string sql_findUID = "select * from c2c_db.t_bind_relation where ";
                string sql_findUID_filter = " (1=1) ";
                int sql_findUID_filter_startLen = sql_findUID_filter.Length;

                if (Fbank_type.Trim() != "")
                {
                    sql_findUID_filter += " And Fbank_type=" + Fbank_type;
                }

                if (bankID != "")
                {
                    sql_findUID_filter += " And (Fbank_id='" + bankID + "' or Fbank_id='" + bankID_Encode + "') ";
                }

                if (creType != "")
                {
                    sql_findUID_filter += " And Fcre_type=" + creType;
                }

                if (creID != "")
                {
                    sql_findUID_filter += " And Fcre_id='" + creID + "' ";
                }

                if (protocolno != "")
                {
                    sql_findUID_filter += " And ( Fprotocol_no='" + protocolno + "' or Fbank_id='" + protocolno + "')";
                }

                if (phoneno != "")
                {
                    sql_findUID_filter += " And Fmobilephone='" + phoneno + "' ";
                }

                if (sql_findUID_filter.Length == sql_findUID_filter_startLen)
                {
                    //throw new Exception("请输入必须的查询条件");
                    return null;
                }

                sql_findUID += sql_findUID_filter;

                da = new MySqlAccess(PublicRes.GetConnString("HT"));
                da.OpenConn();
                return da.dsGetTotalData(sql_findUID);
            }
            catch (Exception err)
            {
                return null;
            }
            finally
            {
                if (da != null)
                {
                    da.Dispose();
                }
            }

        }

        //可能是当日绑定的卡，但是通过卡号查不到对应的uid，所以不能查到绑卡记录，就要查c2c_db_xx.t_card_bind_relation_x
        [WebMethod(Description = "查询一点通业务和快捷支付业务的QQ号码")]
        public DataSet GetBankCardBindList_UIN_2(string Fbank_type, string bankID, string creType, string creID,
            string protocolno, string phoneno, int bindStatue, int limStart, int limCount)
        {
            MySqlAccess da = null;
            try
            {
                da = new MySqlAccess(PublicRes.GetConnString("BD"));
                da.OpenConn();

                if (bankID == "")
                    return null;
                int length = bankID.Length;
                string dbIndex = bankID.Substring(length - 2, 2);
                string tblIndex = bankID.Substring(length - 3, 1);
                string sql_findUID_2 = string.Format(@"select * from c2c_db_{0}.t_card_bind_relation_{1} where ", dbIndex, tblIndex);
                string bankID_md5 = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(bankID, "md5").ToLower();

                string sql_findUID_filter_2 = " (1=1) ";
                int sql_findUID_filter_startLen_2 = sql_findUID_filter_2.Length;

                if (Fbank_type.Trim() != "")
                {
                    sql_findUID_filter_2 += " And Fbank_type=" + Fbank_type;
                }

                if (bankID != "")
                {
                    sql_findUID_filter_2 += " And (Fcard_id='" + bankID + "' or Fcard_id='" + bankID_md5 + "') ";
                }

                if (sql_findUID_filter_2.Length == sql_findUID_filter_startLen_2)
                {
                    //throw new Exception("请输入必须的查询条件");
                    return null;
                }
                sql_findUID_2 += sql_findUID_filter_2;
                DataSet ds = da.dsGetTableByRange(sql_findUID_2, limStart, limCount);
                return ds;
            }
            catch (Exception err)
            {
                return null;
            }
            finally
            {
                if (da != null)
                {
                    da.Dispose();
                }
            }

        }


        [WebMethod(Description = "查询一点通业务和快捷支付业务")]
        public DataSet GetBankCardBindList_2(string u_QQID, string Fbank_type, string bankID, string creType, string creID,
            string protocolno, string phoneno, string strBeginDate, string strEndDate, int queryType, int bindStatue, int limStart, int limCount)
        {
            MySqlAccess da = null;
            try
            {
                string filter = "(1=1)";
                string bankID_Encode = PublicRes.EncryptZerosPadding(bankID);
                string fuid = PublicRes.ConvertToFuid(u_QQID);

                filter += " and fuid=" + fuid;

                if (Fbank_type != "")
                {
                    filter += " and Fbank_type=" + Fbank_type;
                }

                if (bankID != null && bankID.Trim() != "")
                {
                    filter += " and (Fbank_id='" + bankID + "' or Fbank_id='" + bankID_Encode + "') ";
                }

                if (creType != null && creType.Trim() != "")
                {
                    filter += " and Fcre_type='" + creType + "' ";
                }

                if (creID.Trim() != "")
                {
                    filter += " and Fcre_id='" + creID + "' ";
                }

                if (protocolno.Trim() != "")
                {
                    filter += " And ( Fprotocol_no='" + protocolno + "' or Fbank_id='" + protocolno + "')";
                }

                if (phoneno != null && phoneno.Trim() != "")
                {
                    filter += " and Fmobilephone='" + phoneno + "' ";
                }


                if (strBeginDate != null && strBeginDate.Trim() != "")
                {
                    filter += " and Fcreate_time>='" + strBeginDate + "' ";
                }

                if (strEndDate != null && strEndDate.Trim() != "")
                {
                    filter += " and Fcreate_time<='" + strEndDate + "' ";
                }


                if (queryType == 1)
                {
                    // 一点通
                    filter += " and ( (Fbind_type >=1 and Fbind_type<=9) or (Fbind_type >=20 and Fbind_type<=29) or (Fbind_type >=100 and Fbind_type<=119)) ";
                }
                else if (queryType == 2)
                {
                    // 快捷支付
                    filter += " and Fbind_type >=10 and Fbind_type<=19 ";
                }

                if (bindStatue != 99)
                {
                    filter += " and Fbank_status=" + bindStatue;
                }

                //filter += " limit " + limStart + "," + limCount;

                da = new MySqlAccess(PublicRes.GetConnString("BD"));
                da.OpenConn();
                // 有一个专门是Fprotocol_no分表的数据表，所以跟据条件判断查哪个表，因为功能目前暂缓，暂不做
                // 2012/5/29 新增查询证件号码项
                string Sql = "select 1 as FBDIndex , Findex,Fbind_serialno,Fprotocol_no,Fuin,Fuid,Fbank_type,Fbind_flag,Fbind_type,Fbind_status,Fbank_status,right(Fcard_tail,4) as Fcard_tail," +
                    "Fbank_id,Ftruename,Funchain_time_local,Fmodify_time,Fmemo,Fcre_id,Ftelephone,Fmobilephone,Fi_character4 from " + PublicRes.GetTName("t_user_bind", fuid) + " where " + filter;
                //加查临时表
                string Sql2 = "select 2 as FBDIndex ,Findex,Fbind_serialno,Fprotocol_no,Fuin,Fuid,Fbank_type,Fbind_flag,Fbind_type,Fbind_status,Fbank_status,right(Fcard_tail,4) as Fcard_tail," +
                    "Fbank_id,Ftruename,Funchain_time_local,Fmodify_time,Fmemo,Fcre_id,Ftelephone,Fmobilephone,Fi_character4 from c2c_db.t_user_bind_tmp where " + filter;
                Sql = "( " + Sql + " ) union all ( " + Sql2 + " ) limit " + limStart + "," + limCount;
                return da.dsGetTotalData(Sql);
            }
            catch (Exception err)
            {
                return null;
            }
            finally
            {
                if (da != null)
                {
                    da.Dispose();
                }
            }
        }

        //2013/7/18  yinhuang
        [WebMethod(Description = "一点通业务解除绑定")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public void UnbindBankCard(string bankType, string qqid, string protocolNo)
        {
            string msg = "";
            string iResult = "";

            try
            {
                if (myHeader == null)
                {
                    throw new LogicException("不正确的调用方法！");
                }
                string req = "function=BIND_CANCEL";
                req += "&bank_type=" + bankType;
                req += "&qqid=" + qqid;
                req += "&protocol_no=" + protocolNo;
                req += "&login_ip=" + myHeader.UserIP;
                req += "&server_ip=" + myHeader.UserIP;
                string service_name = "bind_modi_service";
                DataSet ds = CommQuery.GetOneTableFromICE(req, "FINANCE_OD_UNBIND_BANKTYPE", service_name, out msg);
                if (ds == null || ds.Tables.Count < 1)
                {
                    throw new Exception(msg);
                }
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];

                    string req_params = "ver=1&request_type=8809";

                    string need_bank = dt.Rows[0]["need2bank"].ToString();
                    if (need_bank != null && need_bank == "1")
                    {
                        req_params += "&bank_type=" + bankType;
                        req_params += "&card_no=" + dt.Rows[0]["card_no"].ToString();
                        byte[] b_msg = System.Text.Encoding.Default.GetBytes(req_params);
                        byte[] b_msg_len = BitConverter.GetBytes(b_msg.Length);
                        byte[] contData = new byte[b_msg_len.Length + b_msg.Length];
                        b_msg_len.CopyTo(contData, 0);
                        b_msg.CopyTo(contData, b_msg_len.Length);
                        //需要去调用前置机解绑
                        //commRes.GetTCPReply(req_params, contData, "10.6.206.72", 15005, out msg, "", out iResult);
                        string qzj_ip = ConfigurationManager.AppSettings["Unbind_QZJ_IP"];
                        string qzj_port = ConfigurationManager.AppSettings["Unbind_QZJ_PORT"];
                        TcpClient tcpClient = new TcpClient();
                        IPAddress ipAddress = IPAddress.Parse(qzj_ip);
                        IPEndPoint ipPort = new IPEndPoint(ipAddress, Int32.Parse(qzj_port));
                        tcpClient.Connect(ipPort);
                        NetworkStream stream = tcpClient.GetStream();
                        stream.Write(contData, 0, contData.Length);
                        byte[] bufferOut = new byte[1024];
                        stream.Read(bufferOut, 0, 1024);
                        byte[] bufferLen = new byte[4];

                        Array.Copy(bufferOut, 0, bufferLen, 0, 4);
                        int c = BitConverter.ToInt32(bufferLen, 0);
                        byte[] cont = new byte[c];
                        Array.Copy(bufferOut, 4, cont, 0, c);
                        string answer = Encoding.Default.GetString(cont);
                        string[] strlist1 = answer.Split('&');
                        if (strlist1.Length == 0)
                        {
                            throw new LogicException("前置机返回结果有误：" + answer);
                        }
                        Hashtable ht = new Hashtable(strlist1.Length);
                        foreach (string strtmp in strlist1)
                        {
                            string[] strlist2 = strtmp.Split('=');
                            if (strlist2.Length != 2)
                            {
                                continue;
                            }
                            ht.Add(strlist2[0].Trim(), strlist2[1].Trim());
                        }

                        if (!ht.Contains("result") || ht["result"].ToString().Trim() != "0")
                        {
                            throw new LogicException("前置机返回结果有误：" + answer);
                        }
                    }
                }
            }
            catch (Exception err)
            {
                throw new LogicException("Service处理失败！" + err.Message);
            }
        }

        //2013/8/12  lxl
        [WebMethod(Description = "特殊银行一点通业务解除绑定")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet UnbindBankCardSpecial(string bankType, string qqid, string card_tail, string bindSerialno, string protocol_no)
        {
            string msg = "";
            string iResult = "";

            try
            {
                if (myHeader == null)
                {
                    throw new LogicException("不正确的调用方法！");
                }

                string req = "";
               
                if (protocol_no == "0")
                {
                    if (card_tail != null && card_tail != "" && card_tail.Length <= 4)
                    {
                        //需要参数query_type=3&bank_type=&card_tail=&qqid=&function=BIND_CANCEL
                        req = "function=BIND_CANCEL&query_type=3";
                        req += "&bank_type=" + bankType;
                        req += "&qqid=" + qqid;
                        req += "&card_tail=" + card_tail;
                        //测试通过：req = "function=BIND_CANCEL&query_type=3&bank_type=3107&qqid=2817570940&card_tail=7834";
                    }
                    else
                    {
                        if (bindSerialno == "")
                        {
                            msg = "绑定序列号为空，无法解绑";
                            throw new LogicException(msg);
                        }
                        else
                        {
                            //需要参数query_type=2&bank_type=&qqid=&bind_serialno=$Fbind_serialno
                            req = "function=BIND_CANCEL&query_type=2";
                            req += "&bank_type=" + bankType;
                            req += "&qqid=" + qqid;
                            req += "&bind_serialno=" + bindSerialno;
                        }
                    }
                }
                else
                {
                    req = "function=BIND_CANCEL&query_type=2";
                    req += "&bank_type=" + bankType;
                    req += "&qqid=" + qqid;
                    req += "&bind_serialno=" + bindSerialno;
                }

                string service_name = "bind_modi_service";
                DataSet ds = CommQuery.GetOneTableFromICE(req, "", service_name, out msg);
         
                if (ds == null)
                {
                    throw new LogicException(msg);
                }
                return ds;
            }
            catch (Exception err)
            {
                throw new LogicException("Service处理失败！" + msg);
            }
        }

        // 2012/5/30 queryType = 1为普通一点通 queryType = 2 为快捷支付
        // isShowAboutDetail : 传入的条件是否限制查t_user_bind，false--要限制
        [WebMethod(Description = "查询一点通业务和快捷支付业务")]
        public DataSet GetBankCardBindList_New(string u_QQID, string Fbank_type, string bankID, string uid, string creType, string creID,
            string protocolno, string phoneno, string strBeginDate, string strEndDate, int queryType, bool isShowAboutDetail,
            int bindStatue, string bind_serialno, int limStart, int limCount)
        {
            MySqlAccess da = null;
            try
            {
                string filter = "(1=1)";
                string fuid = "";
                if (u_QQID != null && u_QQID.Trim() != "")
                {
                    fuid = PublicRes.ConvertToFuid(u_QQID);

                    if (fuid == null || fuid.Trim() == "")
                    {
                        throw new Exception("输入的QQ号码有误！");
                    }

                    filter += " and fuid=" + fuid;
                }

                if (uid.Trim() != "" && fuid.Trim() == "")
                {
                    fuid = uid;
                    filter += " and fuid=" + uid;
                }

                //string bankID_Encode = PublicRes.BankIDEncode_ForBankCardUnbind(bankID);
                string bankID_Encode = PublicRes.EncryptZerosPadding(bankID);

                DataSet ds_findUID = null;

                if (fuid == "")
                {
                    // 如果fuid为空则查询c2c_db.t_bind_relation
                    string sql_findUID = "select fuid from c2c_db.t_bind_relation where ";
                    string sql_findUID_filter = " (1=1) ";
                    int sql_findUID_filter_startLen = sql_findUID_filter.Length;

                    if (Fbank_type.Trim() != "")
                    {
                        sql_findUID_filter += " And Fbank_type=" + Fbank_type;
                    }

                    if (bankID != "")
                    {
                        sql_findUID_filter += " And (Fbank_id='" + bankID + "' or Fbank_id='" + bankID_Encode + "') ";
                    }

                    if (creType != "")
                    {
                        sql_findUID_filter += " And Fcre_type=" + creType;
                    }

                    if (creID != "")
                    {
                        sql_findUID_filter += " And Fcre_id='" + creID + "' ";
                    }

                    if (protocolno != "")
                    {
                        sql_findUID_filter += " And ( Fprotocol_no='" + protocolno + "' or Fbank_id='" + protocolno + "')";
                    }

                    if (phoneno != "")
                    {
                        sql_findUID_filter += " And Fmobilephone='" + phoneno + "' ";
                    }

                    if (sql_findUID_filter.Length == sql_findUID_filter_startLen)
                    {
                        //throw new Exception("请输入必须的查询条件");
                        return null;
                    }

                    sql_findUID += sql_findUID_filter;

                    MySqlAccess da_findUID = new MySqlAccess(PublicRes.GetConnString("HT"));
                    da_findUID.OpenConn();
                    ds_findUID = da_findUID.dsGetTotalData(sql_findUID);
                    //DataSet ds_findUID = da_findUID.dsGetTableByRange(sql_findUID,0,1);

                    if (ds_findUID == null || ds_findUID.Tables.Count == 0 || ds_findUID.Tables[0].Rows.Count == 0)
                    {
                        return null;
                    }

                    // 这里的实现需要和产品沟通，因为一个银行卡号，证件号绑定不止一个uid

                    filter += " and fuid in (";
                    for (int i = 0; i < ds_findUID.Tables[0].Rows.Count; i++)
                    {
                        fuid = ds_findUID.Tables[0].Rows[i]["fuid"].ToString();
                        if (fuid == null || fuid.Trim() == "")
                            return null;
                        filter += fuid + ",";
                    }

                    filter = filter.Substring(0, filter.Length - 1) + ") ";
                }

                if (!isShowAboutDetail)
                {
                    if (Fbank_type != "")
                    {
                        filter += " and Fbank_type=" + Fbank_type;
                    }

                    if (bankID != null && bankID.Trim() != "")
                    {
                        filter += " and (Fbank_id='" + bankID + "' or Fbank_id='" + bankID_Encode + "') ";
                    }

                    if (creType != null && creType.Trim() != "")
                    {
                        filter += " and Fcre_type='" + creType + "' ";
                    }

                    if (creID.Trim() != "")
                    {
                        filter += " and Fcre_id='" + creID + "' ";
                    }

                    if (protocolno.Trim() != "")
                    {
                        filter += " and Fprotocol_no='" + protocolno + "' ";
                    }

                    if (phoneno != null && phoneno.Trim() != "")
                    {
                        filter += " and Fmobilephone='" + phoneno + "' ";
                    }

                    if (bind_serialno != null && bind_serialno.Trim() != "")//序列号
                    {
                        filter += " and Fbind_serialno='" + bind_serialno + "' ";
                    }
                }

                if (strBeginDate != null && strBeginDate.Trim() != "")
                {
                    filter += " and Fcreate_time>='" + strBeginDate + "' ";
                }

                if (strEndDate != null && strEndDate.Trim() != "")
                {
                    filter += " and Fcreate_time<='" + strEndDate + "' ";
                }


                if (queryType == 1)
                {
                    // 一点通
                    filter += " and ( (Fbind_type >=1 and Fbind_type<=9) or (Fbind_type >=20 and Fbind_type<=29) or (Fbind_type >=100 and Fbind_type<=119)) ";
                }
                else if (queryType == 2)
                {
                    // 快捷支付
                    filter += " and Fbind_type >=10 and Fbind_type<=19 ";
                }

                if (bindStatue != 99)
                {
                    filter += " and Fbank_status=" + bindStatue;
                }

                //filter += " limit " + limStart + "," + limCount;

                da = new MySqlAccess(PublicRes.GetConnString("BD"));
                da.OpenConn();
                // 有一个专门是Fprotocol_no分表的数据表，所以跟据条件判断查哪个表，因为功能目前暂缓，暂不做
                // 2012/5/29 新增查询证件号码项
                string Sql = "select 1 as FBDIndex , Findex,Fbind_serialno,Fprotocol_no,Fuin,Fuid,Fbank_type,Fbind_flag,Fbind_type,Fbind_status,Fbank_status,right(Fcard_tail,4) as Fcard_tail," +
                    "Fbank_id,Ftruename,Funchain_time_local,Fmodify_time,Fmemo,Fcre_id,Ftelephone,Fmobilephone,Fi_character4,Fbind_time_bank,Fbind_time_local from " + PublicRes.GetTName("t_user_bind", fuid) + " where " + filter;
                //加查临时表
                string Sql2 = "select 2 as FBDIndex , Findex,Fbind_serialno,Fprotocol_no,Fuin,Fuid,Fbank_type,Fbind_flag,Fbind_type,Fbind_status,Fbank_status,right(Fcard_tail,4) as Fcard_tail," +
                    "Fbank_id,Ftruename,Funchain_time_local,Fmodify_time,Fmemo,Fcre_id,Ftelephone,Fmobilephone,Fi_character4,Fbind_time_bank,Fbind_time_local from c2c_db.t_user_bind_tmp where " + filter;
                Sql = Sql + " union all " + Sql2 + " limit " + limStart + "," + limCount;
                return da.dsGetTotalData(Sql);
            }
            catch (Exception err)
            {
                throw err;
            }
            finally
            {
                if (da != null)
                    da.Dispose();
            }
        }


        [WebMethod(Description = "解除绑定银行卡")]
        public void ModifyBankCardBind(string fuid, string Findex, string Fmemo)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("BD"));
            try
            {
                da.OpenConn();
                string Sql = " select Fbind_flag,Fbind_status,Fbank_status from " + PublicRes.GetTName("t_user_bind", fuid) + " where Findex=" + Findex + " and fuid=" + fuid;
                DataSet ds = da.dsGetTotalData(Sql);

                if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count != 1)
                {
                    throw new Exception("没有查找到相应的记录！");
                }
                else if (ds.Tables[0].Rows[0]["Fbind_flag"].ToString() == "2" && ds.Tables[0].Rows[0]["Fbind_status"].ToString() == "4" && ds.Tables[0].Rows[0]["Fbank_status"].ToString() == "3")
                {
                    throw new Exception("该记录已处于解除绑定状态！");
                }
                else
                {
                    Sql = "update " + PublicRes.GetTName("t_user_bind", fuid) + " set Fbind_flag = 2,Fbind_status = 4,Fbank_status = 3," +
                          "Funchain_time_local = '" + PublicRes.strNowTimeStander + "',Fmodify_time = '" + PublicRes.strNowTimeStander + "'," +
                          "Fmemo = '" + Fmemo + "' where Findex = " + Findex + " and fuid=" + fuid;
                    da.ExecSqlNum(Sql);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                da.Dispose();
            }
        }

        [WebMethod(Description = "同步银行卡绑定")]
        public bool SynchronBankCardBind(string bankType, string cardTail, string bankId)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("BD"));
            try
            {
                if (string.IsNullOrEmpty(cardTail))
                {
                    throw new Exception("card_tail参数为空！");
                }
                if (string.IsNullOrEmpty(bankId))
                {
                    throw new Exception("bankId参数为空！");
                }

                //对bankid解密
                string bankID_Encode = PublicRes.BankIDEncode_ForBankCardUnbind(bankId);

                da.OpenConn();

                string bankID_md5 = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(bankID_Encode, "md5").ToLower();

                string table = "c2c_db_" + cardTail.Substring(cardTail.Length - 2, 2) + ".t_card_bind_relation_" + cardTail.Substring(cardTail.Length - 3, 1);
                string sql = "select Fuin from " + table + " where Fcard_id='" + bankID_md5 + "' and Flstate=1";
                DataSet ds = da.dsGetTotalData(sql);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    //前置机解绑
                    string Msg = "";
                    string req = "ver=1&request_type=8809&bank_type=" + bankType + "&card_no=" + bankId;
                    string qzj_ip = ConfigurationManager.AppSettings["Unbind_QZJ_IP"];
                    string qzj_port = ConfigurationManager.AppSettings["Unbind_QZJ_PORT"];

                    string answer = commRes.GetFromRelay(req, qzj_ip, qzj_port, out Msg);

                    if (answer == "")
                    {
                        return false;
                    }
                    if (Msg != "")
                    {
                        throw new Exception(Msg);
                    }
                    Msg = "";
                    CommQuery.ParseRelayStr(answer, out Msg);
                    if (Msg != "")
                    {
                        throw new Exception(Msg);
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                throw new LogicException("service处理错误：" + e.Message);
            }
            finally
            {
                da.Dispose();
            }
            return false;
        }


        #region 提现规则查询
        [WebMethod]
        public DataSet WS_AppealDQuery(string FSpid, string FUser, int FPriType, int FState, int iPageStart, int iPageMax)
        {
            AppealDQuery cuser = new AppealDQuery(FSpid, FUser, FPriType, FState);

            DataSet ds = cuser.GetResultX(iPageStart, iPageMax, "ZL");

            return ds;
        }


        [WebMethod]
        public int WS_AppealDQueryCount(string FSpid, string FUser, int FPriType, int FState)
        {
            AppealDQuery cuser = new AppealDQuery(FSpid, FUser, FPriType, FState);
            return cuser.GetCount("ZL");
        }
        #endregion

        #region 结算规则查询
        [WebMethod]
        public DataSet WS_AppealSQuery(string Fno, string FUserType, string FUser, string FState, int iPageStart, int iPageMax)
        {
            AppealSQuery cuser = new AppealSQuery(Fno, FUserType, FUser, FState);

            DataSet ds = cuser.GetResultX(iPageStart, iPageMax, "CS");

            return ds;
        }

        [WebMethod]
        public int WS_AppealSQueryCount(string Fno, string FUserType, string FUser, string FState)
        {
            AppealSQuery cuser = new AppealSQuery(Fno, FUserType, FUser, FState);
            return cuser.GetCount("CS");
        }
        #endregion

        #region 信用卡还款查询
     
        #endregion

        #region  付款
        [WebMethod(Description = "付款汇总记录")]
        public DataSet BatPay_InitGrid(string WeekIndex)
        {
            string strBeginDate = DateTime.Parse(WeekIndex).ToString("yyyyMMdd");
            string strSql = "select FBatchID,'0' FUrl,substring(FBatchID,1,8) FDate,FBankType,FPayCount,(FPaySum / 100) FPaySum1 ,FStatus,'0' FStatusName,'0' FMsg, '0' FBankID "
                + " from c2c_zwdb.t_batchpay_rec where FBatchID like '" + strBeginDate + "______'  order by FDate desc";

            DataSet ds = new DataSet();

            DataTable dt;
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("ZWFK"));
            try
            {
                da.OpenConn();
                return da.dsGetTotalData(strSql);
            }
            catch (Exception ex)
            {
                LogHelper.LogInfo("查询数据异常:" + ex.ToString());
                throw ex;
            }
            finally
            {
                da.Dispose();
            }
        }

        #region  获取银行名及编号
        [WebMethod(Description = "获取银行名及编号")]
        public DataSet BatPay_GetBank()
        {
            string strSql = "select Fbank_type,FFlag2_2 from c2c_zwdb.t_bank_class where FFlag2_2<>''";

            DataSet ds = new DataSet();

            DataTable dt;
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("ZW"));
            try
            {
                da.OpenConn();
                return da.dsGetTotalData(strSql);
            }
            finally
            {
                da.Dispose();
            }
        }
        #endregion

        [WebMethod]
        public bool BatPay_CanVisible(string strDate)
        {
            string strSql = "select Max(FBatchID) from c2c_zwdb.t_batchpay_rec where length(Fbatchid)=12";
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("ZWFK"));
            try
            {
                da.OpenConn();
                string strtmp = da.GetOneResult(strSql);
                if (strtmp == "")
                {
                    return true;
                }
                else
                {
                    return (strDate.CompareTo(strtmp.Substring(0, 8)) > 0);
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                da.Dispose();
            }
        }

        /// <summary>
        /// 查询是否存在有未完成的付款回导任务
        /// </summary>
        /// <param name="strBatchID">批次号</param>
        /// <returns>是否存在</returns>
        [WebMethod]
        public bool BatPay_SixCheck(string strBatchID)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("ZW"));
            try
            {

                if (strBatchID.Substring(0, 8).CompareTo(DateTime.Now.ToString("yyyyMMdd")) > 0)
                    return false;

                da.OpenConn();

                string strSql = "select count(1) from c2c_zwdb.t_main_task  where "
                    + " FExecNo in(8,12) and FSchFrequency<9 ";

                if (da.GetOneResult(strSql) != "0")
                    return false;

                strSql = "select count(1) from c2c_zwdb.t_sub_task A,c2c_zwdb.t_main_task B where A.FTaskID=B.FTaskID and "
                    + " B.FExecNo in(8,12) and A.FStatus<5 ";

                return da.GetOneResult(strSql) == "0";
            }
            catch
            {
                return false;
            }
            finally
            {
                da.Dispose();
            }
        }

        /// <summary>
        /// 快照是否完备
        /// </summary>
        /// <param name="strBatchID">批次号</param>
        /// <returns>是否完备</returns>
        [WebMethod]
        public bool BatPay_CheckSnapFinish(string strBatchID)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("ZWFK"));
            try
            {
                da.OpenConn();
                string strCheckDate = strBatchID.Substring(0, 8);
                string strSql = "SELECT Fstate FROM c2c_zwdb.t_snopshot_log WHERE FAccDate ='" + strCheckDate + "' AND FOpType = 0 ORDER BY FSequence DESC";

                if (strBatchID.Length == 14)
                {
                    //商户付款直接返回真，当做快照已完备。
                    return true;
                }

                return da.GetOneResult(strSql) == "0"; //FState = 0代表成功
            }
            catch
            {
                return false;
            }
            finally
            {
                da.Dispose();
            }
        }


        /// <summary>
        /// 查询工单表中除指定工单外是否不存在有11状态的记录
        /// </summary>
        /// <param name="strBatchID">批次号</param>
        /// <returns>是否不存在</returns>
        [WebMethod]
        public bool BatPay_CheckFinish11(string strBatchID)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("ZWFK"));
            try
            {
                da.OpenConn();
                string strSql = "select count(1) from c2c_zwdb.t_batchpay_rec where FStatus=11 and FBatchID not like'"
                    + strBatchID.Substring(0, 8) + "%'";

                string strBatchNo = da.GetOneResult(strSql);

                return da.GetOneResult(strSql) == "0";
            }
            catch
            {
                return false;
            }
            finally
            {
                da.Dispose();
            }
        }

        /// <summary>
        /// 付款回导出现错误时,提取错误信息
        /// </summary>
        /// <param name="strBatchID">批次号(日期+银行)</param>
        /// <param name="iStatus">回导后的执行状态</param>
        /// <returns>返回错误信息</returns>
        [WebMethod]
        public string BatPay_GetErrorMsg(string strBatchID, int iStatus)
        {
            string strSql = "";
            if (iStatus == 9 || iStatus == 11)
            {
                strSql = "select FBatchNoExp from c2c_zwdb.t_batchpay_rec where FBatchID='" + strBatchID + "'";
            }
            else if (iStatus == 10)
            {
                strSql = "select FBatchNoImp from c2c_zwdb.t_batchpay_rec where FBatchID='" + strBatchID + "'";
            }

            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("ZWFK"));
            MySqlAccess da2 = new MySqlAccess(PublicRes.GetConnString("ZW"));
            try
            {
                da2.OpenConn();
                da.OpenConn();
                string batch_no = da2.GetOneResult(strSql);

                string sql = "select FStatusDes from c2c_zwdb.t_sub_task where FBatchNo='" + batch_no + "'";

                string tmp = da.GetOneResult(sql);
                return tmp.Replace("\0", "");
            }
            catch (Exception err)
            {
                return "";
            }
            finally
            {
                da2.Dispose();
                da.Dispose();
            }
        }

        /// <summary>
        /// 获取指定批次号付款数据的条数
        /// </summary>
        /// <param name="batchID">批次号</param>
        /// <returns>数据条数</returns>
        [WebMethod]
        public int BatpayDetail_GetCount(string batchID, int state, string username, string bankacc, string paybank)
        {
            string strSql = "select count(*) from c2c_zwdb_" + batchID.Substring(0, 8) + ".t_payfund_total as a1 ,c2c_zwdb_"
                + batchID.Substring(0, 8) + ".t_pay_relationship as a2 where a1.FbatchID='" + batchID + "' and a2.FSequence=a1.FSequence ";

            if (batchID.Length == 14)
            {
                strSql = "select count(*) from c2c_zwdb_" + batchID.Substring(0, 8) + "B.t_payfund_total as a1 ,c2c_zwdb_"
                    + batchID.Substring(0, 8) + "B.t_pay_relationship as a2 where a1.FbatchID='" + batchID + "' and a2.FSequence=a1.FSequence ";
            }

            if (state != 9)
            {
                strSql += " and a1.FStatus=" + state + " ";
            }

            if (username != null && username != "")
            {
                strSql += " and a1.FTrueName='" + username + "' ";
            }

            if (bankacc != null && bankacc != "")
            {
                strSql += " and a1.FBankAccNo='" + bankacc + "' ";
            }

            if (paybank != null && paybank != "0000" && paybank != "")
            {
                strSql += "and a1.FPayBankType=" + paybank + " ";
            }

            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("ZWFK"));
            try
            {
                da.OpenConn();
                string tmp = da.GetOneResult(strSql);
                int itmp = Int32.Parse(tmp);
                return itmp;
            }
            catch
            {
                return 0;
            }
            finally
            {
                da.Dispose();
            }
        }

        private string ConvertUid2QQ(string uid)
        {
            string errMsg = "";
            string strSql = "uid=" + uid;
            string qqid = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_USERINFO, "fqqid", out errMsg);

            //furion 20070309 email相关。
            if (qqid == null || qqid.Trim() == "")
            {

                qqid = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_USERINFO, "femail", out errMsg);
            }

            if (qqid == null || qqid.Trim() == "")
            {

                qqid = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_USERINFO, "Fmobile", out errMsg);
            }

            return qqid;
        }

        /// <summary>
        /// 显示指定批次号的付款明细数据
        /// </summary>
        /// <param name="max">最大记录条数</param>
        /// <param name="start">开始记录数</param>
        /// <param name="BatchID">指定批次号</param>
        /// <returns>返回数据</returns>
        [WebMethod]
        public DataSet BatpayDetail_BindData(int max, int start, string BatchID, int state, string username, string bankacc, string paybank)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("ZWFK"));
            //MySqlAccess da_yw = new MySqlAccess(PublicRes.GetConnString("YWB_V30"));
            MySqlAccess da_zl = new MySqlAccess(PublicRes.GetConnString("ZL"));
            try
            {
                string strSql = "SELECT a1.FSequence,a1.FTruename,a1.FBankAccNo,a1.FBankName,a1.Fstatus, '0' as FStatusName,a1.FPayBankType,"
                    + "(a1.Famt/100)Famt1,a2.FID FSequence1,a2.Ftde_id,a2.Fuid,(a2.Fnum/100)Fnum1, a2.FImportFlag, '0' as FStatusName1,a1.FRTFlag  FROM c2c_zwdb_"
                    + BatchID.Substring(0, 8) + ".t_payfund_total as a1 ,c2c_zwdb_"
                    + BatchID.Substring(0, 8) + ".t_pay_relationship as a2  where a2.FSequence=a1.FSequence and FbatchID='" + BatchID + "' ";

                if (BatchID.Length == 14)
                {
                    strSql = "SELECT a1.FSequence,a1.FTruename,a1.FBankAccNo,a1.FBankName,a1.Fstatus, '0' as FStatusName,a1.FPayBankType,"
                        + "(a1.Famt/100)Famt1,a2.FID FSequence1,a2.Ftde_id,a2.Fuid,(a2.Fnum/100)Fnum1, a2.FImportFlag, '0' as FStatusName1,a1.FRTFlag  FROM c2c_zwdb_"
                        + BatchID.Substring(0, 8) + "B.t_payfund_total as a1 ,c2c_zwdb_"
                        + BatchID.Substring(0, 8) + "B.t_pay_relationship as a2  where a2.FSequence=a1.FSequence and FbatchID='" + BatchID + "' ";
                }

                if (state != 9)
                {
                    if (state < 5)
                    {
                        strSql += " and a1.FStatus=" + state + " ";
                    }
                    else if (state == 5)
                    {
                        strSql += " and a1.FStatus=2 and ifnull(a1.FRTFlag,0)=0 ";
                    }
                    else if (state == 6)
                    {
                        strSql += " and a1.FStatus=2 and a1.FRTFlag=1 ";
                    }
                    else if (state == 7)
                    {
                        strSql += " and a1.FStatus=2 and a1.FRTFlag=2 ";
                    }
                }

                if (username != null && username != "")
                {
                    strSql += " and a1.FTrueName='" + username + "' ";
                }

                if (bankacc != null && bankacc != "")
                {
                    string serBankaccno = bankacc;
                    if (!bankacc.StartsWith("X"))
                    {
                        serBankaccno = BankLib.BankIOX.GetCreditEncode(bankacc, BankLib.BankIOX.fxykconn);
                    }
                    strSql += " and (a1.FBankAccNo='" + serBankaccno + "' or a1.FBankAccNo='" + bankacc + "') ";
                }

                if (paybank != null && paybank != "0000" && paybank != "")
                {
                    strSql += "and a1.FPayBankType=" + paybank + " ";
                }

                strSql += " order  by a1.Famt desc ,FSequence asc,Fnum1 desc";
                da.OpenConn();
                DataSet ds = da.dsGetTableByRange(strSql, start, max);

                DataTable dt;
                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                }
                else
                {
                    dt = null;
                }

                da.CloseConn();

                //da_yw.OpenConn();

                da_zl.OpenConn();

                if (dt != null && dt.Rows.Count > 0)
                {
                    string strIndex = "";

                    dt.Columns.Add("FQQID", typeof(String));
                    dt.Columns.Add("FRTFlagName", typeof(String));
                    dt.Columns.Add("FPayBankTypeName", typeof(String));

                    foreach (DataRow dr in dt.Rows)
                    {
                        dr.BeginEdit();

                        if (dr["Fstatus"].ToString() == "0")
                        {
                            dr["FStatusName"] = "已生成";
                        }
                        else if (dr["Fstatus"].ToString() == "1")
                        {
                            dr["FStatusName"] = "已提交付款";
                        }
                        else if (dr["Fstatus"].ToString() == "2")
                        {
                            dr["FStatusName"] = "付款成功";
                        }
                        else if (dr["Fstatus"].ToString() == "3")
                        {
                            dr["FStatusName"] = "<font Color=red>付款失败</font>";
                        }

                        if (dr["FImportFlag"].ToString() == "0")
                        {
                            dr["FStatusName1"] = "初始状态";
                        }
                        else if (dr["FImportFlag"].ToString() == "1")
                        {
                            dr["FStatusName1"] = "回导成功";
                        }
                        else if (dr["FImportFlag"].ToString() == "2")
                        {
                            dr["FStatusName1"] = "<font Color=red>回导失败</font>";
                        }
                        else if (dr["FImportFlag"].ToString() == "3")
                        {
                            dr["FStatusName1"] = "已标记";
                        }

                        if (dr["FRTFlag"] == null || dr["FRTFlag"].ToString() == "" || dr["FRTFlag"].ToString() == "0")
                        {
                            dr["FRTFlagName"] = "";
                        }
                        else if (dr["FRTFlag"].ToString() == "1")
                        {
                            dr["FRTFlagName"] = "退票申请中";
                        }
                        else if (dr["FRTFlag"].ToString() == "2")
                        {
                            dr["FRTFlagName"] = "已退票";
                        }

                        if (dr["FPayBankType"] != null)
                        {
                            dr["FPayBankTypeName"] = Q_TCBANKPAY_LIST.GetBankName(dr["FPayBankType"].ToString().Trim());
                        }

                        if (dr["FSequence"] != null)
                        {
                            if (strIndex != dr["FSequence"].ToString().Trim())
                            {
                                strIndex = dr["FSequence"].ToString().Trim();
                            }
                            else
                            {
                                dr["FSequence"] = DBNull.Value;
                                dr["FTruename"] = DBNull.Value;
                                dr["FBankAccNo"] = DBNull.Value;
                                dr["FBankName"] = DBNull.Value;
                                dr["FStatusName"] = DBNull.Value;
                                dr["Famt1"] = DBNull.Value;
                            }
                        }

                        string uid = dr["fuid"].ToString();
                        if (uid != null && uid.Trim() != "")
                        {                           
                            string qqid = ConvertUid2QQ(uid);

                            dr["FQQID"] = qqid;
                        }
                        dr.EndEdit();
                    }
                }

                return ds;
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
            finally
            {
                da.Dispose();
                //da_yw.Dispose();
                da_zl.Dispose();
            }
        }

        #endregion

        #region 订单实时查询
        [WebMethod(Description = "订单查询个数函数")]
        public int GetRealTimeOrderListCount(string u_ID, DateTime u_BeginTime, DateTime u_EndTime, int fstate, float fnum, string banktype)
        {
            try
            {
                RealTimeOrderQueryClass cuser = new RealTimeOrderQueryClass(u_ID, u_BeginTime, u_EndTime, fstate, fnum, banktype, "0");
                return cuser.GetCount("DD");
            }
            catch (Exception e)
            {
                throw new Exception("service发生错误,请联系管理员！");
                return 0;
            }
        }

        [WebMethod(Description = "订单查询函数")]
        public DataSet GetRealTimeOrderList(string u_ID, DateTime u_BeginTime, DateTime u_EndTime, int fstate, float fnum, string banktype, string sorttype, int iPageStart, int iPageMax)
        {
            try
            {
                RealTimeOrderQueryClass cuser = new RealTimeOrderQueryClass(u_ID, u_BeginTime, u_EndTime, fstate, fnum, banktype, sorttype);
                return cuser.GetResultX(iPageStart, iPageMax, "DD");
            }
            catch (Exception e)
            {
                throw new Exception("service发生错误,请联系管理员！");
            }
        }

        #endregion
        // 2011/7/21
        #region	2011/7/21


        [WebMethod(Description = "通过签约号查询投资人签约解约信息")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet GetInversatorSignDetail(int signType, string strID, string serialno, string cerNum, string spid, string spName
            , string beginDateStr, string endDateStr, int lim_start, int lim_count)
        {

            RightAndLog rl = new RightAndLog();
            try
            {
                if (myHeader == null)
                {
                    throw new LogicException("不正确的调用方法！");
                }

                rl.actionType = "查询投资人签约解约信息";
                rl.ID = strID;
                rl.OperID = myHeader.OperID;
                rl.sign = 1;
                rl.strRightCode = "";
                rl.RightString = myHeader.RightString;
                rl.SzKey = myHeader.SzKey;
                rl.type = "查询";
                rl.UserID = myHeader.UserName;
                rl.UserIP = myHeader.UserIP;
    
                Q_InvestorSignInfo query = new Q_InvestorSignInfo(signType, strID, serialno, cerNum, spid, spName, beginDateStr, endDateStr, lim_start, lim_count);

                DataSet queryResult = query.GetResultX_ICE();
                //DataSet queryResult = query.GetResultFromSql();

                if (queryResult == null || queryResult.Tables.Count == 0 || queryResult.Tables[0].Rows.Count == 0)
                    return null;

                queryResult.Tables[0].Columns.Add("Fapp_codeName", typeof(string));
                queryResult.Tables[0].Columns.Add("FdirectName", typeof(string));
                queryResult.Tables[0].Columns.Add("Fbind_channelName", typeof(string));
                queryResult.Tables[0].Columns.Add("Fbind_status_newName", typeof(string));

                queryResult.Tables[0].Columns.Add("Fop_typeName", typeof(string));
                queryResult.Tables[0].Columns.Add("Fchannel_idName", typeof(string));
                queryResult.Tables[0].Columns.Add("Funchain_channelName", typeof(string));
                queryResult.Tables[0].Columns.Add("Fcur_typeName", typeof(string));

                foreach (DataRow dr in queryResult.Tables[0].Rows)
                {
                    switch (dr["Fapp_code"].ToString())
                    {
                        case "1":
                            {
                                dr["Fapp_codeName"] = "基金行业";
                                break;
                            }
                        default:
                            {
                                dr["Fapp_codeName"] = "未定义类型"; break;
                            }
                    }

                    switch (dr["Fdirect"].ToString())
                    {
                        case "1":
                            { dr["FdirectName"] = "出"; break; }
                        default:
                            { dr["FdirectName"] = "入"; break; }
                    }

                    switch (dr["Fbind_channel"].ToString())
                    {
                        case "0": { dr["Fbind_channelName"] = "未知"; break; }
                        case "1": { dr["Fbind_channelName"] = "在线签约（商户发起）"; break; }
                        case "2": { dr["Fbind_channelName"] = "在线签约（财付通主站发起）"; break; }
                        case "3": { dr["Fbind_channelName"] = "离线签约"; break; }
                        case "4": { dr["Fbind_channelName"] = "柜台签约，短信确认签约关系"; break; }
                        default: { dr["Fbind_channelName"] = "未知"; break; }
                    }

                    switch (dr["Fbind_status_new"].ToString())
                    {
                        case "0": { dr["Fbind_status_newName"] = "未定义"; break; }
                        case "1": { dr["Fbind_status_newName"] = "初始状态"; break; }
                        case "2": { dr["Fbind_status_newName"] = "开启"; break; }
                        case "3": { dr["Fbind_status_newName"] = "关闭"; break; }
                        case "4": { dr["Fbind_status_newName"] = "解除"; break; }
                        case "5": { dr["Fbind_status_newName"] = "下发短信中"; break; }
                        default: { dr["Fbind_status_newName"] = "未知"; break; }
                    }


                    switch (dr["Fop_type"].ToString())
                    {
                        case "1": { dr["Fop_typeName"] = "签约"; break; }
                        case "2": { dr["Fop_typeName"] = "解约"; break; }
                        case "3": { dr["Fop_typeName"] = "关闭签约关系"; break; }
                        case "4": { dr["Fop_typeName"] = "重新打开签约关系"; break; }
                        case "5": { dr["Fop_typeName"] = "修改支付渠道"; break; }
                        case "6": { dr["Fop_typeName"] = "修改限额"; break; }
                        case "7": { dr["Fop_typeName"] = "签约通知商户回导成功"; break; }
                        default: { dr["Fop_typeName"] = "未知"; break; }
                    }

                    switch (dr["Fchannel_id"].ToString())
                    {
                        case "1": { dr["Fchannel_idName"] = "手机支付"; break; }
                        case "2": { dr["Fchannel_idName"] = "财付通直通车"; break; }
                        case "3": { dr["Fchannel_idName"] = "自动扣款"; break; }
                        case "4": { dr["Fchannel_idName"] = "希之光网吧平台"; break; }
                        case "5": { dr["Fchannel_idName"] = "拍拍保证金"; break; }
                        case "6": { dr["Fchannel_idName"] = "占位"; break; }
                        case "7": { dr["Fchannel_idName"] = "平台专用账户"; break; }
                        case "8": { dr["Fchannel_idName"] = "基金基础代扣"; break; }
                        default: { dr["Fchannel_idName"] = "未知"; break; }
                    }

                    // 解约渠道的具体转换，有待确认
                    switch (dr["Funchain_channel"].ToString())
                    {
                        case "0": { dr["Funchain_channelName"] = "未知"; break; }
                        case "1": { dr["Funchain_channelName"] = "在线签约（商户发起）"; break; }
                        case "2": { dr["Funchain_channelName"] = "在线签约（财付通主站发起）"; break; }
                        default: { dr["Funchain_channelName"] = "未知"; break; }
                    }

                    switch (dr["Fcur_type"].ToString())
                    {
                        case "1": { dr["Fcur_typeName"] = "RMB"; break; }
                        case "2": { dr["Fcur_typeName"] = "基金币种"; break; }
                        default: { dr["Fcur_typeName"] = "未知"; break; }
                    }
                }

                return queryResult;
            }
            catch (LogicException err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                throw;
            }
            catch (Exception err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                throw new LogicException("Service处理失败！");
            }
            finally
            {
                rl.WriteLog();
            }

        }

        [WebMethod(Description = "以财付通订单号帐号或基金易帐号或两者一齐查询基金易信息")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet GetFundInfo(string listID, string uid, string beginDateStr, string endDateStr, string purType, int limStart, int limCount)
        {

            RightAndLog rl = new RightAndLog();
            try
            {
                if (myHeader == null)
                {
                    throw new LogicException("不正确的调用方法！");
                }

                rl.actionType = "查询基金信息";
                rl.ID = uid;
                rl.OperID = myHeader.OperID;
                rl.sign = 1;
                rl.strRightCode = "";
                rl.RightString = myHeader.RightString;
                rl.SzKey = myHeader.SzKey;
                rl.type = "查询";
                rl.UserID = myHeader.UserName;
                rl.UserIP = myHeader.UserIP;

                Q_FundInfo query = new Q_FundInfo(listID, uid, beginDateStr, endDateStr, purType, limStart, limCount);

                DataSet queryResult = query.GetResultX_ICE();

                if (queryResult == null || queryResult.Tables.Count == 0 || queryResult.Tables[0].Rows.Count == 0)
                    return null;

                queryResult.Tables[0].Columns.Add("CFTAccount");
                queryResult.Tables[0].Columns.Add("FInvestorName");
                queryResult.Tables[0].Columns.Add("FcurTypeName");

                foreach (DataRow dr in queryResult.Tables[0].Rows)
                {
                    // 只显示银行帐号末尾4位
                    string tempStr = dr["Fcard_no"].ToString();
                    if (tempStr.Length > 4)
                        dr["Fcard_no"] = "*********" + tempStr.Substring(tempStr.Length - 4);

                    try
                    {
                        dr["Ftotal_fee"] = (MoneyTransfer.FenToYuan(dr["Ftotal_fee"].ToString())).ToString();
                        dr["Ffund_value"] = (MoneyTransfer.FenToYuan(dr["Ffund_value"].ToString())).ToString();

                        if (queryResult.Tables[0].Columns.Contains("Fcur_type"))
                        {//用于兼容数据库字段还未上线情况
                            if (!(dr["Fcur_type"] is DBNull))
                            {
                                var tmp = dr["Fcur_type"].ToString();
                                if (tmp == "1")
                                    dr["FcurTypeName"] = "RMB";
                                else if (tmp == "2")
                                    dr["FcurTypeName"] = "基金";
                                else if (tmp == "90")
                                    dr["FcurTypeName"] = "余额增值";
                                else
                                    dr["FcurTypeName"] = "未知：" + tmp;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        string str = ex.Message;
                    }

                    switch (dr["Fpur_type"].ToString())
                    {
                        case "1":
                            {
                                dr["Fpur_type"] = "申购";
                                break;
                            }
                        case "2":
                            {
                                dr["Fpur_type"] = "认购";
                                break;
                            }
                        case "3":
                            {
                                dr["Fpur_type"] = "定投";
                                break;
                            }
                        case "4":
                            {
                                dr["Fpur_type"] = "赎回";
                                break;
                            }
                        case "5":
                            {
                                dr["Fpur_type"] = "撤销";
                                break;
                            }
                        case "6":
                            {
                                dr["Fpur_type"] = "分红";
                                break;
                            }
                        case "7":
                            {
                                dr["Fpur_type"] = "认申购失败";
                                break;
                            }
                        case "8":
                            {
                                dr["Fpur_type"] = "比例确认退款";
                                break;
                            }
                        default:
                            {
                                dr["Fpur_type"] = "未定义类型";
                                break;
                            }
                    }

                    switch (dr["Fstate"].ToString())
                    {
                        case "1":
                            {
                                dr["Fstate"] = "等待扣款";
                                break;
                            }
                        case "2":
                            {
                                dr["Fstate"] = "代扣成功";
                                break;
                            }
                        default:
                            {
                                //dr["Fstate"] = "未知类型" + dr["Fstate"].ToString();
                                dr["Fstate"] = null;
                                break;
                            }
                    }

                    switch (dr["Flstate"].ToString())
                    {
                        case "1":
                            {
                                dr["Flstate"] = "有效";
                                break;
                            }
                        case "2":
                            {
                                dr["Flstate"] = "无效";
                                break;
                            }
                        default:
                            {
                                dr["Flstate"] = null;
                                break;
                            }
                    }
                }

                return queryResult;
            }
            catch (LogicException err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                throw;
            }
            catch (Exception err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                throw new LogicException("Service处理失败！");
            }
            finally
            {
                rl.WriteLog();
            }

        }


        [WebMethod(Description = "查询投资人充值或提现信息")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet GetChargeInfo(string qType, string strID, string beginDateStr, string endDateStr, string listid, int lim_start, int lim_count)
        {
            RightAndLog rl = new RightAndLog();
            try
            {
                if (myHeader == null)
                {
                    throw new LogicException("不正确的调用方法！");
                }

                rl.actionType = "查询投资人充值提现信息";
                rl.ID = strID;
                rl.OperID = myHeader.OperID;
                rl.sign = 1;
                rl.strRightCode = "";
                rl.RightString = myHeader.RightString;
                rl.SzKey = myHeader.SzKey;
                rl.type = "查询";
                rl.UserID = myHeader.UserName;
                rl.UserIP = myHeader.UserIP;
    
                Q_ChargeInfo query = new Q_ChargeInfo(qType, strID, beginDateStr, endDateStr, listid, lim_start, lim_count);
                DataSet queryResult = query.GetResultX_ICE();

                if (queryResult == null || queryResult.Tables.Count == 0 || queryResult.Tables[0].Rows.Count == 0)
                    return null;

                queryResult.Tables[0].Columns.Add("FtypeName", typeof(string));
                queryResult.Tables[0].Columns.Add("Fbank_typeName", typeof(string));
                queryResult.Tables[0].Columns.Add("Flist_signName", typeof(string));
                queryResult.Tables[0].Columns.Add("FcurtypeName", typeof(string));
                queryResult.Tables[0].Columns.Add("FsubjectName", typeof(string));

                foreach (DataRow dr in queryResult.Tables[0].Rows)
                {
                    try
                    {
                        dr["Fbalance"] = (MoneyTransfer.FenToYuan(dr["Fbalance"].ToString())).ToString();
                        dr["Fpaynum"] = (MoneyTransfer.FenToYuan(dr["Fpaynum"].ToString())).ToString();
                    }
                    catch (Exception ex)
                    {
                        string str = ex.Message;
                    }

                    switch (dr["Ftype"].ToString())
                    {
                        case "1":
                            {
                                dr["FtypeName"] = "入"; break;
                            }
                        case "2":
                            {
                                dr["FtypeName"] = "出"; break;
                            }
                        case "3":
                            {
                                dr["FtypeName"] = "冻结"; break;
                            }
                        case "4":
                            {
                                dr["FtypeName"] = "解冻"; break;
                            }
                        default:
                            {
                                dr["FtypeName"] = "未知"; break;
                            }
                    }

                    dr["Fbank_typeName"] = getData.getBankName(dr["Fbank_type"].ToString());

                    switch (dr["Flist_sign"].ToString())
                    {
                        case "0":
                            {
                                dr["Flist_signName"] = "正常";
                                break;
                            }
                        case "1":
                            {
                                dr["Flist_signName"] = "被冲正";
                                break;
                            }
                        case "2":
                            {
                                dr["Flist_signName"] = "冲正";
                                break;
                            }
                        default:
                            {
                                dr["Flist_signName"] = "未知";
                                break;
                            }
                    }

                    dr["FcurtypeName"] = getData.GetCurTypeName(dr["Fcurtype"].ToString());
                    dr["FsubjectName"] = getData.GetSubjectName(dr["Fsubject"].ToString());
                }

                return queryResult;
            }
            catch (LogicException err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                throw;
            }
            catch (Exception err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                throw new LogicException("Service处理失败！");
            }
            finally
            {
                rl.WriteLog();
            }
        }


        [WebMethod(Description = "通过证件号码查询用户内部ID")]
        public string QueryUid_ByCerNum(string cerNum)
        {
            try
            {
                string errMsg = "";

                return CommQuery.GetOneResultFromICE("cre_id=" + cerNum, "QUERY_UID_BYCERINFO", "Fuid", out errMsg);
            }
            catch
            {
                return "0";
            }
        }

        /// <summary>
        /// 查询理财通用户账户信息
        /// </summary>
        /// <param name="uin"></param>
        /// <returns></returns>
        [WebMethod(Description = "查询理财通用户账户信息", MessageName = "GetUserFundAccountInfo2")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet GetUserFundAccountInfo(string uin)
        {
            try
            {
                using (MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("fund")))
                {
                    da.OpenConn();
                    string Sql = string.Format("select * from fund_db.t_fund_bind  where Fqqid='{0}'", uin);
                    DataSet ds = da.dsGetTotalData(Sql);

                    if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                        return null;

                    ds.Tables[0].Columns.Add("FstateName", typeof(string));
                    ds.Tables[0].Columns.Add("FlstateName", typeof(string));

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (dr["Fstate"].ToString() == "1")
                        {
                            dr["FstateName"] = "初始态";
                        }
                        else if (dr["Fstate"].ToString() == "2")
                        {
                            dr["FstateName"] = "审核中（预留）";
                        }
                        else if (dr["Fstate"].ToString() == "3")
                        {
                            dr["FstateName"] = "开户完成";
                        }
                        else
                        {
                            dr["FstateName"] = "未定义的状态：" + dr["Fstate"];
                        }

                    }

                    return ds;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("service发生错误，请联系管理员！{0}", ex.Message));
            }

        }


        [WebMethod(Description = "查询基金用户的信息")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet GetUserFundAccountInfo(int type, string param1, string param2)
        {
            RightAndLog rl = new RightAndLog();
            try
            {
                if (myHeader == null)
                {
                    throw new LogicException("不正确的调用方法！");
                }

                rl.actionType = "查询基金用户的信息";
                rl.ID = param2;
                rl.OperID = myHeader.OperID;
                rl.sign = 1;
                rl.strRightCode = "";
                rl.RightString = myHeader.RightString;
                rl.SzKey = myHeader.SzKey;
                rl.type = "查询";
                rl.UserID = myHeader.UserName;
                rl.UserIP = myHeader.UserIP;
    
                string strWhere = "";

                switch (type)
                {
                    case 1:
                        {
                            strWhere += "trade_id=" + param1;
                            break;
                        }
                    case 2:
                        {
                            int creType = 1;
                            if (param1 == "2")
                            {
                                creType = 2;
                            }
                            else if (param1 == "3")
                            {
                                creType = 6;
                            }
                            else if (param1 == "4")
                            {
                                creType = 7;
                            }

                            strWhere += "cre_type=" + creType;
                            strWhere += "&cre_id=" + param2;
                            break;
                        }
                    case 3:
                        {
                            string fuid = PublicRes.ConvertToFuid(param1);

                            if (fuid == null || fuid.Trim() == "")
                                fuid = "0";

                            strWhere += "uid=" + fuid;
                            break;
                        }
                    case 4:
                        {
                            strWhere += "uid=" + param1;
                            break;
                        }
                    case 5:
                        {
                            string fuid = PublicRes.ConvertToFuid(param1);

                            if (fuid == null || fuid.Trim() == "")
                            {
                                fuid = "0";
                            }

                            strWhere += "uid=" + fuid;
                            break;
                        }
                    default:
                        {
                            throw new Exception("未知的搜索类型:" + type);
                        }
                }

                strWhere += "&lim_start=" + 0;
                strWhere += "&lim_count=" + 1;

                string errMsg = "";

                DataSet ds = CommQuery.GetDataSetFromICE(strWhere, "QUERY_FUNDACCOUNTINFO", out errMsg);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    return null;

                ds.Tables[0].Columns.Add("FstateName", typeof(string));
                ds.Tables[0].Columns.Add("FlstateName", typeof(string));

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr["Fstate"].ToString() == "1")
                    {
                        dr["FstateName"] = "初始态";
                    }
                    else if (dr["Fstate"].ToString() == "2")
                    {
                        dr["FstateName"] = "审核中（预留）";
                    }
                    else if (dr["Fstate"].ToString() == "3")
                    {
                        dr["FstateName"] = "开户完成";
                    }
                    else
                    {
                        dr["FstateName"] = "未定义的状态：" + dr["Fstate"];
                    }

                    if (dr["Flstate"].ToString() == "1")
                    {
                        dr["FlstateName"] = "有效";
                    }
                    else if (dr["Flstate"].ToString() == "2")
                    {
                        dr["FlstateName"] = "无效";
                    }
                    else
                    {
                        dr["FlstateName"] = "未定义的状态：" + dr["Flstate"];
                    }
                }
                return ds;
            }
            catch (Exception err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                throw new LogicException("Service处理失败！" + err.Message);
            }
            finally
            {
                rl.WriteLog();
            }
        }


        [WebMethod(Description = "查询基金账户绑定银行卡的信息")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet GetUserFundCardBind(string fuid, string bankType, string bankID)
        {
            RightAndLog rl = new RightAndLog();
            try
            {
                if (myHeader == null)
                {
                    throw new LogicException("不正确的调用方法！");
                }

                rl.actionType = "查询基金账户银行卡绑定信息";
                rl.ID = fuid;
                rl.OperID = myHeader.OperID;
                rl.sign = 1;
                rl.strRightCode = "";
                rl.RightString = myHeader.RightString;
                rl.SzKey = myHeader.SzKey;
                rl.type = "查询";
                rl.UserID = myHeader.UserName;
                rl.UserIP = myHeader.UserIP;

                string errMsg = "";
                string strWhere = "";

                if (fuid != "")
                    strWhere += "&uid=" + fuid;

                strWhere += "&lim_start=0&lim_count=20";

                DataSet ds2 = CommQuery.GetDataSetFromICE(strWhere, "QUERY_FUND_BINDCARD", out errMsg);

                if (ds2 == null || ds2.Tables.Count == 0 || ds2.Tables[0].Rows.Count == 0)
                    return null;

                ds2.Tables[0].Columns.Add("Fbind_stateName", typeof(string));

                foreach (DataRow dr in ds2.Tables[0].Rows)
                {
                    switch (dr["Fbind_state"].ToString())
                    {
                        case "1":
                            {
                                dr["Fbind_stateName"] = "绑定中"; break;
                            }
                        case "2":
                            {
                                dr["Fbind_stateName"] = "绑定完成"; break;
                            }
                        default:
                            {
                                dr["Fbind_stateName"] = "未知" + dr["Fbind_state"].ToString(); break;
                            }
                    }
                }

                return ds2;
            }
            catch (LogicException err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                throw;
            }
            catch (Exception err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                throw new LogicException("Service处理失败！");
            }
            finally
            {
                rl.WriteLog();
            }
        }


        [WebMethod(Description = "查询基金账户的支付银行卡信息")]
        public DataTable GetPayCardInfo(string qqid)
        {
            try
            {
                if (qqid == null || qqid == "")
                {
                    throw new LogicException("财付通账号不能为空！");
                }

                string fuid = PublicRes.ConvertToFuid(qqid);
                if (string.IsNullOrEmpty(fuid))
                {
                    throw new LogicException("财付通账号不存在！");
                }

                string sql = "select * from fund_db.t_fund_pay_card where Fqqid='" + qqid + "'";

                using (MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("fund")))
                {
                    da.OpenConn();
                    var dt = da.GetTable(sql);
                    dt.TableName = "PayCardInfo";
                    return dt;
                }
            }
            catch (Exception err)
            {
                throw new LogicException("Service处理失败！");
            }

        }

        [WebMethod(Description = "查询基金账户的支付银行卡信息(将银行转义封装在里面)")]
        public DataTable GetPayCardInfoEx(string qqid)
        {
            try
            {
                if (qqid == null || qqid == "")
                {
                    throw new LogicException("财付通账号不能为空！");
                }

                string fuid = PublicRes.ConvertToFuid(qqid);
                if (string.IsNullOrEmpty(fuid))
                {
                    throw new LogicException("财付通账号不存在！");
                }

                string sql = "select * from fund_db.t_fund_pay_card where Fqqid='" + qqid + "'";

                using (MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("fund")))
                {
                    da.OpenConn();
                    var dt = da.GetTable(sql);
                    dt.TableName = "PayCardInfo";
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        dt.Columns.Add("Fbank_type_name", typeof(string));
                        foreach (DataRow dr in dt.Rows)
                        {
                            dr["Fbank_type_name"] = BankIO.QueryBankName(dt.Rows[0]["Fbank_type"].ToString());
                        }
                    }
                    return dt;
                }
            }
            catch (Exception err)
            {
                throw new LogicException("Service处理失败！");
            }

        }
        /// <summary>
        /// 查询用户绑定银行卡，走relay hanson 2014.2.17
        /// </summary>
        /// <param name="SingedString"></param>
        /// <returns></returns>
        [WebMethod(Description = "查询用户绑定银行卡")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet GetBindBankCard(string qqId)
        {
            try
            {

                string serviceName = "bind_textquery_service";
                string ICEcommand = string.Empty;
                string ICESql = "qqid=" + qqId + "&query_type=1&bind_flag=1&result_type=2&bind_type_cond=all_type";
                string errmsg = string.Empty;

                DataSet ds = CommQuery.GetDataSetFromICEService(ICESql, ICEcommand, false, serviceName, out errmsg, false);
                if (!string.IsNullOrEmpty(errmsg))
                {
                    throw new Exception("调用ICE失败：" + errmsg);
                }
                return ds;
            }
            catch (Exception ex)
            {
                throw new LogicException("获取用户绑定银行卡异常：" + ex.Message);
            }
        }

        [WebMethod(Description = "查询理财通支持的银行类型")]
        public DataTable GetFundSupportBank()
        {
            try
            {

                string sql = "select * from fund_db.t_fund_bank_config where Flstate=1";

                using (MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("fund")))
                {
                    da.OpenConn();
                    var dt = da.GetTable(sql);
                    dt.TableName = "SupportBank";
                    return dt;
                }
            }
            catch (Exception err)
            {
                throw new LogicException("Service处理失败！");
            }
        }

        [WebMethod(Description = "修改理财通支付银行卡")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public bool ModifyFundPayCard(string qqId, string tradeId, string uid, string bindSerialNo, string bankType, string cardTail, string bankId, string mobile)
        {
            try
            {
                if (myHeader == null)
                {
                    throw new Exception("不正确的调用方法！");
                }
                bool actionResult = false;

                string key = ConfigurationManager.AppSettings["FundPayCardModifyKey"];
                string tokenValue = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}", qqId, uid, bindSerialNo, bankType, cardTail, bankId, mobile, key);
                string token = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(tokenValue, "md5").ToLower();

                StringBuilder reqBuilder = new StringBuilder()
                .AppendFormat("uin={0}", qqId)
                .AppendFormat("&uid={0}", uid)
                .AppendFormat("&bind_serialno={0}", bindSerialNo)
                .AppendFormat("&bank_type={0}", bankType)
                .AppendFormat("&card_tail={0}", cardTail)
                .AppendFormat("&bank_id={0}", bankId)
                .AppendFormat("&mobile={0}", mobile)
                .AppendFormat("&token={0}", token)
                .AppendFormat("&memo={0}", myHeader.UserName);

                string serviceName = "fund_nopass_reset_paycard_service";
                string ICEcommand = string.Empty;
                string ICESql = reqBuilder.ToString();
                string errmsg = string.Empty;

                DataSet ds = CommQuery.GetOneTableFromICE(ICESql, ICEcommand, serviceName, true, out errmsg);

                if (!string.IsNullOrEmpty(errmsg) || ds == null || ds.Tables.Count <= 0)
                {
                    throw new Exception("调用ICE失败：" + errmsg);
                }
                else if (ds.Tables[0].Rows[0]["result"].ToString() != "0" || ds.Tables[0].Rows[0]["res_info"].ToString() != "ok")
                {
                    throw new Exception("调用接口异常：" + ds.Tables[0].Rows[0]["res_info"].ToString());
                }
                else
                {
                    actionResult = true;
                }

                return actionResult;
            }
            catch (Exception ex)
            {
                throw new LogicException("修改理财通支付银行卡：" + ex.Message);
            }

        }

        [Obsolete("合并到FundRoll中QueryFundRollList")]
        [WebMethod(Description = "获取理财通交易记录")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet GetFundTradeList(string qqId, int pageIndex, int pageSize)
        {
            try
            {
                if (qqId == null || qqId == "")
                {
                    throw new LogicException("财付通账号不能为空！");
                }

                string fuid = PublicRes.ConvertToFuid(qqId);
                if (string.IsNullOrEmpty(fuid))
                {
                    throw new LogicException("财付通账号不存在！");
                }

                var tableName = string.Format("fund_db_{0}.t_trade_user_fund_{1}", fuid.Substring(fuid.Length - 2, 2), fuid.Substring(fuid.Length - 3, 1));
                var sql = string.Format("select * from {0} where Fuid='{1}' and Fcur_type=90 limit {2},{3}", tableName, fuid, pageIndex * pageSize, pageSize);

                using (MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("fund")))
                {
                    da.OpenConn();
                    return da.dsGetTotalData(sql);
                }
            }
            catch (Exception err)
            {
                throw new LogicException(string.Format("Service处理失败！{0}", err.Message));
            }
        }

        #endregion


        // 2012/6/18
        #region 财付盾查询

        // 财付盾使用fattr=11检测
        [WebMethod(Description = "查询是否开通财付盾")]
        public string GetUserCrtCFDInfo(string qqid, int creType)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("CRT"));
            try
            {
                string uid = PublicRes.ConvertToFuid(qqid);

                if (uid == null || uid.Length < 3)
                {
                    return "用户不存在";
                }

                string strSql = "select Fvalue,Fstate from " + PublicRes.GetTName("cft_digit_crt", "t_user_attr", uid) + " where Fuid='"
                    + uid + "' and Fattr=" + creType;

                da.OpenConn();

                DataTable dt = da.GetTable(strSql);

                if (dt == null || dt.Rows.Count == 0)
                {
                    return "无证书";
                }

                int Fvalue = Int32.Parse(dt.Rows[0][0].ToString());
                int Fstate = Int32.Parse(dt.Rows[0][0].ToString());

                if (Fstate == 3)
                {
                    return "作废";
                }

                if (Fvalue == 1)
                {
                    return "开通";
                }
                else if (Fvalue == 2)
                {
                    return "未开通";
                }
                else if (Fvalue == 3)
                {
                    return "冻结";
                }

                return "未知情况" + Fstate + Fvalue;
            }
            catch (Exception err)
            {
                return "查询出错";
            }
            finally
            {
                da.Dispose();
            }
        }


        [WebMethod(Description = "查询财付盾绑定信息")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet QueryCFDInfo(string qqid)
        {
            RightAndLog rl = new RightAndLog();
            try
            {
                if (myHeader == null)
                {
                    throw new LogicException("不正确的调用方法！");
                }

                rl.actionType = "查询财付盾绑定信息";
                rl.ID = qqid;
                rl.OperID = myHeader.OperID;
                rl.sign = 1;
                rl.strRightCode = "";
                rl.RightString = myHeader.RightString;
                rl.SzKey = myHeader.SzKey;
                rl.type = "查询";
                rl.UserID = myHeader.UserName;
                rl.UserIP = myHeader.UserIP;

                if (GetUserCrtCFDInfo(qqid, 11) == "开通")
                {
                    string iceCmd = "FINANCE_QUERY_USER_CFDINFO";
                    string iceSql = "uin=" + qqid;
                    string errMsg = "";

                    DataSet ds = CommQuery.GetDataSetFromICE(iceSql, iceCmd, out errMsg);

                    return ds;
                }
                else
                {
                    return null;
                    //throw new Exception("用户没有开通财付盾");
                }

            }
            catch (LogicException err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                throw;
            }
            catch (Exception err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                throw new LogicException("Service处理失败！");
            }
            finally
            {
                rl.WriteLog();
            }
        }


        #endregion

        #region 证书管理

        [WebMethod(Description = "证书管理查询列表个数函数")]
        public int GetMediCertManageCount(string spid, int status, int liststatus)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("INC"));

            try
            {
                da.OpenConn();

                string strwhere = "";
                if (spid.Trim() != "")
                {
                    strwhere += " where Fspid='" + spid + "' ";
                }

                if (status != 0)
                {
                    if (strwhere == "")
                    {
                        strwhere += " where Fstatus=" + status + " ";
                    }
                    else
                    {
                        strwhere += " and Fstatus=" + status + " ";
                    }
                }

                if (liststatus != 0)
                {
                    if (strwhere == "")
                    {
                        strwhere += " where Flist_state=" + liststatus + " ";
                    }
                    else
                    {
                        strwhere += " and Flist_state=" + liststatus + " ";
                    }
                }

                string sql = " select * from c2c_db_inc.t_spm_crt " + strwhere;

                return da.dsGetTotalData(sql).Tables[0].Rows.Count;
            }
            catch (Exception e)
            {
                return 0;
            }
            finally
            {
                da.Dispose();
            }
        }

        [WebMethod(Description = "证书管理查询列表函数")]
        public DataSet GetMediCertManageList(string spid, int status, int liststatus, int iPageStart, int iPageMax)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("INC"));

            try
            {
                DataSet ds = new DataSet();

                da.OpenConn();

                string strwhere = "";
                if (spid.Trim() != "")
                {
                    strwhere += " where Fspid='" + spid + "' ";
                }

                if (status != 0)
                {
                    if (strwhere == "")
                    {
                        strwhere += " where Fstatus=" + status + " ";
                    }
                    else
                    {
                        strwhere += " and Fstatus=" + status + " ";
                    }
                }

                if (liststatus != 0)
                {
                    if (strwhere == "")
                    {
                        strwhere += " where Flist_state=" + liststatus + " ";
                    }
                    else
                    {
                        strwhere += " and Flist_state=" + liststatus + " ";
                    }
                }

                iPageStart += -1;
                string sql = " select * from c2c_db_inc.t_spm_crt " + strwhere + " limit " + iPageStart + "," + iPageMax;

                ds = da.dsGetTotalData(sql);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null)
                {

                    DataTable dt = ds.Tables[0];

                    dt.Columns.Add("FStatusName", typeof(String));
                    dt.Columns.Add("Flist_stateName", typeof(String));

                    foreach (DataRow dr in dt.Rows)
                    {
                        string tmp = dr["FStatus"].ToString();

                        if (tmp == "1")
                        {
                            dr["FStatusName"] = "请求生成私钥";
                        }
                        else if (tmp == "2")
                        {
                            dr["FStatusName"] = "请求生成证书请求";
                        }
                        else if (tmp == "3")
                        {
                            dr["FStatusName"] = "请求签证";
                        }
                        else if (tmp == "4")
                        {
                            dr["FStatusName"] = "请求格式转换";
                        }
                        else if (tmp == "5")
                        {
                            dr["FStatusName"] = "完成";
                        }

                        tmp = dr["Flist_state"].ToString();

                        if (tmp == "1")
                        {
                            dr["Flist_stateName"] = "有效";
                        }
                        else if (tmp == "2")
                        {
                            dr["Flist_stateName"] = "吊销";
                        }
                        else if (tmp == "3")
                        {
                            dr["Flist_stateName"] = "作废";
                        }
                    }
                    return ds;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                da.Dispose();
            }
        }


        [WebMethod(Description = "操作员查询列表函数")]
        public DataSet GetMediOperatorManageList(string spid, string account)
        {
            try
            {
                MediOperatorManageClass cuser = new MediOperatorManageClass(spid, account);

                //kf没有分页? 增加个默认分页.
                int istart = 1;
                int imax = 50;


                int start = istart - 1;
                if (start < 0) start = 0;

                string sql = cuser.ICESQL;
                sql += "&strlimit=" + "limit " + start + "," + imax;
                string errMsg = "";
                DataSet ds = CommQuery.GetDataSetFromICE(sql, CommQuery.QUERY_MUSER, out errMsg);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null)
                {

                    DataTable dt = ds.Tables[0];

                    dt.Columns.Add("FsexName", typeof(String));
                    dt.Columns.Add("FstateName", typeof(String));
                    dt.Columns.Add("Fop_typeName", typeof(String));
                    dt.Columns.Add("FlstateName", typeof(String));
                    //dt.Columns.Add("Fop_alias", typeof(String)); //操作员别名

                    foreach (DataRow dr in dt.Rows)
                    {
                        string tmp = dr["Fsex"].ToString();

                        if (tmp == "1")
                        {
                            dr["FsexName"] = "男";
                        }
                        else if (tmp == "2")
                        {
                            dr["FsexName"] = "女";
                        }

                        tmp = dr["Fstate"].ToString();

                        if (tmp == "1")
                        {
                            dr["FstateName"] = "正常";
                        }
                        else if (tmp == "2")
                        {
                            dr["FstateName"] = "关闭";
                        }

                        tmp = dr["Fop_type"].ToString();

                        if (tmp == "1")
                        {
                            dr["Fop_typeName"] = "管理员";
                        }
                        else if (tmp == "2")
                        {
                            dr["Fop_typeName"] = "操作员";
                        }

                        tmp = dr["Flstate"].ToString();

                        if (tmp == "1")
                        {
                            dr["FlstateName"] = "正常";
                        }
                        else if (tmp == "2")
                        {
                            dr["FlstateName"] = "作废";
                        }

                        //tmp = QuerySpidOrSpalias(dr["Fspid"].ToString(), "", dr["Fqqid"].ToString()); //通过商户号查询操作员别名
                        //dr["Fop_alias"] = tmp;
                    }
                    return ds;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                //da.Dispose();
            }
        }


        [WebMethod(Description = "操作员权限查询列表函数")]
        public int GetMediOperatorRole(string spid, string qqid)
        {           
            string Msg = "";
            string strSql = "qqid=" + qqid + "&spid=" + spid;
            string sign1 = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_MUSER, "Fsign1", out Msg);

            return Int32.Parse(sign1);
        }

        [WebMethod(Description = "获取商户操作员权限函数")]
        public int GetMediOperatorRoleNew(string spid, string qqid, int signorder)
        {
            return MediOperatorManageClass.GetRole(spid, qqid, signorder);
        }

        #endregion

        #region 同步记录查询

        [WebMethod]
        public void ResetSynRecordState(string transid, string createtime, int inum)
        {
            SynRecordClass.ResetSynRecordState(transid, createtime, inum);
        }

        [WebMethod]
        public bool SynPayState(string transid, string createtime)
        {
            string strMsg = "";
            return SynRecordClass.SynPayState(transid, createtime, out strMsg);
        }

        [WebMethod(Description = "获取同步查询列表函数")]
        public DataSet GetSynRecordQueryList(string transid, string begintime, string endtime, int paystatus,
            int synstatus, int syntype, int paytype, string spid, string spbillno, string purchaser,
            string bargainor, int flag, int synresult, int istart, int imax)
        {
            try
            {
                SynRecordClass cuser = new SynRecordClass(transid, begintime, endtime, paystatus,
                    synstatus, syntype, paytype, spid, spbillno, purchaser, bargainor, flag, synresult);

                int start = istart - 1;
                if (start < 0) start = 0;

                //DataSet ds = cuser.GetResultX(istart, imax, "INC");
                string strSql = cuser.ICESQL + "&strlimit=limit " + start + "," + imax;
                string errMsg = "";

                string servicename = CommQuery.QUERY_SYNREC_SP;
                if (transid != "")
                {
                    servicename = CommQuery.QUERY_SYNREC_ID;
                }

                DataSet ds = CommQuery.GetDataSetFromICE(strSql, servicename, out errMsg);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null)
                {

                    #region 转化操作
                    DataTable dt = ds.Tables[0];

                    dt.Columns.Add("FPay_StatusName", typeof(String));
                    dt.Columns.Add("Fsyn_statusName", typeof(String));
                    dt.Columns.Add("Frec_statusName", typeof(String));
                    dt.Columns.Add("Fsyn_typeName", typeof(String));
                    dt.Columns.Add("Fpay_typeName", typeof(String));
                    dt.Columns.Add("Fpay_channelName", typeof(String));

                    dt.Columns.Add("FverName", typeof(String));
                    dt.Columns.Add("Fsp_timeName", typeof(String));
                    dt.Columns.Add("Fbank_typeName", typeof(String));
                    dt.Columns.Add("Ftotal_feeName", typeof(String));
                    dt.Columns.Add("FpriceName", typeof(String));
                    dt.Columns.Add("Ftransport_feeName", typeof(String));
                    dt.Columns.Add("Fprocedure_feeName", typeof(String));
                    dt.Columns.Add("Ffee_typeName", typeof(String));
                    dt.Columns.Add("Ffee1Name", typeof(String));
                    dt.Columns.Add("Ffee2Name", typeof(String));
                    dt.Columns.Add("Ffee3Name", typeof(String));
                    dt.Columns.Add("FvfeeName", typeof(String));
                    dt.Columns.Add("Frp_feeName", typeof(String));
                    dt.Columns.Add("Frb_feeName", typeof(String));
                    dt.Columns.Add("Fcreat_timeName", typeof(String));
                    dt.Columns.Add("Fpay_timeName", typeof(String));
                    dt.Columns.Add("Flast_syn_timeName", typeof(String));
                    dt.Columns.Add("Flast_modify_timeName", typeof(String));
                    dt.Columns.Add("Fsyn_resultName", typeof(String));

                    dt.Columns.Add("flag", typeof(String));

                    foreach (DataRow dr in dt.Rows)
                    {
                        dr["flag"] = "0";

                        string tmp = dr["FPay_Status"].ToString();

                        if (tmp == "1")
                        {
                            dr["FPay_StatusName"] = "支付前";
                        }
                        else if (tmp == "2")
                        {
                            dr["FPay_StatusName"] = "支付成功";
                        }

                        tmp = dr["Fsyn_status"].ToString();

                        if (tmp == "1")
                        {
                            dr["Fsyn_statusName"] = "未同步";
                        }
                        else if (tmp == "2")
                        {
                            dr["Fsyn_statusName"] = "同步失败";
                        }
                        else if (tmp == "3")
                        {
                            dr["Fsyn_statusName"] = "同步成功";
                        }

                        tmp = dr["Frec_status"].ToString();

                        if (tmp == "0")
                        {
                            dr["Frec_statusName"] = "正常";
                        }
                        else if (tmp == "1")
                        {
                            dr["Frec_statusName"] = "作废";
                        }
                        else if (tmp == "3")
                        {
                            dr["Frec_statusName"] = "已删除";
                        }


                        tmp = dr["Fsyn_type"].ToString();

                        if (tmp == "0")
                        {
                            dr["Fsyn_typeName"] = "未定义";
                        }
                        else if (tmp == "1")
                        {
                            dr["Fsyn_typeName"] = "支付同步";
                        }
                        else if (tmp == "2")
                        {
                            dr["Fsyn_typeName"] = "确认收货同步";
                        }
                        else if (tmp == "3")
                        {
                            dr["Fsyn_typeName"] = "退款同步";
                        }

                        tmp = dr["Fpay_type"].ToString();

                        if (tmp == "1")
                        {
                            dr["Fpay_typeName"] = "C2C";
                        }
                        else if (tmp == "2")
                        {
                            dr["Fpay_typeName"] = "B2C";
                        }
                        else if (tmp == "3")
                        {
                            dr["Fpay_typeName"] = "未定义(充值)";
                        }
                        else if (tmp == "4")
                        {
                            dr["Fpay_typeName"] = "快速支付";
                        }
                        else if (tmp == "5")
                        {
                            dr["Fpay_typeName"] = "收款转账(我要收款)";
                        }
                        else if (tmp == "6")
                        {
                            dr["Fpay_typeName"] = "支付转账(按钮付款)";
                        }

                        tmp = dr["Fpay_channel"].ToString();

                        if (tmp == "0")
                        {
                            dr["Fpay_channelName"] = "未定义";
                        }
                        else if (tmp == "1")
                        {
                            dr["Fpay_channelName"] = "余额支付";
                        }
                        else if (tmp == "2")
                        {
                            dr["Fpay_channelName"] = "银行支付";
                        }


                        tmp = dr["Fver"].ToString();

                        if (tmp == "0")
                        {
                            dr["FverName"] = "未定义";
                        }
                        else if (tmp == "1")
                        {
                            dr["FverName"] = "版本1";
                        }
                        else if (tmp == "2")
                        {
                            dr["FverName"] = "版本2";
                        }

                        tmp = dr["Fsp_time"].ToString();
                        dr["Fsp_timeName"] = TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.GetTimeFromTick(tmp);

                        tmp = dr["Fbank_type"].ToString();
                        dr["Fbank_typeName"] = ""; //在客户端更好处理一些.

                        tmp = dr["Ftotal_fee"].ToString();
                        dr["Ftotal_feeName"] = MoneyTransfer.FenToYuan(tmp);

                        tmp = dr["Fprice"].ToString();
                        dr["FpriceName"] = MoneyTransfer.FenToYuan(tmp);

                        tmp = dr["Ftransport_fee"].ToString();
                        dr["Ftransport_feeName"] = MoneyTransfer.FenToYuan(tmp);

                        tmp = dr["Fprocedure_fee"].ToString();
                        dr["Fprocedure_feeName"] = MoneyTransfer.FenToYuan(tmp);

                        tmp = dr["Ffee_type"].ToString();
                        if (tmp == "1")
                        {
                            dr["Ffee_typeName"] = "RMB";
                        }
                        else if (tmp == "2")
                        {
                            dr["Ffee_typeName"] = "USD";
                        }
                        else if (tmp == "3")
                        {
                            dr["Ffee_typeName"] = "HKD";
                        }

                        tmp = dr["Ffee1"].ToString();
                        dr["Ffee1Name"] = MoneyTransfer.FenToYuan(tmp);

                        tmp = dr["Ffee2"].ToString();
                        dr["Ffee2Name"] = MoneyTransfer.FenToYuan(tmp);

                        tmp = dr["Ffee3"].ToString();
                        dr["Ffee3Name"] = MoneyTransfer.FenToYuan(tmp);

                        tmp = dr["Fvfee"].ToString();
                        dr["FvfeeName"] = MoneyTransfer.FenToYuan(tmp);

                        tmp = dr["Frp_fee"].ToString();
                        dr["Frp_feeName"] = MoneyTransfer.FenToYuan(tmp);

                        tmp = dr["Frb_fee"].ToString();
                        dr["Frb_feeName"] = MoneyTransfer.FenToYuan(tmp);

                        tmp = dr["Fcreat_time"].ToString();
                        dr["Fcreat_timeName"] = TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.GetTimeFromTick(tmp);

                        tmp = dr["Fpay_time"].ToString();
                        dr["Fpay_timeName"] = TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.GetTimeFromTick(tmp);

                        tmp = dr["Flast_syn_time"].ToString();
                        dr["Flast_syn_timeName"] = TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.GetTimeFromTick(tmp);

                        tmp = dr["Flast_modify_time"].ToString();
                        dr["Flast_modify_timeName"] = TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.GetTimeFromTick(tmp);


                        tmp = dr["Fsyn_result"].ToString();
                        if (tmp == "0")
                        {
                            dr["Fsyn_resultName"] = "同步成功";
                        }
                        else if (tmp == "1")
                        {
                            dr["Fsyn_resultName"] = "未定义";
                        }
                        else if (tmp == "2")
                        {
                            dr["Fsyn_resultName"] = "连接错误";
                        }
                        else if (tmp == "3")
                        {
                            dr["Fsyn_resultName"] = "apache访问错误";
                        }
                        else if (tmp == "4")
                        {
                            dr["Fsyn_resultName"] = "返回结果没有正确标示";
                        }
                        else if (tmp == "5")
                        {
                            dr["Fsyn_resultName"] = "url 解析格式错误";
                        }
                        else if (tmp == "6")
                        {
                            dr["Fsyn_resultName"] = "域名解释失败";
                        }
                    }


                    #endregion

                    return ds;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception e)
            {
                throw new Exception("service发生错误：" + e.Message);
                return null;
            }
        }


        [WebMethod(Description = "同步记录详细函数")]
        public DataSet GetSynRecordQueryDetail(string transid, string createdate, int flag)
        {
            try
            {

                string strSql = "transaction_id=" + transid + "&create_time=" + SynRecordClass.GetNowSynTableName();
                string errMsg = "";
                string servicename = CommQuery.QUERY_SYNREC_ID;

                DataSet ds = CommQuery.GetDataSetFromICE(strSql, servicename, out errMsg);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0] == null)
                {
                    strSql = "transaction_id=" + transid + "&create_time=" + SynRecordClass.GetPriorSynTableName();
                    ds = CommQuery.GetDataSetFromICE(strSql, servicename, out errMsg);
                }

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0] == null)
                {
                    strSql = "transaction_id=" + transid + "&create_time=" + SynRecordClass.GetSynTableNameFromDate(DateTime.Parse(createdate));
                    ds = CommQuery.GetDataSetFromICE(strSql, servicename, out errMsg);
                }

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0] == null)
                {
                    strSql = "transaction_id=" + transid + "&create_time=" + SynRecordClass.GetPriorSynTableNameFromDate(DateTime.Parse(createdate));
                    ds = CommQuery.GetDataSetFromICE(strSql, servicename, out errMsg);
                }

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null)
                {
                    #region 转化操作
                    DataTable dt = ds.Tables[0];

                    dt.Columns.Add("FPay_StatusName", typeof(String));
                    dt.Columns.Add("Fsyn_statusName", typeof(String));
                    dt.Columns.Add("Frec_statusName", typeof(String));
                    dt.Columns.Add("Fsyn_typeName", typeof(String));
                    dt.Columns.Add("Fpay_typeName", typeof(String));
                    dt.Columns.Add("Fpay_channelName", typeof(String));

                    dt.Columns.Add("FverName", typeof(String));
                    dt.Columns.Add("Fsp_timeName", typeof(String));
                    dt.Columns.Add("Fbank_typeName", typeof(String));
                    dt.Columns.Add("Ftotal_feeName", typeof(String));
                    dt.Columns.Add("FpriceName", typeof(String));
                    dt.Columns.Add("Ftransport_feeName", typeof(String));
                    dt.Columns.Add("Fprocedure_feeName", typeof(String));
                    dt.Columns.Add("Ffee_typeName", typeof(String));
                    dt.Columns.Add("Ffee1Name", typeof(String));
                    dt.Columns.Add("Ffee2Name", typeof(String));
                    dt.Columns.Add("Ffee3Name", typeof(String));
                    dt.Columns.Add("FvfeeName", typeof(String));
                    dt.Columns.Add("Frp_feeName", typeof(String));
                    dt.Columns.Add("Frb_feeName", typeof(String));
                    dt.Columns.Add("Fcreat_timeName", typeof(String));
                    dt.Columns.Add("Fpay_timeName", typeof(String));
                    dt.Columns.Add("Flast_syn_timeName", typeof(String));
                    dt.Columns.Add("Flast_modify_timeName", typeof(String));
                    dt.Columns.Add("Fsyn_resultName", typeof(String));

                    foreach (DataRow dr in dt.Rows)
                    {
                        string tmp = dr["FPay_Status"].ToString();

                        if (tmp == "1")
                        {
                            dr["FPay_StatusName"] = "支付前";
                        }
                        else if (tmp == "2")
                        {
                            dr["FPay_StatusName"] = "支付成功";
                        }

                        tmp = dr["Fsyn_status"].ToString();

                        if (tmp == "1")
                        {
                            dr["Fsyn_statusName"] = "未同步";
                        }
                        else if (tmp == "2")
                        {
                            dr["Fsyn_statusName"] = "同步失败";
                        }
                        else if (tmp == "3")
                        {
                            dr["Fsyn_statusName"] = "同步成功";
                        }

                        tmp = dr["Frec_status"].ToString();

                        if (tmp == "0")
                        {
                            dr["Frec_statusName"] = "正常";
                        }
                        else if (tmp == "1")
                        {
                            dr["Frec_statusName"] = "作废";
                        }
                        else if (tmp == "3")
                        {
                            dr["Frec_statusName"] = "已删除";
                        }


                        tmp = dr["Fsyn_type"].ToString();

                        if (tmp == "0")
                        {
                            dr["Fsyn_typeName"] = "未定义";
                        }
                        else if (tmp == "1")
                        {
                            dr["Fsyn_typeName"] = "支付同步";
                        }
                        else if (tmp == "2")
                        {
                            dr["Fsyn_typeName"] = "确认收货同步";
                        }
                        else if (tmp == "3")
                        {
                            dr["Fsyn_typeName"] = "退款同步";
                        }

                        tmp = dr["Fpay_type"].ToString();

                        if (tmp == "1")
                        {
                            dr["Fpay_typeName"] = "C2C";
                        }
                        else if (tmp == "2")
                        {
                            dr["Fpay_typeName"] = "B2C";
                        }
                        else if (tmp == "3")
                        {
                            dr["Fpay_typeName"] = "未定义(充值)";
                        }
                        else if (tmp == "4")
                        {
                            dr["Fpay_typeName"] = "快速支付";
                        }
                        else if (tmp == "5")
                        {
                            dr["Fpay_typeName"] = "收款转账(我要收款)";
                        }
                        else if (tmp == "6")
                        {
                            dr["Fpay_typeName"] = "支付转账(按钮付款)";
                        }

                        tmp = dr["Fpay_channel"].ToString();

                        if (tmp == "0")
                        {
                            dr["Fpay_channelName"] = "未定义";
                        }
                        else if (tmp == "1")
                        {
                            dr["Fpay_channelName"] = "余额支付";
                        }
                        else if (tmp == "2")
                        {
                            dr["Fpay_channelName"] = "银行支付";
                        }


                        tmp = dr["Fver"].ToString();

                        if (tmp == "0")
                        {
                            dr["FverName"] = "未定义";
                        }
                        else if (tmp == "1")
                        {
                            dr["FverName"] = "版本1";
                        }
                        else if (tmp == "2")
                        {
                            dr["FverName"] = "版本2";
                        }

                        tmp = dr["Fsp_time"].ToString();
                        dr["Fsp_timeName"] = TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.GetTimeFromTick(tmp);

                        tmp = dr["Fbank_type"].ToString();
                        dr["Fbank_typeName"] = ""; //在客户端更好处理一些.

                        tmp = dr["Ftotal_fee"].ToString();
                        dr["Ftotal_feeName"] = MoneyTransfer.FenToYuan(tmp);

                        tmp = dr["Fprice"].ToString();
                        dr["FpriceName"] = MoneyTransfer.FenToYuan(tmp);

                        tmp = dr["Ftransport_fee"].ToString();
                        dr["Ftransport_feeName"] = MoneyTransfer.FenToYuan(tmp);

                        tmp = dr["Fprocedure_fee"].ToString();
                        dr["Fprocedure_feeName"] = MoneyTransfer.FenToYuan(tmp);

                        tmp = dr["Ffee_type"].ToString();
                        if (tmp == "1")
                        {
                            dr["Ffee_typeName"] = "RMB";
                        }
                        else if (tmp == "2")
                        {
                            dr["Ffee_typeName"] = "USD";
                        }
                        else if (tmp == "3")
                        {
                            dr["Ffee_typeName"] = "HKD";
                        }

                        tmp = dr["Ffee1"].ToString();
                        dr["Ffee1Name"] = MoneyTransfer.FenToYuan(tmp);

                        tmp = dr["Ffee2"].ToString();
                        dr["Ffee2Name"] = MoneyTransfer.FenToYuan(tmp);

                        tmp = dr["Ffee3"].ToString();
                        dr["Ffee3Name"] = MoneyTransfer.FenToYuan(tmp);

                        tmp = dr["Fvfee"].ToString();
                        dr["FvfeeName"] = MoneyTransfer.FenToYuan(tmp);

                        tmp = dr["Frp_fee"].ToString();
                        dr["Frp_feeName"] = MoneyTransfer.FenToYuan(tmp);

                        tmp = dr["Frb_fee"].ToString();
                        dr["Frb_feeName"] = MoneyTransfer.FenToYuan(tmp);

                        tmp = dr["Fcreat_time"].ToString();
                        dr["Fcreat_timeName"] = TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.GetTimeFromTick(tmp);

                        tmp = dr["Fpay_time"].ToString();
                        dr["Fpay_timeName"] = TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.GetTimeFromTick(tmp);

                        tmp = dr["Flast_syn_time"].ToString();
                        dr["Flast_syn_timeName"] = TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.GetTimeFromTick(tmp);

                        tmp = dr["Flast_modify_time"].ToString();
                        dr["Flast_modify_timeName"] = TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.GetTimeFromTick(tmp);

                        tmp = dr["Fsyn_result"].ToString();
                        if (tmp == "0")
                        {
                            dr["Fsyn_resultName"] = "同步成功";
                        }
                        else if (tmp == "1")
                        {
                            dr["Fsyn_resultName"] = "未定义";
                        }
                        else if (tmp == "2")
                        {
                            dr["Fsyn_resultName"] = "连接错误";
                        }
                        else if (tmp == "3")
                        {
                            dr["Fsyn_resultName"] = "apache访问错误";
                        }
                        else if (tmp == "4")
                        {
                            dr["Fsyn_resultName"] = "返回结果没有正确标示";
                        }
                        else if (tmp == "5")
                        {
                            dr["Fsyn_resultName"] = "url 解析格式错误";
                        }
                        else if (tmp == "6")
                        {
                            dr["Fsyn_resultName"] = "域名解释失败";
                        }
                    }


                    #endregion

                    return ds;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                throw new Exception("service发生错误,请联系管理员！");
                return null;
            }
        }

        [WebMethod(Description = "执行批量同步")]
        public bool BatchSynPayState(string transIds, int counts, out string msg)
        {
            msg = "";
            try
            {
                bool doAllSucc = true;
                if (transIds == null || transIds == "")
                {
                    msg = "传入的交易单号为空！";
                    return false;

                }
                string[] listIds = transIds.Split('|');
                if (listIds.Length != counts)
                {
                    msg = "传入的交易单列表" + transIds + "和统计出来的笔数不一致！";
                    return false;
                }

                foreach (string listid in listIds)
                {
                    if (listid == null || listid == "")
                        continue;
                    try
                    {
                        //time没有用处
                        string createtime = "";
                        string strMsg = "";
                        if (!SynRecordClass.SynPayState(listid, createtime, out strMsg))
                        {
                            log4net.ILog log = log4net.LogManager.GetLogger("BatchSynPayState");
                            if (log.IsInfoEnabled)
                            {
                                log.InfoFormat(string.Format("SynRecordClass.SynPayState返回false,具体原因={0}", strMsg));
                            }


                            msg += listid + "同步失败！";
                            doAllSucc = false;
                            continue;

                        }
                    }
                    catch (Exception ex)
                    {
                        msg += ex.Message;
                        doAllSucc = false;
                        continue;
                    }
                }

                return doAllSucc;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return false;
            }
        }

        #endregion


        #region 退单
        [WebMethod(Description = "B2C退款查询详细函数")]
        public DataSet GetB2cReturnDetail(string transid, string drawid)
        {
            try
            {
                if (transid == null || transid.Trim() == "")
                {
                    throw new Exception("交易单号不能为空！");
                    return null;
                }


                B2cReturnClass cuser = new B2cReturnClass(transid, drawid);
          
                string errMsg = "";
                DataSet ds = CommQuery.GetDataSetFromICE(cuser.ICESQL, CommQuery.QUERY_MCH_REFUND, out errMsg);
             
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null)
                {
                    DataTable dt = ds.Tables[0];
                    ds.Tables[0].Columns.Add("FexplainEx", typeof(string));
                    ds.Tables[0].Columns.Add("FHandleMemoEx", typeof(string));
                    //根据交易单号查看备注说明信息
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string strPaylistid = ds.Tables[0].Rows[0]["Ftransaction_id"].ToString();
                        MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("ZWTK"));
                        try
                        {
                            string strSql = " select FHandleMemo,Foldid from c2c_zwdb.t_refund_other where FPaylistid = '" + strPaylistid + "'";
                            da.OpenConn();
                            DataTable dt1 = da.GetTable(strSql);
                            if (dt1 != null && dt1.Rows.Count > 0)
                            {
                                //先得出说明信息
                                ds.Tables[0].Rows[0]["FHandleMemoEx"] = dt1.Rows[0]["FHandleMemo"];
                                string strOldID = dt1.Rows[0]["Foldid"].ToString();
                                //根据退款号求备注 （根据交易单号求备注会超时：交易单号不是索引）
                                string sql = " select Fexplain from c2c_zwdb.t_refund_total where foldid = '" + strOldID + "'";
                                DataTable dt2 = da.GetTable(sql);
                                if (dt2 != null && dt2.Rows.Count > 0)
                                {
                                    ds.Tables[0].Rows[0]["FexplainEx"] = dt2.Rows[0]["Fexplain"];
                                }
                            }
                        }
                        finally
                        {
                            da.Dispose();
                        }

                    }

                    #region 转化操作


                    TransferB2cReturnTable(dt);


                    #endregion
                    return ds;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                throw new Exception("service发生错误！" + e.Message);
                return null;
            }
        }


        /// <summary>
        /// 取出指定时间所有银行的付款情况
        /// </summary>
        /// <param name="WeekIndex">指定时间</param>
        /// <returns>返回数据集</returns>
        [WebMethod]
        public DataSet BatPay_InitGrid_R(string WeekIndex)
        {

            string strBeginDate = DateTime.Parse(WeekIndex).ToString("yyyyMMdd");

            string strSql = "select FBatchID,'0' FUrl,substring(FBatchID,1,8) FDate,FBankType,FPayCount,(FPaySum / 100) FPaySum1 ,FStatus,'0' FStatusName,'0' FMsg, '0' FBankID " +
                            " from c2c_zwdb.t_batchrefund_rec where FBatchID like '" + strBeginDate + "%'  order by FDate desc";

            DataSet ds = new DataSet();

            DataTable dt;
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("ZWTK"));
            try
            {
                da.OpenConn();
                dt = da.GetTable(strSql);
            }
            catch (Exception ex)
            {
                LogHelper.LogInfo("查询数据异常:" + ex.ToString());
                throw ex;
            }
            finally
            {
                da.Dispose();
            }

            ds.Tables.Add(dt);
            return ds;
        }

        /// <summary>
        /// 已处理付款的最后日期是否比指定日期早
        /// </summary>
        /// <param name="strDate">指定日期</param>
        /// <returns>是否比指定日期早</returns>
        [WebMethod]
        public bool BatPay_CanVisible_R(string strDate)
        {
            string strSql = "select Max(FBatchID) from c2c_zwdb.t_batchrefund_rec ";
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("ZWTK"));
            try
            {
                da.OpenConn();
                string strtmp = da.GetOneResult(strSql);
                if (strtmp == "")
                {
                    return true;
                }
                else
                {
                    return (strDate.CompareTo(strtmp.Substring(0, 8)) > 0);
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                da.Dispose();
            }
        }


        //银行选项
        [WebMethod]
        public ArrayList GetAllRefundBank()
        {
            try
            {
                //string allBankConfig = ConfigurationManager.AppSettings["RefundFileBankType"].Trim();
                string allBankConfig = ZWDicClass.GetZWDicValue("RefundFileBankType", PublicRes.GetConnString("ZW"));
                string[] banklist = allBankConfig.Split('|');

                ArrayList al = new ArrayList();
                al.Add("9999");

                foreach (string bank in banklist)
                {
                    if (bank.Trim().Length >= 4)
                        al.Add(bank.Substring(0, 4));
                }

                return al;
            }
            catch
            {
                return null;
            }
        }

        [WebMethod(Description = "退单查询函数")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet GetRefundList(string batchid, int ifromtype, int irefundtype, int irefundstate, int ireturnstate, string listid, string Fbank_listid, int iPageStart, int iPageMax)
        {
            RightAndLog rl = new RightAndLog();
            try
            {
                if (myHeader == null)
                {
                    throw new LogicException("不正确的调用方法！");
                }

                rl.actionType = "退单查询函数";
                rl.ID = batchid;
                rl.OperID = myHeader.OperID;
                rl.sign = 1;
                rl.strRightCode = "GetRefundList";
                rl.RightString = myHeader.RightString;
                rl.SzKey = myHeader.SzKey;
                rl.type = "查询";
                rl.UserID = myHeader.UserName;
                rl.UserIP = myHeader.UserIP;

                RefundQueryClass cuser = new RefundQueryClass(batchid, ifromtype, irefundtype, irefundstate, ireturnstate, listid, Fbank_listid);

                DataSet ds = cuser.GetResultX(iPageStart, iPageMax, "ZWTK");

                return ds;

            }
            catch (LogicException err)
            {
                rl.sign = 0;
                rl.ErrorMsg = err.Message;
                throw;
            }
            catch (Exception err)
            {
                rl.sign = 0;
                rl.ErrorMsg = err.Message;
                throw new LogicException("Service处理失败！");
            }
            finally
            {
                rl.WriteLog();
            }
        }

        [WebMethod(Description = "退单查询个数函数")]
        public int GetRefundListCount(string batchid, int ifromtype, int irefundtype, int irefundstate, int ireturnstate, string listid, string Fbank_listid)
        {
            try
            {
                RefundQueryClass cuser = new RefundQueryClass(batchid, ifromtype, irefundtype, irefundstate, ireturnstate, listid, Fbank_listid);
                return cuser.GetCount("ZWTK");
            }
            catch (Exception e)
            {
                throw new Exception("service发生错误,请联系管理员！");
                return 0;
            }
        }

        #endregion

        #region  手机绑定提醒


        [WebMethod(Description = "修改绑定信息")]
        public bool UpDateBindInfo(string QQ)
        {
            MySqlAccess da = null;
            try
            {
                string _fuid = "";
                _fuid = PublicRes.ConvertToFuid(QQ);

                da = new MySqlAccess(PublicRes.GetConnString("MN"));

                da.OpenConn();

                string strTable = "msgnotify_" + _fuid.Substring(_fuid.Length - 3, 2) + ".t_msgnotify_user_"
                    + _fuid.Substring(_fuid.Length - 1, 1);

                string sql = " select * from " + strTable + " where Fuid = '" + _fuid + "'";

                // 2012/5/2 添加用户绑定手机但未绑定到财付通帐号的情况下，需要更新到绑定到财付通帐号
                DataSet ds = da.dsGetTotalData(sql);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    return false;

                int fStatue = Convert.ToInt32(ds.Tables[0].Rows[0]["Fstatus"].ToString());

                // 0x00000004 0x00000008 0x00000010 0x00000100 都需置为为1
                fStatue = fStatue | 0x00000004;
                fStatue = fStatue | 0x00000008;
                fStatue = fStatue | 0x00000010;
                fStatue = fStatue | 0x00000100;

                string updateSql = "update " + strTable + " set Fqqid='" + QQ + "',Fstatus=" + fStatue
                    + " where Fuid='" + _fuid + "' ";

                return da.ExecSql(updateSql);
            }
            catch (Exception ex)
            {
                //string str = ex.Message;
                return false;
            }
            finally
            {
                if (da != null)
                {
                    da.Dispose();
                }
            }
        }


        [WebMethod(Description = "查询手机绑定信息")]
        public DataSet GetMsgNotify(string QQ)
        {
            MySqlAccess da = null;
            try
            {
                string Fuid = PublicRes.ConvertToFuid(QQ);
                if (Fuid == null)
                {
                    return null;
                }
                else
                {
                    da = new MySqlAccess(PublicRes.GetConnString("MN"));
                    da.OpenConn();
                    string strTable = "msgnotify_" + Fuid.Substring(Fuid.Length - 3, 2) + ".t_msgnotify_user_"
                        + Fuid.Substring(Fuid.Length - 1, 1);
                    string sql = " select * from " + strTable + " where Fuid = '" + Fuid + "'";
                    return da.dsGetTotalData(sql);
                }
            }
            catch
            {
                return null;
            }
            finally
            {
                if (da != null)
                {
                    da.Dispose();
                }
            }
        }

        [WebMethod(Description = "根据手机号查询手机绑定信息")]
        public string GetMsgNotifyByPhoneNumber(string phoneNumber)
        {
            MySqlAccess da = null;
            try
            {
                if (string.IsNullOrEmpty(phoneNumber))
                {
                    return null;
                }
                da = new MySqlAccess(PublicRes.GetConnString("MobileBind"));
                da.OpenConn();
                string sql = " select fuid from msgnotify.t_msgnotify_user where Fmobile = '" + phoneNumber + "'";
                var ds = da.dsGetTotalData(sql);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0].Rows[0]["Fqqid"].ToString();
                }
                return string.Empty;
            }
            catch
            {
                return string.Empty;

            }
            finally
            {
                if (da != null)
                {
                    da.Dispose();
                }
            }
        }

        [WebMethod(Description = "解除手机绑定信息")]
        public void UnbindMsgNotify(string Fuid)
        {
            MySqlAccess da = null;
            try
            {
                da = new MySqlAccess(PublicRes.GetConnString("MN"));

                da.OpenConn();

                string sql = " select Fstatus from msgnotify_" + Fuid.Substring(Fuid.Length - 3, 2) + ".t_msgnotify_user_" + Fuid.Substring(Fuid.Length - 1, 1) + " where Fuid = '" + Fuid + "'";

                string Fstatus = da.GetOneResult(sql);

                if (Fstatus == null || Fstatus == "")
                {
                    throw new Exception("不存在该记录");
                }
                else
                {
                    Fstatus = Convert.ToString(Convert.ToInt32(Fstatus), 2);

                    if (Fstatus.Length < 31)
                    {
                        Fstatus = Fstatus.PadLeft(31, '0');
                    }
                    if (Fstatus.Length != 31)
                    {
                        throw new Exception("记录状态数据异常");
                    }

                    try
                    {
                        if (Fstatus.Substring(30, 1).ToString() == "1")
                        {
                            Fstatus = Fstatus.Substring(0, 30) + "0";
                        }
                    }
                    catch { }
                    try
                    {
                        if (Fstatus.Substring(25, 1).ToString() == "1")
                        {
                            Fstatus = Fstatus.Substring(0, 25) + "0" + Fstatus.Substring(26, 5);
                        }
                    }
                    catch { }
                    try
                    {
                        if (Fstatus.Substring(24, 1).ToString() == "1")
                        {
                            Fstatus = Fstatus.Substring(0, 24) + "0" + Fstatus.Substring(25, 6);
                        }
                    }
                    catch { }

                    int NewFstatus = Convert.ToInt32(Fstatus, 2);

                    long timestamp = long.Parse(TimeTransfer.GetTickFromTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));

                    sql = "update msgnotify_" + Fuid.Substring(Fuid.Length - 3, 2) + ".t_msgnotify_user_" + Fuid.Substring(Fuid.Length - 1, 1) + " set Fstatus = " + NewFstatus +
                        ",Fupdatetime = " + timestamp + ",Fmobile=''" + " where Fuid = '" + Fuid + "'";

                    da.ExecSqlNum(sql);
                }
            }
            catch
            {
                throw new Exception("解绑失败!");
            }
            finally
            {
                if (da != null)
                {
                    da.Dispose();
                }
            }
        }


        [WebMethod(Description = "绑定手机或邮箱")]
        public bool BindMsgNotify(string Fqqid, bool IsMobile, string Mobile, bool IsMail, string Mail, string client_ip, string certno, out string BindMail, out string Msg)
        {
            /*转化为2进制(0为未开通,1为开通)不足7位前面补0,排序从最后一位开始
            1.是否开通短信提醒
            2.是否绑定email
            3.是否绑定qq
            4.是否激活
            5.动态验证玛(废弃)
            6.是否开通手机支付
            7.是否绑定手机
            消息通道开通标志
            0x00000001 //短信开通标志
            0x00000002 //email开通标志
            0x00000004 //tips开通标志
            0x00000008 //催费钱包消息开通标志
            0x00000010 //小钱包消息开通标志
            0x00000020 //wap支付开通标志，兼容老版本
            0x00000040 //手机绑定标志，兼容老版本
            0x00000080 //email 绑定标志
            0x00000100 //QQ绑定标志
            */
            Msg = "";
            BindMail = "";
            string Fuid = PublicRes.ConvertToFuid(Fqqid);

            if (Fuid == null || Fuid == "")
            {
                Msg = "该帐号的内部ID为空,绑定失败!";
                return false;
            }

            if (!IsMobile && !IsMail)
            {
                Msg = "选择清空密保时,需要绑定手机或邮箱,绑定失败!";
                return false;
            }

            if ((IsMobile && Mobile == "") || (IsMail && Mail == ""))
            {
                Msg = "选择清空密保时,绑定项的值不能为空,绑定失败!";
                return false;
            }

            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("MN"));

            try
            {
                da.OpenConn();
                string sql = " select * from msgnotify_" + Fuid.Substring(Fuid.Length - 3, 2) + ".t_msgnotify_user_" + Fuid.Substring(Fuid.Length - 1, 1) + " where Fuid = '" + Fuid + "'";
                DataSet ds = da.dsGetTotalData(sql);

                string strSql = "uid=" + Fuid;
                strSql += "&modify_time=" + PublicRes.strNowTimeStander;
                string Fstatus = "";
                int NewFstatus = 0;
                long timestamp = long.Parse(TimeTransfer.GetTickFromTime(PublicRes.strNowTimeStander));
                string old_mobile = "";
                int iresult;

                //绑定、更换手机发风控验证  echo 20140930
                //  Query_Service qs = new Query_Service();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    old_mobile = ds.Tables[0].Rows[0]["Fmobile"].ToString();
                if (!VerifyMobile(Fuid, Fqqid, old_mobile, Mobile, client_ip, certno))
                {
                    Msg = "发风控change_mobile_verify验证不通过";
                    return false;
                }



                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    if (IsMobile)
                    {
                        //Fstatus = "000000000000000000000000" + "1" + "000000";  //手机
                        Fstatus = "000000000000000000000000" + "1" + "000001";  //手机 第一位表示打开短信提醒
                        NewFstatus = Convert.ToInt32(Fstatus, 2);

                        sql = "insert into msgnotify_" + Fuid.Substring(Fuid.Length - 3, 2) + ".t_msgnotify_user_" + Fuid.Substring(Fuid.Length - 1, 1) + "(Fuid,Fmobile,Fstatus,Fregtime,Fupdatetime) " +
                            "values('" + Fuid + "','" + Mobile + "'," + NewFstatus + "," + timestamp + "," + timestamp + ")";

                        strSql += "&mobile=" + Mobile;
                    }
                    else  //只走邮箱
                    {
                        //Fstatus = "00000000000000000000000000000" + "1" + "0";  //邮箱
                        Fstatus = "00000000000000000000000100000" + "1" + "0";  //邮箱  第2位表示是否打开邮件通知状态,第8位邮箱绑定状态。

                        NewFstatus = Convert.ToInt32(Fstatus, 2);

                        sql = "insert into msgnotify_" + Fuid.Substring(Fuid.Length - 3, 2) + ".t_msgnotify_user_" + Fuid.Substring(Fuid.Length - 1, 1) + "(Fuid,Femail,Fstatus,Fregtime,Fupdatetime) " +
                            "values('" + Fuid + "','" + Mail + "'," + NewFstatus + "," + timestamp + "," + timestamp + ")";

                        strSql += "&email=" + Mail;
                    }

                    da.ExecSqlNum(sql);

                    iresult = CommQuery.ExecSqlFromICE(strSql, CommQuery.UPDATE_USERINFO, out Msg);
                    if (iresult != 1)
                    {
                        Msg = "手机或邮箱绑定失败,基本信息表更新了非一条记录:" + Msg;
                        return false;
                    }
                }
                else
                {
                    Fstatus = Convert.ToString(Convert.ToInt32(ds.Tables[0].Rows[0]["Fstatus"].ToString()), 2);
                    //  old_mobile = ds.Tables[0].Rows[0]["Fmobile"].ToString();

                    if (Fstatus.Length < 31)
                    {
                        Fstatus = Fstatus.PadLeft(31, '0');
                    }
                    if (Fstatus.Length != 31)
                    {
                        Msg = "清空密保时,记录状态数据异常,绑定失败!";
                        return false;
                    }

                    if (IsMobile)
                    {
                        //Fstatus = Fstatus.Substring(0,24) + "1" + Fstatus.Substring(25,6);  //手机
                        Fstatus = Fstatus.Substring(0, 24) + "1" + Fstatus.Substring(25, 5) + "1";  //手机
                        NewFstatus = Convert.ToInt32(Fstatus, 2);

                        sql = "update msgnotify_" + Fuid.Substring(Fuid.Length - 3, 2) + ".t_msgnotify_user_" + Fuid.Substring(Fuid.Length - 1, 1) + " set Fstatus = " + NewFstatus +
                            ",Fmobile = '" + Mobile + "',Fupdatetime = " + timestamp + " where Fuid = '" + Fuid + "'";

                        strSql += "&mobile=" + Mobile;
                    }
                    else  //只走邮箱
                    {
                        //Fstatus = Fstatus.Substring(0,29) + "1" + Fstatus.Substring(30,1);
                        Fstatus = Fstatus.Substring(0, 23) + "1" + Fstatus.Substring(24, 5) + "1" + Fstatus.Substring(30, 1);//第2位表示是否打开邮件通知状态,第8位邮箱绑定状态。
                        NewFstatus = Convert.ToInt32(Fstatus, 2);

                        sql = "update msgnotify_" + Fuid.Substring(Fuid.Length - 3, 2) + ".t_msgnotify_user_" + Fuid.Substring(Fuid.Length - 1, 1) + " set Fstatus = " + NewFstatus +
                            ",Femail = '" + Mail + "',Fupdatetime = " + timestamp + " where Fuid = '" + Fuid + "'";

                        strSql += "&email=" + Mail;
                    }

                    da.ExecSqlNum(sql);

                    iresult = CommQuery.ExecSqlFromICE(strSql, CommQuery.UPDATE_USERINFO, out Msg);
                    if (iresult != 1)
                    {
                        Msg = "手机或邮箱绑定失败,基本信息表更新了非一条记录:" + Msg;
                        return false;
                    }
                }

                if (IsMail)
                {
                    BindMail = Mail;
                }
                else if (IsMobile)
                {
                    if (Fqqid.IndexOf("@") > -1)
                        BindMail = Fqqid;
                    else if (ds.Tables[0].Rows.Count > 0)
                        BindMail = ds.Tables[0].Rows[0]["Femail"].ToString();

                    try
                    {
                        SendMobile(Fuid, Fqqid, old_mobile, Mobile, client_ip, certno);
                    }
                    catch { }
                }
                return true;
            }
            catch (Exception ex)
            {
                Msg = "手机或邮箱绑定失败:" + ex.Message;
                return false;
            }
            finally
            {
                da.Dispose();
            }
        }

        //绑定或更换手机前，发风控验证
        public bool VerifyMobile(string uid, string uin, string old_mobile, string new_mobile, string client_ip, string certno)
        {
            try
            {
                //  string hdId = CommUtil.GetHardDiskId();取不到用户的所以传空
                //  string mac = CommUtil.GetNetworkMAC();
                string Data = "purchaser_uid=" + uid + "&purchaser_id=" + uin + "&old_mobile=" + old_mobile
                    + "&new_mobile=" + new_mobile + "&client_ip=" + client_ip +
                    "&cookie=&change_way=2&diskid=&macaddr=&certno=" + certno + "&crt_state=0";
                Data = HttpUtility.UrlEncode(Data, System.Text.Encoding.GetEncoding("GB2312"));
                Data = "protocol=change_mobile_verify&version=1.0&data=" + Data;

                SunLibrary.LoggerFactory.Get("KF_Service VerifyMobile").Info("change_mobile_verify send:" + Data);

                System.Text.Encoding GB2312 = System.Text.Encoding.GetEncoding("GB2312");
                byte[] sendBytes = GB2312.GetBytes(Data);

                string IP = ConfigurationManager.AppSettings["FK_UDP_IP"].ToString();
                string PORT = ConfigurationManager.AppSettings["FK_UDP_PORT"].ToString();
                byte[] answer = UDP.GetUDPReply(sendBytes, IP, Int32.Parse(PORT));
                string answerStr = Encoding.Default.GetString(answer);//  result=0  验证通过。允许修改

                SunLibrary.LoggerFactory.Get("KF_Service VerifyMobile").Info("change_mobile_verify get:" + answerStr);

                if (answerStr.IndexOf("result=0") < 0)
                {
                    return false;
                }
                return true;
            }
            catch
            {
                return false;
            }

        }

        //绑定或更换手机后，发风控通知
        public void SendMobile(string uid, string uin, string old_mobile, string new_mobile, string client_ip, string certno)
        {
            try
            {
                // string hdId = CommUtil.GetHardDiskId();
                //  string mac = CommUtil.GetNetworkMAC();
                string Data = "purchaser_uid=" + uid + "&purchaser_id=" + uin + "&old_mobile=" + old_mobile
                    + "&new_mobile=" + new_mobile + "&client_ip=" + client_ip
                    + "&cookie=&change_way=2&diskid=&macaddr=&certno=" + certno;
                Data = HttpUtility.UrlEncode(Data, System.Text.Encoding.GetEncoding("GB2312"));
                Data = "protocol=change_mobile_notify&version=1.0&data=" + Data;

                SunLibrary.LoggerFactory.Get("KF_Service SendMobile").Info("change_mobile_notify send:" + Data);

                System.Text.Encoding GB2312 = System.Text.Encoding.GetEncoding("GB2312");
                byte[] sendBytes = GB2312.GetBytes(Data);

                string IP = ConfigurationManager.AppSettings["FK_UDP_IP"].ToString();
                string PORT = ConfigurationManager.AppSettings["FK_UDP_PORT"].ToString();
                TENCENT.OSS.CFT.KF.Common.UDP.GetUDPReplyNoReturn(sendBytes, IP, Int32.Parse(PORT));
            }
            catch
            {

            }

        }

        [WebMethod(Description = "获取旧绑定手机")]
        public string GetOldBindMobile(string Fuid, out string Msg)
        {
            string old_mobile = "";
            Msg = "";
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("MN"));

            try
            {
                da.OpenConn();
                string sql = " select * from msgnotify_" + Fuid.Substring(Fuid.Length - 3, 2) + ".t_msgnotify_user_" + Fuid.Substring(Fuid.Length - 1, 1) + " where Fuid = '" + Fuid + "'";
                DataSet ds = da.dsGetTotalData(sql);

                if (ds != null || ds.Tables.Count > 0 || ds.Tables[0].Rows.Count > 0)
                    old_mobile = ds.Tables[0].Rows[0]["Fmobile"].ToString();
                return old_mobile;
            }
            catch (Exception ex)
            {
                Msg = "获取旧绑定手机失败:" + ex.Message;
                return "";
            }
            finally
            {
                da.Dispose();
            }
        }

        private bool IsBindMobilePhone(string Fuid)
        {
            bool bState = false;
            using (var da = new MySqlAccess(PublicRes.GetConnString("MN")))//MySQLAccessFactory.GetMySQLAccess("MN"))
            {

                da.OpenConn();
                string strTable = "msgnotify_" + Fuid.Substring(Fuid.Length - 3, 2) + ".t_msgnotify_user_"
                    + Fuid.Substring(Fuid.Length - 1, 1);
                string sql = " select Fstatus from " + strTable + " where Fuid = '" + Fuid + "'";
                DataSet ds = da.dsGetTotalData(sql);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    int nState = int.Parse(ds.Tables[0].Rows[0]["Fstatus"].ToString());
                    int nBit = 64;
                    if ((nState & nBit) == nBit)
                    {
                        bState = true;
                    }

                }
            }
            return bState;

        }

        /// <summary>
        ///  发风控验证、绑定或更换手机、发风控通知
        /// </summary>
        public bool BindOrChangeMobile(string Fuid, string fuin, string old_mobile, string mobile_no, string client_ip, string certno, string singed, out string msg)
        {
            msg = "BindOrChangeMobile...";
            if (old_mobile == "")
            {
                //已绑定,就不需要再去绑定了
                if (IsBindMobilePhone(Fuid))
                {
                    string strMsg = string.Format("QQ号={0}手机号已经绑定了", fuin);
                    SunLibrary.LoggerFactory.Get("KF_Service").Info(strMsg);
                    return true;
                }
            }
            else
            {
                //更改手机相同时
                if (old_mobile.Trim() == mobile_no.Trim())
                {
                    string strMsg = string.Format("QQ号={0}，更改手机号码相同", fuin);
                    SunLibrary.LoggerFactory.Get("KF_Service").Info(strMsg);
                    return true;
                }

            }
            // 以下三步走
            //发验证
            Query_Service qs = new Query_Service();

            if (!qs.VerifyMobile(Fuid, fuin, old_mobile, mobile_no, client_ip, certno))
            {
                msg = "发风控change_mobile_verify验证不通过";
                return false;
            }

            if (old_mobile == "")
            {
                //绑定手机
                if (!TENCENT.OSS.C2C.Finance.Common.CommLib.CommMailSend.BindMobile(int.Parse(Fuid), mobile_no, singed, out msg))
                    return false;
            }
            else
            {
                //更新手机
                if (!TENCENT.OSS.C2C.Finance.Common.CommLib.CommMailSend.ChangeMobile(int.Parse(Fuid), mobile_no, singed, out msg))
                    return false;
            }

            //更新或绑定手机后，发风控通知
            try
            {
                qs.SendMobile(Fuid, fuin, old_mobile, mobile_no, client_ip, certno);
            }
            catch { }
            return true;
        }


        #endregion

        #region T+0付款查询
        /// <summary>
        /// 取出指定时间所有银行的付款情况
        /// </summary>
        /// <param name="WeekIndex">指定时间</param>
        /// <returns>返回数据集</returns>
        [WebMethod]
        public DataSet BatPay_InitGrid_B(string WeekIndex, string BatchOrder)
        {

            BatchOrder = Common.T0Transfer.Order2Asc(Int32.Parse(BatchOrder));

            string strBeginDate = DateTime.Parse(WeekIndex).ToString("yyyyMMdd");

            string strSql = "select FBatchID,'0' FUrl,substring(FBatchID,1,8) FDate,FBankType,FPayCount,(FPaySum / 100) FPaySum1 ,FStatus,'0' FStatusName,'0' FMsg, '0' FBankID "
                + " from c2c_zwdb.t_batchpay_rec where FBatchID like '" + strBeginDate + "____B" + BatchOrder + "'  order by FDate desc";

            DataSet ds = new DataSet();

            DataTable dt;
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("ZWFK"));
            try
            {
                da.OpenConn();
                dt = da.GetTable(strSql);
            }
            finally
            {
                da.Dispose();
            }

            ds.Tables.Add(dt);
            return ds;
        }

        /// <summary>
        /// 已处理付款的最后日期是否比指定日期早
        /// </summary>
        /// <param name="strDate">指定日期</param>
        /// <returns>是否比指定日期早</returns>
        [WebMethod]
        public bool BatPay_CanVisible_B(string strDate, string batchorder)
        {
            batchorder = Common.T0Transfer.Order2Asc(Int32.Parse(batchorder));

            string strSql = "select Max(FBatchID) from c2c_zwdb.t_batchpay_rec where length(Fbatchid)=14 ";
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("ZWFK"));
            try
            {
                da.OpenConn();
                string strtmp = da.GetOneResult(strSql);
                if (strtmp == "")
                {
                    return true;
                }
                else
                {
                    if (strtmp.Substring(0, 8).CompareTo(strDate) > 0)
                    {
                        return false;
                    }
                    else if (strtmp.Substring(0, 8) == strDate)
                    {
                        strtmp = strtmp.Substring(13, 1);
                        //为啥9的asc码比:大，却compareto时却小于它？

                        int imaxorder = Common.T0Transfer.Asc2Order(strtmp);
                        int inoworder = Common.T0Transfer.Asc2Order(batchorder);
                        return imaxorder < inoworder;
                    }
                    else
                        return true;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                da.Dispose();
            }
        }


        /// <summary>
        /// 显示指定批次号的付款明细数据
        /// </summary>
        /// <param name="max">最大记录条数</param>
        /// <param name="start">开始记录数</param>
        /// <param name="BatchID">指定批次号</param>
        /// <returns>返回数据</returns>
        [WebMethod]
        public DataSet ShowDetail_BindData(int max, int start, string BatchID, int state, string username, string bankacc, string count, string paybank)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("ZWFK"));
            MySqlAccess da_yw = new MySqlAccess(PublicRes.GetConnString("YWB"));
            MySqlAccess da_zl = new MySqlAccess(PublicRes.GetConnString("ZL"));
            try
            {
                string strSql = "SELECT a1.FSequence,a1.FTruename,a1.FBankAccNo,a1.FBankName,a1.Fstatus, '0' as FStatusName,a1.FPayBankType,"
                    + "(a1.Famt/100)Famt1,a2.FID FSequence1,a2.Ftde_id,a2.Fuid,(a2.Fnum/100)Fnum1, a2.FImportFlag, '0' as FStatusName1,a1.FRTFlag  FROM c2c_zwdb_"
                    + BatchID.Substring(0, 8) + ".t_payfund_total as a1 ,c2c_zwdb_"
                    + BatchID.Substring(0, 8) + ".t_pay_relationship as a2  where a2.FSequence=a1.FSequence and FbatchID='" + BatchID + "' ";

                if (BatchID.Length == 14)
                {
                    strSql = "SELECT a1.FSequence,a1.FTruename,a1.FBankAccNo,a1.FBankName,a1.Fstatus, '0' as FStatusName,a1.FPayBankType,"
                        + "(a1.Famt/100)Famt1,a2.FID FSequence1,a2.Ftde_id,a2.Fuid,(a2.Fnum/100)Fnum1, a2.FImportFlag, '0' as FStatusName1,a1.FRTFlag  FROM c2c_zwdb_"
                        + BatchID.Substring(0, 8) + "B.t_payfund_total as a1 ,c2c_zwdb_"
                        + BatchID.Substring(0, 8) + "B.t_pay_relationship as a2  where a2.FSequence=a1.FSequence and FbatchID='" + BatchID + "' ";
                }

                if (state != 9)
                {
                    if (state < 5)
                    {
                        strSql += " and a1.FStatus=" + state + " ";
                    }
                    else if (state == 5)
                    {
                        strSql += " and a1.FStatus=2 and ifnull(a1.FRTFlag,0)=0 ";
                    }
                    else if (state == 6)
                    {
                        strSql += " and a1.FStatus=2 and a1.FRTFlag=1 ";
                    }
                    else if (state == 7)
                    {
                        strSql += " and a1.FStatus=2 and a1.FRTFlag=2 ";
                    }
                }

                if (username != null && username != "")
                {
                    strSql += " and a1.FTrueName='" + username + "' ";
                }

                if (bankacc != null && bankacc != "")
                {
                    strSql += " and a1.FBankAccNo='" + bankacc + "' ";
                }

                if (count != null && count != "" && count != "0")
                {
                    strSql += " and a1.Famt=" + count + " ";
                }

                if (paybank != null && paybank != "0000" && paybank != "")
                {
                    strSql += "and a1.FPayBankType=" + paybank + " ";
                }

                strSql += " order  by a1.Famt desc ,FSequence asc,Fnum1 desc";
                da.OpenConn();
                DataSet ds = da.dsGetTableByRange(strSql, start, max);

                DataTable dt;
                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                }
                else
                {
                    dt = null;
                }

                da.CloseConn();
                da_yw.OpenConn();
                da_zl.OpenConn();

                //对dt处理一下 furion 20050830
                if (dt != null && dt.Rows.Count > 0)
                {
                    string strIndex = "";

                    dt.Columns.Add("FQQID", typeof(String));
                    dt.Columns.Add("FRTFlagName", typeof(String));
                    dt.Columns.Add("FPayBankTypeName", typeof(String));

                    foreach (DataRow dr in dt.Rows)
                    {
                        dr.BeginEdit();

                        if (dr["Fstatus"].ToString() == "0")
                        {
                            dr["FStatusName"] = "已生成";
                        }
                        else if (dr["Fstatus"].ToString() == "1")
                        {
                            dr["FStatusName"] = "已提交付款";
                        }
                        else if (dr["Fstatus"].ToString() == "2")
                        {
                            dr["FStatusName"] = "付款成功";
                        }
                        else if (dr["Fstatus"].ToString() == "3")
                        {
                            dr["FStatusName"] = "<font Color=red>付款失败</font>";
                        }

                        if (dr["FImportFlag"].ToString() == "0")
                        {
                            dr["FStatusName1"] = "初始状态";
                        }
                        else if (dr["FImportFlag"].ToString() == "1")
                        {
                            dr["FStatusName1"] = "回导成功";
                        }
                        else if (dr["FImportFlag"].ToString() == "2")
                        {
                            dr["FStatusName1"] = "<font Color=red>回导失败</font>";
                        }
                        else if (dr["FImportFlag"].ToString() == "3")
                        {
                            dr["FStatusName1"] = "已标记";
                        }

                        if (dr["FRTFlag"] == null || dr["FRTFlag"].ToString() == "" || dr["FRTFlag"].ToString() == "0")
                        {
                            dr["FRTFlagName"] = "";
                        }
                        else if (dr["FRTFlag"].ToString() == "1")
                        {
                            dr["FRTFlagName"] = "退票申请中";
                        }
                        else if (dr["FRTFlag"].ToString() == "2")
                        {
                            dr["FRTFlagName"] = "已退票";
                        }

                        if (dr["FPayBankType"] != null)
                        {
                            dr["FPayBankTypeName"] = GetBankName(dr["FPayBankType"].ToString().Trim());
                        }

                        if (dr["FSequence"] != null)
                        {

                            if (strIndex != dr["FSequence"].ToString().Trim())
                            {
                                strIndex = dr["FSequence"].ToString().Trim();
                            }
                            else
                            {
                                dr["FSequence"] = DBNull.Value;
                                dr["FTruename"] = DBNull.Value;
                                dr["FBankAccNo"] = DBNull.Value;
                                dr["FBankName"] = DBNull.Value;
                                dr["FStatusName"] = DBNull.Value;
                                dr["Famt1"] = DBNull.Value;
                            }
                        }

                        string uid = dr["fuid"].ToString();
                        if (uid != null && uid.Trim() != "")
                        {                        
                            string qqid = ConvertUid2QQ(uid);

                            dr["FQQID"] = qqid;

                        }
                        dr.EndEdit();
                    }
                }
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                da.Dispose();
                da_yw.Dispose();
                da_zl.Dispose();
            }
        }

        /// <summary>
        /// 取得银行名称
        /// </summary>
        /// <param name="banktype">银行类型代码</param>
        /// <returns>银行名称</returns>
        private string GetBankName(string banktype)
        {
            switch (banktype)
            {
                #region 。
                case "1001":
                    return "招商银行";

                case "1002":
                    return "工商银行";

                case "1003":
                    return "建设银行";

                case "1004":
                    return "浦东发展银行";

                case "1005":
                    return "农业银行";

                case "1006":
                    return "民生银行";

                case "1007":
                    return "农行国际卡";

                case "1008":
                    return "深圳发展银行";

                case "1009":
                    return "兴业银行";

                case "1010":
                    return "深圳平安银行";

                case "1011":
                    return "中国邮政储蓄银行";

                case "1020":
                    return "交通银行";

                case "1021":
                    return "中信实业银行";

                case "1022":
                    return "光大银行";

                case "1023":
                    return "农村合作信用社";

                case "1024":
                    return "上海银行";

                case "1025":
                    return "华夏银行";

                case "1026":
                    return "中国银行";
                case "1052":
                    return "中行小额";
                case "1053":
                    return "中行大额";

                case "1027":
                    return "广东发展银行";

                case "1028":
                    return "广州银联";

                case "1099":
                    return "其他银行";

                //furion 20060925 start
                case "1030":
                    return "工行B2B";
                case "1031":
                    return "招行大额";
                case "1032":
                    return "北京银行";
                case "1033":
                    return "网汇通";
                case "1034":
                    return "建行大额";
                case "1035":
                    return "建行非实名";
                case "1036":
                    return "工行外卡";
                case "1037":
                    return "工行大额";
                case "1038":
                    return "招行基础业务";

                case "1039":
                    return "工行直付";

                // START wandy 20080622	
                case "1040":
                    return "建行B2B";
                case "1041":
                    return "民生借记卡";
                case "1042":
                    return "招行B2B";
                case "1050":
                    return "工行信用卡";
                case "1051":
                    return "广发借记卡";
                // END wandy 20080622


                case "2001":
                    return "招行绑定支付";
                case "2002":
                    return "工行绑定支付";
                case "3001":
                    return "兴业信用卡";
                case "3002":
                    return "中行信用卡";

                #endregion

                case "2033":
                    return "邮储一点通";

                case "2003":
                    return "建行E付通";


                case "3003":
                    return "工行EPOS";

                case "9999":
                    return "汇总银行";

                case "0000":
                    return "所有银行";
                default:
                    return "";
            }
        }
        #endregion

        #region  中介订单&仲裁
        [WebMethod(Description = "获取所有中介订单数据")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet Gett_appealList(string begin_appeal_time, string end_appeal_time, string begin_modify_time, string end_modify_time, string appealid,
            string listid, string qqid, string vs_qqid, string appeal_type, string check_state, string Fstate, string fund_flag, string response_flag, int start, int count, out string errMsg)
        {
            string strSql = "";

            // 投诉单生成时间开始点(默认查询过去一年的数据)
            if (begin_appeal_time != "")
            {
                strSql = "begin_appeal_time=" + begin_appeal_time;
            }
            else
            {
                strSql = "begin_appeal_time=" + DateTime.Today.AddYears(-1).ToString("yyyy-MM-dd");
            }

            // 投诉单生成时间结束点
            if (end_appeal_time != "")
            {
                strSql += "&end_appeal_time=" + end_appeal_time;
            }
            else
            {
                strSql += "&end_appeal_time=" + DateTime.Today.AddDays(1).ToString("yyyy-MM-dd");
            }

            // 投诉更新时间开始点
            if (begin_modify_time != "")
            {
                strSql += "&begin_modify_time=" + begin_modify_time;
            }
            else
            {
                strSql += "&begin_modify_time=" + DateTime.Today.AddYears(-1).ToString("yyyy-MM-dd");
            }

            // 投诉更新时间结束点
            if (end_modify_time != "")
            {
                strSql += "&end_modify_time=" + end_modify_time;
            }
            else
            {
                strSql += "&end_modify_time=" + DateTime.Today.AddDays(1).ToString("yyyy-MM-dd");
            }

            //投诉单号
            if (appealid != "")
            {
                strSql += "&appealid=" + appealid;
            }

            // 交易单号
            if (listid != "")
            {
                strSql += "&listid=" + listid;
            }

            // 投诉方QQ
            if (qqid != "")
            {
                strSql += "&qqid=" + qqid;
            }

            // 被投诉方QQ
            if (vs_qqid != "")
            {
                strSql += "&vs_qqid=" + vs_qqid;
            }

            // 投诉类型
            if (appeal_type != "")
            {
                strSql += "&appeal_type=" + appeal_type;
            }

            // 审核状态
            if (check_state != "")
            {
                strSql += "&check_state=" + check_state;
            }

            // 投诉状态
            if (Fstate != "")
            {
                strSql += "&Fstate=" + Fstate;
            }

            // 退款标志
            if (fund_flag != "")
            {
                strSql += "&fund_flag=" + fund_flag;
            }

            // 申诉标志
            if (response_flag != "")
            {
                strSql += "&response_flag=" + response_flag;
            }

            strSql += "&lim_start=" + start;
            strSql += "&lim_count=" + count;
            // 组装消息
            return CommQuery.GetDataSetFromICE(strSql, CommQuery.QUERY_APPEAL_NEW, out errMsg);
        }


        [WebMethod(Description = "获取中介订单投诉留言")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet Gett_message(string listid, int start, int count, out string errMsg)
        {
            string strSql = "listid=" + listid;
            strSql += "&systype=2";
            strSql += "&lim_start=" + start;
            strSql += "&lim_count=" + count;
            // 组装消息
            return CommQuery.GetDataSetFromICE(strSql, CommQuery.QUERY_MESSAGE, out errMsg);
        }


        [WebMethod(Description = "中介订单投诉操作")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public bool Savetp_add_msg(string list_id, string type, string UserName, string UserID, string message, string memo, out string msg)
        {
            string inmsg = "list_id=" + list_id;
            inmsg += "&systype=2";
            inmsg += "&type=" + type;
            inmsg += "&owner_uid=" + UserID;
            inmsg += "&owner_id=" + UserName;
            inmsg += "&message=" + HttpUtility.UrlEncode(message, System.Text.Encoding.GetEncoding("GB2312"));
            inmsg += "&memo=" + HttpUtility.UrlEncode(memo, System.Text.Encoding.GetEncoding("GB2312"));
            string reply;
            short sresult;

            if (TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.middleInvoke("tp_add_msg", inmsg, true, out reply, out sresult, out msg))
            {
                if (sresult != 0)
                {
                    msg = "tp_add_msg接口失败：result=" + sresult + "，msg=" + msg + "，reply=" + reply;
                    return false;
                }
                else
                {
                    if (reply.IndexOf("result=0") > -1)   //&0=298751028&res_info=ok&result=0 正常值
                    {
                        return true;
                    }
                    else
                    {
                        msg = "tp_add_msg接口失败：result=" + sresult + "，msg=" + msg + "，reply=" + reply;
                        return false;
                    }
                }
            }
            else
            {
                msg = "ra_apeal_service接口失败：result=" + sresult + "，msg=" + msg + "，reply=" + reply;
                return false;
            }
        }


        [WebMethod(Description = "中介订单提交审核")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public bool SubmitApprove(string list_id, string transaction_id, int Ffund_flag, string iAppealRole, Int64 AppealFee, Int64 AppealedFee, string UserName, out string msg)
        {
            string inmsg = "";
            if (Ffund_flag == 2) //需要退款
            {
                inmsg += "&fund_flag=2";
                if (iAppealRole == "1")
                {
                    inmsg += "&paybuy=" + AppealFee;
                    inmsg += "&paysale=" + AppealedFee;
                }
                else
                {
                    inmsg += "&paybuy=" + AppealedFee;
                    inmsg += "&paysale=" + AppealFee;
                }
            }
            else
            {
                inmsg += "&fund_flag=1";
            }
            inmsg += "&punish_flag=1";
            inmsg += "&check_user1=" + UserName;
            inmsg += "&check_time1=" + DateTime.Now;
            inmsg += "&speak_flag=00010";
            inmsg += "&modify_time=" + DateTime.Now;
            inmsg += "&appealid=" + list_id;
            inmsg += "&transaction_id=" + transaction_id;
            inmsg += "&MSG_NO=" + list_id;
            string reply;
            short sresult;

            if (TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.middleInvoke("order_appeal_submit_service", inmsg, false, out reply, out sresult, out msg))
            {
                if (sresult != 0)
                {
                    msg = "order_appeal_submit_service接口失败：result=" + sresult + "，msg=" + msg + "，reply=" + reply;
                    return false;
                }
                else
                {
                    if (reply.IndexOf("result=0") > -1)   //&0=298751028&res_info=ok&result=0 正常值
                    {
                        return true;
                    }
                    else
                    {
                        msg = "order_appeal_submit_service接口失败：result=" + sresult + "，msg=" + msg + "，reply=" + reply;
                        return false;
                    }
                }
            }
            else
            {
                msg = "order_appeal_submit_service接口失败：result=" + sresult + "，msg=" + msg + "，reply=" + reply;
                return false;
            }
        }


        [WebMethod(Description = "中介订单审核通过")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public bool ApprovePass(string list_id, string transaction_id, string UserName, out string msg)
        {
            string inmsg = "";
            inmsg += "&check_user2=" + UserName;
            inmsg += "&check_time2=" + DateTime.Now;
            inmsg += "&speak_flag=00000";
            inmsg += "&modify_time=" + DateTime.Now;
            inmsg += "&appealid=" + list_id;
            inmsg += "&transaction_id=" + transaction_id;
            inmsg += "&MSG_NO=" + list_id;
            string reply;
            short sresult;

            if (TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.middleInvoke("order_appeal_submitok_service", inmsg, false, out reply, out sresult, out msg))
            {
                if (sresult != 0)
                {
                    msg = "order_appeal_submitok_service接口失败：result=" + sresult + "，msg=" + msg + "，reply=" + reply;
                    return false;
                }
                else
                {
                    if (reply.IndexOf("result=0") > -1)   //&0=298751028&res_info=ok&result=0 正常值
                    {
                        return true;
                    }
                    else
                    {
                        msg = "order_appeal_submitok_service接口失败：result=" + sresult + "，msg=" + msg + "，reply=" + reply;
                        return false;
                    }
                }
            }
            else
            {
                msg = "order_appeal_submitok_service接口失败：result=" + sresult + "，msg=" + msg + "，reply=" + reply;
                return false;
            }
        }


        [WebMethod(Description = "中介订单审核不申诉")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public bool ApproveCancel(string list_id, string transaction_id, string UserName, out string msg)
        {
            string inmsg = "";
            inmsg += "&check_user2=" + UserName;
            inmsg += "&check_time2=" + DateTime.Now;
            inmsg += "&speak_flag=00100";
            inmsg += "&modify_time=" + DateTime.Now;
            inmsg += "&appealid=" + list_id;
            inmsg += "&transaction_id=" + transaction_id;
            inmsg += "&MSG_NO=" + list_id;
            string reply;
            short sresult;

            if (TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.middleInvoke("order_appeal_submitfail_service", inmsg, false, out reply, out sresult, out msg))
            {
                if (sresult != 0)
                {
                    msg = "order_appeal_submitfail_service接口失败：result=" + sresult + "，msg=" + msg + "，reply=" + reply;
                    return false;
                }
                else
                {
                    if (reply.IndexOf("result=0") > -1)   //&0=298751028&res_info=ok&result=0 正常值
                    {
                        return true;
                    }
                    else
                    {
                        msg = "order_appeal_submitfail_service接口失败：result=" + sresult + "，msg=" + msg + "，reply=" + reply;
                        return false;
                    }
                }
            }
            else
            {
                msg = "order_appeal_submitfail_service接口失败：result=" + sresult + "，msg=" + msg + "，reply=" + reply;
                return false;
            }
        }

        #endregion

        #region 商户号身份证修改与资质上传
        [WebMethod]  //只走SPOA的资质和结算审核流程
        public void BusinessIdentityCardNum(string Fspid, string OldIdentityCardNum, string NewIdentityCardNum, string IDImage, string ElseImage, string UserName, string Reason)
        {
            try
            {              
                new SPOAService().BusinessIdentityCardNum(Fspid, OldIdentityCardNum, NewIdentityCardNum, IDImage, ElseImage, UserName, Reason);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion


        [WebMethod(Description = "获取所有渠道列表函数")]
        public DataSet GetAllChannelList()
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("GWQ"));
            try
            {
                da.OpenConn();
                return da.dsGetTotalData("select * from c2c_db_au.t_direct_relation");
            }
            catch (Exception err)
            {
                return null;
            }
            finally
            {
                da.Dispose();
            }
        }

        [WebMethod(Description = "获取所有付款限制列表函数")]
        public DataSet GetPayLimitList(string qqid, int channelid, string aqqid)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("GWQ"));
            try
            {
                da.OpenConn();

                if (qqid == null || qqid.Trim() == "")
                    return null;

                string tablename = PublicRes.GetTableName("t_pay_limit", qqid);
                if (tablename == null || tablename.Trim() == "")
                    return null;

                string strSql = "select A.*,B.Fchannel_name from  " + tablename + " A,c2c_db_au.t_direct_relation B where A.Fqqid='" + qqid + "' "
                    + " and A.Fchannel_id=B.Fchannel_id ";

                if (aqqid != null && aqqid.Trim() != "")
                {
                    strSql += " and A.Faqqid='" + aqqid + "' ";
                }

                if (channelid != 0)
                {
                    strSql += " and A.Fchannel_id=" + channelid;
                }

                return da.dsGetTotalData(strSql);
            }
            catch (Exception err)
            {
                return null;
            }
            finally
            {
                da.Dispose();
            }
        }

        [WebMethod(Description = "获取委托退款函数")]
        public DataSet GetTrustLimitList(string qqid, string Fspid)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("AP"));

            try
            {
                string uid = PublicRes.ConvertToFuid(qqid);

                if (uid == null || uid.Length < 3)
                {
                    return null;
                }

                da.OpenConn();
                string sql = "select * from app_platform.t_trust_limit where Fuid = '" + uid + "' and Fspid = '" + Fspid + "' and Flstate = 1";
                return da.dsGetTotalData(sql);
            }
            catch (Exception err)
            {
                return null;
            }
            finally
            {
                da.Dispose();
            }
        }

        [WebMethod(Description = "实名认证批量查询")]
        public DataSet GetUserClassLockList(DateTime BeginDate, DateTime EndDate, int fstate, string username, int Count)
        {
            return UserClassClass.GetLockList(BeginDate, EndDate, fstate, username, Count);
        }


        //根据受理人查询（IsFuin为true）,根据财付通帐号查询（IsFuin为false）
        [WebMethod(Description = "实名认证处理查询函数")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet GetUserClassQueryList(string u_BeginTime, string u_EndTime, string fuin, int fstate, string QQType, int iPageStart, int iPageMax, int SortType)
        {
            RightAndLog rl = new RightAndLog();
            try
            {
                if (myHeader == null)
                {
                    throw new LogicException("不正确的调用方法！");
                }

                rl.actionType = "实名认证处理查询函数";
                rl.ID = fuin;
                rl.sign = 1;
                rl.strRightCode = "InfoCenter"; // "CFTUserPickTJ";
                rl.type = "查询";

                PublicRes.SetRightAndLog(myHeader, rl);
                if (!rl.CheckRight())
                {
                    throw new LogicException("用户无权执行此操作！");
                }

                UserClassClass cuser = new UserClassClass(u_BeginTime, u_EndTime, fuin, fstate, QQType, SortType);
                DataSet ds = cuser.GetResultX(iPageStart, iPageMax, "RU");

                long Appeal_BigMoney = long.Parse(System.Configuration.ConfigurationManager.AppSettings["Appeal_BigMoney"]);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    UserClassClass.HandleParameterX(ds);

                    //取出金额后如果超过大金额，打上标记。
                    ds.Tables[0].Columns.Add("Fuincolor", typeof(String));
                    ds.Tables[0].Columns.Add("balance", typeof(String));//金额 echo 20140909

                    ICEAccess ice = new ICEAccess(PublicRes.ICEServerIP, PublicRes.ICEPort);
                    try
                    {
                        ice.OpenConn();

                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            try
                            {
                                dr["Fuincolor"] = "";
                                string fuid = PublicRes.ConvertToFuid(dr["Fqqid"].ToString());

                                string strwhere = "where=" + ICEAccess.URLEncode("fuid=" + fuid + "&");
                                strwhere += ICEAccess.URLEncode("fcurtype=1&");

                                string strResp = "";

                                DataTable dtuser = ice.InvokeQuery_GetDataTable(YWSourceType.用户资源, YWCommandCode.查询用户信息, fuid, strwhere, out strResp);

                                if (dtuser == null || dtuser.Rows.Count == 0)
                                {
                                    dr["balance"] = "0";
                                    continue;
                                }

                                long lbalance = long.Parse(dtuser.Rows[0]["fbalance"].ToString());
                                dr["balance"] = lbalance.ToString();

                                if (lbalance >= Appeal_BigMoney)
                                {
                                    dr["Fuincolor"] = "BIGMONEY";
                                }
                            }
                            catch
                            {
                                continue;
                            }
                        }

                        ice.CloseConn();
                    }
                    finally
                    {
                        ice.Dispose();
                    }
                }

                return ds;

            }
            catch (LogicException err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                throw;
            }
            catch (Exception err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                throw new LogicException("Service处理失败！");
            }
            finally
            {
                rl.WriteLog();
            }
        }


        //专为(实名认证处理查询)做的查询
        [WebMethod(Description = "实名认证处理查询")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet GetUserClassQueryListForThis(string fuin, int iPageStart, int iPageMax)
        {
            RightAndLog rl = new RightAndLog();
            try
            {
                if (myHeader == null)
                {
                    throw new LogicException("不正确的调用方法！");
                }

                rl.actionType = "实名认证处理查询";
                rl.ID = fuin;
                rl.sign = 1;
                rl.strRightCode = "InfoCenter";// "CFTUserPickTJ";
                rl.type = "查询";
	
                PublicRes.SetRightAndLog(myHeader, rl);
                if (!rl.CheckRight())
                {
                    throw new LogicException("用户无权执行此操作！");
                }

                UserClassClass cuser = new UserClassClass(fuin, "UserClassQuery");
                return cuser.GetResultX(iPageStart, iPageMax, "RU");
            }
            catch (LogicException err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                throw;
            }
            catch (Exception err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                throw new LogicException("Service处理失败！");
            }
            finally
            {
                rl.WriteLog();
            }
        }


        //根据受理人查询（IsFuin为true）,根据财付通帐号查询（IsFuin为false）
        [WebMethod(Description = "实名认证处理查询函数个数")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public int GetUserClassQueryCount(string u_BeginTime, string u_EndTime, string fuin, int fstate, string QQType, int SortType)
        {
            try
            {
                UserClassClass cuser = new UserClassClass(u_BeginTime, u_EndTime, fuin, fstate, QQType, SortType);
                return cuser.GetCount("RU");
            }
            catch (Exception e)
            {
                throw new Exception("service发生错误,请联系管理员！");
                return 0;
            }
        }


        [WebMethod(Description = "实名认证详细查询")]
        public DataSet GetUserClassDetail(int flist_id)
        {
            try
            {
                UserClassClass cuser = new UserClassClass(flist_id);
                DataSet ds = cuser.GetResultX(0, 1, "RU");

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    UserClassClass.HandleParameterX(ds);
                }

                return ds;
            }
            catch (LogicException err)
            {
                throw;
            }
            catch (Exception err)
            {
                throw new LogicException("Service处理失败！");
            }
            finally
            {
                System.GC.Collect();
            }
        }


        // 2012/4/4
        [WebMethod(Description = "使用财付通帐号或者银行卡号获取用户实名认证信息")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet GetUserAuthenState(string userAccount, string userBankID, int bankType)
        {
            if (myHeader == null)
            {
                throw new Exception("不正确的调用方法");
            }

            RightAndLog rl = new RightAndLog();

            try
            {
                rl.actionType = "实名认证状态用户名银行卡号查询_New";
                rl.ID = userAccount;
                rl.sign = 1;
                rl.strRightCode = "InfoCenter";// "BaseAccount";
                rl.type = "查询";

                PublicRes.SetRightAndLog(myHeader, rl);

                if (!rl.CheckRight())
                {
                    throw new LogicException("用户无权执行此操作！");
                }

                //1.查询是否通过实名认证
                QueryUserAuthenedStateInfo queryInfo2 = new QueryUserAuthenedStateInfo(userAccount);
                DataSet ds = queryInfo2.GetResultX_ICE();
                DataRow dr = null;
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    //如果结果为空，表示没通过实名认证
                    QueryUserAuthenStateInfo queryInfo = new QueryUserAuthenStateInfo(userAccount, userBankID, bankType);
                    ds = queryInfo.GetResultX_ICE();

                    if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    {
                        return null;
                    }

                    ds.Tables[0].Columns.Add("queryType", typeof(string));

                    // 从目前来看，该查询返回的结果只有1个。
                    dr = ds.Tables[0].Rows[0];
                    dr["queryType"] = "1";// 表示查的是过程表还是已认证表,1代表查过程表，2代表已认证表

                    return ds;
                }
                dr = ds.Tables[0].Rows[0];
                if (dr["Fattr"].ToString() == "2" && dr["Fstate"].ToString() == "1")
                {
                    //Fattr = 2 & Fstate = 1通过认证
                    ds.Tables[0].Columns.Add("queryType", typeof(string));

                    // 从目前来看，该查询返回的结果只有1个。
                    dr = ds.Tables[0].Rows[0];
                    dr["queryType"] = "2";// 表示查的是过程表还是已认证表,1代表查过程表，2代表已认证表

                }
                else
                {
                    //2.未通过，则查询过程表
                    QueryUserAuthenStateInfo queryInfo = new QueryUserAuthenStateInfo(userAccount, userBankID, bankType);
                    ds = queryInfo.GetResultX_ICE();

                    if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    {
                        return null;
                    }

                    ds.Tables[0].Columns.Add("queryType", typeof(string));

                    // 从目前来看，该查询返回的结果只有1个。
                    dr = ds.Tables[0].Rows[0];
                    dr["queryType"] = "1";// 表示查的是过程表还是已认证表,1代表查过程表，2代表已认证表

                }

                return ds;
            }
            catch (System.Exception ex)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(ex.Message);
                throw;
            }
            finally
            {
                rl.WriteLog();
            }
        }



        [WebMethod(Description = "使用证件类型和证件号获取用户实名认证信息_2")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet GetUserAuthenState_ByCre2(string creid, int creType)
        {
            if (myHeader == null)
            {
                throw new Exception("不正确的调用方法");
            }

            RightAndLog rl = new RightAndLog();

            try
            {
                rl.actionType = "实名认证状态证件号查询_New";
                rl.ID = creid;
                rl.OperID = myHeader.OperID;
                rl.sign = 1;
                rl.strRightCode = "";
                rl.RightString = myHeader.RightString;
                rl.SzKey = myHeader.SzKey;
                rl.type = "查询";
                rl.UserID = myHeader.UserName;
                rl.UserIP = myHeader.UserIP;

                QueryAuthenInfo_ByCre2 qs2 = new QueryAuthenInfo_ByCre2(creid, creType);
                DataSet ds = qs2.GetResultX_ICE();

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    return null;
                }

                ds.Tables[0].Columns.Add("Fcre_stat_Name", typeof(string));
                ds.Tables[0].Columns.Add("Fcard_stat_Name", typeof(string));

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    switch (dr["Fcre_stat"].ToString())
                    {
                        case "1":
                            {
                                dr["Fcre_stat_Name"] = "已认证"; break;
                            }
                        case "2":
                            {
                                dr["Fcre_stat_Name"] = "待认证"; break;
                            }
                        case "3":
                            {
                                dr["Fcre_stat_Name"] = "确认错误等待更改信息"; break;
                            }
                        case "4":
                            {
                                dr["Fcre_stat_Name"] = "修改注册信息失败"; break;
                            }
                        case "9":
                            {
                                dr["Fcre_stat_Name"] = "uin客服审核锁定"; break;
                            }
                        case "10":
                            {
                                dr["Fcre_stat_Name"] = "作废"; break;
                            }
                        default:
                            {
                                dr["Fcre_stat_Name"] = "未知类型"; break;
                            }
                    }

                    switch (dr["Fcard_stat"].ToString())
                    {
                        case "1":
                            {
                                dr["Fcard_stat_Name"] = "已认证"; break;
                            }
                        case "2":
                            {
                                dr["Fcard_stat_Name"] = "打款中"; break;
                            }
                        case "3":
                            {
                                dr["Fcard_stat_Name"] = "打款结束认证中"; break;
                            }
                        case "4":
                            {
                                dr["Fcard_stat_Name"] = "打款失败"; break;
                            }
                        case "5":
                            {
                                dr["Fcard_stat_Name"] = "多次金额确认失败"; break;
                            }
                        case "9":
                            {
                                dr["Fcard_stat_Name"] = "uin打款锁定"; break;
                            }
                        case "10":
                            {
                                dr["Fcard_stat_Name"] = "没用"; break;
                            }
                        default:
                            {
                                dr["Fcard_stat_Name"] = "未知类型" + dr["Fcard_stat"].ToString(); break;
                            }
                    }
                }

                return ds;
            }
            catch (System.Exception ex)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(ex.Message);
                throw;
            }
            finally
            {
                rl.WriteLog();
            }
        }


        [WebMethod(Description = "使用证件类型和证件号获取用户实名认证信息")]
        public DataSet GetUserAuthenState_ByCre(int creType, string creNO)
        {
            string serviceName = "au_chk_cre_aued_service";

            string strSql = "cre_id=" + creNO + "&cre_type=" + creType;

            string sReply;
            short iResult;
            string sMsg;

            try
            {
                if (commRes.middleInvoke(serviceName, strSql, true, out sReply, out iResult, out sMsg))
                {
                    if (iResult == 0)
                    {
                        //对sreply进行解析
                        if (sReply == null || sReply.Trim() == "")
                        {
                            sMsg = "调用服务失败,无返回结果" + serviceName + strSql;
                            return null;
                        }
                        else
                        {
                            string[] strlist1 = sReply.Split('&');

                            if (strlist1.Length == 0)
                            {
                                sMsg = "调用服务失败,返回结果有误" + sReply;
                                return null;
                            }

                            DataSet ds = new DataSet();
                            ds.Tables.Add();
                            string[] strList = new string[strlist1.Length];
                            int iIndex = 0;

                            foreach (string strtmp in strlist1)
                            {
                                string[] strlist2 = strtmp.Split('=');
                                if (strlist2.Length != 2)
                                {
                                    sMsg = "调用服务失败,返回结果有误" + sReply;
                                    return null;
                                }

                                ds.Tables[0].Columns.Add(strlist2[0].Trim());
                                strList[iIndex++] = strlist2[1].Trim();
                            }

                            // 因为不清楚具体的SQL语句,所以当查询不出first_authen_id这个则认为查询结果为空
                            //（貌似是Fcre_state不为1和fstate不为1则查不到） 2012/4/9
                            if (!ds.Tables[0].Columns.Contains("first_authen_id"))
                            {
                                return null;
                            }

                            ds.Tables[0].Rows.Add(strList);

                            string fuid = PublicRes.ConvertToFuid(ds.Tables[0].Rows[0]["first_authen_id"].ToString());

                            QueryAuthenRelationInfo qar = new QueryAuthenRelationInfo(fuid);

                            DataSet ds2 = qar.GetResultX_ICE();

                            if (ds2 != null && ds2.Tables.Count != 0 && ds2.Tables[0].Rows.Count != 0)
                            {
                                ds.Tables[0].Columns.Add("authen_id_nums", typeof(string));
                                ds.Tables[0].Rows[0]["authen_id_nums"] = ds2.Tables[0].Rows.Count;
                                for (int i = 0; i < ds2.Tables[0].Rows.Count; i++)
                                {
                                    ds.Tables[0].Columns.Add("authen_id_n" + i, typeof(string));
                                    ds.Tables[0].Rows[0][("authen_id_n" + i)] = ds2.Tables[0].Rows[i]["Fslave_id"].ToString();
                                }
                            }

                            return ds;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        
            return null;
        }

      
        [WebMethod(Description = "删除认证信息")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public bool DelAuthen(string qqid, out string msg)
        {
            RightAndLog rl = new RightAndLog();
            try
            {
                if (myHeader == null)
                {
                    throw new Exception("不正确的调用方法！");
                }

                rl.actionType = "删除认证信息";
                rl.ID = qqid;
                rl.sign = 1;
                rl.strRightCode = "FreezeUser";
                rl.type = "查询";

                PublicRes.SetRightAndLog(myHeader, rl);
                if (!rl.CheckRight())
                {
                    throw new LogicException("用户无权执行此操作！");
                }

                if (qqid == null || qqid.Trim() == "")
                {
                    msg = "参数不足";
                    return false;
                }

                string uid = PublicRes.ConvertToFuid(qqid);

                string inmsg = "uid=" + uid; //删除认证接口改为新接口实现2013.6.13 yinhuang
                inmsg += "&operator=" + myHeader.UserName;
                inmsg += "&memo=客服删除";

                string reply;
                short sresult;

                if (TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.middleInvoke("au_del_auinfo_service", inmsg, true, out reply, out sresult, out msg))
                {
                    if (sresult != 0)
                    {
                        msg = "au_del_authen_service接口失败：result=" + sresult + "，msg=" + "&reply=" + reply;
                        return false;
                    }
                    else
                    {
                        if (reply.StartsWith("result=0"))
                        {
                            return true;
                        }
                        else
                        {
                            msg = "au_del_authen_service接口失败：result=" + sresult + "，msg=" + msg + "&reply=" + reply;
                            return false;
                        }
                    }
                }
                else
                {
                    msg = "au_del_authen_service接口失败：result=" + sresult + "，msg=" + msg + "&reply=" + reply;
                    return false;
                }

            }
            catch (LogicException err)
            {
                rl.sign = 0;
                msg = err.Message;
                return false;
            }
            catch (Exception err)
            {
                rl.sign = 0;

                msg = err.Message;
                return false;
            }
            finally
            {
                rl.WriteLog();
            }

        }



        [WebMethod(Description = "查询个人证书信息")]
        public string GetUserCrtInfo(string qqid)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("CRT"));
            try
            {
                string uid = PublicRes.ConvertToFuid(qqid);

                if (uid == null || uid.Length < 3)
                {
                    return "用户不存在";
                }

                string strSql = "select Fvalue,Fstate from " + PublicRes.GetTName("cft_digit_crt", "t_user_attr", uid) + " where Fuid='"
                    + uid + "' and Fattr=1";

                da.OpenConn();

                DataTable dt = da.GetTable(strSql);

                if (dt == null || dt.Rows.Count == 0)
                {
                    return "无证书";
                }

                int Fvalue = Int32.Parse(dt.Rows[0][0].ToString());
                int Fstate = Int32.Parse(dt.Rows[0][0].ToString());

                if (Fstate == 2)
                {
                    return "证书已作废";
                }

                if (Fvalue == 1)
                {
                    return "已开通证书";
                }
                else if (Fvalue == 2)
                {
                    return "未开通证书";
                }
                else if (Fvalue == 3)
                {
                    return "证书已冻结";
                }

                return "未知情况" + Fstate + Fvalue;
            }
            catch (Exception err)
            {
                return "查询出错";
            }
            finally
            {
                da.Dispose();
            }
        }


        [WebMethod(Description = "按类型查询银行")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet GetBankByType(string typename, string flag)
        {
            if (myHeader == null)
            {
                throw new Exception("不正确的调用方法！");
            }

            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("ZW"));
            try
            {
                da.OpenConn();
                string strSql = "select * from c2c_zwdb.t_bank_class where 1=1 ";


                if (typename != "ALL")
                {
                    if (typename == "SK")
                    {
                        strSql += " and FFlag1='T' ";

                        if (flag != "ALL")
                        {
                            strSql += " and FFlag1_1='" + flag + "' ";
                        }
                    }
                    else if (typename == "FK")
                    {
                        strSql += " and FFlag2='T' ";

                        if (flag != "ALL")
                        {
                            strSql += " and FFlag2_1='" + flag + "' ";
                        }
                    }
                }
                strSql += " order by   hex(Fbank_name)  asc ";

                DataSet ds = da.dsGetTotalData(strSql);
                return ds;
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                da.Dispose();
            }

        }


        [WebMethod(Description = "查询个人证书信息列表")]
        public DataSet GetUserCrtList(string qqid)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("CRT"));
            try
            {
                string uid = PublicRes.ConvertToFuid(qqid);

                if (uid == null || uid.Length < 3)
                {
                    return null;
                }

                string strSql = "select * from " + PublicRes.GetTName("cft_digit_crt", "t_user_crt", uid) + " where Fuid='"
                    + uid + "' and Ftype=1";

                da.OpenConn();

                DataSet ds = da.dsGetTotalData(strSql);

                if (ds == null || ds.Tables.Count == 0)
                {
                    return null;
                }

                ds.Tables[0].Columns.Add("FstateName");
                ds.Tables[0].Columns.Add("FlstateName");

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    int Fstate = Int32.Parse(dr["Fstate"].ToString());
                    int Flstate = Int32.Parse(dr["Flstate"].ToString());

                    if (Fstate == 1)
                    {
                        dr["FstateName"] = "初始状态";
                    }
                    else if (Fstate == 2)
                    {
                        dr["FstateName"] = "有效(已签署)";
                    }
                    else if (Fstate == 3)
                    {
                        dr["FstateName"] = "挂起(暂时冻结)";
                    }
                    else if (Fstate == 4)
                    {
                        dr["FstateName"] = "注销(不再可用)";
                    }
                    else
                    {
                        dr["FstateName"] = "未定义" + Fstate;
                    }

                    if (Flstate == 1)
                    {
                        dr["FlstateName"] = "正常";
                    }
                    else if (Flstate == 2)
                    {
                        dr["FlstateName"] = "冻结";
                    }
                    else
                    {
                        dr["FlstateName"] = "未定义" + Flstate;
                    }
                }

                return ds;
            }
            catch (Exception err)
            {
                return null;
            }
            finally
            {
                da.Dispose();
            }
        }

        [WebMethod(Description = "查询关闭证书服务信息")]
        public DataSet GetDeleteQueryInfo(string qqid)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("CRT"));
            try
            {
                string uid = PublicRes.ConvertToFuid(qqid);

                if (uid == null || uid.Length < 3)
                {
                    return null;
                }

                string strSql = "select Fmodify_time ,Fstandby3 as DeleteIP from " + PublicRes.GetTName("cft_digit_crt", "t_user_attr", uid) + " where Fuid='"
                    + uid + "' and Fattr=1 and Fstate='2' and Fvalue='2'";

                da.OpenConn();

                DataSet ds = da.dsGetTotalData(strSql);

                if (ds == null || ds.Tables.Count == 0)
                {
                    return null;
                }
                return ds;
            }
            catch (Exception err)
            {
                return null;
            }
            finally
            {
                da.Dispose();
            }
        }


        [WebMethod(Description = "删除个人证书")]
        public void DeleteUserCrt(string qqid)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("CRT"));
            try
            {
                string uid = PublicRes.ConvertToFuid(qqid);

                if (uid == null || uid.Length < 3)
                {
                    throw new Exception("该用户不存在!");
                }

                da.OpenConn();

                string Sql = "delete from " + PublicRes.GetTName("cft_digit_crt", "t_user_crt", uid) + " where Fuid='"
                             + uid + "' and Ftype=1";

                da.ExecSqlNum(Sql);

                Sql = "delete from " + PublicRes.GetTName("cft_digit_crt", "t_user_attr", uid) + " where Fuid='"
                      + uid + "' and Fattr=1";

                da.ExecSqlNum(Sql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                da.Dispose();
            }
        }

        //因为删除个人证书直接删除数据库记录不合理，所以做了这个实现
        [WebMethod(Description = "关闭证书服务")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public void DeleteCrtService(string qqid)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("CRT"));
            try
            {
                string uid = PublicRes.ConvertToFuid(qqid);

                if (uid == null || uid.Length < 3)
                {
                    throw new Exception("该用户不存在!");
                }

                da.OpenConn();

                string Sql = "update " + PublicRes.GetTName("cft_digit_crt", "t_user_crt", uid) + " set Fstate = 4 , Flstate = 2 , Fmodify_time = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where Fuid = '"
                              + uid + "' and Ftype = 1 and Flstate != 2";

                da.ExecSqlNum(Sql);
                string IP = myHeader.UserIP;
                Sql = "update " + PublicRes.GetTName("cft_digit_crt", "t_user_attr", uid) + " set Fstate = 2 , Fvalue = '2' , Fmodify_time = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' , Fstandby3 = '" + IP + "' where Fuid = '"
                             + uid + "' and  Fattr = 1";

                da.ExecSqlNum(Sql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                da.Dispose();
            }
        }


        [WebMethod(Description = "查询手机令牌列表")]
        public DataSet GetMobileTokenList(string qqid)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("MOBILETOKEN"));
            try
            {
                string uid = PublicRes.ConvertToFuid(qqid);
                if (uid == null || uid.Length < 3)
                {
                    return null;
                }

                string strSql = "select * from " + PublicRes.GetTName("cft_digit_crt", "t_mbtoken_bindrec", uid) + " where Fuid='"
                    + uid + "'";

                da.OpenConn();

                DataSet ds = da.dsGetTotalData(strSql);

                if (ds == null || ds.Tables.Count == 0)
                {
                    return null;
                }

                ds.Tables[0].Columns.Add("FoperName");
                ds.Tables[0].Columns.Add("FstateName");

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    int Foper = Int32.Parse(dr["Foper"].ToString());
                    int Fstate = Int32.Parse(dr["Fstate"].ToString());

                    if (Foper == 1)
                    {
                        dr["FoperName"] = "绑定";
                    }
                    else if (Foper == 2)
                    {
                        dr["FoperName"] = "未绑定";
                    }
                    else if (Foper == 3)
                    {
                        dr["FoperName"] = "申诉解绑";
                    }

                    if (Fstate == 1)
                    {
                        dr["FstateName"] = "有效";
                    }
                    else if (Fstate == 2)
                    {
                        dr["FstateName"] = "无效";
                    }
                }

                return ds;
            }
            catch (Exception err)
            {
                return null;
            }
            finally
            {
                da.Dispose();
            }
        }


        //2012-8-13 by krola
        //根据订单号查询话费商户的发货记录
        [WebMethod(Description = "根据订单号查询话费商户的发货记录")]
        public DataSet GetPhoneBillRecordByTransID(string strListID, out string strOutMsg)
        {

            strOutMsg = "";
            MySqlAccess sqlcc = new MySqlAccess(PublicRes.GetConnString("MOBILE"));

            try
            {

                sqlcc.OpenConn();

                //1. 根据订单号获取Uin
                DateTime beginTime = DateTime.Parse(ConfigurationManager.AppSettings["sBeginTime"].ToString());
                DateTime endTime = DateTime.Parse(ConfigurationManager.AppSettings["sEndTime"].ToString());
                DataSet ds = GetPayList(strListID, 4, beginTime, endTime, 1, 2);

                if (ds == null || ds.Tables.Count == 0)
                {
                    throw new LogicException("核心订单库中没有找到此订单记录！");
                }

                //买家QQ账号
                string strBuyUid = ds.Tables[0].Rows[0]["Fbuyid"].ToString();
                string strDbName = "mobile.t_trans_" + strBuyUid.Substring(strBuyUid.Length - 2, 2);
                string strSql = "SELECT FTransId,FSubmitTime,FTotalFee,FAmount,FComment,FChgMobile,FState,FChargeTime,FSpName ,FUserState FROM {0} WHERE FTransId='{1}' AND FUin='{2}'";
                strSql = String.Format(strSql, strDbName, strListID, strBuyUid);

                ds = sqlcc.dsGetTotalData(strSql);
                return ds;
            }
            catch (System.Exception ex)
            {
                strOutMsg = ex.Message;
                return null;
            }
            finally
            {
                sqlcc.Dispose();
            }

        }


        //根据手机号查询出所有发货记录
        [WebMethod(Description = "根据订单号查询话费商户的发货记录")]
        public DataSet GetPhoneBillRecordByPhoneNumber(string strPhoneNumber, out string strOutMsg)
        {
            strOutMsg = "";
            MySqlAccess sqlcc = new MySqlAccess(PublicRes.GetConnString("MOBILE"));

            try
            {
                sqlcc.OpenConn();
                string strDbName = "mobile.t_telno_" + strPhoneNumber.Substring(strPhoneNumber.Length - 2, 2);
                string strSql = "SELECT FTransId,FSubmitTime,FTotalFee,FAmount,FComment,FChgMobile,FState,FChargeTime,FSpName,FUserState FROM {0} WHERE FChgMobile='{1}'";
                strSql = String.Format(strSql, strDbName, strPhoneNumber);
                DataSet ds = sqlcc.dsGetTotalData(strSql);

                return ds;
            }
            catch (System.Exception ex)
            {
                strOutMsg = ex.Message;
                return null;
            }
            finally
            {
                sqlcc.Dispose();
            }

        }


        [WebMethod(Description = "银行批次交易查询函数")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet QueryDKBankList(string batchid, string batchid_forbank, string bank_type, string status, string starttime, string endtime, int limStart, int limMax)
        {
            RightAndLog rl = new RightAndLog();
            try
            {
                if (myHeader == null)
                {
                    throw new LogicException("不正确的调用方法！");
                }

                rl.actionType = "银行批次交易查询函数";
                rl.ID = batchid;
                rl.OperID = myHeader.OperID;
                rl.sign = 1;
                rl.strRightCode = "";
                rl.RightString = myHeader.RightString;
                rl.SzKey = myHeader.SzKey;
                rl.type = "查询";
                rl.UserID = myHeader.UserName;
                rl.UserIP = myHeader.UserIP;

                string strSql = " select A.*,B.Fspid,B.Fsp_batchid from cft_cep_db.t_bank_batchrcd A,cft_cep_db.t_batch_record B where A.Fcreate_time between '"
                    + starttime + "' and '" + endtime + "' and B.Fbatchid=A.Fbatchid ";

                if (batchid != "")
                    strSql += " and A.Fbatchid='" + batchid + "' ";

                if (batchid_forbank != "")
                    strSql += " and A.Fbatchid_forbank='" + batchid_forbank + "' ";

                if (bank_type != "")
                    strSql += " and A.Fbank_type='" + bank_type + "' ";

                if (status != "0")
                    strSql += " and A.Fstatus=" + status + " ";

                DataSet ds = new DataSet();

                MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("INC_NEW"));
                try
                {
                    da.OpenConn();
                    ds = da.dsGetTableByRange(strSql, limStart, limMax);

                    if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                        return null;

                    ds.Tables[0].Columns.Add("FstatusName", typeof(string));

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        switch (dr["Fstatus"].ToString())
                        {
                            case "1":
                                {
                                    dr["FstatusName"] = "初始状态"; break;
                                }
                            case "2":
                                {
                                    dr["FstatusName"] = "分批完成"; break;
                                }
                            case "3":
                                {
                                    dr["FstatusName"] = "预支付完成"; break;
                                }
                            case "4":
                                {
                                    dr["FstatusName"] = "部分发送中"; break;
                                }
                            case "5":
                                {
                                    dr["FstatusName"] = "发送完成"; break;
                                }
                            case "6":
                                {
                                    dr["FstatusName"] = "结果获取中"; break;
                                }
                            case "7":
                                {
                                    dr["FstatusName"] = "执行完成"; break;
                                }
                            case "8":
                                {
                                    dr["FstatusName"] = "全部成功"; break;
                                }
                            case "9":
                                {
                                    dr["FstatusName"] = "全部失败"; break;
                                }

                            default:
                                {
                                    dr["FstatusName"] = "未知" + dr["Fstatus"].ToString(); break;
                                }
                        }
                    }

                    return ds;
                }
                catch
                {
                    return null;
                }
                finally
                {
                    da.Dispose();
                }

            }
            catch (LogicException err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                throw;
            }
            catch (Exception err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                throw new LogicException("Service处理失败！(" + err.Message + ")");
            }
            finally
            {
                rl.WriteLog();
            }
        }

        [WebMethodAttribute(Description = "统计银行批次交易情况")]
        public DataSet CountDKBankList(string batchid, string batchid_forbank, string bank_type, string status, string starttime, string endtime)
        {
            string strSql = " select count(*),sum(Fsucpay_count),sum(Fsucpay_fee),sum(0),sum(0),"
                + "sum(Ftotal_count-Fsucpay_count-0),sum(Ftotal_fee-Fsucpay_fee-0) from cft_cep_db.t_bank_batchrcd where Fcreate_time between '"
                + starttime + "' and '" + endtime + "' ";

            if (batchid != "")
                strSql += " and Fbatchid='" + batchid + "' ";

            if (batchid_forbank != "")
                strSql += " and Fbatchid_forbank='" + batchid_forbank + "' ";

            if (bank_type != "")
                strSql += " and Fbank_type='" + bank_type + "' ";

            if (status != "0")
                strSql += " and Fstatus=" + status + " ";

            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("INC_NEW"));
            try
            {
                da.OpenConn();
                DataSet ds = da.dsGetTotalData(strSql);
                return ds;
            }
            catch
            {
                return null;
            }
            finally
            {
                da.Dispose();
            }
        }


        [WebMethod(Description = "银行批次交易查询函数")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet QueryDKBankListDetail(string bank_batch_id)
        {
            RightAndLog rl = new RightAndLog();
            try
            {
                if (myHeader == null)
                {
                    throw new LogicException("不正确的调用方法！");
                }

                rl.actionType = "银行批次交易查询函数";
                rl.ID = bank_batch_id;
                rl.OperID = myHeader.OperID;
                rl.sign = 1;
                rl.strRightCode = "";
                rl.RightString = myHeader.RightString;
                rl.SzKey = myHeader.SzKey;
                rl.type = "查询";
                rl.UserID = myHeader.UserName;
                rl.UserIP = myHeader.UserIP;

                string strSql = " select * from cft_cep_db.t_bank_batchrcd where Fbank_batch_id='" + bank_batch_id + "'";

                DataSet ds = new DataSet();

                MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("INC_NEW"));
                try
                {
                    da.OpenConn();
                    ds = da.dsGetTotalData(strSql);

                    if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                        return null;

                    ds.Tables[0].Columns.Add("FstatusName", typeof(string));

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        switch (dr["Fstatus"].ToString())
                        {
                            case "1":
                                {
                                    dr["FstatusName"] = "初始状态"; break;
                                }
                            case "2":
                                {
                                    dr["FstatusName"] = "分批完成"; break;
                                }
                            case "3":
                                {
                                    dr["FstatusName"] = "预支付完成"; break;
                                }
                            case "4":
                                {
                                    dr["FstatusName"] = "部分发送中"; break;
                                }
                            case "5":
                                {
                                    dr["FstatusName"] = "发送完成"; break;
                                }
                            case "6":
                                {
                                    dr["FstatusName"] = "结果获取中"; break;
                                }
                            case "7":
                                {
                                    dr["FstatusName"] = "执行完成"; break;
                                }
                            case "8":
                                {
                                    dr["FstatusName"] = "全部成功"; break;
                                }
                            case "9":
                                {
                                    dr["FstatusName"] = "全部失败"; break;
                                }

                            default:
                                {
                                    dr["FstatusName"] = "未知" + dr["Fstatus"].ToString(); break;
                                }
                        }
                    }

                    return ds;
                }
                catch
                {
                    return null;
                }
                finally
                {
                    da.Dispose();
                }

            }
            catch (LogicException err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                throw;
            }
            catch (Exception err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                throw new LogicException("Service处理失败！(" + err.Message + ")");
            }
            finally
            {
                rl.WriteLog();
            }
        }


        [WebMethod(Description = "协议库单笔查询函数")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet QueryDKContractList(string spid, string mer_cnr, string sp_batchid,
        string mobile, string bankacc_no, string uname, string credit_id, string mer_aid,
        string status, string strSTime, string strETime, int limStart, int limMax)
        {
            RightAndLog rl = new RightAndLog();
            try
            {
                if (myHeader == null)
                {
                    throw new LogicException("不正确的调用方法！");
                }

                rl.actionType = "协议库单笔查询函数";
                rl.ID = sp_batchid;
                rl.OperID = myHeader.OperID;
                rl.sign = 1;
                rl.strRightCode = "";
                rl.RightString = myHeader.RightString;
                rl.SzKey = myHeader.SzKey;
                rl.type = "查询";
                rl.UserID = myHeader.UserName;
                rl.UserIP = myHeader.UserIP;
 
                string strSql = " select * from cft_cep_db.t_contract  where Fcreate_time>='" + strSTime + "' and Fcreate_time<='" + strETime + "' ";

                if (spid != "")
                    strSql += " and Fspid='" + spid + "' ";

                if (mer_cnr != "")
                    strSql += " and Fmer_cnr='" + mer_cnr + "' ";

                if (sp_batchid != "")
                    strSql += " and Fsp_batchid='" + sp_batchid + "' ";

                if (mobile != "")
                    strSql += " and Fmobile='" + mobile + "' ";

                if (bankacc_no != "")
                {
                    //以md5的方式加密银行卡号
                    bankacc_no = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(bankacc_no, "md5").ToLower();
                    strSql += " and Fbankacc_hash='" + bankacc_no + "' ";//按银行卡号查， 使用Fbankacc_hash字段
                }

                if (uname != "")
                    strSql += " and Funame='" + uname + "' ";

                if (credit_id != "")
                    strSql += " and Fcredit_id='" + credit_id + "' ";

                if (mer_aid != "")
                    strSql += " and Fmer_aid='" + mer_aid + "' ";

                if (status != "0")
                {
                    //1成功 2失败 3处理中
                    if (status == "1")
                        strSql += " and Fstatus=7 ";
                    else if (status == "2")
                        strSql += " and Fstatus=6 ";
                    else if (status == "3")
                        strSql += " and Fstatus in (4,5) ";
                }

                DataSet ds = new DataSet();

                MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("INC_NEW"));
                try
                {
                    da.OpenConn();
                    ds = da.dsGetTableByRange(strSql, limStart, limMax);

                    if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                        return null;

                    ds.Tables[0].Columns.Add("FstatusName", typeof(string));

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        switch (dr["Fstatus"].ToString())
                        {
                            case "1":
                                {
                                    dr["FstatusName"] = "初始化"; break;
                                }
                            case "2":
                                {
                                    dr["FstatusName"] = "已审核"; break;
                                }
                            case "3":
                                {
                                    dr["FstatusName"] = "审核失败"; break;
                                }
                            case "4":
                                {
                                    dr["FstatusName"] = "协议待验证"; break;
                                }
                            case "5":
                                {
                                    dr["FstatusName"] = "需补要素"; break;
                                }
                            case "6":
                                {
                                    dr["FstatusName"] = "协议验证失败"; break;
                                }
                            case "7":
                                {
                                    dr["FstatusName"] = "协议已生效"; break;
                                }
                            default:
                                {
                                    dr["FstatusName"] = "未知" + dr["Fstatus"].ToString(); break;
                                }
                        }
                    }

                    return ds;
                }
                catch
                {
                    return null;
                }
                finally
                {
                    da.Dispose();
                }

            }
            catch (LogicException err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                throw;
            }
            catch (Exception err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                throw new LogicException("Service处理失败！(" + err.Message + ")");
            }
            finally
            {
                rl.WriteLog();
            }
        }



        [WebMethod(Description = "协议单笔查询明细函数")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet QueryDKContractDetail(string cep_cnr)
        {
            RightAndLog rl = new RightAndLog();
            try
            {
                if (myHeader == null)
                {
                    throw new LogicException("不正确的调用方法！");
                }

                rl.actionType = "协议单笔查询明细函数";
                rl.ID = cep_cnr;
                rl.OperID = myHeader.OperID;
                rl.sign = 1;
                rl.strRightCode = "";
                rl.RightString = myHeader.RightString;
                rl.SzKey = myHeader.SzKey;
                rl.type = "查询";
                rl.UserID = myHeader.UserName;
                rl.UserIP = myHeader.UserIP;

                string strSql = " select * from cft_cep_db.t_contract  where Fcep_cnr='" + cep_cnr + "' ";

                DataSet ds = new DataSet();

                MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("INC_NEW"));
                try
                {
                    da.OpenConn();
                    ds = da.dsGetTotalData(strSql);

                    if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                        return null;

                    ds.Tables[0].Columns.Add("FstatusName", typeof(string));

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {

                        switch (dr["Fstatus"].ToString())
                        {
                            case "1":
                                {
                                    dr["FstatusName"] = "初始化"; break;
                                }
                            case "2":
                                {
                                    dr["FstatusName"] = "已审核"; break;
                                }
                            case "3":
                                {
                                    dr["FstatusName"] = "审核失败"; break;
                                }
                            case "4":
                                {
                                    dr["FstatusName"] = "协议待验证"; break;
                                }
                            case "5":
                                {
                                    dr["FstatusName"] = "需补要素"; break;
                                }
                            case "6":
                                {
                                    dr["FstatusName"] = "协议验证失败"; break;
                                }
                            case "7":
                                {
                                    dr["FstatusName"] = "协议已生效"; break;
                                }
                            default:
                                {
                                    dr["FstatusName"] = "未知" + dr["Fstatus"].ToString(); break;
                                }
                        }
                    }

                    return ds;
                }
                catch
                {
                    return null;
                }
                finally
                {
                    da.Dispose();
                }

            }
            catch (LogicException err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                throw;
            }
            catch (Exception err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                throw new LogicException("Service处理失败！(" + err.Message + ")");
            }
            finally
            {
                rl.WriteLog();
            }
        }


        [WebMethod(Description = "协议库批次查询函数")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet QueryDKContractBatchList(string spid, string sp_batchid, string strSTime, string strETime, int limStart, int limMax)
        {
            RightAndLog rl = new RightAndLog();
            try
            {
                if (myHeader == null)
                {
                    throw new LogicException("不正确的调用方法！");
                }

                rl.actionType = "协议库单笔查询函数";
                rl.ID = sp_batchid;
                rl.OperID = myHeader.OperID;
                rl.sign = 1;
                rl.strRightCode = "";
                rl.RightString = myHeader.RightString;
                rl.SzKey = myHeader.SzKey;
                rl.type = "查询";
                rl.UserID = myHeader.UserName;
                rl.UserIP = myHeader.UserIP;

                string strSql = " select *,Ftotal_count-Fsucc_count-Ffail_count as Fhandle_count from cft_cep_db.t_contract_batch  where Fcreate_time>='" + strSTime + "' and Fcreate_time<='" + strETime + "' ";

                if (spid != "")
                    strSql += " and Fspid='" + spid + "' ";


                if (sp_batchid != "")
                    strSql += " and Fsp_batchid='" + sp_batchid + "' ";

                DataSet ds = new DataSet();

                MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("INC_NEW"));
                try
                {
                    da.OpenConn();
                    ds = da.dsGetTableByRange(strSql, limStart, limMax);

                    if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                        return null;

                    ds.Tables[0].Columns.Add("FstateName", typeof(string));

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        switch (dr["Fstate"].ToString())
                        {
                            case "1":
                                {
                                    dr["FstateName"] = "批次初始化"; break;
                                }
                            case "2":
                                {
                                    dr["FstateName"] = "批次记录失败"; break;
                                }
                            case "3":
                                {
                                    dr["FstateName"] = "批次待审批"; break;
                                }
                            case "4":
                                {
                                    dr["FstateName"] = "批次取消"; break;
                                }
                            case "5":
                                {
                                    dr["FstateName"] = "批次审核失败"; break;
                                }
                            case "6":
                                {
                                    dr["FstateName"] = "批次验证中"; break;
                                }
                            case "7":
                                {
                                    dr["FstateName"] = "批次处理中"; break;
                                }
                            case "8":
                                {
                                    dr["FstateName"] = "批次处理完"; break;
                                }
                            case "9":
                                {
                                    dr["FstateName"] = "处理结果全部成功"; break;
                                }
                            case "10":
                                {
                                    dr["FstateName"] = "处理结果部分成功"; break;
                                }
                            case "11":
                                {
                                    dr["FstateName"] = "处理结果全部失败"; break;
                                }
                            case "12":
                                {
                                    dr["FstateName"] = "后台预处理中"; break;
                                }
                            case "13":
                                {
                                    dr["FstateName"] = "预处理完成"; break;
                                }
                            default:
                                {
                                    dr["FstateName"] = "未知" + dr["Fstate"].ToString(); break;
                                }
                        }
                    }

                    return ds;
                }
                catch
                {
                    return null;
                }
                finally
                {
                    da.Dispose();
                }

            }
            catch (LogicException err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                throw;
            }
            catch (Exception err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                throw new LogicException("Service处理失败！(" + err.Message + ")");
            }
            finally
            {
                rl.WriteLog();
            }
        }


        [WebMethod(Description = "协议批次查询明细函数")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet QueryDKContractBatchDetail(string batchid)
        {
            RightAndLog rl = new RightAndLog();
            try
            {
                if (myHeader == null)
                {
                    throw new LogicException("不正确的调用方法！");
                }

                rl.actionType = "协议单笔查询明细函数";
                rl.ID = batchid;
                rl.OperID = myHeader.OperID;
                rl.sign = 1;
                rl.strRightCode = "";
                rl.RightString = myHeader.RightString;
                rl.SzKey = myHeader.SzKey;
                rl.type = "查询";
                rl.UserID = myHeader.UserName;
                rl.UserIP = myHeader.UserIP;
      
                string strSql = " select * from cft_cep_db.t_contract_batch  where Fbatchid='" + batchid + "' ";

                DataSet ds = new DataSet();

                MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("INC_NEW"));
                try
                {
                    da.OpenConn();
                    ds = da.dsGetTotalData(strSql);

                    if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                        return null;

                    ds.Tables[0].Columns.Add("FstateName", typeof(string));

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {

                        switch (dr["Fstate"].ToString())
                        {
                            case "1":
                                {
                                    dr["FstateName"] = "批次初始化"; break;
                                }
                            case "2":
                                {
                                    dr["FstateName"] = "批次记录失败"; break;
                                }
                            case "3":
                                {
                                    dr["FstateName"] = "批次待审批"; break;
                                }
                            case "4":
                                {
                                    dr["FstateName"] = "批次取消"; break;
                                }
                            case "5":
                                {
                                    dr["FstateName"] = "批次审核失败"; break;
                                }
                            case "6":
                                {
                                    dr["FstateName"] = "批次验证中"; break;
                                }
                            case "7":
                                {
                                    dr["FstateName"] = "批次处理中"; break;
                                }
                            case "8":
                                {
                                    dr["FstateName"] = "批次处理完"; break;
                                }
                            case "9":
                                {
                                    dr["FstateName"] = "处理结果全部成功"; break;
                                }
                            case "10":
                                {
                                    dr["FstateName"] = "处理结果部分成功"; break;
                                }
                            case "11":
                                {
                                    dr["FstateName"] = "处理结果全部失败"; break;
                                }
                            case "12":
                                {
                                    dr["FstateName"] = "后台预处理中"; break;
                                }
                            case "13":
                                {
                                    dr["FstateName"] = "预处理完成"; break;
                                }
                            default:
                                {
                                    dr["FstateName"] = "未知" + dr["Fstate"].ToString(); break;
                                }
                        }
                    }

                    return ds;
                }
                catch
                {
                    return null;
                }
                finally
                {
                    da.Dispose();
                }

            }
            catch (LogicException err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                throw;
            }
            catch (Exception err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                throw new LogicException("Service处理失败！(" + err.Message + ")");
            }
            finally
            {
                rl.WriteLog();
            }
        }



        [WebMethod(Description = "银行账户限额批次查询函数")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet GetDKLimit_List(string banktype, string bankaccno, int limStart, int limMax)
        {
            RightAndLog rl = new RightAndLog();
            try
            {
                if (myHeader == null)
                {
                    throw new LogicException("不正确的调用方法！");
                }

                rl.actionType = "银行账户限额批次查询函数";
                rl.ID = bankaccno;
                rl.OperID = myHeader.OperID;
                rl.sign = 1;
                rl.strRightCode = "";
                rl.RightString = myHeader.RightString;
                rl.SzKey = myHeader.SzKey;
                rl.type = "查询";
                rl.UserID = myHeader.UserName;
                rl.UserIP = myHeader.UserIP;
      
                DataSet ds = new DataSet();

                MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("INC_NEW"));
                try
                {
                    da.OpenConn();

                    string roleid = banktype + bankaccno + "@cep.tenpay.com";
                    roleid = PublicRes.ConvertToFuid(roleid);

                    string strSql = " select Fservice_code,'" + banktype + "' as Fbanktype,'" + bankaccno + "' as Fbankaccno, ";

                    strSql += "sum(case Fdimension when 1 then Fonce_data else 0 end)/100 as Fonce_data,";//单笔限额

                    strSql += "sum(case Fdimension when 1 then Fday_data else 0 end)/100 as Fday_sum_data,";//单日限额
                    strSql += "sum(case Fdimension when 3 then Fday_data else 0 end)/100 as Fday_use_data,";//单日累计使用限额

                    strSql += "sum(case Fdimension when 1 then Fweek_data else 0 end)/100 as Fweek_sum_data,";//单周限额
                    strSql += "sum(case Fdimension when 3 then Fweek_data else 0 end)/100 as Fweek_use_data,";//单周累计使用限额

                    strSql += "sum(case Fdimension when 1 then Fmounth_data else 0 end)/100 as Fmonth_sum_data,";//单月限额
                    strSql += "sum(case Fdimension when 3 then Fmounth_data else 0 end)/100 as Fmonth_use_data,";//单月累计使用限额

                    strSql += "sum(case Fdimension when 1 then Fquarter_data else 0 end)/100 as Fquarter_sum_data,";//单季限额
                    strSql += "sum(case Fdimension when 3 then Fquarter_data else 0 end)/100 as Fquarter_use_data,";//单季累计使用限额

                    strSql += "sum(case Fdimension when 1 then Fyear_data else 0 end)/100 as Fyear_sum_data,";//单年限额
                    strSql += "sum(case Fdimension when 3 then Fyear_data else 0 end)/100 as Fyear_use_data,";//单年累计使用限额

                    strSql += "sum(case Fdimension when 2 then Fday_data else 0 end) as Fday_sum_count,";//单日限次
                    strSql += "sum(case Fdimension when 4 then Fday_data else 0 end) as Fday_use_count,";//单日累计使用次数

                    strSql += "sum(case Fdimension when 2 then Fweek_data else 0 end) as Fweek_sum_count,";//单周限次
                    strSql += "sum(case Fdimension when 4 then Fweek_data else 0 end) as Fweek_use_count,";//单周累计使用次数

                    strSql += "sum(case Fdimension when 2 then Fmounth_data else 0 end) as Fmonth_sum_count,";//单月限次
                    strSql += "sum(case Fdimension when 4 then Fmounth_data else 0 end) as Fmonth_use_count,";//单月累计使用次数

                    strSql += "sum(case Fdimension when 2 then Fquarter_data else 0 end) as Fquarter_sum_count,";//单季限次
                    strSql += "sum(case Fdimension when 4 then Fquarter_data else 0 end) as Fquarter_use_count,";//单季累计使用次数

                    strSql += "sum(case Fdimension when 2 then Fyear_data else 0 end) as Fyear_sum_count,";//单年限次
                    strSql += "sum(case Fdimension when 4 then Fyear_data else 0 end) as Fyear_use_count ";//单年累计使用次数

                    strSql += " from " + PublicRes.GetTName("cft_cep_db", "t_service_limit", roleid) + " where Frole_id='" + roleid + "' and Fflag=1 group by Fservice_code ";

                    ds = da.dsGetTableByRange(strSql, limStart, limMax);

                    if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                        return null;

                    ds.Tables[0].Columns.Add("Fspid", typeof(string));
                    ds.Tables[0].Columns.Add("Fspname", typeof(string));
                    ds.Tables[0].Columns.Add("Fcodeid", typeof(string));
                    ds.Tables[0].Columns.Add("Fcodename", typeof(string));

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        string tmp = dr["Fservice_code"].ToString();
                        string spid = tmp.Substring(0, 10);
                        dr["Fspid"] = spid;

                        string msg = "";
                        dr["Fspname"] = CommQuery.GetOneResultFromICE("spid=" + spid, CommQuery.QUERY_MERCHANTINFO, "FName", out msg);
                        string codeid = dr["Fservice_code"].ToString().Replace(spid, "");
                        dr["Fcodeid"] = codeid;

                        if (getData.htService_code.Contains(codeid))
                        {
                            dr["Fcodename"] = getData.htService_code[codeid].ToString();
                        }
                        else
                        {
                            dr["Fcodename"] = "";
                        }
                    }

                    return ds;
                }
                catch
                {
                    return null;
                }
                finally
                {
                    da.Dispose();
                }

            }
            catch (LogicException err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                throw;
            }
            catch (Exception err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                throw new LogicException("Service处理失败！(" + err.Message + ")");
            }
            finally
            {
                rl.WriteLog();
            }
        }


        [WebMethod(Description = "银行账户限额明细查询函数")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet GetDKLimit_Detail(string banktype, string bankaccno, string servicecode)
        {
            RightAndLog rl = new RightAndLog();
            try
            {
                if (myHeader == null)
                {
                    throw new LogicException("不正确的调用方法！");
                }

                rl.actionType = "银行账户限额明细查询函数";
                rl.ID = bankaccno;
                rl.OperID = myHeader.OperID;
                rl.sign = 1;
                rl.strRightCode = "";
                rl.RightString = myHeader.RightString;
                rl.SzKey = myHeader.SzKey;
                rl.type = "查询";
                rl.UserID = myHeader.UserName;
                rl.UserIP = myHeader.UserIP;
  
                DataSet ds = new DataSet();

                MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("INC_NEW"));
                try
                {
                    da.OpenConn();

                    string roleid = banktype + bankaccno + "@cep.tenpay.com";
                    roleid = PublicRes.ConvertToFuid(roleid);

                    string strSql = " select Fservice_code,'" + banktype + "' as Fbanktype,'" + bankaccno + "' as Fbankaccno, ";

                    strSql += "sum(case Fdimension when 1 then Fonce_data else 0 end)/100 as Fonce_data,";//单笔限额

                    strSql += "sum(case Fdimension when 1 then Fday_data else 0 end)/100 as Fday_sum_data,";//单日限额
                    strSql += "sum(case Fdimension when 3 then Fday_data else 0 end)/100 as Fday_use_data,";//单日累计使用限额

                    strSql += "sum(case Fdimension when 1 then Fweek_data else 0 end)/100 as Fweek_sum_data,";//单周限额
                    strSql += "sum(case Fdimension when 3 then Fweek_data else 0 end)/100 as Fweek_use_data,";//单周累计使用限额

                    strSql += "sum(case Fdimension when 1 then Fmounth_data else 0 end)/100 as Fmonth_sum_data,";//单月限额
                    strSql += "sum(case Fdimension when 3 then Fmounth_data else 0 end)/100 as Fmonth_use_data,";//单月累计使用限额

                    strSql += "sum(case Fdimension when 1 then Fquarter_data else 0 end)/100 as Fquarter_sum_data,";//单季限额
                    strSql += "sum(case Fdimension when 3 then Fquarter_data else 0 end)/100 as Fquarter_use_data,";//单季累计使用限额

                    strSql += "sum(case Fdimension when 1 then Fyear_data else 0 end)/100 as Fyear_sum_data,";//单年限额
                    strSql += "sum(case Fdimension when 3 then Fyear_data else 0 end)/100 as Fyear_use_data,";//单年累计使用限额

                    strSql += "sum(case Fdimension when 2 then Fday_data else 0 end) as Fday_sum_count,";//单日限次
                    strSql += "sum(case Fdimension when 4 then Fday_data else 0 end) as Fday_use_count,";//单日累计使用次数

                    strSql += "sum(case Fdimension when 2 then Fweek_data else 0 end) as Fweek_sum_count,";//单周限次
                    strSql += "sum(case Fdimension when 4 then Fweek_data else 0 end) as Fweek_use_count,";//单周累计使用次数

                    strSql += "sum(case Fdimension when 2 then Fmounth_data else 0 end) as Fmonth_sum_count,";//单月限次
                    strSql += "sum(case Fdimension when 4 then Fmounth_data else 0 end) as Fmonth_use_count,";//单月累计使用次数

                    strSql += "sum(case Fdimension when 2 then Fquarter_data else 0 end) as Fquarter_sum_count,";//单季限次
                    strSql += "sum(case Fdimension when 4 then Fquarter_data else 0 end) as Fquarter_use_count,";//单季累计使用次数

                    strSql += "sum(case Fdimension when 2 then Fyear_data else 0 end) as Fyear_sum_count,";//单年限次
                    strSql += "sum(case Fdimension when 4 then Fyear_data else 0 end) as Fyear_use_count ";//单年累计使用次数

                    strSql += " from " + PublicRes.GetTName("cft_cep_db", "t_service_limit", roleid) + " where Frole_id='" + roleid + "' and Fflag=1 and Fservice_code='" + servicecode + "' group by Fservice_code ";

                    ds = da.dsGetTotalData(strSql);

                    if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                        return null;

                    ds.Tables[0].Columns.Add("Fspid", typeof(string));
                    ds.Tables[0].Columns.Add("Fspname", typeof(string));
                    ds.Tables[0].Columns.Add("Fcodeid", typeof(string));
                    ds.Tables[0].Columns.Add("Fcodename", typeof(string));

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        string tmp = dr["Fservice_code"].ToString();
                        string spid = tmp.Substring(0, 10);
                        dr["Fspid"] = spid;

                        string msg = "";
                        dr["Fspname"] = CommQuery.GetOneResultFromICE("spid=" + spid, CommQuery.QUERY_MERCHANTINFO, "FName", out msg);
                        string codeid = dr["Fservice_code"].ToString().Replace(spid, "");
                        dr["Fcodeid"] = codeid;

                        if (getData.htService_code.Contains(codeid))
                        {
                            dr["Fcodename"] = getData.htService_code[codeid].ToString();
                        }
                        else
                        {
                            dr["Fcodename"] = "";
                        }
                    }

                    return ds;
                }
                catch
                {
                    return null;
                }
                finally
                {
                    da.Dispose();
                }

            }
            catch (LogicException err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                throw;
            }
            catch (Exception err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                throw new LogicException("Service处理失败！(" + err.Message + ")");
            }
            finally
            {
                rl.WriteLog();
            }
        }


        [WebMethod(Description = "商户特性批次查询函数")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet GetDKService_List(string bankacc_limit, string spid, string code, int limStart, int limMax)
        {
            RightAndLog rl = new RightAndLog();
            try
            {
                if (myHeader == null)
                {
                    throw new LogicException("不正确的调用方法！");
                }

                rl.actionType = "商户特性批次查询函数";
                rl.ID = spid;
                rl.OperID = myHeader.OperID;
                rl.sign = 1;
                rl.strRightCode = "";
                rl.RightString = myHeader.RightString;
                rl.SzKey = myHeader.SzKey;
                rl.type = "查询";
                rl.UserID = myHeader.UserName;
                rl.UserIP = myHeader.UserIP;

                DataSet ds = new DataSet();

                MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("INC_NEW"));
                try
                {
                    da.OpenConn();

                    string strSql = " select Fservice_code, Fbankacc_limit, ";
                    strSql += "sum(case Fdimension when 1 then Fonce_data else 0 end)/100 as Fonce_data,";//单笔限额

                    strSql += "sum(case Fdimension when 1 then Fday_data else 0 end)/100 as Fday_sum_data,";//单日限额
                    strSql += "sum(case Fdimension when 3 then Fday_data else 0 end)/100 as Fday_use_data,";//单日累计使用限额

                    strSql += "sum(case Fdimension when 1 then Fweek_data else 0 end)/100 as Fweek_sum_data,";//单周限额
                    strSql += "sum(case Fdimension when 3 then Fweek_data else 0 end)/100 as Fweek_use_data,";//单周累计使用限额

                    strSql += "sum(case Fdimension when 1 then Fmounth_data else 0 end)/100 as Fmonth_sum_data,";//单月限额
                    strSql += "sum(case Fdimension when 3 then Fmounth_data else 0 end)/100 as Fmonth_use_data,";//单月累计使用限额

                    strSql += "sum(case Fdimension when 1 then Fquarter_data else 0 end)/100 as Fquarter_sum_data,";//单季限额
                    strSql += "sum(case Fdimension when 3 then Fquarter_data else 0 end)/100 as Fquarter_use_data,";//单季累计使用限额

                    strSql += "sum(case Fdimension when 1 then Fyear_data else 0 end)/100 as Fyear_sum_data,";//单年限额
                    strSql += "sum(case Fdimension when 3 then Fyear_data else 0 end)/100 as Fyear_use_data,";//单年累计使用限额

                    strSql += "sum(case Fdimension when 2 then Fday_data else 0 end) as Fday_sum_count,";//单日限次
                    strSql += "sum(case Fdimension when 4 then Fday_data else 0 end) as Fday_use_count,";//单日累计使用次数

                    strSql += "sum(case Fdimension when 2 then Fweek_data else 0 end) as Fweek_sum_count,";//单周限次
                    strSql += "sum(case Fdimension when 4 then Fweek_data else 0 end) as Fweek_use_count,";//单周累计使用次数

                    strSql += "sum(case Fdimension when 2 then Fmounth_data else 0 end) as Fmonth_sum_count,";//单月限次
                    strSql += "sum(case Fdimension when 4 then Fmounth_data else 0 end) as Fmonth_use_count,";//单月累计使用次数

                    strSql += "sum(case Fdimension when 2 then Fquarter_data else 0 end) as Fquarter_sum_count,";//单季限次
                    strSql += "sum(case Fdimension when 4 then Fquarter_data else 0 end) as Fquarter_use_count,";//单季累计使用次数

                    strSql += "sum(case Fdimension when 2 then Fyear_data else 0 end) as Fyear_sum_count,";//单年限次
                    strSql += "sum(case Fdimension when 4 then Fyear_data else 0 end) as Fyear_use_count ";//单年累计使用次数


                    strSql += " from " + PublicRes.GetTName("cft_cep_db", "t_service_limit", spid) + " where Fflag=1 ";
                    if (spid != "" && spid != null)
                    {
                        strSql += " and Frole_id='" + spid + "' ";
                    }

                    if (code != "9999999")
                    {
                        strSql += " and Fservice_code='" + spid + code + "' ";
                    }

                    if (bankacc_limit.Trim() != "" && bankacc_limit != null)
                    {
                        strSql += " and Fbankacc_limit='" + bankacc_limit + "' ";
                    }

                    strSql += "group by Fservice_code ";

                    ds = da.dsGetTableByRange(strSql, limStart, limMax);

                    if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                        return null;

                    ds.Tables[0].Columns.Add("Fspid", typeof(string));
                    ds.Tables[0].Columns.Add("Fspname", typeof(string));
                    ds.Tables[0].Columns.Add("Fcodeid", typeof(string));
                    ds.Tables[0].Columns.Add("Fcodename", typeof(string));

                    ds.Tables[0].Columns.Add("FlstateName", typeof(string));

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        string servicecode = dr["Fservice_code"].ToString();
                        dr["Fspid"] = spid;

                        string msg = "";
                        dr["Fspname"] = CommQuery.GetOneResultFromICE("spid=" + spid, CommQuery.QUERY_MERCHANTINFO, "FName", out msg);
                        string codeid = servicecode.Replace(spid, "");
                        dr["Fcodeid"] = codeid;

                        if (getData.htService_code.Contains(codeid))
                        {
                            dr["Fcodename"] = getData.htService_code[codeid].ToString();
                        }
                        else
                        {
                            dr["Fcodename"] = "";
                        }

                        strSql = "select Flstate from cft_cep_db.t_sp_service where Fservice_code='" + servicecode + "'";
                        string lstate = da.GetOneResult(strSql);

                        if (lstate == null || lstate == "")
                        {
                            dr["FlstateName"] = "";
                        }
                        else
                        {
                            if (lstate == "1")
                            {
                                dr["FlstateName"] = "有效";
                            }
                            else if (lstate == "2")
                            {
                                dr["FlstateName"] = "无效";
                            }
                            else
                            {
                                dr["FlstateName"] = "冻结";
                            }
                        }
                    }

                    return ds;
                }
                catch (Exception err)
                {
                    throw new Exception(err.Message);
                }
                finally
                {
                    da.Dispose();
                }

            }
            catch (LogicException err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                throw new LogicException(err.Message);
            }
            catch (Exception err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                throw new LogicException("Service处理失败！(" + err.Message + ")");
            }
            finally
            {
                rl.WriteLog();
            }
        }


        [WebMethod(Description = "商户特性单笔9999查询函数")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet GetDKService_Detail9999(string spid)
        {
            RightAndLog rl = new RightAndLog();
            try
            {
                if (myHeader == null)
                {
                    throw new LogicException("不正确的调用方法！");
                }

                rl.actionType = "商户特性单笔9999查询函数";
                rl.ID = spid;
                rl.OperID = myHeader.OperID;
                rl.sign = 1;
                rl.strRightCode = "";
                rl.RightString = myHeader.RightString;
                rl.SzKey = myHeader.SzKey;
                rl.type = "查询";
                rl.UserID = myHeader.UserName;
                rl.UserIP = myHeader.UserIP;
   
                DataSet ds = new DataSet();

                MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("INC_NEW"));
                try
                {
                    da.OpenConn();


                    string strSql = " select Fservice_code, ";

                    strSql += "sum(case Fdimension when 5 then Fonce_data else 0 end)/100 as Fonce_data,";//单笔限额

                    strSql += "sum(case Fdimension when 5 then Fday_data else 0 end)/100 as Fday_sum_data,";//单日限额
                    strSql += "sum(case Fdimension when 7 then Fday_data else 0 end)/100 as Fday_use_data,";//单日累计使用限额

                    strSql += "sum(case Fdimension when 5 then Fweek_data else 0 end)/100 as Fweek_sum_data,";//单周限额
                    strSql += "sum(case Fdimension when 7 then Fweek_data else 0 end)/100 as Fweek_use_data,";//单周累计使用限额

                    strSql += "sum(case Fdimension when 5 then Fmounth_data else 0 end)/100 as Fmonth_sum_data,";//单月限额
                    strSql += "sum(case Fdimension when 7 then Fmounth_data else 0 end)/100 as Fmonth_use_data,";//单月累计使用限额

                    strSql += "sum(case Fdimension when 5 then Fquarter_data else 0 end)/100 as Fquarter_sum_data,";//单季限额
                    strSql += "sum(case Fdimension when 7 then Fquarter_data else 0 end)/100 as Fquarter_use_data,";//单季累计使用限额

                    strSql += "sum(case Fdimension when 5 then Fyear_data else 0 end)/100 as Fyear_sum_data,";//单年限额
                    strSql += "sum(case Fdimension when 7 then Fyear_data else 0 end)/100 as Fyear_use_data,";//单年累计使用限额

                    strSql += "sum(case Fdimension when 6 then Fday_data else 0 end) as Fday_sum_count,";//单日限次
                    strSql += "sum(case Fdimension when 8 then Fday_data else 0 end) as Fday_use_count,";//单日累计使用次数

                    strSql += "sum(case Fdimension when 6 then Fweek_data else 0 end) as Fweek_sum_count,";//单周限次
                    strSql += "sum(case Fdimension when 8 then Fweek_data else 0 end) as Fweek_use_count,";//单周累计使用次数

                    strSql += "sum(case Fdimension when 6 then Fmounth_data else 0 end) as Fmonth_sum_count,";//单月限次
                    strSql += "sum(case Fdimension when 8 then Fmounth_data else 0 end) as Fmonth_use_count,";//单月累计使用次数

                    strSql += "sum(case Fdimension when 6 then Fquarter_data else 0 end) as Fquarter_sum_count,";//单季限次
                    strSql += "sum(case Fdimension when 8 then Fquarter_data else 0 end) as Fquarter_use_count,";//单季累计使用次数

                    strSql += "sum(case Fdimension when 6 then Fyear_data else 0 end) as Fyear_sum_count,";//单年限次
                    strSql += "sum(case Fdimension when 8 then Fyear_data else 0 end) as Fyear_use_count ";//单年累计使用次数

                    strSql += " from " + PublicRes.GetTName("cft_cep_db", "t_service_limit", spid) + " where Frole_id='" + spid + "' and Fflag=1 ";

                    strSql += " and Fservice_code='" + spid + "0000000' ";

                    strSql += "group by Fservice_code ";

                    ds = da.dsGetTotalData(strSql);

                    if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                        return null;

                    return ds;
                }
                catch
                {
                    return null;
                }
                finally
                {
                    da.Dispose();
                }

            }
            catch (LogicException err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                throw;
            }
            catch (Exception err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                throw new LogicException("Service处理失败！(" + err.Message + ")");
            }
            finally
            {
                rl.WriteLog();
            }
        }



        [WebMethod(Description = "商户特性单笔查询函数")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet GetDKService_SPServiceDetail(string servicecode)
        {
            RightAndLog rl = new RightAndLog();
            try
            {
                if (myHeader == null)
                {
                    throw new LogicException("不正确的调用方法！");
                }

                rl.actionType = "商户特性单笔查询函数";
                rl.ID = servicecode;
                rl.OperID = myHeader.OperID;
                rl.sign = 1;
                rl.strRightCode = "";
                rl.RightString = myHeader.RightString;
                rl.SzKey = myHeader.SzKey;
                rl.type = "查询";
                rl.UserID = myHeader.UserName;
                rl.UserIP = myHeader.UserIP;

                DataSet ds = new DataSet();

                MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("INC_NEW"));
                try
                {
                    da.OpenConn();


                    string strSql = " select * from  cft_cep_db.t_sp_service where Fservice_code='" + servicecode + "'";

                    ds = da.dsGetTotalData(strSql);

                    if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                        return null;

                    return ds;
                }
                catch
                {
                    return null;
                }
                finally
                {
                    da.Dispose();
                }

            }
            catch (LogicException err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                throw;
            }
            catch (Exception err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                throw new LogicException("Service处理失败！(" + err.Message + ")");
            }
            finally
            {
                rl.WriteLog();
            }
        }


    
        [WebMethod(Description = "财付通会员账号高级信息查询")]
        public DataSet QueryCFTMemberAdvanced(string account)
        {
            string[] dbInfo = GetDbInfo(account);
            string strSql = string.Format("select * from c2c_db_{0}.t_vipuser_info_{1}  where Fuin='{2}'", dbInfo[0], dbInfo[1], account);
            DataSet ds = new DataSet();
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("QueryMember" + dbInfo[2]));
            try
            {
                da.OpenConn();
                ds = da.dsGetTotalData(strSql);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    return null;
                return ds;
            }
            catch (Exception e)
            {
                throw new Exception("service发生错误,请联系管理员！" + e.Message);
            }
        }


        [WebMethod(Description = "财付值流水查询")]
        public DataSet QueryTurnover(string account, string order, string begin, string end)
        {
            string[] dbInfo = GetDbInfo(account);
            string strSql = string.Format(@"select * from cft_vip_acc_{0}.t_vipuser_acc_{1}  where Fuin='{2}' and 
							FCommit_time between '{3}' and '{4}'", dbInfo[0], dbInfo[1], account, begin, end);
            if (order != null && order != "")
            {
                strSql = string.Format("{0} and FOrig_req like '%{1}%'", strSql, order);
            }
            strSql += " order by FCommit_time";
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("PropertyTurnover"));//财付值交易流水
            DataSet ds = new DataSet();
            try
            {
                da.OpenConn();
                ds = da.dsGetTotalData(strSql);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    return null;
                return ds;
            }
            catch (Exception e)
            {
                throw new Exception("service发生错误,请联系管理员！" + e.Message);
            }
        }


        /// <summary>
        /// 数组第一位是数据库，第二位是表，第三位是主机
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public string[] GetDbInfo(string account)
        {
            string[] dbInfo = new string[3] { "00", "0", "0" };
            if (account.Length < 5)
            {
                return dbInfo;
            }
            uint qq;

            try//如果是qq号
            {
                qq = uint.Parse(account);
                string dbNum = account.Substring(account.Length - 2, 2);
                string tableNum = account.Substring(account.Length - 3, 1);
                string hostNum = Convert.ToString(qq % 1367 % 3);
                dbInfo[0] = dbNum;
                dbInfo[1] = tableNum;
                dbInfo[2] = hostNum;
            }
            catch
            {
                //前两位决定数据库
                int iDb = CharHash(account.Substring(0, 1)) * 10 + CharHash(account.Substring(1, 1));
                iDb = Math.Abs(iDb);

                //第三位决定表
                int iTb = CharHash(account.Substring(2, 1));
                iTb = Math.Abs(iTb);

                //4,5位决定主机
                int iHost = CharHash(account.Substring(3, 1)) * 10 + CharHash(account.Substring(4, 1));
                iHost = Math.Abs(iHost);
                iHost = iDb + iTb * 100 + iHost * 1000;
                iHost = iHost % 1367 % 3;
                dbInfo[0] = iDb.ToString("00");
                dbInfo[1] = iTb.ToString();
                dbInfo[2] = iHost.ToString();
            }
            return dbInfo;
        }


        public int CharHash(string c)
        {
            return (Convert.ToInt32(c[0]) - Convert.ToInt32('0')) % 10;
        }


        [WebMethod(Description = "财付通会员银行卡绑定信息查询")]
        public DataSet QueryBankCardBind(string account)
        {
            int length = account.Length;
            string dbIndex = account.Substring(length - 3, 1);
            string tblIndex = account.Substring(length - 2, 2);
            string strSql = string.Format(@"select * from db_vipbind_{0}.t_vipbind_info_{1} where Fuin = '{2}'", dbIndex, tblIndex, account);
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("CardBind"));
            DataSet ds = new DataSet();
            try
            {
                da.OpenConn();
                ds = da.dsGetTotalData(strSql);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    return null;
                return ds;
            }
            catch (Exception e)
            {
                throw new Exception("service发生错误,请联系管理员！" + e.Message);
            }
            finally
            {
                da.Dispose();
            }
        }


        [WebMethod(Description = "财付通会员银行卡交易信息查询")]
        public DataSet QueryBankCardTransaction(string account)
        {
            int length = account.Length;
            string dbIndex = account.Substring(length - 3, 1);
            string tblIndex = account.Substring(length - 2, 2);
            string strSql = string.Format(@"select * from db_vipbind_card_{0}.t_vip_trans_{1} where Fpayuin = '{2}'", dbIndex, tblIndex, account);
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("CardBind"));
            DataSet ds = new DataSet();
            try
            {
                da.OpenConn();
                ds = da.dsGetTotalData(strSql);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    return null;
                return ds;
            }
            catch (Exception e)
            {
                throw new Exception("service发生错误,请联系管理员！" + e.Message);
            }
        }


        /// <summary>
        /// 按银行卡号后3位分库分表
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <returns></returns>
        [WebMethod(Description = "财付通会员银行卡信息查询")]
        public DataSet QueryBankCardInfo(string cardNumber)
        {
            int length = cardNumber.Length;
            string dbIndex = cardNumber.Substring(length - 3, 1);
            string tblIndex = cardNumber.Substring(length - 2, 2);
            string strSql = string.Format(@"select * from db_vipcard_{0}.t_bank_info_{1} where Fbank_id = '{2}'", dbIndex, tblIndex, cardNumber);
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("CardBind"));
            DataSet ds = new DataSet();
            try
            {
                da.OpenConn();
                ds = da.dsGetTotalData(strSql);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    return null;
                ds.Tables[0].TableName = cardNumber + DateTime.Now.Millisecond;
                return ds;
            }
            catch (Exception e)
            {
                throw new Exception("service发生错误,请联系管理员！" + e.Message);
            }
        }


        [WebMethod(Description = "财付通外卡支付查询，按订单号")]
        public DataSet QueryForeignCardInfoByOrder(string order, string condition)
        {
            int length = order.Length;
            string dataTableIndex = order.Substring(length - 3, 1);
            string dataBaseIndex = order.Substring(length - 2, 2);
            string strSql = string.Format(@"select * from c2c_db_outpay_{0}.t_order_{1} where {2}", dataBaseIndex, dataTableIndex, condition);
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("ForeignCard"));
            DataSet ds = new DataSet();
            try
            {
                da.OpenConn();
                ds = da.dsGetTotalData(strSql);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    return null;
                ds.Tables[0].TableName = order;
                return ds;
            }
            catch (Exception e)
            {
                throw new Exception("service发生错误,请联系管理员！" + e.Message);
            }
        }

        [WebMethod(Description = "财付通外卡支付查询，按商户号")]
        public DataSet QueryForeignCardInfoByMerchant(string merchant, string condition)
        {

            string strSql = "spid=" + merchant;
            string errMsg = "";
            string f_strID = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_MERCHANTINFO, "FuidMiddle", out errMsg);

            if (f_strID == null || f_strID.Trim() == "")
            {
                return null;
            }

            int length = f_strID.Length;
            string dataTableIndex = f_strID.Substring(length - 3, 1);
            string dataBaseIndex = f_strID.Substring(length - 2, 2);
            strSql = string.Format(@"select * from c2c_db_outpay_{0}.t_user_order_{1} where {2}", dataBaseIndex, dataTableIndex, condition);
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("ForeignCard"));
            DataSet ds = new DataSet();
            try
            {
                da.OpenConn();
                ds = da.dsGetTotalData(strSql);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    return null;
                ds.Tables[0].TableName = merchant;
                return ds;
            }
            catch (Exception e)
            {
                throw new Exception("service发生错误,请联系管理员！" + e.Message);
            }
        }


        [WebMethod(Description = "财付通外卡支付查询，按银行订单号")]
        public DataSet QueryForeignCardInfoByBankOrder(string year, string condition)
        {
            string strSql = string.Format(@"select * from c2c_db_outpay.t_pay_list_{0} where {1}", year, condition);
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("ForeignCard"));
            DataSet ds = new DataSet();
            try
            {
                da.OpenConn();
                ds = da.dsGetTotalData(strSql);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    return null;
                ds.Tables[0].TableName = year;
                string order = ds.Tables[0].Rows[0]["Ftransaction_id"].ToString();

                return QueryForeignCardInfoByOrder(order, " Ftransaction_id = '" + order + "'");
            }
            catch (Exception e)
            {
                throw new Exception("service发生错误,请联系管理员！" + e.Message);
            }
        }


        [WebMethod(Description = "查询用户余额")]
        public long GetUserBalance(string qqid, int curtype, out string Msg)
        {
            Msg = "";

            ICEAccess ice = new ICEAccess(PublicRes.ICEServerIP, PublicRes.ICEPort);
            try
            {
                string fuid = PublicRes.ConvertToFuid(qqid);

                string strwhere = "where=" + ICEAccess.URLEncode("fuid=" + fuid + "&");
                strwhere += ICEAccess.URLEncode("fcurtype=" + curtype + "&");

                string strResp = "";

                ice.OpenConn();

                DataTable dtuser = ice.InvokeQuery_GetDataTable(YWSourceType.用户资源, YWCommandCode.查询用户信息, fuid, strwhere, out strResp);

                if (dtuser == null || dtuser.Rows.Count == 0)
                {
                    Msg = "查询不到用户信息";
                    return -1;
                }

                long lbalance = long.Parse(dtuser.Rows[0]["fbalance"].ToString());

                ice.CloseConn();

                return lbalance;
            }
            catch (Exception ex)
            {
                Msg = ex.Message;
                return -1;
            }
            finally
            {
                ice.Dispose();
            }

        }

        [WebMethod(Description = "通过uid查询用户余额")]
        public long GetUserBalanceByUId(string fuid, int curtype, out string Msg)
        {
            Msg = "";

            ICEAccess ice = new ICEAccess(PublicRes.ICEServerIP, PublicRes.ICEPort);
            try
            {

                string strwhere = "where=" + ICEAccess.URLEncode("fuid=" + fuid + "&");
                strwhere += ICEAccess.URLEncode("fcurtype=" + curtype + "&");

                string strResp = "";

                ice.OpenConn();

                DataTable dtuser = ice.InvokeQuery_GetDataTable(YWSourceType.用户资源, YWCommandCode.查询用户信息, fuid, strwhere, out strResp);

                if (dtuser == null || dtuser.Rows.Count == 0)
                {
                    Msg = "查询不到用户信息";
                    return -1;
                }

                long lbalance = long.Parse(dtuser.Rows[0]["fbalance"].ToString());

                ice.CloseConn();

                return lbalance;
            }
            catch (Exception ex)
            {
                Msg = ex.Message;
                return -1;
            }
            finally
            {
                ice.Dispose();
            }

        }


        [WebMethod(Description = "查询用户手机充值记录")]
        public DataSet QueryMobilRecharge(string account, string begin, string end, int index)
        {
            index -= 1;
            int page = 50;
            string strSql = string.Format(@"select * from charge_card_db.t_card_list where Fuin = '{0}' and Fpay_time between '{1}' and '{2}'
											limit {3},{4}", account, begin, end, page * index, page);
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("MobileRecharge"));
            DataSet ds = new DataSet();
            try
            {
                da.OpenConn();
                ds = da.dsGetTotalData(strSql);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    return null;
                ds.Tables[0].TableName = account;
                return ds;
            }
            catch (Exception e)
            {
                throw new Exception("service发生错误,请联系管理员！");
                return null;
            }
        }

        [WebMethod(Description = "查询手机充值记录单")]
        public DataSet QueryMobilRechargeOrder(string order)
        {
            string strSql = string.Format(@"select * from charge_card_db.t_card_list where Flistid = '{0}'", order);
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("MobileRecharge"));
            DataSet ds = new DataSet();
            try
            {
                da.OpenConn();
                ds = da.dsGetTotalData(strSql);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    return null;
                ds.Tables[0].TableName = order;
                ds.Tables[0].Columns.Add("Provider", typeof(string));
                string providerID = ds.Tables[0].Rows[0]["Fsupply_id"].ToString().Trim();
                if (providerID != string.Empty)
                {
                    string sql = "select * from charge_sp_db.t_sp_info where Fsupply_id ='" + providerID + "'";
                    DataSet dsP = new DataSet();
                    dsP = da.dsGetTotalData(sql);
                    if (dsP == null || dsP.Tables.Count == 0 || dsP.Tables[0].Rows.Count == 0)
                    {
                        ds.Tables[0].Rows[0]["Provider"] = string.Empty;
                    }
                    else
                    {
                        ds.Tables[0].Rows[0]["Provider"] = dsP.Tables[0].Rows[0]["Fsupply_name"];
                    }
                }
                return ds;
            }
            catch (Exception e)
            {
                throw new Exception("service发生错误,请联系管理员！");
                return null;
            }
        }


        [WebMethod(Description = "查询手机充值商户")]
        public DataSet QuerySupplierValidDate()
        {
            string strSql = "select * from charge_sp_db.t_sp_rate where FState = 1";
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("MobileRecharge"));
            DataSet ds = new DataSet();
            try
            {
                da.OpenConn();
                return da.dsGetTotalData(strSql);
            }
            catch (Exception e)
            {
                throw new Exception("service发生错误,请联系管理员！");
                return null;
            }
        }

        [WebMethod(Description = "查询手机充值商户名称")]
        public DataSet QuerySupplierName()
        {
            string strSql = "select distinct Fsupply_id,Fsupply_name from charge_sp_db.t_sp_info where FState = 1";
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("MobileRecharge"));
            DataSet ds = new DataSet();
            try
            {
                da.OpenConn();
                return da.dsGetTotalData(strSql);
            }
            catch (Exception e)
            {
                throw new Exception("service发生错误,请联系管理员！");
                return null;
            }
        }

        [WebMethod(Description = "更新手机充值商户有效期")]
        public bool UpdateSupplierVaildDate(string supplierID, DateTime start, DateTime end)
        {
            string str = String.Format("update charge_sp_db.t_sp_rate set Fstart_time = '{0}', Fend_time = '{1}' where Fsupplier_id = '{2}'", start, end, supplierID);
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("MobileRecharge"));
            try
            {
                da.OpenConn();
                da.ExecSql(str);
                return true;
            }
            catch (Exception e)
            {
                throw new Exception("service发生错误,请联系管理员！");
                return false;
            }
        }

        [WebMethod(Description = "查询手机充值产品")]
        public DataSet QueryMobilProduct()
        {
            string str = string.Format(@"select Fcard_type, Fcard_value, Fmin_ref, Fmax_ref, Fsupplier_id from charge_sp_db.t_product_info 
						  where FState = 1 and Fstart_time < '{0}' and Fend_time > '{1}' order by Fcard_type, Fcard_value", DateTime.Now, DateTime.Now);
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("MobileRecharge"));
            try
            {
                da.OpenConn();
                da.ExecSql(str);
                return da.dsGetTotalData(str);
            }
            catch (Exception e)
            {
                throw new Exception("service发生错误,请联系管理员！");
                return null;
            }
        }

        [WebMethod(Description = "查询单一手机充值产品")]
        public DataSet QueryOneMobilProduct(string cardType, string cardValue)
        {
            string str = string.Format(@"select Fcard_type, Fcard_value, Fmin_ref, Fmax_ref, Fsupplier_id from charge_sp_db.t_product_info 
						  where FState = 1 and Fstart_time < '{0}' and Fend_time > '{1}' and Fcard_type = {2} and Fcard_value = {3}",
                          DateTime.Now, DateTime.Now, cardType, cardValue);
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("MobileRecharge"));
            try
            {
                da.OpenConn();
                da.ExecSql(str);
                return da.dsGetTotalData(str);
            }
            catch (Exception e)
            {
                throw new Exception("service发生错误,请联系管理员！");
                return null;
            }
        }

        [WebMethod(Description = "更新手机充值产品")]
        public bool UpdateMobileProduct(string cardType, string cardValue, string min, string max, string supplierID)
        {
            string str = String.Format(@"insert charge_sp_db.t_product_info  (Fcard_type, Fcard_value, Fmin_ref, Fmax_ref,Fsupplier_id, 
			Fstart_time,Fend_Time,Fcreate_time,Fmodify_time,Fstate) values ({0},{1},{2},{3},{4},'{5}','{6}','{7}','{8}',1)", cardType, cardValue,
                min, max, supplierID, DateTime.Now, DateTime.Now.AddYears(3), DateTime.Now, DateTime.Now);
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("MobileRecharge"));
            try
            {
                da.OpenConn();
                da.ExecSql(str);
                return true;
            }
            catch (Exception e)
            {
                throw new Exception("service发生错误,请联系管理员！");
                return false;
            }
        }

        [WebMethod(Description = "删除手机充值产品")]
        public bool DeleteMobileProduct(string cardType, string cardValue)
        {
            string str = String.Format(@"delete from charge_sp_db.t_product_info where Fcard_type = {0} and Fcard_value = {1}", cardType, cardValue);
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("MobileRecharge"));
            try
            {
                da.OpenConn();
                da.ExecSql(str);
                return true;
            }
            catch (Exception e)
            {
                throw new Exception("service发生错误,请联系管理员！");
                return false;
            }
        }


        #region 批量调整代扣状态前对明细打标记。
        [WebMethod]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public bool DK_BatchSelect(ArrayList al, string UserName, out string batchid, out string msg)
        {
            msg = "";
            batchid = "";

            if (al == null || al.Count == 0)
            {
                msg = "请给出正确的交易单ID";
                return false;
            }

            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("INC_NEW"));
            try
            {
                da.OpenConn();
                batchid = DateTime.Now.ToString("yyyyMMddHHmmss001");

                string idlist = "(";
                foreach (object obj in al)
                {
                    string cepid = obj.ToString();
                    idlist += "'" + cepid + "',";
                }

                idlist = idlist.Substring(0, idlist.Length - 1) + ")";

                string strSql = "select * from cft_cep_db.t_cep_list where Fcep_id in " + idlist + " and Fstate=6 and Flstate=1";
                DataTable dt = da.GetTable(strSql);

                if (dt == null || dt.Rows.Count == 0)
                {
                    msg = "找不到正确的明细数据";
                    return false;
                }

                //开始入库。
                foreach (DataRow dr in dt.Rows)
                {
                    string Fcep_id = dr["Fcep_id"].ToString();
                    string Funame = dr["Funame"].ToString();
                    string Fbankacc_no = dr["Fbankacc_no"].ToString();
                    string Famount = dr["Famount"].ToString();
                    string Fpaynum = dr["Fpaynum"].ToString();


                    string Ftransaction_id = dr["Ftransaction_id"].ToString();
                    string Fcoding = dr["Fcoding"].ToString();
                    string Fspid = dr["Fspid"].ToString();
                    string Fbank_list = dr["Fbank_list"].ToString();
                    string Fbank_type = dr["Fbank_type"].ToString();

                    strSql = "select count(*) from cft_cep_db.t_cep_adjust where Fcep_id='" + Fcep_id + "' and Fcheckstate in (1,2)"; //0初始标记　１已发起审批　２审批成功并执行　３撤消审批（允许再发起）
                    if (da.GetOneResult(strSql) == "1")
                        continue;

                    strSql = "delete from cft_cep_db.t_cep_adjust where Fcep_id='" + Fcep_id + "' and Fcheckstate=0 limit 1"; //删除以前中断操作，并未发起审批的标识
                    da.ExecSqlNum(strSql);

                    strSql = "insert cft_cep_db.t_cep_adjust(FCheckBatchID,FFromDB,FCep_ID,Fstartstate,Funame,Fbankacc_no,Famount,Fpaynum,Fcreate_time,Fcreate_user,Fcheckstate,Fmodify_time"
                        + ",Ftransaction_id,Fcoding,Fspid,Fbank_list,Fbank_type)"
                        + " values ('" + batchid + "','cft_cep_db.t_cep_list','" + Fcep_id + "',6,'" + Funame + "','" + Fbankacc_no + "'," + Famount + "," + Fpaynum + ",now(),'" + UserName + "',0,now()"
                        + ",'" + Ftransaction_id + "','" + Fcoding + "','" + Fspid + "','" + Fbank_list + "'," + Fbank_type + ")";
                    da.ExecSqlNum(strSql);
                }

                msg = "执行成功";
                return true;
            }
            catch (Exception err)
            {
                msg = "Service处理失败！" + err.Message;
                return false;
            }
        }

        [WebMethod]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public bool DK_FileSelect(ArrayList al, string UserName, out string batchid, out string msg)
        {
            msg = "";
            batchid = "";

            if (al == null || al.Count == 0)
            {
                msg = "请给出正确的文件";
                return false;
            }

            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("INC_NEW"));
            try
            {
                da.OpenConn();
                batchid = DateTime.Now.ToString("yyyyMMddHHmmss001");

                foreach (object obj in al)
                {
                    //交易单号;银行账号;金额(以分为单位);状态（现在只认失败）;失败原因
                    string strlen = obj.ToString();
                    string[] strs = strlen.Split(';');

                    if (strs.Length < 5)
                    {
                        msg = "文件明细有误" + strlen;
                        return false;
                    }

                    string Fcep_id = strs[0].Trim();
                    string Fbankacc_no = strs[1].Trim();
                    string Famount = strs[2].Trim();

                    if (strs[3].Trim() != "失败")
                    {
                        msg = "有非失败记录，现在暂不支持" + strlen;
                        return false;
                    }

                    //看数据库中有没有相应的记录，有的话进行插入操作。
                    //echo 20141020 加Fstate 4,5
                    string strSql = "select * from cft_cep_db.t_cep_list where Fcep_id='" + Fcep_id + "' and Fbankacc_no='" + Fbankacc_no + "' and ((Fpaynum=" + Famount + " and (Fstate=6 or Fstate=5 or Fstate=4)) or (Famount=" + Famount + " and (Fstate=6 or Fstate=5 or Fstate=4))) and Flstate=1";
                    DataTable dt = da.GetTable(strSql);

                    if (dt == null || dt.Rows.Count != 1)
                    {
                        continue;
                    }

                    DataRow dr = dt.Rows[0];
                    string Funame = dr["Funame"].ToString();
                    string Fpaynum = dr["Fpaynum"].ToString();
                    string Ftransaction_id = dr["Ftransaction_id"].ToString();
                    string Fcoding = dr["Fcoding"].ToString();
                    string Fspid = dr["Fspid"].ToString();
                    string Fbank_list = dr["Fbank_list"].ToString();
                    string Fbank_type = dr["Fbank_type"].ToString();

                    strSql = "select count(*) from cft_cep_db.t_cep_adjust where Fcep_id='" + Fcep_id + "' and Fcheckstate in (1,2)"; //0初始标记　１已发起审批　２审批成功并执行　３撤消审批（允许再发起）
                    if (da.GetOneResult(strSql) == "1")
                        continue;

                    strSql = "delete from cft_cep_db.t_cep_adjust where Fcep_id='" + Fcep_id + "' and Fcheckstate=0 limit 1"; //删除以前中断操作，并未发起审批的标识
                    da.ExecSqlNum(strSql);

                    strSql = "insert cft_cep_db.t_cep_adjust(FCheckBatchID,FFromDB,FCep_ID,Fstartstate,Funame,Fbankacc_no,Famount,Fpaynum,Fcreate_time,Fcreate_user,Fcheckstate,Fmodify_time"
                        + ",Ftransaction_id,Fcoding,Fspid,Fbank_list,Fbank_type)"
                        + " values ('" + batchid + "','cft_cep_db.t_cep_list','" + Fcep_id + "',6,'" + Funame + "','" + Fbankacc_no + "'," + Famount + "," + Fpaynum + ",now(),'" + UserName + "',0,now()"
                        + ",'" + Ftransaction_id + "','" + Fcoding + "','" + Fspid + "','" + Fbank_list + "'," + Fbank_type + ")";
                    da.ExecSqlNum(strSql);

                }

                msg = "执行成功";
                return true;
            }
            catch (Exception err)
            {
                msg = "Service处理失败！" + err.Message;
                return false;
            }
        }
        #endregion

        [WebMethod(Description = "查询代扣审批批次明细")]
        public DataSet DK_QueryCheckBatchAllDetail(string batchid)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("INC_NEW"));
            try
            {
                da.OpenConn();

                string strSql = "select * from cft_cep_db.t_cep_adjust where FCheckBatchID='" + batchid + "'";
                DataSet ds = da.dsGetTotalData(strSql);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null)
                {
                    ds.Tables[0].Columns.Add("FpaynumName");
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        dr.BeginEdit();
                        dr["FpaynumName"] = MoneyTransfer.FenToYuan(dr["Fpaynum"].ToString());
                        dr.EndEdit();
                    }

                    return ds;
                }
                else
                    return null;
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                da.Dispose();
            }
        }


        [WebMethod(Description = "查询代扣审批批次明细总金额")]
        public long DK_QueryCheckBatchSumAmount(string batchid)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("INC_NEW"));
            try
            {
                da.OpenConn();

                string strSql = "select sum(Fpaynum) from cft_cep_db.t_cep_adjust where FCheckBatchID='" + batchid + "'";
                return long.Parse(da.GetOneResult(strSql));
            }
            catch (Exception e)
            {
                return 0;
            }
            finally
            {
                da.Dispose();
            }
        }

        [WebMethod(Description = "查询代扣审批信息")]
        public DataSet DK_QueryCheckInfo(string batchid)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("ht"));
            try
            {
                da.OpenConn();

                string strSql = "select FKey,FValue from c2c_fmdb.t_check_param where FCheckID=(select FID from c2c_fmdb.t_check_main where "
                    + " Fobjid='" + batchid + "' and FCheckType='DKAdjustFail')";
                return da.dsGetTotalData(strSql);
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                da.Dispose();
            }
        }

        [WebMethod(Description = "查询代扣审批明细详细信息")]
        public DataSet DK_QueryCheckDetailInfo(string starttime, string endtime, string banktype, string spid, string coding, string bank_list, string bankaccno, string uname)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("INC_NEW"));
            try
            {
                string strSql = "select * from cft_cep_db.t_cep_adjust where Fcreate_time between '" + starttime + "' and '" + endtime + "' ";

                if (banktype != "0000")
                {
                    strSql += " and Fbank_type=" + banktype;
                }

                if (spid != "")
                {
                    strSql += " and Fspid='" + banktype + "' ";
                }

                if (coding != "")
                {
                    strSql += " and Fcoding='" + coding + "' ";
                }

                if (bank_list != "")
                {
                    strSql += " and Fbank_list='" + bank_list + "' ";
                }

                if (bankaccno != "")
                {
                    strSql += " and Fbankacc_no='" + bankaccno + "' ";
                }

                if (uname != "")
                {
                    strSql += " and Funame='" + uname + "' ";
                }

                da.OpenConn();

                DataSet ds = da.dsGetTotalData(strSql);

                return ds;
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                da.Dispose();
            }
        }


        [WebMethod(Description = "发起代扣未明状态审批")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public bool DK_StartCheck(string batchid, string imgurl, string checkurl, string checkmemo, out string msg)
        {
            msg = "";

            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("INC_NEW"));
            try
            {
                da.OpenConn();

                string sumpaynum = "0";//查询批次金额
                string strSql = "select sum(Fpaynum) from cft_cep_db.t_cep_adjust where Fcheckbatchid='" + batchid + "'";
                sumpaynum = MoneyTransfer.FenToYuan(da.GetOneResult(strSql));

                Check_Service cs = new Check_Service();
                cs.myHeader = myHeader;

                Param[] myparams = new Param[4];
                myparams[0] = new Param();
                myparams[0].ParamName = "batchid";
                myparams[0].ParamValue = batchid;

                myparams[1] = new Param();
                myparams[1].ParamName = "imgurl";
                myparams[1].ParamValue = imgurl;

                myparams[2] = new Param();
                myparams[2].ParamName = "checkmemo";
                myparams[2].ParamValue = checkmemo;

                myparams[3] = new Param();
                myparams[3].ParamName = "returnUrl";
                myparams[3].ParamValue = checkurl;

                cs.StartCheck(batchid, "DKAdjustFail", checkmemo, sumpaynum, myparams);

                msg = "发起审批成功";
                return true;
            }
            catch (Exception err)
            {
                msg = "Service处理失败！" + err.Message;
                return false;
            }
            finally
            {
                da.Dispose();
            }
        }

        #region 网银查询
        [WebMethod(Description = "当月流水或账单查询")]
        public DataSet InternetBankTurnoverBillQuery(string qq, string order, string trunoverID, string payAccount, DateTime beginDate, DateTime endDate, bool isBill)
        {
            MySqlAccess da = null;
            try
            {
                if (string.IsNullOrEmpty(qq) && string.IsNullOrEmpty(order) && string.IsNullOrEmpty(payAccount))
                {
                    throw new Exception("查询条件为空");
                }
                StringBuilder whereCondtion = new StringBuilder();
                whereCondtion.Append(" Fcreate_time between unix_timestamp('" + beginDate + "') and unix_timestamp('" + endDate + "') ");
                if (!string.IsNullOrEmpty(qq))
                {
                    whereCondtion.Append(" and Fuin='" + qq + "' ");
                }
                if (!string.IsNullOrEmpty(order))
                {
                    whereCondtion.Append(" and FSerial_no='" + order + "' ");
                }
                if (!string.IsNullOrEmpty(trunoverID))
                {
                    whereCondtion.Append(" and Fbank_water='" + trunoverID + "' ");
                }
                if (!string.IsNullOrEmpty(payAccount))
                {
                    whereCondtion.Append(" and Fpay_info='" + payAccount + "' ");
                }

                string strSql = string.Format(@"select Fpay_channel, Fservice_code, Fuin, (Fpay_amt/100) as Fpay_amt, (Fbank_amt/100) as Fbank_amt,
            Fstate, Fserial_no, Fbank_water, from_unixtime(Fcreate_time) as Fcreate_time,from_unixtime(Fchg_time) as Fchg_time, Fuser_ip, {0}, Fpay_info,
            {1} from bankck_base.{2} where {3}", isBill ? "Fportal_water" : "Fportal_waer", isBill ? "Fcomment, Fbillstate" : "Fcomment", isBill ? "t_bill_his" : "t_tran_his", whereCondtion.ToString());
                da = new MySqlAccess(PublicRes.GetConnString("INTERNETBANK"));
                da.OpenConn();
                var ds = da.dsGetTotalData(strSql);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    return null;
                return ds;
            }
            catch (Exception e)
            {
                throw new LogicException("service处理失败：" + e.Message);
            }
            finally
            {
                if (da != null)
                {
                    da.Dispose();
                }
            }
        }

        [WebMethod(Description = "从2012年1月到上月历史数据查询")]
        public DataSet InternetBankBillHistoryQuery(string qq, string order, string yearMonth)
        {
            try
            {
                if (string.IsNullOrEmpty(qq) && string.IsNullOrEmpty(order))
                {
                    throw new Exception("查询条件为空");
                }
                StringBuilder whereCondtion = new StringBuilder();
                whereCondtion.Append(" 1 = 1 ");
                if (!string.IsNullOrEmpty(qq))
                {
                    whereCondtion.Append(" and Fuin='" + qq + "' ");
                }
                if (!string.IsNullOrEmpty(order))
                {
                    whereCondtion.Append(" and FSerial_no='" + order + "' ");
                }

                string strSql = string.Format(@"select Fpay_channel, Fservice_code, Fuin, (Fpay_amt/100) as Fpay_amt, (Fbank_amt/100) as Fbank_amt,
            Fstate, Fserial_no, Fbank_water, from_unixtime(Fcreate_time) as Fcreate_time, Fuser_ip, Fportal_water, Fpay_info, Fbillstate, from_unixtime(Fcktime) as Fcktime, 
             from_unixtime(Fchg_time) as Fchg_time,Fcomment from bankck_base.t_bill_his_{0} where {1}", yearMonth, whereCondtion.ToString());
                MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("INTERNETBANK"));
                da.OpenConn();
                var ds = da.dsGetTotalData(strSql);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    return null;
                return ds;
            }
            catch (Exception e)
            {
                throw new LogicException("service处理失败：" + e.Message);
            }
        }
        #endregion


        [WebMethod(Description = "代付批量查询函数")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet QueryBatchInfo_DF(string strBeginDate, string strEndDate, string spid, string spBatchID, string state, int limStart, int limMax)
        {
            RightAndLog rl = new RightAndLog();
            try
            {
                if (myHeader == null)
                {
                    throw new LogicException("不正确的调用方法！");
                }

                rl.actionType = "代付批量查询函数";
                rl.ID = spid;
                rl.OperID = myHeader.OperID;
                rl.sign = 1;
                rl.strRightCode = "";
                rl.RightString = myHeader.RightString;
                rl.SzKey = myHeader.SzKey;
                rl.type = "查询";
                rl.UserID = myHeader.UserName;
                rl.UserIP = myHeader.UserIP;
                /*
                if(!rl.CheckRight())
                {
                    throw new LogicException("用户无权执行此操作！");
                }
                */
                string strWhere = " where Fcreate_time between '" + strBeginDate + "' and '" + strEndDate + "' and Ftype=2 ";
                if (spid.Trim() != "")
                {
                    strWhere += " and Fspid='" + spid + "' ";
                }

                if (spBatchID.Trim() != "")
                    strWhere += " and Fubatch_id='" + spBatchID + "' ";

                if (state != "0")
                    strWhere += " and Fstatus=" + state;

                string strSql = " select * from c2c_db_inc.t_spm_batopt " + strWhere;

                DataSet ds = new DataSet();
                MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("INC"));
                try
                {
                    da.OpenConn();
                    if (limStart == -1 && limMax == -1)
                        ds = da.dsGetTotalData(strSql);
                    else
                        ds = da.dsGetTableByRange(strSql, limStart, limMax);
                }
                finally
                {
                    da.Dispose();
                }

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    return null;

                ds.Tables[0].Columns.Add("FstateName", typeof(string));
                ds.Tables[0].Columns.Add("FHandling_num", typeof(string));
                ds.Tables[0].Columns.Add("FHandling_fee", typeof(string));

                foreach (DataRow dr in ds.Tables[0].Rows)
                {

                    long handling_count = long.Parse(dr["Ffact_num"].ToString()) - long.Parse(dr["Fsucpay_num"].ToString())
                        - long.Parse(dr["Ffailpay_num"].ToString());


                    long handling_amount = long.Parse(dr["Ffact_fee"].ToString()) - long.Parse(dr["Fsucpay_fee"].ToString())
                        - long.Parse(dr["Ffailpay_fee"].ToString());

                    dr["FHandling_num"] = handling_count.ToString();
                    dr["FHandling_fee"] = handling_amount.ToString();

                    switch (dr["Fstatus"].ToString())
                    {
                        case "1":
                            {
                                dr["FstateName"] = "初始状态"; break;
                            }
                        case "2":
                            {
                                dr["FstateName"] = "待审批"; break;
                            }
                        case "3":
                            {
                                dr["FstateName"] = "可付款"; break;
                            }
                        case "4":
                            {
                                dr["FstateName"] = "拒绝付款"; break;
                            }
                        case "5":
                            {
                                dr["FstateName"] = "执行完成"; break;
                            }
                        case "6":
                            {
                                dr["FstateName"] = "受理完成"; break;
                            }
                        case "7":
                            {
                                dr["FstateName"] = "已取消付款"; break;
                            }

                        default:
                            {
                                dr["FstateName"] = "未知" + dr["Fstatus"].ToString(); break;
                            }
                    }
                }

                return ds;
            }
            catch (LogicException err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                throw;
            }
            catch (Exception err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                throw new LogicException("Service处理失败！(" + err.Message + ")");
            }
            finally
            {
                rl.WriteLog();
            }
        }



        [WebMethodAttribute(Description = "统计代付批量的情况")]
        public DataSet CountBatchInfo_DF(string strBeginDate, string strEndDate, string spid, string spBatchID, string state)
        {
            string strWhere = " where Fcreate_time between '" + strBeginDate + "' and '" + strEndDate + "' and Ftype=2 ";
            if (spid.Trim() != "")
            {
                strWhere += " and Fspid='" + spid + "' ";
            }

            if (spBatchID.Trim() != "")
                strWhere += " and Fubatch_id='" + spBatchID + "' ";

            if (state != "0")
                strWhere += " and Fstatus=" + state;

            string strSql = " select count(*),sum(Fsucpay_num),sum(Fsucpay_fee),sum(Ffailpay_num),sum(Ffailpay_fee),"
                + "sum(Ffact_num-Fsucpay_num-Ffailpay_num),sum(Ffact_fee-Fsucpay_fee-Ffailpay_fee) from c2c_db_inc.t_spm_batopt " + strWhere;

            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("INC"));
            try
            {
                da.OpenConn();
                DataSet ds = da.dsGetTotalData(strSql);
                return ds;
            }
            catch
            {
                return null;
            }
            finally
            {
                da.Dispose();
            }
        }


        [WebMethod(Description = "代付单笔查询函数")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet QueryDFInfo(string bankID, string userID, string strBeginDate, string strEndDate, string spid, string spListID
            , string spBatchID, string state, string transaction_id, string bank_type, string service_code, int limStart, int limMax)
        {

            RightAndLog rl = new RightAndLog();
            try
            {
                if (myHeader == null)
                {
                    throw new LogicException("不正确的调用方法！");
                }


                rl.actionType = "代付单笔查询";
                rl.ID = userID;
                rl.OperID = myHeader.OperID;
                rl.sign = 1;
                rl.strRightCode = "";
                rl.RightString = myHeader.RightString;
                rl.SzKey = myHeader.SzKey;
                rl.type = "查询";
                rl.UserID = myHeader.UserName;
                rl.UserIP = myHeader.UserIP;
 
                DataSet ds;
                MySqlAccess dainc = new MySqlAccess(PublicRes.GetConnString("INCB"));
                try
                {
                    dainc.OpenConn();
                    string strSql = "select * from  c2c_db_inc.t_spm_rollopt where Ftype=2 ";

                    if ((!string.IsNullOrEmpty(strBeginDate)) && (!string.IsNullOrEmpty(strEndDate)))
                    {
                        strSql += " and Fmodify_time between '" + strBeginDate + "' and '" + strEndDate + "' ";
                    }

                    if (bankID != null && bankID.Trim() != "")
                    {
                        string bankIDKey = BankLib.BankIOX.GetCreditEncode(bankID, BankLib.BankIOX.fxykconn);
                        strSql += " and ( Fbank_id='" + bankID.Trim() + "' or  Fbank_id='" + bankIDKey.Trim() + "')";
                    }
                    if (userID != null && userID.Trim() != "")
                    {
                        strSql += " and Frecv_true_name='" + userID.Trim() + "' ";
                    }
                    if (spid != null && spid.Trim() != "")
                    {
                        strSql += " and Fspid='" + spid.Trim() + "' ";
                    }
                    if (spListID != null && spListID.Trim() != "")
                    {
                        strSql += " and Fu_serialno='" + spListID.Trim() + "' ";
                    }
                    if (spBatchID != null && spBatchID.Trim() != "")
                    {
                        strSql += " and Fubatch_id='" + spBatchID.Trim() + "' ";
                    }
                    if (state != null && state.Trim() != "0")
                    {
                        if (state == "3")//处理中
                        {
                            strSql += " and Fstatus in (1,4,5) ";
                        }
                        else if (state == "1")//成功  需要包含成功和退票两种状态
                        {
                            strSql += " and Fstatus in(2,6) ";
                        }
                        else if (state == "2")//失败
                        {
                            strSql += " and Fstatus = 3 ";
                        }
                    }
                    if (transaction_id != null && transaction_id.Trim() != "")
                    {
                        strSql += " and Ftransaction_id='" + transaction_id.Trim() + "' ";
                    }
                    if (bank_type != null && bank_type.Trim() != "")
                    {
                        strSql += " and Fbank_type='" + bank_type.Trim() + "' ";
                    }
                    if (service_code != null && service_code.Trim() != "0")
                    {
                        strSql += " and Fuser_type=" + service_code.Trim() + " ";
                    }
                    if (limStart == -1 && limMax == -1)
                        ds = dainc.dsGetTotalData(strSql);
                    else
                        ds = dainc.dsGetTableByRange(strSql, limStart, limMax);
                }
                finally
                {
                    dainc.Dispose();
                }

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    return null;

                ds.Tables[0].Columns.Add("FamountName", typeof(string));
                ds.Tables[0].Columns.Add("Fuser_typeName", typeof(string));
                ds.Tables[0].Columns.Add("FstatusName", typeof(string));
                ds.Tables[0].Columns.Add("Fbank_typeName", typeof(string));

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string tmp = dr["Fbank_type"].ToString().Trim();
                    dr["Fbank_typeName"] = BankIO.QueryBankName(tmp);

                    dr["FamountName"] = MoneyTransfer.FenToYuan(dr["Famount"].ToString());

                    tmp = dr["Fuser_type"].ToString();
                    if (tmp == "1")
                    {
                        dr["Fuser_typeName"] = "个人";
                    }
                    else
                        dr["Fuser_typeName"] = "公司";

                    tmp = dr["Fstatus"].ToString();
                    if (tmp == "1")
                    {
                        dr["FstatusName"] = "初始状态";
                    }
                    else if (tmp == "2")
                    {
                        dr["FstatusName"] = "成功";
                    }
                    else if (tmp == "3")
                    {
                        dr["FstatusName"] = "失败";
                    }
                    else if (tmp == "4")
                    {
                        dr["FstatusName"] = "已提交银行";
                    }
                    else if (tmp == "5")
                    {
                        dr["FstatusName"] = "处理中";
                    }
                    else if (tmp == "6")
                    {
                        dr["FstatusName"] = "退票";
                    }

                }
                return ds;
            }
            catch (LogicException err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                throw;
            }
            catch (Exception err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                throw new LogicException("Service处理失败！");
            }
            finally
            {
                rl.WriteLog();
            }
        }

        [WebMethod(Description = "财付通转账查询")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet QueryCFTTransferInfo(string spid, string strBeginDate, string strEndDate, string trUBatchID,
            string state, int limStart, int limMax)
        {
            RightAndLog rl = new RightAndLog();
            try
            {
                if (myHeader == null)
                {
                    throw new LogicException("不正确的调用方法！");
                }

                rl.actionType = "财付通转账查询";
                rl.ID = spid;
                rl.OperID = myHeader.OperID;
                rl.sign = 1;
                rl.strRightCode = "";
                rl.RightString = myHeader.RightString;
                rl.SzKey = myHeader.SzKey;
                rl.type = "查询";
                rl.UserID = myHeader.UserName;
                rl.UserIP = myHeader.UserIP;
        
                string strWhere = " where Fcreate_time between '" + strBeginDate + "' and '" + strEndDate + "' and Ftype=1 ";
                if (spid.Trim() != "")
                {
                    strWhere += " and Fspid='" + spid + "' ";
                }

                if (trUBatchID.Trim() != "")
                    strWhere += " and Fubatch_id='" + trUBatchID + "' ";

                if (state != "0")
                    strWhere += " and Fstatus=" + state;

                string strSql = " select * from c2c_db_inc.t_spm_batopt " + strWhere;

                DataSet ds = new DataSet();
                MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("INC"));
                try
                {
                    da.OpenConn();
                    if (limStart == -1 && limMax == -1)
                        ds = da.dsGetTotalData(strSql);
                    else
                        ds = da.dsGetTableByRange(strSql, limStart, limMax);
                }
                finally
                {
                    da.Dispose();
                }

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    return null;

                ds.Tables[0].Columns.Add("FstateName", typeof(string));
                ds.Tables[0].Columns.Add("FHandling_num", typeof(string));
                ds.Tables[0].Columns.Add("FHandling_fee", typeof(string));

                foreach (DataRow dr in ds.Tables[0].Rows)
                {

                    long handling_count = long.Parse(dr["Ffact_num"].ToString()) - long.Parse(dr["Fsucpay_num"].ToString())
                        - long.Parse(dr["Ffailpay_num"].ToString());

                    long handling_amount = long.Parse(dr["Ffact_fee"].ToString()) - long.Parse(dr["Fsucpay_fee"].ToString())
                        - long.Parse(dr["Ffailpay_fee"].ToString());

                    dr["FHandling_num"] = handling_count.ToString();
                    dr["FHandling_fee"] = handling_amount.ToString();

                    switch (dr["Fstatus"].ToString())
                    {
                        case "1":
                            {
                                dr["FstateName"] = "初始状态"; break;
                            }
                        case "2":
                            {
                                dr["FstateName"] = "待审批"; break;
                            }
                        case "3":
                            {
                                dr["FstateName"] = "可付款"; break;
                            }
                        case "4":
                            {
                                dr["FstateName"] = "拒绝付款"; break;
                            }
                        case "5":
                            {
                                dr["FstateName"] = "执行完成"; break;
                            }
                        case "6":
                            {
                                dr["FstateName"] = "受理完成"; break;
                            }
                        case "7":
                            {
                                dr["FstateName"] = "已取消付款"; break;
                            }

                        default:
                            {
                                dr["FstateName"] = "未知" + dr["Fstatus"].ToString(); break;
                            }
                    }
                }

                return ds;
            }
            catch (LogicException err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                throw;
            }
            catch (Exception err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                throw new LogicException("Service处理失败！(" + err.Message + ")");
            }
            finally
            {
                rl.WriteLog();
            }
        }

        [WebMethod(Description = "财付通转账详细查询函数")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet QueryTransferDetail(string spid, string trUBatchID, string trBillID, string strBeginDate, string strEndDate)
        {
            RightAndLog rl = new RightAndLog();
            try
            {
                if (myHeader == null)
                {
                    throw new LogicException("不正确的调用方法！");
                }

                rl.actionType = "财付通转账查询";
                rl.ID = spid;
                rl.OperID = myHeader.OperID;
                rl.sign = 1;
                rl.strRightCode = "";
                rl.RightString = myHeader.RightString;
                rl.SzKey = myHeader.SzKey;
                rl.type = "查询";
                rl.UserID = myHeader.UserName;
                rl.UserIP = myHeader.UserIP;
      
                DataSet ds;
                MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("INC"));
                try
                {
                    da.OpenConn();

                    string strWhere = " where Fcreate_time between '" + strBeginDate + "' and '" + strEndDate + "' and Ftype=1 ";
                    if (spid.Trim() != "")
                    {
                        strWhere += " and Fspid='" + spid + "' ";
                    }

                    if (trUBatchID.Trim() != "")
                        strWhere += " and Fubatch_id='" + trUBatchID + "' ";

                    if (trBillID.Trim() != "")
                        strWhere += " and Ftransaction_id='" + trBillID + "' ";

                    string strSql = " select * from c2c_db_inc.t_spm_rollopt " + strWhere + "limit 0,1";

                    ds = da.dsGetTotalData(strSql);
                }
                finally
                {
                    da.Dispose();
                }

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    return null;


                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ds.Tables[0].Columns.Add("FstatusName", typeof(String));

                    DataRow dr = ds.Tables[0].Rows[0];

                    //付款状态
                    string tmp = dr["Fstatus"].ToString();
                    if (tmp == "1")
                    {
                        dr["FstatusName"] = "初始状态";
                    }
                    else if (tmp == "2")
                    {
                        dr["FstatusName"] = "成功";
                    }
                    else if (tmp == "3")
                    {
                        dr["FstatusName"] = "失败";
                    }
                    else if (tmp == "4")
                    {
                        dr["FstatusName"] = "已提交银行";
                    }
                    else if (tmp == "5")
                    {
                        dr["FstatusName"] = "处理中";
                    }

                }
                return ds;
            }
            catch (LogicException err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                throw;
            }
            catch (Exception err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                throw new LogicException("Service处理失败！(" + err.Message + ")");
            }
            finally
            {
                rl.WriteLog();
            }
        }


        [WebMethodAttribute(Description = "统计代付单笔查询情况")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet CountDFInfo(string bankID, string userID, string strBeginDate, string strEndDate, string spid, string spListID
            , string spBatchID, string state, string transaction_id, string bank_type, string service_code)
        {
            DataSet ds;
            MySqlAccess dainc = new MySqlAccess(PublicRes.GetConnString("INCB"));
            try
            {
                dainc.OpenConn();

                string strSql = "select count(*) as totalRecordCount,sum(Famount) as totalPayNum from  c2c_db_inc.t_spm_rollopt where Ftype=2 ";
               
                if ((!string.IsNullOrEmpty(strBeginDate)) && (!string.IsNullOrEmpty(strEndDate)))
                {
                    strSql += " and Fmodify_time between '" + strBeginDate + "' and '" + strEndDate + "' ";
                }

                if (bankID != null && bankID.Trim() != "")
                {
                    strSql += " and Fbank_id='" + bankID.Trim() + "' ";
                }
                if (userID != null && userID.Trim() != "")
                {
                    strSql += " and Frecv_true_name='" + userID.Trim() + "' ";
                }
                if (spid != null && spid.Trim() != "")
                {
                    strSql += " and Fspid='" + spid.Trim() + "' ";
                }
                if (spListID != null && spListID.Trim() != "")
                {
                    strSql += " and Fu_serialno='" + spListID.Trim() + "' ";
                }
                if (spBatchID != null && spBatchID.Trim() != "")
                {
                    strSql += " and Fubatch_id='" + spBatchID.Trim() + "' ";
                }
                if (transaction_id != null && transaction_id.Trim() != "")
                {
                    strSql += " and Ftransaction_id='" + transaction_id.Trim() + "' ";
                }
                if (bank_type != null && bank_type.Trim() != "")
                {
                    strSql += " and Fbank_type='" + bank_type.Trim() + "' ";
                }
                if (service_code != null && service_code.Trim() != "0")
                {
                    strSql += " and Fuser_type=" + service_code.Trim() + " ";
                }
                if (state != null && state.Trim() != "0")
                {
                    if (state == "3")//处理中
                    {
                        strSql += " and Fstatus in (1,4,5) ";
                    }
                    else if (state == "1")//成功
                    {
                        strSql += " and Fstatus in(2,6) ";
                    }
                    else if (state == "2")//失败
                    {
                        strSql += " and Fstatus = 3 ";
                    }
                }


                ds = dainc.dsGetTotalData(strSql);
            }
            finally
            {
                dainc.Dispose();
            }

            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                return null;

            return ds;
        }



        [WebMethod(Description = "代付单笔详细查询函数")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet QueryDFDetail(string cep_id)
        {
            RightAndLog rl = new RightAndLog();
            try
            {
                if (myHeader == null)
                {
                    throw new LogicException("不正确的调用方法！");
                }

                rl.actionType = "代扣单笔详细查询函数";
                rl.ID = cep_id;
                rl.OperID = myHeader.OperID;
                rl.sign = 1;
                rl.strRightCode = "";
                rl.RightString = myHeader.RightString;
                rl.SzKey = myHeader.SzKey;
                rl.type = "查询";
                rl.UserID = myHeader.UserName;
                rl.UserIP = myHeader.UserIP;
          
                DataSet ds;
                MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("INC"));
                try
                {
                    da.OpenConn();
                    string strSql = "select * from c2c_db_inc.t_spm_rollopt where Fauto_id='" + cep_id + "' ";
                    ds = da.dsGetTotalData(strSql);
                }
                finally
                {
                    da.Dispose();
                }

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    return null;

                //20130816 lxl 处理省市、付款状态
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ds.Tables[0].Columns.Add("FstatusName", typeof(String));
                    ds.Tables[0].Columns.Add("Farea_str", typeof(String));
                    ds.Tables[0].Columns.Add("Fcity_str", typeof(String));
                    ds.Tables[0].Columns.Add("Fbank_typeName", typeof(string));
                    DataRow dr = ds.Tables[0].Rows[0];

                    string tmp1 = dr["Fbank_type"].ToString().Trim();
                    dr["Fbank_typeName"] = BankIO.QueryBankName(tmp1);

                    //付款状态
                    string tmp = dr["Fstatus"].ToString();
                    if (tmp == "1")
                    {
                        dr["FstatusName"] = "初始状态";
                    }
                    else if (tmp == "2")
                    {
                        dr["FstatusName"] = "成功";
                    }
                    else if (tmp == "3")
                    {
                        dr["FstatusName"] = "失败";
                    }
                    else if (tmp == "4")
                    {
                        dr["FstatusName"] = "已提交银行";
                    }
                    else if (tmp == "5")
                    {
                        dr["FstatusName"] = "处理中";
                    }
                    else if (tmp == "6")
                    {
                        dr["FstatusName"] = "退票";
                    }

                    //省份
                    string area = dr["Farea"].ToString();
                    if (area != null && area != "")
                    {
                        dr["Farea_str"] = AreaInfo.GetAreaName_Long(Int32.Parse(area));
                    }
                    //地区
                    string city = dr["Fcity"].ToString();
                    if (area != null && area != "" && city != null && city != "")
                    {
                        dr["Fcity_str"] = AreaInfo.GetCityName_Long(Int32.Parse(area), city);
                    }
                }



                return ds;
            }
            catch (LogicException err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                throw;
            }
            catch (Exception err)
            {
                rl.sign = 0;
                rl.ErrorMsg = PublicRes.replaceMStr(err.Message);
                throw new LogicException("Service处理失败！(" + err.Message + ")");
            }
            finally
            {
                rl.WriteLog();
            }
        }

        [WebMethod(Description = "图标管理查询")]
        public DataSet QueryIconInfo(string account)
        {
            string uin = QueryInfo.GetDbNum(account);//计算QQ、email、手机号对应的uin数字字符串
            int length = uin.Length;
            if (length < 3)
            {
                uin = "00000";
                length = uin.Length;
            }
            string dbIndex = uin.Substring(length - 2, 2);
            string tblIndex = uin.Substring(length - 3, 1);
            string strSql = string.Format(@"select Fvipflag, Fvalue from  c2c_db_{0}.t_user_rank_{1} where Fuin = '{2}'", dbIndex, tblIndex, account);
            // MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("t_user_rank", account.Substring(account.Length - 2)));
            MySqlAccess da = null;
            int DBNode = QueryInfo.GetDBNode(uin);//计算分器
            if (DBNode == 0)
                da = new MySqlAccess(PublicRes.GetConnString("ICONINFO1"));
            if (DBNode == 1)
                da = new MySqlAccess(PublicRes.GetConnString("ICONINFO2"));
            if (DBNode == 2)
                da = new MySqlAccess(PublicRes.GetConnString("ICONINFO3"));

            DataSet ds = new DataSet();
            try
            {
                da.OpenConn();
                ds = da.dsGetTotalData(strSql);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    return null;
                return ds;
            }
            catch (Exception e)
            {
                throw new Exception("service发生错误,请联系管理员！" + e.Message);
            }
            finally
            {
                da.Dispose();
            }
        }

        [WebMethod(Description = "刷新图标")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public void RefreshIcon(string account)
        {
            string inmsg = "uin=" + account;

            var parameters = System.Text.Encoding.Default.GetBytes(inmsg);
            var length = new byte[4];
            //  length = BitConverter.GetBytes(parameters.Length);
            length = UDP.GetByteFromInt(parameters.Length + 4);
            List<byte> bufferIn = new List<byte>();
            bufferIn.AddRange(length);
            bufferIn.AddRange(parameters);
            string ip = ConfigurationManager.AppSettings["RefreshIcon_IP"];
            string port = ConfigurationManager.AppSettings["RefreshIcon_PORT"];
            TcpClient tcpClient = new TcpClient();
            IPAddress ipAddress = IPAddress.Parse(ip);
            IPEndPoint ipPort = new IPEndPoint(ipAddress, Int32.Parse(port));
            tcpClient.Connect(ipPort);
            NetworkStream stream = tcpClient.GetStream();
            stream.Write(bufferIn.ToArray(), 0, bufferIn.ToArray().Length);
            byte[] bufferOut = new byte[1024];
            stream.Read(bufferOut, 0, 1024);
            byte[] bufferLen = new byte[4];
            Array.Copy(bufferOut, 0, bufferLen, 0, 4);
            // int c = BitConverter.ToInt32(bufferLen, 0);
            int c = UDP.GetIntFromByte(bufferLen);
            c = c - 4;
            byte[] cont = new byte[c];
            Array.Copy(bufferOut, 4, cont, 0, c);
            string answer = Encoding.Default.GetString(cont);
            string[] strlist1 = answer.Split('&');
            if (strlist1.Length == 0)
            {
                throw new LogicException("返回结果有误：" + answer);
            }
            Hashtable ht = new Hashtable(strlist1.Length);
            foreach (string strtmp in strlist1)
            {
                string[] strlist2 = strtmp.Split('=');
                if (strlist2.Length != 2)
                {
                    continue;
                }
                ht.Add(strlist2[0].Trim(), strlist2[1].Trim());
            }

            if (!ht.Contains("result") || ht["result"].ToString().Trim() != "0")
            {
                throw new LogicException("返回结果有误：" + answer);
            }
        }

        [WebMethod(Description = "熄灭图标")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public void ExtinguishIcon(string account)
        {
            try
            {
                if (myHeader == null)
                {
                    throw new LogicException("不正确的调用方法！");
                }
                var parameters = Encoding.Default.GetBytes("serviceid=setuserlevel&servicetype=62&uin=" + account + "&value=0&clientip=" + myHeader.UserIP + "&serverip=" + myHeader.UserIP + "&src_app=cft_vip\r\n");
                string msg = "";
                string IP = ConfigurationManager.AppSettings["ExtinguishIcon_IP"];
                string PORT = ConfigurationManager.AppSettings["ExtinguishIcon_PORT"];
                string answer = UDP.GetTCPReplyString(parameters, IP, Int32.Parse(PORT), out msg);
                Hashtable paramsHt = new Hashtable();
                if (answer != "")
                {
                    paramsHt = UDP.tcpParameters(answer);

                    if (!paramsHt.Contains("retcode") || paramsHt["retcode"].ToString().Trim() != "0" || !paramsHt.Contains("oidbresult") || paramsHt["oidbresult"].ToString().Trim() != "0")
                    {
                        throw new LogicException("设置等级接口返回结果有误：" + answer);
                    }
                }
                else
                {
                    throw new Exception(msg);
                }

                parameters = Encoding.Default.GetBytes("serviceid=setrichflag2&servicetype=76&uin=" + account + "&value=0&clientip=" + myHeader.UserIP + "&serverip=" + myHeader.UserIP + "&src_app=cft_vip\r\n");
                msg = "";
                answer = UDP.GetTCPReplyString(parameters, IP, Int32.Parse(PORT), out msg);
                paramsHt.Clear();
                if (answer != "")
                {
                    paramsHt = UDP.tcpParameters(answer);

                    if (!paramsHt.Contains("retcode") || paramsHt["retcode"].ToString().Trim() != "0" || !paramsHt.Contains("oidbresult") || paramsHt["oidbresult"].ToString().Trim() != "0")
                    {
                        throw new LogicException("设置普通会员图标接口返回结果有误：" + answer);
                    }
                }
                else
                {
                    throw new Exception(msg);
                }

                parameters = Encoding.Default.GetBytes("serviceid=setrichflag3&servicetype=65&uin=" + account + "&value=0&clientip=" + myHeader.UserIP + "&serverip=" + myHeader.UserIP + "&src_app=cft_vip\r\n");
                msg = "";
                answer = UDP.GetTCPReplyString(parameters, IP, Int32.Parse(PORT), out msg);
                paramsHt.Clear();
                if (answer != "")
                {
                    paramsHt = UDP.tcpParameters(answer);

                    if (!paramsHt.Contains("retcode") || paramsHt["retcode"].ToString().Trim() != "0" || !paramsHt.Contains("oidbresult") || paramsHt["oidbresult"].ToString().Trim() != "0")
                    {
                        throw new LogicException("设置超级会员图标接口返回结果有误：" + answer);
                    }
                }
                else
                {
                    throw new Exception(msg);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        [WebMethod(Description = "自动充值签约查询")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet QueryAutomaticRecharge(string uin, int limStart, int limMax)
        {
            string msg = "";
            try
            {
                if (myHeader == null)
                {
                    throw new Exception("不正确的调用方法！");
                }
                string client_ip = myHeader.UserIP;

                string service_name = "charge_query_plan_service";//接口名
                string req = "";

                DataSet ds = null;
                req = "charge_type=nsp&uin=" + uin + "&offset=" + limStart + "&limit=" + limMax + "&client_ip=" + client_ip;
                ds = CommQuery.GetXmlToDataSetFromICE(req, "", service_name, out msg);
                DataTable dtt = ds.Tables[0];
                return ds;
            }
            catch (Exception e)
            {
                throw new Exception("Service处理失败！" + msg);
            }
        
        }

        [WebMethod(Description = "自动充值交易单查询")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet QueryAutomaticRechargeBillList(string uin, string plan_id, int limStart, int limMax)
        {
            string msg = "";
            try
            {
                if (myHeader == null)
                {
                    throw new Exception("不正确的调用方法！");
                }
                string client_ip = myHeader.UserIP;


                string service_name = "charge_query_bill_service";//接口名
                string req = "";

                DataSet ds = null;
                req = "uin=" + uin + "&offset=" + limStart + "&limit=" + limMax + "&plan_id=" + plan_id + "&client_ip=" + client_ip;
                ds = CommQuery.GetXmlToDataSetFromICE(req, "BATCH_QUERY", service_name, out msg);
                DataTable dtt = ds.Tables[0];
                return ds;
            }
            catch (Exception e)
            {
                throw new Exception("Service处理失败！" + msg);
            }

        }


        [WebMethod(Description = "自动充值扣款方式查询")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet GetBankTypeKK(string uin, string uid)
        {
            string msg = "";
            try
            {
                string service_name = "common_query_service";//接口名
                string spid = ConfigurationManager.AppSettings["AutomaticRechargeSpid"].Trim();
                string req = "";
                req = "fields=uid:" + uid;
                req += "|cur_type:1|spid:" + spid + "|channel_id:8|app_code:9|list_state:1|bind_status:2";
                req += "&flag=1&head_u=107986219&limit=10&offset=0&reqid=208&request_type=4006&sp_id=2000000000&ver=1";

                DataSet ds = null;
                //req = "fields=uid=" + uid + "&uin=" + uin + "&cur_type=1&spid=nsp&channel_id=8&app_code=9
                //    &list_state=1&bind_status=2&flag=1&head_u=107986219&limit=10&offset=0
                //    &reqid=208&request_type=4006&sp_id=2000000000&ver=1";
                ds = CommQuery.GetXmlToDataSetFromICE(req, "", service_name, out msg);
                DataTable dtt = ds.Tables[0];
                return ds;
            }
            catch (Exception e)
            {
                throw new Exception("Service处理失败！" + msg);
            }
        }

        [WebMethod(Description = "通过内部ID得到财付通账号（QQID）")]
        public string Uid2QQ(string fuid)
        {
            return PublicRes.Uid2QQ(fuid);
        }


        //通过服务签名
        [WebMethod(Description = "通过服务签名")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public string SingedByService(string SingedString)
        {
            try
            {
                DataSet ds = null;
                string Msg = "";
                string errMsg = "";
                //md5
                string CFTAccount = ConfigurationManager.AppSettings["CFTAccount"];
                string wxzfAccount = ConfigurationManager.AppSettings["wxzfAccount"];
                string relay_ip = ConfigurationManager.AppSettings["Relay_IP"];
                string relay_port = ConfigurationManager.AppSettings["Relay_PORT"];

                //  string sign = "account_no=" + acc_no + "&bank_type=" + bank_type + "&uid=" + uid + "&uidtoc=" + uidtoc + "&key=";
                string sign = SingedString + "&key=";
                sign = System.Web.HttpUtility.UrlEncode(sign, System.Text.Encoding.GetEncoding("gb2312"));

                Msg = "";
                errMsg = "";
                string req_sign = "request_type=132&ver=1&head_u=&sp_id=" + CFTAccount + "&merchant_spid=" + CFTAccount + "&sp_str=" + sign;

                string sign_md5 = commRes.GetFromRelay(req_sign, relay_ip, relay_port, out Msg);
                if (Msg != "")
                {
                    throw new Exception("1签名出错:" + Msg);
                }
                ds = CommQuery.ParseRelayStr(sign_md5, out errMsg);
                if (errMsg != "")
                {
                    throw new Exception("2签名出错:" + errMsg);
                }
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    sign_md5 = ds.Tables[0].Rows[0]["sp_md5"].ToString();
                }
                return sign_md5;
            }
            catch (Exception ex)
            {
                throw new LogicException("通过服务签名出错！");
            }
        }

        [WebMethod(Description = "获取微信用户的参与的AA交易记录")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet GetAATradeList(string aaUIN, int startIndex, int count)
        {
            var aaUId = PublicRes.ConvertToFuid(aaUIN);

            if (aaUId == null)
                throw new Exception(string.Format("AA财付通帐号{0}查询不到对应的UID", aaUIN));

            var tableName = string.Format("wx_aa_collection_{0}.t_aa_record_{1}", aaUId.Substring(aaUId.Length - 2), aaUId.Substring(aaUId.Length - 3, 1));

            string strSql = string.Format(@"select * from {0} where Fuid='{1}' order by Fcreate_time desc limit {2},{3}", tableName, aaUId, startIndex, count);

            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("WXAA"));
            DataSet ds = new DataSet();
            try
            {
                da.OpenConn();
                ds = da.dsGetTotalData(strSql);
                return ds;
            }
            catch (Exception e)
            {
                throw new Exception("service发生错误,请联系管理员！" + e.Message);
            }
        }

        #region 微信支付
        [WebMethod(Description = "获取指定的AA收单分单记录明细")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet GetAATradeDetailsSingleYear(string aaCollectionNo, DateTime createTime, int startIndex, int count)
        {
            var tableName = string.Format("wx_aa_collection.t_pay_list_{0}", createTime.ToString("yyyy"));
            //var nextYearTableName = string.Format("wx_aa_collection.t_pay_list_{0}", createTime.AddYears(1).ToString("yyyy"));

            string strSql = string.Format(@"select * from {0} where Faa_collection_no='{1}' order by Fcreate_time desc limit {2},{3}", tableName, aaCollectionNo, startIndex, count);

            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("WXAA"));
            DataSet ds = new DataSet();
            try
            {
                da.OpenConn();
                ds = da.dsGetTotalData(strSql);
                return ds;
            }
            catch (Exception e)
            {
                throw new Exception("service发生错误,请联系管理员！" + e.Message);
            }

        }

        [WebMethod(Description = "获取指定的AA收款总单信息")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet QueryAATotalTradeInfo(string aaCollectionNo)
        {
            var tableName = string.Format("wx_aa_collection_{0}.t_collection_list_{1}", aaCollectionNo.Substring(aaCollectionNo.Length - 2), aaCollectionNo.Substring(aaCollectionNo.Length - 3, 1));
            //var nextYearTableName = string.Format("wx_aa_collection.t_pay_list_{0}", createTime.AddYears(1).ToString("yyyy"));

            string strSql = string.Format(@"select * from {0} where Faa_collection_no='{1}' limit 1", tableName, aaCollectionNo);

            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("WXAA"));
            DataSet ds = new DataSet();
            try
            {
                da.OpenConn();
                ds = da.dsGetTotalData(strSql);
                return ds;
            }
            catch (Exception e)
            {
                throw new Exception("service发生错误,请联系管理员！" + e.Message);
            }

        }

        [WebMethod(Description = "获取微信用户的红包接收记录")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet GetReceiveRedPacketList(string hbUIN, DateTime beginTime, DateTime endTime, int startIndex, int count)
        {
            string openId = string.Empty;

            try
            {
                openId = hbUIN.Split('@')[0];
            }
            catch
            {
                throw new Exception(string.Format("红包财付通帐号格式有误", hbUIN));
            }


            var tableName = string.Format("wx_hongbao.t_receive_list_{0}", beginTime.ToString("yyyy"));

            string strSql = string.Format(@"select * from {0} where Freceive_openid='{1}' and Fcreate_time >= '{2}' and  Fcreate_time <= '{3}' order by Fcreate_time desc limit {4},{5}",
                tableName, openId, beginTime.ToString("yyyy-MM-dd 00:00:00"), endTime.ToString("yyyy-MM-dd 23:59:59"), startIndex, count);

            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("WXHongbao"));
            DataSet ds = new DataSet();
            try
            {
                da.OpenConn();
                ds = da.dsGetTotalData(strSql);
                return ds;
            }
            catch (Exception e)
            {
                throw new Exception("service发生错误,请联系管理员！" + e.Message);
            }
        }

        [WebMethod(Description = "获取指定红包的接收记录")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet GetRedPacketDetailList(string sendListId, DateTime createTime, int startIndex, int count)
        {

            var tableName = string.Format("wx_hongbao.t_receive_list_{0}", createTime.ToString("yyyy"));

            string strSql = string.Format(@"select * from {0} where Fsend_list_id='{1}' order by Fcreate_time desc limit {2},{3}",
                tableName, sendListId, startIndex, count);

            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("WXHongbao"));
            DataSet ds = new DataSet();
            try
            {
                da.OpenConn();
                ds = da.dsGetTotalData(strSql);
                return ds;
            }
            catch (Exception e)
            {
                throw new Exception("service发生错误,请联系管理员！" + e.Message);
            }
        }

        [WebMethod(Description = "获取微信用户的红包发送记录")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet GetSendRedPacketList(string hbUIN, DateTime beginTime, DateTime endTime, int startIndex, int count, string payListId = "")
        {
            string openId = string.Empty;

            try
            {
                openId = hbUIN.Split('@')[0];
            }
            catch
            {
                throw new Exception(string.Format("红包财付通帐号格式有误", hbUIN));
            }


            var tableName = string.Format("wx_hongbao.t_send_list_{0}", beginTime.ToString("yyyy"));
            var sqlTemple = new StringBuilder("select * from {0} where Fopenid='{1}' and Fcreate_time >= '{2}' and Fcreate_time <= '{3}'");

            if (!string.IsNullOrEmpty(payListId))
                sqlTemple.Append(string.Format(" and Fpay_listid='{0}'", payListId));

            sqlTemple.Append(" order by Fcreate_time desc limit {4},{5}");

            string strSql = string.Format(sqlTemple.ToString(),
                tableName, openId, beginTime.ToString("yyyy-MM-dd 00:00:00"), endTime.ToString("yyyy-MM-dd 23:59:59"), startIndex, count);

            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("WXHongbao"));
            DataSet ds = new DataSet();
            try
            {
                da.OpenConn();
                ds = da.dsGetTotalData(strSql);
                return ds;
            }
            catch (Exception e)
            {
                throw new Exception("service发生错误,请联系管理员！" + e.Message);
            }
        }

        [WebMethod(Description = "获取小额刷卡账户信息")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet GetSmallCreditCardInfo(string uin)
        {
            try
            {
                if (string.IsNullOrEmpty(uin))
                {
                    throw new Exception("财付通账号不能为空");
                }
                //uin = "201312109686360@wx.tenpay.com";
                string fuid = PublicRes.ConvertToFuid(uin);
                if (string.IsNullOrEmpty(fuid))
                {
                    throw new Exception("财付通账号不存在");
                }

                MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("WXXESK"));
                string table = PublicRes.GetTName("wx_offline_db", "t_offline_user", fuid);
                string strSql = "SELECT * FROM " + table + " WHERE Fuid=" + fuid;
                DataSet ds = new DataSet();
                da.OpenConn();
                ds = da.dsGetTotalData(strSql);
                return ds;
            }
            catch (Exception e)
            {
                throw new Exception("service发生错误,请联系管理员！" + e.Message);
            }

            return null;
        }

        #endregion

        [WebMethod(Description = "境外收单退款查询")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet OverseasReturnQuery(string transid, string drawid)
        {
            //(string transid, string drawid)
            string msg = "";
            try
            {
                string service_name = "fcpay_query_refundinfo_service";//接口名
                string req = "";

                DataSet ds = null;
                req = "request_type=8222";
                if (transid != "")
                    req += "&transaction_id=" + transid;
                if (drawid != "")
                    req += "&draw_id=" + drawid;
                if (transid == "" && drawid == "")
                {
                    throw new Exception("Service处理失败！交易单号和退款单号需存在一个！");
                }

                ds = CommQuery.GetXmlToDataSetFromICE(req, "", service_name, out msg);
                if (msg != "")
                {
                    throw new Exception("Service处理失败！" + msg);
                }
                return ds;
            }
            catch (Exception e)
            {
                throw new Exception("Service处理失败！" + e.Message);
            }
        }


        [WebMethod(Description = "机票订单查询")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet AirTicketsOrderQuery(string sppreno, string ticketno, string transaction_id, string passenger_name, string cert_id,
            string name, string mobile, string uin, string insur_no, string start_time, string end_time, string trade_type, int limit, int page_id)
        {
            string msg = "";
            DataSet ds = new DataSet();
            string errMsg = ""; //解析xml用
            try
            {
                string cgi = "";
                if (sppreno == "" && ticketno == "" && transaction_id == "" && passenger_name == ""
                    && cert_id == "" && name == "" && mobile == "" && insur_no == "")
                {
                    cgi = System.Configuration.ConfigurationManager.AppSettings["QueryAllAirOrderCgi"].ToString();
                    cgi += "?uin=" + uin;
                    cgi += "&trade_type=" + trade_type;
                    cgi += "&start_time=" + start_time;
                    cgi += "&end_time=" + end_time;
                    cgi += "&limit=" + limit;
                    cgi += "&sp_code=kf&page_id=" + page_id;
                }
                else
                {
                    string query_type = "";
                    string wd = "";
                    if (sppreno != "")
                    {
                        query_type = "1";
                        wd = sppreno;
                    }
                    else if (transaction_id != "")
                    {
                        query_type = "2";
                        wd = transaction_id;
                    }

                    else if (insur_no != "")
                    {
                        query_type = "3";
                        wd = insur_no;
                    }
                    else if (ticketno != "")
                    {
                        query_type = "5";
                        wd = ticketno;
                    }
                    else if (cert_id != "")
                    {
                        query_type = "6";
                        wd = cert_id;
                    }
                    else if (passenger_name != "")
                    {
                        query_type = "7";
                        wd = System.Web.HttpUtility.UrlEncode(passenger_name, System.Text.Encoding.GetEncoding("gb2312"));//对wd参数进行编码
                    }
                    else if (mobile != "")
                    {
                        query_type = "8";
                        wd = mobile;
                    }
                    else if (name != "")
                    {
                        query_type = "9";
                        wd = System.Web.HttpUtility.UrlEncode(name, System.Text.Encoding.GetEncoding("gb2312"));//对wd参数进行编码
                    }
                    cgi = System.Configuration.ConfigurationManager.AppSettings["QueryAirOrderByParamCgi"].ToString();
                    cgi += "?query_type=" + query_type;
                    cgi += "&wd=" + wd;
                    cgi += "&trade_type=" + trade_type;
                    cgi += "&start_time=" + start_time;
                    cgi += "&end_time=" + end_time;
                    cgi += "&limit=" + limit;
                    cgi += "&sp_code=kf&page_id=" + page_id;
                }

                string answer = "";
                System.Text.Encoding encoding = System.Text.Encoding.GetEncoding("GB2312");
                //  cgi = "http://kf.trip.qq.com/cgi-bin/v1.0/jp_qq_query_all_order.cgi?_=1377660280772&uin=4197831&trade_type=payed&start_time=2013-07-28&end_time=2013-08-28&limit=10&sp_code=kf&page_id=1";
                System.Net.HttpWebRequest webrequest = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(cgi);
                webrequest.ContentType = "text/xml;charset=GBK";
                System.Net.HttpWebResponse webresponse = (System.Net.HttpWebResponse)webrequest.GetResponse();
                Stream stream = webresponse.GetResponseStream();
                StreamReader streamReader = new StreamReader(stream, encoding);
                answer = streamReader.ReadToEnd();
                //解析xml
                ds = CommQuery.PaseCgiXmlForTravelPlatform(answer, out errMsg);
                webresponse.Close();
                streamReader.Close();

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    if (errMsg != "")
                        throw new Exception("Service处理失败！" + errMsg);
                    return null;
                }

                ds.Tables[0].Columns.Add("strSp_code", typeof(string));
                ds.Tables[0].Columns.Add("strTrade_state", typeof(string));
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    switch (dr["sp_code"].ToString())
                    {
                        case "qunar":
                            {
                                dr["strSp_code"] = "去哪儿网"; break;
                            }
                        case "csair":
                            {
                                dr["strSp_code"] = "南航直销专区"; break;
                            }
                        case "ceair":
                            {
                                dr["strSp_code"] = "东航直销专区"; break;
                            }
                        case "hnair":
                            {
                                dr["strSp_code"] = "海航直销专区"; break;
                            }
                        case "szair":
                            {
                                dr["strSp_code"] = "深航直销专区"; break;
                            }
                        case "scair":
                            {
                                dr["strSp_code"] = "川航直销专区"; break;
                            }
                        case "XX":
                            {
                                dr["strSp_code"] = "黄金假日"; break;
                            }
                        case "XXX":
                            {
                                dr["strSp_code"] = "财付通专区"; break;
                            }
                        default:
                            {
                                dr["strSp_code"] = "未知"; break;
                            }
                    }
                    switch (dr["trade_state"].ToString())
                    {
                        case "1":
                        case "2":
                            {
                                dr["strTrade_state"] = "创建订单"; break;
                            }
                        case "3":
                            {
                                dr["strTrade_state"] = "占座未支付"; break;
                            }
                        case "4":
                            {
                                dr["strTrade_state"] = "申请支付"; break;
                            }
                        case "5":
                        case "6":
                            {
                                dr["strTrade_state"] = "支付成功"; break;
                            }
                        case "7":
                            {
                                dr["strTrade_state"] = "出票异常"; break;
                            }
                        case "8":
                        case "12":
                        case "13":
                            {
                                dr["strTrade_state"] = "出票成功"; break;
                            }
                        case "14":
                        case "16":
                            {
                                dr["strTrade_state"] = "部分退票中"; break;
                            }
                        case "17":
                            {
                                dr["strTrade_state"] = "部分退票成功"; break;
                            }
                        case "18":
                            {
                                dr["strTrade_state"] = "部分退票失败"; break;
                            }
                        case "19":
                        case "21":
                            {
                                dr["strTrade_state"] = "全部退票中"; break;
                            }
                        case "22":
                            {
                                dr["strTrade_state"] = "全部退票成功"; break;
                            }
                        case "23":
                            {
                                dr["strTrade_state"] = "全部退票失败"; break;
                            }
                        case "11":
                            {
                                dr["strTrade_state"] = "出票失败"; break;
                            }
                        case "99":
                            {
                                dr["strTrade_state"] = "抢购成功"; break;
                            }
                        default:
                            {
                                dr["strTrade_state"] = "未知"; break;
                            }
                    }
                }
                return ds;
            }
            catch (Exception e)
            {
                throw new Exception("Service处理失败！" + e.Message);
            }

        }

        [WebMethod(Description = "酒店订单查询")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet HotelOrderQuery(string hotelUin, string hotelName, string start_time, string end_time, int MaxRows, int PageIndex)
        {
            string msg = "";
            DataSet ds = new DataSet();
            string errMsg = ""; //解析xml用
            try
            {
                if (hotelName != "")
                {
                    hotelName = System.Web.HttpUtility.UrlEncode(hotelName, System.Text.Encoding.GetEncoding("utf-8"));//对wd参数进行编码
                }
                string answer = "";
                System.Text.Encoding encoding = System.Text.Encoding.GetEncoding("utf-8");
                string cgi = System.Configuration.ConfigurationManager.AppSettings["HotelOrderQueryList"].ToString();
                cgi += "?SpId=1000000005&HotelName=" + hotelName;
                cgi += "&hotelUin=" + hotelUin;
                cgi += "&ReserveBeginDate=" + start_time;
                cgi += "&ReserveEndDate=" + end_time;
                cgi += "&MaxRows=" + MaxRows;
                cgi += "&PageIndex=" + PageIndex;
                System.Net.HttpWebRequest webrequest = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(cgi);
                //webrequest.ContentType = "text/xml;charset=GBK";
                System.Net.HttpWebResponse webresponse = (System.Net.HttpWebResponse)webrequest.GetResponse();
                Stream stream = webresponse.GetResponseStream();
                StreamReader streamReader = new StreamReader(stream, encoding);
                answer = streamReader.ReadToEnd();
                //解析xml
                ds = CommQuery.PaseCgiJsonForHotelOrderQuery(answer, out errMsg);
                webresponse.Close();
                streamReader.Close();

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    if (errMsg != "")
                        throw new Exception("Service处理失败！" + errMsg);
                    return null;
                }

                ds.Tables[0].Columns.Add("State_str", typeof(string));
                ds.Tables[0].Columns.Add("CheckInfo", typeof(string));
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string CheckInDate = dr["CheckInDate"].ToString();
                    string CheckOutDate = dr["CheckOutDate"].ToString();
                    try
                    {
                        int date = DateTime.Parse(CheckOutDate).CompareTo(DateTime.Parse(CheckInDate));//计算天数
                        dr["CheckInfo"] = dr["RommsCnt"].ToString() + "间|" + date + "晚";
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Service处理失败！CheckInDate及CheckOutDate有误：" + CheckInDate + "  " + CheckOutDate);
                    }


                    switch (dr["State"].ToString())
                    {
                        case "0":
                            {
                                dr["State_str"] = "初始化"; break;
                            }
                        case "1":
                            {
                                dr["State_str"] = "订单提交中"; break;
                            }
                        case "2":
                            {
                                dr["State_str"] = "订单提交成功"; break;
                            }
                        case "3":
                            {
                                dr["State_str"] = "订单提交SP处理失败"; break;
                            }
                        case "4":
                            {
                                dr["State_str"] = "预定成功"; break;
                            }
                        case "5":
                            {
                                dr["State_str"] = "预定失败"; break;
                            }
                        case "6":
                            {
                                dr["State_str"] = "预定处理中"; break;
                            }
                        case "7":
                            {
                                dr["State_str"] = "订单取消"; break;
                            }
                        case "8":
                            {
                                dr["State_str"] = "已入住"; break;
                            }
                        case "9":
                            {
                                dr["State_str"] = "NO SHOW"; break;
                            }
                        case "10":
                            {
                                dr["State_str"] = "已离店(已结账)"; break;
                            }
                        default:
                            {
                                dr["State_str"] = "未知"; break;
                            }
                    }
                }
                return ds;
            }
            catch (Exception e)
            {
                throw new Exception("Service处理失败！" + e.Message);
            }

        }

        [WebMethod(Description = "姓名生僻字查询")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet RareNameQuery(string CardNo)
        {
            string msg = "";
            try
            {
                string service_name = "common_query_service";//接口名
                string req = "";
                string card_tail = CardNo.Substring(CardNo.Length - 4, 4);//卡尾号
                CardNo = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(CardNo, "md5").ToLower();
                req = "flag=1&reqid=2136&fields=card_id:" + CardNo + "|card_tail:" + card_tail;
                DataSet ds = null;
                ds = CommQuery.GetXmlToDataSetFromICE(req, "", "common_query_service", out msg);
                if (msg != "")
                    throw new Exception("Service处理失败！" + msg);
                else
                {
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        DataTable dt = ds.Tables[0];
                        dt.Columns.Add("Fuser_type_str", typeof(String));//用户类型
                        dt.Columns.Add("record_type_str", typeof(String));//登记类型
                        dt.Columns.Add("updateuser", typeof(String));//修改人
                        dt.Columns.Add("card_state_str", typeof(String));//卡状态
                        dt.Columns.Add("modify_type", typeof(String));//登记类型编号

                        foreach (DataRow dr in dt.Rows)
                        {
                            //对bankid解密 等pauluszhou数据倒完后，切成该解密方式
                            string bankID_Encode = PublicRes.BankIDEncode_ForRareName(dr["Fcard_no"].ToString());

                            //老的解密方式
                            //string bankID_Encode = PublicRes.BankIDEncode_ForBankCardUnbind(dr["Fcard_no"].ToString());

                            bankID_Encode = bankID_Encode.Replace("\0", "");
                            dr["Fcard_no"] = bankID_Encode;
                            string name = PublicRes.NameEncode_ForRareName(dr["Faccount_name"].ToString());
                            dr["Faccount_name"] = name.ToString().Replace("\0", "");

                            switch (dr["Fuser_type"].ToString())
                            {
                                case "0":
                                    dr["Fuser_type_str"] = "公司"; break;
                                case "1":
                                    dr["Fuser_type_str"] = "个人"; break;
                                default:
                                    dr["Fuser_type_str"] = "未知"; break;
                            }
                            switch (dr["Fcard_state"].ToString())
                            {
                                case "1":
                                    dr["card_state_str"] = "付款"; break;
                                case "2":
                                    dr["card_state_str"] = "成功付款"; break;
                                case "3":
                                    dr["card_state_str"] = "无效"; break;
                                default:
                                    dr["card_state_str"] = "未知"; break;
                            }

                            string modify_log = dr["Fmodify_log"].ToString();//1100000000,a,2014-05-04 17:07:39|1100000000,b,2014-05-04 17:08:02
                            string[] log = modify_log.Split('|');

                            List<string[]> m = new List<string[]>();
                            List<string[]> result = new List<string[]>();
                            Hashtable logHash = new Hashtable();
                            for (int i = 0; i < log.Length; i++)
                            {
                                m.Add(log[i].Split(','));
                                if (log[i].Split(',').Length == 3)
                                    logHash.Add(log[i].Split(',')[2], i);//取到时间
                            }
                            //排序找到最新的那条记录
                            ArrayList akeys = new ArrayList(logHash.Keys);
                            akeys.Sort();

                            foreach (string k in akeys)
                            {
                                result.Add(log[int.Parse(logHash[k].ToString())].Split(','));
                            }
                            string[] modify = result[result.Count - 1];//取最新的日志
                            if (modify.Length != 3)
                                throw new Exception("Fmodify_log字段有误：" + dr["Fmodify_log"].ToString());

                            dr["updateuser"] = modify[0];
                            switch (modify[1])
                            {
                                case "a":
                                    dr["record_type_str"] = "改名"; break;
                                case "b":
                                    dr["record_type_str"] = "生僻字"; break;
                                case "c":
                                    dr["record_type_str"] = "中小银行信息补填"; break;
                                default:
                                    dr["record_type_str"] = "未知"; break;
                            }
                            dr["modify_type"] = modify[1];
                        }
                    }
                }
                DataTable dtt = ds.Tables[0];
                return ds;
            }
            catch (Exception e)
            {
                throw new Exception("Service处理失败！" + msg);
            }
        }

        [WebMethod(Description = "新增及编辑姓名生僻字")]
        public void AddOneRareName(T_RareName_INFO rareName)
        {
            string msg = "";
            try
            {
                string service_name = "bind_txcard_reg_service";//提现卡信息登记接口
                string req = "&req_src=2&op_type=" + rareName.op_type;
                req += "&card_no=" + rareName.card_no;
                req += "&card_tail=" + rareName.card_no.Substring((rareName.card_no).Length - 4, 4);
                req += "&user_type=" + rareName.user_type;
                req += "&account_name=" + rareName.account_name;
                req += "&card_state=" + rareName.card_state;
                req += "&modify_username=" + rareName.modify_username;
                req += "&modify_type=" + rareName.modify_type;
                CommQuery.GetOneTableFromICE(req, "", service_name, true, out msg);
                if (msg != "")
                    throw new Exception("Service处理失败！" + msg);
            }
            catch (Exception ex)
            {
                throw new Exception("Service处理失败！" + ex.Message);
            }
        }

        [WebMethod(Description = "自动续费查询")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet AtuoRenewQuery(string qqid)
        {
            string msg = "";
            try
            {
                string service_name = "common_query_service";//接口名
                string req = "";
                string uid = PublicRes.ConvertToFuid(qqid);
                if (uid == null || uid.Length < 3)
                {
                    throw new Exception("内部ID不正确！");
                }
                string spid = System.Configuration.ConfigurationManager.AppSettings["AtuoRenewQuerySpid"].ToString();
                //Fqqid+Fcur_type+Flist_state+Fbind_status
                //余额 channel_id:3  快捷:channel_id:8  ToOneDataset
                req = "flag=1&offset=0&limit=10&request_type=4052&reqid=208&fields=uid:" + uid + "|cur_type:1|list_state:1|spid:" + spid + "|channel_id:3";
                DataSet ds = null;
                DataSet ds1 = null;
                DataSet ds2 = null;
                ds1 = CommQuery.GetXmlToDataSetFromICE(req, "", "common_query_service", out msg);
                if (msg != "")
                    throw new Exception("Service处理失败！" + msg);

                req = "flag=1&offset=0&limit=10&request_type=4052&reqid=208&fields=uid:" + uid + "|cur_type:1|list_state:1|spid:" + spid + "|channel_id:8";
                ds2 = CommQuery.GetXmlToDataSetFromICE(req, "", "common_query_service", out msg);
                if (msg != "")
                    throw new Exception("Service处理失败！" + msg);
                ds = ToOneDataset(ds1, ds2);//合并结果集

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];

                    dt.Columns.Add("state", typeof(String));
                    foreach (DataRow dr in dt.Rows)
                    {
                        switch (dr["Fbind_status"].ToString())
                        {
                            case "0":
                                dr["state"] = "未定义"; break;
                            case "1":
                                dr["state"] = "初始状态"; break;
                            case "2":
                                dr["state"] = "开启"; break;
                            case "3":
                                dr["state"] = "关闭"; break;
                            case "4":
                                dr["state"] = "解除"; break;
                            case "5":
                                dr["state"] = "下发短信中"; break;
                            default:
                                dr["state"] = "未知"; break;
                        }
                    }
                }

                return ds;
            }
            catch (Exception e)
            {
                throw new Exception("Service处理失败！" + msg);
            }
        }

        [WebMethod(Description = "自动续费解绑")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public bool AtuoRenewUnbind(string qqid, string channel_id)
        {
            string msg = "";
            try
            {
                if (myHeader == null)
                {
                    throw new LogicException("不正确的调用方法！");
                }
                string service_name = "";
                string req = "";
                string spid = System.Configuration.ConfigurationManager.AppSettings["AtuoRenewQuerySpid"].ToString();
                if (channel_id == "8")
                {
                    service_name = "pl_modtrust_service";//基础代扣签约关系修改
                    DataSet ds = null;
                    long time = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
                    req = "cur_type=1&sp_id=" + spid + "&function=MOD_CANCAL&modify_chan=2&vali_type=100&uin=" + qqid + "&sp_timestamp=" + time;

                    ds = CommQuery.GetOneTableFromICE(req, "", service_name, true, out msg);
                    if (msg != "")
                        throw new Exception("Service处理失败！" + msg);
                    return true;
                }
                else if (channel_id == "3")
                {
                    service_name = "pl_deluid_service";//删除信任用户
                    DataSet ds = null;
                    req = "cur_type=1&opp_u=" + spid + "&user_type=2&channel_id=3&desc=KFWeb&uin=" + qqid + "&client_ip=" + myHeader.UserIP + "&vali_type=100";

                    ds = CommQuery.GetOneTableFromICE(req, "", service_name, true, out msg);
                    if (msg != "")
                        throw new Exception("Service处理失败！" + msg);
                    return true;
                }
                else
                    throw new Exception("Service处理失败！channel_id" + channel_id + "不对");
                return true;
            }
            catch (Exception e)
            {
                throw new Exception("Service处理失败！" + msg);
            }
        }

        [WebMethod(Description = "修改用户资料表记录")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public bool ModifyUserInfo(T_FIELD_VALUE[] Params)
        {
            if (myHeader == null)
            {
                throw new Exception("不正确的调用方法！");
            }
            string strUserID = myHeader.UserName;
            string strPassword = myHeader.UserPassword;
            string strIP = myHeader.UserIP;
            string strRightCode = "ModifyUserInfo";

            //PublicRes.CheckUserRight(strUserID,strPassword,strRightCode);

            try
            {
                M_USER_INFO mpl = new M_USER_INFO(Params);
                return mpl.Execute_30(myHeader.UserName, myHeader.UserIP);
            }
            catch (Exception e)
            {
                throw new Exception("service发生错误,请联系管理员！" + e.Message.Trim());
                return false;
            }
        }

        [WebMethod(Description = "修改用户帐户类型")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public bool modifyUserType(string qqid, string userType, string oldusertype, out string Msg)
        {
            Msg = null;

            if (qqid == null)
            {
                Msg = "传入的参数不完整！";
                return false;
            }


            //furion V30_FURION改动 20090310
            ICEAccess ice = new ICEAccess(PublicRes.ICEServerIP, PublicRes.ICEPort);
            try
            {
                //furion 20061116 email登录修改。
                string strID = PublicRes.ConvertToFuid(qqid);  //先转换成fuid
              
                // TODO1: 客户信息资料外移
              
                ice.OpenConn();
                string strwhere = "where=" + ICEAccess.URLEncode("fcurtype=1&");
                strwhere += ICEAccess.URLEncode("fuid=" + strID + "&");

                string strUpdate = "data=" + ICEAccess.URLEncode("fuser_type=" + userType);
                strUpdate += ICEAccess.ICEEncode("&fmodify_time=" + PublicRes.strNowTimeStander);

                string strResp = "";

                //3.0接口测试需要 furion 20090708
                if (ice.InvokeQuery_Exec(YWSourceType.用户资源, YWCommandCode.修改用户信息, strID, strwhere + "&" + strUpdate, out strResp) != 0)
                {
                    throw new Exception("修改个人账户类型出错！" + strResp);
                }


                string strSql = "uid=" + strID;
                strSql += "&modify_time=" + PublicRes.strNowTimeStander;
                strSql += "&user_type=" + userType;

                int iresult = CommQuery.ExecSqlFromICE(strSql, CommQuery.UPDATE_USERINFO, out Msg);

                if (iresult != 1)
                {
                    return false;
                }
                if (!userType.Trim().Equals(oldusertype.Trim()))
                {
                    string oldname = oldusertype.Trim() == "1" ? "个人" : "公司";
                    string newname = userType.Trim() == "1" ? "个人" : "公司";
                    // PublicRes.writeSysLog_kf(qqid, myHeader.UserName, myHeader.UserIP, oldname, newname, "帐户类型", "changeUserInfo", "");
                }

                Msg = "修改用户帐户类型成功！";
                return true;
            }
            catch (Exception e)
            {
                ice.CloseConn();
                Msg = "修改帐户类型失败！[" + e.Message.ToString().Replace("'", "’") + "]";
                SunLibrary.LoggerFactory.Get("Query_Service").Info("modifyUserType:" + Msg);
                return false;
            }
            finally
            {
                ice.Dispose();
            }
        }

        //furion 20060816
        [WebMethod(Description = "修改用户属性类型")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public bool modifyAttType(string qqid, string atttype, string oldattid, out string Msg)
        {
            Msg = null;

            if (qqid == null)
            {
                Msg = "传入的参数不完整！";
                return false;
            }

            try
            {
                //furion 20061116 email登录修改。
                string strID = PublicRes.ConvertToFuid(qqid);  //先转换成fuid
                string strSql = "uid=" + strID;
                strSql += "&attid=" + atttype;
                strSql += "&modify_time=" + CommQuery.ICEEncode(PublicRes.strNowTimeStander);

                if (CommQuery.ExecSqlFromICE(strSql, CommQuery.UPDATE_USERATT, out Msg) < 0)
                {
                    return false;
                }
                if (!atttype.Trim().Equals(oldattid.Trim()))
                {
                    string oldname = oldattid.Trim();
                    string newname = atttype.Trim();
                }

                Msg = "修改用户属性类型成功！";
                return true;
            }
            catch (Exception e)
            {
                Msg = "修改用户属性类型失败！[" + e.Message.ToString().Replace("'", "’") + "]";
                SunLibrary.LoggerFactory.Get("Query_Service").Info("modifyAttType:" + Msg);
                return false;
            }
        }



    }//class end

    public class CheckResult
    {
        public int recode; //0成功 1失败
        public string retdesc; //错误描述
        public string dealid; //处理ID
    }

    public class C2CCheckClass
    {
        public string listID;   //交易单ID号。
        public int type; //需要调整的类型。
        public string reason; //调整原因。
        public string uid; //用户名。
        public string pwd; //密码.
        public string time; //帐务时间.
        public string uip; //发起人IP。
        public int reBuyer; //退买家金额。
        public int reSaler; //退卖家金额。
    }


}//namespace
