using SunLibraryEX;
using System;
using System.Collections;
using System.Data;

namespace TENCENT.OSS.C2C.Finance.Common.CommLib
{
	/// <summary>
	/// Class1 的摘要说明。
	/// </summary>
	public class commData
	{
		public commData()
		{
			//
			// TODO: 在此处添加构造函数逻辑
			//
		}

        #region 基类
        public class T_CLASS_BASIC
        {
            /// <summary>
            /// 从一条记录，获取成员变量值
            /// </summary>
            /// <param name="dr"></param>
            public void LoadFromDB(DataRow dr)
            {
                System.Reflection.FieldInfo fi;
                string col_name, col_type;
                System.Type this_type = this.GetType();
                for (int i = 0; i < dr.Table.Columns.Count; i++)
                {
                    col_name = dr.Table.Columns[i].ColumnName;
                    fi = this_type.GetField(col_name);
                    if (fi == null)
                        fi = this_type.GetField(col_name.ToLower());
                    if (fi != null)
                    {
                        col_type = dr.Table.Columns[i].DataType.FullName.ToUpper();
                        if (col_type == "System.Int32" || col_type == "System.Int16")
                            fi.SetValue(this, StringEx.GetInt(dr[col_name]));
                        else if (col_type == "System.DateTime")
                            fi.SetValue(this, StringEx.GetDateTime(dr[col_name]));
                        else
                            fi.SetValue(this, StringEx.GetString(dr[col_name]));
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
                string col_name, col_value;
                System.Type this_type = this.GetType();
                foreach (DataRow dr in dt.Rows)
                {
                    col_name = dr["FKey"].ToString().Trim();
                    fi = this_type.GetField(col_name);
                    if (fi == null)
                        fi = this_type.GetField(col_name.ToLower());
                    if (fi != null)
                    {
                        col_value = dr["FValue"].ToString().Trim();
                        if (fi.FieldType.FullName == "System.Boolean")
                            fi.SetValue(this, Convert.ToBoolean(col_value));
                        else
                            fi.SetValue(this, col_value);
                    }
                }
            }
        }
        #endregion

        #region t_bankbulletin_info 表
        public class T_BANKBULLETIN_INFO : T_CLASS_BASIC
        {
            public bool IsNew = false;
            public bool IsOPen = false;
            public string bulletin_id;//银行公告id
            public string banktype;
            public string banktype_str;
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
            public string op_flag;//操作标志:1-新增； 2-修改； 3-作废;若没有
            public string bull_type="1";//公告类型：1-例行维护；2-自动维护；
        }
        #endregion

        public static Hashtable AirCompany()
        {
            Hashtable ht = new Hashtable();
            ht.Add("szAir", "深圳航空_ZH");
            ht.Add("capitalAir", "首都航空_JD");
            ht.Add("southAir", "南方航空_CZ");
            ht.Add("eastAir", "东方航空_MU");
            ht.Add("hnAir", "海南航空_HU");
            ht.Add("ghAir", "中国国际_CA");
            ht.Add("scAir", "四川航空_3U");
            ht.Add("xmAir", "厦门航空_MF");
            ht.Add("hbAir", "河北航空_NS");
            ht.Add("sdAir", "山东航空_SD");

            return ht;
        }

        public static Hashtable PNRStatus()
        {
            Hashtable ht = new Hashtable();
          //  ht.Add("0", "0 成功");
            ht.Add("1", "1 成功");
            ht.Add("2", "2 未登录");
            ht.Add("3", "3 md5验证不通过");
            ht.Add("4", "4 IP受限");
            ht.Add("5", "5 系统错误");
            ht.Add("6", "6 PNR错误，已被删除或者不存在此PNR");
            ht.Add("7", "7 网页错误");
            ht.Add("8", "8 已经入库，不能重复入库");
            ht.Add("9", "9 不允许作为普通产品，需要先备注");
            ht.Add("10", "10 已出票");
            ht.Add("11", "11 深航支付 URL异常");
            ht.Add("13", "13 没有cookie");
            ht.Add("14", "14 DB中没有入库的订单号");
            ht.Add("15", "15 取航空公司票号出错");
            ht.Add("16", "16 PNR不存在");
            ht.Add("17", "17 非南航航班");
            ht.Add("18", "18 不允许导入");
            ht.Add("19", "19 没有票价");
            ht.Add("20", "20 自动支付失败");
            ht.Add("21", "21 查询票号时，无权限，可能不是该用户入的库");
            ht.Add("22", "22 入库成功，但获取支付信息失败");
            ht.Add("23", "23 RSA验证失败");
            ht.Add("24", "24 价格限制不满足，不能入库");
            ht.Add("25", "25 费率限制不满足，不能入库");
            ht.Add("26", "26 燃油税限制不满足，不能入库");
            ht.Add("27", "27 机场税限制不满足，不能入库");
            ht.Add("28", "28 连接超时");
            ht.Add("29", "29 数字证书验证失败");
            ht.Add("30", "30 没有申请自动支付权限");
            ht.Add("31", "31 取数字证书失败");
            ht.Add("32", "32 旅客证件姓名信息不符合航空公司入库要求");
            ht.Add("33", "33 其他错误,pnrsvr原样返回");
            ht.Add("34", "34 验证码破解失败");
            ht.Add("35", "35 价格异常");
            ht.Add("36", "36 b2b帐号不存在");
            ht.Add("38", "38 {PNR}已经入库并支付，不能再次入库,请核实。");
            ht.Add("39", "39 未查到PNR或者票号信息，请核实后重试");
            ht.Add("40", "40 代理人账号未授权，请联系财付通客服人员！");
            ht.Add("41", "41 RELAY网络错误");
            ht.Add("50", "50 异常请求");
            ht.Add("60", "60 IP限制");
            ht.Add("61", "61 用户无此权限");
            ht.Add("62", "62 您最近3次都输入了错误密码，账号已经被锁定，请30分钟后重试");
            ht.Add("63", "63 用户名或密码错误");
            ht.Add("64", "64 乘客个数超过该航司支持的乘客数");
            ht.Add("65", "65 航段个数超过该航司支持的航段数");
            ht.Add("66", "66 价格策略个数超过该航司支持的价格策略数");
            ht.Add("67", "67 乘客个数为0");
            ht.Add("68", "68 航段数为0");
            ht.Add("69", "69 价格策略个数为0");
            ht.Add("70", "70 接口不支持(受限商户)");
            ht.Add("71", "71 航空公司无此价格");
            ht.Add("72", "72 总支付金额不满足最高票价限制");
            ht.Add("73", "73 传入票价与实际票价不一致");
            ht.Add("74", "74 支付金额和实际需要支付金额不一致");
            ht.Add("75", "75 检查操作员与支付帐号绑定关系失败");
            ht.Add("76", "76 找不到对应的航空公司编码");
            ht.Add("77", "77 航空公司B2B网站处理超时,请稍后再试");
            ht.Add("78", "78 查询符合政策pnr信息失败");
            ht.Add("79", "79 航空公司支付链接（URL）异常");
            ht.Add("80", "80 查询代理人与平台关系绑定失败");
            ht.Add("81", "81 内部错误：保存pnr信息失败");
            ht.Add("91", "91 其它原因入库失败");
            ht.Add("92", "92 页面转码失败");
            ht.Add("99", "99 帐号余额不足，请先充值");

            return ht;
        }
	}

