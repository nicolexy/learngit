using System;

using TENCENT.OSS.CFT.KF.DataAccess;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.C2C.Finance.BankLib;

namespace TENCENT.OSS.CFT.KF.KF_Service
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
		public static bool ExecuteCheck(string strCheckID, Finance_Header myHeader, MySqlAccess da)
		{
			myHeader.UserName = myHeader.UserName.Trim().ToLower();

			string strCheckType = GetCheckType(strCheckID);

			if(!PublicRes.IgnoreLimitCheck)
			{
				//furion 20060208 需要加入一个预先检查,检查完后置成执行中状态以避免重复执行.
				if(!PriorCheck(strCheckID,da))
				{				
					throw new LogicException("审批的状态不正确,请确认是否是重复执行");
				}
			}
			//加入完毕

			bool flag = false;
			string errmsg = "";

			try
			{
				flag = CheckFactory.ReturnHandler(strCheckType).DoAction(strCheckID,myHeader) ;
			}
			catch(Exception err)
			{
				errmsg = err.Message;
				flag = false;
			}

			if(!flag)
			{
				LastCheck(strCheckID,da);
				throw new LogicException("执行审批完成后的动作时失败！" + errmsg);
			}
			else
			{
				if(FinishedCheck(strCheckID,da))
				{
					return true;
				}
				else
				{
					throw new LogicException("执行审批完成后的动作时成功，但是在执行更改审批状态时失败！");
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
			
			//da.ExecSql("update c2c_fmdb.t_check_main set FState='finish' where FID=" + strCheckID);
			da.ExecSql("update c2c_fmdb.t_check_main set FNewState=2 where FID=" + strCheckID);
			
		}



		/// <summary>
		/// 更新审批为已执行状态
		/// </summary>
		/// <param name="strCheckID">审批ID</param>
		/// <param name="da">数据访问对象</param>
		/// <returns>是否成功</returns>
		public static bool FinishedCheck(string strCheckID,MySqlAccess da)
		{
			//return da.ExecSql("update c2c_fmdb.t_check_main set FState='finished' where FID=" + strCheckID);
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
			//string strSql = "select FState from c2c_fmdb.t_check_main where FID=" + strCheckID;
			string strSql = "select FNewState from c2c_fmdb.t_check_main where FID=" + strCheckID;
			string state = da.GetOneResult(strSql).Trim();
			if(state != "2")
			{
				return false;
			}
			else
			{
				//strSql = "update c2c_fmdb.t_check_main set FState='execute' where FID=" + strCheckID;
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
			bool DoAction(string strCheckID, Finance_Header myHeader);
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
				switch(strCheckType)
				{
						//有新的审批类型时，把处理它的类填写好，然后加入到下面的分支就行。
						/*
					case "batchpay" :
						return new PayCheck();  //没涉及到资金流水库
						break;
					case "batchrefund" :
						return new BatchRefundCheck();  //没涉及到资金流水库
						break;
					case "fmcz01":
						return new fmCheck();  //FinanceAdjust Check    //资金流水(易出错)
						break;
					case "fmjy02":
						return new fmCheck();  //FinanceAdjust Check    //资金流水(易出错)
						break;
					case "fmfk03":
						return new fmCheck();  //FinanceAdjust Check    //资金流水(易出错)
						break;
					case "fmtk04":
						return new fmCheck();  //FinanceAdjust Check    //资金流水(易出错)
						break;
					case "fmhd05":  //回导调整（失败-〉成功）  （成功-〉失败？ 如何处理）
						return new fmCheck();    //资金流水(易出错)
						break;
					case "czcz01": //充值冲正。
						return new czCheck();   //没涉及到资金流水库
						break;
					case "czjy02": //支付成功冲正。
						return new czCheck();   //没涉及到资金流水库
						break;
					case "czfk03": //买家确认冲正。
						return new czCheck();   //没涉及到资金流水库
						break;
					case "cztk04": //退款冲正
						return new czCheck();   //没涉及到资金流水库
						break;
					case "czcz05": //二期充值冲正
						return new czCheck();   //没涉及到资金流水库
						break;
					case "cztx06": //二期提现冲正。
						return new czCheck();   //没涉及到资金流水库
						break;
					case "czhd07": //付款结果回导冲正。
						return new czCheck();   //没涉及到资金流水库
						break;
					case "czzz08": //转帐冲正。
						return new czCheck();   //没涉及到资金流水库
						break;
						*/
					case "Mediation": //仲裁判断(修改敏感信息)
						return new Mediation();   //没涉及到资金流水库
						/*
					case "mcTrans":
						return new mcTrans();    //对于资金流水库修改较小
					case "ChangeQQ" :
						return new ChangeQQClass();   //没涉及到资金流水库
					case "NewMedi" :
						return new NewMediClass();    //没涉及到资金流水库
					case "ModifyMedi" :
						return new NewMediClass();    //没涉及到资金流水库
					case "NewCoinPub" :
						return new NewCoinPubClass();   //没涉及到资金流水库
					case "ModifyCoinPub" :
						return new NewCoinPubClass();   //没涉及到资金流水库
					case "settle" :
						return new SettleClass();   //对于资金流水库修改较小
					case "customSave" :                   //手工充值
						return new customSaveClass();   //对于资金流水库修改较小
					case "returnticket" :                   //退票
						return new returnticketClass();
					case "otherAdjust" :
						return new otherAdjustClass();

						//为了和旧版本进行适应,需要新建两个新的审批类型,将来再把这个删除掉.
					case "batchexception" :    //对帐异常直接处理
					case "Subbatchexception" :    //子帐户对帐异常直接处理
						return new batchexceptionClass();
					case "refundexception" :
						return new refundexceptionClass();//对帐异常直接退款
					case "twoexception" :
						return new twoexceptionClass();  //两条勾兑
					case "Subacctwoexception" :
						return new SubacctwoexceptionClass();//子帐户两条勾兑   rowenawu   20101023
					case "bankexception" :
						return new bankexceptionClass();

					case "tokenAdjust" :
						return new tokenAdjustClass();     //代金券调帐
					case "realTimeAdjust" :
						return new realTimeAdjustClass(); 
					case "mannalFetch" :
						return new mannalFetchClass();    //手工提现

					case "mannalRefund" :
						return new mannalRefundClass();    //手工退单

					case "HaoYe" :
						return new HaoYeCheck();
					case "proxyFetch" :
						return new proxyFetchClass();
					case "manualTransfer" :
						return new manualTransferClass();
					case "C2CmanualTransfer":
						return new manualTransferClass(); //C2C手工转帐
					case "b2creturn" :
						return new B2CReturnClass();
					case "logonUser" :
						return new logonUserClass();
					case "batchsettle":
						return new batchsettleClass();
					case "b2ctoerror" :
						return new b2ctoerrorClass();
					case "mannallist" :
						return new MannalListClass();
					case "changebankinfo" :
						return new ChangeBankInfoClass();
					case "directpayfail" :
						return new directpayfailClass();

						//新加网银直接退款失败审批
					case "banktofault" :
						return new banktofaultClass();

					case "givebackcheck" :
						return new givebackcheckClass();
					case "quicktraderefund":
						return new QuickTradeClass();
					case "sporderrefund":
						return new SporderRefundClass();
					case "spordercancel":
						return new SporderCancel();

					case "remitfetch"://邮储汇款提现申请
						return new RemitFetchClass();
					case "remitrefund"://邮储汇款退款申请
						return new RemitRefundClass();
					case "remitmodify"://邮储汇款调整状态申请
						return new RemitModifyClass();
					case "remitcancel"://邮储汇款对账作废申请
						return new RemitCancelClass();
					case "tobanksucc":
						return new RefundBankSucc();
					case "tomanualsucc":
						return new RefundManualSucc();
					case "topaysucc":
						return new RefundPaySucc();
					case "failtowait":
						return new RefundFailToWait();//退款失败到待退款状态
					case "batmanualcz":
						return new BatManualCZ();  //对账后批量手工充值

					case "BankTreasurer"://银行资金调拨  rowena  20080821
						return new BankTreasurerClass();

					case "CZToRefund"://充值转退款  rowena  20090921
						return new CZToRefundClass();

					case "RefundToQuickTrade"://提定银行类型退款转充值支付  rowena  20100909
						return new RefundToQuickTradeClass();

					case "batchRefundAdjust"://批量退款调整
						return new BatchadRefundAdjust();

					case "Autotwoexception":
						return new AutotwoexceptionClass(); //系统自动两条勾对
					case "RefundFailHand":
						return new RefundFailHandClass(); //退单异常处理
					case "AdjustOtherRefund":
						return new AdjustOtherRefundClass(); //手工调整退款异常表中数据状态
					case "AdjustRefundRecord":
						return new AdjustRefundRecordClass(); //退款汇总表中状态调整
					case "batchc2ctransfer":
						return new BatchC2CTransfer();//C2C批量转账
					case "BatchLogonUser":
						return new BatchLogonUser();//批量用户注销
					case "ConfigExportTaskComm":
						return new ConfigExportTaskCommClass();//新增、修改通用汇总任务配置表
					case "ConfigExportBank":
						return new ConfigExportBankClass();//新增、修改付款汇总银行配置
					case "ConfigExportTaskTmp":
						return new ConfigExportTaskTmpClass();//新增、修改临时汇总任务临时表
					case "ConfigExportBankRest":
						return new ConfigExportBankRestClass();//新增、修改非付款人临时表
					case "subaccczpayexception":
						return new subaccczpayexception();//子账户充值调整
				
					case "subacctxpayexception":
						return new subacctxpayexception();//子账户提现调整
					case "ConfigBankType":
						return new ConfigBankTypeClass();//银行编码配置
					case "batchrefundapply":
						return new BatchRefundApplyClass();//b2c/fastpay/子帐户批量退款
					case "CopyRights":
						return new CopyRightsClass();//复制审批系统权限 Andrew
					case "RecoverQQ":
						return new RecoverQQClass();//恢复QQ帐号 Andrew

					case "directPayBankConfig":
						return new DirectPayBankConfigClass();//直付系统银行配置
					case "directPayTaskCheck":
						return new DirectPayTaskCheckClass();//直付系统约束条件
					case "directPayTaskModel":
						return new DirectPayTaskModelClass();//直付系统任务模版
					case "directPayTaskExec":
						return new DirectPayTaskExecClass();//直付系统执行任务

					case "SpecialTaskExec":
						return new SpecialTaskExecClass();//启动特殊任务
					case "CNBankTreasurer":
						return new CNBankTreasurerClass();//出纳资金调拨  rowenawu
					case "codcancel":
						return new CODCancelClass();//货到付款对账作废 Andrew
					case "codtransfer":
						return new CODTransferClass();//货到付款转账 Andrew
					case "HongBaoSend":
						return new HongBaoSendClass();//红包发放
					case "PosResultCheck":
					case "PosResultRefund":
						return new PosResultCheckClass();//POS对账结果处理
					case "ConfigBankBulletin":
						return new ConfigBankBulletinClass();//银行维护公告配置  rowenawu
					case "gwqSaveOper":
						return new GWQSaveOperClass();//购物券充值审批
					case "gwqFetchOper":
						return new GWQFetchOperClass();//购物券过期提现审批
					case "gwqCancelOper":
						return new GWQCancelOperClass();//购物券作废审批
					case "SubRefund":
						return new SubRefundClass();//子帐户退款  rowenawu
					case "UploadCancel": 
						return new UploadCancelClass();//对帐作废 rowenawu
					case "ConfigYDTBankType":
						return new ConfigYDTBankTypeClass();//合作一点通银行信息 rowenawu
						*/
					default :
						return null;
						break;
				}
			}
		}


		public class Mediation : IBaseDoCheck
		{
			#region IBaseDoCheck 成员
		
			public bool DoAction(string strCheckID, Finance_Header myHeader)
			{
				return false;

				Finance_Manage fm = new Finance_Manage();
				Finance_Header fh = new Finance_Header();
				fh.UserName = myHeader.UserName;
				fh.UserIP   = myHeader.UserIP;
			
				fm.myHeader = fh;

				//修改密码service
				string Msg = null;

				string strFetchNo = "select fobjID from c2c_fmdb.t_check_main where fid = '" + strCheckID + "'";
				string fetchID = PublicRes.ExecuteOne(strFetchNo,"ht");

				string [] br = new string[3]; 
				string strCheckInfo = "select FcheckUser,FcheckMemo,FcheckTime from c2c_fmdb.t_check_list where fcheckid = '" + strCheckID + "'";
				br[0] = "FcheckUser";
				br[1] = "FcheckMemo";
				br[2] = "FcheckTime";
				br = PublicRes.returnDrData(strCheckInfo,br,"ht");

				string checkUsr = br[0];
				string checkMemo= br[1];
				string checkTime= br[2];

				string [] ar = new string [9];
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

				ar = PublicRes.returnDrData(str,ar,"ht");

				string qqid = ar[0];
				string mail = ar[1];
				string cleanMimao = ar[2];
				string reason   = ar[3];
				string pathBase = ar[4];
				string pathAcc  = ar[5];
				string pathIDCard = ar[6];
				string pathBank   = ar[7];
				string changedName= ar[8];


				bool opSign;

				//此处应该用接口来实现，结构更加清晰
				if(fetchID.Substring(0,3) == "101")  //修改姓名
				{
					//修改姓名的service
					opSign = fm.modifyName(qqid,changedName,"");
				
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

					bool mailSign = PublicRes.sendMail(mail,mailFrom,"财付通提醒您：财付通姓名修改成功！","您好！ 你的姓名修改申诉已经通过！ 姓名改为" + changedName +" .请立即登陆财付通进行查看！","out",out Msg);
				
					if (mailSign == false)
					{
						throw new Exception(Msg);
					}

				}
				else
				{
					opSign = fm.changePwdInfo(qqid,mail,cleanMimao,reason,pathBase,pathAcc,pathIDCard,pathBank,out Msg); 	
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
							+ checkUsr + "&FcheckTime=" + checkTime + "&FcheckMemo=" + checkMemo +"')" + "  where FcheckID ='" + strCheckID + "' and Fkey = 'returnUrl'";

						da.ExecSql(upParam);

						da.ExecSql("COMMIT;");	

						return true;
					}
					catch(Exception e)
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
