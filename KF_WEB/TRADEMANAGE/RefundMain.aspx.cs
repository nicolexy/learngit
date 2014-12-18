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
	/// RefundMain ��ժҪ˵����
	/// </summary>
	public partial class RefundMain : System.Web.UI.Page
	{
		private string WeekIndex = "0";
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				Label_uid.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();
				int operid = Int32.Parse(Session["OperID"].ToString());

				//if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID,"InfoCenter")) Response.Redirect("../login.aspx?wh=1");
				if(!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter",this)) Response.Redirect("../login.aspx?wh=1");

				ButtonBeginDate.Attributes.Add("onclick", "openModeBegin()"); 

				if (!IsPostBack)
				{
					WeekIndex = DateTime.Today.AddDays(-1).ToString("yyyy��MM��dd��");
					TextBoxBeginDate.Text = WeekIndex;
				}
				else
				{
					WeekIndex = TextBoxBeginDate.Text.Trim();
				}

			}
			catch
			{
				Response.Redirect("../login.aspx?wh=1");
			}

			try
			{   
				InitGrid();   
			}
			catch(Exception ex)
			{
				WebUtils.ShowMessage(this.Page,ex.Message);//"��ʾ����ʱ���������ԡ�");
			}
		}

		private void InitGrid()
		{
			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
			DataTable dt = qs.BatPay_InitGrid_R(WeekIndex).Tables[0];

			if(dt == null)
			{
				WebUtils.ShowMessage(this.Page,"��ȡ����ʱ���������ԡ�");
				return;
			}

			ShowBatPay(dt);          
		}
	
		private void ShowBatPay(DataTable dt)
		{
			DateTime BeginDate = DateTime.Parse(WeekIndex);  
			string strDateUse = BeginDate.ToString("yyyyMMdd");
 
			dt.Columns.Add("Detail",typeof(string));
			if(dt.Rows.Count == 0)
			{
				if(CanVisible(strDateUse))
				{
					DataRow dr = dt.NewRow();
					dr["FBatchID"] = strDateUse + "1001R";

					string strDate = BeginDate.ToString("yyyy��MM��dd��");
					dr["FDate"] = strDate;
					dr["FStatusName"] = "��δ�����˵�����";
					dr["FMsg"] = "���Ի����˵�����";
					dr["FUrl"] = "BatchID=" + strDateUse + "1001R" + "&WeekIndex=" + WeekIndex;
					dr["FBankID"] = "��������";
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
						dr["FStatusName"] = "���������˵���¼";
					}
					if((iStatus == 9 ) )
					{
						dr["FMsg"] = "���Ի����˵�����";
					}
					else
					{
						dr["FMsg"] = "";
					}
					if(dr["FPayCount"] != null || dr["FPayCount"].ToString() != "0")
					{
						dr["Detail"] = "��ϸ";
					}

					dr["FUrl"] = "BatchID=" + dr["FBatchID"].ToString() + "&WeekIndex=" + WeekIndex;

					dr["FBankID"] = GetBankName(dr["FBankType"].ToString());

					string tmp = dr["FDate"].ToString();

					tmp = tmp.Substring(0,4) + "��" + tmp.Substring(4,2) + "��" + tmp.Substring(6,2) + "��";
					dr["FDate"] = tmp;
					dr.EndEdit();
				}
			}
                
			dt.DefaultView.Sort = "FBankType ";
			DataGrid1.DataSource = dt.DefaultView;
			DataGrid1.DataBind();
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

		private static bool CanVisible(string strDate)
		{
			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
			return qs.BatPay_CanVisible_R(strDate);
		}

		public static string GetBankName(string strBankID)
		{
			if(strBankID == "9999")
				return "��������";
			else
				return classLibrary.setConfig.convertbankType(strBankID);
			
		}

		public static string GetStatusName(int iStatus, string strBatchID)
		{
			string[] strArray = new string[30];
			strArray[0] = "�����˵�������";
			strArray[1] = "�˵������ѻ���";
			strArray[2] = "�˵����������";
			strArray[3] = "�˵�����������";
			strArray[4] = "�˵������";
			strArray[5] = "�˵��������Ч";
			strArray[6] = "�˵�����������";
			strArray[9] = "<FONT color=red>�˵���������ʧ��</FONT>";
			strArray[10] = "�˵�������Чִ�н��";
			strArray[11] = "<FONT color=red>�˵��������ɺ��дҵ��ϵͳʧ��</FONT>";
			strArray[20] = "�˵��������ύ����"; 
			strArray[21] = "�˵����������"; 
			strArray[22] = "�˵����һ���ϴ��ɹ�"; 
			strArray[23] = "�˵���������ϴ��ɹ�"; 
			strArray[24] = "�˵��������";
			strArray[25] = "�˵����������"; 
			strArray[7] = "ת�������������";

			string tmp = "";
			if(strBatchID != "0" && iStatus > 8 && iStatus < 12) tmp = "";

			if(tmp != "")
				return strArray[iStatus] + "��" + tmp;
			else
				return strArray[iStatus];
            
		}

	}
}
