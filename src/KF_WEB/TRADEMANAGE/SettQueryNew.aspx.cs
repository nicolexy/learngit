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
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;
using TENCENT.OSS.CFT.KF.Common;


namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
	/// <summary>
	/// SettQuery ��ժҪ˵����
	/// </summary>
	public partial class SettQueryNew : PageBase
	{
		protected Wuqi.Webdiyer.AspNetPager pager;

	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			btnBeginDate.Attributes.Add("onclick", "openModeBegin()"); 
			btnEndDate.Attributes.Add("onclick", "openModeEnd()"); 

			try
			{
				this.Label1.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();
				int operid = Int32.Parse(Session["OperID"].ToString());

				//if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "InfoCenter")) Response.Redirect("../login.aspx?wh=1");
				if(!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter",this)) Response.Redirect("../login.aspx?wh=1");
			}
			catch
			{
				Response.Redirect("../login.aspx?wh=1");
			} 

			if(!IsPostBack)
			{
				txtBeginDate.Text = DateTime.Now.ToString("yyyy��MM��dd��");
				txtEndDate.Text = DateTime.Now.ToString("yyyy��MM��dd��");
			}
		}

		private void ValidateDate()
		{
			DateTime begindate;
			DateTime enddate;

			try
			{
				begindate = DateTime.Parse(txtBeginDate.Text);
				enddate = DateTime.Parse(txtEndDate.Text);
			}
			catch
			{
				throw new Exception("������������");
			}

			if(begindate.CompareTo(enddate) > 0)
			{
				throw new Exception("��ֹ����С����ʼ���ڣ����������룡");
			}
			else if(begindate.AddMonths(1) < enddate)
			{
				throw new Exception("��ѡ���ڳ�����Χ��");
			}

			ViewState["begindate"] = DateTime.Parse(begindate.ToString("yyyy-MM-dd 00:00:00"));
			ViewState["enddate"] = DateTime.Parse(enddate.ToString("yyyy-MM-dd 23:59:59"));
			ViewState["Fspid"] = txtSpid.Text.Trim();
		}
		

		private void BindData()
		{
			DateTime begindate = (DateTime)ViewState["begindate"];
			DateTime enddate = (DateTime)ViewState["enddate"];
			string Fspid = ViewState["Fspid"].ToString();

			TENCENT.OSS.CFT.KF.KF_Web.classLibrary.Balance BalanceObj = new Balance(Fspid);
			DataSet ds = BalanceObj.GetBalanceList(Fspid,begindate,enddate);

			this.lblBalanceCount.Text = BalanceObj.UnbalanceCount.ToString();
			this.lblBalance.Text = classLibrary.setConfig.FenToYuan(BalanceObj.Unbalance.ToString());
			this.lblBalanceSuccessCount.Text = BalanceObj.TodayUnbalanceCount.ToString();
			this.lblBalanceSuccess.Text = classLibrary.setConfig.FenToYuan(BalanceObj.TodayUnbalance.ToString());
			this.lblRefundCount.Text = BalanceObj.TodayRefundCount.ToString();
			this.lblRefund.Text = classLibrary.setConfig.FenToYuan(BalanceObj.TodayRefund.ToString());
			this.lblHisBalanceCount.Text = BalanceObj.HisUnbalanceCount.ToString();
			this.lblHisBalance.Text = classLibrary.setConfig.FenToYuan(BalanceObj.HisUnbalance.ToString());

			if(BalanceObj.HisUnbalanceds != null && BalanceObj.HisUnbalanceds.Tables.Count >0)
			{
				UnbalancedgList.DataSource = BalanceObj.HisUnbalanceds.Tables[0].DefaultView;
				UnbalancedgList.DataBind();
				this.UnbalancedgList.Visible = true;
			}
			else
			{
				this.UnbalancedgList.Visible = false;
			}

			if(ds != null && ds.Tables.Count >0)
			{
				dgList.DataSource = ds.Tables[0].DefaultView;
				dgList.DataBind();
				this.dgList.Visible = true;
			}
			else
			{
				this.dgList.Visible = false;
			}
		}


		private void BindDataDetail()
		{
			TENCENT.OSS.CFT.KF.KF_Web.classLibrary.Balance BalanceObj = new Balance(ViewState["Fspid"].ToString());
			DataSet ds = BalanceObj.GetBalanceDetilList(ViewState["Fspid"].ToString(),ViewState["FDrawNo"].ToString());

			if(ds != null && ds.Tables.Count >0)
			{
				this.tableDetail.Visible = true;
				dgListDetail.DataSource = ds.Tables[0].DefaultView;
				dgListDetail.DataBind();
			}
			else
			{
				this.tableDetail.Visible = false;
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
			this.UnbalancedgList.PageIndexChanged += new System.Web.UI.WebControls.DataGridPageChangedEventHandler(this.UnbalancedgList_PageIndexChanged);
			this.UnbalancedgList.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.UnbalancedgList_ItemDataBound);
			this.dgList.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.dgList_ItemCommand);
			this.dgList.PageIndexChanged += new System.Web.UI.WebControls.DataGridPageChangedEventHandler(this.dgList_PageIndexChanged);
			this.dgList.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgList_ItemDataBound);
			this.dgListDetail.PageIndexChanged += new System.Web.UI.WebControls.DataGridPageChangedEventHandler(this.dgListDetail_PageIndexChanged);
			this.dgListDetail.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgListDetail_ItemDataBound);

		}
		#endregion

		
		//�õ��ѽ����¼(�ѻ��˽��,������,�����ѽ������ͬʱΪ0�����ε�(�ͷ�������ϵͳ��ͬ))
		private void dgList_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			if ( e.Item.ItemIndex > -1 )
			{
				e.Item.Cells[1].Text = Convert.ToString(e.Item.ItemIndex + 1 + dgList.CurrentPageIndex * dgList.PageSize);
				e.Item.Cells[3].Text = classLibrary.setConfig.FenToYuan(e.Item.Cells[3].Text.Trim());
				e.Item.Cells[4].Text = classLibrary.setConfig.FenToYuan(e.Item.Cells[4].Text.Trim());
				e.Item.Cells[5].Text = classLibrary.setConfig.FenToYuan(e.Item.Cells[5].Text.Trim());
			}
		}

		protected void btnSearch_Click(object sender, System.EventArgs e)
		{
			this.tableDetail.Visible = false;

			try
			{
				ValidateDate();
			}
			catch(Exception err)
			{
				this.UnbalancedgList.Visible = false;
				this.dgList.Visible = false;

				WebUtils.ShowMessage(this.Page,err.Message);
				return;
			}

			try
			{
				UnbalancedgList.CurrentPageIndex = 0;
				dgList.CurrentPageIndex = 0;
				BindData();
			}
			catch(SoapException eSoap) //����soap���쳣
			{
				this.UnbalancedgList.Visible = false;
				this.dgList.Visible = false;

				string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
				WebUtils.ShowMessage(this.Page,"���÷������" + errStr);
			}
			catch(Exception eSys)
			{
				this.UnbalancedgList.Visible = false;
				this.dgList.Visible = false;

				WebUtils.ShowMessage(this.Page,"��ȡ����ʧ�ܣ�" + eSys.Message.ToString());
			}
		}

		private void UnbalancedgList_PageIndexChanged(object source, System.Web.UI.WebControls.DataGridPageChangedEventArgs e)
		{
			this.UnbalancedgList.CurrentPageIndex = e.NewPageIndex;
			BindData();
		}

		private void dgList_PageIndexChanged(object source, System.Web.UI.WebControls.DataGridPageChangedEventArgs e)
		{
			this.dgList.CurrentPageIndex = e.NewPageIndex;
			BindData();
		}

		private void dgListDetail_PageIndexChanged(object source, System.Web.UI.WebControls.DataGridPageChangedEventArgs e)
		{
			this.dgListDetail.CurrentPageIndex = e.NewPageIndex;
			BindDataDetail();
		}
		
		private void dgList_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
		{
			if(e.CommandName == "Select")
			{
				try
				{
					ViewState["FDrawNo"] = e.Item.Cells[0].Text.Trim();

					dgListDetail.CurrentPageIndex = 0;
					BindDataDetail();
				}
				catch(Exception eSys)
				{
					this.tableDetail.Visible = false;

					WebUtils.ShowMessage(this.Page,"��ȡ����ʧ�ܣ�" + eSys.Message.ToString());
				}
			}
		}

		private string ProductTypeStr(string FProductType)
		{
			if(FProductType == "1")
				return "";
			else if(FProductType == "2")
				return "����֧��";
			else if(FProductType == "3")
				return "����B2C";
			else if(FProductType == "4")
				return "�ʻ�֧��";
			else if(FProductType == "5")
				return "����B2B";
			else if(FProductType == "6")
				return "С��";
			else if(FProductType == "7")
				return "�ж�";
			else if(FProductType == "8")
				return "���";
			else if(FProductType == "9")
				return "�߶�";
			else if(FProductType == "1001")
				return "Q��";
			else if(FProductType == "1002")
				return "��Ա";
			else if(FProductType == "1005")
				return "����";
			else if(FProductType == "1006")
				return "����";
			else if(FProductType == "1007")
				return "����";
			else if(FProductType == "1008")
				return "Q��50";
			else if(FProductType == "1009")
				return "Q��100";
			else if(FProductType == "1010")
				return "Q��150";
			else if(FProductType == "1011")
				return "Q��300";
			else if(FProductType == "1012")
				return "Q��500";
			else if(FProductType == "1013")
				return "����";
			else if(FProductType == "1014")
				return "����";
			else if(FProductType == "1015")
				return "512Ӳ��";
			else if(FProductType == "1016")
				return "����";
			else if(FProductType == "1017")
				return "����VIP";
			else if(FProductType == "1018")
				return "���ٰ���";
			else if(FProductType == "1019")
				return "������ۿ�";
			else
				return "";
		}
		private string ChannelStr(string FChannelNo)
		{
			if(FChannelNo == "0")
				return "�Ƹ�ͨ";
			else if(FChannelNo == "1")
				return "�ռ�";
			else
				return "����";
		}

		private string FeeItemStr(string FFeeItem)
		{
			if(FFeeItem == "1")
				return "��ͨ��";
			else if(FFeeItem == "2")
				return "���";
			else if(FFeeItem == "3")
				return "���";
			else if(FFeeItem == "4")
				return "������";
			else if(FFeeItem == "5")
				return "��Ʒ��";
			else if(FFeeItem == "6")
				return "Q������";
			else if(FFeeItem == "7")
				return "Q������֧��";
			else if(FFeeItem == "8")
				return "����";
			else if(FFeeItem == "9")
				return "��Ա";
			else if(FFeeItem == "10")
				return "��Ա��֤��";
			else if(FFeeItem == "11")
				return "��Ա������";
			else if(FFeeItem == "12")
				return "����";
			else if(FFeeItem == "13")
				return "Q��";
			else if(FFeeItem == "14")
				return "����";
			else if(FFeeItem == "15")
				return "����";
			else if(FFeeItem == "16")
				return "50Q��";
			else if(FFeeItem == "17")
				return "100Q��";
			else if(FFeeItem == "18")
				return "150Q��";
			else if(FFeeItem == "19")
				return "300Q��";
			else if(FFeeItem == "20")
				return "500Q��";
			else if(FFeeItem == "21")
				return "����";
			else if(FFeeItem == "22")
				return "����";
			else if(FFeeItem == "23")
				return "512Ӳ��";
			else if(FFeeItem == "24")
				return "����VIP";
			else if(FFeeItem == "25")
				return "���ٰ���";
			else if(FFeeItem == "26")
				return "�������";
			else if(FFeeItem == "27")
				return "QQ����";
			else
				return "����";
		}

		private string FeeStandardStr(string FFeeStandard,string FPerMolecule,string FPerDenominator)
		{
			int iRate;
			try
			{
				iRate = Convert.ToInt32(FFeeStandard);
			}
			catch
			{
				iRate = 0;
                return FFeeStandard;
			}

			if(iRate == 200)
				return "100%";
			else if (iRate > 100 && iRate < 200)
			{
				// �ٷֱ�
				int Flag = iRate - 100;
				return Flag.ToString() + "%";
			}
			else if (iRate > 1000 && iRate < 2000)
			{
				// ǧ�ֱ�
				int Flag = iRate - 1000;
				return Flag.ToString() + "��";
			}
			else if (iRate > 2100 && iRate < 2200)
			{
				// ÿ��XԪ
				int Flag = iRate - 2100;
				return Flag.ToString() + "Ԫ/��";
			}
			else
			{
				// ���ݱ�׼�Ų���
				if( FPerDenominator == "100" )
				{
					return FPerMolecule + "%";
				}
				else if( FPerDenominator == "1000" )
				{
					return FPerMolecule + "��";
				}
				else
					return "";
			}
		}


		private void UnbalancedgList_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			if(e.Item.ItemIndex > -1)
			{
				int Flag = UnbalancedgList.CurrentPageIndex * UnbalancedgList.PageSize + e.Item.ItemIndex + 1;
				e.Item.Cells[1].Text = Flag.ToString();
				e.Item.Cells[2].Text = ChannelStr(e.Item.Cells[2].Text.Trim());
				e.Item.Cells[3].Text = FeeItemStr(e.Item.Cells[3].Text.Trim());
				if(e.Item.Cells[4].Text.Trim().Length >= 10)
					e.Item.Cells[4].Text = e.Item.Cells[4].Text.Trim().Substring(0,10);
				if(e.Item.Cells[5].Text.Trim().Length >= 10)
					e.Item.Cells[5].Text = e.Item.Cells[5].Text.Trim().Substring(0,10);
				e.Item.Cells[7].Text = classLibrary.setConfig.FenToYuan(e.Item.Cells[7].Text.Trim());
				e.Item.Cells[8].Text = ProductTypeStr(e.Item.Cells[8].Text.Trim());
			}
		}

		private void dgListDetail_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			if(e.Item.ItemIndex > -1)
			{
				int Flag = dgListDetail.CurrentPageIndex * dgListDetail.PageSize + e.Item.ItemIndex + 1;
				e.Item.Cells[1].Text = Flag.ToString();
				e.Item.Cells[2].Text = ChannelStr(e.Item.Cells[2].Text.Trim());
				e.Item.Cells[3].Text = FeeItemStr(e.Item.Cells[3].Text.Trim());
				if(e.Item.Cells[4].Text.Trim().Length >= 10)
				    e.Item.Cells[4].Text = e.Item.Cells[4].Text.Trim().Substring(0,10);
				if(e.Item.Cells[5].Text.Trim().Length >= 10)
				    e.Item.Cells[5].Text = e.Item.Cells[5].Text.Trim().Substring(0,10);
				e.Item.Cells[7].Text = classLibrary.setConfig.FenToYuan(e.Item.Cells[7].Text.Trim());
				e.Item.Cells[8].Text = FeeStandardStr(e.Item.Cells[8].Text.Trim(),e.Item.Cells[9].Text.Trim(),e.Item.Cells[10].Text.Trim());
				e.Item.Cells[11].Text = classLibrary.setConfig.FenToYuan(e.Item.Cells[11].Text.Trim());
				e.Item.Cells[12].Text = ProductTypeStr(e.Item.Cells[12].Text.Trim());
			}
		}

	}
}
