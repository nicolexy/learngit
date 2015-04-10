using System;
using System.Configuration;
using TENCENT.OSS.CFT.KF.DataAccess;
using System.Data;
using System.Collections;
using System.Web.Mail;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.C2C.Finance.Common.CommLib;
using CommLib;

namespace TENCENT.OSS.CFT.KF.KF_Service
{
    [Obsolete("move to DAL&BLL")]
    public class PublicRes
    {
        public static bool isLatin = false;


        private static string f_strDatabase_bsb;
        private static string f_strDataSource_bsb;
        private static string f_strUserID_bsb;
        private static string f_strPassword_bsb;

        private static string f_strDatabase_bsb2;
        private static string f_strDataSource_bsb2;
        private static string f_strUserID_bsb2;
        private static string f_strPassword_bsb2;

        private static string f_strDatabase_bsb3;
        private static string f_strDataSource_bsb3;
        private static string f_strUserID_bsb3;
        private static string f_strPassword_bsb3;

        private static string f_strDatabase_zwNewTable;
        private static string f_strDataSource_zwNewTable;
        private static string f_strUserID_zwNewTable;
        private static string f_strPassword_zwNewTable;

        private static string f_strDatabase_zwOldTable;
        private static string f_strDataSource_zwOldTable;
        private static string f_strUserID_zwOldTable;
        private static string f_strPassword_zwOldTable;

        private static string f_strDatabase_bs;
        private static string f_strDataSource_bs;
        private static string f_strUserID_bs;
        private static string f_strPassword_bs;


        //SPOA数据库
        private static string f_strDatabase_spoa;
        private static string f_strDataSource_spoa;
        private static string f_strUserID_spoa;
        private static string f_strPassword_spoa;


        private static string f_strDatabase_cs;
        private static string f_strDataSource_cs;
        private static string f_strUserID_cs;
        private static string f_strPassword_cs;


        private static string f_strDatabase_jstl;
        private static string f_strDataSource_jstl;
        private static string f_strUserID_jstl;
        private static string f_strPassword_jstl;

        private static string f_strDatabase_jswechat;
        private static string f_strDataSource_jswechat;
        private static string f_strUserID_jswechat;
        private static string f_strPassword_jswechat;

        private static string f_strDatabase_js;
        private static string f_strDataSource_js;
        private static string f_strUserID_js;
        private static string f_strPassword_js;


        private static string f_strDatabase;
        private static string f_strDataSource;
        private static string f_strUserID;
        private static string f_strPassword;

        //业务库备机
        private static string f_strDatabase_ywb;
        private static string f_strDataSource_ywb;
        private static string f_strUserID_ywb;
        private static string f_strPassword_ywb;

        private static string f_strDatabase_zl;
        private static string f_strDataSource_zl;
        private static string f_strUserID_zl;
        private static string f_strPassword_zl;

        private static string f_strDatabase_zlb;
        private static string f_strDataSource_zlb;
        private static string f_strUserID_zlb;
        private static string f_strPassword_zlb;

        private static string f_strDatabase_ht;
        private static string f_strDataSource_ht;
        private static string f_strUserID_ht;
        private static string f_strPassword_ht;

        //cdkey
        private static string f_strDatabase_cdk;
        private static string f_strDataSource_cdk;
        private static string f_strUserID_cdk;
        private static string f_strPassword_cdk;

        //statistics
        private static string f_strDatabase_statistics;
        private static string f_strDataSource_statistics;
        private static string f_strUserID_statistics;
        private static string f_strPassword_statistics;

        //comprehensive
        private static string f_strDatabase_comprehensive;
        private static string f_strDataSource_comprehensive;
        private static string f_strUserID_comprehensive;
        private static string f_strPassword_comprehensive;

        //activity
        private static string f_strDatabase_activity;
        private static string f_strDataSource_activity;
        private static string f_strUserID_activity;
        private static string f_strPassword_activity;

        //wxzfact
        private static string f_strDatabase_wxzfact;
        private static string f_strDataSource_wxzfact;
        private static string f_strUserID_wxzfact;
        private static string f_strPassword_wxzfact;

        //fund
        private static string f_strDatabase_fund;
        private static string f_strDataSource_fund;
        private static string f_strUserID_fund;
        private static string f_strPassword_fund;

        //bank_list_his
        private static string f_strDatabase_banklisthis;
        private static string f_strDataSource_banklisthis;
        private static string f_strUserID_banklisthis;
        private static string f_strPassword_banklisthis;

        private static string f_strDatabase_fcpay;
        private static string f_strDataSource_fcpay;
        private static string f_strUserID_fcpay;
        private static string f_strPassword_fcpay;

        //财付券数据库成员变量
        private static string f_strDatabase_gwq;
        private static string f_strDataSource_gwq;
        private static string f_strUserID_gwq;
        private static string f_strPassword_gwq;


        //权限系统数据库 au
        private static string f_strDatabase_au;
        private static string f_strDataSource_au;
        private static string f_strUserID_au;
        private static string f_strPassword_au;


        private static string f_strDatabase_cft;
        private static string f_strDataSource_cft;
        private static string f_strUserID_cft;
        private static string f_strPassword_cft;

        private static string f_strDatabase_cftb;
        private static string f_strDataSource_cftb;
        private static string f_strUserID_cftb;
        private static string f_strPassword_cftb;

        private static string f_strDatabase_fkdj;
        private static string f_strDataSource_fkdj;
        private static string f_strUserID_fkdj;
        private static string f_strPassword_fkdj;

        //账务主库
        private static string f_strDatabase_zw;
        private static string f_strDataSource_zw;
        private static string f_strUserID_zw;
        private static string f_strPassword_zw;

        //账务付款
        private static string f_strDatabase_zwfk;
        private static string f_strDataSource_zwfk;
        private static string f_strUserID_zwfk;
        private static string f_strPassword_zwfk;

        //账务收款
        private static string f_strDatabase_zwsk;
        private static string f_strDataSource_zwsk;
        private static string f_strUserID_zwsk;
        private static string f_strPassword_zwsk;

        //账务退款
        private static string f_strDatabase_zwtk;
        private static string f_strDataSource_zwtk;
        private static string f_strUserID_zwtk;
        private static string f_strPassword_zwtk;

        //中介数据库
        private static string f_strDatabase_zj;
        private static string f_strDataSource_zj;
        private static string f_strUserID_zj;
        private static string f_strPassword_zj;

        //实名认证数据库
        private static string f_strDatabase_ru;
        private static string f_strDataSource_ru;
        private static string f_strUserID_ru;
        private static string f_strPassword_ru;

        //证书数据库
        private static string f_strDatabase_crt;
        private static string f_strDataSource_crt;
        private static string f_strUserID_crt;
        private static string f_strPassword_crt;

        private static string f_strDatabase_zjb;
        private static string f_strDataSource_zjb;
        private static string f_strUserID_zjb;
        private static string f_strPassword_zjb;


        private static string f_strDatabase_inc;
        private static string f_strDataSource_inc;
        private static string f_strUserID_inc;
        private static string f_strPassword_inc;

        private static string f_strDatabase_bankbulletin;
        private static string f_strDataSource_bankbulletin;
        private static string f_strUserID_bankbulletin;
        private static string f_strPassword_bankbulletin;


        private static string f_strDatabase_inc_new;
        private static string f_strDataSource_inc_new;
        private static string f_strUserID_inc_new;
        private static string f_strPassword_inc_new;

        private static string f_strDatabase_incb;
        private static string f_strDataSource_incb;
        private static string f_strUserID_incb;
        private static string f_strPassword_incb;

        private static string f_strDatabase_incb_new;
        private static string f_strDataSource_incb_new;
        private static string f_strUserID_incb_new;
        private static string f_strPassword_incb_new;

        //杂类数据库
        private static string f_strDatabase_bd;
        private static string f_strDataSource_bd;
        private static string f_strUserID_bd;
        private static string f_strPassword_bd;

        //银行订单查询数据库
        private static string f_strDatabase_dd;
        private static string f_strDataSource_dd;
        private static string f_strUserID_dd;
        private static string f_strPassword_dd;

        private static string f_strDatabase_syninfo;
        private static string f_strDataSource_syninfo;
        private static string f_strUserID_syninfo;
        private static string f_strPassword_syninfo;

        private static string f_strDatabase_user;
        private static string f_strDataSource_user;
        private static string f_strUserID_user;
        private static string f_strPassword_user;

        private static string f_strDatabase_brl;
        private static string f_strDataSource_brl;
        private static string f_strUserID_brl;
        private static string f_strPassword_brl;

        private static string f_strDatabase_tcp;
        private static string f_strDataSource_tcp;
        private static string f_strUserID_tcp;
        private static string f_strPassword_tcp;

        private static string f_strDatabase_order;
        private static string f_strDataSource_order;
        private static string f_strUserID_order;
        private static string f_strPassword_order;

        private static string f_strDatabase_mn;
        private static string f_strDataSource_mn;
        private static string f_strUserID_mn;
        private static string f_strPassword_mn;

        private static string f_strDatabase_rel;
        private static string f_strDataSource_rel;
        private static string f_strUserID_rel;
        private static string f_strPassword_rel;

        private static string f_strDatabase_ap;
        private static string f_strDataSource_ap;
        private static string f_strUserID_ap;
        private static string f_strPassword_ap;

        private static string f_strDatabase_hd;
        private static string f_strDataSource_hd;
        private static string f_strUserID_hd;
        private static string f_strPassword_hd;

        private static string f_strDatabase_uk;
        private static string f_strDataSource_uk;
        private static string f_strUserID_uk;
        private static string f_strPassword_uk;

        //生活缴费
        private static string f_strDatabase_uc;
        private static string f_strDataSource_uc;
        private static string f_strUserID_uc;
        private static string f_strPassword_uc;

        //申诉分库分表
        private static string f_strDatabase_cftnew;
        private static string f_strDataSource_cftnew;
        private static string f_strUserID_cftnew;
        private static string f_strPassword_cftnew;

        //IVR分库分表
        private static string f_strDatabase_ivrnew;
        private static string f_strDataSource_ivrnew;
        private static string f_strUserID_ivrnew;
        private static string f_strPassword_ivrnew;

        // 2012/4/9 邮政汇款数据库相关资料
        private static string f_strDataSource_remit;
        private static string f_strUserID_remit;
        private static string f_strPassword_remit;
        private static string f_strDatabase_remit;

        //微信AA收款
        private static string f_strDataSource_wxaa;
        private static string f_strUserID_wxaa;
        private static string f_strPassword_wxaa;
        private static string f_strDatabase_wxaa;


        //微信红包
        private static string f_strDataSource_wxhongbao;
        private static string f_strUserID_wxhongbao;
        private static string f_strPassword_wxhongbao;
        private static string f_strDatabase_wxhongbao;

        //微信小额刷卡
        private static string f_strDataSource_wxxesk;
        private static string f_strUserID_wxxesk;
        private static string f_strPassword_wxxesk;
        private static string f_strDatabase_wxxesk;
        

        //账户属性
        private static string f_strDataSource_account;
        private static string f_strUserID_account;
        private static string f_strPassword_account;
        private static string f_strDatabase_account;

        public static string f_strServerIP;
        public static int f_iServerPort;

        public static string ICEServerIP3;
        public static int ICEPort3;

        public static int GROUPID;

        public static Finance_Header fh = null;  //furion 20050806 为了调用函数少个参数,任何SERVICE在检验SOAP头后就把头传到这个参数.

        public static string CharSet = "latin1";
        public static bool IgnoreLimitCheck = ConfigurationManager.AppSettings["IgnoreLimitCheck"].Trim().ToLower() == "true";

        //远程日志服务器
        private static string RemoteLogSeverIP;
        private static int RemoteLogServerPort;

        //清空cache服务器
        private static string ReleaseCacheServerIP;
        private static int ReleaseCacheServerPort;


        //furion 20060221
        public static string ICEServerIP;
        public static int ICEPort;

        public static string ICEServerIPSub;
        public static int ICEPortSub;

        //krolalu 8012-8-13 话费发货查询库
        public static string f_strDatabase_mobile;
        public static string f_strDataSource_mobile;
        public static string f_strUserID_mobile;
        public static string f_strPassWord_mobile;

        //wenzou 2012-09-14 财付通会员查询
        public static string f_strDatabase_QueryMember0;
        public static string f_strDataSource_QueryMember0;
        public static string f_strUserID_QueryMember0;
        public static string f_strPassWord_QueryMember0;
        public static string f_strDatabase_QueryMember1;
        public static string f_strDataSource_QueryMember1;
        public static string f_strUserID_QueryMember1;
        public static string f_strPassWord_QueryMember1;
        public static string f_strDatabase_QueryMember2;
        public static string f_strDataSource_QueryMember2;
        public static string f_strUserID_QueryMember2;
        public static string f_strPassWord_QueryMember2;
        //wenzou 2012-11-1 财付值流水查询
        public static string f_strDatabase_PropertyTurnover;
        public static string f_strDataSource_PropertyTurnover;
        public static string f_strUserID_PropertyTurnover;
        public static string f_strPassWord_PropertyTurnover;

        //wenzou 2012-09-19 财付通会员银行卡绑定(爱购卡)
        public static string f_strDatabase_CardBind;
        public static string f_strDataSource_CardBind;
        public static string f_strUserID_CardBind;
        public static string f_strPassWord_CardBind;

        //lxl 2013-09-18 财付通会员图标管理查询
        public static string f_strDatabase_IconInfo1;
        public static string f_strDataSource_IconInfo1;
        public static string f_strUserID_IconInfo1;
        public static string f_strPassWord_IconInfo1;

        public static string f_strDatabase_IconInfo2;
        public static string f_strDataSource_IconInfo2;
        public static string f_strUserID_IconInfo2;
        public static string f_strPassWord_IconInfo2;

        public static string f_strDatabase_IconInfo3;
        public static string f_strDataSource_IconInfo3;
        public static string f_strUserID_IconInfo3;
        public static string f_strPassWord_IconInfo3;

        //wenzou 2012-09-26 外卡支付数据库
        public static string f_strDatabase_ForeignCard;
        public static string f_strDataSource_ForeignCard;
        public static string f_strUserID_ForeignCard;
        public static string f_strPassWord_ForeignCard;

        //wenzou2012-10-16 手机充值卡业务 
        public static string f_strDatabase_MobileRecharge;
        public static string f_strDataSource_MobileRecharge;
        public static string f_strUserID_MobileRecharge;
        public static string f_strPassWord_MobileRecharge;

        //wenzou2013-03-19 手机绑定财付通账号
        public static string f_strDatabase_MobileBind;
        public static string f_strDataSource_MobileBind;
        public static string f_strUserID_MobileBind;
        public static string f_strPassWord_MobileBind;

        //wenzou2013-03-29 网银相关查询        
        public static string f_strDataSource_InternetBank;
        public static string f_strUserID_InternetBank;
        public static string f_strPassWord_InternetBank;
        public static string f_strDatabase_InternetBank;

