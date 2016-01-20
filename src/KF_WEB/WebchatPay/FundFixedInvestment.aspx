<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FundFixedInvestment.aspx.cs" Inherits="TENCENT.OSS.CFT.KF.KF_Web.WebchatPay.FundFixedInvestment" %>

<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>理财通定投定赎查询</title>
    <style type="text/css">
        caption {
            text-align: left;
            background: #b0c3d1;
            padding: 4px;
        }
        tr {
            height:25px
        }
        .bluequestionmark {
            background-repeat: no-repeat;
            display: inline-block;
            position: relative; /*the out div must be position:relative*/
        }

        .tipinfo {
            display: none;
            position: absolute;
            background-color:white;
            z-index:9999;
        }

        .bluequestionmark:hover .tipinfo {
            white-space: nowrap; /*the pop up infomation will show in one line*/
            display: block;
            border: 1px solid #0094ff;
            position: absolute;
            top: 18px;
            left: 25px;
            padding: 6px 10px;
            background-color: white;
            z-index:9999;
        }
    </style>
    <link type="text/css" rel="Stylesheet" href="../STYLES/ossstyle.css" />
    <link rel="Stylesheet" href="../Styles/ossstyle.css" />
</head>
<body>
    <form id="formMain" runat="server">
        <table border="1" cellspacing="1" cellpadding="1" width="1200">
            <tr>
                <td style="width: 100%" bgcolor="#e4e5f7" colspan="2"><font color="red">
                    <img src="../images/page/post.gif" width="20" height="16">理财通定投定赎预约买入查询</font>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:RadioButton ID="WeChatId" GroupName="USERTYPE" runat="server" Text="微信帐号" Checked="true" />
                    <asp:RadioButton ID="WeChatQQ" GroupName="USERTYPE" runat="server" Text="微信绑定QQ" />
                    <asp:RadioButton ID="WeChatMobile" GroupName="USERTYPE" runat="server" Text="微信绑定手机" />
                    <asp:RadioButton ID="WeChatEmail" GroupName="USERTYPE" runat="server" Text="微信绑定邮箱" />
                    <asp:RadioButton ID="WeChatUid" GroupName="USERTYPE" runat="server" Text="微信内部ID" />
                    <asp:RadioButton ID="WeChatCft" GroupName="USERTYPE" runat="server" Text="微信财付通账号Or手Q账号" />
                    &nbsp; &nbsp;
                     <asp:TextBox ID="txt_user" Width="200px" runat="server"></asp:TextBox>
                </td>

            </tr>
            <tr>
                <td style="width: 50%">计划类型：
                     <asp:RadioButton ID="DT" GroupName="PROJECT" runat="server" Text="定投" Checked="true" />
                    <asp:RadioButton ID="HFD" GroupName="PROJECT" runat="server" Text="还房贷" />
                </td>

                <td align="center">
                    <asp:Button ID="btnQuery" runat="server" Width="80px" Text="查 询" OnClick="btnQuery_Click"></asp:Button></td>
            </tr>
        </table>
        <br />

        <asp:DataGrid Width="1200" ID="dg_DT_fundBuyPlan" runat="server" AutoGenerateColumns="False" CssClass="tab_dg" Caption="计划列表(定投)" OnItemCommand="DataGrid1_ItemCommand">
            <HeaderStyle Font-Bold="True" Height="25px" />
            <Columns>
                <asp:BoundColumn DataField="Fplan_id" HeaderText="" Visible="false" />
                <asp:BoundColumn DataField="Fdesc" HeaderText="计划名称" />
                <asp:BoundColumn DataField="Ftype" HeaderText="类型" />
                <asp:BoundColumn DataField="Fcreate_Time" HeaderText="创建时间" />
                <asp:BoundColumn DataField="Ftotal_plan_fee" HeaderText="总计划金额" />
                <asp:BoundColumn DataField="Fplan_fee" HeaderText="单笔金额" />
                <asp:BoundColumn DataField="Ftotal_buy_fee" HeaderText="已申购总金额" />
                <asp:BoundColumn DataField="Fday" HeaderText="计划扣款日期" />
                <asp:BoundColumn DataField="Ffund_name" HeaderText="基金名称" />
