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
using System.Web.Services.Protocols;
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.CFT.KF.KF_Web;
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;

namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
	/// <summary>
	/// BankCardUnbind 的摘要说明。
	/// </summary>
	public partial class BankCardUnbind : System.Web.UI.Page
	{
		protected Wuqi.Webdiyer.AspNetPager Aspnetpager1;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				Label1.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();
				//int operid = Int32.Parse(Session["OperID"].ToString());

				//if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID,"InfoCenter")) Response.Redirect("../login.aspx?wh=1");
				if(!ClassLib.ValidateRight("InfoCenter",this)) Response.Redirect("../login.aspx?wh=1");

				this.rbtn_bkt_JJK.CheckedChanged += new EventHandler(rbtns_CheckedChanged);
				this.rbtn_bkt_XYK.CheckedChanged += new EventHandler(rbtns_CheckedChanged);
				this.rbtn_bkt_ALL.CheckedChanged += new EventHandler(rbtns_CheckedChanged);

				this.rbtn_all.CheckedChanged += new EventHandler(rbtns_CheckedChanged);
				this.rbtn_fastPay.CheckedChanged += new EventHandler(rbtns_CheckedChanged);
				this.rbtn_ydt.CheckedChanged += new EventHandler(rbtns_CheckedChanged);

				if(!IsPostBack)
				{
					if (Request.QueryString["type"] != null && Request.QueryString["type"].ToString().Trim() == "edit")
					{
						ShowEdit();
					}

					btnUnbind.Attributes["onClick"] = "if(!confirm('确定要解除绑定吗？')) return false;"; 

					this.rbtn_all.Checked = true;
					this.rbtn_bkt_ALL.Checked = true;

					this.ddl_BankType.Items.Clear();
					this.ddl_BankType.Items.Add(new ListItem("全部",""));
					AddAllBankType();

					this.pager.RecordCount = 1000;
					this.pager.PageSize = 10;

					this.pager1.RecordCount = 1000;
					this.pager1.PageSize = 10;
				}

				this.pager.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(pager_PageChanged);
				this.pager1.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(pager1_PageChanged);
				this.Datagrid1.ItemCommand +=new DataGridCommandEventHandler(Datagrid1_ItemCommand);

				this.ButtonBeginDate.Attributes.Add("onclick","openModeBegin()");
				this.ButtonEndDate.Attributes.Add("onclick","openModeEnd()");
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

		private void ShowEdit()
		{
			this.PanelList.Visible = false;
			this.PanelMod.Visible = true;

			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
			DataSet ds = qs.GetBankCardBind(Request.QueryString["Fuid"].ToString(),Request.QueryString["Findex"].ToString());

			if(ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count != 1)
			{
				throw new Exception("没有查找到相应的记录！");
			}
			else
			{
				DataRow dr = ds.Tables[0].Rows[0];
				this.lblFuin.Text = ds.Tables[0].Rows[0]["Fuin"].ToString();
				string Fbank_type = ds.Tables[0].Rows[0]["Fbank_type"].ToString();

				this.lblFbank_type.Text = classLibrary.getData.GetBankNameFromBankCode(Fbank_type);

				/*
				if(Fbank_type == "0")
					this.lblFbank_type.Text = "未知";
				else if(Fbank_type == "2001")
					this.lblFbank_type.Text = "招行一点通";
				else if(Fbank_type == "2002")
					this.lblFbank_type.Text = "工行一点通";
				else if(Fbank_type == "2003")
					this.lblFbank_type.Text = "建行一点通";
				else if(Fbank_type == "3001")
					this.lblFbank_type.Text = "兴业信用卡";
				else if(Fbank_type == "3002")
					this.lblFbank_type.Text = "中行信用卡";
				else
					this.lblFbank_type.Text = Fbank_type;
				*/

				this.lblFbind_serialno.Text = ds.Tables[0].Rows[0]["Fbind_serialno"].ToString();
				this.lblFprotocol_no.Text = ds.Tables[0].Rows[0]["Fprotocol_no"].ToString();
				string Fbank_status = ds.Tables[0].Rows[0]["Fbank_status"].ToString();

				if(Fbank_status == "0")
					this.lblFbank_status.Text = "未定义";
				else if(Fbank_status == "1")
					this.lblFbank_status.Text = "预绑定状态(未激活)";
				else if(Fbank_status == "2")
					this.lblFbank_status.Text = "绑定确认(正常)";
				else if(Fbank_status == "3")
					this.lblFbank_status.Text = "解除绑定";
				else
					this.lblFbank_status.Text = "Unknown";

				this.lblFcard_tail.Text = ds.Tables[0].Rows[0]["Fcard_tail"].ToString();
				this.lblFtruename.Text = ds.Tables[0].Rows[0]["Ftruename"].ToString();
				string Fbind_type = ds.Tables[0].Rows[0]["Fbind_type"].ToString();

				if(Fbind_type == "0")
					this.lblFbind_type.Text = "未知类型";
				else if(Fbind_type == "1")
					this.lblFbind_type.Text = "普通借记卡关联";
				else if(Fbind_type == "2")
					this.lblFbind_type.Text = "银行联名卡关联";
				else if(Fbind_type == "3")
					this.lblFbind_type.Text = "信用卡关联";
				else if(Fbind_type == "4")
					this.lblFbind_type.Text = "内部绑定";
				else if(Fbind_type == "20")
					this.lblFbank_type.Text = "普通信用卡关联";
				else
					this.lblFbind_type.Text = "Unknown";

				string Fbind_status = ds.Tables[0].Rows[0]["Fbind_status"].ToString();

				if(Fbind_status == "0")
					this.lblFbind_status.Text = "未定义";
				else if(Fbind_status == "1")
					this.lblFbind_status.Text = "初始状态";
				else if(Fbind_status == "2")
					this.lblFbind_status.Text = "开启";
				else if(Fbind_status == "3")
					this.lblFbind_status.Text = "关闭";
				else if(Fbind_status == "4")
					this.lblFbind_status.Text = "解除";
				else if(Fbind_status == "5")
					this.lblFbind_status.Text = "银行已激活，用户未激活";
				else
					this.lblFbind_status.Text = "Unknown";

				string Fbind_flag = ds.Tables[0].Rows[0]["Fbind_flag"].ToString();

				if(Fbind_flag == "0")
					this.lblFbind_flag.Text = "未知";
				else if(Fbind_flag == "1")
					this.lblFbind_flag.Text = "有效";
				else if(Fbind_flag == "2")
					this.lblFbind_flag.Text = "无效";
				else
					this.lblFbind_flag.Text = "Unknown";

				this.lblFbank_id.Text = ds.Tables[0].Rows[0]["Fbank_id"].ToString();
				this.lblFindex.Text = ds.Tables[0].Rows[0]["Findex"].ToString();
				this.lblFuid.Text = ds.Tables[0].Rows[0]["Fuid"].ToString();
				this.txtFmemo.Text = ds.Tables[0].Rows[0]["Fmemo"].ToString();

				try
				{
					this.lblCreID.Text = ds.Tables[0].Rows[0]["Fcre_id"].ToString();
					if(ds.Tables[0].Rows[0]["Fmobilephone"].ToString() != "")
						this.lblPhone.Text = ds.Tables[0].Rows[0]["Fmobilephone"].ToString();
					else
						this.lblPhone.Text = ds.Tables[0].Rows[0]["Ftelephone"].ToString();

					this.lblUid.Text = ds.Tables[0].Rows[0]["Fuid"].ToString();

					switch(ds.Tables[0].Rows[0]["Fcre_type"].ToString())
					{
						case "1":
						{
							this.lblcreType.Text = "身份证";break;
						}
						case "2":
						{
							this.lblcreType.Text = "护照";break;
						}
						case "3":
						{
							this.lblcreType.Text = "军官证";break;
						}
						default:
						{
							this.lblcreType.Text = "未知";break;
						}
					}

					this.lblCreateTime.Text = dr["Fcreate_time"].ToString();
					this.lblbindTimeLocal.Text = dr["Fbind_time_local"].ToString();
					this.lblbindTimeBank.Text = dr["Fbind_time_bank"].ToString();
					this.lblUnbindTimeLocal.Text = dr["Funchain_time_local"].ToString();
					this.lblUnbindTimeBank.Text = dr["Funchain_time_bank"].ToString();
				}
				catch(Exception ex)
				{
					WebUtils.ShowMessage(this,ex.Message);

				}

				if(Fbind_flag == "2" && Fbind_status == "4" && Fbank_status == "3")
					this.btnUnbind.Enabled = false;
				else
					this.btnUnbind.Enabled = true;
			}
		}

		protected void btnUnbind_Click(object sender, System.EventArgs e)
		{
			try
			{
				Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
				qs.ModifyBankCardBind(this.lblFuid.Text,this.lblFindex.Text,this.txtFmemo.Text.Trim());
				this.btnUnbind.Enabled = false;
			}
			catch(Exception ex)
			{
				WebUtils.ShowMessage(this.Page,ex.Message);
			}
		}

		protected void btnCancel_Click(object sender, System.EventArgs e)
		{
			Response.Redirect("BankCardUnbind.aspx");
		}


		private void BindData_UIN(int index)
		{
			this.pager1.CurrentPageIndex = index;
			try
			{
				Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

				DataSet ds = qs.GetBankCardBindList_UIN(this.ddl_BankType.SelectedValue,this.tbx_bankID.Text.Trim(),
					this.ddl_creType.SelectedValue,this.tbx_creID.Text.Trim(),this.tbx_serNum.Text.Trim(),this.tbx_phone.Text.Trim(),
					int.Parse(this.ddl_bindStatue.SelectedValue),this.pager.PageSize * (index-1),this.pager.PageSize);

				if(ds != null)
				{
					ds.Tables[0].Columns.Add("Fbank_typeStr",typeof(string));

					foreach(DataRow dr in ds.Tables[0].Rows)
					{
						dr["Fbank_typeStr"] = classLibrary.getData.GetBankNameFromBankCode(dr["Fbank_type"].ToString());
						dr["fcre_id"] = setConfig.ConvertCreID(dr["fcre_id"].ToString());
						dr["fbank_id"] = setConfig.ConvertCreID(dr["fbank_id"].ToString());
					}

					this.Datagrid1.DataSource = ds;
					this.Datagrid1.DataBind();

					BindData(ds.Tables[0].Rows[0]["fuin"].ToString(),1);
				}
				else
				{
					WebUtils.ShowMessage(this,"查询一点通账户列表为空");
					this.dgList.DataSource = null;
					this.dgList.DataBind();
					return;
				}
			}
			catch (System.Exception ex)
			{
				WebUtils.ShowMessage(this,ex.Message);
			}
		}


		private void BindData(string qqid,int index)
		{
			this.pager.CurrentPageIndex = index;
			try
			{
				Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

				int queryType = 0;
				if(this.rbtn_ydt.Checked)
				{
					queryType = 1;
				}
				else if(this.rbtn_fastPay.Checked)
				{
					queryType = 2;
				}

				string beginDateStr = this.tbx_beginDate.Text.Trim();
				string endDateStr = this.tbx_endDate.Text.Trim();

				try
				{
					if(beginDateStr != "")
						DateTime.Parse(beginDateStr);

					if(endDateStr != "")
						DateTime.Parse(endDateStr);
				}
				catch
				{
					WebUtils.ShowMessage(this.Page,"请输入正确的日期格式");
					return;
				}

				DataSet ds = qs.GetBankCardBindList_2(qqid.Trim(),this.ddl_BankType.SelectedValue,this.tbx_bankID.Text.Trim(),
					this.ddl_creType.SelectedValue,this.tbx_creID.Text.Trim(),this.tbx_serNum.Text.Trim(),
					this.tbx_phone.Text.Trim(),beginDateStr,endDateStr,queryType,
					int.Parse(this.ddl_bindStatue.SelectedValue),(index - 1) * this.pager.PageSize,this.pager.PageSize);

				if(ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
				{
					this.dgList.DataSource = null;
					this.dgList.DataBind();
					//throw new Exception("没有查找到相应的记录！");	
					WebUtils.ShowMessage(this,"查询一点通信息为空");
					return;
				}
				else
				{
					DataTable dt = ds.Tables[0];
					dt.Columns.Add("Fbank_typeStr",typeof(string));
					dt.Columns.Add("Fbank_statusStr",typeof(string));

					foreach(DataRow dr in dt.Rows)
					{
						/*
							if(dr["Fbank_type"].ToString() == "0")
								dr["Fbank_typeStr"] = "未知";
							else if(dr["Fbank_type"].ToString() == "2001")
								dr["Fbank_typeStr"] = "招行一点通";
							else if(dr["Fbank_type"].ToString() == "2002")
								dr["Fbank_typeStr"] = "工行一点通";
							else if(dr["Fbank_type"].ToString() == "2003")
								dr["Fbank_typeStr"] = "建行一点通";
							else if(dr["Fbank_type"].ToString() == "3001")
								dr["Fbank_typeStr"] = "兴业信用卡";
							else if(dr["Fbank_type"].ToString() == "3002")
								dr["Fbank_typeStr"] = "中行信用卡";
							else
								dr["Fbank_typeStr"] = dr["Fbank_type"].ToString();
							*/

						dr["Fbank_typeStr"] = classLibrary.getData.GetBankNameFromBankCode(dr["Fbank_type"].ToString());

						if(dr["Fbank_status"].ToString() == "0")
							dr["Fbank_statusStr"] = "未定义";
						else if(dr["Fbank_status"].ToString() == "1")
							dr["Fbank_statusStr"] = "预绑定状态(未激活)";
						else if(dr["Fbank_status"].ToString() == "2")
							dr["Fbank_statusStr"] = "绑定确认(正常)";
						else if(dr["Fbank_status"].ToString() == "3")
							dr["Fbank_statusStr"] = "解除绑定";
						else
							dr["Fbank_statusStr"] = "Unknown";
					}
					this.dgList.DataSource = dt.DefaultView;
					dgList.DataBind();
				}
			}
			catch(SoapException eSoap) //捕获soap类异常
			{
				string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
				WebUtils.ShowMessage(this.Page,"调用服务出错：" + errStr);
			}
			catch(Exception eSys)
			{
				WebUtils.ShowMessage(this.Page,"读取数据失败！" + eSys.Message.ToString());
			}
		}


		private void BindData(int index)
		{
			this.pager.CurrentPageIndex = index;
			try
			{
				/*
				if(this.txtQQ.Text.Trim() == "" && this.tbx_uid.Text.Trim() == "")
					throw new Exception("请输入财付通账号或内部ID！");
					*/
	
				Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
				//DataSet ds = qs.GetBankCardBindList(this.txtQQ.Text.Trim(),this.ddl_BankType.SelectedValue);

				DataSet ds = null;

				int queryType = 0;
				
				if(this.rbtn_ydt.Checked)
				{
					queryType = 1;
				}
				else if(this.rbtn_fastPay.Checked)
				{
					queryType = 2;
				}

				string beginDateStr = this.tbx_beginDate.Text.Trim();
				string endDateStr = this.tbx_endDate.Text.Trim();

				try
				{
					if(beginDateStr != "")
						DateTime.Parse(beginDateStr);

					if(endDateStr != "")
						DateTime.Parse(endDateStr);
				}
				catch
				{
					WebUtils.ShowMessage(this.Page,"请输入正确的日期格式");
					return;
				}

				ds = qs.GetBankCardBindList_New(this.txtQQ.Text.Trim(),this.ddl_BankType.SelectedValue,this.tbx_bankID.Text.Trim(),
					this.tbx_uid.Text.Trim(),this.ddl_creType.SelectedValue,this.tbx_creID.Text.Trim(),this.tbx_serNum.Text.Trim(),
					this.tbx_phone.Text.Trim(),beginDateStr,endDateStr,queryType,this.cbx_showAbout.Checked,
					int.Parse(this.ddl_bindStatue.SelectedValue),(index - 1) * this.pager.PageSize,this.pager.PageSize);

				if(ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
				{
					throw new Exception("没有查找到相应的记录！");					
				}
				else
				{
					DataTable dt = ds.Tables[0];
					dt.Columns.Add("Fbank_typeStr",typeof(string));
					dt.Columns.Add("Fbank_statusStr",typeof(string));

					foreach(DataRow dr in dt.Rows)
					{
						/*
						if(dr["Fbank_type"].ToString() == "0")
							dr["Fbank_typeStr"] = "未知";
						else if(dr["Fbank_type"].ToString() == "2001")
							dr["Fbank_typeStr"] = "招行一点通";
						else if(dr["Fbank_type"].ToString() == "2002")
							dr["Fbank_typeStr"] = "工行一点通";
						else if(dr["Fbank_type"].ToString() == "2003")
							dr["Fbank_typeStr"] = "建行一点通";
						else if(dr["Fbank_type"].ToString() == "3001")
							dr["Fbank_typeStr"] = "兴业信用卡";
						else if(dr["Fbank_type"].ToString() == "3002")
							dr["Fbank_typeStr"] = "中行信用卡";
						else
							dr["Fbank_typeStr"] = dr["Fbank_type"].ToString();
						*/

						dr["Fbank_typeStr"] = classLibrary.getData.GetBankNameFromBankCode(dr["Fbank_type"].ToString());

						if(dr["Fbank_status"].ToString() == "0")
							dr["Fbank_statusStr"] = "未定义";
						else if(dr["Fbank_status"].ToString() == "1")
							dr["Fbank_statusStr"] = "预绑定状态(未激活)";
						else if(dr["Fbank_status"].ToString() == "2")
							dr["Fbank_statusStr"] = "绑定确认(正常)";
						else if(dr["Fbank_status"].ToString() == "3")
							dr["Fbank_statusStr"] = "解除绑定";
						else
							dr["Fbank_statusStr"] = "Unknown";
					}
					this.dgList.DataSource = dt.DefaultView;
					dgList.DataBind();
				}
			}
			catch(SoapException eSoap) //捕获soap类异常
			{
				string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
				WebUtils.ShowMessage(this.Page,"调用服务出错：" + errStr);
			}
			catch(Exception eSys)
			{
				WebUtils.ShowMessage(this.Page,"读取数据失败！" + eSys.Message.ToString());
			}
		}

		protected void btnSearch_Click(object sender, System.EventArgs e)
		{
			//BindData(1);

			if(this.tbx_uid.Text.Trim() != "" || this.txtQQ.Text.Trim() != "")
			{
				BindData(1);
			}
			else
			{
				BindData_UIN(1);
			}
		}


		private void rbtns_CheckedChanged(object sender, EventArgs e)
		{
			getData.BankClass[] bkInfoList = null;
			this.ddl_BankType.Items.Clear();

			this.ddl_BankType.Items.Add(new ListItem("全部",""));

			if(this.rbtn_fastPay.Checked)
			{
				if(this.rbtn_bkt_JJK.Checked)
				{
					bkInfoList = getData.GetFPBankList(1);
					for(int i=0;i<bkInfoList.Length;i++)
					{
						this.ddl_BankType.Items.Add(new ListItem(bkInfoList[i].bankName,bkInfoList[i].bankValue.ToString()));
					}
				}
				else if(this.rbtn_bkt_XYK.Checked)
				{
					bkInfoList = getData.GetFPBankList(2);
					for(int i=0;i<bkInfoList.Length;i++)
					{
						this.ddl_BankType.Items.Add(new ListItem(bkInfoList[i].bankName,bkInfoList[i].bankValue.ToString()));
					}
				}
				else if(this.rbtn_bkt_ALL.Checked)
				{
					bkInfoList = getData.GetFPBankList(1);
					for(int i=0;i<bkInfoList.Length;i++)
					{
						this.ddl_BankType.Items.Add(new ListItem(bkInfoList[i].bankName,bkInfoList[i].bankValue.ToString()));
					}

					bkInfoList = getData.GetFPBankList(2);
					for(int i=0;i<bkInfoList.Length;i++)
					{
						this.ddl_BankType.Items.Add(new ListItem(bkInfoList[i].bankName,bkInfoList[i].bankValue.ToString()));
					}
				}
			}
			else if(this.rbtn_ydt.Checked)
			{
				if(this.rbtn_bkt_JJK.Checked)
				{
					bkInfoList = getData.GetOPBankList(1);
					for(int i=0;i<bkInfoList.Length;i++)
					{
						this.ddl_BankType.Items.Add(new ListItem(bkInfoList[i].bankName,bkInfoList[i].bankValue.ToString()));
					}
				}
				else if(this.rbtn_bkt_XYK.Checked)
				{
					bkInfoList = getData.GetOPBankList(2);
					for(int i=0;i<bkInfoList.Length;i++)
					{
						this.ddl_BankType.Items.Add(new ListItem(bkInfoList[i].bankName,bkInfoList[i].bankValue.ToString()));
					}
				}
				else if(this.rbtn_bkt_ALL.Checked)
				{
					bkInfoList = getData.GetOPBankList(1);
					for(int i=0;i<bkInfoList.Length;i++)
					{
						this.ddl_BankType.Items.Add(new ListItem(bkInfoList[i].bankName,bkInfoList[i].bankValue.ToString()));
					}

					bkInfoList = getData.GetOPBankList(2);
					for(int i=0;i<bkInfoList.Length;i++)
					{
						this.ddl_BankType.Items.Add(new ListItem(bkInfoList[i].bankName,bkInfoList[i].bankValue.ToString()));
					}
				}
			}
			else if(this.rbtn_all.Checked)
			{
				if(this.rbtn_bkt_JJK.Checked)
				{
					bkInfoList = getData.GetOPBankList(1);
					for(int i=0;i<bkInfoList.Length;i++)
					{
						this.ddl_BankType.Items.Add(new ListItem(bkInfoList[i].bankName,bkInfoList[i].bankValue.ToString()));
					}

					bkInfoList = getData.GetFPBankList(1);
					for(int i=0;i<bkInfoList.Length;i++)
					{
						this.ddl_BankType.Items.Add(new ListItem(bkInfoList[i].bankName,bkInfoList[i].bankValue.ToString()));
					}
				}
				else if(this.rbtn_bkt_XYK.Checked)
				{
					bkInfoList = getData.GetOPBankList(2);
					for(int i=0;i<bkInfoList.Length;i++)
					{
						this.ddl_BankType.Items.Add(new ListItem(bkInfoList[i].bankName,bkInfoList[i].bankValue.ToString()));
					}

					bkInfoList = getData.GetFPBankList(2);
					for(int i=0;i<bkInfoList.Length;i++)
					{
						this.ddl_BankType.Items.Add(new ListItem(bkInfoList[i].bankName,bkInfoList[i].bankValue.ToString()));
					}
				} 
				else if(this.rbtn_bkt_ALL.Checked)
				{
					AddAllBankType();
				}
			}
		}



		private void AddAllBankType()
		{
			getData.BankClass[] bkInfoList = null;

			bkInfoList = getData.GetOPBankList(1);
			for(int i=0;i<bkInfoList.Length;i++)
			{
				this.ddl_BankType.Items.Add(new ListItem(bkInfoList[i].bankName,bkInfoList[i].bankValue.ToString()));
			}

			bkInfoList = getData.GetOPBankList(2);
			for(int i=0;i<bkInfoList.Length;i++)
			{
				this.ddl_BankType.Items.Add(new ListItem(bkInfoList[i].bankName,bkInfoList[i].bankValue.ToString()));
			}

			bkInfoList = getData.GetFPBankList(1);
			for(int i=0;i<bkInfoList.Length;i++)
			{
				this.ddl_BankType.Items.Add(new ListItem(bkInfoList[i].bankName,bkInfoList[i].bankValue.ToString()));
			}

			bkInfoList = getData.GetFPBankList(2);
			for(int i=0;i<bkInfoList.Length;i++)
			{
				this.ddl_BankType.Items.Add(new ListItem(bkInfoList[i].bankName,bkInfoList[i].bankValue.ToString()));
			}
		}

		private void pager_PageChanged(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
		{
			//this.pager.CurrentPageIndex = e.NewPageIndex;

			//this.BindData(e.NewPageIndex);

			string qqid = this.dgList.Items[0].Cells[2].Text.Trim();

			this.BindData(qqid,e.NewPageIndex);
		}

		private void pager1_PageChanged(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
		{
			this.BindData_UIN(e.NewPageIndex);
		}

		private void Datagrid1_ItemCommand(object source, DataGridCommandEventArgs e)
		{
			if(e.CommandName == "query")
			{
				this.BindData(e.Item.Cells[0].Text.Trim(),1);
			}
		}
	}
}
