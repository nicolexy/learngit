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


namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
	/// <summary>
	/// FundQuery ��ժҪ˵����
	/// </summary>
	public partial class FundQuery : System.Web.UI.Page
	{

		public string  begintime = DateTime.Now.ToString("yyyy-MM-dd");
		public string endtime = DateTime.Now.ToString("yyyy-MM-dd");
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			//Ȩ����֤
			try
			{
				Label1.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();
				int operid = Int32.Parse(Session["OperID"].ToString());

				// if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "FundQuery")) Response.Redirect("../login.aspx?wh=1");
				if(!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("FundQuery",this)) Response.Redirect("../login.aspx?wh=1");
			}
			catch
			{
				Response.Redirect("../login.aspx?wh=1");
			}

			Label8.Text="��ֵ��ѯ";
			if (Request.QueryString["fcurtype"] != null)//rowena 20100722  ���ӻ�����Ŀ
			{
				Label8.Text="�����ֵ��ѯ";
			}
			

			//����ǳ�ֵ������Ҫ�ж��Ƿ�Ϊ�գ�����ֻ�ܹ�Ϊ����
			if (this.dpLst.SelectedValue == "czd")
			{
				this.revNumOnly.Enabled   = true;
				this.rfvNullCheck.Enabled = true;
			}
			else //�����QQID,����֤��Ϊ��ʱ��Ĭ��Ϊ�����ȫ���û�����Ϣ
			{
				this.revNumOnly.Enabled   = false;
				this.rfvNullCheck.Enabled = false;
			}
			
			// �ڴ˴������û������Գ�ʼ��ҳ��

			if(!IsPostBack)
			{
                TextBoxBeginDate.Text = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
                TextBoxEndDate.Text = DateTime.Now.ToString("yyyy-MM-dd 23:59:59");

				classLibrary.setConfig.GetAllBankList(ddlBankType);
				ddlBankType.Items.Insert(0,new ListItem("��������","0000"));

				Table2.Visible = false;				
			}
                

			//����Ǵ�����ҳ����ת��ȡ��ֵ�������飬����ת
			if (Request.QueryString["czID"] != null)
			{
				this.dpLst.SelectedValue = "czd";
				string ID = Request.QueryString["czID"].ToString().Trim();
				//				this.CheckBox1.Checked=true;
				TextBoxBeginDate.Text="2009��01��01�� 00:00:00";
				TextBoxEndDate.Text = "2009��01��01�� 00:00:00";
				if(Request.QueryString["checkdate"] != null)
				{
					string date=Request.QueryString["checkdate"].Trim();
					date=date.Substring(0,4)+"-"+date.Substring(4,2)+"-"+date.Substring(6,2);
					TextBoxBeginDate.Text=DateTime.Parse(date).AddDays(-1).ToString("yyyy-MM-dd 00:00:00");
					TextBoxEndDate.Text=DateTime.Parse(date).AddDays(1).ToString("yyyy-MM-dd 23:59:59");
				
				}
				this.tbQQID.Text=ID;
				clickEvent(ID);
			}
			else
			{
				//��ʼ��ʱ��
			
				if(this.dpLst.SelectedValue == "qq")
				{
					string sID = this.tbQQID.Text.Trim();  //���ڳ�ֵ����ѯһ��ʼֻ֧�֣ѣѺŲ�ѯ��������txbox�� QQID,���������ֵ��ID
					//clickEvent(sID);	
					//furion �Զ���ѯ��ȫ���˷�ʱ�䡣20061201
				}
				else
				{
					Table2.Visible= false;
				}	
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

		public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
		{
			try
			{
				Table2.Visible = true;
				pager.CurrentPageIndex = e.NewPageIndex;
				DateTime enddate = DateTime.Parse(TextBoxEndDate.Text);
				string newczdate=PublicRes.GetZWDicValue("OldOrderCZDataEndTime");
				DateTime dtnewcsdate=DateTime.Parse(newczdate);
				if(enddate.CompareTo(dtnewcsdate)<0)
				{
					BindData(e.NewPageIndex,true);
				}
				else
				{
					BindData(e.NewPageIndex,false);
				}
			}
			catch(LogicException lex)
			{
				
				WebUtils.ShowMessage(this.Page,PublicRes.GetErrorMsg(lex.Message.ToString()));
			}
			catch(SoapException eSoap) //����soap���쳣
			{
				string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
				WebUtils.ShowMessage(this.Page,"���÷������" + errStr);
			}
			catch(Exception eSys)
			{
				WebUtils.ShowMessage(this.Page,"��ȡ����ʧ�ܣ�" + classLibrary.setConfig.replaceHtmlStr(eSys.Message) );
			}
		}

		private void ValidateDate()
		{
			DateTime begindate;
			DateTime enddate;
			string u_ID = tbQQID.Text.Trim();
			try
			{
				begindate = DateTime.Parse(TextBoxBeginDate.Text);
				enddate = DateTime.Parse(TextBoxEndDate.Text);
			}
			catch
			{
				throw new Exception("������������");
			}

			if(begindate.CompareTo(enddate) > 0)
			{
				throw new Exception("��ֹ����С����ʼ���ڣ����������룡");
			}
			string newczdate=PublicRes.GetZWDicValue("OldOrderCZDataEndTime");
			if(newczdate==null ||newczdate=="")
			{
				throw new Exception("δ��ѯ��OldOrderCZDataEndTime��Ӧ������ֵ��" );
			}

            if (this.dpLst.SelectedValue != "total")
            {
                if (string.IsNullOrEmpty(u_ID))
                    throw new Exception("���������뵥�Ż�QQ�Ž��в�ѯ�����������룡");

                if (begindate.AddDays(15).CompareTo(enddate) < 0)
                    throw new Exception("ѡ��ʱ��γ�����ʮ���죬���������룡");

                if (this.dpLst.SelectedValue.ToLower() == "tobank")
                {
                    if(begindate.Year != enddate.Year)
                        throw new Exception("�����еĶ������ݲ�֧�ֿ���Ȳ�ѯ�����������룡");
                }
            }
            else
            {
                if (begindate.AddDays(1).CompareTo(enddate) < 0)
                {
                    throw new Exception("ѡ��ʱ��γ�����һ�죬���������룡");
                }
            }
			DateTime dtnewcsdate=DateTime.Parse(newczdate);
			DateTime  dtnewendate=dtnewcsdate.AddDays(-1);
			if(enddate.CompareTo(dtnewcsdate)>=0 && begindate.CompareTo(dtnewcsdate)<0)
			{
				string nenddate=dtnewendate.ToString("yyyy-MM-dd 23:59:59");
				TextBoxEndDate.Text = nenddate;
				throw new Exception("����"+newczdate+"Ϊ��ʼ���ڻ���"+nenddate+"��������!");
			}
			try
			{
				float tmp = float.Parse(tbFNum.Text.Trim());
				
				if (tmp > 2100000000)
				{
					throw new Exception("������Ϊ21000000Ԫ�����ܳ����ý�");	
				}
			}
			catch
			{
				throw new Exception("��������ȷ�Ľ�");
			}

			ViewState["fnum"]    = tbFNum.Text.Trim();
			ViewState["fnumMax"] = txbNumMax.Text.Trim();
			ViewState["fstate"] = ddlStateType.SelectedValue;

			ViewState["uid"] = classLibrary.setConfig.replaceSqlStr(u_ID);
			ViewState["begindate"] = DateTime.Parse(begindate.ToString("yyyy-MM-dd HH:mm:ss"));
			begintime = begindate.ToString("yyyy-MM-dd");
			ViewState["enddate"] = DateTime.Parse(enddate.ToString("yyyy-MM-dd HH:mm:ss"));
			endtime = enddate.ToString("yyyy-MM-dd");

			ViewState["sorttype"]  = ddlSortType.SelectedValue;
			ViewState["querytype"] = this.dpLst.SelectedValue.Trim();

			//furion 20060324 �������в�ѯ����
			ViewState["banktype"] = ddlBankType.SelectedValue;

			//furion 20050819 �����SQL�����ַ����ж�
			tbQQID.Text = classLibrary.setConfig.replaceSqlStr(u_ID);

		}

		protected void Button2_Click(object sender, System.EventArgs e)
		{
			//			//������ѯ�¼�
			//			if(this.dpLst.SelectedValue == "czd")
			//			{
			string sID = this.tbQQID.Text.Trim();  //���ڳ�ֵ����ѯһ��ʼֻ֧�֣ѣѺŲ�ѯ��������txbox�� QQID,���������ֵ��ID
			clickEvent(sID);	
			//			}else if (this.dpLst.SelectedValue.ToLower() == "tobank") 	
		}

		/// <summary>
		/// ���ݴ����ID���г�ֵ����ѯ��
		/// </summary>
		/// <param name="ID"></param>
		private void clickEvent(string ID)
		{
			Table2.Visible = true;
	
			DateTime begindate = DateTime.Parse(TextBoxBeginDate.Text);
			DateTime enddate = DateTime.Parse(TextBoxEndDate.Text);

            try
            {
                ValidateDate();

                if (CheckBox1.Checked && (this.dpLst.SelectedValue == "czd" || this.dpLst.SelectedValue == "toBank" || this.dpLst.SelectedValue == "BankBack"))
                {
                    //Ĭ��ָ���£����ٲ�ѯ����
                    pager.RecordCount = 9999;
                }
                else
                {
                    //			pager.RecordCount= GetCount(); 
                    pager.RecordCount = 999;
                }
                if (this.dpLst.SelectedValue == "total")
                {

                }
                else
                {
                    this.labCountNum.Text = "";
                    this.labAmount.Text = "";
                    this.labAmount.Visible = false;
                    this.labCountNum.Visible = false;
                    this.Label10.Visible = false;
                    this.Label9.Visible = false;

                }
            }
            catch (Exception err)
            {
                WebUtils.ShowMessage(this.Page, err.Message);
                return;
            }

			Table2.Visible = true;
            try
            {
                Query_Service.Query_Service qs = new Query_Service.Query_Service();
                if (qs.IsNewOrderCZData(enddate))
                    BindData(1, false);
                else
                    BindData(1, true);
            }
            catch (LogicException lex)
            {
                string errStr = PublicRes.GetErrorMsg(lex.Message.ToString());
                WebUtils.ShowMessage(this.Page, errStr);
            }
            catch (SoapException eSoap) //����soap���쳣
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "���÷������" + errStr);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "��ȡ����ʧ�ܣ�" + classLibrary.setConfig.replaceHtmlStr(eSys.Message));
            }
		}

		private void BindData(int index,bool isold)
		{
			string u_ID = ViewState["uid"].ToString();
			DateTime begindate = (DateTime)ViewState["begindate"];
			DateTime enddate = (DateTime)ViewState["enddate"];

			string sorttype  = ViewState["sorttype"].ToString();
			string queryType = ViewState["querytype"].ToString();

			begintime = begindate.ToString("yyyy-MM-dd");
			endtime = enddate.ToString("yyyy-MM-dd");

			float fnum = float.Parse(ViewState["fnum"].ToString());
			float fnumMax = float.Parse(ViewState["fnumMax"].ToString());

			int fstate = Int32.Parse(ViewState["fstate"].ToString());

			int max = pager.PageSize;
			int start = max * (index-1) + 1;

			int newmax = pager.PageSize;
			int newstart = max * (index-1);

			string banktype = ViewState["banktype"].ToString();

			
			int fcurtype=1;

			if (Request.QueryString["fcurtype"] != null)//rowena 20100722  ���ӻ�����Ŀ
			{
				fcurtype=int.Parse(Request.QueryString["fcurtype"].Trim());
			}
			DataSet ds =null;
			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
			Query_Service.Finance_Header fh = classLibrary.setConfig.setFH(this);
			qs.Finance_HeaderValue = fh;

			if(isold)
			{
				//furion ������ʷ��¼��ѯ 20060522
				bool isHistory = CheckBox1.Checked;
				ds = qs.GetFundList(u_ID,queryType,fcurtype,begindate,enddate,fstate,fnum,fnumMax,banktype,sorttype,isHistory, start,max);
			}
			else
			{
				ds=qs.GetBankRollListByListId(u_ID,queryType,fcurtype,begindate,enddate,fstate,fnum,fnumMax,banktype,sorttype, newstart,newmax);
			}
			if((this.dpLst.SelectedValue == "toBank"||this.dpLst.SelectedValue == "BankBack")&&!u_ID.ToUpper().StartsWith("CFT"))
			{
				DataTable cftDetail=new DataTable();
				if(ds!=null&&ds.Tables.Count==1)
				{
					cftDetail=ds.Tables[0];
				}
				
				for(int i=1;i<9;i++)
				{
					string newUID="CFT0"+i.ToString()+u_ID;
					DataSet tmpDS = null;
					if(isold)
					{
						tmpDS = qs.GetFundList(newUID,queryType,fcurtype,begindate,enddate,fstate,fnum,fnumMax,banktype,sorttype,true, start,max);
					}
					else
					{
						tmpDS=qs.GetBankRollListByListId(newUID,queryType,fcurtype,begindate,enddate,fstate,fnum,fnumMax,banktype,sorttype, newstart,newmax);
					}
					DataTable tmpDetail=null;
					if(tmpDS!=null&&tmpDS.Tables.Count==1)
					{
						tmpDetail=tmpDS.Tables[0];
					}
					if(tmpDetail!=null&&tmpDetail.Rows.Count>0)
					{
						if(cftDetail==null||cftDetail.Rows.Count==0)
						{
							ds=tmpDS;
							cftDetail=ds.Tables[0];
									
						}
						else
						{
							foreach(DataRow dr2 in tmpDetail.Rows)
							{
								cftDetail.ImportRow(dr2);
							}
						}
					}
					else
					{
						break;
					}
				}
			}

            //��ʱ�ӵĴ�����ѡ��ʷ��¼ֱ�������� 2014/01/23
            string flag = "1";//��ʶ��¼����ʷ������ͨ�������ڲ鿴��ϸ��ʱ����
            if (CheckBox1.Checked) 
            {
                ds = qs.GetFundList(u_ID, queryType, fcurtype, begindate, enddate, fstate, fnum, fnumMax, banktype, sorttype, true, start, max);
                flag = "2";
            }


			if(ds != null && ds.Tables.Count >0)
			{
                ds.Tables[0].Columns.Add("FHistoryFlag", typeof(String));
                ds.Tables[0].Columns.Add("FNewNum",typeof(String));
				classLibrary.setConfig.FenToYuan_Table(ds.Tables[0],"FNum","FNewNum");

				ds.Tables[0].Columns.Add("FbankName",typeof(String));
				classLibrary.setConfig.GetColumnValueFromDic(ds.Tables[0],"Fbank_type","FbankName","BANK_TYPE");

				ds.Tables[0].Columns.Add("FStateName",typeof(String));
				classLibrary.setConfig.GetColumnValueFromDic(ds.Tables[0],"Fsign","FStateName","TCLIST_SIGN");

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    dr.BeginEdit();
                    dr["FHistoryFlag"] = flag;
                    dr.EndEdit();
                }

				DataGrid1.DataSource = ds.Tables[0].DefaultView;
				DataGrid1.DataBind();
			}
			else
			{
				throw new LogicException("û���ҵ���¼��");
			}
		}

		private int GetCount()
		{
			string u_ID = ViewState["uid"].ToString();
			DateTime begindate = (DateTime)ViewState["begindate"];
			DateTime enddate = (DateTime)ViewState["enddate"];

			float fnum    = float.Parse(ViewState["fnum"].ToString());
			float fnumMax = float.Parse(ViewState["fnumMax"].ToString());
			int fstate = Int32.Parse(ViewState["fstate"].ToString());
			string queryType = ViewState["querytype"].ToString();

			//furion ��������
			string banktype = ViewState["banktype"].ToString();

			//furion ������ʷ��¼��ѯ 20060522
			bool isHistory = CheckBox1.Checked;

			int fcurtype=1;

			if (Request.QueryString["fcurtype"] != null)//rowena 20100722  ���ӻ�����Ŀ
			{
				fcurtype=int.Parse(Request.QueryString["fcurtype"].Trim());
			}
			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
			return qs.GetFundListCount(u_ID,queryType,fcurtype,begindate,enddate,fstate,fnum,fnumMax,banktype,isHistory);
		}

		//���ѡ��ͳ��������������������
		protected void dpLst_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if(this.dpLst.SelectedValue=="total")
			{
				this.tbQQID.Visible=false;
			}
			else
			{
				this.tbQQID.Visible=true;
			}
		}

	}
}
