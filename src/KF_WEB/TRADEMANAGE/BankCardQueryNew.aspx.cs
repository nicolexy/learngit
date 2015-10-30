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
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;
using System.IO;
using CFT.CSOMS.BLL.WechatPay;
using System.Collections.Generic;


namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{

    /// <summary>
    /// BankCardQueryNew ��ժҪ˵����
    /// </summary>
    public partial class BankCardQueryNew : System.Web.UI.Page
    {
        private static log4net.ILog s_log;
        private static log4net.ILog log
        {
            get
            {
                if (s_log == null)
                {
                    s_log = log4net.LogManager.GetLogger("���п���ѯ���£�");
                }

                return s_log;
            }
        }
        protected void Page_Load(object sender, System.EventArgs e)
        {

            try
            {
                Label1.Text = Session["uid"].ToString();
                string szkey = Session["SzKey"].ToString();
                int operid = Int32.Parse(Session["OperID"].ToString());

                //if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID,"InfoCenter")) Response.Redirect("../login.aspx?wh=1");
                if (!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter", this)) Response.Redirect("../login.aspx?wh=1");
            }
            catch
            {
                Response.Redirect("../login.aspx?wh=1");
            }

            if (!IsPostBack)
            {
                TextBoxDate.Text = DateTime.Now.ToString("yyyy��MM��dd��");
                RequestBankInfo();
                Table2.Visible = false;
            }
        }

        private void ShowMsg(string msg)
        {
            Response.Write("<script language=javascript>alert('" + msg + "')</script>");
        }

        /// <summary>
        /// ����������
        /// </summary>
        class PolyphoneIComparer : IComparer
        {
            /// <summary>
            /// ������ ��ͬ����  ���бȽ�
            /// </summary>
            static IDictionary<string, string> PolyphoneDic = new Dictionary<string, string>()
            {
                { "����" , "����" }
            };


            public int Compare(object x, object y)
            {
                var xx = x as string;
                var yy = y as string;

                PolyphoneHandle(ref xx);
                PolyphoneHandle(ref yy);
                return string.Compare(xx, yy);
            }

            private void PolyphoneHandle(ref string zw)
            {
                if (!string.IsNullOrEmpty(zw))
                {
                    foreach (var item in PolyphoneDic.Keys)
                    {
                        if (zw.StartsWith(item))
                        {
                            zw = PolyphoneDic[item];
                            return;
                        }
                    }
                }

            }
        }
        private void RequestBankInfo()
        {

            try
            {
                Hashtable ht = new FastPayService().RequestBankInfo();
                ArrayList akeys = new ArrayList(ht.Keys);
                akeys.Sort(new PolyphoneIComparer());
                foreach (string k in akeys)
                {
                    DropOldBankType.Items.Add(new System.Web.UI.WebControls.ListItem(k.ToString(), ht[k].ToString()));
                }
            }
            catch (Exception ex)
            {
                log.ErrorFormat("����RequestBankInfo����ԭ��{0}", ex.Message.ToString());
            }

        }


        public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            pager.CurrentPageIndex = e.NewPageIndex;
            BindData(e.NewPageIndex);
        }

        protected void btnSearch_Click(object sender, System.EventArgs e)
        {
            try
            {
                ValidateDate();
            }
            catch (Exception err)
            {
                ShowMsg(err.Message);
                return;
            }

            try
            {
                Table2.Visible = true;
                pager.RecordCount = 1000;
                BindData(1);

            }
            catch (SoapException eSoap) //����soap���쳣
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                string strTips = string.Format("����btnSearch_Click����ԭ��{0}", eSoap.Message.ToString());
                log.ErrorFormat("����btnSearch_Click����ԭ��{0}", eSoap.Message.ToString());
                ShowMsg(strTips);

            }
            catch (Exception eSys)
            {
                log.ErrorFormat(eSys.Message.ToString());
                ShowMsg(eSys.Message.ToString());
            }
        }

        private void ValidateDate()
        {


            DateTime Date;

            try
            {
                Date = DateTime.Parse(TextBoxDate.Text);
            }
            catch
            {
                throw new Exception("������������");
            }

            ViewState["Date"] = Date.ToString("yyyyMMdd");
            ViewState["fpay_acc"] = this.txtBankCardID.Text.Trim();
            ViewState["bank_type"] = DropOldBankType.SelectedItem.Value;
            ViewState["bank_name"] = DropOldBankType.SelectedItem.Text;


            if (ViewState["fpay_acc"].ToString() == "")
            {
                throw new Exception("���������п��ţ�");
            }
        }


        private void BindData(int index)
        {
            try
            {
                int max = pager.PageSize;
                //     int start = max * (index - 1) + 1;
                int start = max * (index - 1);
                //  Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
                // DataSet ds = qs.Getfbank_orderList(ViewState["fpay_acc"].ToString(), ViewState["Date"].ToString(), start, max);
                DataSet ds = new FastPayService().QueryBankCardNewList(ViewState["fpay_acc"].ToString(), ViewState["Date"].ToString(), ViewState["bank_type"].ToString(), int.Parse(this.ddlBizType.SelectedValue), start, max);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataGrid1.DataSource = ds.Tables[0].DefaultView;
                    DataGrid1.DataBind();
                }
                else
                {
                    ShowMsg("û���ҵ���¼��");
                }
            }
            catch (Exception eSys)
            {
                string errStr = PublicRes.GetErrorMsg(eSys.Message.ToString());

                log.ErrorFormat("����={0}, ����={1}, ��������={2}, ��������={3} ,����ԭ��={4}", ViewState["fpay_acc"].ToString(), ViewState["Date"].ToString(),
                    ViewState["bank_name"].ToString(), ViewState["bank_type"].ToString(), eSys.Message.ToString());
                WebUtils.ShowMessage(this.Page, "��ȡ����ʧ�ܣ�" + errStr);

            }

        }

        private void DataToGrid(DataSet ds)
        {
            ds.Tables[0].Columns.Add("FamtStr", typeof(string));
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                dr["FamtStr"] = MoneyTransfer.FenToYuan(dr["Famt"].ToString());
            }
            DataGrid1.DataSource = ds.Tables[0].DefaultView;
            DataGrid1.DataBind();
        }

        public void btnBatchQuery(object sender, System.EventArgs e)
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds.Tables.Add(dt);

                #region ����excel�ļ�
                if (!File1.HasFile)
                {
                    ShowMsg("��ѡ���ϴ��ļ���");
                    return;
                }
                if (Path.GetExtension(File1.FileName).ToLower() == ".xls")
                {
                    string uin = Session["uid"].ToString();
                    string path = Server.MapPath("~/") + "PLFile" + "\\bankcard" + uin + ".xls";
                    File1.PostedFile.SaveAs(path);

                    bool isFirst = true;
                    string errMsg = "";

                    DataSet res_ds = PublicRes.readXls(path);
                    System.Data.DataTable res_dt = res_ds.Tables[0];
                    int iColums = res_dt.Columns.Count;
                    int iRows = res_dt.Rows.Count;
                    int nMax = 50;
                    if (iRows > nMax)
                    {
                        string strTips = string.Format("����������ദ��{0}������", nMax);
                        throw new Exception(strTips);
                    }
                    for (int i = 0; i < iRows; i++)
                    {
                        string r1 = res_dt.Rows[i][0].ToString();
                        string r2 = res_dt.Rows[i][1].ToString();
                        string r3 = res_dt.Rows[i][2].ToString();
                        ListItem item = DropOldBankType.Items.FindByText(r3.Trim());

                        if (item == null || string.IsNullOrEmpty(item.Value))
                        {
                            string strTips = string.Format("����:{0}��Ӧ��������{1} �Ҳ�����������ֵ��", r1, r3);
                            ShowMsg(strTips);
                            return;
                        }

                        try
                        {
                            string date = null;
                            try
                            {
                                date = DateTime.Parse(r2).ToString("yyyyMMdd");
                            }
                            catch
                            {
                                string strTips = string.Format("{0}������������!", r2);
                                log.ErrorFormat("{0}������������!", r2);
                                ShowMsg(strTips);
                                return;
                            }


                            //�ӷ�ҳ��ǣ���Ϊ-1����ҳ
                            DataSet tmpDs = new FastPayService().QueryBankCardNewList(r1, date, item.Value, 10100, -1, -1);
                            if (tmpDs != null && tmpDs.Tables.Count > 0 && tmpDs.Tables[0].Rows.Count > 0)
                            {
                                foreach (DataRow dr in tmpDs.Tables[0].Rows)
                                {
                                    if (isFirst)
                                    {
                                        //�����һ�Σ����������
                                        for (int j = 0; j < dr.Table.Columns.Count; j++)
                                        {
                                            dt.Columns.Add(dr.Table.Columns[j].ColumnName);
                                        }
                                        isFirst = false;
                                    }

                                    dt.ImportRow(dr);
                                }
                            }


                        }
                        catch (Exception ex)
                        {
                            errMsg += ex.Message;
                            throw new Exception(errMsg);
                        }
                    }
                }
                else
                {
                    ShowMsg("�ļ���ʽ����ȷ����ѡ��xls��ʽ�ļ��ϴ���");
                    return;
                }
                #endregion

                #region ����excel�ļ�

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    StringWriter sw = new StringWriter();
                    sw.WriteLine("���п���\t���ж�����\t���\tҵ��״̬");

                    foreach (DataRow dr in dt.Rows)
                    {
                        sw.WriteLine("=\"" + dr["fpay_acc"].ToString() + "\"\t=\"" + dr["fbank_order"].ToString() + "\"\t=\"" + dr["FamtStr"]
                            + "\"\t=\"" + dr["Fbiz_type_str"] + "\"");

                    }
                    sw.Close();
                    //string f_name = "���п�������ѯ";
                    //f_name = System.Web.HttpUtility.UrlEncode(System.Text.Encoding.UTF8.GetBytes(f_name));
                    Response.AddHeader("Content-Disposition", "attachment; filename=���п�������ѯ.xls");
                    Response.ContentType = "application/ms-excel";
                    Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
                    Response.Write(sw);
                    Response.End();
                }
                else
                {
                    this.ShowMsg("û�����ݽ������ļ�");
                }
                #endregion
            }
            catch (Exception eSys)
            {
                log.ErrorFormat("����btnBatchQuery����ԭ��{0}", eSys.Message.ToString());
                // ShowMsg(eSys.Message.ToString());
            }
        }
    }
}
