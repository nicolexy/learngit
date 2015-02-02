using System;

namespace TENCENT.OSS.CFT.KF.Common
{
	/// <summary>
	/// getData 的摘要说明。
	/// </summary>
	public class GetData
	{
		// 0代表未初始化，1代表是测试模式，2代表非测试模式
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


		// 0代表未初始化，1代表是新敏感权限模式，2代表非新敏感权限模式
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
			// TODO: 在此处添加构造函数逻辑
			//
		}
	}
}
