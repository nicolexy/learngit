using System;
using System.Collections;

namespace TENCENT.OSS.CFT.KF.DataAccess
{
	/// <summary>
	/// ҵ������
	/// </summary>
	public class YWCommandCode
	{
		public static int �޸��û���Ϣ = 310020;
		public static int ��ѯ�û���Ϣ = 310000; //S_CMD_QUERY_USER 

		public static int ��ѯ���׵���Ϣ = 400000; //S_CMD_QUERY_TRAN_LIST
		public static int �޸Ľ��׵���Ϣ = 400020; //S_CMD_ALTER_TRAN_LIST
		public static int �˿�3�н鵽���=10042 ; //T_CMD_REFUND3_MEDI_TO_BUYER

		public static int ��ѯ�̻���Ϣ = 100000;//S_CMD_QUERY_MEDI
		public static int �����̻� = 100010; //S_CMD_CREATE_MEDI
		public static int �޸��̻���Ϣ = 100020; //S_CMD_ALTER_MEDI


		public static int �������� = 10002; //T_CMD_FETCH	
		public static int ����ȡ�� = 10005; //T_CMD_FETCH_CANCEL
		public static int ����ֱ�ӳɹ� = 10006; //T_CMD_FETCH_DIRECT_SUCC
		public static int �޸����ֱ�ע = 140020; //S_CMD_SIM_ALTER_TCPAY_LIST

		public static int �ջ�ȷ�� = 10032; //T_CMD_PAY32_MEDI_TO_SALER_NOSALER

		public static int �����˿� = 10040; //T_CMD_REFUND1_MEDI_TO_REFUNDER

		public static int �����û��ʻ� = 10006;
		public static int �ⶳ�û��ʻ� = 10007;

		public static int ��ѯ���ĳ�ֵ����Ϣ = 130000;//S_CMD_QUERY_TCBANKROLL_LIST

		public static int ��ѯ�����û���Ϣ = 110000;//S_CMD_QUERY_BANK
		public static int ���������û���Ϣ = 110010;//S_CMD_CREATE_BANK 
		public static int �޸������û���Ϣ = 110020;//S_CMD_ALTER_BANK  

		public static int ���ʻ�ת�� = 10010;//T_CMD_PAY1_BUYER_TO_SALER  
	}

	/// <summary>
	/// ҵ����Դ����
	/// </summary>
	public class YWSourceType
	{
		public static int �û���Դ = 1; //SOURCE_DB_T_USER
		public static int ���׵���Դ = 2;  //SOURCE_DB_T_TRAN_LIST
		public static int �̻���Դ = 6;  //  SOURCE_DB_T_MIDDLE_USER
		public static int ���ֵ���Դ = 4; //SOURCE_DB_T_TCPAY_LIST

		public static int ��ֵ����Դ = 3;//SOURCE_DB_T_TCBANKROLLLIST

		public static int ������Դ = 7;//SOURCE_DB_T_BANK 

	}

	/// <summary>
	/// ��������������
	/// </summary>
	public class OrderSubType
	{
		public static int ������ = 1; //SOURCE_DB_T_USER
		public static int �ύ�� = 2;  //SOURCE_DB_T_TRAN_LIST
		public static int �ع��� = 3;  //  SOURCE_DB_T_MIDDLE_USER
	}

	/// <summary>
	/// ����˵��
	/// </summary>
	public class YWCommandResult
	{

		private static Hashtable ywresult = null;

		#region �Ѵ�����Ϣ���롣
		
