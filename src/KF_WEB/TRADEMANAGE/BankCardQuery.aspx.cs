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
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;
using System.IO;
using CFT.CSOMS.BLL.WechatPay; 


namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
    
    /// <summary>
    /// BankCardQuery 的摘要说明。
    /// </summary>
    public partial class BankCardQuery : System.Web.UI.Page
    {

        protected void Page_Load(object sender, System.EventArgs e)
        {
            ButtonDate.Attributes.Add("onclick", "openMode()");

            try
            {
                Label1.Text = Session["uid"].ToString();
                string szkey = Session["SzKey"].ToString();
                int operid = Int32.Parse(Session["OperID"].ToString());

                //if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID,"InfoCenter")) Response.Redirect("../login.aspx?wh=1");
                if (!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter", this)) Response.Redirect("../login.aspx?wh=1");
            }
            catch
            {
                Response.Redirect("../login.aspx?wh=1");
            }

            if (!IsPostBack)
            {              
                TextBoxDate.Text = DateTime.Now.ToString("yyyy年MM月dd日");
                Table2.Visible = false;
            }
        }

        public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            pager.CurrentPageIndex = e.NewPageIndex;
            BindData(e.NewPageIndex);
        }

        protected void btnSearch_Click(object sender, System.EventArgs e)
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
                Table2.Visible = true;
                pager.RecordCount = 1000;
                BindData(1);

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

        private void ValidateDate()
        {
            DateTime Date;

            try
            {
                Date = DateTime.Parse(TextBoxDate.Text);
            }
            catch
            {
                throw new Exception("日期输入有误！");
            }

            ViewState["Date"] = Date.ToString("yyyyMMdd");
            ViewState["fpay_acc"] = this.txtBankCardID.Text.Trim();


            if (ViewState["fpay_acc"].ToString() == "")
            {
                throw new Exception("请输入银行卡号！");
            }
        }


        private void BindData(int index)
        {
            try
            {
                int max = pager.PageSize;
           //     int start = max * (index - 1) + 1;
                int start = max * (index - 1);
              //  Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
                // DataSet ds = qs.Getfbank_orderList(ViewState["fpay_acc"].ToString(), ViewState["Date"].ToString(), start, max);
                DataSet ds = new FastPayService().QueryBankCardList(ViewState["fpay_acc"].ToString(), ViewState["Date"].ToString(), int.Parse(this.ddlBizType.SelectedValue), start, max);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataGrid1.DataSource = ds.Tables[0].DefaultView;
                    DataGrid1.DataBind();
                }
                else
                {
                    throw new LogicException("没有找到记录！");
                }
            }
            catch (Exception eSys)
            {
                string errStr = PublicRes.GetErrorMsg(eSys.Message.ToString());
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + errStr);

            }
            //if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            //{
            //    ds.Tables[0].Columns.Add("Fbiz_type_str", typeof(String));//业务状态
            //    DataToGrid(ds);
            //}
            //else
            //{//如果前面的查询结果为null，则可能为“中行信用卡”和“兴业信用卡”
            //    DateTime Date= DateTime.Parse(TextBoxDate.Text);
                

            //    DataSet ds2 = qs.Getfbank_twoBank_orderList(ViewState["fpay_acc"].ToString(), Date);
            //    if (ds2 != null && ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
            //    {
            //        DataToGrid(ds2);
            //    }
            //    else
            //    {
            //        throw new LogicException("没有找到记录！");
            //    }
            //}
        }

        private void DataToGrid(DataSet ds)
        {
            ds.Tables[0].Columns.Add("FamtStr", typeof(string));
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                dr["FamtStr"] = MoneyTransfer.FenToYuan(dr["Famt"].ToString());
            }
            DataGrid1.DataSource = ds.Tables[0].DefaultView;
            DataGrid1.DataBind();
        }

        public void btnBatchQuery(object sender, System.EventArgs e)
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds.Tables.Add(dt);

                #region 解析excel文件
                if (!File1.HasFile)
                {
                    WebUtils.ShowMessage(this.Page, "请选择上传文件！");
                    return;
                }
                if (Path.GetExtension(File1.FileName).ToLower() == ".xls")
                {
                    string uin = Session["uid"].ToString();
                    string path = Server.MapPath("~/") + "PLFile" + "\\bankcard" + uin + ".xls";
                    File1.PostedFile.SaveAs(path);

                    bool isFirst = true;
                    string errMsg = "";

                    DataSet res_ds = PublicRes.readXls(path);
                    System.Data.DataTable res_dt = res_ds.Tables[0];
                    int iColums = res_dt.Columns.Count;
                    int iRows = res_dt.Rows.Count;

                    for (int i = 0; i < iRows; i++)
                    {
                        string r1 = res_dt.Rows[i][0].ToString();
                        string r2 = res_dt.Rows[i][1].ToString();

                        try
                        {
                            DateTime Date;

                            try
                            {
                                Date = DateTime.Parse(TextBoxDate.Text);
                            }
                            catch
                            {
                                throw new Exception("日期输入有误！");
                            }

                            string date = Date.ToString("yyyyMMdd");

                            //加分页标记，若为-1不分页
                            DataSet tmpDs = new FastPayService().QueryBankCardList(r1, date,10100, -1, -1);
                            if (tmpDs != null && tmpDs.Tables.Count > 0 && tmpDs.Tables[0].Rows.Count > 0)
                            {
                                foreach (DataRow dr in tmpDs.Tables[0].Rows)
                                {
                                    if (isFirst)
                                    {
                                        //如果第一次，先添加列名
                                        for (int j = 0; j < dr.Table.Columns.Count; j++)
                                        {
                                            dt.Columns.Add(dr.Table.Columns[j].ColumnName);
                                        }
                                        isFirst = false;
                                    }

                                    dt.ImportRow(dr);
                                }
                            }


                        }
                        catch (Exception ex)
                        {
                            errMsg += ex.Message;
                        }
                    }
                }
                else
                {
                    WebUtils.ShowMessage(this.Page, "文件格式不正确，请选择xls格式文件上传。");
                    return;
                }
                #endregion

                #region 导出excel文件

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    StringWriter sw = new StringWriter();
                    sw.WriteLine("银行卡号\t银行订单号\t金额\t业务状态");

                    foreach (DataRow dr in dt.Rows)
                    {
                        sw.WriteLine("=\"" + dr["fpay_acc"].ToString() + "\"\t=\"" + dr["fbank_order"].ToString() + "\"\t=\"" + dr["FamtStr"]
                            + "\"\t=\"" + dr["Fbiz_type_str"] + "\"");

                    }
                    sw.Close();
                    //string f_name = "银行卡批量查询";
                    //f_name = System.Web.HttpUtility.UrlEncode(System.Text.Encoding.UTF8.GetBytes(f_name));
                    Response.AddHeader("Content-Disposition", "attachment; filename=银行卡批量查询.xls");
                    Response.ContentType = "application/ms-excel";
                    Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
                    Response.Write(sw);
                    Response.End();
                }



                #endregion
            }
            catch (Exception eSys)
            {

                WebUtils.ShowMessage(this.Page, "读取数据失败！" + eSys.Message.ToString());

            }
        }
    }
}
