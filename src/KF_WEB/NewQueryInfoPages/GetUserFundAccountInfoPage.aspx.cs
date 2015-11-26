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
using TENCENT.OSS.CFT.KF.Common;
using Tencent.DotNet.Common.UI;

using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using System.Web.Services.Protocols;
using CFT.CSOMS.BLL.CFTAccountModule;

namespace TENCENT.OSS.CFT.KF.KF_Web.NewQueryInfoPages
{
	/// <summary>
	/// GetUserFundAccountInfoPage 的摘要说明。
	/// </summary>
	public partial class GetUserFundAccountInfoPage : System.Web.UI.Page
	{
		protected Wuqi.Webdiyer.AspNetPager pager;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.dd_queryType.SelectedIndexChanged += new EventHandler(dd_queryType_SelectedIndexChanged);
            this.DataGrid1.ItemCommand += new DataGridCommandEventHandler(DataGrid1_ItemCommand);
			this.btn_serach.Click += new EventHandler(btn_serach_Click);
		}

		private void dd_queryType_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.tbx_param2.Text = "";
			this.tbx_param1.Text = "";
			switch(this.dd_queryType.SelectedIndex)
			{
				case 0:
				{
					this.lb_param2.Visible = false;
					this.tbx_param2.Visible = false;
					this.lb_param1.Text = "基金交易账户ID：";
					this.tbx_param1.Visible = true;
					this.dd_creType.Visible = false;
					break;
				}
				case 1:
				{
					this.lb_param1.Text = "证件类型：";
					this.lb_param2.Text = "证件号码：";
					this.lb_param2.Visible = true;
					this.tbx_param1.Visible = false;
					this.dd_creType.Visible = true;
					this.tbx_param2.Visible = true;
					break;
				}
				case 2:
				{
					this.lb_param2.Visible = false;
					this.tbx_param2.Visible = false;
					this.lb_param1.Text = "财付通帐号：";
					this.tbx_param1.Visible = true;
					this.dd_creType.Visible = false;
					break;
				}
				case 3:
				{
					this.lb_param2.Visible = false;
					this.tbx_param2.Visible = false;
					this.lb_param1.Text = "财付通ID：";
					this.tbx_param1.Visible = true;
					this.dd_creType.Visible = false;
					break;
				}
                case 4:
                {
                    this.lb_param2.Visible = false;
                    this.tbx_param2.Visible = false;
                    this.lb_param1.Text = "理财通账号：";
                    this.tbx_param1.Visible = true;
                    this.dd_creType.Visible = false;
                    break;
                }
			}	
		}


		private void ClearDetail()
		{
			this.lb_c1.Text = "";
			this.lb_c2.Text = "";
			this.lb_c3.Text = "";
			this.lb_c5.Text = "";
			this.lb_c6.Text = "";
			this.lb_c8.Text = "";
			this.lb_c9.Text = "";
			this.lb_c10.Text = "";

			this.lb_c7.Text = "";
			this.lb_c4.Text = "";

			this.DataGrid_QueryResult.DataSource = null;
			this.DataGrid_QueryResult.DataBind();
		}



		private void BindDetail(DataSet ds)
		{
			this.lb_c1.Text = ds.Tables[0].Rows[0]["Ftrade_id"].ToString();
			this.lb_c2.Text = ds.Tables[0].Rows[0]["Fuid"].ToString();
			this.lb_c3.Text = ds.Tables[0].Rows[0]["Ftrue_name"].ToString();

            this.lb_c5.Text = ds.Tables[0].Rows[0]["Fcre_type_name"].ToString();
            this.lb_c6.Text = ds.Tables[0].Rows[0]["Fcre_id_hide"].ToString();
			this.lb_c8.Text = ds.Tables[0].Rows[0]["Fcreate_time"].ToString();
			this.lb_c9.Text = ds.Tables[0].Rows[0]["Fmodify_time"].ToString();
			this.lb_c10.Text = ds.Tables[0].Rows[0]["Fmobile"].ToString();

			this.lb_c7.Text = ds.Tables[0].Rows[0]["FlstateName"].ToString();
			this.lb_c4.Text = ds.Tables[0].Rows[0]["FstateName"].ToString();

			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

			qs.Finance_HeaderValue = classLibrary.setConfig.setFH(this);

            //20130827 lxl
            //查询账户余额，与个人账户信息中的余额一样查询，但是币种的类型为基金：Fcurtype=2
            //使用账号查询
            string qqid = qs.Uid2QQ(ds.Tables[0].Rows[0]["Fuid"].ToString());
            DataSet ds1 = null;
            if(qqid!=null)
                ds1 = new AccountService().GetUserAccount(qqid, 2, 1, 1);
            else
                ds1 = new AccountService().GetUserAccountCancel(ds.Tables[0].Rows[0]["Fuid"].ToString(), 2, 1, 1);//已注销的账户查不出对应的QQ号
            if (ds1 == null || ds1.Tables.Count < 1 || ds1.Tables[0].Rows.Count < 1)
            {
                this.lb_c15.Text = "";
            }
            else
            {
                string s_balance = PublicRes.objectToString(ds1.Tables[0], "Fbalance");
                long l_balance = 0;
                if (s_balance != "")
                {
                    l_balance = long.Parse(s_balance);
                }
                this.lb_c15.Text = classLibrary.setConfig.FenToYuan(l_balance);
            }

			DataSet ds2 = qs.GetUserFundCardBind(this.lb_c2.Text,"","");

			if(ds2 == null || ds2.Tables.Count == 0 || ds2.Tables[0].Rows.Count == 0)
			{
				WebUtils.ShowMessage(this,"查询用户的银行卡信息结果为空");
				this.DataGrid_QueryResult.DataSource = null;
				this.DataGrid_QueryResult.DataBind();
				return;
			}

			ds2.Tables[0].Columns.Add("Fbank_typeName",typeof(string));

			foreach(DataRow dr in ds2.Tables[0].Rows)
			{
				dr["Fbank_typeName"] = getData.GetBankNameFromBankCode(dr["Fbank_type"].ToString());
			}

			this.DataGrid_QueryResult.DataSource = ds2;
			this.DataGrid_QueryResult.DataBind();
		}

        private void BindDetail(int index)
        {
            this.lb_c1.Text = DataGrid1.Items[index].Cells[0].Text;
            this.lb_c2.Text = DataGrid1.Items[index].Cells[1].Text;

            this.lb_c3.Text = DataGrid1.Items[index].Cells[2].Text;
            this.lb_c4.Text = DataGrid1.Items[index].Cells[3].Text;
            this.lb_c5.Text = DataGrid1.Items[index].Cells[4].Text;

            this.lb_c6.Text = DataGrid1.Items[index].Cells[5].Text;
            this.lb_c7.Text = DataGrid1.Items[index].Cells[6].Text;


            this.lb_c8.Text = DataGrid1.Items[index].Cells[7].Text;

            this.lb_c9.Text = DataGrid1.Items[index].Cells[8].Text;
            this.lb_c10.Text =DataGrid1.Items[index].Cells[9].Text;

            Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

            qs.Finance_HeaderValue = classLibrary.setConfig.setFH(this);

            string qqid = qs.Uid2QQ(this.lb_c2.Text);
            DataSet ds1 = null;
            if (qqid != null)
                ds1 = new AccountService().GetUserAccount(qqid, 2, 1, 1);
            else
                ds1 = new AccountService().GetUserAccountCancel(this.lb_c2.Text, 2, 1, 1);//已注销的账户查不出对应的QQ号
            if (ds1 == null || ds1.Tables.Count < 1 || ds1.Tables[0].Rows.Count < 1)
            {
                this.lb_c15.Text = "";
            }
            else
            {
                string s_balance = PublicRes.objectToString(ds1.Tables[0], "Fbalance");
                long l_balance = 0;
                if (s_balance != "")
                {
                    l_balance = long.Parse(s_balance);
                }
                this.lb_c15.Text = classLibrary.setConfig.FenToYuan(l_balance);
            }

            DataSet ds2 = qs.GetUserFundCardBind(this.lb_c2.Text, "", "");

            if (ds2 == null || ds2.Tables.Count == 0 || ds2.Tables[0].Rows.Count == 0)
            {
                WebUtils.ShowMessage(this, "查询用户的银行卡信息结果为空");
                this.DataGrid_QueryResult.DataSource = null;
                this.DataGrid_QueryResult.DataBind();
                return;
            }

            ds2.Tables[0].Columns.Add("Fbank_typeName", typeof(string));

            foreach (DataRow dr in ds2.Tables[0].Rows)
            {
                dr["Fbank_typeName"] = getData.GetBankNameFromBankCode(dr["Fbank_type"].ToString());
            }

            this.DataGrid_QueryResult.DataSource = ds2;
            this.DataGrid_QueryResult.DataBind();
           
        }

		private void btn_serach_Click(object sender, EventArgs e)
		{
			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

			//qs.Finance_HeaderValue = setConfig.setFH(Session["OperID"].ToString(),Request.UserHostAddress);

			qs.Finance_HeaderValue = classLibrary.setConfig.setFH(this);

			DataSet ds = null;

			try
			{
				// 采用证件验证
				if(this.dd_queryType.SelectedIndex == 1)
				{
					if(this.tbx_param2.Text.Trim() == "")
					{
						WebUtils.ShowMessage(this,"请输入证件号码");
						return;
					}

					ds = qs.GetUserFundAccountInfo(2,(this.dd_creType.SelectedIndex + 1).ToString(),this.tbx_param2.Text);
				}
				else
				{
					if(this.tbx_param1.Text.Trim() == "")
					{
						WebUtils.ShowMessage(this,"请输入查询参数");
						return;
					}

					ds = qs.GetUserFundAccountInfo(this.dd_queryType.SelectedIndex + 1,this.tbx_param1.Text,this.tbx_param2.Text);
				}
				
				if(ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
				{
					WebUtils.ShowMessage(this,"查询结果为空");
					this.ClearDetail();
					return;
				}

				//this.DataGrid_QueryResult.DataSource = ds;
				//this.DataGrid_QueryResult.DataBind();

                ds.Tables[0].Columns.Add("Fcre_type_name");
                ds.Tables[0].Columns.Add("Fcre_id_hide");
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ds.Tables[0].Rows[i]["Fcre_type_name"] = getData.GetCreNameFromCreCode(ds.Tables[0].Rows[i]["Fcre_type"].ToString());
                    ds.Tables[0].Rows[i]["Fcre_id_hide"] = setConfig.ConvertCreID(ds.Tables[0].Rows[i]["Fcre_id"].ToString());
                }

               this.DataGrid1.DataSource = ds;
               this.DataGrid1.DataBind();
               if (ds.Tables[0].Rows.Count > 1)
               {
                   this.DataGrid1.Visible = true;
               }
               else
               {
                   this.DataGrid1.Visible = false;
                   BindDetail(ds);
               }

			}
            catch (SoapException eSoap) //捕获soap类异常
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "调用服务出错：" + errStr);
            }
			catch(Exception ex)
			{
				WebUtils.ShowMessage(this.Page,ex.Message);
			}
		}

        private void DataGrid1_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            BindDetail(e.Item.ItemIndex);
        }

	}
}
