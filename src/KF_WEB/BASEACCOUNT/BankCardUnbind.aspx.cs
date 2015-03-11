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
using BankCardBindService = CFT.CSOMS.BLL.BankCardBindModule.BankCardBindService;   //避免CFT命名空间相同
using System.Collections.Generic;   

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

            DataSet ds = new BankCardBindService().GetBankCardBindDetail(Request.QueryString["Fuid"].ToString(), Request.QueryString["Findex"].ToString(), Request.QueryString["FBDIndex"].ToString());

            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count != 1)
            {
                WebUtils.ShowMessage(this, "没有查找到相应的记录");
                return;
            }
            else
            {
                //支付限额lxl   
                ds.Tables[0].Columns.Add("Fonce_quota_str", typeof(String));
                ds.Tables[0].Columns.Add("Fday_quota_str", typeof(String));
                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "Fonce_quota", "Fonce_quota_str");
                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "Fday_quota", "Fday_quota_str");
                string Fbind_flag = ds.Tables[0].Rows[0]["Fbind_flag"].ToString();
                string Fbind_status = ds.Tables[0].Rows[0]["Fbind_status"].ToString();
                string Fbank_status = ds.Tables[0].Rows[0]["Fbank_status"].ToString();

                this.lblFuin.Text = ds.Tables[0].Rows[0]["Fuin"].ToString();
                this.lblFbank_type.Text = GetBankType(ds.Tables[0].Rows[0]["Fbank_type"].ToString());
                this.lblFbind_serialno.Text = ds.Tables[0].Rows[0]["Fbind_serialno"].ToString();
                this.lblFprotocol_no.Text = ds.Tables[0].Rows[0]["Fprotocol_no"].ToString();
                this.lblFbank_status.Text = ds.Tables[0].Rows[0]["bank_status_str"].ToString();

                this.lblFcard_tail.Text = ds.Tables[0].Rows[0]["Fcard_tail"].ToString();
                this.lblFcard_tail_db.Text = ds.Tables[0].Rows[0]["Fcard_tail"].ToString();
                this.lblFtruename.Text = ds.Tables[0].Rows[0]["Ftruename"].ToString();
                this.lblFbind_type.Text = ds.Tables[0].Rows[0]["bind_type_str"].ToString();
                this.lblFbind_flag.Text = ds.Tables[0].Rows[0]["bind_flag_str"].ToString();
                
                this.lblFbank_id.Text = ds.Tables[0].Rows[0]["Fbank_id"].ToString();
                this.lblFbind_status.Text = ds.Tables[0].Rows[0]["bind_status_str"].ToString();
                this.lblFindex.Text = ds.Tables[0].Rows[0]["Findex"].ToString();
                this.lblFuid.Text = ds.Tables[0].Rows[0]["Fuid"].ToString();
                this.lblFbankType.Text = ds.Tables[0].Rows[0]["Fbank_type"].ToString();
                
                this.lblcreType.Text = ds.Tables[0].Rows[0]["cre_type_str"].ToString();
                this.lblCreID.Text = classLibrary.setConfig.ConvertCreID(ds.Tables[0].Rows[0]["Fcre_id"].ToString());

                if (ds.Tables[0].Rows[0]["Ftelephone"].ToString() != "")
                { this.lblPhone.Text = ds.Tables[0].Rows[0]["Ftelephone"].ToString(); }
                else
                { this.lblPhone.Text = ds.Tables[0].Rows[0]["Fmobilephone"].ToString(); }

                this.lblUid.Text = ds.Tables[0].Rows[0]["Fuid"].ToString();
                this.lblCreateTime.Text = ds.Tables[0].Rows[0]["Fcreate_time"].ToString();
                this.lblbindTimeLocal.Text = ds.Tables[0].Rows[0]["Fbind_time_local"].ToString();
                this.lblbindTimeBank.Text = ds.Tables[0].Rows[0]["Fbind_time_bank"].ToString();
                this.lblUnbindTimeLocal.Text = ds.Tables[0].Rows[0]["Funchain_time_local"].ToString();

                this.lblUnbindTimeBank.Text = ds.Tables[0].Rows[0]["Funchain_time_bank"].ToString();
                this.lblonce_quota.Text = ds.Tables[0].Rows[0]["Fonce_quota_str"].ToString();
                this.lblday_quota.Text = ds.Tables[0].Rows[0]["Fday_quota_str"].ToString();
                this.lbli_character2.Text = ds.Tables[0].Rows[0]["sms_flag_str"].ToString();
                this.txtFmemo.Text = ds.Tables[0].Rows[0]["Fmemo"].ToString();

                if (Fbind_flag == "2" && Fbind_status == "4" && Fbank_status == "3")
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
                qs.Finance_HeaderValue = classLibrary.setConfig.setFH(this);
           
                //20130809 如果为邮储一点通、建行一点通、工行一点通，它们的协议号存在了bankID字段，不能走统一的解绑方式
                //解绑时调用绑定服务解绑,使用uid 卡尾号
                //使用卡尾号，参数有限长，若超过4位，则使用uid  Fbind_serialno解绑

                //20140425 lxl 问题：存在一个用户一个银行卡绑定了两次的情况，查询出两条绑定记录，但是解绑不了
                //需要用特殊解绑方式的uid  Fbind_serialno来解绑
                //因此要将“调用特殊服务解绑”适用于所有银行，不能解绑的情况下就用该种解绑。
                //本次将UnbindBankCardSpecial接口做调整，适用需求
                if (CheckBoxUnbind.Checked == true)
                {                   
                    DataTable dt = new BankCardBindService().UnBindBankCardBindSpecial(this.lblFbankType.Text, this.lblFuin.Text, this.lblFcard_tail_db.Text,
                        this.lblFbind_serialno.Text, this.lblFprotocol_no.Text);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        string res = dt.Rows[0]["ret_value"].ToString().ToLower();
                        if (res != null && res == "true")
                        {
                            WebUtils.ShowMessage(this.Page, "调用特殊服务解绑成功");
                        }
                    }              
                }
                else
                {
                    //2013/7/18 yinhuang 原来是修改表来解绑，现在使用接口来解绑
                    DataTable dt = new BankCardBindService().UnbindBankCardBind(this.lblFbankType.Text, this.lblFuin.Text, this.lblFprotocol_no.Text, "");
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        string res = dt.Rows[0]["ret_value"].ToString().ToLower();
                        if (res != null && res == "true")
                        {
                            WebUtils.ShowMessage(this.Page, "解绑成功");
                        }
                    }                   
                }
				
				this.btnUnbind.Enabled = false;
			}
            catch (SoapException eSoap) //捕获soap类异常
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message);
                WebUtils.ShowMessage(this.Page, "调用服务出错：" + errStr);
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

        //同步
        protected void btnSynchron_Click(object sender, System.EventArgs e) 
        {
            try 
            {       
                DataTable dt = new BankCardBindService().SyncBankCardBind(this.lblFbankType.Text, this.lblFcard_tail_db.Text, this.lblFbank_id.Text);
                if (dt != null && dt.Rows.Count > 0)
                {
                    string res = dt.Rows[0]["ret_vaule"].ToString().ToLower();
                    if (res != null && res == "true")
                    {
                        WebUtils.ShowMessage(this.Page, "同步成功");
                    }
                }
            }
            catch (SoapException eSoap) //捕获soap类异常
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message);
                WebUtils.ShowMessage(this.Page, "调用服务出错：" + errStr);
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this.Page, ex.Message);
            }
        }

		private void BindData_UIN(int index)
		{
			this.pager1.CurrentPageIndex = index;
			try
			{

                DataSet ds = new BankCardBindService().GetBankCardBindRelationList(this.ddl_BankType.SelectedValue, this.tbx_bankID.Text.Trim(),
                    this.ddl_creType.SelectedValue, this.tbx_creID.Text.Trim(), this.tbx_serNum.Text.Trim(), this.tbx_phone.Text.Trim(),
                    int.Parse(this.ddl_bindStatue.SelectedValue), this.pager1.PageSize * (index - 1), this.pager1.PageSize);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    WebUtils.ShowMessage(this, "查询一点通账户列表为空");
                    this.dgList.DataSource = null;
                    this.dgList.DataBind();
                    return;
                }
                else
                {
                    DataTable dt = ds.Tables[0];
                    dt.Columns.Add("fuin", typeof(string));
                    dt.Columns.Add("Fbank_typeStr", typeof(string));
                    dt.Columns.Add("fcre_id", typeof(string));
                    dt.Columns.Add("fbank_id", typeof(string));
                    dt.Columns.Add("fcard_tail", typeof(string));

                    //重命名，绑定
                    foreach (DataRow dr in dt.Rows)
                    { 
                        dr["fuin"]=dr["uin"];
                        dr["Fbank_typeStr"]= dr["bank_type"];
                        dr["fcre_id"]=dr["cre_id"];
                        dr["fbank_id"]=dr["bank_id"];
                        dr["fcard_tail"]=dr["card_tail"];
                    }

                    this.Datagrid1.DataSource = ds;
                    this.Datagrid1.DataBind();

                    BindData(ds.Tables[0].Rows[0]["fuin"].ToString(), 1);
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

				DataSet ds =  new BankCardBindService().GetBankCardBindList(qqid.Trim(),this.ddl_BankType.SelectedValue,this.tbx_bankID.Text.Trim(),"",
					this.ddl_creType.SelectedValue,this.tbx_creID.Text.Trim(),this.tbx_serNum.Text.Trim(),
					this.tbx_phone.Text.Trim(),beginDateStr,endDateStr,queryType,true,
					int.Parse(this.ddl_bindStatue.SelectedValue),"",(index - 1) * this.pager.PageSize,this.pager.PageSize);

				if(ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
				{
					this.dgList.DataSource = null;
					this.dgList.DataBind();
					WebUtils.ShowMessage(this,"查询一点通信息为空");
					return;
				}
				else
				{
					DataTable dt = ds.Tables[0];
					dt.Columns.Add("Fbank_typeStr",typeof(string));
					dt.Columns.Add("Fbank_statusStr",typeof(string));
                    dt.Columns.Add("Fxyzf_typeStr", typeof(string)); //信用支付类型

					foreach(DataRow dr in dt.Rows)
					{
                        dr["Fbank_typeStr"] = GetBankType(dr["Fbank_type"].ToString());
                        dr["Fbank_statusStr"] = dr["bank_status_str"];
                        dr["Fxyzf_typeStr"] = dr["xyzf_type_str"];		
					}
					this.dgList.DataSource = dt.DefaultView;
					dgList.DataBind();
                    Session["qqid"] = this.dgList.Items[0].Cells[3].Text.Trim();
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

                DataSet ds = new BankCardBindService().GetBankCardBindList(this.txtQQ.Text.Trim(), this.ddl_BankType.SelectedValue, this.tbx_bankID.Text.Trim(),
                    this.tbx_uid.Text.Trim(), this.ddl_creType.SelectedValue, this.tbx_creID.Text.Trim(), this.tbx_serNum.Text.Trim(),
                    this.tbx_phone.Text.Trim(), beginDateStr, endDateStr, queryType, this.cbx_showAbout.Checked,
                    int.Parse(this.ddl_bindStatue.SelectedValue), "", (index - 1) * this.pager.PageSize, this.pager.PageSize);

				if(ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
				{
					throw new Exception("没有查找到相应的记录！");					
				}
				else
				{
					DataTable dt = ds.Tables[0];
                    //重命名绑定
                    dt.Columns.Add("Fbank_typeStr", typeof(string));
					dt.Columns.Add("Fbank_statusStr",typeof(string));
                    dt.Columns.Add("Fxyzf_typeStr", typeof(string)); //信用支付类型

					foreach(DataRow dr in dt.Rows)
					{                  
                        dr["Fbank_typeStr"] = GetBankType(dr["Fbank_type"].ToString());
                        dr["Fbank_statusStr"] = dr["bank_status_str"];
                        dr["Fxyzf_typeStr"] = dr["xyzf_type_str"];			
					}

					this.dgList.DataSource = dt.DefaultView;
					dgList.DataBind();
                    Session["qqid"] = this.dgList.Items[0].Cells[3].Text.Trim();
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
			if(this.tbx_uid.Text.Trim() != "" || this.txtQQ.Text.Trim() != "")
			{
				BindData(1);
			}
			else
			{
				BindData_UIN(1);
			}
		}

        private string GetBankType(string typeId)
        {
            DataSet infos = new BankCardBindService().GetBankDic();
            if (infos != null || infos.Tables.Count > 0 || infos.Tables[0].Rows.Count > 0)
            {
                DataTable dt = infos.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["Fvalue"].ToString() == typeId)
                    {
                        return dr["Fmemo"].ToString();
                    }
                }
            }
            return "未知" + typeId;
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
            string qqid = Session["qqid"].ToString();

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
