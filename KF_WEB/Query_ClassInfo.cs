using System;
using System.Configuration;
using TENCENT.OSS.CFT.KF.DataAccess;
using TENCENT.OSS.CFT.KF.Common;
using System.Data;
using System.Collections;
using TENCENT.OSS.C2C.Finance.Common.CommLib;


namespace TENCENT.OSS.CFT.KF.KF_Service
{
	/// <summary>
	/// �û��ʻ��������ʽ��ʾ
	/// </summary>
	public class T_USER
	{
		public string u_QQID;
		public string u_CurType;				//��������
		public string u_TrueName;				//��ʵ����
		public string u_Balance;				//�ʻ����
		public string u_Con;					//������
		public string u_Yday_Balance;			//�������
		public string u_Quota;					//���ʽ����޶�
		public string u_APay;					//�����Ѹ����
		public string u_Quota_Pay;				//����֧���޶�
		public string u_Save_Time;				//����������
		public string u_Fetch_Time;				//����������
		public string u_Login_IP;				//����¼/�޸ĵ�IP��ַ
		public string u_Modify_Time_C2C;		//��󡾵�¼/�޸ġ�ʱ��(c2c)
		public string u_State;					//�ʻ�״̬
		public string u_Memo;					//�û���ע
		public string u_Modify_Time;			//����޸�ʱ�䣨���أ�
		public string u_User_Type;				//�û����͡�

		public T_USER()
		{
			//�費��Ҫ��ʼ���أ�
		}
	}


	/// <summary>
	/// �н��˻���
	/// </summary>
	public class T_USER_MED : T_CLASS_BASIC
	{
		public string fuid; //uid
		public bool IsNew = false; //�Ƿ�Ϊ�������н��ʻ�
		public string Fcurtype ; //�������͡�
		//**********��ʾʱ��Ҫ���ֶ�
		public string fqqid;
		public string Fuser_type; //�û�����
		public string fspid;  //��������
		public string Fbalance; //�ʻ����
		public string Ftruename; //��ʵ������
		public string Fcompany_name; //��˾���ơ�
		public string FSex; 
		public string FAge;
		public string Fphone;
		public string Fmobile;
		public string Fcre_type; //֤�����͡�
		public string Fcreid; //֤�����롣
		public string Fpcode; //�������롣
		public string Femail; //�û�EMail
		public string Farea; //������ʡ��]
		public string FCity; //����
		public string Faddress; //��ϵ��ַ��
		public string Fmemo; //��ע
		//*********************************
		//��������������չ��Ϣ by edwardzheng
		public string Fatt_id;
		public string fmer_key;

		public void GetInfoFromDB(DataRow dr)
		{
			IsNew = false;
			fuid = QueryInfo.GetString(dr["fuid"]);
			fqqid = QueryInfo.GetString(dr["fqqid"]);
			Fuser_type = QueryInfo.GetInt(dr["Fuser_type"]);
			fspid = QueryInfo.GetString(dr["fspid"]);
			Fbalance = QueryInfo.GetInt(dr["Fbalance"]);
			Ftruename = QueryInfo.GetString(dr["Ftruename"]);
			Fcompany_name = QueryInfo.GetString(dr["Fcompany_name"]);
			FSex = QueryInfo.GetInt(dr["FSex"]);
			FAge = QueryInfo.GetString(dr["FAge"]);
			Fphone = QueryInfo.GetString(dr["Fphone"]);
			Fmobile = QueryInfo.GetString(dr["Fmobile"]);
			Fcre_type = QueryInfo.GetInt(dr["Fcre_type"]);
			Fcreid = QueryInfo.GetString(dr["Fcreid"]);
			Fpcode = QueryInfo.GetString(dr["Fpcode"]);
			Femail = QueryInfo.GetString(dr["Femail"]);
			Farea = QueryInfo.GetString(dr["Farea"]);
			FCity = QueryInfo.GetString(dr["FCity"]);
			Faddress = QueryInfo.GetString(dr["Faddress"]);
			Fmemo = QueryInfo.GetString(dr["Fmemo"]);
			Fcurtype = QueryInfo.GetString(dr["FCurType"]);
			Fatt_id = QueryInfo.GetString(dr["Fatt_id"]);
			fmer_key = QueryInfo.GetString(dr["fmer_key"]);
		}

		private void ValidInfo()
		{			
			if(fspid == null || fspid == "")
			{
				throw new LogicException("�������벻��Ϊ�գ�");
			}

			if(fspid.Length != 10)
			{
				throw new LogicException("�������볤�Ȳ���ȷ��");
			}
		}

		private void InitInfo()
		{
			if(Fbalance == null || Fbalance == "") Fbalance = "0";
			if(FAge  == null || FAge == "") FAge = "0";
			fqqid = fspid;
		}

		private string IntToStr(int i)
		{
			return i.ToString();
		}

