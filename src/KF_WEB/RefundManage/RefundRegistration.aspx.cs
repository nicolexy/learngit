using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using CFT.CSOMS.BLL.RefundModule;
using Tencent.DotNet.Common.UI;
using System.IO;
using TENCENT.OSS.C2C.Finance.Common.CommLib;
using CFT.CSOMS.BLL.TransferMeaning;
namespace TENCENT.OSS.CFT.KF.KF_Web.RefundManage
{
    public partial class RefundRegistration : System.Web.UI.Page
    {

        private int m_nState = 1;
        Dictionary<int, string> dic_m_nCheckState = new Dictionary<int, string>()
        {
            { -2 , "挂起"},
            { 0 , "等待客服处理"},
            { 1 , "已通知用户"},
            { 2 , "收集用户卡号，等待客服审批"},
            { 3 , "等待BG审核"},
            { 4 , "等待风控审核"},
            { 5 , "等待财务处理"},
            { 6 , "财务处理完成"},
            { 7 , "无法处理,转向财务"},
        };

        private string[] m_nAryRefundType = 
        {
            	"商户退单",
				"对帐结果退单",
				"人工录入退单",
				"对帐异常退单",
				"赔付退单",
                "未知类型",
                "未知类型",
                "未知类型",
				"充值单退款",
                "未知类型",
				"拍拍退单",
				"提现退款异常单",
                "准实时退款异常数据",
        };

