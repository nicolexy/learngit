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
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.CFT.KF.KF_Web;
using System.Web.Services.Protocols;
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;

namespace TENCENT.OSS.CFT.KF.KF_Web.NewQueryInfoPages
{
	/// <summary>
    /// QueryFreeFlow ��ժҪ˵����
	/// </summary>
    public partial class QueryFreeFlow : System.Web.UI.Page
	{
        protected void Page_Load(object sender, System.EventArgs e)
		{

			try
			{
				Label1.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();

                if (!IsPostBack)
                {
                    
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
			//this.pager.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(this.ChangePage);

		}
		#endregion

		/*public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
		{
			pager.CurrentPageIndex = e.NewPageIndex;
			BindData(e.NewPageIndex);
		}*/

		private void ValidateDate()
		{
			
            string ccftno = cftNo.Text.ToString();
            if (ccftno == "")
            {
                throw new Exception("�������ѯ�");
            }
            
		}

        public void btnQuery_Click(object sender, System.EventArgs e)
		{
			try
			{
				ValidateDate();
			}
			catch(Exception err)
			{
				WebUtils.ShowMessage(this.Page,err.Message);
				return;
			}
            
			try
			{
                clearDetailTB();
				BindData();
			}
			catch(SoapException eSoap) //����soap���쳣
			{
				string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
				WebUtils.ShowMessage(this.Page,"���÷������" + errStr);
			}
			catch(Exception eSys)
			{
				WebUtils.ShowMessage(this.Page,"��ȡ����ʧ�ܣ�" + eSys.Message.ToString());
			}
		}

        private void BindData()
		{
            string s_cftno = cftNo.Text.Trim();

            lb_c1.Text = s_cftno;

			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            qs.Finance_HeaderValue = classLibrary.setConfig.setFH(this);
            DataSet ds = null;
            
            //1.��ѯ��Ա
            ds = qs.QueryCFTMember(s_cftno);
            if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count != 1)
            {
                //��Ա������
                lb_c2.Text = "��";
            }
            else {
                DataRow row = ds.Tables[0].Rows[0];
                //lb_c1.Text = row["Fuin"].ToString();
                lb_c2.Text = "��";//�Ƿ�ΪQQ��Ա
                lb_c3.Text = row["Fvip_exp_date"].ToString();//��Ա����ʱ��
            }
            
            //2.��ѯʵ����֤
            ds = qs.GetUserAuthenState(s_cftno, "", 0);
            if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count != 1)
            {
                lb_c4.Text = "��";//�Ƿ�ʵ����֤
            }
            else {
                DataRow row = ds.Tables[0].Rows[0];
                if (row["queryType"].ToString() == "2")
                {
                    lb_c4.Text = "��";
                }
                else {
                    lb_c4.Text = "��";
                }
            }
            
            //3.�������
            ds = qs.GetFreeFlowInfo(s_cftno);
            if (ds != null && ds.Tables.Count>0)
            {
                DataTable dt = ds.Tables[0];
                lb_c6.Text = classLibrary.setConfig.FenToYuan(dt.Rows[0]["free_amount"].ToString());//�������
            }
			
            //4.������������û�
            ds = qs.GetUserTypeInfo(s_cftno,3,1,0,1,1); //1ת�˰�����
            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                string ys = dt.Rows[0]["eip_user"].ToString();
                if (ys == "Y") {
                    lb_c5.Text = "��";
                }
                else if (ys == "N")
                {
                    lb_c5.Text = "��";
                }
                else {
                    lb_c5.Text = "";//�Ƿ�Ϊ������
                }
                
            }
            else {
                lb_c5.Text = "��";
            }

            ds = qs.GetUserTypeInfo(s_cftno, 5, 1, 0, 1, 1); //0���ִ���û�
            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                string ys = dt.Rows[0]["eip_user"].ToString();
                if (ys == "Y")
                {
                    lb_c7.Text = "��";
                }
                else if (ys == "N")
                {
                    lb_c7.Text = "��";
                }
                else
                {
                    lb_c7.Text = "";//�Ƿ�Ϊ����û�
                }

            }
            else
            {
                lb_c7.Text = "��";
            }
		}

        private void clearDetailTB() {
            lb_c1.Text = "";
            lb_c2.Text = "";
            lb_c3.Text = "";
            lb_c4.Text = "";
            lb_c5.Text = "";
            lb_c6.Text = "";
            lb_c7.Text = "";
        }
	}
}