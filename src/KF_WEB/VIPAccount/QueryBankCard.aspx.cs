using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using TENCENT.OSS.C2C.Finance.Common.CommLib;
using System.Web.Mail;

namespace TENCENT.OSS.CFT.KF.KF_Web.VIPAccount
{
	/// <summary>
	/// Summary description for QueryBankCard.
	/// </summary>
	public partial class QueryBankCard : System.Web.UI.Page
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btn_query.Attributes.Add("onclick","return CheckCardAccount();");
		}

		private void ShowMsg(string msg)
		{
			Response.Write("<script language=javascript>alert('" + msg + "')</script>");
		}

		private void btn_query_Click(object sender, System.EventArgs e)
		{
			string account = tbx_acc.Text.Trim();
			string card = tbx_card.Text.Trim();
			try
			{
				if(account == "" || account.Length < 3)
				{
					ShowMsg("�˺ų��Ȳ���С��3��");
					return;
				}
				else
				{
					int.Parse(account);
					if(card != "")
					{
						int.Parse(card);
					}
				}
			}
			catch
			{
				ShowMsg("�˺�ֻ�������֣�");
				return;
			}

			StartQuery(account,card);
		}

		private void StartQuery(string account,string card)
		{
			try
			{
				Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
                DataSet ds = null;// qs.QueryBankCardBind(account);
				if(ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
				{
					this.ShowMsg("��ѯ��¼Ϊ�ա�");
					this.DataGrid_Order.DataSource = null;
					this.DataGrid_CardBind = null;
				}
				else
				{
					DataSet cardInfo = new DataSet();
					DataTable dtAll = null;

					foreach(DataRow row in ds.Tables[0].Select())
					{
						string cardNumber = row["Fbank_id"].ToString();
						if(card != "")
						{
							if(cardNumber.IndexOf(card) < 0)
							{
								continue;
							}
						}
                        DataSet oneInfo = null;//qs.QueryBankCardInfo(cardNumber);
						if(oneInfo != null && oneInfo.Tables[0] != null)
						{
							oneInfo.Tables[0].Columns.Add("FstateName",typeof(string));
							oneInfo.Tables[0].Columns.Add("Fstate",typeof(string));
							oneInfo.Tables[0].Columns.Add("time",typeof(string));
                            oneInfo.Tables[0].Columns.Add("banktypeName", typeof(string));//lxl 20130912 ��������
							foreach(DataRow rr in oneInfo.Tables[0].Rows)
							{
								rr["Fstate"] = row["Fstate"].ToString().Trim();
								rr["time"] = row["Fmod_time"].ToString().Trim();
							}
							if( dtAll == null)
							{
								dtAll = oneInfo.Tables[0].Clone();
							}
							cardInfo.Tables.Add(oneInfo.Tables[0].Copy());
						}
					}
				
					foreach(DataTable tbl in cardInfo.Tables)
						foreach(DataRow row in tbl.Rows)
						{
                            //lxl 20130912 ��������
                            string Fbank_type = ds.Tables[0].Rows[0]["Fbank_type"].ToString();
                            row["banktypeName"] = classLibrary.getData.GetBankNameFromBankCode(Fbank_type);

							string cardNumber = row["Fbank_id"].ToString();
							string last4Number = cardNumber.Substring(cardNumber.Length-4,4);
							row["Fbank_id"] = last4Number;
							string state = row["Fstate"].ToString();
							switch(state)
							{
								case "0":
									row["FstateName"] = "Ԥ��";
									break;
								case "1":
									row["FstateName"] = "������������";
									break;
								case "2":
									row["FstateName"] = "�Ѿ���";
									break;
								case "3":
									row["FstateName"] = "�Ѿ������";
									break;
								case "4":
									row["FstateName"] = "��Ч��";
									break;
							}
							dtAll.ImportRow(row);
						}
					this.DataGrid_CardBind.DataSource = dtAll;
					this.DataGrid_CardBind.DataBind();
				

					#region ��ȡ���п�������Ϣ
                    DataSet transaction = null;//qs.QueryBankCardTransaction(account);

					if(transaction == null || transaction.Tables.Count == 0 || transaction.Tables[0].Rows.Count == 0)
					{}
					else
					{
                        transaction.Tables[0].Columns.Add("payStateName", typeof(string));//����״̬
						foreach(DataRow row in transaction.Tables[0].Select())
						{
							string cardNumber = row["Fbank_id"].ToString();
							string last4Number = cardNumber.Substring(cardNumber.Length-4,4);
							if(card != "")
							{
								if(last4Number != card)
								{
									row.Delete();
									continue;
								}
							}
							row["Fbank_id"] = last4Number;

                            string state = row["Fpay_state"].ToString();
                            switch (state)
                            {
                                case "0":
                                    row["payStateName"] = "(0)��ʼ״̬";
                                    break;
                                case "1":
                                    row["payStateName"] = "(1)���л����ѿ�";
                                    break;
                                case "2":
                                    row["payStateName"] = "(2)�ѷ���";
                                    break;
                                case "9":
                                    row["payStateName"] = "(9)��ʱ";
                                    break;
                            }
						}
						this.DataGrid_Order.DataSource = transaction;
						this.DataGrid_Order.DataBind();
					}
					#endregion
				}
			}
			catch(Exception e)
			{
                //TENCENT.OSS.C2C.Finance.Common.CommLib.NewMailSend newMail=new TENCENT.OSS.C2C.Finance.Common.CommLib.NewMailSend();
                //newMail.SendMail("wenzou@tencent.com","","QueryBankCard",e.Message,true,null);
                this.ShowMsg("��ѯ��¼����" + e.Message);
			}
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			btn_query.Click += new EventHandler(btn_query_Click);
		}
		#endregion
	}
}
