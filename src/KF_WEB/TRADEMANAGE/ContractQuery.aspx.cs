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
    public partial class ContractQuery : TENCENT.OSS.CFT.KF.KF_Web.PageBase
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				Label1.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();

				if(!classLibrary.ClassLib.ValidateRight("InfoCenter",this)) Response.Redirect("../login.aspx?wh=1");


				if(!IsPostBack)
				{
                    TextBoxStartCreatedTime.Text = DateTime.Now.AddDays(-60).ToString("yyyy-MM-dd");
                    TextBoxEndCreatedTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
                    TextBoxStartArchiveDay.Text = "";
                    TextBoxEndArchiveDay.Text = "";
                    TextBoxStartBeginDate.Text = "";
                    TextBoxEndBeginDate.Text = "";
                    TextBoxStartEndDate.Text = "";
                    TextBoxEndEndDate.Text = "";

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
			this.dgList.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.dgList_ItemCommand);
            this.pager.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(this.ChangePage);

		}
		

        public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            pager.CurrentPageIndex = e.NewPageIndex;
            BindData(e.NewPageIndex);
        }
        #endregion

        private void ValidateDate()
        {
           ViewState["vendorName"] = txtVendorName.Text.ToString();
            ViewState["customerName"]  = txtCustomerName.Text.ToString();
            ViewState["contractNo"] = txtContractNo.Text.ToString();

            string begin = TextBoxStartCreatedTime.Text.Trim();
            string end = TextBoxEndCreatedTime.Text.Trim();
            HandleTime(begin, end, "startCreatedTime", "endCreatedTime", false);

            begin = TextBoxStartArchiveDay.Text.Trim();
            end = TextBoxEndArchiveDay.Text.Trim();
            HandleTime(begin, end,  "startArchiveDay",  "endArchiveDay", true);

            begin = TextBoxStartBeginDate.Text.Trim();
            end = TextBoxEndBeginDate.Text.Trim();
            HandleTime(begin, end,  "startBeginDate",  "endBeginDate", true);
            begin = TextBoxStartEndDate.Text.Trim();
            end = TextBoxEndEndDate.Text.Trim();
            HandleTime(begin, end,  "startEndDate", "endEndDate", true);
        }

        protected void HandleTime(string begin,string end, string viewTimeNameStart, string viewTimeNameEnd,bool canEmpty)
        {
            if ((string.IsNullOrEmpty(begin) || string.IsNullOrEmpty(end)) && canEmpty)
            {
                 ViewState[viewTimeNameStart] = "";
                 ViewState[viewTimeNameEnd] = "";
            }
            else
            {
                DateTime begindate = new DateTime();
                DateTime enddate = new DateTime();
                try
                {
                    begindate = DateTime.Parse(begin);
                    enddate = DateTime.Parse(end);
                }
                catch
                {
                    throw new Exception("日期输入有误！");
                }

                if (begindate.CompareTo(enddate) > 0)
                {
                    throw new Exception("终止日期小于起始日期，请重新输入！");
                }

                if (!classLibrary.getData.IsTestMode)
                    if (begindate.AddDays(365) < enddate)
                    {
                        throw new Exception("日期间隔大于365天，请重新输入！");
                    }

                ViewState[viewTimeNameStart] = begindate.ToString("yyyy-MM-dd 00:00:00");
                ViewState[viewTimeNameEnd] = enddate.ToString("yyyy-MM-dd 23:59:59");
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

			try
			{
                this.pager.RecordCount = 1000;
				dgList.CurrentPageIndex = 0;
                BindData(1);
			}
			catch(Exception eSys)
			{
				this.dgList.Visible = false;
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + PublicRes.GetErrorMsg(eSys.Message));
			}
		}

        private void BindData(int index)
		{
            this.pager.CurrentPageIndex = index;
            int max = pager.PageSize;
            int start = max * (index - 1);

            DataSet ds = new MerchantService().QueryContract(ViewState["vendorName"].ToString(), ViewState["customerName"].ToString(), ViewState["contractNo"].ToString(),
                                    ViewState["startCreatedTime"].ToString(), ViewState["endCreatedTime"].ToString(),
                                    ViewState["startArchiveDay"].ToString(), ViewState["endArchiveDay"].ToString(),
                                    ViewState["startBeginDate"].ToString(), ViewState["endBeginDate"].ToString(),
                                    ViewState["startEndDate"].ToString(), ViewState["endEndDate"].ToString(),
                                     start,  max);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                this.dgList.Visible = true;
                ViewState["g_dt"] = ds.Tables[0];
                dgList.DataSource = ds.Tables[0].DefaultView;
                dgList.DataBind();
            }
            else
            {
                dgList.DataSource = null;
                throw new LogicException("没有找到记录！");
            }
		}

		private void dgList_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
		{
			if(e.CommandName == "Select")
			{
				try
				{
                    DataTable dt = (DataTable)ViewState["g_dt"];
                    int rid = e.Item.ItemIndex;
                    if(dt.Rows.Count >0)
					{
						this.divInfo.Visible = true;
                        DataRow row = dt.Rows[rid];

                        this.lbCONTRACTID.Text = row["CONTRACTID"].ToString();
                        try
                        {
                            this.lbContractState.Text = new MerchantService().GetContractState(row["CONTRACTID"].ToString().Trim());
                        }catch{
                            this.lbContractState.Text="";
                        }

                        this.lbORGFULLNAME.Text = row["ORGFULLNAME"].ToString();
                        this.lbVENDORNAME.Text = row["VENDORNAME"].ToString();
                        this.lbCUSTOMERNAME.Text = row["CUSTOMERNAME"].ToString();
                        this.lbDEPTNAME.Text = row["DEPTNAME"].ToString();
                        this.lbCATEGORYNAME.Text = row["CATEGORYNAME"].ToString();
                        this.lbFULLPATHCATEGORYNAME.Text = row["FULLPATHCATEGORYNAME"].ToString();
                        this.lbISIMPORTANT.Text = getMarkString(row["ISIMPORTANT"].ToString());
                        this.lbISPURCHASE.Text = getMarkString(row["ISPURCHASE"].ToString());
                        this.lbCONTRACTNO.Text = row["CONTRACTNO"].ToString();
                        this.lbISWARRANT.Text = getMarkString(row["ISWARRANT"].ToString());
                        this.lbISAREA.Text = getMarkString(row["ISAREA"].ToString());
                        this.lbAREANAME.Text = row["AREANAME"].ToString();
                        this.lbSTAFFNAME.Text = row["STAFFNAME"].ToString();
                        this.lbWRITERNAME.Text = row["WRITERNAME"].ToString();
                        this.lbISDATEPROMISE.Text = getMarkString(row["ISDATEPROMISE"].ToString());
                        this.lbISSTANDARD.Text = getMarkString(row["ISSTANDARD"].ToString());
                        this.lbSTARTDATE.Text = row["STARTDATE"].ToString();
                        this.lbENDDATE.Text = row["ENDDATE"].ToString();
                        this.lbCONTENT.Text = row["CONTENT"].ToString();
                        this.lbBALANCETERM.Text = row["BALANCETERM"].ToString();
                        this.lbSETTLEMODENAME.Text = row["SETTLEMODENAME"].ToString();
                        this.lbSETTLEMODERATIO.Text = row["SETTLEMODERATIO"].ToString();
                        this.lbTOTALPRICE.Text = classLibrary.setConfig.FenToYuan(row["TOTALPRICE"].ToString().Replace(".0",""));
                        this.lbCURRENCYNAME.Text = row["CURRENCYNAME"].ToString();
                        this.lbCURRENCYCODE.Text = row["CURRENCYCODE"].ToString();
                        this.lbCURRENCYRATIO.Text = row["CURRENCYRATIO"].ToString();
                        this.lbTOTALAMOUNT.Text = classLibrary.setConfig.FenToYuan(row["TOTALAMOUNT"].ToString().Replace(".0", ""));
                        this.lbSTATE.Text = row["STATE"].ToString();
                        this.lbFOLDERNO.Text = row["FOLDERNO"].ToString();
                        this.lbARCHIVEDAY.Text = row["ARCHIVEDAY"].ToString();
                        this.lbCREATEDTIME.Text = row["CREATEDTIME"].ToString();
                        this.lbISSTRUCTURE.Text = getMarkString(row["ISSTRUCTURE"].ToString());
                        this.lbBUNAME.Text = row["BUNAME"].ToString();
                        this.lbISCLOSEDPAYMENT.Text = getMarkString(row["ISCLOSEDPAYMENT"].ToString());
					}
					else
					{
						this.divInfo.Visible = false;
						WebUtils.ShowMessage(this.Page,"读取数据失败！");
					}
				}
				catch(Exception ex)
				{
					this.divInfo.Visible = false;
					WebUtils.ShowMessage(this.Page,"读取数据失败！"+ex.Message);
				}
			}
		}

        protected string getMarkString(string tmp)
        {
            string str = "";
            if (tmp == "0.0")
                str = "否";
            else
                str = "是";
            return str;

        }

        protected void clear()
        {
            this.lbORGFULLNAME.Text = "";
            this.lbVENDORNAME.Text = "";
            this.lbCUSTOMERNAME.Text = "";
            this.lbDEPTNAME.Text = "";
            this.lbCATEGORYNAME.Text = "";
            this.lbFULLPATHCATEGORYNAME.Text = "";
            this.lbISIMPORTANT.Text = "";
            this.lbISPURCHASE.Text = "";
            this.lbCONTRACTNO.Text = "";
            this.lbISWARRANT.Text = "";
            this.lbISAREA.Text = "";
            this.lbAREANAME.Text = "";
            this.lbSTAFFNAME.Text = "";
            this.lbWRITERNAME.Text = "";
            this.lbISDATEPROMISE.Text = "";
            this.lbISSTANDARD.Text = "";
            this.lbSTARTDATE.Text = "";
            this.lbENDDATE.Text = "";
            this.lbCONTENT.Text = "";
            this.lbBALANCETERM.Text = "";
            this.lbSETTLEMODENAME.Text = "";
            this.lbSETTLEMODERATIO.Text = "";
            this.lbTOTALPRICE.Text = "";
            this.lbCURRENCYNAME.Text = "";
            this.lbCURRENCYCODE.Text = "";
            this.lbCURRENCYRATIO.Text = "";
            this.lbTOTALAMOUNT.Text = "";
            this.lbSTATE.Text = "";
            this.lbFOLDERNO.Text = "";
            this.lbARCHIVEDAY.Text = "";
            this.lbCREATEDTIME.Text = "";
            this.lbISSTRUCTURE.Text = "";
            this.lbBUNAME.Text = "";
            this.lbISCLOSEDPAYMENT.Text = "";

        }
	
	}
}
