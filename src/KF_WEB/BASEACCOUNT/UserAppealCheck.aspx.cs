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
using TENCENT.OSS.CFT.KF.Common;
using System.Web.Services.Protocols;
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;


namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
	/// <summary>
	/// UserAppealCheck 的摘要说明。
	/// </summary>
	public partial class UserAppealCheck : System.Web.UI.Page
	{
		protected Control.UserAppealCheckControl UserAppealCheckControl0;
		protected Control.UserAppealCheckControl UserAppealCheckControl1;
		protected Control.UserAppealCheckControl UserAppealCheckControl2;
		protected Control.UserAppealCheckControl UserAppealCheckControl3;
		protected Control.UserAppealCheckControl UserAppealCheckControl4;
		protected Control.UserAppealCheckControl UserAppealCheckControl5;
		protected Control.UserAppealCheckControl UserAppealCheckControl6;
		protected Control.UserAppealCheckControl UserAppealCheckControl7;
		protected Control.UserAppealCheckControl UserAppealCheckControl8;
		protected Control.UserAppealCheckControl UserAppealCheckControl9;
		protected Control.UserAppealCheckControl UserAppealCheckControl10;
		protected Control.UserAppealCheckControl UserAppealCheckControl11;
		protected Control.UserAppealCheckControl UserAppealCheckControl12;
		protected Control.UserAppealCheckControl UserAppealCheckControl13;
		protected Control.UserAppealCheckControl UserAppealCheckControl14;
		protected Control.UserAppealCheckControl UserAppealCheckControl15;
		protected Control.UserAppealCheckControl UserAppealCheckControl16;
		protected Control.UserAppealCheckControl UserAppealCheckControl17;
		protected Control.UserAppealCheckControl UserAppealCheckControl18;
		protected Control.UserAppealCheckControl UserAppealCheckControl19;
		protected Control.UserAppealCheckControl UserAppealCheckControl20;
		protected Control.UserAppealCheckControl UserAppealCheckControl21;
		protected Control.UserAppealCheckControl UserAppealCheckControl22;
		protected Control.UserAppealCheckControl UserAppealCheckControl23;
		protected Control.UserAppealCheckControl UserAppealCheckControl24;
		protected Control.UserAppealCheckControl UserAppealCheckControl25;
		protected Control.UserAppealCheckControl UserAppealCheckControl26;
		protected Control.UserAppealCheckControl UserAppealCheckControl27;
		protected Control.UserAppealCheckControl UserAppealCheckControl28;
		protected Control.UserAppealCheckControl UserAppealCheckControl29;
		protected Control.UserAppealCheckControl UserAppealCheckControl30;
		protected Control.UserAppealCheckControl UserAppealCheckControl31;
		protected Control.UserAppealCheckControl UserAppealCheckControl32;
		protected Control.UserAppealCheckControl UserAppealCheckControl33;
		protected Control.UserAppealCheckControl UserAppealCheckControl34;
		protected Control.UserAppealCheckControl UserAppealCheckControl35;
		protected Control.UserAppealCheckControl UserAppealCheckControl36;
		protected Control.UserAppealCheckControl UserAppealCheckControl37;
		protected Control.UserAppealCheckControl UserAppealCheckControl38;
		protected Control.UserAppealCheckControl UserAppealCheckControl39;
		protected Control.UserAppealCheckControl UserAppealCheckControl40;
		protected Control.UserAppealCheckControl UserAppealCheckControl41;
		protected Control.UserAppealCheckControl UserAppealCheckControl42;
		protected Control.UserAppealCheckControl UserAppealCheckControl43;
		protected Control.UserAppealCheckControl UserAppealCheckControl44;
		protected Control.UserAppealCheckControl UserAppealCheckControl45;
		protected Control.UserAppealCheckControl UserAppealCheckControl46;
		protected Control.UserAppealCheckControl UserAppealCheckControl47;
		protected Control.UserAppealCheckControl UserAppealCheckControl48;
		protected Control.UserAppealCheckControl UserAppealCheckControl49;
		Query_Service.Query_Service qs = new Query_Service.Query_Service();
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				string szkey = Session["SzKey"].ToString();
				//int operid = Int32.Parse(Session["OperID"].ToString());

				//if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID,"InfoCenter")) Response.Redirect("../login.aspx?wh=1");

				if(!classLibrary.ClassLib.ValidateRight("InfoCenter",this)) Response.Redirect("../login.aspx?wh=1");
			}
			catch
			{
				Response.Redirect("../login.aspx?wh=1");
			}

			if(!IsPostBack)
			{
				ViewState["BeginDate"] = Convert.ToDateTime(Request["BeginDate"].ToString());
				ViewState["EndDate"] = Convert.ToDateTime(Request["EndDate"].ToString());
				ViewState["fstate"] = Request["fstate"].ToString();
				ViewState["ftype"] = Request["ftype"].ToString();
				ViewState["qqtype"] = Request["qqtype"].ToString();
				this.txtCount.Text = Request["Count"].ToString();
				ViewState["uid"] = Session["uid"].ToString();
                ViewState["SortType"] = Request["SortType"].ToString();
                ViewState["dotype"] = Request["dotype"].ToString();
				BindData();
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

		private void BindData()
		{
			string msg = "";
			int TicketsCount;

			try
			{
				try
				{
					TicketsCount = int.Parse(this.txtCount.Text.Trim());
				}
				catch
				{
					throw new Exception("请输入正确的工单数！");
				}
				if(TicketsCount < 1)
				{
					throw new Exception("工单数不允许小于1！");
				}
				if(TicketsCount > 50)
				{
					throw new Exception("工单数的最大值为50！");
				}

				Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
				DateTime beginDate = Convert.ToDateTime(ViewState["BeginDate"]);
				DateTime endDate = Convert.ToDateTime(ViewState["EndDate"]);
				string fstate = ViewState["fstate"].ToString();
				string ftype = ViewState["ftype"].ToString();
				string qqtype = ViewState["qqtype"].ToString();
				string uid = ViewState["uid"].ToString();
                int SortType = Int32.Parse(ViewState["SortType"].ToString());//排序
                string dotype=ViewState["dotype"].ToString();
                DataSet ds = new DataSet();
                if (ftype == "1" || ftype == "5" || ftype == "6" || ftype == "99")//三种类型在原来基础上进行分库分表
                {
                    DataSet dsNew = qs.GetUserAppealLockListDBTB(beginDate, endDate, fstate, ftype, qqtype, uid, TicketsCount, SortType, dotype);//返回所有符合条件的单

                    if (dsNew != null && dsNew.Tables.Count > 0 && dsNew.Tables[0].Rows.Count > 0)
                    {
                        //排序
                        DataTable dt = dsNew.Tables[0];
                        DataView view = dt.DefaultView;
                        if (SortType == 0)
                            view.Sort = "FSubmitTime asc";
                        if (SortType == 1)
                            view.Sort = "FSubmitTime desc";
                        dt = view.ToTable();
                        dt = PublicRes.GetPagedTable(dt, 1, TicketsCount);//利用分页函数取得想要的单数
                        ds.Tables.Add(dt);
                    }

                    //20140108 lxl  返回需要的数据信息，比较数据太大系统反应不过来，优化项
                    ds = qs.GetUserAppealLockListDBTBInnrFun(ds);
                    //领单
                    ds = qs.GetUserAppealLockListDBTB2(ds, uid);
                }
                else
                {
                    ds = qs.GetUserAppealLockList(beginDate, endDate, fstate, ftype, qqtype, uid, TicketsCount, SortType, dotype);
                }

				if(ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count ==0)
				{
					throw new Exception("没有单可领!");
				}
				
				for(int i=0;i<50;i++) //最大为50笔
				{
					System.Web.UI.Control ucl = FindControl("UserAppealCheckControl" + i.ToString());
					if(ucl != null)
					{
						((Control.UserAppealCheckControl)ucl).Clean();
						((Control.UserAppealCheckControl)ucl).Visible = false;
					}
				}

				int TicketCount = ds.Tables[0].Rows.Count;

				for(int i=0;i<TicketCount;i++)
				{
					try
					{
						System.Web.UI.Control ucl = FindControl("UserAppealCheckControl" + i.ToString());
						if(ucl != null)
						{
							((Control.UserAppealCheckControl)ucl)._dr = ds.Tables[0].Rows[i];
							((Control.UserAppealCheckControl)ucl).BindData();
							((Control.UserAppealCheckControl)ucl).Visible = true;
						}
					}
					catch(Exception ex)
					{
						msg += ds.Tables[0].Rows[i]["Fuin"].ToString() + ex.Message;
					}
					continue;
				}

				if(msg != "")
					WebUtils.ShowMessage(this.Page,"批量领单失败:" + msg);
			}
			catch(Exception ex)
			{
				WebUtils.ShowMessage(this.Page,"批量领单失败:" + ex.Message);
			}
		}

        private void Pass(Control.UserAppealCheckControl Control, string UserName, string UserIP, out string msg)
        {
            msg = "";
            string db = Control.db;
            string tb = Control.tb;
            if (db == "" && tb == "")//原表数据
                qs.CFTConfirmAppeal(int.Parse(Control.fid), Control.Comment, UserName, UserIP, out msg);
            else//分库表数据
                qs.CFTConfirmAppealDBTB(Control.fid, Control.db, Control.tb, Control.Comment, UserName, UserIP, out msg);
        }

		private void ShowMsg(string msg)
		{
			Response.Write("<script language=javascript>alert('" + msg + "')</script>");
		}

        private void Cancel(Control.UserAppealCheckControl Control, string UserName, string UserIP, out string msg)
        {
            msg = "";
            string reason = "", OtherReason = "";
            try
            {
                Control.GetRejectReason(out reason, out OtherReason);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return;
            }

            /*
            for(int i=0; i<Control.cblRejectReason.Items.Count; i++)
            {
                if(i == 5)
                    OtherReason = classLibrary.setConfig.replaceMStr(Control.tbOtherReason);
                else if(Control.cblRejectReason.Items[i].Selected)
                {
                    reason += Control.cblRejectReason.Items[i].Text + "&";
                }
            }
            */
            string db = Control.db;
            string tb = Control.tb;
            if (db == "" && tb == "")//原表数据
                qs.CFTCancelAppeal(int.Parse(Control.fid), reason, OtherReason, Control.Comment, UserName, UserIP, out msg);
            else//分库表数据
                qs.CFTCancelAppealDBTB(Control.fid, Control.db, Control.tb, reason, OtherReason, Control.Comment, UserName, UserIP, out msg);
        }

        private void Delete(Control.UserAppealCheckControl Control, string UserName, string UserIP, out string msg)
        {
            msg = "";
            string db = Control.db;
            string tb = Control.tb;
            if (db == "" && tb == "")//原表数据
                qs.CFTDelAppeal(int.Parse(Control.fid), Control.Comment, UserName, UserIP, out msg);
            else//分库表数据
                qs.CFTDelAppealDBTB(Control.fid, Control.db, Control.tb, Control.Comment, UserName, UserIP, out msg);
        }

		protected void btnSubmit_Click(object sender, System.EventArgs e)
		{
			//"1":通过"2":拒绝"3":删除"4":挂起
			string msg = "";
			string Error = "";

			string UserIP = Request.UserHostAddress;
			string UserName = ViewState["uid"].ToString();

			for(int i=0;i<50;i++) //一次处理50笔
			{
				System.Web.UI.Control ucl = FindControl("UserAppealCheckControl" + i.ToString());

				try
				{
					if(ucl != null)
					{
						if(((Control.UserAppealCheckControl)ucl).Visible && ((Control.UserAppealCheckControl)ucl).fid != "")
						{
							msg = "";

							if(((Control.UserAppealCheckControl)ucl).SubmitType == "1")
							{
								Pass(((Control.UserAppealCheckControl)ucl),UserName,UserIP,out msg);
							}
							else if(((Control.UserAppealCheckControl)ucl).SubmitType == "2")
							{
								Cancel(((Control.UserAppealCheckControl)ucl),UserName,UserIP,out msg);
							}
							else if(((Control.UserAppealCheckControl)ucl).SubmitType == "3")
							{
								Delete(((Control.UserAppealCheckControl)ucl),UserName,UserIP,out msg);
							}
							if(msg != "")
							{
                                //lxl 20131111 打印出库表信息
                                Error += ((Control.UserAppealCheckControl)ucl).db + " " + ((Control.UserAppealCheckControl)ucl).tb + " " + ((Control.UserAppealCheckControl)ucl).fid + " :" + msg;
							}
						}
					}
				}
				catch(Exception ex)
				{
                    //lxl 20131111 打印出库表信息
                    Error += ((Control.UserAppealCheckControl)ucl).db + " " + ((Control.UserAppealCheckControl)ucl).tb + " " + ((Control.UserAppealCheckControl)ucl).fid + " :" + ex.Message;
				}
				continue;
			}

			if(Error != "")
			{
				WebUtils.ShowMessage(this.Page,Error);
			}
		}

		protected void btnTicket_Click(object sender, System.EventArgs e)
		{
			 BindData();
		}


	}
}
