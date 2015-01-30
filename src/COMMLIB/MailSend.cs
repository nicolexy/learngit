using System; 
using System.Net.Sockets;//���ڴ����������� 
using System.IO; //���ڴ������İ� 
using System.Text;//���ڴ����ı����� 
using System.Data; 
using System.Net; 

namespace TENCENT.OSS.C2C.Finance.Common.CommLib
{
	/// <summary>
	/// MailSend ��ժҪ˵����
	/// </summary>
	public class MailSend : TcpClient
	{
		private string server;//SMTP����������
		public string Server
		{
			set{this.server = value;}
		}

		private int port;//�˿� 
		public int Port
		{
			set{this.port = value;}
		}

		private string username;//�û��� 
		public string UserName
		{
			set{this.username = value;}
		}

		private string password;//���� 
		public string Password
		{
			set{this.password = value;}
		}

		private string subject;//���� 
		public string Subject
		{
			set{this.subject = value;}
		}

		private string body;//�ı�����
		public string Body
		{
			set{this.body = value;}
		}

		private string htmlbody;//���ı����� 
		public string HtmlBody
		{
			get{return htmlbody;}
			set{this.htmlbody = value;}
		}

		private string from;//�����˵�ַ 
		public string From
		{
			set{this.from = value;}
		}

		private string to;//�ռ��˵�ַ 
		public string To
		{
			set{this.to = value;}
		}

		private string cc;//�����˵�ַ 
		public string CC
		{
			set{this.cc = value;}
		}

		private string fromname;//���������� 
		public string FromName
		{
			set{this.fromname = value;}
		}

		private string toname;//�ռ������� 
		public string ToName
		{
			set{this.toname = value;}
		}

		private string content_type;//�ʼ�����
		public string Content_Type
		{
			set{this.content_type = value;}
		}

		private string encode;//�ʼ����� 
		public string Encode
		{
			set{this.encode = value;}
		}

		private string charset;//���Ա��� 
		public string Charset
		{
			set{this.charset = value;}
		}

		public DataTable filelist;//�����б�

		private int priority;//�ʼ����ȼ�
		public int Priority
		{
			set{this.priority = value;}
		}

		public MailSend()
		{
			filelist=new DataTable();//�Ѷ����������ʼ������

			filelist.Columns.Add(new DataColumn("filename",typeof(string)));//�ļ��� 

			filelist.Columns.Add(new DataColumn("filecontent",typeof(string)));//�ļ����� 
		}

		private void WriteStream(string strCmd)
		{
			WriteStream(strCmd,"gb2312");
		}

