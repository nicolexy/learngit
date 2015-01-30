using System;
using System.Threading;

namespace commLib
{
	/// <summary>
	/// GenID ��ժҪ˵����
	/// </summary>
	public class GenID
	{
		
		public static int StaticNo = 1000; //��ʼֵ ���ﵽ9999����ѭ������10001��ʼ
		public static bool StaticNoManageSign = true;

		/// <summary>
		/// ���ƶ����ţ���֤���ظ�������
		/// ��ʼֵΪ1000��ʹ��ʱ��ÿ��+1�����ﵽ9999ʱ��ѭ��ʹ�á� �������ʹ�á�
		/// </summary>
		
		private static void StaticNoManage()
		{
			//�����־λΪfalse,��ȴ�
			try
			{
				while (!StaticNoManageSign)
				{
					Thread.Sleep(5);
				}

				StaticNoManageSign = false; 

				StaticNo ++;

				if (StaticNo >= 9999) 
				{
					StaticNo = 1001;  //���Ϊ��ʼ״̬
				}
			}
			finally
			{
				StaticNoManageSign = true;
			}
		}

		/// <summary>
		/// ����һ����ͬ�Ľ��׵���
		/// </summary>
		/// <returns></returns>
		public static string genLstID(string MerchartID)
		{
			//���ɽ��׵��ţ����ɹ���μ�tipyluo ֧������V2.0��Ҫ���-��¼2�������
			//���׵���Ϊ28λ������ֵ������ǰ10λΪC2C��վ���(SPID)����֧��ͨͳһ���䣻
			//֮��8λΪ�������������ڣ���20050415��
			//���10λC2C��Ҫ��֤һ���ڲ�ͬ�������û�������ʹ��һ�η��񣩣���ID����ͬ��

			StaticNoManage();

			string str = DateTime.Now.Ticks.ToString();
			int ilen   = str.Length;
			str = str.Substring(ilen-5,2);
			string listID = "02" + MerchartID + DateTime.Now.ToString("yyMMdd") + DateTime.Now.ToString("HHmmss") + StaticNo.ToString(); //�����֤����ǰ̨�Ľ��׵����ظ�����
			return listID;
		}

		public static string genLstID_GetMoney(string MerchartID) //����
		{
			//���ɽ��׵��ţ����ɹ���μ�tipyluo ֧������V2.0��Ҫ���-��¼2�������
			//���׵���Ϊ28λ������ֵ������ǰ10λΪC2C��վ���(SPID)����֧��ͨͳһ���䣻
			//֮��8λΪ�������������ڣ���20050415��
			//���10λC2C��Ҫ��֤һ���ڲ�ͬ�������û�������ʹ��һ�η��񣩣���ID����ͬ��
			
			StaticNoManage();

			string str = DateTime.Now.Ticks.ToString();
			int ilen   = str.Length;
			str = str.Substring(ilen-5,2);
			string listID = "03" + MerchartID + DateTime.Now.ToString("yyMMdd") + DateTime.Now.ToString("HHmmss") + StaticNo.ToString(); //�����֤����ǰ̨�Ľ��׵����ظ�����
			return listID;
		}

		//����һ����ֵ��
		public static string genSaveID(out string Msg)
		{
			Msg = null;
			try
			{
				StaticNoManage();

				string saveID = "103" + DateTime.Now.ToString("yyMMddHHmmssff") + StaticNo.ToString(); //��ȷ�����6λ
				return saveID;	
			}
			catch(Exception e)
			{
				Msg = "���ɳ�ֵ��ʧ�ܣ�" + e.Message.ToString().Replace("'","��");
				throw;
			}
			
		}

		//����һ�������еĶ�����
		public static string genBanklist(out string Msg)
		{
			Msg = null;
			try
			{
				StaticNoManage();

				string newBankList = "CFT" + DateTime.Now.ToString("yMMddHHmmss") + StaticNo.ToString(); //��ȷ�����뼶 ���3λ
				return newBankList;
			}
			catch(Exception e)
			{
				Msg = "���ɸ����ж�����ʧ�ܣ�" + e.Message.ToString().Replace("'","��");
				throw;
			}
		}


		//����һ��������Ϣ��
		public static string genMSGID(out string Msg)
		{
			Msg = null;
			try
			{
				StaticNoManage();

				string saveID = "FW" + DateTime.Now.ToString("yyMMddHHmmssff") + StaticNo.ToString(); //��ȷ�����6λ
				return saveID;	
			}
			catch(Exception e)
			{
				Msg = "���ɶ�����Ϣ��ʧ�ܣ�" + e.Message.ToString().Replace("'","��");
				throw;
			}
			
		}

        //���ɶ���Ǩ�Ʒ�����Ϣ��
        public static string GenOrderMigrationMSGId(string transactionId)
        {
            try
            {
                StaticNoManage();
                string listID = transactionId + DateTime.Now.ToString("yyMMdd") + DateTime.Now.ToString("HHmmss") + StaticNo.ToString();
                return listID;
            }
            catch (Exception e)
            {
                return "";
            }
        }
	}
}
