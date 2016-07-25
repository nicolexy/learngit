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
	/// MobileBindQuery 的摘要说明。
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
                        //对手机号码进行敏感信息处理
                        string fmobile = classLibrary.setConfig.ConvertTelephoneNumber(dr["fmobile"].ToString(), isRight);
                        dr["fmobile"]=fmobile;
						/*转化为2进制(0为未开通,1为开通)不足7位前面补0,排序从最后一位开始
						  1.是否开通短信提醒
						  2.是否绑定email
						  3.是否绑定qq
						  4.是否激活
						  5.动态验证玛(废弃)
						  6.是否开通手机支付
						  7.是否绑定手机
						 */
						string Fstatus = Convert.ToString(Convert.ToInt32(dr["Fstatus"]),2);
						if(Fstatus.Length < 31)
						{
							Fstatus = Fstatus.PadLeft(31,'0');
						}
						if(Fstatus.Length != 31)
						{
							throw new Exception("记录状态数据异常");
						}
						if(Fstatus.Substring(30,1).ToString() == "0")
						{
							dr["MsgState"] = "未开通";
						}
						else
						{
							dr["MsgState"] = "开通";
						}
						if(Fstatus.Substring(25,1).ToString() == "0")
						{
							dr["MobilePayState"] = "未开通";
						}
						else
						{
							dr["MobilePayState"] = "开通";
						}
						if(Fstatus.Substring(24,1).ToString() == "0")
						{
							dr["MobileState"] = "未绑定";
						}
						else
						{
							dr["MobileState"] = "绑定";
						}
						if(Fstatus.Substring(30,1).ToString() == "0" && Fstatus.Substring(25,1).ToString() == "0" && Fstatus.Substring(24,1).ToString() == "0")
						{
							dr["Unbind"] = "";
						}
						else
						{
							if(ClassLib.ValidateRight("DeleteCrt",this))
								dr["Unbind"] = "解绑";
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
				WebUtils.ShowMessage(this.Page,"没有找到记录");
			}
		}


		protected void btnQuery_Click(object sender, System.EventArgs e)
		{
			try
			{
				ViewState["QQ"] = this.txbQQ.Text.Trim();
				BindData(1);
			}
			catch(SoapException eSoap) //捕获soap类异常
			{
				string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
				WebUtils.ShowMessage(this.Page,"调用服务出错：" + errStr);
			}
			catch(Exception eSys)
			{
				WebUtils.ShowMessage(this.Page,"读取数据失败！" + classLibrary.setConfig.replaceMStr(eSys.Message));
			}
		}

        public void btnPhoneNumberQuery(object sender, System.EventArgs e)
        {
            try
            {
                BindPhoneData();
            }
            catch (SoapException eSoap) //捕获soap类异常
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "调用服务出错：" + errStr);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + classLibrary.setConfig.replaceMStr(eSys.Message));
            }
        }

        void BindPhoneData()
        {
            Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            var qqid = qs.GetMsgNotifyByPhoneNumber(this.txtPhoneNumber.Text.Trim());
            if (string.IsNullOrEmpty(qqid))
            {
                WebUtils.ShowMessage(this.Page, "没有找到记录");
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
					WebUtils.ShowMessage(this.Page,"解绑成功");
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
					WebUtils.ShowMessage(this,"更新失败");
					return;
				}

				WebUtils.ShowMessage(this,"更新成功");
			}
			catch(Exception ex)
			{
				WebUtils.ShowMessage(this,"更新失败" + ex.Message);
			}
		}
	}
}
