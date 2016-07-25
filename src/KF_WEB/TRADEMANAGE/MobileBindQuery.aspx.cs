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
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using System.Web.Services.Protocols;
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;
using TENCENT.OSS.CFT.KF.Common;

namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
	/// <summary>
	/// MobileBindQuery ��ժҪ˵����
	/// </summary>
	public partial class MobileBindQuery : System.Web.UI.Page
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				string szkey = Session["SzKey"].ToString();
				int operid = Int32.Parse(Session["OperID"].ToString());
				if(!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter",this)) Response.Redirect("../login.aspx?wh=1");
			}
			catch
			{
				Response.Redirect("../login.aspx?wh=1");
			} 

		}

		private void BindData(int index)
		{
			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

			DataSet ds = qs.GetMsgNotify(ViewState["QQ"].ToString());
            bool isRight = TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("SensitiveRole", this);
			if(ds != null && ds.Tables.Count >0 && ds.Tables[0].Rows.Count > 0)
			{
				ds.Tables[0].Columns.Add("MobileState",typeof(string));
				ds.Tables[0].Columns.Add("MsgState",typeof(string));
				ds.Tables[0].Columns.Add("MobilePayState",typeof(string));
				ds.Tables[0].Columns.Add("Unbind",typeof(string));

				foreach(DataRow dr in ds.Tables[0].Rows)
				{
					try
					{
                        //���ֻ��������������Ϣ����
                        string fmobile = classLibrary.setConfig.ConvertTelephoneNumber(dr["fmobile"].ToString(), isRight);
                        dr["fmobile"]=fmobile;
						/*ת��Ϊ2����(0Ϊδ��ͨ,1Ϊ��ͨ)����7λǰ�油0,��������һλ��ʼ
						  1.�Ƿ�ͨ��������
						  2.�Ƿ��email
						  3.�Ƿ��qq
						  4.�Ƿ񼤻�
						  5.��̬��֤��(����)
						  6.�Ƿ�ͨ�ֻ�֧��
						  7.�Ƿ���ֻ�
						 */
						string Fstatus = Convert.ToString(Convert.ToInt32(dr["Fstatus"]),2);
						if(Fstatus.Length < 31)
						{
							Fstatus = Fstatus.PadLeft(31,'0');
						}
						if(Fstatus.Length != 31)
						{
							throw new Exception("��¼״̬�����쳣");
						}
						if(Fstatus.Substring(30,1).ToString() == "0")
						{
							dr["MsgState"] = "δ��ͨ";
						}
						else
						{
							dr["MsgState"] = "��ͨ";
						}
						if(Fstatus.Substring(25,1).ToString() == "0")
						{
							dr["MobilePayState"] = "δ��ͨ";
						}
						else
						{
							dr["MobilePayState"] = "��ͨ";
						}
						if(Fstatus.Substring(24,1).ToString() == "0")
						{
							dr["MobileState"] = "δ��";
						}
						else
						{
							dr["MobileState"] = "��";
						}
						if(Fstatus.Substring(30,1).ToString() == "0" && Fstatus.Substring(25,1).ToString() == "0" && Fstatus.Substring(24,1).ToString() == "0")
						{
							dr["Unbind"] = "";
						}
						else
						{
							if(ClassLib.ValidateRight("DeleteCrt",this))
								dr["Unbind"] = "���";
							else
								dr["Unbind"] = "";
						}
					}
					catch
					{
						dr["MsgState"] = "Unknown";
						dr["MobilePayState"] = "Unknown";
						dr["MobileState"] = "Unknown";
						dr["Unbind"] = "";
					}
				}

				DataGrid1.DataSource = ds.Tables[0].DefaultView;
				DataGrid1.DataBind();
			}
			else
			{
				WebUtils.ShowMessage(this.Page,"û���ҵ���¼");
			}
		}


		protected void btnQuery_Click(object sender, System.EventArgs e)
		{
			try
			{
				ViewState["QQ"] = this.txbQQ.Text.Trim();
				BindData(1);
			}
			catch(SoapException eSoap) //����soap���쳣
			{
				string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
				WebUtils.ShowMessage(this.Page,"���÷������" + errStr);
			}
			catch(Exception eSys)
			{
				WebUtils.ShowMessage(this.Page,"��ȡ����ʧ�ܣ�" + classLibrary.setConfig.replaceMStr(eSys.Message));
			}
		}

        public void btnPhoneNumberQuery(object sender, System.EventArgs e)
        {
            try
            {
                BindPhoneData();
            }
            catch (SoapException eSoap) //����soap���쳣
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "���÷������" + errStr);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "��ȡ����ʧ�ܣ�" + classLibrary.setConfig.replaceMStr(eSys.Message));
            }
        }

        void BindPhoneData()
        {
            Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            var qqid = qs.GetMsgNotifyByPhoneNumber(this.txtPhoneNumber.Text.Trim());
            if (string.IsNullOrEmpty(qqid))
            {
                WebUtils.ShowMessage(this.Page, "û���ҵ���¼");
            }
            ViewState["QQ"] = qqid;
            BindData(1);
        }

		public void DataGrid1_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
		{
			if(e.CommandName == "Select")
			{
				try
				{
					Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
					qs.UnbindMsgNotify(e.Item.Cells[0].Text.Trim());
					BindData(1);
					WebUtils.ShowMessage(this.Page,"���ɹ�");
				}
				catch(Exception ex)
				{
					WebUtils.ShowMessage(this.Page,ex.Message);
				}
			}
		}

		protected void btnUpdateBindInfo_Click(object sender, System.EventArgs e)
		{
			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
			try
			{
				if(!qs.UpDateBindInfo(this.txbQQ.Text.Trim()))
				{
					WebUtils.ShowMessage(this,"����ʧ��");
					return;
				}

				WebUtils.ShowMessage(this,"���³ɹ�");
			}
			catch(Exception ex)
			{
				WebUtils.ShowMessage(this,"����ʧ��" + ex.Message);
			}
		}
	}
}
