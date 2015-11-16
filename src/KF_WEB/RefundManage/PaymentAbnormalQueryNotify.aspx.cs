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
using TENCENT.OSS.CFT.KF.DataAccess;
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;
using CFT.CSOMS.BLL.RefundModule;
using System.Collections.Generic;

namespace TENCENT.OSS.CFT.KF.KF_Web.RefundManage
{
    public partial class PaymentAbnormalQueryNotify : System.Web.UI.Page
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
          RefundService refundService=  new RefundService();
          
			
			try
			{
				Label1.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();
				int operid = Int32.Parse(Session["OperID"].ToString());

				if(!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter",this)) Response.Redirect("../login.aspx?wh=1");
			}
			catch
			{
				Response.Redirect("../login.aspx?wh=1");
			}

			if(!IsPostBack)
			{
				TextBoxDate.Text = DateTime.Now.ToString("yyyy年MM月dd日");

                PublicRes.GetDropdownlist(RefundService.SubTypePay, ddlSubTypePay);
                PublicRes.GetDropdownlist(RefundService.typeht, ddltype);
                PublicRes.GetDropdownlist(RefundService.notifyStatusht, ddlNotityStatus);
                PublicRes.GetDropdownlist(RefundService.notifyResultht, ddlNotityResult);
                PublicRes.GetDropdownlist(RefundService.accht, ddlAccType);
                PublicRes.GetDropdownlist(RefundService.errht, ddlErrorType);

                //银行下拉列表
                setConfig.GetAllBankListFromDic(ddlBankType);
                ddlBankType.Items.Insert(0, new ListItem("所有银行", ""));
				Table2.Visible = false;				
			}

            ButtonBeginDate.Attributes.Add("onclick", "openModeBegin()"); 
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

		private void ValidateDate()
		{ 
			DateTime date;

			try
			{
				date = DateTime.Parse(TextBoxDate.Text);
			}
			catch
			{
				throw new Exception("日期输入有误！");
			}
            if (string.IsNullOrEmpty(tbBatchID.Text.Trim()))
            {
                throw new Exception("批次不能为空！");
            }
            ViewState["sTime"] = date.ToString("yyyy-MM-dd 00:00:00");
            ViewState["eTime"] = date.ToString("yyyy-MM-dd 23:59:59");
		}

        protected void ButtonQuery_Click(object sender, System.EventArgs e)
		{
			try
			{
				ValidateDate();
			}
			catch(Exception err)
			{
				WebUtils.ShowMessage(this.Page,err.Message);
				return;
			}

			try
			{
                ViewState["recordCount"] = -1;
				Table2.Visible = true;
				BindData(1);
                var count = (int)ViewState["recordCount"];
                pager.RecordCount = count;
                lb_conut.InnerText = count.ToString();
			}
			catch(Exception eSys)
            {
                string errStr = PublicRes.GetErrorMsg(eSys.Message.ToString());
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + errStr);
			}
		}