		private void WriteStream(string strCmd,string charset) 
		{ 
			try
			{
				Stream TcpStream;//����������� 
				strCmd = strCmd + "\r\n"; //���뻻�з� 
				TcpStream =this.GetStream();//��ȡ������
				//��������ת��Ϊbyte[] 
				byte[] bWrite = Encoding.GetEncoding(charset).GetBytes(strCmd.ToCharArray()); 
				//����ÿ��д������ݴ�С�������Ƶģ���ô���ǽ�ÿ��д������ݳ��ȶ��ڣ������ֽڣ�һ������ȳ����ˣ������ͷֲ�д�롣
				int start=0; 
				int length=bWrite.Length; 
				int page=0; 
				int size=75; 
				int count=size; 
				if (length>75) 
				{ 
					//���ݷ�ҳ 
					if ((length/size)*size < length) 
						page=length/size+1; 
					else 
						page=length/size; 
					for (int i=0;i < page; i++) 
					{ 
						start=i*size; 
						if (i==page-1) 
							count=length-(i*size); 
						TcpStream.Write(bWrite,start,count);//������д�뵽�������� 
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
			NetworkStream ns = this.GetStream();//�˴����ɻ�ȡ�������ķ��������� 
			int size=ns.Read(by,0,by.Length);//��ȡ������ 
			if (size>0) 
			{ 
				sp=Encoding.Default.GetString(by);//ת��ΪString 
			} 
			return sp; 
		} 


		private bool OperaStream(string strCmd,string state) 
		{
			string sp=null; 
			bool success=false; 
			try 
			{ 
				WriteStream(strCmd);//д������ 
				sp = ReceiveStream();//���ܷ�����Ϣ 
				if (sp.IndexOf(state)!=-1)//�ж�״̬���Ƿ���ȷ 
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
				//�������� 
				//System.Net.IPAddress ipaddress=(IPAddress)System.Net.Dns.Resolve(this.server).AddressList.GetValue(0);  
 
				System.Net.IPEndPoint endpoint=new IPEndPoint(IPAddress.Parse(this.server),this.port);

				Connect(endpoint);//����Smtp������ 
				ReceiveStream();//��ȡ������Ϣ 

				if (this.username!=null)
				{ 
					//��ʼ���з�������֤ 
					//���״̬����250���ʾ�����ɹ� 
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
					username=AuthStream(username);//�˴���usernameת��ΪBase64�� 
					if (!OperaStream(this.username,"334")) 
					{ 
						this.Close(); 
						return false; 
					} 

					password=AuthStream(password);//�˴���passwordת��ΪBase64�� 
					if (!OperaStream(this.password,"235"))
					{ 
						this.Close(); 
						return false; 
					} 
					return true; 
				} 

				else 

				{ //�������������Ҫ��֤ 
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
			//����·�������ļ���

			//FileStream fstr=new FileStream(path,FileMode.Open);//�����ļ������� 
			FileStream fstr=new FileStream(path, FileMode.Open,FileAccess.Read);//�����ļ������� 

			byte[] by=new byte[Convert.ToInt32(fstr.Length)]; 

			fstr.Read(by,0,by.Length);//��ȡ�ļ�����

			fstr.Close();//�ر� 

			//��ʽת�� 

			String fileinfo=Convert.ToBase64String(by);//ת��Ϊbase64���� 

			//���ӵ��ļ����� 

			DataRow dr=filelist.NewRow(); 

			dr[0]=Path.GetFileName(path);//��ȡ�ļ��� 

			dr[1]=fileinfo;//�ļ����� 
			filelist.Rows.Add(dr);//���� 

		} 


		private void Attachment(bool isHideAttach) 			
		{ //���ļ��б���ѭ�� 

			for (int i=0;i <filelist.Rows.Count; i++)
					
			{ 

				DataRow dr=filelist.Rows[i]; 

				WriteStream("--unique-boundary-1");//�ʼ����ݷָ��� 

				if(isHideAttach)
				{
				WriteStream("Content-Type: application/octet-stream;name=\""+dr[0].ToString()+"\"");//�ļ���ʽ 
				//furion 20060824 ��һ��Ҳ���ڷ��̻�֤����,Ҫͬʱ����һ�·�����־�Ƿ�ɹ�.
				//WriteStream("Content-Type: image/jpeg; name=\""+dr[0].ToString()+"\"");//�ļ���ʽ 

				WriteStream("Content-Transfer-Encoding: base64");//���ݵı��� 

				//WriteStream("Content-Disposition:attachment;filename=\""+dr[0].ToString()+"\"");//�ļ��� 

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
					WriteStream("Content-Transfer-Encoding: base64");//���ݵı��� 

					WriteStream("Content-Disposition:attachment;filename=\""+dr[0].ToString()+"\"");//�ļ��� 
					WriteStream(""); 
				}

				String fileinfo=dr[1].ToString(); 

				WriteStream(fileinfo);//д���ļ������� 

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
			//WriteStream("Date: "+DateTime.Now);//ʱ�� 

			WriteStream("From: "+this.fromname+" <"+this.from+">");//������			

			WriteStream("To: "+this.to);//�ռ��� 

			WriteStream("Cc: "+this.cc);//�ռ��� 

			WriteStream("Subject: "+this.subject);//���� 

			//�ʼ���ʽ 
			WriteStream("MIME-Version:1.0");//MIME�汾

			if(!isHideAttach)
			{
				WriteStream("Content-Type: multipart/mixed; boundary=\"unique-boundary-1\""); 
			}
			else
			{
				WriteStream("Content-Type: multipart/related; boundary=\"unique-boundary-1\""); 
			}

			//WriteStream("Reply-To:"+this.from);//�ظ���ַ

			//WriteStream("X-Priority:"+priority);//���ȼ� 

			 

			//����ID,����

			//WriteStream("Message-Id: "+this.from+"@tencent.com"); 

			WriteStream("Content-Transfer-Encoding: 7bit");//+this.encode);//���ݱ��� 

			//WriteStream("X-Mailer:DS Mail Sender V1.0");//�ʼ������� 

			WriteStream(""); 

			WriteStream(AuthStream("This is a multi-part message in MIME format.")); 

			WriteStream(""); 


			//�Ӵ˴���ʼ���зָ����� 

			WriteStream("--unique-boundary-1");

			//�ڴ˴�����ڶ����ָ��� 

			WriteStream("Content-Type: multipart/alternative;Boundary=\"unique-boundary-2\""); 

			WriteStream("");

			//�ı���Ϣ 

			WriteStream("--unique-boundary-2"); 

			WriteStream("Content-Type: text/plain;charset="+this.charset); 

			WriteStream("Content-Transfer-Encoding:7bit");//+this.encode); 

			WriteStream(""); 

			WriteStream(body); 

			WriteStream("");//һ������д��֮���д�����Ϣ���ֶ� 

			//html��Ϣ 

			WriteStream("--unique-boundary-2"); 

			WriteStream("Content-Type: text/html;charset="+this.charset); 

			WriteStream("Content-Transfer-Encoding:7bit");//+this.encode); 

			WriteStream(""); 

			WriteStream(htmlbody); 

			WriteStream(""); 

			WriteStream("--unique-boundary-2--");//�ָ����Ľ������ţ�β�ͺ�����ˡ�

			WriteStream(""); 

			//���Ӹ��� 

			Attachment(isHideAttach);//������������������潲���ģ�ʵ������������ 

			//furion 20080619 ���͸����д�,Ī��������˸�����?
			WriteStream(""); 

			WriteStream("--unique-boundary-1--"); 

				if (!OperaStream(".","250"))//���д���ˣ�����"." 

				{ 

					this.Close(); //�ر�����

				} 
			this.Close();
		}

	}
}
