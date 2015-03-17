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
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using System.Web.Services.Protocols;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.CFT.KF.DataAccess;

namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
    /// <summary>
    /// OrderQuery ��ժҪ˵����
    /// </summary>
    public partial class OrderQuery : System.Web.UI.Page
    {
        public string tfcurtype = "1";// ����

        protected void Page_Load(object sender, System.EventArgs e)
        {
            // �ڴ˴������û������Գ�ʼ��ҳ��
            try
            {
                Label1.Text = Session["uid"].ToString();

                string szkey = Session["SzKey"].ToString();
                int operid = Int32.Parse(Session["OperID"].ToString());

                //if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "FundQuery")) return;
                if (!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("TradeLogQuery", this)) return;
            }
            catch
            {
                return;
            }

            if (!IsPostBack)
            {
                ButtonBeginDate.Attributes.Add("onclick", "openModeBegin()");
                ButtonEndDate.Attributes.Add("onclick", "openModeEnd()");

                TextBoxBeginDate.Text = DateTime.Now.AddDays(-5).ToString("yyyy��MM��dd��");
                TextBoxEndDate.Text = DateTime.Now.ToString("yyyy��MM��dd��");

                classLibrary.setConfig.GetAllTypeList(ddlStateType, "PAY_STATE");
                ddlStateType.Items.Insert(0, new ListItem("����״̬", "99"));

                Table2.Visible = false;

                this.Label7.Text = "������ѯ";
                Label6.Visible = true;
                tbSaleQQ.Visible = true;
                if (Request.QueryString["fcurtype"] != null)//rowena 20100722  ���ӻ�����Ŀ
                {
                    Label7.Text = "���𶩵���ѯ";
                    Label6.Visible = false;
                    tbSaleQQ.Visible = false;
                    tfcurtype = "2";

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
            this.pager.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(this.ChangePage);

        }
        #endregion

        public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            try
            {
                pager.CurrentPageIndex = e.NewPageIndex;
                BindData(e.NewPageIndex);
            }
            catch (SoapException eSoap) //����soap���쳣
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "���÷������" + errStr);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "��ȡ����ʧ�ܣ�" + classLibrary.setConfig.replaceMStr(eSys.Message));
            }
        }

        private void ValidateDate()
        {
            DateTime begindate;
            DateTime enddate;

            try
            {
                begindate = DateTime.Parse(TextBoxBeginDate.Text);
                enddate = DateTime.Parse(TextBoxEndDate.Text);
            }
            catch
            {
                throw new Exception("������������");
            }

            if (begindate.CompareTo(enddate) > 0)
            {
                throw new Exception("��ֹ����С����ʼ���ڣ����������룡");
            }

            if (dpLst.SelectedValue != "FlistID" || tbListID.Text.Trim() == "")
            {
                if (tbBuyQQ.Text.Trim() == "" && tbSaleQQ.Text.Trim() == "" && tbBuyQQInnerID.Text.Trim() == "" && tbSaleQQInnerID.Text.Trim() == "")
                {
                    throw new Exception("����ʺ�,�����ʺ�,����ʺ��ڲ�ID,�����ʺ��ڲ�ID�����׵�ID��������һ��");
                }
            }

            ViewState["fstate"] = ddlStateType.SelectedValue;

            ViewState["begindate"] = DateTime.Parse(begindate.ToString("yyyy-MM-dd 00:00:00"));
            ViewState["enddate"] = DateTime.Parse(enddate.ToString("yyyy-MM-dd 23:59:59"));

            ViewState["querytype"] = this.dpLst.SelectedValue.Trim();
            ViewState["queryvalue"] = tbListID.Text.Trim();

            ViewState["buyqq"] = tbBuyQQ.Text.Trim();
            ViewState["saleqq"] = tbSaleQQ.Text.Trim();

            ViewState["buyqqID"] = tbBuyQQInnerID.Text.Trim();
            ViewState["saleqqID"] = tbSaleQQInnerID.Text.Trim();

        }

        protected void Button2_Click(object sender, System.EventArgs e)
        {
            try
            {
                ValidateDate();

                Table2.Visible = true;
                pager.RecordCount = GetCount();

                BindData(1);

            }
            catch (SoapException eSoap) //����soap���쳣
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "���÷������" + errStr);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "��ȡ����ʧ�ܣ�" + classLibrary.setConfig.replaceMStr(eSys.Message));
            }
        }


        private int GetCount()
        {
            int fstate = Int32.Parse(ViewState["fstate"].ToString());

            DateTime dtBegin = DateTime.Parse(ViewState["begindate"].ToString());
            DateTime dtEnd = DateTime.Parse(ViewState["enddate"].ToString());

            string querytype = ViewState["querytype"].ToString();
            string queryvalue = ViewState["queryvalue"].ToString();

            string buyqq = ViewState["buyqq"].ToString();
            string saleqq = ViewState["saleqq"].ToString();

            string buyqqIn = ViewState["buyqqID"].ToString();
            string saleqqIn = ViewState["saleqqID"].ToString();

            int fcurtype = 1;
            if (Request.QueryString["fcurtype"] != null)//rowena 20100722  ���ӻ�����Ŀ
            {
                fcurtype = int.Parse(Request.QueryString["fcurtype"].Trim()); ;
            }
            Query_Service.Query_Service qs = new Query_Service.Query_Service();
            return qs.GetQueryListCount(dtBegin, dtEnd, buyqq, saleqq, buyqqIn, saleqqIn, querytype, queryvalue, fstate, fcurtype);
        }

        private void BindData(int index)
        {
            int fstate = Int32.Parse(ViewState["fstate"].ToString());

            DateTime dtBegin = DateTime.Parse(ViewState["begindate"].ToString());
            DateTime dtEnd = DateTime.Parse(ViewState["enddate"].ToString());

            string querytype = ViewState["querytype"].ToString();
            string queryvalue = ViewState["queryvalue"].ToString();

            string buyqq = ViewState["buyqq"].ToString();
            string saleqq = ViewState["saleqq"].ToString();

            string buyqqIn = ViewState["buyqqID"].ToString();
            string saleqqIn = ViewState["saleqqID"].ToString();

            int max = pager.PageSize;
            int start = max * (index - 1) + 1;

            int fcurtype = 1;
            if (Request.QueryString["fcurtype"] != null)//rowena 20100722  ���ӻ�����Ŀ
            {
                fcurtype = int.Parse(Request.QueryString["fcurtype"].Trim()); ;
            }

            Query_Service.Query_Service qs = new Query_Service.Query_Service();

            Finance_Header fh = new Finance_Header();
            fh.UserIP = Request.UserHostAddress;
            fh.UserName = Session["uid"].ToString();
            //fh.UserPassword = "";
            fh.OperID = Int32.Parse(Session["OperID"].ToString());
            fh.SzKey = Session["SzKey"].ToString();
            //fh.RightString = Session["key"].ToString();

            qs.Finance_HeaderValue = fh;

            DataSet ds = qs.GetQueryList(dtBegin, dtEnd, buyqq, saleqq, buyqqIn, saleqqIn, querytype, queryvalue, fstate, fcurtype, start, max);

            if (ds != null && ds.Tables.Count > 0)
            {
                ds.Tables[0].Columns.Add("FpaynumName", typeof(String));
                ds.Tables[0].Columns.Add("FTradeStateName", typeof(String));
                ds.Tables[0].Columns.Add("FlistidUrl", typeof(String));
                ds.Tables[0].Columns.Add("Ftrade_typeName", typeof(String));

                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "Fpaynum", "FpaynumName");

                if (fcurtype == 2)
                {
                    classLibrary.setConfig.GetColumnValueFromDic(ds.Tables[0], "Ftrade_state", "FTradeStateName", "PAY_STATE");
                }
                else
                {
                    classLibrary.setConfig.GetColumnValueFromDic_Fund(ds.Tables[0], "Ftrade_state", "FTradeStateName", "PAY_STATE", fcurtype);
                }
                classLibrary.setConfig.GetColumnValueFromDic(ds.Tables[0], "Ftrade_type", "Ftrade_typeName", "PAYLIST_TYPE");

                GetFlistIdUrl(ds.Tables[0], fcurtype, "FListID", "FlistidUrl");


                if (fstate != 99)
                {
                    ds.Tables[0].DefaultView.RowFilter = "Ftrade_state=" + fstate;
                }

                DataGrid1.DataSource = ds.Tables[0].DefaultView;
                DataGrid1.DataBind();
            }
            else
            {
                throw new LogicException("û���ҵ���¼��");
            }
        }

        private void GetFlistIdUrl(DataTable dt, int fcurtype, string FieldName, string destField)
        {
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    string flist = dr[FieldName].ToString();
                    string desflist = "listid=" + flist + "&fcurtype=" + fcurtype + "";
                    dr.BeginEdit();
                    dr[destField] = desflist;
                    dr.EndEdit();
                }
            }
            catch//(Exception err)
            {
                return;
            }
        }

    }
}
