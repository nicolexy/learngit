using CFT.CSOMS.DAL.CheckModoule;
using CFT.CSOMS.DAL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TENCENT.OSS.C2C.Finance.Common.CommLib;
using TENCENT.OSS.CFT.KF.DataAccess;

namespace CFT.CSOMS.BLL.CheckModoule
{

    /// <summary>
    /// FinishCheck 的摘要说明。
    /// </summary>
    public class FinishCheck
    {
        public FinishCheck()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        /// <summary>
        /// 执行审批(适用于审批人执行类型)
        /// </summary>
        /// <param name="strCheckID">审批ID</param>
        /// <param name="myHeader">SOAP头</param>
        /// <param name="da">数据访问对象</param>
        /// <returns>是否成功</returns>
        public static bool ExecuteCheck(string strCheckID, string user, string ip, MySqlAccess da)
        {
            string strCheckType = GetCheckType(strCheckID);

            if (!PublicRes.IgnoreLimitCheck)
            {
                //furion 20060208 需要加入一个预先检查,检查完后置成执行中状态以避免重复执行.
                if (!PriorCheck(strCheckID, da))
                {
                    throw new Exception("审批的状态不正确,请确认是否是重复执行");
                }
            }
            //加入完毕

            bool flag = false;
            string errmsg = "";

            try
            {
                flag = CheckFactory.ReturnHandler(strCheckType).DoAction(strCheckID, user, ip);
            }
            catch (Exception err)
            {
                errmsg = err.Message;
                flag = false;
            }

            if (!flag)
            {
                LastCheck(strCheckID, da);
                throw new Exception("执行审批完成后的动作时失败！" + errmsg);
            }
            else
            {
                if (FinishedCheck(strCheckID, da))
                {
                    return true;
                }
                else
                {
                    throw new Exception("执行审批完成后的动作时成功，但是在执行更改审批状态时失败！");
                }
            }
        }

        /// <summary>
        /// 如果执行审批完成动作不成功，再改回原状态
        /// </summary>
        /// <param name="strCheckID"></param>
        /// <param name="da"></param>
        private static void LastCheck(string strCheckID, MySqlAccess da)
        {
            da.ExecSql("update c2c_fmdb.t_check_main set FNewState=2 where FID=" + strCheckID);
        }

        /// <summary>
        /// 更新审批为已执行状态
        /// </summary>
        /// <param name="strCheckID">审批ID</param>
        /// <param name="da">数据访问对象</param>
        /// <returns>是否成功</returns>
        public static bool FinishedCheck(string strCheckID, MySqlAccess da)
        {
            return da.ExecSql("update c2c_fmdb.t_check_main set FNewState=3 where FID=" + strCheckID);
        }

        /// <summary>
        /// 为了不多次执行，先把状态改成一个执行中。。。
        /// </summary>
        /// <param name="strCheckID"></param>
        /// <param name="da"></param>
        /// <returns></returns>
        private static bool PriorCheck(string strCheckID, MySqlAccess da)
        {
            string strSql = "select FNewState from c2c_fmdb.t_check_main where FID=" + strCheckID;
            string state = da.GetOneResult(strSql).Trim();
            if (state != "2")
            {
                return false;
            }
            else
            {
                strSql = "update c2c_fmdb.t_check_main set FNewState=9 where FID=" + strCheckID;
                return da.ExecSqlNum(strSql) == 1;
            }
        }

