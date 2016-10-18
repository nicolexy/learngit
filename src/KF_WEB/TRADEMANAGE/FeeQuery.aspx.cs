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
using System.Configuration;
using CFT.CSOMS.BLL.WechatPay;
using CFT.CSOMS.BLL.LifeFeePaymentModule;
using CFT.CSOMS.COMMLIB;

namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
	/// <summary>
	/// FeeQuery 的摘要说明。
	/// </summary>
	public partial class FeeQuery : TENCENT.OSS.CFT.KF.KF_Web.PageBase
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				Label1.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();
				int operid = Int32.Parse(Session["OperID"].ToString());

				//if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID,"InfoCenter")) Response.Redirect("../login.aspx?wh=1");
                if (!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("TradeManagement", this)) Response.Redirect("../login.aspx?wh=1");
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

		protected void btnQuery_Click(object sender, System.EventArgs e)
		{
			labErrMsg.Text = "";
			
			if(this.tblistID.Text=="")
			{
				WebUtils.ShowMessage(this.Page,"请先输入交易单号!");
				return ;
			}
			try
			{
				BindData(1);
			}
			catch(Exception ex)
			{
				string msg=ex.Message.Replace("\'","\\\'");
				WebUtils.ShowMessage(this.Page,msg);
			}
		}

		private void BindData(int index)
		{
            try
            {
                string ftype = "9";// ddlFtype.SelectedValue;
                string state = "9";// ddlState.SelectedValue;
                string paystate = "9";// ddlPaytypeState.SelectedValue;

                string listID = tblistID.Text.Trim();
                ViewState["ListID"] = listID;
                Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

                DataSet ds = qs.QueryFeeDataList(ftype, state, paystate, listID, 0, 1000);

                if (ds != null && ds.Tables.Count > 0)
                {
                    LifeFeePaymentService liftFee = new LifeFeePaymentService();
                    ds.Tables[0].Columns.Add("FrefundNumName", typeof(String));
                    classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "FrefundNum", "FrefundNumName");

                    ds.Tables[0].Columns.Add("FnumName", typeof(String));
                    classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "Fnum", "FnumName");

                    ds.Tables[0].Columns.Add("FPaynumName", typeof(String));
                    classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "FPaynum", "FPaynumName");

                    ds.Tables[0].Columns.Add("FstateName", typeof(String));
                    ds.Tables[0].Columns.Add("FPaytypeName", typeof(String));
                    ds.Tables[0].Columns.Add("FcreateTypeName", typeof(String));
                    ds.Tables[0].Columns.Add("FtypeName", typeof(String));

                    ds.Tables[0].Columns.Add("account", typeof(String));//echo 20141022
                    ds.Tables[0].Columns.Add("StatusStr", typeof(String));//交易单状态
                    ds.Tables[0].Columns.Add("UserId", typeof(String));//缴费账号
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        dr.BeginEdit();

                        string tmp = dr["Fstate"].ToString();
                        if (tmp == "0")
                            tmp = "初始状态";
                        else if (tmp == "1")
                            tmp = "缴费成功";
                        else if (tmp == "2")
                            tmp = "退款成功";
                        else
                            tmp = "退单状态未定";
                        dr["FstateName"] = tmp;

                        tmp = dr["FPaytype"].ToString();
                        if (tmp == "0")
                            tmp = "退款";
                        else if (tmp == "1")
                            tmp = "打款";
                        dr["FPaytypeName"] = tmp;

                        tmp = dr["FcreateType"].ToString();
                        if (tmp == "1")
                            tmp = "来自对账文件";
                        else if (tmp == "2")
                            tmp = "超时插入";
                        dr["FcreateTypeName"] = tmp;

                        tmp = dr["Ftype"].ToString();
                        if (tmp == "water")
                            tmp = "水费";
                        else if (tmp == "elec")
                            tmp = "电费";
                        else if (tmp == "gas")
                            tmp = "气费";
                        else if (tmp == "tel")
                            tmp = "电话费";
                        else if (tmp == "outtime")
                            tmp = "超时退款";
                        dr["FtypeName"] = tmp;

                        //根据交易单号查询买家账户号码
                        string account = GetBuyAccount();
                       //测试用
                       // account = "100000000";
                        if (!string.IsNullOrEmpty(account.Trim()) && account.Trim() != "未知")
                        {
                            dr["account"] = account;
                            DataSet dsLifeFee = liftFee.QueryChargeBill(account, dr["flistid"].ToString());
                            if (dsLifeFee != null && dsLifeFee.Tables.Count > 0 && dsLifeFee.Tables[0].Rows.Count > 0)
                            {
                                dsLifeFee.Tables[0].Columns.Add("FStatusStr", typeof(String));
                                Hashtable ht1 = new Hashtable();
                                ht1.Add("0", "查询欠费成功");
                                ht1.Add("1", "生成交易单成功");
                                ht1.Add("2", "支付成功");
                                ht1.Add("3", "销帐中(通知商户销帐前设置)");
                                ht1.Add("4", "销帐成功");
                                ht1.Add("5", "销帐失败");
                                ht1.Add("6", "退款成功");
                                ht1.Add("7", "交易关闭");
                                CommUtil.DbtypeToPageContent(dsLifeFee.Tables[0], "FStatus", "FStatusStr", ht1);
                                dr["StatusStr"] = dsLifeFee.Tables[0].Rows[0]["FStatusStr"].ToString();
                                dr["UserId"] = dsLifeFee.Tables[0].Rows[0]["FUserId"].ToString();
                            }
                        }
                        else
                        {
                            dr["account"] = account;
                            dr["StatusStr"] = "";
                            dr["UserId"] = "";
                        }


                        dr.EndEdit();
                    }

                    DataGrid1.DataSource = ds.Tables[0].DefaultView;
                    DataGrid1.DataBind();

                }
                else
                {
                    WebUtils.ShowMessage(this.Page, "没有找到记录");
                }
            }
            catch (Exception eSys)
            {
                string err = PublicRes.GetErrorMsg(eSys.Message.ToString());
                WebUtils.ShowMessage(this.Page, "查询异常！" +err);
            }
		}


        private string GetBuyAccount()//参考TradeLogQuery的 买家账户号码
        {
            string acc = "";
            try
            {
                //绑定交易资料信息
                Query_Service.Query_Service myService = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

                myService.Finance_HeaderValue = classLibrary.setConfig.setFH(this);

                string selectStrSession = ViewState["ListID"].ToString();

                DateTime beginTime = DateTime.Parse(ConfigurationManager.AppSettings["sBeginTime"].ToString());

                DateTime endTime = DateTime.Parse(ConfigurationManager.AppSettings["sEndTime"].ToString());

                int istr = 1;

                int imax = 2;

                DataSet ds = new DataSet();
                // iType = 4 根据交易单号查询
                ds = myService.GetPayList(selectStrSession, 4, beginTime, endTime, istr, imax);

                if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                {
                    return acc = "未知";
                }

                //DataTable wx_dt = null;
                //try
                //{
                //    //担心接口还未上线，导致客服现有功能使用不了，暂时这样处理
                //    wx_dt = new WechatPayService().QueryWxTrans(ds.Tables[0].Rows[0]["Flistid"].ToString()); //查询微信转账业务
                //}
                //catch
                //{
                //}

                //if (wx_dt != null && wx_dt.Rows.Count > 0)
                //{
                //    //通过卖家交易单反查付款方
                //    acc = PublicRes.objectToString(wx_dt, "pay_openid");
                //}
                //else
                //{
                    acc = ds.Tables[0].Rows[0]["Fbuyid"].ToString();
                //}
                return acc;
            }
            catch { return acc; }
        }


	}
}
