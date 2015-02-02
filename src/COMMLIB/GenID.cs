using System;
using System.Threading;

namespace commLib
{
	/// <summary>
	/// GenID 的摘要说明。
	/// </summary>
	public class GenID
	{
		
		public static int StaticNo = 1000; //初始值 当达到9999后，则循环，从10001开始
		public static bool StaticNoManageSign = true;

		/// <summary>
		/// 声称订单号，保证不重复管理器
		/// 初始值为1000，使用时，每次+1；当达到9999时，循环使用。 跟在秒后使用。
		/// </summary>
		
		private static void StaticNoManage()
		{
			//如果标志位为false,则等待
			try
			{
				while (!StaticNoManageSign)
				{
					Thread.Sleep(5);
				}

				StaticNoManageSign = false; 

				StaticNo ++;

				if (StaticNo >= 9999) 
				{
					StaticNo = 1001;  //清空为初始状态
				}
			}
			finally
			{
				StaticNoManageSign = true;
			}
		}

		/// <summary>
		/// 生成一个不同的交易单号
		/// </summary>
		/// <returns></returns>
		public static string genLstID(string MerchartID)
		{
			//生成交易单号（生成规则参见tipyluo 支付网关V2.0概要设计-附录2编码规则）
			//交易单号为28位长的数值，其中前10位为C2C网站编号(SPID)，由支付通统一分配；
			//之后8位为订单产生的日期，如20050415；
			//最后10位C2C需要保证一天内不同的事务（用户订购或使用一次服务），其ID不相同。

			StaticNoManage();

			string str = DateTime.Now.Ticks.ToString();
			int ilen   = str.Length;
			str = str.Substring(ilen-5,2);
			string listID = "02" + MerchartID + DateTime.Now.ToString("yyMMdd") + DateTime.Now.ToString("HHmmss") + StaticNo.ToString(); //如果保证不与前台的交易单不重复？？
			return listID;
		}

		public static string genLstID_GetMoney(string MerchartID) //提现
		{
			//生成交易单号（生成规则参见tipyluo 支付网关V2.0概要设计-附录2编码规则）
			//交易单号为28位长的数值，其中前10位为C2C网站编号(SPID)，由支付通统一分配；
			//之后8位为订单产生的日期，如20050415；
			//最后10位C2C需要保证一天内不同的事务（用户订购或使用一次服务），其ID不相同。
			
			StaticNoManage();

			string str = DateTime.Now.Ticks.ToString();
			int ilen   = str.Length;
			str = str.Substring(ilen-5,2);
			string listID = "03" + MerchartID + DateTime.Now.ToString("yyMMdd") + DateTime.Now.ToString("HHmmss") + StaticNo.ToString(); //如果保证不与前台的交易单不重复？？
			return listID;
		}

		//生成一个充值单
		public static string genSaveID(out string Msg)
		{
			Msg = null;
			try
			{
				StaticNoManage();

				string saveID = "103" + DateTime.Now.ToString("yyMMddHHmmssff") + StaticNo.ToString(); //精确到秒后6位
				return saveID;	
			}
			catch(Exception e)
			{
				Msg = "生成充值单失败！" + e.Message.ToString().Replace("'","’");
				throw;
			}
			
		}

		//生成一个给银行的订单号
		public static string genBanklist(out string Msg)
		{
			Msg = null;
			try
			{
				StaticNoManage();

				string newBankList = "CFT" + DateTime.Now.ToString("yMMddHHmmss") + StaticNo.ToString(); //精确到毫秒级 秒后3位
				return newBankList;
			}
			catch(Exception e)
			{
				Msg = "生成给银行订单号失败！" + e.Message.ToString().Replace("'","’");
				throw;
			}
		}


		//生成一个订单消息号
		public static string genMSGID(out string Msg)
		{
			Msg = null;
			try
			{
				StaticNoManage();

				string saveID = "FW" + DateTime.Now.ToString("yyMMddHHmmssff") + StaticNo.ToString(); //精确到秒后6位
				return saveID;	
			}
			catch(Exception e)
			{
				Msg = "生成订单消息号失败！" + e.Message.ToString().Replace("'","’");
				throw;
			}
			
		}

        //生成订单迁移服务消息号
        public static string GenOrderMigrationMSGId(string transactionId)
        {
            try
            {
                StaticNoManage();
                string listID = transactionId + DateTime.Now.ToString("yyMMdd") + DateTime.Now.ToString("HHmmss") + StaticNo.ToString();
                return listID;
            }
            catch (Exception e)
            {
                return "";
            }
        }
	}
}
