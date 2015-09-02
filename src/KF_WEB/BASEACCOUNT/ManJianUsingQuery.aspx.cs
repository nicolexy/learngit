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
    public partial class ManJianUsingQuery : System.Web.UI.Page
    {
        protected System.Data.DataSet DS_TradeLog;
        protected System.Data.DataTable dataTable1;
        protected System.Data.DataColumn Fuid;
        protected System.Data.DataColumn FListID;
        protected System.Data.DataColumn dataColumn1;
        protected System.Data.DataColumn dataColumn2;
        DateTime dt;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            ButtonBeginDate.Attributes.Add("onclick", "openModeBegin()");
            ButtonEndDate.Attributes.Add("onclick", "openModeEnd()");
            try
            {
                Label1.Text = Session["uid"].ToString();
                string szkey = Session["SzKey"].ToString();
                if (!classLibrary.ClassLib.ValidateRight("InfoCenter", this)) Response.Redirect("../login.aspx?wh=1");
            }
            catch
            {
                Response.Redirect("../login.aspx?wh=1");
            }
            // �ڴ˴������û������Գ�ʼ��ҳ��

            if (!IsPostBack)
            {
                string user = "buy";
                ViewState["user"] = user;
                DateTime now = DateTime.Now;
                DateTime d1 = new DateTime(now.Year, now.Month, 1);
                TextBoxBeginDate.Text = d1.ToString("yyyy��MM��dd��");
                TextBoxEndDate.Text = DateTime.Now.ToString("yyyy��MM��dd��");
                BindBankType(ddlBankType);
            }
        }

        public void Item_DataBound(Object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item ||

                e.Item.ItemType == ListItemType.AlternatingItem)

                e.Item.Cells[1].Text = "<nobr>" + e.Item.Cells[1].Text + "</nobr>";
        }

        private void ValidateDate()
        {
            DateTime begindate;
            DateTime enddate;
            try
            {
                begindate = DateTime.Parse(TextBoxBeginDate.Text);
                enddate = DateTime.Parse(TextBoxEndDate.Text);
                ViewState["begindate"] = DateTime.Parse(begindate.ToString("yyyy-MM-dd 00:00:00"));
                ViewState["enddate"] = DateTime.Parse(enddate.ToString("yyyy-MM-dd 23:59:59"));
                ViewState["banktype"] = ddlBankType.SelectedValue;
            }
            catch
            {
                throw new Exception("������������");
            }
            if (begindate.CompareTo(enddate) > 0)
            {
                throw new Exception("��ֹ����С����ʼ���ڣ����������룡");
            }
        }

        void BindBankType(DropDownList ddl)
        {
            ddl.Items.Add(new ListItem("��������", ""));
            ddl.Items.Add(new ListItem("��������", "1001"));
            ddl.Items.Add(new ListItem("��������", "1002"));
            ddl.Items.Add(new ListItem("�������ÿ�", "1050"));
            ddl.Items.Add(new ListItem("��������", "1003"));
            ddl.Items.Add(new ListItem("�ַ�����", "1004"));
            ddl.Items.Add(new ListItem("ũҵ����", "1005"));
            ddl.Items.Add(new ListItem("��������", "1006"));
            ddl.Items.Add(new ListItem("ũ�й��ʿ�", "1007"));
            ddl.Items.Add(new ListItem("���ڷ�չ����", "1008"));
            ddl.Items.Add(new ListItem("��ҵ����", "1009"));
            ddl.Items.Add(new ListItem("������ҵ����", "1010"));
            ddl.Items.Add(new ListItem("�й���ͨ����", "1020"));
            ddl.Items.Add(new ListItem("����ʵҵ����", "1021"));
            ddl.Items.Add(new ListItem("�й��������", "1022"));
            ddl.Items.Add(new ListItem("ũ�����������", "1023"));
            ddl.Items.Add(new ListItem("�Ϻ�����", "1024"));
            ddl.Items.Add(new ListItem("��������", "1025"));
            ddl.Items.Add(new ListItem("�й�����", "1026"));
            ddl.Items.Add(new ListItem("�㶫��չ����", "1027"));
            ddl.Items.Add(new ListItem("�㶫����", "1028"));
            ddl.Items.Add(new ListItem("����B2B", "1040"));
            ddl.Items.Add(new ListItem("������ǿ�", "1041"));
            ddl.Items.Add(new ListItem("����B2B", "1042"));
            ddl.Items.Add(new ListItem("��������", "1044"));
            ddl.Items.Add(new ListItem("��������", "1099"));
        }

        private DateTime SetTime(string aa)  //ʱ��ת������
        {

            //��ʼ��ʱ��	

            dt = new DateTime(int.Parse(aa.Substring(0, 4)), int.Parse(aa.Substring(4, 2)), int.Parse(aa.Substring(6, 2)), int.Parse(aa.Substring(8, 2)), int.Parse(aa.Substring(10, 2)), int.Parse(aa.Substring(12, 2)));

            return dt;

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
            if (this.txtQQ.Text.Trim() == "" || this.txtQQ.Text.Trim() == null)
            {
                WebUtils.ShowMessage(this.Page, "������Ƹ�ͨ�˺ţ�");
                return;
            }
            try
            {
                BindData(0, 1);
                //  this.DataGrid2.Visible = true;
                //  Page.DataBind();
            }
            catch (Exception eSys)
            {
                this.DataGrid2.Visible = false;
                WebUtils.ShowMessage(this.Page, "��ȡ����ʧ�ܣ�" + eSys.Message);
            }
        }


        private void BindData(int i, int pageIndex)
        {

            if (Session["uid"] == null)
            {
                WebUtils.ShowMessage(this.Page, "��ʱ�������µ�½��");
                Response.Redirect("../login.aspx?wh=1");
            }

            else
            {
                try
                {
                    string selectStr = this.txtQQ.Text.Trim();
                    DateTime beginTime = (DateTime)ViewState["begindate"];
                    DateTime endTime = (DateTime)ViewState["enddate"];
                    string banktype = ViewState["banktype"].ToString();
                    int pageSize = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["pageSize"].ToString());
                    int istr = 1 + pageSize * (pageIndex - 1);   //1��ʾ��һҳ
                    int imax = pageSize;
                    #region old
                    //Query_Service.Query_Service myService = new Query_Service.Query_Service();
                    //myService.Finance_HeaderValue = classLibrary.setConfig.setFH(Session["uid"].ToString(), Request.UserHostAddress);
                    //DS_TradeLog = myService.GetManJianUsingList(selectStr, i, beginTime, endTime, banktype, istr, imax);
                    #endregion
                    DS_TradeLog = new TradeService().GetManJianUsingList(selectStr, i, beginTime, endTime, banktype, istr, imax);
                    int total;
                    if (DS_TradeLog != null && DS_TradeLog.Tables.Count != 0 && DS_TradeLog.Tables[0].Rows.Count != 0)
                    {
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
                        total = Int32.Parse(DS_TradeLog.Tables[0].Rows[0]["total"].ToString().Trim());
                        pager.RecordCount = total;
                        pager.PageSize = pageSize;
                        pager.CustomInfoText = "��¼������<font color=\"blue\"><b>" + pager.RecordCount.ToString() + "</b></font>";
                        pager.CustomInfoText += " ��ҳ����<font color=\"blue\"><b>" + pager.PageCount.ToString() + "</b></font>";
                        pager.CustomInfoText += " ��ǰҳ��<font color=\"red\"><b>" + pager.CurrentPageIndex.ToString() + "</b></font>";
                        this.DataGrid2.Visible = true;
                        Page.DataBind();
                    }
                    else
                    {
                        this.DataGrid2.Visible = false;
                        WebUtils.ShowMessage(this.Page, "û�в�ѯ����¼��");
                    }
                }
                catch (SoapException eSoap) //����soap��
                {
                    string str = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                    WebUtils.ShowMessage(this.Page, "��ѯ������ϸ��" + str);
                }
                catch (Exception e)
                {
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

        public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            pager.CurrentPageIndex = e.NewPageIndex;
            BindData(0, pager.CurrentPageIndex);
            Page.DataBind();
        }

    }

}

