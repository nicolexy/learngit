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
namespace TENCENT.OSS.CFT.KF.KF_Web.RefundManage
{
    public partial class RefundRegistration : System.Web.UI.Page
    {

       private  string[] m_nCheckState = 
       {
                "等待客服处理",       
                "已通知用户",
                "收集用户卡号，等待客服审批",
                "等待BG审核",
                "等待风控审核",
                "等待财务处理",
                "财务处理完成",
                "无法处理,转向财务",
            
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
        };

        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                this.operaterName.Text = Session["OperID"].ToString();
                if (!IsPostBack)
                {
 
                   // ButtonBeginDate.Attributes.Add("onclick", "openModeBegin()");
                    //ButtonEndDate.Attributes.Add("onclick", "openModeEnd()");
                    //textBoxBeginDate.Text = DateTime.Now.AddMonths(-3).ToString("yyyy年MM月dd日");
                    //textBoxEndDate.Text = DateTime.Now.ToString("yyyy年MM月dd日");
                    Button btn = (Button)form1.FindControl("btnInputID");
                    if (btn == null)
                    {
                        return;
                    }
                    btn.Enabled = false;
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
            string strFSPID = txtFSPID.Text.Trim();
            
            int iCheck = int.Parse(checkStateListID.SelectedItem.Value);
            //tradeListID.SelectedValue == tradeListID.SelectedItem.Value
            int iTrade = int.Parse(tradeListID.SelectedItem.Value);

            if (string.IsNullOrEmpty(strUid)        &&
                string.IsNullOrEmpty(strBank)       &&
                string.IsNullOrEmpty(strFSPID)      &&
                string.IsNullOrEmpty(strBeginDate)  &&
                string.IsNullOrEmpty(strEndDate)    &&
                iCheck == -1 && iTrade == 0 )
            {
                WebUtils.ShowMessage(this, "查询条件为空，请输入任意查询条件!");
                return;
            }

            DataSet ds = new RefundService().RequestRefundData(strUid, strBank, strFSPID, strBeginDate, strEndDate, iCheck, iTrade);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ds.Tables[0].Columns.Add("FstateEx", typeof(System.String));
                ds.Tables[0].Columns.Add("FbankTypeName", typeof(System.String));
                ds.Tables[0].Columns.Add("FAmtEx", typeof(System.String));  //交易金额转元
                ds.Tables[0].Columns.Add("FReturnAmtEx", typeof(System.String));//退款金额转元
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    dr["FstateEx"] = m_nCheckState[int.Parse(dr["Fstate"].ToString())];
                    if (string.IsNullOrEmpty(dr["FBuyBanktype"].ToString()))
                    {
                        dr["FbankTypeName"] = "未知银行类型";
                    }
                    else
                    {
                        dr["FbankTypeName"] = classLibrary.setConfig.convertbankType(dr["FBuyBanktype"].ToString());
                    }
                    dr["FAmtEx"] = RefundPublicFun.FenToYuan(dr["FAmt"].ToString());
                    dr["FReturnAmtEx"] = RefundPublicFun.FenToYuan(dr["FReturnAmt"].ToString());
                }

