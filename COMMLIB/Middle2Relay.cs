using System;
using System.Configuration;
using System.Data;
using System.Collections;
using System.Web;
using System.Text;
using System.IO;
using System.Security.Cryptography; 
using TENCENT.OSS.C2C.Finance.Common;
using TENCENT.OSS.C2C.Finance.Common.CommLib;
using System.Threading;
using System.Web.Mail;
using System.Net.Sockets;
using System.Net;
using TENCENT.OSS.CFT.KF.DataAccess;
using SunLibrary;

namespace TENCENT.OSS.C2C.Finance.Common.CommLib
{
	/// <summary>
	/// Middle2Relay ��ժҪ˵����
	/// </summary>
	internal class Middle2Relay
	{
		private static string myconnstring = "";
		private static Hashtable ht_relayconfigs = null;

		public Middle2Relay()
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
		}

		/// <summary>
		/// ��ʼ��relayģ�����顣
		/// </summary>
		/// <param name="connstring">relay���ñ����ڵ��������ݴ�</param>
		internal static void InitRelayInfo(string connstring)
		{
			if(connstring == "")
				return;

			//furion 20141023 ������Ӵ��������relay��Ϣת����
			myconnstring = connstring;
			MySqlAccess da = new MySqlAccess(myconnstring);
			try
			{
				da.OpenConn();
				string strSql = "select * from c2c_zwdb.t_relay_config where Fflag=0 "; //0Ϊ���ã�1Ϊ����
				DataTable dt = da.GetTable(strSql);

				if(dt == null || dt.Rows.Count == 0)
					return;

				ht_relayconfigs = new Hashtable(dt.Rows.Count);
				foreach(DataRow dr in dt.Rows)
				{
					RelayConfigInfo rci = new RelayConfigInfo();
					if(rci.InitMyself(dr))
					{
						ht_relayconfigs.Add(dr["Fmiddlename"].ToString().ToLower(),rci);
					}
				}
			}
			finally
			{
				da.Dispose();
			}
		}

		/// <summary>
		/// ��service�Ƿ���Ҫ��relay�������Ҫ����ȡ��·��value��Ȼ����ܣ�Ȼ����á�
		/// </summary>
		/// <param name="middlename">�ӿ�����</param>
		/// <returns></returns>
		internal static bool NeedGoRelay(string middlename)
		{
			if(ht_relayconfigs == null || ht_relayconfigs.Count == 0)
				return false;

			return ht_relayconfigs.ContainsKey(middlename.ToLower());
		}

		/// <summary>
		/// ��ȡ·��ֵ���ɹ��Ļ���������������·��ֵ��ʧ�ܵĻ��������������ʧ����Ϣ
		/// </summary>
		/// <param name="middlename">�ӿ�����</param>
		/// <param name="srcparam">ԭʼ�������޼���</param>
		/// <param name="Msg">���������·��ֵ��ʧ����Ϣ</param>
		/// <returns></returns>
		internal static bool GetRouteParam(string middlename, string srcparam, out string Msg)
		{
			Msg = "";

			if(ht_relayconfigs == null || ht_relayconfigs.Count == 0)
				return false;

			RelayConfigInfo rci = (RelayConfigInfo)ht_relayconfigs[middlename.ToLower()];
			return rci.GetRouteParam(srcparam,out Msg);
		}

