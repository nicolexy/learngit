using System;

using System.Data;
using TENCENT.OSS.CFT.KF.DataAccess;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.C2C.Finance.BankLib;

namespace TENCENT.OSS.CFT.KF.KF_Service
{
	/// <summary>
	/// Check_Class ��ժҪ˵����
	/// </summary>
	public class Check_Class
	{
		public Check_Class()
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
		}



		/// <summary>
		/// ������һ���µ���ˡ��м���Ҫ��Ҫ������һ������������ڶ����жϼ���
		/// �������������ͬ������ͬID��δ������ɵ��������ڣ��쳣�˳���
		/// </summary>
		/// <param name="strUserID">����������</param>
		/// <param name="strMainID">������ID</param>
		/// <param name="strCheckType">��������</param>
		/// <param name="strMemo">����˵��</param>
		/// <param name="strLevelValue">�����ж�ֵ(���Ϊ�������Ԫ)</param>
		/// <param name="myParams">����</param>
		/// <returns>�����Ƿ�ɹ�</returns>
		public static bool CreateCheck(string strUserID, string strMainID, string strCheckType, string strMemo, string strLevelValue, Param[] myParams,Finance_Header myheader)
		{
			strUserID = strUserID.Trim().ToLower();
			

			MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("HT"));
			try
			{
				da.OpenConn();
				da.StartTran();

				//�ж������Ƿ��ܹ����������ͬһ���������ͣ�ͬһ������ID������ͬʱ��������״̬
				string strSql = "select count(1) from c2c_fmdb.t_check_main where FObjID='" + strMainID
					//+ "' and FCheckType='" + strCheckType + "' and (FState<>'finish' and FState<>'finished')";
					//furion 20051226 
					//+ "' and FCheckType='" + strCheckType + "'  and FState<>'finished' ";
					+ "' and FCheckType='" + strCheckType + "'  and FNewState<3 ";

				if(da.GetOneResult(strSql) == "1")  //�Ѿ�����
				{
					//return false;
					throw new LogicException("���ѷ���һ����ͬ�������͵�������");
				}
                //���ͨ���ǿ�꣬ͬһ���Ƹ�ͨ�˺Ų��ܶ�η���ǿ�� v_yqyqguo
                if (strCheckType == "LCTBalanceRedeem")
                {
                    string uin = myParams[0].ParamValue.ToString().Trim();
                    strSql = "select FObjID from c2c_fmdb.t_check_main where FCheckType='" + strCheckType + "' and FCheckMemo like 'LCTFund:true|uin:" + uin + "|total_fee:%'  and FNewState<3 ";
                    string FCheckMemo = da.GetOneResult(strSql);
                    if (!string.IsNullOrEmpty(FCheckMemo))
                    {
                        throw new LogicException("�òƸ�ͨ�˺��Ѿ��д����������ͨ���ǿ�꣡");
                    }
                }

				strSql = "Insert c2c_fmdb.t_check_main(FObjID) values('" + strMainID + "')";
                
				da.ExecSql(strSql);

				strSql = "select Max(FID) from c2c_fmdb.t_check_main where FObjID='" + strMainID + "'";
				string ID = da.GetOneResult(strSql);

				AddParam(ID,myParams,da);

				int iLevel = GetCheckLevel(strCheckType, strLevelValue,da);

				strSql = "update c2c_fmdb.t_check_main set FCheckType='" + strCheckType + "',FStartUser='"
					//+ strUserID + "',FCheckMemo='" + strMemo + "',FState='start',FCheckLevel=" + iLevel.ToString()
					+ strUserID + "',FCheckMemo='" + strMemo + "',FNewState=0,FCheckLevel=" + iLevel.ToString()
					+ ",FCurrLevel=0,FCheckCount=0,FCheckResult=-1,FStartTime=" + PublicRes.strNowTime + ",FCheckMoney='" + strLevelValue + "' where FID=" + ID ;
				da.ExecSql(strSql); 

				//furion 20060106 Ϊ��֧�ַ���ͨ��,�������⴦��.
				if(iLevel == 0 && (strCheckType.ToLower() == "fmfk03" || strCheckType.ToLower() == "fmtk04"
					|| strCheckType.ToLower() == "batchpay"|| strCheckType.ToLower() == "batchrefund"))//���Ӹ�����˿��ͨ�� andrew 20111117
				{
					strSql = "update c2c_fmdb.t_check_main set FCheckResult=0 where FID=" + ID;
					da.ExecSql(strSql);

					da.Commit();

					ValidCheck(ID,myheader,da);
					return true;
				}
				else
				{
					if(iLevel <= 0)
					{
						throw new LogicException("�㷢����������˴�����˶��������ͺ������Ƿ���ȷ��");
					}
					//ÿ�������ߣ���Ҫ�ж����������Լ���һ���º����� furion 20050910

					GetNextCheckLevel(ID,da);
					//furion end

					//				//��������ɹ��������ʼ�
					//����һ�������˷����ʼ�(���η���Ϊ0+1��)
					sendCheckMail(ID,da);
					
					da.Commit();
					return true;
				}

			}
			catch(Exception err)
			{
				da.RollBack();
				log4net.ILog log = log4net.LogManager.GetLogger("Check_Class.CreateCheck");
				if(log.IsErrorEnabled) log.Error(strMainID,err);
				throw;
			}
			finally
			{
				da.Dispose();
			}
		}


		
		/// <summary>
		/// ��ȡ��������Ӧ�ڴ��������͵ļ�����ʵ��һ��Ҫ����ֵ��������Ӧ�Ը��㡣
		/// </summary>
		/// <param name="strCheckType"></param>
		/// <param name="strLevelValue"></param>
		/// <param name="da"></param>
		/// <returns></returns>
		private static int GetCheckLevel(string strCheckType, string strLevelValue, MySqlAccess da)
		{
			for(int i=10; i>0; i--)
			{
				string strSql = "select FCheck" + i.ToString() + " from c2c_fmdb.t_check_type where FTypeID='"
					+ strCheckType + "'";

				string tmp = da.GetOneResult(strSql);
				if(tmp != null && tmp.Trim() != "")
				{
					tmp = String.Format(tmp,strLevelValue);
					tmp = da.GetOneResult("select " + tmp);
					if(tmp != null && tmp.Trim() != "")
					{
						if(tmp == "1") 
						{
							return i;
						}
					}                    
				}
			}
			return 0;
		}


		/// <summary>
		/// �õ�ȷ�е���һ����������˭�� 
		/// </summary>
		/// <param name="strMainID">����ID</param>
		/// <param name="da">���ݷ��ʶ���</param>
		private static void GetNextCheckLevel(string strMainID,MySqlAccess da)
		{
			string strSql = "select * from c2c_fmdb.t_check_main where Fid=" + strMainID;

			DataTable dt= da.GetTable(strSql);
			if(dt == null || dt.Rows.Count == 0)
			{
				throw new LogicException("���Ҳ���ָ����������¼��");
			}
				
			int currlevel = Int32.Parse(dt.Rows[0]["FCurrLevel"].ToString());
			int maxlevel = Int32.Parse(dt.Rows[0]["FCheckLevel"].ToString());
			string checkvalue = dt.Rows[0]["FCheckMoney"].ToString();
			string strCheckType = dt.Rows[0]["FCheckType"].ToString();

			int newcurrlevel = currlevel;

			for(int i=currlevel+1; i<=maxlevel; i++)
			{
				strSql = "select FCheck" + i.ToString() + " from c2c_fmdb.t_check_type where FTypeID='"
					+ strCheckType + "'";
				string tmp = da.GetOneResult(strSql);
				if(tmp != null && tmp.Trim() != "")
				{
					tmp = String.Format(tmp,checkvalue);
					tmp = da.GetOneResult("select " + tmp);
					if(tmp != null && tmp.Trim() != "")
					{
						if(tmp == "1") 
						{
							newcurrlevel = i-1;
							break;
						}
					}                    
				}
			}

			strSql = "update c2c_fmdb.t_check_main set FCurrLevel=" + newcurrlevel.ToString() + " where fid=" + strMainID;
			da.ExecSql(strSql);		
		}



		/// <summary>
		/// ��������֪ͨ�ʼ�
		/// </summary>
		/// <param name="fid">��������ID</param>
		/// <param name="da">���ݷ��ʶ���</param>
		public static void sendCheckMail(string fid, MySqlAccess da)  
		{
			if(PublicRes.IgnoreLimitCheck)
				return ;

			try
			{
				string selectStr = "Select FNewstate,FcheckLevel,FcurrLevel,FcheckType,FstartUser,FcheckMemo,FCheckResult from c2c_fmdb.t_check_main where fid='" + fid + "'";
				string [] ar = new string[7]; //returnDrData
				ar[0] = "FNewstate";
				ar[1] = "FcheckLevel";
				ar[2] = "FcurrLevel";
				ar[3] = "FcheckType";
				ar[4] = "FstartUser";
				ar[5] = "FcheckMemo";

				//furion 20050910 �����µ�
				ar[6] = "FCheckResult";

				//ar = PublicRes.returnDrData(selectStr,ar,"ht"); //furion 20050914 ��������Ĵ���
				ar = da.drData(selectStr, ar);
					
				string checkState = ar[0].ToString().Trim();
				string checkLevel = ar[1].ToString().Trim();
				int    currLevel  = Int32.Parse(ar[2].ToString().Trim()) + 1;  //��ǰ��Ҫ����������
				string checkType  = ar[3].ToString().Trim();
				string startUser  = ar[4].ToString().Trim();
				string checkMemo  = ar[5].ToString().Trim();


				string checkResult = ar[6].Trim();
				string Msg = null;

				string mailFrom  = System.Configuration.ConfigurationManager.AppSettings["mailFrom"].ToString().Trim();


				//���ͬ�⣬����û�н���
				if (checkState != "2" && checkState != "3")  //˵��������û�н���,��Ҫ�����ʼ�����һ��������
				{		
					try
					{
						string cmdStr = "Select FuserID from c2c_fmdb.t_check_user where fcheckType = '" + checkType +"' and flevel = '" + currLevel + "'";
						string mailToStr = da.GetOneResult(cmdStr);

						string sqlStr = "Select fTypeName from c2c_fmdb.t_check_type where ftypeID = '" + checkType + "'";
						string TypeStr = da.GetOneResult(sqlStr);

						string webPath = System.Configuration.ConfigurationManager.AppSettings["webPath"].ToString();
						//					string content = "<a href ='" + webPath + "/login.aspx?name=" + mailToStr + "'>�����������ͣ�" + TypeStr + "�����������ˣ�" + startUser + "��</a><br> ��������������" + checkMemo;
						string content = "<html><style type='text/css'>.check {font-size: 14px;color: #000000;} a:hover {font-size: 14px;color: #0099FF;text-decoration: underline;}</style><body><div align='center'><table width='90%' height='118' border='1' cellpadding='1' cellspacing='0' class = 'check'><tr bgcolor='#eeeeee'><td height='28' colspan='2'><p>�����µ����������:</p></td></tr><tr><td width='223' height='22'><p>��������: " + TypeStr + " </p></td><td width='251'> <p>����������: " + startUser + " </p></td></tr><tr><td colspan='2'><a href ='" + webPath + "/login.aspx?name=" + mailToStr + "'>��������: " + checkMemo + " </a></td></tr></table></div></body></html>";
						PublicRes.sendMail(mailToStr,mailFrom,"[�Ƹ�ͨ�������ϵͳ]�����µ�����������",content,"inner",out Msg);
						
					}
					catch
					{
						throw new LogicException("�ʼ�����ʧ�ܣ�");
					}		
				}	

				if(checkState == "2")
				{
					if(checkResult == "0")
					{
						string mailToStr = startUser;

						string sqlStr = "Select fTypeName from c2c_fmdb.t_check_type where ftypeID = '" + checkType + "'";
						string TypeStr = da.GetOneResult(sqlStr);

						string webPath = System.Configuration.ConfigurationManager.AppSettings["webPath"].ToString();
						//					string content = "<a href ='" + webPath + "/login.aspx?name=" + mailToStr + "'>�����������ͣ�" + TypeStr + "�����������ˣ�" + startUser + "��</a><br> ��������������" + checkMemo;
						string content = "<html><style type='text/css'>.check {font-size: 14px;color: #000000;} a:hover {font-size: 14px;color: #0099FF;text-decoration: underline;}</style><body><div align='center'><table width='90%' height='118' border='1' cellpadding='1' cellspacing='0' class = 'check'><tr bgcolor='#eeeeee'><td height='28' colspan='2'><p>���������ѽ���:</p></td></tr><tr><td width='223' height='22'><p>��������: " + TypeStr + " </p></td><td width='251'> <p>�����ѱ�ͬ�⣬��ȥִ���������� </p></td></tr><tr><td colspan='2'><a href ='" + webPath + "/login.aspx?name=" + mailToStr + "'>��������: " + checkMemo + " </a></td></tr></table></div></body></html>";
						PublicRes.sendMail(mailToStr,mailFrom,"[�Ƹ�ͨ�������ϵͳ]���������ѱ�ͬ��",content,"inner",out Msg);
					}
				}

				if(checkState == "3")
				{
					if(checkResult == "0")
					{
						string mailToStr = startUser;

						string sqlStr = "Select fTypeName from c2c_fmdb.t_check_type where ftypeID = '" + checkType + "'";
						string TypeStr = da.GetOneResult(sqlStr);

						string webPath = System.Configuration.ConfigurationManager.AppSettings["webPath"].ToString();
						//					string content = "<a href ='" + webPath + "/login.aspx?name=" + mailToStr + "'>�����������ͣ�" + TypeStr + "�����������ˣ�" + startUser + "��</a><br> ��������������" + checkMemo;
						string content = "<html><style type='text/css'>.check {font-size: 14px;color: #000000;} a:hover {font-size: 14px;color: #0099FF;text-decoration: underline;}</style><body><div align='center'><table width='90%' height='118' border='1' cellpadding='1' cellspacing='0' class = 'check'><tr bgcolor='#eeeeee'><td height='28' colspan='2'><p>���������ѽ���:</p></td></tr><tr><td width='223' height='22'><p>��������: " + TypeStr + " </p></td><td width='251'> <p>������ִ�� </p></td></tr><tr><td colspan='2'><a href ='" + webPath + "/login.aspx?name=" + mailToStr + "'>��������: " + checkMemo + " </a></td></tr></table></div></body></html>";
						PublicRes.sendMail(mailToStr,mailFrom,"[�Ƹ�ͨ�������ϵͳ]���������ѱ�����",content,"inner",out Msg);
					}
				}
			}
			catch(Exception err)
			{
				log4net.ILog log = log4net.LogManager.GetLogger("Check_Class.SendCheckMail");
				if(log.IsErrorEnabled) log.Error(fid,err);
				throw;
			}
		}


		/// <summary>
		/// ���������
		/// </summary>
		/// <param name="strCheckID"></param>
		/// <param name="myParams"></param>
		/// <param name="da"></param>
		private static void AddParam(string strCheckID, Param[] myParams, MySqlAccess da)
		{
			if(myParams == null || myParams.Length == 0) return;
			
						
			foreach(Param aparam in myParams)
			{
				string strSql = "insert c2c_fmdb.t_check_param(FCheckID,FKEY,FVALUE) values(" + strCheckID
					+ ",'" + aparam.ParamName + "','" + aparam.ParamValue +"')";
				da.ExecSql(strSql);
			}

		}


		/// <summary>
		/// ���鵱ǰ�������ĸ��׶��ˣ�ÿ��һ�����������ͽ��д˼��顣
		/// </summary>
		/// <param name="strCheckID"></param>
		private static void ValidCheck(string strCheckID, Finance_Header myHeader, MySqlAccess da)
		{
			myHeader.UserName = myHeader.UserName.Trim().ToLower();

			//��һ������������ǲ�ͬ�⣬����ȫ��������   
			//furion 20050831 ������� FCurrLevel=FCheckLevel
			//string strSql = "Update c2c_fmdb.t_check_main set FState='finished',FEndTime=" + PublicRes.strNowTime + ",FCurrLevel=FCheckLevel where FID=" 
			string strSql = "Update c2c_fmdb.t_check_main set FNewState=3,FEndTime=" + PublicRes.strNowTime + ",FCurrLevel=FCheckLevel where FID=" 
				+ strCheckID + " and FCheckResult=1";
			da.ExecSql(strSql);

			//�ڶ���������ǰ������������Ҷ��������ˡ�������1����������
			//strSql = "Update c2c_fmdb.t_check_main A, c2c_fmdb.t_check_type B set A.FState='finish',A.FEndTime=" + PublicRes.strNowTime + ""
			strSql = "Update c2c_fmdb.t_check_main A, c2c_fmdb.t_check_type B set A.FNewState=2,A.FEndTime=" + PublicRes.strNowTime + ""
				//+ " where A.FID=" + strCheckID + " and A.FCheckType=B.FTypeID and B.FRoadType=1 and A.FCurrLevel=A.FCheckLevel and A.FState<>'finished'";
				+ " where A.FID=" + strCheckID + " and A.FCheckType=B.FTypeID and B.FRoadType=1 and A.FCurrLevel=A.FCheckLevel and A.FNewState<3";
			da.ExecSql(strSql);

			//������������ǲ����������������һ�����������ˣ�������
			strSql = "select FCheckType from c2c_fmdb.t_check_main where FID=" + strCheckID;
			string checktype = da.GetOneResult(strSql);

			strSql = "select FRoadType from c2c_fmdb.t_check_type where FTypeID='" + checktype + "'";
			if(da.GetOneResult(strSql) == "2")
			{
				strSql = "select FCheckLevel from c2c_fmdb.t_check_main where FID=" + strCheckID;
				string checklevel = da.GetOneResult(strSql);

				strSql = "select count(1) from c2c_fmdb.t_check_user where FCheckType='" + checktype 
					+ "' and FLevel <=" + checklevel;
 
				string ineedcount = da.GetOneResult(strSql);

				//strSql = "Update c2c_fmdb.t_check_main set FState='finish',FEndTime=" + PublicRes.strNowTime + " where FID=" 
				strSql = "Update c2c_fmdb.t_check_main set FNewState=2,FEndTime=" + PublicRes.strNowTime + " where FID=" 
					//+ strCheckID + " and FCheckCount=" + ineedcount + " and FState<>'finished'";
					+ strCheckID + " and FCheckCount=" + ineedcount + " and FNewState<3";
				da.ExecSql(strSql);
			}

			//da.ExecSqls_Trans(al);

			//���Ĳ��������������������ͬ�⣬�����ֱ��ִ�У����ú�����B.FActionType=2Ϊֱ��ִ��
			strSql = "select count(1) from c2c_fmdb.t_check_main A,c2c_fmdb.t_check_type B where A.FID=" + strCheckID 
				//+ " and A.FState='finish' and A.FcheckType=B.FTypeID and B.FActionType=2 and A.FCheckResult=0"; //�ж�����  A.FCheckResult=0 ��ʾͨ��������
				+ " and A.FNewState=2 and A.FcheckType=B.FTypeID and B.FActionType=2 and A.FCheckResult=0"; //�ж�����  A.FCheckResult=0 ��ʾͨ��������

			if(da.GetOneResult(strSql) == "1")//�˴�1��ʾ��ʾ ��������ִ�����FActionType=2����ִ�д��ڣ�ִ������ִ�С�//����ִ�У�������İ�ť����ִ�С�
			{
				if(!FinishCheck.ExecuteCheck(strCheckID,myHeader,da))
				{
					//��ʱ��Ҫ�����쳣����������rollback
					throw new LogicException("ִ����������ʱ�����˴���");
				}
			}
		}
	}
}
