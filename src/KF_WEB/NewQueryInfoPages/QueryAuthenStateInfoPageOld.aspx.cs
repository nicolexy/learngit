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

namespace TENCENT.OSS.CFT.KF.KF_Web.NewQueryInfoPages
{
	/// <summary>
	/// QueryAuthenStateInfoPage ��ժҪ˵����
	/// </summary>
    public partial class QueryAuthenStateInfoPageOld : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.Label lb_c4;
		protected System.Web.UI.WebControls.Label lb_c5;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			/*
				if(Session["OperID"] != null)
					this.lb_operatorID.Text = Session["OperID"].ToString();
			
				//string szkey = Session["SzKey"].ToString();
				int operid = Int32.Parse(Session["OperID"].ToString());

				// Ŀǰ��ҳ�治��ҪȨ��
				//if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID,"InfoCenter")) Response.Redirect("../login.aspx?wh=1");
			*/
			// �ڴ˴������û������Գ�ʼ��ҳ��
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

		protected void btn_submit_acc_Click(object sender, System.EventArgs e)
		{
			if(this.tbx_creid.Text.Trim() == "")
			{
				ShowMsg("֤�����벻��Ϊ�գ�");
				return;
			}

			try
			{
				Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

				//qs.Finance_HeaderValue = setConfig.setFH(Session["OperID"].ToString(),Request.UserHostAddress);
				qs.Finance_HeaderValue = classLibrary.setConfig.setFH(this);

				int creType = classLibrary.getData.GetCreCodeFromCreName(this.ddl_creType.SelectedValue);
				
				DataSet ds = qs.GetUserAuthenState_ByCre2(this.tbx_creid.Text.Trim(),creType);

				if(ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
				{
					this.ShowMsg("��ѯ���Ϊ��");
					this.Clear();
					return;
				}

				this.lb_c1.Text = ds.Tables[0].Rows[0]["Fqqid"].ToString();
				this.lb_c2.Text = ds.Tables[0].Rows[0]["Fcre_stat_Name"].ToString();
				this.lb_c3.Text = ds.Tables[0].Rows[0]["Fcard_stat_Name"].ToString();

				/*		��ʱ���ã��Ľӿ���
				DataSet ds = qs.GetUserAuthenState_ByCre(creType,this.tbx_creid.Text);

				if(ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
				{
					this.ShowMsg("��ѯ���Ϊ��");
					this.Clear();
					return;
				}
	
				string stateName = "";
				string authenTypeName = "";
			
				switch(ds.Tables[0].Rows[0]["state"].ToString().Trim())
				{
					case "1":
					{
						stateName = "��ʵ����֤";break;
					}
					case "2":
					{
						stateName = "�޽����¼";break;
					}
					default:
					{
						stateName = "δ����";break;
					}
				}

				switch(ds.Tables[0].Rows[0]["authen_type"].ToString().Trim())
				{
					case "1":
					{
						authenTypeName = "�����֤";break;
					}
					case "4":
					{
						authenTypeName = "��Ȩ��֤";break;
					}
					case "5":
					{
						authenTypeName = "������ʵ��";break;
					}
					case "6":
					{
						authenTypeName = "һ��ͨʵ����֤";break;
					}
					default:
					{
						authenTypeName = "δ֪";break;
					}
				}

				this.lb_queryAcc.Text = classLibrary.setConfig.ConvertCreID(this.tbx_creid.Text);
				this.lb_c1.Text = ds.Tables[0].Rows[0]["first_authen_id"].ToString();
				this.lb_c2.Text = stateName;
				this.lb_c4.Text = ds.Tables[0].Rows[0]["truename"].ToString();
				this.lb_c5.Text = authenTypeName;

				int authenIdsNum = 0;

				try
				{
					if(ds.Tables[0].Rows[0]["authen_id_nums"] != null)
					{
						try
						{
							authenIdsNum = int.Parse(ds.Tables[0].Rows[0]["authen_id_nums"].ToString());
						}
						catch
						{
							authenIdsNum = 0;
						}

						for(int i=0;i<authenIdsNum;i++)
						{
							try
							{
								if(ds.Tables[0].Rows[0][("authen_id_n" + i)] != null)
								{
									this.lb_c3.Text += ds.Tables[0].Rows[0][("authen_id_n" + i)].ToString() + "<br>";
								}
							}
							catch
							{
							
							}
						}
					}
				}
				catch
				{}

				//this.lb_c3.Text = ds.Tables[0].Rows[0]["first_authen_id"].ToString();

				*/
				
				//this.div_detail.Style.Add("display","inline");
			}
			catch(Exception ex)
			{
				this.Clear();
				throw new Exception(ex.Message);
			}
		}


		private void Clear()
		{
			this.lb_c1.Text = "";
			this.lb_c2.Text = "";
			this.lb_c3.Text = "";
		}


		private void ShowMsg(string msg)
		{
			Response.Write("<script language=javascript>alert('" + msg + "')</script>");
		}


	}
}
