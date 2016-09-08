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

namespace TENCENT.OSS.CFT.KF.KF_Web.TravelPlatform
{
    /// <summary>
    /// PayBusinessQuery ��ժҪ˵����
    /// </summary>
    public partial class HotelOrderQuery : TENCENT.OSS.CFT.KF.KF_Web.PageBase
    {

        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                Label1.Text = Session["uid"].ToString();
                string szkey = Session["SzKey"].ToString();

                if (!classLibrary.ClassLib.ValidateRight("SPInfoManagement", this)) Response.Redirect("../login.aspx?wh=1");

                if (!IsPostBack)
                {
                    TextBoxBeginDate.Value = DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd");
                    TextBoxEndDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            catch
            {
                Response.Redirect("../login.aspx?wh=1");
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
            pager.CurrentPageIndex = e.NewPageIndex;
            BindData(e.NewPageIndex);
        }

        protected void btnSearch_Click(object sender, System.EventArgs e)
        {
            DateTime begindate;
            DateTime enddate;
            try
            {
                begindate = DateTime.Parse(TextBoxBeginDate.Value);
                enddate = DateTime.Parse(TextBoxEndDate.Value);
                if (begindate.CompareTo(enddate) > 0)
                {
                    WebUtils.ShowMessage(this.Page, "��ֹ����С����ʼ���ڣ����������룡");
                    return;
                }
            }
            catch
            {
                WebUtils.ShowMessage(this.Page, "������������");
                return;
            }

            try
            {
                pager.RecordCount = 100000;
                BindData(1);
            }
            catch (SoapException eSoap) //����soap���쳣
            {
                //	this.dgList.Visible = false;
                string errStr = PublicRes.GetErrorMsg(eSoap.Message);
                WebUtils.ShowMessage(this.Page, "���÷������" + errStr);
            }
            catch (Exception eSys)
            {
                //	this.dgList.Visible = false;
                WebUtils.ShowMessage(this.Page, "��ȡ����ʧ�ܣ�" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
            }
        }

        protected void BindData(int index)
        {
            try
            {
                pager.CurrentPageIndex = index;
                int limit = pager.PageSize;
                int page_id = index;
                Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
                string uin = this.TextUin.Text.Trim();//�Ƹ�ͨ�˺�
                string hotelName = this.TextHotelName.Text.Trim();//������
                string start_time = DateTime.Parse(TextBoxBeginDate.Value).ToString("yyyy-MM-dd");//������ʼʱ��
                string end_time = DateTime.Parse(TextBoxEndDate.Value).ToString("yyyy-MM-dd");//��������ʱ��
                DataSet ds = qs.HotelOrderQuery(uin, hotelName, start_time, end_time, limit, page_id);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ds.Tables[0].Columns.Add("PayAmt_str", typeof(String));
                    classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "PayAmt", "PayAmt_str");
                    dgList.DataSource = ds.Tables[0].DefaultView;
                    dgList.DataBind();
                }
                else
                {
                    dgList.DataSource = null;
                    WebUtils.ShowMessage(this.Page, "û���ҵ���¼��");
                }
            }
            catch (SoapException eSoap) //����soap���쳣
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message);
                WebUtils.ShowMessage(this.Page, "���÷������" + errStr);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "��ȡ����ʧ�ܣ�" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
            }
        }
    }
}
