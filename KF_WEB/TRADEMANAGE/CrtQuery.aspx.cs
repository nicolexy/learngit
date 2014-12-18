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
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using System.Web.Services.Protocols;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.CFT.KF.DataAccess;
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;

namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
	/// <summary>
	/// CrtQuery 的摘要说明。
	/// </summary>
	public partial class CrtQuery : System.Web.UI.Page
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// 在此处放置用户代码以初始化页面
			try
			{
				Label1.Text = Session["uid"].ToString();

				string szkey = Session["SzKey"].ToString();
				int operid = Int32.Parse(Session["OperID"].ToString());
				
				//if (AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "InfoCenter") || AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "DeleteCrt"))
				if(ClassLib.ValidateRight("InfoCenter",this) || ClassLib.ValidateRight("DeleteCrt",this))
				{
					//if(AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "DeleteCrt"))
					if(ClassLib.ValidateRight("DeleteCrt",this))
					{
						this.btnDelete.Enabled = true;
						btnDelete.Attributes["onClick"] = "if(!confirm('确定要删除该用户的证书吗？')) return false;";
                        this.btnDeleteService.Enabled = true;
                        btnDeleteService.Attributes["onClick"] = "if(!confirm('确定要关闭该用户的证书服务吗？')) return false;";
					}
					else
					{
						this.btnDelete.Enabled = false;
                        this.btnDeleteService.Enabled = false;
					}
				}
				else
				{
					Response.Redirect("../login.aspx?wh=1");
				}
			}
			catch
			{
				Response.Redirect("../login.aspx?wh=1");
			}
		}

		#region Web 窗体设计器生成的代码
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: 该调用是 ASP.NET Web 窗体设计器所必需的。
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// 设计器支持所需的方法 - 不要使用代码编辑器修改
		/// 此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{    

		}
		#endregion

		protected void btnQuery_Click(object sender, System.EventArgs e)
		{
			try
			{
                if (this.tbQQID.Text.Trim() == "")
                {
                    WebUtils.ShowMessage(this.Page, "请输入用户账户！");
                    return;
                }

				//开始查询
				Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
				DataSet ds = qs.GetUserCrtList(this.tbQQID.Text.Trim());

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
				{
                    ds.Tables[0].Columns.Add("Fmodify_time2", typeof(System.String));
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (dr["Fstate"].ToString()=="4")
                        {
                            dr["Fmodify_time2"] = dr["Fmodify_time"].ToString();
                        }
                    }
                    this.Table2.Visible = true;
                    this.TableDelete.Visible = false;
					DataGrid1.DataSource = ds.Tables[0].DefaultView;
					DataGrid1.DataBind();
				}
				else
				{
                    this.Table2.Visible = false;
					WebUtils.ShowMessage(this.Page,"没有找到合适的记录！");
				}
			}
			catch
			{
                this.Table2.Visible = false;
				WebUtils.ShowMessage(this.Page,"查询出错");
			}
		}

		protected void btnDelete_Click(object sender, System.EventArgs e)
		{
			try
			{
                if (this.tbQQID.Text.Trim() == "")
                {
                    WebUtils.ShowMessage(this.Page, "请输入用户账户！");
                    return;
                }
				string strszkey = Session["SzKey"].ToString().Trim();
				int ioperid = Int32.Parse(Session["OperID"].ToString());
				int iserviceid = Common.AllUserRight.GetServiceID("DeleteCrt") ;
				string struserdata = Session["uid"].ToString().Trim();
 
				string content = struserdata + "执行了[删除用户证书]操作,操作对象[" + this.tbQQID.Text.Trim()
					+ "]时间:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

				Common.AllUserRight.UpdateSession(strszkey,ioperid,PublicRes.GROUPID,iserviceid,struserdata,content);

				string log = SensitivePowerOperaLib.MakeLog("edit",Session["uid"].ToString().Trim(),"[删除用户证书]",
					this.tbQQID.Text.Trim());

				if(!SensitivePowerOperaLib.WriteOperationRecord("DeleteCrt",log,this))
				{
					
				}

				Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

//				Finance_Header fh = new Finance_Header();
//				fh.UserIP = Request.UserHostAddress;
//				fh.UserName = Session["uid"].ToString();
//				fh.OperID = Int32.Parse(Session["OperID"].ToString());
//				fh.SzKey = Session["SzKey"].ToString();
//				qs.Finance_HeaderValue = fh;
				Query_Service.Finance_Header fh = classLibrary.setConfig.setFH(this);
				qs.Finance_HeaderValue = fh;

				qs.DeleteUserCrt(this.tbQQID.Text.Trim());
 
			}
			catch(Exception ex)
			{
				WebUtils.ShowMessage(this.Page,ex.Message);
			}
		
		}
        protected void btnDeleteQuery_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (this.tbQQID.Text.Trim() == "")
                {
                    WebUtils.ShowMessage(this.Page, "请输入用户账户！");
                    return;
                }
                //开始查询
                Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
                DataSet ds = qs.GetDeleteQueryInfo(this.tbQQID.Text.Trim());

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                   DataRow dr = ds.Tables[0].Rows[0];
                   this.lbDeletetime.Text = dr["Fmodify_time"].ToString();
                   this.lbDeleteIP.Text = dr["DeleteIP"].ToString();
                   this.Table2.Visible = false;
                   this.TableDelete.Visible = true;
                }
                else
                {
                    this.TableDelete.Visible = false;
                    WebUtils.ShowMessage(this.Page, "没有找到关闭服务的记录！");
                }
            }
            catch
            {
                this.TableDelete.Visible = false;
                WebUtils.ShowMessage(this.Page, "查询出错");
            }

        }
        protected void btnDeleteService_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (this.tbQQID.Text.Trim() == "")
                {
                    WebUtils.ShowMessage(this.Page, "请输入用户账户！");
                    return;
                }
                string strszkey = Session["SzKey"].ToString().Trim();
                int ioperid = Int32.Parse(Session["OperID"].ToString());
                int iserviceid = Common.AllUserRight.GetServiceID("DeleteCrt");
                string struserdata = Session["uid"].ToString().Trim();

                string content = struserdata + "执行了[关闭证书服务]操作,操作对象[" + this.tbQQID.Text.Trim()
                    + "]时间:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                Common.AllUserRight.UpdateSession(strszkey, ioperid, PublicRes.GROUPID, iserviceid, struserdata, content);

                string log = SensitivePowerOperaLib.MakeLog("edit", Session["uid"].ToString().Trim(), "[关闭证书服务]",
                    this.tbQQID.Text.Trim());

                if (!SensitivePowerOperaLib.WriteOperationRecord("DeleteCrt", log, this))
                {

                }

                Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

                //				Finance_Header fh = new Finance_Header();
                //				fh.UserIP = Request.UserHostAddress;
                //				fh.UserName = Session["uid"].ToString();
                //				fh.OperID = Int32.Parse(Session["OperID"].ToString());
                //				fh.SzKey = Session["SzKey"].ToString();
                //				qs.Finance_HeaderValue = fh;
                //Query_Service.Finance_Header fh = classLibrary.setConfig.setFH(this);
                //qs.Finance_HeaderValue = fh;
                qs.Finance_HeaderValue = classLibrary.setConfig.setFH(this);
                qs.DeleteCrtService(this.tbQQID.Text.Trim());
                WebUtils.ShowMessage(this.Page, "关闭证书服务成功");
            }
            catch (SoapException eSoap) //捕获soap类异常
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "调用服务出错：" + errStr);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "关闭证书服务失败！" + eSys.Message.ToString());
            }
        }
	}
}