        //手机令牌数据库
        private static string f_strDatabase_mt;
        private static string f_strDataSource_mt;
        private static string f_strUserID_mt;
        private static string f_strPassword_mt;
        static PublicRes()
        {
            f_strDatabase_bsb = "mysql";
            f_strDataSource_bsb = ConfigurationManager.AppSettings["DataSource_BSB"];
            f_strUserID_bsb = ConfigurationManager.AppSettings["UserID_BSB"];
            f_strPassword_bsb = ConfigurationManager.AppSettings["Password_BSB"];

            f_strDatabase_bsb2 = "mysql";
            f_strDataSource_bsb2 = ConfigurationManager.AppSettings["DataSource_BSB2"];
            f_strUserID_bsb2 = ConfigurationManager.AppSettings["UserID_BSB2"];
            f_strPassword_bsb2 = ConfigurationManager.AppSettings["Password_BSB2"];

            f_strDatabase_bsb3 = "mysql";
            f_strDataSource_bsb3 = ConfigurationManager.AppSettings["DataSource_BSB3"];
            f_strUserID_bsb3 = ConfigurationManager.AppSettings["UserID_BSB3"];
            f_strPassword_bsb3 = ConfigurationManager.AppSettings["Password_BSB3"];

            f_strDatabase_zwNewTable = "mysql";
            f_strDataSource_zwNewTable = ConfigurationManager.AppSettings["DataSource_ZWNEWTABLE"];
            f_strUserID_zwNewTable = ConfigurationManager.AppSettings["UserID_ZWNEWTABLE"];
            f_strPassword_zwNewTable = ConfigurationManager.AppSettings["Password_ZWNEWTABLE"];

            f_strDatabase_zwOldTable = "mysql";
            f_strDataSource_zwOldTable = ConfigurationManager.AppSettings["DataSource_ZWOLDTABLE"];
            f_strUserID_zwOldTable = ConfigurationManager.AppSettings["UserID_ZWOLDTABLE"];
            f_strPassword_zwOldTable = ConfigurationManager.AppSettings["Password_ZWOLDTABLE"];


            f_strDatabase_bs = "mysql";
            f_strDataSource_bs = ConfigurationManager.AppSettings["DataSource_BS"];
            f_strUserID_bs = ConfigurationManager.AppSettings["UserID_BS"];
            f_strPassword_bs = ConfigurationManager.AppSettings["Password_BS"];


            f_strDataSource_spoa = ConfigurationManager.AppSettings["DataSource_SPOA"];
            f_strDatabase_spoa = ConfigurationManager.AppSettings["DataBase_SPOA"];
            f_strUserID_spoa = ConfigurationManager.AppSettings["UserID_SPOA"];
            f_strPassword_spoa = ConfigurationManager.AppSettings["Password_SPOA"];


            f_strDataSource_cs = ConfigurationManager.AppSettings["DataSource_CS"];
            f_strDatabase_cs = ConfigurationManager.AppSettings["DataBase_CS"];
            f_strUserID_cs = ConfigurationManager.AppSettings["UserID_CS"];
            f_strPassword_cs = ConfigurationManager.AppSettings["Password_CS"];


            f_strDatabase_gwq = "mysql";
            f_strDataSource_jstl = ConfigurationManager.AppSettings["DataSource_JSTL"];
            f_strUserID_jstl = ConfigurationManager.AppSettings["UserID_JSTL"];
            f_strPassword_jstl = ConfigurationManager.AppSettings["Password_JSTL"];

            f_strDatabase_jswechat = "mysql";
            f_strDataSource_jswechat = ConfigurationManager.AppSettings["DataSource_JSWECHAT"];
            f_strUserID_jswechat = ConfigurationManager.AppSettings["UserID_JSWECHAT"];
            f_strPassword_jswechat = ConfigurationManager.AppSettings["Password_JSWECHAT"];


            f_strDataSource_js = ConfigurationManager.AppSettings["DataSource_JS"];
            f_strDatabase_js = ConfigurationManager.AppSettings["DataBase_JS"];
            f_strUserID_js = ConfigurationManager.AppSettings["UserID_JS"];
            f_strPassword_js = ConfigurationManager.AppSettings["Password_JS"];


            f_strDatabase = "mysql";
            f_strDataSource = ConfigurationManager.AppSettings["DataSource"];
            f_strUserID = ConfigurationManager.AppSettings["UserID"];
            f_strPassword = ConfigurationManager.AppSettings["Password"];


            f_strDatabase_zl = "mysql";
            f_strDataSource_zl = ConfigurationManager.AppSettings["DataSource_ZL"];
            f_strUserID_zl = ConfigurationManager.AppSettings["UserID_ZL"];
            f_strPassword_zl = ConfigurationManager.AppSettings["Password_ZL"];

            f_strDatabase_zlb = "mysql";
            f_strDataSource_zlb = ConfigurationManager.AppSettings["DataSource_ZLB"];
            f_strUserID_zlb = ConfigurationManager.AppSettings["UserID_ZLB"];
            f_strPassword_zlb = ConfigurationManager.AppSettings["Password_ZLB"];

            f_strDatabase_ywb = "mysql";
            f_strDataSource_ywb = ConfigurationManager.AppSettings["DataSource_YWB"];
            f_strUserID_ywb = ConfigurationManager.AppSettings["UserID_YWB"];
            f_strPassword_ywb = ConfigurationManager.AppSettings["Password_YWB"];

            f_strDatabase_ht = "mysql";
            f_strDataSource_ht = ConfigurationManager.AppSettings["DataSource_ht"];
            f_strUserID_ht = ConfigurationManager.AppSettings["UserID_ht"];
            f_strPassword_ht = ConfigurationManager.AppSettings["Password_ht"];

            f_strDatabase_cdk = "mysql";
            f_strDataSource_cdk = ConfigurationManager.AppSettings["DataSource_cdk"];
            f_strUserID_cdk = ConfigurationManager.AppSettings["UserID_cdk"];
            f_strPassword_cdk = ConfigurationManager.AppSettings["Password_cdk"];

            f_strDatabase_statistics = "mysql";
            f_strDataSource_statistics = ConfigurationManager.AppSettings["DataSource_statistics"];
            f_strUserID_statistics = ConfigurationManager.AppSettings["UserID_statistics"];
            f_strPassword_statistics = ConfigurationManager.AppSettings["Password_statistics"];

            f_strDatabase_comprehensive = "mysql";
            f_strDataSource_comprehensive = ConfigurationManager.AppSettings["DataSource_comprehensive"];
            f_strUserID_comprehensive = ConfigurationManager.AppSettings["UserID_comprehensive"];
            f_strPassword_comprehensive = ConfigurationManager.AppSettings["Password_comprehensive"];

            f_strDatabase_activity = "mysql";
            f_strDataSource_activity = ConfigurationManager.AppSettings["DataSource_activity"];
            f_strUserID_activity = ConfigurationManager.AppSettings["UserID_activity"];
            f_strPassword_activity = ConfigurationManager.AppSettings["Password_activity"];

            f_strDatabase_wxzfact = "mysql";
            f_strDataSource_wxzfact = ConfigurationManager.AppSettings["DataSource_wxzfact"];
            f_strUserID_wxzfact = ConfigurationManager.AppSettings["UserID_wxzfact"];
            f_strPassword_wxzfact = ConfigurationManager.AppSettings["Password_wxzfact"];

            f_strDatabase_fund = "mysql";
            f_strDataSource_fund = ConfigurationManager.AppSettings["DataSource_fund"];
            f_strUserID_fund = ConfigurationManager.AppSettings["UserID_fund"];
            f_strPassword_fund = ConfigurationManager.AppSettings["Password_fund"];

            f_strDatabase_banklisthis = "mysql";
            f_strDataSource_banklisthis = ConfigurationManager.AppSettings["DataSource_BANKLISTHIS"];
            f_strUserID_banklisthis = ConfigurationManager.AppSettings["UserID_BANKLISTHIS"];
            f_strPassword_banklisthis = ConfigurationManager.AppSettings["Password_BANKLISTHIS"];

            f_strDatabase_fcpay = "mysql";
            f_strDataSource_fcpay = ConfigurationManager.AppSettings["DataSource_FCPAY"];
            f_strUserID_fcpay = ConfigurationManager.AppSettings["UserID_FCPAY"];
            f_strPassword_fcpay = ConfigurationManager.AppSettings["Password_FCPAY"];

            //构造函数中初始化数据库
            f_strDatabase_gwq = "mysql";
            f_strDataSource_gwq = ConfigurationManager.AppSettings["DataSource_gwq"];
            f_strUserID_gwq = ConfigurationManager.AppSettings["UserID_gwq"];
            f_strPassword_gwq = ConfigurationManager.AppSettings["Password_gwq"];

            f_strDatabase_au = "mysql";
            f_strDataSource_au = ConfigurationManager.AppSettings["DataSource_au"];
            f_strUserID_au = ConfigurationManager.AppSettings["UserID_au"];
            f_strPassword_au = ConfigurationManager.AppSettings["Password_au"];

            f_strDatabase_zw = "mysql";
            f_strDataSource_zw = ConfigurationManager.AppSettings["DataSource_zw"];
            f_strUserID_zw = ConfigurationManager.AppSettings["UserID_zw"];
            f_strPassword_zw = ConfigurationManager.AppSettings["Password_zw"];
            //付款
            f_strDatabase_zwfk = "mysql";
            f_strDataSource_zwfk = ConfigurationManager.AppSettings["DataSource_zwfk"];
            f_strUserID_zwfk = ConfigurationManager.AppSettings["UserID_zwfk"];
            f_strPassword_zwfk = ConfigurationManager.AppSettings["Password_zwfk"];
            //收款
            f_strDatabase_zwsk = "mysql";
            f_strDataSource_zwsk = ConfigurationManager.AppSettings["DataSource_zwsk"];
            f_strUserID_zwsk = ConfigurationManager.AppSettings["UserID_zwsk"];
            f_strPassword_zwsk = ConfigurationManager.AppSettings["Password_zwsk"];
            //退款
            f_strDatabase_zwtk = "mysql";
            f_strDataSource_zwtk = ConfigurationManager.AppSettings["DataSource_zwtk"];
            f_strUserID_zwtk = ConfigurationManager.AppSettings["UserID_zwtk"];
            f_strPassword_zwtk = ConfigurationManager.AppSettings["Password_zwtk"];

            f_strDatabase_zj = "mysql";
            f_strDataSource_zj = ConfigurationManager.AppSettings["DataSource_ZJ"];
            f_strUserID_zj = ConfigurationManager.AppSettings["UserID_ZJ"];
            f_strPassword_zj = ConfigurationManager.AppSettings["Password_ZJ"];

            f_strDatabase_crt = "mysql";
            f_strDataSource_crt = ConfigurationManager.AppSettings["DataSource_CRT"];
            f_strUserID_crt = ConfigurationManager.AppSettings["UserID_CRT"];
            f_strPassword_crt = ConfigurationManager.AppSettings["Password_CRT"];


            f_strDatabase_remit = "mysql";
            f_strDataSource_remit = ConfigurationManager.AppSettings["DataSource_REMIT"];
            f_strUserID_remit = ConfigurationManager.AppSettings["UserID_REMIT"];
            f_strPassword_remit = ConfigurationManager.AppSettings["Password_REMIT"];

            f_strServerIP = ConfigurationManager.AppSettings["ServerIP"];
            f_iServerPort = Int32.Parse(ConfigurationManager.AppSettings["ServerPort"]);

            ICEServerIP3 = ConfigurationManager.AppSettings["ICEServerIP3"];
            ICEPort3 = Int32.Parse(ConfigurationManager.AppSettings["ICEPort3"]);

            GROUPID = Int32.Parse(ConfigurationManager.AppSettings["GROUPID"]);

            CharSet = ConfigurationManager.AppSettings["CharSet"];

            RemoteLogSeverIP = ConfigurationManager.AppSettings["LogServer"];
            RemoteLogServerPort = Int32.Parse(ConfigurationManager.AppSettings["LogPort"]);

            ReleaseCacheServerIP = ConfigurationManager.AppSettings["CacheServer"];
            ReleaseCacheServerPort = Int32.Parse(ConfigurationManager.AppSettings["CachePort"]);

            f_strDatabase_cft = ConfigurationManager.AppSettings["DB_CFT"];
            f_strDataSource_cft = ConfigurationManager.AppSettings["DataSource_CFT"];
            f_strUserID_cft = ConfigurationManager.AppSettings["UserID_CFT"];
            f_strPassword_cft = ConfigurationManager.AppSettings["Password_CFT"];

            f_strDatabase_cftb = ConfigurationManager.AppSettings["DB_CFTB"];
            f_strDataSource_cftb = ConfigurationManager.AppSettings["DataSource_CFTB"];
            f_strUserID_cftb = ConfigurationManager.AppSettings["UserID_CFTB"];
            f_strPassword_cftb = ConfigurationManager.AppSettings["Password_CFTB"];

            f_strDatabase_fkdj = "mysql";
            f_strDataSource_fkdj = ConfigurationManager.AppSettings["DataSource_fkdj"];
            f_strUserID_fkdj = ConfigurationManager.AppSettings["UserID_fkdj"];
            f_strPassword_fkdj = ConfigurationManager.AppSettings["Password_fkdj"];

            f_strDatabase_ru = "mysql";
            f_strDataSource_ru = ConfigurationManager.AppSettings["DataSource_RU"];
            f_strUserID_ru = ConfigurationManager.AppSettings["UserID_RU"];
            f_strPassword_ru = ConfigurationManager.AppSettings["Password_RU"];

            f_strDatabase_zjb = "mysql";
            f_strDataSource_zjb = ConfigurationManager.AppSettings["DataSource_ZJB"];
            f_strUserID_zjb = ConfigurationManager.AppSettings["UserID_ZJB"];
            f_strPassword_zjb = ConfigurationManager.AppSettings["Password_ZJB"];

            f_strDatabase_inc = "mysql";
            f_strDataSource_inc = ConfigurationManager.AppSettings["DataSource_INC"];
            f_strUserID_inc = ConfigurationManager.AppSettings["UserID_INC"];
            f_strPassword_inc = ConfigurationManager.AppSettings["Password_INC"];

            f_strDatabase_bankbulletin = "mysql";
            f_strDataSource_bankbulletin = ConfigurationManager.AppSettings["DataSource_BANKBULLETIN"];
            f_strUserID_bankbulletin = ConfigurationManager.AppSettings["UserID_BANKBULLETIN"];
            f_strPassword_bankbulletin = ConfigurationManager.AppSettings["Password_BANKBULLETIN"];


            f_strDatabase_inc_new = "mysql"; ;
            f_strDataSource_inc_new = ConfigurationManager.AppSettings["DataSource_INC_NEW"]; ;
            f_strUserID_inc_new = ConfigurationManager.AppSettings["UserID_INC_NEW"];
            f_strPassword_inc_new = ConfigurationManager.AppSettings["Password_INC_NEW"];

            f_strDatabase_incb = "mysql";
            f_strDataSource_incb = ConfigurationManager.AppSettings["DataSource_INCB"];
            f_strUserID_incb = ConfigurationManager.AppSettings["UserID_INCB"];
            f_strPassword_incb = ConfigurationManager.AppSettings["Password_INCB"];

            f_strDatabase_incb_new = "mysql";
            f_strDataSource_incb_new = ConfigurationManager.AppSettings["DataSource_INCB_NEW"];
            f_strUserID_incb_new = ConfigurationManager.AppSettings["UserID_INCB_NEW"];
            f_strPassword_incb_new = ConfigurationManager.AppSettings["Password_INCB_NEW"];

            f_strDatabase_bd = "mysql";
            f_strDataSource_bd = ConfigurationManager.AppSettings["DataSource_BD"];
            f_strUserID_bd = ConfigurationManager.AppSettings["UserID_BD"];
            f_strPassword_bd = ConfigurationManager.AppSettings["Password_BD"];

            f_strDatabase_dd = "mysql";
            f_strDataSource_dd = ConfigurationManager.AppSettings["DataSource_DD"];
            f_strUserID_dd = ConfigurationManager.AppSettings["UserID_DD"];
            f_strPassword_dd = ConfigurationManager.AppSettings["Password_DD"];

            f_strDatabase_uc = "mysql";
            f_strDataSource_uc = ConfigurationSettings.AppSettings["DataSource_UC"];
            f_strUserID_uc = ConfigurationSettings.AppSettings["UserID_UC"];
            f_strPassword_uc = ConfigurationSettings.AppSettings["Password_UC"];

            f_strDatabase_cftnew = "mysql";
            f_strDataSource_cftnew = ConfigurationSettings.AppSettings["DataSource_CFTNEW"];
            f_strUserID_cftnew = ConfigurationSettings.AppSettings["UserID_CFTNEW"];
            f_strPassword_cftnew = ConfigurationSettings.AppSettings["Password_CFTNEW"];

            f_strDatabase_ivrnew = "mysql";
            f_strDataSource_ivrnew = ConfigurationSettings.AppSettings["DataSource_IVRNEW"];
            f_strUserID_ivrnew = ConfigurationSettings.AppSettings["UserID_IVRNEW"];
            f_strPassword_ivrnew = ConfigurationSettings.AppSettings["Password_IVRNEW"];

            f_strDatabase_syninfo = "mysql";
            f_strDataSource_syninfo = ConfigurationManager.AppSettings["DataSource_SYNINFO"];
            f_strUserID_syninfo = ConfigurationManager.AppSettings["UserID_SYNINFO"];
            f_strPassword_syninfo = ConfigurationManager.AppSettings["Password_SYNINFO"];

            f_strDatabase_user = "mysql";
            f_strDataSource_user = ConfigurationManager.AppSettings["DataSource_USER"];
            f_strUserID_user = ConfigurationManager.AppSettings["UserID_USER"];
            f_strPassword_user = ConfigurationManager.AppSettings["Password_USER"];

            f_strDatabase_brl = "mysql";
            f_strDataSource_brl = ConfigurationManager.AppSettings["DataSource_BRL"];
            f_strUserID_brl = ConfigurationManager.AppSettings["UserID_BRL"];
            f_strPassword_brl = ConfigurationManager.AppSettings["Password_BRL"];

            f_strDatabase_tcp = "mysql";
            f_strDataSource_tcp = ConfigurationManager.AppSettings["DataSource_TCP"];
            f_strUserID_tcp = ConfigurationManager.AppSettings["UserID_TCP"];
            f_strPassword_tcp = ConfigurationManager.AppSettings["Password_TCP"];

            f_strDatabase_order = "mysql";
            f_strDataSource_order = ConfigurationManager.AppSettings["DataSource_ORDER"];
            f_strUserID_order = ConfigurationManager.AppSettings["UserID_ORDER"];
            f_strPassword_order = ConfigurationManager.AppSettings["Password_ORDER"];

            f_strDatabase_mn = "mysql";
            f_strDataSource_mn = ConfigurationManager.AppSettings["DataSource_MN"];
            f_strUserID_mn = ConfigurationManager.AppSettings["UserID_MN"];
            f_strPassword_mn = ConfigurationManager.AppSettings["Password_MN"];

            f_strDatabase_rel = "mysql";
            f_strDataSource_rel = ConfigurationManager.AppSettings["DataSource_REL"];
            f_strUserID_rel = ConfigurationManager.AppSettings["UserID_REL"];
            f_strPassword_rel = ConfigurationManager.AppSettings["Password_REL"];

            f_strDatabase_ap = "mysql";
            f_strDataSource_ap = ConfigurationManager.AppSettings["DataSource_AP"];
            f_strUserID_ap = ConfigurationManager.AppSettings["UserID_AP"];
            f_strPassword_ap = ConfigurationManager.AppSettings["Password_AP"];

            f_strDatabase_hd = "mysql";
            f_strDataSource_hd = ConfigurationManager.AppSettings["DataSource_HD"];
            f_strUserID_hd = ConfigurationManager.AppSettings["UserID_HD"];
            f_strPassword_hd = ConfigurationManager.AppSettings["Password_HD"];

            f_strDatabase_uk = "mysql";
            f_strDataSource_uk = ConfigurationManager.AppSettings["DataSource_UK"];
            f_strUserID_uk = ConfigurationManager.AppSettings["UserID_UK"];
            f_strPassword_uk = ConfigurationManager.AppSettings["Password_UK"];

            f_strDatabase_mobile = "mysql";
            f_strDataSource_mobile = ConfigurationManager.AppSettings["DataSource_MOBILE"].ToString();
            f_strUserID_mobile = ConfigurationManager.AppSettings["UserID_MOBILE"].ToString();
            f_strPassWord_mobile = ConfigurationManager.AppSettings["Password_MOBILE"].ToString();

            f_strDatabase_QueryMember0 = "mysql";
            f_strDataSource_QueryMember0 = ConfigurationManager.AppSettings["DataSource_QueryMember0"];
            f_strUserID_QueryMember0 = ConfigurationManager.AppSettings["UserID_QueryMember0"];
            f_strPassWord_QueryMember0 = ConfigurationManager.AppSettings["Password_QueryMember0"];
            f_strDatabase_QueryMember1 = "mysql";
            f_strDataSource_QueryMember1 = ConfigurationManager.AppSettings["DataSource_QueryMember1"];
            f_strUserID_QueryMember1 = ConfigurationManager.AppSettings["UserID_QueryMember1"];
            f_strPassWord_QueryMember1 = ConfigurationManager.AppSettings["Password_QueryMember1"];
            f_strDatabase_QueryMember2 = "mysql";
            f_strDataSource_QueryMember2 = ConfigurationManager.AppSettings["DataSource_QueryMember2"];
            f_strUserID_QueryMember2 = ConfigurationManager.AppSettings["UserID_QueryMember2"];
            f_strPassWord_QueryMember2 = ConfigurationManager.AppSettings["Password_QueryMember2"];
            f_strDatabase_PropertyTurnover = "mysql";
            f_strDataSource_PropertyTurnover = ConfigurationManager.AppSettings["DataSource_PropertyTurnover"];
            f_strUserID_PropertyTurnover = ConfigurationManager.AppSettings["UserID_PropertyTurnover"];
            f_strPassWord_PropertyTurnover = ConfigurationManager.AppSettings["Password_PropertyTurnover"];

            f_strDatabase_CardBind = "mysql";
            f_strDataSource_CardBind = ConfigurationManager.AppSettings["DataSource_CardBind"];
            f_strUserID_CardBind = ConfigurationManager.AppSettings["UserID_CardBind"];
            f_strPassWord_CardBind = ConfigurationManager.AppSettings["Password_CardBind"];

            f_strDatabase_IconInfo1 = "mysql";
            f_strDataSource_IconInfo1 = ConfigurationManager.AppSettings["DataSource_ICONINFO1"];
            f_strUserID_IconInfo1 = ConfigurationManager.AppSettings["UserID_ICONINFO1"];
            f_strPassWord_IconInfo1 = ConfigurationManager.AppSettings["Password_ICONINFO1"];

            f_strDatabase_IconInfo2 = "mysql";
            f_strDataSource_IconInfo2 = ConfigurationManager.AppSettings["DataSource_ICONINFO2"];
            f_strUserID_IconInfo2 = ConfigurationManager.AppSettings["UserID_ICONINFO2"];
            f_strPassWord_IconInfo2 = ConfigurationManager.AppSettings["Password_ICONINFO2"];

            f_strDatabase_IconInfo3 = "mysql";
            f_strDataSource_IconInfo3 = ConfigurationManager.AppSettings["DataSource_ICONINFO3"];
            f_strUserID_IconInfo3 = ConfigurationManager.AppSettings["UserID_ICONINFO3"];
            f_strPassWord_IconInfo3 = ConfigurationManager.AppSettings["Password_ICONINFO3"];

            f_strDatabase_ForeignCard = "mysql";
            f_strDataSource_ForeignCard = ConfigurationManager.AppSettings["DataSource_ForeignCard"];
            f_strUserID_ForeignCard = ConfigurationManager.AppSettings["UserID_ForeignCard"];
            f_strPassWord_ForeignCard = ConfigurationManager.AppSettings["Password_ForeignCard"];

            f_strDatabase_MobileBind = "mysql";
            f_strDataSource_MobileBind = ConfigurationManager.AppSettings["DataSource_MobileBind"];
            f_strUserID_MobileBind = ConfigurationManager.AppSettings["UserID_MobileBind"];
            f_strPassWord_MobileBind = ConfigurationManager.AppSettings["Password_MobileBind"];

            f_strDatabase_MobileRecharge = "mysql";
            f_strDataSource_MobileRecharge = ConfigurationManager.AppSettings["DataSource_MobileRecharge"];
            f_strUserID_MobileRecharge = ConfigurationManager.AppSettings["UserID_MobileRecharge"];
            f_strPassWord_MobileRecharge = ConfigurationManager.AppSettings["Password_MobileRecharge"];

            //20130816 lxl还是改回原来的配置，不知道什么原因？
            f_strDatabase_MobileRecharge = "mysql";
            f_strUserID_InternetBank = ConfigurationManager.AppSettings["UserID_InternetBank"];
            f_strPassWord_InternetBank = ConfigurationManager.AppSettings["Password_InternetBank"];
            f_strDataSource_InternetBank = ConfigurationManager.AppSettings["DataSource_InternetBank"];

            //furion 20060221
            ICEServerIP = ConfigurationManager.AppSettings["ICEServerIP"].Trim();
            ICEPort = Int32.Parse(ConfigurationManager.AppSettings["ICEPort"].Trim());
            ICEServerIPSub = ConfigurationManager.AppSettings["ICEServerIPSub"].Trim();
            ICEPortSub = Int32.Parse(ConfigurationManager.AppSettings["ICEPortSub"].Trim());


            f_strDatabase_mt = "mysql";
            f_strDataSource_mt = ConfigurationManager.AppSettings["DataSource_MobileToken"];
            f_strUserID_mt = ConfigurationManager.AppSettings["UserID_MobileToken"];
            f_strPassword_mt = ConfigurationManager.AppSettings["Password_MobileToken"];


            f_strDataSource_wxaa = ConfigurationManager.AppSettings["DataSource_wxaa"];
            f_strUserID_wxaa = ConfigurationManager.AppSettings["UserID_wxaa"];
            f_strPassword_wxaa = ConfigurationManager.AppSettings["Password_wxaa"];
            f_strDatabase_wxaa = "mysql";


            //微信红包
            f_strDataSource_wxhongbao = ConfigurationManager.AppSettings["DataSource_wxhongbao"];
            f_strUserID_wxhongbao = ConfigurationManager.AppSettings["UserID_wxhongbao"];
            f_strPassword_wxhongbao = ConfigurationManager.AppSettings["Password_wxhongbao"];
            f_strDatabase_wxhongbao = "mysql";

            f_strDataSource_wxxesk = ConfigurationManager.AppSettings["DataSource_wxxesk"];
            f_strUserID_wxxesk = ConfigurationManager.AppSettings["UserID_wxxesk"];
            f_strPassword_wxxesk = ConfigurationManager.AppSettings["Password_wxxesk"];
            f_strDatabase_wxxesk = "mysql";

            f_strDataSource_account = ConfigurationManager.AppSettings["DataSource_account"];
            f_strUserID_account = ConfigurationManager.AppSettings["UserID_account"];
            f_strPassword_account = ConfigurationManager.AppSettings["Password_account"];
            f_strDatabase_account = "mysql";

        }

