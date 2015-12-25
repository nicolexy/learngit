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

namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
	/// <summary>
	/// PayBusinessQuery ��ժҪ˵����
	/// </summary>
    public partial class MediCertExpireQuery : System.Web.UI.Page
	{
        protected MerchantService merService = new MerchantService();
		protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				Label1.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();
				if(!classLibrary.ClassLib.ValidateRight("InfoCenter",this)) Response.Redirect("../login.aspx?wh=1");

				if(!IsPostBack)
				{
                    TextBoxBeginDate.Text = "";
                    TextBoxEndDate.Text = "";
               //     this.cmdSelectAll.Text = "ȫ��ѡ��";
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
            this.dgList.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.dgList_ItemCommand);
            this.pager.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(this.ChangePage);
         //   this.cmdSelectAll.Click += new System.EventHandler(this.cmdSelectAll_Click); 
        }

		#endregion
        public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            pager.CurrentPageIndex = e.NewPageIndex;
            try
            {
                BindData(e.NewPageIndex);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "��ȡ����ʧ�ܣ�" + PublicRes.GetErrorMsg(eSys.Message));
            }
        }

        private void ValidateDate()
        {
            DateTime begindate = new DateTime(), enddate = new DateTime();
            string s_date = TextBoxBeginDate.Text;
            string e_date = TextBoxEndDate.Text;
            if (string.IsNullOrEmpty(txtFspid.Text.Trim())&&string.IsNullOrEmpty(s_date)&&string.IsNullOrEmpty(e_date))
                throw new Exception("����������һ����ѯ������");
            if (s_date != null && s_date != "" && e_date != null && e_date != "")
            {
                try
                {
                    begindate = DateTime.Parse(s_date);
                    enddate = DateTime.Parse(e_date);
                }
                catch
                {
                    throw new Exception("������������");
                }
                if (System.Convert.ToDateTime(begindate) > System.Convert.ToDateTime(enddate))
                {
                    throw new Exception("��ʼʱ�䲻�ܴ�����ֹʱ��!");
                }
                //����ʱ���Ȳ�����31��
                if (System.Convert.ToDateTime(enddate) >= System.Convert.ToDateTime(begindate).AddDays(31))
                {
                    throw new Exception("��ѯʱ���Ȳ��ܳ���31��!");
                }
                ViewState["begindate"] = begindate.ToString("yyyy-MM-dd");
                ViewState["enddate"] = enddate.ToString("yyyy-MM-dd");
            }
            else
            {
                ViewState["begindate"] = "";
                ViewState["enddate"] = "";
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

			try
			{
              //  cmdSelectAll.Text = "ȫ��ѡ��";
				dgList.CurrentPageIndex = 0;
                this.pager.RecordCount = 1000;
             //   ViewState["dsInfo"] = null;
                BindData(1);
			}
			catch(SoapException eSoap) //����soap���쳣
			{
				string errStr = PublicRes.GetErrorMsg(eSoap.Message);
				WebUtils.ShowMessage(this.Page,"���÷������" + errStr);
			}
			catch(Exception eSys)
			{
				WebUtils.ShowMessage(this.Page,"��ȡ����ʧ�ܣ�" + PublicRes.GetErrorMsg(eSys.Message));
			}
		}

        private void BindData(int index)
        {
            this.pager.CurrentPageIndex = index;
            int max = pager.PageSize;
            int start = max * (index - 1);
            string begindate = ViewState["begindate"].ToString();
            string enddate = ViewState["enddate"].ToString();
            string spid = txtFspid.Text.Trim();

            // DataSet ds = new DataSet();

            DataSet dsInfo = new SPOAService().QueryExpiredCertificate(begindate, enddate, spid, max, start);
            if (dsInfo != null && dsInfo.Tables.Count > 0 && dsInfo.Tables[0].Rows.Count > 0)
            {
                dsInfo.Tables[0].Columns.Add("FmodifyTime", typeof(String));
                dsInfo.Tables[0].Columns.Add("Fmemo", typeof(String));
                dsInfo.Tables[0].Columns.Add("FupdateUser", typeof(String));
            }
            else
            {
                this.dgList.DataSource = null;
                this.dgList.DataBind();
                throw new LogicException("û���ҵ���¼��");
            }

            //  ViewState["dsInfo"] = dsInfo;//����spoa����ds

            //    ds = GetPagedDS(index, ds, dsInfo);//��ҳ������

            dsInfo = AddOperateInfo(dsInfo);
            this.dgList.DataSource = dsInfo.Tables[0].DefaultView;
            this.dgList.DataBind();
        }

        /// <summary>
        /// ��SPOA�ӿڷ��ص�dsÿһ�в�ѯ���ݿ��еĲ�����Ϣ
        /// </summary>
        /// <param name="ds">SPOA�ӿڷ��ص�ds</param>
        private DataSet AddOperateInfo(DataSet ds)
        {
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                string crt_etime = row["Fcrt_etime"].ToString();
                //Ϊ����֤�鵽������һ��
                DateTime date = new DateTime();
                try
                {
                    date = DateTime.Parse(crt_etime);
                }
                catch
                {
                    throw new Exception("֤�鵽����������");
                }
                crt_etime = date.ToString("yyyy-MM-dd HH:mm:ss");
                row["Fcrt_etime"] = crt_etime;

                string spid = row["Fspid"].ToString();
                DataSet dsOper = merService.QueryExpiredCertOperInfo(crt_etime, spid);
                if (dsOper != null && dsOper.Tables.Count > 0 && dsOper.Tables[0].Rows.Count > 0)
                {
                    row["FmodifyTime"] = dsOper.Tables[0].Rows[0]["FmodifyTime"].ToString();
                    row["Fmemo"] = dsOper.Tables[0].Rows[0]["Fmemo"].ToString();
                    row["FupdateUser"] = dsOper.Tables[0].Rows[0]["FupdateUser"].ToString();
                }
                else
                {
                    row["FmodifyTime"] = "";
                    row["Fmemo"] = "";
                    row["FupdateUser"] = "";
                }

            }
            return ds;
        }


        private DataSet GetPagedDS(int index, DataSet ds, DataSet dsInfo)
        {
            //���򼰷�ҳ
            DataTable dt = dsInfo.Tables[0];
            DataView view = dt.DefaultView;
            view.Sort = "Fcrt_etime asc";
            dt = view.ToTable();
            dt = PublicRes.GetPagedTable(dt, index, pager.PageSize);
            ds.Tables.Add(dt);//dsΪ��ҳ��Ľ��
            return ds;
        }
      
        private void dgList_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            string spid = e.Item.Cells[0].Text.Trim();
            string crt_etime = e.Item.Cells[6].Text.Trim();
            string memo = e.Item.Cells[7].Text.Trim();
            if (memo == "&nbsp;")
                memo = "";
            string modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            if (e.CommandName == "EDIT")
            {
                //Response.Redirect("MediCertExpireOperate.aspx?spid=" + spid + "&crt_etime=" + crt_etime);
                Response.Write("<script>window.open('MediCertExpireOperate.aspx?spid=" + spid + "&crt_etime=" + crt_etime + "&memo=" + memo + "','_blank')</script>");
            }
        }
	}
}