        private static string GetCheckType(string strCheckID)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("HT"));
            try
            {
                da.OpenConn();
                return da.GetOneResult("select FCheckType from c2c_fmdb.t_check_main where FID=" + strCheckID);
            }
            finally
            {
                da.Dispose();
            }
        }

        /// <summary>
        /// 实现最后操作的接口。
        /// </summary>
        public interface IBaseDoCheck
        {
            bool DoAction(string strCheckID, string user, string ip);
        }

        /// <summary>
        /// 审批工厂类。
        /// </summary>
        public class CheckFactory
        {
            //		public enum ActionShow
            //		{
            //			allHide = 0,
            //			czcz01  = 1, //充值冲正
            //			czjy02  = 2, //支付成功冲正
            //			czfk03  = 3, //买家确认冲正.
            //			cztk04  = 4, //退款冲正.
            //			czcz05  = 5, //充值冲正二期.
            //			cztx06  = 6, //提现冲正二期.
            //			czhd07  = 7, //付款结果回导冲正
            //			czzz08  = 8, //转帐冲正。
            //			allshow = 9
            //		}

            /// <summary>
            /// 根据审批类型获取处理类
            /// </summary>
            /// <param name="strCheckType">审批类型</param>
            /// <returns>处理类</returns>
            public static IBaseDoCheck ReturnHandler(string strCheckType)
            {
                switch (strCheckType)
                {
                    case "Mediation": //仲裁判断(修改敏感信息)
                        return new Mediation();   //没涉及到资金流水库

                    default:
                        return null;
                        break;
                }
            }
        }

        public class Mediation : IBaseDoCheck
        {
            #region IBaseDoCheck 成员

            public bool DoAction(string strCheckID, string user, string ip)
            {
                return false;

                //修改密码service
                string Msg = null;

                string strFetchNo = "select fobjID from c2c_fmdb.t_check_main where fid = '" + strCheckID + "'";
                string fetchID = PublicRes.ExecuteOne(strFetchNo, "ht");

                string[] br = new string[3];
                string strCheckInfo = "select FcheckUser,FcheckMemo,FcheckTime from c2c_fmdb.t_check_list where fcheckid = '" + strCheckID + "'";
                br[0] = "FcheckUser";
                br[1] = "FcheckMemo";
                br[2] = "FcheckTime";
                br = PublicRes.returnDrData(strCheckInfo, br, "ht");

                string checkUsr = br[0];
                string checkMemo = br[1];
                string checkTime = br[2];

                string[] ar = new string[9];
                string str = "Select * from c2c_fmdb.t_mediation where FfetchID='" + fetchID + "'";
                ar[0] = "Fqqid";
                ar[1] = "FfetchMail";
                ar[2] = "fcleanMibao";
                ar[3] = "freason";
                ar[4] = "FBasePath";
                ar[5] = "FAccPath";
                ar[6] = "FIDCardPath";
                ar[7] = "FbankCardPath";
                ar[8] = "FNameNew";

                ar = PublicRes.returnDrData(str, ar, "ht");

                string qqid = ar[0];
                string mail = ar[1];
                string cleanMimao = ar[2];
                string reason = ar[3];
                string pathBase = ar[4];
                string pathAcc = ar[5];
                string pathIDCard = ar[6];
                string pathBank = ar[7];
                string changedName = ar[8];


                bool opSign;

                //此处应该用接口来实现，结构更加清晰
                if (fetchID.Substring(0, 3) == "101")  //修改姓名
                {
                    //修改姓名的service
                    opSign = new FinishCheckData().modifyName(qqid, changedName, "");

                    string mailFrom = null;
                    try
                    {
                        mailFrom = System.Configuration.ConfigurationManager.AppSettings["OutMailFrom"].ToString().Trim();
                    }
                    catch
                    {
                        Msg = "获取邮件发送人失败！ 请检查Service的Webconfig文件， mailFrom是否存在！";
                        return false;
                    }

                    bool mailSign = PublicRes.sendMail(mail, mailFrom, "财付通提醒您：财付通姓名修改成功！", "您好！ 你的姓名修改申诉已经通过！ 姓名改为" + changedName + " .请立即登陆财付通进行查看！", "out", out Msg);

                    if (mailSign == false)
                    {
                        throw new Exception(Msg);
                    }

                }
                else
                {
                    opSign = new FinishCheckData().changePwdInfo(qqid, mail, cleanMimao, reason, pathBase, pathAcc, pathIDCard, pathBank, user, ip, out Msg);
                }

                if (opSign == false)
                {
                    throw new Exception(Msg);
                    return false;
                }
                else //如果更改成功，则更新仲裁表的相关信息
                {
                    MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("ht"));
                    da.OpenConn();
                    da.ExecSql("START TRANSACTION;");  //开始一个事务

                    try
                    {
                        string upStr = "update c2c_fmdb.t_mediation set FcheckUid ='" + checkUsr + "',FcheckTime = '" + checkTime + "',FcheckMemo = '" + checkMemo + "' where FfetchID='" + fetchID + "'";  //update c2c_fmdb.t_check_main set FState='finished' where FID=" + strCheckID

                        da.ExecSql(upStr);
                        //concat(FcleanMimao, "1212")
                        string upParam = "update c2c_fmdb.t_check_param set Fvalue = concat(Fvalue, " + "'&checkUsr="
                            + checkUsr + "&FcheckTime=" + checkTime + "&FcheckMemo=" + checkMemo + "')" + "  where FcheckID ='" + strCheckID + "' and Fkey = 'returnUrl'";

                        da.ExecSql(upParam);

                        da.ExecSql("COMMIT;");

                        return true;
                    }
                    catch (Exception e)
                    {
                        da.ExecSql("ROLLBACK;");
                        return false;
                    }
                }
            }


            #endregion
        }


    }

}
