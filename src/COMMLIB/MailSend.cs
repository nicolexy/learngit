using System; 
using System.Net.Sockets;//用于处理网络连接 
using System.IO; //用于处理附件的包 
using System.Text;//用于处理文本编码 
using System.Data; 
using System.Net; 

namespace TENCENT.OSS.C2C.Finance.Common.CommLib
{
	/// <summary>
	/// MailSend 的摘要说明。
	/// </summary>
	public class MailSend : TcpClient
	{
		private string server;//SMTP服务器域名
		public string Server
		{
			set{this.server = value;}
		}

		private int port;//端口 
		public int Port
		{
			set{this.port = value;}
		}

		private string username;//用户名 
		public string UserName
		{
			set{this.username = value;}
		}

		private string password;//密码 
		public string Password
		{
			set{this.password = value;}
		}

		private string subject;//主题 
		public string Subject
		{
			set{this.subject = value;}
		}

		private string body;//文本内容
		public string Body
		{
			set{this.body = value;}
		}

		private string htmlbody;//超文本内容 
		public string HtmlBody
		{
			get{return htmlbody;}
			set{this.htmlbody = value;}
		}

		private string from;//发件人地址 
		public string From
		{
			set{this.from = value;}
		}

		private string to;//收件人地址 
		public string To
		{
			set{this.to = value;}
		}

		private string cc;//抄送人地址 
		public string CC
		{
			set{this.cc = value;}
		}

		private string fromname;//发件人姓名 
		public string FromName
		{
			set{this.fromname = value;}
		}

		private string toname;//收件人姓名 
		public string ToName
		{
			set{this.toname = value;}
		}

		private string content_type;//邮件类型
		public string Content_Type
		{
			set{this.content_type = value;}
		}

		private string encode;//邮件编码 
		public string Encode
		{
			set{this.encode = value;}
		}

		private string charset;//语言编码 
		public string Charset
		{
			set{this.charset = value;}
		}

		public DataTable filelist;//附件列表　

		private int priority;//邮件优先级
		public int Priority
		{
			set{this.priority = value;}
		}

		public MailSend()
		{
			filelist=new DataTable();//已定义变量，初始化操作

			filelist.Columns.Add(new DataColumn("filename",typeof(string)));//文件名 

			filelist.Columns.Add(new DataColumn("filecontent",typeof(string)));//文件内容 
		}

		private void WriteStream(string strCmd)
		{
			WriteStream(strCmd,"gb2312");
		}

		private void WriteStream(string strCmd,string charset) 
		{ 
			try
			{
				Stream TcpStream;//定义操作对象 
				strCmd = strCmd + "\r\n"; //加入换行符 
				TcpStream =this.GetStream();//获取数据流
				//将命令行转化为byte[] 
				byte[] bWrite = Encoding.GetEncoding(charset).GetBytes(strCmd.ToCharArray()); 
				//由于每次写入的数据大小是有限制的，那么我们将每次写入的数据长度定在７５个字节，一旦命令长度超过了７５，就分步写入。
				int start=0; 
				int length=bWrite.Length; 
				int page=0; 
				int size=75; 
				int count=size; 
				if (length>75) 
				{ 
					//数据分页 
					if ((length/size)*size < length) 
						page=length/size+1; 
					else 
						page=length/size; 
					for (int i=0;i < page; i++) 
					{ 
						start=i*size; 
						if (i==page-1) 
							count=length-(i*size); 
						TcpStream.Write(bWrite,start,count);//将数据写入到服务器上 
					} 
				}
				else 
					TcpStream.Write(bWrite,0,bWrite.Length);
			
			}
			catch(Exception err) 
			{
				string tmp = err.Message;
			} 
		}
 

		private string ReceiveStream() 
		{ 
			String sp=null; 

			byte[] by=new byte[1024]; 
			NetworkStream ns = this.GetStream();//此处即可获取服务器的返回数据流 
			int size=ns.Read(by,0,by.Length);//读取数据流 
			if (size>0) 
			{ 
				sp=Encoding.Default.GetString(by);//转化为String 
			} 
			return sp; 
		} 


