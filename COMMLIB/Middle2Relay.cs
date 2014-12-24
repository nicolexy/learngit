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
	/// Middle2Relay 的摘要说明。
	/// </summary>
	internal class Middle2Relay
	{
		private static string myconnstring = "";
		private static Hashtable ht_relayconfigs = null;

		public Middle2Relay()
		{
			//
			// TODO: 在此处添加构造函数逻辑
			//
		}

		/// <summary>
		/// 初始化relay模板数组。
		/// </summary>
		/// <param name="connstring">relay配置表所在的连接数据串</param>
		internal static void InitRelayInfo(string connstring)
		{
			if(connstring == "")
				return;

			//furion 20141023 这个连接串里包含了relay信息转发表。
			myconnstring = connstring;
			MySqlAccess da = new MySqlAccess(myconnstring);
			try
			{
				da.OpenConn();
				string strSql = "select * from c2c_zwdb.t_relay_config where Fflag=0 "; //0为可用，1为禁用
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
		/// 本service是否需要走relay，如果需要，先取出路由value，然后加密，然后调用。
		/// </summary>
		/// <param name="middlename">接口名称</param>
		/// <returns></returns>
		internal static bool NeedGoRelay(string middlename)
		{
			if(ht_relayconfigs == null || ht_relayconfigs.Count == 0)
				return false;

			return ht_relayconfigs.ContainsKey(middlename.ToLower());
		}

		/// <summary>
		/// 获取路由值。成功的话，在输出参数里放路由值。失败的话，在输出参数放失败信息
		/// </summary>
		/// <param name="middlename">接口名称</param>
		/// <param name="srcparam">原始参数，无加密</param>
		/// <param name="Msg">输出参数：路由值或失败信息</param>
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
		/// 走relay调用middle接口，返回值或失败信息放在输出参数里。
		/// </summary>
		/// <param name="middlename">接口名称</param>
		/// <param name="srcparam">调用参数，可以是加密后的</param>
		/// <param name="routevalue">路由值，由GetRouteParam函数从没加密的参数串中获取到</param>
		/// <param name="errMsg">接口返回串或失败信息</param>
		/// <returns></returns>
		internal static bool GoRelay(string middlename, string srcparam, string routevalue,out string strReplyinfo,out short iResult, out string errMsg)
		{
			errMsg = "调用前";
			strReplyinfo = "调用前";
			iResult = -1;

			if(!NeedGoRelay(middlename))
			{
				errMsg = "不需要走relay";
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
		/// 用参数调用relay。
		/// </summary>
		/// <param name="req_params">所有参数</param>
		/// <param name="Msg">返回结果或失败信息</param>
		/// <returns></returns>
		private  bool GetFromRelay(string req_params, out string strReplyinfo,out short iResult, out string Msg)
		{
			Msg = "";
			strReplyinfo = "";
			iResult = -1;

			if (req_params == null || req_params == "")
			{
				Msg = "参数为空";
				return false;
			}

            LogHelper.LogInfo("GetFromRelay send req_params:" + req_params);

			TcpClient tcpClient = new TcpClient();
			try
			{
				//将请求参数封装
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
				stream.Read(bufferOut, 0, 4);  //读取返回长度
				int len = BitConverter.ToInt32(bufferOut, 0);
				bufferOut = new byte[len];
				//stream.Read(bufferOut, 0, len); //读取返回内容

                int nowindex = 0;
                while (nowindex < len)
                {
                    nowindex += stream.Read(bufferOut, nowindex, len - nowindex); //读取返回内容
                }

				strReplyinfo = Encoding.Default.GetString(bufferOut);
               // strReplyinfo = CommQuery.IceDecode(Encoding.UTF8.GetString(Encoding.Default.GetBytes(strReplyinfo)));
				iResult = 0;
				Msg = "调用成功";

                LogHelper.LogInfo("GetFromRelay return strReplyinfo:" + strReplyinfo);
				return true;
			}
			catch (Exception e)
			{
				Msg = "调用前置机失败：" + e.Message;
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
		/// 获取路由值
		/// </summary>
		/// <param name="src_string">无加密的原始参数</param>
		/// <param name="result">路由值或为空</param>
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
					//从源串中开始求值。
					int iindexstart = src_string.IndexOf(routefield + "=");
					if(iindexstart > -1)
					{
						//寻找从这里开始后的下一个&
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
						//不允许，错误配置。
						result = "参数有误，"+routefield+"不存在";
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
		/// 调用relay。
		/// </summary>
		/// <param name="srcparam">接口调用参数，加密后</param>
		/// <param name="routevalue">预先提取出来的路由值</param>
		/// <param name="errMsg">返回信息或错误信息</param>
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

			//开始调用函数。
			if(!GetFromRelay(priorstring + "&" + srcparam,out strReplyinfo,out iResult, out errMsg))
			{
				return false;
			}

			return true;
		}

		/// <summary>
		/// 读取配置表中的一行来初始化自已
		/// </summary>
		/// <param name="drrelayconfig">配置表中的一条记录</param>
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
		/// 组合在middle参数前的附加参数
		/// </summary>
		/// <param name="src_string">调用middle的参数</param>
		/// <param name="routevalue">预先提取的路由值</param>
		/// <param name="result">返回值或错误信息</param>
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
