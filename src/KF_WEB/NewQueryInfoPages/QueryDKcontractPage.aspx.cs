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
using Tencent.DotNet.Common.UI;
using CFT.CSOMS.BLL.TransferMeaning;

namespace TENCENT.OSS.CFT.KF.KF_Web.NewQueryInfoPages
{
	/// <summary>
	/// QueryDKcontractPage 的摘要说明。
	/// </summary>
	public partial class QueryDKcontractPage : System.Web.UI.Page
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// 在此处放置用户代码以初始化页面

			if(!IsPostBack)
			{
				this.tbx_beginDate.Value = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss");
				this.tbx_endDate.Value = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss");
				
				this.pager.PageSize = 10;
				this.pager.RecordCount = GetCount();

				if(Request.QueryString["spid"] != null || Request.QueryString["spbatchid"] != null)
				{
					if(Request.QueryString["spid"] != null)
						this.tbx_spid.Text = Request.QueryString["spid"].Trim();

					if(Request.QueryString["spbatchid"] != null)
						this.tbx_sp_batchid.Text = Request.QueryString["spbatchid"].Trim();

					if(Request.QueryString["state"] != null)
					{
						if(Request.QueryString["state"] == "s")
						{
							this.ddl_status.SelectedIndex = 1;
						}
						else if(Request.QueryString["state"] == "f")
						{
							this.ddl_status.SelectedIndex = 2;
						}
						else if(Request.QueryString["state"] == "h")
						{
							this.ddl_status.SelectedIndex = 3;
						}
					}

					if(Request.QueryString["sDate"] != "")
						this.tbx_beginDate.Value = Request.QueryString["sDate"];

					if(Request.QueryString["eDate"] != "")
                        this.tbx_endDate.Value = Request.QueryString["eDate"];

					BindData(1,true);
				}

			}


