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
using CFT.CSOMS.BLL.TradeModule;

namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
	/// <summary>
	/// AdjustList ��ժҪ˵����
	/// </summary>
	public partial class AdjustList : TENCENT.OSS.CFT.KF.KF_Web.PageBase
	{
    
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��
            try
            {
                Label1.Text = Session["uid"].ToString();
                string szkey = Session["SzKey"].ToString();
                int operid = Int32.Parse(Session["OperID"].ToString());

                //if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID,"InfoCenter")) Response.Redirect("../login.aspx?wh=1");
				if(!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter",this)) Response.Redirect("../login.aspx?wh=1");
            }
            catch
            {
                Response.Redirect("../login.aspx?wh=1");
            }

            if(!IsPostBack)
            {
                this.rtnList.Checked = true;
                TextBoxBeginDate.Text = new DateTime(DateTime.Today.Year,DateTime.Today.Month,1).ToString("yyyy-MM-dd");
                TextBoxEndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            }
		}

        private void CheckData()
        {
            ViewState["IsrtnList"] = true;
            
            ViewState["queryType"] = 0;
            ViewState["Flistid"] = ""; 
            ViewState["Fspid"] = "";
            ViewState["begindate"] = "";
            ViewState["enddate"] = "";

            if(this.rtnList.Checked)
            {
                if(this.txtFlistid.Text.Trim().Length != 28)
                    throw new Exception("������28λ�����ţ�");
                else
                    ViewState["Flistid"] = this.txtFlistid.Text.Trim();

                ViewState["queryType"] = 1;
            }
            else if(this.rtbSpid.Checked)
            {
                ViewState["IsrtnList"] = false;

                DateTime begindate;
                DateTime enddate;
                try
                {
                    begindate = DateTime.Parse(TextBoxBeginDate.Text);
                    enddate = DateTime.Parse(TextBoxEndDate.Text);
                    ViewState["begindate"] = begindate;
                    ViewState["enddate"] = enddate;
                }
                catch
                {
                    throw new Exception("������������");
                }

                if(begindate.CompareTo(enddate) > 0)
                {
                    throw new Exception("��ֹ����С����ʼ���ڣ����������룡");
                }

                if(begindate.Year != enddate.Year || begindate.Month != enddate.Month)
                {
                    throw new Exception("��������²�ѯ��");
                }

                if(this.txtFspid.Text.Trim() == "")
                    throw new Exception("�������̻��ţ�");
                else
                    ViewState["Fspid"] = this.txtFspid.Text.Trim();

                ViewState["queryType"] = 3;
            }
            else if(this.rtbSpList.Checked)
            {
                if(this.txtSpList.Text.Trim().Length == 0)
                    throw new Exception("�������̻������ţ�");
                else
                    ViewState["Flistid"] = this.txtSpList.Text.Trim();
                ViewState["queryType"] = 2;
            }
            else
                throw new Exception("��ѡ��һ�ֲ�ѯ��ʽ��");
        }


        private string getTypeStr(int iType)
        {
            string szType;
            switch(iType)
            {
                case 1:
                    szType = "����";
                    break;
                case 2:
                    szType = "����";
                    break;
                default :
                    szType = string.Format("δ�������ͣ�%d", iType);
                    break;
            }
            return szType;
        }

        private string getStatusStr(int iStatus)
        {
            string szStatus;
            switch(iStatus)
            {
                case 1:
                    szStatus = "��ʼ״̬";
                    break;
                case 2:
                    szStatus = "������";
                    break;
                case 3:
                    szStatus = "������";
                    break;
                case 4:
                    szStatus = "�ɹ�";
                    break;
                case 5:
                    szStatus = "ʧ��";
                    break;
                default :
                    szStatus = string.Format("δ�������ͣ�%d", iStatus);
                    break;
            }
            return szStatus;
        }

        private void BindData(int index)
        {
            int max = pager.PageSize;
            int start = max * (index-1);


            //Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            //DataSet ds;
            //ds = qs.GetAirAdjustList(int.Parse(ViewState["queryType"].ToString()), 
            //                            ViewState["Flistid"].ToString(), 
            //                            ViewState["Fspid"].ToString(),
            //                            ViewState["begindate"].ToString(),
            //                            ViewState["enddate"].ToString(),
            //                            start,
            //                            max);
            SettleService service = new SettleService();
            DataTable dt = service.GetAirAdjustList(int.Parse(ViewState["queryType"].ToString()),
                                       ViewState["Flistid"].ToString(),
                                      ViewState["Fspid"].ToString(),
                                     ViewState["begindate"].ToString(),
                                     ViewState["enddate"].ToString(),
                                      start,
                                     max);

 
            dt.Columns.Add("listid",typeof(string));
            dt.Columns.Add("spListid",typeof(string));
            dt.Columns.Add("TimeStr",typeof(string));
            dt.Columns.Add("total",typeof(string));
            dt.Columns.Add("type",typeof(string));
            dt.Columns.Add("status",typeof(string));


            foreach(DataRow dr in dt.Rows)
            {
                dr["listid"] = dr["Ftransaction_id"].ToString();
                //dr["spListid"] = dr["Fsp_bill_no"].ToString();
                dr["TimeStr"] = dr["Fadjust_time"].ToString();
                dr["total"] =  MoneyTransfer.FenToYuan(dr["Fnum"].ToString());
                dr["type"] = getTypeStr(int.Parse(dr["Ftype"].ToString()));
                dr["status"] = getStatusStr(int.Parse(dr["Fstatus"].ToString()));

                //�Զ����Ž������ش��� yinhuang
                string spid = dr["Ftransaction_id"].ToString().Substring(0, 10);
                if (!PublicRes.isWhiteOfSeparate(spid, Session["uid"].ToString())) 
                {
                    dr["spListid"] = classLibrary.setConfig.ConvertID(dr["Fsp_bill_no"].ToString(), 0, 4);
                }
                else
                {
                    dr["spListid"] = dr["Fsp_bill_no"].ToString();
                }
            }
            this.DataGrid1.DataSource = dt.DefaultView;
            this.DataGrid1.DataBind();

        }

        public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            pager.CurrentPageIndex = e.NewPageIndex;
            BindData(e.NewPageIndex);
        }

        protected void btnQuery_Click(object sender, System.EventArgs e)
        {
            try
            {
                CheckData();
            }
            catch(Exception err)
            {
                WebUtils.ShowMessage(this.Page,err.Message);
                return;
            }

            try
            {
                pager.RecordCount= 10000;
                BindData(1);
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

        }
		#endregion
	}
}
