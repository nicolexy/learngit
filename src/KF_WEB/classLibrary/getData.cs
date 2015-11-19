using System;
using System.Collections;
using System.Data.SqlTypes;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.IO;

namespace TENCENT.OSS.CFT.KF.KF_Web.classLibrary
{
	/// <summary>
	/// getData ��ժҪ˵����
	/// </summary>
	public class getData
	{
		private getData()
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
		}

		private static Hashtable bankList = new Hashtable();

		//private static object obj_lockBankList = new object();

		private static int m_isTestMode = 0;
		private static int m_isNewSensitivePowerMode = 0;

		public static bool IsNewSensitivePowerMode
		{
			get
			{
				try
				{
					if(m_isNewSensitivePowerMode == 0)
					{
						if(System.Configuration.ConfigurationManager.AppSettings["isNewSensitivePowerMode"] == null)
						{
							m_isNewSensitivePowerMode = 2;
						}
						else if(System.Configuration.ConfigurationManager.AppSettings["isNewSensitivePowerMode"].Trim().ToLower() == "true")
						{
							m_isNewSensitivePowerMode = 1;
						}
						else
						{
							m_isNewSensitivePowerMode = 2;
						}
					}

					return m_isNewSensitivePowerMode == 1;
				}
				catch
				{
					//m_isNewSensitivePowerMode = 2;
					return false;
				}
			}
		}


		public static bool IsTestMode
		{
			get
			{
				try
				{
					if(m_isTestMode == 0)
					{
						if(System.Configuration.ConfigurationManager.AppSettings["isTestingMode"] == null)
						{
							m_isTestMode = 2;
						}
						else if(System.Configuration.ConfigurationManager.AppSettings["isTestingMode"].Trim().ToLower() == "true")
						{
							m_isTestMode = 1;
						}
						else
						{
							m_isTestMode = 2;
						}
					}

					return m_isTestMode == 1;
				}
				catch
				{
					//m_isTestMode = 2;
					return false;
				}
			}
		}


