using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Reflection;
using TENCENT.OSS.CFT.KF.DataAccess;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;
using TENCENT.OSS.C2C.Finance.Common.CommLib;
using TENCENT.OSS.C2C.Finance.BankLib;
using CFT.CSOMS.BLL.TradeModule;
using CFT.CSOMS.BLL.DKModule;
namespace TENCENT.OSS.CFT.KF.KF_Web.classLibrary
{
	/// <summary>
	/// setConfig ��ժҪ˵����
	/// </summary>
	public class setConfig
	{
		public setConfig()
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
		}


		public static string replaceMStr(string str)  //�Բ������ݿ���ַ����������ַ������滻,���÷Ƿ�sqlע�����
		{
			if(str == null) return null; //furion 20050819

			str = str.Replace("'","��");  
			str = str.Replace("\"","��");
			str = str.Replace("script","�������");
			str = str.Replace("<","��");
			str = str.Replace(">","��");
			str = str.Replace("--","����");
			str = str.Replace("\r","");
			str = str.Replace("\n","");
			return str;
		}
		
	
		public static Query_Service.Finance_Header setFH(string uid,string ip)  //����soapͷ��Ϣ
		{
			Query_Service.Finance_Header fh = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Finance_Header();
			fh.UserIP = ip;
			fh.UserName = uid;

			return fh;
		}


		public static Query_Service.Finance_Header setFH(TemplateControl page)  //����soapͷ��Ϣ
		{
            if (page.Page.Session["uid"] == null)
            {
                throw new ArgumentNullException("Session[uid]", " ��¼״̬���ڣ������µ�¼��");
            }
			Query_Service.Finance_Header fh = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Finance_Header();

			fh.SrcUrl = page.Page.Request.Url.ToString();
			fh.UserIP = page.Page.Request.UserHostAddress;
			fh.UserName = page.Page.Session["uid"].ToString();
			fh.SessionID = page.Page.Session.SessionID;
			fh.SzKey = page.Page.Session["SzKey"].ToString();
			fh.OperID = Int32.Parse(page.Page.Session["OperID"].ToString());
			fh.RightString = page.Page.Session["SzKey"].ToString();
			
			return fh;
		}

        public static ZWBatchPay_Service.Finance_Header ZWSetFH(TemplateControl page)  //����soapͷ��Ϣ
        {
            if (page.Page.Session["uid"] == null)
            {
                throw new ArgumentNullException("Session[uid]", " ��¼״̬���ڣ������µ�¼��");
            }
            ZWBatchPay_Service.Finance_Header fh = new TENCENT.OSS.CFT.KF.KF_Web.ZWBatchPay_Service.Finance_Header();

            fh.UserIP = page.Page.Request.UserHostAddress;
            fh.UserName = page.Page.Session["uid"].ToString();
            fh.SzKey = page.Page.Session["SzKey"].ToString();
            fh.OperID = Int32.Parse(page.Page.Session["OperID"].ToString());
            fh.RightString = page.Page.Session["SzKey"].ToString();

            return fh;
        }

		public static FQuery_Service.Finance_Header FsetFH(TemplateControl page)  //����soapͷ��Ϣ
		{
            if (page.Page.Session["uid"] == null)
            {
                throw new ArgumentNullException("Session[uid]", " ��¼״̬���ڣ������µ�¼��");
            }

			FQuery_Service.Finance_Header Ffh = new TENCENT.OSS.CFT.KF.KF_Web.FQuery_Service.Finance_Header();

			Ffh.UserIP = page.Page.Request.UserHostAddress;
			Ffh.UserName = page.Page.Session["uid"].ToString();
			Ffh.SzKey = page.Page.Session["SzKey"].ToString();
			Ffh.OperID = Int32.Parse(page.Page.Session["OperID"].ToString());
			Ffh.RightString = page.Page.Session["SzKey"].ToString();
			
			return Ffh;
		}
        public static FINANCE_RefundSERVICE.Finance_Header FsetRefundFH(TemplateControl page)  //����soapͷ��Ϣ
        {
            if (page.Page.Session["uid"] == null)
            {
                throw new ArgumentNullException("Session[uid]", " ��¼״̬���ڣ������µ�¼��");
            }

            FINANCE_RefundSERVICE.Finance_Header Ffh = new FINANCE_RefundSERVICE.Finance_Header();

            Ffh.UserIP = page.Page.Request.UserHostAddress;
            Ffh.UserName = page.Page.Session["uid"].ToString();
            Ffh.SzKey = page.Page.Session["SzKey"].ToString();
            Ffh.OperID = Int32.Parse(page.Page.Session["OperID"].ToString());
            Ffh.RightString = page.Page.Session["SzKey"].ToString();

            return Ffh; 
        }

