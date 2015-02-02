namespace TENCENT.OSS.CFT.KF.KF_Web.Control
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	/// <summary>
	///		UserClassCheckControl ��ժҪ˵����
	/// </summary>
	public partial class UserClassCheckControl : System.Web.UI.UserControl
	{
		public DataRow dr;

		protected void Page_Load(object sender, System.EventArgs e)
		{
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
		///		�����֧������ķ��� - ��Ҫʹ�ô���༭��
		///		�޸Ĵ˷��������ݡ�
		/// </summary>
		private void InitializeComponent()
		{

		}
		#endregion

		public void BindData()
		{
			lblfid.Text = dr["flist_id"].ToString().Trim();
			tbComment.Enabled = false;
			tbFCheckInfo.Text = PublicRes.GetString(dr["Fmemo"]);			
			string imagestr = dr["Fpath"].ToString().Trim();
			string url = System.Configuration.ConfigurationManager.AppSettings["UserClassUrlPath"].Trim();
            if (imagestr.IndexOf("|") > 0)
            {//����������ͼƬ
                string[] strtmps = imagestr.Split('|');
                Image1.ImageUrl = url + strtmps[0];
                Image2.ImageUrl = url + strtmps[1];
            }
            else {
                Image1.ImageUrl = url + imagestr;
            }
			
			try
			{
				Query_Service.Query_Service qs = new Query_Service.Query_Service();
				DataSet dsuser =  qs.GetUserClassInfo(dr["Fqqid"].ToString());
				if(dsuser == null || dsuser.Tables.Count == 0 || dsuser.Tables[0].Rows.Count != 1)
				{
					labFQQid.Text = "��ȡ��������" ;
				}
				else
				{
					DataRow druser = dsuser.Tables[0].Rows[0];
					labFQQid.Text = druser["Fqqid"].ToString();
					labFcre_type.Text = GetCreType(druser["Fcre_type"].ToString());
					labFcreid.Text = PublicRes.GetString(druser["Fcreid"]);
					labFtruename.Text = PublicRes.GetString(druser["Ftruename"]);
				}
			}
			catch(Exception err)
			{
				labFQQid.Text = "��ȡ��������" + PublicRes.GetErrorMsg(err.Message);
			}
		}

		private string GetCreType(string creid)
		{			if(creid == null || creid.Trim() == "")
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

			foreach(ListItem li in RejectReason.Items)
			{
				li.Selected = false;
			}
			foreach(ListItem li in rbtnAppeal.Items)
			{
				li.Selected = false;
			}
		}
		public DataRow _dr
		{
			get
			{
				return dr;
			}			set			{				dr = value;			}		}		public string SubmitType
		{
			get
			{
				return rbtnAppeal.SelectedValue;
			}		}		public string flist_id
		{
			get
			{
				return lblfid.Text;
			}		}		public CheckBoxList cblRejectReason
		{
			get
			{
				return RejectReason;
			}		}		public string tbOtherReason		{			get			{				return tbFCheckInfo.Text.Trim();			}		}		public string Comment		{			get			{				return tbComment.Text.Trim();			}		}
	}
}
