using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using CFT.CSOMS.BLL.UserAppealModule;

namespace TENCENT.OSS.CFT.KF.KF_Web.Control
{
	/// <summary>
	///		UserAppealCheckControl 的摘要说明。
	/// </summary>
	public partial class UserAppealCheckControl : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.Label lblTurnBig;
		protected System.Web.UI.WebControls.Label lblTurnShort;
		protected System.Web.UI.WebControls.Label lblTurnBack;

		public DataRow dr;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			//this.cb_2.Attributes.Add("onclick","st2('" + cb_2.ClientID + "','" + this.div_cbxl2.ClientID + "')");
			this.cb_1.Attributes.Add("onclick","st3(this," + this.rbtnReject.ClientID + ")");
			this.cb_2.Attributes.Add("onclick","st2(this," + this.div_cbxl2.ClientID +  "," + this.rbtnReject.ClientID + ")");
			this.cb_3.Attributes.Add("onclick","st2(this," + this.div_cbx_3.ClientID +  "," + this.rbtnReject.ClientID + ")");
			this.cb_4.Attributes.Add("onclick","st3(this," + this.rbtnReject.ClientID + ")");
			this.cb_5.Attributes.Add("onclick","st3(this," + this.rbtnReject.ClientID + ")");
			this.cb_0.Attributes.Add("onclick","st3(this," + this.rbtnReject.ClientID + ")");

			this.hbtn_fPic.Attributes.Add("onclick","st4(" + this.Image1.ClientID + "," + this.Image2.ClientID + ")");
			this.hbtn_sPic.Attributes.Add("onclick","st4(" + this.Image2.ClientID + "," + this.Image1.ClientID + ")");
			//tr_picSelect.Attributes.Add("display","block");

