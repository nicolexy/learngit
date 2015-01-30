using System;
using System.Data;
using TENCENT.OSS.C2C.Finance.Common;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.CFT.KF.DataAccess;
using TENCENT.OSS.C2C.Finance.Common.CommLib;
using System.Collections;

namespace TENCENT.OSS.C2C.Finance.Common.CommLib
{
	/// <summary>
	/// ������ȡһЩ���õ����ݡ����罻�׵����ݣ��û����ϣ�����ȯ��Ϣ�ȵȡ���struct�ķ�ʽ���أ�����ʹ��.
	/// </summary>
	public class getData
	{
		public getData()
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
		}

		static getData()
		{
			htService_code = new Hashtable();
			htService_code.Add("0000000","�̻���������");
			htService_code.Add("0001101","���շѣ�01��");
			htService_code.Add("0001102","�������շѣ�02��");
			htService_code.Add("0001103","��ᱣ�շѣ�03��");
			htService_code.Add("0001104","���ϱ��շѣ�03��");
			htService_code.Add("0001105","�������շѣ�05��");
			htService_code.Add("0001106","�Ʋ����շ�06");
			htService_code.Add("0002101","��ҵ��");
			htService_code.Add("0003101","���");
			htService_code.Add("0004101","ѧ��01");
			htService_code.Add("0004102","�ӷ�02");
			htService_code.Add("0005101","ס�޷�");
			htService_code.Add("0006101","ˮ��");
			htService_code.Add("0007101","���");
			htService_code.Add("0008101","ú����");
			htService_code.Add("0009101","���ߵ��ӷ�");
			htService_code.Add("0010101","˰��");
			htService_code.Add("0011101","��˰�ɷ�");
			htService_code.Add("0012101","���𻮿�");
			htService_code.Add("0013101","����˰");
			htService_code.Add("0014101","��ͨ����");
			htService_code.Add("0015101","����");
			htService_code.Add("0016101","ͨѶ��");
			htService_code.Add("0017101","��ֵ��");
			htService_code.Add("0018101","�����");
			htService_code.Add("0019101","��Ʊ��");
			htService_code.Add("0020101","����");			
		}

		public static string GetSubjectName(string subjectID)
		{
			if(subjectID == null || subjectID.Trim() == "")
				return "δ֪����";

			#region  ��ĿIDת���ɿ�Ŀ����

			switch(subjectID)
			{
				case "1":
				{
					return "��ֵ֧�����н��ջ��";
				}
				case "2":
				{
					return "��ֵ֧��";
				}
				case "3":
				{
					return "���ȷ��";
				}
				case "4":
				{
					return "���ȷ�ϣ��Զ����֣�";
				}
				case "5":
				{
					return "�˿�";
				}
				case "6":
				{
					return "�˿�����һ��";
				}
				case "7":
				{
					return "��ֵ֧�������֧����";
				}
				case "8":
				{
					return "���ȷ�ϣ������ջ��";
				}
				case "9":
				{
					return "���ٽ���";
				}
				case "10":
				{
					return "���֧��";
				}

				case "11":
				{
					return "��ֵ";
				}
				case "12":
				{
					return "��ֵת��";
				}
				case "13":
				{
					return "ת��";
				}
				case "14":
				{
					return "����";
				}
				case "15":
				{
					return "�ص�";
				}
				case "16":
				{
					return "���󸶿�";
				}
				case "17":
				{
					return "�ܾ��տ�";
				}
				case "18":
				{
					return "��������";
				}
				case "19":
				{
					return "ֱ�ӿۿ�";
				}
				case "20":
				{
					return "����ȯ����";
				}
				case "21":
				{
					return "����ȯ����";
				}
				case "22":
				{
					return "����ȯ����ܾ�";
				}
				case "23":
				{
					return "�̻�����";
				}
				case "24":
				{
					return "�ֹ���ֵ";
				}
				case "25":
				{
					return "�����ÿ�����֧��";
				}
				case "26":
				{
					return "�����ÿ�����֧���˿�";
				}
				case "27":
				{
					return "�����ÿ�����֧���˿�ɹ�";
				}
				case "28":
				{
					return "�����ÿ�����Ȩȷ��֧��";
				}
				case "29":
				{
					return "�����ÿ�������";
				}
				case "30":
				{
					return "����";
				}
				case "31":
				{
					return "�����˿�";
				}
				case "32":
				{
					return "����";
				}
				case "33":
				{
					return "�ⶳ";
				}
				default:
				{
					return "δ֪����";
				}
			}
		
			#endregion
		}