		public static Query_Service.Finance_Header setFH(string uid,string ip,string sessionID,string url)
		{
			Query_Service.Finance_Header fh = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Finance_Header();

			fh.SrcUrl = url;
			fh.UserIP = ip;
			fh.UserName = uid;
			fh.SessionID = sessionID;
			
			return fh;
		}


		public static Finance_ManageService.Finance_Header setFH_Finance(TemplateControl page)
        {
            if (page.Page.Session["uid"] == null)
            {
                throw new ArgumentNullException("Session[uid]", " ��¼״̬���ڣ������µ�¼��");
            }

			Finance_ManageService.Finance_Header fh = new Finance_ManageService.Finance_Header();

			fh.SrcUrl = page.Page.Request.Url.ToString();
			fh.UserIP = page.Page.Request.UserHostAddress;
			fh.UserName = page.Page.Session["uid"].ToString();
			fh.SessionID = page.Page.Session.SessionID;
			fh.SzKey = page.Page.Session["SzKey"].ToString();
			fh.OperID = Int32.Parse(page.Page.Session["OperID"].ToString());
			fh.RightString = page.Page.Session["SzKey"].ToString();

			return fh; 
		}

	

		public static Finance_ManageService.Finance_Header setFH_Finance(string uid,string ip)  //����soapͷ��Ϣ
		{
			Finance_ManageService.Finance_Header fh = new TENCENT.OSS.CFT.KF.KF_Web.Finance_ManageService.Finance_Header();
			fh.UserIP = ip;
			fh.UserName = uid;

			return fh;
		}



		public static Finance_ManageService.Finance_Header setFH_Finance(string uid,string ip,string sessionID,string url)
		{
			Finance_ManageService.Finance_Header fh = new TENCENT.OSS.CFT.KF.KF_Web.Finance_ManageService.Finance_Header();

			fh.SrcUrl = url;
			fh.UserIP = ip;
			fh.UserName = uid;
			fh.SessionID = sessionID;
			
			return fh;
		}



		public static Check_WebService.Finance_Header setFH_CheckService(TemplateControl page)
        {
            if (page.Page.Session["uid"] == null)
            {
                throw new ArgumentNullException("Session[uid]", " ��¼״̬���ڣ������µ�¼��");
            }

			Check_WebService.Finance_Header fh = new Check_WebService.Finance_Header();

			fh.SrcUrl = page.Page.Request.Url.ToString();
			fh.SessionID = page.Page.Session.SessionID;

			fh.UserIP = page.Page.Request.UserHostAddress;
			fh.UserName = page.Page.Session["uid"].ToString();
			fh.SzKey = page.Page.Session["SzKey"].ToString();
			fh.OperID = Int32.Parse(page.Page.Session["OperID"].ToString());
			fh.RightString = page.Page.Session["SzKey"].ToString();
			fh.UserPassword = "";

			return fh;
		}


		public static DateTime SetTime(string tmpTime)  //ʱ��ת��
		{
			DateTime dt = new DateTime(int.Parse(tmpTime.Substring(0,4)),int.Parse(tmpTime.Substring(4,2)),int.Parse(tmpTime.Substring(6,2)),	int.Parse(tmpTime.Substring(8,2)),int.Parse(tmpTime.Substring(10,2)),int.Parse(tmpTime.Substring(12,2)));
			return dt;
		}


		public static string exceptionMessage(string msg)  //�쳣��Ϣ����
		{
			string msgStr;

			if (msg == "�������޷��������� --> ���ݿ����޼�¼��")
			{
				msgStr = "<font color = red>���ݿ����޼�¼������ʱ�䣺" + DateTime.Now.ToString() + "</font>";
				return msgStr;
			}
			else
			{
				msgStr = "<font>�������������쳣������ʱ�䣺" + DateTime.Now.ToString() + "</font>";
				return msgStr;
			}
		}

		public static void rnQQIDFromListID(string uid,string ip)
		{
			TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service myService = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
			myService.Finance_HeaderValue = classLibrary.setConfig.setFH("nouse","127.0.0.1");    
		}

