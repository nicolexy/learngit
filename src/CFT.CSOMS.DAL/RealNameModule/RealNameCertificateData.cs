using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Configuration;
using System.Net.Sockets;
using System.Net;
using System.Web;
using System.Collections;
using CFT.CSOMS.DAL.Infrastructure;
using TENCENT.OSS.CFT.KF.DataAccess;
using TENCENT.OSS.C2C.Finance.Common.CommLib;
using CommLib;
using CFT.CSOMS.COMMLIB;

namespace CFT.CSOMS.DAL.RealNameModule
{
    public class RealNameCertificateData
    {
        public DataSet AuMaintainWhiteListC(string src, string Operator, string uin, Int64 uid, int op_type, int valid_days, string sign)
        {
            string relayip = System.Configuration.ConfigurationManager.AppSettings["Auth_Relay_IP"];
            int relayport = int.Parse(System.Configuration.ConfigurationManager.AppSettings["Auth_Relay_Port"]);
            string requesttpe = System.Configuration.ConfigurationManager.AppSettings["AuMaintainWhiteListC_RequestType"];
            try
            {               
                string reqString = !string.IsNullOrEmpty(src) ? "src=" + src : "";
                reqString += !string.IsNullOrEmpty(Operator) ? "&operator=" + Operator : "";
                reqString += !string.IsNullOrEmpty(uin) ? "&uin=" + uin : "";
                reqString += "&uid=" + uid + "&op_type=" + op_type + "&valid_days=" + valid_days + "&sign=" + sign;
                return RelayAccessFactory.GetDSFromRelayAnsNotEncr(reqString, requesttpe, relayip, relayport);
            }
            catch (Exception err)
            {
                throw new Exception(string.Format("白名单设置:{0},{1}", relayip, err.Message));
            }

        }

        public DataSet BindQryBindMaskInfoC(string src, string Operator, Int64 uid, string sign)
        {
            string relayip = System.Configuration.ConfigurationManager.AppSettings["Auth_Relay_IP"];
            int relayport = int.Parse(System.Configuration.ConfigurationManager.AppSettings["Auth_Relay_Port"]);
            string requesttpe = System.Configuration.ConfigurationManager.AppSettings["BindQryBindMaskInfoC_RequestType"];
            try
            {               
                string reqString = !string.IsNullOrEmpty(src) ? "src=" + src : "";
                reqString += !string.IsNullOrEmpty(Operator) ? "&operator=" + Operator : "";
                reqString += "&uid=" + uid + "&sign=" + sign;
                return RelayAccessFactory.GetDSFromRelayAnsNotEncr(reqString, requesttpe, relayip, relayport);
            }
            catch (Exception err)
            {
                throw new Exception(string.Format("绑卡信息查询:{0},{1}", relayip, err.Message));
            }

        }


        public DataSet AuQryAuthStatusServiceByQueryType0(int query_type, string sp_id, string cre_id, string cre_type, string truename, string uin)
        {
            string relayip = System.Configuration.ConfigurationManager.AppSettings["Auth_Relay_IP"];
            int relayport = int.Parse(System.Configuration.ConfigurationManager.AppSettings["Auth_Relay_Port"]);
            string requesttpe = System.Configuration.ConfigurationManager.AppSettings["AuQryAuthStatusService_RequestType"];
            try
            {               
                string reqString = !string.IsNullOrEmpty(sp_id) ? "sp_id=" + sp_id : "";
                reqString += !string.IsNullOrEmpty(cre_id) ? "&cre_id=" + cre_id : "";
                reqString += !string.IsNullOrEmpty(cre_type) ? "&cre_type=" + cre_type : "";
                reqString += !string.IsNullOrEmpty(truename) ? "&truename=" + truename : "";
                reqString += !string.IsNullOrEmpty(uin) ? "&uin=" + uin : "";
                reqString += "&query_type=" + query_type;
                return RelayAccessFactory.GetDSFromRelay(reqString, requesttpe, relayip, relayport);
            }
            catch (Exception err)
            {
                throw new Exception(string.Format("实名状态和认证渠道信息查询:{0},{1}", relayip, err.Message));
            }
        }

