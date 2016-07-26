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
using System.Web.Services;
using System.Web.Services.Protocols;
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.CFT.KF.KF_Web;
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using System.Text;
using System.Net;
using System.IO;
using System.Xml;
using System.Configuration;
using CFT.CSOMS.BLL.PublicService;
using TENCENT.OSS.CFT.KF.KF_Web.InternetBank;

namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
    public partial class OverseasReturnQuery : TENCENT.OSS.CFT.KF.KF_Web.PageBase
    {
        protected PublicService publicService = new PublicService();

        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                Label_uid.Text = Session["uid"].ToString();
                string szkey = Session["SzKey"].ToString();
               
                //int operid = Int32.Parse(Session["OperID"].ToString());

                //if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID,"InfoCenter")) Response.Redirect("../login.aspx?wh=1");

                if (!classLibrary.ClassLib.ValidateRight("InfoCenter", this)) Response.Redirect("../login.aspx?wh=1");


                if (!IsPostBack)
                {
                    this.table3.Visible = false;
                }
            }
            catch
            {
                Response.Redirect("../login.aspx?wh=1");
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

           this.Datagrid1.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.DataGrid1_ItemCommand);
        }
        #endregion

        protected void Button_Click(object sender, System.EventArgs e)
        {
            try
            {
                ViewState["transactionId"] = TextTransactionId.Text.Trim();
                ViewState["drawId"] = TextDrawId.Text.Trim();
                if ((ViewState["transactionId"].ToString() == null || ViewState["transactionId"].ToString() == "")&&
                    (ViewState["drawId"].ToString() == null || ViewState["drawId"].ToString() == ""))
                {
                    WebUtils.ShowMessage(this.Page, "����������һ����ѯ������");
                    return;
                }
                BindData();
            }
            
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "��ȡ����ʧ�ܣ�" + eSys.Message);
            }


        }

        public void ListetailPage(Object sender, CommandEventArgs e)
        {
 
            try
            {
                
                string transaction_id = e.CommandName.ToString().Trim();
                string draw_id = e.CommandArgument.ToString().Trim();
                BindDataDetail(transaction_id, draw_id);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "��ȡ����ʧ�ܣ�" + eSys.Message);
            }

 
        }


        protected void Detail_Click(object sender, System.EventArgs e)
        {
            try
            {
                BindDataDetail(ViewState["transactionId"].ToString(), ViewState["drawId"].ToString());
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "��ȡ����ʧ�ܣ�" + eSys.Message);
            }
        }

        private void DataGrid1_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            string transaction_id = e.Item.Cells[0].Text.Trim();
            string draw_id = e.Item.Cells[1].Text.Trim();
            try
            {
                if (e.CommandName == "query")
                    BindDataDetail(transaction_id, draw_id);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "��ȡ����ʧ�ܣ�" + eSys.Message);
            }
        }
        //�����̻��˿������
        private void BindData()
        {
            try
            {
                Query_Service.Query_Service qs = new Query_Service.Query_Service();
                DataSet ds = qs.OverseasReturnQuery(ViewState["transactionId"].ToString(), ViewState["drawId"].ToString());

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ds.Tables[0].Columns.Add("Frefund_req_fee_rmb_str", typeof(String));
                    ds.Tables[0].Columns.Add("Fcur_type_str", typeof(String));
                   // ds.Tables[0].Columns.Add("order_exist", typeof(String));//�Ƿ���ں��Ľ��׵�
                    ds.Tables[0].Columns.Add("Frefund_req_fee_str", typeof(String));//�˿���(���)
                    ds.Tables[0].Columns.Add("Ffee_type_str", typeof(String));//�������
                    ds.Tables[0].Columns.Add("Frefund_status_str", typeof(String));//�����˿״̬
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        string temp = dr["Fcur_type"].ToString();
                        if (temp == "1")
                            dr["Fcur_type_str"] = "���ʺ�";
                        else if (temp == "0")
                            dr["Fcur_type_str"] = "�����ʺ�";
                        else
                            dr["Fcur_type_str"] = "δ֪";

                        string cur_type = dr["Ffee_type"].ToString();
                        if (dr["Frefund_req_exist"].ToString() == "0" || string.IsNullOrEmpty(cur_type))//Frefund_req_exist=0ʱ����ҽ��Ϊ0����ұ���Ϊ�գ���ʱ���������ת��
                            dr["Frefund_req_fee_str"] = "0";
                        else
                        {
                            if (InternetBankDictionary.CurTypeAIComment.ContainsKey(cur_type))
                            {
                                dr["Ffee_type_str"] = InternetBankDictionary.CurTypeAIComment[cur_type];
                            }
                            else
                            {
                                dr["Ffee_type_str"] = cur_type;
                            }
                            dr["Frefund_req_fee_str"] = publicService.FenToYuan(dr["Frefund_req_fee"].ToString(), dr["Ffee_type"].ToString());
                        }
                        int nStatus = int.Parse(dr["Frefund_status"].ToString());
                        if (nStatus == 1 || nStatus == 2)
                        {
                            dr["Frefund_status_str"] = "δȷ������Ҫ�̻�ԭ�˿�����·���";
                        }
                        else if (nStatus == 3 || nStatus == 5 || nStatus == 6 )
                        {
                            dr["Frefund_status_str"] = "�˿�ʧ��";
                        }
                        else if (nStatus == 4 || nStatus == 10)
                        {
                            dr["Frefund_status_str"] = "�˿�ɹ�";
                        }
                        else if (nStatus == 8 || nStatus == 9 || nStatus == 11 || nStatus == 100)
                        {
                            dr["Frefund_status_str"] = "�˿����";
                        }
                        else 
                        {
                            dr["Frefund_status_str"] = "δ֪";
                        }
                    }
                    classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "Frefund_req_fee_rmb", "Frefund_req_fee_rmb_str");

                    ViewState["transactionId"] = "";
                    ViewState["drawId"] = "";


                    this.table3.Visible = true;
                    dgList.Visible = false;
                    Detail.Visible = false;
                    Datagrid1.Visible = true;
                    Datagrid1.DataSource = ds.Tables[0].DefaultView;
                    Datagrid1.DataBind();
                }
                else
                {

                    Detail.Visible = true;
                    Datagrid1.Visible = false;
                    this.dgList.Visible = false;
                    WebUtils.ShowMessage(this.Page, "û�в�ѯ����¼!");
                    return;
                }

            }
            catch (SoapException eSoap) //����soap���쳣
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message);
                WebUtils.ShowMessage(this.Page, "���÷������" + errStr);
            }
            catch (Exception ex)
            {
                dgList.Visible = false;
                throw new LogicException(ex.Message);
            }
        }

        //���Ľ����˿�����
        private void BindDataDetail(string transaction_id, string draw_id)
        {
            try
            {
                Query_Service.Query_Service qs = new Query_Service.Query_Service();
                DataSet ds = qs.GetB2cReturnDetail(transaction_id, draw_id);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    dgList.Visible = true;
                    dgList.DataSource = ds.Tables[0].DefaultView;
                    dgList.DataBind();
                }
                else
                {
                    this.dgList.Visible = false;
                    WebUtils.ShowMessage(this.Page, "û�в�ѯ����¼");
                }
            }
            catch (SoapException eSoap) //����soap���쳣
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message);
                WebUtils.ShowMessage(this.Page, "���÷������" + errStr);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "��ȡ����ʧ�ܣ�" + eSys.Message);
            }
        }


        //���Ľ����˿����鰴ť
        public void DataGrid1_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            //if (e.Item.ItemType == ListItemType.Item)
            //{
                
                string order_exist = e.Item.Cells[10].Text.Trim();//���Ķ����Ƿ����
                                 
                    if (order_exist == "�˿�ɹ�")
                    {
                        object obj = e.Item.Cells[12].FindControl("DetailID");
                        if (obj != null)
                        {
                            Button lb = (Button)obj;
                            lb.Visible = true;
                        }
                }
          //  }
        }

    }

}

