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
	/// 用户帐户表的类形式表示
	/// </summary>
	public class T_USER
	{
		public string u_QQID;
		public string u_CurType;				//币种类型
		public string u_TrueName;				//真实姓名
		public string u_Balance;				//帐户余额
		public string u_Con;					//冻结金额
		public string u_Yday_Balance;			//昨日余额
		public string u_Quota;					//单笔交易限额
		public string u_APay;					//当日已付金额
		public string u_Quota_Pay;				//单日支付限额
		public string u_Save_Time;				//最近存款日期
		public string u_Fetch_Time;				//最近提款日期
		public string u_Login_IP;				//最后登录/修改的IP地址
		public string u_Modify_Time_C2C;		//最后【登录/修改】时间(c2c)
		public string u_State;					//帐户状态
		public string u_Memo;					//用户备注
		public string u_Modify_Time;			//最后修改时间（本地）
		public string u_User_Type;				//用户类型。

		public T_USER()
		{
			//需不需要初始化呢？
		}
	}


	/// <summary>
	/// 中介账户类
	/// </summary>
	public class T_USER_MED : T_CLASS_BASIC
	{
		public string fuid; //uid
		public bool IsNew = false; //是否为新增的中介帐户
		public string Fcurtype ; //币种类型。
		//**********显示时需要的字段
		public string fqqid;
		public string Fuser_type; //用户类型
		public string fspid;  //机构代码
		public string Fbalance; //帐户余额
		public string Ftruename; //真实姓名。
		public string Fcompany_name; //公司名称。
		public string FSex; 
		public string FAge;
		public string Fphone;
		public string Fmobile;
		public string Fcre_type; //证件类型。
		public string Fcreid; //证件号码。
		public string Fpcode; //邮政编码。
		public string Femail; //用户EMail
		public string Farea; //地区，省级]
		public string FCity; //城市
		public string Faddress; //联系地址。
		public string Fmemo; //备注
		//*********************************
		//以下是新增的扩展信息 by edwardzheng
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
				throw new LogicException("机构代码不能为空！");
			}

			if(fspid.Length != 10)
			{
				throw new LogicException("机构代码长度不正确！");
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
//						throw new LogicException("数据库中已存在指定的机构代码，请重新输入机构代码！");
//					}
//					//新增时要插入中介帐户表，用户资料表，银行帐户绑定表将来再做。
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
//						throw new LogicException("数据库中没有指定的帐户！");
//					}
//					//更新时更新中介帐户表，用户资料表。
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
					throw new LogicException("数据库中没有指定的帐户！");
				}

				string uid = da.GetOneResult("select fuid from c2c_db.t_middle_user WHERE fspid='"+spid+"'");

				string s = "";
				for (int i = 0;i<6; i++)
				{
					//不延时,会生成同一个数字;
					System.Threading.Thread.Sleep(1);

					System.Random rd = new Random();
					s += rd.Next(10);
				}

				Password = s;
				// TODO: 1客户信息资料外移
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
				//不延时,会生成同一个数字;
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
				throw new LogicException("更新muser出错" + errMsg);
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
	/// 用户资料表的类形式
	/// </summary>
	public class T_USER_INFO
	{
		public string u_QQID;								//帐户号码
		public string u_Credit;								//信用等级
		public string u_TrueName;							//真实姓名
		public string u_Sex;									//性别
		public string u_Age;									//年龄
		public string u_Phone;								//固定电话
		public string u_Mobile;								//手机
		public string u_Email;								//用户的E-mail地址
		public string u_Area;								//地区，省级
		public string u_City;								//城市
		public string u_Address;							//联系地址
		public string u_PCode;								//邮政编码
		public string u_Cre_Type;								//证件类型
		public string u_CreID;								//证件号码
		public string u_Memo;								//备注
		public string u_Modify_Time;						//最后修改时间（本地）
	}


	/// <summary>
	/// 用户绑定银行帐户表的类形式
	/// </summary>
	public class T_BANK_USER
	{
		public string u_QQID;
		public string u_Bank_Type;						//银行的类型
		public string u_Bank_Name;					//银行名称/开户行名称
		public string u_TrueName;					//开户姓名
		public string u_Area;						//地区，省级
		public string u_City;						//城市
		public string u_BankID;						//银行帐户号码
		public string u_State;							//帐户状态
		public string u_Login_IP;							//最后修改的IP地址
		public string u_Memo;						//备注
		public string u_Modify_Time;				//注册时间/更新时间
	}


	/// <summary>
	/// 交易单表的类形式
	/// </summary>
	public class T_PAY_LIST
	{
		public string u_ListID;
		public string u_Coding;					//订单编码
		public string u_SPID;					//机构代码名称（发起者）
		public string u_Bank_ListID;			//给银行的订单号
		public string u_Pay_Type;					//支付类型
		public string u_BuyID;					//买家帐户号码
		public string u_Buy_Name;				//付款方名称
		public string u_Buy_Bank_Type;				//买家开户行
		public string u_Buy_BankID;				//买家的银行帐号
		public string u_SaleID;					//卖家帐户号码
		public string u_Sale_Name;				//卖家的名称
		public string u_CurType;					//币种代码
		public string u_State;						//交易状态
		public string u_LState;					//交易单状态
		public string u_Price;						//产品的价格
		public string u_Carriage;					//物流费用
		public string u_PayNum;					//应支付的总价格
		public string u_Fact;						//实际支付费用
		public string u_Procedure;					//交易（服务）手续费
		public string u_Service;					//服务费率
		public string u_Cash;						//现金支付金额
		public string u_Create_Time_C2C;		//定单创建时间（c2c）
		public string u_Create_Time;			//定单创建时间（本地系统时间）
		public string u_Bargain_Time;			//买家付款时间(bank)
		public string u_Receive_Time_C2C;		//打款给卖家时间(c2c)
		public string u_Receive_Time;			//打款给卖家时间（本地）
		public string u_IP;						//最后修改交易单的IP
		public string u_Memo;					//交易说明
		public string u_Explain;				//备注（后台人员操作记录）
		public string u_Modify_Time;			//最后修改时间（c2c）/ 本地系统时间

		public string u_Pay_Time;
	}


	/// <summary>
	/// 用户帐户流水表的类形式
	/// </summary>
	public class T_BANKROLL_LIST
	{		
		public string u_BKID;					//流水的ID号
		public string u_ListID;					//交易单的ID号（C2C网站获得）
		public string u_SPID;					//机构代码名称（发起者）
		public string u_UID;					//用户的ID(QQ号码)买家
		public string u_True_Name;				//用户名称
		public string u_Type;						//交易类型
		public string u_Subject;					//类别(存取贷)
		public string u_FromID;					//对方的ID（QQ号码，银行帐号）
		public string u_From_Name;				//对方的名称
		public string u_Balance;					//帐户余额
		public string u_PayNum;					//金额
		public string u_Bank_Type;					//用户银行的类型
		public string u_CurType;					//币种代码
		public string u_Prove;					//记帐凭证
		public string u_Create_Time;			//创建时间（本地）
		public string u_ApplyID;				//应用系统的ID号
		public string u_IP;						//客户的IP地址
		public string u_Memo;					//备注/说明
		public string u_Modify_Time;			//最后修改时间

		public string u_Action_Type;
	}

	/// <summary>
	/// 用户交易流水表的类形式
	/// </summary>
	public class T_USERPAY_LIST
	{		
		public string u_Ude_ID;					//流水的ID号
		public string u_ListID;					//交易单的ID号（C2C网站获得）
		public string u_SPID;					//机构代码名称（发起者）
		public string u_QQID;					//用户的ID(QQ号码)买家
		public string u_True_Name;				//用户名称
		public string u_Type;						//交易类型
		public string u_Subject;					//类别(存取贷)
		public string u_FromID;					//对方的ID（QQ号码，银行帐号）
		public string u_From_Name;				//对方的名称
		public string u_Balance;					//帐户余额
		public string u_From_Balance;				//对方帐户的余额
		public string u_PayNum;					//金额
		public string u_Bank_Type;					//用户银行的类型
		public string u_CurType;					//币种代码
		public string u_Prove;					//记帐凭证
		public string u_Create_Time;			//创建时间（本地）
		public string u_ApplyID;				//应用系统的ID号
		public string u_IP;						//客户的IP地址
		public string u_Memo;					//备注/说明
		public string u_Modify_Time;			//最后修改时间
	}

	/// <summary>
	/// 腾讯银行帐户表的类形式(尚没有此表)
	/// </summary>
	public class T_TC_BANK
	{
		public string u_Bank_Name;				//银行名称/开户行名称
		public string u_Bank_Type;					//银行的类型
		public string u_TrueName;				//开户姓名
		public string u_Area;					//地区，省级
		public string u_City;					//城市
		public string u_CurType;				//币种代码
		public string u_Balance;					//帐户余额
		public string u_Yd_Balance;				//昨日帐户余额
		public string u_Create_Time;			//开户日期
		public string u_IP;						//最后修改的IP地址
		public string u_Memo;					//备注
		public string u_Modify_Time;			//注册时间/更新时间
	}


	/// <summary>
	/// 腾讯收款记录表的类形式
	/// </summary>
	public class T_TCBANKROLL_LIST
	{
		public string u_ListID;
		public string u_Tde_ID;				//流水的ID号
		public string u_SPID;				//机构代码名称（发起者）
		public string u_UID;				//腾讯帐户的ID号
		public string u_State;
		public string u_Pay_Front_Time;
		public string u_Pay_Time;
		public string u_Type;					//交易类型/业务种类
		public string u_Subject;				//类别(存取贷)
		public string u_Num;					//交易的金额
		public string u_Sign;				//交易标记
		public string u_Bank_List;			//银行定单号
		public string u_Bank_Acc;			//银行订单授权号/凭证号
		public string u_Bank_Type;				//银行的类型
		public string u_CurType;				//币种代码
		public string u_Aid;				//对方的ID（银行帐号）
		public string u_Prove;				//记帐凭证
		public string u_IP;					//客户的IP地址
		public string u_Memo;				//备注/说明
		public string u_Modify_Time;		//创建流水时间（本地）
	}

	/// <summary>
	/// 腾讯付款记录表的类形式
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
	/// 退款单表的类形式
	/// </summary>
	public class T_REFUND
	{
		public string u_ListID;
		public string u_RListID;				//退款单的ID号
		public string u_Create_Time;				
		public string u_SPID;				//机构代码名称（发起者）
		public string u_PayType;				//退款方式
		public string u_BuyID;				//买家帐户号码
		public string u_Buy_Name;			//买家名称
		public string u_Buy_Bank_Type;			//买家银行的类型
		public string u_Buy_BankID;			//买家的银行帐号
		public string u_SaleID;				//卖家帐户号码
		public string u_Sale_Name;			//卖家的名称
		public string u_Sale_Bank_Type;		//卖家银行的类型
		public string u_Sale_BankID;		//卖家的银行帐号
		public string u_State;					//退款的状态（接口）
		public string u_LState;				//退款单的状态
		public string u_PayBuy;				//退还给买家的金额
		public string u_PaySale;				//退还给卖家的金额
		public string u_Procedure;				//交易手续费
		public string u_Bargain_Time;		//C2C请求退款日期
		public string u_OK_Time;			//成交日期（退款日期）
		public string u_IP;					//最后修改的IP地址
		public string u_Memo;				//退款说明
		public string u_Explain;			//操作人员备注
		public string u_Modify_Time;		//最后修改时间
	}


	#region Furion 20050815 增加审批的配置。

	/// <summary>
	/// 审批类型表
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
//				if(IsNew) throw new LogicException("指定的ＩＤ已存在！");
//				else throw new LogicException("指定的ＩＤ不存在！");
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
				throw new LogicException("请输入审批ID和审批名称！");
				return false;
			}

			const int CHECKLENGTH = 30; //现在的审批配置长度是30
			if(LevelInfo[0].FCheck.Length > CHECKLENGTH )
			{
				throw new LogicException("第一个审批配置信息太长，超过了三十个字，请重新输入！");
			}

			if(LevelInfo[1].FCheck.Length > CHECKLENGTH )
			{
				throw new LogicException("第二个审批配置信息太长，超过了三十个字，请重新输入！");
			}

			if(LevelInfo[2].FCheck.Length > CHECKLENGTH )
			{
				throw new LogicException("第三个审批配置信息太长，超过了三十个字，请重新输入！");
			}

			if(LevelInfo[3].FCheck.Length > CHECKLENGTH )
			{
				throw new LogicException("第四个审批配置信息太长，超过了三十个字，请重新输入！");
			}

			string strSql = "select count(1) from c2c_fmdb.t_check_type where FTypeID='" + FTypeID + "'";
			string tmp = PublicRes.ExecuteOne(strSql,"HT");
			if(IsNew)
			{
				//新增
				return tmp == "0";
			}
			else
			{
				//修改
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
	/// 冻结信息。
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
        public string FFreezeChannel; //解冻渠道

		// 2012/4/4 新添加冻结信息的结束日期
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
				/*964243该帐户有2笔冻结单,so暂时改成读第一笔
				if(dt == null || dt.Rows.Count != 1)
				{
					throw new LogicException("查找不到指定的数据或数据重复！");
				}*/
				if(dt == null || dt.Rows.Count == 0)
				{
					throw new LogicException("查找不到指定的数据！");
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
			//用于冻结操作。
			MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("HT"));
			try
			{
				string strSql = "update c2c_fmdb.t_freeze_list set FHandleFinish = 9,FFreezeReason = '系统清除垃圾数据' where FFreezeID = '" + fi.FFreezeID + "' " +
					            "and FFreezeType = " + fi.FFreezeType + " and FHandleFinish = 1";   //清除垃圾数据

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
			//用于解冻操作
			MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("HT"));
			try
			{
				string strSql = "update c2c_fmdb.t_freeze_list set FHandleUserID='{0}',FHandleUserIP='{1}',FHandleTime=now(),"
                            + "FHandleResult='{2}',FHandleFinish=9 where FID={3}";

				strSql = String.Format(strSql,fh.UserName,fh.UserIP,fi.FHandleResult,fi.fid);

				da.OpenConn();
				da.ExecSql(strSql);

				strSql = "update c2c_fmdb.t_freeze_list set FHandleFinish = 9,FFreezeReason = '系统清除垃圾数据' where FFreezeID = '" + fi.FFreezeID + "' " +
					     "and FFreezeType = " + fi.FFreezeType + " and FHandleFinish = 1";   //清除垃圾数据

				da.ExecSql(strSql);
			}
			finally
			{
				da.Dispose();
			}
		}
	}
	#region 基类
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
		/// 从一条记录，获取成员变量值
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
		/// 从审批参数表获取成员变量值
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

    #region t_bankbulletin_info_all 表
    public class T_BANKBULLETIN_INFO_ALL : T_CLASS_BASIC
    {

        public bool IsNew = false; //是否为新增的通用汇总任务配置
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
                msg += "[银行类型]不能为空！";
            }
            if (Ftitle == null || Ftitle == "")
            {
                checkFlag = false;
                msg += "[公告标题]不能为空！";
            }
            if (Fmaintext == null || Fmaintext == "")
            {
                checkFlag = false;
                msg += "[公告正文]不能为空！";
            }

            if (Fcreateuser == null || Fcreateuser == "")
            {
                checkFlag = false;
                msg += "[创建人]不能为空！";
            }


            Fupdatetime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            if (!checkFlag)
            {
                throw new LogicException("t_bankbulletin_info_all配置，校验不通过：" + msg);
            }


            try
            {
                Fstartime = Convert.ToDateTime(Fstartime).ToString("yyyy-MM-dd HH:mm:ss");
            }
            catch (Exception ex)
            {
                throw new LogicException("t_bankbulletin_info_all配置，校验不通过：开始时间不满足条件");
            }
            try
            {
                Fendtime = Convert.ToDateTime(Fendtime).ToString("yyyy-MM-dd HH:mm:ss");
            }
            catch (Exception ex2)
            {
                throw new LogicException("t_bankbulletin_info_all配置，校验不通过：结束时间不满足条件");
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
                        throw new LogicException("t_bankbulletin_info_all配置，校验不通过！银行类型" + Fbanktype + "已经存在！");
                    }
                }
                else
                {
                    if (count != 1)
                    {
                        throw new LogicException("t_bankbulletin_info_all配置，校验不通过！银行类型" + Fbanktype + "不存在！");
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
                    throw new LogicException("t_bankbulletin_info_all配置，校验不通过！开始时间大于结束时间！");
                }

                //开始更新
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
                        throw new Exception("银行渠道维护公告接口bank_channel_bulletin_service返回失败：result=" + result + "，msg=" + msg);


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
                            throw new Exception("银行渠道维护公告接口bank_channel_bulletin_service返回失败：result=" + result + ",msg=" + msg + ",reply=" + reply);

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

    #region t_bankbulletin_type_all 表
    public class T_BANKBULLETIN_TYPE_ALL : T_CLASS_BASIC
    {
        public bool IsNew = false; //是否为新增的通用汇总任务配置
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
        public string Fbusinetype;//业务类型
 //       public string Falwaysworkstate;//关闭策略 0. 按时间段(默认)  1. 长期有效
        public string Isaffectinterface;//是否影响接口
      //  public string Falwtime;//有效时间段：长期有效时
        public string Ftitle;
        public string Ftctext;//弹层正文
        public string Fid;
    }
    #endregion

    #region t_bankbulletin_info 表
    public class T_BANKBULLETIN_INFO : T_CLASS_BASIC
    {
        public bool IsNew = false;
        public bool IsOPen = false;
        public string bulletin_id;//银行公告id
        public string banktype;
        public string businesstype;//业务类型
        public string op_support_flag;//业务子类型
        public string closetype;//是否影响接口
        public string title;
        public string maintext;
        public string popuptext;//弹层正文
        public string startime;
        public string endtime;
        public string createuser;
        public string updateuser;
        public string createtime;
        public string updatetime;
        public string returnUrl;
    }
    #endregion

    #region public_utility_charge.t_puc_new_service_info 表
    public class T_PUCNEWSERVICE_INFO : T_CLASS_BASIC
    {

        public bool IsNew = false; //是否为新增的通用汇总任务配置

        public string Fservicecode;
        public string Fuctype; //标识是哪种类的维护类型
        public string Ftips; //提示信息
        public string Fstartime; //对应种类维护类型的开始时间
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
                msg += "[提示信息]不能为空！";
            }


            Fupdatetime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            if (!checkFlag)
            {
                throw new LogicException("t_puc_new_service_info配置，校验不通过：" + msg);
            }


            try
            {
                Fstartime = Convert.ToDateTime(Fstartime).ToString("yyyy-MM-dd HH:mm:ss");
            }
            catch (Exception ex)
            {
                throw new LogicException("t_puc_new_service_info配置，校验不通过：开始时间不满足条件");
            }
            try
            {
                Fendtime = Convert.ToDateTime(Fendtime).ToString("yyyy-MM-dd HH:mm:ss");
            }
            catch (Exception ex2)
            {
                throw new LogicException("t_puc_new_service_info配置，校验不通过：结束时间不满足条件");
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
                        throw new LogicException("t_puc_new_service_info配置，校验不通过！服务类型" + Fservicecode + "已经存在！");
                    }
                }
                else
                {
                    if (count != 1)
                    {
                        throw new LogicException("t_puc_new_service_info配置，校验不通过！服务类型" + Fservicecode + "不存在！");
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
                    throw new LogicException("t_puc_new_service_info配置，校验不通过！开始时间大于结束时间！");
                }

                //开始更新
                dainc.OpenConn();
                if (IsNew)
                {

                    //不存在这种情况。
                }
                else
                {
                    //对uctype进行与操作。
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
                        //暂不支持。不用更新
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
