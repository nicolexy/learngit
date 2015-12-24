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
using System.Web.Services.Protocols;
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.CFT.KF.KF_Web;

namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
	/// <summary>
	/// BatPayQuery ��ժҪ˵����
	/// </summary>
	public partial class BatPayQuery : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.Button btnMain;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				Label1.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();
				int operid = Int32.Parse(Session["OperID"].ToString());

				//if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID,"InfoCenter")) Response.Redirect("../login.aspx?wh=1");
				if(!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter",this)) Response.Redirect("../login.aspx?wh=1");
			}
			catch
			{
				Response.Redirect("../login.aspx?wh=1");
			}

			if (!IsPostBack)
			{            
				this.txBatchOrder.Text = "1";
				this.TextBoxBeginDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
			}

			InitGrid();
		}

		private void InitGrid()
		{
			Query_Service.Query_Service qs = new Query_Service.Query_Service();
			DataTable dt = qs.BatPay_InitGrid_B(this.TextBoxBeginDate.Text.Trim(),this.txBatchOrder.Text.Trim()).Tables[0];

			if(dt == null)
			{
				WebUtils.ShowMessage(this.Page,"��ȡ����ʱ���������ԡ�");
				return;
			}

			dt.Columns.Add("FBatchOrder");
			ShowBatPay(dt);		
		}

		private void ShowBatPay(DataTable dt)
		{
			DateTime BeginDate = Convert.ToDateTime(TextBoxBeginDate.Text.Trim());
			string strDateUse = BeginDate.ToString("yyyyMMdd");
                
			if(dt.Rows.Count == 0)
			{
				if(CanVisible(strDateUse,this.txBatchOrder.Text.Trim()))
				{
					DataRow dr = dt.NewRow();
					dr["FBatchID"] = strDateUse + "1001B" + T0Transfer.Order2Asc(Int32.Parse(txBatchOrder.Text.Trim()));

					string strDate = BeginDate.ToString("yyyy��MM��dd��");
					dr["FDate"] = strDate;
					dr["FStatusName"] = "��δ���ܸ�������";

					if(CanStartTask(strDateUse + "1001B" + T0Transfer.Order2Asc(Int32.Parse(txBatchOrder.Text.Trim()))))
					{
						dr["FMsg"] = "���Ի��ܸ�������";
					}
					else
					{
						dr["FStatusName"] = "�����ڴ��������������ڳ������ա�";
						dr["FMsg"] = "";
					}
					dr["FUrl"] = "BatchID=" + strDateUse + "1001B" + T0Transfer.Order2Asc(Int32.Parse(txBatchOrder.Text.Trim())) + "&WeekIndex=" + TextBoxBeginDate.Text.Trim()
						+ "&BatchOrder=" + this.txBatchOrder.Text.Trim();

					dr["FBankID"] = "��������";

					dr["FbatchOrder"] = T0Transfer.Asc2Order( dr["FBatchID"].ToString().Substring(13,1));
					dt.Rows.Add(dr);
				}
			}
			else
			{
				foreach(DataRow dr in dt.Rows)
				{
					int iStatus = Int32.Parse(dr["FStatus"].ToString());
					dr.BeginEdit();
					dr["FStatusName"] = GetStatusName(iStatus,dr["FBatchID"].ToString());
					if(iStatus == 5 && (dr["FPayCount"] == null || dr["FPayCount"].ToString() == "0"))
					{
						dr["FStatusName"] = "�������޸����¼";
					}
					if((iStatus == 9 || iStatus == 0 || iStatus==11) && CanStartTask(dr["FBatchID"].ToString()))
					{
						dr["FMsg"] = "���Ի��ܸ�������";
					}
					else
					{
						dr["FMsg"] = "";
					}

					dr["FUrl"] = "BatchID=" + dr["FBatchID"].ToString() + "&WeekIndex=" + TextBoxBeginDate.Text.Trim()
						+ "&BatchOrder=" + this.txBatchOrder.Text.Trim();

					dr["FBankID"] = GetBankName(dr["FBankType"].ToString());

					string tmp = dr["FDate"].ToString();

					tmp = tmp.Substring(0,4) + "��" + tmp.Substring(4,2) + "��" + tmp.Substring(6,2) + "��";
					dr["FDate"] = tmp;

					dr["FbatchOrder"] = T0Transfer.Asc2Order( dr["FBatchID"].ToString().Substring(13,1));
					dr.EndEdit();
				}
			}
                
			dt.DefaultView.Sort = "FBankType ";
			DataGrid1.DataSource = dt.DefaultView;
			DataGrid1.DataBind();
		}


		private static bool CanVisible(string strDate,string batchorder)
		{
			Query_Service.Query_Service qs = new Query_Service.Query_Service();
			return qs.BatPay_CanVisible_B(strDate,batchorder);
		}

		public static bool CanStartTask(string strBatchID)
		{
			if(CheckSnapFinish(strBatchID))
			{
				if(CheckFinish11(strBatchID))
				{								
					Query_Service.Query_Service qs = new Query_Service.Query_Service();
					return qs.BatPay_SixCheck(strBatchID);
				}
			}
			return false;
		}

		private static bool CheckSnapFinish(string strBatchID)
		{
			Query_Service.Query_Service qs = new Query_Service.Query_Service();
			return qs.BatPay_CheckSnapFinish(strBatchID);
		}

		private static bool CheckFinish11(string strBatchID)
		{
			Query_Service.Query_Service qs = new Query_Service.Query_Service();
			return qs.BatPay_CheckFinish11(strBatchID);
		}

		public static string GetStatusName(int iStatus, string strBatchID)
		{
			string[] strArray = new string[30];
			strArray[0] = "���Ի��ܸ�������";
			strArray[1] = "���������ѻ���";
			strArray[2] = "�������������";
			strArray[3] = "��������������";//"���ڽ��и���";
			strArray[4] = "���������";
			strArray[5] = "����������Ч";
			strArray[6] = "���������"; 
			strArray[9] = "<FONT color=red>������������ʧ��</FONT>";  //FBatchNoExp
			strArray[10] = "�����Чִ�н��"; //FBatchNoImp  FStatusDes
			strArray[11] = "<FONT color=red>�����������ɺ��дҵ��ϵͳʧ��</FONT>";
			strArray[20] = "�����������ύ����"; 
			strArray[21] = "�������������"; 
			strArray[22] = "������һ���ϴ��ɹ�"; 
			strArray[23] = "�����������ϴ��ɹ�"; 
			strArray[24] = "����������";
			strArray[25] = "������������"; 
			strArray[7] = "ת�������������";
			string tmp = "";
			if(strBatchID != "0" && iStatus > 8 && iStatus < 12) tmp = GetErrorMsg(strBatchID, iStatus);

			if(tmp != "")
				return strArray[iStatus] + "��" + tmp;
			else
				return strArray[iStatus];
		}

		private static string GetErrorMsg(string strBatchID, int iStatus)
		{
			Query_Service.Query_Service qs = new Query_Service.Query_Service();
			return qs.BatPay_GetErrorMsg(strBatchID,iStatus);
		}

		public static string GetBankName(string strBankID)
		{
			switch(strBankID.Trim())
			{
				case "1001" :
					return "��������";
				case "1002" :
					return "��������";
				case "1003" : 
					return "��������";
				case "1005" :
					return "ũҵ����"; 
				case "1004" :
					return "�ַ�����"; 
				case "1010" :
					return "����ƽ������";
				case "1020" :
					return "��ͨ����";
				case "1027" :
					return "�㶫��չ����";
				case "1021" :
					return "��������";
				case "9999" :
					return "��������";
				default :
					return "�޴�����";
			}
		}


		#region Web ������������ɵĴ���
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: �õ����� ASP.NET Web ���������������ġ�
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// �����֧������ķ��� - ��Ҫʹ�ô���༭���޸�
		/// �˷��������ݡ�
		/// </summary>
		private void InitializeComponent()
		{    
		}
		#endregion

	}
}
