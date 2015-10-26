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
using TENCENT.OSS.C2C.Finance.Common.CommLib;
using System.Configuration;
using CFT.CSOMS.BLL.TransferMeaning;

namespace TENCENT.OSS.CFT.KF.KF_Web.SysManage
{
	/// <summary>
	/// SysBulletinManage ��ժҪ˵����
	/// </summary>
	public partial class SysBulletinManage : System.Web.UI.Page
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				Label1.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();
				//int operid = Int32.Parse(Session["OperID"].ToString());

				//if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID,"InfoCenter")) Response.Redirect("../login.aspx?wh=1");

				if(!classLibrary.ClassLib.ValidateRight("InfoCenter",this)) Response.Redirect("../login.aspx?wh=1");
			}
			catch
			{
				Response.Redirect("../login.aspx?wh=1");
			}

			if(!IsPostBack)
			{
				if(Request.QueryString["sysid"] != null && Request.QueryString["sysid"].Trim() != "")
				{
					ddlSysList.SelectedValue = Request.QueryString["sysid"].Trim();
				}
				btnIssue.Attributes["onClick"] = "return confirm('ȷ��Ҫִ�С�������������');";

                if (Request.QueryString["QuestionManage"] =="1" )
                {
                   
                }

                btQueryContacts.Attributes.Add("onclick", "window.showModalDialog('BulletinContacts.aspx','center=yes;help=no;status=no;dialogheight:600px;dialogwidth:600px;');");
                //lbNotify.Attributes.Add("onclick", "window.showModalDialog('BulletinSendMail.aspx','center=yes;help=no;status=no;dialogheight:400px;dialogwidth:400px;scroll:no');");
			}
           
			BindData();
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

		private void DataGrid1_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			object obj = e.Item.Cells[1].FindControl("labOrder");
			if(obj != null)
			{
				Label lab = (Label)obj;
				lab.Text = (e.Item.ItemIndex+1).ToString();
			}

			obj = e.Item.Cells[9].FindControl("lbGoHistory");
			if(obj != null)
			{
				LinkButton lb = (LinkButton)obj;
				if(lb.Visible)
					lb.Attributes["onClick"] = "return confirm('ȷ��Ҫִ�С��Ƶ���ʷ��������');";
			}

			obj = e.Item.Cells[10].FindControl("lbDel");
			if(obj != null)
			{
				LinkButton lb = (LinkButton)obj;
				if(lb.Visible)
					lb.Attributes["onClick"] = "return confirm('ȷ��Ҫִ�С�ɾ����������');";
			}
		}

        private void BindData()
        {
            try
            {
                string listtype = ddlSysList.SelectedValue;
                string outmsg = "";
                if (listtype == "8" || listtype == "21" || listtype == "22" || listtype == "23" || listtype == "24")
                {
                    this.Table3.Visible = true;
                    this.Table1.Visible = false;
                    Table2.Visible = false;
                    ddl_uctype.Visible = false;

                    string fbanktype = this.txtBankType.Text.Trim();
                    Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
                    DataSet ds = new DataSet();

                    if (listtype == "8")
                        ds = qs.GetSysBankBulletin(fbanktype, out outmsg);
                    else
                    {
                        string busineType = "";
                        //ҵ������
                        //0 ����(Ĭ��)
                        //1 �����п�����
                        //2 ������
                        //3 ���ÿ�����
                        if (listtype == "21")
                            busineType = "0";
                        else if (listtype == "22")
                            busineType = "1";
                        else if (listtype == "23")
                            busineType = "2";
                        else if (listtype == "24")
                            busineType = "3";

                        ds = qs.GetSysBankInterface(busineType, fbanktype, out outmsg);
                    }

                    this.labQueryName.Visible = true;
                    this.txtBankType.Visible = true;
                    this.btnQuery.Visible = true;
                    this.btadd.Visible = true;
                    this.labQueryName.Text = "�������ͱ���";

                    if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    {
                        WebUtils.ShowMessage(this.Page, "û�ж�ȡ�����ݣ�" + PublicRes.GetErrorMsg(outmsg));
                        return;
                    }
                    ds.Tables[0].Columns.Add("FBank_TypeName", typeof(System.String));
                    ds.Tables[0].Columns.Add("Fsysid", typeof(String));

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        dr.BeginEdit();

                        dr["Fsysid"] = listtype;
                        dr["FBank_TypeName"] = Transfer.returnDicStr("BANK_TYPE", dr["Fbanktype"].ToString());

                        dr.EndEdit();
                    }

                    if (listtype == "21" || listtype == "22" || listtype == "23" || listtype == "24")
                        Datagrid2.Columns[3].Visible = false;

                    //����
                    DataTable dt = ds.Tables[0];
                    DataView view = dt.DefaultView;
                    view.Sort = "Fstartime desc";
                    dt = view.ToTable();
                    DataSet dsResult = new DataSet();
                    dsResult.Tables.Add(dt);

                    Datagrid2.DataSource = dsResult.Tables[0].DefaultView;
                    Datagrid2.DataBind();

                }
                else if (listtype == "7") //��������ɷѹ���7����
                {
                    this.Table3.Visible = false;
                    this.Table1.Visible = false;
                    Table2.Visible = true;
                    ddl_uctype.Visible = true;

                    string servicecode = this.txtBankType.Text.Trim();
                    Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

                    DataSet ds = qs.GetUtility_charge(servicecode, out outmsg);

                    if (ds == null || ds.Tables.Count == 0 || ds.Tables[0] == null)
                    {
                        WebUtils.ShowMessage(this.Page, "û�ж�ȡ�����ݣ�" + PublicRes.GetErrorMsg(outmsg));
                        return;
                    }

                    Datagrid3.DataSource = ds.Tables[0].DefaultView;
                    Datagrid3.DataBind();
                    this.labQueryName.Visible = true;
                    this.txtBankType.Visible = true;
                    this.btnQuery.Visible = true;
                    this.btadd.Visible = false; //����������,ֻ����鿴���޸�.

                    this.labQueryName.Text = "�ɷ�ҵ����";
                }
                else
                {
                    this.Table1.Visible = true;
                    this.Table3.Visible = false;
                    Table2.Visible = false;
                    ddl_uctype.Visible = false;

                    this.labQueryName.Visible = false;
                    this.txtBankType.Visible = false;
                    this.btnQuery.Visible = false;
                    this.btadd.Visible = false;

                    this.txtBankType.Text = "";//��Ҫ���

                    Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

                    DataSet ds = qs.GetSysBulletin(listtype, out outmsg);

                    if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    {
                        WebUtils.ShowMessage(this.Page, "û�ж�ȡ�����ݣ�" + PublicRes.GetErrorMsg(outmsg));
                        return;
                    }
                    ds.Tables[0].Columns.Add("UrlNotify", typeof(String));
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        r["UrlNotify"] = "BulletinSendMail.aspx?id=" + r["FID"];
                    }

                    DataGrid1.DataSource = ds.Tables[0].DefaultView;
                    DataGrid1.DataBind();
                }
            }
            catch (SoapException eSoap) //����soap���쳣
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "���÷������" + errStr);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "��ȡ����ʧ�ܣ�" + eSys.Message.ToString());
            }
        }

		private void DataGrid1_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
		{
			string fid = e.Item.Cells[0].Text.Trim();
			string sysid = ddlSysList.SelectedValue;
            string title = e.Item.Cells[3].Text.Trim();

            object obj = e.Item.Cells[11].FindControl("lbNotify");

			switch(e.CommandName)
			{
				case "PRIOR" : //������
					GoPrior(fid);
					break; 
				case "NEXT" : //������
					GoNext(fid);
					break;
				case "CHANGE" : //�޸�
					Response.Redirect("SysBulletinManage_Detail.aspx?sysid=" + sysid + "&fid=" + fid);
					break;
				case "GOHISTORY" : //ת�Ƶ���ʷ��ֻ��sysid=1��Ч
					GoHistory(fid);
					break;
				case "DEL" : //ɾ��������¼
					Del(fid);
                    break;
                case "Notify": //֪ͨ�ʼ�
                    Notify(title,obj);
					break;
			}
		}

        private void Notify(string title, object obj)
        {
            //if (obj != null)
            //{
            //    Button lb = (Button)obj;
            //    title = "aaaaaaa";
            //    string url = "BulletinSendMail.aspx?title=" + title;
            //    lb.Attributes.Add("onclick", "window.showModalDialog('"+url+",'center=yes;help=no;status=no;dialogheight:600px;dialogwidth:600px;scroll:no');");
            //}
            
        }

		private void Del(string fid)
		{
			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

			string msg = "";
			if(qs.SysBulletinDel(fid,Request.UserHostAddress,out msg))
			{
				BindData();
			}
			else
			{
				WebUtils.ShowMessage(this.Page,"ɾ��ʧ�ܣ�" + PublicRes.GetErrorMsg(msg));
			}
		} 

		private void GoHistory(string fid)
		{
			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

			string msg = "";
			if(qs.SysBulletinGoHistory(fid,Request.UserHostAddress,out msg))
			{
				BindData();
			}
			else
			{
				WebUtils.ShowMessage(this.Page,"����ʧ�ܣ�" + PublicRes.GetErrorMsg(msg));
			}
		}

		private void GoPrior(string fid)
		{
			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

			string msg = "";
			if(qs.SysBulletinGoPrior(fid,Request.UserHostAddress,out msg))
			{
				BindData();
			}
			else
			{
				WebUtils.ShowMessage(this.Page,"����ʧ�ܣ�" + PublicRes.GetErrorMsg(msg));
			}
		}

		private void GoNext(string fid)
		{
			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

			string msg = "";
			if(qs.SysBulletinGoNext(fid,Request.UserHostAddress,out msg))
			{
				BindData();
			}
			else
			{
				WebUtils.ShowMessage(this.Page,"����ʧ�ܣ�" + PublicRes.GetErrorMsg(msg));
			}
		}

		protected void btnIssue_Click(object sender, System.EventArgs e)
		{
			//����
			string sysid = ddlSysList.SelectedValue;

			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

			string msg = "";
			if(qs.SysBulletinIssue(sysid,out msg))
			{
				WebUtils.ShowMessage(this.Page,"�����ɹ�");
			}
			else
			{
				WebUtils.ShowMessage(this.Page,"����ʧ�ܣ�" + PublicRes.GetErrorMsg(msg));
			}
		}

		protected void btnNew_Click(object sender, System.EventArgs e)
		{
			//����
			string sysid = ddlSysList.SelectedValue;
			Response.Redirect("SysBulletinManage_Detail.aspx?sysid=" + sysid);
		}

        protected void btadd_Click(object sender, System.EventArgs e)
        {
            string sysid = ddlSysList.SelectedValue;
            Response.Redirect("SysBulletinManage_Detail.aspx?sysid=" + sysid + "&opertype=1");
        }

        protected void btnQuery_Click(object sender, System.EventArgs e)
        {
            BindData();
        }

        public void DGData_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            string listtype = ddlSysList.SelectedValue;
            object obj = e.Item.Cells[11].FindControl("lbNotify");
            if (obj != null)
            {
                LinkButton lb = (LinkButton)obj;
                if (listtype == "1" || listtype == "2")
                {
                    lb.Visible = true;
                }
            }
        }
	}
}