		static void YWCommandCode()
		{
			ywresult = new Hashtable();
			ywresult.Add("60120000","δ֪Ӧ�ô���");
			ywresult.Add("60120001","��������ʧ��");
			ywresult.Add("60120002","db�ṹ�������");
			ywresult.Add("60120101","û���ҵ���Ӧ������");
			ywresult.Add("60120102","��������������");
			ywresult.Add("60120103","�ظ�������");
			ywresult.Add("60120104","�ظ�ִ������");
			ywresult.Add("60120105","�ظ�������Ϣ");
			ywresult.Add("60121001","���ܴ���");
			ywresult.Add("60121002","����ȱʧ");
			ywresult.Add("60121003","�������ʹ���");
			ywresult.Add("60121004","����������");
			ywresult.Add("60121005","�ֿ�ֱ��ֶβ�������");
			ywresult.Add("60121006","����������");
			ywresult.Add("60121007","��������");
			ywresult.Add("60121008","���ݿⳬ������");
			ywresult.Add("60121009","uid���ʹ���");
			ywresult.Add("60121010","��������");
			ywresult.Add("60121101","δ֪�Ĳ�������");
			ywresult.Add("60121102","���ʽ�Ƿ��������ֿ��ܲ�֧�ֻع��Ȳ���");
			ywresult.Add("60121103","�����ֺʹ����಻��Ӧ");
			ywresult.Add("60121104","����ǰ��״̬������");
			ywresult.Add("60121201","��ѯ״̬�����δִ��commit");
			ywresult.Add("60121202","��ѯ״̬�����δִ��rollback");
			ywresult.Add("60121203","��ѯ״̬�����δִ��prepare");
			ywresult.Add("60122001","�û�����");
			ywresult.Add("60122002","�û�����");
			ywresult.Add("60122003","�û���������");
			ywresult.Add("60123000","��������Ԥ�ƴ���,����״̬��������");
			ywresult.Add("60123001","�û���ɫ���������������");
			ywresult.Add("60123002","����������");
			ywresult.Add("60123003","�û����뷽��Ŵ���");
			ywresult.Add("60123004","�û����뷽uid����ǰ��һ��");
			ywresult.Add("60123005","û��������");
			ywresult.Add("60123006","�û���������");
			ywresult.Add("60123007","����ǰ��״̬�޸�");
			ywresult.Add("60123008","����״̬����,������,��������");
			ywresult.Add("60123009","�����������");
			ywresult.Add("60124000","�̻�δ֪����");
			ywresult.Add("60124001","�̻�����");
			ywresult.Add("60124002","�̻�����");
			ywresult.Add("60124003","�̻���������");
			ywresult.Add("60124004","�̻���ˮ�ĶԷ��б�������һ��");
			ywresult.Add("60124005","�̻����׶Է��б��еĽ����Ϣ��������ܽ����Ϣ��һ��");
			ywresult.Add("60110000","ϵͳ�쳣(�ڴ治���)");
			ywresult.Add("60110101","�������ݿ�����ʧ��");
			ywresult.Add("60110102","�������ݿ�ʧ��");
			ywresult.Add("60110103","�����ݿ�Ͽ�����");
			ywresult.Add("60110104","��ȡ�������ݿ�����");
			ywresult.Add("60110105","��ȡ�������ݿ�����");
			ywresult.Add("60110106","�����Զ��ύģʽʧ��");
			ywresult.Add("60110107","��ʼ����ʧ��");
			ywresult.Add("60110108","�ύ����ʧ��");
			ywresult.Add("60110109","�ع�����ʧ��");
			ywresult.Add("60110110","û��ȡ���κν��");
			ywresult.Add("60110111","ȡ���������һ��");
			ywresult.Add("60110112","ȡ����д���");
			ywresult.Add("60110113","�������");
			ywresult.Add("60110114","�����������");
			ywresult.Add("60110115","ƴ��SQL������,û��ֵ");
			ywresult.Add("60110116","�ظ���ʼ����");
			ywresult.Add("60110117","����������,������������");
			ywresult.Add("60111001","�޿��ýڵ�");
			ywresult.Add("60111002","�����ļ���������    ");
			ywresult.Add("60111003","�����ļ��ڵ�״̬����");
			ywresult.Add("60111004","�ļ��нڵ�״̬����");
			ywresult.Add("60111101","���ڴ���������ʧ��");
			ywresult.Add("60111201","�����ӽ�������");
			ywresult.Add("60111302","û�д������ֵ");
			ywresult.Add("60011100","�������񣬲���Ҫ���͵�������");
			ywresult.Add("60011101","��Ч��ҵ�������");
			ywresult.Add("60011102","δ�ҵ���Ӧ��������");
			ywresult.Add("60011103","������Ч");
			ywresult.Add("60011104","�����ֵ���˳���");
			ywresult.Add("60011105","ȡ������Ϣ����");
			ywresult.Add("60011106","�û�������");
			ywresult.Add("60011107","MEDI����ͬ");
			ywresult.Add("60011108","��ͬ�û�");
			ywresult.Add("60011109","����");
			ywresult.Add("60011110","����");
			ywresult.Add("60011111","���Ѿ�����");
			ywresult.Add("60011112","��������");
			ywresult.Add("60011113","�û����ʹ���");
			ywresult.Add("60011114","��״̬��");
			ywresult.Add("60025000","db�������,���ַ��ػᵼ�»ع�����");
			ywresult.Add("60025001","db�����������,���ַ��ػᵼ�»ع�����");
			ywresult.Add("60025002","db��ѯ����");
			ywresult.Add("60025003","db��ѯ�������");
			ywresult.Add("60025004","�ļ��������");
			ywresult.Add("60026000","ϵͳά����");
			ywresult.Add("60027000","�Ѿ������,��ǰ�淵�سɹ�");
		}
		#endregion

		public static string GetResultDetailInfo(string resultcode)
		{
			if(ywresult == null)
				return "";

			if(ywresult.ContainsKey(resultcode))
			{
				return ywresult[resultcode].ToString();
			}
			else
				return "";

		}
	}
}
