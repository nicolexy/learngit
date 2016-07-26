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
using Tencent.DotNet.Common.UI;

namespace TENCENT.OSS.CFT.KF.KF_Web.FreezeManage
{
	/// <summary>
	/// FreezeVerify ��ժҪ˵����
	/// </summary>
	public partial class FreezeVerify : TENCENT.OSS.CFT.KF.KF_Web.PageBase
	{
		protected System.Web.UI.WebControls.TextBox tbx_listno1;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��

			if(!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter",this)) Response.Redirect("../login.aspx?wh=1");

			if(!IsPostBack)
			{
				this.tbx_beginDate.Value = DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd 00:00:00");
                this.tbx_endDate.Value = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
			}
            pager.PageSize = 10;
			pager.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(pager_PageChanged);

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
			this.DataGrid_QueryResult.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.DataGrid_QueryResult_ItemDataBound);

		}
		#endregion


		private void BindData(int index)
		{
			this.pager.CurrentPageIndex = index;

			DateTime beginDate;
			DateTime endDate;

			try
			{
                beginDate = DateTime.Parse(this.tbx_beginDate.Value);
                endDate = DateTime.Parse(this.tbx_endDate.Value);

				if(beginDate.CompareTo(endDate) > 0)
				{
					throw new Exception("��ʼ���ڴ��ڽ�������");
				}
                if (beginDate.AddMonths(1).AddDays(1) < endDate)
				{
					throw new Exception("���ڼ������һ���£����������룡");
				}

				Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

				Query_Service.Finance_Header fh = classLibrary.setConfig.setFH(this);
				qs.Finance_HeaderValue = fh;

				int allRecordCount = 0;

                DataSet ds = qs.GetFreezeList_3(this.tbx_payAccount.Text.Trim(), beginDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"),
                    int.Parse(this.ddl_orderState.SelectedValue), this.tbx_listNo.Text.Trim(), this.tbx_people.Text.Trim(), this.tbx_reason.Text.Trim(),
                    (index - 1) * this.pager.PageSize, this.pager.PageSize, this.ddl_queryOrderType.SelectedValue, out allRecordCount);

				this.lb_count.Text = allRecordCount.ToString();

				this.pager.RecordCount = allRecordCount;

				if(ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
				{
					ShowMsg("��ѯ���Ϊ��");
					this.DataGrid_QueryResult.DataSource = null;
					this.DataGrid_QueryResult.DataBind();
					return;
				}

				ds.Tables[0].Columns.Add("OpUrl",typeof(string));
				ds.Tables[0].Columns.Add("DiaryUrl",typeof(string));
				ds.Tables[0].Columns.Add("handleStateName",typeof(string));
				ds.Tables[0].Columns.Add("handleUserName",typeof(string));

				for(int i=0;i<ds.Tables[0].Rows.Count;i++)
				{
					DataRow dr = ds.Tables[0].Rows[i];

                    dr["OpUrl"] = @".\FreezeVerify.aspx?fid=" + dr["FID"].ToString() + "&ffreeze_id=" + dr["FUin"].ToString();
                    dr["DiaryUrl"] = @".\FreezeDiary.aspx?FFreezeListID=" + dr["FID"].ToString() + "&ffreeze_id=" + dr["FUin"].ToString();

					string stateName = "{0}";

					if(dr["isFreezeListHas"].ToString() == "0")
					{
						stateName = "���˻������ڶ���״̬ ({0})";
					}
					
					switch(dr["Fstate"].ToString())
					{
						case "0":
						{
							stateName = string.Format(stateName,"δ����");
							break;
						}
						case "1":
						{
							stateName = string.Format(stateName,"�ᵥ(�ѽⶳ)");
							break;
						}
						case "2":
						{
                            stateName = string.Format(stateName, "�ᵥ��δ�ⶳ��");
							break;
						}
						case "7":
						{
							stateName = string.Format(stateName,"������");
							break;
						}
						case "8":
						{
							stateName = string.Format(stateName,"����");				
							break;
						}                       				
						default:
						{
							stateName = string.Format(stateName,"δ֪" + dr["Fstate"].ToString());
							break;
						}
					}

					dr["handleStateName"] = stateName;
					dr["handleUserName"] = dr["FCheckUser"].ToString();
					
				}

				ds.AcceptChanges();

				this.DataGrid_QueryResult.DataSource = ds;
				this.DataGrid_QueryResult.DataBind();
			}
			catch(Exception ex)
			{
                WebUtils.ShowMessage(this.Page, ex.Message);
			}
		}

		protected void btn_query_Click(object sender, System.EventArgs e)
		{
			BindData(1);
		}

		private int GetRecordCount()
		{
			// �����ò�ѯ���ݿ�����ѯҳ�����ˣ�ֱ�Ӳ���10000
			return 10000;
		}

		private void ShowMsg(string szMsg)
		{
			Response.Write("<script language=javascript>alert('" + szMsg + "')</script>");
		}

		private void pager_PageChanged(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
		{
			int newPageIndex = e.NewPageIndex;
			this.pager.CurrentPageIndex = newPageIndex;

			BindData(newPageIndex);
		}

		private string ConvertHandleTypeToString(string type)
		{
			switch(type)
			{
				case "1":
				{
					return "�ᵥ(δ�ⶳ)";
				}
				case "2":
				{
					return "�ᵥ(�ѽⶳ)";
				}
				case "3":
				{
					return "����";
				}
				case "10":
				{
					return "����";
				}
				case "100":
				{
					return "���䴦����";
				}
				default:
				{
					return "δ֪����" + type;
				}
			}
		}

		private void DataGrid_QueryResult_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			if(e.Item.Cells[8].Text == "BIGMONEY")
			{
				e.Item.Cells[0].ForeColor = Color.Red;
				e.Item.Cells[1].ForeColor = Color.Red;
			}
		}

	}
}
