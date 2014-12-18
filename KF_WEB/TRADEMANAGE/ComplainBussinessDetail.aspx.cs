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
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.CFT.KF.KF_Web;
using System.Web.Services.Protocols;
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using System.Web.Mail;
using System.IO;

namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
	/// <summary>
    /// ComplainBussinessDetail ��ժҪ˵����
	/// </summary>
    public partial class ComplainBussinessDetail : System.Web.UI.Page
	{
        private string qbussid, bussid, qbegindate, qenddate, qpage;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				//Label1.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();

               // if (!classLibrary.ClassLib.ValidateRight("BussComplain", this)) Response.Redirect("../login.aspx?wh=1");
			}
			catch
			{
				Response.Redirect("../login.aspx?wh=1");
			}
			if(!IsPostBack)
			{
                if (Request.QueryString["qbussid"] != null && Request.QueryString["qbussid"].Trim() != "")
				{
                    qbussid = Request.QueryString["qbussid"].Trim();
                    ViewState["qbussid"] = qbussid;
				}
				else
				{
                    qbussid = "";
                    ViewState["qbussid"] = qbussid;
				}
                if (Request.QueryString["begindate"] != null && Request.QueryString["begindate"].Trim() != "")
                {
                    qbegindate = Request.QueryString["begindate"].Trim();
                    ViewState["begindate"] = qbegindate;
                }
                else
                {
                    ViewState["begindate"] = DateTime.Now.ToString("yyyy��MM��dd��");
                }

                if (Request.QueryString["enddate"] != null && Request.QueryString["enddate"].Trim() != "")
                {
                    qenddate = Request.QueryString["enddate"].Trim();
                    ViewState["enddate"] = qenddate;
                }
                else
                {
                    ViewState["enddate"] = DateTime.Now.ToString("yyyy��MM��dd��");
                }
                if (Request.QueryString["qpage"] != null && Request.QueryString["qpage"].Trim() != "")
                {
                    qpage = Request.QueryString["qpage"];
                    ViewState["qpage"] = qpage;
                }
                else
                {
                    ViewState["qpage"] = "1";
                }

                if (Request.QueryString["bussid"] != null && Request.QueryString["bussid"].Trim() != "")
                {
                    bussid = Request.QueryString["bussid"].Trim();
                    ViewState["bussid"] = bussid;
                }
                else
                {
                    bussid = "";
                    ViewState["bussid"] = bussid;
                }

                if (bussid == "")
				{
					labTitle.Text = "�����̻�";
				}
				else
				{
					labTitle.Text = "�޸��̻�";

                    BindData(bussid);
				}
			}
			else
			{
                qbussid = ViewState["qbussid"].ToString();
                qenddate = ViewState["enddate"].ToString();
                qbegindate = ViewState["begindate"].ToString(); 
                qpage=ViewState["qpage"].ToString();
                bussid = ViewState["bussid"].ToString();
			}

		}

        private void BindData(string bussid)
		{
			//��
			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

			string msg = "";
            Query_Service.ComplainBussClass sc = qs.GetComplainBussDetail(bussid, out msg);
			if(sc != null)
			{
				//�󶨿�ʼ
                bussNumber.Text = sc.FBussId.ToString();
                bussName.Text = sc.FBussName;
                bussEmail.Text = sc.FBussEmail;

                bussNumber.ReadOnly = true;
			}
			else
			{
				WebUtils.ShowMessage(this.Page,"��ȡʧ�ܣ�" + msg);
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

		protected void btnSave_Click(object sender, System.EventArgs e)
		{
			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

			string msg = "";
            Query_Service.ComplainBussClass cb = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.ComplainBussClass();
            cb.FBussName = classLibrary.setConfig.replaceMStr(bussName.Text.Trim());
            cb.FBussEmail = classLibrary.setConfig.replaceMStr(bussEmail.Text.Trim());


            if (cb.FBussName == "")
            {
                WebUtils.ShowMessage(this.Page, "�������̻�����");
                return;
            }
            if (cb.FBussEmail == "")
            {
                WebUtils.ShowMessage(this.Page, "������֪ͨ����");
                return;
            }
			
			//�޸�
            if (bussid != "")
			{
				//�޸ġ�
                cb.FBussId = bussid;
                if(qs.ChangeComplainBuss(cb,out msg))
				{
					WebUtils.ShowMessage(this.Page,"�޸ĳɹ�");
				}
				else
				{
					WebUtils.ShowMessage(this.Page,msg);
				}
			}
			else
			{
				//����
                cb.FBussId = bussNumber.Text.Trim();

                if(qs.AddComplainBuss(cb,out msg))
				{
					WebUtils.ShowMessage(this.Page,"�����ɹ�");
				}
				else
				{
					WebUtils.ShowMessage(this.Page, msg);
				}
			}
		}

		protected void btnBack_Click(object sender, System.EventArgs e)
		{
            Response.Redirect("ComplainBussinessInput.aspx?qbussid=" + ViewState["qbussid"] + "&begindate=" + ViewState["begindate"] + "&enddate=" + ViewState["enddate"] + "&qpage=" + ViewState["qpage"]);
		}
	}
}