		public static string GetCurTypeName(string curTypeCode)
		{
			if(curTypeCode == null || curTypeCode.Trim() == "")
				return "δ֪����" + curTypeCode;

			switch(curTypeCode)
			{
				case "1":
				{
					return "RMB";
				}
				case "2":
				{
					return "����";
				}
				case "80":
				{
					return "��Ϸ���˻�����Ǯ����";
				}
				case "81":
				{
					return "�ʱ�����";
				}
				case "82":
				{
					return "ֱͨ��";
				}
				case "100":
				{
					return "Ԥ��������";
				}
				default:
				{
					return "δ֪����" + curTypeCode;
				}
			}
		}


		public static Hashtable htService_code;

		//��������
		public static string getBankName(string bankID)
		{
			if (bankID == null || bankID.Trim() == "") 
				return "";

			#region ����ת�� ����ȶ� ����Ҫ��ѯ���ݿ�
			switch(bankID.Trim())
			{
				case "1001" :
					return "��������";
				case "1002" :
					return "��������";
				case "1050" :
					return "�������ÿ�";
				case "1003" :
					return "��������";
				case "1004" :
					return "�ַ�����" ;
				case "1005" :
					return "ũҵ����"; 
				case "1006" :
					return "��������"; 
				case "1007" :
					return "ũ�й��ʿ�"; 
				case "1008" :
					return "���ڷ�չ����"; 
				case "1009" :
					return "��ҵ����" ;
				case "1010" :
					return "������ҵ����"; 
				case "1020" :
					return "�й���ͨ����";
				case "1021" :
					return "����ʵҵ����"; 
				case "1022" :
					return "�й��������";
				case "1023" :
					return "ũ�����������"; 
				case "1024" :
					return "�Ϻ�����";
				case "1025" :
					return "��������"; 
				case "1026" :
					return "�й�����";
				case "1027" :
					return "�㶫��չ����"; 
				case "1028" : 
					return "�㶫����";
				case "1099" :
					return "��������"; 
				// START wandy 20080624
				case "1040" :
					return "����B2B";
				case "1041" :
					return "������ǿ�";
				case "1042" :
					return "����B2B";						
				// END wandy 20080624
				case "1044" :
					return "��������";	

			}
			#endregion

			return "��������(" + bankID + ")";
		}

		public static string getBankID(string bankName)
		{
			if (bankName == null || bankName.Trim() == "") 
				return "1099";

			#region ��������ת��ΪID ����ȶ� ����Ҫ��ѯ���ݿ�
			switch(bankName.Trim())
			{
				case "��������" :
					return "1001";
				case "��������" :
					return "1002";
				case "�������ÿ�" :
					return "1050";				
				case "��������" :
					return "1003";
				case "�ַ�����" :
					return "1004" ;
				case "ũҵ����" :
					return "1005"; 
				case "��������" :
					return "1006"; 
				case "ũ�й��ʿ�" :
					return "1007"; 
				case "���ڷ�չ����" :
					return "1008"; 
				case "��ҵ����" :
					return "1009" ;
				case "������ҵ����" :
					return "1010"; 
				case "�й���ͨ����" :
					return "1020";
				case "����ʵҵ����" :
					return "1021"; 
				case "�й��������" :
					return "1022";
				case "ũ�����������" :
					return "1023"; 
				case "�Ϻ�����" :
					return "1024";
				case "��������" :
					return "1025"; 
				case "�й�����" :
					return "1026";
				case "�㶫��չ����" :
					return "1027"; 
				case "�㶫����" : 
					return "1028";
				// START wandy 20080624
				case "����B2B" :
					return "1040";
				case "������ǿ�" :
					return "1041";
				case "����B2B" :
					return "1042";						
				// END wandy 20080624
				case "��������" :
					return "1044";	

				case "��������" :
					return "1099"; 
			}
			#endregion

			CommLib.commRes.sendLog4Log("commLib.getData","������������Ʋ���ȷ��[" + bankName +"]");
			return "1099";
		}

