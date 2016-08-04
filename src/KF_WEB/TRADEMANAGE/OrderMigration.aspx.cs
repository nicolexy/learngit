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
using TENCENT.OSS.C2C.Finance.Common.CommLib;
using TENCENT.OSS.CFT.KF.KF_Web;
using TENCENT.OSS.CFT.KF.KF_Web.Check_WebService;
using System.IO;
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;


namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
	/// <summary>
	/// OrderMigration 的摘要说明。
	/// </summary>
    public class OrderMigration : TENCENT.OSS.CFT.KF.KF_Web.PageBase
	{
        protected System.Web.UI.WebControls.Label Label_uid;
        protected System.Web.UI.WebControls.TextBox txtOrderId;
        protected System.Web.UI.WebControls.Button btMigration;
        protected System.Web.UI.WebControls.Label labErrMsg;
        protected System.Web.UI.WebControls.Label lblUId;
        protected System.Web.UI.WebControls.RegularExpressionValidator rfvNum;
        protected System.Web.UI.WebControls.FileUpload File1;

		private void Page_Load(object sender, System.EventArgs e)
		{
            try
            {
                lblUId.Text = Session["uid"].ToString();
                string sr = Session["SzKey"].ToString();
                if (!ClassLib.ValidateRight("InfoCenter", this)) Response.Redirect("../login.aspx?wh=1");
            }
            catch  //如果没有登陆或者没有权限就跳出
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
            this.btMigration.Click += new System.EventHandler(this.btMigration_Click);
            this.Load += new System.EventHandler(this.Page_Load);

        }
		#endregion

        private void btMigration_Click(object sender, System.EventArgs e)
        {

            string orderId = txtOrderId.Text.Trim();
            if (!SunLibraryEX.StringEx.IsNumber(orderId) || !SunLibraryEX.StringEx.MatchLength(orderId, 0, 32))
            {
                labErrMsg.Text="订单号有误，订单号必须为小于等于32位数字！";
                WebUtils.ShowMessage(this.Page,labErrMsg.Text);
                return;
            }
            string msg = "";
            if(!MigrationCheck(orderId,out  msg))
            {
                labErrMsg.Text="提起订单迁移审批失败，失败信息如下:<br>"+msg;
                WebUtils.ShowMessage(this.Page, "提起订单迁移审批失败，，失败详细信息见页面！");
                return ;
            }
            else
            {
                labErrMsg.Text="提起订单迁移审批申请成功!";
                WebUtils.ShowMessage(this.Page,labErrMsg.Text);
							
            }
        }

        private void BatchMigrate(out string msgWrong)
        {
            msgWrong = "";
            try
            {
                string path = Server.MapPath("~/") + "PLFile" + "\\OrderMigration.xls";
                File1.PostedFile.SaveAs(path);

                DataSet res_ds = PublicRes.readXls(path, "F1");
                DataTable res_dt = res_ds.Tables[0];
                int iColums = res_dt.Columns.Count;
                int iRows = res_dt.Rows.Count;


                for (int i = 0; i < iRows; i++)
                {
                    string r1 = res_dt.Rows[i][0].ToString().Trim();//交易单号
                    try
                    {

                        if (!SunLibraryEX.StringEx.IsNumber(r1) || !SunLibraryEX.StringEx.MatchLength(r1, 0, 32))
                        {
                            msgWrong += "  " + r1 + ": 单号有误，单号必须为小于等于32位数字！" + "</br></br>";
                            continue;
                        }

                        string msgErr = "";
                        if (!MigrationCheck(r1, out  msgErr))
                        {
                            msgWrong += "  " + r1 + ": 失败信息如下:" + msgErr + "</br></br>";
                            continue;
                        }
                    }
                    catch (Exception ex)
                    {
                        msgWrong += "  " + r1 + ": 发起迁移异常:" + PublicRes.GetErrorMsg(ex.Message) + "</br></br>";
                        continue;
                    }
                }
            }
            catch (Exception eSys)
            {
                msgWrong += "批量迁移异常！" + PublicRes.GetErrorMsg(eSys.Message.ToString());
            }
        }

        public void btnBatch_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (!File1.HasFile)
                {
                    WebUtils.ShowMessage(this.Page, "请选择上传文件！");
                    return;
                }
                if (Path.GetExtension(File1.FileName).ToLower() == ".xls")
                {
                    string msgWrong = "";
                    BatchMigrate(out msgWrong);
                    if (msgWrong != "")
                    {
                        labErrMsg.Text = "批量迁移审批失败，失败信息如下:<br>" + msgWrong;
                        WebUtils.ShowMessage(this.Page, "提起订单迁移审批失败，失败信息如下：" + PublicRes.GetErrorMsg(msgWrong));
                        return;
                    }
                    else
                    {
                        labErrMsg.Text = "批量迁移审批申请成功!";
                        WebUtils.ShowMessage(this.Page, labErrMsg.Text);
                        return;
                    }
                }
                else
                {
                    WebUtils.ShowMessage(this.Page, "文件格式不正确，请选择xls格式文件上传。");
                    return;
                }
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + PublicRes.GetErrorMsg(eSys.Message.ToString())); return;
            }
        }

        //设置soap头信息
        private  ZWCheck_Service.Finance_Header SetWebServiceHeader(TemplateControl page)
        {

            ZWCheck_Service.Finance_Header header = new ZWCheck_Service.Finance_Header();
            //header.SrcUrl = page.Page.Request.Url.ToString();
            header.UserIP = page.Page.Request.UserHostAddress;
            header.UserName = (page.Page.Session["uid"] == null) ? "" : page.Page.Session["uid"].ToString();
            //header.SessionID = page.Page.Session.SessionID;
            header.SzKey = (page.Page.Session["SzKey"] == null) ? "" : page.Page.Session["SzKey"].ToString();
            header.OperID = (page.Page.Session["OperID"] == null) ? 0 : Int32.Parse(page.Page.Session["OperID"].ToString());
            header.RightString = (page.Page.Session["SzKey"] == null) ? "" : page.Page.Session["SzKey"].ToString();
            return header;
        }

        private bool MigrationCheck(string orderId,out string msg)
        {
            msg="";
            try
            {
                //调账务系统接口
                ZWCheck_Service.Check_Service checkService = new ZWCheck_Service.Check_Service();
                ZWCheck_Service.Param[] parameters = new ZWCheck_Service.Param[1];
                parameters[0] = new ZWCheck_Service.Param();
                parameters[0].ParamName = "MsgId";
                parameters[0].ParamValue = commLib.GenID.GenOrderMigrationMSGId(orderId);
                checkService.Finance_HeaderValue = SetWebServiceHeader(this);
                checkService.StartCheck(orderId, "OrderMigration", "订单迁移申请", "0", parameters);
               
                return true;
            }
            catch(Exception ex)
            {
                msg+=PublicRes.GetErrorMsg(ex.Message);
                return false;
            }

        }
	}
}
