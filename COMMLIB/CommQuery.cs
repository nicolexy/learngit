using System;
using System.Data;
using System.Collections;
using System.Text;
using System.Xml;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Reflection;


namespace TENCENT.OSS.C2C.Finance.Common.CommLib
{
	/// <summary>
	/// ICE������ѯ
	/// </summary>
	public class CommQuery
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
			{				
				string stmp = System.Web.HttpUtility.UrlDecode(strField,System.Text.Encoding.GetEncoding("gb2312"));
				return stmp.Replace("?26","&");
				/*
				//sreply������а������ʱ��������. 20091120
				//���ַ�����ֽ���,���� %26�������ֽں�,�滻Ϊ & ��һ���ֽ�.
				byte[] buffer = System.Text.Encoding.GetEncoding("gb2312").GetBytes(stmp);
				int ilen = buffer.Length;
				
				//int deslen = ilen;
				byte[] desbuffer = new byte[ilen];
				int desindex = 0;
				
				for(int i=0; i< ilen ; i++)
				{
					if(buffer[i] == 25 && i< ilen - 2)
					{
						//if(buffer[i+1] == 
					}
					else 
					{
						desbuffer[desindex] = buffer[i];
						desindex++;
						continue;
					}
				}
				return stmp;
				*/
			}
		}

		public CommQuery()
		{
		}

		public static string �н��ʻ����� = "QUERY_FINANCE_SUM";
		public static string �����ܻ� = "QUERY_TRADE_SUM";
		public static string QUERY_TRADE_INFO_BYID = "QUERY_CHARGE_INFO_BY_LOGID";
		//public static string QUERY_TRADE_INFO_BYDETAIL = "QUERY_CHARGE_INFO_BY_DETAIL";
		public static string QUERY_TRADE_INFO_BYDETAIL = "QUERY_CHARGE_INFO";

		//public static string QUERY_FUNDINFO_LISTID = "QUERY_FUNDINFO_LISTID";
		//public static string QUERY_FUNDINFO_UID = "QUERY_FUNDINFO_UID";

		public static string QUERY_CHARGEINFO_BYID ="QUERY_CHARGEINFO_BYID";
		public static string QUERY_CHARGEINFO_BYDETAIL = "QUERY_CHARGEINFO_BYDETAIL";

		public static string QUERY_UNDOINFO_BYID = "QUERY_UNDOINFO_BYID";
		public static string QUERY_UNFOINFO_BYUID = "QUERY_UNDOINFO_BYUID";

		public static string QUERY_GETCASHINFO_BYID = "QUERY_GETCASHINFO_BYID";
		public static string QUERY_GETCASHINFO_BYDETAIL = "QUERY_GETCASHINFO_BYDETAIL";
		
		public static string �̻�ת�ʲ�ѯ = "QUERY_SP_PAYLOG_1";
		public static string �̻�ת�ʱ��� = "QUERY_SP_PAYLOG_2";

		public static string C�ʻ��ʽ���ˮ = "QUERY_USER_BANKROLL";

		public static string �����ʽ���ˮ = "QUERY_USER_BANKROLL_FULL";

		public static string ���׵��ʽ���ˮ_���� = "QUERY_BANKROLL_LISTID_2";
		public static string ���׵��ʽ���ˮ_�̻� = "QUERY_BANKROLL_LISTID_1";
		public static string ���׵��ʽ���ˮ_���� = "QUERY_BANKROLL_LISTID_3";

		public static string QUERY_USERINFO = "FINANCE_UI_QUERY_USERINFO";
		public static string UPDATE_USERINFO = "FINANCE_UI_UPDATE_USERINFO";
		public static string INSERT_USERINFO = "FINANCE_UI_INSERT_USERINFO";

        public static string QUERY_UNFINISHTRADE_BUY= "UNREG_QUERY_PAIPAI_TRANS_1";//��ѯ����Ƿ���δ��ɽ���
        public static string QUERY_UNFINISHTRADE_SALE= "UNREG_QUERY_PAIPAI_TRANS_2";//��ѯ�����Ƿ���δ��ɽ���

		public static string QUERY_BANKUSER = "FINANCE_UI_QUERY_BANKUSER";
		public static string UPDATE_BANKUSER = "FINANCE_UI_UPDATE_BANKUSER";
		public static string BATCH_BANKUSER ="FINANCE_UI_QUERY_MULTI_BANK";  //�࿨���֣�һ�β�����п�
        public static string UPDATE_NON_PRIMIRY_BANK = "FINANCE_UI_MOD_FETCH_BANK_STATUS";  //�ⶳ������

		public static string QUERY_DIC = "FINANCE_UI_QUERY_DIC";
		public static string QUERY_DIC_COUNT = "FINANCE_UI_QUERY_DIC_C";
		public static string UPDATE_DICINFO = "FINANCE_UI_UPDATE_DIC";
		public static string INSERT_DICINFO = "FINANCE_UI_INSERT_DIC";

		public static string QUERY_USERATT = "FINANCE_UI_QUERY_USERATT";
		public static string INSERT_USERATT = "FINANCE_UI_INSERT_USERATT";
		public static string UPDATE_USERATT = "FINANCE_UI_UPDATE_USERATT";

		public static string QUERY_MUSER = "FINANCE_UI_QUERY_MUSER";
		public static string UPDATE_MUSER = "FINANCE_UI_UPDATE_MUSER";
		public static string INSERT_MUSER = "FINANCE_UI_INSERT_MUSER";

		public static string QUERY_RELATION = "FINANCE_UI_QUERY_RELATION";
		public static string UPDATE_RELATION = "FINANCE_UI_UPDATE_RELATION";
		public static string INSERT_RELATION = "FINANCE_UI_INSERT_RELATION";
		public static string DELETE_RELATION = "FINANCE_UI_DELETE_RELATION";

		public static string QUERY_MERCHANTINFO = "FINANCE_UI_QUERY_MERCHANTINFO";

		//select * from c2c_db.t_merchant_info where Fspecial='$special$';
		public static string QUERY_MERCHANTINFO_C = "FINANCE_UI_QUERY_MERCHANTINFO_C"; 
		public static string UPDATE_MERCHANTINFO = "FINANCE_UI_UPDATE_MERCHANTINFO";
		public static string INSERT_MERCHANTINFO = "FINANCE_UI_INSERT_MERCHANTINFO";


		public static string QUERY_WANT = "FINANCE_OD_QUERY_WANT";
		public static string UPDATE_WANT = "FINANCE_OD_UPDATE_WANT";

		public static string QUERY_APPEAL = "FINANCE_OD_QUERY_APPEAL";

		public static string QUERY_USERPAY_L = "FINANCE_OD_QUERY_USERPAY_L";
		public static string QUERY_USERPAY_U = "FINANCE_OD_QUERY_USERPAY_U";

		public static string QUERY_SPMREFUND = "FINANCE_OD_QUERY_SPMREFUND";
		public static string QUERY_SPMREFUND_COUNT = "FINANCE_OD_QUERY_SPMREFUND_C";
		public static string UPDATE_SPMREFUND = "FINANCE_OD_UPDATE_SPMREFUND";
		public static string INSERT_SPMREFUND = "FINANCE_OD_INSERT_SPMREFUND";

		//public static string QUERY_ORDER = "FINANCE_OD_QUERY_ORDER";
        public static string QUERY_ORDER = "query_order_service";//ͨ���ӿڲ�ѯ����yinhuang 14/8/9
		public static string UPDATE_ORDER = "FINANCE_OD_UPDATE_ORDER";

		public static string QUERY_TCBANKROLL = "FINANCE_OD_TCBANKROLL";  //������ѯ
		public static string QUERY_TCBANKROLL_COUNT = "FINANCE_OD_TCBANKROLL_C";  //ֻ��ѯcount
		public static string QUERY_TCBANKROLL_HISTORY = "FINANCE_OD_TCBANKROLL_H";  //��ѯ��ʷ
		//public static string QUERY_TCBANKROLL_HCOUNT = "FINANCE_OD_TCBANKROLL_HC";  //��ѯ��ʷcount ����ͨ�������������ѯ��andrew 20110324
		public static string QUERY_TCBANKROLL_PAYSUM = "FINANCE_OD_TCBANKROLL_PAYSUM"; //��ѯͳ��

		public static string QUERY_USEROPLOG = "FINANCE_UI_QUERY_USEROPLOG"; //��ѯ������ˮ��

		public static string QUERY_SYNREC_ID = "FINANCE_QUERY_SYNREC_ID"; //�õ��Ų�ѯͬ����¼
		public static string QUERY_SYNREC_SP = "FINANCE_QUERY_SYNREC_SP"; //�õ��Ų�ѯͬ����¼
		public static string QUERY_TCBANKROLL_S = "QUERY_MUL_CHARGE_LIST"; //��ѯc2c_order_db_xx.t_tcbankroll_list_x(����Fbank_list�ֿ�ֱ�)��queryserver rowenawu 20120301
		public static string QUERY_TCBANKROLL_DAY="FINANCE_OD_TCBANKROLL_DAY"; //��ѯ c2c_order_db.t_tcbankroll_list_yyyyMMdd (�����) ��ͨ�ò�ѯ rowenawu 20120306
		public static string QUERY_MCH_REFUND = "FINANCE_OD_QUERY_MCH_REFUND";//��ѯc2c_db_xx.t_merch_refund_x��transaction_id�ֿ�ֱ� andrew 20120222
		public static string QUERY_TRANSPORT ="FINANCE_QUERY_TRANSPORT"; //��ѯ������ ��Ϊcommquery
		public static string QUERY_REFUND_RELATION = "FINANCE_OD_QUERY_REFUND_RELATION";//��ѯc2c_db_xx.t_refund_relation_x(����Fdraw_id�ֿ�ֱ�) andrew 20120222
		public static string QUERY_USER_REFUND = "FINANCE_OD_QUERY_USER_REFUND";//��ѯc2c_db_xx.t_user_refund_x(����Fsp_uid�ֿ�ֱ�) andrew 20120222
		public static string QUERY_APPEAL_NEW = "FINANCE_UI_QUERY_APPEAL";//��ѯc2c_db_appeal.t_appeal
		public static string QUERY_MESSAGE = "FINANCE_UI_QUERY_MESSAGE";//��ѯc2c_db_$XX$.t_message_$Y$
		public static string QUERY_PAIPAI_REFUND = "FINANCE_OD_QUERY_SPM_REFUND";//��ѯ�����˿����뵥 andrew 20121015

        public static string QUERY_YT_INFO = "FINANCE_OD_QUERY_YT_INFO";//��ѯ��ͨ�˺���Ϣ
        public static string QUERY_YT_CERT_NUM = "FINANCE_OD_QUERY_YT_CERT_NUM";//��ѯ���֤��ͨ��ͨ�˺Ÿ���
        public static string QUERY_YT_FREEZE_INFO = "FINANCE_OD_QUERY_YT_FREEZE_INFO";//��ѯ��ͨ�˺Ŷ�����Ϣ
        public static string QUERY_YT_TRADE_INFO = "FINANCE_OD_QUERY_YT_TRADE_INFO";//��ѯ��ͨ�˺Ž�����Ϣ
        public static string QUERY_EXCHANGE_RATE_HISTROY = "FINANCE_OD_QUERY_EXCHANGE_RATE_H";//��ѯ��ʷ�����ʼ�¼
        public static string QUERY_FREE_FLOW = "FINANCE_OD_QUERY_FREE_FLOW";//��ѯ�������
        public static string QUERY_USER_TYPE = "FINANCE_OD_QUERY_USER_TYPE";//��ѯ�û�����

		public static int UpdateOrderFromICE(string strCmd,out string errMsg)
		{
			errMsg = "";

			string	sReply = "";
			short	iResult;
			string sMsg = "";

			string servicename =  "order_update_service";

			if(commRes.middleInvoke(servicename, strCmd, false, out sReply, out iResult, out sMsg))
			{
				if(iResult == 0)
				{
					errMsg = "���¶����ɹ�";
					return 0;
				}
				else
				{
					errMsg = "reply=" + sReply + "||msg=" + sMsg;
					return -2;
				}
			}
			else
			{
				errMsg = "����ICEʧ��";
				errMsg += "reply=" + sReply + "||msg=" + sMsg;
				return -1;
			}
		}

		public static int LockTcbankrollFromICE(string strCmd,out string errMsg)
		{
			errMsg = "";

			string	sReply = "";
			short	iResult;
			string sMsg = "";

			string servicename =  "order_prerefund_service";

			if(commRes.middleInvoke(servicename, strCmd, false, out sReply, out iResult, out sMsg))
			{
				if(iResult == 0)
				{
					errMsg = "�����˿�Ԥ����ɹ�";
					return 0;
				}
				else
				{
					errMsg = "reply=" + sReply + "||msg=" + sMsg;
					return -2;
				}
			}
			else
			{
				errMsg = "����ICEʧ��";
				errMsg += "reply=" + sReply + "||msg=" + sMsg;
				return -1;
			}
		}

		/// <summary>
		/// ���Ͽ�ĸ��¿�����������¶�����Ҫ������һ���ӿ�order_update_service��
		/// </summary>
		/// <param name="strWhere"></param>
		/// <param name="strCmd"></param>
		/// <param name="errMsg"></param>
		/// <returns></returns>
		public static int ExecSqlFromICE(string strWhere,string strCmd,out string errMsg)
		{
			errMsg = "";

			string	sReply = "";
			short	iResult;
			string sMsg = "";

			string sInmsg = "CMD=" + strCmd + "&" + strWhere;

			//string servicename =  "ex_common_query_service";
			string servicename =  "ui_common_update_service";
			bool needsecret = true;
			//����spmrefundʱ��ʹ��Сţ�Ľӿ�
			if(strCmd == "FINANCE_OD_UPDATE_SPMREFUND")
			{
				servicename = "order_zwupdate_service";
				sInmsg += "&MSG_NO=998" + DateTime.Now.Ticks.ToString();
				needsecret = false;
			}

			if(commRes.middleInvoke(servicename, sInmsg, needsecret, out sReply, out iResult, out sMsg))
			{
				if(iResult == 0)
				{
					errMsg = "ִ�гɹ�";
					//����Ҫ��ȡӰ�������� sReply���AFFECTED_ROW������
					string[] strlist1 = sReply.Split('&');

					if(strlist1.Length == 0)
					{
						errMsg = "����UI_UPDATEʧ��,���ؽ������" + sReply ;
						return -3;
					}

					Hashtable ht = new Hashtable(strlist1.Length);

					foreach(string strtmp in strlist1)
					{
						if(strtmp.Trim() == "")
							continue;
						string[] strlist2 = strtmp.Split('=');
						if(strlist2.Length != 2)
						{
							errMsg = "����UI_UPDATEʧ��,���ؽ������" + sReply ;
							return -4;
						}
                            
						ht.Add(strlist2[0].Trim(),strlist2[1].Trim());
					}

					if(!ht.Contains("result") || ht["result"].ToString().Trim() != "0" || !ht.Contains("AFFECTED_ROW") )
					{
						errMsg = "����UI_UPDATEʧ��,���ؽ������" + sReply ;
						return -5;
					}

					int irowcount = Int32.Parse(ht["AFFECTED_ROW"].ToString().Trim());

					return irowcount;
				}
				else
				{
					errMsg = "reply=" + sReply + "||msg=" + sMsg;
					return -2;
				}
			}
			else
			{
				errMsg = "����ICEʧ��";
				errMsg += "reply=" + sReply + "||msg=" + sMsg;
				return -1;
			}
		}


		public static string IceDecode(string str)
		{
			if(str == null)
				return "";

			return str.Replace("%26","&").Replace("%3d","=").Replace("%3D","=").Replace("%25","%");
		}


		public static DataSet GetDataSetFromReply(string sReply, out string errMsg)
		{
			errMsg = "";
			DataSet dsresult = new DataSet();
			Hashtable ht = new Hashtable();

			string[] strlist1 = sReply.Split('&');

			if(strlist1.Length == 0)
			{
				dsresult = null;
				errMsg = "����ͨ�ò�ѯʧ��,���ؽ������" + sReply ;
				return null;
			}

			ht = new Hashtable(strlist1.Length);

			foreach(string strtmp in strlist1)
			{
				string[] strlist2 = strtmp.Split('=');
				if(strlist2.Length != 2)
				{
					dsresult = null;
					errMsg = "����ͨ�ò�ѯʧ��,���ؽ������" + sReply ;
					return null;
				}
                            
				ht.Add(strlist2[0].Trim(),strlist2[1].Trim());
			}

			if(!ht.Contains("result") || ht["result"].ToString().Trim() != "0" || !ht.Contains("row_num") )
			{
				dsresult = null;
				errMsg = "����ͨ�ò�ѯʧ��,���ؽ������" + sReply ;
				return null;
			}

			int irowcount = Int32.Parse(ht["row_num"].ToString().Trim());

			if(irowcount == 0)
			{
				dsresult = null;
				errMsg = "��ѯ���Ϊ��";
				return null;
			}

			dsresult = new DataSet();
			DataTable dt = new DataTable();
			dsresult.Tables.Add(dt);

			string firstrow = ht["row1"].ToString().Trim();

			firstrow = URLDecode(firstrow);

			string[] strsplit3 = firstrow.Split('&');

			if(strsplit3.Length == 0)
			{
				dsresult = null;
				errMsg = "��ѯ������޷��������ֶ�";
				return null;
			}
						

			foreach(string stmp in strsplit3)
			{
				if(stmp == null || stmp.Trim() == "")
					continue;

				string[] fieldsplit = stmp.Split('=');
						
				if(fieldsplit.Length != 2)
					continue;

				dt.Columns.Add(fieldsplit[0]);
			}

			for(int i=1; i<=irowcount ;i++ )
			{
				string onerow = ht["row" + i].ToString().Trim();
				onerow = URLDecode(onerow);
				string[] strsplit_detail = onerow.Split('&');

				DataRow drfield = dt.NewRow();
				drfield.BeginEdit();							

				foreach(string stmp in strsplit_detail)
				{
					if(stmp == null || stmp.Trim() == "")
						continue;

					string[] fieldsplit = stmp.Split('=');
						
					if(fieldsplit.Length != 2)
						continue;

					//drfield[fieldsplit[0]] = URLDecode(fieldsplit[1].Trim());
					drfield[fieldsplit[0]] = IceDecode(fieldsplit[1].Trim());

					/*
								string strtmp = fieldsplit[1].Trim();
								byte[] srcbuff = Encoding.UTF8.GetBytes(strtmp);
								byte[] desbuff = Encoding.Convert(Encoding.UTF8,Encoding.GetEncoding("gb2312"),srcbuff);
								strtmp = Encoding.GetEncoding("gb2312").GetString(desbuff);

								drfield[fieldsplit[0]] = strtmp;
								*/
				}

				drfield.EndEdit();
				dt.Rows.Add(drfield);

			}

			return dsresult;
		}

		// 2012/4/9
		public static DataSet GetDataSetFromICE(string strWhere, string strCmd, bool isrealtime,string servicename, out string errMsg,bool isscrect)
		{
			DataSet dsresult = null;
			Hashtable ht = null;

			errMsg = "";

			string	sReply;
			short	iResult;
			string sMsg;

			string sInmsg = "CMD=" + strCmd + "&" + strWhere;

			//			string servicename =  "ex_common_query_service";
			//
			//			if(isrealtime)
			//			{
			//				servicename =  "ex_common_query_service";
			//			}

			/*
			if(strCmd=="query_order_service")//��query_order_serviceȥ�ĵ���ȥ��ѯ��ѯandrewguo 20120525
			{
				return GetDataSetFromICE_OrderServer(strWhere,strCmd,isrealtime,out errMsg);
			}
			*/

			if(commRes.middleInvoke(servicename, sInmsg, isscrect, out sReply, out iResult, out sMsg))
			{
				if(iResult == 0)
				{
					//��sreply���н���
					if(sReply == null || sReply.Trim() == "")
					{
						dsresult = null;
						errMsg = "����ͨ�ò�ѯʧ��,�޷��ؽ��" + servicename + sInmsg ;
						return null;
					}
					else
					{
						string[] strlist1 = sReply.Split('&');

						if(strlist1.Length == 0)
						{
							dsresult = null;
							errMsg = "����ͨ�ò�ѯʧ��,���ؽ������" + sReply ;
							return null;
						}

						ht = new Hashtable(strlist1.Length);

						foreach(string strtmp in strlist1)
						{
							string[] strlist2 = strtmp.Split('=');
							if(strlist2.Length != 2)
							{
								dsresult = null;
								errMsg = "����ͨ�ò�ѯʧ��,���ؽ������" + sReply ;
								return null;
							}
                            
							ht.Add(strlist2[0].Trim(),strlist2[1].Trim());
						}

						if(!ht.Contains("result") || ht["result"].ToString().Trim() != "0" || !ht.Contains("row_num") )
						{
							dsresult = null;
							errMsg = "����ͨ�ò�ѯʧ��,���ؽ������" + sReply ;
							return null;
						}

						int irowcount = Int32.Parse(ht["row_num"].ToString().Trim());

						if(irowcount == 0)
						{
							dsresult = null;
							errMsg = "��ѯ���Ϊ��";
							return null;
						}

						dsresult = new DataSet();
						DataTable dt = new DataTable();
						dsresult.Tables.Add(dt);

						string firstrow = ht["row1"].ToString().Trim();

						firstrow = URLDecode(firstrow);

						string[] strsplit3 = firstrow.Split('&');

						if(strsplit3.Length == 0)
						{
							dsresult = null;
							errMsg = "��ѯ������޷��������ֶ�";
							return null;
						}
						

						foreach(string stmp in strsplit3)
						{
							if(stmp == null || stmp.Trim() == "")
								continue;

							string[] fieldsplit = stmp.Split('=');
						
							if(fieldsplit.Length != 2)
								continue;

							dt.Columns.Add(fieldsplit[0]);
						}

						for(int i=1; i<=irowcount ;i++ )
						{
							string onerow = ht["row" + i].ToString().Trim();
							onerow = URLDecode(onerow);
							string[] strsplit_detail = onerow.Split('&');

							DataRow drfield = dt.NewRow();
							drfield.BeginEdit();							

							foreach(string stmp in strsplit_detail)
							{
								if(stmp == null || stmp.Trim() == "")
									continue;

								string[] fieldsplit = stmp.Split('=');
						
								if(fieldsplit.Length != 2)
									continue;

								//drfield[fieldsplit[0]] = URLDecode(fieldsplit[1].Trim());
								drfield[fieldsplit[0]] = IceDecode(fieldsplit[1].Trim());

								/*
								string strtmp = fieldsplit[1].Trim();
								byte[] srcbuff = Encoding.UTF8.GetBytes(strtmp);
								byte[] desbuff = Encoding.Convert(Encoding.UTF8,Encoding.GetEncoding("gb2312"),srcbuff);
								strtmp = Encoding.GetEncoding("gb2312").GetString(desbuff);

								drfield[fieldsplit[0]] = strtmp;
								*/
							}

							drfield.EndEdit();
							dt.Rows.Add(drfield);

						}

					}
					return dsresult;
				}
				else
				{
					dsresult = null;
					errMsg = "����ͨ�ò�ѯʧ��iresult=" + iResult + "|err="  + sMsg;
					return null;
				}
			}
			else
			{
				dsresult = null;
				errMsg = "����ͨ�ò�ѯʧ����:" + sMsg;
				return null;
			}

		}


		public static DataSet GetDataSetFromICE(string strWhere, string strCmd, out string errMsg)
		{
			return GetDataSetFromICE(strWhere,strCmd,false,"ex_common_query_service",out errMsg);
		}


		public static DataSet GetDataSetFromICE(string strWhere, string strCmd, bool isrealtime,string servicename, out string errMsg)
		{
			DataSet dsresult = null;
			Hashtable ht = null;

			errMsg = "";

			string	sReply;
			short	iResult;
			string sMsg;

			string sInmsg = "CMD=" + strCmd + "&" + strWhere;

			//			string servicename =  "ex_common_query_service";
			//
			//			if(isrealtime)
			//			{
			//				servicename =  "ex_common_query_service";
			//			}

			if(strCmd=="query_order_service")//��query_order_serviceȥ�ĵ���ȥ��ѯ��ѯandrewguo 20120525
			{
				return GetDataSetFromICE_OrderServer(strWhere,strCmd,isrealtime,out errMsg);
			}

			if(commRes.middleInvoke(servicename, sInmsg, false, out sReply, out iResult, out sMsg))
			{
				if(iResult == 0)
				{
					//��sreply���н���
					if(sReply == null || sReply.Trim() == "")
					{
						dsresult = null;
						errMsg = "����ͨ�ò�ѯʧ��,�޷��ؽ��" + servicename + sInmsg ;
						return null;
					}
					else
					{
						string[] strlist1 = sReply.Split('&');

						if(strlist1.Length == 0)
						{
							dsresult = null;
							errMsg = "����ͨ�ò�ѯʧ��,���ؽ������" + sReply ;
							return null;
						}

						ht = new Hashtable(strlist1.Length);

						foreach(string strtmp in strlist1)
						{
							string[] strlist2 = strtmp.Split('=');
							if(strlist2.Length != 2)
							{
								dsresult = null;
								errMsg = "����ͨ�ò�ѯʧ��,���ؽ������" + sReply ;
								return null;
							}
                            
							ht.Add(strlist2[0].Trim(),strlist2[1].Trim());
						}

						if(!ht.Contains("result") || ht["result"].ToString().Trim() != "0" || !ht.Contains("row_num") )
						{
							dsresult = null;
							errMsg = "����ͨ�ò�ѯʧ��,���ؽ������" + sReply ;
							return null;
						}

						int irowcount = Int32.Parse(ht["row_num"].ToString().Trim());

						if(irowcount == 0)
						{
							dsresult = null;
							errMsg = "��ѯ���Ϊ��";
							return null;
						}

						dsresult = new DataSet();
						DataTable dt = new DataTable();
						dsresult.Tables.Add(dt);

						string firstrow = ht["row1"].ToString().Trim();

						firstrow = URLDecode(firstrow);

						string[] strsplit3 = firstrow.Split('&');

						if(strsplit3.Length == 0)
						{
							dsresult = null;
							errMsg = "��ѯ������޷��������ֶ�";
							return null;
						}
						

						foreach(string stmp in strsplit3)
						{
							if(stmp == null || stmp.Trim() == "")
								continue;

							string[] fieldsplit = stmp.Split('=');
						
							if(fieldsplit.Length != 2)
								continue;

							dt.Columns.Add(fieldsplit[0]);
						}

						for(int i=1; i<=irowcount ;i++ )
						{
							string onerow = ht["row" + i].ToString().Trim();
							onerow = URLDecode(onerow);
							string[] strsplit_detail = onerow.Split('&');

							DataRow drfield = dt.NewRow();
							drfield.BeginEdit();							

							foreach(string stmp in strsplit_detail)
							{
								if(stmp == null || stmp.Trim() == "")
									continue;

								string[] fieldsplit = stmp.Split('=');
						
								if(fieldsplit.Length != 2)
									continue;

								//drfield[fieldsplit[0]] = URLDecode(fieldsplit[1].Trim());
								drfield[fieldsplit[0]] = IceDecode(fieldsplit[1].Trim());

								/*
								string strtmp = fieldsplit[1].Trim();
								byte[] srcbuff = Encoding.UTF8.GetBytes(strtmp);
								byte[] desbuff = Encoding.Convert(Encoding.UTF8,Encoding.GetEncoding("gb2312"),srcbuff);
								strtmp = Encoding.GetEncoding("gb2312").GetString(desbuff);

								drfield[fieldsplit[0]] = strtmp;
								*/
							}

							drfield.EndEdit();
							dt.Rows.Add(drfield);

						}

					}
					return dsresult;
				}
				else
				{
					dsresult = null;
					errMsg = "����ͨ�ò�ѯʧ��iresult=" + iResult + "|err="  + sMsg;
					return null;
				}
			}
			else
			{
				dsresult = null;
				errMsg = "����ͨ�ò�ѯʧ����:" + sMsg;
				return null;
			}

		}
      
        //��� ��� ͨ�ò�ѯ
        public static DataSet GetDataSetFromICEIA(string strWhere, string strCmd, out string errMsg)
        {
            return GetDataSetFromICEIA(strWhere, strCmd, false, "ex_common_query_service", out errMsg);
        }
        //���middle��ѯ
        public static DataSet GetDataSetFromICEIA(string strWhere, string strCmd, bool isrealtime, string servicename, out string errMsg)
        {
            DataSet dsresult = null;
            Hashtable ht = null;

            errMsg = "";

            string sReply;
            short iResult;
            string sMsg;

            string sInmsg = "CMD=" + strCmd + "&" + strWhere;

            //			string servicename =  "ex_common_query_service";
            //
            //			if(isrealtime)
            //			{
            //				servicename =  "ex_common_query_service";
            //			}

            if (strCmd == "query_order_service")//��query_order_serviceȥ�ĵ���ȥ��ѯ��ѯandrewguo 20120525
            {
                return GetDataSetFromICE_OrderServer(strWhere, strCmd, isrealtime, out errMsg);
            }
            if (commRes.middleInvokeIA(servicename, sInmsg, false, out sReply, out iResult, out sMsg))
            {
                if (iResult == 0)
                {
                    //��sreply���н���
                    if (sReply == null || sReply.Trim() == "")
                    {
                        dsresult = null;
                        errMsg = "����ͨ�ò�ѯʧ��,�޷��ؽ��" + servicename + sInmsg;
                        return null;
                    }
                    else
                    {
                        string[] strlist1 = sReply.Split('&');

                        if (strlist1.Length == 0)
                        {
                            dsresult = null;
                            errMsg = "����ͨ�ò�ѯʧ��,���ؽ������" + sReply;
                            return null;
                        }

                        ht = new Hashtable(strlist1.Length);

                        foreach (string strtmp in strlist1)
                        {
                            string[] strlist2 = strtmp.Split('=');
                            if (strlist2.Length != 2)
                            {
                                dsresult = null;
                                errMsg = "����ͨ�ò�ѯʧ��,���ؽ������" + sReply;
                                return null;
                            }

                            ht.Add(strlist2[0].Trim(), strlist2[1].Trim());
                        }

                        if (!ht.Contains("result") || ht["result"].ToString().Trim() != "0" || !ht.Contains("row_num"))
                        {
                            dsresult = null;
                            errMsg = "����ͨ�ò�ѯʧ��,���ؽ������" + sReply;
                            return null;
                        }

                        int irowcount = Int32.Parse(ht["row_num"].ToString().Trim());

                        if (irowcount == 0)
                        {
                            dsresult = null;
                            errMsg = "��ѯ���Ϊ��";
                            return null;
                        }

                        dsresult = new DataSet();
                        DataTable dt = new DataTable();
                        dsresult.Tables.Add(dt);

                        string firstrow = ht["row1"].ToString().Trim();

                        firstrow = URLDecode(firstrow);

                        string[] strsplit3 = firstrow.Split('&');

                        if (strsplit3.Length == 0)
                        {
                            dsresult = null;
                            errMsg = "��ѯ������޷��������ֶ�";
                            return null;
                        }


                        foreach (string stmp in strsplit3)
                        {
                            if (stmp == null || stmp.Trim() == "")
                                continue;

                            string[] fieldsplit = stmp.Split('=');

                            if (fieldsplit.Length != 2)
                                continue;

                            dt.Columns.Add(fieldsplit[0]);
                        }

                        for (int i = 1; i <= irowcount; i++)
                        {
                            string onerow = ht["row" + i].ToString().Trim();
                            onerow = URLDecode(onerow);
                            string[] strsplit_detail = onerow.Split('&');

                            DataRow drfield = dt.NewRow();
                            drfield.BeginEdit();

                            foreach (string stmp in strsplit_detail)
                            {
                                if (stmp == null || stmp.Trim() == "")
                                    continue;

                                string[] fieldsplit = stmp.Split('=');

                                if (fieldsplit.Length != 2)
                                    continue;

                                //drfield[fieldsplit[0]] = URLDecode(fieldsplit[1].Trim());
                                drfield[fieldsplit[0]] = IceDecode(fieldsplit[1].Trim());

                                /*
                                string strtmp = fieldsplit[1].Trim();
                                byte[] srcbuff = Encoding.UTF8.GetBytes(strtmp);
                                byte[] desbuff = Encoding.Convert(Encoding.UTF8,Encoding.GetEncoding("gb2312"),srcbuff);
                                strtmp = Encoding.GetEncoding("gb2312").GetString(desbuff);

                                drfield[fieldsplit[0]] = strtmp;
                                */
                            }

                            drfield.EndEdit();
                            dt.Rows.Add(drfield);

                        }

                    }
                    return dsresult;
                }
                else
                {
                    dsresult = null;
                    errMsg = "����ͨ�ò�ѯʧ��iresult=" + iResult + "|err=" + sMsg;
                    return null;
                }
            }
            else
            {
                dsresult = null;
                errMsg = "����ͨ�ò�ѯʧ����:" + sMsg;
                return null;
            }

        }

        public static DataSet GetOneTableFromICE(string strWhere, string strCmd, string servicename,bool isCret, out string errMsg) 
        {
            DataSet dsresult = null;
            Hashtable ht = null;

            errMsg = "";

            string sReply;
            short iResult;
            string sMsg;

            string sInmsg = "CMD=" + strCmd + "&" + strWhere;

            if (commRes.middleInvoke(servicename, sInmsg, isCret, out sReply, out iResult, out sMsg))
            {
                if (iResult == 0)
                {
                    //��sreply���н���
                    if (sReply == null || sReply.Trim() == "")
                    {
                        errMsg = "����ͨ�ò�ѯʧ��,�޷��ؽ��" + servicename + sInmsg;
                        return null;
                    }
                    else
                    {
                        string[] strlist1 = sReply.Split('&');

                        if (strlist1.Length == 0)
                        {
                            errMsg = "����ͨ�ò�ѯʧ��,���ؽ������" + sReply;
                            return null;
                        }

                        ht = new Hashtable(strlist1.Length);
                        dsresult = new DataSet();
                        DataTable dt = new DataTable();
                        dsresult.Tables.Add(dt);
                        DataRow drfield = dt.NewRow();

                        drfield.BeginEdit();
                        foreach (string strtmp in strlist1)
                        {
                            string[] strlist2 = strtmp.Split('=');
                            if (strlist2.Length != 2)
                            {
                                continue;
                            }
                            dt.Columns.Add(strlist2[0]);

                            drfield[strlist2[0]] = IceDecode(strlist2[1].Trim());

                            ht.Add(strlist2[0].Trim(), strlist2[1].Trim());
                        }
                        drfield.EndEdit();
                        dt.Rows.Add(drfield);

                        if (!ht.Contains("result") || ht["result"].ToString().Trim() != "0")
                        {
                            errMsg = "����ͨ�ò�ѯʧ��,���ؽ������" + sReply;
                            return null;
                        }
                    }
                    return dsresult;
                }
                else
                {
                    errMsg = "����ͨ�ò�ѯʧ��iresult=" + iResult + "|err=" + sMsg;
                    return null;
                }
            }
            else
            {
                errMsg = "����ͨ�ò�ѯʧ����:" + sMsg;
                return null;
            }
        }
        
        //����̻���Ϣ��ѯ���ù� middleInvoke  ȥ��CMD����
        public static DataSet FCGetOneTableFromICE(string strWhere, string strCmd, string servicename, bool isCret, out string errMsg)
        {
            DataSet dsresult = null;
            Hashtable ht = null;

            errMsg = "";

            string sReply;
            short iResult;
            string sMsg;

            string sInmsg = "&" + strWhere;

            if (commRes.middleInvokeIA(servicename, sInmsg, isCret, out sReply, out iResult, out sMsg))
            {
                if (iResult == 0)
                {
                    //��sreply���н���
                    if (sReply == null || sReply.Trim() == "")
                    {
                        errMsg = "����ͨ�ò�ѯʧ��,�޷��ؽ��" + servicename + sInmsg;
                        return null;
                    }
                    else
                    {
                        string[] strlist1 = sReply.Split('&');

                        if (strlist1.Length == 0)
                        {
                            errMsg = "����ͨ�ò�ѯʧ��,���ؽ������" + sReply;
                            return null;
                        }

                        ht = new Hashtable(strlist1.Length);
                        dsresult = new DataSet();
                        DataTable dt = new DataTable();
                        dsresult.Tables.Add(dt);
                        DataRow drfield = dt.NewRow();

                        drfield.BeginEdit();
                        foreach (string strtmp in strlist1)
                        {
                            string[] strlist2 = strtmp.Split('=');
                            if (strlist2.Length != 2)
                            {
                                continue;
                            }
                            dt.Columns.Add(strlist2[0]);

                            drfield[strlist2[0]] = IceDecode(strlist2[1].Trim());

                            ht.Add(strlist2[0].Trim(), strlist2[1].Trim());
                        }
                        drfield.EndEdit();
                        dt.Rows.Add(drfield);

                        if (!ht.Contains("result") || ht["result"].ToString().Trim() != "0")
                        {
                            errMsg = "����ͨ�ò�ѯʧ��,���ؽ������" + sReply;
                            return null;
                        }
                    }
                    return dsresult;
                }
                else
                {
                    errMsg = "����ͨ�ò�ѯʧ��iresult=" + iResult + "|err=" + sMsg;
                    return null;
                }
            }
            else
            {
                errMsg = "����ͨ�ò�ѯʧ����:" + sMsg;
                return null;
            }
        }

        //���ϵͳ��middleInvokeIA  ȥ��CMD����
        public static DataSet AIGetOneTableFromICE(string strWhere, string strCmd, string servicename, bool isCret, out string errMsg)
        {
            DataSet dsresult = null;
            Hashtable ht = null;

            errMsg = "";

            string sReply;
            short iResult;
            string sMsg;

            string sInmsg = "&" + strWhere;

            if (commRes.middleInvokeIA(servicename, sInmsg, isCret, out sReply, out iResult, out sMsg))
            {
                if (iResult == 0)
                {
                    //��sreply���н���
                    if (sReply == null || sReply.Trim() == "")
                    {
                        errMsg = "����ͨ�ò�ѯʧ��,�޷��ؽ��" + servicename + sInmsg;
                        return null;
                    }
                    else
                    {
                        string[] strlist1 = sReply.Split('&');

                        if (strlist1.Length == 0)
                        {
                            errMsg = "����ͨ�ò�ѯʧ��,���ؽ������" + sReply;
                            return null;
                        }

                        ht = new Hashtable(strlist1.Length);
                        dsresult = new DataSet();
                        DataTable dt = new DataTable();
                        dsresult.Tables.Add(dt);
                        DataRow drfield = dt.NewRow();

                        drfield.BeginEdit();
                        foreach (string strtmp in strlist1)
                        {
                            string[] strlist2 = strtmp.Split('=');
                            if (strlist2.Length != 2)
                            {
                                continue;
                            }
                            dt.Columns.Add(strlist2[0]);

                           // drfield[strlist2[0]] = IceDecode(strlist2[1].Trim());
                            drfield[strlist2[0]] = strlist2[1].Trim();

                            ht.Add(strlist2[0].Trim(), strlist2[1].Trim());
                        }
                        drfield.EndEdit();
                        dt.Rows.Add(drfield);

                        if (!ht.Contains("result") || ht["result"].ToString().Trim() != "0")
                        {
                            errMsg = "����ͨ�ò�ѯʧ��,���ؽ������" + sReply;
                            return null;
                        }
                    }
                    return dsresult;
                }
                else
                {
                    errMsg = "����ͨ�ò�ѯʧ��iresult=" + iResult + "|err=" + sMsg;
                    return null;
                }
            }
            else
            {
                errMsg = "����ͨ�ò�ѯʧ����:" + sMsg;
                return null;
            }
        }

        //��� ���� ͨ�ò�ѯ ��ҪCMD
        public static DataSet GeOnetDataSetFromICEIA(string strWhere, string strCmd, out string errMsg)
        {
            return OneAIGetOneTableFromICE(strWhere, strCmd, "ex_common_query_service", false, out errMsg);
        }
        //��� ���� ͨ�ò�ѯ ��ҪCMD
        public static DataSet OneAIGetOneTableFromICE(string strWhere, string strCmd, string servicename, bool isCret, out string errMsg)
        {
            DataSet dsresult = null;
            Hashtable ht = null;

            errMsg = "";

            string sReply;
            short iResult;
            string sMsg;

            string sInmsg = "CMD=" + strCmd + "&" + strWhere;

            if (commRes.middleInvokeIA(servicename, sInmsg, isCret, out sReply, out iResult, out sMsg))
            {
                if (iResult == 0)
                {
                    //��sreply���н���
                    if (sReply == null || sReply.Trim() == "")
                    {
                        errMsg = "����ͨ�ò�ѯʧ��,�޷��ؽ��" + servicename + sInmsg;
                        return null;
                    }
                    else
                    {
                        string[] strlist1 = sReply.Split('&');

                        if (strlist1.Length == 0)
                        {
                            errMsg = "����ͨ�ò�ѯʧ��,���ؽ������" + sReply;
                            return null;
                        }

                        ht = new Hashtable(strlist1.Length);
                        dsresult = new DataSet();
                        DataTable dt = new DataTable();
                        dsresult.Tables.Add(dt);
                        DataRow drfield = dt.NewRow();

                        drfield.BeginEdit();
                        foreach (string strtmp in strlist1)
                        {
                            string[] strlist2 = strtmp.Split('=');
                            if (strlist2.Length != 2)
                            {
                                continue;
                            }
                            dt.Columns.Add(strlist2[0]);

                            // drfield[strlist2[0]] = IceDecode(strlist2[1].Trim());
                            drfield[strlist2[0]] = strlist2[1].Trim();

                            ht.Add(strlist2[0].Trim(), strlist2[1].Trim());
                        }
                        drfield.EndEdit();
                        dt.Rows.Add(drfield);

                        if (!ht.Contains("result") || ht["result"].ToString().Trim() != "0")
                        {
                            errMsg = "����ͨ�ò�ѯʧ��,���ؽ������" + sReply;
                            return null;
                        }
                    }
                    return dsresult;
                }
                else
                {
                    errMsg = "����ͨ�ò�ѯʧ��iresult=" + iResult + "|err=" + sMsg;
                    return null;
                }
            }
            else
            {
                errMsg = "����ͨ�ò�ѯʧ����:" + sMsg;
                return null;
            }
        }
        public static DataSet GetOneTableFromICE(string strWhere, string strCmd, string servicename, out string errMsg)
        {
            return GetOneTableFromICE(strWhere, strCmd, servicename, true, out errMsg);

        }

        //����
        public static DataSet GetXmlToDataSetFromICE(string strWhere, string strCmd, string servicename, out string errMsg)
        {
            DataSet dsresult = null;
            Hashtable ht = null;

            errMsg = "";

            string sReply;
            short iResult;
            string sMsg;

            string sInmsg = "CMD=" + strCmd + "&" + strWhere;

            if (commRes.middleInvoke(servicename, sInmsg, true, out sReply, out iResult, out sMsg))
            {
                if (iResult == 0)
                {
                    //��sreply���н���
                    if (sReply == null || sReply.Trim() == "")
                    {
                        errMsg = "����ͨ�ò�ѯʧ��,�޷��ؽ��" + servicename + sInmsg;
                        return null;
                    }
                    else
                    {
                        sReply = CleanInvalidXmlChars(sReply);
                        string[] strlist1 = sReply.Split('&');

                        if (strlist1.Length == 0)
                        {
                            errMsg = "����ͨ�ò�ѯʧ��,���ؽ������" + sReply;
                            return null;
                        }

                        ht = new Hashtable(strlist1.Length);

                        foreach (string strtmp in strlist1)
                        {
                            string[] strlist2 = strtmp.Split('=');
                            if (strlist2.Length != 2)
                            {
                                continue;
                            }
                            
                            ht.Add(strlist2[0].Trim(), strlist2[1].Trim());
                        }

                        if (!ht.Contains("result") || ht["result"].ToString().Trim() != "0")
                        {
                            errMsg = "����ͨ�ò�ѯʧ��,���ؽ������" + sReply;
                            return null;
                        }

                        //����xml
                        string rec_xml = ht["rec_info"].ToString();
                        if (rec_xml != null && rec_xml != "") {
                            dsresult = new DataSet();
                            DataTable dt = new DataTable();
                            dsresult.Tables.Add(dt);

                            //<record>
                            XmlDocument doc = new XmlDocument();
                            doc.LoadXml(rec_xml);
                            XmlElement root = doc.DocumentElement;
                            XmlNodeList record_el = root.SelectNodes("record");
                            //foreach(XmlElement el in record_el)
                            for(int i=0;i<record_el.Count;i++)
                            {
                                DataRow drfield = dt.NewRow();
							    drfield.BeginEdit();

                                XmlNodeList ch_list = record_el[i].ChildNodes;
                                foreach (XmlElement ele in ch_list) {
                                    string name = ele.Name.Trim();
                                    if (i == 0) {
                                        dt.Columns.Add(name, typeof(String));
                                    }
                                    drfield[name] = ele.InnerText;
                                }
                                
                                //drfield[name] = el.SelectSingleNode(name).InnerText.ToString();

                                drfield.EndEdit();
							    dt.Rows.Add(drfield);
                            }
                        
                        }
                    }
                    return dsresult;
                }
                else
                {
                    errMsg = "����ͨ�ò�ѯʧ��iresult=" + iResult + "|err=" + sMsg;
                    return null;
                }
            }
            else
            {
                errMsg = "����ͨ�ò�ѯʧ����:" + sMsg;
                return null;
            }

        }

        //������
        public static DataSet GetXmlToDataSetFromICE(string strWhere, string strCmd, string servicename, out string errMsg,bool iskey)
        {
            DataSet dsresult = null;
            Hashtable ht = null;

            errMsg = "";

            string sReply;
            short iResult;
            string sMsg;

            string sInmsg = "CMD=" + strCmd + "&" + strWhere;

            if (commRes.middleInvoke(servicename, sInmsg, iskey, out sReply, out iResult, out sMsg))
            {
                if (iResult == 0)
                {
                    //��sreply���н���
                    if (sReply == null || sReply.Trim() == "")
                    {
                        errMsg = "����ͨ�ò�ѯʧ��,�޷��ؽ��" + servicename + sInmsg;
                        return null;
                    }
                    else
                    {
                        sReply = CleanInvalidXmlChars(sReply);
                        string[] strlist1 = sReply.Split('&');

                        if (strlist1.Length == 0)
                        {
                            errMsg = "����ͨ�ò�ѯʧ��,���ؽ������" + sReply;
                            return null;
                        }

                        ht = new Hashtable(strlist1.Length);

                        foreach (string strtmp in strlist1)
                        {
                            string[] strlist2 = strtmp.Split('=');
                            if (strlist2.Length != 2)
                            {
                                continue;
                            }

                            ht.Add(strlist2[0].Trim(), strlist2[1].Trim());
                        }

                        if (!ht.Contains("result") || ht["result"].ToString().Trim() != "0")
                        {
                            errMsg = "����ͨ�ò�ѯʧ��,���ؽ������" + sReply;
                            return null;
                        }

                        //����xml
                        string rec_xml = ht["rec_info"].ToString();
                        if (rec_xml != null && rec_xml != "")
                        {
                            dsresult = new DataSet();
                            DataTable dt = new DataTable();
                            dsresult.Tables.Add(dt);

                            //<record>
                            XmlDocument doc = new XmlDocument();
                            doc.LoadXml(rec_xml);
                            XmlElement root = doc.DocumentElement;
                            XmlNodeList record_el = root.SelectNodes("record");
                            //foreach(XmlElement el in record_el)
                            for (int i = 0; i < record_el.Count; i++)
                            {
                                DataRow drfield = dt.NewRow();
                                drfield.BeginEdit();

                                XmlNodeList ch_list = record_el[i].ChildNodes;
                                foreach (XmlElement ele in ch_list)
                                {
                                    string name = ele.Name.Trim();
                                    if (i == 0)
                                    {
                                        dt.Columns.Add(name, typeof(String));
                                    }
                                    drfield[name] = ele.InnerText;
                                }

                                //drfield[name] = el.SelectSingleNode(name).InnerText.ToString();

                                drfield.EndEdit();
                                dt.Rows.Add(drfield);
                            }

                        }
                    }
                    return dsresult;
                }
                else
                {
                    errMsg = "����ͨ�ò�ѯʧ��iresult=" + iResult + "|err=" + sMsg;
                    return null;
                }
            }
            else
            {
                errMsg = "����ͨ�ò�ѯʧ����:" + sMsg;
                return null;
            }

        }

        //����  ��֮ͬ���������ؽ�������ʽת��uft8
        public static DataSet GetXmlToDataSetFromICEConvertUTF8(string strWhere, string strCmd, string servicename, out string errMsg)
        {
            DataSet dsresult = null;
            Hashtable ht = null;

            errMsg = "";

            string sReply;
            short iResult;
            string sMsg;

            string sInmsg = "CMD=" + strCmd + "&" + strWhere;

            if (commRes.middleInvoke(servicename, sInmsg, true, out sReply, out iResult, out sMsg))
            {
                if (iResult == 0)
                {
                    //�����ؽ�������ʽת��uft8
                    sReply = Encoding.UTF8.GetString(Encoding.Default.GetBytes(sReply)).Replace("?/","</");
                    //��sreply���н���
                    if (sReply == null || sReply.Trim() == "")
                    {
                        errMsg = "����ͨ�ò�ѯʧ��,�޷��ؽ��" + servicename + sInmsg;
                        return null;
                    }
                    else
                    {
                        sReply = CleanInvalidXmlChars(sReply);
                        string[] strlist1 = sReply.Split('&');

                        if (strlist1.Length == 0)
                        {
                            errMsg = "����ͨ�ò�ѯʧ��,���ؽ������" + sReply;
                            return null;
                        }

                        ht = new Hashtable(strlist1.Length);

                        foreach (string strtmp in strlist1)
                        {
                            string[] strlist2 = strtmp.Split('=');
                            if (strlist2.Length != 2)
                            {
                                continue;
                            }

                            ht.Add(strlist2[0].Trim(), strlist2[1].Trim());
                        }

                        if (!ht.Contains("result") || ht["result"].ToString().Trim() != "0")
                        {
                            errMsg = "����ͨ�ò�ѯʧ��,���ؽ������" + sReply;
                            return null;
                        }

                        //����xml
                        string rec_xml = ht["rec_info"].ToString();
                        if (rec_xml != null && rec_xml != "")
                        {
                            dsresult = new DataSet();
                            DataTable dt = new DataTable();
                            dsresult.Tables.Add(dt);

                            //<record>
                            XmlDocument doc = new XmlDocument();
                            doc.LoadXml(rec_xml);
                            XmlElement root = doc.DocumentElement;
                            XmlNodeList record_el = root.SelectNodes("record");
                            //foreach(XmlElement el in record_el)
                            for (int i = 0; i < record_el.Count; i++)
                            {
                                DataRow drfield = dt.NewRow();
                                drfield.BeginEdit();

                                XmlNodeList ch_list = record_el[i].ChildNodes;
                                foreach (XmlElement ele in ch_list)
                                {
                                    string name = ele.Name.Trim();
                                    if (i == 0)
                                    {
                                        dt.Columns.Add(name, typeof(String));
                                    }
                                    drfield[name] = ele.InnerText;
                                }

                                //drfield[name] = el.SelectSingleNode(name).InnerText.ToString();

                                drfield.EndEdit();
                                dt.Rows.Add(drfield);
                            }

                        }
                    }
                    return dsresult;
                }
                else
                {
                    errMsg = "����ͨ�ò�ѯʧ��iresult=" + iResult + "|err=" + sMsg;
                    return null;
                }
            }
            else
            {
                errMsg = "����ͨ�ò�ѯʧ����:" + sMsg;
                return null;
            }

        }

        //��middle�ӿڻ�ȡ��¼��������total_num����row_num
        public static int GetTotalNumFromICE(string strWhere, string strCmd, string servicename, out string errMsg, bool iskey)
        {
            int totalNum = 0;
            Hashtable ht = null;

            errMsg = "";

            string sReply;
            short iResult;
            string sMsg;

            string sInmsg = "CMD=" + strCmd + "&" + strWhere;

            if (commRes.middleInvoke(servicename, sInmsg, iskey, out sReply, out iResult, out sMsg))
            {
                if (iResult == 0)
                {
                    //��sreply���н���
                    if (sReply == null || sReply.Trim() == "")
                    {
                        errMsg = "����ͨ�ò�ѯʧ��,�޷��ؽ��" + servicename + sInmsg;
                        return 0;
                    }
                    else
                    {
                        sReply = CleanInvalidXmlChars(sReply);
                        string[] strlist1 = sReply.Split('&');

                        if (strlist1.Length == 0)
                        {
                            errMsg = "����ͨ�ò�ѯʧ��,���ؽ������" + sReply;
                            return 0;
                        }

                        ht = new Hashtable(strlist1.Length);

                        foreach (string strtmp in strlist1)
                        {
                            string[] strlist2 = strtmp.Split('=');
                            if (strlist2.Length != 2)
                            {
                                continue;
                            }

                            ht.Add(strlist2[0].Trim(), strlist2[1].Trim());
                        }

                        if (!ht.Contains("result") || ht["result"].ToString().Trim() != "0")
                        {
                            errMsg = "����ͨ�ò�ѯʧ��,���ؽ������" + sReply;
                            return 0;
                        }
                        totalNum = int.Parse(ht["total_num"].ToString());
                    }
                    return totalNum;
                }
                else
                {
                    errMsg = "����ͨ�ò�ѯʧ��iresult=" + iResult + "|err=" + sMsg;
                    return 0;
                }
            }
            else
            {
                errMsg = "����ͨ�ò�ѯʧ����:" + sMsg;
                return 0;
            }

        }

        public static DataSet GetDSForServiceFromICE(string strWhere, string servicename, bool isCret, out string errMsg)
        {
            DataSet dsresult = null;
            Hashtable ht = null;

            errMsg = "";

            string sReply;
            short iResult;
            string sMsg;

            if (commRes.middleInvoke(servicename, strWhere, isCret, out sReply, out iResult, out sMsg))
            {
                if (iResult == 0)
                {
                    //��sreply���н���
                    if (sReply == null || sReply.Trim() == "")
                    {
                        errMsg = "����ͨ�ò�ѯʧ��,�޷��ؽ��" + servicename + strWhere;
                        return null;
                    }
                    else
                    {
                        string[] strlist1 = sReply.Split('&');

                        if (strlist1.Length == 0)
                        {
                            errMsg = "����ͨ�ò�ѯʧ��,���ؽ������" + sReply;
                            return null;
                        }

                        ht = new Hashtable(strlist1.Length);
                        dsresult = new DataSet();
                        DataTable dt = new DataTable();
                        dsresult.Tables.Add(dt);
                        DataRow drfield = dt.NewRow();

                        drfield.BeginEdit();
                        foreach (string strtmp in strlist1)
                        {
                            string[] strlist2 = strtmp.Split('=');
                            if (strlist2.Length != 2)
                            {
                                continue;
                            }
                            dt.Columns.Add(strlist2[0]);

                            drfield[strlist2[0]] = IceDecode(strlist2[1].Trim());

                            ht.Add(strlist2[0].Trim(), strlist2[1].Trim());
                        }
                        drfield.EndEdit();
                        dt.Rows.Add(drfield);

                        if (!ht.Contains("result") || ht["result"].ToString().Trim() != "0")
                        {
                            errMsg = "����ͨ�ò�ѯʧ��,���ؽ������" + sReply;
                            return null;
                        }
                    }
                    return dsresult;
                }
                else
                {
                    errMsg = "����ͨ�ò�ѯʧ��iresult=" + iResult + "|err=" + sMsg;
                    return null;
                }
            }
            else
            {
                errMsg = "����ͨ�ò�ѯʧ����:" + sMsg;
                return null;
            }
        }

        public static DataSet GetDataSetFromICEService(string strWhere, string strCmd, bool isrealtime, string servicename, out string errMsg, bool isscrect)
        {
            DataSet dsresult = null;
            Hashtable ht = null;

            errMsg = "";

            string sReply;
            short iResult;
            string sMsg;

            string sInmsg = "CMD=" + strCmd + "&" + strWhere;

            if (commRes.middleInvoke(servicename, sInmsg, isscrect, out sReply, out iResult, out sMsg))
            {
                if (iResult == 0)
                {
                    //��sreply���н���
                    if (sReply == null || sReply.Trim() == "")
                    {
                        dsresult = null;
                        errMsg = "ICE��ѯʧ��,�޷��ؽ��" + servicename + sInmsg;
                        return null;
                    }
                    else
                    {
                        string[] strlist1 = sReply.Split('&');

                        if (strlist1.Length == 0)
                        {
                            dsresult = null;
                            errMsg = "ICE��ѯʧ��,���ؽ������" + sReply;
                            return null;
                        }

                        ht = new Hashtable(strlist1.Length);

                        foreach (string strtmp in strlist1)
                        {
                            string[] strlist2 = strtmp.Split('=');
                            if (strlist2.Length != 2)
                            {
                                dsresult = null;
                                errMsg = "ICE��ѯʧ��,���ؽ������" + sReply;
                                return null;
                            }

                            ht.Add(strlist2[0].Trim(), strlist2[1].Trim());
                        }

                        if (!ht.Contains("result") || ht["result"].ToString().Trim() != "0")
                        {
                            dsresult = null;
                            errMsg = "ICE��ѯʧ��,���ؽ������" + sReply;
                            return null;
                        }

                        int irowcount = Int32.Parse(ht["ret_num"].ToString().Trim());

                        if (irowcount == 0)
                        {
                            dsresult = null;
                            errMsg = "��ѯ���Ϊ��";
                            return null;
                        }

                        dsresult = new DataSet();
                        DataTable dt = new DataTable();
                        dsresult.Tables.Add(dt);

                        string firstrow = ht["row_1"].ToString().Trim();

                        firstrow = URLDecode(firstrow);

                        string[] strsplit3 = firstrow.Split('&');

                        if (strsplit3.Length == 0)
                        {
                            dsresult = null;
                            errMsg = "��ѯ������޷��������ֶ�";
                            return null;
                        }


                        foreach (string stmp in strsplit3)
                        {
                            if (stmp == null || stmp.Trim() == "")
                                continue;

                            string[] fieldsplit = stmp.Split('=');

                            if (fieldsplit.Length != 2)
                                continue;

                            dt.Columns.Add(fieldsplit[0]);
                        }

                        for (int i = 1; i <= irowcount; i++)
                        {
                            string onerow = ht["row_" + i].ToString().Trim();
                            onerow = URLDecode(onerow);
                            string[] strsplit_detail = onerow.Split('&');

                            DataRow drfield = dt.NewRow();
                            drfield.BeginEdit();

                            foreach (string stmp in strsplit_detail)
                            {
                                if (stmp == null || stmp.Trim() == "")
                                    continue;

                                string[] fieldsplit = stmp.Split('=');

                                if (fieldsplit.Length != 2)
                                    continue;

                                //drfield[fieldsplit[0]] = URLDecode(fieldsplit[1].Trim());
                                drfield[fieldsplit[0]] = IceDecode(fieldsplit[1].Trim());

                                /*
                                string strtmp = fieldsplit[1].Trim();
                                byte[] srcbuff = Encoding.UTF8.GetBytes(strtmp);
                                byte[] desbuff = Encoding.Convert(Encoding.UTF8,Encoding.GetEncoding("gb2312"),srcbuff);
                                strtmp = Encoding.GetEncoding("gb2312").GetString(desbuff);

                                drfield[fieldsplit[0]] = strtmp;
                                */
                            }

                            drfield.EndEdit();
                            dt.Rows.Add(drfield);

                        }

                    }
                    return dsresult;
                }
                else
                {
                    dsresult = null;
                    errMsg = "����ͨ�ò�ѯʧ��iresult=" + iResult + "|err=" + sMsg;
                    return null;
                }
            }
            else
            {
                dsresult = null;
                errMsg = "����ͨ�ò�ѯʧ����:" + sMsg;
                return null;
            }

        }

        public static DataSet PaseRelayXml(string xml, out string errMsg)
        {
            DataSet dsresult = null;
            Hashtable ht = null;
            errMsg = "";

            if (xml != null && xml != "")
            {
                xml = CleanInvalidXmlChars(xml);
                string[] strlist1 = xml.Split('&');

                if (strlist1.Length == 0)
                {
                    errMsg = "����xmlʧ��,���ؽ������" + xml;
                    return null;
                }

                ht = new Hashtable(strlist1.Length);

                foreach (string strtmp in strlist1)
                {
                    string[] strlist2 = strtmp.Split('=');
                    if (strlist2.Length != 2)
                    {
                        continue;
                    }

                    ht.Add(strlist2[0].Trim(), strlist2[1].Trim());
                }

                if (!ht.Contains("result") || ht["result"].ToString().Trim() != "0")
                {
                    errMsg = "����xmlʧ��,���ؽ������" + xml;
                    return null;
                }

                string rec_xml = ht["rec_info"].ToString();
                if (rec_xml != null && rec_xml != "")
                {
                    dsresult = new DataSet();
                    DataTable dt = new DataTable();
                    dsresult.Tables.Add(dt);

                    //<record>
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(rec_xml);
                    XmlElement root = doc.DocumentElement;
                    XmlNodeList record_el = root.SelectNodes("record");
                    if (record_el != null)
                    {
                        for (int i = 0; i < record_el.Count; i++)
                        {
                            DataRow drfield = dt.NewRow();
                            drfield.BeginEdit();

                            XmlNodeList ch_list = record_el[i].ChildNodes;
                            foreach (XmlElement ele in ch_list)
                            {
                                string name = ele.Name.Trim();
                                if (i == 0)
                                {
                                    dt.Columns.Add(name, typeof(String));
                                }
                                drfield[name] = ele.InnerText;
                            }
                            drfield.EndEdit();
                            dt.Rows.Add(drfield);
                        }
                    }
                    
                }  
            }

            return dsresult;
        }

        //��Ʊ������ѯcgi���ؽ��xml����
        public static DataSet PaseCgiXmlForTravelPlatform(string xml, out string errMsg)
        {
            DataSet dsresult = null;
            Hashtable ht = null;
            errMsg = "";
            dsresult = new DataSet();
            DataTable dt = new DataTable();
            dsresult.Tables.Add(dt);
     
            if (xml == null || xml == "")
            {
                errMsg = "����xmlʧ��,���ؽ������" + xml;
                return null;
            }

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlElement root = doc.DocumentElement;
            XmlNode retcode = root.SelectSingleNode("retcode");
          
            if (retcode == null || retcode.InnerText != "00")//�����ɹ�
            {
                errMsg = "����xmlʧ��,���ؽ������" + xml;
                return null;
            }

            XmlNode ret_num = root.SelectSingleNode("ret_num");//�޼�¼
            if (ret_num != null && ret_num.InnerText == "0")
                dsresult = null;
            else
            {
                XmlNode records = root.SelectSingleNode("records");
                XmlNodeList record_el = records.SelectNodes("record");
                for (int i = 0; i < record_el.Count; i++)//ѭ��records
                {
                    DataRow drfield = dt.NewRow();
                    drfield.BeginEdit();

                    XmlNodeList ch_list = record_el[i].ChildNodes;
                    #region
                    foreach (XmlElement ele in ch_list)//ѭ��ÿ��record�еĽڵ�
                    {
                        string name = ele.Name.Trim();
                        XmlAttributeCollection listAttr = null;
                        if (name.Equals("listinfo"))//��ȡlistinfo�е�����
                        {
                            listAttr = ele.Attributes;
                        }
                        else if (name == "flights")//��ȡflights�е�����
                        {
                            XmlNode flightFirstN = ele.FirstChild;
                            if (flightFirstN != null)
                            {
                                listAttr = flightFirstN.Attributes;
                            }
                        }
                        else
                        {
                            continue;
                        }
                        if (i == 0)
                        {
                            for (int k = 0; k < listAttr.Count; k++)
                                dt.Columns.Add(listAttr.Item(k).Name, typeof(String));
                        }
                        for (int j = 0; j< listAttr.Count; j++)
                        {
                            drfield[listAttr.Item(j).Name] = listAttr.Item(j).Value;
                        }
                    }
                    #endregion

                    drfield.EndEdit();
                    dt.Rows.Add(drfield);
                }
            }

            //string a = dsresult.Tables[0].Rows[0]["listid"].ToString();
            //string a1 = dsresult.Tables[0].Rows[0]["uin"].ToString();
            //string a2 = dsresult.Tables[0].Rows[0]["from"].ToString();
            //string a3 = dsresult.Tables[0].Rows[0]["flight_no"].ToString();
            //string b = dsresult.Tables[0].Rows[1]["listid"].ToString();
            //string b1 = dsresult.Tables[0].Rows[1]["uin"].ToString();
            //string b2 = dsresult.Tables[0].Rows[1]["from"].ToString();
            //string b3 = dsresult.Tables[0].Rows[1]["flight_no"].ToString();
            return dsresult;
          
        }

        //�Ƶ궩����ѯcgi���ؽ��json����
        public static DataSet PaseCgiJsonForHotelOrderQuery(string json, out string errMsg)
        {
            DataSet dsresult = null;
            errMsg = "";
            dsresult = new DataSet();
            DataTable dt = new DataTable();
           
            if (json == null || json == "")
            {
                errMsg = "����jsonʧ��,���ؽ������" + json;
                return null;
            }
            try
            {
                JavaScriptSerializer js = new JavaScriptSerializer();   //ʵ����һ���ܹ����л����ݵ���
                ToHotelOrder list = js.Deserialize<ToHotelOrder>(json);    //��json����ת��Ϊ�������Ͳ���ֵ��list
                string result = list.result;
                string res_info = list.res_info;
                string order_num = list.queryorder_info.order_num;
                List<orderdetail> orderdetail = list.queryorder_info.orderdetail;
                dt = ToDataTableTow(orderdetail);
                dsresult.Tables.Add(dt);
                return dsresult;
            }
            catch
            {
                errMsg = "����jsonʧ��,���ؽ������:" + json;
                return null;
            }

        }

        #region �Ƶ궩����ѯcgi���ؽ��json֮�������
        public struct ToHotelOrder
        {
            public string result { get; set; }
            public string res_info { get; set; }
            public queryorder_info queryorder_info;
        }  public struct queryorder_info
        {
            public string order_num { get; set; }
            public List<orderdetail> orderdetail;
        };
        public struct orderdetail
        {
            public string CFTUin { get; set; }
            public string CancelDeadline { get; set; }
            public string CheckInDate { get; set; }
            public string CheckOutDate { get; set; }
            public string CityID { get; set; }
            public string CurrencyCode { get; set; }
            public string HotelID { get; set; }
            public string HotelName { get; set; }
            public string ListID { get; set; }
            public string PayAmt { get; set; }
            public string PayType { get; set; }
            public string RommsCnt { get; set; }
            public string SPTransID { get; set; }
            public string State { get; set; }
        };
        #endregion

        //���⿨��ѯcgi���ؽ��json����
        public static DataSet ParseCgiJsonForXYKQuery(string json, string flag, out string sendCount, out string receiveCount, out string errMsg)
        {
            DataSet dsresult = null;
            errMsg = "";
            sendCount = "0";
            receiveCount = "0";
            dsresult = new DataSet();
            DataTable dt = new DataTable();

            if (json == null || json == "")
            {
                errMsg = "����jsonʧ��,���ؽ������" + json;
                return null;
            }
            try
            {
                JavaScriptSerializer js = new JavaScriptSerializer();   //ʵ����һ���ܹ����л����ݵ���
                XYKResultClass list = js.Deserialize<XYKResultClass>(json);    //��json����ת��Ϊ�������Ͳ���ֵ��list


                string result = list.error.code;
                string retmsg = list.error.message;
                
                if (result == "0")
                {
                    sendCount = list.result.recordonuser.sendcount;
                    receiveCount = list.result.recordonuser.receivecount;
                    if (flag == "send")
                    {
                        //����
                        List<SendDetail> sendDetail = list.result.recordonuser.sendrecord;
                        dt = ToDataTableTow(sendDetail);
                    }
                    else 
                    {
                        //����
                        List<ReceiveDetail> receiveDetail = list.result.recordonuser.receiverecord;
                        dt = ToDataTableTow(receiveDetail);
                    }
                    dsresult.Tables.Add(dt);
                    return dsresult;
                }
                else 
                {
                    errMsg = retmsg;
                    return null;
                }
            }
            catch
            {
                errMsg = "����jsonʧ�ܣ�";
                return null;
            }

        }

        //���⿨���������ѯcgi���ؽ��json����
        public static DataSet ParseCgiJsonForXYKSendDetailQuery(string json, out string errMsg)
        {
            DataSet dsresult = null;
            errMsg = "";
            dsresult = new DataSet();
            DataTable dt = new DataTable();

            if (json == null || json == "")
            {
                errMsg = "����jsonʧ��,���ؽ������" + json;
                return null;
            }
            try
            {
                JavaScriptSerializer js = new JavaScriptSerializer();   //ʵ����һ���ܹ����л����ݵ���
                XYKDetailResultClass list = js.Deserialize<XYKDetailResultClass>(json);    //��json����ת��Ϊ�������Ͳ���ֵ��list
                string result = list.error.code;
                string retmsg = list.error.message;

                if (result == "0")
                {
                    List<SendRecordDetail> sendDetail = list.result.recordonsendid.record;
                    dt = ToDataTableTow(sendDetail);
                    dsresult.Tables.Add(dt);
                    return dsresult;
                }
                else
                {
                    errMsg = retmsg;
                    return null;
                }
            }
            catch
            {
                errMsg = "����jsonʧ�ܣ�";
                return null;
            }

        }

        #region ���⿨�������
        public struct XYKResultClass
        {
            public XYKRetcodeMsg error;
            public XYKRecordClass result;
        };
        public struct XYKRetcodeMsg
        {
            public string code { get; set; }
            public string message { get; set; }
        };

        public struct XYKRecordClass
        {
            
            public SendRecordClass recordonuser;
        };

        public struct SendRecordClass
        {
            public string sendcount { get; set; }
            public string receivecount { get; set; }
            public List<SendDetail> sendrecord;
            public List<ReceiveDetail> receiverecord;
        };
        public struct ReceiveRecordClass
        {
            public string sendcount { get; set; }
            public string receivecount { get; set; }
            public List<SendDetail> sendrecord;
            public List<ReceiveDetail> receiverecord;
        };

        public struct SendDetail
        {
            public string sendid { get; set; }
            public string transid { get; set; }
            public string name { get; set; }
            public string totalgiftnum { get; set; }
            public string totalamount { get; set; }
            public string paytime { get; set; }
            public string wishing { get; set; }
            public string receivenum { get; set; }
        };

        public struct ReceiveDetail
        {
            public string suborderid { get; set; }
            public string receivenumber { get; set; }
            public string sendnickname { get; set; }
            public string createtime { get; set; }
            public string expiretime { get; set; }
            public string recexpiretime { get; set; }
            public string giftId { get; set; }
            public string usestate { get; set; }
            public string name { get; set; }
        };

        //����
        public struct XYKDetailResultClass
        {
            public XYKRetcodeMsg error;
            public XYKSendDetailClass result;
        };
        public struct XYKSendDetailClass 
        {
            public SendDetailClass recordonsendid;
        }
        public struct SendDetailClass 
        {
            public List<SendRecordDetail> record;
        }
        public struct SendRecordDetail 
        {
            public string suborderid { get; set; }
            public string receivenickname { get; set; }
            public string receivenumber { get; set; }
            public string usednum { get; set; }
            public string createtime { get; set; }
            public string expiretime { get; set; }
            public string recexpiretime { get; set; }
            public string status { get; set; }
        }

        #endregion

        /// <summary>
        /// ��������ת����DataTable
        /// </summary>
        /// <param name="list">������</param>
        /// <returns></returns>
        public static DataTable ToDataTableTow(IList list)
        {
            DataTable result = new DataTable();
            if (list.Count > 0)
            {
                PropertyInfo[] propertys = list[0].GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    result.Columns.Add(pi.Name, pi.PropertyType);
                }

                for (int i = 0; i < list.Count; i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in propertys)
                    {
                        object obj = pi.GetValue(list[i], null);
                        tempList.Add(obj);
                    }
                    object[] array = tempList.ToArray();
                    result.LoadDataRow(array, true);
                }
            }

            return result;

        }

        /// <summary>
        /// �����ش�������dataset��
        /// </summary>
        /// <param name="str"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static DataSet ParseRelayStr(string str, out string errMsg) 
        {
            DataSet dsresult = null;
            Hashtable ht = null;
            errMsg = "";

            if (str != null && str != "") 
            {
                string[] strlist1 = str.Split('&'); //result=0&xx1=1&xx2=2

                if (strlist1.Length == 0)
                {
                    dsresult = null;
                    errMsg = "����ʧ��,���ؽ������" + str;
                    return null;
                }

                ht = new Hashtable(strlist1.Length);

                foreach (string strtmp in strlist1)
                {
                    string[] strlist2 = strtmp.Split('=');
                    if (strlist2.Length != 2)
                    {
                        continue;
                    }

                    ht.Add(strlist2[0].Trim(), strlist2[1].Trim());
                }

                if (!ht.Contains("result") || ht["result"].ToString().Trim() != "0")
                {
                    dsresult = null;
                    errMsg = "����ʧ��,���ؽ������" + str;
                    return null;
                }

                dsresult = new DataSet();
                DataTable dt = new DataTable();
                dsresult.Tables.Add(dt);

                //�����
                foreach (string stmp in strlist1)
                {
                    if (stmp == null || stmp.Trim() == "")
                        continue;

                    string[] fieldsplit = stmp.Split('=');

                    if (fieldsplit.Length != 2)
                        continue;

                    dt.Columns.Add(fieldsplit[0]);
                }

                DataRow drfield = dt.NewRow();
                drfield.BeginEdit();	

                //����xx1=1&xx2=2
                foreach (string stmp in strlist1)
                {
                    if (stmp == null || stmp.Trim() == "")
                        continue;

                    string[] fieldsplit = stmp.Split('=');

                    if (fieldsplit.Length != 2)
                        continue;

                    drfield[fieldsplit[0]] = IceDecode(fieldsplit[1].Trim());
                    
                }
                drfield.EndEdit();
                dt.Rows.Add(drfield);

            }

            return dsresult;
        }

        /// <summary>
        /// result=0&roll_1=&roll_2=...��ʽ�ַ�������
        /// </summary>
        /// <param name="str"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static DataSet ParseRelayPageRow(string str, out string errMsg)
        {
            DataSet dsresult = null;
            Hashtable ht = null;
            errMsg = "";

            if (str != null && str != "")
            {
                string[] strlist1 = str.Split('&'); //result=0&xx1=1&xx2=2

                if (strlist1.Length == 0)
                {
                    dsresult = null;
                    errMsg = "����ʧ��,���ؽ������" + str;
                    return null;
                }

                ht = new Hashtable(strlist1.Length);

                foreach (string strtmp in strlist1)
                {
                    string[] strlist2 = strtmp.Split('=');
                    if (strlist2.Length != 2)
                    {
                        continue;
                    }

                    ht.Add(strlist2[0].Trim(), strlist2[1].Trim());
                }

                if (!ht.Contains("result") || ht["result"].ToString().Trim() != "0")
                {
                    dsresult = null;
                    errMsg = "����ʧ��,���ؽ������" + str;
                    return null;
                }

                dsresult = new DataSet();
                DataTable dt = new DataTable();
                dsresult.Tables.Add(dt);

                int irowcount = Int32.Parse(ht["ret_num"].ToString().Trim());
                if (irowcount > 0)
                {
                    for (int i = 0; i < irowcount; i++)
                    {
                        string onerow = ht["roll_" + i].ToString().Trim();
                        onerow = URLDecode(onerow);
                        string[] strsplit_detail = onerow.Split('&');


                        DataRow drfield = dt.NewRow();
                        drfield.BeginEdit();

                        foreach (string stmp in strsplit_detail)
                        {
                            if (stmp == null || stmp.Trim() == "")
                                continue;

                            string[] fieldsplit = stmp.Split('=');

                            if (fieldsplit.Length != 2)
                                continue;

                            if (i == 0)
                            {
                                dt.Columns.Add(fieldsplit[0]);
                            }

                            //drfield[fieldsplit[0]] = URLDecode(fieldsplit[1].Trim());
                            drfield[fieldsplit[0]] = IceDecode(fieldsplit[1].Trim());
                        }

                        drfield.EndEdit();
                        dt.Rows.Add(drfield);
                    }
                }

            }

            return dsresult;
        }


        /// <summary>
        /// ��ʽ�ַ������� result=0&row1=&row2=&row_num=...��ʽ�ַ�������
        /// </summary>
        /// <param name="str"></param>
        /// <param name="errMsg"></param>
        /// <param name="totalNum">�ܼ�¼��</param>
        /// <returns></returns>
        public static DataSet ParseRelayPageRowNum(string str, out string errMsg,out int totalNum)
        {
            DataSet dsresult = null;
            Hashtable ht = null;
            errMsg = "";
            totalNum = 0;

            if (str != null && str != "")
            {
                string[] strlist1 = str.Split('&'); //result=0&xx1=1&xx2=2

                if (strlist1.Length == 0)
                {
                    dsresult = null;
                    errMsg = "����ʧ��,���ؽ������" + str;
                    totalNum = 0;
                    return null;
                }

                ht = new Hashtable(strlist1.Length);

                foreach (string strtmp in strlist1)
                {
                    string[] strlist2 = strtmp.Split('=');
                    if (strlist2.Length != 2)
                    {
                        continue;
                    }

                    ht.Add(strlist2[0].Trim(), strlist2[1].Trim());
                }

                if (!ht.Contains("result") || ht["result"].ToString().Trim() != "0")
                {
                    dsresult = null;
                    errMsg = "����ʧ��,���ؽ������" + str;
                    totalNum = 0;
                    return null;
                }

                dsresult = new DataSet();
                DataTable dt = new DataTable();
                dsresult.Tables.Add(dt);

                int irowcount = Int32.Parse(ht["row_num"].ToString().Trim());
                totalNum = Int32.Parse(ht["total_num"].ToString().Trim());

                if (irowcount > 0)
                {
                    for (int i = 1; i <= irowcount; i++)
                    {
                        string onerow = ht["row" + i].ToString().Trim();
                        onerow = URLDecode(onerow);
                        string[] strsplit_detail = onerow.Split('&');


                        DataRow drfield = dt.NewRow();
                        drfield.BeginEdit();

                        foreach (string stmp in strsplit_detail)
                        {
                            if (stmp == null || stmp.Trim() == "")
                                continue;

                            string[] fieldsplit = stmp.Split('=');

                            if (fieldsplit.Length != 2)
                                continue;

                            if (i == 1)
                            {
                                dt.Columns.Add(fieldsplit[0]);
                            }

                            //drfield[fieldsplit[0]] = URLDecode(fieldsplit[1].Trim());
                            drfield[fieldsplit[0]] = IceDecode(fieldsplit[1].Trim());
                        }

                        drfield.EndEdit();
                        dt.Rows.Add(drfield);
                    }
                }

            }

            return dsresult;
					
        }

        /// <summary>
        /// ��ʽ�ַ������� result=0&row0=&row1=&row_num=...��ʽ�ַ�������
        /// </summary>
        /// <param name="str"></param>
        /// <param name="errMsg"></param>
        /// <param name="totalNum">�ܼ�¼��</param>
        /// <returns></returns>
        public static DataSet ParseRelayPageRowNum0(string str, out string errMsg)
        {
            DataSet dsresult = null;
            Hashtable ht = null;
            errMsg = "";

            if (str != null && str != "")
            {
                string[] strlist1 = str.Split('&'); //result=0&xx1=1&xx2=2

                if (strlist1.Length == 0)
                {
                    dsresult = null;
                    errMsg = "����ʧ��,���ؽ������" + str;
                    return null;
                }

                ht = new Hashtable(strlist1.Length);

                foreach (string strtmp in strlist1)
                {
                    string[] strlist2 = strtmp.Split('=');
                    if (strlist2.Length != 2)
                    {
                        continue;
                    }

                    ht.Add(strlist2[0].Trim(), strlist2[1].Trim());
                }

                if (!ht.Contains("result") || ht["result"].ToString().Trim() != "0")
                {
                    dsresult = null;
                    errMsg = "����ʧ��,���ؽ������" + str;
                    return null;
                }

                dsresult = new DataSet();
                DataTable dt = new DataTable();
                dsresult.Tables.Add(dt);

                int irowcount = Int32.Parse(ht["row_num"].ToString().Trim());


                if (irowcount > 0)
                {
                    for (int i = 0; i < irowcount; i++)
                    {
                        string onerow = ht["row" + i].ToString().Trim();
                        onerow = URLDecode(onerow);
                        string[] strsplit_detail = onerow.Split('&');


                        DataRow drfield = dt.NewRow();
                        drfield.BeginEdit();

                        foreach (string stmp in strsplit_detail)
                        {
                            if (stmp == null || stmp.Trim() == "")
                                continue;

                            string[] fieldsplit = stmp.Split('=');

                            if (fieldsplit.Length != 2)
                                continue;

                            if (i == 0)
                            {
                                dt.Columns.Add(fieldsplit[0]);
                            }

                            //drfield[fieldsplit[0]] = URLDecode(fieldsplit[1].Trim());
                            drfield[fieldsplit[0]] = IceDecode(fieldsplit[1].Trim());
                        }

                        drfield.EndEdit();
                        dt.Rows.Add(drfield);
                    }
                }

            }

            return dsresult;

        }
        /// <summary> 
        /// ���xml�еĲ��Ϸ��ַ� 
        /// </summary> 
        /// <remarks> 
        /// ��Ч�ַ��� 
        /// 0x00 - 0x08 
        /// 0x0b - 0x0c 
        /// 0x0e - 0x1f 
        /// </remarks> 
        public static string CleanInvalidXmlChars(string input)
        {
            try
            {

                if (input == null || input == "")
                    return input;
                else
                {
                    StringBuilder checkedStringBuilder = new StringBuilder();
                    Char[] chars = input.ToCharArray();
                    for (int i = 0; i < chars.Length; i++)
                    {
                        int charValue = Convert.ToInt32(chars[i]);
                        if ((charValue >= 0x00 && charValue <= 0x08) || (charValue >= 0x0b && charValue <= 0x0c) || (charValue >= 0x0e && charValue <= 0x1f))
                            continue;
                        else
                            checkedStringBuilder.Append(chars[i]);

                    }
                    return checkedStringBuilder.ToString();
                }
            }
            catch (Exception ex)
            {

                log4net.ILog log = log4net.LogManager.GetLogger("CleanInvalidXmlChars");
                if (log.IsErrorEnabled)
                    log.Error(ex.Message);
                return "";
            }

        }


		//��������ͨ��query_mul_tcrecv_service����ѯ andrew20120224
		public static DataSet GetDataSetFromICE_QueryServer(string strWhere, string strCmd, out string errMsg)
		{
            return GetDataSetFromICE(strWhere, strCmd, false, "cq_query_tcbanklist_service", out errMsg);
		}
		public static string GetOneResultFromICE(string strWhere, string strCmd,string fieldstr, out string errMsg)
		{
			DataTable dtResult = GetTableFromICE(strWhere,strCmd,out errMsg);		

			if(dtResult == null)
				return null;
			else
			{
				if(dtResult.Rows.Count == 0)
					return null;

				if( dtResult.Rows[0][fieldstr] == null)
					return null;
				else
					return dtResult.Rows[0][fieldstr].ToString();
			}
		}

		public static string[] GetdrDataFromICE(string strWhere, string strCmd,string[] fieldstrs, out string errMsg)
		{
			string[] results = new string[fieldstrs.Length];

			DataTable dtResult = GetTableFromICE(strWhere,strCmd,out errMsg);		

			if(dtResult == null)
				return null;
			else
			{
				if(dtResult.Rows.Count == 0)
					return null;

				for(int i=0;i < fieldstrs.Length;i++)
				{
					string str = fieldstrs[i];
					if( dtResult.Rows[0][str] == null)
					{
						results[i] = "";
					}
					else
					{
						results[i] = dtResult.Rows[0][str].ToString();						
					}
					//fieldstrs[i] = results[i];
				}
			}

			return results;
		}

		public static DataTable GetTableFromICE(string strWhere, string strCmd, out string errMsg)
		{
			DataSet dsresult = GetDataSetFromICE(strWhere,strCmd,out errMsg);		

			if(dsresult == null)
				return null;
			else
			{
				if(dsresult.Tables.Count == 0)
					return null;

				return dsresult.Tables[0];
			}
		}

		public static bool GetDataFromICE(string strWhere, string strCmd, out string errMsg, out DataSet dsresult)
		{
			dsresult = GetDataSetFromICE(strWhere,strCmd,out errMsg);		

			return true;
		}




		// ��������ͨ��query_order_service����ѯ andrew20120524
		// �÷���Ŀǰ���ڲ�sp_order_query_service  alex 20120618
		public static DataSet GetDataSetFromICE_OrderServer(string strWhere, string strCmd, bool issecret, out string errMsg)
		{
			DataSet dsresult = null;
			Hashtable ht = null;

			errMsg = "";

			string	sReply;
			short	iResult;
			string sMsg;

			//			if(strWhere.IndexOf("&")<0)
			//			{
			//				strWhere="&"+strWhere;
			//			}

			//string sInmsg = "CMD=" + strCmd + "&" + strWhere;


			if(commRes.middleInvoke(strCmd,strWhere,issecret,out sReply,out iResult,out sMsg))
			{
				if(iResult == 0)
				{
					//��sreply���н���
					if(sReply == null || sReply.Trim() == "")
					{
						dsresult = null;
						errMsg = "����"+strCmd+"ʧ��,�޷��ؽ��" + strCmd + strWhere ;
						return null;
					}
					else
					{
						string[] strlist1 = sReply.Split('&');

						if(strlist1.Length == 0)
						{
							dsresult = null;
							errMsg = "����"+strCmd+"ʧ��,���ؽ������" + sReply ;
							return null;
						}

						ht = new Hashtable(strlist1.Length);
						string[] strList_Keys = new string[strlist1.Length];
						string[] strList_Values = new string[strlist1.Length];
						int index = 0;

						foreach(string strtmp in strlist1)
						{
							string[] strlist2 = strtmp.Split('=');
							if(strlist2.Length != 2)
							{
								dsresult = null;
								errMsg = "����"+strCmd+"ʧ��,���ؽ������" + sReply ;
								return null;
							}
                            
							ht.Add(strlist2[0].Trim(),strlist2[1].Trim());
							strList_Keys[index] = strlist2[0].Trim(); 
							strList_Values[index++] = strlist2[1].Trim();
						}

						if(!ht.Contains("result") || ht["result"].ToString().Trim() != "0") //|| !ht.Contains("row_num") )
						{
							dsresult = null;
							errMsg = "����"+strCmd+"ʧ��,���ؽ������" + sReply ;
							return null;
						}

						// û��row_num������ʾһ��
						if(!ht.Contains("row_num"))
						{
							dsresult = new DataSet();
							DataTable dt_oneRow = new DataTable();
							dsresult.Tables.Add(dt_oneRow);

							for(int i=0;i<index;i++)
							{
								dt_oneRow.Columns.Add(strList_Keys[i]);
							}

							DataRow dr = dt_oneRow.NewRow();

							dr.BeginEdit();
							for(int i=0;i<index;i++)
							{
								dr[i] = strList_Values[i];
							}
							dr.EndEdit();

							dt_oneRow.Rows.Add(dr);

							return dsresult;
						}

						int irowcount = Int32.Parse(ht["row_num"].ToString().Trim());

						if(irowcount == 0)
						{
							dsresult = null;
							errMsg = "��ѯ���Ϊ��";
							return null;
						}

						dsresult = new DataSet();
						DataTable dt = new DataTable();
						dsresult.Tables.Add(dt);

						string firstrow = ht["row1"].ToString().Trim();

						firstrow = URLDecode(firstrow);

						string[] strsplit3 = firstrow.Split('&');

						if(strsplit3.Length == 0)
						{
							dsresult = null;
							errMsg = "��ѯ������޷��������ֶ�";
							return null;
						}
						

						foreach(string stmp in strsplit3)
						{
							if(stmp == null || stmp.Trim() == "")
								continue;

							string[] fieldsplit = stmp.Split('=');
						
							if(fieldsplit.Length != 2)
								continue;

							dt.Columns.Add(fieldsplit[0]);
						}

						for(int i=1; i<=irowcount ;i++ )
						{
							string onerow = ht["row" + i].ToString().Trim();
							onerow = URLDecode(onerow);
							string[] strsplit_detail = onerow.Split('&');

							DataRow drfield = dt.NewRow();
							drfield.BeginEdit();							

							foreach(string stmp in strsplit_detail)
							{
								if(stmp == null || stmp.Trim() == "")
									continue;

								string[] fieldsplit = stmp.Split('=');
						
								if(fieldsplit.Length != 2)
									continue;

								//drfield[fieldsplit[0]] = URLDecode(fieldsplit[1].Trim());
								drfield[fieldsplit[0]] = IceDecode(fieldsplit[1].Trim());

								/*
									 string strtmp = fieldsplit[1].Trim();
									 byte[] srcbuff = Encoding.UTF8.GetBytes(strtmp);
									 byte[] desbuff = Encoding.Convert(Encoding.UTF8,Encoding.GetEncoding("gb2312"),srcbuff);
									 strtmp = Encoding.GetEncoding("gb2312").GetString(desbuff);

									 drfield[fieldsplit[0]] = strtmp;
									 */
							}

							drfield.EndEdit();
							dt.Rows.Add(drfield);

						}

					}
					return dsresult;
				}
				else
				{
					dsresult = null;
					errMsg = "����"+strCmd+"ʧ��iresult=" + iResult + "|err="  + sMsg;
					return null;
				}
			}
			else
			{
				dsresult = null;
				errMsg = "����"+strCmd+"ʧ����:" + sMsg;
				return null;
			}

		}

	}
	
}
