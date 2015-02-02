using System;

using System.Data;
using System.Data.Odbc;

using System.Collections;
using TENCENT.OSS.CFT.KF.Common;
using System.Configuration;
using System.Text;
using MySql.Data.MySqlClient;

namespace TENCENT.OSS.CFT.KF.DataAccess
{
	/// <summary>
	/// C2C����ƽ̨�������̨Service�����ݷ�����
	/// </summary>
	public class MySqlAccess : IDisposable
	{
		
		private MySqlConnection conn;

		private MySqlTransaction mymt; //��������


		/// <summary>
		/// ���ݿ��ַ�����
		/// </summary>
		//public static string CharSet = ConfigurationSettings.AppSettings["CharSet"];


		public static string ReplaceSqlLimit(string instr)
		{
			//			if(str == null || str.Trim() == "") return "";
			//
			//			string tmp = str.Replace("'","��");
			//			tmp = tmp.Replace("\"","��");
			//			tmp = tmp.Replace("--","����");
			//			return tmp;
			if(instr == null || instr == "")
				return "";

			byte[] outbuff = Encoding.GetEncoding("gb2312").GetBytes(instr);

			for(int i=0 ; i< outbuff.Length ; i++)
			{
				if(outbuff[i] == 39)  //'
					outbuff[i] = 34;  //"
				else if(outbuff[i] == 92) //\
					outbuff[i] = 47; // /
			}

			return Encoding.GetEncoding("gb2312").GetString(outbuff);
		}

		/// <summary>
		/// ���캯��
		/// </summary>
		/// <param name="strConnString">���ݿ������ַ���</param>
		public MySqlAccess(string strConnString)
		{			
			conn = new MySqlConnection(strConnString);
			mymt = null;

		}

		/// <summary>
		/// д��SQL��־
		/// </summary>
		/// <param name="connstring">���������ַ���</param>
		/// <param name="database">��ǰ����</param>
		/// <param name="strsql">SQL���</param>
		public static void WriteSqlLog(string connstring, string database, string strsql)
		{
			//string tmp = "�����ַ�����" + connstring + "�����ݱ�" + database + "�ӣѣ���䣺" + strsql;
			string tmp = "���ݱ�" + database + "�ӣѣ���䣺" + strsql;
			log4net.ILog log = log4net.LogManager.GetLogger("DataAccess.SQL");
			if(log.IsInfoEnabled) log.Info(tmp);
		}

		/// <summary>
		/// ��SQL��䷵�ؽ����
		/// </summary>
		/// <param name="strCmd">SQL����</param>
		/// <returns>�����</returns>
		public DataTable GetTable(string strCmd)
		{
			DataTable result = new DataTable();
			

			MySqlDataAdapter da = new MySqlDataAdapter(strCmd,conn);
			if(mymt != null) da.SelectCommand.Transaction = mymt;
			try
			{
				//furion 20050905 ����SQL��־
				WriteSqlLog(conn.ConnectionString,conn.Database,strCmd);
#if SqlDebug
				long tick = DateTime.Now.Ticks;
#endif

				da.Fill(result);
#if SqlDebug
				SqlDebug.WriteSql(conn.ConnectionString, 
					strCmd,DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),DateTime.Now.Ticks - tick);
#endif

				

				return result;
			}
			catch(Exception err)
			{
				throw TransException(err,strCmd);
			}
			finally
			{
				da.Dispose();
			}
		}

