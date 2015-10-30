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
	/// SettleFreeze ��ժҪ˵����
	/// </summary>
	public partial class SettleFreeze : System.Web.UI.Page
	{
    
        string szSpid;
        string szQQid;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            // �ڴ˴������û������Գ�ʼ��ҳ��
            try
            {
                Label1.Text = Session["uid"].ToString();
                string szkey = Session["SzKey"].ToString();
                int operid = Int32.Parse(Session["OperID"].ToString());

                // if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID,"InfoCenter")) Response.Redirect("../login.aspx?wh=1");
				if(!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter",this)) Response.Redirect("../login.aspx?wh=1");
            }
            catch
            {
                Response.Redirect("../login.aspx?wh=1");
            }

               
            ButtonBeginDate.Attributes.Add("onclick", "openModeBegin()"); 
            ButtonEndDate.Attributes.Add("onclick", "openModeEnd()");

            if(!IsPostBack)
            {
                TextBoxBeginDate.Text = new DateTime(DateTime.Today.Year,DateTime.Today.Month,1).ToString("yyyy��MM��dd��");
                TextBoxEndDate.Text = DateTime.Now.ToString("yyyy��MM��dd��");
            }

        }
 
        private void CheckData()
        {
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
            
            if(this.txtFspid.Text.Trim() == "" && this.txtqqid.Text.Trim() == "")
            {
                throw new Exception("�������̻��Ż��߶����ʻ�");
            }
            
            szSpid = this.txtFspid.Text.Trim();
            szQQid = this.txtqqid.Text.Trim();

        }

        public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            pager.CurrentPageIndex = e.NewPageIndex;
            BindData(e.NewPageIndex);
        }

        private string getRole(int iRole)
        {
            string szRole;
            switch(iRole)
            {
                case 1:
                    szRole = "��Ӧ��";
                    break;
                case 2:
                    szRole = "ƽ̨��";
                    break;
                case 3:
                    szRole = "�Ƹ�ͨ";
                    break;
                case 4:
                    szRole = "��������";
                    break;
                case 5:
                    szRole = "������";
                    break;
                default :
                    szRole = string.Format("δ֪��ɫ��%d", iRole);
                    break;
            }
            return szRole;
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
                    szType = "�ⶳ";
                    break;
                default :
                    szType = string.Format("δ�������ͣ�%d", iType);
                    break;
            }
            return szType;
        }


        private void BindData(int index)
        {
            int max = pager.PageSize;
            int start = max * (index-1);

            //Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            //DataSet ds;
            //ds = qs.GetAirFreeze(this.txtFspid.Text.Trim(), this.txtqqid.Text.Trim(), ViewState["begindate"].ToString(), ViewState["enddate"].ToString(), start,max);
            SettleService service = new SettleService();
            DataTable dt = service.GetAirFreeze(this.txtFspid.Text.Trim(), this.txtqqid.Text.Trim(), ViewState["begindate"].ToString(), ViewState["enddate"].ToString(), start, max);
 
            dt.Columns.Add("listid",typeof(string));
            dt.Columns.Add("spListid",typeof(string));
            dt.Columns.Add("FreezeId",typeof(string));
            dt.Columns.Add("qqid",typeof(string));
            dt.Columns.Add("role",typeof(string));
            dt.Columns.Add("total",typeof(string));
            dt.Columns.Add("type",typeof(string));
            dt.Columns.Add("TimeStr",typeof(string));

            foreach(DataRow dr in dt.Rows)
            {
                dr["listid"] = dr["Ftransaction_id"].ToString();
                
                dr["FreezeId"] = dr["Frefund_id"].ToString();
                
                dr["role"] = getRole(int.Parse(dr["Frole"].ToString()));
                dr["total"] =  MoneyTransfer.FenToYuan(dr["Fnum"].ToString());
                dr["type"] = getTypeStr(int.Parse(dr["Ftype"].ToString()));
                dr["TimeStr"] = dr["Fcreate_time"].ToString();
                string spid = dr["Ftransaction_id"].ToString().Substring(0, 10);
                if (!PublicRes.isWhiteOfSeparate(spid, Session["uid"].ToString()))
                {
                    //���ڰ����� yinhuang
                    dr["spListid"] = classLibrary.setConfig.ConvertID(dr["Fsp_bill_no"].ToString(), 0, 4);
                    string s_qqid = dr["Fqqid"].ToString();
                    int idx = s_qqid.IndexOf("@");
                    if (idx > -1)
                    {
                        //email,����@ǰ�ĺ�2λ+@��
                        string s_st = s_qqid.Substring(0, idx);
                        string s_et = s_qqid.Substring(idx + 1);
                        dr["qqid"] = classLibrary.setConfig.ConvertID(s_st, 0, 2) + "@" + s_et;
                    }
                    else
                    {
                        //���֣��������2λ
                        dr["qqid"] = classLibrary.setConfig.ConvertID(s_qqid, 0, 2);
                    }
                }
                else {
                    dr["spListid"] = dr["Fsp_bill_no"].ToString();
                    dr["qqid"] = dr["Fqqid"].ToString(); //�˺�
                }
            }
            this.DataGrid1.DataSource = dt.DefaultView;
            this.DataGrid1.DataBind();

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
	}
}