		public bool Update(Finance_Header fh)
		{
			return false;
//			ValidInfo();
//			InitInfo();
//
//			ArrayList al = new ArrayList();
//			MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("YW_V30"));
//			try
//			{
//				da.OpenConn();
//
//				if(IsNew)
//				{
//					
//					if(SPIDIsExists(fspid))
//					{
//						throw new LogicException("���ݿ����Ѵ���ָ���Ļ������룬����������������룡");
//					}
//					//����ʱҪ�����н��ʻ����û����ϱ������ʻ��󶨱���������
//					string strSql = "select max(fuid) from c2c_db.t_middle_user";
//					string imax = da.GetOneResult(strSql);
//					if(imax == null || imax == "")
//					{
//						fuid = "10001";
//					}
//					else
//					{
//						fuid = IntToStr(Int32.Parse(imax) + 1);
//					}
//
//					strSql = "Insert c2c_db.t_middle_user(fuid,fqqid,fspid,ftruename,fcurtype,fbalance,fip,fstate,fmemo,fmodify_time,FCreate_Time) values "
//						+ " ('{0}','{1}','{2}','{3}','{4}',{5},'{6}',1,'{7}',now(),now())";
//					strSql = String.Format(strSql,fuid,fqqid,fspid,Ftruename,Fcurtype,Fbalance,fh.UserIP,Fmemo);
//					al.Add(strSql);
//
//
//					string passwd = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile("tencent","md5");
//					string pay_passwd = passwd;
//					strSql = "Insert " + PublicRes.GetTName("t_user_info",fuid) + "(Fuid,Fqqid,Ftruename,Fcompany_name,Fpasswd,Fpay_passwd,Fsex"
//						+ ",Fage,Fphone,Fmobile,Femail,Farea,Fcity,Faddress,Fpcode,Fcre_type,Fcreid,Fip,Fmemo,Fstate,Fuser_type,Fmodify_time) values"
//						+ " ('{0}','{1}','{2}','{3}','{4}','{5}',{6},{7},'{8}','{9}','{10}','{11}','{12}','{13}','{14}',{15},'{16}','{17}','{18}',1,'{19}',now())";
//					strSql = String.Format(strSql,fuid,fqqid,Ftruename,Fcompany_name,passwd,pay_passwd,FSex,FAge,Fphone,Fmobile,Femail,Farea,FCity,
//						Faddress,Fpcode,Fcre_type,Fcreid,fh.UserIP,Fmemo,Fuser_type);
//					al.Add(strSql);
//
//
//				}
//				else
//				{
//					if(!SPIDIsExists(fspid))
//					{
//						throw new LogicException("���ݿ���û��ָ�����ʻ���");
//					}
//					//����ʱ�����н��ʻ����û����ϱ�
//					string strSql = "update c2c_db.t_middle_user set Ftruename='{0}',Fcurtype={1},Fip='{2}',Fstate=1,"
//						+ "FMemo='{3}',Fmodify_time=now() where fuid='{4}'";
//					strSql = String.Format(strSql,Ftruename,Fcurtype, fh.UserIP,Fmemo,fuid);
//					al.Add(strSql);
//
//					strSql = "update " + PublicRes.GetTName("t_user_info",fuid) + " set Ftruename='{0}',Fcompany_name='{1}',"
//						+ "Fsex={2},Fage={3},Fphone='{4}',Fmobile='{5}',Femail='{6}',Farea='{7}',Fcity='{8}',Faddress='{9}'"
//						+ ",Fpcode='{10}',Fcre_type={11},Fcreid='{12}',Fip='{13}',Fmemo='{14}',Fstate=1,Fuser_type={15},Fmodify_time=now()"
//						+ " where fuid='{16}'";
//					strSql = String.Format(strSql,Ftruename,Fcompany_name,FSex,FAge,Fphone,Fmobile,Femail,Farea,FCity,Faddress,
//						Fpcode,Fcre_type,Fcreid,fh.UserIP,Fmemo,Fuser_type,fuid);
//					al.Add(strSql);
//				}
//
//			
//				
//				return da.ExecSqls_Trans(al);
//			}
//			finally
//			{
//				da.Dispose();
//			}
		}


		public bool UpdatePasswd(string spid,out string Password)
		{
			/*
			ArrayList al = new ArrayList();
			MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("YW"));
			MySqlAccess da_zl = new MySqlAccess(PublicRes.GetConnString("ZL"));
			try
			{
				da.OpenConn();
				da_zl.OpenConn();

				if(!SPIDIsExists(spid))
				{
					throw new LogicException("���ݿ���û��ָ�����ʻ���");
				}

				string uid = da.GetOneResult("select fuid from c2c_db.t_middle_user WHERE fspid='"+spid+"'");

				string s = "";
				for (int i = 0;i<6; i++)
				{
					//����ʱ,������ͬһ������;
					System.Threading.Thread.Sleep(1);

					System.Random rd = new Random();
					s += rd.Next(10);
				}

				Password = s;
				// TODO: 1�ͻ���Ϣ��������
				string strSql = "UPDATE c2c_db.t_muser_user SET fpasswd='"+System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(s,"md5").ToLower()+"' "+
					"WHERE fuid="+uid+" AND fqqid='"+spid+"'";
				al.Add(strSql);

				return da_zl.ExecSqls_Trans(al);
			}
			finally
			{
				da.Dispose();
				da_zl.Dispose();
			}
			*/

			string s = "";
			for (int i = 0;i<6; i++)
			{
				//����ʱ,������ͬһ������;
				System.Threading.Thread.Sleep(1);

				System.Random rd = new Random();
				s += rd.Next(10);
			}

			Password = s;

			s = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(Password,"md5").ToLower();

			string errMsg = "";
			string strSql = "spid=" + spid + "&qqid=" + spid;
			strSql += "&passwd=" + s + "&modify_time=" + PublicRes.strNowTimeStander;

			int iresult = CommQuery.ExecSqlFromICE(strSql,CommQuery.UPDATE_MUSER,out errMsg);
 
			if(iresult == 1)
				return true;
			else
				throw new LogicException("����muser����" + errMsg);
		}


		public static bool SPIDIsExists(string spid)
		{
			/*
			//string strSql = "select count(1) from c2c_db.t_middle_user where fspid='" + spid + "'";
			string strSql = "select count(1) from c2c_db.t_merchant_info where fspid='" + spid + "'";
			return PublicRes.ExecuteOne(strSql,"ZL") == "1";
			*/

			string strSql = "spid=" + spid;
			string errMsg = "";
			string testspid = CommQuery.GetOneResultFromICE(strSql,CommQuery.QUERY_MERCHANTINFO,"Fspid",out errMsg);

			if(testspid != null && testspid.Trim() != "")
				return true;
			else
				return false;
		}
	}


	/// <summary>
	/// �û����ϱ������ʽ
	/// </summary>
	public class T_USER_INFO
	{
		public string u_QQID;								//�ʻ�����
		public string u_Credit;								//���õȼ�
		public string u_TrueName;							//��ʵ����
		public string u_Sex;									//�Ա�
		public string u_Age;									//����
		public string u_Phone;								//�̶��绰
		public string u_Mobile;								//�ֻ�
		public string u_Email;								//�û���E-mail��ַ
		public string u_Area;								//������ʡ��
		public string u_City;								//����
		public string u_Address;							//��ϵ��ַ
		public string u_PCode;								//��������
		public string u_Cre_Type;								//֤������
		public string u_CreID;								//֤������
		public string u_Memo;								//��ע
		public string u_Modify_Time;						//����޸�ʱ�䣨���أ�
	}


