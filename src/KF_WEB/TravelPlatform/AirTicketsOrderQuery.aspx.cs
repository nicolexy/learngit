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
    public partial class AirTicketsOrderQuery : System.Web.UI.Page
    {

        protected void Page_Load(object sender, System.EventArgs e)
        {
            ButtonBeginDate.Attributes.Add("onclick", "openModeBegin()");
            ButtonEndDate.Attributes.Add("onclick", "openModeEnd()");
            try
            {
                Label1.Text = Session["uid"].ToString();
                string szkey = Session["SzKey"].ToString();

                if (!classLibrary.ClassLib.ValidateRight("InfoCenter", this)) Response.Redirect("../login.aspx?wh=1");

                if (!IsPostBack)
                {
                    TextBoxBeginDate.Text = DateTime.Now.AddDays(-30).ToString("yyyy��MM��dd��");
                    TextBoxEndDate.Text = DateTime.Now.ToString("yyyy��MM��dd��");
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
                begindate = DateTime.Parse(TextBoxBeginDate.Text);
                enddate = DateTime.Parse(TextBoxEndDate.Text);
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

        private void BindData(int index)
        {
            try
            {
                pager.CurrentPageIndex = index;
                int limit = pager.PageSize;
                int page_id = index;
                Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
                string sppreno = this.TextSppreno.Text.Trim();//ƱԴ������
                string ticketno = this.TextTicketno.Text.Trim();//Ʊ��
                string transaction_id = this.TextTransaction_id.Text.Trim();//�Ƹ�ͨ���׵���
                string passenger_name = this.TextPassenger_name.Text.Trim();//�˻��� ����
                string cert_id = this.TextCert_id.Text.Trim();//�˻��� ֤������
                string name = this.TextName.Text.Trim();//��ϵ�� ���� 
                string mobile = this.TextMobile.Text.Trim();//��ϵ�� �ֻ����� 
                string uin = this.TextUin.Text.Trim();//�Ƹ�ͨ�˺�
                string insur_no = this.TextInsur_no.Text.Trim();//������
                string start_time = DateTime.Parse(TextBoxBeginDate.Text).ToString("yyyy-MM-dd");//������ʼʱ��
                string end_time = DateTime.Parse(TextBoxEndDate.Text).ToString("yyyy-MM-dd");//��������ʱ��
                string trade_type = this.ddlState.SelectedValue;//����״̬

                DataSet ds = qs.AirTicketsOrderQuery(sppreno, ticketno, transaction_id, passenger_name,
                 cert_id, name, mobile, uin, insur_no, start_time, end_time, trade_type, limit, page_id);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ds.Tables[0].Columns.Add("total_money_str", typeof(String));
                    classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "total_money", "total_money_str");
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
