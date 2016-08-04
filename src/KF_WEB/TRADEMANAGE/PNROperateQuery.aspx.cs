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
using CFT.CSOMS.BLL.SPOA;
using CFT.CSOMS.BLL.PNRModule;
using CFT.CSOMS.COMMLIB;

namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
	/// <summary>
	/// PayBusinessQuery ��ժҪ˵����
	/// </summary>
    public partial class PNROperateQuery : TENCENT.OSS.CFT.KF.KF_Web.PageBase
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				Label1.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();
				//int operid = Int32.Parse(Session["OperID"].ToString());

				//if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID,"InfoCenter")) Response.Redirect("../login.aspx?wh=1");

				if(!classLibrary.ClassLib.ValidateRight("InfoCenter",this)) Response.Redirect("../login.aspx?wh=1");


				if(!IsPostBack)
				{
                    //���������б�
                    setConfig.GetAllDownListDic(ddlAgent, "airCompay");
                    ddlAgent.SelectedValue = "";
                    setConfig.GetAllDownListDic(ddlStatus, "PNRStatus");
                    ddlStatus.SelectedValue = "";

                    this.txtstartTime.Text = DateTime.Now.AddMinutes(-10).ToString("yyyy-MM-dd HH:mm:ss");
                    this.txtendTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
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
		

        public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            pager.CurrentPageIndex = e.NewPageIndex;
            BindData(e.NewPageIndex);
        }
        #endregion

        private void ValidateDate()
        {
            string pnr = txtPNR.Text.ToString();
            string startTime = txtstartTime.Text.Trim();
            string endTime = txtendTime.Text.Trim();
            if (string.IsNullOrEmpty(pnr.Trim()) && string.IsNullOrEmpty(startTime) && string.IsNullOrEmpty(endTime))
            {
                throw new Exception("������PNR��ʱ���ѯ��");
            }

            if (!string.IsNullOrEmpty(startTime) && !string.IsNullOrEmpty(endTime))
            {
                DateTime begin;
                DateTime end;

                try
                {
                    begin = DateTime.Parse(startTime);
                    end = DateTime.Parse(endTime);
                    ViewState["begin"] = begin;
                    ViewState["end"] = end;
                }
                catch
                {
                    throw new Exception("ʱ����������");
                }

                if (begin.CompareTo(end) > 0)
                {
                    throw new Exception("����ʱ��С�ڿ�ʼʱ�䣬���������룡");
                }

                if (!classLibrary.getData.IsTestMode)
                    if (begin.AddHours(1) < end)
                    {
                        throw new Exception("ʱ��������1Сʱ�����������룡");
                    }
            }
            else
            {
                ViewState["begin"] = null;
                ViewState["end"] = null;
            }
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

                this.pager.RecordCount = 1000;
				dgList.CurrentPageIndex = 0;
                BindData(1);
		}

        private void BindData(int index)
        {
            try
            {
                this.pager.CurrentPageIndex = index;
                int max = pager.PageSize;
                int start = max * (index - 1);
                string pnr = txtPNR.Text.ToString().Trim();

                int startTime = 0, endTime = 0;
                if (ViewState["begin"] != null && ViewState["end"] != null)
                {
                     startTime = CommUtil.ConvertDateTimeInt((DateTime)ViewState["begin"]);
                     endTime = CommUtil.ConvertDateTimeInt((DateTime)ViewState["end"]);
                }

                DataTable dt = new PNRService().QueryPNROperate(pnr, startTime, endTime, ddlAgent.SelectedValue, ddlStatus.SelectedValue, start, max);
                if (dt != null && dt.Rows.Count > 0)
                {
                    dgList.DataSource = dt.DefaultView;
                    dgList.DataBind();
                }
                else
                {
                    dgList.DataSource = null;
                    WebUtils.ShowMessage(this.Page, "û���ҵ���¼��");
                }
            }
            catch (Exception ex)
            {
                string errStr = PublicRes.GetErrorMsg(ex.Message);
                WebUtils.ShowMessage(this.Page, "��ȡ����ʧ�ܣ�" + errStr);
            }
        }
	}
}