	/// <summary>
	/// �û��������ʻ��������ʽ
	/// </summary>
	public class T_BANK_USER
	{
		public string u_QQID;
		public string u_Bank_Type;						//���е�����
		public string u_Bank_Name;					//��������/����������
		public string u_TrueName;					//��������
		public string u_Area;						//������ʡ��
		public string u_City;						//����
		public string u_BankID;						//�����ʻ�����
		public string u_State;							//�ʻ�״̬
		public string u_Login_IP;							//����޸ĵ�IP��ַ
		public string u_Memo;						//��ע
		public string u_Modify_Time;				//ע��ʱ��/����ʱ��
	}


	/// <summary>
	/// ���׵��������ʽ
	/// </summary>
	public class T_PAY_LIST
	{
		public string u_ListID;
		public string u_Coding;					//��������
		public string u_SPID;					//�����������ƣ������ߣ�
		public string u_Bank_ListID;			//�����еĶ�����
		public string u_Pay_Type;					//֧������
		public string u_BuyID;					//����ʻ�����
		public string u_Buy_Name;				//�������
		public string u_Buy_Bank_Type;				//��ҿ�����
		public string u_Buy_BankID;				//��ҵ������ʺ�
		public string u_SaleID;					//�����ʻ�����
		public string u_Sale_Name;				//���ҵ�����
		public string u_CurType;					//���ִ���
		public string u_State;						//����״̬
		public string u_LState;					//���׵�״̬
		public string u_Price;						//��Ʒ�ļ۸�
		public string u_Carriage;					//��������
		public string u_PayNum;					//Ӧ֧�����ܼ۸�
		public string u_Fact;						//ʵ��֧������
		public string u_Procedure;					//���ף�����������
		public string u_Service;					//�������
		public string u_Cash;						//�ֽ�֧�����
		public string u_Create_Time_C2C;		//��������ʱ�䣨c2c��
		public string u_Create_Time;			//��������ʱ�䣨����ϵͳʱ�䣩
		public string u_Bargain_Time;			//��Ҹ���ʱ��(bank)
		public string u_Receive_Time_C2C;		//��������ʱ��(c2c)
		public string u_Receive_Time;			//��������ʱ�䣨���أ�
		public string u_IP;						//����޸Ľ��׵���IP
		public string u_Memo;					//����˵��
		public string u_Explain;				//��ע����̨��Ա������¼��
		public string u_Modify_Time;			//����޸�ʱ�䣨c2c��/ ����ϵͳʱ��

		public string u_Pay_Time;
	}


	/// <summary>
	/// �û��ʻ���ˮ�������ʽ
	/// </summary>
	public class T_BANKROLL_LIST
	{		
		public string u_BKID;					//��ˮ��ID��
		public string u_ListID;					//���׵���ID�ţ�C2C��վ��ã�
		public string u_SPID;					//�����������ƣ������ߣ�
		public string u_UID;					//�û���ID(QQ����)���
		public string u_True_Name;				//�û�����
		public string u_Type;						//��������
		public string u_Subject;					//���(��ȡ��)
		public string u_FromID;					//�Է���ID��QQ���룬�����ʺţ�
		public string u_From_Name;				//�Է�������
		public string u_Balance;					//�ʻ����
		public string u_PayNum;					//���
		public string u_Bank_Type;					//�û����е�����
		public string u_CurType;					//���ִ���
		public string u_Prove;					//����ƾ֤
		public string u_Create_Time;			//����ʱ�䣨���أ�
		public string u_ApplyID;				//Ӧ��ϵͳ��ID��
		public string u_IP;						//�ͻ���IP��ַ
		public string u_Memo;					//��ע/˵��
		public string u_Modify_Time;			//����޸�ʱ��

		public string u_Action_Type;
	}

	/// <summary>
	/// �û�������ˮ�������ʽ
	/// </summary>
	public class T_USERPAY_LIST
	{		
		public string u_Ude_ID;					//��ˮ��ID��
		public string u_ListID;					//���׵���ID�ţ�C2C��վ��ã�
		public string u_SPID;					//�����������ƣ������ߣ�
		public string u_QQID;					//�û���ID(QQ����)���
		public string u_True_Name;				//�û�����
		public string u_Type;						//��������
		public string u_Subject;					//���(��ȡ��)
		public string u_FromID;					//�Է���ID��QQ���룬�����ʺţ�
		public string u_From_Name;				//�Է�������
		public string u_Balance;					//�ʻ����
		public string u_From_Balance;				//�Է��ʻ������
		public string u_PayNum;					//���
		public string u_Bank_Type;					//�û����е�����
		public string u_CurType;					//���ִ���
		public string u_Prove;					//����ƾ֤
		public string u_Create_Time;			//����ʱ�䣨���أ�
		public string u_ApplyID;				//Ӧ��ϵͳ��ID��
		public string u_IP;						//�ͻ���IP��ַ
		public string u_Memo;					//��ע/˵��
		public string u_Modify_Time;			//����޸�ʱ��
	}

	/// <summary>
	/// ��Ѷ�����ʻ��������ʽ(��û�д˱�)
	/// </summary>
	public class T_TC_BANK
	{
		public string u_Bank_Name;				//��������/����������
		public string u_Bank_Type;					//���е�����
		public string u_TrueName;				//��������
		public string u_Area;					//������ʡ��
		public string u_City;					//����
		public string u_CurType;				//���ִ���
		public string u_Balance;					//�ʻ����
		public string u_Yd_Balance;				//�����ʻ����
		public string u_Create_Time;			//��������
		public string u_IP;						//����޸ĵ�IP��ַ
		public string u_Memo;					//��ע
		public string u_Modify_Time;			//ע��ʱ��/����ʱ��
	}