		/// <summary>
		/// ��relay����middle�ӿڣ�����ֵ��ʧ����Ϣ������������
		/// </summary>
		/// <param name="middlename">�ӿ�����</param>
		/// <param name="srcparam">���ò����������Ǽ��ܺ��</param>
		/// <param name="routevalue">·��ֵ����GetRouteParam������û���ܵĲ������л�ȡ��</param>
		/// <param name="errMsg">�ӿڷ��ش���ʧ����Ϣ</param>
		/// <returns></returns>
		internal static bool GoRelay(string middlename, string srcparam, string routevalue,out string strReplyinfo,out short iResult, out string errMsg)
		{
			errMsg = "����ǰ";
			strReplyinfo = "����ǰ";
			iResult = -1;

			if(!NeedGoRelay(middlename))
			{
				errMsg = "����Ҫ��relay";
				return false;
			}

			RelayConfigInfo rci = (RelayConfigInfo)ht_relayconfigs[middlename.ToLower()];
			return rci.GoRelay(srcparam,routevalue,out strReplyinfo,out iResult, out errMsg);
		}
	}

	internal class RelayConfigInfo
	{
		private string relayip = "";
		private string relayport = "";
		private string headu = ""; //
		private string ver = ""; //1
		private string requesttype = ""; //1000
		private string middlename = ""; //save_service
		private string routetype = ""; //transid
		private string routename = ""; //route_transid
		private string routefield = ""; //listid

		/// <summary>
		/// �ò�������relay��
		/// </summary>
		/// <param name="req_params">���в���</param>
		/// <param name="Msg">���ؽ����ʧ����Ϣ</param>
		/// <returns></returns>
		private  bool GetFromRelay(string req_params, out string strReplyinfo,out short iResult, out string Msg)
		{
			Msg = "";
			strReplyinfo = "";
			iResult = -1;

			if (req_params == null || req_params == "")
			{
				Msg = "����Ϊ��";
				return false;
			}

            LogHelper.LogInfo("GetFromRelay send req_params:" + req_params);

			TcpClient tcpClient = new TcpClient();
			try
			{
				//�����������װ
				byte[] b_msg = System.Text.Encoding.Default.GetBytes(req_params);
				byte[] b_msg_len = BitConverter.GetBytes(b_msg.Length);
				byte[] contData = new byte[b_msg_len.Length + b_msg.Length];
				b_msg_len.CopyTo(contData, 0);
				b_msg.CopyTo(contData, b_msg_len.Length);

				IPAddress ipAddress = IPAddress.Parse(relayip);
				IPEndPoint ipPort = new IPEndPoint(ipAddress, Int32.Parse(relayport));
				tcpClient.Connect(ipPort);
				NetworkStream stream = tcpClient.GetStream();
				stream.Write(contData, 0, contData.Length);

				byte[] bufferOut = new byte[4];
				stream.Read(bufferOut, 0, 4);  //��ȡ���س���
				int len = BitConverter.ToInt32(bufferOut, 0);
				bufferOut = new byte[len];
				//stream.Read(bufferOut, 0, len); //��ȡ��������

                int nowindex = 0;
                while (nowindex < len)
                {
                    nowindex += stream.Read(bufferOut, nowindex, len - nowindex); //��ȡ��������
                }

				strReplyinfo = Encoding.Default.GetString(bufferOut);
               // strReplyinfo = CommQuery.IceDecode(Encoding.UTF8.GetString(Encoding.Default.GetBytes(strReplyinfo)));
				iResult = 0;
				Msg = "���óɹ�";

                LogHelper.LogInfo("GetFromRelay return strReplyinfo:" + strReplyinfo);
				return true;
			}
			catch (Exception e)
			{
				Msg = "����ǰ�û�ʧ�ܣ�" + e.Message;
                LogHelper.LogInfo("GetFromRelay error: " + Msg);
				return false;
			}
			finally 
			{
				if(tcpClient != null)
					tcpClient.Close();
			}
		}

		/// <summary>
		/// ��ȡ·��ֵ
		/// </summary>
		/// <param name="src_string">�޼��ܵ�ԭʼ����</param>
		/// <param name="result">·��ֵ��Ϊ��</param>
		/// <returns></returns>
		public bool GetRouteParam(string src_string,out string result)
		{
			result = "";

			if(src_string == "")
				return false;

			try
			{
				if(routetype != "NULL")
				{
					//��Դ���п�ʼ��ֵ��
					int iindexstart = src_string.IndexOf(routefield + "=");
					if(iindexstart > -1)
					{
						//Ѱ�Ҵ����￪ʼ�����һ��&
						int iindexend = src_string.IndexOf("&",iindexstart + 2);
						if(iindexend > iindexstart)
						{
							result = src_string.Substring(iindexstart + routefield.Length + 1,iindexend - iindexstart - routefield.Length - 1);
						}
						else
							result = src_string.Substring(iindexstart + routefield.Length + 1);

						return true;
					}
					else
					{
						//�������������á�
						result = "��������"+routefield+"������";
						return false;
					}
				}
				return true;
			}
			catch(Exception err)
			{
				result = err.Message;
				return false;
			}
		}

		/// <summary>
		/// ����relay��
		/// </summary>
		/// <param name="srcparam">�ӿڵ��ò��������ܺ�</param>
		/// <param name="routevalue">Ԥ����ȡ������·��ֵ</param>
		/// <param name="errMsg">������Ϣ�������Ϣ</param>
		/// <returns></returns>
		public bool GoRelay(string srcparam, string routevalue, out string strReplyinfo,out short iResult, out string errMsg)
		{
			errMsg = "";
			strReplyinfo = "";
			iResult = -1;

			if(srcparam == null || srcparam == "")
				return false;

			string priorstring = "";
			if(!GetPriorString(srcparam, routevalue,out priorstring))
			{
				errMsg = priorstring;
				return false;
			}

			//��ʼ���ú�����
			if(!GetFromRelay(priorstring + "&" + srcparam,out strReplyinfo,out iResult, out errMsg))
			{
				return false;
			}

			return true;
		}

		/// <summary>
		/// ��ȡ���ñ��е�һ������ʼ������
		/// </summary>
		/// <param name="drrelayconfig">���ñ��е�һ����¼</param>
		/// <returns></returns>
		public bool InitMyself(DataRow drrelayconfig)
		{
			try
			{
				relayip = drrelayconfig["Frelayip"].ToString();
				relayport = drrelayconfig["Frelayport"].ToString();
				headu = drrelayconfig["Fheadu"].ToString();
				ver = drrelayconfig["Fver"].ToString();
				requesttype = drrelayconfig["Frequesttype"].ToString();
				middlename = drrelayconfig["Fmiddlename"].ToString();
				routetype = drrelayconfig["Froutetype"].ToString();
				routename = drrelayconfig["Froutename"].ToString();
				routefield = drrelayconfig["Froutefield"].ToString();
				return true;
			}
			catch
			{
				return false;
			}
		}

		/// <summary>
		/// �����middle����ǰ�ĸ��Ӳ���
		/// </summary>
		/// <param name="src_string">����middle�Ĳ���</param>
		/// <param name="routevalue">Ԥ����ȡ��·��ֵ</param>
		/// <param name="result">����ֵ�������Ϣ</param>
		/// <returns></returns>
		private bool GetPriorString(string src_string, string routevalue, out string result)
		{
			result = "";

			if(src_string == "")
				return false;

			try
			{
				result = "head_u=" + headu 
					+ "&ver=" + ver
					+ "&request_type=" + requesttype;

				if(routetype != "NULL")
				{
					result += "&route_type=" + routetype
						+ "&" + routename + "=" +routevalue;					
				}

				return true;
			}
			catch(Exception err)
			{
				result = err.Message;
				return false;
			}
		}
	}
}
