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
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using System.Web.Services.Protocols;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.CFT.KF.DataAccess;
using CFT.CSOMS.BLL.TransferMeaning;

namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
	/// <summary>
	/// RealtimeOrder ��ժҪ˵����
	/// </summary>
	public partial class RealtimeOrder : TENCENT.OSS.CFT.KF.KF_Web.PageBase
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			//Ȩ����֤
			try
			{
				Label1.Text = Session["uid"].ToString();

				string szkey = Session["SzKey"].ToString();
				int operid = Int32.Parse(Session["OperID"].ToString());
				
				//if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "FundQuery")) Response.Redirect("../login.aspx?wh=1");
                if (!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("TradeManagement", this)) Response.Redirect("../login.aspx?wh=1");
			}
			catch
			{
				Response.Redirect("../login.aspx?wh=1");
			}

			

			//��ʼ��ʱ��
			if(!IsPostBack)
			{
				classLibrary.setConfig.GetAllBankList(ddlBankType);
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

		public static  int    ERR_QUERY_BILL_NOPAY    = 10620001; //û��֧��
		public static  int    ERR_QUERY_BILL_BANKFAIL = 10620002; //����ҵ��ʧ��
		public static  int    ERR_QUERY_BILL_CONNECT  = 10620003; //ϵͳʧ��
		public static  int    ERR_CMD_NOT_SUPPORT     = 10620004; //��֧�ֵĲ���
		public static  int    ERR_INPUT_PARAMETER     = 10620005; //��������
		public static  int    ERR_PROTOCOL            = 10620006; //Э�����
		public static  int    ERR_UNKNOWN             = 10620007; //δ֪����

		protected void Button2_Click(object sender, System.EventArgs e)
		{
			labError.Text = "";
			labFListID.Text = "";
			labFNum.Text = "";
			labFState.Text = "";
			labFSign.Text = "";
			labFBank_List.Text = "";
			labFBank_Acc.Text = "";
			labFBank_Type.Text = "";
			labFaid.Text = "";
			labFaname.Text = "";
			labFpay_front_time.Text = "";
			labFbank_time.Text = "";
			labFmodify_time.Text = "";




			if(tbOrder.Text.Trim() != "")
			{
				string inmsg = "bank_type=" + ddlBankType.SelectedValue;
				inmsg += "&bill_no=" + TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.ICEEncode(tbOrder.Text.Trim());
				inmsg += "&offset=0";
				inmsg += "&limit=10";
//				inmsg += "&s_time=" + CommonRes.ICEEncode( DateTime.Now.ToString("yyyy-MM-dd 00:00:00"));
//				inmsg += "&e_time=" + CommonRes.ICEEncode(DateTime.Now.ToString("yyyy-MM-dd 23:59:59"));

				string reply;
				short result;
				string msg;

				if(TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.middleInvoke("kps_query_tcr_ret_service",inmsg,false,out reply,out result,out msg))
				{
					if(result != 0)
					{
						labError.Text = "���ʲ�ѯ����ʧ�ܣ�result=" + result + "��msg=" + msg;
					}
					else
					{
						if(reply.StartsWith("&result=0"))
						{
							//��ʾ����.
							string[] strreplys = reply.ToLower().Split('&');
							Hashtable ht = new Hashtable(strreplys.Length);

							foreach(string stmp in strreplys)
							{
								if(stmp.Trim() != "" && stmp.IndexOf("=") > 0)
								{
									string[] stmps = stmp.Split('=');

									if(stmps.Length == 2)
									{
										ht.Add(stmps[0],stmps[1]);
									}
								}
							}

							if(ht.ContainsKey("flistid"))
								labFListID.Text = ht["flistid"].ToString();

							if(ht.ContainsKey("fnum"))
								labFNum.Text = MoneyTransfer.FenToYuan( ht["fnum"].ToString());

							if(ht.ContainsKey("fstate"))
							{
								if(ht["fstate"].ToString() == "1")
									labFState.Text = "����ǰ";
								else
									labFState.Text = "�����";
							}

							if(ht.ContainsKey("fsign"))
                                labFSign.Text = Transfer.returnDicStr("TCLIST_SIGN", ht["fsign"].ToString());

							if(ht.ContainsKey("fbank_list"))
								labFBank_List.Text = ht["fbank_list"].ToString();


							if(ht.ContainsKey("fbank_acc"))
								labFBank_Acc.Text = ht["fbank_acc"].ToString();

							if(ht.ContainsKey("fbank_type"))
                                labFBank_Type.Text = Transfer.returnDicStr("BANK_TYPE", ht["fbank_type"].ToString());

							if(ht.ContainsKey("faid"))
								labFaid.Text = ht["faid"].ToString();

							if(ht.ContainsKey("faname"))
								labFaname.Text = ht["faname"].ToString();

							if(ht.ContainsKey("fpay_front_time"))
								labFpay_front_time.Text = ht["fpay_front_time"].ToString();

							if(ht.ContainsKey("fbank_time"))
								labFbank_time.Text = ht["fbank_time"].ToString();

							if(ht.ContainsKey("fmodify_time"))
								labFmodify_time.Text = ht["fmodify_time"].ToString();
						}
						else if(reply.StartsWith("result=1022"))
						{
							labError.Text = "û���ҵ���Ӧ��¼: " ;
						}	//1014
						else if(reply.StartsWith("result=" + ERR_CMD_NOT_SUPPORT))
						{
							labError.Text = "��֧�ֵĲ���:" + reply;
						}
						else
						{
							labError.Text = "���ʲ�ѯ����ʧ�ܣ�reply=" + reply;
						}
					}
				}
				else
				{
					labError.Text = "���ʲ�ѯE�ӿ�ʧ�ܣ�result=" + result + "��msg=" + msg;
				}
			}
		}
	}
}
