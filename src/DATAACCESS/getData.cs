using System;

namespace TENCENT.OSS.CFT.KF.Common
{
	/// <summary>
	/// getData ��ժҪ˵����
	/// </summary>
	public class GetData
	{
		// 0����δ��ʼ����1�����ǲ���ģʽ��2����ǲ���ģʽ
		private static int isTestMode = 0;
		
		public static bool IsTestMode
		{
			get
			{
				try
				{
					if(isTestMode == 0)
					{
						if(System.Configuration.ConfigurationManager.AppSettings["isTestingMode"] == null)
						{
							isTestMode = 2;
						}
						else if(System.Configuration.ConfigurationManager.AppSettings["isTestingMode"].Trim().ToLower() == "true")
						{
							isTestMode = 1;
						}
						else
						{
							isTestMode = 2;
						}
					}
					
					return (isTestMode == 1);
				}
				catch
				{
					return false;
				}
			}
		}


		// 0����δ��ʼ����1������������Ȩ��ģʽ��2�����������Ȩ��ģʽ
		private static int isNewSensitiveMode = 0;

		public static bool IsNewSensitiveMode
		{
			get
			{
				try
				{
					if(isNewSensitiveMode == 0)
					{
						if(System.Configuration.ConfigurationManager.AppSettings["isNewSensitivePowerMode"].Trim().ToLower() == "true")
						{
							isNewSensitiveMode = 1;
						}
						else
						{
							isNewSensitiveMode = 2;
						}
					}

					return (isNewSensitiveMode == 1);
				}
				catch (System.Exception ex)
				{
					return false;
				}
			}
		}

		private GetData()
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
		}
	}
}
