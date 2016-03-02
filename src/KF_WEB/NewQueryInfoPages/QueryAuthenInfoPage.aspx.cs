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
using TENCENT.OSS.CFT.KF.Common;
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;

using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using CFT.CSOMS.BLL.UserAppealModule;


namespace TENCENT.OSS.CFT.KF.KF_Web.NewQueryInfoPages
{
	/// <summary>
	/// QueryAuthenInfoPage ��ժҪ˵����
	/// </summary>
	public partial class QueryAuthenInfoPage : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.Button btn_submit_acc;
		protected System.Web.UI.WebControls.TextBox TextBox1;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
				if(Session["OperID"] != null)
					this.lb_operatorID.Text = Session["OperID"].ToString();
			}

            if (!classLibrary.ClassLib.ValidateRight("InfoCenter", this)) Response.Redirect("../login.aspx?wh=1");
			
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

		}
		#endregion

		
		protected void btn_query_Click(object sender, System.EventArgs e)
		{
			string acc = this.tbx_acc.Text.ToString();
			string bankID = this.tbx_bacc.Text.ToString();
			int bankType = 0;

            if (acc == "" && bankID == "")
			{
				WebUtils.ShowMessage(this,"�������ѯ����");
				return;
			}
			
			try
			{
				this.Clear();
                this.lb_queryAcc.Text = acc;

				string opLog = SensitivePowerOperaLib.MakeLog("get","","",acc,bankID.ToString(),bankType.ToString());

                SensitivePowerOperaLib.WriteOperationRecord("InfoCenter", opLog, this);

				if(bankID != null && bankID.Trim() != "")
					bankType = classLibrary.getData.GetBankCodeFromBankName(this.ddl_bankType.SelectedValue);
								
                bool stateMsg = false;
                DataSet ds = new UserAppealService().GetUserAuthenState(acc, bankID, bankType, out stateMsg);

				if(ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
				{
					//ShowMsg("��ѯ���Ϊ��");
					WebUtils.ShowMessage(this,"��ѯ���Ϊ��");
					return;
				}

				DataRow dr = ds.Tables[0].Rows[0];

				if(dr["queryType"].ToString() == "1")
				{
                    //�����У�������ʾ���� yinhuang 2013.6.14
                    ds.Tables[0].Columns.Add("authenStateName", typeof(string));//�����֤״̬
                    ds.Tables[0].Columns.Add("authenTypeName", typeof(string));//��֤��ʽ
                    ds.Tables[0].Columns.Add("creTypeName", typeof(string));//֤������
                    ds.Tables[0].Columns.Add("Fcard_stat_Name", typeof(string));//���д��״̬

                    Hashtable ht1 = new Hashtable();
                    ht1.Add("1", "����֤");
                    ht1.Add("2", "�����");
                    ht1.Add("3", "��������֤��");
                    ht1.Add("4", "���ʧ��");
                    ht1.Add("5", "��ν��ȷ��ʧ��");
                    ht1.Add("9", "uin�������");
                    ht1.Add("10", "û��");

                    Hashtable ht2 = new Hashtable();
                    ht2.Add("1", "���֤");
                    ht2.Add("2", "����");
                    ht2.Add("3", "����֤");

                    Hashtable ht3 = new Hashtable();
                    ht3.Add("1", "����֤");
                    ht3.Add("2", "����֤");
                    ht3.Add("3", "ȷ�ϴ���ȴ�������Ϣ");
                    ht3.Add("4", "�޸�ע����Ϣʧ��");
                    ht3.Add("9", "uin�ͷ��������");
                    ht3.Add("10", "����");
                    ht3.Add("0", "�����Ϣδ���");

                    Hashtable ht4 = new Hashtable();
                    ht4.Add("1", "�ͷ���֤");
                    ht4.Add("2", "������֤");
                    ht4.Add("3", "paipai��֤");
                    ht4.Add("4", "��Ȩ��֤");
                    ht4.Add("5", "������ʵ��");
                    ht4.Add("6", "һ��ͨʵ����֤");

                    classLibrary.setConfig.DbtypeToPageContent(ds.Tables[0], "Fcard_stat", "Fcard_stat_Name", ht1);
                    classLibrary.setConfig.DbtypeToPageContent(ds.Tables[0], "Fcre_type", "creTypeName", ht2);
                    classLibrary.setConfig.DbtypeToPageContent(ds.Tables[0], "Fcre_stat", "authenStateName", ht3);
                    classLibrary.setConfig.DbtypeToPageContent(ds.Tables[0], "Fauthen_type", "authenTypeName", ht4);

					this.lb_c1.Text = dr["authenTypeName"].ToString();
					this.lb_c2.Text = setConfig.replaceHtmlStr(dr["Fcard_stat_Name"].ToString());
					this.Label3.Text = dr["authenStateName"].ToString();
					this.lb_c3.Text = dr["creTypeName"].ToString();
					this.lb_c4.Text = dr["Fcre_id"].ToString();
					this.lb_c5.Text = classLibrary.getData.GetBankNameFromBankCode(dr["Fbank_type"].ToString());
					this.lb_c6.Text = dr["Fbank_id"].ToString();
					this.lb_c7.Text = dr["Ffirst_authen_id"].ToString();
					this.Label4.Text = dr["Fqqid"].ToString();
					this.lb_c10.Text = setConfig.replaceHtmlStr(dr["Ftried_times"].ToString());
					this.lb_c11.Text = setConfig.replaceHtmlStr(dr["Fcre_change_times"].ToString());
					this.lb_c12.Text = setConfig.replaceHtmlStr(dr["Fcard_changed_times"].ToString());

					this.lb_userStatue.Text = "�û�ʵ����֤��";
				}
				else if(dr["queryType"].ToString() == "2")
				{
                    ds.Tables[0].Columns.Add("authenTypeName", typeof(string));//��֤��ʽ
                    ds.Tables[0].Columns.Add("creTypeName", typeof(string));//֤������

                    Hashtable ht1 = new Hashtable();
                    ht1.Add("1", "�����֤+�ͷ����");
                    ht1.Add("2", "������֤");
                    ht1.Add("3", "paipai��֤");
                    ht1.Add("4", "��Ȩ��֤");
                    ht1.Add("5", "������ʵ��");
                    ht1.Add("6", "һ��ͨʵ����֤");
                    ht1.Add("7", "�غ���ʵ��");
                    ht1.Add("8", "�غ���ʵ����֤");
                    ht1.Add("9", "�ͷ����");
                    ht1.Add("10", "��֤δͨ��");
                    ht1.Add("11", "�����֤+��������Ȩ");

                    Hashtable ht2 = new Hashtable();
                    ht2.Add("1", "���֤");
                    ht2.Add("2", "����");
                    ht2.Add("3", "����֤");
                    ht2.Add("100", "�Թ���Ȩ");

                    classLibrary.setConfig.DbtypeToPageContent(ds.Tables[0], "Fvalue", "authenTypeName", ht1);
                    classLibrary.setConfig.DbtypeToPageContent(ds.Tables[0], "Fstandby1", "creTypeName", ht2);

                    this.lb_c15.Text = setConfig.replaceHtmlStr(dr["authenTypeName"].ToString());
					this.lb_c16.Text = setConfig.replaceHtmlStr(dr["creTypeName"].ToString());
                    var creid = setConfig.replaceHtmlStr(dr["Fattach"].ToString());
                    this.lb_c17.Text = setConfig.ConvertID(creid, creid.Length - 6, 3);
					this.lb_c18.Text = setConfig.replaceHtmlStr(dr["Fcreate_time"].ToString());
					this.lb_c19.Text = setConfig.replaceHtmlStr(dr["Fmodify_time"].ToString());
					this.lb_c20.Text = setConfig.replaceHtmlStr(dr["Fstandby3"].ToString());

					this.lb_userStatue.Text = "�û���ͨ��ʵ����֤";
				}

				//this.div_detail.Style.Add("display","inline");
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
				//WebUtils.ShowMessage(this,setConfig.replaceHtmlStr(ex.Message));
			}
		}

		private void Clear()
		{
            this.lb_queryAcc.Text = "";
            this.lb_c1.Text = "";
			this.lb_c2.Text = "";
			this.lb_c3.Text = "";
			this.lb_c4.Text = "";
			this.lb_c5.Text = "";
			this.lb_c6.Text = "";
			this.lb_c7.Text = "";
			this.lb_c10.Text = "";
			this.lb_c11.Text = "";
			this.lb_c12.Text = "";
			this.Label3.Text = "";
			this.Label4.Text = "";

			this.lb_c15.Text = "";
			this.lb_c16.Text = "";
			this.lb_c17.Text = "";
			this.lb_c18.Text = "";
			this.lb_c19.Text = "";
			this.lb_c20.Text = "";
		}

	}
}
