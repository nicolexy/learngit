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
	/// getData 的摘要说明。
	/// </summary>
	public class getData
	{
		private getData()
		{
			//
			// TODO: 在此处添加构造函数逻辑
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
						return "未知" + bankCode;
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
					return "未知" + bankCode;
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
				case "招商银行":
				{
					return 1001;
				}
				case "工商银行":
				{
					return 1002;
				}
				case "农业银行":
				{
					return 1005;
				}
				case "建设银行":
				{
					return 1003;
				}
				case "中国银行":
				{
					return 1026;
				}
				case "交通银行":
				{
					return 1020;
				}
				case "浦发银行":
				{
					return 1004;
				}
				case "民生银行":
				{
					return 1006;
				}
				case "深发展银行":
				{
					return 1008;
				}
				case "平安银行":
				{
					return 0;
				}
				case "中信银行":
				{
					return 1021;
				}
				case "广发银行":
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
					return "招行一点通";
				}
				case "2002":
				{
					return "工行一点通";
				}
				case "2003":
				{
					return "建行一点通";
				}
				case "3001":
				{
					return "兴业信用卡";
				}
				case "3002":
				{
					return "中行信用卡";
				}
				case "3003":
				{
					return "工行EPOS";
				}
				case "2101":
				{
					return "江苏借记卡";
				}
				case "3100":
				{
					return "光大信用卡";
				}
				case "2034":
				{
					return "光大借记卡";
				}
				case "2005":
				{
					return "农行借记卡";
				}
				default:
				{
					return bankCode;
				}
			}
		}


		public static string GetBankNameFromBankCode(string bankCode)
		{
			// 目前搜索数据库字典未做好，所以先用硬编码
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
				case "身份证":
				{
					return 1;
				}
				case "护照":
				{
					return 2;
				}
				case "军官证":
				{
					return 3;
				}
				case "士兵证":
				{
					return 4;
				}
				case "回乡证":
				{
					return 5;
				}
				case "临时身份证":
				{
					return 6;
				}
				case "户口簿":
				{
					return 7;
				}
				case "警官证":
				{
					return 8;
				}
				case "台胞证":
				{
					return 9;
				}
				case "营业执照":
				{
					return 10;
				}
				case "其它证件":
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
					return "身份证";
				}
				case 2:
				{
					return "护照";
				}
				case 3:
				{
					return "军官证";
				}
				case 4:
				{
					return "士兵证";
				}
				case 5:
				{
					return "回乡证";
				}
				case 6:
				{
					return "临时身份证";
				}
				case 7:
				{
					return "户口簿";
				}
				case 8:
				{
					return "警官证";
				}
				case 9:
				{
					return "台胞证";
				}
				case 10:
				{
					return "营业执照";
				}
				case 11:
				{
					return "其他证件";
				}
				default:
				{
					return "未知的证件类型" + creCode;
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

		// 获取一点通银行列表，type：1代表借记卡，2代表信用卡
		public static BankClass[] GetOPBankList(int type)
		{
			if(type == 1)
			{
				if(bankClassList_JJK == null)
				{
					bankClassList_JJK = new BankClass[]
						{
							new BankClass(2002,"工商银行"),
							new BankClass(2005,"农业银行"),
							new BankClass(2001,"招商银行"),
							new BankClass(2003,"建设银行"),
							new BankClass(2105,"建行联名卡"),
							new BankClass(2006,"中国银行"),
							new BankClass(2119,"中信银行"),
							new BankClass(2034,"光大银行"),
							new BankClass(2101,"江苏银行"),
							new BankClass(2033,"邮储银行"),
							new BankClass(2109,"上海农商"),
							new BankClass(2102,"渤海银行"),
							new BankClass(2110,"鄂尔多斯"),
							new BankClass(2108,"桂林银行"),

							new BankClass(2103,"广东农信"),
							new BankClass(2111,"南昌银行"),
							new BankClass(2104,"晋中商行"),
							new BankClass(2112,"云南农信"),
							new BankClass(2114,"广州银行"),
							new BankClass(2116,"重庆农商"),
							new BankClass(2118,"南粤银行"),
							new BankClass(2113,"包商银行")
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
							new BankClass(3104,"中国银行"),
							new BankClass(3100,"光大银行"),
							new BankClass(3102,"广发银行"),
							new BankClass(3200,"上海农商")
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

		// 获取快捷支付银行列表，type：1代表借记卡，2代表信用卡
		public static BankClass[] GetFPBankList(int type)
		{
			if(type == 1)
			{
				if(bankClassList_JJK_FP == null)
				{
					bankClassList_JJK_FP = new BankClass[]
					{
						new BankClass(4184,"工商银行F"),
						new BankClass(2013,"建设银行F"),
						new BankClass(4186,"农业银行F")
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
						new BankClass(3003,"工商银行F"),
						new BankClass(3007,"农业银行F"),
						new BankClass(3107,"中国银行F"),
						new BankClass(3004,"建设银行F"),
						new BankClass(3006,"招商银行F")
					};
				}

				return bankClassList_XYK_FP;
			}
			else
			{
				return null;
			}
		}




		#region 快捷回复页面的数据管理

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
