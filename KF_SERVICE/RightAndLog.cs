using System;
using System.Configuration;
using TENCENT.OSS.CFT.KF.DataAccess;
using System.Data;
using System.Collections;
using System.Web.Mail;
using System.Text;
using System.IO;
using System.Security.Cryptography; 
using TENCENT.OSS.CFT.KF.Common;

namespace TENCENT.OSS.CFT.KF.KF_Service
{
	/// <summary>
	/// RightAndLog ��ժҪ˵����
	/// </summary>
	public class RightAndLog
	{
		public string UserIP = ""; //
		public string UserID = "";

		public string SzKey = "";
		public int OperID = 0;
		
		public string RightString = ""; //furion 20050824 ��key��ID��ȥ���п��ܳ�������Ȩ�޴���֤��

		public string strRightCode = "";
		public int sign = 0;

		public string type = "";
		public string actionType = "";

		public string detail = "";
		public string ID = "";


		//alexguan 2012/7/19 ���������Ȩ����֤�����ֶ�
		//public string OperID2 = "";	
		public string SessionID = "";
		public string url = "";

		public string ErrorMsg = "";

		public RightAndLog()
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
		}

		public void WriteLog()
		{
			CreateDetail();

			PublicRes.writeSysLog(UserID,UserIP,type,actionType,sign,ID,"",detail);
		}

		public bool CheckRight()
		{
			if(GetData.IsNewSensitiveMode)
			{
				if(UserID == null || UserID == "" || UserIP == null || UserIP == "" || SzKey == null
					|| SzKey == "" || url.Trim() == "" || SessionID.Trim() == "")
				{
					throw new LogicException("Ȩ�޲��ԣ���Ȩִ�д˺�����");
				}

				return AllUserRight.ValidRight(SzKey,UserID,PublicRes.GROUPID,strRightCode,UserIP,SessionID,url);
			}
			else
			{
				if(UserID == null || UserID == "" || UserIP == null || UserIP == "" || SzKey == null
					|| SzKey == "" ||  OperID == 0 )
				{
					throw new LogicException("Ȩ�޲��ԣ���Ȩִ�д˺�����");
				}

				return AllUserRight.ValidRight(SzKey,OperID.ToString(),PublicRes.GROUPID,strRightCode,"","","");
			}
		}

		private void CreateDetail()
		{
			if(detail == null || detail.Trim() == "")
			{
				detail = "�û�" + UserID + "��" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") 
					+ "ʱ��IDΪ" + ID + "�ļ�¼������" + actionType + "�Ĳ������������";
				if(sign == 0)
				{
					detail += "ʧ�ܣ�ʧ��ԭ��Ϊ��" + ErrorMsg;
				}
				else
				{
					detail += "�ɹ���";
				}
			}
		}

		public void WriteRemoteLogFile()
		{
			CreateDetail();

			LogManage lm = new LogManage(PublicRes.f_strServerIP,PublicRes.f_iServerPort);
			lm.Log("Service","finance","0002","warning",detail);
		}

		public void WriteRemoteLogDB()
		{
			CreateDetail();

			LogManage lm = new LogManage(PublicRes.f_strServerIP,PublicRes.f_iServerPort);
			lm.DBLog("Service","finance","0002","warning",detail);
		}
	}
}
