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
using CFT.CSOMS.BLL.PNRModule;

namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
	/// <summary>
	/// PayBusinessQuery 的摘要说明。
	/// </summary>
    public partial class PNROrderQuery : TENCENT.OSS.CFT.KF.KF_Web.PageBase
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				Label1.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();
				//int operid = Int32.Parse(Session["OperID""].ToString());

				//if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID,"InfoCenter")) Response.Redirect("../login.aspx?wh=1");

                if (!classLibrary.ClassLib.ValidateRight("SPInfoManagement", this)) Response.Redirect("../login.aspx?wh=1");

				if(!IsPostBack)
				{
					this.divInfo.Visible = false;
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
		//	this.dgList.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.dgList_ItemCommand);
        //    this.pager.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(this.ChangePage);

		}
		

        public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
       //     pager.CurrentPageIndex = e.NewPageIndex;
            BindData(e.NewPageIndex);
        }
        #endregion

        private void ValidateDate()
        {
            string pnr = txtPNR.Text.ToString();
            string payFlowCode = txtPayflowcode.Text.ToString();

            if (string.IsNullOrEmpty(pnr.Trim()) && string.IsNullOrEmpty(payFlowCode.Trim()))
            {
                throw new Exception("查询条件不能为空！");
            }

        }
        
        protected void btnSearch_Click(object sender, System.EventArgs e)
		{

			this.divInfo.Visible = false;

            try
            {
                ValidateDate();
            }
            catch (Exception err)
            {
                WebUtils.ShowMessage(this.Page, err.Message);
                return;
            }

                BindData(1);
		}

        private void BindData(int index)
		{
            try
            {
                DataTable dt= new PNRService().QueryPNROrder(txtPNR.Text.ToString().Trim(), txtPayflowcode.Text.ToString().Trim());
                if (dt != null && dt.Rows.Count > 0)
                {
                    this.divInfo.Visible = true;
                    clear();
                    DataRow dr = dt.Rows[0];
                    this.lbFagent.Text=dr["Fagent"].ToString();
                    this.lbFpnr.Text=dr["Fpnr"].ToString();
                    this.lbFairUserID.Text=dr["FairUserID"].ToString();
                    this.lbFbillTime.Text=dr["billTime"].ToString();
                    this.lbFbillStatus.Text = dr["FbillStatus_str"].ToString();
                    this.lbFairflowcode.Text=dr["Fairflowcode"].ToString();
                    this.lbFairInTime.Text=dr["airInTime"].ToString();
                    this.lbFairOutTime.Text=dr["airOutTime"].ToString();
                    this.lbForiFee.Text=dr["ForiFee"].ToString();
                    this.lbFtotalFee.Text=dr["FtotalFee"].ToString();
                    this.lbFagentFee.Text=dr["FagentFee"].ToString();
                    this.lbFpayflowcode.Text=dr["Fpayflowcode"].ToString();
                    this.lbFpayTime.Text=dr["payTime"].ToString();
                    this.lbFagentRate.Text=dr["FagentRate"].ToString();
                    this.lbFairportFee.Text=dr["FairportFee"].ToString();
                    this.lbFticketCnt.Text=dr["FticketCnt"].ToString();
                    this.lbFsegmCnt.Text=dr["FsegmCnt"].ToString();
                    this.lbFpassCnt.Text=dr["FpassCnt"].ToString();
                    this.lbFpayurl.Text=dr["Fpayurl"].ToString();
                    this.lbFtickets.Text=dr["Ftickets"].ToString();
                    this.lbFadultPrice.Text=dr["FadultPrice"].ToString();
                    this.lbFplatformid.Text=dr["Fplatformid"].ToString();
                    this.lbFbuyid.Text=dr["Fbuyid"].ToString();
                    this.lbFoperator.Text=dr["Foperator"].ToString();
                    this.lbFbillmark.Text=dr["Fbillmark"].ToString();
                    this.lbFoilFee.Text=dr["FoilFee"].ToString();
                    this.lbFadultNum.Text=dr["FadultNum"].ToString();
                    this.lbFchildNum.Text=dr["FchildNum"].ToString();
                    this.lbFinfantNum.Text=dr["FinfantNum"].ToString();
                    this.lbFstandby1.Text=dr["Fstandby1"].ToString();
                    this.lbFstandby2.Text=dr["Fstandby2"].ToString();
                    this.lbFstandby3.Text=dr["Fstandby3"].ToString();
                    this.lbFstandby4.Text=dr["Fstandby4"].ToString();
                    this.lbFstandby5.Text=dr["Fstandby5"].ToString();
                }
                else
                {
                    this.divInfo.Visible = false;
                    WebUtils.ShowMessage(this.Page, "没有找到记录！");
                }
            }
            catch (Exception ex)
            {
                this.divInfo.Visible = false;
                string errStr = PublicRes.GetErrorMsg(ex.Message);
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + errStr);
            }
		}

        private void clear()
        {
            this.lbFagent.Text = "";
            this.lbFpnr.Text = "";
            this.lbFairUserID.Text = "";
            this.lbFbillTime.Text = "";
            this.lbFbillStatus.Text = "";
            this.lbFairflowcode.Text = "";
            this.lbFairInTime.Text = "";
            this.lbFairOutTime.Text = "";
            this.lbForiFee.Text = "";
            this.lbFtotalFee.Text = "";
            this.lbFagentFee.Text = "";
            this.lbFpayflowcode.Text = "";
            this.lbFpayTime.Text = "";
            this.lbFagentRate.Text = "";
            this.lbFairportFee.Text = "";
            this.lbFticketCnt.Text = "";
            this.lbFsegmCnt.Text = "";
            this.lbFpassCnt.Text = "";
            this.lbFpayurl.Text = "";
            this.lbFtickets.Text = "";
            this.lbFadultPrice.Text = "";
            this.lbFplatformid.Text = "";
            this.lbFbuyid.Text = "";
            this.lbFoperator.Text = "";
            this.lbFbillmark.Text = "";
            this.lbFoilFee.Text = "";
            this.lbFadultNum.Text = "";
            this.lbFchildNum.Text = "";
            this.lbFinfantNum.Text = "";
            this.lbFstandby1.Text = "";
            this.lbFstandby2.Text = "";
            this.lbFstandby3.Text = "";
            this.lbFstandby4.Text = "";
            this.lbFstandby5.Text = "";
        }
	}
}
