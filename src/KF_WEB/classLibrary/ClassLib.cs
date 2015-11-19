using System;
using System.Web.UI;
using System.Configuration;
using System.Collections;
using TENCENT.OSS.CFT.KF.Common;

namespace TENCENT.OSS.CFT.KF.KF_Web.classLibrary
{
	/// <summary>
	/// ClassLib ��ժҪ˵����
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


		public static bool WriteOpLog(string opTargetQQID,string opPowerName,string log,Page page)
		{
			

			throw new Exception("δ��ɵĴ���");
		}


		/// <summary>
		/// δ���Ը÷����Ƿ�ɹ���
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
