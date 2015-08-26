using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;
using System.IO;
using System.Text;
using System.Web.Services.Protocols;
using System.EnterpriseServices;
using System.Configuration;
using TENCENT.OSS.CFT.KF.DataAccess;
using TENCENT.OSS.C2C.Finance.DataAccess;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.C2C.Finance.Common;
using TENCENT.OSS.C2C.Finance.Common.CommLib;


namespace TENCENT.OSS.CFT.KF.KF_Service
{
	/// <summary>
	/// FinanceManage ��ժҪ˵����
	/// </summary>
	public class Finance_Manage : System.Web.Services.WebService
	{
		public Finance_Header myHeader;

		//static �ֶ���������һЩ��Ҫ�Ĺ��õ�״̬
		public  static int j;           
		public  static int ErrorNo;     //Ĭ��Ϊ��δ�ҵ�ƥ�����ݵļ�¼����
		public  static int TypeOfErrorDetail; //���ʽ�������ֱ�����ʽ�����ݲ�ͬ��ֵ����ת����ͬ��ҳ�����չ��
		private static string BatchNo; //���ö�������ִ��ʱ������κ�
		private static string bankType; //��������
		private static long iMaxTaskID; 
		private static int serialNo;    //�����и����������͡�һ��Ψһȷ��һ�ֶ�������ı�ʶ
	

		public Finance_Manage()
		{
			//CODEGEN: �õ����� ASP.NET Web ����������������
			InitializeComponent();
		}     

		//����SQL��䷵��DataSet
		public DataSet getDataSet(string selectStr,int istr,int imax,string dbStr)
		{
			try
			{
				return QueryInfo.GetTable(selectStr,istr,imax,dbStr);
			}
			catch(Exception e)
			{
				throw new Exception("KF_Service-Finance_Manage:getDataSet Error! " + e.Message.ToString().Replace("'","��"));
			}
		}

		#region �����������ɵĴ���
		
		//Web ����������������
		private IContainer components = null;
				
		/// <summary>
		/// �����֧������ķ��� - ��Ҫʹ�ô���༭���޸�
		/// �˷��������ݡ�
		/// </summary>
		private void InitializeComponent()
		{
		}

		/// <summary>
		/// ������������ʹ�õ���Դ��
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if(disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);		
		}
		
		#endregion
		

		
		#region	�û���Ϣ��ѯ���޸�ҳ�� 2012/5/4 ���

		[WebMethod(Description="��֤�û���QQ�������Ƿ����")]
		public bool CheckOldName(string qqid,string name,out string Msg)
		{
			Msg = null;
			qqid = qqid.Trim();
			name = name.Trim();

			string uName = null;

			//����У��
			if (qqid == null || name == null)
			{
				Msg = "����Ĳ�����������";
				return false;
			}

			try
			{
				// TODO1: �ͻ���Ϣ��������
				//furion 20061116 email��¼�޸ġ�
				string strID = PublicRes.ConvertToFuid(qqid);  //��ת����fuid

				/*
				//string selectStr = "Select FtrueName from " + PublicRes.GetTableName("t_user_info",qqid) + " where fqqid = '" + qqid + "'";
				string selectStr = "Select FtrueName from " + PublicRes.GetTName("t_user_info",strID) + " where fuid =" + strID;

				//uName = PublicRes.ExecuteOne(selectStr,"YW_30");	
				uName = PublicRes.ExecuteOne(selectStr,"ZL");	
				*/
				string selectStr = "uid=" + strID;
				uName = CommQuery.GetOneResultFromICE(selectStr,CommQuery.QUERY_USERINFO,"FtrueName",out Msg);
			}
			catch(Exception e)
			{
				Msg = "��ѯ�û��ʺź������Ƿ�һ��ʧ�ܣ�[" + e.Message.ToString().Replace("'","��") + "]";
				return false;
			}
			

			//�����Ƿ�ע��
			if (uName == null || uName == "")
			{
				Msg = "�Բ��𣬸��ʺ�û��ע�ᣡ";
				return false;
			}

			//����һ����
			if (uName.Trim() != name)
			{
				Msg = "�Բ��𣬸��ʺ�ע������������ߵ����������ϣ�";
				return false;
			}

			return true;
		}


		

		
		// 2012/5/4 ������ϵͳ��ع����ƶ���KFϵͳ
		[WebMethod(Description="�޸������͹�˾��")]
		[SoapHeader("myHeader",Direction=SoapHeaderDirection.In)]
		public bool modifyName(string QQID,string changedName,string cCompany)  //�����޸ĺ�������͹�˾����
		{
			ICEAccess ice = new ICEAccess(PublicRes.ICEServerIP,PublicRes.ICEPort);
			try
			{
				/*
				string companyStr;

				if (cCompany != null && cCompany != "")
					companyStr = "', Fcompany_name = '" + cCompany;
				else
					companyStr = "";

				// TODO1: �ͻ���Ϣ��������

				//furion 20061117 email��¼�޸�
				string fuid = PublicRes.ConvertToFuid(QQID);
				//��ͬʱ�޸�3�ű��õ����� t_user_info,t_user,t_bank_user �ֱ�Ϊ��1��2��3
				//string strCmd1 = "update " + PublicRes.GetTableName("t_user_info",QQID) + " SET Ftruename = '" + changedName +  companyStr +"' where Fqqid ='" +  QQID + "'";  //+PublicRes.GetSqlFromQQ(QQID,"fuid");
				string strCmd1 = "update " + PublicRes.GetTName("t_user_info",fuid) + " SET Ftruename = '" + changedName +  companyStr +"' where Fuid =" +  fuid ;  //+PublicRes.GetSqlFromQQ(QQID,"fuid");
				string strCmd2 = "update " + PublicRes.GetTName("t_user",fuid)      + " SET Ftruename = '" + changedName +  companyStr +"' where Fuid =" +  fuid ;
				string strCmd3 = "update " + PublicRes.GetTName("t_bank_user",fuid) + " SET Ftruename = '" + changedName +  companyStr +"' where Fuid =" +  fuid ;
			
				ArrayList al = new ArrayList();
			
				al.Add(strCmd1);
				//al.Add(strCmd2);
				al.Add(strCmd3);
				
				//���cache
				PublicRes.ReleaseCache(QQID,"qqid");
				*/


				string fuid = PublicRes.ConvertToFuid(QQID);
				string strSql = "uid=" + fuid;
				strSql += "&modify_time=" + PublicRes.strNowTimeStander;

				strSql += "&truename=" + changedName;

				if (cCompany != null && cCompany != "")
					strSql += "&company_name=" + cCompany;

				string errMsg = "";
				int iresult = CommQuery.ExecSqlFromICE(strSql,CommQuery.UPDATE_USERINFO,out errMsg);

				if(iresult != 1)
				{
					throw new LogicException("�����˷�һ����¼:" + errMsg);
				}

				//				strSql += "&curtype=1";   (t_fetch_bank �����Ѿ������� truename �� company_name) 20110125 rowenawu
				//
				//				iresult = CommQuery.ExecSqlFromICE(strSql,CommQuery.UPDATE_BANKUSER,out errMsg);

				if(iresult != 1)
				{
					throw new LogicException("�����˷�һ����¼:" + errMsg);
				}

				//furion V30_FURION�Ķ� 20090310 �Ķ�����ʱ��ICE
				//if(PublicRes.ExecuteSqlNum(strCmd2,"YW_30") != 1)
				//return false;

				ice.OpenConn();
				string strwhere = "where=" + ICEAccess.URLEncode("fcurtype=1&");
				strwhere += ICEAccess.URLEncode("fuid=" + fuid + "&");

				string strUpdate = "data=" + ICEAccess.URLEncode("ftruename=" + ICEAccess.URLEncode(changedName));

				if (cCompany != null && cCompany != "")
					strUpdate += ICEAccess.URLEncode("&fcompany_name=" + ICEAccess.URLEncode(cCompany));

				strUpdate += ICEAccess.ICEEncode("&fmodify_time=" + PublicRes.strNowTimeStander);

				string strResp = "";

				//3.0�ӿڲ�����Ҫ furion 20090708
				if(ice.InvokeQuery_Exec(YWSourceType.�û���Դ,YWCommandCode.�޸��û���Ϣ,fuid,strwhere + "&" + strUpdate,out strResp) != 0)
				{
					throw new Exception("�޸ĸ�����������" + strResp);
				}

				//return PublicRes.Execute(al,"YW_30");  //ִ�в����ؽ��
				//return PublicRes.Execute(al,"ZL");  //ִ�в����ؽ��
				return true;
			}
			catch(Exception e)
			{
				ice.CloseConn();
				throw new Exception(e.Message.ToString().Replace("'","��"));
				return false;
			}
			finally
			{
				ice.Dispose();
			}
		}

