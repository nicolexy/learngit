using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.EnterpriseServices;
using System.Configuration;
using TENCENT.OSS.CFT.KF.DataAccess;
using TENCENT.OSS.CFT.KF.Common;

using TENCENT.OSS.C2C.Finance.DataAccess;
using TENCENT.OSS.C2C.Finance.Common;
using TENCENT.OSS.C2C.Finance.Common.CommLib;
using TENCENT.OSS.C2C.Finance.BankLib;
using TENCENT.OSS.CFT.KF.KF_Service;
using System.Xml;

namespace Tencent.KF.IVR
{
	/// <summary>
	/// IVRService 的摘要说明。
	/// </summary>
	public class IVRService : System.Web.Services.WebService
	{
		public IVRService()
		{
			//CODEGEN: 该调用是 ASP.NET Web 服务设计器所必需的
			InitializeComponent();
		}

		#region 组件设计器生成的代码
		
		//Web 服务设计器所必需的
		private IContainer components = null;
				
		/// <summary>
		/// 设计器支持所需的方法 - 不要使用代码编辑器修改
		/// 此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
		}

		/// <summary>
		/// 清理所有正在使用的资源。
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if(disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);		
		}
		
		#endregion


        /// <summary>
        /// 请求外呼数据
        /// </summary>
        /// <param name="strCheckID">申诉单ＩＤ</param>
        /// <param name="strUserID">财付通账号</param>
        /// <param name="strMobile">原手机号码</param>
        /// <param name="intCallNum">第几次呼叫</param>
        /// <returns>1：有数据并成功返回．　０：无数据． -1：执行异常</returns>
        [WebMethod]
        public int GetIVRData(out string strCheckID, out string strUserID, out string strMobile, out int intCallNum)
        {
            strCheckID = "";
            strUserID = "";
            strMobile = "";
            intCallNum = 1;

            //外呼时间限制：0点至8点不外呼
            DateTime d = DateTime.Now;
            if (d.Hour >= 0 && d.Hour < 8)
            {
                return 0;
            }

            //IVR外呼专用furion
            //1,2先不做,后期有需要时可以在未获取到记录时做下插入操作.
            //1:获取外呼表里没有的符合条件的申诉表数据(0,8),这里可以先取外呼表中的最大申诉ID,只用处理其后的申请表数据.
            //2:插入外呼表.呼叫次数0,状态为插入成功.

            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("CFT"));
            try
            {
                string onehourprior = DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss");

                da.OpenConn();
                //3:取最早数据(呼叫次数为0或呼叫次数大于0而且第一次呼叫时间已过1小时的最小申诉单ID)
                string strSql = "select * from t_tenpay_appeal_IVR where Fappealtime>='" + DateTime.Now.AddHours(-48) + "' and (Fcallnum=0 or (Fcallnum=1 and Flastcalltime<='" + onehourprior + "' and Fstate in (1,3))) order by Fappealtime limit 1 ";
                DataTable dt = da.GetTable(strSql);

                if (dt == null || dt.Rows.Count == 0)
                {
                    return 0;
                }

                strCheckID = dt.Rows[0]["FAppealID"].ToString();
                strUserID = dt.Rows[0]["Fuin"].ToString();
                strMobile = dt.Rows[0]["Fmobile"].ToString();
                intCallNum = Int32.Parse(dt.Rows[0]["Fcallnum"].ToString()) + 1;
                //4:置状态为已发送呼叫,呼叫次数加1,送出数据.
                strSql = "update t_tenpay_appeal_IVR set Fcallnum=" + intCallNum + ",Fstate=1,Flastcalltime=now(),Fmodifytime=now() where Fappealid=" + strCheckID;
                if (da.ExecSqlNum(strSql) == 1)
                {
                    return 1;
                }
                else
                {
                    loger.err("GetIVRData", "更新记录时出错" + strCheckID);
                    return -1;
                }
            }
            catch (Exception err)
            {
                loger.err("GetIVRData", "执行出错" + err.Message);
                return -1;
            }
            finally
            {
                da.Dispose();
            }
        }

        /// <summary>
        /// 传回外呼结果
        /// </summary>
        /// <param name="strCheckID">申诉单ＩＤ</param>
        /// <param name="strUserID">>财付通账号</param>
        /// <param name="strMobile">原手机号码</param>
        /// <param name="intResult">呼叫结果</param>
        /// <param name="strMemo">呼叫备注</param>
        /// <param name="MD5">MD5信息做校验使用</param>
        /// <returns>1:成功更新 0:未能成功更新 -1:出现异常．</returns>
        [WebMethod]
        public int SendIVRResult(string strCheckID, string strUserID, string strMobile, int intResult, string strMemo, string MD5)
        {
            //IVR外呼专用furion
            //1:验签.
            string ivrmd5 = System.Configuration.ConfigurationManager.AppSettings["Appeal_IVRMd5"];
            string sourcestr = strCheckID + strUserID + strMobile + intResult + strMemo + ivrmd5;
            string md5 = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sourcestr, "md5").ToLower();
            if (md5 != MD5.ToLower())
            {
                //验签失败.
                loger.err("SendIVRResult", "验签失败" + sourcestr + "|||" + MD5);
                return -1;
            }

            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("CFT"));
            try
            {
                da.OpenConn();
                //2:查询申诉单ID是否存在于外呼表中,根据外呼次数,置相应的字段值,(第N次外呼结果,外呼备注,外呼已完成)
                string strSql = "select * from t_tenpay_appeal_IVR where FappealID=" + strCheckID;
                DataTable dt = da.GetTable(strSql);

                if (dt == null || dt.Rows.Count == 0)
                {
                    //这里找不到记录,应该是异常.
                    loger.err("SendIVRResult", "查找不到外呼单" + strCheckID);
                    return -1;
                }

                DataRow dr = dt.Rows[0];

                if (dr["Fmobile"].ToString() != strMobile)
                {
                    if ("0" + dr["Fmobile"].ToString() != strMobile)
                    {
                        //这个地方异常
                        loger.err("SendIVRResult", "手机参数有误" + strCheckID + "||" + strMobile);
                        return -1;
                    }
                }

                if (dr["FState"].ToString() == "1") //状态为已发送呼叫
                {
                    //更新modifytime callresult,callmemo,并根据callresult更新state字段.
                    //1:用户主动回复1同意.
                    //2:用户主动回复2拒绝
                    //3:用户主动回复其它值.
                    //4:用户不接听电话.
                    //5:用户主动挂机
                    //6:呼叫无法建立(空号,关机)
                   // 7 Ivr主动挂机(超过1分钟用户没有按键)
                    if (intResult < 1 || intResult > 7)
                    {
                        //不允许超出范围.
                        loger.err("SendIVRResult", "呼叫结果超限" + strCheckID + "|" + intResult);
                        return -1;
                    }

                    //2013.09.02 lxl
                    //判断是高分单还是低分单，低分单拒绝申诉或者人工审批，高分单则是拒绝申诉或直接通过
                    string SqlDiFen = "select FParameter from t_tenpay_appeal_trans where Fid='" + strCheckID + "' and FParameter not like '%&AUTO_APPEAL=1%'";
                    DataTable dtDiFen = da.GetTable(SqlDiFen);

                    //高分单
                    if (dtDiFen == null || dtDiFen.Rows.Count == 0)
                    {
                        if (intResult == 1 || intResult == 4 || intResult == 5 || intResult == 7)//20130716添加intResult == 5，需求修改//20131210 lxl 添加7
                        {
                            //1,4时通过申诉,其余拒绝申诉.
                            strSql = "update t_tenpay_appeal_IVR set Fstate=2,Fmodifytime=now(),Fcallresult=" + intResult + ",Fcallmemo='" + PublicRes.replaceMStr(strMemo)
                                + "' where Fappealid='" + strCheckID + "'";
                        }
                        else
                        {
                            strSql = "update t_tenpay_appeal_IVR set Fstate=3,Fmodifytime=now(),Fcallresult=" + intResult + ",Fcallmemo='" + PublicRes.replaceMStr(strMemo)
                                + "' where Fappealid='" + strCheckID + "'";
                        }
                    }
                    else//低分单
                    {
                        if (intResult == 2 || (intResult == 6 && dr["Fcallnum"].ToString() == "1")) //2时通过拒绝申诉,其余人工审批
                        {
                            strSql = "update t_tenpay_appeal_IVR set Fstate=3,Fmodifytime=now(),Fcallresult=" + intResult + ",Fcallmemo='" + PublicRes.replaceMStr(strMemo)
                                + "' where Fappealid='" + strCheckID + "'";
                        }
                        else//Fstate=4加一个状态，表示低分单人工审批
                        {
                            strSql = "update t_tenpay_appeal_IVR set Fstate=4,Fmodifytime=now(),Fcallresult=" + intResult + ",Fcallmemo='" + PublicRes.replaceMStr(strMemo)
                                + "' where Fappealid='" + strCheckID + "'";
                        }
                    }

                    if (da.ExecSqlNum(strSql) != 1)
                    {
                        loger.err("SendIVRResult", "更新呼叫单失败" + strCheckID);
                        return -1;
                    }


                    //先判断申诉单表中状态是否变更,原状态(0,8).
                    strSql = "select Fstate from t_tenpay_appeal_trans where FID=" + strCheckID;
                    string state = da.GetOneResult(strSql);

                    if (state == "0" || state == "8")
                    {
                        //高分单
                        if (dtDiFen == null || dtDiFen.Rows.Count == 0)
                        {
                            //进行确认通过,或拒绝调用.
                            if (intResult == 1 || intResult == 4 || intResult == 5 || intResult == 7)//20130716添加intResult == 5，需求修改//20131210 lxl 添加7
                            {
                                string mesg = "";
                                if (CFTUserAppealClass.ConfirmAppeal(int.Parse(strCheckID), "", "system", "127.0.0.2", out mesg))
                                {
                                    return 1;
                                }
                                else
                                {
                                    loger.err("SendIVRResult", "通过申诉单时失败" + strCheckID + "||" + mesg);
                                    return -1;
                                }
                            }
                            else
                            {
                                //6:呼叫无法建立(空号,关机) 这种情况下,如果是第一次呼叫,允许再进行次呼叫,先不执行拒绝操作.
                                if (intResult == 6 && dr["Fcallnum"].ToString() == "1")
                                {
                                    return 1;
                                }

                                string mesg = "";
                                if (CFTUserAppealClass.CancelAppeal(int.Parse(strCheckID), "原绑定手机拒绝", "原绑定手机拒绝", strMemo, "system", "127.0.0.2", out mesg))
                                {
                                    //避免二次外呼Fcallnum=3,永远不可能有第三次外呼，以此作为标记
                                    strSql = "update t_tenpay_appeal_IVR set Fcallnum=3, Fmodifytime=now() where Fappealid='" + strCheckID + "'";
                                    if (da.ExecSqlNum(strSql) != 1)
                                    {
                                        loger.err("SendIVRResult", "避免二次外呼更新呼叫单失败" + strCheckID);
                                        return -1;
                                    }
                                    return 1;
                                }
                                else
                                {
                                    loger.err("SendIVRResult", "拒绝申诉单时失败" + strCheckID + "||" + mesg);
                                    return -1;
                                }
                            }
                            return 1;
                        }
                        else//低分单
                        {
                            //进行拒绝调用,或者走人工审批流程，改t_tenpay_appeal_trans表中为“未处理”状态即Fstate=0
                            if (intResult == 1 || intResult == 4 || intResult == 5 || intResult == 3 || intResult == 6 || intResult == 7)//20131210 lxl 添加7
                            {
                                //6:呼叫无法建立(空号,关机) 这种情况下,如果是第一次呼叫,允许再进行次呼叫,先不执行拒绝操作.
                                if (intResult == 6 && dr["Fcallnum"].ToString() == "1")
                                {
                                    return 1;
                                }
                                SqlDiFen = "update t_tenpay_appeal_trans set FPickUser='system',FPickTime=now(),Fstate=0 where Fid='" + strCheckID + "' and FUin='" + dr["Fuin"].ToString() + "'";
                                da.ExecSql(SqlDiFen);
                                return 1;
                            }
                            else//intResult=2时
                            {
                                string mesg = "";
                                if (CFTUserAppealClass.CancelAppeal(int.Parse(strCheckID), "原绑定手机拒绝", "原绑定手机拒绝", strMemo, "system", "127.0.0.2", out mesg))
                                {
                                    //避免二次外呼
                                    SqlDiFen = "update t_tenpay_appeal_IVR set Fcallnum=3, Fmodifytime=now() where Fappealid='" + strCheckID + "'";
                                    if (da.ExecSqlNum(SqlDiFen) != 1)
                                    {
                                        loger.err("SendIVRResult", "避免二次外呼更新呼叫单失败" + strCheckID);
                                        return -1;
                                    }
                                    return 1;
                                }
                                else
                                {
                                    loger.err("SendIVRResult", "拒绝申诉单时失败" + strCheckID + "||" + mesg);
                                    return -1;
                                }
                            }

                            return 1;
                        }

                    }
                    else //if(state == "0" || state == "8")
                    {
                        return 0;
                    }
                }
                else//if(dr["FState"].ToString() == "1")
                {
                    //状态与预期发生变更,可能是重复回结果所致.
                    return 0;
                }
            }
            catch (Exception err)
            {
                loger.err("SendIVRResult", "执行异常" + strCheckID + "||" + err.Message);
                return -1;
            }
            finally
            {
                da.Dispose();
            }
        }


		/// <summary>
		/// 请求外呼数据
		/// </summary>
		/// <param name="strCheckID">申诉单ＩＤ</param>
		/// <param name="strUserID">财付通账号</param>
		/// <param name="strMobile">原手机号码</param>
		/// <param name="intCallNum">第几次呼叫</param>
        /// <param name="dbName">IVR库名</param>
        /// <param name="tbName">IVR表名</param>
		/// <returns>1：有数据并成功返回．　０：无数据． -1：执行异常</returns>
        /// 20131113 lxl 加参数dbName tbName
		[WebMethod]
		public  int GetIVRDataNew(out string dbName,out string tbName,out string strCheckID,out string strUserID,out string strMobile,out int intCallNum)
		{
			strCheckID = "";
			strUserID = "";
			strMobile = "";
			intCallNum = 1;
            dbName = "";
            tbName = "";

            //外呼时间限制：0点至8点不外呼
            DateTime d = DateTime.Now;
            if (d.Hour >= 0 && d.Hour < 8)
            {
                return 0;
            }

			//IVR外呼专用furion
			//1,2先不做,后期有需要时可以在未获取到记录时做下插入操作.
			//1:获取外呼表里没有的符合条件的申诉表数据(0,8),这里可以先取外呼表中的最大申诉ID,只用处理其后的申请表数据.
			//2:插入外呼表.呼叫次数0,状态为插入成功.

			MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("CFT"));
            MySqlAccess daIVRFen = new MySqlAccess(PublicRes.GetConnString("IVRNEW"));//IVR新分库分表 
			try
			{
				string onehourprior = DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss");

				da.OpenConn();
                daIVRFen.OpenConn();
				//3:取最早数据(呼叫次数为0或呼叫次数大于0而且第一次呼叫时间已过1小时的最小申诉单ID)
                string strSql = "select * from t_tenpay_appeal_IVR where Fappealtime>='"+DateTime.Now.AddHours(-48)+"' and (Fcallnum=0 or (Fcallnum=1 and Flastcalltime<='" + onehourprior + "' and Fstate in (1,3))) order by Fappealtime limit 1 ";
				DataTable dt = da.GetTable(strSql);

                if (dt == null || dt.Rows.Count == 0)
                {
                    int year = DateTime.Now.Year;
                    int month = DateTime.Now.Month;

                    string str1 = "", str2 = "";
                    if (month < 10)
                        str1 = "select 'db_apeal_IVR_" + year + "' as dbName, 't_tenpay_appeal_IVR_0" + month + "' as tbName, FAppealID,Fuin,Fmobile ,Fcallnum, Fappealtime from db_apeal_IVR_" + year + ".t_tenpay_appeal_IVR_0" + month + " where Fappealtime>='" + DateTime.Now.AddHours(-48) + "' and (Fcallnum=0 or (Fcallnum=1 and Flastcalltime<='" + onehourprior + "' and Fstate in (1,3)))";
                    else
                        str1 = "select 'db_apeal_IVR_" + year + "' as dbName, 't_tenpay_appeal_IVR_" + month + "' as tbName, FAppealID,Fuin,Fmobile ,Fcallnum, Fappealtime  from db_apeal_IVR_" + year + ".t_tenpay_appeal_IVR_" + month + " where Fappealtime>='" + DateTime.Now.AddHours(-48) + "' and (Fcallnum=0 or (Fcallnum=1 and Flastcalltime<='" + onehourprior + "' and Fstate in (1,3)))";

                    if (month != DateTime.Now.AddHours(-48).Month)//不在一个月份，就要加查上个表
                    {
                        if (month == 1)
                            str2 = "select 'db_apeal_IVR_" + (year - 1) + "' as dbName, 't_tenpay_appeal_IVR_" + 12 + "' as tbName, FAppealID,Fuin,Fmobile ,Fcallnum, Fappealtime  from db_apeal_IVR_" + (year - 1) + ".t_tenpay_appeal_IVR_" + 12 + " where Fappealtime>='" + DateTime.Now.AddHours(-48) + "' and (Fcallnum=0 or (Fcallnum=1 and Flastcalltime<='" + onehourprior + "' and Fstate in (1,3)))";
                        else
                        {
                            if (month - 1 < 10)
                                str2 = "select 'db_apeal_IVR_" + year + "' as dbName, 't_tenpay_appeal_IVR_0" + (month - 1) + "' as tbName, FAppealID,Fuin,Fmobile ,Fcallnum, Fappealtime  from db_apeal_IVR_" + year + ".t_tenpay_appeal_IVR_0" + (month - 1) + " where Fappealtime>='" + DateTime.Now.AddHours(-48) + "' and (Fcallnum=0 or (Fcallnum=1 and Flastcalltime<='" + onehourprior + "' and Fstate in (1,3)))";
                            else
                                str2 = "select 'db_apeal_IVR_" + year + "' as dbName, 't_tenpay_appeal_IVR_" + (month - 1) + "' as tbName, FAppealID,Fuin,Fmobile ,Fcallnum, Fappealtime  from db_apeal_IVR_" + year + ".t_tenpay_appeal_IVR_" + (month - 1) + " where Fappealtime>='" + DateTime.Now.AddHours(-48) + "' and (Fcallnum=0 or (Fcallnum=1 and Flastcalltime<='" + onehourprior + "' and Fstate in (1,3)))";
                        }
                    }


                    if (str2 == "")
                        strSql = str1 + " order by Fappealtime limit 1";
                    else
                        strSql = str1 + " union " + str2 + " order by Fappealtime limit 1";
                    dt = daIVRFen.GetTable(strSql);
                    if (dt == null || dt.Rows.Count == 0)
                    {
                        return 0;
                    }
                    else
                    {
                        dbName = dt.Rows[0]["dbName"].ToString();
                        tbName = dt.Rows[0]["tbName"].ToString();
                    }

                }

				strCheckID = dt.Rows[0]["FAppealID"].ToString();
				strUserID = dt.Rows[0]["Fuin"].ToString();
				strMobile = dt.Rows[0]["Fmobile"].ToString();
				intCallNum = Int32.Parse(dt.Rows[0]["Fcallnum"].ToString()) + 1;
				//4:置状态为已发送呼叫,呼叫次数加1,送出数据.
                if (dbName == "" && tbName == "")
                {
                    strSql = "update t_tenpay_appeal_IVR set Fcallnum=" + intCallNum + ",Fstate=1,Flastcalltime=now(),Fmodifytime=now() where Fappealid=" + strCheckID;
                    if (da.ExecSqlNum(strSql) == 1)
                    {
                        return 1;
                    }
                    else
                    {
                        loger.err("GetIVRDataNew", "更新记录时出错" + strCheckID);
                        return -1;
                    }
                }
                else
                {
                    strSql = "update " + dbName + "." + tbName + " set Fcallnum=" + intCallNum + ",Fstate=1,Flastcalltime=now(),Fmodifytime=now() where Fappealid='" + strCheckID+"'" ;
                    if (daIVRFen.ExecSqlNum(strSql) == 1)
                    {
                        return 1;
                    }
                    else
                    {
                        loger.err("GetIVRDataNew", "更新记录时出错,库表:" + dbName + "." + tbName + "; Fappealid:" + strCheckID);
                        return -1;
                    }
                } 
			}
			catch(Exception err)
			{
                loger.err("GetIVRDataNew", "执行出错" + err.Message);
				return -1;
			}
			finally
			{
				da.Dispose();
                daIVRFen.Dispose();
			}
		}

		/// <summary>
		/// 传回外呼结果
		/// </summary>
        /// /// <param name="dbName">IVR库名</param>
        /// <param name="tbName">IVR表名</param>
		/// <param name="strCheckID">申诉单ＩＤ</param>
		/// <param name="strUserID">>财付通账号</param>
		/// <param name="strMobile">原手机号码</param>
		/// <param name="intResult">呼叫结果</param>
		/// <param name="strMemo">呼叫备注</param>
		/// <param name="MD5">MD5信息做校验使用</param>
		/// <returns>1:成功更新 0:未能成功更新 -1:出现异常．</returns>
		[WebMethod]
		public int SendIVRResultNew(string dbName, string tbName,string strCheckID, string strUserID, string strMobile,int intResult,string strMemo,string MD5)
		{
			//IVR外呼专用furion
			//1:验签.
			string ivrmd5 = System.Configuration.ConfigurationManager.AppSettings["Appeal_IVRMd5"];
            string sourcestr = dbName + tbName + strCheckID + strUserID + strMobile + intResult + strMemo + ivrmd5;
			string md5 = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sourcestr,"md5").ToLower();
			if(md5 != MD5.ToLower())
			{
				//验签失败.
                loger.err("SendIVRResultNew", "验签失败" + sourcestr + "|||" + MD5);
				return -1;
			}

			MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("CFT"));
            MySqlAccess daIVRFen = new MySqlAccess(PublicRes.GetConnString("IVRNEW"));//IVR新分库分表
            MySqlAccess daFen = new MySqlAccess(PublicRes.GetConnString("CFTNEW"));//新分库分表 
            try
            {
                da.OpenConn();
                daIVRFen.OpenConn();
                daFen.OpenConn();

                //以前流程
                if ((dbName == "" && tbName == "") || (dbName == null || tbName == null))
                {
                    #region
                    //2:查询申诉单ID是否存在于外呼表中,根据外呼次数,置相应的字段值,(第N次外呼结果,外呼备注,外呼已完成)
                    string strSql = "select * from t_tenpay_appeal_IVR where FappealID=" + strCheckID;
                    DataTable dt = da.GetTable(strSql);

                    if (dt == null || dt.Rows.Count == 0)
                    {
                        //这里找不到记录,应该是异常.
                        loger.err("SendIVRResultNew", "查找不到外呼单" + strCheckID);
                        return -1;
                    }

                    DataRow dr = dt.Rows[0];

                    if (dr["Fmobile"].ToString() != strMobile)
                    {
                        if ("0" + dr["Fmobile"].ToString() != strMobile)
                        {
                            //这个地方异常
                            loger.err("SendIVRResultNew", "手机参数有误" + strCheckID + "||" + strMobile);
                            return -1;
                        }
                    }

                    if (dr["FState"].ToString() == "1") //状态为已发送呼叫
                    {
                        //更新modifytime callresult,callmemo,并根据callresult更新state字段.
                        //1:用户主动回复1同意.
                        //2:用户主动回复2拒绝
                        //3:用户主动回复其它值.
                        //4:用户不接听电话.
                        //5:用户主动挂机
                        //6:呼叫无法建立(空号,关机)
                        // 7 Ivr主动挂机(超过1分钟用户没有按键)
                        if (intResult < 1 || intResult > 7)
                        {
                            //不允许超出范围.
                            loger.err("SendIVRResultNew", "呼叫结果超限" + strCheckID + "|" + intResult);
                            return -1;
                        }

                        //2013.09.02 lxl
                        //判断是高分单还是低分单，低分单拒绝申诉或者人工审批，高分单则是拒绝申诉或直接通过
                        string SqlDiFen = "select FParameter from t_tenpay_appeal_trans where Fid='" + strCheckID + "' and FParameter not like '%&AUTO_APPEAL=1%'";
                        DataTable dtDiFen = da.GetTable(SqlDiFen);

                        //高分单
                        if (dtDiFen == null || dtDiFen.Rows.Count == 0)
                        {
                            if (intResult == 1 || intResult == 4 || intResult == 5 || intResult == 7)//20130716添加intResult == 5，需求修改
                            {
                                //1,4,5时通过申诉,其余拒绝申诉.
                                strSql = "update t_tenpay_appeal_IVR set Fstate=2,Fmodifytime=now(),Fcallresult=" + intResult + ",Fcallmemo='" + PublicRes.replaceMStr(strMemo)
                                    + "' where Fappealid='" + strCheckID + "'";
                            }
                            else
                            {
                                strSql = "update t_tenpay_appeal_IVR set Fstate=3,Fmodifytime=now(),Fcallresult=" + intResult + ",Fcallmemo='" + PublicRes.replaceMStr(strMemo)
                                    + "' where Fappealid='" + strCheckID + "'";
                            }
                        }
                        else//低分单
                        {
                            if (intResult == 2 || (intResult == 6 && dr["Fcallnum"].ToString() == "1")) //2时通过拒绝申诉,其余人工审批
                            {
                                strSql = "update t_tenpay_appeal_IVR set Fstate=3,Fmodifytime=now(),Fcallresult=" + intResult + ",Fcallmemo='" + PublicRes.replaceMStr(strMemo)
                                    + "' where Fappealid='" + strCheckID + "'";
                            }
                            else//Fstate=4加一个状态，表示低分单人工审批
                            {
                                strSql = "update t_tenpay_appeal_IVR set Fstate=4,Fmodifytime=now(),Fcallresult=" + intResult + ",Fcallmemo='" + PublicRes.replaceMStr(strMemo)
                                    + "' where Fappealid='" + strCheckID + "'";
                            }
                        }

                        if (da.ExecSqlNum(strSql) != 1)
                        {
                            loger.err("SendIVRResultNew", "更新呼叫单失败" + strCheckID);
                            return -1;
                        }


                        //先判断申诉单表中状态是否变更,原状态(0,8).
                        strSql = "select Fstate from t_tenpay_appeal_trans where FID=" + strCheckID;
                        string state = da.GetOneResult(strSql);

                        if (state == "0" || state == "8")
                        {
                            //高分单
                            if (dtDiFen == null || dtDiFen.Rows.Count == 0)
                            {
                                //进行确认通过,或拒绝调用.
                                if (intResult == 1 || intResult == 4 || intResult == 5 || intResult == 7)//20130716添加intResult == 5，需求修改
                                {
                                    string mesg = "";
                                    if (CFTUserAppealClass.ConfirmAppeal(int.Parse(strCheckID), "", "system", "127.0.0.2", out mesg))
                                    {
                                        return 1;
                                    }
                                    else
                                    {
                                        loger.err("SendIVRResultNew", "通过申诉单时失败" + strCheckID + "||" + mesg);
                                        return -1;
                                    }
                                }
                                else
                                {
                                    //6:呼叫无法建立(空号,关机) 这种情况下,如果是第一次呼叫,允许再进行次呼叫,先不执行拒绝操作.
                                    if (intResult == 6 && dr["Fcallnum"].ToString() == "1")
                                    {
                                        return 1;
                                    }

                                    string mesg = "";
                                    if (CFTUserAppealClass.CancelAppeal(int.Parse(strCheckID), "原绑定手机拒绝", "原绑定手机拒绝", strMemo, "system", "127.0.0.2", out mesg))
                                    {
                                        //避免二次外呼Fcallnum=3,永远不可能有第三次外呼，以此作为标记
                                        strSql = "update t_tenpay_appeal_IVR set Fcallnum=3, Fmodifytime=now() where Fappealid='" + strCheckID + "'";
                                        if (da.ExecSqlNum(strSql) != 1)
                                        {
                                            loger.err("SendIVRResultNew", "避免二次外呼更新呼叫单失败" + strCheckID);
                                            return -1;
                                        }
                                        return 1;
                                    }
                                    else
                                    {
                                        loger.err("SendIVRResultNew", "拒绝申诉单时失败" + strCheckID + "||" + mesg);
                                        return -1;
                                    }
                                }
                                return 1;
                            }
                            else//低分单
                            {
                                //进行拒绝调用,或者走人工审批流程，改t_tenpay_appeal_trans表中为“未处理”状态即Fstate=0
                                if (intResult == 1 || intResult == 4 || intResult == 5 || intResult == 3 || intResult == 6 || intResult == 7)
                                {
                                    //6:呼叫无法建立(空号,关机) 这种情况下,如果是第一次呼叫,允许再进行次呼叫,先不执行拒绝操作.
                                    if (intResult == 6 && dr["Fcallnum"].ToString() == "1")
                                    {
                                        return 1;
                                    }
                                    SqlDiFen = "update t_tenpay_appeal_trans set FPickUser='system',FPickTime=now(),Fstate=0 where Fid='" + strCheckID + "' and FUin='" + dr["Fuin"].ToString() + "'";
                                    da.ExecSql(SqlDiFen);
                                    return 1;
                                }
                                else//intResult=2时
                                {
                                    string mesg = "";
                                    if (CFTUserAppealClass.CancelAppeal(int.Parse(strCheckID), "原绑定手机拒绝", "原绑定手机拒绝", strMemo, "system", "127.0.0.2", out mesg))
                                    {
                                        //避免二次外呼
                                        SqlDiFen = "update t_tenpay_appeal_IVR set Fcallnum=3, Fmodifytime=now() where Fappealid='" + strCheckID + "'";
                                        if (da.ExecSqlNum(SqlDiFen) != 1)
                                        {
                                            loger.err("SendIVRResultNew", "避免二次外呼更新呼叫单失败" + strCheckID);
                                            return -1;
                                        }
                                        return 1;
                                    }
                                    else
                                    {
                                        loger.err("SendIVRResultNew", "拒绝申诉单时失败" + strCheckID + "||" + mesg);
                                        return -1;
                                    }
                                }

                                return 1;
                            }

                        }
                        else //if(state == "0" || state == "8")
                        {
                            return 0;
                        }
                    }
                    else//if(dr["FState"].ToString() == "1")
                    {
                        //状态与预期发生变更,可能是重复回结果所致.
                        return 0;
                    }

                    #endregion
                }
                else//分库分表流程
                {

                    #region
                    //2:查询申诉单ID是否存在于外呼表中,根据外呼次数,置相应的字段值,(第N次外呼结果,外呼备注,外呼已完成)
                    string strSql = "select * from " + dbName + "." + tbName + " where FappealID='" + strCheckID + "'";
                    DataTable dt = daIVRFen.GetTable(strSql);

                    if (dt == null || dt.Rows.Count == 0)
                    {
                        //这里找不到记录,应该是异常.
                        loger.err("SendIVRResultNew", "查找不到外呼单  数据表：" + dbName + "." + tbName + "   FappealID:" + strCheckID);
                        return -1;
                    }

                    DataRow dr = dt.Rows[0];

                    if (dr["Fmobile"].ToString() != strMobile)
                    {
                        if ("0" + dr["Fmobile"].ToString() != strMobile)
                        {
                            //这个地方异常
                            loger.err("SendIVRResultNew", "手机参数有误  数据表：" + dbName + "." + tbName + "   FappealID:" + strCheckID + "||" + strMobile);
                            return -1;
                        }
                    }

                    if (dr["FState"].ToString() == "1") //状态为已发送呼叫
                    {
                        //更新modifytime callresult,callmemo,并根据callresult更新state字段.
                        //1:用户主动回复1同意.
                        //2:用户主动回复2拒绝
                        //3:用户主动回复其它值.
                        //4:用户不接听电话.
                        //5:用户主动挂机
                        //6:呼叫无法建立(空号,关机)
                        if (intResult < 1 || intResult > 7)
                        {
                            //不允许超出范围.
                            loger.err("SendIVRResultNew", "呼叫结果超限  数据表：" + dbName + "." + tbName + "   FappealID:" + strCheckID + "|" + intResult);
                            return -1;
                        }

                        //判断是高分单还是低分单，低分单拒绝申诉或者人工审批，高分单则是拒绝申诉或直接通过
                        string SqlDiFen = "select FAutoAppeal from " + dbName + "." + tbName + " where FappealID='" + strCheckID + "' and FAutoAppeal=0";
                        DataTable dtDiFen = daIVRFen.GetTable(SqlDiFen);

                         //高分单
                        if (dtDiFen == null || dtDiFen.Rows.Count == 0)
                        {
                            if (intResult == 1 || intResult == 4 || intResult == 5 || intResult == 7)//20130716添加intResult == 5，需求修改
                            {
                                //1,4,5时通过申诉,其余拒绝申诉.
                                strSql = "update  " + dbName + "." + tbName + "  set Fstate=2,Fmodifytime=now(),Fcallresult=" + intResult + ",Fcallmemo='" + PublicRes.replaceMStr(strMemo)
                                    + "' where Fappealid='" + strCheckID + "'";
                            }
                            else
                            {
                                strSql = "update  " + dbName + "." + tbName + "  set Fstate=3,Fmodifytime=now(),Fcallresult=" + intResult + ",Fcallmemo='" + PublicRes.replaceMStr(strMemo)
                                    + "' where Fappealid='" + strCheckID + "'";
                            }
                        }
                        else//低分单
                        {
                            //只有低分单
                            if (intResult == 2 || (intResult == 6 && dr["Fcallnum"].ToString() == "1")) //2时通过拒绝申诉,其余人工审批
                            {
                                strSql = "update   " + dbName + "." + tbName + "  set Fstate=3,Fmodifytime=now(),Fcallresult=" + intResult + ",Fcallmemo='" + PublicRes.replaceMStr(strMemo)
                                    + "' where Fappealid='" + strCheckID + "'";
                            }
                            else//Fstate=4加一个状态，表示低分单人工审批
                            {
                                strSql = "update   " + dbName + "." + tbName + "  set Fstate=4,Fmodifytime=now(),Fcallresult=" + intResult + ",Fcallmemo='" + PublicRes.replaceMStr(strMemo)
                                    + "' where Fappealid='" + strCheckID + "'";
                            }
                        }


                        if (daIVRFen.ExecSqlNum(strSql) != 1)
                        {
                            loger.err("SendIVRResult", "更新呼叫单失败  数据表：" + dbName + "." + tbName + "   FappealID:" + strCheckID);
                            return -1;
                        }

                        string dateTime = dr["FAppealTime"].ToString();
                        int year = DateTime.Parse(dateTime).Year;
                        int month = DateTime.Parse(dateTime).Month;
                        string dbAppeal = "db_appeal_" + year;
                        string tbAppeal = "";
                        if (month < 10)
                            tbAppeal = "t_tenpay_appeal_trans_0" + month;
                        else
                            tbAppeal = "t_tenpay_appeal_trans_" + month;

                        //先判断申诉单表中状态是否变更,原状态(0,8).
                        strSql = "select Fstate from " + dbAppeal + "." + tbAppeal + " where FID='" + strCheckID + "'";

                        string state = daFen.GetOneResult(strSql);

                        #region
                        if (state == "0" || state == "8")
                        {
                            //高分单
                            if (dtDiFen == null || dtDiFen.Rows.Count == 0)
                            {
                                //进行确认通过,或拒绝调用.
                                if (intResult == 1 || intResult == 4 || intResult == 5 || intResult == 7)
                                {
                                    string mesg = "";
                                    if (CFTUserAppealClass.ConfirmAppealDBTB(strCheckID, dbAppeal, tbAppeal, "", "system", "127.0.0.2", out mesg))
                                    {
                                        return 1;
                                    }
                                    else
                                    {
                                        loger.err("SendIVRResultNew", "通过申诉单时失败 数据表：" + dbName + "." + tbName + "   FappealID:" + strCheckID + "||" + mesg);
                                        return -1;
                                    }
                                }
                                else
                                {
                                    //6:呼叫无法建立(空号,关机) 这种情况下,如果是第一次呼叫,允许再进行次呼叫,先不执行拒绝操作.
                                    if (intResult == 6 && dr["Fcallnum"].ToString() == "1")
                                    {
                                        return 1;
                                    }

                                    string mesg = "";
                                    if (CFTUserAppealClass.CancelAppealDBTB(strCheckID, dbAppeal, tbAppeal, "原绑定手机拒绝", "原绑定手机拒绝", strMemo, "system", "127.0.0.2", out mesg))
                                    {
                                        //避免二次外呼Fcallnum=3,永远不可能有第三次外呼，以此作为标记
                                        strSql = "update  " + dbName + "." + tbName + "  set Fcallnum=3, Fmodifytime=now() where Fappealid='" + strCheckID + "'";
                                        if (daIVRFen.ExecSqlNum(strSql) != 1)
                                        {
                                            loger.err("SendIVRResultNew", "避免二次外呼更新呼叫单失败 数据表：" + dbName + "." + tbName + "   FappealID:" + strCheckID);
                                            return -1;
                                        }
                                        return 1;
                                    }
                                    else
                                    {
                                        loger.err("SendIVRResultNew", "拒绝申诉单时失败 数据表：" + dbName + "." + tbName + "   FappealID:" + strCheckID + "||" + mesg);
                                        return -1;
                                    }
                                }
                                return 1;
                            }
                            else//低分单
                            {
                                //进行拒绝调用,或者走人工审批流程，改t_tenpay_appeal_trans表中为“未处理”状态即Fstate=0
                                if (intResult == 1 || intResult == 4 || intResult == 5 || intResult == 3 || intResult == 6 || intResult == 7)
                                {
                                    //6:呼叫无法建立(空号,关机) 这种情况下,如果是第一次呼叫,允许再进行次呼叫,先不执行拒绝操作.
                                    if (intResult == 6 && dr["Fcallnum"].ToString() == "1")
                                    {
                                        return 1;
                                    }
                                    strSql = "update  " + dbAppeal + "." + tbAppeal + "  set FPickUser='system',FPickTime=now(),Fstate=0 where Fid='" + strCheckID + "' and FUin='" + dr["Fuin"].ToString() + "'";
                                    daFen.ExecSql(strSql);
                                    return 1;
                                }
                                else//intResult=2时
                                {
                                    string mesg = "";
                                    if (CFTUserAppealClass.CancelAppealDBTB(strCheckID, dbAppeal, tbAppeal, "原绑定手机拒绝", "原绑定手机拒绝", strMemo, "system", "127.0.0.2", out mesg))
                                    {
                                        //避免二次外呼
                                        strSql = "update   " + dbName + "." + tbName + "  set Fcallnum=3, Fmodifytime=now() where Fappealid='" + strCheckID + "'";
                                        if (daIVRFen.ExecSqlNum(strSql) != 1)
                                        {
                                            loger.err("SendIVRResultNew", "避免二次外呼更新呼叫单失败  数据表：" + dbName + "." + tbName + "   FappealID:" + strCheckID);
                                            return -1;
                                        }
                                        return 1;
                                    }
                                    else
                                    {
                                        loger.err("SendIVRResultNew", "拒绝申诉单时失败  数据表：" + dbName + "." + tbName + "   FappealID:" + strCheckID + "||" + mesg);
                                        return -1;
                                    }
                                }

                                return 1;
                            }
                        }
                        else //if(state == "0" || state == "8")
                        {
                            return 0;
                        }
                        #endregion
                    }
                    else//if(dr["FState"].ToString() == "1")
                    {
                        //状态与预期发生变更,可能是重复回结果所致.
                        return 0;
                    }

                    #endregion

                }
            }
            catch (Exception err)
            {
                if ((dbName == "" && tbName == "") || (dbName == null || tbName == null))
                    loger.err("SendIVRResultNew", "执行异常" + strCheckID + "||" + err.Message);
                else
                    loger.err("SendIVRResultNew", "执行异常  数据表：" + dbName + "." + tbName + "   FappealID:" + strCheckID + "||" + err.Message);
                return -1;
            }
			finally
			{
				da.Dispose();
                daIVRFen.Dispose();
                daFen.Dispose();
			}
		}
	}
}
