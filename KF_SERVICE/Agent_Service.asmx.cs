using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;

using TENCENT.OSS.CFT.KF.Common;

namespace TENCENT.OSS.CFT.KF.KF_Service
{
	/// <summary>
	/// Agent_Service ��ժҪ˵����
	/// </summary>
	[WebService(Namespace="http://Tencent.com/OSS/C2C/Finance/Agent_Service")]
	public class Agent_Service : System.Web.Services.WebService
	{
		/// <summary>
		/// SOAPͷ
		/// </summary>
		public Finance_Header myHeader;

		public Agent_Service()
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

		// WEB ����ʾ��
		// HelloWorld() ʾ�����񷵻��ַ��� Hello World
		// ��Ҫ���ɣ���ȡ��ע�������У�Ȼ�󱣴沢������Ŀ
		// ��Ҫ���Դ� Web �����밴 F5 ��

//		[WebMethod]
//		public string HelloWorld()
//		{
//			return "Hello World";
//		}


		
		[WebMethod(Description="��ȡ�ʴ�����̻�")] 
		public string[] GetRemitSpid()
		{
			try
			{	
				string spidstr= System.Configuration.ConfigurationManager.AppSettings["RemitSpid"].ToString().Trim();
				if(spidstr!="")
				{
					string [] spids =spidstr.Split('|');
					return spids;
				}
				else
				{
					return null;
				}
			}
			catch(Exception err)
			{
				throw new LogicException("Service����ʧ�ܣ�"+err.Message);
			}
		}


		[WebMethod(Description="�ʴ�����ѯ����")] 
		[SoapHeader("myHeader", Direction=SoapHeaderDirection.In)]
		public DataSet GetRemitDataList(string batchid,string tranType,string dataType,string remitType,string tranState,string spid,string remitRec,string listID, int start,int max )
		{
			try
			{	
				RemitQueryClass cuser = new RemitQueryClass(batchid,tranType,dataType,remitType,tranState,spid,remitRec,listID);
				DataSet ds = cuser.GetResultX(start,max,"ZW");

				if(ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
					return null;

				DataRow dr = ds.Tables[0].Rows[0];

				QueryRemitStateInfo query2 = new QueryRemitStateInfo(dr["Ford_date"].ToString(),dr["Ford_ssn"].ToString());

				DataSet ds2 = query2.GetResultX(0,1,"REMIT");

				if(ds2 == null || ds2.Tables.Count == 0 || ds2.Tables[0].Rows.Count == 0)
					return null;

				ds.Tables[0].Columns.Add("Fdest_name",typeof(string));
				ds.Tables[0].Columns.Add("Fdest_card",typeof(string));
				ds.Tables[0].Columns.Add("Fstate",typeof(string));

				ds.Tables[0].Rows[0]["Fdest_name"] = ds2.Tables[0].Rows[0]["Fdest_name"].ToString();
				ds.Tables[0].Rows[0]["Fdest_card"] = ds2.Tables[0].Rows[0]["Fdest_card"].ToString();

				switch(ds2.Tables[0].Rows[0]["Fstate"].ToString())
				{
					case "1":
					{
						ds.Tables[0].Rows[0]["Fstate"] = "֧��ǰ";break;
					}
					case "2":
					{
						ds.Tables[0].Rows[0]["Fstate"] = "֧���ɹ�";break;
					}
					case "3":
					{
						ds.Tables[0].Rows[0]["Fstate"] = "�ʴ�������";break;
					}
					case "4":
					{
						ds.Tables[0].Rows[0]["Fstate"] = "�ʴ��տ�ȷ��";break;
					}	
					case "5":
					{
						ds.Tables[0].Rows[0]["Fstate"] = "�ʴ��ѶҸ����տ���ȡǮ��";break;
					}	
					case "6":
					{
						ds.Tables[0].Rows[0]["Fstate"] = "ת���˻�";break;
					}	
					case "7":
					{
						ds.Tables[0].Rows[0]["Fstate"] = "�˿�ɹ�";break;
					}	
					case "8":
					{
						ds.Tables[0].Rows[0]["Fstate"] = "�ѳ���";break;
					}	
					case "9":
					{
						ds.Tables[0].Rows[0]["Fstate"] = "�˿���";break;
					}
					case "10":
					{
						ds.Tables[0].Rows[0]["Fstate"] = "����ʧ��";break;
					}
					default:
					{
						ds.Tables[0].Rows[0]["Fstate"] = "δ�����״̬";break;
					}	
				}
				
				return ds;
			}
			catch(Exception err)
			{
				throw new LogicException("Service����ʧ�ܣ�"+err.Message);
			}
		}



		[WebMethod(Description="�ʴ�����¼����ѯ����")] 
		[SoapHeader("myHeader", Direction=SoapHeaderDirection.In)]
		public int GetRemitListCount(string batchid,string tranType,string dataType,string remitType,string tranState,string spid,string remitRec,string listID)
		{
			try
			{	
				RemitQueryClass cuser = new RemitQueryClass(batchid,tranType,dataType,remitType,tranState,spid,remitRec,listID);
				return cuser.GetCount("ZW");
			}
			catch(Exception err)
			{
				throw new LogicException("Service����ʧ�ܣ�"+err.Message);
			}
		}


	}
}
