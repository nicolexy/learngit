using System;
using System.Web.UI;
using Tencent.DotNet.Common.UI;
using TENCENT.OSS.CFT.KF.KF_Web;
//using TENCENT.OSS.CFT.KF.KF_Web.SensitivePowerSystem_Service;
using System.Configuration;
using System.Collections;
using TENCENT.OSS.CFT.KF.Common;

namespace TENCENT.OSS.CFT.KF.KF_Web.classLibrary
{
	/// <summary>
	/// SensitivePowerOperaLib ��ժҪ˵����
	/// </summary>
	public class SensitivePowerOperaLib
	{
		public SensitivePowerOperaLib()
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
		}


		private static bool IsOpenSensitiveCheck()
		{
			return getData.IsNewSensitivePowerMode;
		}

		private static bool IsOutOfTime(TemplateControl control)
		{
			//return (control.Page.Session["OperID"] == null || control.Page.Session["SzKey"] == null || control.Page.Session["Ticket"] == null);
		
			bool b1 = control.Page.Session["OperID"] == null;
			bool b2 = control.Page.Session["SzKey"] == null;
			//bool b3 = control.Page.Session["Ticket"] == null;
			bool b4 = control.Page.Session["uid"] == null;

			return (b1 || b2 || b4);
		}



		// ���û����Javascript������ǿ��еġ�
		private static void HandleSPResult(Page page,string msg,bool isRedirect,string url)
		{
			string strJs = "<script language=javascript>alert('" + setConfig.replaceHtmlStr(msg) + "');";

			if(isRedirect)
			{
				strJs += "location.href='" + url + "'";
				//strJs += "location.href='http://www.baidu.com'";
			}

			strJs += "</script>";

			page.Response.Write(strJs);
			page.Response.Flush();
		}
	


		public static bool CheckSPReturnResult(SensitiveVerifyService.Result result,Page page)
		{
			if(!IsOpenSensitiveCheck())
				return true;

			if(result.status == 0)
			{
				return true;
			}
			else if(result.status >= 100 && result.status <= 199)
			{
				//WebUtils.ShowMessage(page.Page,"������֤Ȩ�޳���,�����ԣ�����ϵϵͳά����Ա��" + result.status_info);

				//HandleSPResult(page,"������֤Ȩ�޳���,�����ԣ�����ϵϵͳά����Ա��" + result.status_info,false,"");

				return false;	
			}
			else if(result.status >= 200 && result.status <= 299)
			{
				//WebUtils.ShowMessage(page.Page,"��û�����е�¼״̬����Ҫ���߼�������е�¼״̬��" + result.status_info);

				//page.Page.Response.Redirect(result.login_url);

				//page.Response.Write("<script language=javascript>alert('��û�����е�¼״̬����Ҫ���߼�������е�¼״̬');location.href='" + result.login_url + "'</script>");

				//page.Response.Flush();

				//HandleSPResult(page,"��û�����е�¼״̬����Ҫ���߼�������е�¼״̬��" + result.status_info,true,result.login_url);

                //page.Page.Response.Redirect(result.login_url);

               // page.Page.Response.Write("<script>window.parent.location.href = '" + result.login_url + "';</script>");

                var script = "<script type='text/javascript'>"+
                                 "var win = window.parent.window || window;" +
                                 "win.location.href = '" + result.login_url + "';" +
                             "</script>";
                page.Page.Response.Write(script);
                page.Page.Response.Cookies.Clear(); // ���Cookie;
                page.Page.Session.Clear();          // ���Session;
               
				return false;
			}
			else if(result.status >= 300 && result.status <= 399)
			{
				//WebUtils.ShowMessage(page.Page,"��û�е�ǰ������Ȩ��," + result.status_info + "  ��ͨȨ�������:" + result.auth_apply_url);
				
				//HandleSPResult(page,"��û�е�ǰ������Ȩ��," + result.status_info + "  ��ͨȨ�������:" + result.auth_apply_url,false,"");

				return false;
			}

			return false;
		}


