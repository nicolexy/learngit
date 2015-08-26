using CFT.CSOMS.DAL.CreditModule;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace CFT.CSOMS.BLL.CreditModule
{
    public class CreditCardService
    {
        /// <summary>
        /// 查询信用卡还款记录数
        /// </summary>
        /// <param name="Flistid"></param>
        /// <returns></returns>
        public DataSet GetCreditQueryListCount(string Flistid)
        {
            return new CreditData().GetCreditQueryListCount(Flistid);
        }

        /// <summary>
        /// 查询信用卡还款记录(按财付通号查询)
        /// </summary>
        /// <param name="QQOrEmail"></param>
        /// <param name="begindate"></param>
        /// <param name="enddate"></param>
        /// <param name="istart"></param>
        /// <param name="imax"></param>
        /// <returns></returns>
        public DataSet GetCreditQueryListForFaid(string QQOrEmail, DateTime begindate, DateTime enddate, int istart, int imax)
        {
            DataSet ds = new CreditData().GetCreditQueryListForFaid(QQOrEmail, begindate, enddate, istart, imax);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ds.Tables[0].Columns.Add("Fbank_name", typeof(string));
                ds.Tables[0].Columns.Add("Fnum_str", typeof(string));
                ds.Tables[0].Columns.Add("Fsign_str", typeof(string));

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    dr["Fbank_name"] = TransferMeaning.Transfer.convertbankType(dr["Fbank_type"].ToString());
                    dr["Fnum_str"] = !string.IsNullOrEmpty(dr["Fnum"].ToString()) ? MoneyTransfer.FenToYuan(dr["Fnum"].ToString()) : "";
                    string tmp = dr["Fsign"].ToString();
                    if (tmp == "1")
                    {
                        dr["Fsign_str"] = "成功";
                    }
                    else if (tmp == "2")
                    {
                        dr["Fsign_str"] = "失败";
                    }
                    else if (tmp == "3" || tmp == "4")
                    {
                        dr["Fsign_str"] = "还款中";
                    }
                    else
                    {
                        dr["Fsign_str"] = "UnKnown";
                    }
                }
            }

            return ds;
        }

        /// <summary>
        /// 查询信用卡还款记录
        /// </summary>
        /// <param name="Flistid"></param>
        /// <param name="istart"></param>
        /// <param name="imax"></param>
        /// <returns></returns>
        public DataSet GetCreditQueryList(string Flistid, int istart, int imax)
        {
            DataSet ds = new CreditData().GetCreditQueryList(Flistid, istart, imax);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ds.Tables[0].Columns.Add("Fbank_name", typeof(string));
                ds.Tables[0].Columns.Add("Fnum_str", typeof(string));
                ds.Tables[0].Columns.Add("Fsign_str", typeof(string));

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    dr["Fbank_name"] = TransferMeaning.Transfer.convertbankType(dr["Fbank_type"].ToString());
                    dr["Fnum_str"] = !string.IsNullOrEmpty(dr["Fnum"].ToString()) ? MoneyTransfer.FenToYuan(dr["Fnum"].ToString()) : "";
                    string tmp = dr["Fsign"].ToString();
                    if (tmp == "1")
                    {
                        dr["Fsign_str"] = "成功";
                    }
                    else if (tmp == "2")
                    {
                        dr["Fsign_str"] = "失败";
                    }
                    else if (tmp == "3" || tmp == "4")
                    {
                        dr["Fsign_str"] = "还款中";
                    }
                    else
                    {
                        dr["Fsign_str"] = "UnKnown";
                    }
                }
            }

            return ds;
        }
    }
}