        public static bool ReleaseCache(string strqqid, string type)
        {

            return true;

            /*
            string error = null;
            try
            {
                //furion 20090610 不再清除cache
                //如果是内部ID,进行一次转换，转换成QQID
                if (type == "uid")
                {
                    string str = "Select Fqqid From " + PublicRes.GetTName("t_user",strqqid) + " where fuid='" + strqqid +"'";
                    strqqid = PublicRes.ExecuteOne(str,"ywb_V30");
                }

                int qqid = 0;
                try
                {
                    qqid = Int32.Parse(strqqid);
                }
                catch
                {
                    error = "error qqid";
                    return false;
                }

                TReleaseCache tc = new TReleaseCache(qqid);
                byte[] buffer = tc.ToByte();

                string serverip = ReleaseCacheServerIP;   //"172.16.61.46";
                int serverport  = ReleaseCacheServerPort;

                if (!UDP.GetTCPReply(buffer,serverip,serverport,out error))
                {
                    //写报警日志
                    string logIP = RemoteLogSeverIP;    //System.Configuration.ConfigurationManager.AppSettings["LogServer"].Trim();
                    int logPort  = RemoteLogServerPort; //Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["LogPort"].Trim());
                    LogManage lm = new LogManage(logIP,logPort);
                    lm.DBLog("service","payment","warning","RelCache","清空cache失败！["+strqqid + "]");

                    //					//写文件
                    //					WriteFile("清空cache失败！");
			    
                    return false;
                }
                return true;
            }
            catch
            {
                //写报警日志
                string logIP = RemoteLogSeverIP;    //System.Configuration.ConfigurationManager.AppSettings["LogServer"].Trim();
                int logPort  = RemoteLogServerPort; //Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["LogPort"].Trim());
                LogManage lm = new LogManage(logIP,logPort);
                lm.DBLog("service","payment","warning","RelCache","清空cache失败！["+strqqid + "]");

                //				//写文件
                //				WriteFile("清空cache失败！");
			    
                return false;
            }
            */
        }


        /// <summary>
        /// 返回根据ID分机器的连接字符串
        /// </summary>
        /// <param name="FlagName">分机器使用的标志</param>
        /// <param name="sepvalue">分机器的判断value，如内部ID后两位</param>
        /// <returns></returns>
        public static string GetConnString(string FlagName, string sepvalue)
        {
            MySqlAccess daht = new MySqlAccess(DbConnectionString.Instance.GetConnectionString("HT"));
            try
            {
                string strSql = " select FconfigName from c2c_fmdb.t_sepdb where FMinValue<=" + sepvalue + " and FMaxValue>=" + sepvalue
                    + " and FFlagName='" + FlagName.ToLower() + "'";

                daht.OpenConn();
                string configname = daht.GetOneResult(strSql);

                return DbConnectionString.Instance.GetConnectionString(configname);

                //string dbconfig = ConfigurationManager.AppSettings["DB_" + configname];
                //if (dbconfig == null || dbconfig == "")
                //    dbconfig = "mysql";

                //string connModule = "Driver=[MySQL ODBC 3.51 Driver]; Server={0}; Database={3}; UID={1}; PWD={2}; Option=3";
                //string db50list = ConfigurationManager.AppSettings["DB50List"];
                //if (db50list.IndexOf(";" + configname + ";") > -1)
                //{
                //    connModule = "Driver=[mysql ODBC 5.2a Driver]; Server={0}; Database={3}; UID={1}; PWD={2};charset=latin1; Option=3";
                //}

                //return String.Format(connModule, ConfigurationManager.AppSettings["DataSource_" + configname], ConfigurationManager.AppSettings["UserID_" + configname]
                //    , ConfigurationManager.AppSettings["Password_" + configname], dbconfig).Replace("[", "{").Replace("]", "}");
            }
            catch
            {
                return "";
            }
            finally
            {
                daht.Dispose();
            }
        }

        public static string GetConnString()
        {
            return GetConnString("YWB");
        }