  //对帐异常，挂账表的的相关参数
	public struct hangAccData
	{
		public string fBatchID; //格式：200512121001  1001指银行类型
		public string fBatchNo;
		public string fSeqNo;
		public string fCheckType;
		public string fErrorFlag;
		public string fAdjustFlag;
		public string fInitFlag;
		public string fBankOrder;
		public string fBank_backid;
		public string famt;
		public string fBank_date;
		public string fBankType;
		public string fTde_id;
		public string fListid;
		public string fSubject;
		public string FState;
		public string FErrMsg;
		public string FPickUser;
		public string FPickTime;
		public string fflag;

		public string fauid;
		public string faid;
		public string faname;
		public string fnum;
		public string fsummary;
	}

	public struct tradeList
	{
		public string Fbank_listid;    //银行订单号                                             
		public string flistid;         //交易单ID                                               
		public string fspid;           //spid                                                   
		public string fbuy_uid;        //fbuy_uid                                               
		public string fbuyid;          //fbuyid                                                 
		public string fcurtype;        //fcurtype                                               
		public string fpaynum;         //fpaynum                                                
		public string fip;             //fip                                                    
		public string Fbuy_bank_type;  //fbuy_bank_type                                         
		public string fcreate_time;    //fcreate_time                                           
		public string fbuy_name;       //fbuy_name                                              
		public string fsaleid;         //fsaleid                                                
		public string fsale_uid;       //fsale_uid                                              
		public string fsale_name;      //fsale_name                                             
		public string fsale_bankid;    //fsale_bankid                                           
		public string Fpay_type;       //                                                       
		public string Fbuy_bankid;                                                              
		public string Fsale_bank_type;                                                          
		public string Fprice;                                                                   
		public string Fcarriage;                                                                
		public string Fprocedure;                                                               
		public string Fmemo;                                                                    
		public string Fcash;                                                                    
		public string Ftoken;                                                                   
		public string Ffee3;          //其它费用                                                   
		public string Fstate;         //交易的状态  Fstate，Fpay_time，Freceive_time，Fmodify_time 
		public string Fpay_time;      //买家付款时间（本地）                                       
		public string Freceive_time;  //打款给卖家时间                                           
		public string Fmodify_time;   //最后修改时间                                             
		public string Ftrade_type;    //交易类型 1 c2c 2 b2c 3 fastpay 4 转帐                     
		public string Flstate;        //交易单的状态                                              
		public string Fcoding;     
		public string Fbank_backid;   //银行返回的订单号  
		public string Ffact;          //支付总费用                                           
		public  string Fmedi_sign;
		public string Fgwq_listid;    
		public string Fchannel_id;
	}

