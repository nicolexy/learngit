using System;



using System.Data;

using System.Data.Odbc;



using System.Collections;

using TENCENT.OSS.CFT.KF.Common;

using System.Configuration;

using System.Text;



namespace TENCENT.OSS.CFT.KF.DataAccess

{

	public class MySqlAccess : IDisposable

	{

		private OdbcConnection conn;



		private OdbcTransaction mymt; //��������



		private string ExecUID = ""; //ִ�����Ĳ���UID��



		private static bool canwrite = false;



		public static string CharSet = ConfigurationManager.AppSettings["CharSet"];

		//furion 20050905 ����дSQL��־�ĺ�����

		//����DataAccess��Ҫ������������á�

		//string LogServer = ConfigurationManager.AppSettings["LogServer"];

		//int ServerPort = Int32.Parse(ConfigurationManager.AppSettings["LogPort"]);

		//string f_strDataSource_ht = ConfigurationManager.AppSettings["DataSource_ht"];

		//string f_strUserID_ht = ConfigurationManager.AppSettings["UserID_ht"];

		//string f_strPassword_ht = ConfigurationManager.AppSettings["Password_ht"];

		public static void WriteSqlLog(string connstring, string defaultdb, string sql, string UID)

		{

			WriteSqlLog(connstring,defaultdb,sql,UID,"");

		}



		public static void WriteSqlLog(string connstring, string defaultdb, string sql, string UID, string flag)

		{



			string tmp="";

			if(defaultdb==""&&sql=="")

			{

				tmp=connstring;

			}

			else

			{

				tmp = "���ݱ�" + defaultdb + "�ӣѣ���䣺" + sql;

			}

			log4net.ILog log = log4net.LogManager.GetLogger("DataAccess.SQL");

			if(log.IsInfoEnabled) log.Info(tmp);

			// 2012/6/21 �����־�ʼ�����
			SendLogger sendLogger = SendLogger.GetInstance();

			if(sendLogger != null)
			{
				if(sendLogger.IsCatchLog)
				{
					sendLogger.AddLog(tmp);
				}
			}
		}



