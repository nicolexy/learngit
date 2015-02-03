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
using System.Configuration;


namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
	/// <summary>
	/// PhoneBillQuery ��ժҪ˵����
	/// </summary>
	public partial class PhoneBillQuery : System.Web.UI.Page
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
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
			this.dgList.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgList_ItemDataBound);
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
			catch(SoapException eSoap) //����soap���쳣
			{
				string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
				WebUtils.ShowMessage(this.Page,"���÷������" + errStr);
			}
			catch(Exception eSys)
			{
				WebUtils.ShowMessage(this.Page,"��ȡ����ʧ�ܣ�" + classLibrary.setConfig.replaceMStr(eSys.Message) );
			}
		}


		private void BindData(int index)
		{
			string strOutMsg = "";

			int max = pager.PageSize;
			int start = max * (index-1) + 1;

			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
			Query_Service.Finance_Header fh = classLibrary.setConfig.setFH(this);
			qs.Finance_HeaderValue = fh;

			DataSet ds;
			if(rbtFlistID.Checked)
			{

				ds = qs.GetPhoneBillRecordByTransID(tbListID.Text.Trim(), out strOutMsg);
							
			}
			else
			{
				ds = qs.GetPhoneBillRecordByPhoneNumber(tbPhoneNo.Text.Trim(), out strOutMsg);
			}
			

				if(ds != null && ds.Tables.Count >0)
				{
					ds.Tables[0].Columns.Add("FTotalFee_yuan",typeof(String));
					ds.Tables[0].Columns.Add("FAmount_yuan",typeof(String));
					ds.Tables[0].Columns.Add("FComment_conv",typeof(String));
					ds.Tables[0].Columns.Add("Fstate_conv",typeof(String));
					ds.Tables[0].Columns.Add("FSpName_conv", typeof(String));
                    ds.Tables[0].Columns.Add("FUserStateName", typeof(String));//�û���¼״̬

					foreach (DataRow dr in ds.Tables[0].Rows)
					{
						dr.BeginEdit();

						dr["FTotalFee_yuan"]	= MoneyTransfer.FenToYuan(dr["FTotalFee"].ToString());
						dr["FAmount_yuan"]		= MoneyTransfer.FenToYuan(dr["FAmount"].ToString());
                      
                        switch (dr["FUserState"].ToString())
                        {
                            case "0":
                                dr["FUserStateName"] = "������ֵ"; break;
                            case "1":
                                dr["FUserStateName"] = "QQ�û���ֵ"; break;
                            case "2":
                                dr["FUserStateName"] = "QQע���û���ֵ"; break;
                            case "3":
                                dr["FUserStateName"] = "EMAILע���û���ֵ"; break;
                            default:
                                dr["FUserStateName"] = "δ֪"; break;
                        }

						switch (dr["FComment"].ToString().Substring(0,1))
						{
							case "1":
								dr["FComment_conv"] = "���";break;
							case "2":
								dr["FComment_conv"] = "����(48Сʱ)";break;
							case "3":
								dr["FComment_conv"] = "����(12Сʱ)";break;
							case "4":
								dr["FComment_conv"] = "����(2Сʱ)";break;
							default:
								dr["FComment_conv"] = "δ֪";break;
						}


						switch( dr["Fstate"].ToString())
						{
							case "1":
								dr["Fstate_conv"] = "Ԥ��ѯ";break;
							case "2":
								dr["Fstate_conv"] = "Ԥ��ֵ";break;
							case "3":
								dr["Fstate_conv"] = "��ֵ�����ͳɹ�";break;
							case "4":
								dr["Fstate_conv"] = "��ֵ������ʧ��";break;
							case "13":
							case "14":
								dr["Fstate_conv"] = "��ֵ�ɹ�";break;						
							case "23":
							case "24":
								dr["Fstate_conv"] = "���˿�";break;		
							case "33":
							case "34":
								dr["Fstate_conv"] = "��ֵʧ��";break;
							default:
								dr["Fstate_conv"] = "״̬λ��:"+dr["Fstate"].ToString();
								break;
						}

						switch( dr["FSpName"].ToString() )
						{
							case "gy":
								dr["FSpName_conv"] = "������Ѹ";break;
							case "jtd":
								dr["FSpName_conv"] = "��ͨ��";break;
							case "zt":
								dr["FSpName_conv"] = "����";break;
							case "of":
								dr["FSpName_conv"] = "ŷ��";break;
							default:
								dr["FSpName_conv"] = dr["FSpName"].ToString();
								break;

						}

						dr.EndEdit();
					}



					dgList.DataSource = ds.Tables[0].DefaultView;
					dgList.DataBind();
				}
				else
				{
					throw new LogicException("û���ҵ���¼��"+strOutMsg);
				}

		}

		private void dgList_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
// 			if(e.Item.ItemIndex > -1)
// 			{
// 				if(e.Item.Cells[0].Text.Trim() != "")
// 					e.Item.Cells[0].Text = DateTime.Parse(e.Item.Cells[0].Text.Trim()).ToString("yyyy-MM-dd");
// 				if(e.Item.Cells[2].Text.Trim() == "3001")
// 					e.Item.Cells[3].Text = "��ҵ����";
// 				if(e.Item.Cells[5].Text.Trim() != "")
// 					e.Item.Cells[5].Text = MoneyTransfer.FenToYuan(e.Item.Cells[5].Text.Trim());
// 				if(e.Item.Cells[6].Text.Trim() == "1")
// 					e.Item.Cells[6].Text = "�ɹ�";
// 				else if(e.Item.Cells[6].Text.Trim() == "2")
// 					e.Item.Cells[6].Text = "ʧ��";
// 				else if(e.Item.Cells[6].Text.Trim() == "3" || e.Item.Cells[6].Text.Trim() == "4")
// 					e.Item.Cells[6].Text = "������";
// 				else
// 					e.Item.Cells[6].Text = "Unknow";
//			}
		}

		protected void Textbox1_TextChanged(object sender, System.EventArgs e)
		{
		
		}

		protected void btnSearch_Click(object sender, System.EventArgs e)
		{
			try
			{
				BindData(1);
			}
			catch(SoapException eSoap) //����soap���쳣
			{
				string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
				WebUtils.ShowMessage(this.Page,"���÷������" + errStr);
			}
			catch(Exception eSys)
			{
				WebUtils.ShowMessage(this.Page,"��ȡ����ʧ�ܣ�" + classLibrary.setConfig.replaceMStr(eSys.Message));
			}
		}
	}
}
