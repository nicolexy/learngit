using System;

using TENCENT.OSS.CFT.KF.DataAccess;
using TENCENT.OSS.CFT.KF.Common;
using System.Data;

namespace TENCENT.OSS.C2C.Finance.BankLib
{
	/// <summary>
	/// BankRefundIO 的摘要说明。
	/// </summary>
	public class BankRefundIO
	{

		
		private string fBatchID;
		private string fBankType;

		/// <summary>
		/// 在处理过程中的处理信息
		/// </summary>
		public string ResultString = ""; //20050711 给导入文件一个提示信息。

		private static string fconnstring;

		private static string incconnstring="";

		private static string ywconnstring="";

		private static string hdconnstring=""; //rowena  查合单时用

		private static string zjbconnstring="";//订单备机库
		//只是在汇总银行导入的时候标识一下是哪个银行的付款结果文件。
		public string InputBankType = "";

		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="BatchID">工单ID</param>
		/// <param name="zwconnstring">帐务数据库连接字符串</param>
		public BankRefundIO(string BatchID, string zwconnstring,string incconn,string ywconn,string hdconn,string zjbconn)
		{
			if(BatchID == null || BatchID == "" || BatchID.Length != 13)
			{
				throw new Exception("批次号有误！");
			}

			fBatchID = BatchID;
			fBankType = BatchID.Substring(8,4);

			if(fBankType.IndexOf("E")>-1)
			{
				fBankType = BatchID.Substring(6,4);
			}

			fconnstring = zwconnstring;
			incconnstring=incconn;
			ywconnstring=ywconn;
			hdconnstring=hdconn;
			zjbconnstring=zjbconn;
		}


		public static Param[] GetRefundState_Other(string batchid, string strconn)
		{
			Param[] ht = new Param[8];
			MySqlAccess da = new MySqlAccess(strconn);
			try
			{
				da.OpenConn();

				string strOther="( o.FHandleBatchId='"+batchid+"' or o.FBeforeBatchId1='"+batchid+"' or o.FBeforeBatchId2='"+batchid+"' or o.FBeforeBatchId3='"+batchid+"' or o.FBeforeBatchId4='"+batchid+"')";

				string strSql = "select count(*),sum(Freturnamt) from c2c_zwdb.t_refund_total t right join c2c_zwdb.t_refund_other o on t.foldid=o.foldid  where "+strOther+" and t.Fstate=2"; //成功
				DataTable dt = da.GetTable(strSql);
				string successcount = dt.Rows[0][0].ToString();
				string successsum = MoneyTransfer.FenToYuan(dt.Rows[0][1].ToString());


				strSql = "select count(*),sum(Freturnamt) from c2c_zwdb.t_refund_total t right join c2c_zwdb.t_refund_other o on t.foldid=o.foldid  where "+strOther+" and t.Fstate=3"; //成功
				dt = da.GetTable(strSql);
				string errorcount = dt.Rows[0][0].ToString();
				string errorssum = MoneyTransfer.FenToYuan(dt.Rows[0][1].ToString());

				strSql = "select count(*),sum(Freturnamt) from c2c_zwdb.t_refund_total  t right join c2c_zwdb.t_refund_other o on t.foldid=o.foldid  where "+strOther+" and t.Fstate in(1,5,6,8,7)"; //退款处理中   //退款处理中
				dt = da.GetTable(strSql);
				string payingcount = dt.Rows[0][0].ToString();
				string payingssum = MoneyTransfer.FenToYuan(dt.Rows[0][1].ToString());

				//				strSql = "select count(*),sum(Famt) from c2c_zwdb.t_payfund_other where Fbatchid='" 
				//					+ batchid + "' and FErrType=1 and FStatus=1"; //凋整为失败
				//				dt = da.GetTable(strSql);
				string adjustcount ="0";
				string adjustsum ="0.00";

				da.CloseConn();
				ht[0] = new BankLib.Param();
				ht[0].ParamName = "successcount";
				ht[0].ParamValue = successcount;

				ht[1] = new BankLib.Param();
				ht[1].ParamName = "successsum";
				ht[1].ParamValue = successsum;

				ht[2] = new BankLib.Param();
				ht[2].ParamName = "errorcount";
				ht[2].ParamValue = errorcount;

				ht[3] = new BankLib.Param();
				ht[3].ParamName = "errorssum";
				ht[3].ParamValue = errorssum;

				ht[4] = new BankLib.Param();
				ht[4].ParamName = "payingcount";
				ht[4].ParamValue = payingcount;

				ht[5] = new BankLib.Param();
				ht[5].ParamName = "payingssum";
				ht[5].ParamValue = payingssum;

				ht[6] = new BankLib.Param();
				ht[6].ParamName = "adjustcount";
				ht[6].ParamValue = adjustcount;

				ht[7] = new BankLib.Param();
				ht[7].ParamName = "adjustsum";
				ht[7].ParamValue = adjustsum;
				return ht;
			}
			catch(Exception err)
			{
				log4net.ILog log = log4net.LogManager.GetLogger("BatchPay_Service.GetRefundState_Other");
				if(log.IsErrorEnabled)
					log.Error("batchid=" + batchid,err);

				return null;
			}
			finally
			{
				da.Dispose();
			}

		}



		public class ZWDicClass
		{
			//获取账务配置置
			public static string GetZWDicValue(string dicKey,string  strConn)
			{
				MySqlAccess dazw = new MySqlAccess(strConn);
				try
				{
					if(dicKey==null||dicKey=="")
						return "";
			
					string strSql="select FDicValue from c2c_zwdb.t_zwdic_info where FDicKey='"+dicKey+"'";
					dazw.OpenConn();
				
					string fvalue= dazw.GetOneResult(strSql);
					if(fvalue==null||fvalue=="")
					{
						throw new LogicException("未查询到"+dicKey+"对应的配置的值！");
					}
					else
					{
						return fvalue;
					}


				}
				catch(Exception ex)
				{
					log4net.ILog log = log4net.LogManager.GetLogger("获取账务配置置失败");
					if(log.IsErrorEnabled) log.Error(ex.Message);
					return "";
				}
				finally
				{
					dazw.Dispose();
				
				}
			}
		}
	}
}