		public static string ReplaceSqlLimit(string instr)
		{

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



		public MySqlAccess(string strConnString, string UID)

		{			

			conn = new OdbcConnection(strConnString);

			ExecUID = UID;

			mymt = null;



			canwrite = ConfigurationManager.AppSettings["WriteSqlLog"].Trim().ToUpper() == "TRUE";

		}

		//furion end



		public MySqlAccess(string strConnString)

			:this(strConnString,"")

		{			

			

		}

        /// <summary>
        /// ��������ѯ����ֹע��
        /// </summary>
        /// <param name="strCmd"></param>
        /// <returns></returns>
        public DataTable GetTable_Parameters(string strCmd, System.Collections.Generic.List<string> Parameter)
        {
            OdbcDataAdapter adapter = null;
            try
            {
                OdbcCommand command = new OdbcCommand(strCmd, conn);
                int i = 1;
                foreach (string item in Parameter)
                {
                    command.Parameters.AddWithValue(i.ToString(), item);
                    i++;
                }
                adapter = new OdbcDataAdapter(command);
                if (mymt != null) adapter.SelectCommand.Transaction = mymt;

                DataTable data = new DataTable();
                adapter.Fill(data);

                WriteSqlLog(conn.ConnectionString, conn.Database, strCmd, ExecUID);

                return data;
            }
            catch (Exception err)
            {
                throw TransException(err, strCmd);
            }
            finally
            {
                adapter.Dispose();
            }
        }

        public bool ExecSql_Parameters(string strCmd, System.Collections.Generic.List<string> Parameter)
        {
            try
            {
                OdbcCommand command = new OdbcCommand(strCmd, conn);
                int i = 1;
                foreach (string item in Parameter)
                {
                    command.Parameters.AddWithValue(i.ToString(), item);
                    i++;
                }

                if (mymt != null) command.Transaction = mymt;
                //furion 20050905 ����SQL��־
                WriteSqlLog(conn.ConnectionString, conn.Database, strCmd, ExecUID);
                return command.ExecuteNonQuery() > 0;
            }
            catch (Exception err)
            {
                throw TransException(err, strCmd);
            }
        }


        public string GetOneResult_Parameters(string strCmd, System.Collections.Generic.List<string> Parameter)  //��õ������
        {
            try
            {
                OdbcCommand command = new OdbcCommand(strCmd, conn);
                int i = 1;
                foreach (string item in Parameter)
                {
                    command.Parameters.AddWithValue(i.ToString(), item);
                    i++;
                }
                if (mymt != null) command.Transaction = mymt;
                //furion 20050905 ����SQL��־
                WriteSqlLog(conn.ConnectionString, conn.Database, strCmd, ExecUID);
                object obj = command.ExecuteScalar();
                if (obj != null)
                    return obj.ToString();
                else
                    return null;
            }
            catch (Exception err)
            {
                throw TransException(err, strCmd);
            }
        }

        /// <summary>

        /// ��SQL��䷵�ؽ����

        /// </summary>

        /// <param name="strCmd"></param>

        /// <returns></returns>

        public DataTable GetTable(string strCmd)

		{

			DataTable result = new DataTable();

			



			OdbcDataAdapter da = new OdbcDataAdapter(strCmd,conn);

			if(mymt != null) da.SelectCommand.Transaction = mymt;

			try

			{

				da.Fill(result);



				//furion 20050905 ����SQL��־

				WriteSqlLog(conn.ConnectionString,conn.Database,strCmd,ExecUID);



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



		public bool ExecSql(string strCmd)   

		{

			try

			{



				OdbcCommand command = new OdbcCommand(strCmd,conn);



				if(mymt != null) command.Transaction = mymt;



				//furion 20050905 ����SQL��־

				WriteSqlLog(conn.ConnectionString,conn.Database,strCmd,ExecUID);



				return command.ExecuteNonQuery()>0;

			}

			catch(Exception err)

			{

				throw TransException(err,strCmd);

			}

		}





		public int ExecSqlNum(string strCmd)   

		{

			try

			{



				OdbcCommand command = new OdbcCommand(strCmd,conn);



				if(mymt != null) command.Transaction = mymt;



				//furion 20050905 ����SQL��־

				WriteSqlLog(conn.ConnectionString,conn.Database,strCmd,ExecUID);



				return command.ExecuteNonQuery();

			}

			catch(Exception err)

			{

				throw TransException(err,strCmd);

			}

		}



		public string GetOneResult(string strCmd)  //��õ������

		{

			try

			{

				

				OdbcCommand command = new OdbcCommand(strCmd,conn);

				if(mymt != null) command.Transaction = mymt;



				//furion 20050905 ����SQL��־

				WriteSqlLog(conn.ConnectionString,conn.Database,strCmd,ExecUID);



				object obj = command.ExecuteScalar();





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



		public bool ExecSqls_Trans(ArrayList al)  //ִ������

		{

			OdbcTransaction mt;

			//furionzhang 2005-07-19 Ϊ�˷�ֹ������֣����������񼶱�

			//mt = conn.BeginTransaction(); 

			mt = conn.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);



			OdbcCommand command = new OdbcCommand("11",conn,mt);  

			try

			{

				foreach(object obj in al)

				{

					string tmp = obj.ToString();

				

					command.CommandText = tmp;



					//furion 20050905 ����SQL��־

					WriteSqlLog(conn.ConnectionString,conn.Database,tmp,ExecUID);



					command.ExecuteNonQuery();

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

		/// <param name="strCmd"></param>

		/// <param name="istart"></param>

		/// <param name="imax"></param>

		/// <returns></returns>

		public DataTable GetTableByRange(string strCmd, int istart, int imax)

		{

			return dsGetTableByRange(strCmd, istart, imax).Tables["table1"];

		}

		

		public DataSet dsGetTableByRange(string strCmd, int istart, int imax) //imax ����

		{

			DataSet ds = new DataSet();





			int start = istart - 1;

			if(start < 0) start=0;



			strCmd += " limit " + start + "," + imax + " ";



			OdbcDataAdapter da = new OdbcDataAdapter(strCmd,conn);

			if(mymt != null) da.SelectCommand.Transaction = mymt;

			try

			{

				//furion 20050905 ����SQL��־

				WriteSqlLog(conn.ConnectionString,conn.Database,strCmd,ExecUID);



				//da.Fill(ds,istart-1,imax,"table1");

				

				da.Fill(ds,"table1");



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



		public DataSet dsGetTotalData(string strCmd)  //һ�η������еļ�¼��������Ҫһ���Դ���ĵط� ���絼��Excel�ļ�

		{

			DataSet ds = new DataSet();



	

			OdbcDataAdapter da = new OdbcDataAdapter(strCmd,conn);

			if(mymt != null) da.SelectCommand.Transaction = mymt;

			try

			{

				//furion 20050905 ����SQL��־

				WriteSqlLog(conn.ConnectionString,conn.Database,strCmd,ExecUID);



				da.Fill(ds,"table1");

				

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


		public string [] drData(string strCmd, string [] ar)

		{

			if(ar == null || ar.Length == 0) return null;



			OdbcDataReader dr = null;

			try

			{





				OdbcCommand myCmd = new OdbcCommand(strCmd,conn);



				if(mymt != null) myCmd.Transaction = mymt;





				//furion 20050905 ����SQL��־

				WriteSqlLog(conn.ConnectionString,conn.Database,strCmd,ExecUID);





				dr = myCmd.ExecuteReader();    //CommandBehavior.CloseConnection



			

				string [] myar = new string[ar.Length]; 



				int i=0;

				while (dr.Read()  && i< ar.Length)

				{

					foreach(string s in ar)

					{

						myar[i] = dr[s].ToString();						



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

		/// ����һ��DataReader��������

		/// </summary>

		/// <returns></returns>

		public ArrayList drReturn(string strCmd,string[] ar)  //ar�����Ҫ�Ĳ�������

		{

			if(ar == null || ar.Length == 0) return null;



			OdbcDataReader dr = null;

			try

			{



				OdbcCommand myCmd = new OdbcCommand(strCmd,conn);

				if(mymt != null) myCmd.Transaction = mymt;



				//furion 20050905 ����SQL��־

				WriteSqlLog(conn.ConnectionString,conn.Database,strCmd,ExecUID);



				dr = myCmd.ExecuteReader(CommandBehavior.CloseConnection);



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



		public static void InitConn(OdbcConnection aconn)

		{

			try

			{

				OdbcCommand tmpcommand = new OdbcCommand("SET character_set_results='" + CharSet +  "'",aconn);

				tmpcommand.ExecuteNonQuery();



				tmpcommand.CommandText = "SET character_set_connection='" + CharSet +  "'";

				tmpcommand.ExecuteNonQuery();



				tmpcommand.CommandText = "SET collation_connection='" + CharSet +  "'";

				tmpcommand.ExecuteNonQuery();



				tmpcommand.CommandText = "SET character_set_client='" + CharSet +  "'";

				tmpcommand.ExecuteNonQuery();

				



				tmpcommand.Dispose();

			}

			catch

			{

				//�����κδ�����ЩMYSQL��֧��������

			}

		}



		public bool OpenConn()

		{

			try

			{

				if(conn.State != ConnectionState.Open)

				{

					conn.Open();					

					InitConn(conn);

				}

				return true;

			}

			catch(Exception err)

			{

				throw TransException(err,"");

			}

		}



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

				return false;

			}

		}



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

				return false;

			}

		}



		public void CloseConn()

		{

			RollBack();



			if(conn.State != ConnectionState.Closed )

			{

				conn.Close();

			}

		}



		#endregion

		#region IDisposable ��Ա
		public void Dispose()
		{
			CloseConn();
			conn.Dispose();
			conn = null;
		}
		#endregion

		private DataAccessException TransException(Exception err, string sql)
		{
			string message = err.Message;

			string LogServer = ConfigurationManager.AppSettings["LogServer"];

			int ServerPort = Int32.Parse(ConfigurationManager.AppSettings["LogPort"]);

			LogManage lm = new LogManage(LogServer, ServerPort);

			lm.Log("dataaccess","finance","warning","0001",conn.ConnectionString + ": " + message);

			lm.DBLog("dataaccess","finance","warning","0001",conn.ConnectionString + ": " + message);

			//furion 20050905 ����SQL��־

			WriteSqlLog(conn.ConnectionString,conn.Database,sql,ExecUID,"ERROR��" + err.Message);

			return new DataAccessException(message,conn.ConnectionString,sql,err);

		}
	}
}

