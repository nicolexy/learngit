using System;
using System.Collections;
using TENCENT.OSS.C2C.Finance;

namespace TENCENT.OSS.CFT.KF.KF_Service
{
	/// <summary>
	/// FinanceManage 的摘要说明。
	/// </summary>
	public class FinanceManage
	{
		public FinanceManage()
		{
			//
			// TODO: 在此处添加构造函数逻辑
			//
		}



		
		/// <summary>
		/// 修改密码操作
		/// </summary>
		/// <param name="qqid">用户的QQ号</param>
		/// <param name="mail">取回的邮箱</param>
		/// <param name="sign">取回成功标志</param>
		/// <param name="reason">取回的原因</param>
		/// <param name="pathUinfo">用户基本信息图片地址</param>
		/// <param name="AccInfo">用户帐户信息图片地址</param>
		/// <param name="IDCardInfo">身份证图片地址</param>
		/// <param name="BankCardInfo">银行卡图片地址</param>
		/// <returns></returns>
		/// <returns></returns>
		public static bool modifyPwd(string qqid,string mail,string reason,string cleanMimao,string pathUinfo,
			string AccInfo,string IDCardInfo,string BankCardInfo,string uid,string uip,out string Msg)
		{
			//验证用户是否注册
			Msg = null;
			//furion 20061120 email登录修改
			string fuid = PublicRes.ConvertToFuid(qqid);

			/*
			//string strRegist = "Select count(1) from " + PublicRes.GetTableName("t_user",qqid) + " where fqqid = '" + qqid + "'" ;
			//string strRegist = "Select count(1) from " + PublicRes.GetTName("t_user",fuid) + " where fuid =" + fuid  ;
			string strRegist = "Select count(1) from " + PublicRes.GetTName("t_user_info",fuid) + " where fuid =" + fuid  ;

			//string iCount	 = PublicRes.ExecuteOne(strRegist,"YW_30");
			//string iCount	 = PublicRes.ExecuteOne(strRegist,"ywb_30");
			string iCount	 = PublicRes.ExecuteOne(strRegist,"ZL_30");
			*/

			string strsql = "uid=" + fuid;
			string errMsg = "";
			string icount = TENCENT.OSS.C2C.Finance.Common.CommLib.CommQuery.GetOneResultFromICE(strsql,
				TENCENT.OSS.C2C.Finance.Common.CommLib.CommQuery.QUERY_USERINFO,"fuid",out errMsg);

			if (icount == null || icount == "0")
			{
				Msg = "对不起，用户没有注册！";
				return false;
			}

			//如果验证正确,生成密码，并且修改用户的密码和密保（根据用户的要求决定）
			try
			{
				string pwdStr = makePwd();
				
				//修改密码(MD5)
				string modifyPwdStr = null; 
				string content = "您好！你的财付通支付密码申诉已通过，新的密码为： " + pwdStr + "     [为了您的密码安全，密码可能比较复杂，并且不支持复制粘贴，请妥善保管好您的密码]"; //邮件发送

				//以md5的方式加密生成的密码
				pwdStr = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(pwdStr,"md5").ToLower();


				//新格式如下． md5(md5(支付密码)统一转小写 + uid)_
				pwdStr += fuid;
				pwdStr = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(pwdStr,"md5").ToLower() + "_";
				/*
				if (cleanMimao.ToLower() == "false")
				{
					// TODO: 1客户信息资料外移
					//furion 20061120 email登录修改
					//modifyPwdStr = "update " + PublicRes.GetTableName("t_user_info",qqid) + " SET Fpasswd = '" + pwdStr + "' where fqqid ='" + qqid + "'";
					modifyPwdStr = "update " + PublicRes.GetTName("t_user_info",fuid) + " SET Fpasswd = '" + pwdStr + "' where fuid =" + fuid ;
				}
				else if (cleanMimao.ToLower() == "true")  //清空密码保护
				{
					//modifyPwdStr = "update " + PublicRes.GetTableName("t_user_info",qqid) + " SET Fpasswd = '" + pwdStr 
					modifyPwdStr = "update " + PublicRes.GetTName("t_user_info",fuid) + " SET Fpasswd = '" + pwdStr 
						+ "',Fquestion1 = null,Fanswer1 = null,Fquestion2 = null,Fanswer2 = null" 
						//+ " where fqqid ='" + qqid + "'";
						+ " where fuid =" + fuid ;

					content += "   <br>您的密码保护问题已被清空，请登陆重新设置您的密码保护问题！";
				}
				else
				{
					Msg = "是否清除密保为空！~，请检查！";
					return false;
				}

				//PublicRes.ExecuteSql(modifyPwdStr,"YW_30");
				PublicRes.ExecuteSql(modifyPwdStr,"ZL_30");
				
				*/

				string strSql = "uid=" + fuid;
				strSql += "&modify_time=" +  PublicRes.strNowTimeStander;
				strSql += "&passwd=" + pwdStr;
                strSql += "&pass_flag=0";

				if (cleanMimao.ToLower() == "true") 
				{
					strSql += "&question1=";
					strSql += "&answer1=";
					strSql += "&question2=";
					strSql += "&answer2=";
				}

				int iresult = TENCENT.OSS.C2C.Finance.Common.CommLib.CommQuery.ExecSqlFromICE(strSql
					,TENCENT.OSS.C2C.Finance.Common.CommLib.CommQuery.UPDATE_USERINFO,out Msg);

				if(iresult != 1)
				{
					return false;
				}

				//清除cache
				if (!PublicRes.ReleaseCache(qqid,"qqid"))
				{
					TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.sendLog4Log("FiannceManage.ModifyPwd"
						,"修改用户密码时，清除用户"+ qqid + "cache失败！请检查！");
				}

				//PublicRes.WriteFile("qqid:" + qqid + ",MD5:" + pwdStr); 这里日志不能够记录
				PublicRes.CloseFile();

				//发送邮件
				string mailFrom = null;
				try
				{
					mailFrom = System.Configuration.ConfigurationManager.AppSettings["OutMailFrom"].ToString().Trim();	
				}
				catch
				{
					Msg = "获取邮件发送人失败！ 请检查Service的Webconfig文件， mailFrom是否存在！";
					return false;
				}
					
				string subject = "腾讯财付通密码取回！";

				string err = null;
				string type = "out"; //外部邮件

				//邮件发送函数
				bool mailSign = PublicRes.sendMail(mail,mailFrom,subject,content,type,out err);
				
				if (mailSign == false)
				{
					Msg = "邮件发送失败！请重新发送！" + err;
					return false;
				}
				else if (mailSign == true)
				{
					Msg = "邮件发送成功！";
					return true;
				}

				return true;
			}
			catch(Exception e)
			{
				Msg = "修改密码失败！" +e.Message.ToString().Replace("'","’");
				return false;
			}
		}