        public static DataSet returnDataSet(string selectStr, int fcurtype, DateTime beginTime, DateTime endTime, int i, string type, int istr, int imax, string uid, string ip)
		{
            return returnDataSet(selectStr, fcurtype, beginTime, endTime, i, type, istr, imax, uid, ip, false);
		}
        public static DataSet returnDataSet(string selectStr, int fcurtype, DateTime beginTime, DateTime endTime, int i, string type, int istr, int imax, string uid, string ip, bool isHistory)
		{	
			// ��Ϊ��Щ��ѯ������ҪCheckRight�����Կ���ʹ�þɵ�setFH
			Query_Service.Query_Service myService = new Query_Service.Query_Service();
			myService.Finance_HeaderValue = classLibrary.setConfig.setFH(uid,ip);     
            
			DataSet ds = new DataSet();

			if (type == "TradeLog") //���׼�¼
			{
				ds = myService.GetUserPayList(selectStr,i,beginTime,endTime,istr,imax);
			}
			else if(type == "GetPayList")  //�û����׵�
			{
				ds = myService.GetPayList(selectStr,i,beginTime,endTime,istr,imax);
                log4net.LogManager.GetLogger(string.Format("��¼���׵���־��selectStr = {0} ,i = {1},beginTime = {2},endTime = {3},istr = {4},imax = {5}", selectStr, i, beginTime, endTime, istr, imax));//��һ�����⣬�¸��汾ɾ������־��darrenran
			}
			else if( type == "Bankroll")  //�ʽ���ˮ��¼
			{
                ds = myService.GetBankRollList(selectStr, fcurtype, beginTime, endTime, istr, imax);
			}
			else if (type == "PayList")  //�����¼
			{
				//ds = myService.GetTCBankPAYList(selectStr,i,beginTime,endTime,istr,imax);
                DateTime beginTime_Temp = beginTime;
                DataSet ds_Temp = new DataSet();
                while (beginTime_Temp < endTime)
                {
                    ds_Temp = new PickService().GetTCBankPAYList(selectStr, i, beginTime_Temp, endTime, istr, imax);//2424,2425
                    ds = PublicRes.ToOneDataset(ds, ds_Temp);
                    beginTime_Temp = beginTime_Temp.AddMonths(1);
                    beginTime_Temp = DateTime.Parse(beginTime_Temp.Year + "-" + beginTime_Temp.Month + "-01");
                }

			}
			else if (type  == "Gather")  // �տ��¼
			{
				ds = myService.GetTCBankRollList(selectStr,i,beginTime,endTime,false, istr,imax);
			}
			else if (type == "Refund")  //�˿
			{
				//ds = myService.GetRefund(selectStr,0,beginTime,endTime,istr,imax);  //0 ����QQID�����˿
				ds = myService.GetRefund(selectStr,i,beginTime,endTime,istr,imax);  //0 ����QQID�����˿
			}
			else if (type == "ButtonInfo")  //�̼ҹ��߲˵�
			{
				ds = myService.GetUserButtonInfo(selectStr,istr,imax);  //0 ����QQID�����˿
			}
			return ds;
		}

		public static string GetStringStr(object obj)
		{
			if(obj == null)
				return "";
			else
				return obj.ToString();
		}
		
		//�����������
		public static string FenToYuan(string fen)
		{
			//rayguo 06.04.16 ֧�ִ���ķ�Ԫת��
			bool strong = false;
			if (fen.IndexOf("<B>") !=-1 || fen.IndexOf("<b>") !=-1)
			{
				strong = true;
				fen    = fen.Replace("<B>","").Replace("</B>","").Replace("<b>","").Replace("/b","");
			}

			//furion 20051012 ��Ԫת��ר����
			string s = MoneyTransfer.FenToYuan(fen) + "Ԫ";

			if (strong == true)
			{
				s = "<font color =blue><B>" + s + "</B></font>";
			}

			return s;
		}



		public static string FenToYuan(double fen)
		{
			return MoneyTransfer.FenToYuan(fen) + "Ԫ";
		}

        public static int YuanToFen(double doublenum) 
        {
            return MoneyTransfer.YuanToFen(doublenum);
        }


		/// <summary>
		/// ��һ�����ݱ��е�ĳ���ֶδӷ�ת����Ԫ��furion 20050820
		/// </summary>
		/// <param name="dt">Ҫת���ı�</param>
		/// <param name="FieldName">Ҫת�����ֶ�</param>
		public static void FenToYuan_Table(DataTable dt, string FieldName, string destField)
		{
			try
			{			
				foreach(DataRow dr in dt.Rows)
				{
					string fen = dr[FieldName].ToString();
					string yuan = FenToYuan(fen).Replace("Ԫ","");
					dr.BeginEdit();
					dr[destField] = yuan;
					dr.EndEdit();
				}
			}
			catch(Exception)
			{
				return;
			}
		}