	//购物券单的相关信息
	public struct tokenList
	{
		public string Fauto_id     ;
		public string Fticket_id   ;
		public string Ftde_id      ;
		public string Fson_id      ;
		public string Fuser_id     ;
		public string Fuser_uid    ;
		public string Fpub_id      ;
		public string Fpub_uid     ;
		public string Fpub_name    ;
		public string Fdonate_id   ;
		public string Fdonate_uid  ;
		public string Flistid      ;
		public string Fspid        ;
		public string Fatt_name    ;
		public string Fmer_id      ;
		public string Fdonate_type ;
		public string Fdonate_num  ;
		public string Ftype        ;
		public string Fpub_type    ;
		public string Fstate       ;
		public string Flist_state  ;
		public string Fadjust_flag ;
		public string Ffee         ;
		public string Fuse_pro     ;
		public string Fmin_fee     ;
		public string Fmax_num     ;
		public string Ffact_fee    ;
		public string Fstime       ;
		public string Fetime       ;
		public string Fpub_time    ;
		public string Fuse_time    ;
		public string Fdonate_time ;
		public string Furl         ;
		public string Fpub_user    ;
		public string Fuser_ip     ;
		public string Fpub_ip      ;
		public string Fstandby1    ;
		public string Fstandby2    ;
		public string Fstandby3    ;
		public string Fstandby4    ;
		public string Fmemo        ;
		public string Fmodify_time ;
		public string Fuse_listid  ;

	}

	/// <summary>
	/// 定义腾讯收款单/充值单
	/// </summary>
	public struct bkrlInfo
	{
		public string FlistID;
		public string Fspid;
		public string Fauid;
		public string Faid;
		public string Fcurtype;
		public string Fnum;
		public string FbankType;
		public string Fip;
		public string Fsubject; //会计科目
		public string FpayFrontTime; //付款前时间
		public string Fstate;
	}

	//帐务权限控制类型
	public enum accControl
	{
		user = 1,
		sp   = 2,
		role = 3
	}

	//购物券的状态
	public enum tokenState
	{
		/// <summary>
		/// 已发行
		/// </summary>
		published     = 1,

		/// <summary>
		/// 已激活授权
		/// </summary>
		activePrior   = 2,

		/// <summary>
		/// 已激活
		/// </summary>
        actived       = 3,

		/// <summary>
		/// 已使用
		/// </summary>
		beUsed        = 4,

		/// <summary>
		/// 已赠送
		/// </summary>
		sended        = 5,

		/// <summary>
		/// 已拒绝（激活）
		/// </summary>
		refuse        = 6,

		/// <summary>
		/// 确认使用
		/// </summary>
		comfirmUse    = 7
	}

	/// <summary>
	/// 商户帐户属性的基本信息
	/// </summary>
	public struct PropInfo
	{
		public string qqid  ;
		public string fuid  ;
		public string tname ; //模板名称
		public string roleid  ;
		public string roleName;
		public short  Fdebit ; //借贷方向
		public short  Fstate ; //属性状态：1 正常 2 作废
		public int    Fsign1 ; //状态标记1，核心交易类  
		public int    Fsign2 ; //状态标记2，其它交易类  
		public int    Fsign3 ; //状态标记3，帐户功能1   
		public int    Fsign4 ; //状态标记4，帐户功能2   
	}

	/// <summary>
	/// 统计的Item值
	/// </summary>
	public enum TjItem
	{
		Default= 0,
		save   = 205, //充值      
		fetch  = 213, //提现      
		gether = 206, //收款      
		payNum = 209, //付款      
		TcBuss = 210, //公司业务  
		PaiPai = 211, //拍拍业务  
		Other  = 212  //第三方业务
	}

	/// <summary>
	/// 统计项(收付款)
	/// </summary>
	public enum TjCommItem
	{
		/// <summary>
		/// 默认视图
		/// </summary>
		Default = 0,

		Day = 1,
		
		Week = 2,

		Month= 3
		
	}

	public enum commDataType
	{
		Default = 0,
		usermodify,
		manualsave,
		manualfetch,
		refound,
		thirdprofi,
		/// <summary>
		/// 快速交易退单
		/// </summary>
		fastPayRefound
	}
}
