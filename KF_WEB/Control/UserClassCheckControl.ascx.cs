namespace TENCENT.OSS.CFT.KF.KF_Web.Control
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	/// <summary>
	///		UserClassCheckControl 的摘要说明。
	/// </summary>
	public partial class UserClassCheckControl : System.Web.UI.UserControl
	{
		public DataRow dr;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			// 在此处放置用户代码以初始化页面
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
			lblfid.Text = dr["flist_id"].ToString().Trim();
			tbComment.Enabled = false;
			tbFCheckInfo.Text = PublicRes.GetString(dr["Fmemo"]);			
			string imagestr = dr["Fpath"].ToString().Trim();
			string url = System.Configuration.ConfigurationManager.AppSettings["UserClassUrlPath"].Trim();
            if (imagestr.IndexOf("|") > 0)
            {//增加正反面图片
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
					labFQQid.Text = "读取数据有误" ;
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
				labFQQid.Text = "读取数据有误：" + PublicRes.GetErrorMsg(err.Message);
			}
		}

		private string GetCreType(string creid)
		{			if(creid == null || creid.Trim() == "")
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
