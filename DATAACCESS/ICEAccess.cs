using System;
using System.Data;
using System.Collections;
using BackProcessClientCS;

namespace TENCENT.OSS.CFT.KF.DataAccess
{
	/// <summary>
	/// ��ICE��װΪ�����ݿ�һ���ķ����ࡣ
	/// </summary>
	public class ICEAccess : IDisposable
	{
		private const int MiddleNo = 1;

		public static string ICEEncode(string instr)
		{
			if(instr == null || instr.Trim() == "")
				return instr;
			else
			{
				return instr.Replace("%","%25").Replace("=","%3d").Replace("&","%26");
			}
		}

		public static string URLEncode(string strField)
		{
			if(strField == null || strField == "")
				return "";
			else
				return 	System.Web.HttpUtility.UrlEncode(strField,System.Text.Encoding.GetEncoding("gb2312"));
		}

		public static string URLDecode(string strField)
		{
			if(strField == null || strField == "")
				return "";
			else
				return 	System.Web.HttpUtility.UrlDecode(strField,System.Text.Encoding.GetEncoding("gb2312"));
		}

		private string serverIP = "172.25.38.9";
		private int serverPort = 6644; 

		private ICEInvoke iceconn = null;
		private string lastError = "";

		public ICEAccess(string ServerIP, int ServerPort)
		{
			serverIP = ServerIP;
			serverPort = ServerPort;
		}

		public bool OpenConn()
		{
			if(iceconn != null)
			{
				iceconn.Dispose();
				iceconn = null;
			}
			
			iceconn = new ICEInvoke();
			if(iceconn.Init(serverIP,serverPort))
			{
				return true;
			}
			else
			{
				lastError = iceconn.GetLastError();
				return false;
			}
			
		}

		public void CloseConn()
		{
			if(iceconn != null)
			{
				iceconn.Dispose();
				iceconn = null;
			}
		}

		//��Ҫʵ�֣���ѯ���ֶΣ���ѯ����¼������¼���鷵�أ�ִ���޸ġ�

		/// <summary>
		/// �ͷ���Դ
		/// </summary>
		public void Dispose()
		{
			CloseConn();
		}


		

		/// <summary>
		/// ��Դ��������ѯ,����һ�м�¼
		/// </summary>
		/// <param name="iSourceType"></param>
		/// <param name="iSourceCmd"></param>
		/// <param name="strKey"></param>
		/// <param name="strPara"></param>
		/// <param name="strResp"></param>
		/// <returns></returns>
		public DataTable InvokeQuery_GetDataTable(int iSourceType,int iSourceCmd,string strKey,string strPara,out string strResp)
		{
			DataTable dtresult = null;
			strResp = "����ICEʱ����";

			if(strPara == null || strPara.Trim() == "")
				return null;

			int iParaLen = strPara.Length;
			int iRespLen = 0;
			int iresult = -1;

			if(iceconn.InvokeQuery(iSourceType,iSourceCmd,strKey,MiddleNo,iParaLen,strPara,out iRespLen,out strResp,out iresult))
			{
				if(iresult == 0 && strResp.StartsWith("result=0"))
				{
					//����strResp	
					//strResp = URLDecode(strResp);

					string[] strsplit = strResp.Split('&');

					if(strsplit.Length == 0)
						return null;

					dtresult = new DataTable();
					Hashtable ht = new Hashtable(strsplit.Length);

					foreach(string stmp in strsplit)
					{
						if(stmp == null || stmp.Trim() == "")
							continue;

						string[] fieldsplit = stmp.Split('=');
						
						if(fieldsplit.Length != 2)
							continue;

						ht.Add(fieldsplit[0],URLDecode(fieldsplit[1].Trim()));
						dtresult.Columns.Add(fieldsplit[0]);
					}

					DataRow dr = dtresult.NewRow();
					dr.BeginEdit();
					foreach(DataColumn dc in dtresult.Columns)
					{
						dr[dc.ColumnName] = ht[dc.ColumnName]; 
					}
					dr.EndEdit();

					dtresult.Rows.Add(dr);
					
					return dtresult;
				}
				else
				{
					strResp = "[" + strResp + "] [" +  YWCommandResult.GetResultDetailInfo(iresult.ToString()) + "]";
				}
			}

			return dtresult;
		}

