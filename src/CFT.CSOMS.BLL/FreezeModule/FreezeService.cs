using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CFT.CSOMS.DAL.FreezeModule;
using System.Data;
using CFT.CSOMS.DAL.Infrastructure;

namespace CFT.CSOMS.BLL.FreezeModule
{
    public class FreezeService
    {
        public void SendWechatMsg(string reqsource, string accid, string templateid, string cont1, string cont2, string cont3, string msg_type)
        {
            new FreezeData().SendWechatMsg(reqsource, accid, templateid, cont1, cont2, cont3, msg_type);
        }

        public bool SyncCreid(string uin, string oldCredId, string newCredId, int creType, string optUser, string ip)
        {
            return new FreezeData().SyncCreid(uin, oldCredId, newCredId, creType, optUser, ip);
        }

        public string GetCashOutFreezeListId(string Uid)
        {
            return (new FreezeData()).GetCashOutFreezeListId(Uid);
        }

        /// <summary>
        /// 更新冻结备注函数
        /// </summary>
        public void UpdateFreezeListDetail(string fid, string FreezeReason, string UserName, string UserIP)
        {
            (new FreezeData()).UpdateFreezeListDetail(fid, FreezeReason, UserName, UserIP);
        }

        /// <summary>
        /// 冻结查询函数
        /// </summary>
        public DataSet GetFreezeList(DateTime u_BeginTime, DateTime u_EndTime, string freezeuser, string username, int handletype, int statetype, string qqid, int iPageStart, int iPageMax)
        {
            return (new FreezeData()).GetFreezeList(u_BeginTime, u_EndTime, freezeuser, username, handletype, statetype, qqid, iPageStart, iPageMax);
        }

        public int GetFreezeListCount(DateTime u_BeginTime, DateTime u_EndTime, string freezeuser, string username, int handletype, int statetype, string qqid)
        {
            return (new FreezeData()).GetFreezeListCount(u_BeginTime, u_EndTime, freezeuser, username, handletype, statetype, qqid);
        }

        /// <summary>
        /// 冻结查询详细函数
        /// </summary>
        /// <param name="tdeid"></param>
        /// <returns></returns>
        public DataSet GetFreezeListDetail(string tdeid)
        {
            DataSet ds = new FreezeData().GetFreezeListDetail(tdeid);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ds.Tables[0].Columns.Add("strFFreezeType", typeof(string));
                ds.Tables[0].Columns.Add("strFfreeze_channel", typeof(string));

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string tmp = PublicRes.GetString(dr["FFreezeType"]);
                    if (tmp == "1")
                    {
                        dr["strFFreezeType"] = "冻结帐户";
                    }
                    else if (tmp == "2")
                    {
                        dr["strFFreezeType"] = "锁定交易单";
                    }

                    tmp = PublicRes.GetString(dr["Ffreeze_channel"]);
                    if (tmp == "1")
                    {
                        dr["strFfreeze_channel"] = "风控冻结";
                    }
                    else if (tmp == "2")
                    {
                        dr["strFfreeze_channel"] = "拍拍冻结";
                    }
                    else if (tmp == "3")
                    {
                        dr["strFfreeze_channel"] = "用户冻结";
                    }
                    else if (tmp == "4")
                    {
                        dr["strFfreeze_channel"] = "商户冻结";
                    }
                    else if (tmp == "5")
                    {
                        dr["strFfreeze_channel"] = "BG接口冻结";
                    }
                    else
                    {
                        dr["strFfreeze_channel"] = "无冻结渠道";
                    }
                }
            }

            return ds;
        }

        /// <summary>
        /// 用户冻结记录查询
        /// </summary>
        public DataSet QueryUserFreezeRecord(string strBeginDate, string strEndDate, string fpayAccount, double freezeFin, string flistID, int iPageStart, int iPageMax)
        {         
            try
            {
                DataSet ds = new FreezeData().QueryUserFreezeRecord(strBeginDate, strEndDate, fpayAccount, freezeFin, flistID, iPageStart, iPageMax);

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

                    dr["strFreason"] = TransferMeaning.Transfer.convertSubject(dr["Fsubject"].ToString());

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

                    dr["strBankName"] = TransferMeaning.Transfer.convertbankType(dr["Fbank_type"].ToString());

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
            catch (Exception err)
            {
                throw new Exception("Service处理失败！");
            }           
        }
    }
}
