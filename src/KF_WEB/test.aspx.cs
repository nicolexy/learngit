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
using TENCENT.OSS.CFT.KF.DataAccess;
using System.Text;
using System.IO;
using System.Data.OleDb;
using System.Data.Odbc;
using TENCENT.OSS.CFT.KF.Common;
using System.Net.Sockets;
using System.Net;
using System.Collections.Generic;
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;
using TENCENT.OSS.C2C.Finance.Common.CommLib;
using System.Text.RegularExpressions;
using CFT.CSOMS.BLL.SPOA;
using CFT.CSOMS.BLL.CFTAccountModule;
using CFT.CSOMS.BLL.WechatPay;
using CFT.CSOMS.BLL.TradeModule;
using CFT.CSOMS.BLL.FundModule;
using BankLib;
using CFT.CSOMS.BLL.ForeignCurrencyModule;
using CFT.CSOMS.COMMLIB;
using SunLibrary;


namespace TENCENT.OSS.CFT.KF.KF_Web
{
	/// <summary>
	/// test 的摘要说明。
	/// </summary>
    public partial class test : System.Web.UI.Page
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
        
            string Uid = "527123677";
               var dbNum = Uid.Substring(Uid.Length - 2, 2);
              var tableNum = Uid.Substring(Uid.Length - 3, 1);
 
            //string ref_param = ViewState["ref_param"] == null ? "" : ViewState["ref_param"].ToString();
            //DataSet ds = new TradeService().GetBankRollList("272906037", DateTime.Now.AddDays(-1000), DateTime.Now, 0, 10, ref ref_param);
            //ViewState["ref_param"] = ref_param;

            //DataSet d1 = new TradeService().GetListidFromUserOrder("", "298686752", 0, 1);
            //DataSet d2 = new TradeService().GetQueryList(Convert.ToDateTime("2010-1-1"), Convert.ToDateTime("2015-5-10"), "", "", "298686752", "", "coding", "1", 2, 99, 0, 15);
            //DataSet d3 = new TradeService().GetQueryList(Convert.ToDateTime("2010-1-1"), Convert.ToDateTime("2015-5-10"), "", "", "", "298731022", "", "", 0, 99, 0, 15);

            //DataSet  d4 = new TradeService().GetManJianUsingList("298686752", 0, Convert.ToDateTime("2010-1-1"), Convert.ToDateTime("2015-5-10"), "", 0, 15);
           // DataSet d5 = new TradeService().Q_PAY_LIST("298686752", 0, Convert.ToDateTime("2010-1-1"), Convert.ToDateTime("2015-5-10"), 0, 15);
            //DataSet d6 = new TradeService().Q_PAY_LIST("298686752", 9, Convert.ToDateTime("2010-1-1"), Convert.ToDateTime("2015-5-10"), 0, 15);
            //DataSet d7 = new TradeService().Q_PAY_LIST("298731022", 10, Convert.ToDateTime("2010-1-1"), Convert.ToDateTime("2015-5-10"), 0, 15);
            //DataSet d8 = new TradeService().Q_PAY_LIST("301778752", 13, Convert.ToDateTime("2010-1-1"), Convert.ToDateTime("2015-5-10"), 0, 15); 
           //DataSet d9 = new TradeService().MediListQueryClass("2000000501", "180801201206211018163765", "", "", "", "", 0, 15);
           //  DataSet d91 = new TradeService().MediListQueryClass("2000000501", "", "", "", "", "", 0, 15);
           // DataSet d92 = new TradeService().MediListQueryClass("2000000501", "N7life", "2011-11-9 0:00:00", "2011-11-15 0:00:00", "9", "", 0, 15);
           //DataSet d93 = new TradeService().MediListQueryClass("2000000501", "", "2010-1-1", "", "3", "", 0, 15);

            //var dsDetailList = new WechatPayService().QueryWebchatHB("1000000000201501012000009051", 3, "127.0.0.1", 0, 10);

            //发邮件  服务地址变了：10.12.23.14 7600
            // SendEmail("10.12.23.14", 7600, "direct=1&&direct_email=1985119654@qq.com&actiontype=102217&p_name=lxlKF&p_parm1=https://www.tenpay.com/v2/cs/v2");
            //SendEmail("172.25.38.34", 7600, "direct=1&&direct_email=466678748@qq.com&actiontype=2217&p_name=echoCS&p_parm1=https://www.tenpay.com/v2/cs/v2");
        //    string p_parm4 = "- 证件号码与原注册证件号码不符<br>- 请您重新提交申述表时，上传账户资金来源截图。请参考“<a href='http://help.tenpay.com/cgi-bin/helpcenter/help_center.cgi?id=2232'> 特殊申诉找回</a>”指引<br>- <br/><br>温馨提示：</br>如果您的账号已经绑定了手机号码，并可有效接收验证码，您可以直接进入<a href='https://www.tenpay.com/v2.0/main/getPwdByMobile_index.shtml'>手机绑定找回支付密码地址</a>通过绑定手机快速找回您的支付密码。<br>";
         //   SendEmail("10.12.23.14", 7600, "direct=1&&direct_email=466678748@qq.com&actiontype=102032&p_name=lxlKF&p_parm1=&p_parm2=&p_parm3=&p_parm4=" + p_parm4);
           
