using System;
using System.Configuration;
using System.Data;
using System.Collections;
using System.Web;
using System.Text;
using System.IO;
using System.Security.Cryptography; 
using System.Threading;

namespace TENCENT.OSS.CFT.KF.Common
{
	/// <summary>
	/// commMethod 用来处理一些通用的方法。比如发送日志，Web和service都需要使用到的一些公用的方法，函数。
	/// 主要减轻publicRes的压力，减少冗余。
	/// </summary>
	public class commRes
	{
		public static string ICEEncode(string instr)
		{
			if(instr == null || instr.Trim() == "")
				return instr;
			else
			{
				return instr.Replace("%","%25").Replace("=","%3d").Replace("&","%26");
			}
		}

		public commRes()
		{
		}

		private static string ICEServerIP;
		private static int ICEPort;
		private static string iceUsr;
		private static string icePwd;

		static commRes()
		{
			ICEServerIP  = ConfigurationSettings.AppSettings["ICEServerIP"].Trim();
			ICEPort  = Int32.Parse(ConfigurationSettings.AppSettings["ICEPort"].Trim());
			iceUsr = ConfigurationSettings.AppSettings["iceUsr"].ToString();
			icePwd = ConfigurationSettings.AppSettings["icePwd"].ToString();
		}


		/// <summary>
		/// 向middle发送请求包（分加密和非加密两种）
		/// </summary>
		/// <param name="serviceName"></param>
		/// <param name="requestMsg"></param>
		/// <param name="secret"></param>
		/// <param name="strReplyInfo"></param>
		/// <param name="iResult">0表示成功；9999表示调用异常错误,其他为ICE接口返回错误</param>
		/// <param name="Msg"></param>
		/// <returns></returns>
		public static bool middleInvoke(string serviceName,string requestMsg,bool secret,out string strReplyInfo,out short iResult,out string Msg)
		{
			//目前只能为ex_common_query_service使用
			strReplyInfo = "";
			iResult = 9999;
			Msg = "";
			try
			{

				CFTGWCLib.CFTCryptoClass cf = new CFTGWCLib.CFTCryptoClass();

				if(serviceName != "ex_common_query_service" && serviceName != "order_update_service")
				{
					string CFTAcc = ConfigurationSettings.AppSettings["CFTAccount"].Trim();  //未在web.config中配置

					if (secret == true)
						requestMsg = cf.EncapsRequest(CFTAcc,requestMsg);      //加密
			
				
					requestMsg = "sp_id=" + CFTAcc + "&request_text=" + ICEEncode(requestMsg);  //组包
				}

				ICEReceiveThread irt = new ICEReceiveThread(ICEServerIP,ICEPort,serviceName,requestMsg,0,20000,iceUsr,icePwd);

				irt.OpenThread();

				//for(int i=0; i<50; i++)
				for(int i=0; i<400; i++)
				{
					Thread.Sleep(50);
					if(irt.flag > 0)
					{
						irt.Close();
						strReplyInfo = irt.strReplyInfo;
						iResult = (short)irt.iResult;
						Msg = irt.Msg;
						return true;
					}

					if(i == 399)
					{
						irt.Close();
						throw new Exception("ICE处理超时！");
					}
				}		

				return true;
			}
			catch(Exception err)
			{
				iResult = 9999;
				Msg = "调用ICE服务前失败" + err.Message;
				return false;
			}		
		}

	}


	/// <summary>
	/// ICE超时处理线程
	/// </summary>
	public class ICEReceiveThread
	{
		/// <summary>
		/// 接收到的信息
		/// </summary>
		public string strReplyInfo;
		public short iResult;
		public string Msg;

		/// <summary>
		/// 是否已得到结果
		/// </summary>
		public int flag = 0;

		private Thread tr = null;

		private string ficeserverip;
		private int ficeport;
		private string fservicename;
		private string frequestmsg;
		private short fserverflag;
		private short ftimeout;
		private string fuser;
		private string fpwd;

		/// <summary>
		/// 关闭线程
		/// </summary>
		public void Close()
		{
			if(tr != null )
			{
				tr.Abort();
			}

		}

		/// <summary>
		/// 构造函数
		/// </summary>
		public ICEReceiveThread(string iceserverip, int iceport, string servicename, string requestmsg, 
			short serverflag, short timeout, string user, string pwd)
		{
			ficeserverip = iceserverip;
			ficeport = iceport;
			fservicename = servicename;
			frequestmsg = requestmsg;
			fserverflag = serverflag;
			ftimeout = timeout;
			fuser = user;
			fpwd = pwd;
		}

		/// <summary>
		/// 开始线程
		/// </summary>
		public void OpenThread()
		{
			Thread tr = new Thread(new ThreadStart(RunThread));
			tr.Start();
		}

		/// <summary>
		/// 运行线程
		/// </summary>
		void RunThread()
		{

			try
			{
				
				BackProcessClient.CIceClientProxyClass cc = new BackProcessClient.CIceClientProxyClass();
				iResult = -1;
				iResult = cc.Init(ficeserverip, ficeport, out Msg); //初始化 IP 端口号
				

				//ICEInvoke cc = new ICEInvoke();
				try
				{
					//bool test = cc.Init(ficeserverip,ficeport);

					if(iResult == 0)
						//if(test)
					{
						//int iresult = 0;
						cc.MiddleInvoke(fservicename,frequestmsg,fserverflag,ftimeout,fuser,fpwd,out strReplyInfo,out iResult,out Msg);
						//iResult = (short)iresult;
						flag = 1;
					}
					else
					{
						Msg = "初始化ICE报错 " + Msg;
						strReplyInfo = "";
						flag = 9;
					}
				}
				finally
				{
					cc.Finit();	

					//cc.Dispose();
				}
			}
			catch(Exception err)
			{
				Msg = err.Message;
				flag = 9;
			}            
		}


	}
}