	/// <summary>
	/// ��Ѷ�տ��¼�������ʽ
	/// </summary>
	public class T_TCBANKROLL_LIST
	{
		public string u_ListID;
		public string u_Tde_ID;				//��ˮ��ID��
		public string u_SPID;				//�����������ƣ������ߣ�
		public string u_UID;				//��Ѷ�ʻ���ID��
		public string u_State;
		public string u_Pay_Front_Time;
		public string u_Pay_Time;
		public string u_Type;					//��������/ҵ������
		public string u_Subject;				//���(��ȡ��)
		public string u_Num;					//���׵Ľ��
		public string u_Sign;				//���ױ��
		public string u_Bank_List;			//���ж�����
		public string u_Bank_Acc;			//���ж�����Ȩ��/ƾ֤��
		public string u_Bank_Type;				//���е�����
		public string u_CurType;				//���ִ���
		public string u_Aid;				//�Է���ID�������ʺţ�
		public string u_Prove;				//����ƾ֤
		public string u_IP;					//�ͻ���IP��ַ
		public string u_Memo;				//��ע/˵��
		public string u_Modify_Time;		//������ˮʱ�䣨���أ�
	}

	/// <summary>
	/// ��Ѷ�����¼�������ʽ
	/// </summary>
	public class T_TCBANKPAY_LIST
	{
		public string u_ListID;
		public string u_Tde_ID;				
		public string u_Bank_List;				
		public string u_Bankid;				
		public string u_State;
		public string u_Type;
		public string u_Subject;
		public string u_Num;					
		public string u_Sign;				
		public string u_Bank_Acc;				
		public string u_Bank_Type;			
		public string u_Curtype;			
		public string u_Aid;			    
		public string u_ABankid;			
		public string u_aName;				
		public string u_Prove;				
		public string u_IP;				   
		public string u_Memo;				
		public string u_Modify_Time;		
		public string u_Pay_Front_Time;		
		public string u_Pay_Time;			
		public string u_Uid;		       
	}

	/// <summary>
	/// �˿�������ʽ
	/// </summary>
	public class T_REFUND
	{
		public string u_ListID;
		public string u_RListID;				//�˿��ID��
		public string u_Create_Time;				
		public string u_SPID;				//�����������ƣ������ߣ�
		public string u_PayType;				//�˿ʽ
		public string u_BuyID;				//����ʻ�����
		public string u_Buy_Name;			//�������
		public string u_Buy_Bank_Type;			//������е�����
		public string u_Buy_BankID;			//��ҵ������ʺ�
		public string u_SaleID;				//�����ʻ�����
		public string u_Sale_Name;			//���ҵ�����
		public string u_Sale_Bank_Type;		//�������е�����
		public string u_Sale_BankID;		//���ҵ������ʺ�
		public string u_State;					//�˿��״̬���ӿڣ�
		public string u_LState;				//�˿��״̬
		public string u_PayBuy;				//�˻�����ҵĽ��
		public string u_PaySale;				//�˻������ҵĽ��
		public string u_Procedure;				//����������
		public string u_Bargain_Time;		//C2C�����˿�����
		public string u_OK_Time;			//�ɽ����ڣ��˿����ڣ�
		public string u_IP;					//����޸ĵ�IP��ַ
		public string u_Memo;				//�˿�˵��
		public string u_Explain;			//������Ա��ע
		public string u_Modify_Time;		//����޸�ʱ��
	}


	#region Furion 20050815 �������������á�

	/// <summary>
	/// �������ͱ�
	/// </summary>
	public class T_CheckType
	{
		public bool IsNew;

		public string FTypeID;
		public string FTypeName;
		public int FRoadType;
		public int FActionType;

		public CheckLevelInfo[] LevelInfo = new CheckLevelInfo[4];

		public static T_CheckType[] GetAllCheckType()
		{
			string strSql = "select FTypeID,FTypeName from c2c_fmdb.t_check_type";
			MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("HT"));
			try
			{
				T_CheckType[] Result = null;

				da.OpenConn();
				DataTable dt = da.GetTable(strSql);

				if(dt != null)
				{
					Result = new T_CheckType[dt.Rows.Count];

					for(int i = 0; i < dt.Rows.Count; i++)
					{
						Result[i] = new T_CheckType();
						Result[i].FTypeID = QueryInfo.GetString(dt.Rows[i]["FTypeID"]);
						Result[i].FTypeName = QueryInfo.GetString(dt.Rows[i]["FTypeName"]);
					}					
				}

				return Result;
			}
			finally
			{
				da.Dispose();
			}
		}

		public static T_CheckType GetCheckInfo(string typeID)
		{
			T_CheckType Result = new T_CheckType();
			Result.IsNew = false;

			string strSql = "select * from c2c_fmdb.t_check_type where FTypeID='" + typeID + "'";

			MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("HT"));

