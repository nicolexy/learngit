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
	/// C2C交易平台（财务后台Service）数据访问类
	/// </summary>
	public class MySqlAccess : IDisposable
	{
		
		private MySqlConnection conn;

		private MySqlTransaction mymt; //加入事务。


		/// <summary>
		/// 数据库字符集。
		/// </summary>
		//public static string CharSet = ConfigurationSettings.AppSettings["CharSet"];


		public static string ReplaceSqlLimit(string instr)
		{
			//			if(str == null || str.Trim() == "") return "";
			//
			//			string tmp = str.Replace("'","‘");
			//			tmp = tmp.Replace("\"","“");
			//			tmp = tmp.Replace("--","——");
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
		/// 构造函数
		/// </summary>
		/// <param name="strConnString">数据库连接字符串</param>
		public MySqlAccess(string strConnString)
		{			
			conn = new MySqlConnection(strConnString);
			mymt = null;

		}

		/// <summary>
		/// 写入SQL日志
		/// </summary>
		/// <param name="connstring">数据连接字符串</param>
		/// <param name="database">当前库名</param>
		/// <param name="strsql">SQL语句</param>
		public static void WriteSqlLog(string connstring, string database, string strsql)
		{
			//string tmp = "连接字符串：" + connstring + "．数据表：" + database + "ＳＱＬ语句：" + strsql;
			string tmp = "数据表：" + database + "ＳＱＬ语句：" + strsql;
			log4net.ILog log = log4net.LogManager.GetLogger("DataAccess.SQL");
			if(log.IsInfoEnabled) log.Info(tmp);
		}

		/// <summary>
		/// 从SQL语句返回结果表。
		/// </summary>
		/// <param name="strCmd">SQL命令</param>
		/// <returns>结果表</returns>
		public DataTable GetTable(string strCmd)
		{
			DataTable result = new DataTable();
			

			MySqlDataAdapter da = new MySqlDataAdapter(strCmd,conn);
			if(mymt != null) da.SelectCommand.Transaction = mymt;
			try
			{
				//furion 20050905 加入SQL日志
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
		/// 执行语句，并返回是否影响到数据库
		/// </summary>
		/// <param name="strCmd">SQL命令</param>
		/// <returns>是否影响</returns>
		public bool ExecSql(string strCmd)   
		{
			try
			{

				MySqlCommand command = new MySqlCommand(strCmd,conn);

				if(mymt != null) command.Transaction = mymt;

				//furion 20050905 加入SQL日志
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
		/// 执行语句，并返回影响记录条数
		/// </summary>
		/// <param name="strCmd">SQL命令</param>
		/// <returns>影响记录条数</returns>
		public int ExecSqlNum(string strCmd)   
		{
			try
			{

				MySqlCommand command = new MySqlCommand(strCmd,conn);

				if(mymt != null) command.Transaction = mymt;

				//furion 20050905 加入SQL日志
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
		/// 获得SQL执行结果
		/// </summary>
		/// <param name="strCmd">SQL命令</param>
		/// <returns>SQL结果</returns>
		public string GetOneResult(string strCmd)  //获得单个结果
		{
			try
			{
				
				MySqlCommand command = new MySqlCommand(strCmd,conn);
				if(mymt != null) command.Transaction = mymt;

				//furion 20050905 加入SQL日志
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
		/// 获得SQL执行结果
		/// </summary>
		/// <param name="strCmd">SQL命令</param>
		/// <returns>SQL结果</returns>
		public object GetOneResult_OBJ(string strCmd)  //获得单个结果
		{
			try
			{
				
				MySqlCommand command = new MySqlCommand(strCmd,conn);
				if(mymt != null) command.Transaction = mymt;

				//furion 20050905 加入SQL日志
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
		/// 事务中执行一批SQL语句
		/// </summary>
		/// <param name="al">SQL语句组</param>
		/// <returns>事务是否成功</returns>
		public bool ExecSqls_Trans(ArrayList al)  //执行事务
		{
			MySqlTransaction mt;
			//furionzhang 2005-07-19 为了防止脏读出现，加入了事务级别。
			//mt = conn.BeginTransaction(); 
			mt = conn.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);

			MySqlCommand command = new MySqlCommand("11",conn,mt);  
			try
			{
				foreach(object obj in al)
				{
					string tmp = obj.ToString();
				
					command.CommandText = tmp;

					//furion 20050905 加入SQL日志
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
		/// 从SQL命令返回一个数据表(可以指定返回记录的范围) 
		/// </summary>
		/// <param name="strCmd">SQL命令</param>
		/// <param name="istart">记录开始位置</param>
		/// <param name="imax">最多返回条数</param>
		/// <returns>结果表</returns>
		public DataTable GetTableByRange(string strCmd, int istart, int imax)
		{
			return dsGetTableByRange(strCmd, istart, imax).Tables["table1"];
		}
		
		/// <summary>
		/// 从SQL命令返回一个数据集(可以指定返回记录的范围)
		/// </summary>
		/// <param name="strCmd">SQL命令</param>
		/// <param name="istart">记录开始位置</param>
		/// <param name="imax">最多返回条数</param>
		/// <returns>结果集</returns>
		public DataSet dsGetTableByRange(string strCmd, int istart, int imax) //imax 步长
		{
			DataSet ds = new DataSet();

			int start = istart - 1;
			if(start < 0) start=0;

			strCmd += " limit " + start + "," + imax + " ";

			MySqlDataAdapter da = new MySqlDataAdapter(strCmd,conn);
			if(mymt != null) da.SelectCommand.Transaction = mymt;
			try
			{
				
				//furion 20050905 加入SQL日志
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
		/// 获取所有记录
		/// </summary>
		/// <param name="strCmd">SQL</param>
		/// <returns>记录集</returns>
		public DataSet dsGetTotalData(string strCmd)  //一次返回所有的记录，用于需要一次性处理的地方 比如导成Excel文件
		{
			DataSet ds = new DataSet();


			MySqlDataAdapter da = new MySqlDataAdapter(strCmd,conn);
			if(mymt != null) da.SelectCommand.Transaction = mymt;
			try
			{
				//furion 20050905 加入SQL日志
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
		/// 用DataReader返回查询结果。传入一个数据库字段数组，输出一个结果数组(string 类型)
		/// </summary>
		/// <param name="strCmd">SQL</param>
		/// <param name="ar">输入字段数组</param>
		/// <returns>输出结果数组</returns>
		public string [] drData(string strCmd, string [] ar)
		{
			if(ar == null || ar.Length == 0) return null;

			MySqlDataReader dr = null;
			try
			{


				MySqlCommand myCmd = new MySqlCommand(strCmd,conn);

				if(mymt != null) myCmd.Transaction = mymt;


				//furion 20050905 加入SQL日志
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
		/// 返回一个集合
		/// </summary>
		/// <param name="strCmd">SQL</param>
		/// <param name="ar">参数数组</param>
		/// <returns>集合</returns>
		public ArrayList drReturn(string strCmd,string[] ar)  //ar存放需要的参数数组
		{
			if(ar == null || ar.Length == 0) return null;

			MySqlDataReader dr = null;
			try
			{

				MySqlCommand myCmd = new MySqlCommand(strCmd,conn);
				if(mymt != null) myCmd.Transaction = mymt;

				//furion 20050905 加入SQL日志
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
					//就每一行，给一个数组，用来存放每行中需要的元素
					ArrayList al = new ArrayList();
					foreach(string s in ar)  
					{						
						al.Add(dr[s]);  //选择需要的参数结果，添加到数组					
					}

					aral.Add(al);  //将整个结果添加到一个大的可变数组中
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

		#region 处理数据库的连接

		/// <summary>
		/// 初始化指定的连接
		/// </summary>
		/// <param name="aconn">连接对象</param>
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
				//不做任何处理，有些MYSQL不支持这个命令。
			}
		}

		/// <summary>
		/// 打开数据库连接
		/// </summary>
		/// <returns>打开是否成功</returns>
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
		/// 开始事务
		/// </summary>
		/// <returns>开始是否成功</returns>
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
		/// 提交事务
		/// </summary>
		/// <returns>事务是否成功</returns>
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
		/// 回滚事务
		/// </summary>
		/// <returns>是否成功</returns>
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
		/// 关闭连接
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

		#region IDisposable 成员

		/// <summary>
		/// 释放资源
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
			//furion 原来没有日志系统,用了别人的,现在用log4net吧,不再用其它的,不然减慢速度.
			//furion 20060523

			string message = err.Message;
			//
			//			string LogServer = ConfigurationSettings.AppSettings["LogServer"];
			//			int ServerPort = Int32.Parse(ConfigurationSettings.AppSettings["LogPort"]);
			//			LogManage lm = new LogManage(LogServer, ServerPort);
			//			lm.Log("dataaccess","finance","warning","0001",conn.ConnectionString + ": " + message);
			//			lm.DBLog("dataaccess","finance","warning","0001",conn.ConnectionString + ": " + message);

			//furion 20050905 加入SQL日志 //换成log4net
			//WriteSqlLog(conn.ConnectionString,conn.Database,sql,ExecUID,"ERROR：" + err.Message);
			log4net.ILog log = log4net.LogManager.GetLogger("DataAccess.Error");
			if(log.IsErrorEnabled) log.Error(sql,err);

			return new DataAccessException(message,conn.ConnectionString,sql,err);
		}
		
	}
}
