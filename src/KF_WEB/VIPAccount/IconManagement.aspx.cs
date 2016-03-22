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
using System.Web.Services.Protocols;

namespace TENCENT.OSS.CFT.KF.KF_Web.VIPAccount
{
	/// <summary>
	/// Summary description for IconManagement.
	/// </summary>
	public partial class IconManagement : System.Web.UI.Page
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    

		}
		#endregion

        protected void btnSearch_Click(object sender, System.EventArgs e)
        {

            if (this.tbx_acc.Text.Trim() != "")
            {
                BindData();
            }
            else
            {
                WebUtils.ShowMessage(this, "请输入账号");
            }
        }

        private void BindData()
        {
            try
            {
                Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
                DataSet ds = null;//qs.QueryIconInfo(this.tbx_acc.Text.Trim());

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    this.tbmemberGrade.Text = "";
                    this.tbiconGrade.Text = "";
                    this.Button_Flash.Visible = false;
                    this.Button_extinguish.Visible = false;
                    throw new Exception("没有查找到相应的记录！");
                }
                else
                {
                    DataTable dt = ds.Tables[0];
                    string vipflag = ds.Tables[0].Rows[0]["Fvipflag"].ToString();
                    int value = int.Parse(ds.Tables[0].Rows[0]["Fvalue"].ToString());
                    string memberGrade = "", iconGrade = "";//会员等级，图标等级
                    if (-2147483648 <= value && value <= 199)
                    {
                        memberGrade = "0";
                        iconGrade = "0";
                    }
                    if (200 <= value && value <= 1499)
                    {
                        memberGrade = "1";
                        iconGrade = "1";
                    }
                    if (1500 <= value && value <= 6999)
                    {
                        memberGrade = "2";
                        iconGrade = "2";
                    }
                    if (7000 <= value && value <= 17999)
                    {
                        memberGrade = "3";
                        iconGrade = "3";
                    }
                    if (18000 <= value && value <= 2147483640)
                    {
                        memberGrade = "4";
                        iconGrade = "4";
                    }
                    if (2147483641 <= value && value <= 2147483642)
                    {
                        memberGrade = "5";
                        iconGrade = "5";
                    }
                    if (2147483643 <= value && value <= 2147483647)
                    {
                        memberGrade = "6";
                        iconGrade = "6";
                    }

                    //0和4图标是灭的同时等级无意义
                    if (vipflag == "0" || vipflag == "4")
                    {
                        memberGrade = "无";
                        iconGrade = "无";
                    }

                    this.Button_Flash.Visible = true;
                    this.Button_extinguish.Visible = true;
                    this.tbmemberGrade.Text = memberGrade;
                    this.tbiconGrade.Text = iconGrade;
                }
            }
            catch (SoapException eSoap) //捕获soap类异常
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "调用服务出错：" + errStr);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + eSys.Message.ToString());
            }
        }

        protected void lit_Click(object sender, System.EventArgs e)
        {
            try
            {
                //Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
                //qs.RefreshIcon(this.tbx_acc.Text.Trim());
                WebUtils.ShowMessage(this.Page, "成功刷新图标");
            }
            catch (SoapException eSoap) //捕获soap类异常
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "调用服务出错：" + errStr);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + eSys.Message.ToString());
            }
        }

        protected void extinguish_Click(object sender, System.EventArgs e)
        {
            try
            {
                //Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
                //qs.Finance_HeaderValue = classLibrary.setConfig.setFH(this);
                //qs.ExtinguishIcon(this.tbx_acc.Text.Trim());
                WebUtils.ShowMessage(this.Page, "成功熄灭图标");
            }
            catch (SoapException eSoap) //捕获soap类异常
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "调用服务出错：" + errStr);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + eSys.Message.ToString());
            }

        }
    }
}
