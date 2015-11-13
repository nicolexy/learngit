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
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using System.Configuration;
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;
using CFT.CSOMS.BLL.TradeModule;
using System.Collections.Generic;

namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
    /// <summary>
    /// bankrolllog 的摘要说明。
    /// </summary>
    public partial class bankrolllog : System.Web.UI.Page
    {
        protected System.Data.DataSet DS_Bankroll;
        protected System.Data.DataTable dataTable1;
        protected System.Data.DataColumn dataColumn1;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    //绑定第一页数据
                    BindData(1);
                }
                catch (Exception err)
                {
                    WebUtils.ShowMessage(this.Page, "操作超时！请重新查询。" + PublicRes.GetErrorMsg(err.Message));
                }
            }
        }

        private void BindData(int pageIndex)
        {
            DateTime beginTime, endTime;

            if (Request.QueryString["BeginDate"] != null)
            {
                beginTime = DateTime.Parse(Request.QueryString["BeginDate"].ToString());
                endTime = DateTime.Parse(Request.QueryString["EndDate"].ToString());
            }
            else
            {
                beginTime = DateTime.Now.AddDays(-PublicRes.PersonInfoDayCount);
                endTime = DateTime.Now.AddDays(1);
            }

            int pageSize = Int32.Parse(ConfigurationManager.AppSettings["pageSize"].ToString());  //通过webconfig控制页大小
            int istr = 1 + pageSize * (pageIndex - 1);  //初始为1（事实上索引0始）
            int imax = pageSize;                       //每页显示10条记录

            #region type==QQID
            if (Request.QueryString["type"].ToString() == "QQID")
            {

                string selectStr = Request.QueryString["qqid"] != null ? Request.QueryString["qqid"].ToString() : Session["QQID"].ToString();
                string fuid = Session["fuid"] as String;

                int fcurtype = 1;
                if (Request.QueryString["currtype"] != null)
                {
                    fcurtype = int.Parse(Request.QueryString["currtype"].Trim()); //来自保证金查询
                }

                //增加了子帐户

                if (Request.QueryString["Fcurtype"] != null && Request.QueryString["Fcurtype"].ToString() != "")
                {
                    Query_Service.Query_Service myService = new Query_Service.Query_Service();
                    this.DS_Bankroll = myService.GetChildrenBankRollList(selectStr, beginTime, endTime, Request.QueryString["Fcurtype"].ToString(), istr, imax);
                }
                else
                {
                    //this.DS_Bankroll = classLibrary.setConfig.returnDataSet(selectStr,beginTime,endTime,0,"Bankroll",istr,imax,Session["uid"].ToString(),Request.UserHostAddress);
                    //KF人员反映有很多金额为0的空数据，这里过滤下

                    string ref_param = ViewState["ref_param"] == null ? "" : ViewState["ref_param"].ToString();

                    if (!string.IsNullOrEmpty(fuid))
                        this.DS_Bankroll = new TradeService().GetBankRollList("", fuid, beginTime, endTime,"", istr, imax, ref  ref_param);  //注销账户通过qqid无法查到fuid,直接通过fuid查询。
                    else
                        this.DS_Bankroll = new TradeService().GetBankRollList(selectStr, "", beginTime, endTime,"", istr, imax, ref  ref_param);

                    ViewState["ref_param"] = ref_param;


                    if (DS_Bankroll != null && DS_Bankroll.Tables.Count != 0 && DS_Bankroll.Tables[0].Rows.Count != 0)
                    {
                        for (int i = 0; i < DS_Bankroll.Tables[0].Rows.Count; i++)
                        {
                            if (DS_Bankroll.Tables[0].Rows[i]["Fpaynum"].ToString().Trim() == "0")
                            {
                                DS_Bankroll.Tables[0].Rows[i].Delete();
                                i--;
                            }
                        }
                    }
                }

                int total;

                if (DS_Bankroll != null && DS_Bankroll.Tables.Count != 0 && DS_Bankroll.Tables[0].Rows.Count != 0)
                    total = 1000;//Int32.Parse(DS_Bankroll.Tables[0].Rows[0]["total"].ToString());
                else
                    total = 0;
                pager.RecordCount = total;
                pager.PageSize = pageSize;
                pager.CustomInfoText = "记录总数：<font color=\"blue\"><b>" + pager.RecordCount.ToString() + "</b></font>";
                pager.CustomInfoText += " 总页数：<font color=\"blue\"><b>" + pager.PageCount.ToString() + "</b></font>";
                pager.CustomInfoText += " 当前页：<font color=\"red\"><b>" + pager.CurrentPageIndex.ToString() + "</b></font>";
            } 
            #endregion

            #region type==ListID
            else if (Request.QueryString["type"].ToString() == "ListID")
            {
                if (Session["uid"] == null)
                {
                    Response.Redirect("../login.aspx?wh=1"); //重新登陆
                }

                Query_Service.Query_Service myService = new Query_Service.Query_Service();
                myService.Finance_HeaderValue = classLibrary.setConfig.setFH(this);
                imax = 30; //如果是查询交易单时，一次性显示所有的纪录
                string selectStr = Session["ListID"].ToString();
                this.DS_Bankroll = myService.GetBankRollList_withID(beginTime, endTime, selectStr, istr, imax);
            } 
            #endregion

            #region 结果处理
            if (DS_Bankroll != null && DS_Bankroll.Tables.Count != 0)
            {
                DS_Bankroll.Tables[0].Columns.Add("FpaynumStr", typeof(string));
                DS_Bankroll.Tables[0].Columns.Add("FbalanceStr", typeof(string));
                string[] CoverPickFuid = System.Configuration.ConfigurationManager.AppSettings["CoverPickFuid"].ToString().Split('|');

                foreach (DataRow dr in DS_Bankroll.Tables[0].Rows)
                {
                    try
                    {
                        string Fpaynum = classLibrary.setConfig.FenToYuan(dr["Fpaynum"].ToString());
                        string Fbalance = classLibrary.setConfig.FenToYuan(dr["Fbalance"].ToString());
                        dr["FpaynumStr"] = Fpaynum;
                        dr["FbalanceStr"] = Fbalance;

                        var fmemo = dr["Fmemo"] as string;  //乱码Bug问题
                        var list= new List<string> { "1000039701", "1000030901", "1000030601", "1000040101", " 1000035801", "1000035001", "1000040901", "1000037301" }; //这几个商户备注的是utf-8 类型的字符集
                        if (!string.IsNullOrEmpty(fmemo) && list.Contains((string)dr["Ffromid"]))
                        {
                            var buff = System.Text.Encoding.GetEncoding("gb2312").GetBytes(fmemo);
                            dr["Fmemo"] = System.Text.Encoding.UTF8.GetString(buff);
                        }

                        for (int i = 0; i < CoverPickFuid.Length; i++)
                        {
                            if (CoverPickFuid[i].ToString() == dr["Fuid"].ToString())
                            {
                                try
                                {                                  
                                    int PointIndex = Fpaynum.IndexOf(".");
                                    dr["FpaynumStr"] = "******" + Fpaynum.Substring(PointIndex - 1, Fpaynum.Length - PointIndex + 1);
                                    PointIndex = Fbalance.IndexOf(".");
                                    dr["FbalanceStr"] = "******" + Fbalance.Substring(PointIndex - 1, Fbalance.Length - PointIndex + 1);
                                }
                                catch
                                {
                                    dr["FpaynumStr"] = "******";
                                    dr["FbalanceStr"] = "******";
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("金额Fpaynum：" + dr["Fnum"].ToString() + "金额Fbalance：" + dr["Fbalance"].ToString() + "转换有问题" + ex.Message);
                    }
                }
            } 
            #endregion
            Page.DataBind();
        }

        public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            pager.CurrentPageIndex = e.NewPageIndex;
            BindData(pager.CurrentPageIndex);
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
            this.DS_Bankroll = new System.Data.DataSet();
            this.dataTable1 = new System.Data.DataTable();
            this.dataColumn1 = new System.Data.DataColumn();
            ((System.ComponentModel.ISupportInitialize)(this.DS_Bankroll)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataTable1)).BeginInit();
            // 
            // DS_Bankroll
            // 
            this.DS_Bankroll.DataSetName = "NewDataSet";
            this.DS_Bankroll.Locale = new System.Globalization.CultureInfo("zh-CN");
            this.DS_Bankroll.Tables.AddRange(new System.Data.DataTable[] { this.dataTable1 });
            // 
            // dataTable1
            // 
            this.dataTable1.Columns.AddRange(new System.Data.DataColumn[] { this.dataColumn1 });
            this.dataTable1.TableName = "Table1";
            // 
            // dataColumn1
            // 
            this.dataColumn1.ColumnName = "FBKid";
            ((System.ComponentModel.ISupportInitialize)(this.DS_Bankroll)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataTable1)).EndInit();
        }

        #endregion
    }
}