		public static bool CheckSPReturnResult(SensitiveVerifyService.Result result,UserControl page)
		{
			if(!IsOpenSensitiveCheck())
				return true;

			if(result.status == 0)
			{
				return true;
			}
			else if(result.status >= 100 && result.status <= 199)
			{
				//WebUtils.ShowMessage(page,"������֤Ȩ�޳���,�����ԣ�����ϵϵͳά����Ա��" + result.status_info);
				//ClassLib.ShowMessage(page,"������֤Ȩ�޳���,�����ԣ�����ϵϵͳά����Ա��" + result.status_info);
				return false;	
			}
			else if(result.status >= 200 && result.status <= 299)
			{
				//WebUtils.ShowMessage(page,"��û�����е�¼״̬����Ҫ���߼�������е�¼״̬��" + result.status_info);
				//ClassLib.ShowMessage(page,"��û�����е�¼״̬����Ҫ���߼�������е�¼״̬��" + result.status_info);
				page.Response.Redirect(result.login_url);
				return false;
			}
			else if(result.status >= 300 && result.status <= 399)
			{
				//WebUtils.ShowMessage(page,"��û�е�ǰ������Ȩ��," + result.status_info + "  ��ͨȨ�������:" + result.auth_apply_url);
				//ClassLib.ShowMessage(page,"��û�е�ǰ������Ȩ��," + result.status_info + "  ��ͨȨ�������:" + result.auth_apply_url);
				return false;
			}

			return false;
		}


		public static bool CheckSession(string ticket,Page page)
		{
			string loginName = "",szKey = "";

			if(page.Page.Session["uid"] != null)
				loginName = page.Page.Session["uid"].ToString();

			if(page.Page.Session["SzKey"] != null)
				szKey = page.Page.Session["SzKey"].ToString();

			return CheckSession(ticket,page.Page.Request.Url.ToString(),page.Page.Request.UserHostAddress,page.Page.Session.SessionID,loginName,szKey,page);
		}

		public static bool CheckSession(string ticket,UserControl page)
		{
			string loginName = "",szKey = "";

			if(page.Session["uid"] != null)
				loginName = page.Session["uid"].ToString();

			if(page.Session["SzKey"] != null)
				szKey = page.Session["SzKey"].ToString();

			return CheckSession(ticket,page.Request.Url.ToString(),page.Request.UserHostAddress,page.Session.SessionID,loginName,szKey,page);
		}

		private static bool CheckSession(string ticket,string url,string ip,string sessionID,string loginName,string szKey,Page page)
		{
			if(!IsOpenSensitiveCheck())
				return true;

			SensitiveVerifyService.Result ret = Common.AllUserRight.CheckSession(ticket,url,ip,sessionID,loginName,szKey);
			if(CheckSPReturnResult(ret,page))
			{	
				// ������Ȩ��ϵͳû��OperaID�Ļ�ȡ�ˣ�Ӧ����κ;�ϵͳ���ݣ�
				if(page.Page.Session["OperID"] == null)
					page.Page.Session["OperID"] = "0";

				if(page.Page.Session["SzKey"] == null)
					page.Page.Session["SzKey"] = ret.auth_cm_com_session_key;

				if(page.Page.Session["uid"] == null)
					page.Page.Session["uid"] = ret.user_name;

				return true;
			}
			else
			{
				return false;
			}
		}

		private static bool CheckSession(string ticket,string url,string ip,string sessionID,string loginName,string szKey,UserControl page)
		{
			if(!IsOpenSensitiveCheck())
				return true;

			SensitiveVerifyService.Result ret = Common.AllUserRight.CheckSession(ticket,url,ip,sessionID,loginName,szKey);

			if(CheckSPReturnResult(ret,page))
			{
				// ������Ȩ��ϵͳû��OperaID�Ļ�ȡ�ˣ�Ӧ����κ;�ϵͳ���ݣ�
				if(page.Session["OperID"] == null)
					page.Session["OperID"] = "0";

				if(page.Session["SzKey"] == null)
					page.Session["SzKey"] = ret.auth_cm_com_session_key;

				if(page.Session["uid"] == null)
					page.Session["uid"] = ret.user_name;

				return true;
			}
			else
			{
				return false;
			}
		}
	
		public static bool CheckAuth(string opName,UserControl control)
		{
			// ����Ȩ���ĵ���дÿ��ҳ�涼��Ҫ����checkSession��������CheckAuth��ߵ���checkSession
			//if(IsOutOfTime(control) || !CheckSession(control.Page.Session["Ticket"].ToString(),control))
            if (IsOutOfTime(control))
            {
                control.Page.Response.Redirect("../login.aspx?wh=1");
                return false;
            }

			return CheckAuth(opName,control.Page.Session["uid"].ToString(),control.Page.Session["SzKey"].ToString(),
				control.Page.Request.Url.ToString(),control.Page.Request.UserHostAddress,control.Page.Session.SessionID,control);
		}

		public static bool CheckAuth(string opName,Page page)
		{
			// ����Ȩ���ĵ���дÿ��ҳ�涼��Ҫ����checkSession��������CheckAuth��ߵ���checkSession
			//if(IsOutOfTime(page) || !CheckSession(page.Session["Ticket"].ToString(),page))
            if (IsOutOfTime(page))
            {
                page.Response.Redirect("../login.aspx?wh=1");
                return false;
            }

			return CheckAuth(opName,page.Session["uid"].ToString(),page.Session["SzKey"].ToString(),page.Request.Url.ToString(),
				page.Request.UserHostAddress,page.Session.SessionID,page);
		}

