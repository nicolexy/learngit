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

namespace CFT.CSOMS.DAL.BankcardUnbind
{
    public class BankcardbindData
    {        
        /// <summary>
        /// 查询绑卡记录
        /// </summary>
        /// <param name="Fbank_type"></param>
        /// <param name="bankID"></param>
        /// <param name="creType"></param>
        /// <param name="creID"></param>
        /// <param name="protocolno"></param>
        /// <param name="phoneno"></param>
        /// <param name="bindStatue"></param>
        /// <param name="limStart"></param>
        /// <param name="limCount"></param>
        /// <returns></returns>
        public DataSet GetBankCardBindList_UIN(string Fbank_type, string bankID,
            string creType, string creID, string protocolno, string phoneno, int bindStatue, int limStart, int limCount)
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

                da = MySQLAccessFactory.GetMySQLAccess("DataSource_ht");
                da.OpenConn();
                return da.dsGetTotalData(sql_findUID);
            }
            catch (Exception err)
            {
                throw new Exception("service处理错误：" + err.Message);
                //return null;
            }
            finally
            {
                if (da != null)
                {
                    da.Dispose();
                }
            }
        }



        /// <summary>
        /// /可能是当日绑定的卡，但是通过卡号查不到对应的uid，所以不能查到绑卡记录，就要查c2c_db_xx.t_card_bind_relation_x
        /// </summary>
        /// <param name="Fbank_type"></param>
        /// <param name="bankID"></param>
        /// <param name="creType"></param>
        /// <param name="creID"></param>
        /// <param name="protocolno"></param>
        /// <param name="phoneno"></param>
        /// <param name="bindStatue"></param>
        /// <param name="limStart"></param>
        /// <param name="limCount"></param>
        /// <returns></returns>
        public DataSet GetBankCardBindList_UIN_2(string Fbank_type, string bankID,
            string creType, string creID, string protocolno, string phoneno, int bindStatue, int limStart, int limCount)
        {
            MySqlAccess da = null;
            try
            {
                da = MySQLAccessFactory.GetMySQLAccess("BD");
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
                    return null;
                }
                sql_findUID_2 += sql_findUID_filter_2;
                DataSet ds = da.dsGetTableByRange(sql_findUID_2, limStart, limCount);
                return ds;
            }
            catch (Exception err)
            {
                throw new Exception("service处理错误：" + err.Message);
                //return null;
            }
            finally
            {
                if (da != null)
                {
                    da.Dispose();
                }
            }

        }




        /// <summary>
        /// 获得卡绑定详细信息
        /// </summary>
        /// <param name="fuid"></param>
        /// <param name="findex"></param>
        /// <param name="fBDIndex"></param>
        /// <returns></returns>
        public DataSet GetBankCardBindDetail(string fuid, string findex, string fBDIndex)
        {
            //MySqlAccess da = MySQLAccessFactory.GetMySQLAccess("DataSource_ht");
            MySqlAccess da=new MySqlAccess(DbConnectionString.Instance.GetConnectionString("BD"));
            try
            {
                // 2012/5/29 新增加查询字段Fcre_id
                da.OpenConn();
                string Sql = "";
                if (fBDIndex != null && fBDIndex == "1")
                {
                    Sql = "select Findex,Fbind_serialno,Fprotocol_no,Fuin,Fuid,Fbank_type,Fbind_flag,Fbind_type,Fbind_status," +
                                "Fbank_status,Fcard_tail,Fbank_id,Ftruename,Funchain_time_local,Fmodify_time," +
                                "Fmemo,Fcre_id,Ftelephone,Fmobilephone,Fcreate_time,Fbind_time_local,Fbind_time_bank,Funchain_time_bank,Fcre_type,Fonce_quota,Fday_quota,Fi_character2 & 0x01 as sms_flag from "
                                + PublicRes.GetTName("c2c_db", "t_user_bind", fuid) + " where Findex=" + findex + " and fuid=" + fuid;
                }
                else if (fBDIndex != null && fBDIndex == "2")//该Findex的记录在临时表
                {
                    Sql = "select Findex,Fbind_serialno,Fprotocol_no,Fuin,Fuid,Fbank_type,Fbind_flag,Fbind_type,Fbind_status," +
                                "Fbank_status,Fcard_tail,Fbank_id,Ftruename,Funchain_time_local,Fmodify_time," +
                                "Fmemo,Fcre_id,Ftelephone,Fmobilephone,Fcreate_time,Fbind_time_local,Fbind_time_bank,Funchain_time_bank,Fcre_type,Fonce_quota,Fday_quota,Fi_character2 & 0x01 as sms_flag from c2c_db.t_user_bind_tmp where Findex=" + findex + " and fuid=" + fuid;
                }
                return da.dsGetTotalData(Sql);
            }
            catch (Exception err)
            {
                throw new Exception("service处理错误：" + err.Message);
                //return null;
            }
            finally
            {
                da.Dispose();
            }
        }



         /// <summary>
        /// 根据条件查询绑定的卡列表 qqId不能为空
        /// </summary>
        /// <param name="fuin"></param>
        /// <param name="Fbank_type"></param>
        /// <param name="bankID"></param>
        /// <param name="uid"></param>
        /// <param name="creType"></param>
        /// <param name="creID"></param>
        /// <param name="protocolno"></param>
        /// <param name="phoneno"></param>
        /// <param name="strBeginDate"></param>
        /// <param name="strEndDate"></param>
        /// <param name="queryType"></param>
        /// <param name="isShowAboutDetail"></param>
        /// <param name="bindStatue"></param>
        /// <param name="bind_serialno"></param>
        /// <param name="limStart"></param>
        /// <param name="limCount"></param>
        /// <returns></returns>
        public DataSet GetBankCardBindList(string fuin, string Fbank_type, string bankID, string uid,
            string creType, string creID, string protocolno, string phoneno, string strBeginDate,
            string strEndDate, int queryType, bool isShowAboutDetail, int bindStatue, string bind_serialno, int limStart, int limCount)
        {
            MySqlAccess da = null;
            try
            {
                string filter = "(1=1)";
                string fuid = "";
                if (fuin != null && fuin.Trim() != "")
                {
                    //fuid = CFTAccountModule.AccountService.ConvertToFuid(fuin);
                    fuid = CFTAccount.AccountData.ConvertToFuid(fuin);
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

                    MySqlAccess da_findUID = MySQLAccessFactory.GetMySQLAccess("DataSource_ht");
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

                    /*
                    fuid = ds_findUID.Tables[0].Rows[0]["fuid"].ToString();
                    if(fuid == null || fuid.Trim() == "")
                        return null;
                    filter += " and fuid=" + fuid;
                    */

                    //throw new Exception("必须输入QQ帐号或内部ID作为查询条件！");
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

                da = MySQLAccessFactory.GetMySQLAccess("BD");
                da.OpenConn();
                // 有一个专门是Fprotocol_no分表的数据表，所以跟据条件判断查哪个表，因为功能目前暂缓，暂不做
                // 2012/5/29 新增查询证件号码项
                string Sql = "select 1 as FBDIndex , Findex,Fbind_serialno,Fprotocol_no,Fuin,Fuid,Fbank_type,Fbind_flag,Fbind_type,Fbind_status,Fbank_status,right(Fcard_tail,4) as Fcard_tail," +
                    "Fbank_id,Ftruename,Funchain_time_local,Fmodify_time,Fmemo,Fcre_id,Ftelephone,Fmobilephone,Fi_character4,Fbind_time_bank,Fbind_time_local from " + PublicRes.GetTName("c2c_db", "t_user_bind", fuid) + " where " + filter;
                //加查临时表
                string Sql2 = "select 2 as FBDIndex , Findex,Fbind_serialno,Fprotocol_no,Fuin,Fuid,Fbank_type,Fbind_flag,Fbind_type,Fbind_status,Fbank_status,right(Fcard_tail,4) as Fcard_tail," +
                    "Fbank_id,Ftruename,Funchain_time_local,Fmodify_time,Fmemo,Fcre_id,Ftelephone,Fmobilephone,Fi_character4,Fbind_time_bank,Fbind_time_local from c2c_db.t_user_bind_tmp where " + filter;
                Sql = Sql + " union all " + Sql2 + " limit " + limStart + "," + limCount;
                return da.dsGetTotalData(Sql);
            }
            catch (Exception err)
            {
                throw new Exception("service处理错误：" + err.Message);
                //return null;
            }
            finally
            {
                if (da != null)
                    da.Dispose();
            }
        }

     /// <summary>
        /// 查询一点通业务和快捷支付业务
     /// </summary>
     /// <param name="fuin"></param>
     /// <param name="Fbank_type"></param>
     /// <param name="bankID"></param>
     /// <param name="uid"></param>
     /// <param name="creID"></param>
     /// <param name="protocolno"></param>
     /// <param name="phoneno"></param>
     /// <param name="strBeginDate"></param>
     /// <param name="strEndDate"></param>
     /// <param name="bindStatue"></param>
     /// <param name="bind_serialno"></param>
     /// <param name="Operator"></param>
     /// <param name="bind_type"></param>
     /// <param name="cre_type"></param>
     /// <param name="limStart"></param>
     /// <param name="limCount"></param>
     /// <returns></returns>
        public DataSet GetBankCardBindList_New(string fuin, string Fbank_type, string bankID, string uid, string creID, string protocolno, string phoneno,
            string strBeginDate,string strEndDate, int bindStatue, string bind_serialno, string Operator,int bind_type,string cre_type, int limStart, int limCount)
        {
            string serverIp = CFT.Apollo.Common.Configuration.AppSettings.Get<string>("BankCardIP", "10.123.9.169");
            int serverPort = CFT.Apollo.Common.Configuration.AppSettings.Get<int>("BankCardPort", 22000);
            try
            {

                //cre_id=43030219851127080X
                //card_id=6214837554557000
                //mobile=1322953200
                //fuin=17934958
                //uid=295214000
                // limCount =10;
                // string dd = PublicRes.ConvertToFuid(fuin);

                //银行卡号加密
                //bankID = PublicRes.EncryptZerosPadding(bankID);
                if (!string.IsNullOrEmpty(fuin) && string.IsNullOrEmpty(uid))
                {
                    uid = PublicRes.ConvertToFuid(fuin);
                }


                string reqString = "operator=" + Operator + "&start=" + limStart + "&limit=" + limCount + "&bind_type=" + bind_type;
                //reqString += !string.IsNullOrEmpty(fuin) ? "&qqid=" + fuin : "";
                reqString += !string.IsNullOrEmpty(uid) ? "&uid=" + uid : "";
                reqString += !string.IsNullOrEmpty(bankID) ? "&card_id=" + bankID : "";
                reqString += !string.IsNullOrEmpty(creID) ? "&cre_id=" + creID : "";
                reqString += !string.IsNullOrEmpty(phoneno) ? "&mobile=" + phoneno : "";
                reqString += !string.IsNullOrEmpty(bind_serialno) ? "&bind_serialno=" + bind_serialno : "";
                reqString += !string.IsNullOrEmpty(protocolno) ? "&protocol_no=" + protocolno : "";
                reqString += bindStatue != 99 ? "&bind_status=" + bindStatue : "";
                reqString += !string.IsNullOrEmpty(cre_type) ? "&cre_type=" + cre_type : "";
                reqString += !string.IsNullOrEmpty(Fbank_type) ? "&bank_type=" + Fbank_type : "";
                reqString += !string.IsNullOrEmpty(strBeginDate) ? "&begin_time=" + strBeginDate : "";
                reqString += !string.IsNullOrEmpty(strEndDate) ? "&end_time=" + strEndDate : "";

                return RelayAccessFactory.GetDSFromRelayFromXML(reqString, "101140", serverIp, serverPort, true);
            }
            catch (Exception err)
            {
                throw new Exception(string.Format("查询一点通业务和快捷支付业务:ip:{0};{1}", serverIp, err.Message));
            }
           
        }

        /// <summary>
        /// 同步绑定信息
        /// </summary>
        /// <param name="bankType"></param>
        /// <param name="cardTail"></param>
        /// <param name="bankId"></param>
        /// <returns></returns>
        public bool SyncBankCardBind(string bankType, string cardTail, string bankId,string uid="")
        {
            //MySqlAccess da = MySQLAccessFactory.GetMySQLAccess("BD");
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

                //da.OpenConn();

                string bankID_md5 = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(bankID_Encode, "md5").ToLower();

                //string table = "c2c_db_" + cardTail.Substring(cardTail.Length - 2, 2) + ".t_card_bind_relation_" + cardTail.Substring(cardTail.Length - 3, 1);
                //string sql = "select Fuin from " + table + " where Fcard_id='" + bankID_md5 + "' and Flstate=1";
                //DataSet ds = da.dsGetTotalData(sql);

                
                string serverIp = CFT.Apollo.Common.Configuration.AppSettings.Get<string>("BankCardIP", "10.123.9.169");
                int serverPort = CFT.Apollo.Common.Configuration.AppSettings.Get<int>("BankCardPort", 22000);
                if (string.IsNullOrEmpty(uid)) 
                {
                    uid = "2000000501";
                }

                string reqString = "operator=" + uid + "&query_type=1&bank_id=" + bankID_md5 + "&card_tail=" + cardTail + "&bank_type=" + bankType;
                string res = RelayAccessFactory.RelayInvoke(reqString, "101140", true, false, serverIp, serverPort, "");
                //need2bank=1是对应卡关系表数量为0 
                //need2bank=2是对应卡关系表数量大于0
                if (res.Contains("need2bank=1"))
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
                throw new Exception("service处理错误：" + e.Message);
            }
            finally
            {
               // da.Dispose();
            }
            return false;
        }


        /// <summary>
        /// 普通解绑操作
        /// </summary>
        /// <param name="bankType"></param>
        /// <param name="qqid"></param>
        /// <param name="protocolNo"></param>
        public bool UnbindBankCardBind(String bankType, String qqid, String protocolNo, String userIP)
        {
            string msg = "";
            bool retVal = false;
            try
            {
                string req = "function=BIND_CANCEL";
                req += "&bank_type=" + bankType;
                req += "&qqid=" + qqid;
                req += "&protocol_no=" + protocolNo;
                req += "&login_ip=" + userIP;
                req += "&server_ip=" + userIP;
                string service_name = "bind_modi_service";
                DataSet ds = CommQuery.GetOneTableFromICE(req, "FINANCE_OD_UNBIND_BANKTYPE", service_name, out msg);
                if (ds == null || ds.Tables.Count < 1)
                {
                    throw new Exception(msg);
                }
                if (ds != null && ds.Tables.Count > 0 )
                {
                    DataTable dt = ds.Tables[0];
                    string req_params = "ver=1&request_type=8809";
                    string need_bank = "0";
                    if (dt.Columns.Contains("need2bank"))
                    {
                        need_bank = dt.Rows[0]["need2bank"].ToString();
                    }

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
                            throw new Exception("前置机返回结果有误：" + answer);
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
                            throw new Exception("前置机返回结果有误：" + answer);
                        }
                        else
                        {
                            retVal = ht["result"].ToString().Trim() == "0"; //result 返回为0时是正确的
                        }
                    }
                    else
                    {
                        if (dt.Columns.Contains("result"))
                        {
                            retVal = dt.Rows[0]["result"].ToString().Trim() == "0"; //result 返回为0时是正确的
                        }
                    }
                }
            }
            catch (Exception err)
            {
                throw new Exception("Service处理失败！" + err.Message);
            }
            return retVal;
        }


        /// <summary>
        /// 特殊解绑操作
        /// </summary>
        /// <param name="bankType"></param>
        /// <param name="qqid"></param>
        /// <param name="card_tail"></param>
        /// <param name="bindSerialno"></param>
        /// <param name="protocol_no"></param>
        /// <returns></returns>
        public bool UnBindBankCardBindSpecial(string bankType, string qqid, string card_tail, string bindSerialno, string protocol_no)
        {
            string msg = "";
            try
            {
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
                            throw new Exception(msg);
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
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    var res_info = dt.Rows[0]["res_info"];
                    if (res_info != null && res_info.ToString() == "ok")
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception err)
            {
                throw new Exception("Service处理失败！" + msg, err);
            }
        }
    }
}
