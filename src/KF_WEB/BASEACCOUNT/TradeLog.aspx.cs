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
using System.Web.Services.Protocols;
using System.Reflection;
using Wuqi.Webdiyer;
using System.Configuration;
using CFT.CSOMS.BLL.TradeModule;

namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
    /// <summary>
    /// TradeLog ��ժҪ˵����
    /// </summary>
    public partial class TradeLog : PageBase
    {
        protected System.Data.DataSet DS_TradeLog;
        protected System.Data.DataTable dataTable1;
        protected System.Data.DataColumn Fuid;
        protected System.Data.DataColumn FListID;
        protected System.Data.DataColumn dataColumn1;
        protected System.Data.DataColumn dataColumn2;
        DateTime dt;
        //		int total; //ҳ��

        protected void Page_Load(object sender, System.EventArgs e)
        {
            // �ڴ˴������û������Գ�ʼ��ҳ��
            if (!IsPostBack)
            {
                string user = "buy";
                if (Request.QueryString["user"] != null)
                {
                    user = Request.QueryString["user"].ToString().Trim();
                }

                ViewState["user"] = user;
                BindData(0, 1); //����DataTable��Ϊ����Դ
                //ȡ�ü�¼�������� 
                Page.DataBind();
            }
        }

        public void Item_DataBound(Object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item ||
                e.Item.ItemType == ListItemType.AlternatingItem)
                e.Item.Cells[1].Text = "<nobr>" + e.Item.Cells[1].Text + "</nobr>";
        }

        private DateTime SetTime(string aa)  //ʱ��ת������
        {
            //��ʼ��ʱ��	
            dt = new DateTime(int.Parse(aa.Substring(0, 4)), int.Parse(aa.Substring(4, 2)), int.Parse(aa.Substring(6, 2)), int.Parse(aa.Substring(8, 2)), int.Parse(aa.Substring(10, 2)), int.Parse(aa.Substring(12, 2)));
            return dt;
        }

        private void BindData(int i, int pageIndex)
        {
            if (Session["uid"] == null)
            {
                WebUtils.ShowMessage(this.Page, "��ʱ�������µ�½��");
                Response.Write("<script language=javascript>window.parent.location='../login.aspx?wh=1'</script>");
            }
            else
            {
                try
                {
                    string selectStr = Session["QQID"].ToString();
                    string fuid = "";
                    if (Session["fuid"] != null)
                    {
                        fuid = Session["fuid"].ToString();
                    }
                    DateTime beginTime = DateTime.Now.AddDays(-PublicRes.PersonInfoDayCount);
                    DateTime endTime = DateTime.Now.AddDays(1);
                    int pageSize = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["pageSize"].ToString());
                    int istr = 1 + pageSize * (pageIndex - 1);   //1��ʾ��һҳ
                    int imax = pageSize;
                    int iusertype = 0;

                    if (ViewState["user"] != null)
                    {
                        if (ViewState["user"].ToString() == "sale")
                        {
                            iusertype = 9; //����
                        }
                        else if (ViewState["user"].ToString() == "Unfinished")
                        {
                            iusertype = -1; //���δ��ɽ��׵�
                        }
                        else if (ViewState["user"].ToString() == "SaleUnfinished")
                        {
                            iusertype = -2; //����δ��ɽ��׵�
                        }
                        // 2012/5/29  �����н鶩����ѯ
                        else if (ViewState["user"].ToString() == "mediOrder")
                        {
                            iusertype = 10;
                        }
                    }

                    if (iusertype >= 0)
                    {
                        //if (u_IDType == 0 || u_IDType == 9 || u_IDType == 10 || u_IDType == 13)
                        DS_TradeLog = new TradeService().Q_PAY_LIST(selectStr, 0, beginTime, endTime, istr, imax, fuid);
                        //DS_TradeLog = classLibrary.setConfig.returnDataSet(selectStr, 1, beginTime, endTime, iusertype, "GetPayList", istr, imax, Session["uid"].ToString(), Request.UserHostAddress);
                    }
                    else
                    {
                        DS_TradeLog = new TradeService().GetListidFromUserOrder(selectStr, Session["uid"].ToString(), istr, imax, iusertype);
                    }

                    if (DS_TradeLog != null && DS_TradeLog.Tables.Count != 0 && DS_TradeLog.Tables[0].Rows.Count != 0)
                    {
                        var dv = DS_TradeLog.Tables[0].DefaultView;
                        dv.Sort = "Fcreate_time DESC";
                        DS_TradeLog.Tables.RemoveAt(0);
                        DS_TradeLog.Tables.Add(dv.ToTable());

                        foreach (DataRow row in DS_TradeLog.Tables[0].Rows)
                        {
                            bool isC2C = false;
                            int type = 0;
                            if (int.TryParse(row["Ftrade_type"].ToString(), out type))
                            {
                                if (type == 1)
                                {
                                    isC2C = true;
                                }
                            }
                            if (isC2C)
                            {
                                row["Fstate"] = 100;
                            }
                        }
                    }

                    //��ѯ���׵��ļ�¼��

                    int total;

                    if (DS_TradeLog != null && DS_TradeLog.Tables.Count != 0 && DS_TradeLog.Tables[0].Rows.Count != 0)
                        total = Int32.Parse(DS_TradeLog.Tables[0].Rows[0]["total"].ToString().Trim());
                    else
                        total = 0;
                    pager.RecordCount = total;
                    pager.PageSize = pageSize;
                    pager.CustomInfoText = "��¼������<font color=\"blue\"><b>" + pager.RecordCount.ToString() + "</b></font>";
                    pager.CustomInfoText += " ��ҳ����<font color=\"blue\"><b>" + pager.PageCount.ToString() + "</b></font>";
                    pager.CustomInfoText += " ��ǰҳ��<font color=\"red\"><b>" + pager.CurrentPageIndex.ToString() + "</b></font>";
                }
                catch (SoapException eSoap) //����soap��
                {
                    LogError("BaseAccount.TradeLog", "private void BindData(int i, int pageIndex)", eSoap);
                    string str = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                    WebUtils.ShowMessage(this.Page, "��ѯ������ϸ��" + str);
                }
                catch (Exception e)
                {
                    LogError("BaseAccount.TradeLog", "private void BindData(int i, int pageIndex)", e);
                    Response.Write("<font color=red>��ѯ������ϸ��" + e.Message + "</font>");
                }
            }
        }

        #region Web ������������ɵĴ���

        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: �õ����� ASP.NET Web ���������������ġ�
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// �����֧������ķ��� - ��Ҫʹ�ô���༭���޸�
        /// �˷��������ݡ�
        /// </summary>
        private void InitializeComponent()
        {
            this.DS_TradeLog = new System.Data.DataSet();
            this.dataTable1 = new System.Data.DataTable();
            this.Fuid = new System.Data.DataColumn();
            this.FListID = new System.Data.DataColumn();
            this.dataColumn1 = new System.Data.DataColumn();
            this.dataColumn2 = new System.Data.DataColumn();
            ((System.ComponentModel.ISupportInitialize)(this.DS_TradeLog)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataTable1)).BeginInit();
            // 
            // DS_TradeLog
            // 
            this.DS_TradeLog.DataSetName = "NewDataSet";
            this.DS_TradeLog.Locale = new System.Globalization.CultureInfo("zh-CN");
            this.DS_TradeLog.Tables.AddRange(new System.Data.DataTable[] { this.dataTable1 });
            // 
            // dataTable1
            // 
            this.dataTable1.Columns.AddRange(new System.Data.DataColumn[] { this.Fuid, this.FListID, this.dataColumn1, this.dataColumn2 });
            this.dataTable1.TableName = "Table1";
            // 
            // Fuid
            // 
            this.Fuid.ColumnName = "Fuid";
            // 
            // FListID
            // 
            this.FListID.ColumnName = "FlistID";
            // 
            // dataColumn1
            // 
            this.dataColumn1.ColumnName = "Column1";
            // 
            // dataColumn2
            // 
            this.dataColumn2.ColumnName = "Column2";
            ((System.ComponentModel.ISupportInitialize)(this.DS_TradeLog)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataTable1)).EndInit();
        }

        #endregion

        private void DataGrid1_PayList_SelectedIndexChanged(object sender, System.EventArgs e)
        {

        }

        public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            pager.CurrentPageIndex = e.NewPageIndex;
            BindData(0, pager.CurrentPageIndex);
            Page.DataBind();
        }
    }

}

