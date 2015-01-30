using System;
using System.Collections.Generic;
using System.Linq;
using CFT.CSOMS.DAL.SPOA;
using System.Text;
using System.Data;
using TENCENT.OSS.C2C.Finance.Common.CommLib;
using CFT.CSOMS.DAL.Infrastructure;
using CFT.CSOMS.COMMLIB;

namespace CFT.CSOMS.BLL.SPOA
{
    /// <summary>
    /// 商户相关查询类
    /// </summary>
    public class MerchantService
    {

        public string GetMerchantCFuid(string spid)
        {
            return new MerchantData().GetMerchantCFuid(spid);
        }

        /// <summary>
        /// 商户证书到期查询
        /// </summary>
        /// <param name="crt_etime">证书到期到期时间</param>
        /// <param name="spid">商户号</param>
        /// <returns></returns>
        public DataSet QueryExpiredCertOperInfo(string crt_etime, string spid)
        {
            return new MerchantData().QueryExpiredCertOperInfo(crt_etime, spid);
        }

        /// <summary>
        ///  编辑商户证书到期备注
        /// </summary>
        /// <param name="spid"></param>
        /// <param name="memo"></param>
        /// <param name="crt_etime"></param>
        /// <param name="updateUser"></param>
        public bool EditExpiredCertMemo(string spid, string memo, string crt_etime, string updateUser)
        {
            try
            {
                if (new MerchantData().QueryExpiredCertExistOrNot(crt_etime, spid))//已存在则更新，否则插入
                {
                    new MerchantData().UpdateExpiredCertOperInfo(spid, memo, crt_etime, updateUser);
                }
                else
                {
                    new MerchantData().AddExpiredCertOperInfo(spid, memo, crt_etime, updateUser);
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("编辑商户证书到期备注异常：" + ex.Message);
            }

        }

        //查询商户联系信息
        public DataSet QuerySPContactInfo(string spid)
        {
            return new MerchantData().QuerySPContactInfo(spid);
        }

        //修改商户联系信息
        public bool InsertOrUpdateSPContactInfo(SPContact aci, string uid, string ip)
        {
            CFT.CSOMS.DAL.SPOA.SPContact con = new CFT.CSOMS.DAL.SPOA.SPContact();
            con.spid = aci.spid;
            con.name1 = aci.name1;
            con.standbya1 = aci.standbya1;
            con.tele1 = aci.tele1;
            con.mobile1 = aci.mobile1;
            con.email1 = aci.email1;

            con.name2 = aci.name2;
            con.tele2 = aci.tele2;
            con.qqnum2 = aci.qqnum2;
            con.email2 = aci.email2;

            con.name3 = aci.name3;
            con.tele3 = aci.tele3;
            con.qqnum3 = aci.qqnum3;
            con.email3 = aci.email3;

            con.name4 = aci.name4;
            con.tele4 = aci.tele4;
            con.qqnum4 = aci.qqnum4;
            con.email4 = aci.email4;

            con.name5 = aci.name5;
            con.tele5 = aci.tele5;
            con.qqnum5 = aci.qqnum5;
            con.email5 = aci.email5;

            con.name6 = aci.name6;
            con.tele6 = aci.tele6;
            con.qqnum6 = aci.qqnum6;
            con.email6 = aci.email6;

            con.name7 = aci.name7;
            con.tele7 = aci.tele7;
            con.qqnum7 = aci.qqnum7;
            con.email7 = aci.email7;

            return new MerchantData().InsertOrUpdateSPContactInfo(con, uid, ip);
        }

        public DataTable queryMerchantCertStat(int s_time, int e_time, string spid, string year)
        {
            return new MerchantData().queryMerchantCertStat(s_time, e_time, spid, year);
        }

        public DataSet QueryContract(string vendorName, string customerName, string contractNo,
                                    string startCreatedTime, string endCreatedTime,
                                    string startArchiveDay, string endArchiveDay,
                                    string startBeginDate, string endBeginDate,
                                    string startEndDate, string endEndDate,
                                    int start, int max)
        {
            return new MerchantData().QueryContract(vendorName, customerName, contractNo,
                                                      startCreatedTime, endCreatedTime,
                                                      startArchiveDay, endArchiveDay,
                                                      startBeginDate, endBeginDate,
                                                      startEndDate, endEndDate,
                                                      start, max);
        }

        public string GetContractState(string contractid)
        {
            return new MerchantData().GetContractState(contractid);
        }

        public DataTable QueryCertExpiredInfo(string spid,string startTime, string endTime)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("FSpid", typeof(String));
            dt.Columns.Add("FCertNum", typeof(String));
            dt.Columns.Add("FOldCertClinkIntTotal", typeof(String));
            dt.Columns.Add("FCertUsed_LastTime", typeof(String));
            dt.Columns.Add("FCertValidTimeEnd", typeof(String));
            dt.Columns.Add("FMixedStr", typeof(String));
            DataRow row = dt.NewRow();

            DataSet ds1 = new MerchantData().QueryCertExpiredInfo(spid,startTime, endTime);
            DataSet ds2 = new MerchantData().QueryCgiInfo(spid, startTime, endTime, 0, 1);
            if ((ds1 == null || ds1.Tables.Count == 0 && ds1.Tables[0].Rows.Count == 0) &&
                (ds2 == null || ds2.Tables.Count == 0 && ds2.Tables[0].Rows.Count == 0))
            {
                return null;
            }

            row.BeginEdit();
            if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
            {
                int num = 0;
                foreach (DataRow dr in ds1.Tables[0].Rows)
                {
                    num += int.Parse(dr["FOldCertClinkInt"].ToString());
                }
                row["FSpid"] = ds1.Tables[0].Rows[0]["FSpid"].ToString();
                row["FCertNum"] = ds1.Tables[0].Rows[0]["FCertNum"].ToString();
                row["FOldCertClinkIntTotal"] = num.ToString();
            }
            else
            {
                row["FSpid"] = spid;
                row["FCertNum"] ="";
                row["FOldCertClinkIntTotal"] ="";
            }


            if (ds2 != null && ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
            {
                row["FCertUsed_LastTime"] = ds2.Tables[0].Rows[0]["FModifyTime"].ToString();
                string validTime=ds2.Tables[0].Rows[0]["FCertValidTimeEnd"].ToString();
                row["FCertValidTimeEnd"] = CommUtil.GMT2Local(validTime).ToString("yyyy-MM-dd HH:mm:ss");
                if (ds2.Tables[0].Rows[0]["FMixed"].ToString() == "1")
                    row["FMixedStr"] = "否";
                else
                    row["FMixedStr"] = "是";
            }
            else
            {
                row["FCertUsed_LastTime"] = "";
                row["FCertValidTimeEnd"] = "";
                row["FMixedStr"] = "";
            }

            row.EndEdit();
            dt.Rows.Add(row);

            return dt;
        }
        public DataSet QueryCgiInfo(string spid,string startTime, string endTime, int start, int max)
        {
            return new MerchantData().QueryCgiInfo(spid, startTime, endTime, start, max);
        }
    }

    #region 商户联系信息
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