            //发QQTips测试
            //SendQQTips("172.25.38.6", 7600, "direct=1&&direct_qq=466678748&actiontype=102217&url=https://www.tenpay.com/v2/cs/v2/?pwd=1");
            //SendQQTips("172.25.38.34", 7600, "direct=1&&direct_qq=466678748&actiontype=2217&url=https://www.tenpay.com/v2/cs/v2/?pwd=1");

            //发短信测试
     //      SendMessage("172.25.38.6", 7600, "direct=1&&direct_mobile=18718489269&actiontype=102217&url=www.tenpay.com/v2/cs ");
     //      SendMessage("172.25.38.6", 7600, "direct=1&&direct_mobile=18718489269&actiontype=102222&url=www.tenpay.com/v2/cs&reason=身份证号码有误开发环境");
       //    SendMessage("172.25.38.34", 7600, "direct=1&&direct_mobile=18718489269&actiontype=2217&url=www.tenpay.com/v2/cs&reason=身份证号码有误测试环境");
   
            
         //   测试更新手机接口
          //Send("172.25.38.6", 7600,"");
         // Send("172.25.38.34", 7600, "");

          //  测试绑定手机
          //  SendBindMobile("10.12.23.14", 7600, "");


            ////测试申诉直接通过函数
            //bool IsLockList = false;
            //if (Request.QueryString["type"] != null)
            //{
            //    IsLockList = true;
            //}
            //Finance_Header fh = new Finance_Header();
            //fh.UserIP = Request.UserHostAddress;

            //Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            //qs.CFTUserAppealPass(fh.UserIP, IsLockList);
            


   
            //var parameters = Encoding.Default.GetBytes("request_type=8161&optype=1&uid=295179101&sign=a18ec65dc2a0be89ac5c39a923d2aaf1");
            //var length = new byte[4];
            //length = BitConverter.GetBytes(parameters.Length);
            //List<byte> bufferIn = new List<byte>();
            //bufferIn.AddRange(length);
            //bufferIn.AddRange(parameters);
            //var r = GetTCPReply(bufferIn.ToArray(), "10.6.208.182", 22000); 

            //TestQuerySPContactInfo();
            //string reply;
            //short result;
            //string sMsg;
            //string inmsg = "spid=2000000501&CMD=FINANCE_MERINFO_STANDBY_XX&standby6=siming.huang@mangocity.com;liping.li@mangocity.com;bo.wei@mangoctestmodify@qq.com&client_ip=::1&operator_id=1100000000&module=修改外卡商户客服联系邮箱&function=modify_kf_email&modify_time=2014-09-12 17:04:19";

            //ICEAccessFactory.ICEMiddleInvoke("ui_common_update_service", inmsg, true);
            //string date = HttpUtility.UrlEncode("2014-07-22 23:59:59");

            //testAPI();

           

           // string bankCard = BankLib.BankIOX.DecryptNoPadding("8Qz189FjLX5CU24z_L2yWw==");
           // int biz_type = 10100;
           // string bankDate = "20140929";
           // int limit = 10;
           // int offset = 0;
           // FastPayService fast = new FastPayService();
           // DataSet ds = fast.QueryBankCardList(bankCard, bankDate, biz_type, offset, limit);

           // //订单解耦测试
           //OrderDecoupled();


            //TestRelayInvoke1("10.12.23.14", "22000", "request_type=100569&ver=1&head_u=&sp_id=&draw_id=104201308040012310737");
            //TestRelayInvoke2("10.12.23.14", "22000", "request_type=100568&ver=1&head_u=&sp_id=&transaction_id=2000000501901308040012310734");
            //TestRelayInvoke2("10.12.23.14", "22000", "request_type=100567&ver=1&head_u=&sp_id=&listid=2000000501901204240011520734");


