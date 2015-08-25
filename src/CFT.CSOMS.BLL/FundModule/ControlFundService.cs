using CFT.CSOMS.DAL.CFTAccount;
using CFT.CSOMS.DAL.FundModule;
using CFT.CSOMS.DAL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using TENCENT.OSS.C2C.Finance.BankLib;

namespace CFT.CSOMS.BLL.FundModule
{
    public class ControlFundService
    {
        /// <summary>
        /// 解除用户受控资金
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="cur_type">类型</param>
        /// <param name="balance">金额</param>
        /// <param name="opera">操作人</param>
        /// <param name="dt">解绑资金信息</param>
        /// <returns></returns>
        public bool RemoveUserControlFin(string qqid, string cur_type, string balance, string opera, int type, DataTable dt)
        {
            string fuid = AccountData.ConvertToFuid(qqid);
            //  string fuid = "540444925";

            if (fuid == null || fuid == "")
            {
                throw new Exception("根据C帐号获取Fuid失败	qqid:" + qqid);
            }
            if (fuid == null || fuid.Length < 3)
            {
                throw new Exception("内部ID不正确！");
            }
            if (new ControlFundData().RemoveUserControlFin(fuid, cur_type, balance, opera, type))
            {
                foreach (DataRow item in dt.Rows)
                {
                    new ControlFundData().RemoveControledFinLogInsert(qqid, item["FbalanceStr"].ToString(), item["FtypeText"].ToString(), item["cur_type"].ToString(), DateTime.Now, opera);
                }
                return true;
            }
            else return false;
        }

        //解绑单条用户受控资金
        public bool UnbindSingleCtrlFund(string qqid, string uid, string cur_type, string balance)
        {
            DataTable dt = QueryUserCtrlFund(qqid, uid);
            bool isExist = false;
            if (dt == null || dt.Rows.Count == 0)
            {
                return false; //解绑失败
            }
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["cur_type"].ToString().Trim() == cur_type.Trim() && dr["balance"].ToString().Trim() == balance.Trim())
                {
                    isExist = true;
                    string balanceStr = MoneyTransfer.FenToYuan(balance);
                    return RemoveUserControlFin(qqid, cur_type, balanceStr, uid, 3, dt);
                }
            }
            return isExist;
        }

        //解绑当前用户全部受控资金
        public bool UnbindAllCtrlFund(string qqid, string uid)
        {
            DataTable dt = QueryUserCtrlFund(qqid, uid);
            if (dt == null || dt.Rows.Count == 0)
            {
                return false;
            }
            return RemoveUserControlFin(qqid, "", "", uid, 4, dt);
        }

        public DataSet RemoveControledFinLogQuery(string qqid)
        {
            return new ControlFundData().RemoveControledFinLogQuery(qqid);
        }

        /// <summary>
        /// 查询用户受控资金
        /// </summary>
        /// <param name="qqid"></param>
        /// <param name="opera"></param>
        /// <returns></returns>
        public DataTable QueryUserCtrlFund(string qqid, string opera)
        {
            try
            {
                DataTable dt = QueryUserControledRecordCgi(qqid, opera);
                if (dt == null || dt.Rows.Count == 0)
                {
                    return null;
                }

                dt.Columns.Add("Fcur_typeName", typeof(string));
                dt.Columns.Add("FstateName", typeof(string));
                dt.Columns.Add("Fcreate_time", typeof(string));
                dt.Columns.Add("FtypeText", typeof(string));
                dt.Columns.Add("Fmodify_time", typeof(string));
                dt.Columns.Add("FbalanceStr", typeof(string));
                dt.Columns.Add("uid", typeof(string));
                dt.Rows[0]["FbalanceStr"] = MoneyTransfer.FenToYuan(dt.Rows[0]["balance"].ToString().Trim());

                foreach (DataRow dr in dt.Rows)
                {
                    string cur_type = dr["cur_type"].ToString();
                    DataSet ds = QueryUserControledRecord(qqid, "", "", cur_type, 0, 1);
                    if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    {
                        dr["Fcur_typeName"] = "";
                        dr["FstateName"] = "";
                        dr["Fcreate_time"] = "";
                        dr["Ftype_Text"] = "";
                        dr["Fmodify_time"] = "";
                        dr["uid"] = "";
                    }
                    else
                    {
                        DataRow row = ds.Tables[0].Rows[0];
                        dr["Fcur_typeName"] = row["Fcur_typeName"].ToString();
                        dr["FstateName"] = row["FlstateName"].ToString();
                        dr["Fcreate_time"] = row["Fcreate_time"].ToString();
                        dr["FtypeText"] = row["FtypeText"].ToString();
                        dr["Fmodify_time"] = row["Fmodify_time"].ToString();
                        dr["uid"] = row["uid"].ToString();
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                log4net.LogManager.GetLogger("QueryUserCtrlFund 查询受控资金异常： " + ex.Message);
                throw new Exception("查询记录为空" + ex.Message);
            }
        }

        public DataTable QueryUserControledRecordCgi(string qqid, string opera)
        {

            if (string.IsNullOrEmpty(qqid))
            {
                throw new ArgumentNullException("qqid为空！");
            }
            if (string.IsNullOrEmpty(opera))
            {
                throw new ArgumentNullException("opera为空！");
            }

            string fuid = AccountData.ConvertToFuid(qqid);

            if (fuid == null || fuid == "")
            {
                throw new Exception("根据C帐号获取Fuid失败	qqid:" + qqid);
            }
            if (fuid == null || fuid.Length < 3)
            {
                throw new Exception("内部ID不正确！");
            }
            return new ControlFundData().QueryUserControledRecordCgi(fuid, opera);
        }

        public DataSet QueryUserControledRecord(string qqid, string strBeginDate, string strEndDate, string cur_type, int iNumStart, int iNumMax)
        {
            try
            {
                string fuid = new CFTAccountModule.AccountService().QQ2Uid(qqid);
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
                        dr["FtypeText"] = TransferMeaning.Transfer.convertbankType(type);
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
            catch (Exception err)
            {
                throw new Exception("Service处理失败！" + err.Message);
            }
        }

    }
}
