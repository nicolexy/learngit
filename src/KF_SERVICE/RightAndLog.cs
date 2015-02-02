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
	/// RightAndLog 的摘要说明。
	/// </summary>
	public class RightAndLog
	{
		public string UserIP = ""; //
		public string UserID = "";

		public string SzKey = "";
		public int OperID = 0;
		
		public string RightString = ""; //furion 20050824 用key和ID再去验有可能出错，还用权限串验证。

		public string strRightCode = "";
		public int sign = 0;

		public string type = "";
		public string actionType = "";

		public string detail = "";
		public string ID = "";


		//alexguan 2012/7/19 添加新敏感权限验证所需字段
		//public string OperID2 = "";	
		public string SessionID = "";
		public string url = "";

		public string ErrorMsg = "";

		public RightAndLog()
		{
			//
			// TODO: 在此处添加构造函数逻辑
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
					throw new LogicException("权限不对，无权执行此函数！");
				}

				return AllUserRight.ValidRight(SzKey,UserID,PublicRes.GROUPID,strRightCode,UserIP,SessionID,url);
			}
			else
			{
				if(UserID == null || UserID == "" || UserIP == null || UserIP == "" || SzKey == null
					|| SzKey == "" ||  OperID == 0 )
				{
					throw new LogicException("权限不对，无权执行此函数！");
				}

				return AllUserRight.ValidRight(SzKey,OperID.ToString(),PublicRes.GROUPID,strRightCode,"","","");
			}
		}

		private void CreateDetail()
		{
			if(detail == null || detail.Trim() == "")
			{
				detail = "用户" + UserID + "在" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") 
					+ "时对ID为" + ID + "的记录进行了" + actionType + "的操作，操作结果";
				if(sign == 0)
				{
					detail += "失败！失败原因为：" + ErrorMsg;
				}
				else
				{
					detail += "成功！";
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