		public static string makePwd()
		{
			string str1 = "ABCDEFGHIJKLMNPQRSTUVWXYZabcdefghjkmnpqrstuvwxyz";   //数据字典大写ABCDEF + 小写ghijklmnopqrstuvwxyz
			int iStr1 = str1.Length;
			
			string str2 = "2345678998765432";                   //数据字典特殊字符 + 数字逆序
			int iStr2 = str2.Length;

			string str3 = "abcdefghjkmnpqrstuvwxyzABCDEFGHIJKLMNPQRSTUVWXYZ";   //数据字典小写abcdef + 大写GHIJKLMNOPQRSTUVWXYZ
			int iStr3 = str3.Length;

			string str4 = "2345678998765432";   //数据字典小写abcdef + 大写GHIJKLMNOPQRSTUVWXYZ
			int iStr4 = str4.Length;

			string s= null;

			ArrayList al = new ArrayList();
			al.Add(str1);
			al.Add(str2);
			al.Add(str3);
			al.Add(str4);
			
			for (int i = 0;i<4; i++)
			{
				s += pwd(al[i].ToString());  //从每个数据字典中随机取出2位，组成12位随机密码
			}

			return s;

		}



		private static string pwd(string str)
		{
			int ln = str.Length;
			
			System.Random rd = new Random();
			
			string pwd = null;
			
			for (int i = 0; i< 2; i++)
			{
				System.Threading.Thread.Sleep(1);
				int j = rd.Next(str.Length);
				pwd += str[j];
			}

			return pwd;
		}
	


	}
}
