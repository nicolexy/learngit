using SunLibraryEX;
using System;
using System.Collections;
using System.Data;

namespace TENCENT.OSS.C2C.Finance.Common.CommLib
{
	/// <summary>
	/// Class1 ��ժҪ˵����
	/// </summary>
	public class commData
	{
		public commData()
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
		}

        #region ����
        public class T_CLASS_BASIC
        {
            /// <summary>
            /// ��һ����¼����ȡ��Ա����ֵ
            /// </summary>
            /// <param name="dr"></param>
            public void LoadFromDB(DataRow dr)
            {
                System.Reflection.FieldInfo fi;
                string col_name, col_type;
                System.Type this_type = this.GetType();
                for (int i = 0; i < dr.Table.Columns.Count; i++)
                {
                    col_name = dr.Table.Columns[i].ColumnName;
                    fi = this_type.GetField(col_name);
                    if (fi == null)
                        fi = this_type.GetField(col_name.ToLower());
                    if (fi != null)
                    {
                        col_type = dr.Table.Columns[i].DataType.FullName.ToUpper();
                        if (col_type == "System.Int32" || col_type == "System.Int16")
                            fi.SetValue(this, StringEx.GetInt(dr[col_name]));
                        else if (col_type == "System.DateTime")
                            fi.SetValue(this, StringEx.GetDateTime(dr[col_name]));
                        else
                            fi.SetValue(this, StringEx.GetString(dr[col_name]));
                    }
                }
            }

            /// <summary>
            /// �������������ȡ��Ա����ֵ
            /// </summary>
            /// <param name="dt"></param>
            public void LoadFromParamDB(DataTable dt)
            {
                System.Reflection.FieldInfo fi;
                string col_name, col_value;
                System.Type this_type = this.GetType();
                foreach (DataRow dr in dt.Rows)
                {
                    col_name = dr["FKey"].ToString().Trim();
                    fi = this_type.GetField(col_name);
                    if (fi == null)
                        fi = this_type.GetField(col_name.ToLower());
                    if (fi != null)
                    {
                        col_value = dr["FValue"].ToString().Trim();
                        if (fi.FieldType.FullName == "System.Boolean")
                            fi.SetValue(this, Convert.ToBoolean(col_value));
                        else
                            fi.SetValue(this, col_value);
                    }
                }
            }
        }
        #endregion

        #region t_bankbulletin_info ��
        public class T_BANKBULLETIN_INFO : T_CLASS_BASIC
        {
            public bool IsNew = false;
            public bool IsOPen = false;
            public string bulletin_id;//���й���id
            public string banktype;
            public string banktype_str;
            public string businesstype;//ҵ������
            public string op_support_flag;//ҵ��������
            public string closetype;//�Ƿ�Ӱ��ӿ�
            public string title;
            public string maintext;
            public string popuptext;//��������
            public string startime;
            public string endtime;
            public string createuser;
            public string updateuser;
            public string createtime;
            public string updatetime;
            public string returnUrl;
            public string op_flag;//������־:1-������ 2-�޸ģ� 3-����;��û��
            public string bull_type="1";//�������ͣ�1-����ά����2-�Զ�ά����
        }
        #endregion

        public static Hashtable AirCompany()
        {
            Hashtable ht = new Hashtable();
            ht.Add("szAir", "���ں���_ZH");
            ht.Add("capitalAir", "�׶�����_JD");
            ht.Add("southAir", "�Ϸ�����_CZ");
            ht.Add("eastAir", "��������_MU");
            ht.Add("hnAir", "���Ϻ���_HU");
            ht.Add("ghAir", "�й�����_CA");
            ht.Add("scAir", "�Ĵ�����_3U");
            ht.Add("xmAir", "���ź���_MF");
            ht.Add("hbAir", "�ӱ�����_NS");
            ht.Add("sdAir", "ɽ������_SD");

            return ht;
        }

