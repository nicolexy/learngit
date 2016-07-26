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
using System.Web.Services.Protocols;
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using TENCENT.OSS.CFT.KF.Common;
using CFT.CSOMS.BLL.TransferMeaning;

namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
	/// <summary>
	/// B2CReturnQuery_Detail ��ժҪ˵����
	/// </summary>
	public partial class B2CReturnQuery_Detail : TENCENT.OSS.CFT.KF.KF_Web.PageBase
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��
			try
			{

				this.labUid.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();
				int operid = Int32.Parse(Session["OperID"].ToString());

				//if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "InfoCenter")) Response.Redirect("../login.aspx?wh=1");
				if(!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter",this)) Response.Redirect("../login.aspx?wh=1");
			}

			catch
			{
				Response.Redirect("../login.aspx?wh=1");
			} 

			if(!IsPostBack)
			{

				string tranid = Request.QueryString["tranid"];

				if(tranid == null || tranid.Trim() == "")
				{
					WebUtils.ShowMessage(this.Page,"��������");
					return;
				}


				string drawid = Request.QueryString["drawid"];
				if(drawid == null || drawid.Trim() == "")
				{
					drawid = "";
				}

				try
				{
					BindInfo(tranid,drawid);
					ViewState["tranid"] = tranid;
					ViewState["drawid"] = drawid;
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

		private void BindInfo(string tranid, string drawid)
		{

			Query_Service.Query_Service qs = new Query_Service.Query_Service();

			DataSet ds =  qs.GetB2cReturnDetail(tranid,drawid);

			if(ds != null && ds.Tables.Count >0 && ds.Tables[0].Rows.Count > 0 )
			{
				DataRow dr = ds.Tables[0].Rows[0];				

				labFareaName.Text = PublicRes.GetString(dr["FareaName"]);
                labFbank_typeName.Text = Transfer.returnDicStr("BANK_TYPE", PublicRes.GetInt(dr["FBank_Type"]));

				labFbank_id.Text = PublicRes.GetString(dr["Fbank_id"]);
				labFbank_name.Text = PublicRes.GetString(dr["Fbank_name"]);
				labFbuyid.Text = PublicRes.GetString(dr["Fbuyid"]);
				labFcityName.Text = PublicRes.GetString(dr["FcityName"]);

				labFcreate_time.Text = PublicRes.GetString(dr["Fcreate_time"]);
				labFdraw_id.Text = PublicRes.GetString(dr["Fdraw_id"]);
				labFemail.Text = PublicRes.GetString(dr["Femail"]);
				labFip.Text = PublicRes.GetString(dr["Fip"]);
				labFlist_stateName.Text = PublicRes.GetString(dr["Flist_stateName"]);
                labFmemo.Text = dr["FexplainEx"].ToString();

				labFmodify_time.Text = PublicRes.GetString(dr["Fmodify_time"]);
				labFop_name.Text = PublicRes.GetString(dr["Fop_name"]);
				labFrb_feeName.Text = PublicRes.GetString(dr["Frb_feeName"]);
				labFrefund_typeName.Text = PublicRes.GetString(dr["Frefund_typeName"]);
				labFrp_feeName.Text = PublicRes.GetString(dr["Frp_feeName"]);
				labFspid.Text = PublicRes.GetString(dr["Fspid"]);
				labFstatusName.Text = PublicRes.GetString(dr["FstatusName"]);
				labFtransaction_id.Text = PublicRes.GetString(dr["Ftransaction_id"]);
				labFQQ.Text = PublicRes.GetString(dr["Frec_uin"]);
				labFaddress.Text = PublicRes.GetString(dr["Faddress"]);

                this.labFstandby6.Text = PublicRes.GetString(dr["Fstandby6"]);//���ֵ�id

                labHandleMemo.Text = dr["FHandleMemoEx"].ToString();
			}
			else
			{
				throw new LogicException("û���ҵ���¼��");
			}
		}
	}
}

