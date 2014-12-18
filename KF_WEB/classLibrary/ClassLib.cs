using System;
using System.Web.UI;
using System.Configuration;
using System.Collections;
using TENCENT.OSS.CFT.KF.Common;

namespace TENCENT.OSS.CFT.KF.KF_Web.classLibrary
{
	/// <summary>
	/// ClassLib 的摘要说明。
	/// </summary>
	public class ClassLib
	{
		public static bool ValidateRight(string powerName,TemplateControl page)
		{
			try
			{
				if(getData.IsNewSensitivePowerMode)
				{
					return SensitivePowerOperaLib.CheckAuth(powerName,page.Page);
				}
				else
				{
					return AllUserRight.ValidRight(page.Page.Session["SzKey"].ToString(),page.Page.Session["OperID"].ToString(),
						PublicRes.GROUPID,powerName,"","","");	
				}
			}
			catch
			{
				return false;
			}
		}


		/*
		public static bool ValidateRight(string powerName,UserControl control)
		{
			try
			{
				if(getData.IsNewSensitivePowerMode)
				{
					return SensitivePowerOperaLib.CheckAuth(powerName,control);
				}
				else
				{
					return AllUserRight.ValidRight(control.Session["SzKey"].ToString(),int.Parse(control.Session["OperID"].ToString()),
						PublicRes.GROUPID,powerName);	
				}
			}
			catch (System.Exception ex)
			{
				return false;
			}
		}
		*/



		public static bool WriteOpLog(string opTargetQQID,string opPowerName,string log,Page page)
		{
			

			throw new Exception("未完成的代码");
		}



		/*
		private static bool ValidateRight(string userName,string sessionKey,string url,string ip,string sessionID
			,string powerName,int OperaID,Page page)
		{
			if(getData.IsNewSensitivePowerMode)
			{
				int iindex = (int)ht[powerName.Trim().ToUpper()];

				int iRightID = rights[iindex].RightID;

				return SensitivePowerOperaLib.CheckAuth(iRightID,userName,sessionKey,url,ip,sessionID,page);
			}
			else
			{
				return AllUserRight.ValidRight(sessionKey,OperaID,PublicRes.GROUPID,powerName);
			}
		}
		*/



		/// <summary>
		/// 未测试该方法是否成功！
		/// </summary>
		/// <param name="page"></param>
		/// <param name="strMsg"></param>
		public static void ShowMessage(TemplateControl page,string strMsg)
		{
			string msg = setConfig.replaceHtmlStr(strMsg);
			page.Page.Response.Write("<script language=javascript>alert('" + msg + "')");
		}



		public static bool IsDataSetNull(System.Data.DataSet ds)
		{
			if(ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
				return true;

			return false;
		}
	}
}