		/// <summary>
		/// ִ����䣬�������Ƿ�Ӱ�쵽���ݿ�
		/// </summary>
		/// <param name="strCmd">SQL����</param>
		/// <returns>�Ƿ�Ӱ��</returns>
		public bool ExecSql(string strCmd)   
		{
			try
			{

				MySqlCommand command = new MySqlCommand(strCmd,conn);

				if(mymt != null) command.Transaction = mymt;

				//furion 20050905 ����SQL��־
				WriteSqlLog(conn.ConnectionString,conn.Database,strCmd);
#if SqlDebug
				long tick = DateTime.Now.Ticks;
#endif

				int i = command.ExecuteNonQuery();
#if SqlDebug
				SqlDebug.WriteSql(conn.ConnectionString, strCmd,DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),DateTime.Now.Ticks - tick);
#endif
				return i>0;
			}
			catch(Exception err)
			{
				throw TransException(err,strCmd);
			}
		}

		/// <summary>
		/// ִ����䣬������Ӱ���¼����
		/// </summary>
		/// <param name="strCmd">SQL����</param>
		/// <returns>Ӱ���¼����</returns>
		public int ExecSqlNum(string strCmd)   
		{
			try
			{

				MySqlCommand command = new MySqlCommand(strCmd,conn);

				if(mymt != null) command.Transaction = mymt;

				//furion 20050905 ����SQL��־
				WriteSqlLog(conn.ConnectionString,conn.Database,strCmd);

#if SqlDebug
				long tick = DateTime.Now.Ticks;
#endif

				int i = command.ExecuteNonQuery();
#if SqlDebug
				SqlDebug.WriteSql(conn.ConnectionString, strCmd,DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),DateTime.Now.Ticks - tick);
#endif

				return i;
			}
			catch(Exception err)
			{
				throw TransException(err,strCmd);
			}
		}

		/// <summary>
		/// ���SQLִ�н��
		/// </summary>
		/// <param name="strCmd">SQL����</param>
		/// <returns>SQL���</returns>
		public string GetOneResult(string strCmd)  //��õ������
		{
			try
			{
				
				MySqlCommand command = new MySqlCommand(strCmd,conn);
				if(mymt != null) command.Transaction = mymt;

				//furion 20050905 ����SQL��־
				WriteSqlLog(conn.ConnectionString,conn.Database,strCmd);

				
#if SqlDebug
				long tick = DateTime.Now.Ticks;
#endif

				object obj = command.ExecuteScalar();
#if SqlDebug
				SqlDebug.WriteSql(conn.ConnectionString, strCmd,DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),DateTime.Now.Ticks - tick);
#endif

				if(obj != null)
					return obj.ToString();
				else
					return null;
			}
			catch(Exception err)
			{
				throw TransException(err,strCmd);
			}
		}

		/// <summary>
		/// ���SQLִ�н��
		/// </summary>
		/// <param name="strCmd">SQL����</param>
		/// <returns>SQL���</returns>
		public object GetOneResult_OBJ(string strCmd)  //��õ������
		{
			try
			{
				
				MySqlCommand command = new MySqlCommand(strCmd,conn);
				if(mymt != null) command.Transaction = mymt;

				//furion 20050905 ����SQL��־
				WriteSqlLog(conn.ConnectionString,conn.Database,strCmd);

#if SqlDebug
				long tick = DateTime.Now.Ticks;
#endif

				object obj = command.ExecuteScalar();
#if SqlDebug
				SqlDebug.WriteSql(conn.ConnectionString, strCmd,DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),DateTime.Now.Ticks - tick);
#endif

				return obj;
			}
			catch(Exception err)
			{
				throw TransException(err,strCmd);
			}
		}

		/// <summary>
		/// ������ִ��һ��SQL���
		/// </summary>
		/// <param name="al">SQL�����</param>
		/// <returns>�����Ƿ�ɹ�</returns>
		public bool ExecSqls_Trans(ArrayList al)  //ִ������
		{
			MySqlTransaction mt;
			//furionzhang 2005-07-19 Ϊ�˷�ֹ������֣����������񼶱�
			//mt = conn.BeginTransaction(); 
			mt = conn.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);

			MySqlCommand command = new MySqlCommand("11",conn,mt);  
			try
			{
				foreach(object obj in al)
				{
					string tmp = obj.ToString();
				
					command.CommandText = tmp;

					//furion 20050905 ����SQL��־
					WriteSqlLog(conn.ConnectionString,conn.Database,tmp);

					
#if SqlDebug
					long tick = DateTime.Now.Ticks;
#endif

					command.ExecuteNonQuery();
#if SqlDebug
					SqlDebug.WriteSql(conn.ConnectionString, tmp,DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),DateTime.Now.Ticks - tick);
#endif
				}
				mt.Commit();
				return true;
			}
			catch(Exception err)
			{
				mt.Rollback();
				throw TransException(err,"");
			}
		}

		/// <summary>
		/// ��SQL�����һ�����ݱ�(����ָ�����ؼ�¼�ķ�Χ) 
		/// </summary>
		/// <param name="strCmd">SQL����</param>
		/// <param name="istart">��¼��ʼλ��</param>
		/// <param name="imax">��෵������</param>
		/// <returns>�����</returns>
		public DataTable GetTableByRange(string strCmd, int istart, int imax)
		{
			return dsGetTableByRange(strCmd, istart, imax).Tables["table1"];
		}
		
		/// <summary>
		/// ��SQL�����һ�����ݼ�(����ָ�����ؼ�¼�ķ�Χ)
		/// </summary>
		/// <param name="strCmd">SQL����</param>
		/// <param name="istart">��¼��ʼλ��</param>
		/// <param name="imax">��෵������</param>
		/// <returns>�����</returns>
		public DataSet dsGetTableByRange(string strCmd, int istart, int imax) //imax ����
		{
			DataSet ds = new DataSet();

			int start = istart - 1;
			if(start < 0) start=0;

			strCmd += " limit " + start + "," + imax + " ";

			MySqlDataAdapter da = new MySqlDataAdapter(strCmd,conn);
			if(mymt != null) da.SelectCommand.Transaction = mymt;
			try
			{
				
				//furion 20050905 ����SQL��־
				WriteSqlLog(conn.ConnectionString,conn.Database,strCmd);

#if SqlDebug
				long tick = DateTime.Now.Ticks;
#endif
				
				da.Fill(ds,"table1");
#if SqlDebug
				SqlDebug.WriteSql(conn.ConnectionString, strCmd,DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),DateTime.Now.Ticks - tick);
#endif

				

				return ds;
			}
			catch(Exception err)
			{
				throw TransException(err,strCmd);
			}
			finally
			{
				da.Dispose();
			}	
		}

		/// <summary>
		/// ��ȡ���м�¼
		/// </summary>
		/// <param name="strCmd">SQL</param>
		/// <returns>��¼��</returns>
		public DataSet dsGetTotalData(string strCmd)  //һ�η������еļ�¼��������Ҫһ���Դ���ĵط� ���絼��Excel�ļ�
		{
			DataSet ds = new DataSet();


			MySqlDataAdapter da = new MySqlDataAdapter(strCmd,conn);
			if(mymt != null) da.SelectCommand.Transaction = mymt;
			try
			{
				//furion 20050905 ����SQL��־
				WriteSqlLog(conn.ConnectionString,conn.Database,strCmd);

#if SqlDebug
				long tick = DateTime.Now.Ticks;
#endif

				da.Fill(ds);
#if SqlDebug
				SqlDebug.WriteSql(conn.ConnectionString, strCmd,DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),DateTime.Now.Ticks - tick);
#endif

				
				
				return ds;
			}
			catch(Exception err)
			{
				throw TransException(err,strCmd);
			}
			finally
			{
				da.Dispose();
			}
		}

		/// <summary>
		/// ��DataReader���ز�ѯ���������һ�����ݿ��ֶ����飬���һ���������(string ����)
		/// </summary>
		/// <param name="strCmd">SQL</param>
		/// <param name="ar">�����ֶ�����</param>
		/// <returns>����������</returns>
		public string [] drData(string strCmd, string [] ar)
		{
			if(ar == null || ar.Length == 0) return null;

			MySqlDataReader dr = null;
			try
			{


				MySqlCommand myCmd = new MySqlCommand(strCmd,conn);

				if(mymt != null) myCmd.Transaction = mymt;


				//furion 20050905 ����SQL��־
				WriteSqlLog(conn.ConnectionString,conn.Database,strCmd);

#if SqlDebug
				long tick = DateTime.Now.Ticks;
#endif

				dr = myCmd.ExecuteReader();    //CommandBehavior.CloseConnection
#if SqlDebug
				SqlDebug.WriteSql(conn.ConnectionString, strCmd,DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),DateTime.Now.Ticks - tick);
#endif

				

			
				string [] myar = new string[ar.Length]; 

				int i=0;
				while (dr.Read() && i< ar.Length - 1)
				{
					foreach(string s in ar )
					{
						if(dr[s] != null)
						{
							//myar[i] = dr[s].ToString().Replace("\\","").Replace("\"","\\\"").Replace("'","\\'");		
							myar[i] = ReplaceSqlLimit(dr[s].ToString());
						}
						else
						{
							myar[i] = "";
						}

						i++;
					}
				}
				return myar;
			}
			catch(Exception err)
			{
				throw TransException(err,strCmd);
			}
			finally
			{
				if(dr != null && !dr.IsClosed)
				{
					dr.Close();
				}
			}
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		/// <param name="strCmd">SQL</param>
		/// <param name="ar">��������</param>
		/// <returns>����</returns>
		public ArrayList drReturn(string strCmd,string[] ar)  //ar�����Ҫ�Ĳ�������
		{
			if(ar == null || ar.Length == 0) return null;

			MySqlDataReader dr = null;
			try
			{

				MySqlCommand myCmd = new MySqlCommand(strCmd,conn);
				if(mymt != null) myCmd.Transaction = mymt;

				//furion 20050905 ����SQL��־
				WriteSqlLog(conn.ConnectionString,conn.Database,strCmd);

#if SqlDebug
				long tick = DateTime.Now.Ticks;
#endif

				dr = myCmd.ExecuteReader(CommandBehavior.CloseConnection);
#if SqlDebug
				SqlDebug.WriteSql(conn.ConnectionString, strCmd,DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),DateTime.Now.Ticks - tick);
#endif

				

				ArrayList aral = new ArrayList();
				while (dr.Read())
				{
					//��ÿһ�У���һ�����飬�������ÿ������Ҫ��Ԫ��
					ArrayList al = new ArrayList();
					foreach(string s in ar)  
					{						
						al.Add(dr[s]);  //ѡ����Ҫ�Ĳ����������ӵ�����					
					}

					aral.Add(al);  //�����������ӵ�һ����Ŀɱ�������
				}

				return aral;
			}
			catch(Exception err)
			{
				throw TransException(err,strCmd);
			}
			finally
			{
				if(dr != null && !dr.IsClosed)
				{
					dr.Close();
				}
			}
		}

		#region �������ݿ������

		/// <summary>
		/// ��ʼ��ָ��������
		/// </summary>
		/// <param name="aconn">���Ӷ���</param>
		public static void InitConn(MySqlConnection aconn)
		{
			try
			{
				MySqlCommand tmpcommand = new MySqlCommand("",aconn);

				//				string strSql = " SET AUTOCOMMIT=0 ";
				//				tmpcommand.CommandText = strSql;
				//				tmpcommand.ExecuteNonQuery();

				//				string strSql = " SET character_set_client='" + CharSet +  "' ";
				//				tmpcommand.CommandText = strSql;
				//				tmpcommand.ExecuteNonQuery();
				//
				//				strSql = " SET character_set_connection='" + CharSet +  "'; ";
				//				tmpcommand.CommandText = strSql;
				//				tmpcommand.ExecuteNonQuery();
				//
				//				strSql = " SET character_set_database='" + CharSet +  "'; ";
				//				tmpcommand.CommandText = strSql;
				//				tmpcommand.ExecuteNonQuery();
				//
				//				strSql = " SET character_set_results='" + CharSet +  "'; ";
				//				tmpcommand.CommandText = strSql;
				//				tmpcommand.ExecuteNonQuery();
				//
				//				strSql = " SET character_set_server='" + CharSet +  "';";
				//				tmpcommand.CommandText = strSql;
				//				tmpcommand.ExecuteNonQuery();
				//
				//				strSql = " SET collation_connection='" + CharSet +  "_bin'; ";
				//				tmpcommand.CommandText = strSql;
				//				tmpcommand.ExecuteNonQuery();
				//
				//				strSql = " SET collation_database='" + CharSet +  "_bin'; ";
				//				tmpcommand.CommandText = strSql;
				//				tmpcommand.ExecuteNonQuery();
				//
				//				strSql = " SET collation_server='" + CharSet +  "_bin';";
				//				tmpcommand.CommandText = strSql;
				//				tmpcommand.ExecuteNonQuery();		
								

				tmpcommand.Dispose();
			}
			catch
			{
				//�����κδ�����ЩMYSQL��֧��������
			}
		}

		/// <summary>
		/// �����ݿ�����
		/// </summary>
		/// <returns>���Ƿ�ɹ�</returns>
		public bool OpenConn()
		{
			try
			{
				if(conn.State != ConnectionState.Open)
				{
					conn.Open();					
					//InitConn(conn);
				}
				return true;
			}
			catch(Exception err)
			{
				throw TransException(err,"");
			}
		}

		/// <summary>
		/// ��ʼ����
		/// </summary>
		/// <returns>��ʼ�Ƿ�ɹ�</returns>
		public bool StartTran()
		{
			try
			{
				if(conn.State != ConnectionState.Open)
				{
					conn.Open();
				}

				mymt = conn.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);//(System.Data.IsolationLevel.ReadCommitted);

				return true;
			}
			catch(Exception err)
			{
				throw TransException(err,"");
			}
		}

		/// <summary>
		/// �ύ����
		/// </summary>
		/// <returns>�����Ƿ�ɹ�</returns>
		public bool Commit()
		{
			try
			{
				if(mymt != null)
				{
					mymt.Commit();
					mymt = null;
				}
				return true;
			}
			catch
			{
				//return false;
				throw;
			}
		}

		/// <summary>
		/// �ع�����
		/// </summary>
		/// <returns>�Ƿ�ɹ�</returns>
		public bool RollBack()
		{
			try
			{
				if(mymt != null)
				{
					mymt.Rollback();
					mymt = null;
				}
				return true;
			}
			catch
			{
				//return false;
				throw;
			}
		}

		/// <summary>
		/// �ر�����
		/// </summary>
		public void CloseConn()
		{
			if(conn.State != ConnectionState.Closed )
			{
				RollBack();
				conn.Close();
			}
		}

		#endregion

		#region IDisposable ��Ա

		/// <summary>
		/// �ͷ���Դ
		/// </summary>
		public void Dispose()
		{
			CloseConn();

			conn.Dispose();
			conn = null;
		}

		#endregion

		private DataAccessException TransException(Exception err, string sql)
		{
			//furion ԭ��û����־ϵͳ,���˱��˵�,������log4net��,������������,��Ȼ�����ٶ�.
			//furion 20060523

			string message = err.Message;
			//
			//			string LogServer = ConfigurationSettings.AppSettings["LogServer"];
			//			int ServerPort = Int32.Parse(ConfigurationSettings.AppSettings["LogPort"]);
			//			LogManage lm = new LogManage(LogServer, ServerPort);
			//			lm.Log("dataaccess","finance","warning","0001",conn.ConnectionString + ": " + message);
			//			lm.DBLog("dataaccess","finance","warning","0001",conn.ConnectionString + ": " + message);

			//furion 20050905 ����SQL��־ //����log4net
			//WriteSqlLog(conn.ConnectionString,conn.Database,sql,ExecUID,"ERROR��" + err.Message);
			log4net.ILog log = log4net.LogManager.GetLogger("DataAccess.Error");
			if(log.IsErrorEnabled) log.Error(sql,err);

			return new DataAccessException(message,conn.ConnectionString,sql,err);
		}
		
	}
}
