using System;
using System.Data;
using System.Web.Services.Protocols;
using Tencent.DotNet.Common.UI;
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using System.Collections;
using CFT.CSOMS.BLL.ActivityModule;
using CFT.CSOMS.COMMLIB;


namespace TENCENT.OSS.CFT.KF.KF_Web.NewQueryInfoPages
{
	/// <summary>
    /// QueryWebchatPayActivity ��ժҪ˵����
	/// </summary>
    public partial class QueryWebchatPayActivity : System.Web.UI.Page
	{

        private string[] m_nUserState = 
        {
            "����漰",
            "���û�",
            "���û�",
            "�з�ˢ����",
            "δ֪",
        };

        protected void Page_Load(object sender, System.EventArgs e)
		{
            ButtonBeginDate.Attributes.Add("onclick", "openModeBegin()");
            ButtonEndDate.Attributes.Add("onclick", "openModeEnd()");

            this.ddlActId.SelectedIndexChanged += new EventHandler(ddlActIdSelectedIndexChanged);

			try
			{
				Label1.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();

                if (!IsPostBack)
                {
                    TextBoxBeginDate.Text = DateTime.Now.ToString("yyyy��MM��dd��");
                    TextBoxEndDate.Text = DateTime.Now.ToString("yyyy��MM��dd��");
                    
                    //setConfig.GetActivityList(ddlActId, true);
                }
                 
			}
			catch
			{
				Response.Redirect("../login.aspx?wh=1");
			}
		}

        private void ddlActIdSelectedIndexChanged(object sender, EventArgs e) 
        {
            switch (this.ddlActId.SelectedIndex) 
            {
                case 0:
                    {
                        this.PanelDetail.Visible = false;
                        this.Table2.Visible = true;

                        this.txtTime1.Visible = true;
                        this.txtTime2.Visible = true;
                        this.Label3.Text = "�Ƹ�ͨ������/�˺ţ�";
                        break;
                    }
                case 1:
                    {
                        this.PanelDetail.Visible = true;
                        this.Table2.Visible = false;

                        this.txtTime1.Visible = false;
                        this.txtTime2.Visible = false;
                        this.Label3.Text = "΢�źţ�";
                        break;
                    }
                case 2:
                    {
                        this.PanelDetail.Visible = false;
                        this.Table2.Visible = true;

                        this.txtTime1.Visible = true;
                        this.txtTime2.Visible = true;
                        this.Label3.Text = "�Ƹ�ͨ������/�˺ţ�";
                        break;
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
            this.SendDG.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.SendDG_ItemCommand);
            this.pager.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(this.ChangePage);
            this.sendPager.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(this.SendPage);
            this.receivePager.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(this.ReceivePage);
		}
		#endregion

		public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
		{
			pager.CurrentPageIndex = e.NewPageIndex;
			BindData(e.NewPageIndex);
		}
        public void SendPage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            sendPager.CurrentPageIndex = e.NewPageIndex;
            BindSendData(e.NewPageIndex);
        }
        public void ReceivePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            this.receivePager.CurrentPageIndex = e.NewPageIndex;
            BindReceiveData(e.NewPageIndex);
        }

		private void ValidateDate()
		{
            DateTime begindate, enddate;

            try
            {
                string s_date = TextBoxBeginDate.Text;
                if (s_date != null && s_date != "")
                {
                    begindate = DateTime.Parse(s_date);
                }
                string e_date = TextBoxEndDate.Text;
                if (e_date != null && e_date != "")
                {
                    enddate = DateTime.Parse(e_date);
                }
            }
			catch
			{
				throw new Exception("������������");
			}

            string cft_no = txtCftNo.Text.Trim();

            if (cft_no == "")
            {
                throw new Exception("��������Ϊ�գ�");
            }
		}

