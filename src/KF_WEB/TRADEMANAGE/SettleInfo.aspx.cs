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
using CFT.CSOMS.BLL.TradeModule;

namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
	/// <summary>
	/// SettleInfo 的摘要说明。
	/// </summary>
	public partial class SettleInfo : System.Web.UI.Page
	{
        protected System.Web.UI.WebControls.Label Label38;
        protected System.Web.UI.WebControls.Label Label48;
        protected System.Web.UI.WebControls.Label Label50;
        protected System.Web.UI.WebControls.Label Label52;
        protected System.Web.UI.WebControls.Label Label56;
        protected System.Web.UI.WebControls.Label Label58;
        protected System.Web.UI.WebControls.Label Label60;
        protected System.Web.UI.WebControls.Label Label62;
        protected System.Web.UI.WebControls.Label Label64;
    
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// 在此处放置用户代码以初始化页面
            try
            {
                labUid.Text = Session["uid"].ToString();
                string szkey = Session["SzKey"].ToString();
                int operid = Int32.Parse(Session["OperID"].ToString());

                // if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID,"InfoCenter")) Response.Redirect("../login.aspx?wh=1");
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
                if(this.txtListid.Text.Trim().Length != 28)
                    throw new Exception("请输入28位订单号！");

                BindInfo(this.txtListid.Text.Trim());
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

        private string getofferFee(string fee, int sign)
        {
            string szFee;
            if(sign == 2)
            {
                szFee = "-" + fee;
            }
            else
            {
                szFee = fee;
            }
            return szFee;
        }

        private string getOperType(int iOperType)
        {
            string szOperType;
            switch(iOperType)
            {
                case 0:
                    szOperType="未使用";
                    break;
                case 1:
                    szOperType="分账中";
                    break;
                case 2:
                    szOperType="分账退款";
                    break;
                case 3:
                    szOperType="分账回退";
                    break;
                case 4:
                    szOperType="部分退款";
                    break;
                case 5:
                    szOperType="全额退款";
                    break;
                case 6:
                    szOperType="冻结";
                    break;
                case 7:
                    szOperType="解冻";
                    break;
                case 11:
                    szOperType = "补差支付";
                    break; 
                default :
                    szOperType = string.Format("未知类型：{0}", iOperType);
                    break;
            }
            return szOperType;
        }

        private void BindInfo(string listid)
        {
            //Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            //DataSet ds;
            //ds = qs.GetSettleListAppend(listid);

            SettleService service = new SettleService();
            DataTable dt = service.GetSettleListAppend(listid);
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                //settleDetail.NavigateUrl = "SettleInfoDetail.aspx?listid=" +  PublicRes.GetString(dr["Flistid"]);

                Flistid.Text = PublicRes.GetString(dr["Flistid"]);
                //隐藏订单号 yinhuang
                string spid = dr["Flistid"].ToString().Substring(0, 10);
                if (!PublicRes.isWhiteOfSeparate(spid, Session["uid"].ToString()))
                {
                    //不在白名单
                    Fcoding.Text = classLibrary.setConfig.ConvertID(PublicRes.GetString(dr["Fcoding"]), 0, 4);
                }
                else
                {
                    Fcoding.Text = PublicRes.GetString(dr["Fcoding"]);
                }

                Fpnr.Text = PublicRes.GetString(dr["Fpnr"]);
                Fcontact.Text = PublicRes.GetString(dr["Fcontact"]);
                Fpri_spid.Text = PublicRes.GetString(dr["Fpri_spid"]);
                Fflight_info.Text = PublicRes.GetString(dr["Fflight_info"]);

                Fphone.Text = PublicRes.GetString(dr["Fphone"]);
                Fticket_num.Text = PublicRes.GetString(dr["Fticket_num"]);
                Frefund_num.Text = PublicRes.GetString(dr["Frefund_num"]);
                Ffreeze_num.Text = PublicRes.GetString(dr["Ffreeze_num"]);
                Ftotal_fee.Text = MoneyTransfer.FenToYuan(dr["Ftotal_fee"].ToString());
                Frefund_fee.Text = MoneyTransfer.FenToYuan(dr["Frefund_fee"].ToString());

                Fbus_type.Text = PublicRes.GetString(dr["Fbus_type"]);
                Fbus_args.Text = PublicRes.GetString(dr["Fbus_args"]);
                Fbus_desc.Text = PublicRes.GetString(dr["Fbus_desc"]);
                Fsp_bankurl.Text = PublicRes.GetString(dr["Fsp_bankurl"]);
                Fbus_refund_args.Text = PublicRes.GetString(dr["Fbus_refund_args"]);
                Fb2c_refund_args.Text = PublicRes.GetString(dr["Fb2c_refund_args"]);
                Fbus_freeze_args.Text = PublicRes.GetString(dr["Fbus_freeze_args"]);
                Fcreate_time.Text = PublicRes.GetString(dr["Fcreate_time"]);
                Fmodify_time.Text = PublicRes.GetString(dr["Fmodify_time"]);
                Fsttl_fee.Text = MoneyTransfer.FenToYuan(dr["Fsttl_fee"].ToString());
                Frf_fee.Text = MoneyTransfer.FenToYuan(dr["Frf_fee"].ToString());
                Ffreeze_fee.Text = MoneyTransfer.FenToYuan(dr["Ffreeze_fee"].ToString());
                Fadjustout_fee.Text = MoneyTransfer.FenToYuan(dr["Fadjustout_fee"].ToString());
                Fadjustin_fee.Text = MoneyTransfer.FenToYuan(dr["Fadjustin_fee"].ToString());
                Forder_fee.Text = getofferFee(MoneyTransfer.FenToYuan(dr["Foffer_fee"].ToString()), int.Parse(dr["Foffer_sign"].ToString()));
                Fop_type.Text = getOperType(int.Parse(dr["Fop_type"].ToString()));
                Fop_listid.Text = PublicRes.GetString(dr["Fop_listid"]);
                Fsp_pay_amount.Text = MoneyTransfer.FenToYuan(dr["Fsp_state"].ToString()); //子单分账金额
                Fsp_refund_amount.Text = MoneyTransfer.FenToYuan(dr["Fstandby2"].ToString());//子单退款金额
            }
            else
            {
                throw new LogicException("没有找到记录！");
            }
        }
	}
}
