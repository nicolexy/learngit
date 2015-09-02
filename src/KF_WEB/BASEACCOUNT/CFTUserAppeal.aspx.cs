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
	/// CFTUserAppeal ��ժҪ˵����
	/// </summary>
	public partial class CFTUserAppeal : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.DropDownList ddlStateType;
		protected System.Web.UI.WebControls.TextBox tbFNum;
		protected System.Web.UI.WebControls.DropDownList Dropdownlist1;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��
			// �ڴ˴������û������Գ�ʼ��ҳ��
			try
			{
				Label1.Text = Session["uid"].ToString();

				string szkey = Session["SzKey"].ToString();
				//int operid = Int32.Parse(Session["OperID"].ToString());

				//if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID,"CFTUserAppeal")) Response.Redirect("../login.aspx?wh=1");

				if(!classLibrary.ClassLib.ValidateRight("CFTUserAppeal",this)) Response.Redirect("../login.aspx?wh=1");
			}
			catch
			{
				Response.Redirect("../login.aspx?wh=1");
			}

			if(!IsPostBack)
			{
				Table2.Visible = false;				
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
			this.DataGrid1.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.DataGrid1_ItemDataBound);
			this.pager.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(this.ChangePage);

		}
		#endregion

		public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
		{
			pager.CurrentPageIndex = e.NewPageIndex;
			BindData(e.NewPageIndex);
		}

		private void ValidateDate()
		{
			string stmp = tbFuin.Text.Trim();
			if(stmp == "")
			{
				throw new Exception("�������û��ʺţ�");
			}

			ViewState["fuin"] = classLibrary.setConfig.replaceMStr(stmp);
		}

		protected void Button2_Click(object sender, System.EventArgs e)
		{
			try
			{
				string strszkey = Session["SzKey"].ToString().Trim();
				int ioperid = Int32.Parse(Session["OperID"].ToString());
				int iserviceid = Common.AllUserRight.GetServiceID("CFTUserAppeal") ;
				string struserdata = Session["uid"].ToString().Trim();
				string content = struserdata + "ִ����[�������߲�ѯ]����,��������[" + classLibrary.setConfig.replaceMStr(tbFuin.Text.Trim()) + "]ʱ��:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

				Common.AllUserRight.UpdateSession(strszkey,ioperid,PublicRes.GROUPID,iserviceid,struserdata,content);

				string log = SensitivePowerOperaLib.MakeLog("get",struserdata,"[�������߲�ѯ]",
					tbFuin.Text.Trim(),"","","99","99","","1",pager.PageSize.ToString());

				if(!SensitivePowerOperaLib.WriteOperationRecord("CFTUserAppeal",log,this))
				{
					
				}

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
				pager.RecordCount= 10000;//GetCount(); 
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

			Finance_Header fh = setConfig.setFH(this);
			qs.Finance_HeaderValue = fh;
            DataSet ds = new DataSet();
            DataSet ds1 = qs.GetCFTUserAppealListNew(fuin, "", "", 99, 100, "", "9", start, max, 99);//ftype=100��ѯ������������

            //�����ҳ����
            if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds1.Tables[0];
                dt = PublicRes.GetPagedTable(dt, index, pager.PageSize);
                ds.Tables.Add(dt);
            }

            ds = qs.GetCFTUserAppealListFunction(ds);//�ȷ�ҳ�ٴ����ڲ���Ϣ

			if(ds != null && ds.Tables.Count >0 && ds.Tables[0].Rows.Count > 0)
			{
				DataGrid1.DataSource = ds.Tables[0].DefaultView;
				DataGrid1.DataBind();
			}
			else
			{
				//throw new LogicException("û���ҵ���¼��");
				WebUtils.ShowMessage(this,"��ѯ��¼Ϊ��");
			}
		}

		private void DataGrid1_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			string stmp = e.Item.Cells[0].Text.Trim();
			int strlen = stmp.Length;

			if(strlen > 5)
			{
				stmp = "***" + stmp.Substring(3,strlen-3);
				e.Item.Cells[0].Text = stmp;
			}
		}
	}
}