		private static string GetBankNameFromBankCode_bySearchDB(string bankCode)
		{
			try
			{
				if(bankList == null)
					bankList = new Hashtable();

				if(bankList.Count == 0)
				{
					string errMsg = "";

					DataSet ds = setConfig.QueryDicInfoByType("BANK_TYPE",out errMsg);

					if(ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
					{
						return "δ֪" + bankCode;
					}

					foreach(DataRow dr in ds.Tables[0].Rows)
					{
						if(bankList[dr["Fvalue"].ToString()] == null)
							bankList.Add(dr["Fvalue"].ToString(),dr["Fmemo"].ToString());
					}
				}
			

				if(bankList.Contains(bankCode))
				{
					string result = bankList[bankCode].ToString();

					return result;
				}
				else
					return "δ֪" + bankCode;
			}
			catch
			{
				return bankCode;
			}
		}


		private static int GetBankCodeFromBankName_byPreCoding(string bankName)
		{
			switch(bankName)
			{
				case "��������":
				{
					return 1001;
				}
				case "��������":
				{
					return 1002;
				}
				case "ũҵ����":
				{
					return 1005;
				}
				case "��������":
				{
					return 1003;
				}
				case "�й�����":
				{
					return 1026;
				}
				case "��ͨ����":
				{
					return 1020;
				}
				case "�ַ�����":
				{
					return 1004;
				}
				case "��������":
				{
					return 1006;
				}
				case "�չ����":
				{
					return 1008;
				}
				case "ƽ������":
				{
					return 0;
				}
				case "��������":
				{
					return 1021;
				}
				case "�㷢����":
				{
					return 1027;
				}
				default:
				{
					return 0;
				}
			}
		}


		private static string GetBankNameFromBankCode_byPreCoding(string bankCode)
		{
			switch(bankCode)
			{
				case "2001":
				{
					return "����һ��ͨ";
				}
				case "2002":
				{
					return "����һ��ͨ";
				}
				case "2003":
				{
					return "����һ��ͨ";
				}
				case "3001":
				{
					return "��ҵ���ÿ�";
				}
				case "3002":
				{
					return "�������ÿ�";
				}
				case "3003":
				{
					return "����EPOS";
				}
				case "2101":
				{
					return "���ս�ǿ�";
				}
				case "3100":
				{
					return "������ÿ�";
				}
				case "2034":
				{
					return "����ǿ�";
				}
				case "2005":
				{
					return "ũ�н�ǿ�";
				}
				default:
				{
					return bankCode;
				}
			}
		}


		public static string GetBankNameFromBankCode(string bankCode)
		{
			// Ŀǰ�������ݿ��ֵ�δ���ã���������Ӳ����
			//return GetBankNameFromBankCode_byPreCoding(bankCode);

			return GetBankNameFromBankCode_bySearchDB(bankCode);

		}


		public static int GetBankCodeFromBankName(string bankName)
		{
			return GetBankCodeFromBankName_byPreCoding(bankName);
		}


		public static int GetCreCodeFromCreName(string creName)
		{
			return GetCreCodeFromCreName_PreCoding(creName);
		}


		public static string GetCreNameFromCreCode(int creCode)
		{
			return GetCreNameFromCreCode_PreCoding(creCode);
		}


		public static string GetCreNameFromCreCode(string creCode)
		{
			return GetCreNameFromCreCode_PreCoding(int.Parse(creCode));
		}


		private static int GetCreCodeFromCreName_PreCoding(string creName)
		{
			switch(creName)
			{
				case "���֤":
				{
					return 1;
				}
				case "����":
				{
					return 2;
				}
				case "����֤":
				{
					return 3;
				}
				case "ʿ��֤":
				{
					return 4;
				}
				case "����֤":
				{
					return 5;
				}
				case "��ʱ���֤":
				{
					return 6;
				}
				case "���ڲ�":
				{
					return 7;
				}
				case "����֤":
				{
					return 8;
				}
				case "̨��֤":
				{
					return 9;
				}
				case "Ӫҵִ��":
				{
					return 10;
				}
				case "����֤��":
				{
					return 11;
				}
				default:
				{
					return 0;
				}
			}
		}


		private static string GetCreNameFromCreCode_PreCoding(int creCode)
		{
			switch(creCode)
			{
				case 1:
				{
					return "���֤";
				}
				case 2:
				{
					return "����";
				}
				case 3:
				{
					return "����֤";
				}
				case 4:
				{
					return "ʿ��֤";
				}
				case 5:
				{
					return "����֤";
				}
				case 6:
				{
					return "��ʱ���֤";
				}
				case 7:
				{
					return "���ڲ�";
				}
				case 8:
				{
					return "����֤";
				}
				case 9:
				{
					return "̨��֤";
				}
				case 10:
				{
					return "Ӫҵִ��";
				}
				case 11:
				{
					return "����֤��";
				}
				default:
				{
					return "δ֪��֤������" + creCode;
				}
			}
		}


		public class BankClass
		{
			public BankClass(int _value,string name)
			{
				bankValue = _value;
				bankName = name;
			}

			public int bankValue;
			public string bankName;
		}


		private static BankClass[] bankClassList_JJK = null;
		private static BankClass[] bankClassList_XYK = null;

		// ��ȡһ��ͨ�����б�type��1�����ǿ���2�������ÿ�
		public static BankClass[] GetOPBankList(int type)
		{
			if(type == 1)
			{
				if(bankClassList_JJK == null)
				{
					bankClassList_JJK = new BankClass[]
						{
							new BankClass(2002,"��������"),
							new BankClass(2005,"ũҵ����"),
							new BankClass(2001,"��������"),
							new BankClass(2003,"��������"),
							new BankClass(2105,"����������"),
							new BankClass(2006,"�й�����"),
							new BankClass(2119,"��������"),
							new BankClass(2034,"�������"),
							new BankClass(2101,"��������"),
							new BankClass(2033,"�ʴ�����"),
							new BankClass(2109,"�Ϻ�ũ��"),
							new BankClass(2102,"��������"),
							new BankClass(2110,"������˹"),
							new BankClass(2108,"��������"),

							new BankClass(2103,"�㶫ũ��"),
							new BankClass(2111,"�ϲ�����"),
							new BankClass(2104,"��������"),
							new BankClass(2112,"����ũ��"),
							new BankClass(2114,"��������"),
							new BankClass(2116,"����ũ��"),
							new BankClass(2118,"��������"),
							new BankClass(2113,"��������")
						};
				}

				return bankClassList_JJK;
			}
			else if(type == 2)
			{
				if(bankClassList_XYK == null)
				{
					bankClassList_XYK = new BankClass[]
						{
							new BankClass(3104,"�й�����"),
							new BankClass(3100,"�������"),
							new BankClass(3102,"�㷢����"),
							new BankClass(3200,"�Ϻ�ũ��")
						};
				}

				return bankClassList_XYK;
			}
			else
			{
				return null;
			}
		}


		private static BankClass[] bankClassList_JJK_FP = null;
		private static BankClass[] bankClassList_XYK_FP = null;

		// ��ȡ���֧�������б�type��1�����ǿ���2�������ÿ�
		public static BankClass[] GetFPBankList(int type)
		{
			if(type == 1)
			{
				if(bankClassList_JJK_FP == null)
				{
					bankClassList_JJK_FP = new BankClass[]
					{
						new BankClass(4184,"��������F"),
						new BankClass(2013,"��������F"),
						new BankClass(4186,"ũҵ����F")
					};
				}

				return bankClassList_JJK_FP;
			}
			else if(type == 2)
			{
				if(bankClassList_XYK_FP == null)
				{
					bankClassList_XYK_FP = new BankClass[]
					{
						new BankClass(3003,"��������F"),
						new BankClass(3007,"ũҵ����F"),
						new BankClass(3107,"�й�����F"),
						new BankClass(3004,"��������F"),
						new BankClass(3006,"��������F")
					};
				}

				return bankClassList_XYK_FP;
			}
			else
			{
				return null;
			}
		}




		#region ��ݻظ�ҳ������ݹ���

		private const string FILE_NAME = "FreezeFastReply.txt";
		private const int MAX_FASTREPLY_COUNT = 255;
		private const int PER_READ_COUNT = 1024;
		private static char[] SPLIT_CHAR = new char[]{'|'};
		private static int strCount = 0;
		private static string[] strFastReplayList = null;

		private static FileStream fs = null;


		private static string[] GetFreezeFastReplay(System.Web.UI.Page page)
		{
			try
			{
				strCount = 0;
				strFastReplayList = new string[MAX_FASTREPLY_COUNT];
				string path = page.Server.MapPath(page.Request.ApplicationPath) + "\\" + FILE_NAME;
				fs = new FileStream(path,FileMode.Open);

				byte[] buff = new byte[PER_READ_COUNT];
				int readIndex = 0,readCount = 0;

				while((readCount=fs.Read(buff,readIndex,buff.Length)) > 0)
				{
					readIndex += readCount;
					string strRead = System.Text.Encoding.UTF8.GetString(buff,0,buff.Length);
					string[] strRead_List = strRead.Split(SPLIT_CHAR);
					for(int i=0;i<(strRead_List.Length - 1);i++)
					{
						if(strRead_List[i] != null && strRead_List[i].Trim() != "" && strRead_List[i][0] != '\0')
							strFastReplayList[strCount++] = setConfig.replaceHtmlStr(strRead_List[i]);
					}
				}

				return strFastReplayList;
			}
			catch(Exception ex)
			{
				string str = ex.Message;
				//strFastReplayList = null;
				return strFastReplayList;
			}
			finally
			{
				if(fs != null)
				{
					fs.Close();
					fs=null;
				}
			}
		}



		public static string[] GetFreezeFastReplay(System.Web.UI.Page page,bool needUpdate)
		{
			if(strFastReplayList == null)
			{
				return GetFreezeFastReplay(page);
			}
			else
			{
				if(needUpdate)
				{
					return GetFreezeFastReplay(page);
				}

				return strFastReplayList;
			}
		}


		public static bool AddFreezeFastReplay(System.Web.UI.Page page,string reply)
		{
			try
			{
				if(strFastReplayList == null)
				{
					GetFreezeFastReplay(page);
				}

				reply += "|";

				string path = page.Server.MapPath(page.Request.ApplicationPath) + "\\" + FILE_NAME;
				fs = new FileStream(path,FileMode.Append);

				byte[] buff = System.Text.Encoding.UTF8.GetBytes(reply.ToCharArray(),0,reply.Length);

				fs.Write(buff,0,buff.Length);

				fs.Flush();

				strFastReplayList[strCount++] = reply;

				return true;
			}
			catch(Exception ex)
			{
				string str = ex.Message;
				return false;
			}
			finally
			{
				if(fs != null)
				{
					fs.Close();
					fs=null;
				}
			}
		}



		public static bool DeleteFreezeFastReply(System.Web.UI.Page page,int index)
		{
			return UpdateFreezeFastReply(page,"",index);		
		}



		public static bool DeleteFreezeFastReply_2(System.Web.UI.Page page,int index)
		{
			try
			{
				if(strFastReplayList == null)
				{
					GetFreezeFastReplay(page);
				}

				string path = page.Server.MapPath(page.Request.ApplicationPath) + "\\" + FILE_NAME;
				fs = new FileStream(path,FileMode.Open);

				if(strFastReplayList[index] != null && strFastReplayList[index].Trim() != "")
				{
					int wordLen = 0;
					for(int i=0;i<index;i++)
					{
						if(strFastReplayList[i] != null && strFastReplayList[i].Trim() != "")
						{
							wordLen += strFastReplayList[i].Length;
						}
					}

					fs.Seek(wordLen,SeekOrigin.Begin);
					byte[] buff = new byte[strFastReplayList[index].Length];
					for(int i=0;i<buff.Length;i++)
						buff[i] = 0;
					fs.Write(buff,0,buff.Length);

					strFastReplayList[index] = "";

					fs.Flush();
				}

				return true;
			}
			catch(Exception ex)
			{
				string str = ex.Message;
				return false;
			}
			finally
			{
				if(fs != null)
				{
					fs.Close();
					fs=null;
				}
			}
		}



		public static bool UpdateFreezeFastReply(System.Web.UI.Page page,string reply,int index)
		{
			try
			{
				if(strFastReplayList == null)
				{
					GetFreezeFastReplay(page);
				}

				string path = page.Server.MapPath(page.Request.ApplicationPath) + "\\" + FILE_NAME;
				fs = new FileStream(path,FileMode.Create);

				if(strFastReplayList[index] != null && strFastReplayList[index].Trim() != "")
				{
					strFastReplayList[index] = reply;

					//foreach(string str in strFastReplayList)
					for(int i=0;i<strCount;i++)
					{
						if(strFastReplayList[i] != null && strFastReplayList[i].Trim() != "")
						{
							string str = new string(strFastReplayList[i].ToCharArray());
							str += "|";
							byte[] buff = System.Text.Encoding.UTF8.GetBytes(str.ToCharArray(),0,str.Length);
							fs.Write(buff,0,buff.Length);
						}
					}

					fs.Flush();
				}

				fs.Close();

				//strFastReplayList = null;
				//GetFreezeFastReplay(page);

				return true;
			}
			catch(Exception ex)
			{
				string str = ex.Message;
				return false;
			}
			finally
			{
				if(fs != null)
				{
					fs.Close();
					fs=null;
				}
			}
		}

#endregion

		


	}
}
