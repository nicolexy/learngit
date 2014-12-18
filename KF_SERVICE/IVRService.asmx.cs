using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.EnterpriseServices;
using System.Configuration;
using TENCENT.OSS.CFT.KF.DataAccess;
using TENCENT.OSS.CFT.KF.Common;

using TENCENT.OSS.C2C.Finance.DataAccess;
using TENCENT.OSS.C2C.Finance.Common;
using TENCENT.OSS.C2C.Finance.Common.CommLib;
using TENCENT.OSS.C2C.Finance.BankLib;
using TENCENT.OSS.CFT.KF.KF_Service;
using System.Xml;

namespace Tencent.KF.IVR
{
	/// <summary>
	/// IVRService ��ժҪ˵����
	/// </summary>
	public class IVRService : System.Web.Services.WebService
	{
		public IVRService()
		{
			//CODEGEN: �õ����� ASP.NET Web ����������������
			InitializeComponent();
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


        /// <summary>
        /// �����������
        /// </summary>
        /// <param name="strCheckID">���ߵ��ɣ�</param>
        /// <param name="strUserID">�Ƹ�ͨ�˺�</param>
        /// <param name="strMobile">ԭ�ֻ�����</param>
        /// <param name="intCallNum">�ڼ��κ���</param>
        /// <returns>1�������ݲ��ɹ����أ������������ݣ� -1��ִ���쳣</returns>
        [WebMethod]
        public int GetIVRData(out string strCheckID, out string strUserID, out string strMobile, out int intCallNum)
        {
            strCheckID = "";
            strUserID = "";
            strMobile = "";
            intCallNum = 1;

            //���ʱ�����ƣ�0����8�㲻���
            DateTime d = DateTime.Now;
            if (d.Hour >= 0 && d.Hour < 8)
            {
                return 0;
            }

            //IVR���ר��furion
            //1,2�Ȳ���,��������Ҫʱ������δ��ȡ����¼ʱ���²������.
            //1:��ȡ�������û�еķ������������߱�����(0,8),���������ȡ������е��������ID,ֻ�ô����������������.
            //2:���������.���д���0,״̬Ϊ����ɹ�.

            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("CFT"));
            try
            {
                string onehourprior = DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss");

                da.OpenConn();
                //3:ȡ��������(���д���Ϊ0����д�������0���ҵ�һ�κ���ʱ���ѹ�1Сʱ����С���ߵ�ID)
                string strSql = "select * from t_tenpay_appeal_IVR where Fappealtime>='" + DateTime.Now.AddHours(-48) + "' and (Fcallnum=0 or (Fcallnum=1 and Flastcalltime<='" + onehourprior + "' and Fstate in (1,3))) order by Fappealtime limit 1 ";
                DataTable dt = da.GetTable(strSql);

                if (dt == null || dt.Rows.Count == 0)
                {
                    return 0;
                }

                strCheckID = dt.Rows[0]["FAppealID"].ToString();
                strUserID = dt.Rows[0]["Fuin"].ToString();
                strMobile = dt.Rows[0]["Fmobile"].ToString();
                intCallNum = Int32.Parse(dt.Rows[0]["Fcallnum"].ToString()) + 1;
                //4:��״̬Ϊ�ѷ��ͺ���,���д�����1,�ͳ�����.
                strSql = "update t_tenpay_appeal_IVR set Fcallnum=" + intCallNum + ",Fstate=1,Flastcalltime=now(),Fmodifytime=now() where Fappealid=" + strCheckID;
                if (da.ExecSqlNum(strSql) == 1)
                {
                    return 1;
                }
                else
                {
                    loger.err("GetIVRData", "���¼�¼ʱ����" + strCheckID);
                    return -1;
                }
            }
            catch (Exception err)
            {
                loger.err("GetIVRData", "ִ�г���" + err.Message);
                return -1;
            }
            finally
            {
                da.Dispose();
            }
        }

