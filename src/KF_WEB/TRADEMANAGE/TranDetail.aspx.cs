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
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using System.Web.Services.Protocols;
using TENCENT.OSS.CFT.KF.Common;

namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
	/// <summary>
	/// TranDetail ��ժҪ˵����
	/// </summary>
	public partial class TranDetail : System.Web.UI.Page
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��
			try
			{

				labUid.Text = "ҳ���ѷ���";
				return;
			}
			catch
			{
				Response.Redirect("../login.aspx?wh=1");
			}

			if(!IsPostBack)
			{
				
				string listid = Request.QueryString["listid"];
				int trantype = Int32.Parse(Request.QueryString["trantype"]);

				if(listid == null || listid.Trim() == "")
				{
					WebUtils.ShowMessage(this.Page,"��������");
				}


				try
				{
//					BindInfo(listid,trantype);
				}
				catch(LogicException err)
				{
					WebUtils.ShowMessage(this.Page,err.Message);
				}
				catch(SoapException eSoap) //����soap���쳣
				{
					string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
					WebUtils.ShowMessage(this.Page,"���÷������" + errStr);
				}
				catch(Exception eSys)
				{
					WebUtils.ShowMessage(this.Page,"��ȡ����ʧ�ܣ�" + eSys.Message.ToString());
				}
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

		private void BindInfo(string appealid, int trantype)
		{			
//			Query_Service.Query_Service qs = new Query_Service.Query_Service();
//
//			DataSet ds =  qs.GetTransportDetail(appealid,trantype);
//
//			if(ds != null && ds.Tables.Count >0 && ds.Tables[0].Rows.Count == 1 )
//			{
//				ds.Tables[0].Columns.Add("Ftransport_typeName");
//				ds.Tables[0].Columns.Add("Fgoods_typeName");
//				ds.Tables[0].Columns.Add("Ftran_typeName");
//				ds.Tables[0].Columns.Add("FstateName");
//
//				#region ת��
//				foreach(DataRow dr in ds.Tables[0].Rows)
//				{
//					string strtmp = dr["Ftransport_type"].ToString();
//					if(strtmp == "1")
//					{
//						dr["Ftransport_typeName"] = "��������";
//					}
//					else if(strtmp == "2")
//					{
//						dr["Ftransport_typeName"] = "�򷽷����˻�";
//					}						
//					else
//					{
//						dr["Ftransport_typeName"] = "δ֪����" + strtmp;
//					}
//
//					strtmp = dr["Fgoods_type"].ToString();
//					if(strtmp == "1")
//					{
//						dr["Fgoods_typeName"] = "ʵ����Ʒ";
//					}
//					else if(strtmp == "2")
//					{
//						dr["Fgoods_typeName"] = "������Ʒ";
//					}						
//					else
//					{
//						dr["Fgoods_typeName"] = "δ֪����" + strtmp;
//					}
//
//					strtmp = dr["Ftran_type"].ToString();
//					if(strtmp == "1")
//					{
//						dr["Ftran_typeName"] = "ƽ��";
//					}
//					else if(strtmp == "2")
//					{
//						dr["Ftran_typeName"] = "���";
//					}			
//					else if(strtmp == "3")
//					{
//						dr["Ftran_typeName"] = "email����";
//					}
//					else if(strtmp == "4")
//					{
//						dr["Ftran_typeName"] = "�ֻ�";
//					}
//					else if(strtmp == "5")
//					{
//						dr["Ftran_typeName"] = "����";
//					}
//					else
//					{
//						dr["Ftran_typeName"] = "δ֪����" + strtmp;
//					}
//					
//
//					
//					foreach(DataColumn dc in ds.Tables[0].Columns)
//					{
//						System.Web.UI.Control obj = FindControl(dc.ColumnName);
//						if(obj != null)
//						{
//							Label lab = (Label)obj;
//							lab.Text = ds.Tables[0].Rows[0][dc.ColumnName].ToString();
//						}
//					}
//
//				}
//
//				#endregion
//				
//			}
		}

	}
}
