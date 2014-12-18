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
using Tencent.DotNet.Common.UI;
using System.IO;
using System.Web.Services.Protocols;

namespace TENCENT.OSS.CFT.KF.KF_Web.NewQueryInfoPages
{
	/// <summary>
	/// QueryDKInfoPage ��ժҪ˵����
	/// </summary>
	public partial class QueryDKInfoPage : System.Web.UI.Page
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��

			if(!IsPostBack)
			{
				this.tbx_beginDate.Text = DateTime.Now.AddDays(-6).ToString("yyyy-MM-dd HH:mm:ss");
				this.tbx_endDate.Text = DateTime.Now.AddDays(0).ToString("yyyy-MM-dd HH:mm:ss");

				this.ButtonBeginDate.Attributes.Add("onclick","openModeBegin()");
				this.ButtonEndDate.Attributes.Add("onclick","openModeEnd()");

                GetAllBankList(ddlBankType);//��ȡ�������������б�

				ddl_service_code.Items.Clear();
                ddl_service_code.Items.Add(new ListItem("ȫ��", "9999999"));
				foreach(DictionaryEntry de in TENCENT.OSS.C2C.Finance.Common.CommLib.getData.htService_code)
				{
					ddl_service_code.Items.Add(new ListItem(de.Value.ToString(), de.Key.ToString()));
				}
				ddl_service_code.SelectedValue = "9999999";

				this.pager.PageSize = 10;
				this.pager.RecordCount = GetCount();

				if(Request.QueryString["spid"] != null || Request.QueryString["batchid"] != null)
				{
					if(Request.QueryString["spid"] != null)
						this.tbx_spid.Text = Request.QueryString["spid"].Trim();

					if(Request.QueryString["batchid"] != null)
						this.tbx_spBatchID.Text = Request.QueryString["batchid"].Trim();

					if(Request.QueryString["state"] != null)
					{
						if(Request.QueryString["state"] == "s")
						{
							this.ddl_state.SelectedIndex = 1;
						}
						else if(Request.QueryString["state"] == "f")
						{
							this.ddl_state.SelectedIndex = 2;
						}
						else if(Request.QueryString["state"] == "h")
						{
							this.ddl_state.SelectedIndex = 3;
						}
					}

					if(Request.QueryString["sDate"] != "")
						this.tbx_beginDate.Text = Request.QueryString["sDate"];

					if(Request.QueryString["eDate"] != "")
						this.tbx_endDate.Text = Request.QueryString["eDate"];

					BindData(1,true);
				}

				if(!ClassLib.ValidateRight("DKAdjust",this)) 
				{
					btn_batchadjust.Visible = false;
				}
			}


			this.pager.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(pager_PageChanged);
			this.DataGrid_QueryResult.ItemCommand += new DataGridCommandEventHandler(DataGrid_QueryResult_ItemCommand);
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


	
		private int GetCount()
		{
			return 1000;
		}

        protected void lkPrint_Click(Object sender, EventArgs e)
        {

        }