		/// <summary>
		/// ������Դ��������ѯ�ӿ�,����һ��ֵ.
		/// </summary>
		/// <param name="iSourceType"></param>
		/// <param name="iSourceCmd"></param>
		/// <param name="strKey"></param>
		/// <param name="strPara"></param>
		/// <param name="strResp"></param>
		/// <returns></returns>
		public int InvokeQuery_GetOneResult(int iSourceType,int iSourceCmd,string strKey,string strPara,string strFieldName,out string strResp)
		{
			int iresult = -1;
			strResp = "����ICEʱ����";

			if(strPara == null || strPara.Trim() == "")
				return iresult;

			if(strFieldName == null || strFieldName.Trim() == "")
				return iresult;

			int iParaLen = strPara.Length;
			int iRespLen = 0;

			if(iceconn.InvokeQuery(iSourceType,iSourceCmd,strKey,MiddleNo,iParaLen,strPara,out iRespLen,out strResp,out iresult))
			{
				if(iresult == 0)
				{
					//����strResp	
					//strResp = URLDecode(strResp);
					strFieldName = strFieldName.Trim().ToLower();

					string strtmp = "&" + strFieldName + "=";
					int istartindex = strResp.ToLower().IndexOf(strtmp);

					if(istartindex == -1)
					{
						strResp = "δ��ѯ��ָ���ֶ�[" + strFieldName + "]";
						return -1;
					}

					istartindex += strtmp.Length;
					int iendindex = strResp.IndexOf("&",istartindex);
					if(iendindex == -1)
						iendindex = strResp.Length;

					strResp = strResp.Substring(istartindex  ,iendindex - istartindex);
					strResp = URLDecode(strResp);

					return iresult;
				}
				else
				{
					strResp = "[" + strResp + "] [" +  YWCommandResult.GetResultDetailInfo(iresult.ToString()) + "]";
				}
			}

			return iresult;
		}

		/// <summary>
		/// ������Դ�������޸Ľӿ�
		/// </summary>
		/// <param name="iSourceType"></param>
		/// <param name="iSourceCmd"></param>
		/// <param name="strKey"></param>
		/// <param name="strPara"></param>
		/// <param name="strResp"></param>
		/// <returns></returns>
		public int InvokeQuery_Exec(int iSourceType,int iSourceCmd,string strKey,string strPara,out string strResp)
		{
			int iresult = -1;
			strResp = "����ICEʱ����";

			if(strPara == null || strPara.Trim() == "")
				return iresult;

			int iParaLen = strPara.Length;
			int iRespLen = 0;

			if(iceconn.InvokeQuery(iSourceType,iSourceCmd,strKey,MiddleNo,iParaLen,strPara,out iRespLen,out strResp,out iresult))
			{
				if(iresult == 0 || iresult == 60027000)
				{
					if(strResp.StartsWith("result=0") || strResp.StartsWith("result=60027000"))
					{
						iresult = 0;
					}
					else
					{
						iresult = Int32.Parse(strResp.Substring(0,strResp.IndexOf("&")).Replace("result=","").Replace("&",""));
						strResp = "[" + strResp + "]";
					}
				}
				else
				{
					strResp = "[" + strResp + "] [" +  YWCommandResult.GetResultDetailInfo(iresult.ToString()) + "]";
				}
			}

			return iresult;
		}


		/// <summary>
		/// �������������
		/// </summary>
		/// <param name="transListNo"></param>
		/// <param name="sequenceNo"></param>
		/// <param name="cmdNo"></param>
		/// <param name="middleNo"></param>
		/// <param name="request"></param>
		/// <param name="strResp"></param>
		/// <returns></returns>
		public int Invoke30(string transListNo,string sequenceNo,int cmdNo,int middleNo,string request, out string strResp)
		{
			int iresult = -1;
			strResp = "����ICEʱ����";

			if(request == null || request.Trim() == "")
				return iresult;


			if(iceconn.InvokeV30(transListNo,sequenceNo,cmdNo,middleNo,request,out iresult,out strResp))
			{
				if(iresult == 0 || iresult == 60027000)
				{
					if(strResp.StartsWith("result=0") || strResp.StartsWith("result=60027000"))
					{
						iresult = 0;
					}
					else
					{
						iresult = Int32.Parse(strResp.Substring(0,strResp.IndexOf("&")).Replace("result=","").Replace("&",""));
						strResp = "[" + strResp + "]";
					}
				}
				else
				{
					strResp = "[" + strResp + "] [" +  YWCommandResult.GetResultDetailInfo(iresult.ToString()) + "]";
				}
			}

			return iresult;
		}
	}
}
