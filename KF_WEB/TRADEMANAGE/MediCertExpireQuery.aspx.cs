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
using TENCENT.OSS.CFT.KF.DataAccess;
using System.Web.Services.Protocols;
using System.Xml.Schema;
using System.Xml;
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.CFT.KF.KF_Web;
using CFT.CSOMS.BLL.SPOA;

namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
	/// <summary>
	/// PayBusinessQuery 的摘要说明。
	/// </summary>
    public partial class MediCertExpireQuery : System.Web.UI.Page
	{
        protected MerchantService merService = new MerchantService();
		protected void Page_Load(object sender, System.EventArgs e)
		{
            ButtonBeginDate.Attributes.Add("onclick", "openModeBegin()");
            ButtonEndDate.Attributes.Add("onclick", "openModeEnd()"); 
			try
			{
				Label1.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();
				if(!classLibrary.ClassLib.ValidateRight("InfoCenter",this)) Response.Redirect("../login.aspx?wh=1");

				if(!IsPostBack)
				{
                    TextBoxBeginDate.Text = "";
                    TextBoxEndDate.Text = "";
               //     this.cmdSelectAll.Text = "全部选中";
				}
			}
			catch
			{
				Response.Redirect("../login.aspx?wh=1");
			}
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
            this.dgList.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.dgList_ItemCommand);
            this.pager.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(this.ChangePage);
         //   this.cmdSelectAll.Click += new System.EventHandler(this.cmdSelectAll_Click); 
        }

		#endregion
        public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            pager.CurrentPageIndex = e.NewPageIndex;
            try
            {
                BindData(e.NewPageIndex);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + PublicRes.GetErrorMsg(eSys.Message));
            }
        }

        private void ValidateDate()
        {
            DateTime begindate = new DateTime(), enddate = new DateTime();
            string s_date = TextBoxBeginDate.Text;
            string e_date = TextBoxEndDate.Text;
            if (string.IsNullOrEmpty(txtFspid.Text.Trim())&&string.IsNullOrEmpty(s_date)&&string.IsNullOrEmpty(e_date))
                throw new Exception("请至少输入一个查询条件！");
            if (s_date != null && s_date != "" && e_date != null && e_date != "")
            {
                try
                {
                    begindate = DateTime.Parse(s_date);
                    enddate = DateTime.Parse(e_date);
                }
                catch
                {
                    throw new Exception("日期输入有误！");
                }
                if (System.Convert.ToDateTime(begindate) > System.Convert.ToDateTime(enddate))
                {
                    throw new Exception("起始时间不能大于终止时间!");
                }
                //限制时间跨度不超过31天
                if (System.Convert.ToDateTime(enddate) >= System.Convert.ToDateTime(begindate).AddDays(31))
                {
                    throw new Exception("查询时间跨度不能超过31天!");
                }
                ViewState["begindate"] = begindate.ToString("yyyy-MM-dd");
                ViewState["enddate"] = enddate.ToString("yyyy-MM-dd");
            }
            else
            {
                ViewState["begindate"] = "";
                ViewState["enddate"] = "";
            }
           
        }
        
        protected void btnSearch_Click(object sender, System.EventArgs e)
		{
            try
            {
                ValidateDate();
            }
            catch (Exception err)
            {
                WebUtils.ShowMessage(this.Page, err.Message);
                return;
            }

			try
			{
              //  cmdSelectAll.Text = "全部选中";
				dgList.CurrentPageIndex = 0;
                this.pager.RecordCount = 1000;
             //   ViewState["dsInfo"] = null;
                BindData(1);
			}
			catch(SoapException eSoap) //捕获soap类异常
			{
				string errStr = PublicRes.GetErrorMsg(eSoap.Message);
				WebUtils.ShowMessage(this.Page,"调用服务出错：" + errStr);
			}
			catch(Exception eSys)
			{
				WebUtils.ShowMessage(this.Page,"读取数据失败！" + PublicRes.GetErrorMsg(eSys.Message));
			}
		}

        private void BindData(int index)
        {
            this.pager.CurrentPageIndex = index;
            int max = pager.PageSize;
            int start = max * (index - 1);
            string begindate = ViewState["begindate"].ToString();
            string enddate = ViewState["enddate"].ToString();
            string spid = txtFspid.Text.Trim();

            // DataSet ds = new DataSet();

            DataSet dsInfo = new SPOAService().QueryExpiredCertificate(begindate, enddate, spid, max, start);
            if (dsInfo != null && dsInfo.Tables.Count > 0 && dsInfo.Tables[0].Rows.Count > 0)
            {
                dsInfo.Tables[0].Columns.Add("FmodifyTime", typeof(String));
                dsInfo.Tables[0].Columns.Add("Fmemo", typeof(String));
                dsInfo.Tables[0].Columns.Add("FupdateUser", typeof(String));
            }
            else
            {
                this.dgList.DataSource = null;
                this.dgList.DataBind();
                throw new LogicException("没有找到记录！");
            }

            //  ViewState["dsInfo"] = dsInfo;//缓存spoa返回ds

            //    ds = GetPagedDS(index, ds, dsInfo);//分页及排序

            dsInfo = AddOperateInfo(dsInfo);
            this.dgList.DataSource = dsInfo.Tables[0].DefaultView;
            this.dgList.DataBind();
        }

        /// <summary>
        /// 对SPOA接口返回的ds每一行查询数据库中的操作信息
        /// </summary>
        /// <param name="ds">SPOA接口返回的ds</param>
        private DataSet AddOperateInfo(DataSet ds)
        {
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                string crt_etime = row["Fcrt_etime"].ToString();
                //为了让证书到期日期一致
                DateTime date = new DateTime();
                try
                {
                    date = DateTime.Parse(crt_etime);
                }
                catch
                {
                    throw new Exception("证书到期日期有误！");
                }
                crt_etime = date.ToString("yyyy-MM-dd HH:mm:ss");
                row["Fcrt_etime"] = crt_etime;

                string spid = row["Fspid"].ToString();
                DataSet dsOper = merService.QueryExpiredCertOperInfo(crt_etime, spid);
                if (dsOper != null && dsOper.Tables.Count > 0 && dsOper.Tables[0].Rows.Count > 0)
                {
                    row["FmodifyTime"] = dsOper.Tables[0].Rows[0]["FmodifyTime"].ToString();
                    row["Fmemo"] = dsOper.Tables[0].Rows[0]["Fmemo"].ToString();
                    row["FupdateUser"] = dsOper.Tables[0].Rows[0]["FupdateUser"].ToString();
                }
                else
                {
                    row["FmodifyTime"] = "";
                    row["Fmemo"] = "";
                    row["FupdateUser"] = "";
                }

            }
            return ds;
        }


        private DataSet GetPagedDS(int index, DataSet ds, DataSet dsInfo)
        {
            //排序及分页
            DataTable dt = dsInfo.Tables[0];
            DataView view = dt.DefaultView;
            view.Sort = "Fcrt_etime asc";
            dt = view.ToTable();
            dt = PublicRes.GetPagedTable(dt, index, pager.PageSize);
            ds.Tables.Add(dt);//ds为分页后的结果
            return ds;
        }

        //private void cmdSelectAll_Click(object sender, System.EventArgs e)
        //{
        //    selectAll();
        //}

        //private void selectAll()
        //{
        //    System.Web.UI.WebControls.CheckBox chkExport;
        //    if (cmdSelectAll.Text == "全部选中")
        //    {
        //        foreach (DataGridItem oDataGridItem in dgList.Items)
        //        {
        //            chkExport = (CheckBox)oDataGridItem.FindControl("chkExport");
        //            chkExport.Checked = true;
        //        }
        //        cmdSelectAll.Text = "全部不选";
        //    }
        //    else
        //    {
        //        foreach (DataGridItem oDataGridItem in dgList.Items)
        //        {
        //            chkExport = (CheckBox)oDataGridItem.FindControl("chkExport");
        //            chkExport.Checked = false;
        //        }
        //        cmdSelectAll.Text = "全部选中";
        //    }
        //} 

        //protected void btnSendEmailNotice_Click(object sender, System.EventArgs e)
        //{
        //    try
        //    {
        //        System.Web.UI.WebControls.CheckBox chkExport;
        //        string errSpid = "";
        //        foreach (DataGridItem oDataGridItem in dgList.Items)
        //        {
        //            string spid = "";
        //            chkExport = (CheckBox)oDataGridItem.FindControl("chkExport");
        //            if (chkExport.Checked)
        //            {
        //                spid = ((HtmlInputHidden)oDataGridItem.FindControl("SelectID")).Value;
        //                if (!string.IsNullOrEmpty(spid))
        //                {
        //                    SPOA_Service.SPOA_Service spoaService = new TENCENT.OSS.CFT.KF.KF_Web.SPOA_Service.SPOA_Service();
        //                    string sendXml = "<root><function>1064</function><param><sp_id>" + spid + "</sp_id>";
        //                    sendXml += "<ApplyUser>" + Session["uid"].ToString() + "</ApplyUser>";
        //                    sendXml += "<notice_type>2</notice_type>";//线上用3
        //                    sendXml += "<email_mode>1</email_mode>";
        //                    sendXml += "<cgi_ip></cgi_ip>";
        //                    sendXml += "</param></root>";
        //                    XmlDocument doc = new XmlDocument();
        //                    doc.LoadXml(sendXml);
        //                    XmlNode xmlresuts = spoaService.MerchantHelper(doc);
        //                    string ret = xmlresuts.SelectNodes("ret")[0].ChildNodes[0].InnerText;
        //                    if (ret != "0")
        //                        errSpid +=" "+spid;
        //                }
        //                else
        //                    throw new Exception("spid为空！");
        //            }
        //        }

        //        if(errSpid!=""){
        //            WebUtils.ShowMessage(this.Page, "邮件短信通知失败的商户号：" + errSpid);
        //            return;
        //        }
        //        WebUtils.ShowMessage(this.Page, "发送邮件短信通知成功！");
        //    }
        //    catch (Exception eSys)
        //    {
        //        WebUtils.ShowMessage(this.Page, "发送邮件短信通知失败！" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
        //    }
        //}

        private void dgList_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            string spid = e.Item.Cells[0].Text.Trim();
            string crt_etime = e.Item.Cells[6].Text.Trim();
            string memo = e.Item.Cells[7].Text.Trim();
            if (memo == "&nbsp;")
                memo = "";
            string modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            if (e.CommandName == "EDIT")
            {
                //Response.Redirect("MediCertExpireOperate.aspx?spid=" + spid + "&crt_etime=" + crt_etime);
                Response.Write("<script>window.open('MediCertExpireOperate.aspx?spid=" + spid + "&crt_etime=" + crt_etime + "&memo=" + memo + "','_blank')</script>");
            }
        }
	}
}