		private void BindData(int pageIndex,bool needUpdateCount)
		{
			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

			//qs.Finance_HeaderValue = setConfig.setFH(Session["OperID"].ToString(),Request.UserHostAddress);

			qs.Finance_HeaderValue = classLibrary.setConfig.setFH(this);

			try
			{
				DateTime sTime ,eTime;
				string strSTime,strETime;
				try
				{
					sTime = DateTime.Parse(this.tbx_beginDate.Text);
					eTime = DateTime.Parse(this.tbx_endDate.Text);

					strSTime = sTime.ToString("yyyy-MM-dd HH:mm:ss");
					strETime = eTime.ToString("yyyy-MM-dd HH:mm:ss");

					if(sTime.AddDays(7) <= eTime)
					{
						WebUtils.ShowMessage(this,"���ڿ�Ȳ��ܴ���һ��");
						return;
					}
					
					/*
					if(this.tbx_spid.Text.Trim() == "")
					{
						WebUtils.ShowMessage(this,"��ѯ�̻��Ų���Ϊ��");
						return;
					}
					*/
				}
				catch
				{
					WebUtils.ShowMessage(this,"���ڸ�ʽ����ȷ");
					return;
				}

                DataSet ds = qs.QueryDkInfo(this.txb_explain.Text, this.tbx_bankID.Text, this.tbx_userName.Text, strSTime,
                    strETime, this.tbx_spid.Text, this.tbx_spListID.Text, this.tbx_spBatchID.Text, this.tbx_cep_id.Text, this.ddl_state.SelectedValue,
                    txb_transaction_id.Text.Trim(), this.ddlBankType.SelectedValue, ddl_service_code.SelectedValue,
					(pageIndex - 1) * this.pager.PageSize,this.pager.PageSize);

				if(ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
				{
                    this.btn_outExcel.Visible = false;
					WebUtils.ShowMessage(this,"��ѯ���Ϊ��");
					this.Clear();
					return;
				}

                this.btn_outExcel.Visible = true;
				ds.Tables[0].Columns.Add("transId_url",typeof(string));

				ds.Tables[0].Columns.Add("FpaynumName",typeof(string));
				double sTotalMoney = 0,fTotalMoney = 0,hTotalMoney = 0;
				int sTotalNums = 0,fTotalNums = 0,hTotalNums = 0;

				foreach(DataRow dr in ds.Tables[0].Rows)
				{
					
					if(dr["Fstate"].ToString() == "8")
					{
                        sTotalMoney += double.Parse(dr["Fpaynum"].ToString());
						sTotalNums++;
					}
					else if(dr["Fstate"].ToString() == "9")
					{
						fTotalNums++;
                        fTotalMoney += double.Parse(dr["Fpaynum"].ToString());
					}
					else
					{
						hTotalNums++;
                        hTotalMoney += double.Parse(dr["Fpaynum"].ToString());
					}

					classLibrary.setConfig.GetColumnValueFromDic(ds.Tables[0],"Ftrade_state","Ftrade_stateName","PAY_STATE");

					dr["FpaynumName"] = setConfig.FenToYuan(dr["Fpaynum"].ToString());
					dr["Fbankacc_no"] = setConfig.ConvertAccount(dr["Fbankacc_no"].ToString());
					dr["transId_url"] = "../TradeManage/TradeLogQuery.aspx?id=" + dr["Ftransaction_id"].ToString();
				}


                //if(needUpdateCount)
                //{
                //    if(this.ddl_state.SelectedValue == "0")
                //    {
                //        DataSet ds2 = qs.CountDKInfo(this.txb_transaction_id.Text, this.ddlBankType.SelectedValue, this.tbx_bankID.Text, this.tbx_userName.Text, strSTime, strETime,
                //            this.tbx_spid.Text,this.tbx_spListID.Text,this.tbx_spBatchID.Text,"0");

                //        this.pager.RecordCount = int.Parse(ds2.Tables[0].Rows[0]["totalRecordCount"].ToString());

                //        ds2 = qs.CountDKInfo(this.txb_transaction_id.Text, this.ddlBankType.SelectedValue, this.tbx_bankID.Text, this.tbx_userName.Text, strSTime, strETime,
                //            this.tbx_spid.Text,this.tbx_spListID.Text,this.tbx_spBatchID.Text,"1");

                //        this.lb_successAllMoney.Text = setConfig.FenToYuan(ds2.Tables[0].Rows[0]["totalPayNum"].ToString());
                //        this.lb_successNum.Text = ds2.Tables[0].Rows[0]["totalRecordCount"].ToString();

                //        ds2 = qs.CountDKInfo(this.txb_transaction_id.Text, this.ddlBankType.SelectedValue, this.tbx_bankID.Text, this.tbx_userName.Text, strSTime, strETime,
                //            this.tbx_spid.Text,this.tbx_spListID.Text,this.tbx_spBatchID.Text,"2");

                //        this.lb_failAllMoney.Text = setConfig.FenToYuan(ds2.Tables[0].Rows[0]["totalPayNum"].ToString());
                //        this.lb_failNum.Text = ds2.Tables[0].Rows[0]["totalRecordCount"].ToString();

                //        ds2 = qs.CountDKInfo(this.txb_transaction_id.Text, this.ddlBankType.SelectedValue, this.tbx_bankID.Text, this.tbx_userName.Text, strSTime, strETime,
                //            this.tbx_spid.Text,this.tbx_spListID.Text,this.tbx_spBatchID.Text,"3");

                //        this.lb_handlingAllMoney.Text = setConfig.FenToYuan(ds2.Tables[0].Rows[0]["totalPayNum"].ToString());
                //        this.lb_handlingNum.Text = ds2.Tables[0].Rows[0]["totalRecordCount"].ToString();
                //    }
                //    else
                //    {
                //        DataSet ds2 = qs.CountDKInfo(this.txb_transaction_id.Text, this.ddlBankType.SelectedValue, this.tbx_bankID.Text, this.tbx_userName.Text, strSTime, strETime,
                //            this.tbx_spid.Text,this.tbx_spListID.Text,this.tbx_spBatchID.Text,this.ddl_state.SelectedValue);	

                //        switch(this.ddl_state.SelectedValue)
                //        {
                //            case "1":
                //            {
                //                this.pager.RecordCount = int.Parse(ds2.Tables[0].Rows[0]["totalRecordCount"].ToString());
                //                this.lb_successAllMoney.Text = setConfig.FenToYuan(ds2.Tables[0].Rows[0]["totalPayNum"].ToString());
                //                this.lb_successNum.Text = ds2.Tables[0].Rows[0]["totalRecordCount"].ToString();
                //                this.lb_failAllMoney.Text = "0";
                //                this.lb_failNum.Text = "0";
                //                this.lb_handlingAllMoney.Text = "0";
                //                this.lb_handlingNum.Text = "0";
                //                break;
                //            }
                //            case "2":
                //            {
                //                this.pager.RecordCount = int.Parse(ds2.Tables[0].Rows[0]["totalRecordCount"].ToString());
                //                this.lb_failAllMoney.Text = setConfig.FenToYuan(ds2.Tables[0].Rows[0]["totalPayNum"].ToString());
                //                this.lb_failNum.Text = ds2.Tables[0].Rows[0]["totalRecordCount"].ToString();
                //                this.lb_successAllMoney.Text = "0";
                //                this.lb_successNum.Text = "0";
                //                this.lb_handlingAllMoney.Text = "0";
                //                this.lb_handlingNum.Text = "0";
                //                break;
                //            }
                //            case "3":
                //            {
                //                this.pager.RecordCount = int.Parse(ds2.Tables[0].Rows[0]["totalRecordCount"].ToString());
                //                this.lb_handlingAllMoney.Text = setConfig.FenToYuan(ds2.Tables[0].Rows[0]["totalPayNum"].ToString());
                //                this.lb_handlingNum.Text = ds2.Tables[0].Rows[0]["totalRecordCount"].ToString();
                //                this.lb_failAllMoney.Text = "0";
                //                this.lb_failNum.Text = "0";
                //                this.lb_successAllMoney.Text = "0";
                //                this.lb_successNum.Text = "0";
                //                break;
                //            }
                //        }
                //    }
                //}


                this.lb_failAllMoney.Text = setConfig.FenToYuan(fTotalMoney).ToString();
				this.lb_failNum.Text = fTotalNums.ToString();

                this.lb_successAllMoney.Text = setConfig.FenToYuan(sTotalMoney).ToString();
				this.lb_successNum.Text = sTotalNums.ToString();

                this.lb_handlingAllMoney.Text = setConfig.FenToYuan(hTotalMoney).ToString();
				this.lb_handlingNum.Text = hTotalNums.ToString();

				this.DataGrid_QueryResult.DataSource = ds;
                //DataView dv = ds.Tables[0].DefaultView;
                //dv.Sort = "Fcreate_time, Funame  DESC";
                //this.DataGrid_QueryResult.DataSource = dv;
				this.DataGrid_QueryResult.DataBind();
			}
			catch(Exception ex)
			{
				WebUtils.ShowMessage(this,ex.Message);
			}
		}

        /**
         * ���ۻ�ȡ�������������
         * */
        public void GetAllBankList(DropDownList ddl)
        {
            DataTable dt;

            Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            dt = qs.QueryDkInfo_GetBank().Tables[0];

            if (dt != null)
            {

                ddl.Items.Clear();
                this.ddlBankType.Items.Add(new ListItem("ȫ��", ""));
                foreach (DataRow dr in dt.Rows)
                {
                    string Fbank_type = dr["Fbank_type"].ToString();
                    string FFlag_name = dr["Fbank_sname"].ToString();

                    ddl.Items.Add(new ListItem(FFlag_name, Fbank_type));
                }

            }

            //ddl.Items.Add(new ListItem("��������","1002"));
            //ddl.Items.Add(new ListItem("��������","1003"));
            //ddl.Items.Add(new ListItem("ũҵ����","1005"));
            //ddl.Items.Add(new ListItem("�ַ�����","1004"));
            //ddl.Items.Add(new ListItem("����ƽ������","1010"));
            //ddl.Items.Add(new ListItem("��ͨ����","1020"));
            //ddl.Items.Add(new ListItem("�㶫��չ����","1027"));
        }

        //public void GetAllBankList(DropDownList ddl)
        //{
        //    try
        //    {
        //        ddl.Items.Clear();
        //        string[] bankName = new string[30];
        //        bankName[0] = "��������";
        //        bankName[1] = "�й���������";
        //        bankName[2] = "�й���������";
        //        bankName[3] = "�Ϻ��ֶ���չ����";
        //        bankName[4] = "�й�ũҵ����";
        //        bankName[5] = "�й���������";
        //        bankName[6] = "���ڷ�չ����";
        //        bankName[7] = "������ҵ����";
        //        bankName[8] = "�й�����";
        //        bankName[9] = "�й�������������";
        //        bankName[10] = "��������";
        //        bankName[11] = "���ڷ�չ����";
        //        bankName[12] = "��ҵ����";
        //        bankName[13] = "ƽ������";
        //        bankName[14] = "�й���ͨ����";
        //        bankName[15] = "����ʵҵ����";
        //        bankName[16] = "�й��������";
        //        bankName[17] = "�й����н�ǿ�";
        //        bankName[18] = "�㶫��չ����";
        //        bankName[19] = "�й��������ÿ�";
        //        bankName[20] = "����ũ������";
        //        bankName[21] = "��������";

        //        System.Web.Caching.Cache objCache = System.Web.HttpRuntime.Cache;
        //        string bankStr = "BANK_TYPE";
        //        Hashtable ht = new Hashtable();
        //        if (objCache[bankStr] == null)
        //            TENCENT.OSS.C2C.Finance.BankLib.BankIO.queryDic(bankStr);
        //        ht = (Hashtable)objCache[bankStr];
        //        if (ht != null)
        //        {
        //            this.ddlBankType.Items.Add(new ListItem("ȫ��", ""));
        //            ArrayList akeys = new ArrayList(ht.Keys);
        //            for (int i = 0; i < bankName.Length; i++)
        //            {
        //                foreach (string k in akeys)
        //                {

        //                    if (ht[k].Equals(bankName[i]))
        //                    {
        //                        this.ddlBankType.Items.Add(new ListItem(ht[k].ToString(), k.ToString()));
        //                    }
        //                }
        //            }
                   
        //        }
        //    } catch (Exception e) //û�д������ֵ��ж���memo
        //    {
        //        this.ddlBankType.Items.Add(new ListItem("ȫ��", ""));
        //    }
        //}

		protected void btn_serach_Click(object sender, System.EventArgs e)
		{
			BindData(1,true);

			this.pager.CurrentPageIndex = 1;
		}

		private void pager_PageChanged(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
		{
			this.pager.CurrentPageIndex = e.NewPageIndex;

			BindData(e.NewPageIndex,false);
		}


		private void Clear()
		{
			this.lb_c1.Text = "";
			this.lb_c2.Text = "";
			this.lb_c3.Text = "";
			this.lb_c4.Text = "";
			this.lb_c5.Text = "";
			this.lb_c6.Text = "";
			this.lb_c7.Text = "";
			this.lb_c8.Text = "";
			this.lb_c9.Text = "";
			this.lb_c10.Text = "";
			this.lb_c11.Text = "";
			this.lb_c12.Text = "";
			this.lb_c13.Text = "";
			this.lb_c14.Text = "";
			this.lb_c15.Text = "";
			this.lb_c16.Text = "";
			this.lb_c17.Text = "";

			this.lb_c18.Text = "";
			this.lb_c19.Text = "";
			this.lb_c20.Text = "";
			this.lb_c21.Text = "";
			this.lb_c22.Text = "";
			this.lb_c23.Text = "";
			this.lb_c24.Text = "";
			this.lb_c25.Text = "";
			this.lb_c26.Text = "";
			this.lb_c27.Text = "";
			this.lb_c28.Text = "";
			this.lb_c29.Text = "";
			this.lb_c30.Text = "";
			this.lb_c31.Text = "";
			this.lb_c32.Text = "";
			this.lb_c33.Text = "";
			this.lb_c34.Text = "";
			this.lb_c35.Text = "";
			this.lb_c36.Text = "";
            this.lb_c37.Text = "";
            this.lb_c38.Text = "";

			this.DataGrid_QueryResult.DataSource = null;
			this.DataGrid_QueryResult.DataBind();

            this.lb_successAllMoney.Text ="0";
            this.lb_successNum.Text = "0";

            this.lb_failAllMoney.Text = "0";
            this.lb_failNum.Text = "0";

            this.lb_handlingAllMoney.Text = "0";
            this.lb_handlingNum.Text = "0";
		}



		private DataSet GetDKDetail(string cep_id)
		{
			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

			//qs.Finance_HeaderValue = setConfig.setFH(Session["OperID"].ToString(),Request.UserHostAddress);

			qs.Finance_HeaderValue = classLibrary.setConfig.setFH(this);

			string strSTime,strETime;

			try
			{
				strSTime = DateTime.Parse(this.tbx_beginDate.Text.Trim()).ToString("yyyy-MM-dd HH:mm:ss");
				strETime = DateTime.Parse(this.tbx_endDate.Text.Trim()).ToString("yyyy-MM-dd HH:mm:ss");
			}
			catch
			{
				WebUtils.ShowMessage(this,"���ڸ�ʽ����ȷ");
				return null;
			}

			return qs.QueryDkDetail(cep_id,strSTime,strETime);
		}


		private void DataGrid_QueryResult_ItemCommand(object source, DataGridCommandEventArgs e)
		{
			if(e.CommandName == "detail")
			{
				DataSet ds = GetDKDetail(e.Item.Cells[0].Text.Trim());

				if(ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
				{
					WebUtils.ShowMessage(this,"��ѯ���Ϊ��");
					Clear();
					return;
				}

				DataRow dr = ds.Tables[0].Rows[0];

				string strCreateDate = dr["Fcreate_time"].ToString();
				DateTime createDate = DateTime.Parse(strCreateDate);

				this.lb_c1.Text = dr["Fcoding"].ToString();
				this.lb_c2.Text = "<a href=../BaseAccount/PayBusinessQuery.aspx?spid=" + dr["Fspid"].ToString() + " target=_blank>" + dr["Fspid"].ToString() + "</a>";
				this.lb_c3.Text = dr["Ftransaction_id"].ToString();
				this.lb_c4.Text = dr["Fbank_list"].ToString();
				this.lb_c5.Text = dr["FstateName"].ToString();

				this.lb_c6.Text = setConfig.ConvertAccount(dr["Fbankacc_no"].ToString());
				this.lb_c7.Text = "<a href=" + "../TradeManage/TradeLogQuery.aspx?id=" + dr["Ftransaction_id"].ToString() + " target=_blank >" + e.Item.Cells[7].Text.Trim() + "</a>";
				this.lb_c8.Text = dr["Fbankacc_typeName"].ToString();
				this.lb_c9.Text = setConfig.FenToYuan(dr["Fpaynum"].ToString());
				this.lb_c10.Text = dr["Funame"].ToString();
				this.lb_c11.Text = dr["Ftrade_typeName"].ToString();
				this.lb_c12.Text = dr["Fbank_name"].ToString();
				this.lb_c13.Text = dr["Fcreate_time"].ToString();
				this.lb_c14.Text = dr["Fpay_time"].ToString();
				this.lb_c15.Text = dr["FchannelName"].ToString();
				this.lb_c16.Text = dr["Frcd_typeName"].ToString();
				this.lb_c17.Text = "<a href=" + "./QueryDKInfoPage.aspx?batchid=" 
					+ dr["Fsp_batchid"].ToString() + "&sDate=" + createDate.ToString("yyyy-MM-dd") 
					+ "&eDate=" + createDate.AddDays(1).ToString("yyyy-MM-dd") + " target=_blank >" + dr["Fsp_batchid"].ToString() + "</a>";

				this.lb_c18.Text = dr["Fservice_codeName"].ToString();
				this.lb_c19.Text = dr["Fmemo"].ToString();
				this.lb_c20.Text = dr["FailedReason"].ToString();
				this.lb_c21.Text = dr["Fbankacc_attrName"].ToString();
				//this.lb_c22.Text = dr[""].ToString();
				this.lb_c23.Text = dr["Fcep_id"].ToString();
				this.lb_c24.Text = dr["Fbank_batch_id"].ToString();
				this.lb_c25.Text = dr["Fservice_code"].ToString();
				this.lb_c26.Text = getData.GetBankNameFromBankCode(dr["Fbank_type"].ToString());
				this.lb_c27.Text = dr["Fbankacc_typeName"].ToString();
				this.lb_c28.Text = dr["Fservice_code"].ToString().Replace(dr["Fspid"].ToString(),"");

				this.lb_c29.Text = getData.GetCreNameFromCreCode(dr["Fcredit_type"].ToString());
				this.lb_c30.Text = setConfig.ConvertCreID(dr["Fcredit_id"].ToString());
				this.lb_c31.Text = dr["Fbank_channel"].ToString();
				this.lb_c32.Text = dr["Fchannel"].ToString();
				this.lb_c33.Text = dr["Fpay_type"].ToString();
				this.lb_c34.Text = setConfig.FenToYuan(dr["Famount"].ToString());
				this.lb_c35.Text = dr["Fadjust_flag"].ToString();
                this.lb_c36.Text = dr["Fbankacc_time"].ToString();
                this.lb_c37.Text = dr["Fbank_roll"].ToString();
                //20130821 lxl �����ֻ���
                this.lb_c38.Text = e.Item.Cells[14].Text;
				
			}
		}

		private ArrayList GetAllSelect()
		{
			ArrayList al = new ArrayList();
			for(int i = 0; i < DataGrid_QueryResult.Items.Count; i++)
			{
				System.Web.UI.Control obj = DataGrid_QueryResult.Items[i].Cells[1].FindControl("CheckBox1");
				if(obj != null)	
				{
					CheckBox cb = (CheckBox)obj;
					string fid = DataGrid_QueryResult.Items[i].Cells[0].Text.Trim();					

					if(cb.Checked &&DataGrid_QueryResult.Items[i].Cells[8].Text.Trim() == "��������ǰ")
					{
						al.Add(fid);
					}
				}
			}
			return al;
		}

		protected void btn_batchadjust_Click(object sender, System.EventArgs e)
		{
			//��ѡ��ĵ��Ŵ������Σ�Ȼ����ת������ҳ�档
			ArrayList al = GetAllSelect();
			if(al.Count > 0)
			{
				//���������κţ�����ϸ�������Σ�Ȼ��������κ�����������������ID��
				
				Query_Service.Query_Service fs = new Query_Service.Query_Service();
				Query_Service.Finance_Header Ffh = classLibrary.setConfig.setFH(this);
				fs.Finance_HeaderValue = Ffh;

				object[] newal = new object[al.Count];
				al.CopyTo(newal,0);

				string msg = "";
				string batchid = "";
				if(fs.DK_BatchSelect(newal,Session["uid"].ToString(), out batchid, out msg))
				{
//					string url = "DKAdjust.aspx?querytype=adjust&batchid=" + batchid;
//					string str = "window.showModalDialog(\""+url+"\" , \"��������\")";
//					Response.Write("<script language='javascript'>" + str + "</script>");

					//�����ɹ�����ת����������ҳ�档
					Response.Redirect("DKAdjust.aspx?querytype=adjust&batchid=" + batchid);
					
					return;
				}
				else
				{
					msg=msg.Replace("\'","\\\'");
					WebUtils.ShowMessage(this.Page,"ѡ����ϸ��¼ʧ�� \\r\\n" + msg);
				}
			}
			else
			{
				WebUtils.ShowMessage(this.Page,"���ȹ�ѡ��Ҫ����״̬������!");
			}
		}

        protected void btn_outExcel_Click(object sender, System.EventArgs e)
        {
            BindDataOutExcel();

        }

        private void BindDataOutExcel()
        {
            Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

            //qs.Finance_HeaderValue = setConfig.setFH(Session["OperID"].ToString(),Request.UserHostAddress);

            qs.Finance_HeaderValue = classLibrary.setConfig.setFH(this);

            try
            {
                DateTime sTime, eTime;
                string strSTime, strETime;
                try
                {
                    sTime = DateTime.Parse(this.tbx_beginDate.Text);
                    eTime = DateTime.Parse(this.tbx_endDate.Text);

                    strSTime = sTime.ToString("yyyy-MM-dd HH:mm:ss");
                    strETime = eTime.ToString("yyyy-MM-dd HH:mm:ss");

                    if (sTime.AddDays(7) <= eTime)
                    {
                        WebUtils.ShowMessage(this, "���ڿ�Ȳ��ܴ���һ��");
                        return;
                    }
                }
                catch
                {
                    WebUtils.ShowMessage(this, "���ڸ�ʽ����ȷ");
                    return;
                }

                DataSet ds = qs.QueryDkInfo(this.txb_explain.Text, this.tbx_bankID.Text, this.tbx_userName.Text, strSTime,
                    strETime, this.tbx_spid.Text, this.tbx_spListID.Text, this.tbx_spBatchID.Text, this.tbx_cep_id.Text,this.ddl_state.SelectedValue,
                    txb_transaction_id.Text.Trim(), this.ddlBankType.SelectedValue, ddl_service_code.SelectedValue,
                     -1, -1);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    WebUtils.ShowMessage(this, "��ѯ���Ϊ��");
                    this.Clear();
                    return;
                }

                ds.Tables[0].Columns.Add("FpaynumName", typeof(string));

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    classLibrary.setConfig.GetColumnValueFromDic(ds.Tables[0], "Ftrade_state", "Ftrade_stateName", "PAY_STATE");

                    dr["FpaynumName"] = setConfig.FenToYuan(dr["Fpaynum"].ToString());
                    dr["Fbankacc_no"] = setConfig.ConvertAccount(dr["Fbankacc_no"].ToString());
                }

                DataTable dt = ds.Tables[0];
                StringWriter sw = new StringWriter();
                string excelHeader = DataGrid_QueryResult.Columns[3].HeaderText;
                for (int i = 4; i < DataGrid_QueryResult.Columns.Count-1; i++)
                {
                    excelHeader += "\t" + DataGrid_QueryResult.Columns[i].HeaderText;
                }
                sw.WriteLine(excelHeader);
                string str = "\"\t=\"";
                foreach (DataRow dr in dt.Rows)
                {
                    sw.WriteLine("=\"" + dr["Fcoding"].ToString() + str + dr["Ftransaction_id"].ToString() + str + dr["Fcreate_time"] + str
                        + dr["FpaynumName"] + str + dr["Fservice_codeName"] + str + dr["FstateName"] + str 
                        + dr["Ftrade_stateName"] + str + dr["Fbank_typeName"] + str + dr["Fbankacc_no"] + str 
                        + dr["Funame"] + str+ dr["FailedReason"] + "\"");
                }
                sw.Close();
                Response.AddHeader("Content-Disposition", "attachment; filename=���۵��ʲ�ѯ.xls");
                Response.ContentType = "application/ms-excel";
                Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
                Response.Write(sw);
                Response.End();
            }
            catch (SoapException eSoap) //����soap���쳣
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message);
                WebUtils.ShowMessage(this.Page, "���÷������" + errStr);
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this, ex.Message);
            }
        }
    
    }
}
