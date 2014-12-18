namespace TENCENT.OSS.CFT.KF.KF_Web.Control
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using System.Collections;

	/// <summary>
	///		commonDataSet 的摘要说明。
	/// </summary>
	public partial class commonDataSet : System.Web.UI.UserControl
	{
		protected Wuqi.Webdiyer.AspNetPager netPager;

		private   DataSet ds;
		private Hashtable ht;
		private int istr = 0;
		protected System.Web.UI.WebControls.Button Button1; //开始索引页码
		private int ilen = 2;// 测试分页 Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["pageSize"]); //长度

		protected void Page_Load(object sender, System.EventArgs e)
		{
			// 在此处放置用户代码以初始化页面
			if (!Page.IsPostBack)
			BindData();
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
		
		private void BindData()
		{
			this.dgInfo.AutoGenerateColumns = false;
			this.dgInfo.DataSource          = ds.Tables[0].DefaultView;  //设置数据源

			foreach(string i in ht.Keys)
			{
				BoundColumn bc = new BoundColumn();
				bc.DataField   = i;
				bc.HeaderText  = ht[i].ToString(); 
				dgInfo.Columns.Add(bc);		
			}

			this.dgInfo.DataBind();
		}


		/// <summary>
		/// 设置数据源 DataSet
		/// </summary>
		public DataSet dataSetData
		{
			get
			{
				return ds;
			}
			set
			{
				ds = value;
			}
		}

		/// <summary>
		/// 设置需要显示的字段和标题名称
		/// 例：Hashtable ht = new Hashtable();
		///     ht.Add("uin","QQ号码");
		///     ht.Add("dttm","时间");
		///     commDs.ht    = ht;  //commDs是控件名称
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