		//��ȡ��Ѷ��ֵ���������Ϣ
		public static bool saveListInfo(string bankOrderID,string bankType,string bankDate,MySqlAccess da,long payNum,out bkrlInfo bk,out string Msg)
		{
			Msg = null;
			bk  = new bkrlInfo();
			try
			{
				DateTime payFromtTime = DateTime.Parse(bankDate);
				string startdate = payFromtTime.AddDays(-1).ToString("yyyy-MM-dd 00:00:00");
				string enddate = payFromtTime.AddDays(1).ToString("yyyy-MM-dd 23:59:59");

				string [] ar = new string[11];
				ar[0] = "Flistid";
				ar[1] = "Fspid";
				ar[2] = "Fauid";
				ar[3] = "Faid";
				ar[4] = "Fcurtype";
				ar[5] = "Fnum";
				ar[6] = "Fbank_Type";
				ar[7] = "Fip";
				ar[8] = "Fsubject";
				ar[9] = "Fpay_front_time";
				ar[10]= "Fstate";

				/*
				string sBankroll = "Select Flistid,Fspid,Fauid,Faid,Fcurtype,Fnum,Fbank_Type,Fip,Fsubject,Fpay_front_time,Fstate " 
					+ " From c2c_db.t_tcbankroll_list " 
					//+ " where Fbank_list = '" + bankOrderID  + "' and Fbank_type='" + bankType + "' and Fpay_front_time like '" + bankDate + "%'" 
					+ " where Fbank_list = '" + bankOrderID  + "' and Fbank_type='" + bankType + "' and Fpay_front_time between '" + startdate + "' and  '" + enddate + "' and Fnum=" + payNum + " "
					+ " order by ftde_id DESC  limit 0,1 for update" ;   //��ѯ���µ�һ��(����г�ֵ�������Ϊ��ֵ��)
				

				ar = da.drData(sBankroll,ar);
				*/

				string sBankroll = "bank_list=" + bankOrderID  + "&bank_type=" + bankType + "&fronttime_start=" + startdate + "&fronttime_end=" + enddate + "&num=" + payNum  +
					"&start_time=" + payFromtTime.ToString("yyyy-MM-dd");  //����ʱ����� andrew 20110322

				ar = CommQuery.GetdrDataFromICE(sBankroll,CommQuery.QUERY_TCBANKROLL,ar,out Msg);

				if (ar[9] != null && ar[9] != "")
					ar[9]= DateTime.Parse(ar[9]).ToString("yyyy-MM-dd HH:mm:ss");  //��ʽ��ʱ�䡣����ʱ�䣬�����ʽ��

				bk.FlistID = ar[0];
				bk.Fspid   = ar[1];
				bk.Fauid   = ar[2];
				bk.Faid    = ar[3];
				bk.Fcurtype= ar[4];
				bk.Fnum    = ar[5];
				bk.FbankType=ar[6];
				bk.Fip     = ar[7];
				bk.Fsubject= ar[8];
				bk.Fstate  = ar[10];
				
				return true;
			}
			catch(Exception e)
			{
				Msg = "��Ѷ��ֵ���������Ϣ�쳣�������еĶ����ţ�" + bankOrderID + " �������ڣ�" + bankDate + "�������ͣ� " + bankType + " �쳣��Ϣ��" + commRes.replaceHtmlStr(e.Message);
				return false;
			}
		}

	}
}
