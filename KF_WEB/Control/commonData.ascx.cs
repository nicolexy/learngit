namespace TENCENT.OSS.CFT.KF.KF_Web.Control
{
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
	using TENCENT.OSS.CFT.KF.KF_Web.Control;
	using Tencent.DotNet.Common.UI;
	using Tencent.DotNet.OSS.Web.UI;
	using TENCENT.OSS.CFT.KF.Common;
	using System.Text;

	/// <summary>
	///		commonData ��ժҪ˵����
	/// </summary>
	public partial class commonData : System.Web.UI.UserControl
	{










		public    string  Msg;
		int       istr=0;
		int       pageSize = 20;
		public    DataSet ds;   //����ԴDataSet
		Hashtable ht;

		protected string   queryType;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��
			// if (Session["uid"] == null || !AllUserRight.ValidRight(Session["szkey"].ToString(),Int32.Parse(Session["OperID"].ToString()),PublicRes.GROUPID, "HistoryModify")) Response.Redirect("../login.aspx?wh=1");
			if(Session["uid"] == null || !classLibrary.ClassLib.ValidateRight("HistoryModify",this)) Response.Redirect("../login.aspx?wh=1");
			pageSize = Int32.Parse(ddlPageSize.SelectedValue);
			
			string strbe = this.TextBoxBeginDate.ClientID;
			string stred = this.TextBoxEndDate.ClientID;

			ButtonBeginDate.Attributes.Add("onclick", "openModeBegin(" + strbe + ")"); 
			ButtonEndDate.Attributes.Add("onclick", "openModeEnd(" + stred + ")"); 

			ClientSubmitBind(this.txbCustom,this.btQuery);

			if (!Page.IsPostBack)
			{
				this.TextBoxEndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

				if (!bindaData())
				{
					return;
				}

				//���Ҫ��ȷ��ѯ���ֶκ�ֵ
//				int i = ht.Count;
//				for (i=0;i<ht.Count;i++)
//				{
//					this.ddlCondition.Items.Add(new ListItem(ht[i].Keys.ToString(),"��" + ht.Values[i].ToString() + "��ѯ"));	
//				}
				
				IEnumerator im = ht.GetEnumerator();
				im.MoveNext();
				Response.Write(im.Current.GetHashCode());

				foreach(string s in ht.Keys)
				{
					this.ddlCondition.Items.Add(new ListItem("��" + ht[s].ToString() + "��ѯ",s));
				}

				this.ddlCondition.DataBind();
			}
	
			//Page.DataBind();
		}

		private bool bindaData()
		{
			//�����ѯ���
			DateTime bgDate = DateTime.Now;
			DateTime edDate = DateTime.Now;
			string whereStr = "";   //�����ж����
  
			try
			{
				bgDate = DateTime.Parse(this.TextBoxBeginDate.Text.Trim());
				edDate = DateTime.Parse(this.TextBoxEndDate.Text.Trim());
			}
			catch
			{
				Msg = "���ڸ�ʽ����������飡";
				return false;
			}

			if (this.txbCustom.Text != "" && this.txbCustom.Text.Trim() != "")
			{
				whereStr = " and " + this.ddlCondition.SelectedValue + " = '" + classLibrary.setConfig.replaceMStr(this.txbCustom.Text.Trim()) + "' " ;  //�ض���ѯ
			}
			

			Query_Service.Query_Service qs = new Query_Service.Query_Service();

			if (ViewState["newIndex"] != null)
				istr = Int32.Parse(ViewState["newIndex"].ToString());
			else
				istr = 0;

			if (!qs.getQueryData(istr*pageSize,pageSize,bgDate,edDate,whereStr,queryType,out ds,out Msg)) return false;

			if (ds == null || ds.Tables.Count == 0 ||ds.Tables[0].Rows.Count == 0) 
			{
				AspNetPager1.Visible = false;
				this.Page.DataBind();

				Msg = "û����ѡ����Χ�ڵ����ݡ�";
				WebUtils.ShowMessage(this.Page,Msg);
				return false;
			}

			//����Ҫ���������Դ
			this.dgInfo.AutoGenerateColumns = false;  

			foreach(string i in ht.Keys)
			{
				BoundColumn bc = new BoundColumn();
				bc.DataField   = i;
				bc.HeaderText  = ht[i].ToString(); 
				dgInfo.Columns.Add(bc);		
			}

			AspNetPager1.PageSize    = pageSize;
			AspNetPager1.RecordCount = Int32.Parse(ds.Tables[0].Rows[0]["icount"].ToString());
			
			AspNetPager1.CustomInfoText ="��¼������<font color=\"blue\"><b>"+AspNetPager1.RecordCount.ToString()+"</b></font>";
			AspNetPager1.CustomInfoText+=" ��ҳ����<font color=\"blue\"><b>" +AspNetPager1.PageCount.ToString()+"</b></font>";
			AspNetPager1.CustomInfoText+=" ��ǰҳ��<font color=\"red\"><b>"  +AspNetPager1.CurrentPageIndex.ToString()+"</b></font>";

			
			this.dgInfo.DataSource = ds.Tables[0].DefaultView;
			this.dgInfo.DataBind();

			return true;
		}

		/// <summary>
		/// Ϊָ��������Ĭ���ύ��ť���������ڸ��������ʱ�����»س����������ύ��ť��Click�¼�
		/// </summary>
		/// <param name="txtBox">Ҫ�󶨵������</param>
		/// <param name="btnBindSubmit">Ҫ�󶨵��ύ��ť��Ϊnullʱ��ʹ�س�����Ч</param>
		public void ClientSubmitBind(System.Web.UI.WebControls.TextBox txtBox,System.Web.UI.WebControls.Button btnBindSubmit)
		{
			string script;
			if (btnBindSubmit!=null)
				script = "if(event.keyCode == 13){document.getElementById('" + btnBindSubmit.ClientID + "').click();event.returnValue=false;}"; 
			else
				script = "if(event.keyCode == 13){event.returnValue=false;}"; 
			txtBox.Attributes["onkeydown"] = script;
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
			this.AspNetPager1.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(this.AspNetPager1_PageChanged);

		}
		#endregion

		protected void btQuery_Click(object sender, System.EventArgs e)
		{
			ViewState["newIndex"] = null;  //������µ��һ�β�ѯ������ղ�ѯ�ķ�ҳ�������޷���ѯ�����ݣ����絥�ʣ�
			AspNetPager1.Visible = true;
			this.AspNetPager1.CurrentPageIndex = 1;

			bindaData();
		}

		private void AspNetPager1_PageChanged(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
		{
			istr = e.NewPageIndex;
			AspNetPager1.CurrentPageIndex = istr;

			ViewState["newIndex"] = e.NewPageIndex -1;

			bindaData();
		}

		protected void ddlCondition_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			//����ѡ�������˵��仯�����������ǿգ���ʾ����Ҫ��ȷ��ѯ����յ�ǰ��ҳ�� ViewState["newIndex"]	
			if (txbCustom.Text != "" && txbCustom.Text.Trim() !="")
			{
				AspNetPager1.Visible = true;
				ViewState["newIndex"] = null;
  
				bindaData();
			}
		}

		public string   QueryType
		{
			get
			{
				return queryType;
			}
			set
			{
				 queryType = value;
			}
		}

		/// <summary>
		/// ������Ҫ��ʾ���ֶκͱ�������
		/// ����Hashtable ht = new Hashtable();
		///     ht.Add("uin","QQ����");
		///     ht.Add("dttm","ʱ��");
		///     commDs.ht    = ht;  //commDs�ǿؼ�����
		/// </summary>
		public Hashtable htData
		{
			get
			{
				return ht;
			}
			set
			{
				ht = value; 
			}
		}
	}
}
