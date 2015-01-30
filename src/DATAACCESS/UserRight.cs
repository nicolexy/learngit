/************************************************************
  FileName: UserRight.cs
  Author: Furion       Version :    1.0.0      Date:2005-05-31
  Description: Ȩ�޹����࣬��֤�û��Ƿ�Ϸ�����֤�û��Ƿ���Ȩ�ޡ�    // ģ������      
  Version:   1.0.0      // �汾��Ϣ
  Function List:   // ��Ҫ�������书��
    1. ValidateUser   ��֤�û��Ƿ�Ϸ���
    2. ValidateRight  ��֤�û��Ƿ�����Ӧ��Ȩ�ޡ�
      <author>  <time>   <version >   <desc>
      Furion  2005-05-31     1.0.0     build this moudle  
***********************************************************/
using System;
using System.Configuration;
using TENCENT.OSS.CFT.KF.DataAccess;
using System.Data;
using System.Collections;

using System.Text;
using System.IO;
using System.Security.Cryptography; 

namespace TENCENT.OSS.CFT.KF.Common
{
	/// <summary>
	/// Ȩ�޹����࣬��֤�û��Ƿ�Ϸ�����֤�û��Ƿ���Ȩ�ޡ�
	/// </summary>
	public class UserRight
	{
		public UserRight()
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
		}

		/// <summary>
		/// ��֤�û��Ƿ�Ϸ���
		/// </summary>
		public static TCreateSessionReply ValidateUser(string strUserID, string strPassword, string strServerIP, int iServerPort)
		{
            try
            {
                cSession mysession = new cSession(strServerIP,iServerPort);

                TCreateSessionQuest myQuest = new TCreateSessionQuest();
                myQuest.szPassword = strPassword;
                myQuest.szUserName = strUserID;
                myQuest.szUserData = "KFWEB";

                TCreateSessionReply myreply  = mysession.CsCreateSessiond(myQuest);
                return myreply;
            }
            catch  
            {
            	
                return null;
            }
		}

		/// <summary>
		/// ��֤�û��Ƿ���ִ��Ȩ�ޡ�
		/// </summary>
		public static bool ValidateRight(string strSzkey, int iOperId, int iGroupID, int iServiceId, string strServerIP, int iServerPort)
		{
            try
            {
                cSession mysession = new cSession(strServerIP,iServerPort);

                TVerifySessionQuest myQuest = new TVerifySessionQuest();
                myQuest.szKey = strSzkey;
                myQuest.OperId = iOperId;
                myQuest.GroupId = iGroupID;
                myQuest.ServiceId = iServiceId;
                myQuest.iUin = 0;
            
                TVerifySessionReply myreply  = mysession.CsVerfiySessiond(myQuest);
                return myreply.OpResult == 0;
            }
            catch(Exception ex)
            {
                return false;
            }            
		}

        public static void DelLoginUser(string strSzkey, int iOperId, string strServerIP, int iServerPort)
        {
            try
            {
                cSession mysession = new cSession(strServerIP,iServerPort);
                TDeleteSessionQuest myQuest = new TDeleteSessionQuest();
                myQuest.szKey = strSzkey;
                myQuest.OperId = iOperId;
            
                TDeleteSessionReply myreply  = mysession.CsDeleteSessiond(myQuest);
            }
            catch
            {}
        }

		public static void UpdateSession(string strSzkey, int iOperID,int iGroupID, int iServiceID,string struserdata,
			string content,string strServerIP,int iServerPort)
		{
			try
			{
				cSession mysession = new cSession(strServerIP,iServerPort);
				TUpdateSessionQuest myquest = new TUpdateSessionQuest();
				myquest.GroupId = iGroupID;
				myquest.iUin = 0;
				myquest.OperId = iOperID;
				myquest.ServiceId = iServiceID;
				myquest.szContext = content;
				myquest.szKey = strSzkey;
				myquest.szUserData = "KFWEB";

				TUpdateSessionReply usr = mysession.CsUpdateSessiond(myquest);

				if(usr.OpResult != 0)
				{
					
				}
			}
			catch(Exception err)
			{
				
			}
		}

	}
}