        public static string GetConnString(string strDBType)
        {
            string sConnStr = "";

            //string connModule = "Driver=[MySQL ODBC 3.51 Driver]; Server={0}; Database={3}; UID={1}; PWD={2}; Option=3s";
            //string db50list = ConfigurationManager.AppSettings["DB50List"];
            //if (db50list.IndexOf(";" + strDBType + ";") > -1)
            //{
            //    connModule = "Driver=[mysql ODBC 5.2a Driver]; Server={0}; Database={3}; UID={1}; PWD={2};charset=latin1; Option=3";
            //}

            if (strDBType.ToUpper() == "YW")
            {
                //sConnStr = String.Format(connModule, f_strDataSource, f_strUserID, f_strPassword, f_strDatabase);
                return DbConnectionString.Instance.GetConnectionString("YW");
            }
            else if (strDBType.ToUpper() == "YWB")
            {
                //sConnStr = String.Format(connModule, f_strDataSource_ywb, f_strUserID_ywb, f_strPassword_ywb, f_strDatabase_ywb);
                return DbConnectionString.Instance.GetConnectionString("YWB");
            }
            else if (strDBType.ToUpper() == "ZL")
            {
                //sConnStr = String.Format(connModule, f_strDataSource_zl, f_strUserID_zl, f_strPassword_zl, f_strDatabase_zl);
                return DbConnectionString.Instance.GetConnectionString("ZL");
            }
            else if (strDBType.ToUpper() == "ZLB")
            {
                //sConnStr = String.Format(connModule, f_strDataSource_zlb, f_strUserID_zlb, f_strPassword_zlb, f_strDatabase_zlb);
                return DbConnectionString.Instance.GetConnectionString("ZLB");
            }
            else if (strDBType.ToUpper() == "HT")//需要更新key（DataSource_ht）
            {
                //sConnStr = String.Format(connModule, f_strDataSource_ht, f_strUserID_ht, f_strPassword_ht, f_strDatabase_ht);
                return DbConnectionString.Instance.GetConnectionString("DataSource_ht");
            }
            else if (strDBType.ToUpper() == "CDK")
            {
                //sConnStr = String.Format(connModule, f_strDataSource_cdk, f_strUserID_cdk, f_strPassword_cdk, f_strDatabase_cdk);
                return DbConnectionString.Instance.GetConnectionString("CDK");
            }
            else if (strDBType.ToUpper() == "STATISTICS")
            {
                //sConnStr = String.Format(connModule, f_strDataSource_statistics, f_strUserID_statistics, f_strPassword_statistics, f_strDatabase_statistics);
                return DbConnectionString.Instance.GetConnectionString("STATISTICS");
            }
            else if (strDBType.ToUpper() == "COMPREHENSIVE")
            {
                //sConnStr = String.Format(connModule, f_strDataSource_comprehensive, f_strUserID_comprehensive, f_strPassword_comprehensive, f_strDatabase_comprehensive);
                return DbConnectionString.Instance.GetConnectionString("COMPREHENSIVE");
            }
            else if (strDBType.ToUpper() == "ACTIVITY")
            {
                //sConnStr = String.Format(connModule, f_strDataSource_activity, f_strUserID_activity, f_strPassword_activity, f_strDatabase_activity);
                return DbConnectionString.Instance.GetConnectionString("ACTIVITY");
            }
            else if (strDBType.ToUpper() == "WXZFACT")
            {
                //sConnStr = String.Format(connModule, f_strDataSource_wxzfact, f_strUserID_wxzfact, f_strPassword_wxzfact, f_strDatabase_wxzfact);
                return DbConnectionString.Instance.GetConnectionString("WXZFACT");
            }
            else if (strDBType.ToUpper() == "FUND")//需要更新key(DataSource_fund)
            {
                //sConnStr = String.Format(connModule, f_strDataSource_fund, f_strUserID_fund, f_strPassword_fund, f_strDatabase_fund);
                return DbConnectionString.Instance.GetConnectionString("DataSource_fund");
            }
            else if (strDBType.ToUpper() == "BANKLISTHIS")
            {
                //sConnStr = String.Format(connModule, f_strDataSource_banklisthis, f_strUserID_banklisthis, f_strPassword_banklisthis, f_strDatabase_banklisthis);
                return DbConnectionString.Instance.GetConnectionString("BANKLISTHIS");
            }
            else if (strDBType.ToUpper() == "FCPAY")//需要更新key（DataSource_FCPAY）
            {
                //sConnStr = String.Format(connModule, f_strDataSource_fcpay, f_strUserID_fcpay, f_strPassword_fcpay, f_strDatabase_fcpay);
                return DbConnectionString.Instance.GetConnectionString("DataSource_FCPAY");
            }
            else if (strDBType.ToUpper() == "GWQ")
            {
                //sConnStr = String.Format(connModule, f_strDataSource_gwq, f_strUserID_gwq, f_strPassword_gwq, f_strDatabase_gwq);
                return DbConnectionString.Instance.GetConnectionString("GWQ");
            }
            else if (strDBType.ToUpper() == "CFT")
            {
                //sConnStr = String.Format(connModule, f_strDataSource_cft, f_strUserID_cft, f_strPassword_cft, f_strDatabase_cft);
                return DbConnectionString.Instance.GetConnectionString("CFT");
            }
            else if (strDBType.ToUpper() == "CFTB")
            {
                //sConnStr = String.Format(connModule, f_strDataSource_cftb, f_strUserID_cftb, f_strPassword_cftb, f_strDatabase_cftb);
                return DbConnectionString.Instance.GetConnectionString("CFTB");
            }
            else if (strDBType.ToUpper() == "FKDJ")
            {
                //sConnStr = String.Format(connModule, f_strDataSource_fkdj, f_strUserID_fkdj, f_strPassword_fkdj, f_strDatabase_fkdj);
                return DbConnectionString.Instance.GetConnectionString("FKDJ");
            }
            else if (strDBType.ToUpper() == "ZW")
            {
                //sConnStr = String.Format(connModule, f_strDataSource_zw, f_strUserID_zw, f_strPassword_zw, f_strDatabase_zw);
                return DbConnectionString.Instance.GetConnectionString("ZW");
            }
            else if (strDBType.ToUpper() == "ZWFK")
            {
                //sConnStr = String.Format(connModule, f_strDataSource_zwfk, f_strUserID_zwfk, f_strPassword_zwfk, f_strDatabase_zwfk);
                return DbConnectionString.Instance.GetConnectionString("ZWFK");
            }
            else if (strDBType.ToUpper() == "ZWSK")
            {
                //sConnStr = String.Format(connModule, f_strDataSource_zwsk, f_strUserID_zwsk, f_strPassword_zwsk, f_strDatabase_zwsk);
                return DbConnectionString.Instance.GetConnectionString("ZWSK");
            }
            else if (strDBType.ToUpper() == "ZWTK")
            {
                //sConnStr = String.Format(connModule, f_strDataSource_zwtk, f_strUserID_zwtk, f_strPassword_zwtk, f_strDatabase_zwtk);
                return DbConnectionString.Instance.GetConnectionString("ZWTK");
            }
            else if (strDBType.ToUpper() == "ZJ")
            {
                //sConnStr = String.Format(connModule, f_strDataSource_zj, f_strUserID_zj, f_strPassword_zj, f_strDatabase_zj);
                return DbConnectionString.Instance.GetConnectionString("ZJ");
            }
            else if (strDBType.ToUpper() == "RU")
            {
                //sConnStr = String.Format(connModule, f_strDataSource_ru, f_strUserID_ru, f_strPassword_ru, f_strDatabase_ru);
                return DbConnectionString.Instance.GetConnectionString("RU");
            }
            else if (strDBType.ToUpper() == "CRT")
            {
                //sConnStr = String.Format(connModule, f_strDataSource_crt, f_strUserID_crt, f_strPassword_crt, f_strDatabase_crt);
                return DbConnectionString.Instance.GetConnectionString("CRT");
            }
            else if (strDBType.ToUpper() == "ZJB")
            {
                //sConnStr = String.Format(connModule, f_strDataSource_zjb, f_strUserID_zjb, f_strPassword_zjb, f_strDatabase_zjb);
                return DbConnectionString.Instance.GetConnectionString("ZJB");
            }
            else if (strDBType.ToUpper() == "INC")
            {
                //sConnStr = String.Format(connModule, f_strDataSource_inc, f_strUserID_inc, f_strPassword_inc, f_strDatabase_inc);
                return DbConnectionString.Instance.GetConnectionString("INC");
            }
            else if (strDBType.ToUpper() == "BANKBULLETIN")
            {
                //sConnStr = String.Format(connModule, f_strDataSource_bankbulletin, f_strUserID_bankbulletin, f_strPassword_bankbulletin, f_strDatabase_bankbulletin);
                return DbConnectionString.Instance.GetConnectionString("BANKBULLETIN");
            }
            else if (strDBType.ToUpper() == "INC_NEW")
            {
                //sConnStr = String.Format(connModule, f_strDataSource_inc_new, f_strUserID_inc_new, f_strPassword_inc_new, f_strDatabase_inc_new);
                return DbConnectionString.Instance.GetConnectionString("INC_NEW");
            }
            else if (strDBType.ToUpper() == "INCB")
            {
                //sConnStr = String.Format(connModule, f_strDataSource_incb, f_strUserID_incb, f_strPassword_incb, f_strDatabase_incb);
                return DbConnectionString.Instance.GetConnectionString("INCB");
            }
            else if (strDBType.ToUpper() == "INCB_NEW")
            {
                //sConnStr = String.Format(connModule, f_strDataSource_incb_new, f_strUserID_incb_new, f_strPassword_incb_new, f_strDatabase_incb_new);
                return DbConnectionString.Instance.GetConnectionString("INCB_NEW");
            }
            else if (strDBType.ToUpper() == "CS")
            {
                //sConnStr = String.Format(connModule, f_strDataSource_cs, f_strUserID_cs, f_strPassword_cs, f_strDatabase_cs);
                return DbConnectionString.Instance.GetConnectionString("CS");
            }
            else if (strDBType.ToUpper() == "JSTL")
            {
                //sConnStr = String.Format(connModule, f_strDataSource_jstl, f_strUserID_jstl, f_strPassword_jstl, f_strDatabase_jstl);
                return DbConnectionString.Instance.GetConnectionString("JSTL");
            }
            else if (strDBType.ToUpper() == "JSWECHAT")
            {
                //sConnStr = String.Format(connModule, f_strDataSource_jswechat, f_strUserID_jswechat, f_strPassword_jswechat, f_strDatabase_jswechat);
                return DbConnectionString.Instance.GetConnectionString("JSWECHAT");
            }
            else if (strDBType.ToUpper() == "JS")
            {
                //sConnStr = String.Format(connModule, f_strDataSource_js, f_strUserID_js, f_strPassword_js, f_strDatabase_js);
                return DbConnectionString.Instance.GetConnectionString("JS");
            }
            else if (strDBType.ToUpper() == "BS")
            {
                //sConnStr = String.Format(connModule, f_strDataSource_bs, f_strUserID_bs, f_strPassword_bs, f_strDatabase_bs);
                return DbConnectionString.Instance.GetConnectionString("BS");
            }
            else if (strDBType.ToUpper() == "BSB")
            {
                //sConnStr = String.Format(connModule, f_strDataSource_bsb, f_strUserID_bsb, f_strPassword_bsb, f_strDatabase_bsb);
                return DbConnectionString.Instance.GetConnectionString("BSB");
            }
            else if (strDBType.ToUpper() == "BSB2")
            {
                //sConnStr = String.Format(connModule, f_strDataSource_bsb2, f_strUserID_bsb2, f_strPassword_bsb2, f_strDatabase_bsb2);
                return DbConnectionString.Instance.GetConnectionString("BSB2");
            }
            else if (strDBType.ToUpper() == "BD")
            {
                //sConnStr = String.Format(connModule, f_strDataSource_bd, f_strUserID_bd, f_strPassword_bd, f_strDatabase_bd);
                return DbConnectionString.Instance.GetConnectionString("BD");
            }
            else if (strDBType.ToUpper() == "DD")
            {
                //sConnStr = String.Format(connModule, f_strDataSource_dd, f_strUserID_dd, f_strPassword_dd, f_strDatabase_dd);
                return DbConnectionString.Instance.GetConnectionString("DD");
            }
            else if (strDBType.ToUpper() == "SYNINFO")
            {
                //sConnStr = String.Format(connModule, f_strDataSource_syninfo, f_strUserID_syninfo, f_strPassword_syninfo, f_strDatabase_syninfo);
                return DbConnectionString.Instance.GetConnectionString("SYNINFO");
            }
            else if (strDBType.ToUpper() == "USER")
            {
                //sConnStr = String.Format(connModule, f_strDataSource_user, f_strUserID_user, f_strPassword_user, f_strDatabase_user);
                return DbConnectionString.Instance.GetConnectionString("USER");
            }
            else if (strDBType.ToUpper() == "BRL")
            {
                //sConnStr = String.Format(connModule, f_strDataSource_brl, f_strUserID_brl, f_strPassword_brl, f_strDatabase_brl);
                return DbConnectionString.Instance.GetConnectionString("BRL");
            }
            else if (strDBType.ToUpper() == "TCP")
            {
                //sConnStr = String.Format(connModule, f_strDataSource_tcp, f_strUserID_tcp, f_strPassword_tcp, f_strDatabase_tcp);
                return DbConnectionString.Instance.GetConnectionString("TCP");
            }
            else if (strDBType.ToUpper() == "ORDER")
            {
                //sConnStr = String.Format(connModule, f_strDataSource_order, f_strUserID_order, f_strPassword_order, f_strDatabase_order);
                return DbConnectionString.Instance.GetConnectionString("ORDER");
            }
            else if (strDBType.ToUpper() == "MN")
            {
                //sConnStr = String.Format(connModule, f_strDataSource_mn, f_strUserID_mn, f_strPassword_mn, f_strDatabase_mn);
                return DbConnectionString.Instance.GetConnectionString("MN");
            }
            else if (strDBType.ToUpper() == "REL")
            {
                //sConnStr = String.Format(connModule, f_strDataSource_rel, f_strUserID_rel, f_strPassword_rel, f_strDatabase_rel);
                return DbConnectionString.Instance.GetConnectionString("REL");
            }
            else if (strDBType.ToUpper() == "AP")
            {
                //sConnStr = String.Format(connModule, f_strDataSource_ap, f_strUserID_ap, f_strPassword_ap, f_strDatabase_ap);
                return DbConnectionString.Instance.GetConnectionString("AP");
            }
            else if (strDBType.ToUpper() == "HD")
            {
                //sConnStr = String.Format(connModule, f_strDataSource_hd, f_strUserID_hd, f_strPassword_hd, f_strDatabase_hd);
                return DbConnectionString.Instance.GetConnectionString("HD");
            }
            else if (strDBType.ToUpper() == "UK")
            {
                //sConnStr = String.Format(connModule, f_strDataSource_uk, f_strUserID_uk, f_strPassword_uk, f_strDatabase_uk);
                return DbConnectionString.Instance.GetConnectionString("UK");
            }
            else if (strDBType.ToUpper() == "REMIT")
            {
                //sConnStr = String.Format(connModule, f_strDataSource_remit, f_strUserID_remit, f_strPassword_remit, f_strDatabase_remit);
                return DbConnectionString.Instance.GetConnectionString("REMIT");
            }
            else if (strDBType.ToUpper() == "MOBILE")
            {
                //sConnStr = String.Format(connModule, f_strDataSource_mobile, f_strUserID_mobile, f_strPassWord_mobile, f_strDatabase_mobile);
                return DbConnectionString.Instance.GetConnectionString("MOBILE");
            }
            else if (strDBType.ToUpper() == "QUERYMEMBER0")
            {
                //sConnStr = String.Format(connModule, f_strDataSource_QueryMember0, f_strUserID_QueryMember0, f_strPassWord_QueryMember0, f_strDatabase_QueryMember0);
                return DbConnectionString.Instance.GetConnectionString("QUERYMEMBER0");
            }
            else if (strDBType.ToUpper() == "QUERYMEMBER1")
            {
                //sConnStr = String.Format(connModule, f_strDataSource_QueryMember1, f_strUserID_QueryMember1, f_strPassWord_QueryMember1, f_strDatabase_QueryMember1);
                return DbConnectionString.Instance.GetConnectionString("QUERYMEMBER1");
            }
            else if (strDBType.ToUpper() == "QUERYMEMBER2")
            {
                //sConnStr = String.Format(connModule, f_strDataSource_QueryMember2, f_strUserID_QueryMember2, f_strPassWord_QueryMember2, f_strDatabase_QueryMember2);
                return DbConnectionString.Instance.GetConnectionString("QUERYMEMBER2");
            }
            else if (strDBType.ToUpper() == "PROPERTYTURNOVER")
            {
                //sConnStr = String.Format(connModule, f_strDataSource_PropertyTurnover, f_strUserID_PropertyTurnover, f_strPassWord_PropertyTurnover, f_strDatabase_PropertyTurnover);
                return DbConnectionString.Instance.GetConnectionString("PROPERTYTURNOVER");
            }
            else if (strDBType.ToUpper() == "CARDBIND")
            {
                //sConnStr = String.Format(connModule, f_strDataSource_CardBind, f_strUserID_CardBind, f_strPassWord_CardBind, f_strDatabase_CardBind);
                return DbConnectionString.Instance.GetConnectionString("CARDBIND");
            }
            else if (strDBType.ToUpper() == "FOREIGNCARD")
            {
                //sConnStr = String.Format(connModule, f_strDataSource_ForeignCard, f_strUserID_ForeignCard, f_strPassWord_ForeignCard, f_strDatabase_ForeignCard);
                return DbConnectionString.Instance.GetConnectionString("FOREIGNCARD");
            }
            else if (strDBType.ToUpper() == "MOBILERECHARGE")
            {
                //sConnStr = String.Format(connModule, f_strDataSource_MobileRecharge, f_strUserID_MobileRecharge, f_strPassWord_MobileRecharge, f_strDatabase_MobileRecharge);
                return DbConnectionString.Instance.GetConnectionString("MOBILERECHARGE");
            }
            else if (strDBType.ToUpper() == "MOBILEBIND")
            {
                //sConnStr = String.Format(connModule, f_strDataSource_MobileBind, f_strUserID_MobileBind, f_strPassWord_MobileBind, f_strDatabase_MobileBind);
                return DbConnectionString.Instance.GetConnectionString("MOBILEBIND");
            }
            else if (strDBType.ToUpper() == "INTERNETBANK")
            {
                //sConnStr = String.Format(connModule, f_strDataSource_InternetBank, f_strUserID_InternetBank, f_strPassWord_InternetBank, f_strDatabase_InternetBank);
                return DbConnectionString.Instance.GetConnectionString("INTERNETBANK");
            }
            else if (strDBType.ToUpper() == "MOBILETOKEN")
            {
                //sConnStr = String.Format(connModule, f_strDataSource_mt, f_strUserID_mt, f_strPassword_mt, f_strDatabase_mt);
                return DbConnectionString.Instance.GetConnectionString("MOBILETOKEN");
            }
            else if (strDBType.ToUpper() == "BSB3")
            {
                //sConnStr = String.Format(connModule, f_strDataSource_bsb3, f_strUserID_bsb3, f_strPassword_bsb3, f_strDatabase_bsb3);
                return DbConnectionString.Instance.GetConnectionString("BSB3");
            }
            else if (strDBType.ToUpper() == "ZWNEWTABLE")
            {
                //sConnStr = String.Format(connModule, f_strDataSource_zwNewTable, f_strUserID_zwNewTable, f_strPassword_zwNewTable, f_strDatabase_zwNewTable);
                return DbConnectionString.Instance.GetConnectionString("ZWNEWTABLE");
            }
            else if (strDBType.ToUpper() == "ZWOLDTABLE") //需要对应新key（DataSource_ZWOLDTABLE）
            {
                //sConnStr = String.Format(connModule, f_strDataSource_zwOldTable, f_strUserID_zwOldTable, f_strPassword_zwOldTable, f_strDatabase_zwOldTable);
                return DbConnectionString.Instance.GetConnectionString("DataSource_ZWOLDTABLE");
            }
            else if (strDBType.ToUpper() == "ICONINFO1")
            {
                //sConnStr = String.Format(connModule, f_strDataSource_IconInfo1, f_strUserID_IconInfo1, f_strPassWord_IconInfo1, f_strDatabase_IconInfo1);
                return DbConnectionString.Instance.GetConnectionString("ICONINFO1");
            }
            else if (strDBType.ToUpper() == "ICONINFO2")
            {
                //sConnStr = String.Format(connModule, f_strDataSource_IconInfo2, f_strUserID_IconInfo2, f_strPassWord_IconInfo2, f_strDatabase_IconInfo2);
                return DbConnectionString.Instance.GetConnectionString("ICONINFO2");
            }
            else if (strDBType.ToUpper() == "ICONINFO3")
            {
                //sConnStr = String.Format(connModule, f_strDataSource_IconInfo3, f_strUserID_IconInfo3, f_strPassWord_IconInfo3, f_strDatabase_IconInfo3);
                return DbConnectionString.Instance.GetConnectionString("ICONINFO3");
            }
            else if (strDBType.ToUpper() == "UC")
            {
                //sConnStr = String.Format(connModule, f_strDataSource_uc, f_strUserID_uc, f_strPassword_uc, f_strDatabase_uc);
                return DbConnectionString.Instance.GetConnectionString("UC");
            }
            else if (strDBType.ToUpper() == "CFTNEW")
            {
                //sConnStr = String.Format(connModule, f_strDataSource_cftnew, f_strUserID_cftnew, f_strPassword_cftnew, f_strDatabase_cftnew);
                return DbConnectionString.Instance.GetConnectionString("CFTNEW");
            }
            else if (strDBType.ToUpper() == "IVRNEW")
            {
                //sConnStr = String.Format(connModule, f_strDataSource_ivrnew, f_strUserID_ivrnew, f_strPassword_ivrnew, f_strDatabase_ivrnew);
                return DbConnectionString.Instance.GetConnectionString("IVRNEW");
            }
            else if (strDBType.ToUpper() == "WXAA")
            {
                //sConnStr = String.Format(connModule, f_strDataSource_wxaa, f_strUserID_wxaa, f_strPassword_wxaa, f_strDatabase_wxaa);
                return DbConnectionString.Instance.GetConnectionString("WXAA");
            }
            else if (strDBType.ToUpper() == "WXHONGBAO")
            {
                //sConnStr = String.Format(connModule, f_strDataSource_wxhongbao, f_strUserID_wxhongbao, f_strPassword_wxhongbao, f_strDatabase_wxhongbao);
                return DbConnectionString.Instance.GetConnectionString("WXHONGBAO");
            }
            else if (strDBType.ToUpper() == "WXXESK")
            {
                //sConnStr = String.Format(connModule, f_strDataSource_wxxesk, f_strUserID_wxxesk, f_strPassword_wxxesk, f_strDatabase_wxxesk);
                return DbConnectionString.Instance.GetConnectionString("WXXESK");
            }
               
            else if (System.Text.RegularExpressions.Regex.IsMatch(strDBType, @"^zw(\d{1,2})$"))
            {
                var index = strDBType.Substring(2, strDBType.Length - 2);
                return DbConnectionString.Instance.GetConnectionString("zw" + index);
            }
            return sConnStr;//.Replace("[", "{").Replace("]", "}");
        }