            //if(Request["wechatname"]!=null){
            //    LogHelper.LogInfo(" test.aspx  wechatname ：" + Request["wechatname"].ToString());
            //    WeChatInfo(Request["wechatname"].ToString());
            //}
        }


        private void WeChatInfo(string wechatName) {
            string retInfo = WeChatHelper.GetAAOpenIdFromWeChatNameTest(wechatName);

            LogHelper.LogInfo(" test.aspx  private void WeChatInfo  retInfo：" + retInfo);

            Response.Write(retInfo);
            Response.End();
        }

        private static void OrderDecoupled()
        {
            string strReplyInfo = "";
            short iResult = 0;
            string Msg = "";
            bool index = false;
            //index = new commRes().NoStaticMiddleInvokeTest("query_order_service", "listid=2000000501321409301230003370", false, out strReplyInfo, out iResult, out Msg);

            //string watch_word = "MODIFY_REFUND_WATCH_WORD";
            //watch_word = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(watch_word, "md5").ToLower();
            //index = new commRes().NoStaticMiddleInvokeTest("itg_modify_refund_service", "&req_type=23&transaction_id=2000000501901407240012800101&draw_id=104201407240012800104&watch_word=" + watch_word, true, out strReplyInfo, out iResult, out Msg);

            //string time = TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.GetTickFromTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            //index = new commRes().NoStaticMiddleInvokeTest("syn_update4KZ_service", "update_type=2&transaction_id=2000000501901411110000297630&pay_type=1&modify_time=" + time, true, out strReplyInfo, out iResult, out Msg);
            //index = new commRes().NoStaticMiddleInvokeTest("medi_sync_order_service", "transaction_id=2000000501901411110000297630&cur_type=1&business_type=1", true, out strReplyInfo, out iResult, out Msg);
            //index = new commRes().NoStaticMiddleInvokeTest("order_update_service", "MSG_NO=1111111&transaction_id=2000000501901411110000297630&flstate=2&fmodify_time=2014-11-12 14:15:00", true, out strReplyInfo, out iResult, out Msg);
            //index = new commRes().NoStaticMiddleInvokeTest("sp_order_query_service", "partner_id=2000000501&sp_billno=4274470753&business_type=1", true, out strReplyInfo, out iResult, out Msg);
            index = new commRes().NoStaticMiddleInvokeTest("cq_query_tcbanklist_service", "CMD=QUERY_MUL_CHARGE_LIST&listid=2000000501201410160001209100&bank_list=20141016470020000005010001209101&bank_type=4700&offset=0&limit=10", false, out strReplyInfo, out iResult, out Msg);
        }

        private static DataSet TestRelayInvoke1(string ip,string port,string req)
        {
            string Msg = ""; //重置

            string answer = commRes.GetFromRelay(req, ip, port, out Msg);

            if (answer == "")
            {
                  return null;
            }
            if (Msg != "")
            {
                throw new Exception("调relay异常：" + Msg);
            }

            //解析relay str
            DataSet ds = CommQuery.ParseRelayStr(answer, out Msg);

            if (Msg != "")
            {
                throw new Exception("解析relay异常：" + Msg);
            }
            return ds;

        }

        private static DataSet TestRelayInvoke2(string ip, string port, string req)
        {
            string Msg = ""; //重置

            string answer = commRes.GetFromRelay(req, ip, port, out Msg);

            if (answer == "")
            {
                return null;
            }
            if (Msg != "")
            {
                throw new Exception("调relay异常：" + Msg);
            }

            //解析relay str
            DataSet ds  = CommQuery.ParseRelayPageRowNum0(answer, out Msg);

            if (Msg != "")
            {
                throw new Exception("解析relay异常：" + Msg);
            }
            return ds;

        }

        string GetTCPReply(byte[] bufferIn, string IP, int port)
        {
            using (var tcpClient = new TcpClient())
            {
                var ipAddress = IPAddress.Parse(IP);
                var ipPort = new IPEndPoint(ipAddress, port);
                tcpClient.Connect(ipPort);
                var stream = tcpClient.GetStream();
                stream.Write(bufferIn, 0, bufferIn.Length);
                var bufferOut = new byte[1024];
                stream.Read(bufferOut, 0, 1024);
                return Encoding.Default.GetString(bufferOut);
            }
        }		

		protected void Button1_Click(object sender, System.EventArgs e)
		{
			string pwdStr = "2222";
			string fuid = "295170320";

			pwdStr = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(pwdStr,"md5").ToLower();


			//新格式如下． md5(md5(支付密码)统一转小写 + uid)_
			pwdStr += fuid;
			pwdStr = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(pwdStr,"md5").ToLower() + "_";

			TextBox1.Text = pwdStr;
			return;


			string strSql = "select faddress from c2c_db_03.t_user_info_0 where fuid=110000003";

			MySqlAccess da = new MySqlAccess(TextBox1.Text);
			try
			{
				Encoding latin = Encoding.GetEncoding("latin1");
				Encoding gb2312 = Encoding.GetEncoding("gb2312");
				Encoding utf8 = Encoding.GetEncoding("utf-8");
				Encoding big5 = Encoding.GetEncoding("big5");
				Encoding unicode = Encoding.Unicode;  //Encoding.GetEncoding(54936)
				Encoding t20936 = Encoding.GetEncoding(20936);
				Encoding gb13000 = Encoding.GetEncoding("gb13000");

				da.OpenConn();			
			

				string tmp = da.GetOneResult(strSql);
			   
			  	Label1.Text = tmp;
                tmp = Server.UrlDecode(tmp);

				byte[] buffer = latin.GetBytes(tmp);
				buffer = Encoding.Convert(latin,big5,buffer);
				tmp = big5.GetString(buffer);

				Label2.Text = tmp;

				buffer = latin.GetBytes(tmp);


//				//da.ExecSql("insert test.abc(name) values('" + tmp +"')");
//				TextBox2.Text += "latin解码数据库读取出来的内容";
//				foreach(byte obj in buffer)
//				{
//					TextBox2.Text += "-" + obj.ToString();
//				}
//				TextBox2.Text += "\n\r";
//
//				buffer = latin.GetBytes(strin);
//				TextBox2.Text += "latin解码字符串读取出来的内容";
//				foreach(byte obj in buffer)
//				{
//					TextBox2.Text += "-" + obj.ToString();
//				}
//				TextBox2.Text += "\n\r";
//
//				buffer = gb2312.GetBytes(strin);
//				TextBox2.Text += "gb2312解码字符串读取出来的内容";
//				foreach(byte obj in buffer)
//				{
//					TextBox2.Text += "-" + obj.ToString();
//				}
//				TextBox2.Text += "\n\r";
//
//			
//				buffer = utf8.GetBytes(strin);
//				TextBox2.Text += "utf8解码字符串读取出来的内容";
//				foreach(byte obj in buffer)
//				{
//					TextBox2.Text += "-" + obj.ToString();
//				}
//				TextBox2.Text += "\n\r";
//
//			
//
//				buffer = unicode.GetBytes(strin);
//				TextBox2.Text += "unicode解码字符串读取出来的内容";
//				foreach(byte obj in buffer)
//				{
//					TextBox2.Text += "-" + obj.ToString();
//				}
//				TextBox2.Text += "\n\r";


				//byte[] bufferout = Encoding.Convert(latin,Encoding.GetEncoding(54936),buffer);
				tmp = latin.GetString(buffer);				
			}
			finally
			{
				da.Dispose();
			}
		}

		protected void Button2_Click(object sender, System.EventArgs e)
		{
			PublicRes.isLatin = false;

			System.Data.Odbc.OdbcConnection myconn =
                new OdbcConnection("Driver={MySQL ODBC 5.2a Driver};charset=latin1; Server=172.16.61.44; Database=test; UID=root; PWD=root1234; Option=3");
			try
			{
				string strSql = "select faddress from c2c_db_03.t_user_info_0 where fuid=110000003";

				myconn.Open();

				OdbcCommand mycommand = new OdbcCommand(strSql,myconn);
//				DataSet ds = myda.dsGetTotalData(strSql);
//				ds.WriteXml("c:\\1.xml");
//				string tmp = ds.Tables[0].Rows[0][0].ToString();
				string tmp = mycommand.ExecuteScalar().ToString();

				string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" 
					+"Data Source=c:\\1.xls" 
					+";Extended Properties='Excel 8.0'"; //;HDR=Yes;IMEX=1


				OleDbConnection conn = new OleDbConnection(strConn);
				try
				{
					conn.Open();
					OleDbCommand command = new OleDbCommand();
					command.Connection = conn;
					
						strSql = "INSERT INTO [SHEET1$](test) values('" +tmp+  "') ";
						command.CommandText = strSql;
						command.ExecuteNonQuery();
					
				}
				finally
				{
					conn.Close();
					conn.Dispose();
				}
			}
			finally
			{
				myconn.Close();
				myconn.Dispose();
			}			
		}

		protected void Button3_Click(object sender, System.EventArgs e)
		{
			IVRService.IVRService ivr = new TENCENT.OSS.CFT.KF.KF_Web.IVRService.IVRService();
			string strCheckID = "";//　申诉单ＩＤ
　　　　　　string strUserID = "";//  财付通账号
　　　　　　string strMobile = "";//　原手机号码．
            int   intCallNum= 0;// 第几次呼叫．　
            string dbName = "";
            string tbName = "";
            int iresult = ivr.GetIVRDataNew(out dbName, out tbName, out strCheckID, out strUserID, out strMobile, out intCallNum);

			tbAppealID.Text = strCheckID;
			tbcallnum.Text = intCallNum.ToString();
			tbmemo.Text = "呼叫结果";
			tbmobile.Text = strMobile;
			tbresult.Text = "6";
			tbuin.Text = strUserID;

		}

		protected void Button4_Click(object sender, System.EventArgs e)
		{
			IVRService.IVRService ivr = new TENCENT.OSS.CFT.KF.KF_Web.IVRService.IVRService();
			string ivrmd5 = "%u^e(F3!";
            string sourcestr = dbName.Text + tbName.Text+tbAppealID.Text + tbuin.Text + tbmobile.Text + tbresult.Text + tbmemo.Text + ivrmd5;
			string md5 = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sourcestr,"md5").ToLower();

            ivr.SendIVRResultNew(dbName.Text, tbName.Text, tbAppealID.Text, tbuin.Text, tbmobile.Text, Int32.Parse(tbresult.Text), tbmemo.Text, md5);
		}
        
        //通过消息服务更新手机接口测试
        protected void Send(string ip, int port, string msg)
        {
            Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
           string singed= qs.SingedByService("client_id=web-signcheck@tenpay&cmd=update_mobile&mobile=18718489260&uid=299703647");

            //[1]协议 1
            byte[] v = new byte[1] { 0x02 };
            //[2]是否需要返回结果 0不需要
            byte[] needr = UDP.GetByteFromInt(0);
            //[3]服务类型 CMD_EMAIL_MSG_ON 33
            byte[] cmd_type = UDP.GetByteFromInt(4);

            //[5]内容
            //{1}用户内部ID 1直发模式 0使用uid
            byte[] b_uid = UDP.GetByteFromInt(299703647);
            //内容[2] QQ号码 
            byte[] b_qqid= UDP.GetByteFromInt(299703647);
            //内容[3] Femail
            byte[] b_email=new byte[66];
            byte[] email = System.Text.Encoding.GetEncoding("GB2312").GetBytes("");
            email.CopyTo(b_email, 0);
            //内容[4] Fmobile
            byte[] b_moblile=new byte[65];
            byte[] moblile = System.Text.Encoding.GetEncoding("GB2312").GetBytes("18718489260");
            moblile.CopyTo(b_moblile, 0);
            //内容[5] Fstatus
            byte[] b_status = UDP.GetByteFromInt(0x00000040);
            //内容[6] FregTime
            byte[] b_regTime = UDP.GetByteFromUInt(GetStamp(DateTime.Now));
            //内容[7] FupdateTime
            byte[] b_updateTime = UDP.GetByteFromUInt(GetStamp(DateTime.Now));
          

            //签名
            byte[] sign = System.Text.Encoding.GetEncoding("GB2312").GetBytes(singed);

            //<1>对内容组包
            byte[] contData = new byte[b_uid.Length + b_qqid.Length + b_email.Length + b_moblile.Length + b_status.Length + b_regTime.Length + b_updateTime.Length + sign.Length];
            b_uid.CopyTo(contData, 0);
            b_qqid.CopyTo(contData, b_uid.Length);
            b_email.CopyTo(contData, b_uid.Length + b_qqid.Length);
            b_moblile.CopyTo(contData, b_uid.Length + b_qqid.Length + b_email.Length);
            b_status.CopyTo(contData, b_uid.Length + b_qqid.Length + b_email.Length + b_moblile.Length);
            b_regTime.CopyTo(contData, b_uid.Length + b_qqid.Length + b_email.Length + b_moblile.Length + b_status.Length);
            b_updateTime.CopyTo(contData, b_uid.Length + b_qqid.Length + b_email.Length + b_moblile.Length + b_status.Length + b_regTime.Length);
            sign.CopyTo(contData, b_uid.Length + b_qqid.Length + b_email.Length + b_moblile.Length + b_status.Length + b_regTime.Length + b_updateTime.Length);
          
            //[4]内容长度
            byte[] cont_len = UDP.GetByteFromInt(contData.Length);
            //[6]包序列号
            byte[] seq = UDP.GetByteFromInt(0);

            //<2>发送包
            byte[] data = new byte[v.Length + needr.Length + cmd_type.Length + cont_len.Length + contData.Length + seq.Length];
            v.CopyTo(data, 0);
            needr.CopyTo(data, v.Length);
            cmd_type.CopyTo(data, v.Length + needr.Length);
            cont_len.CopyTo(data, v.Length + needr.Length + cmd_type.Length);
            contData.CopyTo(data, v.Length + needr.Length + cmd_type.Length + cont_len.Length);
            seq.CopyTo(data, v.Length + needr.Length + cmd_type.Length + cont_len.Length + contData.Length);

            byte[] result = UDP.GetUDPReply(data, ip, port);
           
            byte[] m_dwRetCode = new byte[4] { result[5], result[6], result[7], result[8]};
            int dwRetCode = UDP.GetIntFromByte(m_dwRetCode);
            if (dwRetCode == 0)
            {
                //手机更新成功
            }
            else
            {
                byte[] m_dwContentLen = new byte[4] { result[9], result[10], result[11], result[12] };
                int dwContentLen = UDP.GetIntFromByte(m_dwContentLen);
                if (dwContentLen > 0)
                {
                    byte[] m_sContent = new byte[dwContentLen];
                    for (int i = 0; i < dwContentLen; i++)
                    {
                        m_sContent[i] = result[13 + i];
                    }
                    string sContent = Encoding.Default.GetString(m_sContent);//错误信息
                 //   sContent = System.Text.RegularExpressions.Regex.Match(sContent, @"[^\\\0]*").Value;
                    System.Text.RegularExpressions.Regex re = new System.Text.RegularExpressions.Regex(@"([^A-Za-z\u4E00-\u9FA5])",System.Text.RegularExpressions.RegexOptions.Multiline|System.Text.RegularExpressions.RegexOptions.ExplicitCapture);
                    System.Text.RegularExpressions.MatchCollection mc = re.Matches(sContent);
                    foreach (Match ma in mc)
                    {
                        sContent = sContent.Replace(ma.Value,"");
                    }
                }
            }
        }

        protected void SendBindMobile(string ip, int port, string msg)
        {
            Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            string singed = qs.SingedByService("client_id=web-signcheck@tenpay&cmd=update_mobile&mobile=18718489269&uid=333754915");

            //[1]协议 1
            byte[] v = new byte[1] { 0x02 };
            //[2]是否需要返回结果 0不需要
            byte[] needr = UDP.GetByteFromInt(0);
            //[3]服务类型 CMD_EMAIL_MSG_ON 33
            byte[] cmd_type = UDP.GetByteFromInt(41);

            //[5]内容
            //{1}用户内部ID 1直发模式 0使用uid
            byte[] b_uid = UDP.GetByteFromInt(333754915);
            //内容[2] QQ号码 
            byte[] b_qqid = UDP.GetByteFromInt(333754915);
            //内容[3] Femail
            byte[] b_email = new byte[66];
            byte[] email = System.Text.Encoding.GetEncoding("GB2312").GetBytes("");
            email.CopyTo(b_email, 0);
            //内容[4] Fmobile
            byte[] b_moblile = new byte[65];
            byte[] moblile = System.Text.Encoding.GetEncoding("GB2312").GetBytes("18718489269");
            moblile.CopyTo(b_moblile, 0);
            //内容[5] Fstatus
            byte[] b_status = UDP.GetByteFromInt(0x00000040);
            //内容[6] FregTime
            byte[] b_regTime = UDP.GetByteFromUInt(GetStamp(DateTime.Now));
            //内容[7] FupdateTime
            byte[] b_updateTime = UDP.GetByteFromUInt(GetStamp(DateTime.Now));


            //签名
            byte[] sign = System.Text.Encoding.GetEncoding("GB2312").GetBytes(singed);

            //<1>对内容组包
            byte[] contData = new byte[b_uid.Length + b_qqid.Length + b_email.Length + b_moblile.Length + b_status.Length + b_regTime.Length + b_updateTime.Length + sign.Length];
            b_uid.CopyTo(contData, 0);
            b_qqid.CopyTo(contData, b_uid.Length);
            b_email.CopyTo(contData, b_uid.Length + b_qqid.Length);
            b_moblile.CopyTo(contData, b_uid.Length + b_qqid.Length + b_email.Length);
            b_status.CopyTo(contData, b_uid.Length + b_qqid.Length + b_email.Length + b_moblile.Length);
            b_regTime.CopyTo(contData, b_uid.Length + b_qqid.Length + b_email.Length + b_moblile.Length + b_status.Length);
            b_updateTime.CopyTo(contData, b_uid.Length + b_qqid.Length + b_email.Length + b_moblile.Length + b_status.Length + b_regTime.Length);
            sign.CopyTo(contData, b_uid.Length + b_qqid.Length + b_email.Length + b_moblile.Length + b_status.Length + b_regTime.Length + b_updateTime.Length);

            //[4]内容长度
            byte[] cont_len = UDP.GetByteFromInt(contData.Length);
            //[6]包序列号
            byte[] seq = UDP.GetByteFromInt(0);

            //<2>发送包
            byte[] data = new byte[v.Length + needr.Length + cmd_type.Length + cont_len.Length + contData.Length + seq.Length];
            v.CopyTo(data, 0);
            needr.CopyTo(data, v.Length);
            cmd_type.CopyTo(data, v.Length + needr.Length);
            cont_len.CopyTo(data, v.Length + needr.Length + cmd_type.Length);
            contData.CopyTo(data, v.Length + needr.Length + cmd_type.Length + cont_len.Length);
            seq.CopyTo(data, v.Length + needr.Length + cmd_type.Length + cont_len.Length + contData.Length);

            byte[] result = UDP.GetUDPReply(data, ip, port);

            byte[] m_dwRetCode = new byte[4] { result[5], result[6], result[7], result[8] };
            int dwRetCode = UDP.GetIntFromByte(m_dwRetCode);
            if (dwRetCode == 0)
            {
                //手机更新成功
            }
            else
            {
                byte[] m_dwContentLen = new byte[4] { result[9], result[10], result[11], result[12] };
                int dwContentLen = UDP.GetIntFromByte(m_dwContentLen);
                if (dwContentLen > 0)
                {
                    byte[] m_sContent = new byte[dwContentLen];
                    for (int i = 0; i < dwContentLen; i++)
                    {
                        m_sContent[i] = result[13 + i];
                    }
                    string sContent = Encoding.Default.GetString(m_sContent);//错误信息
                    //   sContent = System.Text.RegularExpressions.Regex.Match(sContent, @"[^\\\0]*").Value;
                    System.Text.RegularExpressions.Regex re = new System.Text.RegularExpressions.Regex(@"([^A-Za-z\u4E00-\u9FA5])", System.Text.RegularExpressions.RegexOptions.Multiline | System.Text.RegularExpressions.RegexOptions.ExplicitCapture);
                    System.Text.RegularExpressions.MatchCollection mc = re.Matches(sContent);
                    foreach (Match ma in mc)
                    {
                        sContent = sContent.Replace(ma.Value, "");
                    }
                }
            }
        }

        protected  UInt32 GetStamp(DateTime dt)
        {
            TimeSpan ts = dt - TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            UInt32 uiStamp = Convert.ToUInt32(ts.TotalSeconds);
            return uiStamp;
        }

        //测试QQTips
        private void SendQQTips(string ip, int port, string msg)
        {
            //[1]协议 1
            byte[] v = new byte[1] { 0x02 };
            //[2]是否需要返回结果 0不需要
            byte[] needr = UDP.GetByteFromInt(0);
            //[3]服务类型 CMD_EMAIL_MSG_ON 33
            byte[] cmd_type = UDP.GetByteFromInt(0);
            //[5]内容
            //{1}用户内部ID 1直发模式 0使用uid
            byte[] b_uid = UDP.GetByteFromInt(1);
            //{2}发送掩码  0x00000002
            byte[] b_mask = UDP.GetByteFromInt(0x00000007);
            //{4}消息
            byte[] b_msg = System.Text.Encoding.GetEncoding("GB2312").GetBytes(msg);
            //{3}消息长度
            byte[] b_msg_len = UDP.GetByteFromInt(b_msg.Length);

            //<1>对内容组包
            byte[] contData = new byte[b_uid.Length + b_mask.Length + b_msg_len.Length + b_msg.Length];
            b_uid.CopyTo(contData, 0);
            b_mask.CopyTo(contData, b_uid.Length);
            b_msg_len.CopyTo(contData, b_uid.Length + b_mask.Length);
            b_msg.CopyTo(contData, b_uid.Length + b_mask.Length + b_msg_len.Length);

            //[4]内容长度
            byte[] cont_len = UDP.GetByteFromInt(contData.Length);
            //[6]包序列号
            byte[] seq = UDP.GetByteFromInt(0);

            //<2>发送包
            byte[] data = new byte[v.Length + needr.Length + cmd_type.Length + cont_len.Length + contData.Length + seq.Length];
            v.CopyTo(data, 0);
            needr.CopyTo(data, v.Length);
            cmd_type.CopyTo(data, v.Length + needr.Length);
            cont_len.CopyTo(data, v.Length + needr.Length + cmd_type.Length);
            contData.CopyTo(data, v.Length + needr.Length + cmd_type.Length + cont_len.Length);
            seq.CopyTo(data, v.Length + needr.Length + cmd_type.Length + cont_len.Length + contData.Length);

             UDP.SendUDP(data, ip, port);

        }

        //测试短信
        private void SendMessage(string ip, int port, string msg)
        {
            //[1]协议 1
            byte[] v = new byte[1] { 0x02 };
            //[2]是否需要返回结果 0不需要
            byte[] needr = UDP.GetByteFromInt(0);
            //[3]服务类型 CMD_EMAIL_MSG_ON 33
            byte[] cmd_type = UDP.GetByteFromInt(0);
            //[5]内容
            //{1}用户内部ID 1直发模式 0使用uid
            byte[] b_uid = UDP.GetByteFromInt(1);
            //{2}发送掩码  0x00000002
            byte[] b_mask = UDP.GetByteFromInt(0x00000007);
            //{4}消息
            byte[] b_msg = System.Text.Encoding.GetEncoding("GB2312").GetBytes(msg);
            //{3}消息长度
            byte[] b_msg_len = UDP.GetByteFromInt(b_msg.Length);

            //<1>对内容组包
            byte[] contData = new byte[b_uid.Length + b_mask.Length + b_msg_len.Length + b_msg.Length];
            b_uid.CopyTo(contData, 0);
            b_mask.CopyTo(contData, b_uid.Length);
            b_msg_len.CopyTo(contData, b_uid.Length + b_mask.Length);
            b_msg.CopyTo(contData, b_uid.Length + b_mask.Length + b_msg_len.Length);

            //[4]内容长度
            byte[] cont_len = UDP.GetByteFromInt(contData.Length);
            //[6]包序列号
            byte[] seq = UDP.GetByteFromInt(0);

            //<2>发送包
            byte[] data = new byte[v.Length + needr.Length + cmd_type.Length + cont_len.Length + contData.Length + seq.Length];
            v.CopyTo(data, 0);
            needr.CopyTo(data, v.Length);
            cmd_type.CopyTo(data, v.Length + needr.Length);
            cont_len.CopyTo(data, v.Length + needr.Length + cmd_type.Length);
            contData.CopyTo(data, v.Length + needr.Length + cmd_type.Length + cont_len.Length);
            seq.CopyTo(data, v.Length + needr.Length + cmd_type.Length + cont_len.Length + contData.Length);

            UDP.SendUDP(data, ip, port);

           
        }

        //测试邮件
        private void SendEmail(string ip, int port, string msg)
        {
            //[1]协议 1
            byte[] v = new byte[1] { 0x02 };
            //[2]是否需要返回结果 0不需要
            byte[] needr = UDP.GetByteFromInt(0);
            //[3]服务类型 CMD_EMAIL_MSG_ON 33
            byte[] cmd_type = UDP.GetByteFromInt(0);
            //[5]内容
            //{1}用户内部ID 1直发模式 0使用uid
            byte[] b_uid = UDP.GetByteFromInt(1);
            //{2}发送掩码  0x00000002
            byte[] b_mask = UDP.GetByteFromInt(0x00000007);
            //{4}消息
            byte[] b_msg = System.Text.Encoding.GetEncoding("GB2312").GetBytes(msg);
            //{3}消息长度
            byte[] b_msg_len = UDP.GetByteFromInt(b_msg.Length);

            //<1>对内容组包
            byte[] contData = new byte[b_uid.Length + b_mask.Length + b_msg_len.Length + b_msg.Length];
            b_uid.CopyTo(contData, 0);
            b_mask.CopyTo(contData, b_uid.Length);
            b_msg_len.CopyTo(contData, b_uid.Length + b_mask.Length);
            b_msg.CopyTo(contData, b_uid.Length + b_mask.Length + b_msg_len.Length);

            //[4]内容长度
            byte[] cont_len = UDP.GetByteFromInt(contData.Length);
            //[6]包序列号
            byte[] seq = UDP.GetByteFromInt(0);

            //<2>发送包
            byte[] data = new byte[v.Length + needr.Length + cmd_type.Length + cont_len.Length + contData.Length + seq.Length];
            v.CopyTo(data, 0);
            needr.CopyTo(data, v.Length);
            cmd_type.CopyTo(data, v.Length + needr.Length);
            cont_len.CopyTo(data, v.Length + needr.Length + cmd_type.Length);
            contData.CopyTo(data, v.Length + needr.Length + cmd_type.Length + cont_len.Length);
            seq.CopyTo(data, v.Length + needr.Length + cmd_type.Length + cont_len.Length + contData.Length);

            UDP.SendUDP(data, ip, port);
        }

        //test查询商户联系信息
        private void TestQuerySPContactInfo()
        {
            MerchantService mer = new MerchantService();
            DataSet tb = mer.QuerySPContactInfo("2000000501");
        }




        private void testAPI(){
            //以测试环境url创建HttpWebRequest
            System.Net.HttpWebRequest httpWebRequest = (System.Net.HttpWebRequest)System.Net.WebRequest
                .Create("http://localhost:61131/CSAPI/BaseInfoService.asmx/GetUserAppealList?appid=10001&uin=453294603&start_date=2014-01-01%2000:00:00&end_date=2014-08-28%2000:00:00&state=99&type=99&qqtype=&dotype=9&offset=0&limit=10&token=f099b4989238ef1e4af4530f83cd6b33");
            //设置HttpWebRequest头信息
            httpWebRequest.Method = "GET";
            httpWebRequest.Timeout = 600000;
            httpWebRequest.ServicePoint.Expect100Continue = false;
            httpWebRequest.CachePolicy = new System.Net.Cache.HttpRequestCachePolicy(System.Net.Cache.HttpRequestCacheLevel.NoCacheNoStore);
            httpWebRequest.ContentType = "application/xml; charset=gb2312";
            httpWebRequest.Accept = "application/xml";

            //获取响应输入流
            System.Net.WebResponse webResponse = httpWebRequest.GetResponse();
            System.IO.Stream responseStream = webResponse.GetResponseStream();
            //读取返回流  
            var myStreamReader = new System.IO.StreamReader(responseStream, Encoding.GetEncoding("gb2312"));
            string result = myStreamReader.ReadToEnd();
            result = HttpUtility.UrlDecode(result);
            //关闭请求连接  
            myStreamReader.Close();

        }
    }

    

}