			tr_picSelect.Visible = false;
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
		///		设计器支持所需的方法 - 不要使用代码编辑器
		///		修改此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{

		}
		#endregion

		public void BindData()
		{
            string ftype = dr["FType"].ToString();
            if (ftype == "1" || ftype == "5" || ftype == "6" || ftype == "99")//三种类型在原来基础上进行分库分表
            {
                lbldb.Text = dr["DBName"].ToString();
                lbltb.Text = dr["tableName"].ToString();
            }
            else
            {
                lbldb.Text = "";
                lbltb.Text = "";
            }

            //lxl 20131111 为了后面传参的必要
            lblftype.Text = ftype;
            lbldb.Visible = false;
            lbltb.Visible = false;
            lblftype.Visible = false;

			//读取内容，读出QQ号后，就要读取后台信息。
			lblfid.Text = dr["Fid"].ToString();
				
			//labFuin.Text = dr["Fuin"].ToString();  //不要了,重复展示

			labFTypeName.Text = dr["FTypeName"].ToString();
			cre_id.Text = dr["cre_id"].ToString();
			new_cre_id.Text = dr["new_cre_id"].ToString();

			cre_type.Text = GetCreType(dr["cre_type"].ToString());

			email.Text = dr["Femail"].ToString();

			labFstatename.Text = dr["FStateName"].ToString();

			if(dr["clear_pps"].ToString() == "1" && dr["FType"].ToString() == "1")
			{
				clear_pps.Text = "清除";
			}
			else if (dr["FType"].ToString() == "1" && dr["clear_pps"].ToString() != "1")
			{
				clear_pps.Text = "不清除";
			}
			else
			{
				clear_pps.Text = "";
			}

			tbReason.Text = dr["reason"].ToString();
			tbComment.Text = dr["Fcomment"].ToString();

			old_name.Text = dr["old_name"].ToString();
			new_name.Text = dr["new_name"].ToString();
			//tbFCheckInfo.Text = PublicRes.GetString(dr["FCheckInfo"]);

			if(dr["FType"].ToString() == "3")
			{
				old_name.Text = dr["old_company"].ToString();
				new_name.Text = dr["new_company"].ToString();
			}
		
			labIsAnswer.Text = dr["labIsAnswer"].ToString();
			lblBindMobileUser.Text = dr["mobile_no"].ToString();
			lblBindMailUser.Text = dr["Femail"].ToString();
			lblstandard_score.Text = dr["standard_score"].ToString();
			lblscore.Text = dr["score"].ToString();
			lbldetail_score.Text = dr["detail_score"].ToString();
			lblrisk_result.Text = dr["risk_result"].ToString();
			string imagestr = dr["cre_image"].ToString().Trim();
			string url = System.Configuration.ConfigurationManager.AppSettings["AppealUrlPath"].Trim();

			if(!url.EndsWith("/"))
				url += "/";

            //之前只显示一张图片
            //if(imagestr != "")
            //{
            //    Image1.ImageUrl =  url + imagestr;
            //    imgOther.ImageUrl = url + imagestr;
            //}
            //else
            //{
            //    Image1.ImageUrl = "";
            //    Image1.AlternateText = "图片已丢失";
	
            //    imgOther.ImageUrl = "";
            //    imgOther.AlternateText = "图片已丢失";
            //}
            string urlCGI = System.Configuration.ConfigurationManager.AppSettings["GetAppealImageCgi"].Trim();//拉图片cgi
            if (imagestr != "")
            {
                //与列表中进入详情时的实现方式一致
                if (dr["FType"].ToString() == "0" || dr["FType"].ToString() == "9" || dr["FType"].ToString() == "10")
                {
                    if (imagestr.IndexOf("|") > 0)
                    {
                        string[] imgUrls = imagestr.Split('|');
                        Image1.ImageUrl = url + imgUrls[0];
                        Image2.ImageUrl = url + imgUrls[1];
                    }
                    else
                    {
                        Image1.ImageUrl = url + imagestr;
                    }
                }
                else if (dr["FType"].ToString() == "1" || dr["FType"].ToString() == "5" || dr["FType"].ToString() == "6")//20131111 lxl 分库分表三种类型
                {
                    if (imagestr.IndexOf("|") > 0)
                    {
                        string[] imgUrls = imagestr.Split('|');
                        if (db == null || db == "" || tb == null || tb == "")
                        {
                            Image1.ImageUrl = url + imgUrls[0];
                            Image2.ImageUrl = url + imgUrls[1];
                        }
                        else
                        {
                            this.Image1.ImageUrl = urlCGI + imgUrls[0];
                            this.Image2.ImageUrl = urlCGI + imgUrls[1];
                        }
                    }
                    else
                    {
                        if (db == null || db == "" || tb == null || tb == "")
                            Image1.ImageUrl = url + imagestr;
                        else
                            this.Image1.ImageUrl = urlCGI + imagestr;
                    }
                }
                else
                {
                    Image1.ImageUrl = url + imagestr;
                }
            }
            else
            {
                Image1.ImageUrl = "";
                Image1.AlternateText = "图片已丢失";
                Image2.ImageUrl = "";
                Image2.AlternateText = "图片已丢失";
            }

			try
			{
				Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
				DataSet dsuser =  qs.GetAppealUserInfo(dr["Fuin"].ToString());
				if(dsuser == null || dsuser.Tables.Count == 0 || dsuser.Tables[0].Rows.Count != 1)
				{
					labFQQid.Text = "读取数据有误" ;
				}
				else
				{
					DataRow druser = dsuser.Tables[0].Rows[0];

					labFQQid.Text = druser["Fqqid"].ToString();
					labFbalance.Text = classLibrary.setConfig.FenToYuan(druser["FBalance"].ToString());
					labFCon.Text = classLibrary.setConfig.FenToYuan(druser["Fcon"].ToString());

					labFcre_type.Text = GetCreType(druser["Fcre_type"].ToString());

					labFcreid.Text = PublicRes.GetString(druser["Fcreid"]);
					labFEmail.Text = PublicRes.GetString(druser["FEmail"]);
					labFtruename.Text = PublicRes.GetString(druser["Ftruename"]);

					labFBankAcc.Text = PublicRes.GetString(druser["Fbankid"]);

					if(dr["FType"].ToString() == "3")
					{
						labFtruename.Text = PublicRes.GetString(druser["Fcompany_name"]);
					}

                    qs.Finance_HeaderValue = classLibrary.setConfig.setFH(this);
                    //2.查询实名认证
                    DataSet dsA = qs.GetUserAuthenState(druser["Fqqid"].ToString(), "", 0);
                    //if (dsA == null || dsA.Tables.Count < 1 || dsA.Tables[0].Rows.Count != 1)
                    //{
                    //    lbauthenState.Text = "否";//是否实名认证
                    //}
                    //else
                    //{
                    //    DataRow row = dsA.Tables[0].Rows[0];
                    //    if (row["queryType"].ToString() == "2")
                    //    {
                    //        lbauthenState.Text = "是";
                    //    }
                    //    else
                    //    {
                    //        lbauthenState.Text = "否";
                    //    }
                    //}

                    //2.查询实名认证
                    bool stateMsg = false;
                    DataSet authenState = new UserAppealService().GetUserAuthenState(druser["Fqqid"].ToString(), "", 0, out stateMsg);
                    if (stateMsg)
                    {
                        lbauthenState.Text = "是";
                    }
                    else
                    {
                        lbauthenState.Text = "否";
                    }             
				}
			}
			catch(Exception err)
			{
				labFQQid.Text = "读取数据有误：" + PublicRes.GetErrorMsg(err.Message);
			}
		}

		private string GetCreType(string creid)
		{
			if(creid == null || creid.Trim() == "")
				return "未指定类型";

			int icreid = 0;
			try
			{
				icreid = Int32.Parse(creid);
			}
			catch
			{
				return "不正确类型" + creid;
			}

			if(icreid >=1 && icreid <= 11)
			{
				if(icreid == 1)
				{
					return "身份证";
				}
				else if(icreid == 2)
				{
					return "护照";
				}
				else if(icreid == 3)
				{
					return "军官证";
				}
				else if(icreid == 4)
				{
					return "士兵证";
				}
				else if(icreid == 5)
				{
					return "回乡证";
				}
				else if(icreid == 6)
				{
					return "临时身份证";
				}
				else if(icreid == 7)
				{
					return "户口簿";
				}
				else if(icreid == 8)
				{
					return "警官证";
				}
				else if(icreid == 9)
				{
					return "台胞证";
				}
				else if(icreid == 10)
				{
					return "营业执照";
				}
				else if(icreid == 11)
				{
					return "其它证件";
				}
				else
				{
					return "不正确类型" + creid;
				}
			}
			else
			{
				return "不正确类型" + creid;
			}

		}

		public void Clean()
		{
			lblfid.Text = "";

			this.cb_0.Checked = false;
			this.cb_1.Checked = false;
			this.cb_2.Checked = false;
			this.cb_3.Checked = false;
			this.cb_4.Checked = false;

			this.rbtnOK.Checked = false;
			this.rbtnReject.Checked = false;
			this.rbtnDelete.Checked = false;
			this.rbtnSub.Checked = false;

			foreach(ListItem li in cbxl_2.Items)
			{
				li.Selected = false;
			}
			foreach(ListItem li in this.rbtnAppeal.Items)
			{
				li.Selected = false;
			}

			this.cbx_detail_cbx3.Checked = false;
		}

		public DataRow _dr
		{
			get
			{
				return dr;
			}
			set
			{
				dr = value;
			}
		}

		public string SubmitType
		{
			/*
			get
			{
				return rbtnAppeal.SelectedValue;
			}
			*/

			get
			{
				if(this.rbtnOK.Checked)
					return "1";
				else if(this.rbtnReject.Checked)
					return "2";
				else if(this.rbtnDelete.Checked)
					return "3";
				else if(this.rbtnSub.Checked)
					return "4";

				return "";
			}
		}
		public string fid
		{
			get
			{
				return lblfid.Text;
			}
		}

        public string db
        {
            get
            {
                return lbldb.Text;
            }
        }

        public string tb
        {
            get
            {
                return lbltb.Text;
            }
        }

        public string ftype
        {
            get
            {
                return lblftype.Text;
            }
        }

		// 获取拒绝原因
		public bool GetRejectReason(out string reason,out string otherReason)
		{
			reason = "";otherReason = "";
			string tips_3 = "特殊申诉找回地址";
			//string tips_3 = @"请您重新提交申述表时，上传账户资金来源截图。请参考“<a href='http://help.tenpay.com/cgi-bin/helpcenter/help_center.cgi?id=2232&type=0'>特殊申诉找回</a>”指引";
			//string tips_3 = @"请您重新提交申述表时，上传账户资金来源截图。请参考“特殊申诉找回”指引:http://help.tenpay.com/cgi-bin/helpcenter/help_center.cgi?id=2232&type=0";
			if(this.cb_0.Checked && this.tbFCheckInfo.Text.Trim() == "")
			{
				throw new Exception("请输入其它原因！");
			}
			if(this.cb_1.Checked)
			{
				reason += this.cb_1.Text + "&";
			}
			if(this.cb_5.Checked)
			{
				reason += this.cb_5.Text + "&";
			}
			if(this.cb_2.Checked)
			{
				reason += this.cb_2.Text+ "&";
				for(int i=0;i<this.cbxl_2.Items.Count;i++)
				{
					if(this.cbxl_2.Items[i].Selected)
						reason += this.cbxl_2.Items[i].Value + "&";
				}
			}
			if(this.cb_3.Checked)
			{
				reason += this.cb_3.Text + "&";
				if(this.cbx_detail_cbx3.Checked)
					reason += tips_3 + "&";
			}
			if(this.cb_4.Checked)
			{
				reason += this.cb_4.Text + "&";
			}

			if(this.cb_0.Checked)
			{
				otherReason = classLibrary.setConfig.replaceMStr(tbFCheckInfo.Text.Trim());
			}

			if(reason.Trim() == "" && otherReason.Trim() == "")
				throw new Exception("请选择拒绝原因！");

			return true;
		}


		private void ShowMsg(string msg)
		{
			Response.Write("<script language=javascript>alert('" + msg + "')</script>");
		}
	
		public string Comment
		{
			get
			{
				return tbComment.Text.Trim();
			}
		}

	}
}
