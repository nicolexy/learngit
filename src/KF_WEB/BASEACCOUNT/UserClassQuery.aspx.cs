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
using TENCENT.OSS.CFT.KF.DataAccess;
using System.Web.Services.Protocols;

using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;
using TENCENT.OSS.CFT.KF.Common;

namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
	/// <summary>
	/// UserClassQuery ��ժҪ˵����
	/// </summary>
	public partial class UserClassQuery : System.Web.UI.Page
	{


	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��
			try
			{
				Label1.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();
				//int operid = Int32.Parse(Session["OperID"].ToString());
				//if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID,"CFTUserPickTJ")) Response.Redirect("../login.aspx?wh=1");

				if(!classLibrary.ClassLib.ValidateRight("CFTUserPickTJ",this)) Response.Redirect("../login.aspx?wh=1");
			}
			catch
			{
				Response.Redirect("../login.aspx?wh=1");
			}
			if(!IsPostBack)
			{
				Table2.Visible = false;				}
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
		{    			this.pager.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(this.ChangePage);

		}
		#endregion
		public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
		{
			pager.CurrentPageIndex = e.NewPageIndex;
			BindData(e.NewPageIndex);
		}
		private void ValidateDate()
		{
			string stmp = txtQQ.Text.Trim();
			ViewState["fuin"] = classLibrary.setConfig.replaceMStr(stmp);
		}
		protected void Button2_Click(object sender, System.EventArgs e)
		{
			try
			{
				ValidateDate();
			}
			catch(Exception err)
			{
				WebUtils.ShowMessage(this.Page,err.Message);
				return;
			}
			try
			{
				Table2.Visible = true;
				pager.RecordCount= 10000; 
				BindData(1);
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
		private void BindData(int index)
		{
			string fuin = ViewState["fuin"].ToString();

			int max = pager.PageSize;
			int start = max * (index-1) + 1;
			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
//			Finance_Header fh = new Finance_Header();
//			fh.UserIP = Request.UserHostAddress;
//			fh.UserName = Session["uid"].ToString();
//			fh.OperID = Int32.Parse(Session["OperID"].ToString());
//			fh.SzKey = Session["SzKey"].ToString();
//			qs.Finance_HeaderValue = fh;			Query_Service.Finance_Header fh = classLibrary.setConfig.setFH(this);
			qs.Finance_HeaderValue = fh;
			DataSet ds = qs.GetUserClassQueryListForThis(fuin,start,max);
			if(ds != null && ds.Tables.Count >0)
			{
				ds.Tables[0].Columns.Add("Result",typeof(string));
				ds.Tables[0].Columns.Add("URL",typeof(string));
				foreach(DataRow dr in ds.Tables[0].Rows)
				{
					string Fpickstate = dr["Fpickstate"].ToString();
					string Fcard_stat = dr["Fcard_stat"].ToString();
                    dr["URL"] = "CFTUserCheck.aspx?fid=&db=&tb=&flist_id=" + dr["flist_id"].ToString();
					/*		2012/5/30 �޸�����ı�ʾ�߼�
					if(Fpickstate == "0")
					{
						if(Fcard_stat == "1")
							dr["Result"] = "���֤�����";
						else if(Fcard_stat == "2")
							dr["Result"] = "�����";
						else if(Fcard_stat == "3")
							dr["Result"] = "��������֤��";
						else if(Fcard_stat == "4")
							dr["Result"] = "���ʧ��";
						else if(Fcard_stat == "5")
							dr["Result"] = "���ȷ��ʧ��";
						else if(Fcard_stat == "9")
							dr["Result"] = "��֤����";
						else if(Fcard_stat == "10")
							dr["Result"] = "��֤ʧ��,�ȴ�����";
						else
							dr["Result"] = "δ����δ����" + Fpickstate;
					}
					else if(Fpickstate == "1")
					{
						dr["Result"] = "���쵥";
					}
					else if(Fpickstate == "2")
					{
						dr["Result"] = "��֤�ɹ�";
					}
					else if(Fpickstate == "3")
					{
						dr["Result"] = "��֤ʧ��";
					}
					else
					{
						dr["Result"] = "δ����" + Fpickstate;
					}
					*/

					if(dr["Fcard_stat"].ToString() == "1" && dr["Fcre_stat"].ToString() == "1")
					{
						dr["Result"] = "��֤�ɹ�";
					}
					else if(dr["Fcard_stat"].ToString() == "9" && dr["Fcre_stat"].ToString() == "9")
					{
						dr["Result"] = "��֤ʧ��";
					}
					else if(dr["Fcard_stat"].ToString() == "10" && dr["Fcre_stat"].ToString() == "10")
					{
						dr["Result"] = "����";
					}
					else
					{
						dr["Result"] = "��֤������";
					}
				}
				DataGrid1.DataSource = ds.Tables[0].DefaultView;
				DataGrid1.DataBind();
			}
			else
			{
				throw new LogicException("û���ҵ���¼��");
			}
		}

		
	}
}
