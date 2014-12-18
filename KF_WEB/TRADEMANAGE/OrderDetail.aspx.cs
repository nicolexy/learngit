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
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using System.Web.Services.Protocols;
using TENCENT.OSS.CFT.KF.Common;


namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
	/// <summary>
	/// OrderDetail ��ժҪ˵����
	/// </summary>
	public partial class OrderDetail : System.Web.UI.Page
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��
			try
			{
				labUid.Text = Session["uid"].ToString();
			}
			catch
			{
				Response.Redirect("../login.aspx?wh=1");
			}

			if(!IsPostBack)
			{			
				string listid = Request.QueryString["listid"];

				if(listid == null || listid.Trim() == "")
				{
					WebUtils.ShowMessage(this.Page,"��������");
				}

				try
				{
					BindInfo(listid);
				}
				catch(LogicException err)
				{
					WebUtils.ShowMessage(this.Page,err.Message);
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


		private void BindInfo(string listid)
		{

			Query_Service.Query_Service qs = new Query_Service.Query_Service();

			DataSet ds =  qs.GetQueryListDetail(listid);

			if(ds != null && ds.Tables.Count >0 && ds.Tables[0].Rows.Count > 0 )
			{
				hlTrade.NavigateUrl = "TradeLogQuery.aspx?id=" + listid;
				//�����
				ds.Tables[0].Columns.Add("FpriceName");
				ds.Tables[0].Columns.Add("FcarriageName");
				ds.Tables[0].Columns.Add("FpaynumName");
				ds.Tables[0].Columns.Add("FfactName");
				ds.Tables[0].Columns.Add("FprocedureName");
				ds.Tables[0].Columns.Add("FcashName");
				ds.Tables[0].Columns.Add("FtokenName");
				ds.Tables[0].Columns.Add("Ffee3Name");
				ds.Tables[0].Columns.Add("FpaybuyName");
				ds.Tables[0].Columns.Add("FpaysaleName");
				//�������ֵ���
				ds.Tables[0].Columns.Add("Fpay_typeName");
				ds.Tables[0].Columns.Add("Ftrade_stateName");				
				ds.Tables[0].Columns.Add("FlstateName");
				ds.Tables[0].Columns.Add("Ftrade_typeName");
				ds.Tables[0].Columns.Add("Fadjust_flagName");
				//�ֹ�ת���
				ds.Tables[0].Columns.Add("Frefund_stateName");
				ds.Tables[0].Columns.Add("Fchannel_idName");
				ds.Tables[0].Columns.Add("Frefund_typeName");
				ds.Tables[0].Columns.Add("Fappeal_signName");
				ds.Tables[0].Columns.Add("Fmedi_signName");
				

				classLibrary.setConfig.FenToYuan_Table(ds.Tables[0],"Fprice","FpriceName");
				classLibrary.setConfig.FenToYuan_Table(ds.Tables[0],"Fcarriage","FcarriageName");
				classLibrary.setConfig.FenToYuan_Table(ds.Tables[0],"Fpaynum","FpaynumName");
				classLibrary.setConfig.FenToYuan_Table(ds.Tables[0],"Ffact","FfactName");
				classLibrary.setConfig.FenToYuan_Table(ds.Tables[0],"Fprocedure","FprocedureName");
				classLibrary.setConfig.FenToYuan_Table(ds.Tables[0],"Fcash","FcashName");
				classLibrary.setConfig.FenToYuan_Table(ds.Tables[0],"Ffee3","Ffee3Name");
				classLibrary.setConfig.FenToYuan_Table(ds.Tables[0],"Fpaybuy","FpaybuyName");
				classLibrary.setConfig.FenToYuan_Table(ds.Tables[0],"Fpaysale","FpaysaleName");
				classLibrary.setConfig.FenToYuan_Table(ds.Tables[0],"Ftoken","FtokenName");

				classLibrary.setConfig.GetColumnValueFromDic(ds.Tables[0],"Fpay_type","Fpay_typeName","PAY_TYPE");
				classLibrary.setConfig.GetColumnValueFromDic(ds.Tables[0],"Ftrade_state","Ftrade_stateName","PAY_STATE");
				classLibrary.setConfig.GetColumnValueFromDic(ds.Tables[0],"Flstate","FlstateName","PAY_LSTATE");
				classLibrary.setConfig.GetColumnValueFromDic(ds.Tables[0],"Ftrade_type","Ftrade_typeName","PAYLIST_TYPE");
				classLibrary.setConfig.GetColumnValueFromDic(ds.Tables[0],"Fadjust_flag","Fadjust_flagName","ADJUST_FLAG");
				//��ʾ������Ϣ
				DataRow drmain = ds.Tables[0].Rows[0];
				string strtmp = drmain["Frefund_state"].ToString();
				if(strtmp == "0")
				{
					drmain["Frefund_stateName"] = "��ʼ״̬";
				}
				else if(strtmp == "1")
				{
					drmain["Frefund_stateName"] = "�����˿�(�˿�ǰ)";
				}
				else if(strtmp == "2")
				{
					drmain["Frefund_stateName"] = "�˿�ɹ�";
				}
				else if(strtmp == "3")
				{
					drmain["Frefund_stateName"] = "�˿�ʧ��";
				}
				else if(strtmp == "4")
				{
					drmain["Frefund_stateName"] = "�����˿�ȴ�����ȷ��Э��";
				}
				else if(strtmp == "5")
				{
					drmain["Frefund_stateName"] = "����յ����������˻����ȴ�����ȷ��Э��";
				}
				else if(strtmp == "6")
				{
					drmain["Frefund_stateName"] = "��ң�Ҫ�˿���Ҳ�ͬ���˿�Э�飬�ȴ�����޸��˿�����";
				}
				else if(strtmp == "7")
				{
					drmain["Frefund_stateName"] = "����յ�����Ҫ�˻������Ҳ�ͬ���˻�Э�飬�ȴ�����޸��˿�����";
				}
				else if(strtmp == "8")
				{
					drmain["Frefund_stateName"] = "����ͬ���˻����ȴ���ҷ����˻�";
				}
				else if(strtmp == "9")
				{
					drmain["Frefund_stateName"] = "����ͬ���˿���׽������˿�ɹ���";
				}
				else if(strtmp == "10")
				{
					drmain["Frefund_stateName"] = "����ѷ��˻����ȴ�����ȷ��";
				}
				else if(strtmp == "11")
				{
					drmain["Frefund_stateName"] = "����ȷ���յ��˻������׽������˿�ɹ���";
				}
				else if(strtmp == "12")
				{
					drmain["Frefund_stateName"] = "����ѷ��˻������Ҳ�ͬ���˿�ȴ�����޸��˿�����";
				}
				else if(strtmp == "13")
				{
					drmain["Frefund_stateName"] = "����ѷ��˻���������޸��˿����룬�ȴ�����ȷ��";
				}
				else
				{
					drmain["Frefund_stateName"] = "δ֪����" + strtmp;
				}

				strtmp = drmain["Fchannel_id"].ToString();
				if(strtmp == "1")
				{
					drmain["Fchannel_idName"] = "�Ƹ�ͨ";
				}
				else if(strtmp == "2")
				{
					drmain["Fchannel_idName"] = "������";
				}
				else if(strtmp == "3")
				{
					drmain["Fchannel_idName"] = "�ͻ���СǮ��";
				}
				else if(strtmp == "4")
				{
					drmain["Fchannel_idName"] = "�ֻ�֧��";
				}
				else if(strtmp == "5")
				{
					drmain["Fchannel_idName"] = "������";
				}
				else
				{
					drmain["Fchannel_idName"] = "δ֪����" + strtmp;
				}

				strtmp = drmain["Frefund_type"].ToString();	
				drmain["Frefund_typeName"] = strtmp;


				strtmp = drmain["Fappeal_sign"].ToString();
				if(strtmp == "1")
				{
					drmain["Fappeal_signName"] = "����";
				}
				else if(strtmp == "2")
				{
					drmain["Fappeal_signName"] = "��ת����";
				}
				else
				{
					drmain["Fappeal_signName"] = "δ֪����" + strtmp;
				}

				strtmp = drmain["Fmedi_sign"].ToString();
				if(strtmp == "1")
				{
					drmain["Fmedi_signName"] = "���н齻��";
				}
				else if(strtmp == "2")
				{
					drmain["Fmedi_signName"] = "���н齻��";
				}
				else
				{
					drmain["Fmedi_signName"] = "δ֪����" + strtmp;
				}


				foreach(DataColumn dc in ds.Tables[0].Columns)
				{
					System.Web.UI.Control obj = FindControl(dc.ColumnName);
					if(obj != null)
					{
						Label lab = (Label)obj;
						lab.Text = drmain[dc.ColumnName].ToString();
					}
				}

				int iType = 1;

				DateTime beginTime = DateTime.Parse(PublicRes.sBeginTime);
				DateTime endTime   = DateTime.Parse(PublicRes.sEndTime);

				int istr = 1;
				int imax = 2;
		
				DataSet dsMemo = qs.GetPayList(listid,iType,beginTime,endTime,istr,imax);				
				if(dsMemo == null || dsMemo.Tables.Count<1 || dsMemo.Tables[0].Rows.Count<1) 
				{
					this.LB_Fexplain.Text = "��ѯʧ�ܣ�";
					this.LB_FMemo.Text = "��ѯʧ�ܣ�";
				}
				else
				{
					this.LB_FMemo.Text = dsMemo.Tables[0].Rows[0]["Fmemo"].ToString();
					this.LB_Fexplain.Text = dsMemo.Tables[0].Rows[0]["Fexplain"].ToString();
				}

				//��ʾͶ�ߵ���Ϣ.
				DataSet dsappeal = qs.GetAppealList(listid);
				if(dsappeal != null && dsappeal.Tables.Count >0)
				{
					dsappeal.Tables[0].Columns.Add("FstateName");
					dsappeal.Tables[0].Columns.Add("Fpunish_flagName");
					dsappeal.Tables[0].Columns.Add("Fcheck_stateName");
					dsappeal.Tables[0].Columns.Add("Fappeal_typeName");

					foreach(DataRow dr in dsappeal.Tables[0].Rows)
					{
						strtmp = dr["FState"].ToString();
						if(strtmp == "0")
						{
							dr["FstateName"] = "��ʼ״̬";
						}
						else if(strtmp == "1")
						{
							dr["FstateName"] = "����Ͷ��";
						}
						else if(strtmp == "2")
						{
							dr["FstateName"] = "Ͷ�ߴ�����";
						}
						else if(strtmp == "3")
						{
							dr["FstateName"] = "Ͷ�ߴ������";
						}
						else if(strtmp == "4")
						{
							dr["FstateName"] = "ȡ��Ͷ��";
						}
						else
						{
							dr["FstateName"] = "δ֪����" + strtmp;
						}

						strtmp = dr["Fpunish_flag"].ToString();
						if(strtmp == "1")
						{
							dr["Fpunish_flagName"] = "���账��";
						}
						else if(strtmp == "2")
						{
							dr["Fpunish_flagName"] = "�������";
						}
						else if(strtmp == "3")
						{
							dr["Fpunish_flagName"] = "��������";
						}
						else
						{
							dr["Fpunish_flagName"] = "δ֪����" + strtmp;
						}

						strtmp = dr["Fcheck_state"].ToString();
						if(strtmp == "1")
						{
							dr["Fcheck_stateName"] = "δ���";
						}
						else if(strtmp == "2")
						{
							dr["Fcheck_stateName"] = "���ύ���";
						}
						else if(strtmp == "3")
						{
							dr["Fcheck_stateName"] = "��˲�ͨ��";
						}
						else if(strtmp == "4")
						{
							dr["Fcheck_stateName"] = "���ͨ��";
						}
						else if(strtmp == "5")
						{
							dr["Fcheck_stateName"] = "���˿�";
						}
						else
						{
							dr["Fcheck_stateName"] = "δ֪����" + strtmp;
						}


						strtmp = dr["Fappeal_type"].ToString();
						if(strtmp == "1")
						{
							dr["Fappeal_typeName"] = "�ɽ�����";
						}
						else if(strtmp == "2")
						{
							dr["Fappeal_typeName"] = "�ջ������ȷ�ϣ�";
						}
						else if(strtmp == "3")
						{
							dr["Fappeal_typeName"] = "������Ͷ����ң��˿����";
						}
						else if(strtmp == "4")
						{
							dr["Fappeal_typeName"] = "��Ҷ�������";
						}
						else if(strtmp == "5")
						{
							dr["Fappeal_typeName"] = "�ɽ�����";
						}
						else if(strtmp == "6")
						{
							dr["Fappeal_typeName"] = "���Ҿܾ�ʹ�òƸ�ͨ����";
						}
						else if(strtmp == "7")
						{
							dr["Fappeal_typeName"] = "�տ����";
						}
						else if(strtmp == "8")
						{
							dr["Fappeal_typeName"] = "��Ʒ����������";
						}
						else if(strtmp == "9")
						{
							dr["Fappeal_typeName"] = "���Ҷ�������";
						}
						else if(strtmp == "10")
						{
							dr["Fappeal_typeName"] = "�����Ͷ�����ң��˿����";
						}
						else if(strtmp == "11")
						{
							dr["Fappeal_typeName"] = "����Ҫ�������ȷ���ջ�";
						}
						else
						{
							dr["Fappeal_typeName"] = "δ֪����" + strtmp;
						}
					}

					DataGrid1.DataSource = dsappeal.Tables[0].DefaultView;
				}

				//��ʾ������ˮ��Ϣ.
				DataSet dsuserpay = qs.GetUserpayList(listid);
				if(dsuserpay != null && dsuserpay.Tables.Count >0)
				{
					dsuserpay.Tables[0].Columns.Add("FtypeName");
					dsuserpay.Tables[0].Columns.Add("FsubjectName");
					dsuserpay.Tables[0].Columns.Add("FpaynumName");
					dsuserpay.Tables[0].Columns.Add("FpaybuyName");
					dsuserpay.Tables[0].Columns.Add("FpaysaleName");

					classLibrary.setConfig.GetColumnValueFromDic(dsuserpay.Tables[0],"Fsubject","FsubjectName","USERPAY_SUBJECT");
					foreach(DataRow dr in dsuserpay.Tables[0].Rows)
					{
						strtmp = dr["Ftype"].ToString();
						if(strtmp == "1")
						{
							dr["FtypeName"] = "��";
						}
						else if(strtmp == "2")
						{
							dr["FtypeName"] = "��";
						}						
						else
						{
							dr["FtypeName"] = "δ֪����" + strtmp;
						}

//						strtmp = dr["Fsubject"].ToString();
//						if(strtmp == "1")
//						{
//							dr["FsubjectName"] = "�������ɶ���";
//						}
//						else if(strtmp == "2")
//						{
//							dr["FsubjectName"] = "����������";
//						}	
//						else if(strtmp == "3")
//						{
//							dr["FsubjectName"] = "����֧���ɹ�";
//						}	
//						else if(strtmp == "4")
//						{
//							dr["FsubjectName"] = "���ҷ���";
//						}	
//						else if(strtmp == "5")
//						{
//							dr["FsubjectName"] = "��������˿�(����δ����)";
//						}	
//						else if(strtmp == "6")
//						{
//							dr["FsubjectName"] = "����ջ�ȷ�ϣ�δ�ã�";
//						}	
//						else if(strtmp == "7")
//						{
//							dr["FsubjectName"] = "�޸�������";
//						}	
//						else if(strtmp == "8")
//						{
//							dr["FsubjectName"] = "����ջ�ȷ��";
//						}	
//						else if(strtmp == "9")
//						{
//							dr["FsubjectName"] = "�޸Ķ����۸�";
//						}	
//						else if(strtmp == "10")
//						{
//							dr["FsubjectName"] = "��Ҫ��������";
//						}	
//						else if(strtmp == "11")
//						{
//							dr["FsubjectName"] = "��Ҫ����ȷ��";
//						}	
//						else if(strtmp == "12")
//						{
//							dr["FsubjectName"] = "��Ҫ����ܾ�";
//						}	
//						else if(strtmp == "13")
//						{
//							dr["FsubjectName"] = "���׹ر�";
//						}	
//						else if(strtmp == "14")
//						{
//							dr["FsubjectName"] = "ȡ���˿�";
//						}	
//						else if(strtmp == "15")
//						{
//							dr["FsubjectName"] = "���Ҳ�ͬ���˿��������˿";
//						}	
//						else if(strtmp == "16")
//						{
//							dr["FsubjectName"] = "����޸��˿����루��������˿";
//						}	
//						else if(strtmp == "17")
//						{
//							dr["FsubjectName"] = "��������˿�(�����ѷ��������δ�յ���)";
//						}	
//						else if(strtmp == "18")
//						{
//							dr["FsubjectName"] = "����ͬ���˿�Э�飬�������ѷ��������Ҫ�˻���";
//						}	
//						else if(strtmp == "19")
//						{
//							dr["FsubjectName"] = "��������";
//						}	
//						else if(strtmp == "20")
//						{
//							dr["FsubjectName"] = "ȡ������";
//						}	
//						else if(strtmp == "21")
//						{
//							dr["FsubjectName"] = "����ȷ���˿������δ������";
//						}	
//						else if(strtmp == "22")
//						{
//							dr["FsubjectName"] = "���׹���";
//						}	
//						else if(strtmp == "23")
//						{
//							dr["FsubjectName"] = "��������";
//						}	
//						else
//						{
//							dr["FsubjectName"] = "δ֪����" + strtmp;
//						}
					}

					classLibrary.setConfig.FenToYuan_Table(dsuserpay.Tables[0],"Fpaynum","FpaynumName");
					classLibrary.setConfig.FenToYuan_Table(dsuserpay.Tables[0],"Fpaybuy","FpaybuyName");
					classLibrary.setConfig.FenToYuan_Table(dsuserpay.Tables[0],"Fpaysale","FpaysaleName");

					DataGrid2.DataSource = dsuserpay.Tables[0].DefaultView;
				}

				//��ʾ������
				DataSet dstransport = qs.GetTransportList(listid);
				if(dstransport != null && dstransport.Tables.Count >0)
				{
					dstransport.Tables[0].Columns.Add("Ftransport_typeName");
					dstransport.Tables[0].Columns.Add("Fgoods_typeName");
					dstransport.Tables[0].Columns.Add("Ftran_typeName");
					dstransport.Tables[0].Columns.Add("FstateName");

					foreach(DataRow dr in dstransport.Tables[0].Rows)
					{
						strtmp = dr["Ftransport_type"].ToString();
						if(strtmp == "1")
						{
							dr["Ftransport_typeName"] = "��������";
						}
						else if(strtmp == "2")
						{
							dr["Ftransport_typeName"] = "�򷽷����˻�";
						}						
						else
						{
							dr["Ftransport_typeName"] = "δ֪����" + strtmp;
						}

						strtmp = dr["Fgoods_type"].ToString();
						if(strtmp == "1")
						{
							dr["Fgoods_typeName"] = "ʵ����Ʒ";
						}
						else if(strtmp == "2")
						{
							dr["Fgoods_typeName"] = "������Ʒ";
						}						
						else
						{
							dr["Fgoods_typeName"] = "δ֪����" + strtmp;
						}

						strtmp = dr["Ftran_type"].ToString();
						if(strtmp == "1")
						{
							dr["Ftran_typeName"] = "ƽ��";
						}
						else if(strtmp == "2")
						{
							dr["Ftran_typeName"] = "���";
						}			
						else if(strtmp == "3")
						{
							dr["Ftran_typeName"] = "email����";
						}
						else if(strtmp == "4")
						{
							dr["Ftran_typeName"] = "�ֻ�";
						}
						else if(strtmp == "5")
						{
							dr["Ftran_typeName"] = "����";
						}
						else
						{
							dr["Ftran_typeName"] = "δ֪����" + strtmp;
						}
					}

					Datagrid3.DataSource = dstransport.Tables[0].DefaultView;
				}
				this.DataBind();
			}
			else
			{
				throw new LogicException("û���ҵ���¼��");
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
