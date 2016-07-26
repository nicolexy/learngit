using System;
using System.Data;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mail;
using System.Collections;
using System.ComponentModel;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Security;


namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
	/// <summary>
	/// PageBase ��ժҪ˵����
	/// </summary>
	public class PageBase : TENCENT.OSS.CFT.KF.KF_Web.PageBase
	{

		#region ��Ա����
		public string JS_BodyId = "bodyid";							
		public string JS_FormId = "Form1";

		public enum CurType
		{
			����� = 1
		}

		public enum FeeStandardTag
		{
			���ֵ� = 0  , ��� = 1 , ���� = 2 , ���½��׽�� = 3 ,���������̻����׶� = 4 ,���½��׽��ֵ� = 5
		}

		public enum FeeStandardCalUnit
		{
			���� = 0,��� = 1,���� = 2,��Ŀ1 = 3 ,��Ŀ2 = 5,��Ŀ3 = 6,���������½��׶� = 4,�����������̻�����������=7
		}

		public enum FeeStandardPriceFormat
		{
			���� = 1,���� = 2,�������ʲ��� = 3,���ճ������ʲ��� = 4
		}

		public enum SettUseTag
		{
			��ʹ�� = 1 , δʹ�� = 0, �Ѽ����� = 2
		}

		public enum FeeRecordStatus
		{
			ɾ�� = 0,
			���� = 1,
			����� = 2,
			��Ч = 9
		}

		public enum FeeContractCyc
		{
			���� = 1, ���� = 2, ���� = 3, ���� = 4, ����ͷ = 5, ����ͷ = 6, ����ָ������ = 7, N�պ���� = 8
		}

		public enum FeeContractStatus
		{
			���� = 1,��ͣ = 2,ȡ�� = 9,���� = 3
		}

		public enum IncomeSumStatus
		{
			δ���� = 0, �Ѵ��� = 1, ���� = 2
		}
		#endregion

		public PageBase()
		{
		}

		#region ��ʽ����
		public string MoneyFormat(string text)
		{
			try
			{
				long Value = Convert.ToInt64(text);
				string sign = Value>=0?"":"-";
				Value = System.Math.Abs(Value);

				string dest = Convert.ToString(Value % 100);
				dest = dest.PadLeft(2,'0');
				dest = "."+dest;

				if (Value<100)
				{
					dest = "0"+dest;
				}
				else
				{
					string origin = Convert.ToString(Value/100);
					while (origin.Length>0)
					{
						if (origin.Length>3)
						{
							dest = ","+origin.Substring(origin.Length-3,3)+dest;
							origin = origin.Remove(origin.Length-3,3);
						}
						else
						{
							dest = origin+dest;
							origin = "";
						}
					}
				}
				return sign+dest;
			}
			catch
			{
				return text;
			}
		}

		public string DateTimeFormatLongDate(string date)
		{
			try
			{
				return Convert.ToDateTime(date).ToString("yyyy��MM��dd��");
			}
			catch
			{
				return date;
			}
		}


		public static string DateTimeFormatLong(string time)
		{
			try
			{
				return Convert.ToDateTime(time).ToString("yyyy��MM��dd��");
			}
			catch
			{
				return time;
			}
		}

		public string EnumGetName(Type enumType,string theValue)
		{
			string[] nameList = Enum.GetNames(enumType);
			for (int i=0;i<nameList.Length;i++)
			{
				string value = Enum.Format( enumType, Enum.Parse(enumType,nameList[i]), "d");
				if ( value==theValue )
					return nameList[i];
			}
			return "Unknown("+theValue+")";
		}

		#endregion

		#region �������,����ʽ
		public bool CheckString(System.Web.UI.WebControls.TextBox text,string name,bool Required)
		{
			text.Text = text.Text.Trim();
			bool ok;
			if ( text.Text=="" )
				ok = !Required;
			else
				ok = true;
			
			if ( !ok )
				ClientAlertFocus("��"+name+"������Ϊ�գ����������롣",text);
			return ok;
		}

		private void ClientAlertFocus(string msg,System.Web.UI.WebControls.TextBox txtBox)
		{
			string Script;
			System.Web.UI.Control uiForm = txtBox.Parent;

			if (uiForm!=null && uiForm.GetType().ToString()=="System.Web.UI.HtmlControls.HtmlForm")
				Script = uiForm.ClientID+"."+txtBox.ID+".focus();"+uiForm.ClientID+"."+txtBox.ID+".select();";
			else
				Script = JS_FormId+"."+txtBox.ID+".focus();"+JS_FormId+"."+txtBox.ID+".select();";

			msg = msg.Replace("'","\\'");
			if (msg!="")
				Script = "alert('"+msg+"');"+Script;

			ClientScript(Script,true);
		}

		private new void ClientScript(string Script,bool Rewrite)
		{
			System.Web.UI.HtmlControls.HtmlGenericControl body = 
				(System.Web.UI.HtmlControls.HtmlGenericControl)Page.FindControl(JS_BodyId);
			if (body!=null)
			{
				if (Script==null)
				{
					if (Rewrite)
						body.Attributes.Remove("onload");
				}
				else
				{
					if (Rewrite)
						body.Attributes["onload"] = Script;
					else
						body.Attributes["onload"] += Script;
				}
			}
		}

		public void PageStateSave(System.Web.UI.WebControls.WebControl control)
		{
			string name = PageStateName(control);
			if ( control is TextBox )
				Session[name] = ((TextBox)control).Text;
			else if ( control is ListControl )
				Session[name] = ((ListControl)control).SelectedValue;
			else if ( control is CheckBox )
				Session[name] = ((CheckBox)control).Checked;
			else
				throw new ApplicationException("PageStateSave��֧�ֵ����ͣ�"+control.GetType().FullName);
		}

		private string PageStateName(System.Web.UI.WebControls.WebControl control)
		{
			return "SunPageState."+this.GetType().FullName+"."+control.UniqueID;
		}

		public void EnumFillList(Type enumType,bool isChar,string AllItemText,params System.Web.UI.WebControls.ListControl[] list)
		{
			for (int j=0;j<list.Length;j++)
			{
				list[j].Items.Clear();
				if (AllItemText!="")
					list[j].Items.Add(new System.Web.UI.WebControls.ListItem(AllItemText,""));
				string[] nameList = Enum.GetNames(enumType);
				for (int i=0;i<nameList.Length;i++)
				{
					string value = Enum.Format( enumType, Enum.Parse(enumType,nameList[i]), "d");
					if (isChar)
						value = EnumGetChar(value);
					list[j].Items.Add(new System.Web.UI.WebControls.ListItem(nameList[i],value));
				}
				if (list[j].Items.Count>0)
					list[j].SelectedIndex = 0;
			}
		}

		private string EnumGetChar(string theValue)
		{
			return EnumGetChar(Convert.ToInt32(theValue));
		}

		private string EnumGetChar(int theValue)
		{
			return Convert.ToString(Convert.ToChar(theValue));
		}

		#endregion

		#region ��������亯��
		public void FillChannelNo(params System.Web.UI.WebControls.ListControl[] lists)
		{
			for (int i=0;i<lists.Length;i++)
				dsFill(CacheChannelNo.Tables[0],"FChannelNo","FName","",lists[i],"");
		}

		private int dsFill(System.Data.DataTable table,string valueField,string textField,string textField2,System.Web.UI.WebControls.ListControl Source,string AllItemText)
		{
			Source.Items.Clear();
			if (AllItemText!="")
				Source.Items.Add(new System.Web.UI.WebControls.ListItem(AllItemText,""));
			string sText,sText2,sValue;
			System.Web.UI.WebControls.ListItem li;
			for (int i=0;i<table.Rows.Count;i++)
			{
				sValue = table.Rows[i][valueField].ToString();
				sText = table.Rows[i][textField].ToString();
				if (textField2!="") sText2 = "  "+ table.Rows[i][textField2].ToString();
				else sText2 = "";
				li = new System.Web.UI.WebControls.ListItem(sText+sText2,sValue);
				Source.Items.Add(li);
			}
			Source.SelectedIndex=0;
			return Source.Items.Count;
		}

		public void PageStateRestore(System.Web.UI.WebControls.WebControl control)
		{
			string name = PageStateName(control);
			if ( Session[name] == null )
				return;
			string value = Session[name].ToString();

			if ( control is TextBox )
				((TextBox)control).Text = value;
			else if ( control is ListControl )
			{
				if ( ((ListControl)control).Items.FindByValue(value) != null )
					((ListControl)control).SelectedValue = value;
			}
			else if ( control is CheckBox )
				((CheckBox)control).Checked = Convert.ToBoolean(value);
			else
				throw new ApplicationException("PageStateRestore��֧�ֵ����ͣ�"+control.GetType().FullName);
		}
		#endregion

		#region ������ϸ��Ϣ��������
		public string ScriptPopup(string Url,int Height,int Width,bool ShowScrollBar)
		{
			return String.Format(
				"javascript:window.open('{0}','','height={1},width={2},toolbar=no,menubar=no,scrollbars={3},resizable=no,location=no,status=no');",
				Url,Height,Width,ShowScrollBar?"yes":"no");
		}

		public void SetSpidShow(System.Web.UI.WebControls.HyperLink link)
		{
			link.Attributes["onclick"] = ScriptPopup("SpidShow.aspx?id="+link.Text,450,700,true);
			link.Text = CacheSpidName(link.Text);
			link.NavigateUrl = "#";
			link.ForeColor = System.Drawing.Color.Black;
		}
		#endregion

		#region ������Ŀ�����ݻ��棬���ڽ�����ʾ
		private const string CacheFeeItemId = "MerchantCache_FeeItem";
		public DataSet CacheFeeItem
		{
			get
			{
				if ( Application[CacheFeeItemId] == null )
				{
					DataSet ds = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service().QueryForSelect4();
					Application[CacheFeeItemId] = ds;
				}
				return (System.Data.DataSet)Application[CacheFeeItemId];
			}
			set
			{
				if ( Application[CacheFeeItemId] != null )
					((System.Data.DataSet)Application[CacheFeeItemId]).Dispose();
				Application[CacheFeeItemId] = value;
			}
		}

		public string CacheFeeItemName(string id)
		{
			for (int i=0;i<CacheFeeItem.Tables[0].Rows.Count;i++)
			{
				if ( CacheFeeItem.Tables[0].Rows[i]["FFeeItem"].ToString() == id )
					return CacheFeeItem.Tables[0].Rows[i]["FName"].ToString();
			}
			return id;
		}
		#endregion

		#region �����׼�����ݻ��棬���ڽ�����ʾ
		private const string CacheFeeStandardId = "MerchantCache_FeeStandard";
		public DataSet CacheFeeStandard
		{
			get
			{
				if ( Application[CacheFeeStandardId] == null )
				{
					DataSet ds = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service().QueryForSelect5();
					Application[CacheFeeStandardId] = ds;
				}
				return (System.Data.DataSet)Application[CacheFeeStandardId];
			}
			set
			{
				if ( Application[CacheFeeStandardId] != null )
					((System.Data.DataSet)Application[CacheFeeStandardId]).Dispose();
				Application[CacheFeeStandardId] = value;
			}
		}

		public string CacheFeeStandardName(string id)
		{
			for (int i=0;i<CacheFeeStandard.Tables[0].Rows.Count;i++)
			{
				if ( CacheFeeStandard.Tables[0].Rows[i]["FFeeStandard"].ToString() == id )
					return CacheFeeStandard.Tables[0].Rows[i]["FName"].ToString();
			}
			return id;
		}
		#endregion
		
		#region ���������ݻ��棬���ڽ�����ʾ
		private const string CacheChannelNoId = "MerchantCache_ChannelNo";
		public System.Data.DataSet CacheChannelNo
		{
			get
			{
				if ( Application[CacheChannelNoId] == null )
				{
					DataSet ds = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service().QueryForSelect3();
					Application[CacheChannelNoId] = ds;
				}
				return (System.Data.DataSet)Application[CacheChannelNoId];
			}
			set
			{
				if ( Application[CacheChannelNoId] != null )
					((System.Data.DataSet)Application[CacheChannelNoId]).Dispose();
				Application[CacheChannelNoId] = value;
			}
		}

		public string CacheChannelNoName(string id)
		{
			for (int i=0;i<CacheChannelNo.Tables[0].Rows.Count;i++)
			{
				if ( CacheChannelNo.Tables[0].Rows[i]["FChannelNo"].ToString() == id )
					return CacheChannelNo.Tables[0].Rows[i]["FName"].ToString();
			}
			return id;
		}
		#endregion

		#region ��Ʒ�����ݻ��棬���ڽ�����ʾ
		private const string CacheProductTypeId = "MerchantCache_ProductType";
		public System.Data.DataSet CacheProductType
		{
			get
			{
				if ( Application[CacheProductTypeId] == null )
				{
					DataSet ds = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service().QueryForSelect1();
					Application[CacheProductTypeId] = ds;
				}
				return (System.Data.DataSet)Application[CacheProductTypeId];
			}
			set
			{
				if ( Application[CacheProductTypeId] != null )
					((System.Data.DataSet)Application[CacheProductTypeId]).Dispose();
				Application[CacheProductTypeId] = value;
			}
		}

		public string CacheProductTypeName(string channel,string id)
		{
			for (int i=0;i<CacheProductType.Tables[0].Rows.Count;i++)
			{
				if ( CacheProductType.Tables[0].Rows[i]["FChannelNo"].ToString() == channel )
					if ( CacheProductType.Tables[0].Rows[i]["FProductType"].ToString() == id )
						return CacheProductType.Tables[0].Rows[i]["FName"].ToString();
			}
			return id;
		}
		#endregion
	
		#region �̻������ݻ��棬���ڽ�����ʾ
		private const string CacheSpidId = "MerchantCache_Spid";
		public System.Data.DataSet CacheSpid_NOUSE = null;
		/*
		{
			get
			{
				if ( Application[CacheSpidId] == null )
				{
					DataSet ds = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service().QueryForSelect2_NOUSE();
					Application[CacheSpidId] = ds;
				}
				return (System.Data.DataSet)Application[CacheSpidId];
			}
			set
			{
				if ( Application[CacheSpidId] != null )
					((System.Data.DataSet)Application[CacheSpidId]).Dispose();
				Application[CacheSpidId] = value;
			}
		}
		*/

		public string CacheSpidName(string id)
		{
			/*
			for (int i=0;i<CacheSpid.Tables[0].Rows.Count;i++)
			{
				if ( CacheSpid.Tables[0].Rows[i]["FSpid"].ToString() == id )
					return CacheSpid.Tables[0].Rows[i]["FName"].ToString();
			}
			return id;
			*/

			DataSet ds = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service().QueryForSelect6(id);
			if(ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 1)
				return ds.Tables[0].Rows[0]["Fname"].ToString();
			else
				return "UNKNOWN(" + id + ")";
		}
		#endregion
	}
}