        public static Hashtable PNRStatus()
        {
            Hashtable ht = new Hashtable();
          //  ht.Add("0", "0 �ɹ�");
            ht.Add("1", "1 �ɹ�");
            ht.Add("2", "2 δ��¼");
            ht.Add("3", "3 md5��֤��ͨ��");
            ht.Add("4", "4 IP����");
            ht.Add("5", "5 ϵͳ����");
            ht.Add("6", "6 PNR�����ѱ�ɾ�����߲����ڴ�PNR");
            ht.Add("7", "7 ��ҳ����");
            ht.Add("8", "8 �Ѿ���⣬�����ظ����");
            ht.Add("9", "9 ��������Ϊ��ͨ��Ʒ����Ҫ�ȱ�ע");
            ht.Add("10", "10 �ѳ�Ʊ");
            ht.Add("11", "11 �֧�� URL�쳣");
            ht.Add("13", "13 û��cookie");
            ht.Add("14", "14 DB��û�����Ķ�����");
            ht.Add("15", "15 ȡ���չ�˾Ʊ�ų���");
            ht.Add("16", "16 PNR������");
            ht.Add("17", "17 ���Ϻ�����");
            ht.Add("18", "18 ��������");
            ht.Add("19", "19 û��Ʊ��");
            ht.Add("20", "20 �Զ�֧��ʧ��");
            ht.Add("21", "21 ��ѯƱ��ʱ����Ȩ�ޣ����ܲ��Ǹ��û���Ŀ�");
            ht.Add("22", "22 ���ɹ�������ȡ֧����Ϣʧ��");
            ht.Add("23", "23 RSA��֤ʧ��");
            ht.Add("24", "24 �۸����Ʋ����㣬�������");
            ht.Add("25", "25 �������Ʋ����㣬�������");
            ht.Add("26", "26 ȼ��˰���Ʋ����㣬�������");
            ht.Add("27", "27 ����˰���Ʋ����㣬�������");
            ht.Add("28", "28 ���ӳ�ʱ");
            ht.Add("29", "29 ����֤����֤ʧ��");
            ht.Add("30", "30 û�������Զ�֧��Ȩ��");
            ht.Add("31", "31 ȡ����֤��ʧ��");
            ht.Add("32", "32 �ÿ�֤��������Ϣ�����Ϻ��չ�˾���Ҫ��");
            ht.Add("33", "33 ��������,pnrsvrԭ������");
            ht.Add("34", "34 ��֤���ƽ�ʧ��");
            ht.Add("35", "35 �۸��쳣");
            ht.Add("36", "36 b2b�ʺŲ�����");
            ht.Add("38", "38 {PNR}�Ѿ���Ⲣ֧���������ٴ����,���ʵ��");
            ht.Add("39", "39 δ�鵽PNR����Ʊ����Ϣ�����ʵ������");
            ht.Add("40", "40 �������˺�δ��Ȩ������ϵ�Ƹ�ͨ�ͷ���Ա��");
            ht.Add("41", "41 RELAY�������");
            ht.Add("50", "50 �쳣����");
            ht.Add("60", "60 IP����");
            ht.Add("61", "61 �û��޴�Ȩ��");
            ht.Add("62", "62 �����3�ζ������˴������룬�˺��Ѿ�����������30���Ӻ�����");
            ht.Add("63", "63 �û������������");
            ht.Add("64", "64 �˿͸��������ú�˾֧�ֵĳ˿���");
            ht.Add("65", "65 ���θ��������ú�˾֧�ֵĺ�����");
            ht.Add("66", "66 �۸���Ը��������ú�˾֧�ֵļ۸������");
            ht.Add("67", "67 �˿͸���Ϊ0");
            ht.Add("68", "68 ������Ϊ0");
            ht.Add("69", "69 �۸���Ը���Ϊ0");
            ht.Add("70", "70 �ӿڲ�֧��(�����̻�)");
            ht.Add("71", "71 ���չ�˾�޴˼۸�");
            ht.Add("72", "72 ��֧�����������Ʊ������");
            ht.Add("73", "73 ����Ʊ����ʵ��Ʊ�۲�һ��");
            ht.Add("74", "74 ֧������ʵ����Ҫ֧����һ��");
            ht.Add("75", "75 ������Ա��֧���ʺŰ󶨹�ϵʧ��");
            ht.Add("76", "76 �Ҳ�����Ӧ�ĺ��չ�˾����");
            ht.Add("77", "77 ���չ�˾B2B��վ����ʱ,���Ժ�����");
            ht.Add("78", "78 ��ѯ��������pnr��Ϣʧ��");
            ht.Add("79", "79 ���չ�˾֧�����ӣ�URL���쳣");
            ht.Add("80", "80 ��ѯ��������ƽ̨��ϵ��ʧ��");
            ht.Add("81", "81 �ڲ����󣺱���pnr��Ϣʧ��");
            ht.Add("91", "91 ����ԭ�����ʧ��");
            ht.Add("92", "92 ҳ��ת��ʧ��");
            ht.Add("99", "99 �ʺ����㣬���ȳ�ֵ");

            return ht;
        }
	}

  //�����쳣�����˱�ĵ���ز���
	public struct hangAccData
	{
		public string fBatchID; //��ʽ��200512121001  1001ָ��������
		public string fBatchNo;
		public string fSeqNo;
		public string fCheckType;
		public string fErrorFlag;
		public string fAdjustFlag;
		public string fInitFlag;
		public string fBankOrder;
		public string fBank_backid;
		public string famt;
		public string fBank_date;
		public string fBankType;
		public string fTde_id;
		public string fListid;
		public string fSubject;
		public string FState;
		public string FErrMsg;
		public string FPickUser;
		public string FPickTime;
		public string fflag;

		public string fauid;
		public string faid;
		public string faname;
		public string fnum;
		public string fsummary;
	}

	public struct tradeList
	{
		public string Fbank_listid;    //���ж�����                                             
		public string flistid;         //���׵�ID                                               
		public string fspid;           //spid                                                   
		public string fbuy_uid;        //fbuy_uid                                               
		public string fbuyid;          //fbuyid                                                 
		public string fcurtype;        //fcurtype                                               
		public string fpaynum;         //fpaynum                                                
		public string fip;             //fip                                                    
		public string Fbuy_bank_type;  //fbuy_bank_type                                         
		public string fcreate_time;    //fcreate_time                                           
		public string fbuy_name;       //fbuy_name                                              
		public string fsaleid;         //fsaleid                                                
		public string fsale_uid;       //fsale_uid                                              
		public string fsale_name;      //fsale_name                                             
		public string fsale_bankid;    //fsale_bankid                                           
		public string Fpay_type;       //                                                       
		public string Fbuy_bankid;                                                              
		public string Fsale_bank_type;                                                          
		public string Fprice;                                                                   
		public string Fcarriage;                                                                
		public string Fprocedure;                                                               
		public string Fmemo;                                                                    
		public string Fcash;                                                                    
		public string Ftoken;                                                                   
		public string Ffee3;          //��������                                                   
		public string Fstate;         //���׵�״̬  Fstate��Fpay_time��Freceive_time��Fmodify_time 
		public string Fpay_time;      //��Ҹ���ʱ�䣨���أ�                                       
		public string Freceive_time;  //��������ʱ��                                           
		public string Fmodify_time;   //����޸�ʱ��                                             
		public string Ftrade_type;    //�������� 1 c2c 2 b2c 3 fastpay 4 ת��                     
		public string Flstate;        //���׵���״̬                                              
		public string Fcoding;     
		public string Fbank_backid;   //���з��صĶ�����  
		public string Ffact;          //֧���ܷ���                                           
		public  string Fmedi_sign;
		public string Fgwq_listid;    
		public string Fchannel_id;
	}

	//����ȯ���������Ϣ
	public struct tokenList
	{
		public string Fauto_id     ;
		public string Fticket_id   ;
		public string Ftde_id      ;
		public string Fson_id      ;
		public string Fuser_id     ;
		public string Fuser_uid    ;
		public string Fpub_id      ;
		public string Fpub_uid     ;
		public string Fpub_name    ;
		public string Fdonate_id   ;
		public string Fdonate_uid  ;
		public string Flistid      ;
		public string Fspid        ;
		public string Fatt_name    ;
		public string Fmer_id      ;
		public string Fdonate_type ;
		public string Fdonate_num  ;
		public string Ftype        ;
		public string Fpub_type    ;
		public string Fstate       ;
		public string Flist_state  ;
		public string Fadjust_flag ;
		public string Ffee         ;
		public string Fuse_pro     ;
		public string Fmin_fee     ;
		public string Fmax_num     ;
		public string Ffact_fee    ;
		public string Fstime       ;
		public string Fetime       ;
		public string Fpub_time    ;
		public string Fuse_time    ;
		public string Fdonate_time ;
		public string Furl         ;
		public string Fpub_user    ;
		public string Fuser_ip     ;
		public string Fpub_ip      ;
		public string Fstandby1    ;
		public string Fstandby2    ;
		public string Fstandby3    ;
		public string Fstandby4    ;
		public string Fmemo        ;
		public string Fmodify_time ;
		public string Fuse_listid  ;

	}

	/// <summary>
	/// ������Ѷ�տ/��ֵ��
	/// </summary>
	public struct bkrlInfo
	{
		public string FlistID;
		public string Fspid;
		public string Fauid;
		public string Faid;
		public string Fcurtype;
		public string Fnum;
		public string FbankType;
		public string Fip;
		public string Fsubject; //��ƿ�Ŀ
		public string FpayFrontTime; //����ǰʱ��
		public string Fstate;
	}

	//����Ȩ�޿�������
	public enum accControl
	{
		user = 1,
		sp   = 2,
		role = 3
	}

	//����ȯ��״̬
	public enum tokenState
	{
		/// <summary>
		/// �ѷ���
		/// </summary>
		published     = 1,

		/// <summary>
		/// �Ѽ�����Ȩ
		/// </summary>
		activePrior   = 2,

		/// <summary>
		/// �Ѽ���
		/// </summary>
        actived       = 3,

		/// <summary>
		/// ��ʹ��
		/// </summary>
		beUsed        = 4,

		/// <summary>
		/// ������
		/// </summary>
		sended        = 5,

		/// <summary>
		/// �Ѿܾ������
		/// </summary>
		refuse        = 6,

		/// <summary>
		/// ȷ��ʹ��
		/// </summary>
		comfirmUse    = 7
	}

	/// <summary>
	/// �̻��ʻ����ԵĻ�����Ϣ
	/// </summary>
	public struct PropInfo
	{
		public string qqid  ;
		public string fuid  ;
		public string tname ; //ģ������
		public string roleid  ;
		public string roleName;
		public short  Fdebit ; //�������
		public short  Fstate ; //����״̬��1 ���� 2 ����
		public int    Fsign1 ; //״̬���1�����Ľ�����  
		public int    Fsign2 ; //״̬���2������������  
		public int    Fsign3 ; //״̬���3���ʻ�����1   
		public int    Fsign4 ; //״̬���4���ʻ�����2   
	}

	/// <summary>
	/// ͳ�Ƶ�Itemֵ
	/// </summary>
	public enum TjItem
	{
		Default= 0,
		save   = 205, //��ֵ      
		fetch  = 213, //����      
		gether = 206, //�տ�      
		payNum = 209, //����      
		TcBuss = 210, //��˾ҵ��  
		PaiPai = 211, //����ҵ��  
		Other  = 212  //������ҵ��
	}

	/// <summary>
	/// ͳ����(�ո���)
	/// </summary>
	public enum TjCommItem
	{
		/// <summary>
		/// Ĭ����ͼ
		/// </summary>
		Default = 0,

		Day = 1,
		
		Week = 2,

		Month= 3
		
	}

	public enum commDataType
	{
		Default = 0,
		usermodify,
		manualsave,
		manualfetch,
		refound,
		thirdprofi,
		/// <summary>
		/// ���ٽ����˵�
		/// </summary>
		fastPayRefound
	}
}
