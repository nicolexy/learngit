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
		public double HisUnbalance = 0;      //��ʷδ������
		public int HisUnbalanceCount = 0;    //��ʷδ�������
		public double TodayUnbalance = 0;    //����δ������
		public int TodayUnbalanceCount = 0;  //����δ�������
		public double TodayRefund = 0;       //�����˿���
		public int TodayRefundCount = 0;     //�����˿����
		public int UnbalanceCount = 0;       //δ���㽻���ܱ���(��ʷδ�������+����δ�������+�����˿����)
		public double Unbalance = 0;         //δ���㽻���ܽ��(��ʷδ������+����δ������-�����˿���)
		public DataSet HisUnbalanceds;    //δ�����¼


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
		
		public DataSet GetHisUnBalance(string Fspid)      //�õ���ʷδ�����¼
		{
			try
			{
				Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
				DataSet ds  = qs.GetQuerySettlementList(Fspid);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) 
                {
                    //���Ϊ�գ��ٲ�һ��΢���̻���
                    ds = qs.QuerySettlementListWechat(Fspid);
                }
				return ds;
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public DataSet GetTodayList(string Fspid)      //����δ������� ����δ������ �����˿���� �����˿���
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


		//�õ��ѽ����¼(�ѻ��˽��,������,�����ѽ������ͬʱΪ0�����ε�(�ͷ�������ϵͳ��ͬ))
		public DataSet GetBalanceList(string Fspid,DateTime BeginDate,DateTime EndDate)
		{
			try
			{
				Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
				DataSet ds  = qs.GetQueryBalanceList(Fspid,BeginDate,EndDate);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) 
                {
                    //���Ϊ�գ��ٲ�һ��΢���̻���
                    ds = qs.QueryBalanceListWechat(Fspid, BeginDate, EndDate);
                }
				return ds;
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
		public DataSet GetBalanceDetilList(string Fspid,string FDrawNo)       //�õ�ÿ�ʽ����¼����ˮ
		{
			try
			{
				Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
				DataSet ds  = qs.GetQueryBalanceDetailList(Fspid,FDrawNo);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) 
                {
                    //���Ϊ�գ��ٲ�΢���̻���
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