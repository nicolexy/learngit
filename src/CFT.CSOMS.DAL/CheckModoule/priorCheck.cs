using CFT.CSOMS.DAL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using TENCENT.OSS.C2C.Finance.BankLib;
using TENCENT.OSS.C2C.Finance.Common.CommLib;
using TENCENT.OSS.C2C.Finance.DataAccess;
using TENCENT.OSS.CFT.KF.DataAccess;

namespace CFT.CSOMS.DAL.CheckModoule
{
    /// <summary>
    /// 定义一个前置的检查权限的接口
    /// </summary>
    public interface IpriorCheck
    {
        bool checkRight(Param[] pa, out string Msg);
    }

    /// <summary>
    /// 实现最后操作的接口。
    /// </summary>
    public interface IBaseDoCheck
    {
        bool DoAction(string strCheckID, string user, string ip);
    }

    /// <summary>
    /// priorCheck 的摘要说明。
    /// </summary>
    public class priorCheck
    {
        public priorCheck()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        /// <summary>
        /// 根据每一种不同类型的审批，分别处理不同的权限检查。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static IpriorCheck GetHandler(string str)
        {
            switch (str)
            {
                case "ChangeQQ":                  //furion 20051030 加入修改QQ
                    return new ChangeQQClass();
                case "DKAdjustFail": //客服系统发起的代扣调整状态审批
                    return new DKAdjustFailClass();
                default:
                    return null;
            }

            return null;
        }


        public class ChangeQQClass : IpriorCheck, IBaseDoCheck
        {
            //逻辑：如果同种类型的话可以更改。
            //如果不同类型的话，需要判断新的帐号是否是旧帐号的相关属性信息。
            private bool CheckTwoQQ(string oldqq, string newqq, out string Msg)
            {
                Msg = "";

                if (oldqq == null || oldqq.Trim() == "")
                {
                    Msg = "请给出原帐户！";
                    return false;
                }

                if (newqq == null || newqq.Trim() == "")
                {
                    Msg = "请给出新帐户号码！";
                    return false;
                }

                TENCENT.OSS.CFT.KF.Common.enmQQType oldtype = TENCENT.OSS.CFT.KF.Common.DESCode.GetQQType(oldqq.Trim());
                TENCENT.OSS.CFT.KF.Common.enmQQType newtype = TENCENT.OSS.CFT.KF.Common.DESCode.GetQQType(newqq.Trim());

                //furion 20061117 email登录修改
                if (oldtype == TENCENT.OSS.CFT.KF.Common.enmQQType.UNKNOWN || newtype == TENCENT.OSS.CFT.KF.Common.enmQQType.UNKNOWN)
                {
                    Msg = "请给出正确的帐户号码";
                    return false;
                }

                string olduid3 = "";
                if (oldtype == TENCENT.OSS.CFT.KF.Common.enmQQType.EMAIL)
                {
                    if (!TENCENT.OSS.CFT.KF.Common.DESCode.GetEmailUid(oldqq, out olduid3))
                    {
                        Msg = "解析EMAIL时出现错误！" + olduid3;
                        return false;
                    }
                }
                else
                    olduid3 = oldqq.Trim();

                string newuid3 = "";
                if (newtype == TENCENT.OSS.CFT.KF.Common.enmQQType.EMAIL)
                {
                    if (!TENCENT.OSS.CFT.KF.Common.DESCode.GetEmailUid(newqq, out newuid3))
                    {
                        Msg = "解析EMAIL时出现错误！" + newuid3;
                        return false;
                    }
                }
                else
                    newuid3 = newqq.Trim();

                try
                {
                    string strSql = "uin=" + oldqq + "&wheresign=1";
                    string struid = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_RELATION, "fuid", out Msg);

                    if (struid == null)
                    {
                        Msg = "原帐户不存在或已作废！";
                        return false;
                    }


                    strSql = "uin=" + newqq;
                    struid = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_RELATION, "fuid", out Msg);
                    if (struid != null)
                    {
                        Msg = "新帐号已经是财付通帐户，请重新选择帐号！";
                        return false;
                    }

                    //furion 新加判断，同类切换允许，不同类切换时原号的新属性必须未绑定。
                    if (newtype != oldtype)
                    {
                        // TODO: 1客户信息资料外移
                        string fuid = PublicRes.ConvertToFuid(oldqq);

                        strSql = "uid=" + fuid;
                        string fieldstr = "";

                        if (newtype == TENCENT.OSS.CFT.KF.Common.enmQQType.QQ)
                        {
                            fieldstr = "Fqqid";
                        }
                        else if (newtype == TENCENT.OSS.CFT.KF.Common.enmQQType.EMAIL)
                        {
                            fieldstr = "Femail";
                        }
                        else if (newtype == TENCENT.OSS.CFT.KF.Common.enmQQType.MOBILE)
                        {
                            fieldstr = "FMobile";
                        }
                        string oldotherid = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_USERINFO, fieldstr, out Msg);

                        if (oldotherid != "")
                        {
                            string oldotheruid3 = oldotherid;


                            strSql = "uin=" + oldotheruid3 + "&wheresign=1";
                            struid = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_RELATION, "fuid", out Msg);
                            if (struid != null)
                            {
                                string strmsg = "";
                                if (newtype == TENCENT.OSS.CFT.KF.Common.enmQQType.EMAIL)
                                {
                                    strmsg = "EMAIL";
                                }
                                else if (newtype == TENCENT.OSS.CFT.KF.Common.enmQQType.MOBILE)
                                {
                                    strmsg = "手机号";
                                }
                                else if (newtype == TENCENT.OSS.CFT.KF.Common.enmQQType.QQ)
                                {
                                    strmsg = "QQ号";
                                }

                                Msg = "旧帐号的" + strmsg + "已绑定为帐号类型，无法修改为新帐号。";
                                return false;
                            }
                        }
                    }

