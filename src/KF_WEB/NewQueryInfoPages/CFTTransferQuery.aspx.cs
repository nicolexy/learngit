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
using Tencent.DotNet.Common.UI;
using TENCENT.OSS.C2C.Finance.BankLib;
using System.IO;
using System.Text;
using System.Web.Services.Protocols;

namespace TENCENT.OSS.CFT.KF.KF_Web.NewQueryInfoPages
{
    public partial class CFTTransferQuery : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.pager.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(pager_PageChanged);
            this.DataGrid_QueryResult.ItemCommand += new DataGridCommandEventHandler(DataGrid_QueryResult_ItemCommand);

            if (!IsPostBack)
            {
                this.tbx_beginDate.Value = DateTime.Now.AddDays(-6).ToString("yyyy-MM-dd HH:mm:ss");
                this.tbx_endDate.Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                this.pager.PageSize = 10;
                this.pager.RecordCount = GetCount();
            }
               
        }

     
        private void ValidateDate()
        {

            DateTime sTime, eTime;
            string strSTime, strETime;
            try
            {
                sTime = DateTime.Parse(this.tbx_beginDate.Value);
                eTime = DateTime.Parse(this.tbx_endDate.Value);

                strSTime = sTime.ToString("yyyy-MM-dd HH:mm:ss");
                strETime = eTime.ToString("yyyy-MM-dd HH:mm:ss");
                ViewState["strSTime"] = strSTime;
                ViewState["strETime"] = strETime;
            }
            catch
            {
                throw new Exception("日期格式不正确！");
            }

        }

        protected void btn_serach_Click(object sender, System.EventArgs e)
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

                if (this.tbx_trBillID.Text != "")
                {
                    this.collect.Visible = false;
                    this.Clear();
                    this.DataGrid_QueryResult.DataSource = null;
                    this.DataGrid_QueryResult.DataBind();
                    BindTransferDetail("", "", this.tbx_trBillID.Text);//通过转账单号单独查询详情
                }
                else
                {
                    if (this.tbx_spid.Text == "")
                    {
                        WebUtils.ShowMessage(this, "请输入商户号");
                        return;
                    }
                    else
                    {
                        BindData(1);
                        this.pager.CurrentPageIndex = 1;
                    }
                }
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + eSys.Message.ToString());
            }
        }

        //绑定汇总列表
        private void BindData(int pageIndex)
        {
            Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

            //qs.Finance_HeaderValue = setConfig.setFH(Session["OperID"].ToString(),Request.UserHostAddress);

            qs.Finance_HeaderValue = classLibrary.setConfig.setFH(this);

            try
            {
               

                DataSet ds = qs.QueryCFTTransferInfo(this.tbx_spid.Text,ViewState["strSTime"].ToString(),  ViewState["strETime"].ToString(),
                    this.tbx_trBatchID.Text,  this.ddl_state.SelectedValue,
                   (pageIndex - 1) * this.pager.PageSize, this.pager.PageSize);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    WebUtils.ShowMessage(this, "查询结果为空");
                    this.collect.Visible = false;
                    this.Clear();
                    this.DataGrid_QueryResult.DataSource = null;
                    this.DataGrid_QueryResult.DataBind();
                    return;
                }

                ds.Tables[0].Columns.Add("Fsucpay_amountName", typeof(string));
                ds.Tables[0].Columns.Add("Fpre_mountName", typeof(string));

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    dr["Fsucpay_amountName"] = setConfig.FenToYuan(dr["Fsucpay_fee"].ToString());
                    dr["Fpre_mountName"] = setConfig.FenToYuan(dr["Fpre_fee"].ToString());
                }

                double sTotalMoney = 0, fTotalMoney = 0, hTotalMoney = 0;
                long sTotalNums = 0, fTotalNums = 0, hTotalNums = 0;
                //需求查询出全部结果
                DataSet dsStatistics = qs.QueryCFTTransferInfo(this.tbx_spid.Text,ViewState["strSTime"].ToString(),  ViewState["strETime"].ToString(),
                    this.tbx_trBatchID.Text, this.ddl_state.SelectedValue, -1, -1);
                if (dsStatistics != null && dsStatistics.Tables.Count != 0 && dsStatistics.Tables[0].Rows.Count != 0)
                {
                    foreach (DataRow dr in dsStatistics.Tables[0].Rows)
                    {
                        sTotalNums += long.Parse(dr["Fsucpay_num"].ToString());
                        fTotalNums += long.Parse(dr["Ffailpay_num"].ToString());
                        hTotalNums += long.Parse(dr["FHandling_num"].ToString());

                        sTotalMoney += double.Parse(dr["Fsucpay_fee"].ToString());
                        fTotalMoney += double.Parse(dr["Ffailpay_fee"].ToString());
                        hTotalMoney += double.Parse(dr["FHandling_fee"].ToString());
                    }

                    this.lb_failAllMoney.Text = setConfig.FenToYuan(fTotalMoney.ToString());
                    this.lb_failNum.Text = fTotalNums.ToString();

                    this.lb_successAllMoney.Text = setConfig.FenToYuan(sTotalMoney.ToString());
                    this.lb_successNum.Text = sTotalNums.ToString();

                    this.lb_handlingAllMoney.Text = setConfig.FenToYuan(hTotalMoney.ToString());
                    this.lb_handlingNum.Text = hTotalNums.ToString();
                }
                else
                {
                    WebUtils.ShowMessage(this, "统计失败");
                    //return;
                }

                this.collect.Visible = true;
                this.PanelInfo.Visible = false;
                this.DataGrid_QueryResult.DataSource = ds;
                this.DataGrid_QueryResult.DataBind();
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

        private void pager_PageChanged(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            this.pager.CurrentPageIndex = e.NewPageIndex;

            BindData(e.NewPageIndex);
        }

        //查询转账详情
        private void BindTransferDetail(string spid, string ubatch_id, string trBillID)
        {
            try{
            DataSet ds = GetTransferDetail(spid, ubatch_id, trBillID);//通过商户号、批次号查询详情

            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                WebUtils.ShowMessage(this, "查询结果为空");
                Clear();
                this.PanelInfo.Visible = false;
                return;
            }

            DataRow dr = ds.Tables[0].Rows[0];

            //自动跳转到直付商户查询结果中
            this.lb_c1.Text = "<a href=../BaseAccount/PayBusinessQuery.aspx?spid=" + dr["Fspid"].ToString() + " target=_blank>" + dr["Fspid"].ToString() + "</a>";
            //this.lb_c1.Text = dr["Fspid"].ToString();
            this.lb_c2.Text = setConfig.FenToYuan(dr["Famount"].ToString());
            this.lb_c3.Text = dr["Fbatch_id"].ToString();
            this.lb_c4.Text = dr["Ftransaction_id"].ToString();
            this.lb_c5.Text = dr["Frecv_uin"].ToString();
            this.lb_c6.Text = dr["Frecv_true_name"].ToString();

            this.lb_c7.Text = dr["FstatusName"].ToString();
            this.lb_c8.Text = dr["Fresult_info"].ToString();
            this.lb_c9.Text = dr["Fcreate_time"].ToString();
            this.lb_c10.Text = dr["Fmodify_time"].ToString();
            this.lb_c11.Text = dr["Fdesc"].ToString();
            this.lb_c12.Text = dr["Fauto_id"].ToString();
            this.PanelInfo.Visible = true;
            }
            catch (Exception eSys)
            {
                throw new Exception(eSys.Message);
            }
        }
        
        private DataSet GetTransferDetail(string spid, string ubatch_id, string trBillID)
        {
            try
            {
            Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

            //qs.Finance_HeaderValue = setConfig.setFH(Session["OperID"].ToString(),Request.UserHostAddress);

            qs.Finance_HeaderValue = classLibrary.setConfig.setFH(this);

            return qs.QueryTransferDetail(spid, ubatch_id, trBillID, ViewState["strSTime"].ToString(),  ViewState["strETime"].ToString());
            }
            catch (SoapException eSoap) //捕获soap类异常
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "调用服务出错：" + errStr);
                return null;
            }
            catch (Exception eSys)
            {
                throw new Exception(eSys.Message);
            }
        }

        //通过汇总列表点击查询详情列查询详情
        private void DataGrid_QueryResult_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "detail")
                    BindTransferDetail(e.Item.Cells[0].Text.Trim(), e.Item.Cells[1].Text.Trim(), "");//通过商户号、批次号查询详情
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + eSys.Message.ToString());
            }
        }

        private int GetCount()
        {
            return 1000;
        }

        private void Clear()
        {
            this.lb_c1.Text = "";
            this.lb_c2.Text = "";
            this.lb_c3.Text = "";
            this.lb_c4.Text = "";
            this.lb_c5.Text = "";
            this.lb_c6.Text = "";
            this.lb_c7.Text = "";
            this.lb_c8.Text = "";
            this.lb_c9.Text = "";
            this.lb_c10.Text = "";
            this.lb_c11.Text = "";
            this.lb_c12.Text = "";
        }
    
    }
}