                if (bState)
                {
                    StringWriter sw = new StringWriter();
                    string excelHeader = gridInfor.Columns[0].HeaderText;
                    for (int i = 1; i < gridInfor.Columns.Count - 1; i++)
                    {
                        excelHeader += "\t" + gridInfor.Columns[i].HeaderText;
                    }
                    sw.WriteLine(excelHeader);

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        sw.WriteLine("=\"" + dr["FpayListid"].ToString() + "\"\t=\"" + dr["FReturnstate"].ToString() + "\"\t=\"" + dr["FAmtEx"]
                            + "\"\t" + dr["FReturnAmtEx"] + "\t=\"" + dr["FcreateTime"] + "\"\t=\""
                            + dr["FUserEmail"] + "\"\t=\"" + dr["FbankListid"] + "\"\t=\"" +
                            dr["FstateEx"] + "\"\t=\"" + dr["FbankTypeName"] + "\"");

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
            for (int i = 0; i < gridInfor.Rows.Count; i++)
            {
                CheckBox ckBox = (CheckBox)gridInfor.Rows[i].FindControl("checkbox");
                if (ckBox.Checked)
                {
                    string strUinID = gridInfor.DataKeys[i].Values["FpayListid"].ToString();

                    string strRefundType = "未知类型";
                    int nIndex = int.Parse(gridInfor.DataKeys[i].Values["FrefundType"].ToString())-1;
                    if(nIndex>=0 && nIndex < m_nAryRefundType.Length)
                    {
                        strRefundType = m_nAryRefundType[nIndex];
                    }                  
                    string strBankListId = gridInfor.DataKeys[i].Values["FbankListid"].ToString();
                    //string strIdentity = gridInfor.DataKeys[i].Values["FIdentity"].ToString();
                    string strBankName = gridInfor.DataKeys[i].Values["FbankName"].ToString();
                    
                    string strBankType = gridInfor.DataKeys[i].Values["FbankType"].ToString();
                    string strCreate = gridInfor.DataKeys[i].Values["FcreateTime"].ToString();
                    string strTrueName = gridInfor.DataKeys[i].Values["FtrueName"].ToString();
                    string strRefundId = gridInfor.DataKeys[i].Values["FoldId"].ToString();

                    /*
                     asp.net中打开新窗口的多种方法(转载)
                    1.Response.Redirect("XXX.aspx",true)——直接转向新的页面，原窗口被代替;
                    2. Response.Write("<script>window.open(XXX.aspx','_blank')</script>")——原窗口保留，另外新增一个新页面;
                    3.Response.Write("<script>window.location=XXX.aspx'</script>")——打开新的页面，原窗口被代替;
                    4.Server.Transfer("XXX.aspx")——打开新的页面;
                    5.Response.Write("<script>window.showModelessDialog(XXX.aspx')</script>")——原窗口保留，以对话框形式打开新                   
                    */
                    Response.Write("<script>window.open('AddInformation.aspx?uinID=" + strUinID + "&refundType=" + strRefundType + "&bankListId=" + strBankListId
                    + "&bankName=" + strBankName + "&bankType=" + strBankType + "&create=" + strCreate + "&trueName=" + strTrueName + "&refundId=" + strRefundId + "','_blank')</script>");
                    return;
                }
            }

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
        protected void OnCheckBox_CheckedSelect(object sender, System.EventArgs e)
        {
            
            Button btn = (Button)form1.FindControl("btnInputID");
            if (btn == null)
            {
                return;
            }
            btn.Enabled = false;
            CheckBox curCheckBox = (CheckBox)sender;
            DataControlFieldCell curCel = (DataControlFieldCell)curCheckBox.Parent;      
            GridViewRow curRow = (GridViewRow)curCel.Parent;
            if (curRow == null)
            {
                return;
            }
            if (!curCheckBox.Checked)
            {
                curRow.BackColor = gridInfor.RowStyle.BackColor;
                return;
            }
            curRow.BackColor = System.Drawing.Color.Green;
            for (int i = 0; i < gridInfor.Rows.Count; i++)
            {
                CheckBox cb = (CheckBox)gridInfor.Rows[i].FindControl("checkbox");
                if (cb.ClientID != curCheckBox.ClientID)
                {
                    cb.Checked = false;
                    DataControlFieldCell cel = (DataControlFieldCell)cb.Parent;
                    GridViewRow row = (GridViewRow)cel.Parent;
                    row.BackColor = gridInfor.RowStyle.BackColor;
                }
                else
                {
                    //仅当等待客服处理、已通知用户的状态下允许提交资料
                    string strText = gridInfor.Rows[i].Cells[7].Text;
                    if (strText == m_nCheckState[0] || strText == m_nCheckState[1])
                    {
                        btn.Enabled = true;
                    }
                }
            }
           
        }
        //通知
        protected void OnBtnClickNotice_Click(object sender, System.EventArgs e)
        {
            //string strParam = ((Button)sender).CommandArgument.ToString();  
            //Response.Redirect("NoticeUserByEmail.aspx");
            //FpayListid
            string strParam = ((Button)sender).CommandArgument.ToString();
            string[] arrParam = strParam.Split(',');
            Response.Write("<script>window.open('NoticeUserByEmail.aspx?truename=" + arrParam[0]+"&paybanklist="+arrParam[1] +"&oldid="+arrParam[2]+ "','_blank')</script>");

        }
        //通知银行
        protected void OnBtnClickBank_Click(object sender, System.EventArgs e)
        {
            string strParam = ((Button)sender).CommandArgument.ToString();       
            string[] arrParam = strParam.Split(',');
            /*"FpayListid,FCardType,FbankListid,FbankName,FbankType,FcreateTime,FtrueName,FmodifyTime,FReturnAmt,FAmt,FbankAccNo,FbankTypeOld"
            string strURL = "NoticeBankByEmail.aspx?newBankName=" +arrParam[0]+ "&newBankAccNo="+arrParam[1]+"&trueName="+arrParam[2]+"&returnDate="+arrParam[3]
                + "&bankType=" + arrParam[4] + "&createTime=" + arrParam[5] + "&bankListID=" + arrParam[6] + "&returnAmt=" + arrParam[7] + "&amt=" + arrParam[8];
            Response.Redirect(strURL);*/
            if (arrParam.Length < 0 || arrParam.Length > 9)
            {
                WebUtils.ShowMessage(this.Page, "通知银行详情取参长度不对！");
                return;
            }
            
            string strNewBank = classLibrary.setConfig.convertbankType(arrParam[0]);
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
            /*string strURL = "RefundDetails.aspx?foldId=" + arrParam[0];
            Response.Redirect(strURL);*/

            Response.Write("<script>window.open('RefundDetails.aspx?foldId=" + arrParam[0] + "','_blank')</script>");


        }
        /*
        //作废
        protected void OnLinkBtnObsolete_Click(object sender, System.EventArgs e)
        {

            try
            {
                string strParam = ((LinkButton)sender).CommandArgument.ToString();
                string[] arrParam = strParam.Split(',');
                if (arrParam.Length < 1)
                {
                    WebUtils.ShowMessage(this.Page, "特殊退款作废项取参长度不对！");
                    return;
                }
                if (!string.IsNullOrEmpty(arrParam[0]))
                {
                    new RefundService().SetRefundCheckState(7,arrParam[0]);
                    BindData();
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }*/

        protected void gridInfor_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridInfor.PageIndex = e.NewPageIndex;
            BindData();
        }

        
        protected void btnSetActor_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbActor.Text))
            {
                Session["uid"] = tbActor.Text;
                RefundPublicFun.operatorName = tbActor.Text; 
            }
        }
     /*   private void showMsg(string msg)
        {
            Response.Write("<script language=javascript>alert('" + msg + "')</script>");
        }

       
        protected void btnAddActor_Click(object sender, EventArgs e)
        {
            string strlevel = ddaddlevelid.SelectedItem.Value;
            if (string.IsNullOrEmpty(tbuserlistid.Text))
            {
                showMsg("请输入设置权限的角色名");
                return;
            }
            var refund = new RefundService();
            //Session["uid"].ToString()
            if (!refund.SetKFCheckUserInfo(tbuserlistid.Text, strlevel,"yonghualiu", 1))
            {
                showMsg("设置权限的角色名没有成功");
            }
        }

        protected void btnDelActor_Click(object sender, EventArgs e)
        {

        }
        */
    }
}