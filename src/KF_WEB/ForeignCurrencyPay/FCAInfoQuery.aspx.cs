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
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.CFT.KF.KF_Web;
using System.Web.Services.Protocols;
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using System.Xml;
using CFT.CSOMS.BLL.ForeignCurrencyModule;
using System.Text;

namespace TENCENT.OSS.CFT.KF.KF_Web.ForeignCurrencyPay
{
	/// <summary>
    /// QueryYTTrade 的摘要说明。
	/// </summary>
    public partial class FCAInfoQuery : TENCENT.OSS.CFT.KF.KF_Web.PageBase
	{
        protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				Label1.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();

                if (!IsPostBack)
                {
                    if (!classLibrary.ClassLib.ValidateRight("InfoCenter", this)) Response.Redirect("../login.aspx?wh=1");
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
           

		}
		#endregion


		private void ValidateDate()
		{
            string spid = txtspid.Text.ToString();
            if (spid == "")
            {
                throw new Exception("请输入商户编号！");
            }
		}

        public void btnQuery_Click(object sender, System.EventArgs e)
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
            clearDT();
            GetDetail();
        }


        private void GetDetail()
        {
            string spid = txtspid.Text.ToString();
            try
            {
                clearDT();
                ForeignCurrencyService FCBLLService = new ForeignCurrencyService();
                TemplateControl temp = this;
                string ip = this.Page.Request.UserHostAddress;
                DataSet ds = FCBLLService.MerInfoQuery(spid, "", ip);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    string name = dt.Rows[0]["mer_name"].ToString(); //商户名称
                    lb_c1.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(name));
                    lb_c2.Text = dt.Rows[0]["address"].ToString();//商户地址
                    lb_c3.Text = dt.Rows[0]["sp_web"].ToString();//网站域名
                    lb_c4.Text = dt.Rows[0]["postal_code"].ToString();//邮政编号
                    lb_c5.Text = dt.Rows[0]["area"].ToString();//所属区域
                    lb_c6.Text = dt.Rows[0]["phone"].ToString();//联系电话
                    lb_c7.Text = dt.Rows[0]["email"].ToString();//Email
                    lb_c8.Text = dt.Rows[0]["mobile"].ToString();//联系手机
                    string tmp=dt.Rows[0]["mer_type"].ToString();//商户类型
                    switch (tmp)
                    {
                        case "1":
                            lb_c15.Text = "虚拟"; break;
                        case "2":
                            lb_c15.Text = " 实物"; break;
                        case "3":
                            lb_c15.Text = " 航空"; break;
                    }
                    lb_c19.Text = tmp;
                    lb_c10.Text = dt.Rows[0]["boss_name"].ToString();//法人姓名
                    tmp = dt.Rows[0]["boss_cre_type"].ToString();//法人代表证件类型
                    tmp = PublicRes.GetCreType(tmp);
                    lb_c11.Text = tmp;
                    lb_c12.Text = dt.Rows[0]["boss_cre_no"].ToString();//法人证件号码
                    lb_c13.Text = dt.Rows[0]["mer_no"].ToString();//商户营业执照编号
                    //以上是需要要的接口有的，下面的是接口返回的
                    lb_c14.Text = dt.Rows[0]["uid"].ToString();//商户内部id
                    tmp = dt.Rows[0]["inner_sp"].ToString();//是否是内部商户
                    switch (tmp)
                    {
                        case "1":
                            lb_c15.Text = "公司内部商户";break;
                        case "2":
                            lb_c15.Text = " 外部商户";break;
                    }
                    lb_c16.Text = dt.Rows[0]["company_name"].ToString();//公司名称
                    lb_c17.Text = dt.Rows[0]["country"].ToString();//国别
                    lb_c18.Text = dt.Rows[0]["city"].ToString();//城市
                    lb_c19.Text = dt.Rows[0]["memo"].ToString();//说明
                    lb_c20.Text = dt.Rows[0]["inner_memo"].ToString();//内部修改备注
                    lb_c21.Text = dt.Rows[0]["modify_user"].ToString();//操作员
                    lb_c22.Text = dt.Rows[0]["bd_user"].ToString();//商户拓展bd
                    lb_c23.Text = dt.Rows[0]["modify_ip"].ToString();//IP地址
                    lb_c24.Text = dt.Rows[0]["create_time"].ToString();//创建时间
                    lb_c25.Text = dt.Rows[0]["uid"].ToString();//修改时间
                    tmp = dt.Rows[0]["state"].ToString();//商户状态
                    switch (tmp)
                    {
                        case "1":
                            lb_c26.Text = "初始状态"; break;
                        case "2":
                            lb_c26.Text = " 注册完成"; break;
                    }
                    tmp = dt.Rows[0]["lstate"].ToString();//物理状态
                    switch (tmp)
                    {
                        case "1":
                            lb_c27.Text = "正常 "; break;
                        case "2":
                            lb_c27.Text = " 冻结"; break;
                        case "3":
                            lb_c27.Text = " 作废"; break;
                    }
                }
                else
                {
                    WebUtils.ShowMessage(this.Page, "没有查询到记录！");
                    return;
                }
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + HttpUtility.JavaScriptStringEncode(eSys.Message));
            }
        }

        private void clearDT()
        {
            lb_c1.Text = "";
            lb_c2.Text = "";
            lb_c3.Text = "";
            lb_c4.Text = "";
            lb_c5.Text = "";
            lb_c6.Text = "";
            lb_c7.Text = "";
            lb_c8.Text = "";
            lb_c9.Text = "";
            lb_c10.Text = "";

            lb_c11.Text = "";
            lb_c12.Text = "";
            lb_c13.Text = "";
            lb_c14.Text = "";
            lb_c15.Text = "";
            lb_c16.Text = "";
            lb_c17.Text = "";
            lb_c18.Text = "";
            lb_c19.Text = "";
            lb_c20.Text = "";

            lb_c21.Text = "";
            lb_c22.Text = "";
            lb_c23.Text = "";
            lb_c24.Text = "";
            lb_c25.Text = "";
            lb_c26.Text = "";
            lb_c27.Text = "";
            lb_c28.Text = "";
            lb_c29.Text = "";
            lb_c30.Text = "";


        }

  

	}
}