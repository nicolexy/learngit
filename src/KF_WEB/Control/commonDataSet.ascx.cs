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
	///		commonDataSet ��ժҪ˵����
	/// </summary>
	public partial class commonDataSet : System.Web.UI.UserControl
	{
		protected Wuqi.Webdiyer.AspNetPager netPager;

		private   DataSet ds;
		private Hashtable ht;
		private int istr = 0;
		protected System.Web.UI.WebControls.Button Button1; //��ʼ����ҳ��
		private int ilen = 2;// ���Է�ҳ Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["pageSize"]); //����

		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��
			if (!Page.IsPostBack)
			BindData();
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
		
		private void BindData()
		{
			this.dgInfo.AutoGenerateColumns = false;
			this.dgInfo.DataSource          = ds.Tables[0].DefaultView;  //��������Դ

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
		/// ��������Դ DataSet
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
