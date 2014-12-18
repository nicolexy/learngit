using System;
using System.Web.Services.Protocols;

namespace TENCENT.OSS.CFT.KF.KF_Service
{
	/// <summary>
	/// 访问WEB服务的SOAP头类。
	/// </summary>
	public class Finance_Header :SoapHeader 
	{
		public string UserName;
		public string UserIP;

		//furion 20050802 因为现在的Service调用需要用KEY和登录ID调用,所以加上两个参数.
		public string SzKey = "";
		public int OperID = 0;

		public string RightString = ""; //furion 20050824 因为超时的原因，需要客户端传递过来。		/// <summary>
		/// 用户密码(没有使用)
		/// </summary>
		public string UserPassword;



		/// <summary>
		/// 新敏感权限验证系统需要添加的字段，浏览器的SessionID
		/// </summary>
		public string SessionID = "";

		/// <summary>
		/// 新敏感权限验证系统需要添加的字段，操作源的URL
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
