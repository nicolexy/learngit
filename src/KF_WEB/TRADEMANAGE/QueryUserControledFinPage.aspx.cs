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

using TENCENT.OSS.CFT.KF.KF_Web;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using Tencent.DotNet.Common.UI;
using CFT.CSOMS.BLL.CFTAccountModule;
using System.Web.Services.Protocols;
using CFT.CSOMS.BLL.TradeModule;
using CFT.CSOMS.BLL.FundModule;

namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
	/// <summary>
	/// QueryUserControledFinPage ��ժҪ˵����
	/// </summary>
	public partial class QueryUserControledFinPage : System.Web.UI.Page
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(Session["OperID"] != null)
				this.lb_operatorID.Text = Session["OperID"].ToString();
			
			//lb_operatorID.Text = Session["uid"].ToString();
			string szkey = Session["SzKey"].ToString();
			int operid = Int32.Parse(Session["OperID"].ToString());

			// ����Ӧ�����ʲôȨ�ޣ�
			// if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID,"InfoCenter")) Response.Redirect("../login.aspx?wh=1");
			if(!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter",this)) Response.Redirect("../login.aspx?wh=1");

			if(!IsPostBack)
			{
                //this.tbx_beginDate.Text = DateTime.Now.AddDays(-1).ToString("yyyy��MM��dd��");
                //this.tbx_endDate.Text = DateTime.Now.ToString("yyyy��MM��dd��");

				// û��Ҫ�����ѯ�ܼ�¼�ĸ����ˣ���ΪĿǰûʲô��
                //this.pager.RecordCount = 1000;
                //this.pager.PageSize = 10;
			}

            //this.btnBeginDate.Attributes.Add("onclick", "openModeBegin()"); 
            //this.btnEndDate.Attributes.Add("onclick","openModeEnd()");

		//	this.pager.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(pager_PageChanged);
			// �ڴ˴������û������Գ�ʼ��ҳ��
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
            this.DataGrid_QueryResult.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.DataGrid_ItemCommand);
		}
		#endregion



        private void StartQuery(string qqid)
        {
            try
            {
                //int iNumStart = 0;

                //iNumStart = (this.pager.CurrentPageIndex - 1) * this.pager.PageSize;

                Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
                //qs.Finance_HeaderValue = setConfig.setFH(Session["OperID"].ToString(),Request.UserHostAddress);
                qs.Finance_HeaderValue = classLibrary.setConfig.setFH(this);

                DataTable dt = new ControlFundService().QueryUserControledRecordCgi(qqid, Session["uid"].ToString());
                if (dt == null || dt.Rows.Count == 0)
                {
                    this.ShowMsg("��ѯ��¼Ϊ��");
                }

                dt.Columns.Add("Fcur_typeName", typeof(String));
                dt.Columns.Add("FbalanceStr", typeof(String));
                dt.Columns.Add("FlstateName", typeof(String));
                dt.Columns.Add("Fcreate_time", typeof(String));
                dt.Columns.Add("FtypeText", typeof(String));
                dt.Columns.Add("Fmodify_time", typeof(String));
                dt.Columns.Add("uid", typeof(String));

                foreach (DataRow dr in dt.Rows)
                {
                    string cur_type = dr["cur_type"].ToString().Trim();
                    DataSet ds = qs.QueryUserControledRecord(qqid, "", "",cur_type, 0, 1);
                   // ds = qs.QueryUserControledRecord(qqid, "2014-07-30 00:00:00", "2014-08-01 00:00:00", "", 0, 1);
                    if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    {
                        dr["Fcur_typeName"] = "";
                        dr["FlstateName"] = "";
                        dr["Fcreate_time"] = "";
                        dr["FtypeText"] = "";
                        dr["Fmodify_time"] = "";
                        dr["uid"] = "";
                    }
                    else
                    {
                        DataRow row = ds.Tables[0].Rows[0];
                        dr["Fcur_typeName"] = row["Fcur_typeName"].ToString();
                        dr["FlstateName"] = row["FlstateName"].ToString();
                        dr["Fcreate_time"] = row["Fcreate_time"].ToString();
                        dr["FtypeText"] = row["FtypeText"].ToString();
                        dr["Fmodify_time"] = row["Fmodify_time"].ToString();
                        dr["uid"] = row["uid"].ToString();
                    }
                }

                classLibrary.setConfig.FenToYuan_Table(dt, "balance", "FbalanceStr");

                this.DataGrid_QueryResult.DataSource = dt;
                this.DataGrid_QueryResult.DataBind();
            }
            catch (SoapException eSoap) //����soap���쳣
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "���÷������" + errStr);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "��ѯ�쳣��" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
            }
        }
        private void RemoveLogQuery(string qqid)
        {
            //new TradeService().RemoveInsert(this.tbx_acc.Text.Trim(), "1000.00", "������ÿ�һ��ͨ", "13100", DateTime.Now, Session["uid"].ToString());
            try
            {
                DataSet dsRemove = new ControlFundService().RemoveControledFinLogQuery(qqid);
                this.DataGrid_Remove.DataSource = dsRemove.Tables[0];
                this.DataGrid_Remove.DataBind();
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "��ѯ�����־�쳣��" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
            }
        }
        private DataTable getRemoveLog(int? index = null)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("FbalanceStr", typeof(System.String));
            dt.Columns.Add("FtypeText", typeof(System.String));
            dt.Columns.Add("cur_type", typeof(System.String));
            if (index.HasValue)
            {
                DataRow dr = dt.NewRow();
                dr["FbalanceStr"] = this.DataGrid_QueryResult.Items[index.Value].Cells[1].Text.Trim();
                dr["FtypeText"] = this.DataGrid_QueryResult.Items[index.Value].Cells[4].Text.Trim();
                dr["cur_type"] = this.DataGrid_QueryResult.Items[index.Value].Cells[5].Text.Trim();
                dt.Rows.Add(dr);
            }
            else
            {
                foreach (DataGridItem item in this.DataGrid_QueryResult.Items)
                {
                    DataRow dr = dt.NewRow();
                    dr["FbalanceStr"] = item.Cells[1].Text.Trim();
                    dr["FtypeText"] = item.Cells[4].Text.Trim();
                    dr["cur_type"] = item.Cells[5].Text.Trim();
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }
		protected void btn_query_Click(object sender, System.EventArgs e)
		{
            //string strBeginDate = "",strEndDate = "";

            //try
            //{
            //    if(this.tbx_beginDate.Text.Trim() != "" && this.tbx_endDate.Text.Trim() != "")
            //    {
            //        strBeginDate = DateTime.Parse(this.tbx_beginDate.Text).ToString("yyyy-MM-dd HH:mm:ss");
            //        strEndDate = DateTime.Parse(this.tbx_endDate.Text).ToString("yyyy-MM-dd HH:mm:ss");
            //    }
            //}
            //catch
            //{
            //    ShowMsg("���ڸ�ʽ����ȷ��");
            //    return;
            //}

			StartQuery(this.tbx_acc.Text);
            RemoveLogQuery(this.tbx_acc.Text);
		}
        protected void btn_removeAll_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (!ClassLib.ValidateRight("UnFinanceControl", this))
                {
                    throw new Exception("��Ȩ�ޣ�");
                }
                DataTable dt = getRemoveLog();
                if (new ControlFundService().RemoveUserControlFin(this.tbx_acc.Text.Trim(), "", "", Session["uid"].ToString(), 4, dt))
                {
                    WebUtils.ShowMessage(this.Page, "����ɹ���");
                    RemoveLogQuery(this.tbx_acc.Text.Trim());
                }

            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "����������˻�����쳣��" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
            }

        }

		private void ShowMsg(string msg)
		{
			Response.Write("<script language=javascript>alert('" + msg + "')</script>");
		}

        //private void pager_PageChanged(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        //{
        //    this.pager.CurrentPageIndex = e.NewPageIndex;

        //    string strBeginDate = "",strEndDate = "";

        //    try
        //    {
        //        if(this.tbx_beginDate.Text.Trim() != "" && this.tbx_endDate.Text.Trim() != "")
        //        {
        //            strBeginDate = DateTime.Parse(this.tbx_beginDate.Text).ToString("yyyy-MM-dd");
        //            strEndDate = DateTime.Parse(this.tbx_endDate.Text).ToString("yyyy-MM-dd");
        //        }
        //    }
        //    catch
        //    {
        //        ShowMsg("���ڸ�ʽ����ȷ��");
        //        return;
        //    }

        //    StartQuery(this.tbx_acc.Text,strBeginDate,strEndDate);
        //}

        //�����ť
       
        public void DataGrid1_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            //if (e.Item.ItemType == ListItemType.Item)
            //{
            object obj = e.Item.Cells[7].FindControl("removeButton");
           
            if (obj != null)
            {
                decimal balance = Decimal.Parse(e.Item.Cells[9].Text.Trim());//�ܿؽ��
                Button lb = (Button)obj;
                if (balance > 0)
                {
                    lb.Visible = true;
                }
            }
            //  }
        }

        private void DataGrid_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
         //   string uid = e.Item.Cells[8].Text.Trim();
            string cur_type = e.Item.Cells[5].Text.Trim();//����
            string balance = e.Item.Cells[9].Text.Trim();//���

            DataTable dt = getRemoveLog(e.Item.ItemIndex);
            try
            {

                if (e.CommandName == "remove")
                {
                    if (!ClassLib.ValidateRight("UnFinanceControl", this))
                    {
                        throw new Exception("��Ȩ�ޣ�");
                    }
                    if (new ControlFundService().RemoveUserControlFin(this.tbx_acc.Text.Trim(), cur_type, balance, Session["uid"].ToString(), 3,dt))
                    {
                        WebUtils.ShowMessage(this.Page, "����ɹ���");
                        RemoveLogQuery(this.tbx_acc.Text.Trim());
                    }
                }
            
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "����û��ܿ��ʽ��쳣��" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
            }
        }


	}
}
