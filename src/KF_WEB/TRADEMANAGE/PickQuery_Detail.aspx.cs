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
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.CFT.KF.KF_Web;
using System.Web.Services.Protocols;
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using CFT.CSOMS.BLL.TradeModule;
using CFT.CSOMS.BLL.TransferMeaning;
using CFT.Apollo.Logging;

namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
	/// <summary>
	/// PickQuery_Detail ��ժҪ˵����
	/// </summary>
	public partial class PickQuery_Detail : TENCENT.OSS.CFT.KF.KF_Web.PageBase
	{
        PickService pickservice = new PickService();
		protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				Label1.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();
				int operid = Int32.Parse(Session["OperID"].ToString());
                
				//if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID,"InfoCenter")) Response.Redirect("../login.aspx?wh=1");
                if (!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("TradeManagement", this)) Response.Redirect("../login.aspx?wh=1");

				if(!IsPostBack)
				{
				
					//string tdeid = Request.QueryString["tdeid"];
					string listid = Request.QueryString["listid"];
					string begintime = Request.QueryString["begintime"];
					string endtime = Request.QueryString["endtime"];

					if(listid == null || listid.Trim() == "")
					{
						WebUtils.ShowMessage(this.Page,"��������");
					}

					try
					{
						BindInfo(listid,begintime,endtime);
					}
					catch(LogicException err)
					{
						WebUtils.ShowMessage(this.Page,err.Message);
					}
					catch(SoapException eSoap) //����soap���쳣
					{
						string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
						WebUtils.ShowMessage(this.Page,"���÷������" + errStr);
					}
					catch(Exception eSys)
					{
						WebUtils.ShowMessage(this.Page,"��ȡ����ʧ�ܣ�" + eSys.Message.ToString());
					}
				}
			}
			catch
			{
				Response.Redirect("../login.aspx?wh=1");
			}
		}

		#region Web ������������ɵĴ���
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: �õ����� ASP.NET Web ���������������ġ�
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// �����֧������ķ��� - ��Ҫʹ�ô���༭���޸�
		/// �˷��������ݡ�
		/// </summary>
		private void InitializeComponent()
		{    

		}
		#endregion

		private void BindInfo(string listid, string begintime, string endtime)
		{
			DateTime u_beginTime  = new DateTime(1940,1,1);
			DateTime u_endTime  = new DateTime(1940,1,1);

			if(begintime == null || endtime == null)
			{				
			}
			else
			{
				try
				{
					u_beginTime = DateTime.Parse(begintime + " 00:00:00");
					u_endTime = DateTime.Parse(endtime + " 23:59:59");
				}
				catch
				{
					u_beginTime  = new DateTime(1940,1,1);
					u_endTime  = new DateTime(1940,1,1);
				}

			}

            LogHelper.LogInfo("���� CFT.Apollo.Bow.Withdraw.WithdrawRepository.GetItemByListid");
            DataTable dt = pickservice.GetPickListDetail(listid);

			if(dt != null && dt.Rows.Count > 0 )
			{
				DataRow dr = dt.Rows[0];
				labFListID.Text = PublicRes.GetString(dr["FListID"]);

				long itmp = long.Parse(PublicRes.GetString(dr["FNum"]));

				//furion 20051012 ��Ԫת��ר����
				//float ltmp = (float)itmp/100;
				double ltmp = MoneyTransfer.FenToYuan(itmp);

				string[] CoverPickFuid = System.Configuration.ConfigurationManager.AppSettings["CoverPickFuid"].ToString().Split('|');

				for(int i=0; i<CoverPickFuid.Length; i++)
				{
					if(CoverPickFuid[i].ToString() == dr["Fuid"].ToString())
					{
						try
						{
							int PointIndex = ltmp.ToString().IndexOf(".");
							dr["FNewNum"] = "******" + ltmp.ToString().Substring(PointIndex-1,ltmp.ToString().Length-PointIndex+1);
						}
						catch
						{
							dr["FNewNum"] = "******";
						}
					}
				}

				labFNum.Text = ltmp.ToString();

				string tmp = PublicRes.GetInt(dr["FState"]);
                labFState.Text = Transfer.returnDicStr("TCLIST_STATE", tmp);

				tmp = PublicRes.GetInt(dr["Fsign"]);
                labFSign.Text = Transfer.returnDicStr("TCLIST_SIGN", tmp);

                labFaBank_Type.Text = PublicRes.GetString(Transfer.returnDicStr("BANK_TYPE", PublicRes.GetInt(dr["FaBank_Type"])));

				labFaid.Text = PublicRes.GetString(dr["faid"]);
                bool isRight_SensitiveRole = TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("SensitiveRole", this);
                labFaname.Text = classLibrary.setConfig.ConvertName(PublicRes.GetString(dr["faname"]), isRight_SensitiveRole);
                
                //labFabankid.Text = classLibrary.setConfig.ConvertID(PublicRes.GetString(dr["Fabankid"]),4,4);
                labFabankid.Text = classLibrary.setConfig.BankCardNoSubstring(PublicRes.GetString(dr["Fabankid"]), isRight_SensitiveRole);
                
				labFpay_time.Text = PublicRes.GetDateTime(dr["Fpay_time"]);
				labFmodify_time.Text = PublicRes.GetDateTime(dr["FModify_time"]);

                labFbankName.Text = PublicRes.GetString(dr["Fbank_name"]);
                labFbankID.Text = GetBankIDName(dr["Fbankid"].ToString());
                labFbankType.Text = PublicRes.GetString(Transfer.returnDicStr("BANK_TYPE", PublicRes.GetInt(dr["Fbank_Type"])));
                labFmemo.Text = PublicRes.GetString(dr["Fmemo"]);
                try
                {
                    //���ּ�¼��ѯ����һ��Ԥ�Ƶ���ʱ�䣨Fstandby3���ֶΣ����ֶν�����΢����Ǯ������
                    if (dr["Fproduct"].ToString().Trim() == "7")
                    {
                        lbl_Fstandby3.Text = PublicRes.GetString(dr["Fstandby3"]);
                    }
                }
                catch (Exception exc)
                {
                    lbl_Fstandby3.Text = exc.Message;
                }

			}
			else
			{
				throw new LogicException("û���ҵ���¼��");
			}
		}

        private string GetBankIDName(string bankId)
        {
            string bankIDInfo = TENCENT.OSS.C2C.Finance.BankLib.BankRefundIO.ZWDicClass.GetZWDicValue("TcpayBankid", PublicRes.GetConnString("ZW"));//PublicRes.GetZWDicValue("TcpayBankid");
            if (bankIDInfo == null || bankIDInfo == "")
            {
                return "δ֪��(" + bankId + ")";
            }
            string[] bankInfos = bankIDInfo.Split('|');
            foreach (string oneInfo in bankInfos)
            {

                if (oneInfo.StartsWith(bankId + "="))
                {
                    return oneInfo.Replace(bankId + "=", "") + "(" + bankId + ")";
                }
            }
            return "δ֪��(" + bankId + ")";
        }
	}
}
