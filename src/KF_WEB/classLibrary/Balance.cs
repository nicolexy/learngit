using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace TENCENT.OSS.CFT.KF.KF_Web.classLibrary
{
	public class Balance
	{
		public double HisUnbalance = 0;      //历史未结算金额
		public int HisUnbalanceCount = 0;    //历史未结算笔数
		public double TodayUnbalance = 0;    //今日未结算金额
		public int TodayUnbalanceCount = 0;  //今日未结算笔数
		public double TodayRefund = 0;       //今日退款金额
		public int TodayRefundCount = 0;     //今日退款笔数
		public int UnbalanceCount = 0;       //未结算交易总笔数(历史未结算笔数+今日未结算笔数+今日退款笔数)
		public double Unbalance = 0;         //未结算交易总金额(历史未结算金额+今日未结算金额-今日退款金额)
		public DataSet HisUnbalanceds;    //未结算记录


		public Balance(string Fspid)
		{
			try
			{
				HisUnbalanceds  = GetHisUnBalance(Fspid);

				if(HisUnbalanceds != null && HisUnbalanceds.Tables.Count > 0)
				{
					foreach(DataRow dr in HisUnbalanceds.Tables[0].Rows)
					{
						if(dr["FTransactionCount"] != null && dr["FTransactionCount"].ToString().Trim() != "")
							HisUnbalanceCount += Convert.ToInt32(dr["FTransactionCount"].ToString());
						if(dr["FTransactionAmount"] != null && dr["FTransactionAmount"].ToString().Trim() != "")
							HisUnbalance += Convert.ToDouble(dr["FTransactionAmount"].ToString());
					}
				}

				DataSet dsToday = GetTodayList(Fspid);

				if(dsToday != null && dsToday.Tables.Count > 0)
				{
					if(dsToday.Tables[0].Rows[0][0] != null && dsToday.Tables[0].Rows[0][0].ToString().Trim() != "")
						TodayUnbalanceCount = Convert.ToInt16(dsToday.Tables[0].Rows[0][0].ToString());
					if(dsToday.Tables[0].Rows[0][1] != null && dsToday.Tables[0].Rows[0][1].ToString().Trim() != "")
						TodayUnbalance = Convert.ToDouble(dsToday.Tables[0].Rows[0][1].ToString());
					if(dsToday.Tables[0].Rows[0][2] != null && dsToday.Tables[0].Rows[0][2].ToString().Trim() != "")
						TodayRefundCount = Convert.ToInt32(dsToday.Tables[0].Rows[0][2].ToString());
					if(dsToday.Tables[0].Rows[0][3] != null && dsToday.Tables[0].Rows[0][3].ToString().Trim() != "")
						TodayRefund = Convert.ToDouble(dsToday.Tables[0].Rows[0][3].ToString());
				}

				UnbalanceCount = HisUnbalanceCount + TodayUnbalanceCount + TodayRefundCount;
				Unbalance = HisUnbalance + TodayUnbalance - TodayRefund;
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
		
		public DataSet GetHisUnBalance(string Fspid)      //得到历史未结算记录
		{
			try
			{
				Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
				DataSet ds  = qs.GetQuerySettlementList(Fspid);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) 
                {
                    //如果为空，再查一下微信商户号
                    ds = qs.QuerySettlementListWechat(Fspid);
                }
				return ds;
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public DataSet GetTodayList(string Fspid)      //今日未结算笔数 今日未结算金额 今日退款笔数 今日退款金额
		{
			try
			{
				Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
				DataSet ds  = qs.GetQuerySettlementTodayList(Fspid);
				return ds;
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}


		//得到已结算记录(把划账金额,结算金额,手续费金额三项同时为0的屏蔽掉(客服与其它系统不同))
		public DataSet GetBalanceList(string Fspid,DateTime BeginDate,DateTime EndDate)
		{
			try
			{
				Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
				DataSet ds  = qs.GetQueryBalanceList(Fspid,BeginDate,EndDate);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) 
                {
                    //如果为空，再查一下微信商户号
                    ds = qs.QueryBalanceListWechat(Fspid, BeginDate, EndDate);
                }
				return ds;
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
		public DataSet GetBalanceDetilList(string Fspid,string FDrawNo)       //得到每笔结算记录的流水
		{
			try
			{
				Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
				DataSet ds  = qs.GetQueryBalanceDetailList(Fspid,FDrawNo);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) 
                {
                    //如果为空，再查微信商户号
                    ds = qs.QueryBalanceDetailListWechat(Fspid, FDrawNo);
                }
				return ds;
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
		
	}
}