        public void btnQuery_Click(object sender, System.EventArgs e)
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
                this.pager.RecordCount = 1000;
                this.receivePager.RecordCount = 1000;
                this.sendPager.RecordCount = 1000;
                BindData(1);
			}
			catch(SoapException eSoap) //����soap���쳣
			{
				string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
				WebUtils.ShowMessage(this.Page,"���÷������" + errStr);
			}
			catch(Exception eSys)
			{
                string errStr = PublicRes.GetErrorMsg(eSys.Message.ToString());
                WebUtils.ShowMessage(this.Page, "��ȡ����ʧ�ܣ�" + errStr);
			}
		}

        private void BindData(int index)
		{
            
            string s_stime = TextBoxBeginDate.Text;
            string s_begindate = "";
            string xykTime = "";
            if (s_stime != null && s_stime != "")
            {
                DateTime begindate = DateTime.Parse(s_stime);
                s_begindate = begindate.ToString("yyyy-MM-dd 00:00:00");
                xykTime = begindate.ToString("yyyy-MM-01 00:00:00");
            }
            string s_etime = TextBoxEndDate.Text;
            string s_enddate = "";
            if (s_etime != null && s_etime != "")
            {
                DateTime enddate = DateTime.Parse(s_etime);
                s_enddate = enddate.ToString("yyyy-MM-dd 23:59:59");
            }

            string cft_no = txtCftNo.Text.Trim();
            string act_id = ddlActId.SelectedValue;

            int max = pager.PageSize;
            int start = max * (index - 1);

            DataSet ds = null;
            if (act_id == "wxzfact")
            {
                ds = new ActivityService().QueryActivity(start, max, act_id, cft_no, s_begindate, s_enddate);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataGrid1.Columns[2].HeaderText = "�����";
                    DataGrid1.DataSource = ds.Tables[0].DefaultView;
                    DataGrid1.DataBind();
                }
                else
                {
                    DataGrid1.DataSource = null;
                    DataGrid1.DataBind();
                }
            }
            else if (act_id == "xyk") 
            {
                
                string wxUin =  WeChatHelper.GetUINFromWeChatName(cft_no);
                this.lblAccUin.Text = wxUin;
                //lblTotal.Text = "0";

                //����
                ds = new ActivityService().QueryActivity(start, max, act_id, cft_no, xykTime, s_enddate, "send");
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    //lblTotal.Text = ds.Tables[0].Rows[0]["FSendCount"].ToString();

                    SendDG.DataSource = ds.Tables[0].DefaultView;
                    SendDG.DataBind();
                }
                else 
                {
                    SendDG.DataSource = null;
                    SendDG.DataBind();
                }

                ds = new ActivityService().QueryActivity(start, max, act_id, cft_no, xykTime, s_enddate, "rec");
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0) 
                {
                    ReceiveDG.DataSource = ds.Tables[0].DefaultView;
                    ReceiveDG.DataBind();
                }
                else
                {
                    ReceiveDG.DataSource = null;
                    ReceiveDG.DataBind();
                }

                SendDetailDG.DataSource = null;
                SendDetailDG.DataBind();
            }
            else if (act_id == "handq")
            {
                DataGrid1.DataSource = null;
                DataGrid1.DataBind();
                DataGrid1.Columns[2].HeaderText = "�Ƹ�ͨ�ʺ�";
                ds = new ActivityService().QueryHandQActivity(cft_no, s_begindate, s_enddate, start, max);//(start, max, act_id, cft_no, xykTime, s_enddate, "rec");
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    #region
                    /*
                                <asp:BoundColumn DataField="FUin" HeaderText="�û��˺�"></asp:BoundColumn>
                                <asp:BoundColumn DataField="FActId" HeaderText="�ID"></asp:BoundColumn>
								<asp:BoundColumn DataField="FActName" HeaderText="�����"></asp:BoundColumn>
                                <asp:BoundColumn DataField="FTransId" HeaderText="������"></asp:BoundColumn>
                                <asp:BoundColumn DataField="FAccepter" HeaderText="���ս�ƷQQ"></asp:BoundColumn>
                                <asp:BoundColumn DataField="FState_str" HeaderText="�ʸ�״̬"></asp:BoundColumn>
                                <asp:BoundColumn DataField="FPrizeDesc_str" HeaderText="��Ʒ����"></asp:BoundColumn>
                                <asp:BoundColumn DataField="FTicketOrder" HeaderText="��Ʒ��Ϣ"></asp:BoundColumn>
                                <asp:BoundColumn DataField="FPayFee_str" HeaderText="֧�����"></asp:BoundColumn>
                                <asp:BoundColumn DataField="FPayTime" HeaderText="֧��ʱ��"></asp:BoundColumn>
                                <asp:BoundColumn DataField="FCreateTime" HeaderText="����ʱ��"></asp:BoundColumn>
                                <asp:BoundColumn DataField="FErrInfo" HeaderText="������Ϣ"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fstandby2_str" HeaderText="��������"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fstandby2_str2" HeaderText="��Ʒ����"></asp:BoundColumn>
                     * 
                     * 
                                <asp:BoundColumn DataField="sendnickname" HeaderText="���ͷ�΢���ǳ�"></asp:BoundColumn>
                                <asp:BoundColumn DataField="name" HeaderText="��Ʒ����"></asp:BoundColumn>
                                <asp:BoundColumn DataField="suborderid" HeaderText="������"></asp:BoundColumn>
                                <asp:BoundColumn DataField="createtime" HeaderText="����ʱ��"></asp:BoundColumn>
                                <asp:BoundColumn DataField="expiretime" HeaderText="��Ʒ����ʱ��"></asp:BoundColumn>
                                <asp:BoundColumn DataField="usestateStr" HeaderText="ʹ��״̬"></asp:BoundColumn>
                                <asp:BoundColumn DataField="giftId" HeaderText="��ƷID"></asp:BoundColumn>
                     */
                    #endregion
                    ds.Tables[0].Columns.Add("FActName", typeof(String));
                    ds.Tables[0].Columns.Add("FAccepter", typeof(String));
                    ds.Tables[0].Columns.Add("FState_str", typeof(String));
                    ds.Tables[0].Columns.Add("FPrizeDesc_str", typeof(String));
                    ds.Tables[0].Columns.Add("FTicketOrder", typeof(String));
                    ds.Tables[0].Columns.Add("FPayFee_str", typeof(String));
                    ds.Tables[0].Columns.Add("Fstandby2_str", typeof(String));
                    ds.Tables[0].Columns.Add("Fstandby2_str2", typeof(String));
                    // ��û�п��������ֶ���ʲô���壬��UI�а�����ֻ��������
                    ds.Tables[0].Columns.Add("sendnickname", typeof(String));
                    ds.Tables[0].Columns.Add("name", typeof(String));
                    ds.Tables[0].Columns.Add("suborderid", typeof(String));
                    ds.Tables[0].Columns.Add("createtime", typeof(String));
                    ds.Tables[0].Columns.Add("expiretime", typeof(String));
                    ds.Tables[0].Columns.Add("giftId", typeof(String));
                    ds.Tables[0].Columns.Add("usestateStr", typeof(String));

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        dr["FActName"] = dr["FStandby2"].ToString();
                        dr["FAccepter"] = dr["Fuin"].ToString();

                        if (dr["FState"].ToString() == "0")
                        {
                            dr["FState_str"] = "��֧��";
                        }
                        else if (dr["FState"].ToString() == "1")
                        {
                            dr["FState_str"] = "��֧��";
                        }
                        else if (dr["FState"].ToString() == "7")
                        {
                            dr["FState_str"] = "����/���֤�Ѳ���";
                        }
                        else
                        {
                            dr["FState_str"] = dr["FState"].ToString();
                        }

                        dr["FPrizeDesc_str"] = dr["FPrizeDesc"].ToString();
                        dr["FTicketOrder"] = dr["FPrizeInfo"].ToString();
                        dr["FPayFee_str"] = setConfig.FenToYuan(dr["FPayFee"].ToString()).ToString();
                        dr["Fstandby2_str2"] = dr["FPrizeType"].ToString();

                        if (dr["FPrizeLv"].ToString() == "-1")
                        {
                            dr["Fstandby2_str"] = string.Format("{0}default", m_nUserState[int.Parse(dr["FUsrState"].ToString())]);
                        }
                        else if (dr["FPrizeLv"].ToString() == "0")
                        {
                            dr["Fstandby2_str"] = string.Format("{0}ĩ�Ƚ�", m_nUserState[int.Parse(dr["FUsrState"].ToString())]);
                        }
                        else if (dr["FPrizeLv"].ToString() == "99")
                        {
                            dr["Fstandby2_str"] = string.Format("{0}��ˢ����", m_nUserState[int.Parse(dr["FUsrState"].ToString())]);
                        }
                        else
                        {
                            dr["Fstandby2_str"] = string.Format("{0}{1}", m_nUserState[int.Parse(dr["FUsrState"].ToString())], dr["FPrizeLv"].ToString());
                        }
                        

                    }

                    DataGrid1.DataSource = ds.Tables[0].DefaultView;
                    DataGrid1.DataBind();
                }
            }
            
			
		}

        private void BindSendData(int index)
        {
            string s_stime = TextBoxBeginDate.Text;
            string s_begindate = "";
            if (s_stime != null && s_stime != "")
            {
                DateTime begindate = DateTime.Parse(s_stime);
                s_begindate = begindate.ToString("yyyy-MM-01 00:00:00");
            }
            
            string cft_no = txtCftNo.Text.Trim();
            string act_id = ddlActId.SelectedValue;

            int max = pager.PageSize;
            int start = max * (index - 1);

            DataSet ds = new ActivityService().QueryActivity(start, max, act_id, cft_no, s_begindate, "", "send");
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                SendDG.DataSource = ds.Tables[0].DefaultView;
                SendDG.DataBind();
            }
            else
            {
                SendDG.DataSource = null;
                SendDG.DataBind();
            }

            SendDetailDG.DataSource = null;
            SendDetailDG.DataBind();
        }

        private void BindReceiveData(int index)
        {
            string s_stime = TextBoxBeginDate.Text;
            string s_begindate = "";
            if (s_stime != null && s_stime != "")
            {
                DateTime begindate = DateTime.Parse(s_stime);
                s_begindate = begindate.ToString("yyyy-MM-01 00:00:00");
            }
            
            string cft_no = txtCftNo.Text.Trim();
            string act_id = ddlActId.SelectedValue;

            int max = pager.PageSize;
            int start = max * (index - 1);

            DataSet ds = ds = new ActivityService().QueryActivity(start, max, act_id, cft_no, s_begindate, "", "rec");
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ReceiveDG.DataSource = ds.Tables[0].DefaultView;
                ReceiveDG.DataBind();
            }
            else
            {
                ReceiveDG.DataSource = null;
                ReceiveDG.DataBind();
            }

            SendDetailDG.DataSource = null;
            SendDetailDG.DataBind();
        }

        private void SendDG_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {

            string sendid = e.Item.Cells[0].Text.Trim();
            GetDetail(sendid);
        }
        private void GetDetail(string sendid) 
        {
            DataSet ds = new ActivityService().QueryXYKSendDetail(sendid);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                SendDetailDG.DataSource = ds.Tables[0].DefaultView;
                SendDetailDG.DataBind();
            }
            else
            {
                SendDetailDG.DataSource = null;
                SendDetailDG.DataBind();
            }
        }
	}
}