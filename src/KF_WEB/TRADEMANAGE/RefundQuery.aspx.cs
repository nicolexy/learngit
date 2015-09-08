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
using System.Web.Services.Protocols;
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.CFT.KF.KF_Web;
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;


namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
	/// <summary>
	/// RefundQuery ��ժҪ˵����
	/// </summary>
	public partial class RefundQuery : System.Web.UI.Page
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			ButtonBeginDate.Attributes.Add("onclick", "openModeBegin()"); 

			if(Session["uid"] == null)
				Response.Redirect("../login.aspx?wh=1");

			if (!IsPostBack)
			{
				try
				{
					Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
					object[] al = qs.GetAllRefundBank();
                    ddlBankTypeInit(al);
                    
					string fbatchid = Request["batchid"];
					if(fbatchid != null && fbatchid.Trim().Length == 13)
					{
						ViewState["batchid"] = fbatchid.Trim();

						TextBoxBeginDate.Text = fbatchid.Substring(0,4) + "-" + fbatchid.Substring(4,2) + "-" + fbatchid.Substring(6,2);
						ddlBankType.SelectedValue = fbatchid.Substring(8,4);

						//	pager.RecordCount= 1000;
						//	BindData(1);
					}
					else
					{
						ViewState["batchid"] = DateTime.Now.AddDays(-1).ToString("yyyyMMdd") + "9999R";

						TextBoxBeginDate.Text = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
						ddlBankType.SelectedValue = "9999";
					}
				}
				catch(Exception ex)
				{
					WebUtils.ShowMessage(this.Page,"��ʼ������" + ex.Message);
				}

			}
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
			this.pager.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(this.ChangePage);

		}
		#endregion

        /// <summary>
        /// ��ʼ��ddlBankType item����
        /// ����������
        /// </summary>
        public void ddlBankTypeInit(object[] al)
        {
            Hashtable bankTypeName = new Hashtable();
            ddlBankType.Items.Clear();
            if (al != null)
            {
                foreach (Object obj in al)
                {
                    string banktype = obj.ToString();
                    string bankname = classLibrary.setConfig.convertbankType(banktype);
                    if (!bankTypeName.Contains(bankname))
                        bankTypeName.Add(bankname, banktype);
                }
            }

            ArrayList akeys = new ArrayList(bankTypeName.Keys);
            akeys.Sort();
            foreach (string k in akeys)
            {
                ddlBankType.Items.Add(new ListItem(k.ToString(), bankTypeName[k].ToString()));
            }

        }

		public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
		{
			try
			{
				pager.CurrentPageIndex = e.NewPageIndex;
				BindData(e.NewPageIndex);
			}
			catch(SoapException eSoap) //����soap���쳣
			{
				string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
				WebUtils.ShowMessage(this.Page,"���÷������" + errStr);
			}
			catch(Exception eSys)
			{
				WebUtils.ShowMessage(this.Page,"��ȡ����ʧ�ܣ�" + eSys.Message);
			}
		}


		private int GetCount()
		{
			string batchid = ViewState["batchid"].ToString();
			int ifromtype = Int32.Parse(ddlFromType.SelectedValue);
			int irefundtype = Int32.Parse(ddlRefundType.SelectedValue);
			int irefundstate = Int32.Parse(ddlRefundState.SelectedValue);
			int ireturnstate = Int32.Parse(ddlReturnState.SelectedValue);
			string listid = tbListID.Text.Trim();
            string Fbank_listid = tbFbank_listid.Text.Trim();

			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            return qs.GetRefundListCount(batchid, ifromtype, irefundtype, irefundstate, ireturnstate, listid, Fbank_listid);
		}

		private void BindData(int index)
		{
			string batchid = ViewState["batchid"].ToString();
			int ifromtype = Int32.Parse(ddlFromType.SelectedValue);
			int irefundtype = Int32.Parse(ddlRefundType.SelectedValue);
			int irefundstate = Int32.Parse(ddlRefundState.SelectedValue);
			int ireturnstate = Int32.Parse(ddlReturnState.SelectedValue);
			string listid = tbListID.Text.Trim();
            string Fbank_listid = tbFbank_listid.Text.Trim();

			int max = pager.PageSize;
			int start = max * (index-1) + 1;

			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

			Query_Service.Finance_Header fh = classLibrary.setConfig.setFH(this);
			qs.Finance_HeaderValue = fh;

            DataSet ds = qs.GetRefundList(batchid, ifromtype, irefundtype, irefundstate, ireturnstate, listid, Fbank_listid, start, max);

			if(ds != null && ds.Tables.Count >0)
			{
				ds.Tables[0].Columns.Add("FreturnamtName",typeof(String));
				classLibrary.setConfig.FenToYuan_Table(ds.Tables[0],"Freturnamt","FreturnamtName");

				ds.Tables[0].Columns.Add("FamtName",typeof(String));
				classLibrary.setConfig.FenToYuan_Table(ds.Tables[0],"Famt","FamtName");

				ds.Tables[0].Columns.Add("FstateName",typeof(String));
				ds.Tables[0].Columns.Add("FreturnStateName",typeof(String));

				ds.Tables[0].Columns.Add("FrefundPathName",typeof(String));
				
				foreach(DataRow dr in ds.Tables[0].Rows)
				{
					dr.BeginEdit();

					string tmp = dr["Fstate"].ToString();
					if(tmp == "0") 
						tmp = "��ʼ״̬";
					else if(tmp == "1") 
						tmp = "�˵�������";
					else if(tmp == "2") 
						tmp = "�˵��ɹ�";
					else if(tmp == "3") 
						tmp = "�˵�ʧ��";
					else if(tmp == "4") 
						tmp = "�˵�״̬δ��";
					else if(tmp == "5") 
						tmp = "�ֹ��˵���";
					else if(tmp == "6") 
						tmp = "�����ֹ��˵�";
					else if(tmp == "7") 
						tmp = "����ת�����";

					
					dr["FstateName"] = tmp;

					tmp = dr["FreturnState"].ToString();
					if(tmp == "1") 
						tmp = "�ص�ǰ";
					else if(tmp == "2") 
						tmp = "�ص���";					

					dr["FreturnStateName"] = tmp;

					tmp = dr["FrefundPath"].ToString();
					if(tmp == "1") 
						tmp = "�����˵�";
					else if(tmp == "2") 
						tmp = "�ӿ��˵�";
					else if(tmp == "3") 
						tmp = "�˹���Ȩ";
					else if(tmp == "4") 
						tmp = "ת���˵�";
					else if(tmp == "5") 
						tmp = "ת�����";
					else if(tmp == "6") 
						tmp = "�����˿�";
					
					dr["FrefundPathName"] = tmp;

					dr.EndEdit();
				}

				DataGrid1.DataSource = ds.Tables[0].DefaultView;
				DataGrid1.DataBind();
			}
			else
			{
				throw new LogicException("û���ҵ���¼��");
			}
		}


        protected void btnQuery_Click(object sender, System.EventArgs e)
        {
            labErrMsg.Text = "";

            string date = DateTime.Parse(TextBoxBeginDate.Text).ToString("yyyyMMdd");
            string bank = ddlBankType.SelectedValue;
            
            //���Σ�1 2 3 4 5����Ӧ��ĸ��R T U W Y
            //Ĭ��ѡ���һ��
            string batchid = date + bank + "R";
            if (ddlFbatchid.SelectedValue != "0")
            {
                batchid = date + bank + ddlFbatchid.SelectedValue;
            }
            ViewState["batchid"] = batchid;

            pager.RecordCount = 1000;//GetCount(); 

            BindData(1);

        }

	}
}
