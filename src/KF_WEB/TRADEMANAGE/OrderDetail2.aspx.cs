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
using Tencent.DotNet.OSS.Web.UI;
using System.Web.Services.Protocols;
using TENCENT.OSS.C2C.Finance.Common;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.CFT.KF.KF_Web;


namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
    /// <summary>
    /// OrderDetail ��ժҪ˵����
    /// </summary>
    public partial class OrderDetail2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {
            // �ڴ˴������û������Գ�ʼ��ҳ��

            var uid = Session["uid"] as string;
            if (string.IsNullOrEmpty(uid))
            {
                Response.Redirect("../login.aspx?wh=1");
            }

            if (!IsPostBack)
            {
                string listid = Request.QueryString["listid"];

                if (string.IsNullOrEmpty(listid))
                {
                    WebUtils.ShowMessage(this.Page, "��������");
                }

                Session["ListID"] = listid;

                try
                {
                    BindInfo(listid);
                }
                catch (LogicException err)
                {
                    WebUtils.ShowMessage(this.Page, err.Message);
                }
                catch (SoapException eSoap) //����soap���쳣
                {
                    string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                    WebUtils.ShowMessage(this.Page, "���÷������" + errStr);
                }
                catch (Exception eSys)
                {
                    WebUtils.ShowMessage(this.Page, "��ȡ����ʧ�ܣ�" + eSys.Message.ToString());
                }
            }

        }

        private void BindInfo(string listid)
        {
            int iType = 1;

            //�󶨽���������Ϣ
            FQuery_Service.Query_Service fs = new TENCENT.OSS.CFT.KF.KF_Web.FQuery_Service.Query_Service();
            FQuery_Service.Finance_Header Ffh = classLibrary.setConfig.FsetFH(this);
            fs.Finance_HeaderValue = Ffh;

            string selectStrSession = listid;//Session["ListID"].ToString();

            DateTime beginTime = DateTime.Parse(PublicRes.sBeginTime);
            DateTime endTime = DateTime.Parse(PublicRes.sEndTime);

            int istr = 1;
            int imax = 2;
            int fcurtype = 1;
            if (Request.QueryString["fcurtype"] != null)//rowena 20100722  ���ӻ�����Ŀ
            {
                fcurtype = int.Parse(Request.QueryString["fcurtype"].Trim()); ;
            }

            DataSet ds = new DataSet();
            ds = fs.GetPayList(selectStrSession, iType, fcurtype, beginTime, endTime, istr, imax);
            if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
            {
                //���û�����ݣ����ؽ��׵���ʾ����
                throw new Exception("���ݿ��޴˼�¼");
            }
            //                Response.Write("DS:" + ds.Tables[0].Rows[0][0].ToString());
            this.LB_Fbank_backid.Text = ds.Tables[0].Rows[0]["Fbank_backid"].ToString();
            this.LB_Fbank_listid.Text = ds.Tables[0].Rows[0]["Fbank_listid"].ToString();
            this.LB_Fspid.Text = ds.Tables[0].Rows[0]["Fspid"].ToString().Trim();
            this.LB_Fcurtype.Text = classLibrary.setConfig.convertMoney_type(ds.Tables[0].Rows[0]["Fcurtype"].ToString());
            this.DropDownList2_tradeState.SelectedValue = ds.Tables[0].Rows[0]["Fstate"].ToString();

            this.LB_Flstate.Text = classLibrary.setConfig.convertTradeState(ds.Tables[0].Rows[0]["Flstate"].ToString());
            hlOrder.NavigateUrl = "TradeLogQuery.aspx?id=" + listid;

            this.LB_Fcreate_time.Text = ds.Tables[0].Rows[0]["Fcreate_time"].ToString();
            this.LB_Fip.Text = ds.Tables[0].Rows[0]["Fip"].ToString();
            this.LB_FMemo.Text = ds.Tables[0].Rows[0]["Fmemo"].ToString();
            this.LB_Fexplain.Text = ds.Tables[0].Rows[0]["Fexplain"].ToString();
            this.LB_Fcatch_desc.Text = ds.Tables[0].Rows[0]["Fcatch_desc"].ToString();

            this.LB_Fpay_type.Text = classLibrary.setConfig.cPay_type(ds.Tables[0].Rows[0]["Fpay_type"].ToString());

            ds.Tables[0].Columns.Add("Fchannel_id_str");

            Hashtable ht = new Hashtable();
            ht.Add("1", "�Ƹ�ͨ");
            ht.Add("2", "������");
            ht.Add("3", "�ͻ���СǮ��");
            ht.Add("4", "�ֻ�֧��");
            ht.Add("5", "������");
            ht.Add("6", "ivr");
            ht.Add("7", "ƽ̨ר���˻�");
            ht.Add("8", "ί�д���"); //ht.Add("8", "�����������");
            ht.Add("9", "΢֧��");
            ht.Add("100", "�ֻ��ͻ��ˣ��ֻ���ֵ����ֵ�Ƹ�ͨ��");
            ht.Add("101", "�ֻ��Ƹ�ͨHTMl5֧�����ģ��ֻ���ֵ����ֵ�Ƹ�ͨ��");
            ht.Add("102", "�ֻ�qq");
            ht.Add("103", "Pos�յ�");
            ht.Add("104", "΢����");
            ht.Add("105", "΢��Webɨ��֧��");
            ht.Add("106", "΢��app��ת֧��");
            ht.Add("107", "΢�Ź����ʺ���֧��");
            ht.Add("108", "�ֻ�֧��-wap");
            ht.Add("109", "�ֻ�֧��-HTML5");
            ht.Add("110", "�ֻ�֧��-�ͻ���");
            ht.Add("111", "��q֧��");
            ht.Add("112", "��ƽSDK");
            //2015-2-27��Ծǿ���
            ht.Add("113", "΢������֧��");
            ht.Add("114", "΢��b2cת��");
            ht.Add("115", "΢��ƽ̨���֧��");
            ht.Add("116", "΢��c2cת��");
            ht.Add("117", "��Q b2c");
            //2016-02-23 ���
            ht.Add("118", "΢��ί�д���");
            ht.Add("119", "��Qί�д���");
            ht.Add("120", "��q c2cת��");
            ht.Add("121", "΢��ʵʱ����");
            ht.Add("122", "΢��ʵʱ����");
            ht.Add("123", "�����ص�����");
            ht.Add("124", "�Ƹ�ͨnfc֧������q��");
            ht.Add("125", "��Qƽ̨���֧��");
            ht.Add("126", "��Qɨ��֧��");
            ht.Add("127", "��Q���ں�֧��");
            ht.Add("128", "��Q����֧��");

            classLibrary.setConfig.DbtypeToPageContent(ds.Tables[0], "Fchannel_id", "Fchannel_id_str", ht);
            this.LB_Fchannel_id.Text = ds.Tables[0].Rows[0]["Fchannel_id_str"].ToString();

            //var tmpFchannel = ds.Tables[0].Rows[0]["Fchannel_id"].ToString();
            //if (tmpFchannel == "1")
            //{
            //    tmpFchannel = "�Ƹ�ͨ";
            //}
            //else if (tmpFchannel == "2")
            //{
            //    tmpFchannel = "������";
            //}
            //else if (tmpFchannel == "3")
            //{
            //    tmpFchannel = "�ͻ���СǮ��";
            //}
            //else if (tmpFchannel == "4")
            //{
            //    tmpFchannel = "�ֻ�֧��";
            //}
            //else if (tmpFchannel == "5")
            //{
            //    tmpFchannel = "������";
            //}
            //else
            //{
            //    tmpFchannel = "δ֪����" + tmpFchannel;
            //}
            //this.LB_Fchannel_id.Text = tmpFchannel;

            this.LB_Fmediuid.Text = ds.Tables[0].Rows[0]["Fmediuid"].ToString();
            this.LB_Fmedinum.Text = MoneyTransfer.FenToYuan(PublicRes.GetString(ds.Tables[0].Rows[0]["Fmedinum"]));

            this.LB_Fmodify_time.Text = ds.Tables[0].Rows[0]["Fmodify_time"].ToString();
            this.lbTradeType.Text = classLibrary.setConfig.convertPayType(ds.Tables[0].Rows[0]["Ftrade_type"].ToString().Trim());
            this.lbAdjustFlag.Text = classLibrary.setConfig.convertAdjustSign(ds.Tables[0].Rows[0]["Fadjust_flag"].ToString().Trim());
            this.LB_Flistid.Text = ds.Tables[0].Rows[0]["Flistid"].ToString();


            //Fchargeuid    Int    �������˻��ڲ�id
            //Fchargenum    bigInt    ���ν����ܹ��յ�������        
            //Ftotalnum    Bigint    ���ν����ܽ��
            //Fbuyerpaytotal    Bigint    �����֧���ܽ��(��Ϊ����)
            LB_Fchargeuid.Text = PublicRes.GetString(ds.Tables[0].Rows[0]["Fchargeuid"]);
            LB_Fchargenum.Text = MoneyTransfer.FenToYuan(PublicRes.GetString(ds.Tables[0].Rows[0]["Fchargenum"]));
            LB_Ftotalnum.Text = MoneyTransfer.FenToYuan(PublicRes.GetString(ds.Tables[0].Rows[0]["Ftotalnum"]));
            LB_Fbuyerpaytotal.Text = MoneyTransfer.FenToYuan(PublicRes.GetString(ds.Tables[0].Rows[0]["Fbuyerpaytotal"]));
            //Fbuyerrefundtotal    Bigint    ������˿��ܽ��(��Ϊ����)
            //Fsellerpaytotal    Bigint    �������յ��ܽ��(��Ϊ����)
            //Fsellerrefundtotal    Bigint    �������˿��ܽ��(��Ϊ����)    
            //Frolenum    int    ���뷽����,���Ϊ8��
            LB_Fbuyerrefundtotal.Text = MoneyTransfer.FenToYuan(PublicRes.GetString(ds.Tables[0].Rows[0]["Fbuyerrefundtotal"]));
            LB_Fsellerpaytotal.Text = MoneyTransfer.FenToYuan(PublicRes.GetString(ds.Tables[0].Rows[0]["Fsellerpaytotal"]));
            LB_Fsellerrefundtotal.Text = MoneyTransfer.FenToYuan(PublicRes.GetString(ds.Tables[0].Rows[0]["Fsellerrefundtotal"]));
            LB_Frolenum.Text = PublicRes.GetString(ds.Tables[0].Rows[0]["Frolenum"]);

            //Fuid0    int    �ڲ�ID����
            //Fplanpaynum0    bigint    Ԥ��֧��(����)��� ����ʾ�� ����ʾ��
            //Fpaynum0    Bigint    ʵ��֧��(����)��� ����ʾ�� ����ʾ��
            //Frefund0    bigint    �˿��� ���ź�paynum�෴
            LB_Fuid0.Text = PublicRes.GetString(ds.Tables[0].Rows[0]["Fuid0"]);
            LB_Fplanpaynum0.Text = MoneyTransfer.FenToYuan(PublicRes.GetString(ds.Tables[0].Rows[0]["Fplanpaynum0"]));
            LB_Fpaynum0.Text = MoneyTransfer.FenToYuan(PublicRes.GetString(ds.Tables[0].Rows[0]["Fpaynum0"]));
            LB_Frefund0.Text = MoneyTransfer.FenToYuan(PublicRes.GetString(ds.Tables[0].Rows[0]["Frefund0"]));

            //Fuid1    int    �ڲ�ID����
            //Fplanpaynum1    bigint    Ԥ��֧��(����)��� ����ʾ�� ����ʾ��
            //Fpaynum1    Bigint    ʵ��֧��(����)��� ����ʾ�� ����ʾ��
            //Frefund1    bigint    �˿��� ���ź�paynum�෴
            LB_Fuid1.Text = PublicRes.GetString(ds.Tables[0].Rows[0]["Fuid1"]);
            LB_Fplanpaynum1.Text = MoneyTransfer.FenToYuan(PublicRes.GetString(ds.Tables[0].Rows[0]["Fplanpaynum1"]));
            LB_Fpaynum1.Text = MoneyTransfer.FenToYuan(PublicRes.GetString(ds.Tables[0].Rows[0]["Fpaynum1"]));
            LB_Frefund1.Text = MoneyTransfer.FenToYuan(PublicRes.GetString(ds.Tables[0].Rows[0]["Frefund1"]));

            //Fuid2    int    �ڲ�ID����
            //Fplanpaynum2    bigint    Ԥ��֧��(����)��� ����ʾ�� ����ʾ��
            //Fpaynum2    Bigint    ʵ��֧��(����)��� ����ʾ�� ����ʾ��
            //Frefund2    bigint    �˿��� ���ź�paynum�෴
            LB_Fuid2.Text = PublicRes.GetString(ds.Tables[0].Rows[0]["Fuid2"]);
            LB_Fplanpaynum2.Text = MoneyTransfer.FenToYuan(PublicRes.GetString(ds.Tables[0].Rows[0]["Fplanpaynum2"]));
            LB_Fpaynum2.Text = MoneyTransfer.FenToYuan(PublicRes.GetString(ds.Tables[0].Rows[0]["Fpaynum2"]));
            LB_Frefund2.Text = MoneyTransfer.FenToYuan(PublicRes.GetString(ds.Tables[0].Rows[0]["Frefund2"]));

            //Fuid3    int    �ڲ�ID����
            //Fplanpaynum3    bigint    Ԥ��֧��(����)��� ����ʾ�� ����ʾ��
            //Fpaynum3    Bigint    ʵ��֧��(����)��� ����ʾ�� ����ʾ��
            //Frefund3    bigint    �˿��� ���ź�paynum�෴
            LB_Fuid3.Text = PublicRes.GetString(ds.Tables[0].Rows[0]["Fuid3"]);
            LB_Fplanpaynum3.Text = MoneyTransfer.FenToYuan(PublicRes.GetString(ds.Tables[0].Rows[0]["Fplanpaynum3"]));
            LB_Fpaynum3.Text = MoneyTransfer.FenToYuan(PublicRes.GetString(ds.Tables[0].Rows[0]["Fpaynum3"]));
            LB_Frefund3.Text = MoneyTransfer.FenToYuan(PublicRes.GetString(ds.Tables[0].Rows[0]["Frefund3"]));


            string strtmp = "";

            //��ʾͶ�ߵ���Ϣ.
            DataSet dsappeal = fs.GetAppealList(listid);
            if (dsappeal != null && dsappeal.Tables.Count > 0)
            {
                dsappeal.Tables[0].Columns.Add("FstateName");
                dsappeal.Tables[0].Columns.Add("Fpunish_flagName");
                dsappeal.Tables[0].Columns.Add("Fcheck_stateName");
                dsappeal.Tables[0].Columns.Add("Fappeal_typeName");

                foreach (DataRow dr in dsappeal.Tables[0].Rows)
                {
                    strtmp = dr["FState"].ToString();
                    if (strtmp == "0")
                    {
                        dr["FstateName"] = "��ʼ״̬";
                    }
                    else if (strtmp == "1")
                    {
                        dr["FstateName"] = "����Ͷ��";
                    }
                    else if (strtmp == "2")
                    {
                        dr["FstateName"] = "Ͷ�ߴ�����";
                    }
                    else if (strtmp == "3")
                    {
                        dr["FstateName"] = "Ͷ�ߴ������";
                    }
                    else if (strtmp == "4")
                    {
                        dr["FstateName"] = "ȡ��Ͷ��";
                    }
                    else
                    {
                        dr["FstateName"] = "δ֪����" + strtmp;
                    }

                    strtmp = dr["Fpunish_flag"].ToString();
                    if (strtmp == "1")
                    {
                        dr["Fpunish_flagName"] = "���账��";
                    }
                    else if (strtmp == "2")
                    {
                        dr["Fpunish_flagName"] = "�������";
                    }
                    else if (strtmp == "3")
                    {
                        dr["Fpunish_flagName"] = "��������";
                    }
                    else
                    {
                        dr["Fpunish_flagName"] = "δ֪����" + strtmp;
                    }

                    strtmp = dr["Fcheck_state"].ToString();
                    if (strtmp == "1")
                    {
                        dr["Fcheck_stateName"] = "δ���";
                    }
                    else if (strtmp == "2")
                    {
                        dr["Fcheck_stateName"] = "���ύ���";
                    }
                    else if (strtmp == "3")
                    {
                        dr["Fcheck_stateName"] = "��˲�ͨ��";
                    }
                    else if (strtmp == "4")
                    {
                        dr["Fcheck_stateName"] = "���ͨ��";
                    }
                    else if (strtmp == "5")
                    {
                        dr["Fcheck_stateName"] = "���˿�";
                    }
                    else
                    {
                        dr["Fcheck_stateName"] = "δ֪����" + strtmp;
                    }


                    strtmp = dr["Fappeal_type"].ToString();
                    if (strtmp == "1")
                    {
                        dr["Fappeal_typeName"] = "�ɽ�����";
                    }
                    else if (strtmp == "2")
                    {
                        dr["Fappeal_typeName"] = "�ջ������ȷ�ϣ�";
                    }
                    else if (strtmp == "3")
                    {
                        dr["Fappeal_typeName"] = "������Ͷ����ң��˿����";
                    }
                    else if (strtmp == "4")
                    {
                        dr["Fappeal_typeName"] = "��Ҷ�������";
                    }
                    else if (strtmp == "5")
                    {
                        dr["Fappeal_typeName"] = "�ɽ�����";
                    }
                    else if (strtmp == "6")
                    {
                        dr["Fappeal_typeName"] = "���Ҿܾ�ʹ�òƸ�ͨ����";
                    }
                    else if (strtmp == "7")
                    {
                        dr["Fappeal_typeName"] = "�տ����";
                    }
                    else if (strtmp == "8")
                    {
                        dr["Fappeal_typeName"] = "��Ʒ����������";
                    }
                    else if (strtmp == "9")
                    {
                        dr["Fappeal_typeName"] = "���Ҷ�������";
                    }
                    else if (strtmp == "10")
                    {
                        dr["Fappeal_typeName"] = "�����Ͷ�����ң��˿����";
                    }
                    else if (strtmp == "11")
                    {
                        dr["Fappeal_typeName"] = "����Ҫ�������ȷ���ջ�";
                    }
                    else
                    {
                        dr["Fappeal_typeName"] = "δ֪����" + strtmp;
                    }
                }

                DataGrid1.DataSource = dsappeal.Tables[0].DefaultView;
            }

            //��ʾ������ˮ��Ϣ.
            DataSet dsuserpay = fs.GetUserpayList(listid);
            if (dsuserpay != null && dsuserpay.Tables.Count > 0)
            {
                dsuserpay.Tables[0].Columns.Add("FtypeName");
                dsuserpay.Tables[0].Columns.Add("FsubjectName");
                dsuserpay.Tables[0].Columns.Add("FpaynumName");
                dsuserpay.Tables[0].Columns.Add("FpaybuyName");
                dsuserpay.Tables[0].Columns.Add("FpaysaleName");

                classLibrary.setConfig.GetColumnValueFromDic(dsuserpay.Tables[0], "Fsubject", "FsubjectName", "USERPAY_SUBJECT");

                foreach (DataRow dr in dsuserpay.Tables[0].Rows)
                {
                    strtmp = dr["Ftype"].ToString();
                    if (strtmp == "1")
                    {
                        dr["FtypeName"] = "��";
                    }
                    else if (strtmp == "2")
                    {
                        dr["FtypeName"] = "��";
                    }
                    else
                    {
                        dr["FtypeName"] = "δ֪����" + strtmp;
                    }
                }

                classLibrary.setConfig.FenToYuan_Table(dsuserpay.Tables[0], "Fpaynum", "FpaynumName");
                classLibrary.setConfig.FenToYuan_Table(dsuserpay.Tables[0], "Fpaybuy", "FpaybuyName");
                classLibrary.setConfig.FenToYuan_Table(dsuserpay.Tables[0], "Fpaysale", "FpaysaleName");

                DataGrid2.DataSource = dsuserpay.Tables[0].DefaultView;
            }

            //��ʾ������
            DataSet dstransport = fs.GetTransportList(listid);
            if (dstransport != null && dstransport.Tables.Count > 0)
            {
                dstransport.Tables[0].Columns.Add("Ftransport_typeName");
                dstransport.Tables[0].Columns.Add("Fgoods_typeName");
                dstransport.Tables[0].Columns.Add("Ftran_typeName");
                dstransport.Tables[0].Columns.Add("FstateName");

                foreach (DataRow dr in dstransport.Tables[0].Rows)
                {
                    strtmp = dr["Ftransport_type"].ToString();
                    if (strtmp == "1")
                    {
                        dr["Ftransport_typeName"] = "��������";
                    }
                    else if (strtmp == "2")
                    {
                        dr["Ftransport_typeName"] = "�򷽷����˻�";
                    }
                    else
                    {
                        dr["Ftransport_typeName"] = "δ֪����" + strtmp;
                    }

                    strtmp = dr["Fgoods_type"].ToString();
                    if (strtmp == "1")
                    {
                        dr["Fgoods_typeName"] = "ʵ����Ʒ";
                    }
                    else if (strtmp == "2")
                    {
                        dr["Fgoods_typeName"] = "������Ʒ";
                    }
                    else
                    {
                        dr["Fgoods_typeName"] = "δ֪����" + strtmp;
                    }

                    strtmp = dr["Ftran_type"].ToString();
                    if (strtmp == "1")
                    {
                        dr["Ftran_typeName"] = "ƽ��";
                    }
                    else if (strtmp == "2")
                    {
                        dr["Ftran_typeName"] = "���";
                    }
                    else if (strtmp == "3")
                    {
                        dr["Ftran_typeName"] = "email����";
                    }
                    else if (strtmp == "4")
                    {
                        dr["Ftran_typeName"] = "�ֻ�";
                    }
                    else if (strtmp == "5")
                    {
                        dr["Ftran_typeName"] = "����";
                    }
                    else
                    {
                        dr["Ftran_typeName"] = "δ֪����" + strtmp;
                    }
                }

                Datagrid3.DataSource = dstransport.Tables[0].DefaultView;
            }
            this.DataBind();
        }

    }
}
