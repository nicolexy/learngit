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
	/// 用来获取一些公用的数据。比如交易单数据，用户资料，代金券信息等等。以struct的方式返回，便于使用.
	/// </summary>
	public class getData
	{
		public getData()
		{
			//
			// TODO: 在此处添加构造函数逻辑
			//
		}

		static getData()
		{
			htService_code = new Hashtable();
			htService_code.Add("0000000","商户总体配置");
			htService_code.Add("0001101","保险费（01）");
			htService_code.Add("0001102","续期寿险费（02）");
			htService_code.Add("0001103","社会保险费（03）");
			htService_code.Add("0001104","养老保险费（03）");
			htService_code.Add("0001105","车辆保险费（05）");
			htService_code.Add("0001106","财产保险费06");
			htService_code.Add("0002101","物业费");
			htService_code.Add("0003101","租金");
			htService_code.Add("0004101","学费01");
			htService_code.Add("0004102","杂费02");
			htService_code.Add("0005101","住宿费");
			htService_code.Add("0006101","水费");
			htService_code.Add("0007101","电费");
			htService_code.Add("0008101","煤气费");
			htService_code.Add("0009101","有线电视费");
			htService_code.Add("0010101","税款");
			htService_code.Add("0011101","非税缴费");
			htService_code.Add("0012101","基金划款");
			htService_code.Add("0013101","车船税");
			htService_code.Add("0014101","交通罚款");
			htService_code.Add("0015101","货款");
			htService_code.Add("0016101","通讯费");
			htService_code.Add("0017101","充值款");
			htService_code.Add("0018101","还款还贷");
			htService_code.Add("0019101","机票款");
			htService_code.Add("0020101","其他");			
		}

		public static string GetSubjectName(string subjectID)
		{
			if(subjectID == null || subjectID.Trim() == "")
				return "未知类型";

			#region  科目ID转换成科目名称

			switch(subjectID)
			{
				case "1":
				{
					return "充值支付（中介收货款）";
				}
				case "2":
				{
					return "充值支付";
				}
				case "3":
				{
					return "买家确认";
				}
				case "4":
				{
					return "买家确认（自动提现）";
				}
				case "5":
				{
					return "退款";
				}
				case "6":
				{
					return "退款（退卖家货款）";
				}
				case "7":
				{
					return "充值支付（余额支付）";
				}
				case "8":
				{
					return "买家确认（卖家收货款）";
				}
				case "9":
				{
					return "快速交易";
				}
				case "10":
				{
					return "余额支付";
				}

				case "11":
				{
					return "充值";
				}
				case "12":
				{
					return "充值转帐";
				}
				case "13":
				{
					return "转帐";
				}
				case "14":
				{
					return "提现";
				}
				case "15":
				{
					return "回导";
				}
				case "16":
				{
					return "请求付款";
				}
				case "17":
				{
					return "拒绝收款";
				}
				case "18":
				{
					return "过期请求";
				}
				case "19":
				{
					return "直接扣款";
				}
				case "20":
				{
					return "购物券激活";
				}
				case "21":
				{
					return "购物券发行";
				}
				case "22":
				{
					return "购物券激活拒绝";
				}
				case "23":
				{
					return "商户提现";
				}
				case "24":
				{
					return "手工充值";
				}
				case "25":
				{
					return "（信用卡）绑定支付";
				}
				case "26":
				{
					return "（信用卡）绑定支付退款";
				}
				case "27":
				{
					return "（信用卡）绑定支付退款成功";
				}
				case "28":
				{
					return "（信用卡）授权确认支付";
				}
				case "29":
				{
					return "（信用卡）还款";
				}
				case "30":
				{
					return "分账";
				}
				case "31":
				{
					return "分账退款";
				}
				case "32":
				{
					return "冻结";
				}
				case "33":
				{
					return "解冻";
				}
				default:
				{
					return "未知类型";
				}
			}
		
			#endregion
		}


		public static string GetCurTypeName(string curTypeCode)
		{
			if(curTypeCode == null || curTypeCode.Trim() == "")
				return "未知类型" + curTypeCode;

			switch(curTypeCode)
			{
				case "1":
				{
					return "RMB";
				}
				case "2":
				{
					return "基金";
				}
				case "80":
				{
					return "游戏子账户（零钱包）";
				}
				case "81":
				{
					return "彩贝积分";
				}
				case "82":
				{
					return "直通车";
				}
				case "100":
				{
					return "预付卡币种";
				}
				default:
				{
					return "未知类型" + curTypeCode;
				}
			}
		}


		public static Hashtable htService_code;

		//银行类型
		public static string getBankName(string bankID)
		{
			if (bankID == null || bankID.Trim() == "") 
				return "";

			#region 银行转化 相对稳定 不需要查询数据库
			switch(bankID.Trim())
			{
				case "1001" :
					return "招商银行";
				case "1002" :
					return "工商银行";
				case "1050" :
					return "工行信用卡";
				case "1003" :
					return "建设银行";
				case "1004" :
					return "浦发银行" ;
				case "1005" :
					return "农业银行"; 
				case "1006" :
					return "民生银行"; 
				case "1007" :
					return "农行国际卡"; 
				case "1008" :
					return "深圳发展银行"; 
				case "1009" :
					return "兴业银行" ;
				case "1010" :
					return "深圳商业银行"; 
				case "1020" :
					return "中国交通银行";
				case "1021" :
					return "中信实业银行"; 
				case "1022" :
					return "中国光大银行";
				case "1023" :
					return "农村合作信用社"; 
				case "1024" :
					return "上海银行";
				case "1025" :
					return "华夏银行"; 
				case "1026" :
					return "中国银行";
				case "1027" :
					return "广东发展银行"; 
				case "1028" : 
					return "广东银联";
				case "1099" :
					return "其他银行"; 
				// START wandy 20080624
				case "1040" :
					return "建行B2B";
				case "1041" :
					return "民生借记卡";
				case "1042" :
					return "招行B2B";						
				// END wandy 20080624
				case "1044" :
					return "中信银行";	

			}
			#endregion

			return "其它银行(" + bankID + ")";
		}

		public static string getBankID(string bankName)
		{
			if (bankName == null || bankName.Trim() == "") 
				return "1099";

			#region 银行名称转化为ID 相对稳定 不需要查询数据库
			switch(bankName.Trim())
			{
				case "招商银行" :
					return "1001";
				case "工商银行" :
					return "1002";
				case "工行信用卡" :
					return "1050";				
				case "建设银行" :
					return "1003";
				case "浦发银行" :
					return "1004" ;
				case "农业银行" :
					return "1005"; 
				case "民生银行" :
					return "1006"; 
				case "农行国际卡" :
					return "1007"; 
				case "深圳发展银行" :
					return "1008"; 
				case "兴业银行" :
					return "1009" ;
				case "深圳商业银行" :
					return "1010"; 
				case "中国交通银行" :
					return "1020";
				case "中信实业银行" :
					return "1021"; 
				case "中国光大银行" :
					return "1022";
				case "农村合作信用社" :
					return "1023"; 
				case "上海银行" :
					return "1024";
				case "华夏银行" :
					return "1025"; 
				case "中国银行" :
					return "1026";
				case "广东发展银行" :
					return "1027"; 
				case "广东银联" : 
					return "1028";
				// START wandy 20080624
				case "建行B2B" :
					return "1040";
				case "民生借记卡" :
					return "1041";
				case "招行B2B" :
					return "1042";						
				// END wandy 20080624
				case "中信银行" :
					return "1044";	

				case "其他银行" :
					return "1099"; 
			}
			#endregion

			CommLib.commRes.sendLog4Log("commLib.getData","传入的银行名称不正确。[" + bankName +"]");
			return "1099";
		}

		//获取腾讯充值单的相关信息
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
					+ " order by ftde_id DESC  limit 0,1 for update" ;   //查询最新的一条(如果有充值单，则必为充值单)
				

				ar = da.drData(sBankroll,ar);
				*/

				string sBankroll = "bank_list=" + bankOrderID  + "&bank_type=" + bankType + "&fronttime_start=" + startdate + "&fronttime_end=" + enddate + "&num=" + payNum  +
					"&start_time=" + payFromtTime.ToString("yyyy-MM-dd");  //增加时间参数 andrew 20110322

				ar = CommQuery.GetdrDataFromICE(sBankroll,CommQuery.QUERY_TCBANKROLL,ar,out Msg);

				if (ar[9] != null && ar[9] != "")
					ar[9]= DateTime.Parse(ar[9]).ToString("yyyy-MM-dd HH:mm:ss");  //格式化时间。遇到时间，必须格式化

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
				Msg = "腾讯充值单的相关信息异常！给银行的订单号：" + bankOrderID + " 帐务日期：" + bankDate + "银行类型： " + bankType + " 异常信息：" + commRes.replaceHtmlStr(e.Message);
				return false;
			}
		}

	}
}