        //查询记录列表
		private void BindData(int index)
		{
            string sTime = ViewState["sTime"].ToString();
            string eTime = ViewState["eTime"].ToString();

            string batchID = tbBatchID.Text.Trim();
            string packageID = tbPackageID.Text.Trim();
            string listid = tblistid.Text.Trim();
            string type = ddltype.SelectedValue;
            string subTypePay = ddlSubTypePay.SelectedValue;
            string notityStatus = ddlNotityStatus.SelectedValue;
            string notityResult = ddlNotityResult.SelectedValue;
            string bankType =ddlBankType.SelectedValue;
            string errorType =ddlErrorType.SelectedValue;
            string accType = ddlAccType.SelectedValue;

            ChooseRadio.SelectedValue = "1";//每次查询都选定单选

            this.pager.CurrentPageIndex = index;
			int max = pager.PageSize;
			int start = max * (index-1);

            var count = (int)ViewState["recordCount"];
            DataSet ds = new RefundService().QueryPaymenAbnormal(sTime, eTime, batchID, packageID,
                listid, type, subTypePay, notityStatus, notityResult, bankType, errorType, accType, start, max , ref count);

            ViewState["recordCount"] = count;

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ds.Tables[0].Columns.Add("Fbank_type_str", typeof(String));
                classLibrary.setConfig.GetColumnValueFromDic(ds.Tables[0], "Fbank_type", "Fbank_type_str", "BANK_TYPE");
                
                SendBtnControl(false);

                DataGrid1.DataSource = ds.Tables[0].DefaultView;
                DataGrid1.DataBind();
            }
            else
            {
                SendBtnControl(true);
                DataGrid1.DataSource = null;
                DataGrid1.DataBind();
                showMsg("没有找到记录");
            }
		}

        //发送通知函数
        protected void SendNotifies(string notifyType)
        {
            try
            {
                Session["AbnorListid"] = null;
                Session["sTime"] = ViewState["sTime"].ToString();
                Session["eTime"] = ViewState["eTime"].ToString();
                Session["batchID"] = tbBatchID.Text.Trim();
                Session["packageID"] = tbPackageID.Text.Trim();
                Session["listid"] = tblistid.Text.Trim();
                Session["type"] = ddltype.SelectedValue;
                Session["subTypePay"] = ddlSubTypePay.SelectedValue;
                Session["notityStatus"] = ddlNotityStatus.SelectedValue;
                Session["notityResult"] = ddlNotityResult.SelectedValue;
                Session["bankType"] = ddlBankType.SelectedValue;
                Session["errorType"] = ddlErrorType.SelectedValue;
                Session["accType"] = ddlAccType.SelectedValue;
                string ip = Request.UserHostAddress.ToString();
                if (ip == "::1")
                    ip = "127.0.0.1";
                Session["client_ip"] = ip;

                string by = "";//查询方式

                //全选所有根据条件来查询所有记录发送通知
                //其他根据单号来发送通知
                if (this.ChooseRadio.SelectedValue == "3")//全选所有
                {
                    by = "condition";
                }
                else
                {
                    ArrayList listid = new ArrayList();
                    try
                    {
                        listid = GetCheckData();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("获取已勾选数据异常：" + ex.Message);
                    }

                    if (listid == null || listid.Count == 0)
                    {
                        BindData(pager.CurrentPageIndex);
                        throw new Exception("请先选中需要操作的数据！");
                    }
                    by = "listid";
                    Session["AbnorListid"] = listid;

                }
                SendBtnControl(true);//隐藏按钮避免二次提交

              //  Response.Write("<script language=javascript>window.showModalDialog('SendPaymentAbnorNotify.aspx?by=" + by + "&notifyType=" + notifyType + "','','dialogWidth:900px;DialogHeight=1000px;status:no');</script>");
                Response.Write("<script language=javascript>window.open('SendPaymentAbnorNotify.aspx?by=" + by + "&notifyType=" + notifyType + "');</script>");
            }                                              
            catch (Exception err)
            {
                string errStr = PublicRes.GetErrorMsg(err.Message.ToString());
                showMsg("发送通知异常：" + errStr);
            }
        }

        #region 发送通知按钮
        protected void SendWX_Click(object sender, System.EventArgs e)
        {
            SendNotifies("1");
        }

        protected void SendMES_Click(object sender, System.EventArgs e)
        {
            SendNotifies("4");
        }

        //protected void SendQQ_Click(object sender, System.EventArgs e)
        //{
        //    SendNotifies("2");
        //}
        //protected void SendEmail_Click(object sender, System.EventArgs e)
        //{
        //    SendNotifies("3");
        //}
        //protected void SendTips_Click(object sender, System.EventArgs e)
        //{
        //    SendNotifies("5");
        //}
        //protected void SendWallet_Click(object sender, System.EventArgs e)
        //{
        //    SendNotifies("6");
        //}
        #endregion

        protected void Choose_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (this.ChooseRadio.SelectedValue == "2")//全选当页
            {
                ChooseData(true);
            }
            else
            {
                ChooseData(false);
            }
           
        }

        //全选当页数据
        protected void ChooseData(bool allPage)
        {

            int count = DataGrid1.Items.Count;
            if (count <= 0)
            {
                showMsg("没有可选的数据项");
                return;
            }

            for (int i = 0; i < count; i++)
            {
                System.Web.UI.Control obj = DataGrid1.Items[i].Cells[10].FindControl("CheckBox2");
                if (obj != null && obj.Visible)
                {
                    CheckBox cb = (CheckBox)obj;
                    if (allPage)
                    {
                        cb.Checked = true;
                    }
                    else
                    {
                        cb.Checked = false;
                    }
                }
            }
        }
        
        //获取已勾选数据
        private ArrayList GetCheckData()
        {
            ArrayList listid = new ArrayList();
            try
            {
                int count = DataGrid1.Items.Count;

                for (int i = 0; i < count; i++)
                {
                    System.Web.UI.Control obj = DataGrid1.Items[i].Cells[10].FindControl("CheckBox2");
                    if (obj != null && obj.Visible)
                    {
                        CheckBox cb = (CheckBox)obj;
                        if (cb.Checked)
                        {
                            listid.Add(DataGrid1.Items[i].Cells[4].Text.Trim());
                        }
                    }
                }
                return listid;
            }
            catch (Exception ex)
            {
                throw new Exception("勾选异常："+ex.Message);
            }
        }

        private void SendBtnControl(bool hide)
        {
            string notityStatus = ddlNotityStatus.SelectedValue;
            this.btSendWX.Visible = true;
            this.btSendMES.Visible = true;
            //this.btSendQQ.Visible = true;
            //this.btSendEmail.Visible = true;
            //this.btSendTips.Visible = true;
            //this.btSendWallet.Visible = true;
            //待发送、发送中及所有状态不允许发送通知
            if (hide || string.IsNullOrEmpty(notityStatus) || notityStatus == "1" || notityStatus == "2")
            {
                this.btSendWX.Visible = false;
                this.btSendMES.Visible = false;
                //this.btSendQQ.Visible = false;
                //this.btSendEmail.Visible = false;
                //this.btSendTips.Visible = false;
                //this.btSendWallet.Visible = false;
            }
        }

        private void showMsg(string msg)
        {
            Response.Write("<script language=javascript>alert('" + msg + "')</script>");
        }
     
        public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            pager.CurrentPageIndex = e.NewPageIndex;
            BindData(e.NewPageIndex);
        }
	}
}