        /// <summary>
        /// ����������
        /// </summary>
        /// <param name="strCheckID">���ߵ��ɣ�</param>
        /// <param name="strUserID">>�Ƹ�ͨ�˺�</param>
        /// <param name="strMobile">ԭ�ֻ�����</param>
        /// <param name="intResult">���н��</param>
        /// <param name="strMemo">���б�ע</param>
        /// <param name="MD5">MD5��Ϣ��У��ʹ��</param>
        /// <returns>1:�ɹ����� 0:δ�ܳɹ����� -1:�����쳣��</returns>
        [WebMethod]
        public int SendIVRResult(string strCheckID, string strUserID, string strMobile, int intResult, string strMemo, string MD5)
        {
            //IVR���ר��furion
            //1:��ǩ.
            string ivrmd5 = System.Configuration.ConfigurationManager.AppSettings["Appeal_IVRMd5"];
            string sourcestr = strCheckID + strUserID + strMobile + intResult + strMemo + ivrmd5;
            string md5 = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sourcestr, "md5").ToLower();
            if (md5 != MD5.ToLower())
            {
                //��ǩʧ��.
                loger.err("SendIVRResult", "��ǩʧ��" + sourcestr + "|||" + MD5);
                return -1;
            }

            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("CFT"));
            try
            {
                da.OpenConn();
                //2:��ѯ���ߵ�ID�Ƿ�������������,�����������,����Ӧ���ֶ�ֵ,(��N��������,�����ע,��������)
                string strSql = "select * from t_tenpay_appeal_IVR where FappealID=" + strCheckID;
                DataTable dt = da.GetTable(strSql);

                if (dt == null || dt.Rows.Count == 0)
                {
                    //�����Ҳ�����¼,Ӧ�����쳣.
                    loger.err("SendIVRResult", "���Ҳ��������" + strCheckID);
                    return -1;
                }

                DataRow dr = dt.Rows[0];

                if (dr["Fmobile"].ToString() != strMobile)
                {
                    if ("0" + dr["Fmobile"].ToString() != strMobile)
                    {
                        //����ط��쳣
                        loger.err("SendIVRResult", "�ֻ���������" + strCheckID + "||" + strMobile);
                        return -1;
                    }
                }

                if (dr["FState"].ToString() == "1") //״̬Ϊ�ѷ��ͺ���
                {
                    //����modifytime callresult,callmemo,������callresult����state�ֶ�.
                    //1:�û������ظ�1ͬ��.
                    //2:�û������ظ�2�ܾ�
                    //3:�û������ظ�����ֵ.
                    //4:�û��������绰.
                    //5:�û������һ�
                    //6:�����޷�����(�պ�,�ػ�)
                   // 7 Ivr�����һ�(����1�����û�û�а���)
                    if (intResult < 1 || intResult > 7)
                    {
                        //����������Χ.
                        loger.err("SendIVRResult", "���н������" + strCheckID + "|" + intResult);
                        return -1;
                    }

                    //2013.09.02 lxl
                    //�ж��Ǹ߷ֵ����ǵͷֵ����ͷֵ��ܾ����߻����˹��������߷ֵ����Ǿܾ����߻�ֱ��ͨ��
                    string SqlDiFen = "select FParameter from t_tenpay_appeal_trans where Fid='" + strCheckID + "' and FParameter not like '%&AUTO_APPEAL=1%'";
                    DataTable dtDiFen = da.GetTable(SqlDiFen);

                    //�߷ֵ�
                    if (dtDiFen == null || dtDiFen.Rows.Count == 0)
                    {
                        if (intResult == 1 || intResult == 4 || intResult == 5 || intResult == 7)//20130716���intResult == 5�������޸�//20131210 lxl ���7
                        {
                            //1,4ʱͨ������,����ܾ�����.
                            strSql = "update t_tenpay_appeal_IVR set Fstate=2,Fmodifytime=now(),Fcallresult=" + intResult + ",Fcallmemo='" + PublicRes.replaceMStr(strMemo)
                                + "' where Fappealid='" + strCheckID + "'";
                        }
                        else
                        {
                            strSql = "update t_tenpay_appeal_IVR set Fstate=3,Fmodifytime=now(),Fcallresult=" + intResult + ",Fcallmemo='" + PublicRes.replaceMStr(strMemo)
                                + "' where Fappealid='" + strCheckID + "'";
                        }
                    }
                    else//�ͷֵ�
                    {
                        if (intResult == 2 || (intResult == 6 && dr["Fcallnum"].ToString() == "1")) //2ʱͨ���ܾ�����,�����˹�����
                        {
                            strSql = "update t_tenpay_appeal_IVR set Fstate=3,Fmodifytime=now(),Fcallresult=" + intResult + ",Fcallmemo='" + PublicRes.replaceMStr(strMemo)
                                + "' where Fappealid='" + strCheckID + "'";
                        }
                        else//Fstate=4��һ��״̬����ʾ�ͷֵ��˹�����
                        {
                            strSql = "update t_tenpay_appeal_IVR set Fstate=4,Fmodifytime=now(),Fcallresult=" + intResult + ",Fcallmemo='" + PublicRes.replaceMStr(strMemo)
                                + "' where Fappealid='" + strCheckID + "'";
                        }
                    }

                    if (da.ExecSqlNum(strSql) != 1)
                    {
                        loger.err("SendIVRResult", "���º��е�ʧ��" + strCheckID);
                        return -1;
                    }


                    //���ж����ߵ�����״̬�Ƿ���,ԭ״̬(0,8).
                    strSql = "select Fstate from t_tenpay_appeal_trans where FID=" + strCheckID;
                    string state = da.GetOneResult(strSql);

                    if (state == "0" || state == "8")
                    {
                        //�߷ֵ�
                        if (dtDiFen == null || dtDiFen.Rows.Count == 0)
                        {
                            //����ȷ��ͨ��,��ܾ�����.
                            if (intResult == 1 || intResult == 4 || intResult == 5 || intResult == 7)//20130716���intResult == 5�������޸�//20131210 lxl ���7
                            {
                                string mesg = "";
                                if (CFTUserAppealClass.ConfirmAppeal(int.Parse(strCheckID), "", "system", "127.0.0.2", out mesg))
                                {
                                    return 1;
                                }
                                else
                                {
                                    loger.err("SendIVRResult", "ͨ�����ߵ�ʱʧ��" + strCheckID + "||" + mesg);
                                    return -1;
                                }
                            }
                            else
                            {
                                //6:�����޷�����(�պ�,�ػ�) ���������,����ǵ�һ�κ���,�����ٽ��дκ���,�Ȳ�ִ�оܾ�����.
                                if (intResult == 6 && dr["Fcallnum"].ToString() == "1")
                                {
                                    return 1;
                                }

                                string mesg = "";
                                if (CFTUserAppealClass.CancelAppeal(int.Parse(strCheckID), "ԭ���ֻ��ܾ�", "ԭ���ֻ��ܾ�", strMemo, "system", "127.0.0.2", out mesg))
                                {
                                    //����������Fcallnum=3,��Զ�������е�����������Դ���Ϊ���
                                    strSql = "update t_tenpay_appeal_IVR set Fcallnum=3, Fmodifytime=now() where Fappealid='" + strCheckID + "'";
                                    if (da.ExecSqlNum(strSql) != 1)
                                    {
                                        loger.err("SendIVRResult", "�������������º��е�ʧ��" + strCheckID);
                                        return -1;
                                    }
                                    return 1;
                                }
                                else
                                {
                                    loger.err("SendIVRResult", "�ܾ����ߵ�ʱʧ��" + strCheckID + "||" + mesg);
                                    return -1;
                                }
                            }
                            return 1;
                        }
                        else//�ͷֵ�
                        {
                            //���оܾ�����,�������˹��������̣���t_tenpay_appeal_trans����Ϊ��δ����״̬��Fstate=0
                            if (intResult == 1 || intResult == 4 || intResult == 5 || intResult == 3 || intResult == 6 || intResult == 7)//20131210 lxl ���7
                            {
                                //6:�����޷�����(�պ�,�ػ�) ���������,����ǵ�һ�κ���,�����ٽ��дκ���,�Ȳ�ִ�оܾ�����.
                                if (intResult == 6 && dr["Fcallnum"].ToString() == "1")
                                {
                                    return 1;
                                }
                                SqlDiFen = "update t_tenpay_appeal_trans set FPickUser='system',FPickTime=now(),Fstate=0 where Fid='" + strCheckID + "' and FUin='" + dr["Fuin"].ToString() + "'";
                                da.ExecSql(SqlDiFen);
                                return 1;
                            }
                            else//intResult=2ʱ
                            {
                                string mesg = "";
                                if (CFTUserAppealClass.CancelAppeal(int.Parse(strCheckID), "ԭ���ֻ��ܾ�", "ԭ���ֻ��ܾ�", strMemo, "system", "127.0.0.2", out mesg))
                                {
                                    //����������
                                    SqlDiFen = "update t_tenpay_appeal_IVR set Fcallnum=3, Fmodifytime=now() where Fappealid='" + strCheckID + "'";
                                    if (da.ExecSqlNum(SqlDiFen) != 1)
                                    {
                                        loger.err("SendIVRResult", "�������������º��е�ʧ��" + strCheckID);
                                        return -1;
                                    }
                                    return 1;
                                }
                                else
                                {
                                    loger.err("SendIVRResult", "�ܾ����ߵ�ʱʧ��" + strCheckID + "||" + mesg);
                                    return -1;
                                }
                            }

                            return 1;
                        }

                    }
                    else //if(state == "0" || state == "8")
                    {
                        return 0;
                    }
                }
                else//if(dr["FState"].ToString() == "1")
                {
                    //״̬��Ԥ�ڷ������,�������ظ��ؽ������.
                    return 0;
                }
            }
            catch (Exception err)
            {
                loger.err("SendIVRResult", "ִ���쳣" + strCheckID + "||" + err.Message);
                return -1;
            }
            finally
            {
                da.Dispose();
            }
        }


		/// <summary>
		/// �����������
		/// </summary>
		/// <param name="strCheckID">���ߵ��ɣ�</param>
		/// <param name="strUserID">�Ƹ�ͨ�˺�</param>
		/// <param name="strMobile">ԭ�ֻ�����</param>
		/// <param name="intCallNum">�ڼ��κ���</param>
        /// <param name="dbName">IVR����</param>
        /// <param name="tbName">IVR����</param>
		/// <returns>1�������ݲ��ɹ����أ������������ݣ� -1��ִ���쳣</returns>
        /// 20131113 lxl �Ӳ���dbName tbName
		[WebMethod]
		public  int GetIVRDataNew(out string dbName,out string tbName,out string strCheckID,out string strUserID,out string strMobile,out int intCallNum)
		{
			strCheckID = "";
			strUserID = "";
			strMobile = "";
			intCallNum = 1;
            dbName = "";
            tbName = "";

            //���ʱ�����ƣ�0����8�㲻���
            DateTime d = DateTime.Now;
            if (d.Hour >= 0 && d.Hour < 8)
            {
                return 0;
            }

			//IVR���ר��furion
			//1,2�Ȳ���,��������Ҫʱ������δ��ȡ����¼ʱ���²������.
			//1:��ȡ�������û�еķ������������߱�����(0,8),���������ȡ������е��������ID,ֻ�ô����������������.
			//2:���������.���д���0,״̬Ϊ����ɹ�.

			MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("CFT"));
            MySqlAccess daIVRFen = new MySqlAccess(PublicRes.GetConnString("IVRNEW"));//IVR�·ֿ�ֱ� 
			try
			{
				string onehourprior = DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss");

				da.OpenConn();
                daIVRFen.OpenConn();
				//3:ȡ��������(���д���Ϊ0����д�������0���ҵ�һ�κ���ʱ���ѹ�1Сʱ����С���ߵ�ID)
                string strSql = "select * from t_tenpay_appeal_IVR where Fappealtime>='"+DateTime.Now.AddHours(-48)+"' and (Fcallnum=0 or (Fcallnum=1 and Flastcalltime<='" + onehourprior + "' and Fstate in (1,3))) order by Fappealtime limit 1 ";
				DataTable dt = da.GetTable(strSql);

                if (dt == null || dt.Rows.Count == 0)
                {
                    int year = DateTime.Now.Year;
                    int month = DateTime.Now.Month;

                    string str1 = "", str2 = "";
                    if (month < 10)
                        str1 = "select 'db_apeal_IVR_" + year + "' as dbName, 't_tenpay_appeal_IVR_0" + month + "' as tbName, FAppealID,Fuin,Fmobile ,Fcallnum, Fappealtime from db_apeal_IVR_" + year + ".t_tenpay_appeal_IVR_0" + month + " where Fappealtime>='" + DateTime.Now.AddHours(-48) + "' and (Fcallnum=0 or (Fcallnum=1 and Flastcalltime<='" + onehourprior + "' and Fstate in (1,3)))";
                    else
                        str1 = "select 'db_apeal_IVR_" + year + "' as dbName, 't_tenpay_appeal_IVR_" + month + "' as tbName, FAppealID,Fuin,Fmobile ,Fcallnum, Fappealtime  from db_apeal_IVR_" + year + ".t_tenpay_appeal_IVR_" + month + " where Fappealtime>='" + DateTime.Now.AddHours(-48) + "' and (Fcallnum=0 or (Fcallnum=1 and Flastcalltime<='" + onehourprior + "' and Fstate in (1,3)))";

                    if (month != DateTime.Now.AddHours(-48).Month)//����һ���·ݣ���Ҫ�Ӳ��ϸ���
                    {
                        if (month == 1)
                            str2 = "select 'db_apeal_IVR_" + (year - 1) + "' as dbName, 't_tenpay_appeal_IVR_" + 12 + "' as tbName, FAppealID,Fuin,Fmobile ,Fcallnum, Fappealtime  from db_apeal_IVR_" + (year - 1) + ".t_tenpay_appeal_IVR_" + 12 + " where Fappealtime>='" + DateTime.Now.AddHours(-48) + "' and (Fcallnum=0 or (Fcallnum=1 and Flastcalltime<='" + onehourprior + "' and Fstate in (1,3)))";
                        else
                        {
                            if (month - 1 < 10)
                                str2 = "select 'db_apeal_IVR_" + year + "' as dbName, 't_tenpay_appeal_IVR_0" + (month - 1) + "' as tbName, FAppealID,Fuin,Fmobile ,Fcallnum, Fappealtime  from db_apeal_IVR_" + year + ".t_tenpay_appeal_IVR_0" + (month - 1) + " where Fappealtime>='" + DateTime.Now.AddHours(-48) + "' and (Fcallnum=0 or (Fcallnum=1 and Flastcalltime<='" + onehourprior + "' and Fstate in (1,3)))";
                            else
                                str2 = "select 'db_apeal_IVR_" + year + "' as dbName, 't_tenpay_appeal_IVR_" + (month - 1) + "' as tbName, FAppealID,Fuin,Fmobile ,Fcallnum, Fappealtime  from db_apeal_IVR_" + year + ".t_tenpay_appeal_IVR_" + (month - 1) + " where Fappealtime>='" + DateTime.Now.AddHours(-48) + "' and (Fcallnum=0 or (Fcallnum=1 and Flastcalltime<='" + onehourprior + "' and Fstate in (1,3)))";
                        }
                    }


                    if (str2 == "")
                        strSql = str1 + " order by Fappealtime limit 1";
                    else
                        strSql = str1 + " union " + str2 + " order by Fappealtime limit 1";
                    dt = daIVRFen.GetTable(strSql);
                    if (dt == null || dt.Rows.Count == 0)
                    {
                        return 0;
                    }
                    else
                    {
                        dbName = dt.Rows[0]["dbName"].ToString();
                        tbName = dt.Rows[0]["tbName"].ToString();
                    }

                }

				strCheckID = dt.Rows[0]["FAppealID"].ToString();
				strUserID = dt.Rows[0]["Fuin"].ToString();
				strMobile = dt.Rows[0]["Fmobile"].ToString();
				intCallNum = Int32.Parse(dt.Rows[0]["Fcallnum"].ToString()) + 1;
				//4:��״̬Ϊ�ѷ��ͺ���,���д�����1,�ͳ�����.
                if (dbName == "" && tbName == "")
                {
                    strSql = "update t_tenpay_appeal_IVR set Fcallnum=" + intCallNum + ",Fstate=1,Flastcalltime=now(),Fmodifytime=now() where Fappealid=" + strCheckID;
                    if (da.ExecSqlNum(strSql) == 1)
                    {
                        return 1;
                    }
                    else
                    {
                        loger.err("GetIVRDataNew", "���¼�¼ʱ����" + strCheckID);
                        return -1;
                    }
                }
                else
                {
                    strSql = "update " + dbName + "." + tbName + " set Fcallnum=" + intCallNum + ",Fstate=1,Flastcalltime=now(),Fmodifytime=now() where Fappealid='" + strCheckID+"'" ;
                    if (daIVRFen.ExecSqlNum(strSql) == 1)
                    {
                        return 1;
                    }
                    else
                    {
                        loger.err("GetIVRDataNew", "���¼�¼ʱ����,���:" + dbName + "." + tbName + "; Fappealid:" + strCheckID);
                        return -1;
                    }
                } 
			}
			catch(Exception err)
			{
                loger.err("GetIVRDataNew", "ִ�г���" + err.Message);
				return -1;
			}
			finally
			{
				da.Dispose();
                daIVRFen.Dispose();
			}
		}

		/// <summary>
		/// ����������
		/// </summary>
        /// /// <param name="dbName">IVR����</param>
        /// <param name="tbName">IVR����</param>
		/// <param name="strCheckID">���ߵ��ɣ�</param>
		/// <param name="strUserID">>�Ƹ�ͨ�˺�</param>
		/// <param name="strMobile">ԭ�ֻ�����</param>
		/// <param name="intResult">���н��</param>
		/// <param name="strMemo">���б�ע</param>
		/// <param name="MD5">MD5��Ϣ��У��ʹ��</param>
		/// <returns>1:�ɹ����� 0:δ�ܳɹ����� -1:�����쳣��</returns>
		[WebMethod]
		public int SendIVRResultNew(string dbName, string tbName,string strCheckID, string strUserID, string strMobile,int intResult,string strMemo,string MD5)
		{
			//IVR���ר��furion
			//1:��ǩ.
			string ivrmd5 = System.Configuration.ConfigurationManager.AppSettings["Appeal_IVRMd5"];
            string sourcestr = dbName + tbName + strCheckID + strUserID + strMobile + intResult + strMemo + ivrmd5;
			string md5 = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sourcestr,"md5").ToLower();
			if(md5 != MD5.ToLower())
			{
				//��ǩʧ��.
                loger.err("SendIVRResultNew", "��ǩʧ��" + sourcestr + "|||" + MD5);
				return -1;
			}

			MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("CFT"));
            MySqlAccess daIVRFen = new MySqlAccess(PublicRes.GetConnString("IVRNEW"));//IVR�·ֿ�ֱ�
            MySqlAccess daFen = new MySqlAccess(PublicRes.GetConnString("CFTNEW"));//�·ֿ�ֱ� 
            try
            {
                da.OpenConn();
                daIVRFen.OpenConn();
                daFen.OpenConn();

                //��ǰ����
                if ((dbName == "" && tbName == "") || (dbName == null || tbName == null))
                {
                    #region
                    //2:��ѯ���ߵ�ID�Ƿ�������������,�����������,����Ӧ���ֶ�ֵ,(��N��������,�����ע,��������)
                    string strSql = "select * from t_tenpay_appeal_IVR where FappealID=" + strCheckID;
                    DataTable dt = da.GetTable(strSql);

                    if (dt == null || dt.Rows.Count == 0)
                    {
                        //�����Ҳ�����¼,Ӧ�����쳣.
                        loger.err("SendIVRResultNew", "���Ҳ��������" + strCheckID);
                        return -1;
                    }

                    DataRow dr = dt.Rows[0];

                    if (dr["Fmobile"].ToString() != strMobile)
                    {
                        if ("0" + dr["Fmobile"].ToString() != strMobile)
                        {
                            //����ط��쳣
                            loger.err("SendIVRResultNew", "�ֻ���������" + strCheckID + "||" + strMobile);
                            return -1;
                        }
                    }

                    if (dr["FState"].ToString() == "1") //״̬Ϊ�ѷ��ͺ���
                    {
                        //����modifytime callresult,callmemo,������callresult����state�ֶ�.
                        //1:�û������ظ�1ͬ��.
                        //2:�û������ظ�2�ܾ�
                        //3:�û������ظ�����ֵ.
                        //4:�û��������绰.
                        //5:�û������һ�
                        //6:�����޷�����(�պ�,�ػ�)
                        // 7 Ivr�����һ�(����1�����û�û�а���)
                        if (intResult < 1 || intResult > 7)
                        {
                            //����������Χ.
                            loger.err("SendIVRResultNew", "���н������" + strCheckID + "|" + intResult);
                            return -1;
                        }

                        //2013.09.02 lxl
                        //�ж��Ǹ߷ֵ����ǵͷֵ����ͷֵ��ܾ����߻����˹��������߷ֵ����Ǿܾ����߻�ֱ��ͨ��
                        string SqlDiFen = "select FParameter from t_tenpay_appeal_trans where Fid='" + strCheckID + "' and FParameter not like '%&AUTO_APPEAL=1%'";
                        DataTable dtDiFen = da.GetTable(SqlDiFen);

                        //�߷ֵ�
                        if (dtDiFen == null || dtDiFen.Rows.Count == 0)
                        {
                            if (intResult == 1 || intResult == 4 || intResult == 5 || intResult == 7)//20130716���intResult == 5�������޸�
                            {
                                //1,4,5ʱͨ������,����ܾ�����.
                                strSql = "update t_tenpay_appeal_IVR set Fstate=2,Fmodifytime=now(),Fcallresult=" + intResult + ",Fcallmemo='" + PublicRes.replaceMStr(strMemo)
                                    + "' where Fappealid='" + strCheckID + "'";
                            }
                            else
                            {
                                strSql = "update t_tenpay_appeal_IVR set Fstate=3,Fmodifytime=now(),Fcallresult=" + intResult + ",Fcallmemo='" + PublicRes.replaceMStr(strMemo)
                                    + "' where Fappealid='" + strCheckID + "'";
                            }
                        }
                        else//�ͷֵ�
                        {
                            if (intResult == 2 || (intResult == 6 && dr["Fcallnum"].ToString() == "1")) //2ʱͨ���ܾ�����,�����˹�����
                            {
                                strSql = "update t_tenpay_appeal_IVR set Fstate=3,Fmodifytime=now(),Fcallresult=" + intResult + ",Fcallmemo='" + PublicRes.replaceMStr(strMemo)
                                    + "' where Fappealid='" + strCheckID + "'";
                            }
                            else//Fstate=4��һ��״̬����ʾ�ͷֵ��˹�����
                            {
                                strSql = "update t_tenpay_appeal_IVR set Fstate=4,Fmodifytime=now(),Fcallresult=" + intResult + ",Fcallmemo='" + PublicRes.replaceMStr(strMemo)
                                    + "' where Fappealid='" + strCheckID + "'";
                            }
                        }

                        if (da.ExecSqlNum(strSql) != 1)
                        {
                            loger.err("SendIVRResultNew", "���º��е�ʧ��" + strCheckID);
                            return -1;
                        }


                        //���ж����ߵ�����״̬�Ƿ���,ԭ״̬(0,8).
                        strSql = "select Fstate from t_tenpay_appeal_trans where FID=" + strCheckID;
                        string state = da.GetOneResult(strSql);

                        if (state == "0" || state == "8")
                        {
                            //�߷ֵ�
                            if (dtDiFen == null || dtDiFen.Rows.Count == 0)
                            {
                                //����ȷ��ͨ��,��ܾ�����.
                                if (intResult == 1 || intResult == 4 || intResult == 5 || intResult == 7)//20130716���intResult == 5�������޸�
                                {
                                    string mesg = "";
                                    if (CFTUserAppealClass.ConfirmAppeal(int.Parse(strCheckID), "", "system", "127.0.0.2", out mesg))
                                    {
                                        return 1;
                                    }
                                    else
                                    {
                                        loger.err("SendIVRResultNew", "ͨ�����ߵ�ʱʧ��" + strCheckID + "||" + mesg);
                                        return -1;
                                    }
                                }
                                else
                                {
                                    //6:�����޷�����(�պ�,�ػ�) ���������,����ǵ�һ�κ���,�����ٽ��дκ���,�Ȳ�ִ�оܾ�����.
                                    if (intResult == 6 && dr["Fcallnum"].ToString() == "1")
                                    {
                                        return 1;
                                    }

                                    string mesg = "";
                                    if (CFTUserAppealClass.CancelAppeal(int.Parse(strCheckID), "ԭ���ֻ��ܾ�", "ԭ���ֻ��ܾ�", strMemo, "system", "127.0.0.2", out mesg))
                                    {
                                        //����������Fcallnum=3,��Զ�������е�����������Դ���Ϊ���
                                        strSql = "update t_tenpay_appeal_IVR set Fcallnum=3, Fmodifytime=now() where Fappealid='" + strCheckID + "'";
                                        if (da.ExecSqlNum(strSql) != 1)
                                        {
                                            loger.err("SendIVRResultNew", "�������������º��е�ʧ��" + strCheckID);
                                            return -1;
                                        }
                                        return 1;
                                    }
                                    else
                                    {
                                        loger.err("SendIVRResultNew", "�ܾ����ߵ�ʱʧ��" + strCheckID + "||" + mesg);
                                        return -1;
                                    }
                                }
                                return 1;
                            }
                            else//�ͷֵ�
                            {
                                //���оܾ�����,�������˹��������̣���t_tenpay_appeal_trans����Ϊ��δ����״̬��Fstate=0
                                if (intResult == 1 || intResult == 4 || intResult == 5 || intResult == 3 || intResult == 6 || intResult == 7)
                                {
                                    //6:�����޷�����(�պ�,�ػ�) ���������,����ǵ�һ�κ���,�����ٽ��дκ���,�Ȳ�ִ�оܾ�����.
                                    if (intResult == 6 && dr["Fcallnum"].ToString() == "1")
                                    {
                                        return 1;
                                    }
                                    SqlDiFen = "update t_tenpay_appeal_trans set FPickUser='system',FPickTime=now(),Fstate=0 where Fid='" + strCheckID + "' and FUin='" + dr["Fuin"].ToString() + "'";
                                    da.ExecSql(SqlDiFen);
                                    return 1;
                                }
                                else//intResult=2ʱ
                                {
                                    string mesg = "";
                                    if (CFTUserAppealClass.CancelAppeal(int.Parse(strCheckID), "ԭ���ֻ��ܾ�", "ԭ���ֻ��ܾ�", strMemo, "system", "127.0.0.2", out mesg))
                                    {
                                        //����������
                                        SqlDiFen = "update t_tenpay_appeal_IVR set Fcallnum=3, Fmodifytime=now() where Fappealid='" + strCheckID + "'";
                                        if (da.ExecSqlNum(SqlDiFen) != 1)
                                        {
                                            loger.err("SendIVRResultNew", "�������������º��е�ʧ��" + strCheckID);
                                            return -1;
                                        }
                                        return 1;
                                    }
                                    else
                                    {
                                        loger.err("SendIVRResultNew", "�ܾ����ߵ�ʱʧ��" + strCheckID + "||" + mesg);
                                        return -1;
                                    }
                                }

                                return 1;
                            }

                        }
                        else //if(state == "0" || state == "8")
                        {
                            return 0;
                        }
                    }
                    else//if(dr["FState"].ToString() == "1")
                    {
                        //״̬��Ԥ�ڷ������,�������ظ��ؽ������.
                        return 0;
                    }

                    #endregion
                }
                else//�ֿ�ֱ�����
                {

                    #region
                    //2:��ѯ���ߵ�ID�Ƿ�������������,�����������,����Ӧ���ֶ�ֵ,(��N��������,�����ע,��������)
                    string strSql = "select * from " + dbName + "." + tbName + " where FappealID='" + strCheckID + "'";
                    DataTable dt = daIVRFen.GetTable(strSql);

                    if (dt == null || dt.Rows.Count == 0)
                    {
                        //�����Ҳ�����¼,Ӧ�����쳣.
                        loger.err("SendIVRResultNew", "���Ҳ��������  ���ݱ�" + dbName + "." + tbName + "   FappealID:" + strCheckID);
                        return -1;
                    }

                    DataRow dr = dt.Rows[0];

                    if (dr["Fmobile"].ToString() != strMobile)
                    {
                        if ("0" + dr["Fmobile"].ToString() != strMobile)
                        {
                            //����ط��쳣
                            loger.err("SendIVRResultNew", "�ֻ���������  ���ݱ�" + dbName + "." + tbName + "   FappealID:" + strCheckID + "||" + strMobile);
                            return -1;
                        }
                    }

                    if (dr["FState"].ToString() == "1") //״̬Ϊ�ѷ��ͺ���
                    {
                        //����modifytime callresult,callmemo,������callresult����state�ֶ�.
                        //1:�û������ظ�1ͬ��.
                        //2:�û������ظ�2�ܾ�
                        //3:�û������ظ�����ֵ.
                        //4:�û��������绰.
                        //5:�û������һ�
                        //6:�����޷�����(�պ�,�ػ�)
                        if (intResult < 1 || intResult > 7)
                        {
                            //����������Χ.
                            loger.err("SendIVRResultNew", "���н������  ���ݱ�" + dbName + "." + tbName + "   FappealID:" + strCheckID + "|" + intResult);
                            return -1;
                        }

                        //�ж��Ǹ߷ֵ����ǵͷֵ����ͷֵ��ܾ����߻����˹��������߷ֵ����Ǿܾ����߻�ֱ��ͨ��
                        string SqlDiFen = "select FAutoAppeal from " + dbName + "." + tbName + " where FappealID='" + strCheckID + "' and FAutoAppeal=0";
                        DataTable dtDiFen = daIVRFen.GetTable(SqlDiFen);

                         //�߷ֵ�
                        if (dtDiFen == null || dtDiFen.Rows.Count == 0)
                        {
                            if (intResult == 1 || intResult == 4 || intResult == 5 || intResult == 7)//20130716���intResult == 5�������޸�
                            {
                                //1,4,5ʱͨ������,����ܾ�����.
                                strSql = "update  " + dbName + "." + tbName + "  set Fstate=2,Fmodifytime=now(),Fcallresult=" + intResult + ",Fcallmemo='" + PublicRes.replaceMStr(strMemo)
                                    + "' where Fappealid='" + strCheckID + "'";
                            }
                            else
                            {
                                strSql = "update  " + dbName + "." + tbName + "  set Fstate=3,Fmodifytime=now(),Fcallresult=" + intResult + ",Fcallmemo='" + PublicRes.replaceMStr(strMemo)
                                    + "' where Fappealid='" + strCheckID + "'";
                            }
                        }
                        else//�ͷֵ�
                        {
                            //ֻ�еͷֵ�
                            if (intResult == 2 || (intResult == 6 && dr["Fcallnum"].ToString() == "1")) //2ʱͨ���ܾ�����,�����˹�����
                            {
                                strSql = "update   " + dbName + "." + tbName + "  set Fstate=3,Fmodifytime=now(),Fcallresult=" + intResult + ",Fcallmemo='" + PublicRes.replaceMStr(strMemo)
                                    + "' where Fappealid='" + strCheckID + "'";
                            }
                            else//Fstate=4��һ��״̬����ʾ�ͷֵ��˹�����
                            {
                                strSql = "update   " + dbName + "." + tbName + "  set Fstate=4,Fmodifytime=now(),Fcallresult=" + intResult + ",Fcallmemo='" + PublicRes.replaceMStr(strMemo)
                                    + "' where Fappealid='" + strCheckID + "'";
                            }
                        }


                        if (daIVRFen.ExecSqlNum(strSql) != 1)
                        {
                            loger.err("SendIVRResult", "���º��е�ʧ��  ���ݱ�" + dbName + "." + tbName + "   FappealID:" + strCheckID);
                            return -1;
                        }

                        string dateTime = dr["FAppealTime"].ToString();
                        int year = DateTime.Parse(dateTime).Year;
                        int month = DateTime.Parse(dateTime).Month;
                        string dbAppeal = "db_appeal_" + year;
                        string tbAppeal = "";
                        if (month < 10)
                            tbAppeal = "t_tenpay_appeal_trans_0" + month;
                        else
                            tbAppeal = "t_tenpay_appeal_trans_" + month;

                        //���ж����ߵ�����״̬�Ƿ���,ԭ״̬(0,8).
                        strSql = "select Fstate from " + dbAppeal + "." + tbAppeal + " where FID='" + strCheckID + "'";

                        string state = daFen.GetOneResult(strSql);

                        #region
                        if (state == "0" || state == "8")
                        {
                            //�߷ֵ�
                            if (dtDiFen == null || dtDiFen.Rows.Count == 0)
                            {
                                //����ȷ��ͨ��,��ܾ�����.
                                if (intResult == 1 || intResult == 4 || intResult == 5 || intResult == 7)
                                {
                                    string mesg = "";
                                    if (CFTUserAppealClass.ConfirmAppealDBTB(strCheckID, dbAppeal, tbAppeal, "", "system", "127.0.0.2", out mesg))
                                    {
                                        return 1;
                                    }
                                    else
                                    {
                                        loger.err("SendIVRResultNew", "ͨ�����ߵ�ʱʧ�� ���ݱ�" + dbName + "." + tbName + "   FappealID:" + strCheckID + "||" + mesg);
                                        return -1;
                                    }
                                }
                                else
                                {
                                    //6:�����޷�����(�պ�,�ػ�) ���������,����ǵ�һ�κ���,�����ٽ��дκ���,�Ȳ�ִ�оܾ�����.
                                    if (intResult == 6 && dr["Fcallnum"].ToString() == "1")
                                    {
                                        return 1;
                                    }

                                    string mesg = "";
                                    if (CFTUserAppealClass.CancelAppealDBTB(strCheckID, dbAppeal, tbAppeal, "ԭ���ֻ��ܾ�", "ԭ���ֻ��ܾ�", strMemo, "system", "127.0.0.2", out mesg))
                                    {
                                        //����������Fcallnum=3,��Զ�������е�����������Դ���Ϊ���
                                        strSql = "update  " + dbName + "." + tbName + "  set Fcallnum=3, Fmodifytime=now() where Fappealid='" + strCheckID + "'";
                                        if (daIVRFen.ExecSqlNum(strSql) != 1)
                                        {
                                            loger.err("SendIVRResultNew", "�������������º��е�ʧ�� ���ݱ�" + dbName + "." + tbName + "   FappealID:" + strCheckID);
                                            return -1;
                                        }
                                        return 1;
                                    }
                                    else
                                    {
                                        loger.err("SendIVRResultNew", "�ܾ����ߵ�ʱʧ�� ���ݱ�" + dbName + "." + tbName + "   FappealID:" + strCheckID + "||" + mesg);
                                        return -1;
                                    }
                                }
                                return 1;
                            }
                            else//�ͷֵ�
                            {
                                //���оܾ�����,�������˹��������̣���t_tenpay_appeal_trans����Ϊ��δ����״̬��Fstate=0
                                if (intResult == 1 || intResult == 4 || intResult == 5 || intResult == 3 || intResult == 6 || intResult == 7)
                                {
                                    //6:�����޷�����(�պ�,�ػ�) ���������,����ǵ�һ�κ���,�����ٽ��дκ���,�Ȳ�ִ�оܾ�����.
                                    if (intResult == 6 && dr["Fcallnum"].ToString() == "1")
                                    {
                                        return 1;
                                    }
                                    strSql = "update  " + dbAppeal + "." + tbAppeal + "  set FPickUser='system',FPickTime=now(),Fstate=0 where Fid='" + strCheckID + "' and FUin='" + dr["Fuin"].ToString() + "'";
                                    daFen.ExecSql(strSql);
                                    return 1;
                                }
                                else//intResult=2ʱ
                                {
                                    string mesg = "";
                                    if (CFTUserAppealClass.CancelAppealDBTB(strCheckID, dbAppeal, tbAppeal, "ԭ���ֻ��ܾ�", "ԭ���ֻ��ܾ�", strMemo, "system", "127.0.0.2", out mesg))
                                    {
                                        //����������
                                        strSql = "update   " + dbName + "." + tbName + "  set Fcallnum=3, Fmodifytime=now() where Fappealid='" + strCheckID + "'";
                                        if (daIVRFen.ExecSqlNum(strSql) != 1)
                                        {
                                            loger.err("SendIVRResultNew", "�������������º��е�ʧ��  ���ݱ�" + dbName + "." + tbName + "   FappealID:" + strCheckID);
                                            return -1;
                                        }
                                        return 1;
                                    }
                                    else
                                    {
                                        loger.err("SendIVRResultNew", "�ܾ����ߵ�ʱʧ��  ���ݱ�" + dbName + "." + tbName + "   FappealID:" + strCheckID + "||" + mesg);
                                        return -1;
                                    }
                                }

                                return 1;
                            }
                        }
                        else //if(state == "0" || state == "8")
                        {
                            return 0;
                        }
                        #endregion
                    }
                    else//if(dr["FState"].ToString() == "1")
                    {
                        //״̬��Ԥ�ڷ������,�������ظ��ؽ������.
                        return 0;
                    }

                    #endregion

                }
            }
            catch (Exception err)
            {
                if ((dbName == "" && tbName == "") || (dbName == null || tbName == null))
                    loger.err("SendIVRResultNew", "ִ���쳣" + strCheckID + "||" + err.Message);
                else
                    loger.err("SendIVRResultNew", "ִ���쳣  ���ݱ�" + dbName + "." + tbName + "   FappealID:" + strCheckID + "||" + err.Message);
                return -1;
            }
			finally
			{
				da.Dispose();
                daIVRFen.Dispose();
                daFen.Dispose();
			}
		}
	}
}
