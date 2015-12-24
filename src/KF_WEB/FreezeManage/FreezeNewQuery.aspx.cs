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
using CFT.CSOMS.COMMLIB;
using System.Configuration;
using CFT.CSOMS.BLL.FreezeModule;
using System.Text;
using System.Net;
using System.Xml;
using System.IO;
using System.Web.Services.Protocols;
using CFT.CSOMS.BLL.CFTAccountModule;

namespace TENCENT.OSS.CFT.KF.KF_Web.FreezeManage
{
	/// <summary>
    /// FreezeNewQuery ��ժҪ˵����
	/// </summary>
    public partial class FreezeNewQuery : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.TextBox tbx_listno1;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��

			if(!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter",this)) Response.Redirect("../login.aspx?wh=1");
			if(!IsPostBack)
			{
                DateTime dt = DateTime.Now;
                if (DateTime.Now.Month == 3)
                    dt = DateTime.Now.AddDays(-27);//��ֹ����3��1�ţ�����3���µ�ʱ��Σ��鲻��2�µ�����
                else
                    dt = DateTime.Now.AddDays(-29);
                DateTime grayDate = DateTime.Parse("2014-06-05");
                if (dt.CompareTo(grayDate) < 0)
                {
                    this.tbx_beginDate.Value = grayDate.ToString("yyyy-MM-dd 00:00:00");
                }
                else 
                {
                    this.tbx_beginDate.Value = dt.ToString("yyyy-MM-dd 00:00:00");
                }

                this.tbx_endDate.Value = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
			}

			//this.pager.RecordCount = this.GetRecordCount();
            pager.PageSize = 10;
			pager.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(pager_PageChanged);

			//this.DataGrid_QueryResult.ItemCommand += new DataGridCommandEventHandler(DataGrid_QueryResult_ItemCommand);
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
                //TENCENT.OSS.C2C.Finance.Common.CommLib.CommMailSend.SendMsgQQTips("75942086", "102236", "url=http://action.tenpay.com/cuifei/2014/fengkong/unfreeze_suc.shtml");
                //userid=75942086
                //TENCENT.OSS.C2C.Finance.Common.CommLib.CommMailSend.SendMessage("13528403205", "102237", "user=75942086");
                //return;
                #region ��ѯ��������
                string uin = "";
                if (!string.IsNullOrEmpty(this.tbx_payAccount.Text))
                {
                    string queryType = GetQueryType();
                    uin = AccountService.GetQQID(queryType, this.tbx_payAccount.Text);
                }

                beginDate = DateTime.Parse(this.tbx_beginDate.Value);
				endDate = DateTime.Parse(this.tbx_endDate.Value);

				if(beginDate.CompareTo(endDate) > 0)
				{
                    ShowMsg("��ʼ���ڴ��ڽ�������");
                    return;
				}
				if(beginDate.AddMonths(1).AddDays(1) < endDate)
				{
                    ShowMsg("���ڼ������һ���£����������룡");
                    return;
				}

                //string freezeGrayDate = System.Configuration.ConfigurationManager.AppSettings["FreezeGrayDate"];
                string freezeGrayDate = "2014-06-05";
                DateTime grayDate = DateTime.Parse(freezeGrayDate);
                if (beginDate.CompareTo(grayDate) < 0) 
                {
                    ShowMsg("ֻ�ܲ�ѯ" + freezeGrayDate + "֮��ļ�¼��֮ǰ�ļ�¼�뵽[��ؽⶳ���]��ѯ��");
                    return;
                }

                //��ȡ����״̬
                int state = 99;
                int ftype = int.Parse(this.ddlType.SelectedValue);
                if (ftype == 8 || ftype == 19)
                {
                    state = int.Parse(this.ddl_orderState.SelectedValue);
                }
                else if (ftype == 11)
                {
                    state = int.Parse(this.ddl_orderStateSpecial.SelectedValue);
                }



                #endregion

                DataSet ds = null;  //�����
                Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
                Query_Service.Finance_Header fh = classLibrary.setConfig.setFH(this);
                qs.Finance_HeaderValue = fh;

                int allRecordCount = 0;

                ds = qs.GetFreezeList_New(uin, beginDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"), int.Parse(this.ddlType.SelectedValue),
                  state, this.tbx_listNo.Text.Trim(), this.tbx_people.Text.Trim(), this.tbx_reason.Text.Trim(),
                  (index - 1) * this.pager.PageSize, this.pager.PageSize, this.ddl_queryOrderType.SelectedValue, "ddl_channel.SelectedValue", out allRecordCount);

                
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
                ds.Tables[0].Columns.Add("channel_str", typeof(string));

				for(int i=0;i<ds.Tables[0].Rows.Count;i++)
				{
					DataRow dr = ds.Tables[0].Rows[i];

                    if (!(dr["FsubmitTime"] is DBNull))
                    {
                        if (dr["FsubmitTime"].ToString() != "") 
                        {
                            DateTime subTime = DateTime.Parse(dr["FsubmitTime"].ToString());
                            dr["OpUrl"] = @"FreezeProcessDetail.aspx?fid=" + dr["FID"].ToString() + "&ffreeze_id=" + dr["FUin"].ToString() + "&fsubmit_date=" + dr["FsubmitTime"].ToString();
                        }
                    }
					dr["DiaryUrl"] = @"FreezeDiary.aspx?FFreezeListID="+ dr["FID"].ToString() + "&ffreeze_id=" + dr["FUin"].ToString();
                    dr["handleUserName"] = dr["FCheckUser"].ToString();

                    #region ״̬����
                       string stateName = "{0}";
                    if (ftype == 8 || ftype == 19)//��������״̬����
                    {
                        if (dr["isFreezeListHas"].ToString() == "0")
                        {
                            stateName = "���˻������ڶ���״̬ ({0})";
                        }

                        switch (dr["Fstate"].ToString())
                        {
                            case "0":
                                {
                                    stateName = string.Format(stateName, "δ����");
                                    break;
                                }
                            case "1":
                                {
                                    stateName = string.Format(stateName, "�ᵥ(�ѽⶳ)");
                                    break;
                                }
                            case "2":
                                {
                                    stateName = string.Format(stateName, "����������");
                                    break;
                                }
                            case "7":
                                {
                                    stateName = string.Format(stateName, "������");
                                    break;
                                }
                            case "8":
                                {
                                    stateName = string.Format(stateName, "����");
                                    break;
                                }
                            case "10":
                                {
                                    stateName = string.Format(stateName, "�Ѳ�������");
                                    break;
                                }
                            case "21":
                                {
                                    stateName = string.Format(stateName, "�ᵥ���޶�����־��");
                                    break;
                                }
                            default:
                                {
                                    stateName = string.Format(stateName, "δ֪" + dr["Fstate"].ToString());
                                    break;
                                }
                        }

                        dr["handleStateName"] = stateName;
                    }
                    else if (ftype == 11)//�����һ�֧������״̬
                    {
                        switch (dr["Fstate"].ToString())
                        {
                            case "0":
                                {
                                    stateName = string.Format(stateName, "δ����"); break;
                                }
                            case "1":
                                {
                                    stateName = string.Format(stateName, "���߳ɹ�"); break;
                                }
                            case "2":
                                {
                                    stateName = string.Format(stateName, "����ʧ��"); break;
                                }
                            case "7":
                                {
                                    stateName = string.Format(stateName, "��ɾ��"); break;
                                }
                            case "11":
                                {
                                    stateName = string.Format(stateName, "����������"); break;
                                }
                            case "12":
                                {
                                    stateName = string.Format(stateName, "�Ѳ�������"); break;
                                }
                            case "20":
                                {
                                    stateName = string.Format(stateName, "�ᵥ��δ�������ϣ�"); break;
                                }
                            default:
                                {
                                    stateName = string.Format(stateName, "δ֪" + dr["Fstate"].ToString());
                                    break;
                                }
                        }

                        dr["handleStateName"] = stateName;
                        //this.DataGrid_QueryResult.Columns[this.DataGrid_QueryResult.Columns.Count - 7].Visible = false;//���ض���ԭ����
                    }
                    else
                    {
                        stateName = string.Format(stateName, "�������Ͳ��ԣ�" + dr["Fstate"].ToString());
                    }
                    #endregion

                    #region �������� 
                    //string Ffreeze_channel = (dr["channel_str"] = dr["Ffreeze_channel"]).ToString();
                    //foreach (ListItem item in ddl_channel.Items)
                    //{
                    //    if (item.Value == Ffreeze_channel)
                    //    {
                    //        dr["channel_str"] = item.Text;
                    //        continue;
                    //    }
                    //} 
                    #endregion
                
                    
				}

				ds.AcceptChanges();

				this.DataGrid_QueryResult.DataSource = ds;
				this.DataGrid_QueryResult.DataBind();
            }
            catch (SoapException eSoap) //����soap���쳣
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "���÷������" + errStr);
            }
			catch(Exception ex)
			{
                ShowMsg(ex.Message);
                return;
			}
		}

		protected void btn_query_Click(object sender, System.EventArgs e)
		{
			BindData(1);
		}

        private string GetQueryType()
        {
            if (string.IsNullOrEmpty(this.tbx_payAccount.Text))
            {
                throw new Exception("������Ҫ��ѯ���˺�");
            }
            if (this.WeChatCft.Checked)
            {
                return "WeChatCft";
            }
            else if (this.WeChatUid.Checked)
            {
                return "WeChatUid";
            }
            else if (this.WeChatQQ.Checked)
            {
                return "WeChatQQ";
            }
            else if (this.WeChatMobile.Checked)
            {
                return "WeChatMobile";
            }
            else if (this.WeChatEmail.Checked)
            {
                return "WeChatEmail";
            }
            else if (this.WeChatId.Checked)
            {
                return "WeChatId";
            }

            return null;
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

        protected void ddlType_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            int ftype = int.Parse(this.ddlType.SelectedValue);
            if (ftype == 8 || ftype == 19)
            {
                this.ddl_orderState.Visible = true;
                this.ddl_orderStateSpecial.Visible = false;     
            }
            else if (ftype == 11)
            {
                this.ddl_orderState.Visible = false;
                this.ddl_orderStateSpecial.Visible = true;
            }
        }

	}
}