			try
			{
				da.OpenConn();
				DataTable dt = da.GetTable(strSql);

				if(dt != null && dt.Rows.Count == 1)
				{
					Result.FTypeID = typeID;
					Result.FTypeName = QueryInfo.GetString(dt.Rows[0]["FTypeName"]);
					Result.FRoadType = Int32.Parse(QueryInfo.GetInt(dt.Rows[0]["FRoadType"]));
					Result.FActionType = Int32.Parse(QueryInfo.GetInt(dt.Rows[0]["FActionType"]));

					Result.LevelInfo[0] = new CheckLevelInfo();
					Result.LevelInfo[0].FCheck = QueryInfo.GetString(dt.Rows[0]["FCheck1"]);

					Result.LevelInfo[1] = new CheckLevelInfo();
					Result.LevelInfo[1].FCheck = QueryInfo.GetString(dt.Rows[0]["FCheck2"]);

					Result.LevelInfo[2] = new CheckLevelInfo();
					Result.LevelInfo[2].FCheck = QueryInfo.GetString(dt.Rows[0]["FCheck3"]);

					Result.LevelInfo[3] = new CheckLevelInfo();
					Result.LevelInfo[3].FCheck = QueryInfo.GetString(dt.Rows[0]["FCheck4"]);

					strSql = "select * from c2c_fmdb.t_check_user where FCheckType='" + typeID + "'";
					dt = da.GetTable(strSql);
					if(dt != null)
					{
						foreach(DataRow dr in dt.Rows)
						{
							int iIndex = Int32.Parse(dr["FLevel"].ToString());
							Result.LevelInfo[iIndex - 1].FLevelIndex = iIndex;
							Result.LevelInfo[iIndex - 1].FLevelName = QueryInfo.GetString(dr["FLevelName"]);
							Result.LevelInfo[iIndex - 1].FUserID = QueryInfo.GetString(dr["FUserID"]);
						}
					}
				}
				return Result;
			}
			finally
			{
				da.Dispose();
			}
		}

		public bool UpdateCheckInfo()
		{
			return false;
//			if(!Validate())
//			{
//				if(IsNew) throw new LogicException("ָ���ģɣ��Ѵ��ڣ�");
//				else throw new LogicException("ָ���ģɣĲ����ڣ�");
//			}
//
//			string strSql = "";
//			ArrayList al = new ArrayList();
//			if(IsNew)
//			{
//				strSql = "Insert c2c_fmdb.t_check_type(FTypeID,FTypeName,FRoadType,FActionType,FCheck1,FCheck2,FCheck3,FCheck4)"
//					+ " values('{0}','{1}',{2},{3},'{4}','{5}','{6}','{7}')";
//				strSql = String.Format(strSql,FTypeID,FTypeName,FRoadType,FActionType,LevelInfo[0].FCheck,LevelInfo[1].FCheck,LevelInfo[2].FCheck,LevelInfo[3].FCheck);
//
//				al.Add(strSql);
//
//				for(int i = 0; i < 4; i++)
//				{
//					strSql = "Insert c2c_fmdb.t_check_user(FCheckType,FUserID,FLevel,FLevelName)"
//						+ " values('{0}','{1}',{2},'{3}')";
//
//					strSql = String.Format(strSql,FTypeID,LevelInfo[i].FUserID,LevelInfo[i].FLevelIndex,LevelInfo[i].FLevelName);
//					al.Add(strSql);
//				}
//			}
//			else
//			{
//				strSql = "Update c2c_fmdb.t_check_type set FTypeName='{0}',FRoadType={1},FActionType={2},FCheck1='{3}',"
//					+ "FCheck2='{4}',FCheck3='{5}',FCheck4='{6}' where FTypeID='{7}'";
//
//				strSql = String.Format(strSql,FTypeName,FRoadType,FActionType,LevelInfo[0].FCheck,LevelInfo[1].FCheck,LevelInfo[2].FCheck,LevelInfo[3].FCheck,FTypeID);
//				al.Add(strSql);
//
//				for(int i = 0; i < 4; i++)
//				{
//					strSql = "Update c2c_fmdb.t_check_user set FUserID='{0}',FLevelName='{1}' where FCheckType='{2}' and FLevel={3}";
//
//					strSql = String.Format(strSql,LevelInfo[i].FUserID,LevelInfo[i].FLevelName,FTypeID,LevelInfo[i].FLevelIndex);
//					al.Add(strSql);
//				}
//			}
//
//			MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("HT"));
//			try
//			{
//				da.OpenConn();
//				return da.ExecSqls_Trans(al);
//			}
//			finally
//			{
//				da.Dispose();
//			}

		}

		private bool Validate()
		{
			if(FTypeID == null || FTypeID.Trim() == "" || FTypeName == null || FTypeName.Trim() == "")
			{
				throw new LogicException("����������ID���������ƣ�");
				return false;
			}

			const int CHECKLENGTH = 30; //���ڵ��������ó�����30
			if(LevelInfo[0].FCheck.Length > CHECKLENGTH )
			{
				throw new LogicException("��һ������������Ϣ̫������������ʮ���֣����������룡");
			}

			if(LevelInfo[1].FCheck.Length > CHECKLENGTH )
			{
				throw new LogicException("�ڶ�������������Ϣ̫������������ʮ���֣����������룡");
			}

			if(LevelInfo[2].FCheck.Length > CHECKLENGTH )
			{
				throw new LogicException("����������������Ϣ̫������������ʮ���֣����������룡");
			}

			if(LevelInfo[3].FCheck.Length > CHECKLENGTH )
			{
				throw new LogicException("���ĸ�����������Ϣ̫������������ʮ���֣����������룡");
			}

			string strSql = "select count(1) from c2c_fmdb.t_check_type where FTypeID='" + FTypeID + "'";
			string tmp = PublicRes.ExecuteOne(strSql,"HT");
			if(IsNew)
			{
				//����
				return tmp == "0";
			}
			else
			{
				//�޸�
				return tmp == "1";
			}
		}
	}

	public struct CheckLevelInfo
	{
		public int FLevelIndex;
		public string FUserID;
		public string FLevelName;
		public string FCheck;
	}

	#endregion


	/// <summary>
	/// ������Ϣ��
	/// </summary>
	public class FreezeInfo
	{
		public string fid;
		public string username;
		public string contact;
		public int FFreezeType;
		public string FFreezeID;
		public string FFreezeReason;
		public string FHandleResult;
        public string FFreezeChannel; //�ⶳ����

		// 2012/4/4 ����Ӷ�����Ϣ�Ľ�������
		public string strFreezeEndDate;

		public static FreezeInfo GetExistFreeze(string afreezeid,int FFreezeType)
		{
			MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("HT"));
			try
			{
				FreezeInfo fi = new FreezeInfo();
				
				da.OpenConn();
				string strSql = "select * from c2c_fmdb.t_freeze_list where FFreezeID='" + afreezeid + "' and FFreezeType=" + FFreezeType + " and FHandleFinish=1";
				DataTable dt = da.GetTable(strSql);
				/*964243���ʻ���2�ʶ��ᵥ,so��ʱ�ĳɶ���һ��
				if(dt == null || dt.Rows.Count != 1)
				{
					throw new LogicException("���Ҳ���ָ�������ݻ������ظ���");
				}*/
				if(dt == null || dt.Rows.Count == 0)
				{
					throw new LogicException("���Ҳ���ָ�������ݣ�");
				}
				else
				{
                    fi.username = QueryInfo.GetString(dt.Rows[0]["FUserName"]);
					fi.contact = QueryInfo.GetString(dt.Rows[0]["FContact"]);
					fi.fid = QueryInfo.GetString(dt.Rows[0]["FID"]);
                    fi.FFreezeChannel = QueryInfo.GetString(dt.Rows[0]["Ffreeze_channel"]);
				}

				return fi;
			}
			finally
			{
				da.Dispose();
			}
		}

		public static void CreateNewFreeze(Finance_Header fh, FreezeInfo fi)
		{
			//���ڶ��������
			MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("HT"));
			try
			{
				string strSql = "update c2c_fmdb.t_freeze_list set FHandleFinish = 9,FFreezeReason = 'ϵͳ�����������' where FFreezeID = '" + fi.FFreezeID + "' " +
					            "and FFreezeType = " + fi.FFreezeType + " and FHandleFinish = 1";   //�����������

				da.OpenConn();
				da.ExecSql(strSql);

                strSql = "Insert c2c_fmdb.t_freeze_list(FFreezeUserID,FFreezeUserIP,FFreezeTime,FFreezeType,FFreezeID,FUserName,FContact,FFreezeReason,Ffreeze_channel,FHandleFinish)"
					+ " values('{0}','{1}',now(),{2},'{3}','{4}','{5}','{6}',{7},1)";

				strSql = String.Format(strSql,fh.UserName,fh.UserIP,fi.FFreezeType,fi.FFreezeID,fi.username,fi.contact,fi.FFreezeReason,fi.FFreezeChannel);

				da.ExecSql(strSql);
			}
			finally
			{
				da.Dispose();
			}
		}

		public static void UpdateFreezeInfo(Finance_Header fh, FreezeInfo fi)
		{
			//���ڽⶳ����
			MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("HT"));
			try
			{
				string strSql = "update c2c_fmdb.t_freeze_list set FHandleUserID='{0}',FHandleUserIP='{1}',FHandleTime=now(),"
                            + "FHandleResult='{2}',FHandleFinish=9 where FID={3}";

				strSql = String.Format(strSql,fh.UserName,fh.UserIP,fi.FHandleResult,fi.fid);

				da.OpenConn();
				da.ExecSql(strSql);

				strSql = "update c2c_fmdb.t_freeze_list set FHandleFinish = 9,FFreezeReason = 'ϵͳ�����������' where FFreezeID = '" + fi.FFreezeID + "' " +
					     "and FFreezeType = " + fi.FFreezeType + " and FHandleFinish = 1";   //�����������

				da.ExecSql(strSql);
			}
			finally
			{
				da.Dispose();
			}
		}
	}
	#region ����
	public class T_CLASS_BASIC
	{
		public static bool SPIDIsExists(string spid)
		{
			/*
			//string strSql = "select count(1) from c2c_db.t_middle_user where fspid='" + spid + "'";
			string strSql = "select count(1) from c2c_db.t_merchant_info where fspid='" + spid + "'";
			return PublicRes.ExecuteOne(strSql,"ZL") == "1";
			*/

			string strSql = "spid=" + spid;
			string errMsg = "";
			string testspid = CommQuery.GetOneResultFromICE(strSql,CommQuery.QUERY_MERCHANTINFO,"Fspid",out errMsg);

			if(testspid != null && testspid.Trim() != "")
				return true;
			else
				return false;
		}
		
		/// <summary>
		/// ��һ����¼����ȡ��Ա����ֵ
		/// </summary>
		/// <param name="dr"></param>
		public void LoadFromDB(DataRow dr)
		{
			System.Reflection.FieldInfo fi;
			string col_name,col_type;
			System.Type this_type = this.GetType();
			for (int i=0;i<dr.Table.Columns.Count;i++)
			{
				col_name = dr.Table.Columns[i].ColumnName;
				fi = this_type.GetField(col_name);
				if ( fi==null )
					fi = this_type.GetField(col_name.ToLower());
				if ( fi!=null )
				{
					col_type = dr.Table.Columns[i].DataType.FullName.ToUpper();
					if ( col_type == "System.Int32" || col_type == "System.Int16" )
						fi.SetValue(this,QueryInfo.GetInt(dr[col_name]));
					else if ( col_type == "System.DateTime" )
						fi.SetValue(this,QueryInfo.GetDateTime(dr[col_name]));
					else
						fi.SetValue(this,QueryInfo.GetString(dr[col_name]));
				}
			}
		}

		

		/// <summary>
		/// �������������ȡ��Ա����ֵ
		/// </summary>
		/// <param name="dt"></param>
		public void LoadFromParamDB(DataTable dt)
		{
			System.Reflection.FieldInfo fi;
			string col_name,col_value;
			System.Type this_type = this.GetType();
			foreach(DataRow dr in dt.Rows)
			{
				col_name = dr["FKey"].ToString().Trim();
				fi = this_type.GetField(col_name);
				if ( fi==null )
					fi = this_type.GetField(col_name.ToLower());
				if ( fi!=null )
				{
					col_value = dr["FValue"].ToString().Trim();
					if (fi.FieldType.FullName=="System.Boolean")
						fi.SetValue(this,Convert.ToBoolean(col_value));
					else
						fi.SetValue(this,col_value);
				}
			}
		}
	}
	#endregion

    #region t_bankbulletin_info_all ��
    public class T_BANKBULLETIN_INFO_ALL : T_CLASS_BASIC
    {

        public bool IsNew = false; //�Ƿ�Ϊ������ͨ�û�����������
        public bool IsOPen = false;
        public string Fbanktype;
        public string Ftitle;
        public string Fmaintext;
        public string Fstartime;
        public string Fendtime;
        public string Fcreateuser;
        public string Fcreatetime;
        public string Fupdateuser;
        public string Fupdatetime;
        public string returnUrl;

        public bool CheckRight()
        {
            bool checkFlag = true;
            string msg = "";

            if (Fbanktype == null || Fbanktype == "")
            {
                checkFlag = false;
                msg += "[��������]����Ϊ�գ�";
            }
            if (Ftitle == null || Ftitle == "")
            {
                checkFlag = false;
                msg += "[�������]����Ϊ�գ�";
            }
            if (Fmaintext == null || Fmaintext == "")
            {
                checkFlag = false;
                msg += "[��������]����Ϊ�գ�";
            }

            if (Fcreateuser == null || Fcreateuser == "")
            {
                checkFlag = false;
                msg += "[������]����Ϊ�գ�";
            }


            Fupdatetime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            if (!checkFlag)
            {
                throw new LogicException("t_bankbulletin_info_all���ã�У�鲻ͨ����" + msg);
            }


            try
            {
                Fstartime = Convert.ToDateTime(Fstartime).ToString("yyyy-MM-dd HH:mm:ss");
            }
            catch (Exception ex)
            {
                throw new LogicException("t_bankbulletin_info_all���ã�У�鲻ͨ������ʼʱ�䲻��������");
            }
            try
            {
                Fendtime = Convert.ToDateTime(Fendtime).ToString("yyyy-MM-dd HH:mm:ss");
            }
            catch (Exception ex2)
            {
                throw new LogicException("t_bankbulletin_info_all���ã�У�鲻ͨ��������ʱ�䲻��������");
            }

            MySqlAccess dazw = new MySqlAccess(PublicRes.GetConnString("INC"));
            try
            {
                dazw.OpenConn();
                string strSql = "select count(*) from c2c_db_inc.t_bankbulletin_info_all  where Fbanktype='" + Fbanktype + "'";
                int count = Convert.ToInt32(dazw.GetOneResult(strSql));
                if (IsNew)
                {
                    if (count != 0)
                    {
                        throw new LogicException("t_bankbulletin_info_all���ã�У�鲻ͨ������������" + Fbanktype + "�Ѿ����ڣ�");
                    }
                }
                else
                {
                    if (count != 1)
                    {
                        throw new LogicException("t_bankbulletin_info_all���ã�У�鲻ͨ������������" + Fbanktype + "�����ڣ�");
                    }
                }




            }
            finally
            {
                dazw.Dispose();

            }

            return true;
        }


        public bool Update(Finance_Header fh)
        {

            ArrayList al = new ArrayList();
            MySqlAccess dainc = new MySqlAccess(PublicRes.GetConnString("INC"));
            string strSql = "";
            try
            {
                CheckRight();
                DateTime dFstartime = DateTime.Parse(Fstartime);
                if (IsNew)
                {
                    if (dFstartime <= DateTime.Now)
                    {
                        Fstartime = DateTime.Now.AddMinutes(5).ToString("yyyy-MM-dd HH:mm:ss");
                    }
                }
                DateTime dFendtime = DateTime.Parse(Fendtime);
                if (!IsNew)
                {
                    if (IsOPen)
                    {
                        Fendtime = DateTime.Now.AddMinutes(5).ToString("yyyy-MM-dd HH:mm:ss");
                    }
                }
                if (DateTime.Parse(Fstartime) >= dFendtime)
                {
                    throw new LogicException("t_bankbulletin_info_all���ã�У�鲻ͨ������ʼʱ����ڽ���ʱ�䣡");
                }

                //��ʼ����
                dainc.OpenConn();
                #region old
                //				if(IsNew)
                //				{
                //					
                //					strSql = "Insert c2c_db_inc.t_bankbulletin_info "
                //						+ " (Fbanktype,Ftitle,Fmaintext,Fstartime,Fendtime,Fcreateuser,Fcreatetime,Fupdateuser,Fupdatetime)"
                //						+ " values "
                //						+ " ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')";
                //					strSql = String.Format(strSql,
                //						Fbanktype,Ftitle,Fmaintext,Fstartime,Fendtime,Fcreateuser,Fcreatetime,Fupdateuser,Fupdatetime);
                //
                //					al.Add(strSql);



                //				}
                //				else
                //				{
                //					strSql = "UPDATE c2c_db_inc.t_bankbulletin_info  SET "
                //						+ " Ftitle='{1}',Fmaintext='{2}',Fstartime='{3}',Fendtime='{4}', Fupdateuser='{5}',Fupdatetime='{6}' "
                //						+ " WHERE Fbanktype='{0}'";
                //					strSql = String.Format(strSql,
                //						Fbanktype,Ftitle,Fmaintext,Fstartime,Fendtime,Fupdateuser,Fupdatetime);
                //
                //					al.Add(strSql);
                //				}
                //				return dainc.ExecSqls_Trans(al);
                #endregion
                string request = "banktype=" + Fbanktype + "&";
                request += "title=" + Ftitle + "&";
                request += "maintext=" + Fmaintext + "&";
                request += "startime=" + Fstartime + "&";
                request += "endtime=" + Fendtime + "&";
                request += "createuser=" + Fcreateuser + "&";
                request += "updateuser=" + Fupdateuser + "&";
                request += "interface_type=0";

                string Msg = "";
                bool rest = false;

                short result = -1;
                string reply = "";
                string msg = "";
                if (commRes.middleInvoke("bank_channel_bulletin_service", request, false, out reply, out result, out msg))
                {


                    if (result != 0)
                    {
                        rest = false;
                        throw new Exception("��������ά������ӿ�bank_channel_bulletin_service����ʧ�ܣ�result=" + result + "��msg=" + msg);


                    }
                    else
                    {
                        if (reply.IndexOf("result=0") > -1)
                        {

                            rest = true;



                        }
                        else
                        {
                            rest = false;
                            throw new Exception("��������ά������ӿ�bank_channel_bulletin_service����ʧ�ܣ�result=" + result + ",msg=" + msg + ",reply=" + reply);

                        }

                    }
                }
                return rest;

            }
            catch (Exception ex)
            {
                log4net.ILog log = log4net.LogManager.GetLogger("bank_channel_bulletin_service");
                if (log.IsErrorEnabled)
                    log.Error(Fcreateuser, ex);
                throw new Exception(ex.Message.Trim());
                return false;
            }
            finally
            {
                dainc.Dispose();
            }

        }

    }
    #endregion

    #region t_bankbulletin_type_all ��
    public class T_BANKBULLETIN_TYPE_ALL : T_CLASS_BASIC
    {
        public bool IsNew = false; //�Ƿ�Ϊ������ͨ�û�����������
        public bool IsOPen = false;
        public string Fbanktype;
        public string Fmaintext;
        public string Fstartime;
        public string Fendtime;
        public string Fcreateuser;
        public string Fcreatetime;
        public string Fupdateuser;
        public string Fupdatetime;
        public string returnUrl;
        public string Farea;
        public string Fcity;
        public string Fbusinetype;//ҵ������
 //       public string Falwaysworkstate;//�رղ��� 0. ��ʱ���(Ĭ��)  1. ������Ч
        public string Isaffectinterface;//�Ƿ�Ӱ��ӿ�
      //  public string Falwtime;//��Чʱ��Σ�������Чʱ
        public string Ftitle;
        public string Ftctext;//��������
        public string Fid;
    }
    #endregion

    #region t_bankbulletin_info ��
    public class T_BANKBULLETIN_INFO : T_CLASS_BASIC
    {
        public bool IsNew = false;
        public bool IsOPen = false;
        public string bulletin_id;//���й���id
        public string banktype;
        public string businesstype;//ҵ������
        public string op_support_flag;//ҵ��������
        public string closetype;//�Ƿ�Ӱ��ӿ�
        public string title;
        public string maintext;
        public string popuptext;//��������
        public string startime;
        public string endtime;
        public string createuser;
        public string updateuser;
        public string createtime;
        public string updatetime;
        public string returnUrl;
    }
    #endregion

    #region public_utility_charge.t_puc_new_service_info ��
    public class T_PUCNEWSERVICE_INFO : T_CLASS_BASIC
    {

        public bool IsNew = false; //�Ƿ�Ϊ������ͨ�û�����������

        public string Fservicecode;
        public string Fuctype; //��ʶ���������ά������
        public string Ftips; //��ʾ��Ϣ
        public string Fstartime; //��Ӧ����ά�����͵Ŀ�ʼʱ��
        public string Fendtime;

        public string Fupdatetime;
        public string returnUrl;

        public bool CheckRight()
        {
            bool checkFlag = true;
            string msg = "";

            if (Ftips == null || Ftips == "")
            {
                checkFlag = false;
                msg += "[��ʾ��Ϣ]����Ϊ�գ�";
            }


            Fupdatetime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            if (!checkFlag)
            {
                throw new LogicException("t_puc_new_service_info���ã�У�鲻ͨ����" + msg);
            }


            try
            {
                Fstartime = Convert.ToDateTime(Fstartime).ToString("yyyy-MM-dd HH:mm:ss");
            }
            catch (Exception ex)
            {
                throw new LogicException("t_puc_new_service_info���ã�У�鲻ͨ������ʼʱ�䲻��������");
            }
            try
            {
                Fendtime = Convert.ToDateTime(Fendtime).ToString("yyyy-MM-dd HH:mm:ss");
            }
            catch (Exception ex2)
            {
                throw new LogicException("t_puc_new_service_info���ã�У�鲻ͨ��������ʱ�䲻��������");
            }

            MySqlAccess dazw = new MySqlAccess(PublicRes.GetConnString("UC"));
            try
            {
                dazw.OpenConn();
                //string strSql="select count(*) from public_utility_charge.t_puc_new_service_info  where Fservicecode='"+Fservicecode+"'";
                string strSql = "select count(*) from public_utility_charge_platform.t_charge_resources_info  where FResourcesId='" + Fservicecode + "'";
                int count = Convert.ToInt32(dazw.GetOneResult(strSql));
                if (IsNew)
                {
                    if (count != 0)
                    {
                        throw new LogicException("t_puc_new_service_info���ã�У�鲻ͨ������������" + Fservicecode + "�Ѿ����ڣ�");
                    }
                }
                else
                {
                    if (count != 1)
                    {
                        throw new LogicException("t_puc_new_service_info���ã�У�鲻ͨ������������" + Fservicecode + "�����ڣ�");
                    }
                }




            }
            finally
            {
                dazw.Dispose();

            }

            return true;
        }


        public bool Update(Finance_Header fh)
        {

            ArrayList al = new ArrayList();
            MySqlAccess dainc = new MySqlAccess(PublicRes.GetConnString("UC"));
            string strSql = "";
            try
            {
                CheckRight();
                DateTime dFstartime = DateTime.Parse(Fstartime);
                if (dFstartime <= DateTime.Now)
                {
                    Fstartime = DateTime.Now.AddMinutes(5).ToString("yyyy-MM-dd HH:mm:ss");
                }
                DateTime dFendtime = DateTime.Parse(Fendtime);
                if (DateTime.Parse(Fstartime) >= dFendtime)
                {
                    throw new LogicException("t_puc_new_service_info���ã�У�鲻ͨ������ʼʱ����ڽ���ʱ�䣡");
                }

                //��ʼ����
                dainc.OpenConn();
                if (IsNew)
                {

                    //���������������
                }
                else
                {
                    //��uctype�����������
                    //strSql = "select Flimit_type from public_utility_charge.t_puc_new_service_info  where Fservicecode='"+Fservicecode+"'";
                    strSql = "select Flimit_type from public_utility_charge_platform.t_charge_resources_info  where FResourcesId='" + Fservicecode + "'";
                    string limittype = dainc.GetOneResult(strSql);

                    int newtype = Int32.Parse(limittype) | Int32.Parse(Fuctype);




                    //strSql = "UPDATE public_utility_charge.t_puc_new_service_info  SET Flimit_type=" + newtype + ",Fmodify_time='" + Fupdatetime + "' ";
                    strSql = "UPDATE public_utility_charge_platform.t_charge_resources_info  SET Flimit_type=" + newtype + ",Fmodifytime='" + Fupdatetime + "' ";
                    if (Fuctype == "8")
                    {
                        strSql += ",Frepair_limit_starttime='" + Fstartime + "',Frepair_limit_endtime='" + Fendtime + "',Frepair_limit_tips='" + Ftips + "' ";
                    }
                    else
                    {
                        //�ݲ�֧�֡����ø���
                    }

                    //strSql += " where Fservicecode='"+Fservicecode+"'";
                    strSql += " where FResourcesId='" + Fservicecode + "'";

                    al.Add(strSql);
                }
                return dainc.ExecSqls_Trans(al);
            }
            finally
            {
                dainc.Dispose();
            }

        }

    }
    #endregion
}
