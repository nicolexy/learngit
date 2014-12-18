using System;
using System.Collections;
using TENCENT.OSS.C2C.Finance;

namespace TENCENT.OSS.CFT.KF.KF_Service
{
	/// <summary>
	/// FinanceManage ��ժҪ˵����
	/// </summary>
	public class FinanceManage
	{
		public FinanceManage()
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
		}



		
		/// <summary>
		/// �޸��������
		/// </summary>
		/// <param name="qqid">�û���QQ��</param>
		/// <param name="mail">ȡ�ص�����</param>
		/// <param name="sign">ȡ�سɹ���־</param>
		/// <param name="reason">ȡ�ص�ԭ��</param>
		/// <param name="pathUinfo">�û�������ϢͼƬ��ַ</param>
		/// <param name="AccInfo">�û��ʻ���ϢͼƬ��ַ</param>
		/// <param name="IDCardInfo">���֤ͼƬ��ַ</param>
		/// <param name="BankCardInfo">���п�ͼƬ��ַ</param>
		/// <returns></returns>
		/// <returns></returns>
		public static bool modifyPwd(string qqid,string mail,string reason,string cleanMimao,string pathUinfo,
			string AccInfo,string IDCardInfo,string BankCardInfo,string uid,string uip,out string Msg)
		{
			//��֤�û��Ƿ�ע��
			Msg = null;
			//furion 20061120 email��¼�޸�
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
				Msg = "�Բ����û�û��ע�ᣡ";
				return false;
			}

			//�����֤��ȷ,�������룬�����޸��û���������ܱ��������û���Ҫ�������
			try
			{
				string pwdStr = makePwd();
				
				//�޸�����(MD5)
				string modifyPwdStr = null; 
				string content = "���ã���ĲƸ�֧ͨ������������ͨ�����µ�����Ϊ�� " + pwdStr + "     [Ϊ���������밲ȫ��������ܱȽϸ��ӣ����Ҳ�֧�ָ���ճ���������Ʊ��ܺ���������]"; //�ʼ�����

				//��md5�ķ�ʽ�������ɵ�����
				pwdStr = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(pwdStr,"md5").ToLower();


				//�¸�ʽ���£� md5(md5(֧������)ͳһתСд + uid)_
				pwdStr += fuid;
				pwdStr = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(pwdStr,"md5").ToLower() + "_";
				/*
				if (cleanMimao.ToLower() == "false")
				{
					// TODO: 1�ͻ���Ϣ��������
					//furion 20061120 email��¼�޸�
					//modifyPwdStr = "update " + PublicRes.GetTableName("t_user_info",qqid) + " SET Fpasswd = '" + pwdStr + "' where fqqid ='" + qqid + "'";
					modifyPwdStr = "update " + PublicRes.GetTName("t_user_info",fuid) + " SET Fpasswd = '" + pwdStr + "' where fuid =" + fuid ;
				}
				else if (cleanMimao.ToLower() == "true")  //������뱣��
				{
					//modifyPwdStr = "update " + PublicRes.GetTableName("t_user_info",qqid) + " SET Fpasswd = '" + pwdStr 
					modifyPwdStr = "update " + PublicRes.GetTName("t_user_info",fuid) + " SET Fpasswd = '" + pwdStr 
						+ "',Fquestion1 = null,Fanswer1 = null,Fquestion2 = null,Fanswer2 = null" 
						//+ " where fqqid ='" + qqid + "'";
						+ " where fuid =" + fuid ;

					content += "   <br>�������뱣�������ѱ���գ����½���������������뱣�����⣡";
				}
				else
				{
					Msg = "�Ƿ�����ܱ�Ϊ�գ�~�����飡";
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

				//���cache
				if (!PublicRes.ReleaseCache(qqid,"qqid"))
				{
					TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.sendLog4Log("FiannceManage.ModifyPwd"
						,"�޸��û�����ʱ������û�"+ qqid + "cacheʧ�ܣ����飡");
				}

				//PublicRes.WriteFile("qqid:" + qqid + ",MD5:" + pwdStr); ������־���ܹ���¼
				PublicRes.CloseFile();

				//�����ʼ�
				string mailFrom = null;
				try
				{
					mailFrom = System.Configuration.ConfigurationManager.AppSettings["OutMailFrom"].ToString().Trim();	
				}
				catch
				{
					Msg = "��ȡ�ʼ�������ʧ�ܣ� ����Service��Webconfig�ļ��� mailFrom�Ƿ���ڣ�";
					return false;
				}
					
				string subject = "��Ѷ�Ƹ�ͨ����ȡ�أ�";

				string err = null;
				string type = "out"; //�ⲿ�ʼ�

				//�ʼ����ͺ���
				bool mailSign = PublicRes.sendMail(mail,mailFrom,subject,content,type,out err);
				
				if (mailSign == false)
				{
					Msg = "�ʼ�����ʧ�ܣ������·��ͣ�" + err;
					return false;
				}
				else if (mailSign == true)
				{
					Msg = "�ʼ����ͳɹ���";
					return true;
				}

				return true;
			}
			catch(Exception e)
			{
				Msg = "�޸�����ʧ�ܣ�" +e.Message.ToString().Replace("'","��");
				return false;
			}
		}



		public static string makePwd()
		{
			string str1 = "ABCDEFGHIJKLMNPQRSTUVWXYZabcdefghjkmnpqrstuvwxyz";   //�����ֵ��дABCDEF + Сдghijklmnopqrstuvwxyz
			int iStr1 = str1.Length;
			
			string str2 = "2345678998765432";                   //�����ֵ������ַ� + ��������
			int iStr2 = str2.Length;

			string str3 = "abcdefghjkmnpqrstuvwxyzABCDEFGHIJKLMNPQRSTUVWXYZ";   //�����ֵ�Сдabcdef + ��дGHIJKLMNOPQRSTUVWXYZ
			int iStr3 = str3.Length;

			string str4 = "2345678998765432";   //�����ֵ�Сдabcdef + ��дGHIJKLMNOPQRSTUVWXYZ
			int iStr4 = str4.Length;

			string s= null;

			ArrayList al = new ArrayList();
			al.Add(str1);
			al.Add(str2);
			al.Add(str3);
			al.Add(str4);
			
			for (int i = 0;i<4; i++)
			{
				s += pwd(al[i].ToString());  //��ÿ�������ֵ������ȡ��2λ�����12λ�������
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
