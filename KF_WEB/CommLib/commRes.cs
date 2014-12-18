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
	/// commMethod ��������һЩͨ�õķ��������緢����־��Web��service����Ҫʹ�õ���һЩ���õķ�����������
	/// ��Ҫ����publicRes��ѹ�����������ࡣ
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
		/// ��middle������������ּ��ܺͷǼ������֣�
		/// </summary>
		/// <param name="serviceName"></param>
		/// <param name="requestMsg"></param>
		/// <param name="secret"></param>
		/// <param name="strReplyInfo"></param>
		/// <param name="iResult">0��ʾ�ɹ���9999��ʾ�����쳣����,����ΪICE�ӿڷ��ش���</param>
		/// <param name="Msg"></param>
		/// <returns></returns>
		public static bool middleInvoke(string serviceName,string requestMsg,bool secret,out string strReplyInfo,out short iResult,out string Msg)
		{
			//Ŀǰֻ��Ϊex_common_query_serviceʹ��
			strReplyInfo = "";
			iResult = 9999;
			Msg = "";
			try
			{

				CFTGWCLib.CFTCryptoClass cf = new CFTGWCLib.CFTCryptoClass();

				if(serviceName != "ex_common_query_service" && serviceName != "order_update_service")
				{
					string CFTAcc = ConfigurationSettings.AppSettings["CFTAccount"].Trim();  //δ��web.config������

					if (secret == true)
						requestMsg = cf.EncapsRequest(CFTAcc,requestMsg);      //����
			
				
					requestMsg = "sp_id=" + CFTAcc + "&request_text=" + ICEEncode(requestMsg);  //���
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
						throw new Exception("ICE����ʱ��");
					}
				}		

				return true;
			}
			catch(Exception err)
			{
				iResult = 9999;
				Msg = "����ICE����ǰʧ��" + err.Message;
				return false;
			}		
		}

	}


	/// <summary>
	/// ICE��ʱ�����߳�
	/// </summary>
	public class ICEReceiveThread
	{
		/// <summary>
		/// ���յ�����Ϣ
		/// </summary>
		public string strReplyInfo;
		public short iResult;
		public string Msg;

		/// <summary>
		/// �Ƿ��ѵõ����
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
		/// �ر��߳�
		/// </summary>
		public void Close()
		{
			if(tr != null )
			{
				tr.Abort();
			}

		}

		/// <summary>
		/// ���캯��
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
		/// ��ʼ�߳�
		/// </summary>
		public void OpenThread()
		{
			Thread tr = new Thread(new ThreadStart(RunThread));
			tr.Start();
		}

		/// <summary>
		/// �����߳�
		/// </summary>
		void RunThread()
		{

			try
			{
				
				BackProcessClient.CIceClientProxyClass cc = new BackProcessClient.CIceClientProxyClass();
				iResult = -1;
				iResult = cc.Init(ficeserverip, ficeport, out Msg); //��ʼ�� IP �˿ں�
				

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
						Msg = "��ʼ��ICE���� " + Msg;
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