		private static bool CheckAuth(string opName,string userName,string sessionKey,string url,string ip,string sessionID,Page page)
		{
			if(!IsOpenSensitiveCheck())
				return true;

			SensitiveVerifyService.Result retResult = AllUserRight.CheckAuth(opName,userName,sessionKey,url,ip,sessionID);
			return CheckSPReturnResult(retResult,page);
		}

		private static bool CheckAuth(string opName,string userName,string sessionKey,string url,string ip,string sessionID,UserControl page)
		{
			if(!IsOpenSensitiveCheck())
				return true;

			SensitiveVerifyService.Result retResult = AllUserRight.CheckAuth(opName,userName,sessionKey,url,ip,sessionID);

			return CheckSPReturnResult(retResult,page);
		}

		public static string MakeLog(string opType,string targetQQID,string editDesc,params string[] strParams)
		{
			if(!IsOpenSensitiveCheck())
				return "";

			string log = "|" + opType + "|";

			foreach(string str in strParams)
			{
				log += str + ",";
			}

			log += "|" + targetQQID + "|" + editDesc + "|";

			return log;
		}

		public static bool WriteOperationRecord(string opPowerName,string log,Page page)
		{
			if(IsOutOfTime(page))
				page.Page.Response.Redirect("../login.aspx?wh=1");

			return WriteOperationRecord(opPowerName,page.Page.Session["uid"].ToString(),page.Page.Session["SzKey"].ToString(),page.Page.Request.Url.ToString(),
				page.Page.Request.UserHostAddress,page.Page.Session.SessionID,log,page);
		}

		private static bool WriteOperationRecord(string opName,string userName,string sessionKey,string url,string ip,string sessionID
			,string log,Page page)
		{
			if(!IsOpenSensitiveCheck())
				return true;

			SensitiveVerifyService.Result ret = AllUserRight.WriteLog(opName,userName,sessionKey,url,ip,sessionID,log);

			return CheckSPReturnResult(ret,page);
		}

		public static bool LogOut(Page page)
		{
			if(IsOutOfTime(page))
				page.Page.Response.Redirect("../login.aspx?wh=1");

			return LogOut(page.Page.Session["uid"].ToString(),page.Page.Session["SzKey"].ToString(),page.Page.Request.Url.ToString(),
				page.Page.Request.UserHostAddress,page.Page.Session.SessionID,page);
		}

		private static bool LogOut(string userName,string sessionKey,string url,string ip,string sessionID,Page page)
		{
			if(!IsOpenSensitiveCheck())
				return true;

			SensitiveVerifyService.auth authService = new SensitiveVerifyService.auth();

			SensitiveVerifyService.Request rq = new SensitiveVerifyService.Request();

			rq.system_id = 62;		// �̶���ֵ
			rq.system_idSpecified = true;
			rq.user_name = userName;
			rq.auth_cm_com_session_key = sessionKey;
			rq.user_url = url;
			rq.user_ip = ip;
			rq.local_session_id = sessionID;

			SensitiveVerifyService.Result ret = authService.LogOut(rq);

			return CheckSPReturnResult(ret,page);
		}

		public static string Echo(string echoStr)
		{
			SensitiveVerifyService.auth authService = new SensitiveVerifyService.auth();

			string strResult = authService.Echo(echoStr);

			return strResult;
		}

		public static int GetPowerID(string powerName)
		{
			return PowerLib.GetPowerID(powerName);
		}


		#region PowerLib


		class PowerLib
		{
			const int RIGHTCOUNT = 255;
			public static OneRight[] rights;

			private static string serverIP;
			private static int serverPort;

			private static Hashtable ht;

			public static bool IgnoreLimitCheck = false;

			private PowerLib() { }