		private bool OperaStream(string strCmd,string state) 
		{
			string sp=null; 
			bool success=false; 
			try 
			{ 
				WriteStream(strCmd);//写入命令 
				sp = ReceiveStream();//接受返回信息 
				if (sp.IndexOf(state)!=-1)//判断状态码是否正确 
					success=true; 
			} 
			catch(Exception ex) 
			{
				string msg = ex.Message;
			}
			return success; 
		}


		public bool getMailServer() 
		{ 
			try 
			{ 
				//域名解析 
				//System.Net.IPAddress ipaddress=(IPAddress)System.Net.Dns.Resolve(this.server).AddressList.GetValue(0);  
 
				System.Net.IPEndPoint endpoint=new IPEndPoint(IPAddress.Parse(this.server),this.port);

				Connect(endpoint);//连接Smtp服务器 
				ReceiveStream();//获取连接信息 

				if (this.username!=null)
				{ 
					//开始进行服务器认证 
					//如果状态码是250则表示操作成功 
					if (!OperaStream("EHLO Localhost","250")) 
					{ 
						this.Close(); 
						return false; 
					} 

					if (!OperaStream("AUTH LOGIN","334")) 
					{ 
						this.Close(); 
						return false; 
					} 
					username=AuthStream(username);//此处将username转换为Base64码 
					if (!OperaStream(this.username,"334")) 
					{ 
						this.Close(); 
						return false; 
					} 

					password=AuthStream(password);//此处将password转换为Base64码 
					if (!OperaStream(this.password,"235"))
					{ 
						this.Close(); 
						return false; 
					} 
					return true; 
				} 

				else 

				{ //如果服务器不需要认证 
					if (OperaStream("HELO Localhost","250")) 
					{ 
						return true; 
					} 
					else 
					{ 
						return false; 
					}
				}
			} 
			catch(Exception ex)
			{ 
				log4net.ILog log = log4net.LogManager.GetLogger("MailSend.getMailServer");
				if(log.IsErrorEnabled)
					log.Error(this.subject,ex);

				return false;
			} 

		}


		private string AuthStream(String strCmd) 
		{ 
			try 
			{ 

				byte[] by=Encoding.Default.GetBytes(strCmd.ToCharArray()); 

				strCmd=Convert.ToBase64String(by); 

			} 

			catch(Exception ex) 

			{
				return ex.ToString();
			} 

			return strCmd; 

		} 


		public void LoadAttFile(String path) 
		{ 
			//根据路径读出文件流

			//FileStream fstr=new FileStream(path,FileMode.Open);//建立文件流对象 
			FileStream fstr=new FileStream(path, FileMode.Open,FileAccess.Read);//建立文件流对象 

			byte[] by=new byte[Convert.ToInt32(fstr.Length)]; 

			fstr.Read(by,0,by.Length);//读取文件内容

			fstr.Close();//关闭 

			//格式转换 

			String fileinfo=Convert.ToBase64String(by);//转化为base64编码 

			//增加到文件表中 

			DataRow dr=filelist.NewRow(); 

			dr[0]=Path.GetFileName(path);//获取文件名 

			dr[1]=fileinfo;//文件内容 
			filelist.Rows.Add(dr);//增加 

		} 


		private void Attachment(bool isHideAttach) 			
		{ //对文件列表做循环 

			for (int i=0;i <filelist.Rows.Count; i++)
					
			{ 

				DataRow dr=filelist.Rows[i]; 

				WriteStream("--unique-boundary-1");//邮件内容分隔符 

				if(isHideAttach)
				{
				WriteStream("Content-Type: application/octet-stream;name=\""+dr[0].ToString()+"\"");//文件格式 
				//furion 20060824 这一块也用于发商户证书了,要同时测试一下发送日志是否成功.
				//WriteStream("Content-Type: image/jpeg; name=\""+dr[0].ToString()+"\"");//文件格式 

				WriteStream("Content-Transfer-Encoding: base64");//内容的编码 

				//WriteStream("Content-Disposition:attachment;filename=\""+dr[0].ToString()+"\"");//文件名 

				//("Content-ID","IMG"+newInteger(i).toString());
				//mm.setSubType("related");
				WriteStream("Content-ID: <"+dr[0].ToString()+"@01C66855.BD2FE7A0>");
				WriteStream("Content-Description:" +dr[0].ToString());
				WriteStream("Content-Location: "+dr[0].ToString());

				WriteStream(""); 
				}
				else
				{
					WriteStream("Content-Type: application/octet-stream;name=\""+dr[0].ToString()+"\"");
					WriteStream("Content-Transfer-Encoding: base64");//内容的编码 

					WriteStream("Content-Disposition:attachment;filename=\""+dr[0].ToString()+"\"");//文件名 
					WriteStream(""); 
				}

				String fileinfo=dr[1].ToString(); 

				WriteStream(fileinfo);//写入文件的内容 

				WriteStream(""); 
			} 
		}


