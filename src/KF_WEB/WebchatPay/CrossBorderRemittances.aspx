<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CrossBorderRemittances.aspx.cs" Inherits="TENCENT.OSS.CFT.KF.KF_Web.WebchatPay.CrossBorderRemittances" %>
 <%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>跨境汇款</title>
    <style type="text/css">
        @import url( ../STYLES/common.css );
    </style>
     <script type="text/javascript" src="../SCRIPTS/My97DatePicker/WdatePicker.js"></script>

</head>
<body>
    <form id="formMain" runat="server">
        <div class="container">
            <div class="style_title">
                跨境汇款
            </div>
            <table cellspacing="1" cellpadding="1" >
                <caption>
                    查询条件
                </caption>
                <tr>
                    <td class="tb_query_title">微信号：
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txt_WeChatId" Width="170px"></asp:TextBox>
                    </td>
                    <td class="tb_query_title">手机号码：</td>
                    <td>
                        <asp:TextBox runat="server" ID="txt_s_phone_no" Width="170px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="tb_query_title">支付单号：
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txt_wx_pay_id" Width="170px"></asp:TextBox>
                    </td>
                    <td class="tb_query_title">支付状态：</td>
                    <td>
                        <asp:DropDownList ID="ddl_wx_pay_state" runat="server" Width="172px">
                            <asp:ListItem Value="" Text="-请选择支付状态-"></asp:ListItem>
                            <asp:ListItem Value="0" Text="未支付"></asp:ListItem>
                            <asp:ListItem Value="1" Text="支付成功"> </asp:ListItem>
                        </asp:DropDownList></td>
                </tr>
                <tr>
                    <td class="tb_query_title">汇款类型：
                    </td>
                    <td>
                        <asp:DropDownList ID="ddl_remit_type" runat="server" Width="172px">
                             <asp:ListItem Value="" Text="-请选择汇款类型-"></asp:ListItem>
                             <asp:ListItem Value="1" Text="个人对个人汇款（P2P）"></asp:ListItem>
                             <asp:ListItem Value="2" Text="个人对自己汇款（P2S）"></asp:ListItem>
                             <asp:ListItem Value="3" Text="个人对企业/机构汇款（P2B）"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td class="tb_query_title">时间：</td>
                    <td><asp:TextBox runat="server" ID="txt_start_date" onclick="WdatePicker()" class="Wdate" Width="170px"></asp:TextBox>
                        到<asp:TextBox runat="server" ID="txt_end_date"  onclick="WdatePicker()" class="Wdate" Width="170px"></asp:TextBox></td>
                </tr>

                <tr>
                    <td colspan="4" style="text-align: center">
                        <asp:Button ID="btnSerach" runat="server" Text="查 询" OnClick="btnSerach_Click" />

                    </td>
                </tr>
            </table>
            <br />
            <asp:DataGrid ID="DataGrid1" runat="server" AutoGenerateColumns="False" Caption="查询结果" 
                CssClass="datagrid" GridLines="None" cellspacing="1" cellpadding="1" OnItemCommand="DataGrid1_ItemCommand" >
                <HeaderStyle  CssClass="th"/>
                <Columns>
                    <asp:BoundColumn DataField="wx_pay_id" HeaderText="支付单号" />
                     <asp:BoundColumn DataField="pay_time" HeaderText="支付时间" />
                     <asp:BoundColumn DataField="pay_state" HeaderText="支付状态" />
                     <asp:BoundColumn DataField="pay_amount" HeaderText="支付总金额（元）" />
                     <asp:BoundColumn DataField="remit_type" HeaderText="汇款类型" />
                     <asp:BoundColumn DataField="remit_state" HeaderText="汇款状态" />
                     <asp:BoundColumn DataField="accept_time" HeaderText="收款确认时间" />
                    <asp:ButtonColumn Text="查看" CommandName="detail" />
                </Columns>
            </asp:DataGrid>
            <webdiyer:aspnetpager id="pager1" runat="server" alwaysshow="True" numericbuttoncount="5" showcustominfosection="left" width="1100px"
                pagingbuttonspacing="0" showinputbox="always" cssclass="mypager" horizontalalign="right" onpagechanged="ChangePage"
                submitbuttontext="转到" numericbuttontextformatstring="[{0}]">
        </webdiyer:aspnetpager>
            <asp:Panel runat="server" ID="panDetail" Visible="false" >
            <table cellspacing="1" cellpadding="1"  >
                  <caption>
                    详情
                </caption>
                <tr>
                    <td class="tb_query_title">汇款人账号：</td>
                     <td> <asp:Label runat="server" ID="lbl_accid"></asp:Label> </td> 
                    <td class="tb_query_title">汇款币种：</td>
                     <td><asp:Label runat="server" ID="lbl_cur_type"> </asp:Label></td>
                </tr>
                <tr>
                    <td class="tb_query_title">汇款类型：</td>
                     <td> <asp:Label runat="server" ID="lbl_remit_type"></asp:Label> </td> 
                    <td class="tb_query_title">汇款金额（外币）：</td>
                     <td><asp:Label runat="server" ID="lbl_fx_amount"> </asp:Label></td>
                </tr>
                 <tr>
                    <td class="tb_query_title">支付单号：</td>
                     <td> <asp:Label runat="server" ID="lbl_wx_pay_id"></asp:Label> </td> 
                    <td class="tb_query_title">当日汇率：</td>
                     <td><asp:Label runat="server" ID="lbl_fx_rate"> </asp:Label></td>
                </tr>
                 <tr>
                    <td class="tb_query_title">支付时间：</td>
                     <td> <asp:Label runat="server" ID="lbl_pay_time"></asp:Label> </td> 
                    <td class="tb_query_title">收款确认时间：</td>
                     <td><asp:Label runat="server" ID="lbl_accept_time"> </asp:Label></td>
                </tr>
                 <tr>
                    <td class="tb_query_title">支付状态：</td>
                     <td> <asp:Label runat="server" ID="lbl_pay_state"></asp:Label> </td> 
                    <td class="tb_query_title">提现时间：</td>
                     <td><asp:Label runat="server" ID="lbl_fetch_time"> </asp:Label></td>
                </tr>
                 <tr>
                    <td class="tb_query_title">支付总金额：</td>
                     <td> <asp:Label runat="server" ID="lbl_pay_amount"></asp:Label> </td> 
                    <td class="tb_query_title">收款方姓名：</td>
                     <td><asp:Label runat="server" ID="lbl_r_name"> </asp:Label></td>
                </tr>
                 <tr>
                    <td class="tb_query_title">汇款状态：</td>
                     <td> <asp:Label runat="server" ID="lbl_remit_state"></asp:Label> </td> 
                    <td class="tb_query_title">收款方生日：</td>
                     <td><asp:Label runat="server" ID="lbl_r_dob"> </asp:Label></td>
                </tr>
                 <tr>
                    <td class="tb_query_title">汇款失败原因：</td>
                     <td> <asp:Label runat="server" ID="lbl_error_desc"></asp:Label> </td> 
                    <td class="tb_query_title">收款方银行类型：</td>
                     <td><asp:Label runat="server" ID="lbl_r_bank_type"> </asp:Label></td>
                </tr>
                 <tr>
                    <td class="tb_query_title">购汇金额（人民币）：</td>
                     <td> <asp:Label runat="server" ID="lbl_cny_amount"></asp:Label> </td> 
                    <td class="tb_query_title">收款方银行代码：</td>
                     <td><asp:Label runat="server" ID="lbl_r_bank_code"> </asp:Label></td>
                </tr>
                 <tr>
                    <td class="tb_query_title">手续费金额：</td>
                     <td> <asp:Label runat="server" ID="lbl_fee_amount"></asp:Label> </td> 
                    <td class="tb_query_title">收款方银行名称：</td>
                     <td><asp:Label runat="server" ID="lbl_r_bank_name"> </asp:Label></td>
                </tr>
                 <tr>
                    <td class="tb_query_title">汇款人地址：</td>
                     <td> <asp:Label runat="server" ID="lbl_s_address"></asp:Label> </td> 
                    <td class="tb_query_title">收款方银行账号：</td>
                     <td><asp:Label runat="server" ID="lbl_r_bank_acc"> </asp:Label></td>
                </tr>
                 <tr>
                    <td class="tb_query_title">汇款用途：</td>
                     <td> <asp:Label runat="server" ID="lbl_purpose"></asp:Label> </td> 
                    <td class="tb_query_title">收款方手机号码：</td>
                     <td><asp:Label runat="server" ID="lbl_r_phone_no"> </asp:Label></td>
                </tr>
                 <tr>
                    <td class="tb_query_title">是否需要补全信息：</td>
                     <td> <asp:Label runat="server" ID="lbl_inquiry"></asp:Label> </td> 
                    <td class="tb_query_title">收款方邮箱：</td>
                     <td><asp:Label runat="server" ID="lbl_r_email"> </asp:Label></td>
                </tr>
                 <tr>
                    <td class="tb_query_title">是否已补全：</td>
                     <td> <asp:Label runat="server" ID="lbl_replied"></asp:Label> </td> 
                    <td class="tb_query_title">收款方地址：</td>
                     <td><asp:Label runat="server" ID="lbl_r_address"> </asp:Label></td>
                </tr>
                 <tr>
                    <td class="tb_query_title">补全信息原因：</td>
                     <td> <asp:Label runat="server" ID="lbl_inq_reason"></asp:Label> </td> 
                    <td class="tb_query_title">&nbsp;</td>
                     <td>&nbsp;</td>
                </tr>
                 <tr>
                    <td class="tb_query_title">补全信息项：</td>
                     <td> <asp:Label runat="server" ID="lbl_inq_items"></asp:Label> </td> 
                    <td class="tb_query_title">&nbsp;</td>
                     <td>&nbsp;</td>
                </tr>
            </table>
                </asp:Panel>
        </div>
        <br />
    </form>
</body>
</html>
