using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using CFT.CSOMS.DAL.CFTAccount;

namespace CFT.CSOMS.DAL.FundModule
{
    using CFT.CSOMS.DAL.Infrastructure;
    using System.Xml;

    public class FundRoll
    {
        //获取理财通交易记录  查基金交易（客户）表
        public DataTable QueryFundRollList(string u_QQID, DateTime u_BeginTime, DateTime u_EndTime, string Fcurtype, int istr, int imax, int Ftype)
        {
            string uid = AccountData.ConvertToFuid(u_QQID);
            if (uid == null || uid.Length < 3)
            {
                throw new Exception("内部ID不正确！");
            }

            using (var da = MySQLAccessFactory.GetMySQLAccess("Fund"))
            {
                var table_name = string.Format("fund_db_{0}.t_trade_user_fund_{1}", uid.Substring(uid.Length - 2), uid.Substring(uid.Length - 3, 1));

                var sqlBuilder = new StringBuilder(string.Format("select * from {0} where Fuid='{1}'", table_name, uid));
                string begin=u_BeginTime.ToString("yyyy-MM-dd HH:mm:ss");
                string end=u_EndTime.ToString("yyyy-MM-dd HH:mm:ss");
                if (begin != "1900-01-01 00:00:00" && end != "1900-01-01 00:00:00")
                {
                    sqlBuilder.AppendFormat(" and Facc_time between '{0}' and '{1}' ", begin, end);
                }
                if (!string.IsNullOrEmpty(Fcurtype))//基金公司
                    sqlBuilder.AppendFormat(" and Fcur_type = '{0}'", Fcurtype);
                if (Ftype != -1)
                {
                    if (Ftype == 1)//出入：入：Fpur_type =1 申购 Fstate 2,3状态的为申购成功
                        sqlBuilder.AppendFormat(" and Fpur_type = 1 and Fstate in (2,3) ");
                    if (Ftype == 2)//出入：出：Fpur_type =4 赎回  Fstate 5，10状态的为赎回成功
                        sqlBuilder.AppendFormat(" and Fpur_type =4  and Fstate in (5,10) ");
                }
                sqlBuilder.AppendFormat(" order by Facc_time desc limit " + istr + "," + imax);
                
                da.OpenConn();
                DataSet ds = da.dsGetTotalData(sqlBuilder.ToString());

                return ds.Tables[0];
            }
        }

        //从KF_SERVICE迁移过来的
        //注意有修改代码内容  int start = istr;//注意修改了
        public DataSet GetChildrenBankRollList(string u_QQID, DateTime u_BeginTime, DateTime u_EndTime, string Fcurtype, int istr, int imax, int Ftype, string Fmemo)
        {
            try
            {
                string fuid = AccountData.ConvertToFuid(u_QQID); 
                if (fuid == null || fuid == "" || fuid == "0")
                    return null;

                //reqid=124 是uid(180000000 - 1999999999)
                //reqid=117 是uid < 180000000的

                string reqid = "124";
                if (Int64.Parse(fuid) < 180000000)
                    reqid = "117";
                string inmsg = "fields=begin_time:" + CFT.CSOMS.COMMLIB.CommUtil.ICEEncode(u_BeginTime.ToString("yyyy-MM-dd HH:mm:ss"));
                inmsg += "|end_time:" + CFT.CSOMS.COMMLIB.CommUtil.ICEEncode(u_EndTime.ToString("yyyy-MM-dd HH:mm:ss"));
                inmsg += "|uid:" + fuid;
                inmsg += "|cur_type:" + Fcurtype;
                if (Ftype != 0)
                    inmsg += "|type:" + Ftype;
                if (!string.IsNullOrEmpty(Fmemo))
                    inmsg += "|memo:" + Fmemo;
                inmsg += "&flag=2";   //写死2,原因问小牛
                //int start = istr - 1;
                int start = istr;//注意修改了
                inmsg += "&offset=" + start.ToString();  //从0开始的
                inmsg += "&limit=" + imax;
                inmsg += "&reqid=" + reqid;

                string reply;
                short sresult;
                string msg;

                //common_simquery_service
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
                                        dr["Fpaynum1"] = CFT.CSOMS.COMMLIB.CommUtil.FenToYuan(dr["Fpaynum"].ToString());
                                        dr["FbalanceNum"] = CFT.CSOMS.COMMLIB.CommUtil.FenToYuan(dr["Fbalance"].ToString());

                                        dr["Fvs_qqid"] = "";
                                    }
                                    else
                                    {
                                        dr["Fpaynum2"] = CFT.CSOMS.COMMLIB.CommUtil.FenToYuan(dr["Fpaynum"].ToString());
                                        dr["FbalanceNum"] = CFT.CSOMS.COMMLIB.CommUtil.FenToYuan(dr["Fbalance"].ToString());

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
        
    }
}
