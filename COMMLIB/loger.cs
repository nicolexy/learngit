using System;

namespace TENCENT.OSS.C2C.Finance.Common.CommLib
{
	/// <summary>
	/// loger 的摘要说明。
	/// 日志处理类
	/// </summary>
	public class loger
	{
		public loger()
		{
			//
			// TODO: 在此处添加构造函数逻辑
			//
		}

		/// <summary>
		/// 发送log4日志,同时记录文件日志
		/// </summary>
		/// <param name="modeInfo">发送的模块名称</param>
		/// <param name="Msg">发送的消息</param>
		/// <returns></returns>
		public static bool err(string modeInfo,string Msg)
		{
			log4net.ILog log = log4net.LogManager.GetLogger(modeInfo);
			if(log.IsErrorEnabled) log.Error(Msg);

			//commRes.WriteFile("[" + modeInfo + "]" + Msg);
			//commRes.CloseFile();

			return true;	
		}

	}




	
}
