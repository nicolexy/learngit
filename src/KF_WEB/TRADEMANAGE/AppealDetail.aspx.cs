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
	/// AppealDetail ��ժҪ˵����
	/// </summary>
	public partial class AppealDetail : TENCENT.OSS.CFT.KF.KF_Web.PageBase
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
				
				string tdeid = Request.QueryString["appealid"];

				if(tdeid == null || tdeid.Trim() == "")
				{
					WebUtils.ShowMessage(this.Page,"��������");
				}


				try
				{
					BindInfo(tdeid);
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

		private void BindInfo(string appealid)
		{
			
			Query_Service.Query_Service qs = new Query_Service.Query_Service();

			DataSet ds =  qs.GetAppealListDetail(appealid);

			if(ds != null && ds.Tables.Count >0 && ds.Tables[0].Rows.Count > 0 )
			{//�����
				ds.Tables[0].Columns.Add("Ftotal_feeName");
				ds.Tables[0].Columns.Add("Ftoken_feeName");
				ds.Tables[0].Columns.Add("FpaybuyName");
				ds.Tables[0].Columns.Add("FpaysaleName");
				//�������ֵ���
				ds.Tables[0].Columns.Add("Flist_stateName");
				ds.Tables[0].Columns.Add("Freturn_stateName");					
				ds.Tables[0].Columns.Add("Flist_state_nextName");
				ds.Tables[0].Columns.Add("Freturn_state_nextName");
				ds.Tables[0].Columns.Add("FlstateName");
			

				
				//�ֹ�ת���
				ds.Tables[0].Columns.Add("Ftran_stateName");
				ds.Tables[0].Columns.Add("Ftran_state_nextName");
				ds.Tables[0].Columns.Add("FstateName");
				ds.Tables[0].Columns.Add("Fresponse_flagName");
				ds.Tables[0].Columns.Add("Fpunish_flagName");
				ds.Tables[0].Columns.Add("Fcheck_stateName");
				ds.Tables[0].Columns.Add("Ffund_flagName");
				ds.Tables[0].Columns.Add("Fappeal_typeName");

				
				

				classLibrary.setConfig.FenToYuan_Table(ds.Tables[0],"Ftotal_fee","Ftotal_feeName");
				classLibrary.setConfig.FenToYuan_Table(ds.Tables[0],"Ftoken_fee","Ftoken_feeName");
				classLibrary.setConfig.FenToYuan_Table(ds.Tables[0],"Fpaybuy","FpaybuyName");
				classLibrary.setConfig.FenToYuan_Table(ds.Tables[0],"Fpaysale","FpaysaleName");

				classLibrary.setConfig.GetColumnValueFromDic(ds.Tables[0],"Flist_state","Flist_stateName","PAY_STATE");
				classLibrary.setConfig.GetColumnValueFromDic(ds.Tables[0],"Freturn_state","Freturn_stateName","RLIST_STATE");
				classLibrary.setConfig.GetColumnValueFromDic(ds.Tables[0],"Flist_state_next","Flist_state_nextName","PAY_STATE");
				classLibrary.setConfig.GetColumnValueFromDic(ds.Tables[0],"Freturn_state_next","Freturn_state_nextName","RLIST_STATE");
				classLibrary.setConfig.GetColumnValueFromDic(ds.Tables[0],"Flstate","FlstateName","PAY_LSTATE");
				//��ʾ������Ϣ
				DataRow drmain = ds.Tables[0].Rows[0];
				string strtmp = drmain["Ftran_state"].ToString();
				if(strtmp == "1")
				{
					drmain["Ftran_stateName"] = "����";
				}
				else if(strtmp == "2")
				{
					drmain["Ftran_stateName"] = "����";
				}
				else if(strtmp == "3")
				{
					drmain["Ftran_stateName"] = "����";
				}				
				else
				{
					drmain["Ftran_stateName"] = "δ֪����" + strtmp;
				}

				strtmp = drmain["Ftran_state_next"].ToString();
				if(strtmp == "1")
				{
					drmain["Ftran_state_nextName"] = "����";
				}
				else if(strtmp == "2")
				{
					drmain["Ftran_state_nextName"] = "����";
				}
				else if(strtmp == "3")
				{
					drmain["Ftran_state_nextName"] = "����";
				}				
				else
				{
					drmain["Ftran_state_nextName"] = "δ֪����" + strtmp;
				}

				strtmp = drmain["Fstate"].ToString();
				if(strtmp == "0")
				{
					drmain["FstateName"] = "��ʼ״̬";
				}
				else if(strtmp == "1")
				{
					drmain["FstateName"] = "����Ͷ��";
				}
				else if(strtmp == "2")
				{
					drmain["FstateName"] = "Ͷ�ߴ�����";
				}
				else if(strtmp == "3")
				{
					drmain["FstateName"] = "Ͷ�ߴ������";
				}	
				else if(strtmp == "4")
				{
					drmain["FstateName"] = "ȡ��Ͷ��";
				}	
				else
				{
					drmain["FstateName"] = "δ֪����" + strtmp;
				}

				strtmp = drmain["Fresponse_flag"].ToString();
				if(strtmp == "1")
				{
					drmain["Fresponse_flagName"] = "������";
				}
				else if(strtmp == "2")
				{
					drmain["Fresponse_flagName"] = "������";
				}			
				else
				{
					drmain["Fresponse_flagName"] = "δ֪����" + strtmp;
				}


				strtmp = drmain["Fpunish_flag"].ToString();
				if(strtmp == "1")
				{
					drmain["Fpunish_flagName"] = "���账��";
				}
				else if(strtmp == "2")
				{
					drmain["Fpunish_flagName"] = "�������";
				}		
				else if(strtmp == "3")
				{
					drmain["Fpunish_flagName"] = "��������";
				}	
				else
				{
					drmain["Fpunish_flagName"] = "δ֪����" + strtmp;
				}

				strtmp = drmain["Fcheck_state"].ToString();
				if(strtmp == "1")
				{
					drmain["Fcheck_stateName"] = "δ���";
				}
				else if(strtmp == "2")
				{
					drmain["Fcheck_stateName"] = "���ύ���";
				}		
				else if(strtmp == "3")
				{
					drmain["Fcheck_stateName"] = "��˲�ͨ��";
				}	
				else if(strtmp == "4")
				{
					drmain["Fcheck_stateName"] = "���ͨ��";
				}	
				else if(strtmp == "5")
				{
					drmain["Fcheck_stateName"] = "���˿�";
				}	
				else
				{
					drmain["Fcheck_stateName"] = "δ֪����" + strtmp;
				}

				strtmp = drmain["Ffund_flag"].ToString();
				if(strtmp == "1")
				{
					drmain["Ffund_flagName"] = "���漰";
				}
				else if(strtmp == "2")
				{
					drmain["Ffund_flagName"] = "�漰";
				}						
				else
				{
					drmain["Ffund_flagName"] = "δ֪����" + strtmp;
				}

				strtmp = drmain["Fappeal_type"].ToString();
				if(strtmp == "1")
				{
					drmain["Fappeal_typeName"] = "�ɽ�����";
				}
				else if(strtmp == "2")
				{
					drmain["Fappeal_typeName"] = "�ջ������ȷ�ϣ�";
				}
				else if(strtmp == "3")
				{
					drmain["Fappeal_typeName"] = "������Ͷ����ң��˿����";
				}
				else if(strtmp == "4")
				{
					drmain["Fappeal_typeName"] = "��Ҷ�������";
				}
				else if(strtmp == "5")
				{
					drmain["Fappeal_typeName"] = "�ɽ�����";
				}
				else if(strtmp == "6")
				{
					drmain["Fappeal_typeName"] = "���Ҿܾ�ʹ�òƸ�ͨ����";
				}
				else if(strtmp == "7")
				{
					drmain["Fappeal_typeName"] = "�տ����";
				}
				else if(strtmp == "8")
				{
					drmain["Fappeal_typeName"] = "��Ʒ����������";
				}
				else if(strtmp == "9")
				{
					drmain["Fappeal_typeName"] = "���Ҷ�������";
				}
				else if(strtmp == "10")
				{
					drmain["Fappeal_typeName"] = "�����Ͷ�����ң��˿����";
				}
				else if(strtmp == "11")
				{
					drmain["Fappeal_typeName"] = "����Ҫ�������ȷ���ջ�";
				}
				else
				{
					drmain["Fappeal_typeName"] = "δ֪����" + strtmp;
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

			}
			else
			{
				throw new LogicException("û���ҵ���¼��");
			}
		}

	}
}