        private RefundPublicFun.itemData[] aryItem = null;

        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                this.operaterName.Text = Session["uid"].ToString();
                if (!IsPostBack)
                {
                    if (Session["uid"].ToString() == RefundPublicFun.OPERATOR.Trim())
                    {
                        SetRighBtnState(true);
                    }
                    else
                    {
                        SetRighBtnState(false);
                        log4net.ILog log = log4net.LogManager.GetLogger("RefundRegistration");
                        log.InfoFormat("uid={0},oper={1}", Session["uid"].ToString(), RefundPublicFun.OPERATOR.Trim());
                    }
                }
            }
            catch
            {
                Response.Redirect("../login.aspx?wh=1");
            }

        }


        private void BindData(bool bState = false)
        {
            string strBeginDate = null;
            string strEndDate   = null;
            if (!string.IsNullOrEmpty(textBoxBeginDate.Text))
            {
                DateTime begindate = DateTime.Parse(textBoxBeginDate.Text.Trim());
                strBeginDate = begindate.ToString("yyyy-MM-dd 00:00:00");
            }           
            if (!string.IsNullOrEmpty(textBoxEndDate.Text))
            {
                DateTime enddate = DateTime.Parse(textBoxEndDate.Text.Trim());
                strEndDate = enddate.ToString("yyyy-MM-dd 23:59:59");
            }


            string strUid = txtUinID.Text.Trim();
            string strBank = txtBankID.Text.Trim();
            string strFSPID = txtSPID.Text.Trim();
            string strOldID = txtOldID.Text.Trim();
            string strOperater = txtOperator.Text.Trim();
           // int xxx = int.Parse(txtMin.Text.Trim()) * 100;
           // int yyyy = int.Parse(txtMax.Text.Trim()) * 100;
            string strMin = "0";
            string strMax = "0";
            if (!string.IsNullOrEmpty(txtMin.Text.Trim()))
            {
                strMin = string.Format("{0}", int.Parse(txtMin.Text.Trim()) * 100);
            }
            else
            {
                strMin = null;
            }

            if (!string.IsNullOrEmpty(txtMax.Text.Trim()))
            {
                strMax = string.Format("{0}", int.Parse(txtMax.Text.Trim()) * 100);
            }
            else
            {
                strMax = null;
            }
            
            
            int iCheck = int.Parse(checkStateListID.SelectedItem.Value);
            //tradeListID.SelectedValue == tradeListID.SelectedItem.Value
            int iTrade = int.Parse(tradeListID.SelectedItem.Value);
            int payScene = int.Parse(ddDoneID.SelectedValue);
            int payChannel = int.Parse(ddPayID.SelectedValue);

            if (string.IsNullOrEmpty(strUid)        &&
                string.IsNullOrEmpty(strBank)       &&
                string.IsNullOrEmpty(strFSPID)      &&
                string.IsNullOrEmpty(strBeginDate)  &&
                string.IsNullOrEmpty(strEndDate)    &&

                string.IsNullOrEmpty(strOldID) &&
                string.IsNullOrEmpty(strOperater) &&
                string.IsNullOrEmpty(strMin) &&
                string.IsNullOrEmpty(strMax) &&
                iCheck == -1 && iTrade == 0 )
            {
                WebUtils.ShowMessage(this, "查询条件为空，请输入任意查询条件!");
                return;
            }

            DataSet ds = new RefundService().RequestRefundData(strUid, strBank, strFSPID, strBeginDate, strEndDate, iCheck, iTrade, strOldID, strOperater, strMin, strMax, payScene,payChannel);
            lb_RefundCountAll.Text="0";
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ViewState["CacheData"] = ds;
                ds.Tables[0].Columns.Add("FstateEx", typeof(System.String));
                ds.Tables[0].Columns.Add("FbankTypeName", typeof(System.String));
                ds.Tables[0].Columns.Add("FAmtEx", typeof(System.String));  //交易金额转元
                ds.Tables[0].Columns.Add("FReturnAmtEx", typeof(System.String));//退款金额转元
                ds.Tables[0].Columns.Add("TotalDB_Fstate_str", typeof(System.String));
                ds.Tables[0].Columns.Add("FrefundType_str", typeof(System.String));
                var dic = new Dictionary<int, string>()
                {
                    { 0, "初始状态" },
                    { 1, "退单流程中" },
                    { 2, "退单成功" },
                    { 3, "退单失败" },
                    { 4, "退单状态未定" },
                    { 5, "手工退单中" },
                    { 6, "申请手工处理" },
                    { 7, "申请转入代发" },
                    { 8, "挂异常处理中" },
                };

                var dicDataSource = new Dictionary<Int16, string>()
                {
                    { 1 ,"商户退单"},
                    { 2 ,"对帐结果退单"},
                    { 3 ,"手工退单"},
                    { 4 ,"对帐异常退单"},
                    { 5 ,"赔付退单"},
                    { 9 ,"充值单退款"},
                    { 11 ,"拍拍退单"}
                };
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    dr["FstateEx"] = dic_m_nCheckState[int.Parse(dr["Fstate"].ToString())];
                    if (string.IsNullOrEmpty(dr["FBuyBanktype"].ToString()))
                    {
                        dr["FbankTypeName"] = "未知银行类型";
                    }
                    else
                    {
                        dr["FbankTypeName"] = Transfer.convertbankType(dr["FBuyBanktype"].ToString());
                    }
                    var TotalDB_Fstate = (int)dr["TotalDB_Fstate"];
                    dr["TotalDB_Fstate_str"] = dic.ContainsKey(TotalDB_Fstate) ? dic[TotalDB_Fstate] : "其他";
                    var FrefundType = (Int16)dr["FrefundType"];
                    dr["FrefundType_str"] = dicDataSource.ContainsKey(FrefundType) ? dicDataSource[FrefundType] : "其他";

                    dr["FAmtEx"] = RefundPublicFun.FenToYuan(dr["FAmt"].ToString());
                    dr["FReturnAmtEx"] = RefundPublicFun.FenToYuan(dr["FReturnAmt"].ToString());
                }
                lb_RefundCountAll.Text = ds.Tables[0].Rows.Count.ToString();
                pager.RecordCount = ds.Tables[0].Rows.Count;
                if (bState)
                {
                    StringWriter sw = new StringWriter();
                    string excelHeader = gridInfor.Columns[0].HeaderText;
                    for (int i = 1; i < gridInfor.Columns.Count - 2; i++)
                    {
                        excelHeader += "\t" + gridInfor.Columns[i].HeaderText;
                    }
                    excelHeader += "\t商户号\t买家财付通账号\t手机号";
                    sw.WriteLine(excelHeader);

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        sw.WriteLine(
                            "=\"" + dr["FpayListid"].ToString() + 
                            //"\"\t=\"" + dr["FReturnstate"].ToString() + 
                            "\"\t=\"" + dr["FAmtEx"] + 
                            "\"\t" + dr["FReturnAmtEx"] + 
                            "\t=\"" + dr["FcreateTime"] + 
                            "\"\t=\"" + dr["FUserEmail"] + 
                            "\"\t=\"" + dr["FbankListid"] + 
                            "\"\t=\"" + dr["FstateEx"] +
                            "\"\t=\"" + dr["TotalDB_FCreateMemo"] +
                            "\"\t=\"" + dr["TotalDB_Fstate_str"] +
                            "\"\t=\"" + dr["FbankTypeName"] +
                            "\"\t=\"" + dr["FSPID"] +
                            "\"\t=\"" + dr["fbuyid"] +
                            "\"\t=\"" + dr["FUserTEL"] + 
                            "\""
                            );

                    }
                    sw.Close();
                    string strFile = string.Format("异常退款{0}", DateTime.Now.ToString("yyyyMMddHHmmss"));
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + strFile+".xls");
                    Response.ContentType = "application/ms-excel";
                    Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
                    Response.Write(sw);
                    Response.End();
                }
                else
                {
                    ViewState["DsCached"] = ds.Tables[0];
                    gridInfor.DataSource = ds.Tables[0].DefaultView;
                    gridInfor.DataBind();
                }

            }
            else
            {

                    WebUtils.ShowMessage(this, "没有查到匹配的数据。");
                    gridInfor.DataSource = null;
                    gridInfor.DataBind();


            }     
        }
        //查询
        protected void OnBtnCheck_Click(object sender, EventArgs e)
        {
            BindData();
           
        }
        //客服补填资料
        protected void OnBtnInputInfor_Click(object sender, EventArgs e)
        {
            string strUinID = null;
            string strPayListid = null;
            string strRefundType = null;
            string strOldId = null;
            string strCreateTime = null;
            string strBankType = null;
            for (int i = 0; i < gridInfor.Rows.Count; i++)
            {
                CheckBox ckBox = (CheckBox)gridInfor.Rows[i].FindControl("checkbox");
                int nState = int.Parse(gridInfor.DataKeys[i].Values["Fstate"].ToString());
                if (nState >= 2 && nState != -2)
                {
                    //已发启审批的单，不能直接转财务处理
                    continue;
                }
                if (ckBox.Checked)
                {
                    if (i != 0)
                    {
                        strUinID += "|";
                        strOldId += "|";
                        strPayListid += "|";
                        strRefundType += "|";
                        strCreateTime += "|";
                        strBankType += "|";
                    }     
                    var FpayListid=gridInfor.DataKeys[i].Values["FpayListid"].ToString();
                    strUinID += FpayListid;

                    if (!string.IsNullOrEmpty(gridInfor.DataKeys[i].Values["FrefundType"].ToString()))
                    {
                        int nIndex = int.Parse(gridInfor.DataKeys[i].Values["FrefundType"].ToString()) - 1;
                        if (nIndex >= 0 && nIndex < m_nAryRefundType.Length)
                        {
                            strRefundType += m_nAryRefundType[nIndex];
                        }
                        else
                        {
                            strRefundType += "未知类型;";
                        }
                    }
                    else
                    {
                        strRefundType += "未知类型;";
                    }

                    strPayListid += gridInfor.DataKeys[i].Values["FbankListid"].ToString();
                    strOldId += gridInfor.DataKeys[i].Values["FoldId"].ToString();
                    strCreateTime += gridInfor.DataKeys[i].Values["FcreateTime"].ToString();
                    var dt = ViewState["DsCached"] as DataTable;
                    strBankType += dt.Select("FpayListid = '" + FpayListid + "'")[0]["Fbuybanktype"];
                }
            }
            Response.Write("<script>window.open('AddInformation.aspx?uinID=" + strUinID + "&refundType=" + strRefundType + "&bankListId=" + strPayListid + "&oldId=" + strOldId + "&time=" + strCreateTime + "&bankType=" + strBankType + "','_blank')</script>");
            return;


        }
        //收件人管理
        protected void OnBtnReceiveManager_Click(object sender, EventArgs e)
        {
            //Response.Redirect("BankEmailManage.aspx");
            Response.Write("<script>window.open('BankEmailManage.aspx" + "','_blank')</script>");
        }
        //导出execl
        protected void OnBtnExecl_Click(object sender, EventArgs e)
        {
            BindData(true);
        }

        //通知
        protected void OnBtnClickNotice_Click(object sender, System.EventArgs e)
        {
           
            string strParam = ((Button)sender).CommandArgument.ToString();
            string[] arrParam = strParam.Split(',');
            Response.Write("<script>window.open('NoticeUserByEmail.aspx?truename=" + arrParam[0]+"&paybanklist="+arrParam[1] +"&oldid="+arrParam[2]+ "','_blank')</script>");

        }
        //通知银行
        protected void OnBtnClickBank_Click(object sender, System.EventArgs e)
        {
            string strParam = ((Button)sender).CommandArgument.ToString();       
            string[] arrParam = strParam.Split(',');
            
            if (arrParam.Length < 0 || arrParam.Length > 9)
            {
                WebUtils.ShowMessage(this.Page, "通知银行详情取参长度不对！");
                return;
            }

            string strNewBank = Transfer.convertbankType(arrParam[0]);
            Response.Write("<script>window.open('NoticeBankByEmail.aspx?newBankName=" + strNewBank + "&newBankAccNo=" + arrParam[1] + "&trueName=" + arrParam[2] + "&returnDate=" + arrParam[3]
                + "&bankType=" + arrParam[4] + "&createTime=" + arrParam[5] + "&bankListID=" + arrParam[6] + "&returnAmt=" + arrParam[7] + "&amt=" + arrParam[8] + "','_blank')</script>");
        }
        //详情
        protected void OnLinkBtnDetail_Click(object sender, System.EventArgs e)
        {
            string strParam = ((LinkButton)sender).CommandArgument.ToString();       
            string[] arrParam = strParam.Split(',');
            if (arrParam.Length < 1)
            {
                //WebUtils
                WebUtils.ShowMessage(this.Page, "特殊退款作详情取参长度不对！");
                return;
            }
         
            Response.Write("<script>window.open('RefundDetails.aspx?foldId=" + arrParam[0] + "','_blank')</script>");

        }
      

        //protected void gridInfor_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //{
        //    gridInfor.PageIndex = e.NewPageIndex;
        //    BindData();
        //}

        private void SetRighBtnState(bool state =false)
        {
            txtContext.Visible = state;
            btnText.Visible = state;         
        }
        protected void OnBtnYonghua_Click(object sender, EventArgs e)
        {
            if (Session["uid"].ToString().Trim() != RefundPublicFun.OPERATOR.Trim())
            {
                return;
            }

            new RefundService().RequestInfoChange(txtContext.Text.Trim(), Session["uid"].ToString().Trim());
        }

        protected void OnBtnChangeItem_Click(object sender, EventArgs e)
        {
            
            for (int i = 0; i < gridInfor.Rows.Count; i++)
            {
                CheckBox cb = (CheckBox)gridInfor.Rows[i].FindControl("checkbox");
                if (btnSelID.Text == "全选")
                {
                    cb.Checked = true;
                }
                else
                {
                    cb.Checked = false;
                }
              
            }
            if (btnSelID.Text == "全选")
            {
                btnSelID.Text = "取消全选";
            }
            else
            {
                btnSelID.Text = "全选";
            }
        }

        
        protected void OnBtnCaiWu_Click(object sender, EventArgs e)
        {
            string strOldID = null;
            for (int i = 0; i < gridInfor.Rows.Count; i++)
            {
                CheckBox ckBox = (CheckBox)gridInfor.Rows[i].FindControl("checkbox");
                if (ckBox.Checked)
                {
                    if (i != 0)
                    {
                        strOldID += "|";
                    }
                    strOldID += gridInfor.DataKeys[i].Values["FoldId"].ToString();
                    int nState = int.Parse(gridInfor.DataKeys[i].Values["Fstate"].ToString());
                    if (nState > m_nState)
                    {
                        //string strUinID = gridInfor.DataKeys[i].Values["FpayListid"].ToString();
                        continue;
                    }
                    try
                    {
                        new RefundService().SetRefundCheckState(7, strOldID);
                    }
                    catch (Exception ex)
                    {
                        //log.ErrorFormat("设置退款状态：操作者：{0} 退款号strOldId=：{1} 程序出错原因:{2}", Session["uid"].ToString(), strOldId, ex.Message);
                        throw new Exception(ex.Message);
                    }                    
                }
            }
            WebUtils.ShowMessage(this.Page, "转财务处理成功。");
        }

        protected void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            try
            {
                if (gridInfor.PageCount < e.NewPageIndex)
                {
                    WebUtils.ShowMessage(this.Page, "没有找到数据");
                    return;
                }
                pager.CurrentPageIndex = e.NewPageIndex;
                gridInfor.PageIndex = e.NewPageIndex - 1;
                var ds = ViewState["CacheData"] as DataSet;
                gridInfor.DataSource = ds.Tables[0];
                gridInfor.DataBind();
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this.Page, "异常:" + PublicRes.GetErrorMsg(ex.Message));
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string strOldID = null;
            for (int i = 0; i < gridInfor.Rows.Count; i++)
            {
                CheckBox ckBox = (CheckBox)gridInfor.Rows[i].FindControl("checkbox");
                if (ckBox.Checked)
                {
                    if (i != 0)
                    {
                        strOldID += "|";
                    }
                    strOldID += gridInfor.DataKeys[i].Values["FoldId"].ToString();
                    int nState = int.Parse(gridInfor.DataKeys[i].Values["Fstate"].ToString());
                    if (nState != 0)
                    {
                        WebUtils.ShowMessage(this.Page, "操作失败，请检查审批状态！");
                        return;
                    }
                    try
                    {
                        new RefundService().SetRefundCheckState(-2, strOldID);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
            }
            WebUtils.ShowMessage(this.Page, "挂起成功。");
        }
    }
}