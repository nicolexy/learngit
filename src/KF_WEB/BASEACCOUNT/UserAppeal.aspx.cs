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
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using TENCENT.OSS.CFT.KF.Common;
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using System.Web.Services.Protocols;
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;
using CFT.Apollo.Logging;

namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
	/// <summary>
    /// UserAppeal ��ժҪ˵����
	/// </summary>
	public partial class UserAppeal : System.Web.UI.Page
	{
       
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��
			// �ڴ˴������û������Գ�ʼ��ҳ��
			ButtonBeginDate.Attributes.Add("onclick", "openModeBegin()"); 
			ButtonEndDate.Attributes.Add("onclick", "openModeEnd()"); 

			try
			{
				Label1.Text = Session["uid"].ToString();

				string szkey = Session["SzKey"].ToString();
				//int operid = Int32.Parse(Session["OperID"].ToString());

				//if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID,"CFTUserPick")) Response.Redirect("../login.aspx?wh=1");
			}
			catch
			{
				Response.Redirect("../login.aspx?wh=1");
			}

			if(!IsPostBack)
			{
                //һ��Ȩ��
                ViewState["CFTUserPick"] = classLibrary.ClassLib.ValidateRight("CFTUserPick", this);
                //����Ȩ��
                ViewState["CFTUserPickQuer"] = classLibrary.ClassLib.ValidateRight("CFTUserPickQuer", this);

                if (!(bool.Parse(ViewState["CFTUserPick"].ToString()) || bool.Parse(ViewState["CFTUserPickQuer"].ToString())))
                    Response.Redirect("../login.aspx?wh=1");

				TextBoxBeginDate.Text = DateTime.Now.AddDays(-3).ToString("yyyy��MM��dd��");
				TextBoxEndDate.Text = DateTime.Now.ToString("yyyy��MM��dd��");
				DropDownListShow();

				Table2.Visible = false;				
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

		public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
		{
			pager.CurrentPageIndex = e.NewPageIndex;
			BindData(e.NewPageIndex);
		}

		private void ValidateDate()
		{
			DateTime begindate;
			DateTime enddate;

			try
			{
				begindate = DateTime.Parse(TextBoxBeginDate.Text);
				enddate = DateTime.Parse(TextBoxEndDate.Text);
			}
			catch
			{
				throw new Exception("������������");
			}

			if(begindate.CompareTo(enddate) > 0)
			{
				throw new Exception("��ֹ����С����ʼ���ڣ����������룡");
			}

            if (!classLibrary.getData.IsTestMode)
                if (this.tbFuin.Text == "" && begindate.AddDays(30) < enddate)
                {
                    throw new Exception("�˺�Ϊ�գ����ڼ������30�죬���������룡");
                }

			ViewState["fstate"] = ddlState.SelectedValue;
			ViewState["fstateUserClass"] = ddlStateUserClass.SelectedValue;

			ViewState["fuin"] = classLibrary.setConfig.replaceMStr(tbFuin.Text.Trim());

			ViewState["begindate"] = DateTime.Parse(begindate.ToString("yyyy-MM-dd 00:00:00"));
			ViewState["enddate"] = DateTime.Parse(enddate.ToString("yyyy-MM-dd 23:59:59"));

			ViewState["ftype"] = ddlType.SelectedValue;
			ViewState["QQType"] = ddlQQType.SelectedValue;
            ViewState["SortType"] = ddlSortType.SelectedValue;//����
            

			//���Ӹ߷ֵ��˹�����ѯ������
			ViewState["dotype"] = DDL_DoType.SelectedValue;
		}

		protected void Button2_Click(object sender, System.EventArgs e)
		{
			try
			{
				ValidateDate();
			}
			catch(Exception err)
            {
                LogHelper.LogError("�����쳣��" + err.ToString(), "UserAppeal");
				WebUtils.ShowMessage(this.Page,err.Message);
				return;
			}

			try
			{
				Table2.Visible = true;
				pager.RecordCount= GetCount(); 
				BindData(1);
			}
			catch(SoapException eSoap) //����soap���쳣
            {
                LogHelper.LogError("���÷������" + eSoap.ToString(), "UserAppeal");
				string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
				WebUtils.ShowMessage(this.Page,"���÷������" + errStr);
			}
			catch(Exception eSys)
            {
                LogHelper.LogError("��ȡ����ʧ�ܣ�" + eSys.ToString(), "UserAppeal");
				WebUtils.ShowMessage(this.Page,"��ȡ����ʧ�ܣ�" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
			}
		}

		private int GetCount()
		{
			string fuin = ViewState["fuin"].ToString();
			DateTime begin = (DateTime)ViewState["begindate"];
			DateTime end = (DateTime)ViewState["enddate"];

			string begindate = begin.ToString("yyyy-MM-dd 00:00:00");
			string enddate = end.ToString("yyyy-MM-dd 23:59:59");

			int fstate = Int32.Parse(ViewState["fstate"].ToString());
			int fstateUserClass = Int32.Parse(ViewState["fstateUserClass"].ToString());
			int ftype = Int32.Parse(ViewState["ftype"].ToString());
			string QQType = ViewState["QQType"].ToString();
            int SortType = Int32.Parse(ViewState["SortType"].ToString());//����

			//���Ӹ߷ֵ��ͷֵ���ѯ
			string dotype = ViewState["dotype"].ToString();
			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

			int Count;
            if (ftype == 20)
			{
                Count = qs.GetUserClassQueryCount(begindate, enddate, fuin, fstateUserClass, QQType, SortType);
			}
            if (ftype == 1 || ftype == 5 || ftype == 6 || ftype == 99)//����������ԭ�������Ͻ��зֿ�ֱ�
            {
                Count = qs.GetCFTUserAppealCountNew(fuin, begindate, enddate, fstate, ftype, QQType, dotype, SortType);
            }
            else
            {
                Count = qs.GetCFTUserAppealCount(fuin, begindate, enddate, fstate, ftype, QQType, dotype, SortType);
            }

			this.lblTotal.Text = "��¼����:" + Count;
			return Count;
		}

		private void BindData(int index)
		{
            pager.CurrentPageIndex = index;
			string fuin = ViewState["fuin"].ToString();
			DateTime begin = (DateTime)ViewState["begindate"];
			DateTime end = (DateTime)ViewState["enddate"];

			string begindate = begin.ToString("yyyy-MM-dd 00:00:00");
			string enddate = end.ToString("yyyy-MM-dd 23:59:59");

			int fstate = Int32.Parse(ViewState["fstate"].ToString());
			int fstateUserClass = Int32.Parse(ViewState["fstateUserClass"].ToString());
			int ftype = Int32.Parse(ViewState["ftype"].ToString());
			string QQType = ViewState["QQType"].ToString();
            int SortType = Int32.Parse(ViewState["SortType"].ToString());//����
            

			int max = pager.PageSize;
			int start = max * (index-1) + 1;

			//���Ӹ߷ֵ��ͷֵ���ѯ
			string dotype = ViewState["dotype"].ToString();

			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
			Finance_Header fh = classLibrary.setConfig.setFH(this);
			qs.Finance_HeaderValue = fh;

            DataSet ds = new DataSet();
			if(ftype == 20)
			{
                ds = qs.GetUserClassQueryList(begindate, enddate, fuin, fstateUserClass, QQType, start, max, SortType);
			}
            if (ftype == 1 || ftype == 5 || ftype == 6 || ftype == 99)//����������ԭ�������Ͻ��зֿ�ֱ�
            {
               DataSet dsNew = qs.GetCFTUserAppealListNew(fuin, begindate, enddate, fstate, ftype, QQType, dotype, start, max, SortType);
                //�����ҳ����
                if (dsNew != null && dsNew.Tables.Count > 0 && dsNew.Tables[0].Rows.Count > 0)
                {
                    //����
                    DataTable dt = dsNew.Tables[0];
                    DataView view = dt.DefaultView;
                    if (SortType==0)
                        view.Sort = "FSubmitTime asc";
                    if (SortType == 1)
                        view.Sort = "FSubmitTime desc";
                    dt = view.ToTable();
                    dt = PublicRes.GetPagedTable(dt, index, pager.PageSize);
                    ds.Tables.Add(dt);
                }

                ds = qs.GetCFTUserAppealListFunction(ds);//�������ٴ����ڲ���Ϣ
            }
			else
			{
                ds = qs.GetCFTUserAppealList(fuin, begindate, enddate, fstate, ftype, QQType, dotype, start, max, SortType);
			}

			if(ds != null && ds.Tables.Count >0)
			{
				ds.Tables[0].Columns.Add("URL",typeof(string));

                if (ftype == 20 && ds.Tables[0].Rows.Count > 0)
				{
					ds.Tables[0].Columns.Add("FUin",typeof(String));
					ds.Tables[0].Columns.Add("Femail",typeof(String));
					ds.Tables[0].Columns.Add("FTypeName",typeof(String));
					ds.Tables[0].Columns.Add("FStateName",typeof(String));
					ds.Tables[0].Columns.Add("FSubmitTime",typeof(String));
					ds.Tables[0].Columns.Add("FCheckInfo",typeof(String));

					foreach(DataRow dr in ds.Tables[0].Rows)
					{
						dr["FUin"] = dr["Fqqid"].ToString();
						dr["FTypeName"] = "ʵ����֤";
						dr["FStateName"] = dr["FpickstateName"].ToString();
						dr["FSubmitTime"] = dr["Fcreate_time"].ToString();
						dr["FCheckInfo"] = dr["Fmemo"].ToString();
                        dr["URL"] = "CFTUserCheck.aspx?fid=&db=&tb=&flist_id=" + dr["flist_id"].ToString();

						//���������Ӵ��������ʾ������Ϊ�Ѿ���VIP��ѯѡ������Բ��ô���VIP���֣�
						if(dr["Fuincolor"].ToString() == "BIGMONEY")
						{
							dr["FUin"] = "<FONT color=\"red\">" + dr["Fuin"] + "</FONT>";
						}
					}
				}
                else
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (ftype == 1 || ftype == 5 || ftype == 6 || ftype == 99)
                        {
                            //�����������������ݿ⡢����������
                            dr["URL"] = "CFTUserCheck.aspx?fid=" + dr["FID"].ToString() + "&flist_id=&db=" + dr["DBName"]+"&tb="+dr["tableName"];
                        }
                        else
                        {
                            dr["URL"] = "CFTUserCheck.aspx?fid=" + dr["FID"].ToString() + "&flist_id=&db=&tb=";
                        }
                        //���������Ӵ��������ʾ������Ϊ�Ѿ���VIP��ѯѡ������Բ��ô���VIP���֣�
                        if (dr["Fuincolor"].ToString() == "BIGMONEY")
                        {
                            dr["FUin"] = "<FONT color=\"red\">" + dr["Fuin"] + "</FONT>";
                        }
                    }
                }

				DataGrid1.DataSource = ds.Tables[0].DefaultView;
				DataGrid1.DataBind();
			}
			else
			{
				throw new LogicException("û���ҵ���¼��");
			}
		}

		protected void ddlType_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			DropDownListShow();
		}

		private void DropDownListShow()
		{
			if(this.ddlType.SelectedValue == "20")
			{
				this.ddlState.Visible = false;
				this.ddlStateUserClass.Visible = true;
			}
			else
			{
				this.ddlState.Visible = true;
				this.ddlStateUserClass.Visible = false;
			}
		}

		protected void btnGet_Click(object sender, System.EventArgs e)
		{
            try
            {
                ValidateDate();
            }
            catch (Exception err)
            {
                LogHelper.LogError("��ȡ�ύ�����쳣��" + err.ToString(), "UserAppeal");
                WebUtils.ShowMessage(this.Page, err.Message);
                return;
            }

			try
			{
				DateTime begindate;
				DateTime enddate;
				int TicketsCount;

				try
				{
					begindate = DateTime.Parse(TextBoxBeginDate.Text);
					enddate = DateTime.Parse(TextBoxEndDate.Text);
				}
				catch
				{
					throw new Exception("������������");
				}

				if(begindate.CompareTo(enddate) > 0)
				{
					throw new Exception("��ֹ����С����ʼ���ڣ����������룡");
				}

				try
				{
					TicketsCount = int.Parse(this.txtCount.Text.Trim());
				}
				catch
				{
					throw new Exception("��������ȷ�Ĺ�������");
				}
				if(TicketsCount < 1)
				{
					throw new Exception("������������С��1��");
				}
				if(TicketsCount > 50)
				{
					throw new Exception("�����������ֵΪ50��");
				}

				if(!classLibrary.getData.IsTestMode)
					if(begindate.AddDays(30) < enddate)
					{
						throw new Exception("���ڼ������30�죬���������룡");
					}

                //���Ӹ߷ֵ��˹�����ѯ������
                ViewState["dotype"] = DDL_DoType.SelectedValue;

                int SortType = Int32.Parse(ViewState["SortType"].ToString());//����

				if(ddlType.SelectedValue == "20")
				{
					if(ddlStateUserClass.SelectedValue != "99" && ddlStateUserClass.SelectedValue != "0" && ddlStateUserClass.SelectedValue != "1")//0�����������쵥,�����99��Ϊ���쵥����,���쵥�ǻ���0,����1Ҳ������
					{
						throw new Exception("��״̬�������쵥");
					}
					Response.Write("<script>window.open('UserClassCheck.aspx?BeginDate=" + begindate.ToString("yyyy-MM-dd") + "&EndDate=" + enddate.ToString("yyyy-MM-dd") + "&fstate=" +
						ddlStateUserClass.SelectedValue + "&Count=" + TicketsCount.ToString() + "&SortType=" + SortType + "','_blank','');</script>");
				}
				else
				{
					if(ddlState.SelectedValue != "99" && ddlState.SelectedValue != "0" && ddlState.SelectedValue != "3" && ddlState.SelectedValue != "4"
						&& ddlState.SelectedValue != "5" && ddlState.SelectedValue != "6" && ddlState.SelectedValue != "8")//0,3,4,5,6�����������쵥,�����99��Ϊ���쵥����,���쵥�ǻ���0,3,4,5,6��,����8Ҳ������
					{
						throw new Exception("������״̬�������쵥");
					}
					Response.Write("<script>window.open('UserAppealCheck.aspx?BeginDate=" + begindate.ToString("yyyy-MM-dd") + "&EndDate=" + enddate.ToString("yyyy-MM-dd") + "&fstate=" +
                        ddlState.SelectedValue + "&ftype=" + ddlType.SelectedValue + "&qqtype=" + ddlQQType.SelectedValue + "&Count=" + TicketsCount.ToString() + "&SortType=" + SortType + "&dotype=" + DDL_DoType.SelectedValue + "','_blank','');</script>");
				}
			}
			catch(Exception err)
            {
                LogHelper.LogError("��ȡ��Ϣ�쳣��" + err.ToString(), "UserAppeal");
				WebUtils.ShowMessage(this.Page,err.Message);
				return;
			}
		}

        //��ϸ���ݰ�ť
        public void DataGrid1_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            object obj = e.Item.Cells[8].FindControl("queryButton");
            if (obj != null)
            {
                long balance = long.Parse(e.Item.Cells[7].Text.Trim());//�˻��ʽ�
                LinkButton lb = (LinkButton)obj;
                if (balance / 100 >= 1000)//�˻��ʽ���1000Ԫ������1000Ԫ�����ϵ�������һ��Ȩ�ޣ��ɴ����κν������ߣ�
                {
                    if (bool.Parse(ViewState["CFTUserPick"].ToString()))
                    {
                        lb.Visible = true;
                    }
                }
                else//1000Ԫ�����������ѵ�¼ҳ���˵����Ȩ��
                {
                    lb.Visible = true;
                }
            }
        }   
	}
}