        public static string GetTableName(string strTable, string strID)  // 返回按照QQ号分表的表名， QQID
        {
            strID = ConvertToFuid(strID);  //先转换成fuid

            if (strID.Length > 2)
            {
                return "c2c_db_" + strID.Substring(strID.Length - 2) + "." + strTable + "_" + strID.Substring(strID.Length - 3, 1);
            }
            else return "c2c_db." + strTable;
        }

        public static string GetTableName2(string strTable, string strID)  // 返回按照QQ号分表的表名， QQID，倒数23位为库名，倒数1位为表名
        {
            strID = ConvertToFuid(strID);  //先转换成fuid

            if (strID.Length > 2)
            {
                return "c2c_db_" + strID.Substring(strID.Length - 3, 2) + "." + strTable + "_" + strID.Substring(strID.Length - 1, 1);
            }
            else return "c2c_db." + strTable;
        }

        public static string GetTableByDate(string strTable)   //返回按照时间排序的表名(用作插入流水或者记录类) date格式为yyyy-MM  ,.ToString("yyyy-MM").Replace("-","")
        {
            return "c2c_db_medi_user.t_bankroll_list_" + DateTime.Now.ToString("yyyy-MM").Replace("-", "").Substring(0, 6);
        }

        public static string GetTableByDate(string strTable, string timeStr)   //时间需要根据不同的情况判断，返回按照时间排序的表名 date格式为yyyy-MM  ,.ToString("yyyy-MM").Replace("-","")
        {
            return "c2c_db_medi_user.t_bankroll_list_" + timeStr.Replace(":", "").Replace(" ", "").Replace("-", "").Substring(0, 6);
        }

        public static string GetTableNameByCreid(string strTable, string strID)  // 返回按照身份证号分表的表名
        {
            if (strID.Length > 2)
            {
                string s_db = "";
                string l_s = strID.Substring(strID.Length - 1);

                if (l_s.ToUpper() == "X")
                {
                    s_db = "00";
                    return "statistics_db_" + s_db + "." + strTable + "_0";
                }
                else
                {
                    s_db = strID.Substring(strID.Length - 2);
                    return "statistics_db_" + s_db + "." + strTable + "_" + strID.Substring(strID.Length - 3, 1);
                }

            }
            else return "statistics_db." + strTable;
        }

        public static string GetSqlFromQQ(string strQQID, string strIDName)
        {
            //return " " + strIDName + "=(select fuid from " + GetTName("t_relation",strQQID) + " where fqqid='" + strQQID + "')";
            string fuid = ConvertToFuid(strQQID);
            return " " + strIDName + "=" + fuid + " ";
        }

        public static string GetTName(string dbname, string strTable, string strID)
        {


            if (strID.Length < 8 && Int32.Parse(strID) < 10000000)
            {
                if (strTable == "t_user")
                {
                    return dbname + ".t_middle_user";
                }
                else //还有一个帐户流水判断，需要时间字段暂时不加了。
                {
                    if (strID.Length > 2)
                    {
                        return dbname + "_" + strID.Substring(strID.Length - 2) + "." + strTable + "_" + strID.Substring(strID.Length - 3, 1);
                    }
                    else return dbname + "." + strTable;
                }
            }
            else
            {
                if (strID.Length > 2)
                {
                    return dbname + "_" + strID.Substring(strID.Length - 2) + "." + strTable + "_" + strID.Substring(strID.Length - 3, 1);
                }
                else return dbname + "." + strTable;
            }

        }

        //		GetRelationTableName
        public static string GetTName(string strTable, string strID)
        {
            return GetTName("c2c_db", strTable, strID);
        }

