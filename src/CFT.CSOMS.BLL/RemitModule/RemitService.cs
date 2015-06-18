using CFT.CSOMS.DAL.Infrastructure;
using CFT.CSOMS.DAL.RemitModule;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using TENCENT.OSS.CFT.KF.Common;

namespace CFT.CSOMS.BLL.RemitModule
{
    public class RemitService
    {
        /// <summary>
        /// 邮储汇款查询函数
        /// </summary>   
        public DataSet GetRemitDataList(string batchid, string tranType, string dataType, string remitType, string tranState, string spid, string remitRec, string listID, int start, int max)
        {
            try
            {
                DataSet ds = new RemitData().GetRemitDataList(batchid, tranType, dataType, remitType, tranState, spid, remitRec, listID, start, max);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    return null;
                }

                DataRow dr = ds.Tables[0].Rows[0];

                string str1 = dr["Ford_date"].ToString();
                string str2 = dr["Ford_ssn"].ToString();

                DataSet ds2 = new RemitData().GetRemitStateInfo(dr["Ford_date"].ToString(), dr["Ford_ssn"].ToString());

                if (ds2 == null || ds2.Tables.Count == 0 || ds2.Tables[0].Rows.Count == 0)
                {
                    return null;
                }

                ds.Tables[0].Columns.Add("Fdest_name", typeof(string));
                ds.Tables[0].Columns.Add("Fdest_card", typeof(string));
                ds.Tables[0].Columns.Add("Fstate", typeof(string));

                ds.Tables[0].Rows[0]["Fdest_name"] = ds2.Tables[0].Rows[0]["Fdest_name"].ToString();
                ds.Tables[0].Rows[0]["Fdest_card"] = ds2.Tables[0].Rows[0]["Fdest_card"].ToString();


                #region 转换字段

                switch (ds2.Tables[0].Rows[0]["Fstate"].ToString())
                {
                    case "1":
                        {
                            ds.Tables[0].Rows[0]["Fstate"] = "支付前"; break;
                        }
                    case "2":
                        {
                            ds.Tables[0].Rows[0]["Fstate"] = "支付成功"; break;
                        }
                    case "3":
                        {
                            ds.Tables[0].Rows[0]["Fstate"] = "邮储已收讫"; break;
                        }
                    case "4":
                        {
                            ds.Tables[0].Rows[0]["Fstate"] = "邮储收款确认"; break;
                        }
                    case "5":
                        {
                            ds.Tables[0].Rows[0]["Fstate"] = "邮储已兑付（收款人取钱）"; break;
                        }
                    case "6":
                        {
                            ds.Tables[0].Rows[0]["Fstate"] = "转入退汇"; break;
                        }
                    case "7":
                        {
                            ds.Tables[0].Rows[0]["Fstate"] = "退款成功"; break;
                        }
                    case "8":
                        {
                            ds.Tables[0].Rows[0]["Fstate"] = "已超期"; break;
                        }
                    case "9":
                        {
                            ds.Tables[0].Rows[0]["Fstate"] = "退款中"; break;
                        }
                    case "10":
                        {
                            ds.Tables[0].Rows[0]["Fstate"] = "挂起失败"; break;
                        }
                    default:
                        {
                            ds.Tables[0].Rows[0]["Fstate"] = "未定义的状态"; break;
                        }
                }


                if (ds != null && ds.Tables.Count > 0 || ds.Tables[0].Rows.Count > 0)
                {
                    ds.Tables[0].Columns.Add("FremitfeeName", typeof(String));
                    MoneyTransfer.FenToYuan_Table(ds.Tables[0], "Fremit_fee", "FremitfeeName");

                    ds.Tables[0].Columns.Add("FremitpayfeeName", typeof(String));
                    MoneyTransfer.FenToYuan_Table(ds.Tables[0], "Fremit_pay_fee", "FremitpayfeeName");

                    ds.Tables[0].Columns.Add("FprocedureName", typeof(String));
                    MoneyTransfer.FenToYuan_Table(ds.Tables[0], "Fprocedure", "FprocedureName");

                    ds.Tables[0].Columns.Add("FotherprocedureName", typeof(String));
                    MoneyTransfer.FenToYuan_Table(ds.Tables[0], "Fother_procedure", "FotherprocedureName");

                    ds.Tables[0].Columns.Add("FtranstateName", typeof(String));
                    ds.Tables[0].Columns.Add("FtrantypeName", typeof(String));
                    ds.Tables[0].Columns.Add("FremittypeName", typeof(String));
                    ds.Tables[0].Columns.Add("FdatatypeName", typeof(String));

                    foreach (DataRow dr2 in ds.Tables[0].Rows)
                    {
                        dr2.BeginEdit();

                        string tmp = dr2["Ftran_type"].ToString();
                        if (tmp == "1")
                            tmp = "汇款";
                        else if (tmp == "2")
                            tmp = "退汇";
                        else if (tmp == "3")
                            tmp = "改汇";
                        else if (tmp == "4")
                            tmp = "逾期退汇";
                        else
                            tmp = "状态未知:" + tmp;
                        dr2["FtrantypeName"] = tmp;

                        tmp = dr2["Ftran_state"].ToString();
                        if (tmp == "1")
                            tmp = "成功";
                        else if (tmp == "2")
                            tmp = "失败";
                        else if (tmp == "3")
                            tmp = "挂起";
                        else if (tmp == "4")
                            tmp = "挂起成功";
                        else if (tmp == "5")
                            tmp = "挂起失败";
                        else
                            tmp = "状态未知:" + tmp;
                        dr2["FtranstateName"] = tmp;

                        tmp = dr2["Fremit_type"].ToString();
                        if (tmp == "1")
                            tmp = "按地址汇款";
                        else if (tmp == "2")
                            tmp = "按密码汇款";
                        else if (tmp == "3")
                            tmp = "入账汇款";
                        else
                            tmp = "状态未知:" + tmp;
                        dr2["FremittypeName"] = tmp;

                        tmp = dr2["Fdata_type"].ToString();
                        if (tmp == "1")
                            tmp = "汇款直接成功";
                        else if (tmp == "2")
                            tmp = "汇款挂起";
                        else if (tmp == "3")
                            tmp = "汇款失败";
                        else if (tmp == "4")
                            tmp = "汇款挂起后成功";
                        else if (tmp == "5")
                            tmp = "汇款挂起后失败";
                        else if (tmp == "6")
                            tmp = "退汇成功";
                        else if (tmp == "7")
                            tmp = "退汇挂起";
                        else if (tmp == "8")
                            tmp = "退汇失败";
                        else if (tmp == "9")
                            tmp = "退汇挂起后成功";
                        else if (tmp == "10")
                            tmp = "退汇挂起后失败";
                        else if (tmp == "11")
                            tmp = "改汇成功";
                        else if (tmp == "12")
                            tmp = "改汇挂起";
                        else if (tmp == "13")
                            tmp = "改汇失败";
                        else if (tmp == "14")
                            tmp = "改汇挂起成功";
                        else if (tmp == "15")
                            tmp = "改汇挂起失败";
                        else if (tmp == "16")
                            tmp = "逾期退汇";
                        else if (tmp == "21")
                            tmp = "邮储线下退汇";
                        else
                            tmp = "状态未知:" + tmp;
                        dr2["FdatatypeName"] = tmp;

                        dr2.EndEdit();
                    }
                }

                #endregion

                return ds;
            }
            catch (Exception ex)
            {
                log4net.LogManager.GetLogger("GetRemitDataList:  查询邮政汇款失败! ： " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 邮储汇款记录数查询函数
        /// </summary>     
        public int GetRemitListCount(string batchid, string tranType, string dataType, string remitType, string tranState, string spid, string remitRec, string listID)
        {
            return new RemitData().GetRemitListCount(batchid, tranType, dataType, remitType, tranState, spid, remitRec, listID);
        }

        /// <summary>
        /// 获取邮储汇款商户
        /// </summary>
        /// <returns></returns>
        public DataTable GetRemitSpid()
        {
            try
            {
                string spidstr = System.Configuration.ConfigurationManager.AppSettings["RemitSpid"].ToString().Trim();
                //string spidstr = "2201160801|2000000501";

                if (spidstr != "")
                {
                    string[] spids = spidstr.Split('|');
                    return ConvertToTable("spid", spids);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                log4net.LogManager.GetLogger("获取邮储汇款商户失败!" + ex.Message);
                throw new LogicException("Service处理失败！");
            }
        }

        public static DataTable ConvertToTable(string columnName, string[] array)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(columnName, typeof(string));

            for (int i = 0; i < array.Length; i++)
            {
                DataRow dr = dt.NewRow();
                dr[columnName] = array[i].ToString();
                dt.Rows.Add(dr);
            }

            return dt;
        }

    }
}
