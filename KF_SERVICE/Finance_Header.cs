using System;
using System.Web.Services.Protocols;

namespace TENCENT.OSS.CFT.KF.KF_Service
{
	/// <summary>
	/// ����WEB�����SOAPͷ�ࡣ
	/// </summary>
	public class Finance_Header :SoapHeader 
	{
		public string UserName;
		public string UserIP;

		//furion 20050802 ��Ϊ���ڵ�Service������Ҫ��KEY�͵�¼ID����,���Լ�����������.
		public string SzKey = "";
		public int OperID = 0;

		public string RightString = ""; //furion 20050824 ��Ϊ��ʱ��ԭ����Ҫ�ͻ��˴��ݹ�����		/// <summary>
		/// �û�����(û��ʹ��)
		/// </summary>
		public string UserPassword;



		/// <summary>
		/// ������Ȩ����֤ϵͳ��Ҫ��ӵ��ֶΣ��������SessionID
		/// </summary>
		public string SessionID = "";

		/// <summary>
		/// ������Ȩ����֤ϵͳ��Ҫ��ӵ��ֶΣ�����Դ��URL
		/// </summary>
		public string SrcUrl = "";

		public Finance_Header()
			:this("","")
		{
		}
		public Finance_Header(string aUserName, string aUserIP)
		{
			UserName = aUserName;
			UserIP = aUserIP;
		}
	}
}
