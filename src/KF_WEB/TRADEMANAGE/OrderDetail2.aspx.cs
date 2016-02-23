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
    /// OrderDetail 的摘要说明。
    /// </summary>
    public partial class OrderDetail2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {
            // 在此处放置用户代码以初始化页面

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
                    WebUtils.ShowMessage(this.Page, "参数有误！");
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
                catch (SoapException eSoap) //捕获soap类异常
                {
                    string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                    WebUtils.ShowMessage(this.Page, "调用服务出错：" + errStr);
                }
                catch (Exception eSys)
                {
                    WebUtils.ShowMessage(this.Page, "读取数据失败！" + eSys.Message.ToString());
                }
            }

        }

        private void BindInfo(string listid)
        {
            int iType = 1;

            //绑定交易资料信息
            FQuery_Service.Query_Service fs = new TENCENT.OSS.CFT.KF.KF_Web.FQuery_Service.Query_Service();
            FQuery_Service.Finance_Header Ffh = classLibrary.setConfig.FsetFH(this);
            fs.Finance_HeaderValue = Ffh;

            string selectStrSession = listid;//Session["ListID"].ToString();

            DateTime beginTime = DateTime.Parse(PublicRes.sBeginTime);
            DateTime endTime = DateTime.Parse(PublicRes.sEndTime);

            int istr = 1;
            int imax = 2;
            int fcurtype = 1;
            if (Request.QueryString["fcurtype"] != null)//rowena 20100722  增加基金项目
            {
                fcurtype = int.Parse(Request.QueryString["fcurtype"].Trim()); ;
            }

            DataSet ds = new DataSet();
            ds = fs.GetPayList(selectStrSession, iType, fcurtype, beginTime, endTime, istr, imax);
            if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
            {
                //如果没有数据，隐藏交易单显示界面
                throw new Exception("数据库无此记录");
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
            ht.Add("1", "财付通");
            ht.Add("2", "拍拍网");
            ht.Add("3", "客户端小钱包");
            ht.Add("4", "手机支付");
            ht.Add("5", "第三方");
            ht.Add("6", "ivr");
            ht.Add("7", "平台专用账户");
            ht.Add("8", "委托代扣"); //ht.Add("8", "基金基础代扣");
            ht.Add("9", "微支付");
            ht.Add("100", "手机客户端（手机充值卡充值财付通）");
            ht.Add("101", "手机财付通HTMl5支付中心（手机充值卡充值财付通）");
            ht.Add("102", "手机qq");
            ht.Add("103", "Pos收单");
            ht.Add("104", "微生活");
            ht.Add("105", "微信Web扫码支付");
            ht.Add("106", "微信app跳转支付");
            ht.Add("107", "微信公众帐号内支付");
            ht.Add("108", "手机支付-wap");
            ht.Add("109", "手机支付-HTML5");
            ht.Add("110", "手机支付-客户端");
            ht.Add("111", "手q支付");
            ht.Add("112", "数平SDK");
            //2015-2-27郭跃强添加
            ht.Add("113", "微信离线支付");
            ht.Add("114", "微信b2c转账");
            ht.Add("115", "微信平台余额支付");
            ht.Add("116", "微信c2c转账");
            ht.Add("117", "手Q b2c");
            //2016-02-23 添加
            ht.Add("118", "微信委托代扣");
            ht.Add("119", "手Q委托代扣");
            ht.Add("120", "手q c2c转账");
            ht.Add("121", "微信实时结算");
            ht.Add("122", "微信实时分润");
            ht.Add("123", "订单关单冲正");
            ht.Add("124", "财付通nfc支付（手q）");
            ht.Add("125", "手Q平台余额支付");
            ht.Add("126", "手Q扫码支付");
            ht.Add("127", "手Q公众号支付");
            ht.Add("128", "手Q境外支付");

            classLibrary.setConfig.DbtypeToPageContent(ds.Tables[0], "Fchannel_id", "Fchannel_id_str", ht);
            this.LB_Fchannel_id.Text = ds.Tables[0].Rows[0]["Fchannel_id_str"].ToString();

            //var tmpFchannel = ds.Tables[0].Rows[0]["Fchannel_id"].ToString();
            //if (tmpFchannel == "1")
            //{
            //    tmpFchannel = "财付通";
            //}
            //else if (tmpFchannel == "2")
            //{
            //    tmpFchannel = "拍拍网";
            //}
            //else if (tmpFchannel == "3")
            //{
            //    tmpFchannel = "客户端小钱包";
            //}
            //else if (tmpFchannel == "4")
            //{
            //    tmpFchannel = "手机支付";
            //}
            //else if (tmpFchannel == "5")
            //{
            //    tmpFchannel = "第三方";
            //}
            //else
            //{
            //    tmpFchannel = "未知类型" + tmpFchannel;
            //}
            //this.LB_Fchannel_id.Text = tmpFchannel;

            this.LB_Fmediuid.Text = ds.Tables[0].Rows[0]["Fmediuid"].ToString();
            this.LB_Fmedinum.Text = MoneyTransfer.FenToYuan(PublicRes.GetString(ds.Tables[0].Rows[0]["Fmedinum"]));

            this.LB_Fmodify_time.Text = ds.Tables[0].Rows[0]["Fmodify_time"].ToString();
            this.lbTradeType.Text = classLibrary.setConfig.convertPayType(ds.Tables[0].Rows[0]["Ftrade_type"].ToString().Trim());
            this.lbAdjustFlag.Text = classLibrary.setConfig.convertAdjustSign(ds.Tables[0].Rows[0]["Fadjust_flag"].ToString().Trim());
            this.LB_Flistid.Text = ds.Tables[0].Rows[0]["Flistid"].ToString();


            //Fchargeuid    Int    手续费账户内部id
            //Fchargenum    bigInt    本次交易总共收的手续费        
            //Ftotalnum    Bigint    本次交易总金额
            //Fbuyerpaytotal    Bigint    买家已支付总金额(总为负数)
            LB_Fchargeuid.Text = PublicRes.GetString(ds.Tables[0].Rows[0]["Fchargeuid"]);
            LB_Fchargenum.Text = MoneyTransfer.FenToYuan(PublicRes.GetString(ds.Tables[0].Rows[0]["Fchargenum"]));
            LB_Ftotalnum.Text = MoneyTransfer.FenToYuan(PublicRes.GetString(ds.Tables[0].Rows[0]["Ftotalnum"]));
            LB_Fbuyerpaytotal.Text = MoneyTransfer.FenToYuan(PublicRes.GetString(ds.Tables[0].Rows[0]["Fbuyerpaytotal"]));
            //Fbuyerrefundtotal    Bigint    买家已退款总金额(总为正数)
            //Fsellerpaytotal    Bigint    卖家已收到总金额(总为正数)
            //Fsellerrefundtotal    Bigint    卖家已退款总金额(总为负数)    
            //Frolenum    int    参与方数量,最多为8个
            LB_Fbuyerrefundtotal.Text = MoneyTransfer.FenToYuan(PublicRes.GetString(ds.Tables[0].Rows[0]["Fbuyerrefundtotal"]));
            LB_Fsellerpaytotal.Text = MoneyTransfer.FenToYuan(PublicRes.GetString(ds.Tables[0].Rows[0]["Fsellerpaytotal"]));
            LB_Fsellerrefundtotal.Text = MoneyTransfer.FenToYuan(PublicRes.GetString(ds.Tables[0].Rows[0]["Fsellerrefundtotal"]));
            LB_Frolenum.Text = PublicRes.GetString(ds.Tables[0].Rows[0]["Frolenum"]);

            //Fuid0    int    内部ID号码
            //Fplanpaynum0    bigint    预计支付(收入)金额 负表示出 正表示入
            //Fpaynum0    Bigint    实际支付(收入)金额 负表示出 正表示入
            //Frefund0    bigint    退款金额 符号和paynum相反
            LB_Fuid0.Text = PublicRes.GetString(ds.Tables[0].Rows[0]["Fuid0"]);
            LB_Fplanpaynum0.Text = MoneyTransfer.FenToYuan(PublicRes.GetString(ds.Tables[0].Rows[0]["Fplanpaynum0"]));
            LB_Fpaynum0.Text = MoneyTransfer.FenToYuan(PublicRes.GetString(ds.Tables[0].Rows[0]["Fpaynum0"]));
            LB_Frefund0.Text = MoneyTransfer.FenToYuan(PublicRes.GetString(ds.Tables[0].Rows[0]["Frefund0"]));

            //Fuid1    int    内部ID号码
            //Fplanpaynum1    bigint    预计支付(收入)金额 负表示出 正表示入
            //Fpaynum1    Bigint    实际支付(收入)金额 负表示出 正表示入
            //Frefund1    bigint    退款金额 符号和paynum相反
            LB_Fuid1.Text = PublicRes.GetString(ds.Tables[0].Rows[0]["Fuid1"]);
            LB_Fplanpaynum1.Text = MoneyTransfer.FenToYuan(PublicRes.GetString(ds.Tables[0].Rows[0]["Fplanpaynum1"]));
            LB_Fpaynum1.Text = MoneyTransfer.FenToYuan(PublicRes.GetString(ds.Tables[0].Rows[0]["Fpaynum1"]));
            LB_Frefund1.Text = MoneyTransfer.FenToYuan(PublicRes.GetString(ds.Tables[0].Rows[0]["Frefund1"]));

            //Fuid2    int    内部ID号码
            //Fplanpaynum2    bigint    预计支付(收入)金额 负表示出 正表示入
            //Fpaynum2    Bigint    实际支付(收入)金额 负表示出 正表示入
            //Frefund2    bigint    退款金额 符号和paynum相反
            LB_Fuid2.Text = PublicRes.GetString(ds.Tables[0].Rows[0]["Fuid2"]);
            LB_Fplanpaynum2.Text = MoneyTransfer.FenToYuan(PublicRes.GetString(ds.Tables[0].Rows[0]["Fplanpaynum2"]));
            LB_Fpaynum2.Text = MoneyTransfer.FenToYuan(PublicRes.GetString(ds.Tables[0].Rows[0]["Fpaynum2"]));
            LB_Frefund2.Text = MoneyTransfer.FenToYuan(PublicRes.GetString(ds.Tables[0].Rows[0]["Frefund2"]));

            //Fuid3    int    内部ID号码
            //Fplanpaynum3    bigint    预计支付(收入)金额 负表示出 正表示入
            //Fpaynum3    Bigint    实际支付(收入)金额 负表示出 正表示入
            //Frefund3    bigint    退款金额 符号和paynum相反
            LB_Fuid3.Text = PublicRes.GetString(ds.Tables[0].Rows[0]["Fuid3"]);
            LB_Fplanpaynum3.Text = MoneyTransfer.FenToYuan(PublicRes.GetString(ds.Tables[0].Rows[0]["Fplanpaynum3"]));
            LB_Fpaynum3.Text = MoneyTransfer.FenToYuan(PublicRes.GetString(ds.Tables[0].Rows[0]["Fpaynum3"]));
            LB_Frefund3.Text = MoneyTransfer.FenToYuan(PublicRes.GetString(ds.Tables[0].Rows[0]["Frefund3"]));


            string strtmp = "";

            //显示投诉单信息.
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
                        dr["FstateName"] = "初始状态";
                    }
                    else if (strtmp == "1")
                    {
                        dr["FstateName"] = "申请投诉";
                    }
                    else if (strtmp == "2")
                    {
                        dr["FstateName"] = "投诉处理中";
                    }
                    else if (strtmp == "3")
                    {
                        dr["FstateName"] = "投诉处理完毕";
                    }
                    else if (strtmp == "4")
                    {
                        dr["FstateName"] = "取消投诉";
                    }
                    else
                    {
                        dr["FstateName"] = "未知类型" + strtmp;
                    }

                    strtmp = dr["Fpunish_flag"].ToString();
                    if (strtmp == "1")
                    {
                        dr["Fpunish_flagName"] = "无需处罚";
                    }
                    else if (strtmp == "2")
                    {
                        dr["Fpunish_flagName"] = "处罚买家";
                    }
                    else if (strtmp == "3")
                    {
                        dr["Fpunish_flagName"] = "处罚卖家";
                    }
                    else
                    {
                        dr["Fpunish_flagName"] = "未知类型" + strtmp;
                    }

                    strtmp = dr["Fcheck_state"].ToString();
                    if (strtmp == "1")
                    {
                        dr["Fcheck_stateName"] = "未审核";
                    }
                    else if (strtmp == "2")
                    {
                        dr["Fcheck_stateName"] = "已提交审核";
                    }
                    else if (strtmp == "3")
                    {
                        dr["Fcheck_stateName"] = "审核不通过";
                    }
                    else if (strtmp == "4")
                    {
                        dr["Fcheck_stateName"] = "审核通过";
                    }
                    else if (strtmp == "5")
                    {
                        dr["Fcheck_stateName"] = "已退款";
                    }
                    else
                    {
                        dr["Fcheck_stateName"] = "未知类型" + strtmp;
                    }


                    strtmp = dr["Fappeal_type"].ToString();
                    if (strtmp == "1")
                    {
                        dr["Fappeal_typeName"] = "成交不买";
                    }
                    else if (strtmp == "2")
                    {
                        dr["Fappeal_typeName"] = "收货不付款（确认）";
                    }
                    else if (strtmp == "3")
                    {
                        dr["Fappeal_typeName"] = "（卖家投诉买家）退款纠纷";
                    }
                    else if (strtmp == "4")
                    {
                        dr["Fappeal_typeName"] = "买家恶意评价";
                    }
                    else if (strtmp == "5")
                    {
                        dr["Fappeal_typeName"] = "成交不卖";
                    }
                    else if (strtmp == "6")
                    {
                        dr["Fappeal_typeName"] = "卖家拒绝使用财付通交易";
                    }
                    else if (strtmp == "7")
                    {
                        dr["Fappeal_typeName"] = "收款不发货";
                    }
                    else if (strtmp == "8")
                    {
                        dr["Fappeal_typeName"] = "商品与描述不符";
                    }
                    else if (strtmp == "9")
                    {
                        dr["Fappeal_typeName"] = "卖家恶意评价";
                    }
                    else if (strtmp == "10")
                    {
                        dr["Fappeal_typeName"] = "（买家投诉卖家）退款纠纷";
                    }
                    else if (strtmp == "11")
                    {
                        dr["Fappeal_typeName"] = "卖家要求买家先确认收货";
                    }
                    else
                    {
                        dr["Fappeal_typeName"] = "未知类型" + strtmp;
                    }
                }

                DataGrid1.DataSource = dsappeal.Tables[0].DefaultView;
            }

            //显示交易流水信息.
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
                        dr["FtypeName"] = "入";
                    }
                    else if (strtmp == "2")
                    {
                        dr["FtypeName"] = "出";
                    }
                    else
                    {
                        dr["FtypeName"] = "未知类型" + strtmp;
                    }
                }

                classLibrary.setConfig.FenToYuan_Table(dsuserpay.Tables[0], "Fpaynum", "FpaynumName");
                classLibrary.setConfig.FenToYuan_Table(dsuserpay.Tables[0], "Fpaybuy", "FpaybuyName");
                classLibrary.setConfig.FenToYuan_Table(dsuserpay.Tables[0], "Fpaysale", "FpaysaleName");

                DataGrid2.DataSource = dsuserpay.Tables[0].DefaultView;
            }

            //显示物流单
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
                        dr["Ftransport_typeName"] = "卖方发货";
                    }
                    else if (strtmp == "2")
                    {
                        dr["Ftransport_typeName"] = "买方发送退货";
                    }
                    else
                    {
                        dr["Ftransport_typeName"] = "未知类型" + strtmp;
                    }

                    strtmp = dr["Fgoods_type"].ToString();
                    if (strtmp == "1")
                    {
                        dr["Fgoods_typeName"] = "实物物品";
                    }
                    else if (strtmp == "2")
                    {
                        dr["Fgoods_typeName"] = "虚拟物品";
                    }
                    else
                    {
                        dr["Fgoods_typeName"] = "未知类型" + strtmp;
                    }

                    strtmp = dr["Ftran_type"].ToString();
                    if (strtmp == "1")
                    {
                        dr["Ftran_typeName"] = "平邮";
                    }
                    else if (strtmp == "2")
                    {
                        dr["Ftran_typeName"] = "快递";
                    }
                    else if (strtmp == "3")
                    {
                        dr["Ftran_typeName"] = "email发货";
                    }
                    else if (strtmp == "4")
                    {
                        dr["Ftran_typeName"] = "手机";
                    }
                    else if (strtmp == "5")
                    {
                        dr["Ftran_typeName"] = "其它";
                    }
                    else
                    {
                        dr["Ftran_typeName"] = "未知类型" + strtmp;
                    }
                }

                Datagrid3.DataSource = dstransport.Tables[0].DefaultView;
            }
            this.DataBind();
        }

    }
}