        /// <summary>
        /// ��һ�����ݱ��е�ĳ���ֶδӷ�ת����Ԫ��furion 20050820
        /// </summary>
        /// <param name="dt">Ҫת���ı�</param>
        /// <param name="FieldName">Ҫת�����ֶ�</param>
        public static void RateToYuan_Table(DataTable dt, string FieldName, string destField)
        {
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    string strfen = dr[FieldName].ToString();
                    double yuan = (double)(Int64.Parse(strfen)) /100000000; ;
                    dr.BeginEdit();
                    dr[destField] = yuan;
                    dr.EndEdit();
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        /// <summary>
        /// ͨ�����ݿ��ֶ�����ת��ҳ����ʾ���ݡ�furion 20130517
        /// </summary>
        /// <param name="dt">Ҫת���ı�</param>
        /// <param name="FieldName">Ҫת�����ֶ�</param>
        public static void DbtypeToPageContent(DataTable dt, string FieldName, string destField, Hashtable ht) 
        {
            try
            {
                if (ht == null)
                    return;
                foreach (DataRow dr in dt.Rows)
                {
                    string field = dr[FieldName].ToString();
                    string ret = "";
                    if (ht.ContainsKey(field))
                    {
                        ret = ht[field].ToString();
                    }
                    else 
                    {
                        ret = "δ֪��"+field;
                    }
                    
                    dr.BeginEdit();
                    dr[destField] = ret;
                    dr.EndEdit();
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        /// <summary>
        /// Ϊ������Ĭ��ֵ
        /// </summary>
        /// <param name="dt">Ҫת���ı�</param>
        /// <param name="FieldName">Ҫת�����ֶ�</param>
        public static void setDefaultValue(DataTable dt, string FieldName, string defaultValue)
        {
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if (defaultValue != null) {
                        dr.BeginEdit();
                        dr[FieldName] = defaultValue.Trim();
                        dr.EndEdit();
                    }
                }
            }
            catch (Exception)
            {
                return;
            }
        }


		//��ѯ�����ֵ䷽��
		public static void bindDic(string type,DropDownList ddl)
		{
			string [] ar = new string[2];
			Hashtable ht = new Hashtable();
			ar[0] = "Fmemo";
			ar[1] = "Fvalue";

			string Msg = "";
			DataSet ds =  QueryDicInfoByType(type,out Msg);

			if(ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				ddl.Items.Clear();

				foreach(DataRow dr in ds.Tables[0].Rows)
				{
					ddl.Items.Add(new ListItem(dr["Fmemo"].ToString(),dr["Fvalue"].ToString()));
				}
			}

            if (type == "CRE_TYPE")
                ddl.Items.Add(new ListItem("δָ��", "0"));

			ddl.DataBind();
		}

		//ͨ�����Ͳ�ѯ�����ֶα���Ϣ
		public static DataSet QueryDicInfoByType(string type,out string Msg)
		{
			Msg="";
			try
			{
				//�Ȳ�ѯ���ܱ���
				string icesql = "type=" + type;
				string count = CommQuery.GetOneResultFromICE(icesql,CommQuery.QUERY_DIC_COUNT,"acount",out Msg);
				if(count==null||count==""||count=="0")
				{
					return null;
				}
				int allCount=Convert.ToInt32(count);
				if(allCount<=0)
				{
					return null;
				}

				
				DataTable dt_all=new DataTable();
				dt_all.Columns.Add("Fno",System.Type.GetType("System.String")); 
				dt_all.Columns.Add("FType",System.Type.GetType("System.String")); 
				dt_all.Columns.Add("Fvalue",System.Type.GetType("System.String")); 
				dt_all.Columns.Add("Fmemo",System.Type.GetType("System.String")); 
				dt_all.Columns.Add("Fsymbol",System.Type.GetType("System.String")); 
				
				string strSqlTmp = "type=" + type;	
				int limitStart=0;
				int onceCount=20;//һ�η��ر���

				while(allCount>limitStart)
				{
					string strSqlLimit = "&strlimit=limit "+limitStart+","+onceCount;
					string strSql=strSqlTmp+strSqlLimit;
					DataTable dt_one = CommQuery.GetTableFromICE(strSql,CommQuery.QUERY_DIC,out Msg);
					if(dt_one!=null&&dt_one.Rows.Count>0)
					{
						foreach(DataRow dr2 in dt_one.Rows)
						{
							string fno=dr2["Fno"].ToString();
							string FType=dr2["FType"].ToString();
							string Fvalue=dr2["Fvalue"].ToString();
							string Fmemo=dr2["Fmemo"].ToString();
							string symbol=dr2["Fsymbol"].ToString();

							DataRow drNew =dt_all.NewRow();
							drNew["Fno"]=fno;
							drNew["FType"]=FType;
							drNew["Fvalue"]=Fvalue;
							drNew["Fmemo"]=Fmemo;
							drNew["Fsymbol"]=symbol;
							dt_all.Rows.Add(drNew);
						}
						
					}

					limitStart=limitStart+onceCount;
				}

				int num = dt_all.Rows.Count;
				DataSet ds=new DataSet();
				ds.Tables.Add(dt_all);
				return ds;
			}
			catch
			{
				return null;
			}
		}

		public static string GetDicValue(string atype, string avalue)
		{
			string Msg;
			string strSql = "type=" + atype + "&value=" + avalue;	
			string temp = CommQuery.GetOneResultFromICE(strSql,CommQuery.QUERY_DIC,"Fmemo",out Msg);

			return temp;
		}

		//��ȡ�����ֵ�
		public static void queryDic(string type)
		{
			string Msg;
			DataSet ds =  QueryDicInfoByType(type,out Msg);

			if (ds == null) //�����ȡ�����ֵ�ʧ��
			{
				throw new Exception(Msg);
			}

			Hashtable myht = new Hashtable();
			try
			{
				foreach(DataRow dr in ds.Tables[0].Rows)
				{
					myht.Add(dr["Fvalue"].ToString(),dr["Fmemo"].ToString());
				}
			}
			catch (Exception e)
			{
				throw e;
			}

			//�������ֵ�
			HttpContext.Current.Application[type] = myht;
		}

		public static string returnDicStr(string type,string sType)
		{
			try
			{
				if (sType == "")  //����գ��򷵻ؿ�
				{
					return "";
				}
				else
				{
					Hashtable ht = new Hashtable();

					if (HttpContext.Current.Application[type] == null)
						queryDic(type);
					ht = (Hashtable)HttpContext.Current.Application[type];
			
					string memo = ht[sType].ToString();
					return memo;
				}
			}
			catch  //û�д������ֵ��ж���memo
			{
				return "";
			} 
		}

		public static string accountState(string sType)  //�˻�״̬ת��  USER_STATE
		{  
			return returnDicStr("USER_STATE",sType);
		}

		public static string convertFuser_type(string sType) //�˻�����ת��  USER_TYPE
		{
			return returnDicStr("USER_TYPE",sType);
		}

		public static string convertMoney_type(string sType) //��������ת�� CUR_TYPE
		{
			return returnDicStr("CUR_TYPE",sType);
		}

		public static string convertbankType(string sType)  //��������ת�� BANK_TYPE
		{
			if(sType == "9999")
				return "��������";

			return returnDicStr("BANK_TYPE",sType);
		}

		public static string convertTradeState(string sType)  //���׵�״̬ RLIST_STATE
		{
			return returnDicStr("RLIST_STATE",sType);
		}

		public static string convertSex(string sType)  //�û��Ա�
		{
			return returnDicStr("SEX",sType);
		}

		public static string convertTradeListState(string sType)  //���׵�״̬ PAY_STATE
		{
			return returnDicStr("PAY_STATE",sType);
		}

		public static string convertSubject(string sType)  //���,��Ŀ  BG_SUBJECT
		{
			return returnDicStr("BG_SUBJECT",sType);
		}

		public static string convertActionType(string sType)  //��������  �ڲ�֮��������ϵ 
		{
			return returnDicStr("ACTION_TYPE",sType);
		}
		 
		public static string convertTradeType(string sType)  //�뻹�ǳ� 
		{
			return returnDicStr("BG_TYPE",sType);
		}

		public static string convertPayType(string sType)  //������� c2c,b2c ,ת��
		{
			return returnDicStr("PAYLIST_TYPE",sType);
		}

		//	    public static string convertAdjustFlag(string )  //�������׻���ת�ʱ�־
		
		public static string convertCurrentState(string sType)  //��ǰ״̬ TCPAY_STATE
		{
			return returnDicStr("TCPAY_STATE",sType);
		}

		public static string convertTradeSign(string sType)  //���ױ�� 1-�ɹ� 2-ʧ�� TCLIST_SIGN
		{
			return returnDicStr("TCLIST_SIGN",sType);
		}

		//
		public static string convertTCSubject(string sType)  //��� ��Ŀ TCLIST_SUBJECT
		{
			return returnDicStr("TCLIST_SUBJECT",sType);
		}

		public static string convertCheckState(string sType)  //���������ִ��״̬ TASK_STATUS
		{
			return returnDicStr("TASK_STATUS",sType);
		}


		public static string convertCheckType(string sType)  //������������� SUB_TASK_NO
		{
			return returnDicStr("SUB_TASK_NO",sType);
		}

		public static string cRefundState(string sType)  //�˿�״̬ REFUND_STATE
		{
			return returnDicStr("REFUND_STATE",sType);
		}

		public static string cRlistState(string sType)  //�˿״̬ RLIST_STATE
		{
			return returnDicStr("RLIST_STATE",sType);
		}

		public static string cPay_type(string sType)  //֧������ PAY_TYPE
		{
			return returnDicStr("PAY_TYPE",sType);
		}

		public static string convertTCfSubject(string sType)  //�������� TC_PLIST_SUBJECT
		{
			return returnDicStr("TC_PLIST_SUBJECT",sType);
		}

		public static string convertTCState(string sType)  //�������� TCLIST_State
		{
			return returnDicStr("TCLIST_State",sType);
		}

		public static string convertInnerCkType(string sType)  //�ڲ����ʵ����� 
		{
			return returnDicStr("SUB_TASK_NO1",sType);   
		} 

		public static string convertAdjustSign(string sType)  //���ʱ�� 
		{
			return returnDicStr("ADJUST_FLAG",sType);   
		} 

		public static string convertBPAY(string sType)  //���֧��״̬ 1 ���� 2 �ر� 
		{
			return returnDicStr("FBPAY_STATE",sType);   
		} 

		

		//furion 20050804 ȡ��ָ�����͵����м���ֵ��
		public static Hashtable GetAllValueByType(string sType)
		{
			Hashtable ht = new Hashtable();

			try
			{
				if (HttpContext.Current.Application[sType] == null)
					queryDic(sType);

				ht = (Hashtable)HttpContext.Current.Application[sType];
			
				return ht;
			}
			catch
			{
				return null;
			}
		}

		//furion 20050804  ��ȡ�����е��������в��ŵ�����ѡ�����
		//�����������Ҫ����չΪ��ȡ����ָ�����͵ļ�ֵ����������ѡ����У�
		public static void GetAllBankList(System.Web.UI.WebControls.DropDownList ddl)
		{
			//GetAllTypeList(ddl,"BANK_TYPE");

			ddl.Items.Clear();
			Query_Service.Query_Service myService = new Query_Service.Query_Service();
			Query_Service.Finance_Header fh = setFH(HttpContext.Current.Session["uid"].ToString(),HttpContext.Current.Request.UserHostAddress);
			
			myService.Finance_HeaderValue = fh;

			// �����������ҪCheckRight�����Կ���ʹ�þɵ�setFH
			System.Data.DataSet ds = myService.GetBankByType("ALL","");
            
			if(ds != null && ds.Tables.Count>0 && ds.Tables[0].Rows.Count>0)
			{
				foreach(DataRow dr in ds.Tables[0].Rows)
				{
					ListItem li = new ListItem(dr["Fbank_name"].ToString(),dr["Fbank_type"].ToString());
					ddl.Items.Add(li);
				}
			}
		}
        //v_yqyqguo 20151223  ����֧�ֵ�����
        public static void GetAllBankList_DK(System.Web.UI.WebControls.DropDownList ddl)
        {
            ddl.Items.Clear();
            DataTable dtDKBankList = new DKService().GetDKBankList();
            classLibrary.setConfig.GetColumnValueFromDic(dtDKBankList, "Fbank_type", "Fbank_name", "BANK_TYPE");
            ddl.DataTextField = "Fbank_name";
            ddl.DataValueField = "Fbank_sname";
            ddl.DataSource = dtDKBankList;
            ddl.DataBind();
        }
         
        public static void GetActivityList(System.Web.UI.WebControls.DropDownList ddl)
        {
            if (ddl == null) return;
            ddl.Items.Clear();
            ddl.Items.Add(new ListItem("ȫ��", ""));
            Query_Service.Query_Service myService = new Query_Service.Query_Service();
            System.Data.DataSet ds = myService.QueryActivityList("");
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0) 
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    ListItem li = new ListItem(dr["FActName"].ToString(), dr["FActID"].ToString());
                    ddl.Items.Add(li);
                }
            }
        }


		public static char FirstLetter(int nCode)
		{
			char strLetter = '0';
			if(nCode >= 1601 && nCode < 1637) strLetter = 'A';
			if(nCode >= 1637 && nCode < 1833) strLetter = 'B';
			if(nCode >= 1833 && nCode < 2078) strLetter = 'C';
			if(nCode >= 2078 && nCode < 2274) strLetter = 'D';
			if(nCode >= 2274 && nCode < 2302) strLetter = 'E';
			if(nCode >= 2302 && nCode < 2433) strLetter = 'F';
			if(nCode >= 2433 && nCode < 2594) strLetter = 'G';
			if(nCode >= 2594 && nCode < 2787) strLetter = 'H';
			if(nCode >= 2787 && nCode < 3106) strLetter = 'J';
			if(nCode >= 3106 && nCode < 3212) strLetter = 'K';
			if(nCode >= 3212 && nCode < 3472) strLetter = 'L';
			if(nCode >= 3472 && nCode < 3635) strLetter = 'M';
			if(nCode >= 3635 && nCode < 3722) strLetter = 'N';
			if(nCode >= 3722 && nCode < 3730) strLetter = 'O';
			if(nCode >= 3730 && nCode < 3858) strLetter = 'P';
			if(nCode >= 3858 && nCode < 4027) strLetter = 'Q';
			if(nCode >= 4027 && nCode < 4086) strLetter = 'R';
			if(nCode >= 4086 && nCode < 4390) strLetter = 'S';
			if(nCode >= 4390 && nCode < 4558) strLetter = 'T';
			if(nCode >= 4558 && nCode < 4684) strLetter = 'W';
			if(nCode >= 4684 && nCode < 4925) strLetter = 'X';
			if(nCode >= 4925 && nCode < 5249) strLetter = 'Y';
			if(nCode >= 5249 && nCode < 5590) strLetter = 'Z';

			return strLetter;
		}


		public class StrBankListComparer : System.Collections.IComparer	
		{
			public int Compare(object obj1,object obj2)
			{
				string str1 = obj1.ToString();
				string str2 = obj2.ToString();

				if(str1.Length > 2 && str2.Length > 2)
				{
					byte[] buff1 = System.Text.Encoding.Default.GetBytes(str1.ToCharArray(),0,2);
					byte[] buff2 = System.Text.Encoding.Default.GetBytes(str2.ToCharArray(),0,2);

					int nCode1 = 0,nCode2 = 0;

					nCode1 = (buff1[0] - 0xa0) * 100 + buff1[1] - 0xa0;
					nCode2 = (buff2[0] - 0xa0) * 100 + buff2[1] - 0xa0;

					return FirstLetter(nCode1).CompareTo(FirstLetter(nCode2));
				}

				return 1;
			}
		}


		public static void GetAllTypeList(System.Web.UI.WebControls.DropDownList ddl, string sType)
		{
			ddl.Items.Clear();

			Hashtable ht = GetAllValueByType(sType);
			if(ht == null || ht.Count == 0) return;

			foreach(DictionaryEntry myDE in ht)
			{
				ListItem li = new ListItem(myDE.Value.ToString(),myDE.Key.ToString());
				ddl.Items.Add(li);
			}
		}

		//��DataTable��ת��ֵ����ʶֵ����ʶ���ơ�
		public static void GetColumnValueFromDic(DataTable dt, string SourceColumn, string DestColumn, string sType)
		{
			try
			{
				Hashtable ht = GetAllValueByType(sType);
				if(ht == null || ht.Count == 0) return;

				foreach(DataRow dr in dt.Rows)
				{
					string tmp = PublicRes.GetString(dr[SourceColumn]);
					if(tmp != "")
					{
						dr.BeginEdit();
						if(ht.ContainsKey(tmp))
						{
							dr[DestColumn] = ht[tmp].ToString();
						}
						else
						{
							dr[DestColumn] = "δ֪����(" + tmp + ")";
						}
						dr.EndEdit();
					}
				}
			}
			catch
			{}
		}
		//��DataTable��ת��ֵ����ʶֵ����ʶ���ơ�//����ר��
		public static void GetColumnValueFromDic_Fund(DataTable dt, string SourceColumn, string DestColumn, string sType,int fcurtype)
		{
			try
			{
				Hashtable ht = GetAllValueByType(sType);
				if(ht == null || ht.Count == 0) return;

				foreach(DataRow dr in dt.Rows)
				{
					string tmp = PublicRes.GetString(dr[SourceColumn]);
					if(tmp != "")
					{
						dr.BeginEdit();
						if(ht.ContainsKey(tmp))
						{
							dr[DestColumn] = ht[tmp].ToString();
							if(fcurtype==2 && sType=="PAY_STATE")
							{
								string [] splits=ht[tmp].ToString().Split('/');
								if(splits.Length>0)
								{
									dr[DestColumn] = splits[0];
								}
							}
							
						}
						else
						{
							dr[DestColumn] = "δ֪����(" + tmp + ")";
						}
						dr.EndEdit();
					}
				}
			}
			catch
			{}
		}
		//furion end

		public static string returnDate(string dateStr)  //����20050615֮�����ݷ��ؿɶ��ԽϺõ�2005��06��15��
		{
			return dateStr.Substring(0,4) + "��" + dateStr.Substring(4,2) +"��" + dateStr.Substring(6,2) + "��";
		}

		public static string convertBase64(string str)
		{
			return System.Text.Encoding.Default.GetString(Convert.FromBase64String(str.Replace("-","+").Replace("_","/")));
		}

		public static string convertToBase64(string tmp)
		{
			byte[] by = System.Text.Encoding.Default.GetBytes(tmp.ToCharArray()); 
			return Convert.ToBase64String(by).Replace("=","%3D").Replace("+","-").Replace("/","_");
		}

		/// <summary>
		/// ���������
		/// </summary>
		/// <param name="creid"></param>
		/// <returns></returns>
		public static string ConvertCreID(string creid)
		{
			string tmp = creid.Trim();
			if(tmp.Length >= 15)
			{
				tmp = tmp.Substring(0,tmp.Length-6) + "***" + tmp.Substring(tmp.Length-3,3);			
			}
			return tmp;
		}

        /// <summary>
        /// �����ַ���������ǰstartλ����endλ��������ͨ�˺� �����̻��쵥:���������Ϣ�е����֤�������ʺ���չʾǰ3λ�ͺ�5λ
        /// </summary>
        /// <param name="str">�ַ���</param>
        /// <param name="start">����ǰ����λ</param>
        /// <param name="end">���������λ</param>
        /// <returns></returns>
        public static string ConvertID(string str,int start,int end)
        {
            if (str == null)
            {
                return "";
            }

            string tmp = str.Trim();
            int len = tmp.Length;
            if (len < start || len < end) {
                return tmp;
            }
            string x = "";
            int l = len-(start+end);
            for (int i = 0; i < l; i++) 
            {
                x += "*";
            }
            if (len > start + end)
            {
                tmp = tmp.Substring(0, start) + x + tmp.Substring(len - end, end);
            }
            return tmp;
        }

        /// <summary>
        /// �����м�λ������
        /// </summary>
        /// <param name="mid">�����м����λ</param>
        /// <returns></returns>
        public static string ConvertID(string id, int mid) 
        {
            if (string.IsNullOrEmpty(id)) 
            {
                return "";
            }
            int len = id.Length;
            if (len <= mid) 
            {
                return id;
            }
            int r_len = len - mid;
            int pre = 0;//ǰ�汣��λ��
            int last = 0;//���汣��λ��
            int y = r_len % 2;
            if (y == 0)
            {
                last = r_len / 2;
                pre = last;
            }
            else 
            {
                last = r_len / 2;
                pre = last + y;
            }


            return ConvertID(id,pre,last);
        }


		public static string ConvertAccount(string account)
		{
			string tmp = account.Trim();
			if(tmp.Length > 7)
			{
				tmp = tmp.Substring(0,tmp.Length - 7) + "****" + tmp.Substring(tmp.Length -3,3);
			}

			return tmp;
		}
		/// <summary>
		/// ȥ�������ַ�
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static string replaceHtmlStr(string str)  
		{
			if(str == null) return null; //furion 20050819

			str = str.Replace("\"","��");
			str = str.Replace("'","��");  
			
			str = str.Replace("script","�������");
			str = str.Replace("<","��");
			str = str.Replace(">","��");
			return str;
		}


		/// <summary>
		/// ȥ�������ַ�
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static string replaceSqlStr(string instr) 
		{
			return TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.replaceSqlStr(instr); 
		}


        public static void GetAllBankListFromDic(System.Web.UI.WebControls.DropDownList ddl)
        {
            ddl.Items.Clear();
            Hashtable ht = new Hashtable();
            ht = BankIO.GetBankHashTable();
            if (ht == null || ht.Count == 0) return;
            Hashtable htTurn = new Hashtable();
            //��ֵת���£���������
            foreach (DictionaryEntry de in ht)
            {
                if (!(htTurn.Contains(de.Value.ToString())))
                {
                    htTurn.Add(de.Value.ToString(), de.Key.ToString());
                }
            }

            ArrayList akeys = new ArrayList(htTurn.Keys);
            akeys.Sort();
            foreach (string k in akeys)
            {
                ListItem li = new ListItem(k.ToString(), htTurn[k].ToString());
                ddl.Items.Add(li);
            }
        }

        public static void GetAllDownListDic(System.Web.UI.WebControls.DropDownList ddl, string mark)
        {
            ddl.Items.Clear();
            Hashtable ht = new Hashtable();
            if (mark=="airCompay")
                ht = commData.AirCompany();
            if (mark == "PNRStatus")
                ht = commData.PNRStatus();
            if (ht == null || ht.Count == 0) return;
            Hashtable htTurn = new Hashtable();
            //��ֵת���£���������
            foreach (DictionaryEntry de in ht)
            {
                if (!(htTurn.Contains(de.Value.ToString())))
                {
                    htTurn.Add(de.Value.ToString(), de.Key.ToString());
                }
            }

            ArrayList akeys = new ArrayList(htTurn.Keys);
            akeys.Sort();
            foreach (string k in akeys)
            {
                ListItem li = new ListItem(k.ToString(), htTurn[k].ToString());
                ddl.Items.Add(li);
            }
            ddl.Items.Add(new ListItem("ȫ��", ""));
        }

      
	}
}
