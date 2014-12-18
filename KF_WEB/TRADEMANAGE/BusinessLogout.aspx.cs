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


namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
	/// <summary>
	/// BusinessLogout ��ժҪ˵����
	/// </summary>
	public partial class BusinessLogout : System.Web.UI.Page
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				Label1.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();
				int operid = Int32.Parse(Session["OperID"].ToString());

				//if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID,"InfoCenter")) Response.Redirect("../login.aspx?wh=1");
				if(!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter",this)) Response.Redirect("../login.aspx?wh=1");

				if(!IsPostBack)
				{
					this.btnLogout.Attributes["onClick"] = "if(!confirm('ȷ��Ҫע�����̻���')) return false;"; 
				}
			}
			catch
			{
				Response.Redirect("../login.aspx?wh=1");
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
            this.DataGrid1.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.DataGrid1_ItemCommand);
		}
		#endregion

		protected void btnLogout_Click(object sender, System.EventArgs e)
		{
			try
			{
				string strszkey = Session["SzKey"].ToString().Trim();
				int ioperid = Int32.Parse(Session["OperID"].ToString());
				int iserviceid = TENCENT.OSS.CFT.KF.Common.AllUserRight.GetServiceID("InfoCenter") ;
				string struserdata = Session["uid"].ToString().Trim();
				string content = struserdata + "ִ����[�̻�ע��]����,��������[" + this.txtFspid.Text.Trim()
					+ "]ʱ��:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

				Common.AllUserRight.UpdateSession(strszkey,ioperid,PublicRes.GROUPID,iserviceid,struserdata,content);
			}
			catch(Exception err)
			{
				WebUtils.ShowMessage(this.Page,err.Message);
				return;
			}

			try
			{
				if(this.txtFspid.Text.Trim() == "")
				{
					throw new Exception("�������̻���!");
				}
				if(this.txtReason.Text.Trim() == "")
				{
					throw new Exception("������ԭ��!");
				}

				string log = SensitivePowerOperaLib.MakeLog("edit",Session["uid"].ToString().Trim(),"[�̻�ע��]",
					this.txtFspid.Text.Trim(),Session["uid"].ToString(),this.txtReason.Text.Trim());

				if(!SensitivePowerOperaLib.WriteOperationRecord("InfoCenter",log,this))
				{
					
				}

				Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
				qs.BusinessLogout(this.txtFspid.Text.Trim(),Session["uid"].ToString(),this.txtReason.Text.Trim());
			}
			catch(SoapException eSoap)
			{
				string errStr = PublicRes.GetErrorMsg(eSoap.Message);
				WebUtils.ShowMessage(this.Page,"���÷������" + errStr);
			}
			catch(Exception eSys)
			{
				WebUtils.ShowMessage(this.Page,eSys.Message);
			}
		}

        protected void btnQuery_Click(object sender, System.EventArgs e)
        {
            Table2.Visible = true;
            try
            {
                string spid = this.txtFspid.Text;
                if (string.IsNullOrEmpty(spid))
                {
                    throw new Exception("�������̻���!");
                }

                Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
                DataSet ds = qs.QueryBussFreezeList(spid.Trim(), "8", "");
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ds.Tables[0].Columns.Add("amendstate_str", typeof(String));//״̬
                    ds.Tables[0].Columns.Add("amendtype_str", typeof(String));//״̬

                    Hashtable ht1 = new Hashtable();
                    ht1.Add("0", "�ȴ���������");
                    ht1.Add("1", "�ȴ��쵼����");
                    ht1.Add("-1", "�ȴ��������");
                    ht1.Add("11", "�ȴ���Ч��");
                    ht1.Add("2", "�ȴ���������");
                    ht1.Add("-2", "�ȴ���Ʒ���");
                    ht1.Add("3", "����ͨ��");
                    ht1.Add("-3", "�ȴ��ͷ�����");
                    ht1.Add("4", "��������");
                    ht1.Add("7", "���뱻���");
                    ht1.Add("8", "�ȴ���С�̻������");
                    ht1.Add("9", "�ȴ�ָ�������");

                    Hashtable ht2 = new Hashtable();
                    ht2.Add("8", "�̻�ע��");

                    classLibrary.setConfig.DbtypeToPageContent(ds.Tables[0], "amendstate", "amendstate_str", ht1);
                    classLibrary.setConfig.DbtypeToPageContent(ds.Tables[0], "amendtype", "amendtype_str", ht2);

                    DataGrid1.DataSource = ds.Tables[0].DefaultView;
                    DataGrid1.DataBind();
                }
            }
            catch (SoapException eSoap)
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message);
                WebUtils.ShowMessage(this.Page, "���÷������" + errStr);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, eSys.Message);
            }
        }

        private void DataGrid1_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            Table3.Visible = true;
            string s = e.Item.Cells[7].Text;
            txtApplyResult.Text = s.Replace("&nbsp;", ""); //ע��ԭ��

            //ͨ��taskid��ȡ�̻���Ϣ
            string taskid = e.Item.Cells[8].Text;
            string spid = e.Item.Cells[9].Text;
            string spname = e.Item.Cells[0].Text;
            string disresult = e.Item.Cells[10].Text;//��ͨ��ԭ��

            txtCheckNoResult.Text = disresult.Replace("&nbsp;", "");

            lbSpid.Text = spid;
            lbSpname.Text = spname;
            
            Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            s = qs.GetBDName(spid);
            lbBd.Text = s;
            s = qs.GetWWWAddress(spid);
            lbNetDns.Text = s;
            s = qs.GetTradeType(spid);
            lbClss.Text = s;
        }
	}
}
