using System;
using System.Reflection;
using System.Web;
using System.Web.SessionState;


namespace TENCENT.OSS.CFT.KF.KF_Web.UnitTest
{
	/// <summary>
	/// UT_CheckService 的摘要说明。
	/// </summary>
	public class UT_CheckService : ICheck
	{
		private static Check_WebService.Check_Service cs = null;
		private static UT_CheckService m_instance = null;
		public static EveryCheckFinHandler EveryCheckFinNotiryHandler = null;
		private UT_CheckService()
		{
			//
			// TODO: 在此处添加构造函数逻辑
			//

			cs = new Check_WebService.Check_Service();
		}

		#region ICheck 成员

		public bool DoAllCheck()
		{
			Type t = typeof(UT_CheckService);

			MethodInfo[] methods = t.GetMethods();

			for(int i=0;i<methods.Length;i++)
			{
				MethodInfo runMethod = methods[i];
				bool iResult = false;
				if(runMethod.IsStatic && runMethod.ReturnType == typeof(bool))
				{
					iResult = (bool)methods[i].Invoke(this,null);

					if(EveryCheckFinNotiryHandler != null)
						EveryCheckFinNotiryHandler(iResult,methods[i].Name);
				}
			}

			return false;
		}

		public bool DoCheck(int index)
		{
			Type t = typeof(UT_CheckService);

			MethodInfo method = t.GetMethod("UT_StartCheck_ForChangeName_OK");

			if(method != null)
			{
				bool iResult = (bool)method.Invoke(this,null);

				if(EveryCheckFinNotiryHandler != null)
					EveryCheckFinNotiryHandler(iResult,method.Name);
			}

			return false;
		}

		#endregion

		public static UT_CheckService GetInstance(EveryCheckFinHandler handler)
		{
			if(m_instance == null)
				m_instance = new UT_CheckService();

			EveryCheckFinNotiryHandler = handler;

			return m_instance;
		}


		
		public static bool UT_StartCheck_ForChangeName_OK()
		{
			string QQID = "359230096";
			string mail = "359230096@qq.com";
			string uid  = "1100000000";	
			string fetchNo = "101" + DateTime.Now.ToString("yyyyMMddHHmmssff").ToString();  //101修改姓名
			string commTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").ToString();
			string checkType = "Mediation";
			string reason = "测试修改姓名";
			string strLevel = "0";
			string accPath = "",infoPath = "",oldIdCardPath = "";
			string newName = "NewName";
			string Msg = "";
			Check_WebService.Finance_Header fh = new Check_WebService.Finance_Header();

			fh.UserName = "1100000000";
			fh.UserIP   = "127.0.0.1";

			cs.Finance_HeaderValue = fh;
			Check_WebService.Param [] myParams = new Check_WebService.Param[2]; 
			myParams[0]  = new Check_WebService.Param();  //对象数组 先要分别实例化
			myParams[1]  = new Check_WebService.Param();

			try
			{
				cs.StartCheck(fetchNo,checkType,reason,strLevel,myParams);

				Finance_ManageService.Finance_Manage fm = new Finance_ManageService.Finance_Manage();
				return fm.insertMediation(QQID,mail,"",reason,uid,fetchNo,commTime,accPath,infoPath,oldIdCardPath,"",commTime,newName,oldIdCardPath,out Msg);
			}
			catch
			{
				return false;
			}
		}


		public static bool UT_StartCheck_ForChangeName_NotOK()
		{
			return false;
		}

	}
}