			static PowerLib() 
			{
				serverIP = ConfigurationManager.AppSettings["ServerIP"];
				serverPort = Int32.Parse(ConfigurationManager.AppSettings["ServerPort"]);

				rights = new OneRight[RIGHTCOUNT];
				//�������е�Ȩ�ޡ�


                rights[181] = new OneRight("BankClassifyInfo", 181, "���з�����Ϣ");
                rights[182] = new OneRight("FCXGAccountPassWordReset", 182, "����˻���������");
                rights[176] = new OneRight("InfoCenter", 176, "������Ϣ����");
                rights[177] = new OneRight("PayManagement", 177, "֧������ ");
                rights[178] = new OneRight("TradeManagement", 178, "���׹���");
                rights[179] = new OneRight("SPInfoManagement", 179, "�̻���Ϣ����");
                rights[180] = new OneRight("SystemManagement", 180, "ϵͳ����");
                rights[196] = new OneRight("ShowIDCrad", 196, "���֤��ѯ");
				rights[18] = new OneRight("UserReport",18,"���Ͷ��");
				rights[19] = new OneRight("HistoryModify",19,"�����޸���ʷ");
				rights[81] = new OneRight("FreezeUser",81,"�����ʻ���ť");
				rights[82] = new OneRight("FreezeBalance",82,"���������ť");
				rights[85] = new OneRight("LockTradeList",85,"�������׵���ť");
				rights[183] = new OneRight("UnFreezeUser",183,"�ⶳ�ʻ���ť");
				rights[184] = new OneRight("UnLockTradeList",184,"�������׵���ť");
                rights[195] = new OneRight("CFTUserPick", 195, "�Ƹ�ͨ�������ߴ���");
                rights[192] = new OneRight("CFTUserPickQuer", 192, "�Ƹ�ͨ�������ߴ����ѯ");//����Ȩ�� ��ѯ��¼��1000Ԫ�����������
                rights[206] = new OneRight("NameAbnormalCheck", 206, "�����쳣����");
                rights[208] = new OneRight("ClearMobileNumber", 208, "�ֻ���������");
                rights[191] = new OneRight("DrawAndApprove", 191, "�����̻��쵥�����");
                rights[199] = new OneRight("DeleteCrt", 199, "ɾ������֤��");
                rights[150] = new OneRight("MobileConfig", 150, "�ֻ���������");
                //add by yinhuang 2013/12/10
                rights[151] = new OneRight("BussComplain", 151, "�̻�Ͷ�������޸�");
                rights[152] = new OneRight("LCTQuery", 152, "���ͨ��ѯ");
                rights[204] = new OneRight("BalanceControl", 204, "���ͨ������");
                
                //lxl 20140731
                rights[205] = new OneRight("UnFinanceControl", 205, "�ʽ��ܿؽ��");

                rights[185] = new OneRight("UnFreezeChannelFK", 185, "�ⶳ��ض�������");
                rights[186] = new OneRight("UnFreezeChannelPP", 186, "�ⶳ���Ķ�������");
                rights[187] = new OneRight("UnFreezeChannelYH", 187, "�ⶳ�û���������");
                rights[188] = new OneRight("UnFreezeChannelSH", 188, "�ⶳ�̻���������");
                rights[189] = new OneRight("UnFreezeChannelBG", 189, "�ⶳBG�ӿڶ�������");

                rights[200] = new OneRight("ModifyFundPayCard", 200, "���ͨ��ȫ�����");
                rights[201] = new OneRight("ModifyFundPayCardByCS", 201, "���ͨ��ȫ�����(�ͷ�)");
                rights[202] = new OneRight("InternetBankRefund", 202, "�����ύ�����˿�");
                rights[162] = new OneRight("PayBusinessCMD", 162, "�����ύ�����˿�");
                rights[193] = new OneRight("RefundCheck", 193, "�˿�Ǽ�");

                rights[203] = new OneRight("RefundMerchantCheck", 203, "�˿��̻�¼��");
                rights[194] = new OneRight("BatchCancellation", 194, "����ע��");
                //���Ӵ��۵�����ťȨ��
                rights[198] = new OneRight("DKAdjust", 198, "���۵���״̬");

                rights[169] = new OneRight("RealNameCertification", 169, "ʵ����֤����������");

                rights[170] = new OneRight("IceOutPPSecurityMoney", 170, "�ⶳ���ı�֤�𶩵�");
                rights[171] = new OneRight("LCTMenu", 171, "���ͨ�˵�");
                rights[172] = new OneRight("HandQMenu", 172, "��Q֧���˵�");
                rights[173] = new OneRight("CreditQueryMenu", 173, "���ÿ�����˵�");
                rights[174] = new OneRight("FastPayMenu", 174, "���֧���˵�");
                rights[175] = new OneRight("TencentbusinessMenu", 175, "����˾ҵ��˵�");

                rights[196] = new OneRight("SensitiveRole", 196, "����Ȩ��");
				ht = new Hashtable(RIGHTCOUNT);
				for(int i=0 ; i< RIGHTCOUNT; i++)
				{
					if(rights[i] != null)
					{
						ht.Add(rights[i].RightItem.Trim().ToUpper(),i);
					}
				}
			}

			public static int GetPowerID(string powerName)
			{
				try
				{
					if(powerName == null || powerName.Trim() == "")
						return -1;

					int iindex = (int)ht[powerName.Trim().ToUpper()];

					int iRightID = rights[iindex].RightID;

					return iRightID;
				}
				catch
				{
					return -1;
				}
			}

		}

		#endregion

	}

}
