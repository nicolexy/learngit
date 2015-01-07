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
using CFT.CSOMS.COMMLIB;
using System.Configuration;
using CFT.CSOMS.BLL.FreezeModule;
using System.Text;
using System.Net;
using System.Xml;
using System.IO;

namespace TENCENT.OSS.CFT.KF.KF_Web.FreezeManage
{
	/// <summary>
    /// FreezeNewQuery 的摘要说明。
	/// </summary>
    public partial class FreezeNewQuery : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.TextBox tbx_listno1;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// 在此处放置用户代码以初始化页面

			if(!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter",this)) Response.Redirect("../login.aspx?wh=1");

			this.btnBeginDate.Attributes.Add("onclick","openModeBegin()");
			this.btnEndDate.Attributes.Add("onclick","openModeEnd()");

			if(!IsPostBack)
			{
                DateTime dt = DateTime.Now.AddDays(-30);
                DateTime grayDate = DateTime.Parse("2014-06-05");
                if (dt.CompareTo(grayDate) < 0)
                {
                    this.tbx_beginDate.Text = grayDate.ToString("yyyy-MM-dd 00:00:00");
                }
                else 
                {
                    this.tbx_beginDate.Text = dt.ToString("yyyy-MM-dd 00:00:00");
                }

                this.tbx_endDate.Text = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
			}

			//this.pager.RecordCount = this.GetRecordCount();
            pager.PageSize = 10;
			pager.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(pager_PageChanged);

			//this.DataGrid_QueryResult.ItemCommand += new DataGridCommandEventHandler(DataGrid_QueryResult_ItemCommand);
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
			this.DataGrid_QueryResult.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.DataGrid_QueryResult_ItemDataBound);

		}
		#endregion




		private void BindData(int index)
		{
			this.pager.CurrentPageIndex = index;

			DateTime beginDate;
			DateTime endDate;

			try
			{
                //TENCENT.OSS.C2C.Finance.Common.CommLib.CommMailSend.SendMsgQQTips("75942086", "102236", "url=http://action.tenpay.com/cuifei/2014/fengkong/unfreeze_suc.shtml");
                //userid=75942086
                //TENCENT.OSS.C2C.Finance.Common.CommLib.CommMailSend.SendMessage("13528403205", "102237", "user=75942086");
                //return;
                #region 查询条件处理
                string uin = "";
                if (!string.IsNullOrEmpty(this.tbx_payAccount.Text))
                {
                    uin = getQQID();
                }

                beginDate = DateTime.Parse(this.tbx_beginDate.Text);
				endDate = DateTime.Parse(this.tbx_endDate.Text);

				if(beginDate.CompareTo(endDate) > 0)
				{
                    ShowMsg("开始日期大于结束日期");
                    return;
				}
				if(beginDate.AddMonths(1).AddDays(1) < endDate)
				{
                    ShowMsg("日期间隔大于一个月，请重新输入！");
                    return;
				}

                //string freezeGrayDate = System.Configuration.ConfigurationManager.AppSettings["FreezeGrayDate"];
                string freezeGrayDate = "2014-06-05";
                DateTime grayDate = DateTime.Parse(freezeGrayDate);
                if (beginDate.CompareTo(grayDate) < 0) 
                {
                    ShowMsg("只能查询" + freezeGrayDate + "之后的记录！之前的记录请到[风控解冻审核]查询。");
                    return;
                }

                //获取申诉状态
                int state = 99;
                int ftype = int.Parse(this.ddlType.SelectedValue);
                if (ftype == 8 || ftype == 19)
                {
                    this.ddl_orderState.Visible = true;
                    this.ddl_orderStateSpecial.Visible = false;
                    state = int.Parse(this.ddl_orderState.SelectedValue);
                }
                else if (ftype == 11)
                {
                    this.ddl_orderState.Visible =false;
                    this.ddl_orderStateSpecial.Visible = true;
                    state = int.Parse(this.ddl_orderStateSpecial.SelectedValue);
                }



                #endregion

                DataSet ds = null;  //结果集
                Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
                Query_Service.Finance_Header fh = classLibrary.setConfig.setFH(this);
                qs.Finance_HeaderValue = fh;

                int allRecordCount = 0;

                //ds = qs.GetFreezeList_New(uin, beginDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"), 
                //    int.Parse(this.ddl_orderState.SelectedValue), this.tbx_listNo.Text.Trim(), this.tbx_people.Text.Trim(), this.tbx_reason.Text.Trim(),
                //    (index - 1) * this.pager.PageSize, this.pager.PageSize, this.ddl_queryOrderType.SelectedValue, out allRecordCount);

                ds = qs.GetFreezeList_New(uin, beginDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"), int.Parse(this.ddlType.SelectedValue),
                  state, this.tbx_listNo.Text.Trim(), this.tbx_people.Text.Trim(), this.tbx_reason.Text.Trim(),
                  (index - 1) * this.pager.PageSize, this.pager.PageSize, this.ddl_queryOrderType.SelectedValue, out allRecordCount);

                
				this.lb_count.Text = allRecordCount.ToString();

				this.pager.RecordCount = allRecordCount;

				if(ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
				{
					ShowMsg("查询结果为空");
					this.DataGrid_QueryResult.DataSource = null;
					this.DataGrid_QueryResult.DataBind();
					return;
				}

				ds.Tables[0].Columns.Add("OpUrl",typeof(string));
				ds.Tables[0].Columns.Add("DiaryUrl",typeof(string));
				ds.Tables[0].Columns.Add("handleStateName",typeof(string));
				ds.Tables[0].Columns.Add("handleUserName",typeof(string));

				for(int i=0;i<ds.Tables[0].Rows.Count;i++)
				{
					DataRow dr = ds.Tables[0].Rows[i];

                    if (!(dr["FsubmitTime"] is DBNull))
                    {
                        if (dr["FsubmitTime"].ToString() != "") 
                        {
                            DateTime subTime = DateTime.Parse(dr["FsubmitTime"].ToString());
                            dr["OpUrl"] = @"FreezeProcessDetail.aspx?fid=" + dr["FID"].ToString() + "&ffreeze_id=" + dr["FUin"].ToString() + "&fsubmit_date=" + dr["FsubmitTime"].ToString();
                        }
                    }
                    
					dr["DiaryUrl"] = @"FreezeDiary.aspx?FFreezeListID="+ dr["FID"].ToString() + "&ffreeze_id=" + dr["FUin"].ToString();
                    dr["handleUserName"] = dr["FCheckUser"].ToString();

                    #region 状态处理
                       string stateName = "{0}";
                    if (ftype == 8 || ftype == 19)//冻结类型状态处理
                    {
                        if (dr["isFreezeListHas"].ToString() == "0")
                        {
                            stateName = "该账户不处于冻结状态 ({0})";
                        }

                        switch (dr["Fstate"].ToString())
                        {
                            case "0":
                                {
                                    stateName = string.Format(stateName, "未处理");
                                    //dr["handleStateName"] = stateName;
                                    //dr["handleUserName"] = dr["FCheckUser"].ToString();
                                    break;
                                }
                            case "1":
                                {
                                    stateName = string.Format(stateName, "结单(已解冻)");
                                    //dr["handleStateName"] = stateName;
                                    //dr["handleUserName"] =  dr["FCheckUser"].ToString();
                                    break;
                                }
                            case "2":
                                {
                                    stateName = string.Format(stateName, "待补充资料");
                                    //dr["handleStateName"] = "结单（未解冻）";
                                    //dr["handleUserName"] =  dr["FCheckUser"].ToString();
                                    break;
                                }
                            case "7":
                                {
                                    stateName = string.Format(stateName, "已作废");
                                    //dr["handleStateName"] = "已作废";
                                    //dr["handleUserName"] = dr["FCheckUser"].ToString();
                                    break;
                                }
                            case "8":
                                {
                                    stateName = string.Format(stateName, "挂起");
                                    //dr["handleStateName"] = "挂起";
                                    //dr["handleUserName"] = dr["FCheckUser"].ToString();
                                    break;
                                }
                            case "10":
                                {
                                    stateName = string.Format(stateName, "已补充资料");
                                    //dr["handleStateName"] = "挂起";
                                    //dr["handleUserName"] = dr["FCheckUser"].ToString();
                                    break;
                                }
                            /*
                            case "5":
                            {
                                dr["handleStateName"] = "补充处理结果";
                                dr["handleUserName"] = dr["FCheckUser"].ToString();
                                break;
                            }
                            */
                            default:
                                {
                                    stateName = string.Format(stateName, "未知" + dr["Fstate"].ToString());
                                    //stateName += "(未知" + dr["Fstate"].ToString() + ")";
                                    //dr["handleStateName"] = "未知" + dr["Fstate"].ToString();
                                    //dr["handleUserName"] = "未知";
                                    break;
                                }
                        }

                        dr["handleStateName"] = stateName;
                    }
                    else if (ftype == 11)//特殊找回支付密码状态
                    {
                        switch (dr["Fstate"].ToString())
                        {
                            case "0":
                                {
                                    stateName = string.Format(stateName, "未处理"); break;
                                }
                            case "1":
                                {
                                    stateName = string.Format(stateName, "申诉成功"); break;
                                }
                            case "2":
                                {
                                    stateName = string.Format(stateName, "申诉失败"); break;
                                }
                            case "7":
                                {
                                    stateName = string.Format(stateName, "已删除"); break;
                                }
                            case "11":
                                {
                                    stateName = string.Format(stateName, "待补充资料"); break;
                                }
                            case "12":
                                {
                                    stateName = string.Format(stateName, "已补充资料"); break;
                                }
                            default:
                                {
                                    stateName = string.Format(stateName, "未知" + dr["Fstate"].ToString());
                                    break;
                                }
                        }

                        this.DataGrid_QueryResult.Columns[this.DataGrid_QueryResult.Columns.Count - 7].Visible = false;//隐藏冻结原因列
                    }
                    else
                    {
                        stateName = string.Format(stateName, "申诉类型不对：" + dr["Fstate"].ToString());
                    }
                    #endregion
					
				}

				ds.AcceptChanges();

				this.DataGrid_QueryResult.DataSource = ds;
				this.DataGrid_QueryResult.DataBind();
			}
			catch(Exception ex)
			{
                ShowMsg(ex.Message);
                return;
			}
		}

		protected void btn_query_Click(object sender, System.EventArgs e)
		{
			BindData(1);
		}

        string getQQID()
        {
            if (string.IsNullOrEmpty(this.tbx_payAccount.Text))
            {
                return "";
            }
            var id = this.tbx_payAccount.Text.Trim();
            if (this.WeChatCft.Checked)
            {
                return id;
            }
            else if (this.WeChatUid.Checked)
            {
                var qs = new Query_Service.Query_Service();
                return qs.Uid2QQ(id);
            }
            else if (this.WeChatQQ.Checked || this.WeChatMobile.Checked || this.WeChatEmail.Checked)
            {
                string queryType = string.Empty;
                if (this.WeChatQQ.Checked)
                {
                    queryType = "QQ";
                }
                else if (this.WeChatMobile.Checked)
                {
                    queryType = "Mobile";
                }
                else if (this.WeChatEmail.Checked)
                {
                    queryType = "Email";
                }

                string openID = string.Empty, errorMessage = string.Empty;
                int errorCode = 0;
                var IPList = ConfigurationManager.AppSettings["WeChat"].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                for (int j = 0; j < IPList.Length; j++)
                {
                    if (getOpenIDFromWeChat(queryType, id, out openID, out errorCode, out errorMessage, IPList[j]))
                    {
                        break;
                    }
                }
                if (errorCode == 0)
                {
                    return openID + "@wx.tenpay.com";
                }
                else if (errorCode == 1)
                {
                    throw new Exception("没有此用户");
                }
                else
                {
                    throw new Exception(errorCode + errorMessage);
                }
            }
            else if (this.WeChatId.Checked)
            {
                return WeChatHelper.GetUINFromWeChatName(id);
            }

            return id;
        }

        //通过微信绑定的QQ、手机或邮箱信息查询其openID，对应的财付通账号便是openID@wx.tenpay.com
        bool getOpenIDFromWeChat(string queryType, string ID, out string openID, out int errorCode, out string errorMessage, string IP)
        {
            openID = errorMessage = string.Empty;
            errorCode = 0;
            try
            {
                string parameterString = "<Request>{0}<AppId>wx482cac0d58846383</AppId></Request>";
                string IDstring = string.Empty;
                string API;
                if (queryType == "QQ")
                {
                    IDstring = string.Format("<QQ>{0}</QQ>", ID);
                    API = "ConvertQQToOuterAcctId";
                }
                else if (queryType == "Mobile")
                {
                    IDstring = string.Format("<Mobile>{0}</Mobile>", ID);
                    API = "ConvertMobileToOuterAcctId";
                }
                else if (queryType == "Email")
                {
                    IDstring = string.Format("<Email>{0}</Email>", ID);
                    API = "ConvertEmailToOuterAcctId";
                }
                else
                {
                    errorCode = -1;
                    errorMessage = "查询类型不正确";
                    return false;
                }
                parameterString = string.Format(parameterString, IDstring);
                var data = Encoding.Default.GetBytes(parameterString);
                var request = (HttpWebRequest)WebRequest.Create(string.Format("http://{0}:12137/cgi-bin/{1}?f=xml&appname=wx_tenpay", IP, API));
                request.Method = "POST";
                request.ContentType = "text/xml;charset=UTF-8";
                var parameter = request.GetRequestStream();
                parameter.Write(data, 0, data.Length);
                var response = (HttpWebResponse)request.GetResponse();
                var myResponseStream = response.GetResponseStream();
                var myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                var resultXml = new XmlDocument();
                resultXml.LoadXml(myStreamReader.ReadToEnd());
                myStreamReader.Close();
                myResponseStream.Close();
                var responseNode = resultXml.SelectSingleNode("Response");
                errorCode = Convert.ToInt32(responseNode.SelectSingleNode("error").SelectSingleNode("code").InnerText);
                errorMessage = responseNode.SelectSingleNode("error").SelectSingleNode("message").InnerText;
                openID = responseNode.SelectSingleNode("result").SelectSingleNode("OuterAcctId").InnerText;
                return true;
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
                return false;
            }
        }

		private int GetRecordCount()
		{
			// 不采用查询数据库来查询页面数了，直接采用10000
			return 10000;
		}


		private void ShowMsg(string szMsg)
		{
			Response.Write("<script language=javascript>alert('" + szMsg + "')</script>");
		}

		private void pager_PageChanged(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
		{
			int newPageIndex = e.NewPageIndex;
			this.pager.CurrentPageIndex = newPageIndex;

			BindData(newPageIndex);
		}


		/*
		private void BindData_ForDiary(int index)
		{
			Query_Service.Query_Service qs = new Query_Service.Query_Service();

			string tdeid = ViewState["FFreezeListID"].ToString();

			DataSet ds =  qs.GetFreezeDiary("",tdeid,"","","","","","",0,1);

			if(ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
			{
				WebUtils.ShowMessage(this,"该记录的处理日志为空");
				this.Datagrid1.DataSource = null;
				this.Datagrid1.DataBind();
				return;
			}

			ds.Tables[0].Columns.Add("DiaryHandleResult",typeof(string));

			foreach(DataRow dr in ds.Tables[0].Rows)
			{
				dr["DiaryHandleResult"] = dr["FCreateDate"].ToString() + "  " + dr["FHandleUser"].ToString() 
					+ " 执行了 " + ConvertHandleTypeToString(dr["FHandleType"].ToString()) + " 操作，结果为：" + dr["FHandleResult"].ToString();
			}

			this.Datagrid1.DataSource = ds;
			this.Datagrid1.DataBind();
		}
		*/


		private string ConvertHandleTypeToString(string type)
		{
			switch(type)
			{
				case "1":
				{
					return "结单(未解冻)";
				}
				case "2":
				{
					return "结单(已解冻)";
				}
				case "3":
				{
					return "作废";
				}
				case "10":
				{
					return "挂起";
				}
				case "100":
				{
					return "补充处理结果";
				}
				default:
				{
					return "未知操作" + type;
				}
			}
		}

		private void DataGrid_QueryResult_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			if(e.Item.Cells[8].Text == "BIGMONEY")
			{
				e.Item.Cells[0].ForeColor = Color.Red;
				e.Item.Cells[1].ForeColor = Color.Red;
			}
		}

        protected void ddlType_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            int ftype = int.Parse(this.ddlType.SelectedValue);
            if (ftype == 8 || ftype == 19)
            {
                this.ddl_orderState.Visible = true;
                this.ddl_orderStateSpecial.Visible = false;
            }
            else if (ftype == 11)
            {
                this.ddl_orderState.Visible = false;
                this.ddl_orderStateSpecial.Visible = true;
            }
        }

	}
}