        public DataSet AuQryAuthStatusServiceByQueryType0(int query_type, string sp_id, string cre_id, string cre_type, string truename, string uin, string src, string Operator, string sign, int request_detail_info)
        {
            string relayip = System.Configuration.ConfigurationManager.AppSettings["Auth_Relay_IP"];
            int relayport = int.Parse(System.Configuration.ConfigurationManager.AppSettings["Auth_Relay_Port"]);
            string requesttpe = System.Configuration.ConfigurationManager.AppSettings["AuQryAuthStatusService_RequestType"];
            try
            {             
                string reqString = !string.IsNullOrEmpty(sp_id) ? "sp_id=" + sp_id : "";
                reqString += !string.IsNullOrEmpty(cre_id) ? "&cre_id=" + cre_id : "";
                reqString += !string.IsNullOrEmpty(cre_type) ? "&cre_type=" + cre_type : "";
                reqString += !string.IsNullOrEmpty(truename) ? "&truename=" + truename : "";
                reqString += !string.IsNullOrEmpty(uin) ? "&uin=" + uin : "";
                reqString += !string.IsNullOrEmpty(src) ? "&src=" + src : "";
                reqString += !string.IsNullOrEmpty(Operator) ? "&operator=" + Operator : "";
                reqString += "&query_type=" + query_type + "&request_detail_info=" + request_detail_info + "&sign=" + sign;

                return RelayAccessFactory.GetDSFromRelay(reqString, requesttpe, relayip, relayport);
            }
            catch (Exception err)
            {
                throw new Exception(string.Format("实名状态和认证渠道信息查询:{0},{1}", relayip, err.Message));
            }
        }

        public DataSet AuQryAuthStatusServiceByQueryType1(int query_type, string uin, Int64 uid)
        {
            string relayip = System.Configuration.ConfigurationManager.AppSettings["Auth_Relay_IP"];
            int relayport = int.Parse(System.Configuration.ConfigurationManager.AppSettings["Auth_Relay_Port"]);
            string requesttpe = System.Configuration.ConfigurationManager.AppSettings["AuQryAuthStatusService_RequestType"];
            try
            {                
                string reqString = !string.IsNullOrEmpty(uin) ? "uin=" + uin : "";
                reqString += "&query_type=" + query_type + "&uid=" + uid;

                return RelayAccessFactory.GetDSFromRelay(reqString, requesttpe, relayip, relayport);
            }
            catch (Exception err)
            {
                throw new Exception(string.Format("实名状态和认证渠道信息查询:{0},{1}", relayip, err.Message));
            }
        }
        public DataSet AuQryAuthStatusServiceByQueryType1(int query_type, string uin, Int64 uid, int request_detail_info, string src, string Operator, string sign)
        {
            try
            {
                string relayip = System.Configuration.ConfigurationManager.AppSettings["Auth_Relay_IP"];
                int relayport = int.Parse(System.Configuration.ConfigurationManager.AppSettings["Auth_Relay_Port"]);
                string requesttpe = System.Configuration.ConfigurationManager.AppSettings["AuQryAuthStatusService_RequestType"];
                string reqString = !string.IsNullOrEmpty(uin) ? "uin=" + uin : "";
                reqString += !string.IsNullOrEmpty(src) ? "&src=" + src : "";
                reqString += !string.IsNullOrEmpty(Operator) ? "&operator=" + Operator : "";
                reqString += !string.IsNullOrEmpty(Operator) ? "&operator=" + Operator : "";
                reqString += "&uid=" + uid + "&query_type=" + query_type + "&request_detail_info=" + request_detail_info + "&sign=" + sign;

                return RelayAccessFactory.GetDSFromRelay(reqString, requesttpe, relayip, relayport);
            }
            catch (Exception err)
            {
                throw new Exception(string.Format("实名状态和认证渠道信息查询:{0},{1}", "10.123.9.160", err.Message));
            }
        }