        [WebMethod(Description = "�����û����ʻ�����")]
        public bool getUserType(string qqid, out string userType, out string Msg)
        {
            userType = null;
            Msg = null;

            if (qqid == null)
            {
                Msg = "����Ĳ�����������";
                return false;
            }

            try
            {
                string strID = PublicRes.ConvertToFuid(qqid);  //��ת����fuid

                /*
                //string str = "select Fuser_type from " + PublicRes.GetTName("t_user",strID) + " where fuid = '" + strID + "'";
                string str = "select Fuser_type from " + PublicRes.GetTName("t_user_info",strID) + " where fuid = '" + strID + "'";
                //userType = PublicRes.ExecuteOne(str,"YW_30");
                userType = PublicRes.ExecuteOne(str,"ZL");

                Msg = "��ȡ�û��ʻ����ͳɹ���";
                return true;
                */

                string strSql = "uid=" + strID;
                userType = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_USERINFO, "Fuser_type", out Msg);

                if (userType != null && userType.Trim() != "")
                    return true;
                else
                    return false;
            }
            catch (Exception e)
            {
                Msg = "��ȡ�ʻ�����ʧ�ܣ�[" + e.Message.ToString().Replace("'", "��") + "]";
                return false;
            }
        }


		/// <summary>
		/// ����ȡ�ز���
		/// </summary>
		/// <param name="qqid">�û���QQ��</param>
		/// <param name="mail">ȡ�ص�����</param>
		/// <param name="cleanMimao">�Ƿ�����ܱ�</param>
		/// <param name="reason">ȡ�ص�ԭ��</param>
		/// <param name="pathUinfo">�û�������ϢͼƬ��ַ</param>
		/// <param name="AccInfo">�û��ʻ���ϢͼƬ��ַ</param>
		/// <param name="IDCardInfo">���֤ͼƬ��ַ</param>
		/// <param name="BankCardInfo">���п�ͼƬ��ַ</param>
		/// <returns></returns>
		[WebMethod(Description="����ȡ��")]
		[SoapHeader("myHeader", Direction=SoapHeaderDirection.In)]
		public bool changePwdInfo(string qqid,string mail,string cleanMimao,string reason,string pathUinfo,
			string AccInfo,string IDCardInfo,string BankCardInfo,out string Msg)
		{
			Msg = null;
			RightAndLog rl = new RightAndLog();
			try
			{
				if(myHeader == null)
				{
					throw new Exception("����ȷ�ĵ��÷�����");
				}

				//				rl.actionType = "����ȡ��";
				//				rl.ID = SPID;
				//				rl.OperID = myHeader.OperID;
				//				rl.sign = 1;
				//				rl.strRightCode = "ChangePassword";
				//				rl.RightString = myHeader.RightString;
				//				rl.SzKey = myHeader.SzKey;
				//				rl.type = "�ʻ������޸�";
				//				rl.UserID = myHeader.UserName;
				//				rl.UserIP = myHeader.UserIP;				
				//				if(!rl.CheckRight())
				//				{
				//					Msg = "�û�Ȩ�޲��㣬���ܽ��С�����ȡ�ز�����";
				//					return false;
				//				}

				//дҵ����û�ִ��������ȡ�صĲ��������������ṩ��ѯ


				if (!PublicRes.ReleaseCache(qqid,"qqid"))
				{
					TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.sendLog4Log("FiannceManage.ModifyPwd"
						,"�޸��û�����ʱ��Ԥ����û�"+ qqid + "cacheʧ�ܣ����飡");
					return false;
				}

				//ȡ���������,����ͬ��
				bool pwdSign = FinanceManage.modifyPwd(qqid, mail,reason,cleanMimao, pathUinfo,AccInfo, 
					IDCardInfo, BankCardInfo,myHeader.UserName,myHeader.UserIP,out Msg);
				
				if (pwdSign == false)
				{
					//д������־
					PublicRes.WriteFile("�޸����루�ܱ���ʧ�ܣ�[" + qqid +"]" + Msg);
					//дʧ�ܵ�ҵ����־
					return false;
				}
				else if (pwdSign == true)
				{
					//д������־
					PublicRes.WriteFile("�޸����루�ܱ����ɹ���[" + qqid +"]" + Msg);
					//д�ɹ���ҵ����־
					

					return true;	
				}

				return true;
				
			}
			catch(Exception e)
			{
				Msg = "ִ�г���[" + e.Message.ToString().Replace("'","��") + "]";
				return false;
			}

		}



		
		[WebMethod(Description="�����ٲñ���ص��ٲ���Ϣ")]
		public bool insertMediation(string qqid,string mail,string cleanMimao,string reason,string uid,string fetchNO,string commTime,
			string pathAcc,string pathBase,string pathIDCard,string pathBank,string commitTime,string fnameNew,string FIDCardNew,out string Msg)
		{
			string fName  = null;
			string fbankNo= null;
			Msg = null;
 
			//��ȡ��ص���Ϣ
			try
			{
				bool exeSign = getAccInfo(qqid,out fName,out fbankNo,out Msg);		
				if (exeSign == false)
				{
					return false;
				}
			}
			catch(Exception e)
			{
				Msg = Msg + "[" +e.Message.ToString().Replace("'","��") + "]";
				return false;
			}
			
			//�������ݿ�
			try
			{
				string insertStr = "Insert c2c_fmdb.t_mediation (FfetchID,FName,Fqqid,FbankNO,fCleanMibao,FfetchMail,Freason,FBasePath,FaccPath,FIDCardPath,FbankCardPath,FNameNew,FIDCardNew,FcommitTime,FlastModifyTime) VALUES ( '" 
					+ fetchNO + "','"
					+ fName   + "','"
					+ qqid   + "','"
					+ fbankNo + "','"
					+ cleanMimao + "','"
					+ mail    + "','"
					+ reason  + "','"
					+ pathBase+ "','"
					+ pathAcc + "','"
					+ pathIDCard + "','"
					+ pathBank   + "','"
					+ fnameNew   + "','"
					+ FIDCardNew + "','"
					+ commitTime + "',"
					+ PublicRes.strNowTime
					+")";
				PublicRes.ExecuteSql(insertStr,"ht");
				return true;
			}
			catch(Exception eStr)
			{
				string strMsg = "�����ٲñ����ʧ�ܣ�";
				Msg = strMsg + "[" + eStr.Message.ToString().Replace("'","��") + "]";
				return false;
			}	
		}
		



		[WebMethod(Description="����QQ�Ż�ȡ�����������ʺ�")]
		public bool getAccInfo(string qqid,out string name,out string bankNo,out string Msg)
		{
			name   = null;
			bankNo = null;
			Msg    = null;

			try
			{
				string fuid = PublicRes.ConvertToFuid(qqid);
				if(fuid == null)
					fuid = "0";

				/*
				// TODO1: �ͻ���Ϣ��������
				//furion 20061116 email��¼�޸�
				//string str = "Select FtrueName,FbankID From " + PublicRes.GetTableName("t_bank_user",qqid) + " where fqqid = '" + qqid + "'";
				string str = "Select FtrueName,FbankID From " + PublicRes.GetTName("t_bank_user",fuid) + " where fuid =" + fuid ;

				*/
				string [] ar = new string[1];
				ar[0] = "FbankID";
				

				/*
				//ar = PublicRes.returnDrData(str,ar,"YW_30");
				ar = PublicRes.returnDrData(str,ar,"ZL");
				*/
				string strSql = "uid=" + fuid;
				strSql += "&curtype=1";

				ar = CommQuery.GetdrDataFromICE(strSql,CommQuery.QUERY_BANKUSER,ar,out Msg);

				string strSql1 = "uid=" + fuid;
				name = CommQuery.GetOneResultFromICE(strSql1,CommQuery.QUERY_USERINFO,"Ftruename",out Msg);

				if(ar!=null && ar.Length>0)
				{
					bankNo = ar[0].ToString().Trim();	
				}
				
				Msg = "��ȡ�����������ʺųɹ���";
				return true;
			}
			catch(Exception e)
			{
				Msg = "����QQ�����ȡ�����������ʺ�ʧ�ܣ�" + e.Message.ToString().Replace("'","��");
				return false;
			}
		}




		[WebMethod(Description="����û��Ƿ�ע���������")]
		public bool checkUserReg(string qqid,out string Msg)
		{
			Msg = null;

			try
			{
				//furion 20061115 email��¼���
				if(qqid == null || qqid.Trim().Length < 3)
				{
					Msg = "�ʺŲ���Ϊ��";
					return false;
				}

				//start
				string qq = qqid.Trim();
				string uid3 = "";
				try
				{
					long itmp = long.Parse(qq);
					uid3 = qq;
				}
				catch
				{
					if(!Common.DESCode.GetEmailUid(qq,out uid3))
					{
						Msg = "����EMAILʱ����" + uid3;
						return false;
					}
				}
				//end	
				/*
				//����û�ע���ˣ����Ǵ�������״̬��Ҳ���ܹ�ִ���κβ���
				string ralationStr = "select Fsign from " + PublicRes.GetTName("t_relation",uid3) + " where Fqqid='" + qq +"'";
				
				//t_relation�Ƶ��û����Ͽ⡣furion 20090302
				//string relaSign    = PublicRes.ExecuteOne(ralationStr,"YW_30");
				string relaSign    = PublicRes.ExecuteOne(ralationStr,"ZL");

				*/
						
				string strSql = "uin=" + qq;		
				string relaSign = CommQuery.GetOneResultFromICE(strSql,CommQuery.QUERY_RELATION,"Fsign",out Msg);

				if (relaSign == null || relaSign.Trim() == "")
				{
					Msg = "���ʺ�û��ע�ᣡ";
					return false;
				}
 
				if (relaSign == "2")
				{
					Msg = "���ʻ�״̬Ϊ����״̬�����ܹ������κβ�����";
					return false;
				}
				else if (relaSign != "1")
				{
					Msg = "���˻�״̬��־("+ relaSign +")������������ϵ����Ա�쿴��";
					return false;
				}

				/*
				string uidStr = "select Fuid from " + PublicRes.GetTName("t_relation",uid3) + " where Fqqid='" + qq +"'";
				
				//furion 20090302 t_relation�Ƶ��û����Ͽ��
				//Msg           = PublicRes.ExecuteOne(uidStr,"YW_30").Trim();
				Msg           = PublicRes.ExecuteOne(uidStr,"ZL").Trim();
				*/

				string errMsg = "";
				strSql = "uin=" + qq;		
				Msg = CommQuery.GetOneResultFromICE(strSql,CommQuery.QUERY_RELATION,"Fuid",out errMsg);

				return true;	
			}
			catch(Exception e)
			{
				Msg = "����û��Ƿ�ע���������ʧ�ܣ�" + PublicRes.replaceHtmlStr(e.Message.ToString());
				return false;
			}
		}

        [WebMethod(Description = "���Uid�Ƿ���� ��userinfo�У�����t_relation��")]
        public bool CheckRecoverUid(string uid, string recoverQQid, out string Msg, out string type)
        {
            Msg = null;
            type = "";

            try
            {

                if (uid == null || uid.Trim().Length < 3)
                {
                    Msg = "�ʺŲ���Ϊ��";
                    return false;
                }



                //				string errMsg = "";
                //				string strSql = "uid=" + uid;
                //				string qqid = CommQuery.GetOneResultFromICE(strSql,CommQuery.QUERY_USERINFO,"Fqqid",out errMsg);
                //
                //				if(qqid==null||qqid=="")
                //				{
                //					Msg="δ��ѯ��"+uid+"��Ӧ��QQId";
                //					return false;
                //				}

                //��Ϊ֧������� 20111229
                string errMsg = "";
                string strSql = "uid=" + uid;
                DataSet ds = CommQuery.GetDataSetFromICE(strSql, CommQuery.QUERY_USERINFO, out errMsg);

                string qqid_query = "";
                string mobile_query = "";
                string email_query = "";
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count != 1)
                {
                    Msg = "δ��ѯ��" + uid + "��Ӧ��QQId";
                    return false;
                }
                else
                {
                    qqid_query = ds.Tables[0].Rows[0]["Fqqid"].ToString();
                    mobile_query = ds.Tables[0].Rows[0]["fmobile"].ToString();
                    email_query = ds.Tables[0].Rows[0]["femail"].ToString();
                    if (qqid_query == recoverQQid ||
                        mobile_query == recoverQQid ||
                        email_query == recoverQQid)
                    {
                        if (mobile_query == recoverQQid && recoverQQid.Length == 11 && qqid_query == recoverQQid)
                        {
                            type = "mobile";
                            return true;
                        }
                        else if (email_query == recoverQQid && recoverQQid.IndexOf("@") > 0)
                        {
                            type = "email";
                            return true;
                        }
                        else if (qqid_query == recoverQQid)
                        {
                            type = "qq";
                            return true;
                        }
                        Msg = "��Ҫ�ָ����ʺ�" + recoverQQid + "�ʺ�����δ֪��";
                        return false;
                    }
                    else
                    {
                        Msg = "��Ҫ�ָ����ʺ�" + recoverQQid + "�ڲ�ID��Ϊ��" + uid;
                        return false;
                    }
                }


                //				string strSql = "uin=" + qqid;		
                //				string fuid = CommQuery.GetOneResultFromICE(strSql,CommQuery.QUERY_RELATION,"Fuid",out Msg);
                //
                //				if (fuid!=null&&fuid!="")
                //				{
                //					Msg=fuid+"�Ѿ�����RELATION���У�";
                //					return false;
                //				}

                return true;
            }
            catch (Exception e)
            {
                Msg = e.Message;
                return false;
            }
        }

		#endregion



		[WebMethod(Description="���׹���_���׵��������")]
		[SoapHeader("myHeader",Direction=SoapHeaderDirection.In)] 
		public bool freezeTrade(string listID,string flstate) //���뽻�׵��ź�Ҫ�޸ĵ�״̬ 1:���� 2 ���� 3 ����
		{
			//furion 20051227 �ͷ�ϵͳֻ������
			if(flstate != "1")
			{
				//throw new Exception("��֧�ִ˹���");
				//furion 20080604 ���ڿ�����
			}

			if(myHeader == null)
			{
				throw new Exception("����ȷ�ĵ��÷�����");
			}
			string strUserID = myHeader.UserName;
			string strIP = myHeader.UserIP;
			string strRightCode = "ModifyPayList";
			//���ȸ���listID�ֱ�ȡ����Һ����ҵĽ��ױ���
			ICEAccess ice = new ICEAccess(PublicRes.ICEServerIP,PublicRes.ICEPort);
			try
			{ 
				// TODO1: furion ���ݿ��Ż� 20080111
				//furion need ****20070509 �����Ƿ���Ҫ����
				//������׵�����Ϊ����״̬����������ж���������
				//				string strLst = "select flstate from " + PublicRes.GetTName("t_tran_list",listID) + " WHERE FlistID = '" + listID +"'";
				//				string lstSign= PublicRes.ExecuteOne(strLst,"yw_30");
				
				/*
				string strLst = "select flstate from " + PublicRes.GetTName("t_order",listID) + " WHERE FlistID = '" + listID +"'";
				string lstSign= PublicRes.ExecuteOne(strLst,"ZJ");
				*/

				string errMsg = "";
				string strLst = "listid=" + listID;
				string lstSign = CommQuery.GetOneResultFromICE(strLst,CommQuery.QUERY_ORDER,"flstate",out errMsg);

				if (lstSign == "3") //����״̬
				{
					throw new Exception("���׵�����״̬����������ж���������!");
					return false;
				}


				//ע��Ҫͬʱ������������Ľ��׵���״̬ 1:���� 2 ���� 3 ����  fbuy_uid/fsale_uid
				//				string updateStr1 = "UPDATE " + PublicRes.GetTName("t_tran_list",listID) + "     SET flstate = '" + flstate + "' WHERE FlistID = '" + listID +"'";   //�ܱ�
				//				string updateStr2 = "UPDATE " + PublicRes.getTableNameFromLsd("fbuy_uid",listID, "t_pay_list") + "  SET flstate = '" + flstate + "' WHERE FlistID = '" + listID +"'";      //��ҷֿ�ֱ�
				//				string updateStr3 = "UPDATE " + PublicRes.getTableNameFromLsd("fsale_uid",listID,"t_pay_list") + "  SET flstate = '" + flstate + "' WHERE FlistID = '" + listID +"'";      //���ҷֿ�ֱ�

				//				string strSelectBuyID  = "SELECT fbuy_uid  FROM " + PublicRes.GetTName("t_tran_list",listID) + " WHERE flistid ='" + listID + "'" ;
				//				string buyuid = PublicRes.ExecuteOne(strSelectBuyID,"yw_30");
				//
				//				strSelectBuyID  = "SELECT fsale_uid  FROM " + PublicRes.GetTName("t_tran_list",listID) + " WHERE flistid ='" + listID + "'" ;
				//				string saleuid = PublicRes.ExecuteOne(strSelectBuyID,"yw_30");

				/*
				string strSelectBuyID  = "SELECT fbuy_uid  FROM " + PublicRes.GetTName("t_order",listID) + " WHERE flistid ='" + listID + "'" ;
				string buyuid = PublicRes.ExecuteOne(strSelectBuyID,"ZJ");

				strSelectBuyID  = "SELECT fsale_uid  FROM " + PublicRes.GetTName("t_order",listID) + " WHERE flistid ='" + listID + "'" ;
				string saleuid = PublicRes.ExecuteOne(strSelectBuyID,"ZJ");
				*/

				string sqllst = "listid=" + listID;
				string buyuid = CommQuery.GetOneResultFromICE(sqllst,CommQuery.QUERY_ORDER,"fbuy_uid",out errMsg);
				string saleuid = CommQuery.GetOneResultFromICE(sqllst,CommQuery.QUERY_ORDER,"fsale_uid",out errMsg);

				//				string updateStr1 = "UPDATE " + PublicRes.GetTName("t_tran_list",listID) + "     SET flstate = '" + flstate + "',Fmodify_time=now() WHERE FlistID = '" + listID +"'";   //�ܱ�
				//				string updateStr2 = "UPDATE " + PublicRes.GetTName("t_pay_list",buyuid) + "  SET flstate = '" + flstate + "',Fmodify_time=now() WHERE FlistID = '" + listID +"'";      //��ҷֿ�ֱ�
				//				string updateStr3 = "UPDATE " + PublicRes.GetTName("t_pay_list",saleuid) + "  SET flstate = '" + flstate + "',Fmodify_time=now() WHERE FlistID = '" + listID +"'";      //���ҷֿ�ֱ�
			
				//				ArrayList al = new ArrayList();
				//				al.Add(updateStr1);
				//al.Add(updateStr2);
				//al.Add(updateStr3);
           
				//PublicRes.Execute(al,"yw_30"); //����ִ��

				ice.OpenConn();
				string strwhere = "where=" + ICEAccess.URLEncode("flistid=" + listID + "&fcurtype=1&");

				string strUpdate = "data=" + ICEAccess.URLEncode("flstate=" + flstate);
				strUpdate += ICEAccess.URLEncode("&fmodify_time=" + PublicRes.strNowTimeStander);

				string strResp = "";

				//3.0�ӿڲ�����Ҫ furion 20090708
				int iresult = ice.InvokeQuery_Exec(YWSourceType.���׵���Դ,YWCommandCode.�޸Ľ��׵���Ϣ,listID,strwhere + "&" + strUpdate,out strResp);
				if(iresult != 0 && iresult !=60120101)
				{
					throw new Exception("�������׵�ʱ����" + strResp);	
				}


				/*
				
				//furion ͬʱ���¶���,���ж�Ӱ������.��Ϊ�п��ܲ����ڶ���.
				string updateStr1 = "UPDATE " + PublicRes.GetTName("t_order",listID) + "     SET flstate = '" + flstate + "',Fmodify_time=now() WHERE FlistID = '" + listID +"'";   //�ܱ�
				string updateStr2 = "UPDATE " + PublicRes.GetTName("t_user_order",buyuid) + "  SET flstate = '" + flstate + "',Fmodify_time=now() WHERE FlistID = '" + listID +"'";      //��ҷֿ�ֱ�
				string updateStr3 = "UPDATE " + PublicRes.GetTName("t_user_order",saleuid) + "  SET flstate = '" + flstate + "',Fmodify_time=now() WHERE FlistID = '" + listID +"'";      //���ҷֿ�ֱ�

				updateStr1 += " and fbuy_uid='" + buyuid + "' and fsale_uid='" + saleuid + "' ";
				//furion 20081015 �޸������Ҷ���,����Ҳ��һ��������.��Ϊû�ж�Ӱ�캯��,���ô���. 
				PublicRes.ExecuteSql(updateStr1,"ZJ");
				//PublicRes.ExecuteSql(updateStr2,"BS");
				//PublicRes.ExecuteSql(updateStr3,"BS");
				*/

				string inmsg = "MSG_NO=" + listID + flstate;
				inmsg += "&transaction_id=" + listID;
				inmsg += "&flstate=" + flstate;
				inmsg += "&fmodify_time=" + PublicRes.strNowTimeStander;

				string reply;
				short sresult;
				string Msg = "";

				if(commRes.middleInvoke("order_update_service",inmsg,true,out reply,out sresult,out Msg))
				{
					if(sresult != 0)
					{
						Msg =  "order_update_service�ӿ�ʧ�ܣ�result=" + sresult + "��msg=" + Msg + "&reply=" + reply;
						throw new LogicException(Msg);
					}
					else
					{
						if(reply.IndexOf("result=0") > -1)
						{
						}
						else
						{
							Msg =  "order_update_service�ӿ�ʧ�ܣ�result=" + sresult + "��msg=" + Msg + "&reply=" + reply;
							throw new LogicException(Msg);
						}
					}
				}
				else
				{
					Msg = "order_update_service�ӿ�ʧ�ܣ�result=" + sresult + "��msg=" + Msg + "&reply=" + reply;
					throw new LogicException(Msg);
				}

				PublicRes.writeSysLog(strUserID,strIP,"dj","���׽ⶳ",1,listID,"״̬��"+flstate);

				return true;
			}	
			catch(Exception e)
			{
				throw new Exception("���׵��������!" + e.Message.Replace("'","��"));
				return false;
			}
		}



		[WebMethod(Description="���ص�ǰ��static�ֶ�ֵ")]  
		[SoapHeader("myHeader",Direction=SoapHeaderDirection.In)]
		public string [] returnStaticStr()
		{
			try
			{
				//���ؾ�̬�ֶ�ֵ����������ã���ӳ��ǰ��״ֵ̬
				string [] ar = new string[3];  //������
				ar[0] = BatchNo; //���ö�������ִ��ʱ������κ�
				ar[1] = TypeOfErrorDetail.ToString(); //���ʽ�������ֱ�����ʽ�����ݲ�ͬ��ֵ����ת����ͬ��ҳ�����չ��
				ar[2] = ErrorNo.ToString();

				return ar;
			}
			catch(Exception e)
			{
				//throw new Exception("KF_Service-Insert_Service:AdjustTradeRequest Error! " + e.Message.ToString().Replace("'","��"));
				throw new Exception("Service���ó�������ϵ����Ա");
				return null;
			}
		}
		
	

		[WebMethod(Description="�����˻�")]
		[SoapHeader("myHeader",Direction=SoapHeaderDirection.In)]
		public bool freezeAccount(string uid,int type)  
		{
			try
			{
				string freezeStr=null;

				//furion 20070119
				string fuid = PublicRes.ConvertToFuid(uid);
			
				string errMsg = "";
				string strSql = "uid=" + fuid;		
				strSql += "&modify_time=" + PublicRes.strNowTimeStander;
				strSql += "&curtype=1";
				if (type == 1)  //������������Ҫ����
				{
					strSql += "&state=2";
				}
				else if (type == 2)  //���붳�� ��Ҫ�ⶳ ������
				{
					strSql += "&state=1";
				}

				//int iresult = CommQuery.ExecSqlFromICE(strSql,CommQuery.UPDATE_BANKUSER,out errMsg);
				CommQuery.ExecSqlFromICE(strSql,CommQuery.UPDATE_BANKUSER,out errMsg);

//				if(iresult != 1)
//				{
//					throw new LogicException("�����˷�һ����¼:" + errMsg);
//				}

				//���cache
				PublicRes.ReleaseCache(uid,"qqid");
				return true;

				/*
				// TODO1: �ͻ���Ϣ��������
				if (type == 1)  //������������Ҫ����
				{
					//freezeStr = "UPDATE " + PublicRes.GetTableName("t_bank_user",uid) + " SET fstate = 2 WHERE fqqid=" + uid;   //1 ���� 2 ����
					freezeStr = "UPDATE " + PublicRes.GetTName("t_bank_user",fuid) + " SET fstate = 2 WHERE fuid=" + fuid;   //1 ���� 2 ����
				}
				else if (type == 2)  //���붳�� ��Ҫ�ⶳ ������
				{
					//freezeStr = "UPDATE " + PublicRes.GetTableName("t_bank_user",uid) + " SET fstate = 1 WHERE fqqid=" + uid;   //1 ���� 2 ����
					freezeStr = "UPDATE " + PublicRes.GetTName("t_bank_user",fuid) + " SET fstate = 1 WHERE fuid=" + fuid;   //1 ���� 2 ����
				}

				//���cache
				PublicRes.ReleaseCache(uid,"qqid");

				//return PublicRes.ExecuteSql(freezeStr,"YW_30");
				return PublicRes.ExecuteSql(freezeStr,"ZL");
				*/
			}	
			catch(Exception e)
			{
				//throw new Exception("Finance_Service-Insert_Service:AdjustTradeRequest Error! " + e.Message.ToString().Replace("'","��"));
				throw new Exception("�����˻�����" + e.Message.ToString().Replace("'","��"));
				return false;		
			}
		}

		//freezePerAccount
		[WebMethod(Description="��������˻�")]
		[SoapHeader("myHeader",Direction=SoapHeaderDirection.In)]
        public bool freezePerAccount(string uid, int type, string username)
		{
			string mediflag = "false";

			if(myHeader == null)
			{
				throw new Exception("����ȷ�ĵ��÷�����");
			}
			string strUserID = myHeader.UserName;
			string strIP = myHeader.UserIP;
			
			//MySqlAccess dazl = new MySqlAccess(PublicRes.GetConnString("ZL"));
			ICEAccess ice = new ICEAccess(PublicRes.ICEServerIP,PublicRes.ICEPort);
			try
			{
				//dazl.OpenConn();
				//dazl.StartTran();
			
				//furion 20060331 ���ж��û��Ƿ����̻�.
				if(mediflag.ToLower() == "false")
				{
					/*
					// TODO1: �ͻ���Ϣ��������
					//int itmp = Int32.Parse(PublicRes.ExecuteOne("select count(*) from c2c_db.t_merchant_info where Fspecial='" + uid + "' ","YW_30"));
					//int itmp = Int32.Parse(PublicRes.ExecuteOne("select count(*) from c2c_db.t_merchant_info where Fspecial='" + uid + "' ","ZL"));
					int itmp = Int32.Parse(dazl.GetOneResult("select count(*) from c2c_db.t_merchant_info where Fspecial='" + uid + "' "));
					
					if(itmp > 0)
					{
						throw new LogicException("���������ⶳ�̻����󶨵�QQ����");
					}
					*/

					//�����жϣ�29813�� 10***@mch.tenpay.com��Ĳ�����
					if(uid.StartsWith("29813"))
					{
						throw new LogicException("���������ⶳ�ڲ�ʹ���˺�");
					}

					if(uid.StartsWith("10") && uid.ToLower().EndsWith("@mch.tenpay.com"))
					{
						throw new LogicException("���������ⶳ�ڲ�ʹ���˺�");
					}

					string Msg = "";
					string selectStr = "special=" + uid;
					string Fspecial = CommQuery.GetOneResultFromICE(selectStr,CommQuery.QUERY_MERCHANTINFO,"Fspecial",out Msg);

					if(Fspecial != null && Fspecial.Trim() != "")
					{
						throw new LogicException("���������ⶳ�̻����󶨵�QQ����");
					}
					
					
				}

				//furion 20061116 email��¼���
				string fuid = PublicRes.ConvertToFuid(uid);

				if(fuid == null || fuid.Length<3)
				{
					throw new Exception("����ⶳ�����˻������ڲ��ɣĲ���ȷ��" );
				}

				string strSql = "uid=" + fuid;
				strSql += "&modify_time=" + PublicRes.strNowTimeStander;
					
				int newtype = 0;

				if (type == 1)  //������������Ҫ����
				{
					strSql += "&state=2";
					newtype = 2;
				}
				else if (type == 2)  //���붳�� ��Ҫ�ⶳ ������
				{				
					strSql += "&state=1";
					newtype = 1;
				}

                //echo 20141211 �ⶳui_common_update_service�ӿ�ȫ������ui_unfreeze_user_service
                if (type == 1)//������֮ǰ������
                {
                    string errMsg = "";
                    int iresult = CommQuery.ExecSqlFromICE(strSql, CommQuery.UPDATE_USERINFO, out errMsg);

                    if (iresult != 1)
                    {
                        throw new LogicException("�����˷�һ����¼:" + errMsg);
                    }

                    strSql += "&curtype=1";

                    //iresult = 
                    CommQuery.ExecSqlFromICE(strSql, CommQuery.UPDATE_BANKUSER, out errMsg);

                }
                else if (type == 2) //�ⶳui_common_update_service�ӿڻ���ui_unfreeze_user_service
                {
                    string errMsg = "";
                    string sql = "uid=" + fuid;
                    string fcredit = CommQuery.GetOneResultFromICE(sql, CommQuery.QUERY_USERINFO, "Fcreid", out errMsg);
                    string fcretype = CommQuery.GetOneResultFromICE(sql, CommQuery.QUERY_USERINFO, "Fcre_type", out errMsg);

                    string Msg = "";
                    string req = "uin=" + uid + "&cre_id=" + fcredit + "&cre_type=" + fcretype + "&caller=" + myHeader.UserName;
                    req += "&source=2&client_ip=" + myHeader.UserIP + "&name=" + username + "&modify_time=" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    req += "&op_id=" + myHeader.OperID + "&op_name=" + myHeader.UserName;
                    CommQuery.GetDSForServiceFromICE(req, "ui_unfreeze_user_service", true, out Msg);
                    if (Msg != "")
                    {
                        throw new Exception("��ui_unfreeze_user_service�ⶳ�쳣��"+Msg);
                    }
                }


//				if(iresult != 1)
//				{
//					throw new LogicException("�����˷�һ����¼:" + errMsg);
//				}

				/*
				string freezeStr1 = "";
				string freezeStr2 = "";
				int newtype = 0;

				if (type == 1)  //������������Ҫ����
				{
					newtype = 2;
				}
				else if (type == 2)  //���붳�� ��Ҫ�ⶳ ������
				{				
					newtype = 1;
				}

				freezeStr1 = "UPDATE " + PublicRes.GetTName("t_user_info",fuid) + " SET fstate = " + newtype + " WHERE fuid=" + fuid;   //1 ���� 2 ����
				freezeStr2 = "UPDATE " + PublicRes.GetTName("t_bank_user",fuid) + " SET fstate = " + newtype + " WHERE fuid=" + fuid;   //1 ���� 2 ����

				//���cache
				PublicRes.ReleaseCache(uid,"qqid");

				if(dazl.ExecSqlNum(freezeStr1) != 1)
				{
					throw new Exception("����ⶳ�����˻���������t_user_info����" );
				}

				if(dazl.ExecSqlNum(freezeStr2) != 1)
				{
					throw new Exception("����ⶳ�����˻���������t_bank_user����" );
				}
				
				

				//return PublicRes.ExecuteSql(freezeStr,"YW_30");

				//furion 20090304 ���ڵĶ�����Ҫ�������ط���һ���ں��ģ�����t_bank_user/t_user_info
				//����t_user�Ķ��������ʽ�ֹ���Ķ��ᣬ��Χ�������Ķ��ᡣ
				//��Ҫ�ȶ�����Χ�ٴ�����ġ�
				dazl.Commit();
*/
				ice.OpenConn();
				string strwhere = "where=" + ICEAccess.URLEncode("fcurtype=1&");
				strwhere += ICEAccess.URLEncode("fuid=" + fuid + "&");
                strwhere += ICEAccess.URLEncode("fstate=" + type + "&");

				string strUpdate = "data=" + ICEAccess.URLEncode("fstate=" + newtype);
				strUpdate += ICEAccess.URLEncode("&fmodify_time=" + PublicRes.strNowTimeStander);

				string strResp = "";

				//3.0�ӿڲ�����Ҫ furion 20090708
				if(ice.InvokeQuery_Exec(YWSourceType.�û���Դ,YWCommandCode.�޸��û���Ϣ,fuid,strwhere + "&" + strUpdate,out strResp) != 0)
				{
					throw new Exception("����ⶳ�����˻�����" + strResp);
				}

				return true;
			}	
			catch(Exception e)
			{
				ice.CloseConn();
				//dazl.RollBack();
				//throw new Exception("Finance_Service-Insert_Service:AdjustTradeRequest Error! " + e.Message.ToString().Replace("'","��"));
				throw new Exception("��������˻�����" + e.Message.ToString().Replace("'","��"));
				return false;		
			}
			finally
			{
				ice.Dispose();
				//dazl.Dispose();
				try
				{
					PublicRes.writeSysLog(strUserID,strIP,"dj","��������˻�",1,uid,"");	
				}
				catch
				{
					throw new Exception("д��־ʧ�ܣ�");
				}
			}	
		}

        //add by yinhuang 2013/8/15
        [WebMethod(Description = "��������˻�wechat")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public bool FreezePerAccountWechat(string uid, int type)
        {
            string mediflag = "false";

            if (myHeader == null)
            {
                throw new Exception("����ȷ�ĵ��÷�����");
            }
            string strUserID = myHeader.UserName;
            string strIP = myHeader.UserIP;

            try
            {
                //furion 20060331 ���ж��û��Ƿ����̻�.
                if (mediflag.ToLower() == "false")
                {     
                    //�����жϣ�29813�� 10***@mch.tenpay.com��Ĳ�����
                    if (uid.StartsWith("29813"))
                    {
                        throw new LogicException("���������ⶳ�ڲ�ʹ���˺�");
                    }

                    if (uid.StartsWith("10") && uid.ToLower().EndsWith("@mch.tenpay.com"))
                    {
                        throw new LogicException("���������ⶳ�ڲ�ʹ���˺�");
                    }

                    string Msg = "";
                    string selectStr = "special=" + uid;
                    string Fspecial = CommQuery.GetOneResultFromICE(selectStr, CommQuery.QUERY_MERCHANTINFO, "Fspecial", out Msg);

                    if (Fspecial != null && Fspecial.Trim() != "")
                    {
                        throw new LogicException("���������ⶳ�̻����󶨵�QQ����");
                    }
                }

                //furion 20061116 email��¼���
                string fuid = PublicRes.ConvertToFuid(uid);

                if (fuid == null || fuid.Length < 3)
                {
                    throw new Exception("����ⶳ�����˻������ڲ��ɣĲ���ȷ��");
                }

                string strSql = "uid=" + fuid;
                strSql += "&modify_time=" + PublicRes.strNowTimeStander;

                if (type == 1)  //������������Ҫ����
                {
                    strSql += "&state=2";
                }
                else if (type == 2)  //���붳�� ��Ҫ�ⶳ ������
                {
                    strSql += "&state=1";
                }

                string errMsg = "";
                int iresult = CommQuery.ExecSqlFromICE(strSql, CommQuery.UPDATE_USERINFO, out errMsg);

                if (iresult != 1)
                {
                    throw new LogicException("�����˷�һ����¼:" + errMsg);
                }

                return true;
            }
            catch (Exception e)
            {
                throw new Exception("��������˻�wechat����" + e.Message.ToString().Replace("'", "��"));
                return false;
            }
            finally
            {
                try
                {
                    PublicRes.writeSysLog(strUserID, strIP, "dj", "��������˻�", 1, uid, "");
                }
                catch
                {
                    throw new Exception("д��־ʧ�ܣ�");
                }
            }
        }

        [WebMethod(Description = "��������˻�webchat_new")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public bool FreezePerAccountWechat_New(string uin, string username, string channel) 
        {
            if (myHeader == null)
            {
                throw new Exception("����ȷ�ĵ��÷�����");
            }

            try
            {
                if (string.IsNullOrEmpty(uin)) 
                {
                    throw new Exception("�˺�Ϊ��");
                }
                uin = uin.Trim();
                string uid = PublicRes.ConvertToFuid(uin);
                if (uid == null || uid.Length < 3)
                {
                    throw new Exception(uin + "�˺Ų�����");
                }
                string errMsg = "";
                string strSql = "uid=" + uid;
                string fcredit = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_USERINFO, "Fcreid", out errMsg);
                string fcretype = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_USERINFO, "Fcre_type", out errMsg);
                string Msg = "";
                //client_ip,modify_time;name;caller uin source=2 watch_word cre_id
                string req = "uin=" + uin + "&cre_id=" + fcredit + "&cre_type=" + fcretype + "&caller=" + myHeader.UserName;
                req += "&source=2&client_ip=" + myHeader.UserIP + "&channel=" + channel + "&name=" + username + "&modify_time=" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                CommQuery.GetDSForServiceFromICE(req, "ui_freeze_user_service", true, out Msg);
                if (Msg != "") {
                    throw new Exception(Msg);
                }

                return true;
            }
            catch (Exception e) 
            {
                throw new LogicException("service����ʧ��:" + e.Message);
                return false;
            }
        }

        [WebMethod(Description = "�ⶳ�����˻�webchat_new")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public bool UnFreezePerAccountWechat_New(string uin, string username)
        {
            if (myHeader == null)
            {
                throw new Exception("����ȷ�ĵ��÷�����");
            }

            try
            {
                if (string.IsNullOrEmpty(uin))
                {
                    throw new Exception("�˺�Ϊ��");
                }
                uin = uin.Trim();
                string uid = PublicRes.ConvertToFuid(uin);
                if (uid == null || uid.Length < 3)
                {
                    throw new Exception(uin + "�˺Ų�����");
                }
                string errMsg = "";
                string strSql = "uid=" + uid;
                string fcredit = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_USERINFO, "Fcreid", out errMsg);
                string fcretype = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_USERINFO, "Fcre_type", out errMsg);

                string Msg = "";
                //client_ip,modify_time;name;caller uin source=2 watch_word cre_id
                string req = "uin=" + uin + "&cre_id=" + fcredit + "&cre_type=" + fcretype + "&caller=" + myHeader.UserName;
                req += "&source=2&client_ip=" + myHeader.UserIP + "&name=" + username + "&modify_time=" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                CommQuery.GetDSForServiceFromICE(req, "ui_unfreeze_user_service", true, out Msg);
                if (Msg != "")
                {
                    throw new Exception(Msg);
                }

                return true;
            }
            catch (Exception e)
            {
                throw new LogicException("service����ʧ��:" + e.Message);
                return false;
            }
        }

        //[WebMethod(Description = "���ID�����׵�ID")] //2015-8-11 �Ľӿ� v_yqyqguo
        //public string TdeToID(string tdeid)  //���븶�ID
        //{
        //    try
        //    {
        //        //��ͬʱ�޸�3�ű��õ����� t_user_info,t_user,t_bank_user �ֱ�Ϊ��1��2��3	
        //        string tmp = PublicRes.ExecuteOne("select Flistid from c2c_db.t_tcpay_list where ftde_id='" + tdeid + "' and Fsubject<>4 " ,"ywb");  //ִ�в����ؽ��
        //        if(tmp == null || tmp.ToString().Trim() == "")
        //        {
        //            tmp = PublicRes.ExecuteOne("select Flistid from c2c_db.t_refund_list where Frlistid = (select FListID from c2c_db.t_tcpay_list where ftde_id='" + tdeid + "' and Fsubject=4 )" ,"ywb");  //ִ�в����ؽ��
        //            //return "0";
        //        }
        //        return tmp.ToString().Trim();
        //    }
        //    catch(Exception e)
        //    {
        //        throw new Exception(e.Message.ToString().Replace("'","��"));
        //        return "0";
        //    }
        //}

		/// <summary>
		/// ���ݴ���Ľ��׵����飬������ϵͳ��ȡ���������׵��б�
		/// </summary>
		/// <param name="ar">���׵�����</param>
		/// <param name="time">����ʱ�䣨YYYYMMDD��������ȷ�����ݿ�</param>
		/// <returns>���غ����ݵĽ��׵���Ϣ��DataSet</returns>
		[WebMethod(Description="���ݽ��׵��Ż�ȡ��������ϸ��")]
		public DataSet dsReBatCheck(string [] ar,string time)
		{
			try
			{
				StringBuilder sb = new StringBuilder("");
				foreach(string s in ar)
				{
					sb.Append("Fbank_list=");
					sb.Append("'"+s+"'");
					sb.Append(" || ");
				}

				int n = sb.Length;
				string lstStr = sb.ToString().Trim().Substring(0,n-3);  //ȥ������||��

				string bankrollTable = "c2c_zwdb_" + time + ".t_bankroll_result ";
				//�쿴����״̬
				string selectStr = "Select * from (Select * from " + bankrollTable + "order by FbatchNO DESC) A where " + lstStr + " group by Fbank_list";
				
				return PublicRes.returnDSAll(selectStr,"zw");
			}
			catch(Exception e)
			{
				throw new Exception("ͨ�����׵��Ż�ȡ��������ϸ������[" + e.Message.ToString().Replace("'","��") + "]");
			}
		}
		
		[WebMethod(Description="������ʷ��ѯ")]
		[SoapHeader("myHeader", Direction=SoapHeaderDirection.In)]
		public DataSet logOnUserHistory(DateTime startDate,DateTime endDate,string qqid,string handid,int startIndex,int lenth,out string Msg)
		{
			Msg = null;
			string whereStr = " where 1=1 ";
			
			//��ʽ��ʱ��
			string strBgDateTime;
			string strEdDateTime;
 
			try
			{
				strBgDateTime = startDate.ToString("yyyy-MM-dd 00:00:00");
				strEdDateTime = endDate.ToString("yyyy-MM-dd 23:59:59");		
			}
			catch
			{
				Msg = "������ʷ��ѯʱ�䲻��ȷ�����飡";
				return null;
			}
		


			//��ѯ
			if (strBgDateTime != null && strBgDateTime.Trim() != "" && strEdDateTime != null && strEdDateTime.Trim() != "")
			{
				whereStr += " and flastModifyTime >= '" +strBgDateTime + "' and flastModifyTime <= '" +strEdDateTime + "'";
			}

			if (qqid != null && qqid.Trim() != "")
			{
				whereStr += " and fqqid = '" + qqid + "' ";
			}

			if (handid != null && handid.Trim() != "")
			{
				whereStr += " and handid = '" + handid + "' ";
			}

			//furion 20061122 ����������һ��.
			//�õ��ܼ�¼��
			//string fstrSql_count = "select *   from  c2c_fmdb.t_logon_history " + " " + whereStr;
			//string fstrSql_count = "select count(*)   from  c2c_fmdb.t_logon_history " + " " + whereStr;
    		//DataSet dsTmp = PublicRes.returnDSAll(fstrSql_count,"ht");
			//int count =  int.Parse(dsTmp.Tables[0].Rows.Count.ToString());
			int count =  10000;//int.Parse(dsTmp.Tables[0].Rows[0][0].ToString());

			string str = "select Fid, Fqqid, Fquid, Freason, handid, handip, FlastModifyTime,  " + count + " as icount from c2c_fmdb.t_logon_history " + whereStr + " order by fid DESC limit " + startIndex + "," + lenth; 
			return PublicRes.returnDSAll(str,"ht");
		}


		/// <summary>
		/// ��������ɾ���û���Ӧ��ϵ����¼��ʷ���ɡ���������ϵͳ�㲻��ȥ�ҵ����û���������ϡ���06.02.20��richardȷ�� rayguo��
		/// �Ժ�������ѯ���������ʻ��������Ϣ�����ṩһ�������ڲ�ID��ѯ�Ĺ��ܼ��ɴӺ�̨����ϵͳ�л�ȡ�����Ҫ����Ϣ��
		/// </summary>
		/// <param name="qqid"></param>
		/// <returns></returns>
		[WebMethod(Description="��������")]
		[SoapHeader("myHeader", Direction=SoapHeaderDirection.In)]
		public bool logOnUser(string qqid,string reason,string user, string userIP,out string Msg)
		{
			Msg = null;
			//MySqlAccess da = null;
			MySqlAccess fmda = null;
			
			//2.0ֱ��ע���ˣ������з��գ�Ϊ�˲��ܣ���Ҳ��ʱע�� furion 20091225
			/*
			if(myHeader == null)
			{
				throw new Exception("����ȷ�ĵ��÷�����");
			}
			
			RightAndLog rl = new RightAndLog();
			rl.actionType = "��������";
			rl.ID = qqid;
			rl.OperID = myHeader.OperID;
			rl.sign = 1;
			rl.strRightCode = "logonUser";
			rl.RightString = myHeader.RightString;
			rl.SzKey = myHeader.SzKey;
			rl.type = "��������";
			rl.UserID = myHeader.UserName;
			rl.UserIP = myHeader.UserIP;				
			if(!rl.CheckRight())
			{
				throw new LogicException("�û���Ȩִ�д˲�����");
			}		
			*/

			//da   = new MySqlAccess(PublicRes.GetConnString("YW_30"));
			fmda = new MySqlAccess(PublicRes.GetConnString("ht"));

			//MySqlAccess da_zl = new MySqlAccess(PublicRes.GetConnString("ZL"));
			try
			{				
				//furion 20080522 ɾ��ʵ����֤.
				string inmsg = "uin=" + qqid;
				inmsg += "&operator=" + user;
				inmsg += "&memo=��������ɾ��" ;

				string reply;
				short sresult;

				//3.0�ӿڲ�����Ҫ furion 20090708
				if(commRes.middleInvoke("au_del_authen_service",inmsg,true,out reply,out sresult,out Msg))
				{
					if(sresult != 0)
					{
						Msg =  "au_del_authen_service�ӿ�ʧ�ܣ�result=" + sresult + "��msg=" + Msg + "&reply=" + reply;
						return false;
					}
					else
					{
						if(reply.IndexOf("result=0") > -1)
						{
						}
						else if(reply.IndexOf("result=22520101") > -1)
						{
						}
						else
						{
							Msg =  "au_del_authen_service�ӿ�ʧ�ܣ�result=" + sresult + "��msg=" + Msg + "&reply=" + reply;
							return false;
						}
					}
				}
				else
				{
					Msg = "au_del_authen_service�ӿ�ʧ�ܣ�result=" + sresult + "��msg=" + Msg + "&reply=" + reply;
					return false;
				}

				//end ɾ��ʵ����֤
				//da.OpenConn();
				//da.StartTran();

				//da_zl.OpenConn();
				//da_zl.StartTran();

				fmda.OpenConn();
				fmda.StartTran();

				//�����жϡ�����û���δ�����Ľ��׻����������������ȣ������������ Ŀǰ����򵥽���ж�
				string uid = PublicRes.ConvertToFuid(qqid);
				if (uid == null || uid.Trim() == "" || uid.Trim() == "0")
				{
					Msg = "��ȡQQ" + qqid + "�����Ӧ��UIDʧ��!"; 
					//return false;
					//furion 20070130 �������������,�ͷ�����,�������ظ�����.
					return true;
				}

				//				string str = "select fbalance,fcon from " + PublicRes.GetTName("t_user",uid) + " where fuid = '" + uid + "'";
				//				DataTable dt = PublicRes.returnDSAll(str,"YW_30").Tables[0];
				//				
				//				if (dt != null && dt.Rows.Count !=0 && dt.Rows[0]["fbalance"] != null && dt.Rows[0]["fcon"] != null)
				//				{
				//					string banlance = dt.Rows[0]["fbalance"].ToString().Trim();
				//					string fcon     = dt.Rows[0]["fcon"].ToString().Trim();
				//
				////					if (banlance != "0" || fcon != "0")  //ȡ���жϡ��û���������ͨ���󣬼����Խ���������
				////					{
				////						Msg = "�����ʻ�" + qqid + "�����[" + banlance + "]���߶������[" + fcon + "]��Ϊ��.�������������������";
				////						commRes.sendLog4Log("Finance_Manage.logOnUser",Msg);
				////						return false;
				////					}	
				// 				}


				//������ʷ�������ݿ�
				string nowTime = PublicRes.strNowTimeStander;
				string insertStr = "insert into c2c_fmdb.t_logon_history (fqqid,fquid,freason,handid,handip,flastMOdifyTime) values ('" 
					+ qqid + "','" + PublicRes.ConvertToFuid(qqid) + "','" + commRes.replaceSqlStr(reason) + "','" + user + "','" + userIP + "','" + nowTime + "')";
				if (!fmda.ExecSql(insertStr))
				{
					//da.RollBack();
					fmda.RollBack();

					Msg = "����ʱ������ʷ�������ݿ����";
					commRes.sendLog4Log("FinanceManage.LogOnUser",Msg);
					return false;
				}

				/*
				// TODO1: �ͻ���Ϣ��������
				//furion 20070206 �������,��ȫ��,����Ҫ��ѯ�������еİ󶨵��ʺ�.
				string strSql = "select ifnull(fqqid,''),ifnull(Femail,''),ifnull(Fmobile,'') from " 
					+ PublicRes.GetTName("t_user_info",uid) + " where fuid= '" + uid + "'";
				//DataSet ds = da.dsGetTotalData(strSql);
				DataSet ds = da_zl.dsGetTotalData(strSql);
				
				*/

				string strSql = "uid=" + uid;
				DataSet ds = CommQuery.GetDataSetFromICE(strSql,CommQuery.QUERY_USERINFO,out Msg);

				if(ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count != 1)
				{
					fmda.RollBack();
					Msg = "��ȡ�û�����ʱ����." + Msg;
					return false;
				}
				
				string strtmp = QueryInfo.GetString( ds.Tables[0].Rows[0]["fqqid"]);
				if(strtmp != "")
				{
					string fuid = PublicRes.ConvertToFuid(strtmp);
					if(fuid != null && fuid == uid)
					{
						/*
						//���QQ�Ŷ�Ӧ�Ķ�Ӧ��ϵ��
						string delStr = "delete from " + PublicRes.GetTName("t_relation",strtmp) + " where fqqid ='" + strtmp + "'";

						//furion 20090302 t_relation�Ƶ��û����Ͽ��
						//int i = da.ExecSqlNum(delStr);
						int i = da_zl.ExecSqlNum(delStr);
						*/

						strSql = "uin=" + strtmp;		
						int i = CommQuery.ExecSqlFromICE(strSql,CommQuery.DELETE_RELATION,out Msg);
						
				
						if (i < 0)
						{
							//da.RollBack();
							fmda.RollBack();
							//da_zl.RollBack();

							Msg = "ɾ������ԭ��" + Msg;
							commRes.sendLog4Log("FinanceManage.LogOnUser",Msg);
							return false;
						}
					}
				}

				strtmp = QueryInfo.GetString(ds.Tables[0].Rows[0]["Femail"]);
				if(strtmp != "")
				{
					string fuid = PublicRes.ConvertToFuid(strtmp);
					if(fuid != null && fuid == uid)
					{
						//���email��Ӧ�Ķ�Ӧ��ϵ��
						string uid3 = "";
						if(!Common.DESCode.GetEmailUid(strtmp,out uid3))
						{
							//da.RollBack();
							fmda.RollBack();
							//da_zl.RollBack();

							Msg = "��ȡ�ڲ�IDʱ����." + uid3;
							return false;
						}

						/*
						string delStr = "delete from " + PublicRes.GetTName("t_relation",uid3) + " where fqqid ='" + strtmp + "'";

						//furion 20090302 t_relation�Ƶ��û����Ͽ��
						//int i = da.ExecSqlNum(delStr);
						int i = da_zl.ExecSqlNum(delStr);
						*/

						strSql = "uin=" + strtmp;		
						int i = CommQuery.ExecSqlFromICE(strSql,CommQuery.DELETE_RELATION,out Msg);
				
						
						if (i < 0)
						{
							//da.RollBack();
							fmda.RollBack();
							//da_zl.RollBack();

							Msg = "ɾ������ɾ�������� " + i + " ����";
							commRes.sendLog4Log("FinanceManage.LogOnUser",Msg);
							return false;
						}
						
					}
				}

				strtmp = QueryInfo.GetString(ds.Tables[0].Rows[0]["Fmobile"]);
				if(strtmp != "")
				{
					string fuid = PublicRes.ConvertToFuid(strtmp);
					if(fuid != null && fuid == uid)
					{
						/*
						//����ֻ��Ŷ�Ӧ�Ķ�Ӧ��ϵ��
						string delStr = "delete from " + PublicRes.GetTName("t_relation",strtmp) + " where fqqid ='" + strtmp + "'";

						//furion 20090302 t_relation�Ƶ��û����Ͽ��
						//int i = da.ExecSqlNum(delStr);
						int i = da_zl.ExecSqlNum(delStr);
						*/
						strSql = "uin=" + strtmp;		
						int i = CommQuery.ExecSqlFromICE(strSql,CommQuery.DELETE_RELATION,out Msg);
				
						if (i < 0)
						{
							//da.RollBack();
							fmda.RollBack();
							//da_zl.RollBack();

							Msg = "ɾ������ɾ�������� " + i + " ����";
							commRes.sendLog4Log("FinanceManage.LogOnUser",Msg);
							return false;
						}
					}
				}


				//da.Commit();
				fmda.Commit();
				//da_zl.Commit();
				return true;
			}
			catch(Exception e)
			{
				//da.RollBack();
				fmda.RollBack();
				//da_zl.RollBack();

				Msg = "ɾ���û���ϵ��Ӧ���¼ʧ�ܣ����飡" + commRes.replaceHtmlStr(e.Message);
				commRes.sendLog4Log("FinanceManage.LogOnUser",Msg);
				return false;
			}
			finally
			{
				//da.Dispose();
				fmda.Dispose();
				//da_zl.Dispose();
			}


		}

	}
}