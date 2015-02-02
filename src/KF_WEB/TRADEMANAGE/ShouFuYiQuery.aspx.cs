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
	/// AgencyBusinessQuery ��ժҪ˵����
	/// </summary>
    public partial class ShouFuYiQuery : System.Web.UI.Page
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				Label1.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();
				//int operid = Int32.Parse(Session["OperID"].ToString());

				//if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID,"InfoCenter")) Response.Redirect("../login.aspx?wh=1");

				if(!ClassLib.ValidateRight("InfoCenter",this)) Response.Redirect("../login.aspx?wh=1");

				if(!IsPostBack)
				{
					this.divInfo.Visible = false;
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
			this.dgList.PageIndexChanged += new System.Web.UI.WebControls.DataGridPageChangedEventHandler(this.dgList_PageIndexChanged);

		}
		#endregion

		protected void btnSearch_Click(object sender, System.EventArgs e)
		{
			this.divInfo.Visible = false;

			try
			{
				dgList.CurrentPageIndex = 0;
				BindData();
			}
			catch(SoapException eSoap) //����soap���쳣
			{
				this.dgList.Visible = false;
				string errStr = PublicRes.GetErrorMsg(eSoap.Message);
				WebUtils.ShowMessage(this.Page,"���÷������" + errStr);
			}
			catch(Exception eSys)
			{
				this.dgList.Visible = false;
				WebUtils.ShowMessage(this.Page,"��ȡ����ʧ�ܣ�" + eSys.Message);
			}
		}

		private void BindData()
		{
			try
			{
                dgList.Visible = true;
				//Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
                //DataSet ds = qs.GetShouFuYiList(this.txtQQ.Text.Trim());
                DataSet ds = new SPOAService().GetShouFuYiList(this.txtQQ.Text.Trim()); //�޸�Ϊ�ӿڷ��ʷ�ʽ yinhuang 2014/04/14
                if (ds == null||ds.Tables.Count==0||ds.Tables[0] == null || ds.Tables[0].Rows.Count == 0)
                {
                    dgList.Visible = false;
                    WebUtils.ShowMessage(this.Page, "û�м�¼");
                    return;
                }

				dgList.DataSource = ds.Tables[0].DefaultView;
				dgList.DataBind();
			}
			catch(Exception ex)
			{
				dgList.Visible = false;
				throw new LogicException(ex.Message); 
			}
		}

		private void dgList_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
		{
            //if(e.CommandName == "Select")
            //{
            //    try
            //    {
            //        Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            //        DataSet ds = qs.GetAgencyBusinessInfo(e.Item.Cells[0].Text);
            //        if(ds.Tables[0].Rows.Count == 1)
            //        {
            //            this.divInfo.Visible = true;

            //            this.lblNo.Text = ds.Tables[0].Rows[0]["Fid"].ToString();
            //            this.lblQQ.Text = ds.Tables[0].Rows[0]["Fqqid"].ToString();
            //            this.lblURL.Text = ds.Tables[0].Rows[0]["Fdomain"].ToString();
            //            this.lblPhone.Text = ds.Tables[0].Rows[0]["Ftel"].ToString();
            //            this.lblMobile.Text = ds.Tables[0].Rows[0]["FMobile"].ToString();
            //            this.lblEmail.Text = ds.Tables[0].Rows[0]["Femail"].ToString();
            //            this.lblTradeType.Text = ds.Tables[0].Rows[0]["TradeName"].ToString();
            //            this.lblName.Text = ds.Tables[0].Rows[0]["FName"].ToString();
            //            this.lblAddress.Text = ds.Tables[0].Rows[0]["Faddress"].ToString();
            //            this.lblAddressNo.Text = ds.Tables[0].Rows[0]["Fpostcode"].ToString();
            //            this.lblRemark.Text = ds.Tables[0].Rows[0]["Fmemo"].ToString();
            //            this.lblCreateTime.Text = ds.Tables[0].Rows[0]["Fcreate_time"].ToString();
            //            this.lblModifyTime.Text = ds.Tables[0].Rows[0]["Fmodify_time"].ToString();
            //            this.lblApprovePersonID.Text = ds.Tables[0].Rows[0]["Fcheck_id"].ToString();
            //            this.lblApprovePerson.Text = ds.Tables[0].Rows[0]["Fcheck_user"].ToString();
            //            this.lblApproveTime.Text = ds.Tables[0].Rows[0]["Fcheck_time"].ToString();
            //            this.lblType.Text = ds.Tables[0].Rows[0]["DictName"].ToString();
            //            this.lblCommendatory.Text = ds.Tables[0].Rows[0]["Fsuggester"].ToString();
            //            this.lblOperateRemark.Text = ds.Tables[0].Rows[0]["Fop_memo"].ToString();
            //        }
            //        else
            //        {
            //            throw new Exception("���ݶ�ȡʧ��");
            //        }
            //    }
            //    catch(Exception ex)
            //    {
            //        throw new Exception(ex.Message);
            //    }
            //}
		}

		private void dgList_PageIndexChanged(object source, System.Web.UI.WebControls.DataGridPageChangedEventArgs e)
		{
			this.dgList.CurrentPageIndex = e.NewPageIndex;
			BindData();
		}
	}
}