        public DataSet PQueryUserInfoService(string uin, Int64 uid, string query_attach, int curtype)
        {
            string relayip = System.Configuration.ConfigurationManager.AppSettings["UserInfoQuota_Relay_IP"];
            int relayport = int.Parse(System.Configuration.ConfigurationManager.AppSettings["UserInfoQuota_Relay_Port"]);
            string requesttpe = System.Configuration.ConfigurationManager.AppSettings["PQueryUserInfoService_RequestType"];
            try
            {
                string reqString = "query_attach=" + query_attach + "&curtype=" + curtype;
                reqString += !string.IsNullOrEmpty(uin) ? "&uin=" + uin : "";
                reqString += (uid!=-1) ? "&uid=" + uid : "";

                return RelayAccessFactory.GetDSFromRelay(reqString, requesttpe, relayip, relayport);

            }
            catch (Exception err)
            {
                throw new Exception(string.Format("账户信息查询:{0},{1}", relayip, err.Message));
            }
        }

        public DataSet PQueryCreRelationC(string cre_id, int cre_type)
        {
            string relayip = System.Configuration.ConfigurationManager.AppSettings["UserInfoQuota_Relay_IP"];
            int relayport = int.Parse(System.Configuration.ConfigurationManager.AppSettings["UserInfoQuota_Relay_Port"]);
            string requesttpe = System.Configuration.ConfigurationManager.AppSettings["PQueryCreRelationC_RequestType"];
            try
            {
                string reqString = !string.IsNullOrEmpty(cre_id) ? "cre_id=" + cre_id : "";
                reqString += "&cre_type=" + cre_type;
                return RelayAccessFactory.GetDSFromRelay(reqString, requesttpe, relayip, relayport);
            }
            catch (Exception err)
            {
                throw new Exception(string.Format("证件下账户列表查询:{0},{1}", relayip, err.Message));
            }
        }


        public DataSet QuotaBanQueryC(int uid_type, Int64 uid, int have_cre_photocopy, string cre_id, int cre_type)
        {
            string relayip = System.Configuration.ConfigurationManager.AppSettings["UserInfoQuota_Relay_IP"];
            int relayport = int.Parse(System.Configuration.ConfigurationManager.AppSettings["UserInfoQuota_Relay_Port"]);
            string requesttpe = System.Configuration.ConfigurationManager.AppSettings["QuotaBanQueryC_RequestType"];
            try
            {
                string reqString = !string.IsNullOrEmpty(cre_id) ? "cre_id=" + cre_id : "";
                reqString += "&uid_type=" + uid_type + "&uid=" + uid + "&have_cre_photocopy=" + have_cre_photocopy + "&cre_type=" + cre_type;
                return RelayAccessFactory.GetDSFromRelayAnsNotEncr(reqString, requesttpe, relayip, relayport);
            }
            catch (Exception err)
            {
                throw new Exception(string.Format("账户限额数据查询:{0},{1}", relayip, err.Message));
            }
        }
        public DataSet QuotaBanQueryC(int uid_type, Int64 uid, int have_cre_photocopy)
        {
            string relayip = System.Configuration.ConfigurationManager.AppSettings["UserInfoQuota_Relay_IP"];
            int relayport = int.Parse(System.Configuration.ConfigurationManager.AppSettings["UserInfoQuota_Relay_Port"]);
            string requesttpe = System.Configuration.ConfigurationManager.AppSettings["QuotaBanQueryC_RequestType"];
            try
            {
                string  reqString = "uid_type=" + uid_type + "&uid=" + uid + "&have_cre_photocopy=" + have_cre_photocopy ;
                return RelayAccessFactory.GetDSFromRelayAnsNotEncr(reqString, requesttpe, relayip, relayport);
            }
            catch (Exception err)
            {
                throw new Exception(string.Format("账户限额数据查询:{0},{1}", relayip, err.Message));
            }
        }
    }
}
