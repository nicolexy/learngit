using System;
using System.Data;
using System.Web.Services.Protocols;
using Tencent.DotNet.Common.UI;
using TENCENT.OSS.CFT.KF.Common;
using System.Drawing;
using CFT.CSOMS.BLL.TradeModule;

namespace TENCENT.OSS.CFT.KF.KF_Web.SpSettle
{
	/// <summary>
    /// SpControlQuery 的摘要说明。
	/// </summary>
    public partial class SpControlQuery : System.Web.UI.Page
	{
    
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// 在此处放置用户代码以初始化页面
            try
            {
                labUid.Text = Session["uid"].ToString();
                string szkey = Session["SzKey"].ToString();
                int operid = Int32.Parse(Session["OperID"].ToString());

				if(!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter",this)) Response.Redirect("../login.aspx?wh=1");
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

        protected void Button_qry_Click(object sender, System.EventArgs e)
        {
            try
            {
                string spid = this.txtSpid.Text.Trim();
                if (spid == "")
                    throw new LogicException("商户号不能为空！");

                BindInfo(spid);
            }
            catch(LogicException err)
            {
                WebUtils.ShowMessage(this.Page,err.Message);
            }
            catch(SoapException eSoap) //捕获soap类异常
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page,"调用服务出错：" + errStr);
            }
            catch(Exception eSys)
            {
                WebUtils.ShowMessage(this.Page,"读取数据失败！" + eSys.Message.ToString());
            }
        }

        private string getControllMethod(int type) 
        {
            string ret = "";
            switch (type) 
            {
                case 1:
                    ret = "分账中";
                    break;
                case 2:
                    ret = "分账中";
                    break;
                case 3:
                    ret = "分账中";
                    break;
                case 4:
                    ret = "分账中";
                    break;
                default:
                    ret = string.Format("未知：{0}", type);
                    break;
            }
            return ret;
        }
        
        private void BindInfo(string spid)
        {
            //Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            //DataSet ds;
            //ds = qs.QuerySpControl(spid);
            SettleService service = new SettleService();
            DataTable dt = service.QuerySpControl(spid);
            if(dt != null&&  dt.Rows.Count > 0 )
            {
                DataRow dr = dt.Rows[0];

                lbSpid.Text = PublicRes.GetString(dr["Fspid"]);

                lbControll.Text = getControllMethod(int.Parse(dr["Fcontroll_method"].ToString()));
                lbRule.Text = PublicRes.GetString(dr["Fcontroll_close_type"]);
                lbArgs.Text = PublicRes.GetString(dr["Fcontroll_args"]);
                lbOperator.Text = PublicRes.GetString(dr["Foperator"]);
                lbExplain.Text = PublicRes.GetString(dr["Fexplain"]);
                lbCreateTime.Text = PublicRes.GetString(dr["Fcreate_time"]);
                lbModifyTime.Text = PublicRes.GetString(dr["Fmodify_time"]);

                //商户权限
                string spc = PublicRes.GetString(dr["Frule"]);
                if (spc != "" && spc.Length == 64) 
                {
                    string h_rule = spc.Substring(0, 32); //高位
                    string l_rule = spc.Substring(32);  //低位
                    

                    //先初始化颜色
                    lb_c1.ForeColor = Color.Blue;
                    lb_c2.ForeColor = Color.Blue;
                    lb_c3.ForeColor = Color.Blue;
                    lb_c4.ForeColor = Color.Blue;
                    lb_c5.ForeColor = Color.Blue;
                    lb_c6.ForeColor = Color.Blue;
                    lb_c7.ForeColor = Color.Blue;
                    lb_c8.ForeColor = Color.Blue;
                    lb_c9.ForeColor = Color.Blue;

                    string s = l_rule.Substring(31, 1);
                    lb_c1.Text = (s == "1") ? "开通" : "关闭";
                    if (s == "0") {lb_c1.ForeColor = Color.Gray;}
                    s = l_rule.Substring(30, 1);
                    lb_c2.Text = (s == "1") ? "开通" : "关闭";
                    if (s == "0") { lb_c2.ForeColor = Color.Gray; }
                    s = l_rule.Substring(29, 1);
                    lb_c3.Text = (s == "1") ? "开通" : "关闭";
                    if (s == "0") { lb_c3.ForeColor = Color.Gray; }
                    s = l_rule.Substring(28, 1);
                    lb_c4.Text = (s == "1") ? "开通" : "关闭";
                    if (s == "0") { lb_c4.ForeColor = Color.Gray; }
                    s = l_rule.Substring(27, 1);
                    lb_c5.Text = (s == "1") ? "开通" : "关闭";
                    if (s == "0") { lb_c5.ForeColor = Color.Gray; }
                    s = l_rule.Substring(26, 1);
                    lb_c6.Text = (s == "1") ? "开通" : "关闭";
                    if (s == "0") { lb_c6.ForeColor = Color.Gray; }
                    s = l_rule.Substring(25, 1);
                    lb_c7.Text = (s == "1") ? "开通" : "关闭";
                    if (s == "0") { lb_c7.ForeColor = Color.Gray; }
                    s = l_rule.Substring(24, 1);
                    lb_c8.Text = (s == "1") ? "开通" : "关闭";
                    if (s == "0") { lb_c8.ForeColor = Color.Gray; }
                    s = l_rule.Substring(23, 1);
                    lb_c9.Text = (s == "1") ? "开通" : "关闭";
                    if (s == "0") { lb_c9.ForeColor = Color.Gray; }
                }
                
            }
            else
            {
                throw new LogicException("没有找到记录！");
            }
        }
	}
}
