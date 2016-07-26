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
	/// UserAccountQuery1 的摘要说明。
	/// </summary>
	public partial class UserAccountQuery1 : PageBase
	{
        public string dProv, dCity;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// 在此处放置用户代码以初始化页面
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


		#region Web 窗体设计器生成的代码
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: 该调用是 ASP.NET Web 窗体设计器所必需的。
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// 设计器支持所需的方法 - 不要使用代码编辑器修改
		/// 此方法的内容。
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
            string s_state = e.Item.Cells[2].Text.Trim();//冻结状态
            string s_optype = e.Item.Cells[4].Text.Trim();//非主卡
            if (obj != null)
            {
                LinkButton lb = (LinkButton)obj;
                if (s_state != "" && s_state == "冻结" && s_optype == "非主卡")
                {
                    lb.Visible = true;
                }
                lb.Attributes["onClick"] = "return confirm('确定要执行“解冻”操作吗？');";
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

            string s_curtype = e.Item.Cells[17].Text.Trim(); //币种
            string s_cardtail = e.Item.Cells[1].Text.Trim(); //卡尾号
            string s_banktype = e.Item.Cells[9].Text.Trim(); //银行类型
            switch (e.CommandName)
            {
                case "DETAIL": //显示明细
                    showEdit();
                    ShowDETAIL(iprov, icity, state, bankid, trueName, LastIP, BankName, Modify_Time, Memo, banktype, Compayname, AccCreate);
                    break;
                case "CHANGE": //解冻
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
                WebUtils.ShowMessage(this.Page, "解冻成功！");
                BindInfo(1, 1);
            }
            catch (SoapException eSoap) //捕获soap类异常
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "调用服务出错：" + errStr);
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
				WebUtils.ShowMessage(this.Page,"超时，请重新登陆。");
			}

			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            //string qqid = qs.Uid2QQ(this.TextBox1_QQID.Text.Trim());
			try
			{
                DataSet ds = qs.GetBatchUserBankAccount(Session["QQID"].ToString().Trim());
				if(ds == null || ds.Tables.Count<1 || ds.Tables[0].Rows.Count<1) 
				{
					WebUtils.ShowMessage(this.Page,"用户绑定银行帐户不存在!");
					return;
				}	
				ds.Tables[0].Columns.Add("FstateName",typeof(String));
				ds.Tables[0].Columns.Add("FcurtypeName",typeof(String));
				ds.Tables[0].Columns.Add("Fprimary_flagName",typeof(String));
                ds.Tables[0].Columns.Add("Fbankid_str", typeof(String)); //yinhuang 对银行账号处理

				foreach(DataRow dr in ds.Tables[0].Rows)
				{
					dr.BeginEdit();
                    dr["Fbankid_str"] = classLibrary.setConfig.ConvertID(dr["Fbankid"].ToString(), 4, 5);
					string fstate=dr["Fstate"].ToString();
					if(fstate=="1")
					{
                        dr["FstateName"] = "正常";
					}
					if(fstate=="2")
					{
						dr["FstateName"]="冻结";
					}
					if(fstate=="3")
					{
						dr["FstateName"]="作废";
					}
					string fcurtype=dr["Fcurtype"].ToString();
					if(fcurtype=="1")
					{
						dr["FcurtypeName"]="人民币";
					}
					else if(fcurtype=="2")
					{
						dr["FcurtypeName"]="基金";
					}
					else
					{
						dr["FcurtypeName"]="其它";
					}
					string fprimaryflag=dr["Fprimary_flag"].ToString();
					if(fprimaryflag=="1")
					{
						dr["Fprimary_flagName"]="主卡";
					}
					else
					{
						dr["Fprimary_flagName"]="非主卡";
					}
					dr.EndEdit();
				
				 
				}
				this.DGData.DataSource=ds;
				this.DGData.DataBind();
			}
			catch(Exception eSys)
			{
				WebUtils.ShowMessage(this.Page,"读取数据失败！" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
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
            if (Label1_State.Text.Trim() == "正常")
            {

                Response.Write("<script language=javascript>window.parent.location='freezeBankAcc.aspx?id=true&uid=" + this.Label3_QQID.Text.Trim() + "'</script>");
            }
            else if (Label1_State.Text.Trim() == "冻结")
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
                WebUtils.ShowMessage(this.Page, "超时，请重新登陆。");
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
                
                if (Label1_State.Text.Trim() == "正常")
                {
                    this.lkb_acc.Text = "冻结";  //如果帐户为正常，可以进行冻结操作
                }
                else if (Label1_State.Text.Trim() == "冻结")
                {
                    this.lkb_acc.Text = "解冻";   //如果帐户为冻结，则可以进行解冻操作
                }
                this.Label3_QQID.Text = Session["QQID"].ToString().Trim();
            }
            catch
            {

                WebUtils.ShowMessage(this.Page, "数据库无记录！");

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
