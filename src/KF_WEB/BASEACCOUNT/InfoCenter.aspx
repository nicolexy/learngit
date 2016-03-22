<%@ Page Language="c#" CodeBehind="InfoCenter.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.BaseAccount.InfoCenter" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>InfoCenter</title>
    <style type="text/css">
        @import url( ../STYLES/ossstyle.css );
        .style2
        {
            color: #000000;
        }
        .style3
        {
            color: #ff0000;
        }
        BODY
        {
            background-image: url(../IMAGES/Page/bg01.gif);
        }
    </style>
    <script language="javascript" type="text/javascript">

        function IsNumber(string, sign) {
            var number;
            if (string == null)
                return false;

            number = new Number(string);
            if (isNaN(number)) {
                return false;
            }
            else {
                return true;
            }
        }

        function checkvlid() {
            with (Form1) {
                if (TextBox1_InputQQ.value == "") {
                    alert("请输入用户帐户号!!");
                    TextBox1_InputQQ.focus();
                    return false;
                }
            }
            return true;
        }
    </script>
</head>
<body>
    <form id="Form1" method="post" runat="server">
    <table cellspacing="1" cellpadding="0" width="95%" align="center" bgcolor="#666666"
        border="0">
        <tr bgcolor="#e4e5f7" style="background-image: ../IMAGES/Page/bg_bl.gif">
            <td valign="middle" colspan="2" height="20">
                <table style="height: 90%" cellspacing="0" cellpadding="1" width="97%" border="0">
                    <tr>
                        <td width="80%" style="background-image: ../IMAGES/Page/bg_bl.gif" height="18">
                            <font color="#ff0000"><strong><font color="#ff0000">&nbsp;</font></strong><img height="16"
                                src="../IMAGES/Page/post.gif" width="20">
                                用户查询</font>
                        </td>
                        <td width="20%" style="background-image: ../IMAGES/Page/bg_bl.gif">
                            操作员代码: <span class="style3">
                                <asp:Label ID="Label_uid" runat="server" Width="73px"></asp:Label></span>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr bgcolor="#ffffff">
            <td>
                <table style="height: 100%" cellspacing="0" cellpadding="1" width="100%" border="0">
                    <tr>
                        <td style="padding-left: 100px">
                            输入：&nbsp;
                            <asp:TextBox ID="TextBox1_InputQQ" runat="server"></asp:TextBox>
                            &nbsp;
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Width="150px"
                                Display="Dynamic" ErrorMessage="RequiredFieldValidator" ControlToValidate="TextBox1_InputQQ">请输入用户帐号</asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <input id="CFT" name="IDType" runat="server" type="radio" checked /><label for="CFT">C账号</label>
                            <input id="InternalID" name="IDType" runat="server" type="radio" /><label for="InternalID">内部账号</label>
                            
                        </td>
                    </tr>
                </table>
            </td>
            <td width="25%">
                <div align="center">
                    <asp:CheckBox ID="CheckBox1" runat="server" Text="历史记录"></asp:CheckBox>&nbsp;
                    <asp:Button ID="Button1" runat="server" Text="查 询" OnClick="Button1_Click"></asp:Button></div>
            </td>
        </tr>
    </table>
    <br />
    <table height="127" cellspacing="0" cellpadding="0" width="95%" align="center" border="0">
        <tr>
            <td bgcolor="#666666">
                <table height="192" cellspacing="1" cellpadding="0" width="100%" align="center" border="0">
                    <tr bgcolor="#e4e5f7" background="../IMAGES/Page/bg_bl.gif">
                        <td background="../IMAGES/Page/bg_bl.gif" colspan="5" height="20">
                            <strong><font color="#ff0000">&nbsp;<img height="16" src="../IMAGES/Page/post.gif"
                                width="20">
                            </font></strong><font color="#ff0000">用户账户资料</font>
                            <div align="center">
                                <font face="宋体"></font>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 3px" width="20%" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee"
                            height="3">
                            &nbsp;QQ 账号:
                        </td>
                        <td style="height: 3px" bgcolor="#ffffff" height="3">
                            &nbsp;<span class="style2">
                                <asp:Label ID="Label1_Acc" runat="server">22000254</asp:Label></span>
                        </td>
                        <td style="height: 8px" width="11%" background="../IMAGES/Page/bg_bl.gif" bgcolor="#e4e5f7"
                            height="8">
                            <asp:Label ID="labQQstate" runat="server"></asp:Label>
                        </td>
                        <td style="height: 3px" width="24%" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee"
                            height="3">
                            &nbsp;<font face="宋体">真实姓名:</font>
                        </td>
                        <td style="height: 3px" bgcolor="#ffffff" height="3">
                            &nbsp;<font face="宋体">
                                <asp:Label ID="Label14_Ftruename" runat="server">张三</asp:Label></font> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:LinkButton runat="server" ID="lbtn_synName" ForeColor="blue" OnClick="SyncUserNameClick">同步姓名</asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 3px" width="20%" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee"
                            height="3">
                               EMAIL账号:
                        </td>
                        <td style="height: 3px" bgcolor="#ffffff" height="3">
                            &nbsp;<span class="style2">
                                <asp:Label ID="labEmail" runat="server">at126@126.com</asp:Label></span>
                        </td>
                        <td style="height: 8px" width="11%" background="../IMAGES/Page/bg_bl.gif" bgcolor="#e4e5f7"
                            height="8">
                            <asp:Label ID="labEmailState" runat="server"></asp:Label>
                        </td>
                        <td style="height: 3px" width="24%" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee"
                            height="3">
                            &nbsp;<font face="宋体">内部ID:</font>
                        </td>
                        <td style="height: 3px" bgcolor="#ffffff" height="3">
                            &nbsp;<font face="宋体">
                                <asp:Label ID="lbInnerID" runat="server"></asp:Label></font>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 13px" width="20%" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee"
                            height="13">
                            <font face="宋体">&nbsp;手机帐号:</font>
                        </td>
                        <td style="height: 13px" bgcolor="#ffffff" height="13">
                            <font face="宋体">&nbsp;
                                <asp:Label ID="labMobile" runat="server"></asp:Label></font>
                        </td>
                        <td style="height: 8px" width="11%" background="../IMAGES/Page/bg_bl.gif" bgcolor="#e4e5f7"
                            height="8">
                            <asp:Label ID="labMobileState" runat="server"></asp:Label>
                        </td>
                        <td style="height: 13px" width="24%" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee"
                            height="13">
                            <font face="宋体">&nbsp;余额支付状态:</font>
                        </td>
                        <td style="height: 13px" bgcolor="#ffffff" height="13">
                            <font face="宋体">&nbsp;</font>
                            <asp:Label ID="lbLeftPay" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 8px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee"
                            height="8">
                            <font face="宋体">&nbsp;帐户状态:</font>
                        </td>
                        <td style="height: 8px" width="18%" bgcolor="#ffffff" height="8">
                            <font face="宋体">&nbsp;
                                <asp:Label ID="Label12_Fstate" runat="server">正常</asp:Label></font><font face="宋体"></font>
                        </td>
                        <td style="height: 8px" width="11%" background="../IMAGES/Page/bg_bl.gif" bgcolor="#e4e5f7"
                            height="8">
                            <div align="center">
                                <asp:LinkButton ID="LinkButton3" runat="server" OnClick="LinkButton3_Click"></asp:LinkButton></div>
                        </td>
                        <td style="height: 8px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee"
                            height="8">
                            <font face="宋体">&nbsp;帐户类型:</font>
                        </td>
                        <td style="height: 8px" bgcolor="#ffffff" height="8">
                            <font face="宋体">&nbsp;
                                <asp:Label ID="Label13_Fuser_type" runat="server">个人</asp:Label></font><font face="宋体"></font>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 15px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee"
                            height="15">
                            &nbsp;<font face="宋体">可用余额</font>:
                        </td>
                        <td style="height: 15px" bgcolor="#ffffff" height="15">
                            &nbsp;
                            <asp:Label ID="Label15_Useable" runat="server" Width="100px" ForeColor="Red"></asp:Label><font
                                face="宋体"></font>
                        </td>
                         <td style="height: 8px" bgcolor="#ffffff" height="15">
                            &nbsp;
                            <asp:Label ID="Label19_OpenOrNot" runat="server" Width="100px" ForeColor="Red"></asp:Label><font
                                face="宋体"></font>
                        </td>
                        <td style="height: 15px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee"
                            height="15">
                            &nbsp;冻结金额:
                        </td>
                        <td style="height: 15px" bgcolor="#ffffff" height="15">
                            &nbsp;
                            <asp:Label ID="Label4_Freeze" runat="server" Width="120px"></asp:Label>
                            <span style="margin-left:20px;">
                                <span>分账冻结金额:</span>
                                <asp:Label ID="lb_Freeze_amt" runat="server" Width="120px">0</asp:Label>
                            </span>
                        </td>
                    </tr>
                    <tr>
                       <%-- <td style="height: 4px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee"
                            height="4">
                            &nbsp;昨日余额:
                        </td>
                        <td style="height: 4px" bgcolor="#ffffff" colspan="2" height="4">
                            &nbsp;
                            <asp:Label ID="Label5_YestodayLeft" runat="server">10</asp:Label>
                        </td>--%>
                        <td style="height: 4px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee"
                            height="4">
                            &nbsp;帐户余额<font face="宋体">:</font>
                        </td>
                        <td style="height: 4px" bgcolor="#ffffff" height="4">
                            &nbsp;<font face="宋体">
                                <asp:Label ID="Label3_LeftAcc" runat="server" Width="180px">3000</asp:Label></font>
                        </td>
                        <td style="height: 4px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee" height="4"></td>
                        <td style="height: 4px" bgcolor="#ffffff" colspan="2" height="4"></td>
                    </tr>
                    <tr>
                        <td style="height: 4px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee"
                            height="4">
                            <font face="宋体">&nbsp;币种类型:</font>
                        </td>
                        <td style="height: 4px" bgcolor="#ffffff" colspan="2" height="4">
                            <font face="宋体">&nbsp;</font>
                            <asp:Label ID="Label2_Type" runat="server">代金券</asp:Label>
                        </td>
                        <td style="height: 4px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee"
                            height="4">
                            <font face="宋体">&nbsp;注册时间:</font>
                        </td>
                        <td style="height: 4px" bgcolor="#ffffff" height="4">
                            <font face="宋体">&nbsp;
                                <asp:Label ID="lblLoginTime" runat="server">10</asp:Label></font>
                        </td>
                    </tr>
          <%--          <tr>
                        <td style="height: 4px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee"
                            height="4">
                            <font face="宋体">&nbsp;当日已付:</font>
                        </td>
                        <td style="height: 4px" bgcolor="#ffffff" height="4" colspan="2">
                            <font face="宋体">&nbsp;
                                <asp:Label ID="Label16_Fapay" runat="server">10</asp:Label></font>
                        </td>
                        <td style="height: 13px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee">
                            &nbsp;单笔交易限额:
                        </td>
                        <td style="height: 13px" bgcolor="#ffffff" height="13">
                            &nbsp;
                            <asp:Label ID="Label7_SingleMax" runat="server">2000</asp:Label>
                        </td>
                    </tr>--%>
               <%--     <tr>
                        <td style="height: 13px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee"
                            height="13">
                            &nbsp;单日支付限额:
                        </td>
                        <td style="height: 13px" bgcolor="#ffffff" height="13" colspan="2">
                            &nbsp;
                            <asp:Label ID="Label8_PerDayLmt" runat="server">5000</asp:Label>
                        </td>
                        <td style="height: 13px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee">
                            <font face="宋体">&nbsp;当日提现金额:</font>
                        </td>
                        <td style="height: 13px" bgcolor="#ffffff" height="13">
                            <font face="宋体">&nbsp;
                                <asp:Label ID="lbFetchMoney" runat="server"></asp:Label></font>
                        </td>
                    </tr>--%>
                   <%-- <tr>
                        <td style="height: 13px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee"
                            height="13">
                            <font face="宋体">&nbsp;当日已充值金额:</font>
                        </td>
                        <td style="height: 13px" bgcolor="#ffffff" height="13" colspan="2">
                            <font face="宋体">&nbsp;
                                <asp:Label ID="lbSave" runat="server"></asp:Label></font>
                        </td>
                        <td style="height: 6px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee"
                            height="6">
                            &nbsp;最近存款日期:
                        </td>
                        <td style="height: 6px" bgcolor="#ffffff" height="6">
                            &nbsp;
                            <asp:Label ID="Label9_LastSaveDate" runat="server">2005-03-01</asp:Label>
                        </td>
                    </tr>--%>
                    <tr>
                       <%-- <td style="height: 6px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee"
                            height="6">
                            &nbsp;最近提款日期:
                        </td>
                        <td style="height: 6px" bgcolor="#ffffff" height="6" colspan="2">
                            &nbsp;
                            <asp:Label ID="Label10_Drawing" runat="server">2005-04-15</asp:Label>
                        </td>--%>
                        <td style="height: 14px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee"
                            height="14">
                            <font face="宋体">&nbsp;最后登陆IP地址:</font>
                        </td>
                        <td style="height: 14px" bgcolor="#ffffff" height="14">
                            <font face="宋体">&nbsp;
                                <asp:Label ID="Label17_Flogin_ip" runat="server">202.103.24.68</asp:Label></font>
                        </td>
                        <td style="height: 4px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee" height="4"></td>
                        <td style="height: 4px" bgcolor="#ffffff" colspan="2" height="4"></td>
                    </tr>
                    <tr>
                        <td style="height: 14px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee"
                            height="14">
                            <font face="宋体">&nbsp;最后修改时间:</font>
                        </td>
                        <td style="height: 14px" bgcolor="#ffffff" height="14" colspan="2">
                            <font face="宋体">&nbsp;</font>
                            <asp:Label ID="Label6_LastModify" runat="server">2005-05-01</asp:Label>
                        </td>
                        <td style="height: 14px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee"
                            height="14">
                            <font face="宋体">&nbsp;产品属性:</font>
                        </td>
                        <td style="height: 14px" bgcolor="#ffffff" height="14">
                            <font face="宋体">&nbsp;</font>
                            <asp:Label ID="Label18_Attid" runat="server">BB</asp:Label>
                        </td>
                    </tr>
                   <%-- <tr>
                        <td style="height: 14px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee"
                            height="14">
                            <font face="宋体">&nbsp;财富值:</font>
                        </td>
                        <td style="height: 14px" bgcolor="#ffffff" height="14" colspan="2">
                            <font face="宋体">&nbsp;</font>
                            <asp:Label ID="vip_value" runat="server"></asp:Label>
                        </td>
                        <td style="height: 14px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee"
                            height="14">
                            <font face="宋体">&nbsp;会员类型:</font>
                        </td>
                        <td style="height: 14px" bgcolor="#ffffff" height="14">
                            <font face="宋体">&nbsp;</font>
                            <asp:Label ID="vip_flag" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 14px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee"
                            height="14">
                            <font face="宋体">&nbsp;会员等级:</font>
                        </td>
                        <td style="height: 14px" bgcolor="#ffffff" height="14" colspan="2">
                            <font face="宋体">&nbsp;</font>
                            <asp:Label ID="vip_level" runat="server"></asp:Label>
                        </td>
                        <td style="height: 14px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee"
                            height="14">
                            <font face="宋体">&nbsp;会员开通方式:</font>
                        </td>
                        <td style="height: 14px" bgcolor="#ffffff" height="14">
                            <font face="宋体">&nbsp;</font>
                            <asp:Label ID="vip_channel" runat="server"></asp:Label>
                        </td>
                    </tr>--%>
                    <tr>
                        <%--<td style="height: 14px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee"
                            height="14">
                            <font face="宋体">&nbsp;会员关闭时间:</font>
                        </td>
                        <td style="height: 14px" bgcolor="#ffffff" height="14" colspan="2">
                            <font face="宋体">&nbsp;</font>
                            <asp:Label ID="vip_exp_date" runat="server"></asp:Label>
                        </td>--%>

                        
                        <td background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee" height="10">
                            &nbsp;备注:
                        </td>
                        <td bgcolor="#ffffff" height="10" colspan="2">
                            &nbsp;
                            <asp:Label ID="Label11_Remark" runat="server">这个家伙很懒，什么都没有留下来。</asp:Label>
                        </td>
                        
                        <td style="height: 14px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee"
                            height="14">
                            <font face="宋体">&nbsp;实名认证:</font>
                        </td>
                        <td style="height: 14px" bgcolor="#ffffff" height="14" colspan="3">
                            <font face="宋体">&nbsp;</font>
                            <asp:Label ID="labUserClassInfo" runat="server"></asp:Label>
                        </td>
                        
                    </tr>
                    <tr>
                        <td  style="height: 14px" bgcolor="#ffffff" height="14" colspan="4"></td>
                        <td style="height: 16px" bgcolor="#ffffff" height="16">
                            <font face="宋体">
                                <asp:Button ID="btnDelClass" runat="server" Text="删除认证" Visible="False" OnClick="btnDelClass_Click">
                                </asp:Button></font>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <br>
    <table cellspacing="0" cellpadding="0" width="95%" align="center" border="0">
        <tr>
            <td valign="top" align="center">
                <asp:DataGrid ID="dgList" runat="server" Width="1150px" ItemStyle-HorizontalAlign="Center"
                    HeaderStyle-HorizontalAlign="Center" HorizontalAlign="Center" PageSize="5" AutoGenerateColumns="False"
                    GridLines="Horizontal" CellPadding="1" BackColor="White" BorderWidth="1px" BorderStyle="None"
                    BorderColor="#E7E7FF" AllowPaging="True" OnPageIndexChanged="dgList_PageIndexChanged">
                    <FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
                    <SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
                    <AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
                    <ItemStyle HorizontalAlign="Center" ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
                    <HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C">
                    </HeaderStyle>
                    <Columns>
                        <asp:BoundColumn DataField="Fqqid" HeaderText="帐号"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Fmemo" HeaderText="备注"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Fauthen_operator" HeaderText="操作人员"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Fmodify_time" HeaderText="最后修改时间"></asp:BoundColumn>
                    </Columns>
                    <PagerStyle ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
    <table height="35" cellspacing="0" cellpadding="0" width="95%" align="center" border="0">
        <tr>
            <td bgcolor="#666666">
                <table height="35" cellspacing="1" cellpadding="0" width="100%" align="center" border="0">
                    <tr bgcolor="#e4e5f7">
                        <td background="../IMAGES/Page/bg_bl.gif" bgcolor="#e4e5f7" colspan="3" height="20">
                            <font color="#ff0000">&nbsp;<img height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp;
                                <asp:LinkButton ID="LKBT_TradeLog" runat="server" ForeColor="Red" OnClick="LKBT_TradeLog_Click">买家交易单</asp:LinkButton></font>|
                            <span class="style2">
                                    <asp:LinkButton ID="LKBT_TradeLog_Sale" runat="server" ForeColor="Black" OnClick="LKBT_TradeLog_Sale_Click">卖家交易单</asp:LinkButton>|
                                    <%--<asp:LinkButton ID="LKBT_TradeLog_Unfinished" runat="server" ForeColor="Black" OnClick="LKBT_TradeLog_Unfinished_Click">买家未完成交易单</asp:LinkButton>|--%>
                                    <%--<asp:LinkButton ID="LKBT_TradeLog_Sale_Unfinished" runat="server" ForeColor="Black" OnClick="LKBT_TradeLog_Sale_Unfinished_Click">卖家未完成交易单</asp:LinkButton>|--%>
                                    <asp:LinkButton ID="LKBT_bankrollLog" runat="server" ForeColor="Black" OnClick="LKBT_bankrollLog_Click">用户资金流水</asp:LinkButton>|
                                    <asp:LinkButton ID="LKBT_GatheringLog" runat="server" ForeColor="Black" OnClick="LKBT_GatheringLog_Click">充值记录</asp:LinkButton>|
                                    <asp:LinkButton ID="LkBT_PaymentLog" runat="server" ForeColor="Black" OnClick="LkBT_PaymentLog_Click">提现记录</asp:LinkButton>&nbsp;|
                                    <%--<asp:LinkButton ID="LkBT_Refund" runat="server" ForeColor="Black" OnClick="LkBT_Refund_Click">买家退款单</asp:LinkButton>&nbsp;|--%>
                                    <%--<asp:LinkButton ID="LkBT_Refund_Sale" runat="server" ForeColor="Black" OnClick="LkBT_Refund_Sale_Click">卖家退款单</asp:LinkButton>&nbsp;|--%>
                                    <%--<asp:LinkButton ID="LkBT_ButtonInfo" runat="server" ForeColor="Black" OnClick="LkBT_ButtonInfo_Click">商家工具按钮</asp:LinkButton>&nbsp;|--%>
                                    <%--<asp:LinkButton ID="LkBT_Gwq" runat="server" ForeColor="Black" OnClick="LkBT_Gwq_Click">财付券</asp:LinkButton>&nbsp;|--%>
                                    <%--<asp:LinkButton ID="LkBT_mediOrder" runat="server" ForeColor="Black" OnClick="LkBT_mediOrder_Click">中介交易</asp:LinkButton>--%>
                            </span>
                            <div align="center">
                                <font face="Tahoma"></font>
                            </div>
                        </td>
                        <td width="30" background="../IMAGES/Page/bg_bl.gif" height="20">
                            &nbsp;
                        </td>
                        <td align="right" width="30" background="../IMAGES/Page/bg_bl.gif">
                            <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="../Images/Page/down.gif">
                            </asp:ImageButton>
                        </td>
                    </tr>
                    <tr>
                        <td bgcolor="#ffffff" colspan="5" height="12">
                            <iframe id="iframe0" name="iframe0" marginwidth="0" marginheight="0" src="<%=iFramePath%>"
                                frameborder="0" width="100%" height="20"></iframe>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
