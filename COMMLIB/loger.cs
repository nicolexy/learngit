using System;

namespace TENCENT.OSS.C2C.Finance.Common.CommLib
{
	/// <summary>
	/// loger ��ժҪ˵����
	/// ��־������
	/// </summary>
	public class loger
	{
		public loger()
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
		}

		/// <summary>
		/// ����log4��־,ͬʱ��¼�ļ���־
		/// </summary>
		/// <param name="modeInfo">���͵�ģ������</param>
		/// <param name="Msg">���͵���Ϣ</param>
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