        //将QQ转化为fuid，并返回
        [Obsolete("CFT.CSOMS.BLL.CFTAccountModule.AccountService.ConvertToFuid")]
        public static string ConvertToFuid(string QQID)
        {
            try
            {
                //furion 20061115 email登录相关
                if (QQID == null || QQID.Trim().Length < 3)
                    return null;

                //start
                string qqid = QQID.Trim();

                /*
                string uid3 = "";
                try
                {
                    long itmp = long.Parse(qqid);
                    uid3 = qqid;
                }
                catch
                {
                    if(!Common.DESCode.GetEmailUid(qqid,out uid3))
                        return null;
                }
                //end			

                string str = "select fuid from " + GetTName("t_relation",uid3) + " where fqqid= '" + qqid + "'";

                //furion 20070206 如果fuid=0时也应该算为空.
                //int fuid = Int32.Parse(ExecuteOne(str,"YWB_30"));
                int fuid = Int32.Parse(ExecuteOne(str,"ZL"));
				
                if(fuid > 100)
                    return fuid.ToString();
                else
                    return null;
                */

                string errMsg = "";
                string strSql = "uin=" + qqid;
                string struid = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_RELATION, "fuid", out errMsg);

                if (struid == null)
                    return null;
                else
                    return struid;

            }
            catch(Exception ex)
            {
                SunLibrary.LoggerFactory.Get("KF_Service PublicRes").Info("ConvertToFuid error:" + ex);
                return null;
            }
        }
        /// <summary>
        /// 获取QQ对应的内部ID，（必须是真正存在帐户，快速交易用户返回空）
        /// </summary>
        /// <param name="QQID"></param>
        /// <returns></returns>
        public static string ConvertToFuidX(string QQID)
        {
            try
            {
                //furion 20061115 email登录相关
                if (QQID == null || QQID.Trim().Length < 3)
                    return null;

                //start
                string qqid = QQID.Trim();

                /*
                string uid3 = "";
                try
                {
                    long itmp = long.Parse(qqid);
                    uid3 = qqid;
                }
                catch
                {
                    if(!Common.DESCode.GetEmailUid(qqid,out uid3))
                        return null;
                }
                //end			

                string str = "select fuid from " + GetTName("t_relation",uid3) + " where fqqid= '" + qqid + "' and Fsign=1";

                //furion 20070206 如果fuid=0时也应该算为空.
                //int fuid = Int32.Parse(ExecuteOne(str,"YWB_30"));
                int fuid = Int32.Parse(ExecuteOne(str,"ZL"));

                if(fuid > 100)
                    return fuid.ToString();
                else
                    return null;
                */

                string errMsg = "";
                string strSql = "uin=" + qqid + "&wheresign=1";
                string struid = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_RELATION, "fuid", out errMsg);

                if (struid == null)
                    return null;
                else
                    return struid;
            }
            catch
            {
                return null;
            }
        }

        //uid转为qqid
        public static string Uid2QQ(string uid)
        {
            //MySqlAccess da = new MySqlAccess(GetConnString("YWB_30"));
            //MySqlAccess da = new MySqlAccess(GetConnString("ZL"));
            try
            {
                /*
                // TODO: 1客户信息资料外移
                //furion 20061121 email登录修改
                //string str = "select fqqid from " + GetTName("t_user",uid) + " where fuid= '" + uid + "'";
                //return ExecuteOne(str,"YWB_30");
                da.OpenConn();
                string strSql = "select ifnull(fqqid,''),ifnull(Femail,''),ifnull(Fmobile,'') from " 
                    + GetTName("t_user_info",uid) + " where fuid= '" + uid + "'";
                DataSet ds = da.dsGetTotalData(strSql);
                */

                string errMsg = "";
                string strSql = "uid=" + uid;
                DataSet ds = CommQuery.GetDataSetFromICE(strSql, CommQuery.QUERY_USERINFO, out errMsg);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count != 1)
                {
                    return null;
                }

                string strtmp = QueryInfo.GetString(ds.Tables[0].Rows[0]["fqqid"]);
                if (strtmp != "")
                {
                    string fuid = ConvertToFuid(strtmp);
                    if (fuid != null && fuid == uid)
                    {
                        return strtmp;
                    }
                }

                strtmp = QueryInfo.GetString(ds.Tables[0].Rows[0]["Femail"]);
                if (strtmp != "")
                {
                    string fuid = ConvertToFuid(strtmp);
                    if (fuid != null && fuid == uid)
                    {
                        return strtmp;
                    }
                }

                strtmp = QueryInfo.GetString(ds.Tables[0].Rows[0]["Fmobile"]);
                if (strtmp != "")
                {
                    string fuid = ConvertToFuid(strtmp);
                    if (fuid != null && fuid == uid)
                    {
                        return strtmp;
                    }
                }

                return null;
            }
            catch
            {
                return null;
            }
            finally
            {
                //da.Dispose();
            }
        }

        public static string GetZWdbNameByDate(string strTable)
        {

            return "c2c_zwdb_" + DateTime.Now.ToString("yyyyMMdd") + "." + strTable;

        }

        public static string ExecuteOne(string sqlStr, string dbStr) //查询单个结果
        {
            MySqlAccess da = null;
            try
            {
                da = new MySqlAccess(PublicRes.GetConnString(dbStr));  //连接数据库
                da.OpenConn();
                return da.GetOneResult(sqlStr);
            }
            finally
            {
                da.Dispose();
            }
        }

        public static string ExecuteOne_Conn(string sqlStr, string connstr) //查询单个结果
        {
            MySqlAccess da = null;
            try
            {
                da = new MySqlAccess(PublicRes.GetConnString(connstr));  //连接数据库
                da.OpenConn();
                return da.GetOneResult(sqlStr);
            }
            finally
            {
                da.Dispose();
            }
        }

        public static DataSet returnDSbyRange(string strCmd, int istr, int imax, string dbStr)
        {
            MySqlAccess da = null;

            try
            {
                da = new MySqlAccess(PublicRes.GetConnString(dbStr));  //连接帐务数据库
                da.OpenConn();
                return da.dsGetTableByRange(strCmd, istr, imax);
            }
            finally
            {
                da.Dispose();
            }
        }

        public static DataSet returnDSAll(string strCmd, string dbStr)
        {
            MySqlAccess da = null;

            try
            {
                da = new MySqlAccess(PublicRes.GetConnString(dbStr));  //连接数据库类型
                da.OpenConn();
                return da.dsGetTotalData(strCmd);
            }
            finally
            {
                da.Dispose();
            }
        }

        public static DataSet returnDSAll_Conn(string strCmd, string connstr)
        {
            MySqlAccess da = null;

            try
            {
                da = new MySqlAccess(connstr);  //连接数据库类型
                da.OpenConn();
                return da.dsGetTotalData(strCmd);
            }
            finally
            {
                da.Dispose();
            }
        }

        public static bool Execute(ArrayList al, string dbStr)  //执行事务(多条)
        {
            MySqlAccess da = null;
            try
            {
                da = new MySqlAccess(PublicRes.GetConnString(dbStr));
                da.OpenConn();
                da.ExecSqls_Trans(al);  //事务的函数
                return true;
            }
            finally
            {
                da.Dispose();
            }
        }

        public static bool ExecuteSql(string cmd, string dbStr)  //执行一条非查询的SQL语句
        {
            MySqlAccess da = null;
            try
            {
                da = new MySqlAccess(PublicRes.GetConnString(dbStr));
                da.OpenConn();
                da.ExecSql(cmd);
                return true;
            }
            finally
            {
                da.Dispose();
            }
        }

        public static int ExecuteSqlNum(string cmd, string dbStr)  //执行一条非查询的SQL语句
        {
            MySqlAccess da = null;
            try
            {
                da = new MySqlAccess(PublicRes.GetConnString(dbStr));
                da.OpenConn();
                return da.ExecSqlNum(cmd);
            }
            finally
            {
                da.Dispose();
            }
        }

        //用DataReader获取数据，获取单行多列数据
        public static string[] returnDrData(string strCmd, string[] ar, string dbStr)
        {
            MySqlAccess da = null;
            try
            {
                da = new MySqlAccess(PublicRes.GetConnString(dbStr));  //选择读取的数据库
                da.OpenConn();
                return da.drData(strCmd, ar);
            }
			finally  //释放资源
            {
                da.Dispose();
            }
        }

        //用DataReader获取数据，获取多行多列数据
        public static ArrayList returnDrDataMore(string strCmd, string[] ar, string dbStr)
        {
            MySqlAccess da = null;
            try
            {
                da = new MySqlAccess(PublicRes.GetConnString(dbStr));  //选择读取的数据库
                da.OpenConn();
                return da.drReturn(strCmd, ar);
            }
            finally
            {
                da.Dispose();
            }
        }

        public static string[] returnlistInfo(string listID, out TENCENT.OSS.C2C.Finance.Common.CommLib.tradeList tl, MySqlAccess da)  //帐务调整的共用函数：获取交易单的相关信息
        {
            // TODO1: furion 数据库优化 20080111
            /*
            string cmdStr = "Select Fbank_listid,Fcoding,flistid,fspid,fbuy_uid,fbuyid,fcurtype,fpaynum,fip,Fbuy_bank_type,fcreate_time," 
                + "fbuy_name,fsaleid,fsale_uid,fsale_name,fsale_bankid,Fpay_type,Fbuy_bankid,Fsale_bank_type,Fprice," 
                //+ "Fcarriage,Fprocedure,Fmemo,Fcash,Ftoken,Ffee3,Fstate,Fpay_time,Freceive_time,Fmodify_time,Ftrade_type,Flstate,Fbank_backid,Ffact from " 
                + "Fcarriage,Fprocedure,Fmemo,Fcash,Ftoken,Ffee3,Ftrade_state,Fpay_time,Freceive_time,Fmodify_time,Ftrade_type,Flstate,Fbank_backid,Ffact,Fmedi_sign,Fgwq_listid from " 
                //+ PublicRes.GetTName("t_tran_list",listID) 
                + PublicRes.GetTName("t_order",listID) 
                //+ " where flistid ='" + listID + "' limit 0,1 for update ";  //rayguo 2006.04.24 增加1条限制,当买家/卖家内部ID后三位一样时，根据交易单号查询会出现2条一目一样的数据，需要过滤
                + " where flistid ='" + listID + "' limit 0,1  ";
            //string [] ar = new string [34];
            //string [] ar = new string [35];
            */

            string[] ar = new string[37];

            ar[0] = "Fbank_listid";    //银行订单号
            ar[1] = "flistid";         //交易单ID
            ar[2] = "fspid";           //spid
            ar[3] = "fbuy_uid";        //fbuy_uid
            ar[4] = "fbuyid";          //fbuyid
            ar[5] = "fcurtype";        //fcurtype
            ar[6] = "fpaynum";         //fpaynum
            ar[7] = "fip";             //fip
            ar[8] = "Fbuy_bank_type";  //fbuy_bank_type
            ar[9] = "fcreate_time";    //fcreate_time
            ar[10] = "fbuy_name";       //fbuy_name 
            ar[11] = "fsaleid";        //fsaleid
            ar[12] = "fsale_uid";       //fsale_uid
            ar[13] = "fsale_name";      //fsale_name
            ar[14] = "fsale_bankid";    //fsale_bankid
            ar[15] = "Fpay_type";       // 
            ar[16] = "Fbuy_bankid";
            ar[17] = "Fsale_bank_type";
            ar[18] = "Fprice";
            ar[19] = "Fcarriage";
            ar[20] = "Fprocedure";
            ar[21] = "Fmemo";
            ar[22] = "Fcash";
            ar[23] = "Ftoken";
            ar[24] = "Ffee3";       //其它费用
            //ar[25]= "Fstate";      //交易的状态  Fstate，Fpay_time，Freceive_time，Fmodify_time
            ar[25] = "Ftrade_state";
            ar[26] = "Fpay_time";   //买家付款时间（本地）
            ar[27] = "Freceive_time"; //打款给卖家时间
            ar[28] = "Fmodify_time";  //最后修改时间
            ar[29] = "Ftrade_type";  //交易类型 1 c2c 2 b2c 3 fastpay 4 转帐
            ar[30] = "Flstate";      //交易单的状态
            ar[31] = "Fcoding";
            ar[32] = "Fbank_backid";
            ar[33] = "Ffact";
            ar[34] = "Fmedi_sign";
            ar[35] = "Fgwq_listid";
            ar[36] = "Fchannel_id";


            /*
            string cmdStr = "select * from "
                + PublicRes.GetTName("t_order",listID) 
                + " where flistid ='" + listID + "' limit 0,1  ";

            ar = da.drData(cmdStr,ar);
            */

            string errMsg = "";
            string cmdStr = "listid=" + listID;
            ar = CommQuery.GetdrDataFromICE(cmdStr, CommQuery.QUERY_ORDER, ar, out errMsg);

            if (ar[9] != null && ar[9] != "" && !ar[9].StartsWith("0000-00"))
                ar[9] = DateTime.Parse(ar[9]).ToString("yyyy-MM-dd HH:mm:ss");   //格式化时间
            if (ar[26] != null && ar[26] != "" && !ar[26].StartsWith("0000-00"))
                ar[26] = DateTime.Parse(ar[26]).ToString("yyyy-MM-dd HH:mm:ss");  //格式化时间
            if (ar[27] != null && ar[27] != "" && !ar[27].StartsWith("0000-00"))
                ar[27] = DateTime.Parse(ar[27]).ToString("yyyy-MM-dd HH:mm:ss");  //格式化时间
            if (ar[28] != null && ar[28] != "" && !ar[28].StartsWith("0000-00"))
                ar[28] = DateTime.Parse(ar[28]).ToString("yyyy-MM-dd HH:mm:ss");   //格式化时间

            //替换一些敏感字符
            ar[10] = TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.replaceSqlStr(ar[10]);
            ar[13] = TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.replaceSqlStr(ar[13]);
            ar[21] = TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.replaceSqlStr(ar[21]);

            tl = new TENCENT.OSS.C2C.Finance.Common.CommLib.tradeList();

            tl.Fbank_listid = ar[0];
            tl.flistid = ar[1];
            tl.fspid = ar[2];
            tl.fbuy_uid = ar[3];
            tl.fbuyid = ar[4];
            tl.fcurtype = ar[5];
            tl.fpaynum = ar[6];
            tl.fip = ar[7];
            tl.Fbuy_bank_type = ar[8];
            tl.fcreate_time = ar[9];
            tl.fbuy_name = ar[10];
            tl.fsaleid = ar[11];
            tl.fsale_uid = ar[12];
            tl.fsale_name = ar[13];
            tl.fsale_bankid = ar[14];
            tl.Fpay_type = ar[15];
            tl.Fbuy_bankid = ar[16];
            tl.Fsale_bank_type = ar[17];
            tl.Fprice = ar[18];
            tl.Fcarriage = ar[19];
            tl.Fprocedure = ar[20];
            tl.Fmemo = ar[21];
            tl.Fcash = ar[22];
            tl.Ftoken = ar[23];
            tl.Ffee3 = ar[24];
            tl.Fstate = ar[25];
            tl.Fpay_time = ar[26];
            tl.Freceive_time = ar[27];
            tl.Fmodify_time = ar[28];
            tl.Ftrade_type = ar[29];
            tl.Flstate = ar[30];
            tl.Fcoding = ar[31];
            tl.Fbank_backid = ar[32];
            tl.Ffact = ar[33];

            tl.Fmedi_sign = QueryInfo.GetInt(ar[34]);
            tl.Fgwq_listid = QueryInfo.GetString(ar[35]);

            tl.Fchannel_id = ar[36];
            return ar;
        }

        public static string[] returnlistInfo(string listID, out TENCENT.OSS.C2C.Finance.Common.CommLib.tradeList tl, string noUpdate)
        {
            //MySqlAccess dazj = new MySqlAccess(PublicRes.GetConnString("ZJ"));
            try
            {
                //dazj.OpenConn();
                //return returnlistInfo(listID,out tl,dazj,noUpdate);
                return returnlistInfo(listID, out tl, null, noUpdate);
            }
            finally
            {
                //dazj.Dispose();
            }
        }
        public static string[] returnlistInfo(string listID, out TENCENT.OSS.C2C.Finance.Common.CommLib.tradeList tl, MySqlAccess da, string noUpdate)  //帐务调整的共用函数：获取交易单的相关信息
        {
            // TODO: 1furion 数据库优化 20080111
            /*
            string cmdStr = "Select Fbank_listid,Fcoding,flistid,fspid,fbuy_uid,fbuyid,fcurtype,fpaynum,fip,Fbuy_bank_type,fcreate_time," 
                + "fbuy_name,fsaleid,fsale_uid,fsale_name,fsale_bankid,Fpay_type,Fbuy_bankid,Fsale_bank_type,Fprice," 
                + "Fcarriage,Fprocedure,Fmemo,Fcash,Ftoken,Ffee3,Ftrade_state,Fpay_time,Freceive_time,Fmodify_time,Ftrade_type,Flstate,Fbank_backid,Ffact,Fmedi_sign,Fgwq_listid from " 
                //+ PublicRes.GetTName("t_tran_list",listID) 
                + PublicRes.GetTName("t_order",listID) 
                + " where flistid ='" + listID + "' limit 0,1 ";
            //string [] ar = new string [34];
            //string [] ar = new string [35];

            */

            string[] ar = new string[37];
            ar[0] = "Fbank_listid";    //银行订单号
            ar[1] = "flistid";         //交易单ID
            ar[2] = "fspid";           //spid
            ar[3] = "fbuy_uid";        //fbuy_uid
            ar[4] = "fbuyid";          //fbuyid
            ar[5] = "fcurtype";        //fcurtype
            ar[6] = "fpaynum";         //fpaynum
            ar[7] = "fip";             //fip
            ar[8] = "Fbuy_bank_type";  //fbuy_bank_type
            ar[9] = "fcreate_time";    //fcreate_time
            ar[10] = "fbuy_name";       //fbuy_name 
            ar[11] = "fsaleid";        //fsaleid
            ar[12] = "fsale_uid";       //fsale_uid
            ar[13] = "fsale_name";      //fsale_name
            ar[14] = "fsale_bankid";    //fsale_bankid
            ar[15] = "Fpay_type";       // 
            ar[16] = "Fbuy_bankid";
            ar[17] = "Fsale_bank_type";
            ar[18] = "Fprice";
            ar[19] = "Fcarriage";
            ar[20] = "Fprocedure";
            ar[21] = "Fmemo";
            ar[22] = "Fcash";
            ar[23] = "Ftoken";
            ar[24] = "Ffee3";       //其它费用
            //ar[25]= "Fstate";      //交易单的状态  Fstate，Fpay_time，Freceive_time，Fmodify_time
            ar[25] = "Ftrade_state";
            ar[26] = "Fpay_time";   //买家付款时间（本地）
            ar[27] = "Freceive_time"; //打款给卖家时间
            ar[28] = "Fmodify_time";  //最后修改时间
            ar[29] = "Ftrade_type";  //交易类型 1 c2c 2 b2c 3 fastpay 4 转帐
            ar[30] = "Flstate";      //交易单的状态
            ar[31] = "Fcoding";
            ar[32] = "Fbank_backid";
            ar[33] = "Ffact";         //总支付费用
            ar[34] = "Fmedi_sign";
            ar[35] = "Fgwq_listid";
            ar[36] = "Fchannel_id";


            /*
            string cmdStr = "select * from "
                + PublicRes.GetTName("t_order",listID) 
                + " where flistid ='" + listID + "' limit 0,1  ";

			

            //				ar = PublicRes.returnDrData(cmdStr,ar,"YW_30");
            //ar[6],ar[18],ar[19],ar[20],ar[5]
            ar = da.drData(cmdStr,ar);
			
            */

            string errMsg = "";
            string cmdStr = "listid=" + listID;
            ar = CommQuery.GetdrDataFromICE(cmdStr, CommQuery.QUERY_ORDER, ar, out errMsg);

            if (ar[9] != null && ar[9] != "" && !ar[9].StartsWith("0000-00"))
                ar[9] = DateTime.Parse(ar[9]).ToString("yyyy-MM-dd HH:mm:ss");   //格式化时间
            if (ar[26] != null && ar[26] != "" && !ar[26].StartsWith("0000-00"))
                ar[26] = DateTime.Parse(ar[26]).ToString("yyyy-MM-dd HH:mm:ss");  //格式化时间
            if (ar[27] != null && ar[27] != "" && !ar[27].StartsWith("0000-00"))
                ar[27] = DateTime.Parse(ar[27]).ToString("yyyy-MM-dd HH:mm:ss");  //格式化时间
            if (ar[28] != null && ar[28] != "" && !ar[28].StartsWith("0000-00"))
                ar[28] = DateTime.Parse(ar[28]).ToString("yyyy-MM-dd HH:mm:ss");   //格式化时间

            //替换一些敏感字符
            ar[10] = commRes.replaceSqlStr(ar[10]);
            ar[13] = commRes.replaceSqlStr(ar[13]);
            ar[21] = commRes.replaceSqlStr(ar[21]);

            tl = new TENCENT.OSS.C2C.Finance.Common.CommLib.tradeList();

            tl.Fbank_listid = ar[0];
            tl.flistid = ar[1];
            tl.fspid = ar[2];
            tl.fbuy_uid = ar[3];
            tl.fbuyid = ar[4];
            tl.fcurtype = ar[5];
            tl.fpaynum = ar[6];
            tl.fip = ar[7];
            tl.Fbuy_bank_type = ar[8];
            tl.fcreate_time = ar[9];
            tl.fbuy_name = ar[10];
            tl.fsaleid = ar[11];
            tl.fsale_uid = ar[12];
            tl.fsale_name = ar[13];
            tl.fsale_bankid = ar[14];
            tl.Fpay_type = ar[15];
            tl.Fbuy_bankid = ar[16];
            tl.Fsale_bank_type = ar[17];
            tl.Fprice = ar[18];
            tl.Fcarriage = ar[19];
            tl.Fprocedure = ar[20];
            tl.Fmemo = ar[21];
            tl.Fcash = ar[22];
            tl.Ftoken = ar[23];
            tl.Ffee3 = ar[24];
            tl.Fstate = ar[25];
            tl.Fpay_time = ar[26];
            tl.Freceive_time = ar[27];
            tl.Fmodify_time = ar[28];
            tl.Ftrade_type = ar[29];
            tl.Flstate = ar[30];
            tl.Fcoding = ar[31];
            tl.Fbank_backid = ar[32];
            tl.Ffact = ar[33];

            tl.Fmedi_sign = QueryInfo.GetInt(ar[34]);
            tl.Fgwq_listid = QueryInfo.GetString(ar[35]);

            tl.Fchannel_id = ar[36];
            return ar;
        }

        //TODO: c2c_db.t_middle_user 所在DB已经扩容，但此方法没有调用，故没有修改配置，
        public static string[] returnMiddleInfo(string fqqid, MySqlAccess da)  //帐务调整的共用函数：获取中介账户的相关信息
        {

            //从中介账户中提取需要的数据
            string middleStr = "Select fuid,fqqid,ftruename,fbalance from c2c_db.t_middle_user where fqqid ='" + fqqid + "' for update ";
            //furion 20061127 绝对不能锁定10000号。
            if (fqqid == "10000" || fqqid == "9999")
            {
                middleStr = "Select fuid,fqqid,ftruename,fbalance from c2c_db.t_middle_user where fqqid ='" + fqqid + "' ";
            }
            string[] mr = new string[4];
            mr[0] = "fuid";
            mr[1] = "fqqid";
            mr[2] = "ftruename";
            mr[3] = "fbalance";
            mr = da.drData(middleStr, mr);  //PublicRes.returnDrData(middleStr,mr,"YW_30");

            mr[2] = commRes.replaceSqlStr(mr[2]);

            return mr;
        }


        private static TCreateSessionReply ValidateUser(string strUserID, string strPassword)
        {

            return UserRight.ValidateUser(strUserID, strPassword, f_strServerIP, f_iServerPort);


        }

        private static bool ValidateRight(string szKey, int iOperId, string strRightCode)
        {
            int itmp = 0;
            try
            {
                itmp = Int32.Parse(strRightCode);
            }
            catch
            {
                return false;
            }

            return UserRight.ValidateRight(szKey, iOperId, 56, itmp, f_strServerIP, f_iServerPort);

        }

        private static void DelLoginUser(string szKey, int iOperId)
        {

            UserRight.DelLoginUser(szKey, iOperId, f_strServerIP, f_iServerPort);


        }

        //根据ListID确定买家或者卖家的对应的表名
        public static string getTableNameFromLsd(string uidType, string listID, string tableName)  //第一个参数：fbuy_uid/fsale_uid 确定是获得买家的号或者是卖家的号　
        {/*
			// TODO1: furion 数据库优化 20080111
			//string strSelectBuyID  = "SELECT  " + uidType + "  FROM " + PublicRes.GetTName("t_tran_list",listID) + " WHERE flistid ='" + listID + "'" ;
			string strSelectBuyID  = "SELECT  " + uidType + "  FROM " + PublicRes.GetTName("t_order",listID) + " WHERE flistid ='" + listID + "'" ;
			//string strTableName = PublicRes.GetTName(tableName,PublicRes.ExecuteOne(strSelectBuyID,"YW_30"));
			string strTableName = PublicRes.GetTName(tableName,PublicRes.ExecuteOne(strSelectBuyID,"ZJ"));
			*/

            string errMsg = "";
            string strSelectBuyID = "listid=" + listID;
            string buyid = CommQuery.GetOneResultFromICE(strSelectBuyID, CommQuery.QUERY_ORDER, uidType, out errMsg);

            string strTableName = PublicRes.GetTName(tableName, buyid);

            return strTableName;

        }
        //该函数未被调用
        public static string returnFactionType(string QQID, string listID)
        {
            //没有用到的函数
            string cmdStr = "Select faction_type  from " + PublicRes.GetTableName("t_bankroll_list", QQID) + " where flistid ='" + listID + "' Order by fbkid DESC limit 0,1";
            return ExecuteOne(cmdStr, "ywb_V30");

        }
        /*
         * strUserID--用户名称
         * ip---ip地址
         * type--类型
         * actionEvent--事件
         * 
         * 
         * */
        public static void writeSysLog(string strUserID, string ip, string type, string actionEvent, int sign, string id, string opType) //opType: 操作对象的类型
        {/*客服系统日志是和帐务记一起的，现在要分离出来，客服不记这里
			
			string signStr;
			
			if(sign == 1)
				signStr = "成功!";
			else
				signStr = "失败!";

			string detail = strUserID + "进行“" + actionEvent + "”操作。操作ID" + id + "," + opType + signStr;

			//写入日志： 用户 + 时间 + 动作类型 + 具体动作 + IP地址
			string logStr = "Insert c2c_fmdb.t_log (FUserID,FactionTime,FactionType,FActionID,FActionName,Fsign,FMemo,Fip) Values (" +
				"'" + strUserID         + "',"  + 
				" NOW() ,"        +           //取数据库时间
				"'" + type        + "',"+     //类型 tz  (调帐)   数据字典  
				"'" + id          + "',"+     //ID为标识某一操作的关键ID。如交易单的ID，查询的QQ号。
				"'" + actionEvent + "',"    + //描述：充值调整
				"'" + sign        + "',"  +   //成功失败标志  1表示成功 0表示失败
				"'" + detail        + "',"  + 
				"'" + ip          + "')" ;

			PublicRes.ExecuteSql(logStr,"ht"); //插入数据库 帐务管理数据库
					
*/
        }

        public static void writeSysLog(string type, string actionEvent, int sign, string id, string opType, string detai)
        {
            writeSysLog(fh.UserName, fh.UserIP, type, actionEvent, sign, id, opType, detai);
        }

        /// <summary>
        /// 写本地日志
        /// </summary>
        /// <param name="strUserID">用户名</param>
        /// <param name="ip">操作IP</param>
        /// <param name="type">操作类型</param>
        /// <param name="actionEvent">操作函数</param>
        /// <param name="sign">是否成功</param>
        /// <param name="id">操作ID</param>
        /// <param name="opType">已不再使用了</param>
        /// <param name="detail">详细描述</param>
        public static void writeSysLog(string strUserID, string ip, string type, string actionEvent, int sign, string id, string opType, string detail) //opType: 操作对象的类型
        {/*客服系统日志是和帐务记一起的，现在要分离出来，客服不记这里
			
			string signStr;
			
			if(sign == 1)
				signStr = "成功!";
			else
				signStr = "失败!";

			//				string detail = strUserID + "对" + id + "进行“" + actionEvent + "”操作。" + signStr;

			//写入日志： 用户 + 时间 + 动作类型 + 具体动作 + IP地址
			string logStr = "Insert c2c_fmdb.t_log (FUserID,FactionTime,FactionType,FActionID,FActionName,Fsign,FMemo,Fip) Values (" +
				"'" + strUserID         + "',"  + 
				" NOW() ,"        +           //取数据库时间
				"'" + type        + "',"+     //类型 tz  (调帐)   数据字典  
				"'" + id          + "',"+     //ID为标识某一操作的关键ID。如交易单的ID，查询的QQ号。
				"'" + actionEvent + "',"    + //描述：充值调整
				"'" + sign        + "',"  +   //成功失败标志  1表示成功 0表示失败
				"'" + detail        + "',"  + 
				"'" + ip          + "')" ;

			PublicRes.ExecuteSql(logStr,"ht"); //插入数据库 帐务管理数据库
			*/
        }

        public static bool sendMail(string mailToStr, string mailFromStr, string subject, string content)  //发送邮件
        {
            if (PublicRes.IgnoreLimitCheck)
                return true;

            try
            {
                TENCENT.OSS.C2C.Finance.Common.CommLib.NewMailSend newMail = new TENCENT.OSS.C2C.Finance.Common.CommLib.NewMailSend();
                newMail.SendMail(mailToStr, "", subject, content, true, null);

                //				MailMessage mail = new MailMessage();
                //				mail.From = mailFromStr;        //发件人
                //				mail.To   = mailToStr;          //收件人
                //				//mail.BodyEncoding = System.Text.Encoding.Unicode;
                //				mail.BodyFormat = MailFormat.Html;
                //				mail.Body = content; //邮件内容
                //				mail.Priority = MailPriority.High; //优先级
                //				mail.Subject  = subject;           //邮件主题
                //
                //				SmtpMail.SmtpServer = ConfigurationManager.AppSettings["smtpServer"].ToString(); //"192.168.1.27";  邮件服务器地址
                //
                //				SmtpMail.Send(mail);
                return true;
            }
            catch (Exception er)
            {
                return false;
            }
        }


        // 这个SendMail方法是专用于审核类的方法的，为了向前兼容所以多创建了这些方法。。。
        public static bool sendMail(string mailToStr, string mailFromStr, string subject, string content, string type, out string Msg)  //发送邮件
        {
            Msg = null;

            if (PublicRes.IgnoreLimitCheck)
                return true;

            try
            {
                TENCENT.OSS.C2C.Finance.Common.CommLib.NewMailSend newMail = new TENCENT.OSS.C2C.Finance.Common.CommLib.NewMailSend();
                newMail.SendMail(mailToStr, "", subject, content, true, null);

                //				MailMessage mail = new MailMessage();
                //				mail.From = mailFromStr;        //发件人
                //				mail.To   = mailToStr;          //收件人
                //				//mail.BodyEncoding = System.Text.Encoding.Unicode;
                //				mail.BodyFormat = MailFormat.Html;
                //				mail.Body = content; //邮件内容
                //				mail.Priority = MailPriority.High; //优先级
                //				mail.Subject  = subject;           //邮件主题
                //
                //				//SmtpMail.SmtpServer = ConfigurationManager.AppSettings["smtpServer"].ToString(); //"192.168.1.27";  邮件服务器地址
                //			
                //				if (type == "inner")
                //				{
                //					SmtpMail.SmtpServer = null;
                //					//SmtpMail.SmtpServer.Insert(0,ConfigurationManager.AppSettings["smtpServer"].ToString().Trim());	
                //					SmtpMail.SmtpServer = ConfigurationManager.AppSettings["smtpServer"].ToString(); //"192.168.1.27";  邮件服务器地址
                //					SmtpMail.Send(mail);
                //					SmtpMail.SmtpServer = null;
                //					
                //				}
                //				else if(type == "out")  //外部邮箱
                //				{
                //					SmtpMail.SmtpServer = null;
                //					//SmtpMail.SmtpServer.Insert(0,ConfigurationManager.AppSettings["OutSmtpServer"].ToString().Trim());	
                //					SmtpMail.SmtpServer = ConfigurationManager.AppSettings["OutSmtpServer"].ToString();
                //					SmtpMail.Send(mail);
                //					SmtpMail.SmtpServer = null;
                //				}
                //				else
                //				{
                //					Msg  = "邮件类型错误！ 请检查！只能为inner或out。";
                //					return false;
                //				}


                //邮件发送本地日志
                PublicRes.WriteFile_ForCheck("发送审批邮件成功！ 收件人：" + mailToStr + " 邮件内容：" + content);
                PublicRes.CloseFile();

                return true;
            }
            catch (Exception er)
            {
                Msg = er.Message.ToString().Replace("'", "’");
                return false;
            }
        }

        /// <summary>
        /// 获得数据库的时间 格式：yyyy-MM-dd HH:mm:ss
        /// </summary>
        public static string strNowTimeStander
        {
            get
            {
                //MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("YWB-V30"));
                MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("ZW"));
                try
                {
                    da.OpenConn();
                    return da.GetOneResult("select DATE_FORMAT(now(),'%Y-%m-%d %H:%i:%s')");
                }
                catch
                {
                    return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                }
                finally
                {
                    da.Dispose();
                }
            }
        }


        /// <summary>
        /// 获得数据库的时间(带单引号) 格式：'yyyy-MM-dd HH:mm:ss'
        /// </summary>
        public static string strNowTime
        {
            get
            {
                //MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("YWB_30"));
                //MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("ZJB"));
                MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("HT"));
                try
                {
                    da.OpenConn();
                    string tmp = da.GetOneResult("select DATE_FORMAT(now(),'%Y-%m-%d %H:%i:%s')");
                    return "'" + tmp + "'";
                }
                catch
                {
                    string tmp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    return "'" + tmp + "'";
                }
                finally
                {
                    da.Dispose();
                }
            }
        }




        public static string ICEEncode(string instr)
        {
            if (instr == null || instr.Trim() == "")
                return instr;
            else
            {
                return instr.Replace("%", "%25").Replace("=", "%3d").Replace("&", "%26");
            }
        }

        public static string GetNameFromQQ(string uid)
        {
            //如果为商户,取商户名.
            //为个人取个人名,为公司类型取公司名称
            //MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("YWB"));
            //MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("ZL"));
            try
            {
                if (uid == null || uid.Trim().Length < 3)
                    return "";

                //da.OpenConn();
                //个人
                uid = ConvertToFuid(uid);

                /*
                string strSql = "select Fuser_type from " + GetTName("t_user_info",uid) + " where fuid=" + uid;
                if(da.GetOneResult(strSql) == "1")
                {
                    strSql = "select Ftruename from " + GetTName("t_user_info",uid) + " where fuid=" + uid;
                }
                else
                {
                    strSql = "select Fcompany_name from " + GetTName("t_user_info",uid) + " where fuid=" + uid;
                }

                return da.GetOneResult(strSql);
                */

                string strSql = "uid=" + uid;
                string errMsg = "";
                string usertype = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_USERINFO, "Fuser_type", out errMsg);

                string fieldstr = "Fcompany_name";
                if (usertype == "1")
                {
                    fieldstr = "Ftruename";
                }

                return CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_USERINFO, fieldstr, out errMsg);
            }
            catch (Exception err)
            {
                return "";
            }
            finally
            {
                //da.Dispose();
            }
        }

        public static void GetUserName_Table(DataTable dt, string idField, string nameField)
        {
            try
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    string errMsg = "";

                    foreach (DataRow dr in dt.Rows)
                    {
                        string tmp = dr[idField].ToString();

                        /*
                        //string strTable = PublicRes.GetTName("t_user",tmp);
                        string strTable = PublicRes.GetTName("t_user_info",tmp);

                        string strSql = "select Ftruename from " + strTable + " where Fuid=" + tmp;
                        //tmp = PublicRes.ExecuteOne(strSql,"YW_30");
                        //tmp = PublicRes.ExecuteOne(strSql,"YWB_30");
                        tmp = PublicRes.ExecuteOne(strSql,"ZL");
                        */

                        string strSql = "uid=" + tmp;
                        tmp = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_USERINFO, "Ftruename", out errMsg);

                        dr.BeginEdit();
                        dr[nameField] = tmp;
                        dr.EndEdit();
                    }
                }
            }
            catch
            {
                //我没有办法进行什么处理。
            }
        }

        public static void GetTureBankList(DataTable dt, string idField)
        {
            try
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    string uniteFlag = ConfigurationManager.AppSettings["UniteFlag"];
                    foreach (DataRow dr in dt.Rows)
                    {
                        string tmp = dr[idField].ToString();
                        if (tmp != null && tmp.StartsWith(uniteFlag))
                        {
                            tmp = tmp.Substring(5);
                            dr.BeginEdit();
                            dr[idField] = tmp;
                            dr.EndEdit();
                        }

                    }
                }

            }
            catch
            {
                //我没有办法进行什么处理。
            }

        }

        public static void GetTureBankListForView(DataTable dt, string idField)
        {
            try
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    string uniteFlag = ConfigurationManager.AppSettings["UniteFlag"];
                    foreach (DataRow dr in dt.Rows)
                    {
                        string tmp = dr[idField].ToString();
                        if (tmp != null && tmp.StartsWith(uniteFlag))
                        {
                            tmp = tmp.Substring(5);
                            dr.BeginEdit();
                            dr[idField] = "<FONT color=\"red\">" + tmp + "</font>";
                            dr.EndEdit();
                        }

                    }
                }
            }
            catch
            {
                //我没有办法进行什么处理。
            }

        }

        public static DataTable returnDataTable(string strCmd, string dbStr)
        {
            MySqlAccess da = null;

            try
            {
                da = new MySqlAccess(PublicRes.GetConnString(dbStr));  //连接数据库类型
                da.OpenConn();
                return da.GetTable(strCmd);
            }
            finally
            {
                da.Dispose();
            }
        }

        public static string replaceMStr(string str)  //对插入数据库的字符串的敏感字符进行替换,放置非法sql注入访问
        {
            if (str == null) return null; //furion 20050819

            str = str.Replace("'", "’");
            str = str.Replace("\"", "”");
            str = str.Replace("script", "ｓｃｒｉｐｔ");
            str = str.Replace("<", "〈");
            str = str.Replace(">", "〉");
            str = str.Replace("-", "－");
            return str;
        }

        public static string filename = System.Configuration.ConfigurationManager.AppSettings["logPath"].ToString() + DateTime.Now.ToString("yyyyMMdd") + ".txt";  //每天一个日志   \\"c:\\333.txt";
        public static StreamWriter swFromFile = null;
        public static void WriteFile(string strmsg)
        {
            //			if (swFromFile == null)
            //			{
            //				FileStream fs1 = new FileStream(filename,FileMode.Append,FileAccess.Write,FileShare.ReadWrite);
            //				swFromFile = new StreamWriter(fs1,System.Text.Encoding.GetEncoding("GB2312"));
            //			}
            //
            //			swFromFile.WriteLine(strmsg);
        }


        // 为审核类写的WriteFile方法
        public static void WriteFile_ForCheck(string strmsg)
        {
            log4net.ILog log = log4net.LogManager.GetLogger("Finance_service.PublicRes.WriteFile");
            if (log.IsInfoEnabled)
                log.Info(strmsg);
        }

        public static void CloseFile()
        {
            //			swFromFile.Flush();
            //			swFromFile.Close();
            //			swFromFile = null;
        }
        public DataSet GetSqlServerData(string sql)
        {
            DataSqlServer dss = new DataAccess.DataSqlServer(f_strDataSource_spoa, f_strDatabase_spoa, f_strUserID_spoa, f_strPassword_spoa, sql);
            return dss.GetDataSet();
        }
        public void ModifySqlServerData(string sql)
        {
            DataSqlServer dss = new DataAccess.DataSqlServer(f_strDataSource_spoa, f_strDatabase_spoa, f_strUserID_spoa, f_strPassword_spoa, sql);
            dss.ModifyValue();
        }

        public static DateTime ConvertToDateTime(string checkTime)
        {
            try
            {
                if (checkTime.Length == 8)
                {
                    return Convert.ToDateTime(checkTime.Substring(0, 4) + "-" + checkTime.Substring(4, 2) + "-" + checkTime.Substring(6, 2));
                }
                else
                {
                    return Convert.ToDateTime(checkTime);
                }

            }
            catch (Exception ex)
            {

                return System.DateTime.Now;


            }
        }



        #region  2011/7/21


        public static string GetPayLogName()
        {
            return "t_paylog_" + DateTime.Now.ToString("yyyyMMdd");
        }


        #endregion

        public static string GetWatchWord(string methodName)
        {
            string keyvalue = "";
            if (methodName != null)
            {
                string wordkey = methodName.ToLower() + "_word";
                if (ConfigurationManager.AppSettings[wordkey] != null)
                {
                    keyvalue = ConfigurationManager.AppSettings[wordkey].Trim();
                }
                else
                {
                    keyvalue = ConfigurationManager.AppSettings["ICEPASSWORD"].Trim();
                }
            }
            else
            {
                keyvalue = ConfigurationManager.AppSettings["ICEPASSWORD"].Trim();
            }

            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(keyvalue, "md5").ToLower();
        }



        /// <summary>
        /// 去除敏感字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string replaceHtmlStr(string str)
        {
            if (str == null) return null; //furion 20050819

            str = str.Replace("\"", "＂");
            str = str.Replace("'", "＇");

            str = str.Replace("script", "ｓｃｒｉｐｔ");
            str = str.Replace("<", "〈");
            str = str.Replace(">", "〉");
            return str;
        }


        public static string getCgiString(string instr)
        {
            if (instr == null || instr.Trim() == "")
                return "";

            //System.Text.Encoding enc = System.Text.Encoding.GetEncoding("GB2312");
            return System.Web.HttpContext.Current.Server.UrlDecode(instr).Replace("\r\n", "").Trim()
                .Replace("%3d", "=").Replace("%20", " ").Replace("%26", "&").Replace("%7", "|");
        }



        public static string getCgiString2(string instr)
        {
            if (instr == null || instr.Trim() == "")
                return "";

            //System.Text.Encoding enc = System.Text.Encoding.GetEncoding("GB2312");
            return System.Web.HttpContext.Current.Server.UrlDecode(instr).Replace("\r\n", "").Trim()
                .Replace("%3d", "=").Replace("%20", " ").Replace("%26", "&").Replace("%25", "%");
        }




        public static void SetRightAndLog(Finance_Header header, RightAndLog r1)
        {
            r1.OperID = header.OperID;
            r1.RightString = header.RightString;
            r1.SzKey = header.SzKey;
            r1.UserID = header.UserName;
            r1.UserIP = header.UserIP;
            r1.SessionID = header.SessionID;
            //r1.OperID2 = header.UserName;
            r1.url = header.SrcUrl;
        }



        //public static string EncryptZerosPadding(string source,byte[] key,byte[] iv)
        public static string EncryptZerosPadding(string source)
        {
            if (source.Trim() == "")
                return "";

            try
            {

                DESCryptoServiceProvider des = new DESCryptoServiceProvider();

                //把字符串放到byte数组中  
                byte[] inputByteArray = Encoding.Default.GetBytes(source);

                byte[] key = { 0x4a, 0x08, 0x80, 0x58, 0x13, 0xad, 0x46, 0x89 };

                byte[] iv = { 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18 };

                des.Key = key;
                des.IV = iv;
                des.Padding = System.Security.Cryptography.PaddingMode.Zeros;//0填充
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);

                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                string bas64Str = Convert.ToBase64String(ms.ToArray());
                return bas64Str.Replace("+", "-").Replace("/", "_");//.Replace("=","%3d").Replace("=","%3D");


            }
            catch (Exception ex)
            {
                return "";
            }
        }





        /// <summary>
        /// 一点通银行卡解密方法
        /// </summary>
        /// <param name="base64Source"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public static string DecryptNoPadding_ForBankCardUnbind(string base64Source, byte[] key, byte[] iv)
        {
            try
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();

                //middle的base64转换不标准，这个地方需要替换下 
                byte[] inputByteArray = Convert.FromBase64String(base64Source.Replace("-", "+").Replace("_", "/").Replace("%3d", "=").Replace("%3D", "="));

                //建立加密对象的密钥和偏移量，此值重要，不能修改 

                des.Key = key;
                des.IV = iv;
                //Padding设置为None，这个很重要，因为middle是这个加密的
                des.Padding = System.Security.Cryptography.PaddingMode.None;
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();

                //return System.Text.Encoding.ASCII.GetString(ms.ToArray()).Trim(); 

                return System.Text.Encoding.GetEncoding("gb2312").GetString(ms.ToArray()).Trim().TrimEnd('\0'); //支持汉字解密 andrew
            }
            catch (Exception ex)
            {
                string stmp = ex.Message;
                return "";
            }
        }



        /// <summary>
        /// 一点通银行卡解密方法
        /// </summary>
        /// <param name="base64Bankid"></param>
        /// <returns></returns>
        public static string BankIDEncode_ForBankCardUnbind(string base64Bankid)
        {
            byte[] iv = { 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18 };
            byte[] newkey = { 0x4a, 0x08, 0x80, 0x58, 0x13, 0xad, 0x46, 0x89 };

            try
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();

                //middle的base64转换不标准，这个地方需要替换下 
                byte[] inputByteArray = Convert.FromBase64String(base64Bankid.Replace("-", "+").Replace("_", "/").Replace("%3d", "=").Replace("%3D", "="));

                //建立加密对象的密钥和偏移量，此值重要，不能修改 

                des.Key = newkey;
                des.IV = iv;
                //Padding设置为None，这个很重要，因为middle是这个加密的
                des.Padding = System.Security.Cryptography.PaddingMode.None;
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();

                return System.Text.Encoding.ASCII.GetString(ms.ToArray()).Trim();
            }
            catch (Exception ex)
            {
                return base64Bankid;
            }
        }


        /// <summary>
        /// 姓名生僻字  银行卡解密方法
        /// 与一点通解密算法一样，秘钥不一样
        /// </summary>
        /// <param name="base64Bankid"></param>
        /// <returns></returns>
        public static string BankIDEncode_ForRareName(string base64Bankid)
        {
            byte[] iv = { 0x3e, 0x3e, 0x3a, 0x6e, 0x4f, 0x6a, 0x3f, 0x0a };
            byte[] newkey = { 0x1a, 0x2a, (byte)(-0x58 & 0xFF), 0x59, 0x4a, (byte)(-0x63 & 0xFF), 0x5f, (byte)(-0x4d & 0xFF) };

            try
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();

                //middle的base64转换不标准，这个地方需要替换下 
                byte[] inputByteArray = Convert.FromBase64String(base64Bankid.Replace("-", "+").Replace("_", "/").Replace("%3d", "=").Replace("%3D", "="));

                //建立加密对象的密钥和偏移量，此值重要，不能修改 

                des.Key = newkey;
                des.IV = iv;
                //Padding设置为None，这个很重要，因为middle是这个加密的
                des.Padding = System.Security.Cryptography.PaddingMode.None;
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();

                return System.Text.Encoding.ASCII.GetString(ms.ToArray()).Trim();
            }
            catch (Exception ex)
            {
                return base64Bankid;
            }
        }

        /// <summary>
        /// 姓名生僻字  姓名解密方法
        /// 与一点通解密算法一样，秘钥不一样
        /// </summary>
        /// <param name="base64Bankid"></param>
        /// <returns></returns>
        public static string NameEncode_ForRareName(string base64Bankid)
        {
            byte[] iv = { 0x2e, 0x3a, 0x35, 0x67, 0x45, 0x6a, 0x7f, 0x0a };
            byte[] newkey = { 0x3a, 0x2a, (byte)(-0x28 & 0xFF), 0x59, 0x43, (byte)(-0x23 & 0xFF), 0x16, (byte)(-0x65 & 0xFF) };

            try
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();

                //middle的base64转换不标准，这个地方需要替换下 
                byte[] inputByteArray = Convert.FromBase64String(base64Bankid.Replace("-", "+").Replace("_", "/").Replace("%3d", "=").Replace("%3D", "="));

                //建立加密对象的密钥和偏移量，此值重要，不能修改 

                des.Key = newkey;
                des.IV = iv;
                //Padding设置为None，这个很重要，因为middle是这个加密的
                des.Padding = System.Security.Cryptography.PaddingMode.None;
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();

                return System.Text.Encoding.GetEncoding("gb2312").GetString(ms.ToArray()).Trim();
            }
            catch (Exception ex)
            {
                return base64Bankid;
            }
        }

        public static DataSet QueyNewCZDataDataSet(string strSql, out string errMsg)
        {
            errMsg = "";
            try
            {
                string msg = "";
                DataSet dt_order = CommQuery.GetDataSetFromICE_QueryServer(strSql, CommQuery.QUERY_TCBANKROLL_S, out msg);
                return dt_order;
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                return null;
            }

        }

        //根据时间来查询是否为新的充值单
        public static bool IsNewOrderCZData(DateTime payFrontTime)
        {
            try
            {
                string oldEndTime = ZWDicClass.GetZWDicValue("OldOrderCZDataEndTime", PublicRes.GetConnString("ZW"));
                if (oldEndTime == null || oldEndTime == "")
                {
                    throw new Exception("未查询到OldOrderCZDataEndTime对应的配置值！");
                }

                if (payFrontTime.CompareTo(Convert.ToDateTime(oldEndTime)) >= 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new LogicException("未查询到OldOrderCZDataEndTime对应的配置值！");
            }


        }

        public enum ModifySPMRefundType
        {

            银行成功回导20 = 20,
            银行失败转入代发21 = 21,
            商户退单账务汇总22 = 22,
            申请撤销退款23 = 23,
            撤销退款24 = 24,
            拒绝撤销退款25 = 25,
            资料重填26 = 26,
            实时退款汇总27 = 27,
            实时退款对账后生效28 = 28
        }

        //更新退款申请表状态 andrew 20120514 新增
        /*
        20： 银行成功回导
        21：银行失败,转入代发
        22：商户退单账务汇总
        23：申请撤销退款
        24：撤销退款
        25：拒绝撤销退款
        26：资料重填
        27： 实时退款汇总
        28：实时退款对账后生效
        */
        public static bool ModifySPMRefundService(int req_type, string transaction_id, string draw_id, out string errMsg)
        {
            errMsg = "";
            try
            {
                short result;
                string msg;
                string reply;

                string inmsg = "&req_type=" + req_type.ToString();
                inmsg += "&transaction_id=" + transaction_id;
                inmsg += "&draw_id=" + draw_id;
                inmsg += "&watch_word=" + PublicRes.GetWatchWord("itg_modify_refund_service"); ;


                if (commRes.middleInvoke("itg_modify_refund_service", inmsg, true, out reply, out result, out msg))
                {
                    if (result != 0)
                    {
                        errMsg = "调用itg_modify_refund_service返回失败：result=" + result + "，msg=" + msg;
                        return false;
                    }
                    else
                    {
                        if (reply.IndexOf("result=0") > -1)
                        {
                            return true;
                        }
                        else if (reply.IndexOf("result=22221205") > -1)//重入返回码:	22221205
                        {
                            return true;
                        }
                        else
                        {
                            errMsg = "调用itg_modify_refund_service返回失败：reply=" + reply;
                            return false;
                        }
                    }
                }
                else
                {
                    errMsg = "调用itg_modify_refund_service返回失败：result=" + result + "，msg=" + msg;
                    return false;
                }
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                return false;
            }
        }


        //根据uin来确定买卖订单库所属DB,uin为商户号或财付通帐号，userType=1表示商户，userType=2表示用户
        public static string GetBSBOrderDBString(string qq, string innerId, int userType)
        {
            try
            {
                string fuid = "";
                if (qq != "")
                {
                    if (userType == 1)//商户
                    {
                        string strSql = "spid=" + qq;
                        string errMsg = "";
                        fuid = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_MERCHANTINFO, "FuidMiddle", out errMsg);
                    }
                    else if (userType == 2)//用户
                    {

                        fuid = PublicRes.ConvertToFuid(qq);
                    }

                    if (fuid == null || fuid.Trim() == "")
                    {
                        return "";
                        log4net.ILog log = log4net.LogManager.GetLogger("根据uid来确定买卖订单库所属DB");
                        if (log.IsErrorEnabled) log.Error("查找不到指定的商户！spid=" + qq);
                    }

                    if (fuid == null || fuid.Length < 2)
                    {
                        return "";
                        log4net.ILog log = log4net.LogManager.GetLogger("根据uid来确定买卖订单库所属DB");
                        if (log.IsErrorEnabled) log.Error("uid不符号要求！uid=" + fuid);
                    }
                }
                if (innerId != "")
                {
                    fuid = innerId;
                }
                int num = Convert.ToInt32(fuid.Substring(fuid.Length - 2));
                if (0 <= num && num <= 49)//0-49
                {
                    return "BSB3";
                }
                else if (50 <= num && num <= 99)//50-99
                {
                    return "BSB2";
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                log4net.ILog log = log4net.LogManager.GetLogger("根据uid来确定买卖订单库所属DB");
                if (log.IsErrorEnabled) log.Error(ex.ToString());
                return "";
            }
        }

        //通过服务签名
        public static string SingedByService(string SingedString)
        {
            try
            {
                DataSet ds = null;
                string Msg = "";
                string errMsg = "";
                //md5
                string CFTAccount = ConfigurationManager.AppSettings["CFTAccount"];
                string wxzfAccount = ConfigurationManager.AppSettings["wxzfAccount"];
                string relay_ip = ConfigurationManager.AppSettings["Relay_IP"];
                string relay_port = ConfigurationManager.AppSettings["Relay_PORT"];

                //  string sign = "account_no=" + acc_no + "&bank_type=" + bank_type + "&uid=" + uid + "&uidtoc=" + uidtoc + "&key=";
                string sign = SingedString + "&key=";
                sign = System.Web.HttpUtility.UrlEncode(sign, System.Text.Encoding.GetEncoding("gb2312"));

                Msg = "";
                errMsg = "";
                string req_sign = "request_type=132&ver=1&head_u=&sp_id=" + CFTAccount + "&merchant_spid=" + CFTAccount + "&sp_str=" + sign;

                string sign_md5 = commRes.GetFromRelay(req_sign, relay_ip, relay_port, out Msg);
                if (Msg != "")
                {
                    throw new Exception("1签名出错:" + Msg);
                }
                ds = CommQuery.ParseRelayStr(sign_md5, out errMsg);
                if (errMsg != "")
                {
                    throw new Exception("2签名出错:" + errMsg);
                }
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    sign_md5 = ds.Tables[0].Rows[0]["sp_md5"].ToString();
                }
                return sign_md5;
            }
            catch (Exception ex)
            {
                throw new LogicException("通过服务签名出错！");
            }


        }
    }
}