                    return true;
                }
                finally
                {
                    //da.Dispose();
                    //da_zl.Dispose();
                }
            }

            public bool checkRight(Param[] pa, out string Msg)
            {
                Msg = "";


                //获得已有参数.
                string oldqq = "";
                string newqq = "";

                foreach (Param param in pa)
                {
                    if (param.ParamName.ToLower().Trim() == "oldqq")
                    {
                        oldqq = param.ParamValue.Trim();
                    }
                    else if (param.ParamName.ToLower().Trim() == "newqq")
                    {
                        newqq = param.ParamValue.Trim();
                    }
                }

                return CheckTwoQQ(oldqq, newqq, out Msg);

            }

            #region IBaseDoCheck 成员

            public bool DoAction(string strCheckID, string user, string ip)
            {
                //已改动 furion V30_FURION核心查询需改动 用接口实现修改
                MySqlAccess daht = new MySqlAccess(PublicRes.GetConnString("HT"));

                string ICEServerIP = System.Configuration.ConfigurationManager.AppSettings["ICEServerIP"].ToString();
                int ICEPort = int.Parse(System.Configuration.ConfigurationManager.AppSettings["ICEPort"].ToString());

                ICEAccess ice = new ICEAccess(ICEServerIP, ICEPort);
                try
                {
                    string strSql = "Select * from c2c_fmdb.t_check_param where fcheckid =" + strCheckID;

                    daht.OpenConn();

                    DataTable dt = daht.GetTable(strSql);

                    if (dt == null || dt.Rows.Count == 0)
                    {
                        throw new Exception("审批参数已丢失！");
                    }

                    string oldqq = "";
                    string newqq = "";
                    string memo = "";

                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dr["FKey"].ToString().Trim().ToLower() == "oldqq")
                        {
                            oldqq = dr["FValue"].ToString().Trim();
                        }
                        else if (dr["FKey"].ToString().Trim().ToLower() == "newqq")
                        {
                            newqq = dr["FValue"].ToString().Trim();
                        }
                        else if (dr["FKey"].ToString().Trim().ToLower() == "memo")
                        {
                            memo = dr["FValue"].ToString().Trim();
                        }
                    }

                    //改为调用接口实现

                    string inmsg1 = "&uin=" + oldqq;
                    inmsg1 += "&new_uin=" + newqq;
                    inmsg1 += "&memo=" + ICEAccess.ICEEncode(memo);
                    inmsg1 += "&op_type=3";
                    inmsg1 += "&watch_word=" + PublicRes.GetWatchWord("ui_acc_replacement_service");

                    string reply = "";
                    string msg = "";
                    short result = -1;

                    if (commRes.middleInvoke("ui_acc_replacement_service", inmsg1, true, out reply, out result, out msg))
                    {
                        if (result != 0)
                        {
                            msg = "转换接口ui_acc_replacement_service返回失败：result=" + result + "，msg=" + msg;
                            return false;
                        }
                        else
                        {
                            if (reply.IndexOf("result=0") > -1)
                            {
                                return true;
                            }
                            else
                            {
                                msg = "转换接口ui_acc_replacement_service返回失败：reply=" + reply;
                                return false;
                            }
                        }
                    }
                    else
                    {
                        return false;
                    }

                }
                catch (Exception err)
                {
                    ice.CloseConn();

                    log4net.ILog log = log4net.LogManager.GetLogger("ChangeQQClass.DoAction");
                    if (log.IsErrorEnabled) log.Error(strCheckID, err);

                    throw;
                }
                finally
                {
                    daht.Dispose();
                    ice.Dispose();
                }
            }

            #endregion
        }

        public class DKAdjustFailClass : IpriorCheck
        {


            #region IpriorCheck 成员

            public bool checkRight(Param[] pa, out string Msg)
            {
                Msg = "";

                string batchid = "";
                string checkmemo = "";

                foreach (Param dr in pa)
                {
                    if (dr.ParamName == "batchid")
                    {
                        batchid = dr.ParamValue.Trim();
                    }

                    if (dr.ParamName == "checkmemo")
                    {
                        checkmemo = dr.ParamValue.Trim();
                    }
                }

                if (batchid == "")
                {
                    Msg = "此审批单参数有误";
                    return false;
                }

                MySqlAccess dainc = new MySqlAccess(PublicRes.GetConnString("INC_NEW"));
                try
                {
                    dainc.OpenConn();
                    //0初始标记　１已发起审批　２审批成功并执行　３撤消审批（允许再发起）
                    //7 已发送银行   8 交易成功   9 交易失败

                    string strSql = "update cft_cep_db.t_cep_adjust set Fcheckstate=1,Fmodify_time=now(),Fendstate=9 where FCheckBatchID ='" + batchid + "' and Fcheckstate=0";

                    dainc.ExecSqlNum(strSql);

                    return true;
                }
                catch (Exception err)
                {
                    Msg = err.Message;

                    log4net.ILog log = log4net.LogManager.GetLogger("AdjustData_DirClass.checkRight");
                    if (log.IsErrorEnabled) log.Error(Msg, err);

                    return false;
                }
                finally
                {
                    dainc.Dispose();
                }
            }

            #endregion


        }
    }
}