			this.pager.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(pager_PageChanged);
			this.DataGrid_QueryResult.ItemCommand += new DataGridCommandEventHandler(DataGrid_QueryResult_ItemCommand);
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
		/// 设计器支持所需的方法 - 不要使用代码编辑器修改
		/// 此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{    

		}
		#endregion


	
		private int GetCount()
		{
			return 1000;
		}



		private void BindData(int pageIndex,bool needUpdateCount)
		{
			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

			//qs.Finance_HeaderValue = setConfig.setFH(Session["OperID"].ToString(),Request.UserHostAddress);

			qs.Finance_HeaderValue = classLibrary.setConfig.setFH(this);

			try
			{
				DateTime sTime ,eTime;
				string strSTime,strETime;
				try
				{
                    sTime = DateTime.Parse(this.tbx_beginDate.Value);
                    eTime = DateTime.Parse(this.tbx_endDate.Value);

					strSTime = sTime.ToString("yyyy-MM-dd HH:mm:ss");
					strETime = eTime.ToString("yyyy-MM-dd HH:mm:ss");

									
				}
				catch
				{
					WebUtils.ShowMessage(this,"日期格式不正确");
					return;
				}

				DataSet ds =qs.QueryDKContractList(tbx_spid.Text.Trim(),tbx_mer_cnr.Text.Trim(),tbx_sp_batchid.Text.Trim(),
					tbx_mobile.Text.Trim(),tbx_bankacc_no.Text.Trim(),tbx_uname.Text.Trim(),tbx_credit_id.Text.Trim(),tbx_mer_aid.Text.Trim(),
					ddl_status.SelectedValue,strSTime,strETime,
					(pageIndex - 1) * this.pager.PageSize,this.pager.PageSize);

				if(ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
				{
					WebUtils.ShowMessage(this,"查询结果为空");
					this.Clear();
					return;
				}

				ds.Tables[0].Columns.Add("Fbankacc_attrName",typeof(string));
				ds.Tables[0].Columns.Add("Fbank_typeName",typeof(string));
				ds.Tables[0].Columns.Add("Fbankacc_typeName",typeof(string));

				foreach(DataRow dr in ds.Tables[0].Rows)
				{
					dr["Fbank_typeName"] = getData.GetBankNameFromBankCode(dr["Fbank_type"].ToString());
					
					if(dr["Fbankacc_attr"].ToString() == "0")
						dr["Fbankacc_attrName"] = "公司";
					else
						dr["Fbankacc_attrName"] = "个人";

					string tmp = dr["Fbankacc_type"].ToString();
					if(tmp == "1")
						dr["Fbankacc_typeName"] = "存折";
					else if(tmp == "2")
						dr["Fbankacc_typeName"] = "信用卡";
					else 
						dr["Fbankacc_typeName"] = "银行卡";

				}

				

				this.DataGrid_QueryResult.DataSource = ds;
				this.DataGrid_QueryResult.DataBind();
			}
			catch(Exception ex)
			{
				WebUtils.ShowMessage(this,ex.Message);
			}
		}



		protected void btn_serach_Click(object sender, System.EventArgs e)
		{
			BindData(1,true);

			this.pager.CurrentPageIndex = 1;
		}

		private void pager_PageChanged(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
		{
			this.pager.CurrentPageIndex = e.NewPageIndex;

			BindData(e.NewPageIndex,false);
		}


		private void Clear()
		{
			this.lb_c1.Text = "";
			this.lb_c2.Text = "";
			this.lb_c3.Text = "";
			this.lb_c4.Text = "";
			this.lb_c5.Text = "";
			this.lb_c6.Text = "";
			this.lb_c7.Text = "";
			this.lb_c8.Text = "";
			this.lb_c9.Text = "";
			this.lb_c10.Text = "";
			this.lb_c11.Text = "";
			this.lb_c12.Text = "";
			this.lb_c13.Text = "";
			this.lb_c14.Text = "";
			this.lb_c15.Text = "";
			this.lb_c16.Text = "";
			this.lb_c17.Text = "";
			this.lb_c18.Text = "";
			this.lb_c19.Text = "";
			this.lb_c20.Text = "";
			this.lb_c21.Text = "";
			this.lb_c22.Text = "";
			this.lb_c23.Text = "";
			this.lb_c24.Text = "";
			

			Label1.Text = "";
			Label2.Text = "";
			Label3.Text = "";
			Label4.Text = "";
			Label5.Text = "";
			Label6.Text = "";
			Label7.Text = "";
			Label8.Text = "";
			Label9.Text = "";
			Label10.Text = "";

			Label11.Text = "";
			Label12.Text = "";
			Label13.Text = "";
			Label14.Text = "";
			Label15.Text = "";
			Label16.Text = "";
			Label17.Text = "";
			Label18.Text = "";
			Label19.Text = "";
			Label20.Text = "";

			Label21.Text = "";
			Label22.Text = "";
			Label23.Text = "";
			Label24.Text = "";
			Label25.Text = "";
			Label26.Text = "";
			Label27.Text = "";
			Label28.Text = "";
			Label29.Text = "";
			Label30.Text = "";

			this.DataGrid_QueryResult.DataSource = null;
			this.DataGrid_QueryResult.DataBind();
		}



		private DataSet QueryDKContractDetail(string cep_cnr)
		{
			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
			qs.Finance_HeaderValue = classLibrary.setConfig.setFH(this);

			return qs.QueryDKContractDetail(cep_cnr);
		}


		private void DataGrid_QueryResult_ItemCommand(object source, DataGridCommandEventArgs e)
		{
			if(e.CommandName == "detail")
			{
				DataSet ds = QueryDKContractDetail(e.Item.Cells[0].Text.Trim());

				if(ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
				{
					WebUtils.ShowMessage(this,"查询结果为空");
					Clear();
					return;
				}

				DataRow dr = ds.Tables[0].Rows[0];

				lb_c1.Text = dr["Fspid"].ToString();
				lb_c2.Text = dr["Fcep_cnr"].ToString();
				lb_c3.Text = dr["Fmer_cnr"].ToString();
				lb_c4.Text = dr["Frelate_cnr"].ToString();
				
				lb_c5.Text = dr["Fmer_aid"].ToString();
				if(dr["Fbankacc_attr"].ToString() == "0")
					lb_c6.Text = "公司";
				else
					lb_c6.Text = "个人";
				lb_c7.Text = dr["Funame"].ToString();
				lb_c8.Text = dr["Fclient_alias"].ToString();

				lb_c9.Text = dr["Fbank_name"].ToString();
				lb_c10.Text = getData.GetBankNameFromBankCode(dr["Fbank_type"].ToString());
                lb_c11.Text = dr["Fbankacc_tail"].ToString();
				string tmp = dr["Fbankacc_type"].ToString();
				if(tmp == "1")
					lb_c12.Text = "存折";
				else if(tmp == "2")
					lb_c12.Text = "信用卡";
				else 
					lb_c12.Text = "银行卡";

				lb_c13.Text = dr["Fbank_area"].ToString();
				lb_c14.Text = dr["Fbank_city"].ToString();
				lb_c15.Text = getData.GetCreNameFromCreCode(dr["Fcredit_type"].ToString());
				lb_c16.Text = dr["Fcredit_id"].ToString();

				lb_c17.Text = dr["Faddress"].ToString();
				lb_c18.Text = dr["Fpost_code"].ToString();
				lb_c19.Text = dr["Fphone"].ToString();
				lb_c20.Text = dr["Fmobile"].ToString();

				lb_c21.Text = dr["FstatusName"].ToString();
				tmp = dr["Flstate"].ToString();
				if(tmp == "1")
					lb_c22.Text = "有效";
				else if(tmp == "2")
					lb_c22.Text = "冻结";
				else if(tmp == "3")
					lb_c22.Text = "解除协议";
				else if(tmp == "4")
					lb_c22.Text = "作废";
				lb_c23.Text = setConfig.FenToYuan(dr["Fautopay_amount"].ToString());
                lb_c24.Text = Transfer.convertMoney_type(dr["Fautopay_curtype"].ToString());

				tmp = dr["Fautopay_type"].ToString();
				if(tmp == "1")
					Label1.Text = "每月固定日期";
				else if(tmp == "2")
					Label1.Text = "小时";
				else if(tmp == "3")
					Label1.Text = "日";
				else if(tmp == "4")
					Label1.Text = "周";
				else if(tmp == "5")
					Label1.Text = "月";
				else if(tmp == "6")
					Label1.Text = "季";
				else if(tmp == "7")
					Label1.Text = "年";
				Label2.Text = dr["Fautopay_period"].ToString();

                //lxl 20140714 转义，有文档
                tmp = dr["Fceppay_mode"].ToString();
                switch (tmp)
                {
                    case "1": Label3.Text="余额"; break;
                    case "2": Label3.Text = "一点通"; break;
                    case "4": Label3.Text = "借记卡/存折直扣"; break;
                    case "8": Label3.Text = "信用卡直扣"; break;
                    case "10": Label3.Text = "保留（原意义快捷代扣B2C/快速交易）"; break;
                    case "20": Label3.Text = "充值"; break;
                    default: Label3.Text = tmp; break;
                }

                tmp = dr["Fsubmit_channel"].ToString();
                int channel=int.Parse(tmp);
                string submit_channel = "";
                if ((channel & 0x01)!=0)
                    submit_channel += "允许商户主站发起 ";
                if ((channel & 0x02) != 0)
                    submit_channel += "允许商户直连接口发起 ";
                if ((channel & 0x04) != 0)
                    submit_channel += "允许客户短信发起 ";
                if ((channel & 0x08) != 0)
                    submit_channel += "允许商户短信发起 ";
                if ((channel & 0x10) != 0)
                    submit_channel += "允许客户IVR发起 ";
                if ((channel & 0x20) != 0)
                    submit_channel += "允许商户IVR发起 ";
                if ((channel & 0x40) != 0)
                    submit_channel += "允许客户手机WAP/客户端发起 ";
                if ((channel & 0x80) != 0)
                    submit_channel += "允许商户手机WAP/客户端发起 ";
                if ((channel & 0x100) != 0)
                    submit_channel += "允许客户主站/小钱包发起 ";
                if ((channel & 0x200) != 0)
                    submit_channel += "允许客户专有终端发起 ";
                if ((channel & 0x400) != 0)
                    submit_channel += "允许商户专有终端发起 ";
                if ((channel & 0x800) != 0)
                    submit_channel += "允许行业专用平台发起 ";
                if ((channel & 0x1000) != 0)
                    submit_channel += "允许手机刷卡渠道发起 ";
                if ((channel & 0x2000) != 0)
                    submit_channel += "保留（商户系统发起渠道使用） ";
                Label4.Text = submit_channel;

				Label5.Text = dr["Fvalid_count"].ToString();

                tmp = dr["Fsub_state"].ToString();
                switch (tmp)
                {
                    case "99": Label6.Text = "已生效"; break;
                    case "100": Label6.Text = "初始化"; break;
                    case "101": Label6.Text = "付款中"; break;
                    case "102": Label6.Text = "付款回导结果成功"; break;
                    case "103": Label6.Text = "短信已下发"; break;
                    case "104": Label6.Text = "用户确认金额失败"; break;
                    case "105": Label6.Text = "金额确认成功实名认证中"; break;
                    case "106": Label6.Text = "验金额失败"; break;
                    case "107": Label6.Text = "签约失败"; break;
                    case "108": Label6.Text = "协议成功"; break;
                    case "109": Label6.Text = "打款失败"; break;
                    case "110": Label6.Text = "资料验证失败"; break;
                    case "300": Label6.Text = "短信签约初始化"; break;
                    case "301": Label6.Text = "资料验证失败（包括无鉴权渠道）"; break;
                    case "302": Label6.Text = "签约短信已下发"; break;
                    case "303": Label6.Text = "短信回复确认签约"; break;
                    case "304": Label6.Text = "短信回复拒绝签约:签约失败，无法修改"; break;
                    default: Label6.Text = tmp; break;
                }

                tmp = dr["Fsubmit_type"].ToString();
                switch (tmp)
                {
                    case "1": Label7.Text = "用户触发"; break;
                    case "2": Label7.Text = "商户触发"; break;
                    case "3": Label7.Text = "自动周期性扣款"; break;
                    default: Label7.Text = tmp; break;
                }

                tmp = dr["Fsubmit_type"].ToString();
                switch (tmp)
                {
                    case "1": Label8.Text = "短信提醒"; break;
                    case "2": Label8.Text = "邮件提醒"; break;
                    case "3": Label8.Text = "QQtips提醒"; break;
                    default: Label8.Text = tmp; break;
                }
				//Label8.Text = dr["Fcnr_alert"].ToString();

                tmp = dr["Fdirect"].ToString();
                switch (tmp)
                {
                    case "1": Label9.Text = "出"; break;
                    case "2": Label9.Text = "入"; break;
                    case "3": Label9.Text = "出入"; break;
                    default: Label9.Text = tmp; break;
                }
				//Label9.Text = dr["Fdirect"].ToString();

                tmp = dr["Fservice_type"].ToString();
                switch (tmp)
                {
                    case "0": Label10.Text = "对公"; break;
                    case "1": Label10.Text = "对私"; break;
                    case "2": Label10.Text = "对公对私"; break;
                    default: Label10.Text = tmp; break;
                }
				//Label10.Text = dr["Fservice_type"].ToString();

				Label11.Text = dr["Fservice_code"].ToString();

                tmp = dr["Fcontract_mode"].ToString();
                switch (tmp)
                {
                    case "1": Label12.Text = "四方两两－商户客户线下协议"; break;
                    case "2": Label12.Text = "客户财付通银行三方协议――线上"; break;
                    case "4": Label12.Text = "客户商户财付通三方"; break;
                    default: Label12.Text = tmp; break;
                }
				//Label12.Text = dr["Fcontract_mode"].ToString();

				Label13.Text = dr["Fcreate_time"].ToString();
				Label14.Text = dr["Fmodify_time"].ToString();
				Label15.Text = dr["Fvalid_time"].ToString();
				Label16.Text = dr["Fstime"].ToString();

				Label17.Text = dr["Fmemo"].ToString();
				Label18.Text = dr["Fetime"].ToString();
				Label19.Text = dr["Fdesc"].ToString();
				Label20.Text = dr["Fis_user"].ToString();

				Label21.Text = dr["Fuin"].ToString();
				Label22.Text = dr["Fuid"].ToString();
				Label23.Text = dr["Flast_retcode"].ToString();
				Label24.Text = dr["Fclient_ip"].ToString();

				Label25.Text = dr["Flast_retinfo"].ToString();
				Label26.Text = dr["Fmodify_ip"].ToString();
				Label27.Text = dr["Fbatchid"].ToString();
				Label27.NavigateUrl = "./QueryDKcontractListPage.aspx?spid=" + dr["Fspid"].ToString() + "&spbatchid=" + dr["Fsp_batchid"].ToString() +"&sDate=" + dr["Fcreate_time"] + "&eDate=" + dr["Fmodify_time"];
				Label28.Text = dr["Fbatch_fname"].ToString();

				Label29.Text = dr["Fsp_batchid"].ToString();
				Label29.NavigateUrl = "./QueryDKcontractListPage.aspx?spid=" + dr["Fspid"].ToString() + "&spbatchid=" + dr["Fsp_batchid"].ToString() +  "&sDate=" + dr["Fcreate_time"] + "&eDate=" + dr["Fmodify_time"];
				Label30.Text = dr["Fcontract_path"].ToString();
			}
		}
	}
}
