using System;
using System.Data;
using System.Web.UI.WebControls;
using Tencent.DotNet.Common.UI;
using System.Web.Services.Protocols;
using System.Web;
using CFT.CSOMS.BLL.TransferMeaning;


namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
	/// <summary>
	/// UserAccountQuery1 ��ժҪ˵����
	/// </summary>
	public partial class UserAccountQuery1 : PageBase
	{
        public string dProv, dCity;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��
			if (!IsPostBack)
			{
				try
				{
					this.Label_uid.Text = Session["uid"].ToString();
					string szkey = Session["SzKey"].ToString();
                    if (!classLibrary.ClassLib.ValidateRight("InfoCenter", this)) Response.Redirect("../login.aspx?wh=1");
				}
				catch
				{
					Response.Redirect("../login.aspx?wh=1");
				} 
				showList();
			}
			else
			{
				CheckInput();
			}			
		}
		private void CheckInput()
		{
			#region			
			Button1.Attributes.Add("onclick", "return checkvlid(this);" );						
			#endregion
		}
		protected void showEdit()
		{
			this.PanelList.Visible   = false;
			this.PanelDetail.Visible = true;
		}
		protected void showList()
		{
			this.PanelList.Visible   = true;
			this.PanelDetail.Visible = false;			
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
            this.DGData.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.DGData_ItemCommand);
		}
		#endregion

		protected void Button1_Click(object sender, System.EventArgs e)
		{
			clickEvent();
		}

        public void DGData_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            object obj = e.Item.Cells[13].FindControl("lbChange");
            string s_state = e.Item.Cells[2].Text.Trim();//����״̬
            string s_optype = e.Item.Cells[4].Text.Trim();//������
            if (obj != null)
            {
                LinkButton lb = (LinkButton)obj;
                if (s_state != "" && s_state == "����" && s_optype == "������")
                {
                    lb.Visible = true;
                }
                lb.Attributes["onClick"] = "return confirm('ȷ��Ҫִ�С��ⶳ��������');";
            }
        }

        private void DGData_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            string iprov = e.Item.Cells[5].Text.Trim();
            string icity = e.Item.Cells[6].Text.Trim();
            string state = e.Item.Cells[2].Text.Trim();
            string bankid = e.Item.Cells[7].Text.Trim();
            string trueName = e.Item.Cells[15].Text.Trim();
            string LastIP = e.Item.Cells[8].Text.Trim();
            string BankName = e.Item.Cells[0].Text.Trim();
            string Modify_Time = e.Item.Cells[12].Text.Trim();
            Session["QQID"] = this.TextBox1_QQID.Text.Trim();
            string Memo = e.Item.Cells[10].Text.Trim().Replace("&nbsp;", "");
            string banktype = Transfer.convertbankType(e.Item.Cells[9].Text.Trim());
            string Compayname = e.Item.Cells[16].Text.Trim().Replace("&nbsp;", ""); ;
            string AccCreate = e.Item.Cells[11].Text.Trim();

            string s_curtype = e.Item.Cells[17].Text.Trim(); //����
            string s_cardtail = e.Item.Cells[1].Text.Trim(); //��β��
            string s_banktype = e.Item.Cells[9].Text.Trim(); //��������
            switch (e.CommandName)
            {
                case "DETAIL": //��ʾ��ϸ
                    showEdit();
                    ShowDETAIL(iprov, icity, state, bankid, trueName, LastIP, BankName, Modify_Time, Memo, banktype, Compayname, AccCreate);
                    break;
                case "CHANGE": //�ⶳ
                    freezeCard(s_curtype,s_banktype, s_cardtail);
                    break;
            }
        }

        private void freezeCard(string curtype, string banktype, string cardtail) 
        {
            try 
            {
                Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
                qs.FreezeNonPrimaryCard(Session["QQID"].ToString(), 1, curtype, banktype, cardtail);
                WebUtils.ShowMessage(this.Page, "�ⶳ�ɹ���");
                BindInfo(1, 1);
            }
            catch (SoapException eSoap) //����soap���쳣
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "���÷������" + errStr);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, PublicRes.GetErrorMsg(eSys.Message.ToString()));
            }
        }

		private void clickEvent()
		{
			Session["QQID"] = this.TextBox1_QQID.Text.Trim(); 
			BindInfo(1,1);
		}

		private void BindInfo(int istr,int imax)
		{
			
			try
			{
				Session["uid"].ToString();
			}
			catch
			{
				WebUtils.ShowMessage(this.Page,"��ʱ�������µ�½��");
			}

			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            //string qqid = qs.Uid2QQ(this.TextBox1_QQID.Text.Trim());
			try
			{
                DataSet ds = qs.GetBatchUserBankAccount(Session["QQID"].ToString().Trim());
				if(ds == null || ds.Tables.Count<1 || ds.Tables[0].Rows.Count<1) 
				{
					WebUtils.ShowMessage(this.Page,"�û��������ʻ�������!");
					return;
				}	
				ds.Tables[0].Columns.Add("FstateName",typeof(String));
				ds.Tables[0].Columns.Add("FcurtypeName",typeof(String));
				ds.Tables[0].Columns.Add("Fprimary_flagName",typeof(String));
                ds.Tables[0].Columns.Add("Fbankid_str", typeof(String)); //yinhuang �������˺Ŵ���

				foreach(DataRow dr in ds.Tables[0].Rows)
				{
					dr.BeginEdit();
                    dr["Fbankid_str"] = classLibrary.setConfig.ConvertID(dr["Fbankid"].ToString(), 4, 5);
					string fstate=dr["Fstate"].ToString();
					if(fstate=="1")
					{
                        dr["FstateName"] = "����";
					}
					if(fstate=="2")
					{
						dr["FstateName"]="����";
					}
					if(fstate=="3")
					{
						dr["FstateName"]="����";
					}
					string fcurtype=dr["Fcurtype"].ToString();
					if(fcurtype=="1")
					{
						dr["FcurtypeName"]="�����";
					}
					else if(fcurtype=="2")
					{
						dr["FcurtypeName"]="����";
					}
					else
					{
						dr["FcurtypeName"]="����";
					}
					string fprimaryflag=dr["Fprimary_flag"].ToString();
					if(fprimaryflag=="1")
					{
						dr["Fprimary_flagName"]="����";
					}
					else
					{
						dr["Fprimary_flagName"]="������";
					}
					dr.EndEdit();
				
				 
				}
				this.DGData.DataSource=ds;
				this.DGData.DataBind();
			}
			catch(Exception eSys)
			{
				WebUtils.ShowMessage(this.Page,"��ȡ����ʧ�ܣ�" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
			}
			showList();
		}

		protected void btBack_Click(object sender, System.EventArgs e)
		{
			showList();
			//			Response.Redirect("./UserBankInfoQuery.aspx");
		}

        protected void lkb_acc_Click(object sender, System.EventArgs e)
        {
            if (Label1_State.Text.Trim() == "����")
            {

                Response.Write("<script language=javascript>window.parent.location='freezeBankAcc.aspx?id=true&uid=" + this.Label3_QQID.Text.Trim() + "'</script>");
            }
            else if (Label1_State.Text.Trim() == "����")
            {
                Response.Write("<script language=javascript>window.parent.location='freezeBankAcc.aspx?id=false&uid=" + this.Label3_QQID.Text.Trim() + "'</script>");
            }

        }

        protected void ShowDETAIL(string iprov, string icity, string state, string bankid, string trueName, string LastIP, 
            string BankName, string Modify_Time, string Memo, string banktype, string Compayname, string AccCreate)
        {
            try
            {
                Session["uid"].ToString();
            }
            catch
            {
                WebUtils.ShowMessage(this.Page, "��ʱ�������µ�½��");
            }

            try
            {
                bool isRight = TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("SensitiveRole", this);
                dProv = iprov;
                dCity = icity;
                this.Label1_State.Text = state;
                this.Label2_BankID.Text = classLibrary.setConfig.BankCardNoSubstring(bankid, isRight);              
                this.Label4_TrueName.Text = classLibrary.setConfig.ConvertName(trueName, isRight); ;
                this.Label6_LastIP.Text = LastIP;
                this.Label8_BankName.Text = BankName;
                this.Label11_Modify_Time.Text = Modify_Time;
                this.Label12_Memo.Text = Memo;
                this.Label13_BankType.Text = banktype;
                this.lbCompay.Text = Compayname;
                this.lbAccCreate.Text = AccCreate;
                
                if (Label1_State.Text.Trim() == "����")
                {
                    this.lkb_acc.Text = "����";  //����ʻ�Ϊ���������Խ��ж������
                }
                else if (Label1_State.Text.Trim() == "����")
                {
                    this.lkb_acc.Text = "�ⶳ";   //����ʻ�Ϊ���ᣬ����Խ��нⶳ����
                }
                this.Label3_QQID.Text = Session["QQID"].ToString().Trim();
            }
            catch
            {

                WebUtils.ShowMessage(this.Page, "���ݿ��޼�¼��");

                this.Label1_State.Text = "";
                this.Label2_BankID.Text = "";
                this.Label3_QQID.Text = "";
                this.Label4_TrueName.Text = "";

                this.Label6_LastIP.Text = "";
                this.Label8_BankName.Text = "";

                this.Label11_Modify_Time.Text = "";
                this.Label12_Memo.Text = "";
                this.Label13_BankType.Text = "";
            }
        }
	}
}