<%--                <asp:BoundColumn DataField="Ffund_code" HeaderText="基金编码" />
                <asp:BoundColumn DataField="Fspid" HeaderText="商户号" />--%>
                <asp:BoundColumn DataField="Fbank_type" HeaderText="银行类型" />
                <asp:BoundColumn DataField="Fcard_tail" HeaderText="卡尾号" />
                <asp:BoundColumn DataField="Fstate" HeaderText="签约状态" />
                <asp:BoundColumn DataField="Flstate" HeaderText="计划状态" />
                <asp:ButtonColumn Text="其他" CommandName="other" />
                <asp:ButtonColumn Text="扣款记录" CommandName="KKrecord" />
            </Columns>
        </asp:DataGrid>

        <asp:DataGrid Width="1200" ID="dg_HFD_FundFetchPlan" runat="server" AutoGenerateColumns="False" CssClass="tab_dg" Caption="计划列表(定赎)" OnItemCommand="DataGrid4_ItemCommand">
            <HeaderStyle Font-Bold="True" Height="25px" />
            <Columns>
                <asp:BoundColumn DataField="Fplan_id" HeaderText="" Visible="false" />
                <asp:BoundColumn DataField="Fdesc" HeaderText="计划名称" />
                <asp:BoundColumn DataField="Ftype" HeaderText="类型" />
                <asp:BoundColumn DataField="Fcreate_Time" HeaderText="创建时间" />
                <asp:BoundColumn DataField="Ftotal_plan_fee" HeaderText="总计划金额" />
                <asp:BoundColumn DataField="Fplan_fee" HeaderText="一次还款金额" />
                <asp:BoundColumn DataField="Ftotal_fetch_fee" HeaderText="已还款总金额" />
                <asp:BoundColumn DataField="Fday" HeaderText="还款日期" />
                <asp:BoundColumn DataField="Ffetch_order_type" HeaderText="还款方式" />
                <%--        <asp:BoundColumn DataField="Ffund_code_list" HeaderText="还款基金列表" />
                <asp:BoundColumn DataField="Fspid_list" HeaderText="还款基金列表"/>--%>
                <asp:TemplateColumn HeaderText="还款基金列表">
                    <ItemTemplate>
                        <div class="bluequestionmark">
                             <asp:Label Text='<%# DataBinder.Eval(Container, "DataItem.Ffund_name_list1")  %>' runat="server"   ID="Label2" NAME="Label1">
                            </asp:Label>
                            <br />
                            <asp:Label class="tipinfo" Text='<%# DataBinder.Eval(Container, "DataItem.Ffund_name_list") %>' runat="server"   ID="Label1" NAME="Label1">
                            </asp:Label>
                        </div>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:BoundColumn DataField="Ffund_name" HeaderText="优先指定还款基金名称" />
             <%--   <asp:BoundColumn DataField="Ffund_code" HeaderText="优先指定还款基金" />
                <asp:BoundColumn DataField="Fspid" HeaderText="指定优先还款基金SPID" />--%>
                <asp:BoundColumn DataField="Flstate" HeaderText="计划状态" />
                <asp:BoundColumn DataField="Fbank_type" HeaderText="银行类型" />
                <asp:BoundColumn DataField="Fcard_tail" HeaderText="卡尾号" />
                <asp:ButtonColumn Text="其他" CommandName="other" />
                <asp:ButtonColumn Text="还款记录" CommandName="KKrecord" />
            </Columns>
        </asp:DataGrid>

        <webdiyer:AspNetPager ID="pager1" runat="server" AlwaysShow="True" NumericButtonCount="5" ShowCustomInfoSection="left" Width="1200px"
            PagingButtonSpacing="0" ShowInputBox="always" CssClass="mypager" HorizontalAlign="right" OnPageChanged="ChangePage1"
            SubmitButtonText="转到" NumericButtonTextFormatString="[{0}]">
        </webdiyer:AspNetPager>

        <asp:DataGrid ID="dg_fundBuyPlanByPlanid" runat="server" AutoGenerateColumns="False" CssClass="tab_dg" Caption="计划详情补充(定投)" Width="1200">
            <HeaderStyle Font-Bold="True" Height="25px" />
            <Columns>
                <asp:BoundColumn DataField="Fplan_first_fee" HeaderText="首笔金额" />
                <asp:BoundColumn DataField="Ftotal_profit" HeaderText="累计总收益（预留）" />
                <asp:BoundColumn DataField="Fprofit" HeaderText="昨日收益（预留）" />
                <asp:BoundColumn DataField="Ftotal_buy_times" HeaderText="已申购总次数" />
                <asp:BoundColumn DataField="Fcease_reason" HeaderText="终止原因" />
                <asp:BoundColumn DataField="Flast_pay_date" HeaderText="最近一次发起扣款日期" />
                <asp:BoundColumn DataField="Fnext_pay_date" HeaderText="下一次扣款日期" />
                <asp:BoundColumn DataField="Fmodify_Time" HeaderText="最后修改时间" />
            </Columns>
        </asp:DataGrid>

        <asp:DataGrid ID="dg_PlanBuyOrder" runat="server" AutoGenerateColumns="False" CssClass="tab_dg" Caption="扣款记录(定投)" Width="1200">
            <HeaderStyle Font-Bold="True" Height="25px" />
            <Columns>
                <asp:BoundColumn DataField="Fbuyid" HeaderText="申购单号" />
                <asp:BoundColumn DataField="Fpayid" HeaderText="代扣单号" />
                <asp:BoundColumn DataField="Ftotal_fee" HeaderText="金额" />
                <asp:BoundColumn DataField="Fstate" HeaderText="扣款状态" />
                <asp:BoundColumn DataField="Flstate" HeaderText="订单状态" />
                <asp:BoundColumn DataField="Ffund_name" HeaderText="基金名称" />
               <%-- <asp:BoundColumn DataField="Ffund_code" HeaderText="" />
                <asp:BoundColumn DataField="Fspid" HeaderText="商户号" />--%>
                <asp:BoundColumn DataField="Fbank_type" HeaderText="银行类型" />
                <asp:BoundColumn DataField="Fcard_tail" HeaderText="卡尾号" />
                <asp:BoundColumn DataField="Fplan_pay_date" HeaderText="计划扣款日期" />
                <asp:BoundColumn DataField="Ftry_pay_days" HeaderText="尝试扣款日期" />
                <asp:BoundColumn DataField="Fcreate_Time" HeaderText="创建时间" />
                <asp:BoundColumn DataField="Facc_time" HeaderText="扣款对账时间" />
                <asp:BoundColumn DataField="Fplan_pay_date" HeaderText="计划扣款日期" />
                <asp:BoundColumn DataField="Flist_pay_date" HeaderText="计划扣款日期" />
                <asp:BoundColumn DataField="Fmodify_Time" HeaderText="最后修改时间" />
            </Columns>
        </asp:DataGrid>


        <asp:DataGrid Width="1200" ID="dg_FundFetchPlanByPlanid" runat="server" AutoGenerateColumns="False" CssClass="tab_dg" Caption="计划详情补充(定赎)">
            <HeaderStyle Font-Bold="True" Height="25px" />
            <Columns>
                <asp:BoundColumn DataField="Fnext_redeem_date" HeaderText="下次t+1赎回发起日期" />
                <asp:BoundColumn DataField="Fnext_fetch_date" HeaderText="提现到账日期" />
                <asp:BoundColumn DataField="Ftotal_fetch_times" HeaderText="已还款总次数" />
                <asp:BoundColumn DataField="Ftotal_fetch_fee" HeaderText="已还款总金额" />
                <asp:BoundColumn DataField="Fbind_serialno" HeaderText="快捷绑卡序列号" />
                <asp:BoundColumn DataField="Flstate" HeaderText="有效状态" />
                <asp:BoundColumn DataField="Fbussi_type" HeaderText="还款类型" />
                <asp:BoundColumn DataField="Fmodify_time" HeaderText="最后修改时间" />
            </Columns>
        </asp:DataGrid>

        <asp:DataGrid Width="1200px" ID="dg_PlanFetchOrder" runat="server" AutoGenerateColumns="False" CssClass="tab_dg" Caption="还款记录(定赎)" OnItemDataBound="dg_PlanFetchOrder_ItemDataBound">
            <HeaderStyle Font-Bold="True" Height="25px" />
            <Columns>
                <asp:BoundColumn DataField="Flistid" HeaderText="还款交易单" />
                <asp:BoundColumn DataField="Fcft_fetch_id" HeaderText="提现单号" />
                <asp:BoundColumn DataField="Fcft_bank_billno" HeaderText="赎回单号" />
                <asp:BoundColumn DataField="Ftotal_fee" HeaderText="还款金额" />
                <asp:BoundColumn DataField="Fstate" HeaderText="还款状态" />
                <asp:BoundColumn DataField="Ftotal_transfer_fee" HeaderText="已经赎回金额" />
                <asp:BoundColumn DataField="Flstate" HeaderText="有效状态" />
                 <asp:BoundColumn DataField="Ffund_name" HeaderText="基金名称" />
 <%--               <asp:BoundColumn DataField="Ffund_code" HeaderText="赎回基金" />
                <asp:BoundColumn DataField="Fspid" HeaderText="商户号" />--%>
                <asp:BoundColumn DataField="Fbank_type" HeaderText="银行类型" />
                <asp:BoundColumn DataField="Fcard_tail" HeaderText="卡尾号" />
                <asp:BoundColumn DataField="Fbind_serialno" HeaderText="快捷绑卡序列号" />
                <asp:BoundColumn DataField="Fbussi_type" HeaderText="还款类型" />
                <asp:BoundColumn DataField="Fdate" HeaderText="设定还款日期" />
                <asp:BoundColumn DataField="Fredeem_info" HeaderText="多基金赎回列表"  />
             <%--   <asp:TemplateColumn HeaderText="多基金赎回列表">
                    <ItemTemplate>
                        <div class="bluequestionmark">
                             <asp:Label Text='<%# DataBinder.Eval(Container, "DataItem.Fredeem_info")  %>' runat="server"   ID="Label2" NAME="Label1">
                            </asp:Label>
                            <br />
                            <table class="tipinfo" runat="server" id="tb_Fredeem_info">
                            </table>
                            <asp:Label  Text='<%# DataBinder.Eval(Container, "DataItem.Fredeem_info") %>' runat="server"   ID="Label1" NAME="Label1">
                            </asp:Label>
                        </div>
                    </ItemTemplate>
                </asp:TemplateColumn>--%>
                <asp:BoundColumn DataField="Fredeem_date" HeaderText="赎回发起日期" />
                <asp:BoundColumn DataField="Ffail_reason" HeaderText="失败原因" />
                <asp:BoundColumn DataField="Ffail_desc" HeaderText="失败原因描述" />
                <asp:BoundColumn DataField="Fcreate_time" HeaderText="创建时间" />
                <asp:BoundColumn DataField="Fmodify_time" HeaderText="最后修改时间" />
                <asp:BoundColumn DataField="Facc_time" HeaderText="赎回对账时间" />
            </Columns>
        </asp:DataGrid>

        <webdiyer:AspNetPager ID="pager2" runat="server" AlwaysShow="True" NumericButtonCount="5" ShowCustomInfoSection="left" Width="1200px"
            PagingButtonSpacing="0" ShowInputBox="always" CssClass="mypager" HorizontalAlign="right" OnPageChanged="ChangePage2"
            SubmitButtonText="转到" NumericButtonTextFormatString="[{0}]" Visible="false">
        </webdiyer:AspNetPager>
    </form>
</body>
</html>