		public void Send()
		{
			Send(true);
		}

		public void Send(bool isHideAttach)
		{
			getMailServer();

			if(!OperaStream("mail from: " + this.from,"250"))
			{
				this.Close();
				return;
			}

			string[] tolist = this.to.Split(';');
			foreach(string str in tolist)
			{
				if(str != null && str.Trim() != "")
				{
					if(!OperaStream("rcpt to: " + str.Trim(),"250"))
						//if(!OperaStream("rcpt to: " + this.from,"250"))
					{
						this.Close();
						return;
					}
				}
				else
				{
					this.Close();
					return;
				}
			}

			if(this.cc != null && this.cc.Trim() != "")
			{
				string[] cclist = this.cc.Split(';');
				foreach(string str in cclist)
				{
					if(str != null && str.Trim() != "")
					{
						if(!OperaStream("rcpt to: " + str.Trim(),"250"))
							//if(!OperaStream("rcpt to: " + this.from,"250"))
						{
							this.Close();
							return;
						}
					}
					else
					{
						this.Close();
						return;
					}
				}
			}

			if(!OperaStream("data" ,"354"))
			{
				this.Close();
				return;
			}
			//WriteStream("Date: "+DateTime.Now);//时间 

			WriteStream("From: "+this.fromname+" <"+this.from+">");//发件人			

			WriteStream("To: "+this.to);//收件人 

			WriteStream("Cc: "+this.cc);//收件人 

			WriteStream("Subject: "+this.subject);//主题 

			//邮件格式 
			WriteStream("MIME-Version:1.0");//MIME版本

			if(!isHideAttach)
			{
				WriteStream("Content-Type: multipart/mixed; boundary=\"unique-boundary-1\""); 
			}
			else
			{
				WriteStream("Content-Type: multipart/related; boundary=\"unique-boundary-1\""); 
			}

			//WriteStream("Reply-To:"+this.from);//回复地址

			//WriteStream("X-Priority:"+priority);//优先级 

			 

			//数据ID,随意

			//WriteStream("Message-Id: "+this.from+"@tencent.com"); 

			WriteStream("Content-Transfer-Encoding: 7bit");//+this.encode);//内容编码 

			//WriteStream("X-Mailer:DS Mail Sender V1.0");//邮件发送者 

			WriteStream(""); 

			WriteStream(AuthStream("This is a multi-part message in MIME format.")); 

			WriteStream(""); 


			//从此处开始进行分隔输入 

			WriteStream("--unique-boundary-1");

			//在此处定义第二个分隔符 

			WriteStream("Content-Type: multipart/alternative;Boundary=\"unique-boundary-2\""); 

			WriteStream("");

			//文本信息 

			WriteStream("--unique-boundary-2"); 

			WriteStream("Content-Type: text/plain;charset="+this.charset); 

			WriteStream("Content-Transfer-Encoding:7bit");//+this.encode); 

			WriteStream(""); 

			WriteStream(body); 

			WriteStream("");//一个部分写完之后就写如空信息，分段 

			//html信息 

			WriteStream("--unique-boundary-2"); 

			WriteStream("Content-Type: text/html;charset="+this.charset); 

			WriteStream("Content-Transfer-Encoding:7bit");//+this.encode); 

			WriteStream(""); 

			WriteStream(htmlbody); 

			WriteStream(""); 

			WriteStream("--unique-boundary-2--");//分隔符的结束符号，尾巴后面多了—

			WriteStream(""); 

			//增加附件 

			Attachment(isHideAttach);//这个方法是我们在上面讲过的，实际上他放在这 

			//furion 20080619 发送附件有错,莫非这里多了个空行?
			WriteStream(""); 

			WriteStream("--unique-boundary-1--"); 

				if (!OperaStream(".","250"))//最后写完了，输入"." 

				{ 

					this.Close(); //关闭连接

				} 
			this.Close();
		}

	}
}
