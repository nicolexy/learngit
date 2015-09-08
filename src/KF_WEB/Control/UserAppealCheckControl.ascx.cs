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
	///		UserAppealCheckControl ��ժҪ˵����
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
		///		�����֧������ķ��� - ��Ҫʹ�ô���༭��
		///		�޸Ĵ˷��������ݡ�
		/// </summary>
		private void InitializeComponent()
		{

		}
		#endregion

		public void BindData()
		{
            string ftype = dr["FType"].ToString();
            if (ftype == "1" || ftype == "5" || ftype == "6" || ftype == "99")//����������ԭ�������Ͻ��зֿ�ֱ�
            {
                lbldb.Text = dr["DBName"].ToString();
                lbltb.Text = dr["tableName"].ToString();
            }
            else
            {
                lbldb.Text = "";
                lbltb.Text = "";
            }

            //lxl 20131111 Ϊ�˺��洫�εı�Ҫ
            lblftype.Text = ftype;
            lbldb.Visible = false;
            lbltb.Visible = false;
            lblftype.Visible = false;

			//��ȡ���ݣ�����QQ�ź󣬾�Ҫ��ȡ��̨��Ϣ��
			lblfid.Text = dr["Fid"].ToString();
				
			//labFuin.Text = dr["Fuin"].ToString();  //��Ҫ��,�ظ�չʾ

			labFTypeName.Text = dr["FTypeName"].ToString();
			cre_id.Text = dr["cre_id"].ToString();
			new_cre_id.Text = dr["new_cre_id"].ToString();

			cre_type.Text = GetCreType(dr["cre_type"].ToString());

			email.Text = dr["Femail"].ToString();

			labFstatename.Text = dr["FStateName"].ToString();

			if(dr["clear_pps"].ToString() == "1" && dr["FType"].ToString() == "1")
			{
				clear_pps.Text = "���";
			}
			else if (dr["FType"].ToString() == "1" && dr["clear_pps"].ToString() != "1")
			{
				clear_pps.Text = "�����";
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

            //֮ǰֻ��ʾһ��ͼƬ
            //if(imagestr != "")
            //{
            //    Image1.ImageUrl =  url + imagestr;
            //    imgOther.ImageUrl = url + imagestr;
            //}
            //else
            //{
            //    Image1.ImageUrl = "";
            //    Image1.AlternateText = "ͼƬ�Ѷ�ʧ";
	
            //    imgOther.ImageUrl = "";
            //    imgOther.AlternateText = "ͼƬ�Ѷ�ʧ";
            //}
            string urlCGI = System.Configuration.ConfigurationManager.AppSettings["GetAppealImageCgi"].Trim();//��ͼƬcgi
            if (imagestr != "")
            {
                //���б��н�������ʱ��ʵ�ַ�ʽһ��
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
                else if (dr["FType"].ToString() == "1" || dr["FType"].ToString() == "5" || dr["FType"].ToString() == "6")//20131111 lxl �ֿ�ֱ���������
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
                Image1.AlternateText = "ͼƬ�Ѷ�ʧ";
                Image2.ImageUrl = "";
                Image2.AlternateText = "ͼƬ�Ѷ�ʧ";
            }

			try
			{
				Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
				DataSet dsuser =  qs.GetAppealUserInfo(dr["Fuin"].ToString());
				if(dsuser == null || dsuser.Tables.Count == 0 || dsuser.Tables[0].Rows.Count != 1)
				{
					labFQQid.Text = "��ȡ��������" ;
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
                    //2.��ѯʵ����֤
                    DataSet dsA = qs.GetUserAuthenState(druser["Fqqid"].ToString(), "", 0);
                    //if (dsA == null || dsA.Tables.Count < 1 || dsA.Tables[0].Rows.Count != 1)
                    //{
                    //    lbauthenState.Text = "��";//�Ƿ�ʵ����֤
                    //}
                    //else
                    //{
                    //    DataRow row = dsA.Tables[0].Rows[0];
                    //    if (row["queryType"].ToString() == "2")
                    //    {
                    //        lbauthenState.Text = "��";
                    //    }
                    //    else
                    //    {
                    //        lbauthenState.Text = "��";
                    //    }
                    //}

                    //2.��ѯʵ����֤
                    bool stateMsg = false;
                    DataSet authenState = new UserAppealService().GetUserAuthenState(druser["Fqqid"].ToString(), "", 0, out stateMsg);
                    if (stateMsg)
                    {
                        lbauthenState.Text = "��";
                    }
                    else
                    {
                        lbauthenState.Text = "��";
                    }             
				}
			}
			catch(Exception err)
			{
				labFQQid.Text = "��ȡ��������" + PublicRes.GetErrorMsg(err.Message);
			}
		}

		private string GetCreType(string creid)
		{
			if(creid == null || creid.Trim() == "")
				return "δָ������";

			int icreid = 0;
			try
			{
				icreid = Int32.Parse(creid);
			}
			catch
			{
				return "����ȷ����" + creid;
			}

			if(icreid >=1 && icreid <= 11)
			{
				if(icreid == 1)
				{
					return "���֤";
				}
				else if(icreid == 2)
				{
					return "����";
				}
				else if(icreid == 3)
				{
					return "����֤";
				}
				else if(icreid == 4)
				{
					return "ʿ��֤";
				}
				else if(icreid == 5)
				{
					return "����֤";
				}
				else if(icreid == 6)
				{
					return "��ʱ���֤";
				}
				else if(icreid == 7)
				{
					return "���ڲ�";
				}
				else if(icreid == 8)
				{
					return "����֤";
				}
				else if(icreid == 9)
				{
					return "̨��֤";
				}
				else if(icreid == 10)
				{
					return "Ӫҵִ��";
				}
				else if(icreid == 11)
				{
					return "����֤��";
				}
				else
				{
					return "����ȷ����" + creid;
				}
			}
			else
			{
				return "����ȷ����" + creid;
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

		// ��ȡ�ܾ�ԭ��
		public bool GetRejectReason(out string reason,out string otherReason)
		{
			reason = "";otherReason = "";
			string tips_3 = "���������һص�ַ";
			//string tips_3 = @"���������ύ������ʱ���ϴ��˻��ʽ���Դ��ͼ����ο���<a href='http://help.tenpay.com/cgi-bin/helpcenter/help_center.cgi?id=2232&type=0'>���������һ�</a>��ָ��";
			//string tips_3 = @"���������ύ������ʱ���ϴ��˻��ʽ���Դ��ͼ����ο������������һء�ָ��:http://help.tenpay.com/cgi-bin/helpcenter/help_center.cgi?id=2232&type=0";
			if(this.cb_0.Checked && this.tbFCheckInfo.Text.Trim() == "")
			{
				throw new Exception("����������ԭ��");
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
				throw new Exception("��ѡ��ܾ�ԭ��");

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
