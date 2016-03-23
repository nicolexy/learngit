<%@ Page Language="c#" CodeBehind="TradeLogQueryNew.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.TradeManage.TradeLogQueryNew" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>交易单查询</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="C#" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <style type="text/css">
        @import url( ../STYLES/ossstyle.css );

        UNKNOWN {
            COLOR: #000000;
        }

        .style3 {
            COLOR: #ff0000;
        }

        BODY {
            BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif);
        }
    </style>
    <script type="text/javascript" src="../SCRIPTS/My97DatePicker/WdatePicker.js"></script>
</head>
<body>
    <form id="Form1" method="post" runat="server">
        <font face="宋体"></font><font face="宋体"></font><font face="宋体"></font><font face="宋体"></font><font face="宋体"></font><font face="宋体"></font><font face="宋体"></font><font face="宋体"></font>
        <br>
        <table cellspacing="1" cellpadding="0" width="95%" align="center" bgcolor="#666666" border="0">
            <tr bgcolor="#e4e5f7">
                <td valign="middle" colspan="2" height="20">
                    <table height="90%" cellspacing="0" cellpadding="1" width="100%" border="0">
                        <tr>
                            <td width="80%" background="../IMAGES/Page/bg_bl.gif" height="18"><font color="#ff0000"><strong><font color="#ff0000">&nbsp;</font></strong><img height="16" src="../IMAGES/Page/post.gif" width="20">
                                交易查询</font>
                                <div align="right"><font face="Tahoma" color="#ff0000"></font></div>
                            </td>
                            <td width="20%" background="../IMAGES/Page/bg_bl.gif">操作员代码: <span class="style3">
                                <asp:Label ID="Label_uid" runat="server">Label</asp:Label></span></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr bgcolor="#ffffff">
                <td>
                    <table height="100%" cellspacing="0" cellpadding="1" width="100%" border="0">
                        <tr>
                            <td style="HEIGHT: 37px; padding-left: 60px;" width="78%">
                                <p align="left">
                                    输入交易单号：<asp:TextBox ID="TextBox1_ListID" runat="server" Width="200px"></asp:TextBox>
                                </p>
                            </td>
                            <td style="HEIGHT: 40px" width="3%">&nbsp;</td>
                        </tr>
                    </table>
                </td>
                <td width="25%">
                    <div align="center">
                        <asp:Button ID="btQuery" runat="server" Width="66px" BorderStyle="Groove" Text="查询" Height="23px" OnClick="btQuery_Click"></asp:Button>&nbsp;
                    </div>
                </td>
            </tr>
        </table>
        <div align="center">
            <br>
            <table height="362" cellspacing="0" cellpadding="0" width="95%" align="center" border="0">
                <tr>
                    <td bgcolor="#666666">
                        <table height="391" cellspacing="1" cellpadding="0" width="100%" align="center" border="0">
                            <tr bgcolor="#e4e5f7">
                                <td style="HEIGHT: 15px" colspan="5" height="18"><strong></strong>
                                    <table cellspacing="0" cellpadding="1" width="100%" border="0">
                                        <tr>
                                            <td background="../IMAGES/Page/bg_bl.gif" height="20"><strong><font color="#ff0000">
                                                <img height="16" src="../IMAGES/Page/post.gif" width="20">
                                            </font></strong><font color="#ff0000">交易单资料</font>
                                            </td>
                                            <td width="20%" background="../IMAGES/Page/bg_bl.gif">
                                                <!--<div align="center"><A href="../ACCOUNTMANAGE/AdjustDepositMoney.aspx?id=<%=LB_Flistid.Text.Trim()%>" ><font color="red">分析</font></A>|</div> -->
                                                <font face="宋体">
                                                    <asp:HyperLink ID="hlOrder" runat="server">订单详细信息</asp:HyperLink></font>
                                                |
                                                    <font face="宋体">
                                                        <asp:HyperLink ID="btn_tradeBaseInfo" runat="server">交易基本信息</asp:HyperLink></font>
                                            </td>
                                            <td width="5%" background="../IMAGES/Page/bg_bl.gif">
                                                <div align="left"><a href="tradeLogQueryNew.aspx?id=<%=LB_Flistid.Text.Trim()%>" target="_blank"><font color="red"><font color="red">全屏</font></font></a></div>
                                            </td>
                                        </tr>
                                    </table>
                                    <div align="center"><font face="宋体"></font></div>
                                </td>
                            </tr>
                            <tr>
                                <td style="WIDTH: 20%; HEIGHT: 18px" bgcolor="#eeeeee">&nbsp; 交易单号:</td>
                                <td style="WIDTH: 30%; HEIGHT: 18px" bgcolor="#ffffff">&nbsp;<span class="style4">
                                    <asp:Label ID="LB_Flistid" runat="server"></asp:Label></span></td>
                                <td style="WIDTH: 20%; HEIGHT: 18px" bgcolor="#eeeeee">&nbsp;交易状态:</td>
                                <td style="WIDTH: 30%; HEIGHT: 18px" bgcolor="#ffffff" colspan="2">&nbsp;
										<asp:Label ID="lblTradeState" runat="server"></asp:Label></td>
                            </tr>
                            <tr>
                                <td style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee"><font face="宋体">&nbsp;是否调帐标志:</font></td>
                                <td style="WIDTH: 136px; HEIGHT: 18px" bgcolor="#ffffff"><font face="宋体">&nbsp; </font>
                                    <asp:Label ID="lbAdjustFlag" runat="server"></asp:Label></td>
                                <td style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee">
                                    <p><font face="宋体">&nbsp;交易类型:</font></p>
                                </td>
                                <td style="WIDTH: 156px; HEIGHT: 18px" bgcolor="#ffffff" colspan="2"><font face="宋体">&nbsp;
											<asp:Label ID="lbTradeType" runat="server"></asp:Label></font></td>
                            </tr>
                            <tr>
                                <td style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee"><font face="宋体">&nbsp;支付绑定序列号:</font></td>
                                <td style="WIDTH: 136px; HEIGHT: 18px" bgcolor="#ffffff"><font face="宋体">&nbsp; </font>
                                    <asp:Label ID="lbPayBindSeqId" runat="server"></asp:Label></td>
                                <td style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee">
                                    <p><font face="宋体">&nbsp;关闭原因:</font></p>
                                </td>
                                <td style="WIDTH: 156px; HEIGHT: 18px" bgcolor="#ffffff" colspan="2"><font face="宋体">&nbsp;
											<asp:Label ID="lbCloseReason" runat="server"></asp:Label></font></td>
                            </tr>
                            <tr>
                                <td style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee"><font face="宋体">&nbsp;给银行订单号:</font></td>
                                <td style="WIDTH: 136px; HEIGHT: 18px" bgcolor="#ffffff"><font face="宋体">&nbsp;
											<asp:Label ID="LB_Fbank_listid" runat="server"></asp:Label></font></td>
                                <td style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee"><font face="宋体">&nbsp;银行返回订单号:</font></td>
                                <td style="WIDTH: 156px; HEIGHT: 18px" bgcolor="#ffffff" colspan="2"><font face="宋体">&nbsp;
											<asp:Label ID="LB_Fbank_backid" runat="server"></asp:Label></font></td>
                            </tr>
                            <tr>
                                <%--<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"><FONT face="宋体">&nbsp;机构代码名称(发起):</FONT></TD>
									<TD style="WIDTH: 136px; HEIGHT: 18px" bgColor="#ffffff"><FONT face="宋体">&nbsp;
											<asp:label id="LB_Fspid" runat="server"></asp:label></FONT></TD>--%>
                                <td style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee"><font face="宋体">&nbsp;交易单的状态:</font></td>
                                <td style="HEIGHT: 18px" width="62px" bgcolor="#ffffff"><font face="宋体">&nbsp;
											<asp:Label ID="LB_Flstate" runat="server" ForeColor="Red"></asp:Label></font></td>
                                <td style="HEIGHT: 18px" width="124px" background="../IMAGES/Page/bg_bl.gif" bgcolor="#e4e5f7">&nbsp;
										<asp:LinkButton ID="LinkButton3_action" runat="server" OnClick="LinkButton3_action_Click" OnClientClick="if(!confirm('确定要执行该操作吗？')) return false;">冻结</asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:LinkButton ID="LinkButton_synchro" runat="server" OnClick="LinkButton_synchro_Click" OnClientClick="if(!confirm('确定要同步订单状态吗？')) return false;">同步订单状态</asp:LinkButton></td>
                                <td style="WIDTH: 156px; HEIGHT: 18px" bgcolor="#ffffff" colspan="2"></td>
                            </tr>
                            <tr>
                                <td style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee"><font face="宋体">&nbsp;买家内部帐号:</font></td>
                                <td style="WIDTH: 136px; HEIGHT: 18px" bgcolor="#ffffff"><font face="宋体">&nbsp;
											<asp:Label ID="LB_Fbuy_uid" runat="server"></asp:Label></font></td>
                                <td style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee"><font face="宋体">&nbsp;卖家内部帐号:</font></td>
                                <td style="WIDTH: 156px; HEIGHT: 18px" bgcolor="#ffffff" colspan="2"><font face="宋体">&nbsp;
											<asp:Label ID="LB_Fsale_uid" runat="server"></asp:Label></font></td>
                            </tr>
                            <tr>
                                <td style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee">&nbsp;买家账户号码:</td>
                                <td style="WIDTH: 136px; HEIGHT: 18px" bgcolor="#ffffff">&nbsp;
										<asp:Label ID="LB_Fbuyid" runat="server"></asp:Label></td>
                                <td style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee">&nbsp;卖家账户账号:</td>
                                <td style="WIDTH: 156px; HEIGHT: 18px" bgcolor="#ffffff" colspan="2">&nbsp;
										<asp:Label ID="LB_Fsaleid" runat="server"></asp:Label></td>
                            </tr>
                            <tr>
                                <td style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee">&nbsp;买家财付通账号:</td>
                                <td style="WIDTH: 136px; HEIGHT: 18px" bgcolor="#ffffff">&nbsp;
										<asp:Label ID="LB_FbuyidCFT" runat="server"></asp:Label></td>
                                <td style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee">&nbsp;卖家财付通账号:</td>
                                <td style="WIDTH: 156px; HEIGHT: 18px" bgcolor="#ffffff" colspan="2">&nbsp;
										<asp:Label ID="LB_FsaleidCFT" runat="server"></asp:Label></td>
                            </tr>
                            <tr>
                                <td style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee">&nbsp;买家真实名称:</td>
                                <td style="WIDTH: 136px; HEIGHT: 18px" bgcolor="#ffffff" height="18">&nbsp;
										<asp:Label ID="LB_Fbuy_name" runat="server"></asp:Label></td>
                                <td style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee">&nbsp;卖家真实名称:</td>
                                <td style="WIDTH: 156px; HEIGHT: 18px" bgcolor="#ffffff" colspan="2">&nbsp;
										<asp:Label ID="LB_Fsale_name" runat="server"></asp:Label></td>
                            </tr>
                            <tr>
                                <td style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee">
                                    <p>&nbsp;买家开户银行类型:</p>
                                </td>
                                <td style="WIDTH: 136px; HEIGHT: 18px" bgcolor="#ffffff">&nbsp;
										<asp:Label ID="LB_Fbuy_bank_type" runat="server"></asp:Label></td>
                                <td style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee">&nbsp;卖家开户银行类型:</td>
                                <td style="WIDTH: 156px; HEIGHT: 18px" bgcolor="#ffffff" colspan="2"><font face="宋体">&nbsp;
											<asp:Label ID="LB_Fsale_bank_type" runat="server"></asp:Label></font></td>
                            </tr>
                            <tr>
                                <%--<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
										height="18">&nbsp;币种代码:</TD>
									<TD style="WIDTH: 136px; HEIGHT: 18px" bgColor="#ffffff" height="18">&nbsp;
										<asp:label id="LB_Fcurtype" runat="server"></asp:label></TD>--%>
                                <td style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee"
                                    height="18">&nbsp;支付类型:</td>
                                <td style="WIDTH: 156px; HEIGHT: 18px" bgcolor="#ffffff" height="17">&nbsp;<font face="宋体">
                                    <asp:Label ID="LB_Fpay_type" runat="server"></asp:Label></font></td>
                                <td style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee" height="18">&nbsp;代金券ID:</td>
                                <td style="WIDTH: 156px; HEIGHT: 18px" bgcolor="#ffffff" colspan="2" height="17">&nbsp;<asp:Label ID="LB_Fsale_bankid" runat="server"></asp:Label></td>
                            </tr>
                            <tr>
                                <td style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee"
                                    height="18"><font face="宋体">&nbsp;产品的价格:</font></td>
                                <td style="WIDTH: 136px; HEIGHT: 18px" bgcolor="#ffffff" height="18"><font face="宋体">&nbsp;
											<asp:Label ID="LB_Fprice" runat="server"></asp:Label></font></td>
                                <td style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee" height="18"><font face="宋体">&nbsp;物流费用:</font></td>
                                <td style="WIDTH: 156px; HEIGHT: 18px" bgcolor="#ffffff" colspan="2" height="17"><font face="宋体">&nbsp;
											<asp:Label ID="LB_Fcarriage" runat="server"></asp:Label></font></td>
                            </tr>
                            <tr>
                                <td style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee"
                                    height="18"><font face="宋体">&nbsp;实际支付费用:</font></td>
                                <td style="WIDTH: 136px; HEIGHT: 18px" bgcolor="#ffffff" height="18"><font face="宋体">&nbsp;
											<asp:Label ID="LB_Fpaynum" runat="server"></asp:Label></font></td>
                                <td style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee" height="18">
                                    <p><font face="宋体">&nbsp;总支付费用:</font></p>
                                </td>
                                <td style="WIDTH: 156px; HEIGHT: 18px" bgcolor="#ffffff" colspan="2" height="17"><font face="宋体">&nbsp;
											<asp:Label ID="LB_Ffact" runat="server"></asp:Label></font></td>
                            </tr>
                            <tr>
                                <td style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee"
                                    height="18"><font face="宋体">&nbsp;交易(服务)手续费:</font></td>
                                <td style="WIDTH: 136px; HEIGHT: 18px" bgcolor="#ffffff" height="18"><font face="宋体">&nbsp;
											<asp:Label ID="LB_Fprocedure" runat="server"></asp:Label></font></td>
                                <%--<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
										height="18"><FONT face="宋体">&nbsp;服务费率:</FONT></TD>
									<TD style="WIDTH: 156px; HEIGHT: 18px" bgColor="#ffffff" colSpan="2" height="17"><FONT face="宋体">&nbsp;
											<asp:label id="LB_Fservice" runat="server"></asp:label></FONT></TD>--%>
                                <td sstyle="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee" height="18"><font face="宋体">&nbsp;备注:</font></td>
                                <td bgcolor="#ffffff" height="18"><font face="宋体">&nbsp;<asp:Label ID="LB_Fexplain" runat="server"></asp:Label></font></td>
                            </tr>
                            <tr>
                                <td style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee"
                                    height="18"><font face="宋体">&nbsp;现金支付金额:</font></td>
                                <td style="WIDTH: 136px; HEIGHT: 18px" bgcolor="#ffffff" height="18"><font face="宋体">&nbsp;
											<asp:Label ID="LB_Fcash" runat="server"></asp:Label></font></td>
                                <td style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee"
                                    height="18"><font face="宋体">&nbsp;订单编码:</font></td>
                                <td style="WIDTH: 156px; HEIGHT: 18px" bgcolor="#ffffff" colspan="2" height="17"><font face="宋体">&nbsp;
											<asp:Label ID="LB_Fcoding" runat="server"></asp:Label>
                                    <asp:HyperLink ID="HyperLink1" runat="server" Target="_blank"></asp:HyperLink></font></td>
                            </tr>
                            <tr>
                                <td style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee"
                                    height="18"><font face="宋体">&nbsp;订单创建时间(C2C):</font></td>
                                <td style="WIDTH: 136px; HEIGHT: 18px" bgcolor="#ffffff" height="18"><font face="宋体">&nbsp;
											<asp:Label ID="LB_FCreate_time_c2c" runat="server"></asp:Label></font></td>
                                <td style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee" height="18">
                                    <p><font face="宋体">&nbsp;订单创建时间(本地):</font></p>
                                </td>
                                <td style="WIDTH: 156px; HEIGHT: 18px" bgcolor="#ffffff" colspan="2" height="17"><font face="宋体">&nbsp;
											<asp:Label ID="LB_Fcreate_time" runat="server"></asp:Label></font></td>
                            </tr>
                            <tr>
                                <td style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee"
                                    height="18"><font face="宋体">&nbsp;买家付款时间(Bank):</font></td>
                                <td style="WIDTH: 136px; HEIGHT: 18px" bgcolor="#ffffff" height="18"><font face="宋体">&nbsp;
											<asp:Label ID="LB_Fbargain_time" runat="server"></asp:Label></font></td>
                                <td style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee"
                                    height="18">
                                    <p><font face="宋体">&nbsp;买家付款时间(本地):</font></p>
                                </td>
                                <td style="WIDTH: 156px; HEIGHT: 18px" bgcolor="#ffffff" colspan="2" height="17"><font face="宋体">&nbsp;
											<asp:Label ID="LB_Fpay_time" runat="server"></asp:Label>&nbsp;</font></td>
                            </tr>
                            <tr>
                                <td style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee"
                                    height="18"><font face="宋体">&nbsp;可打款给卖家时间(C2C):</font></td>
                                <td style="WIDTH: 136px; HEIGHT: 18px" bgcolor="#ffffff" height="18"><font face="宋体">&nbsp;
											<asp:Label ID="LB_Freceive_time_c2c" runat="server"></asp:Label></font></td>
                                <td style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee"
                                    height="18"><font face="宋体">&nbsp;可打款卖家时间(本地):</font></td>
                                <td style="WIDTH: 156px; HEIGHT: 18px" bgcolor="#ffffff" colspan="2" height="17"><font face="宋体">&nbsp;
											<asp:Label ID="LB_Freceive_time" runat="server"></asp:Label></font></td>
                            </tr>
                            <tr>
                                <td style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee"
                                    height="18"><font face="宋体">&nbsp;最后修改交易单的IP:</font></td>
                                <td style="WIDTH: 136px; HEIGHT: 18px" bgcolor="#ffffff" height="18"><font face="宋体">&nbsp;
											<asp:Label ID="LB_Fip" runat="server"></asp:Label></font></td>
                                <td style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee"
                                    height="18"><font face="宋体">&nbsp;最后修改时间(本地)</font></td>
                                <td style="WIDTH: 156px; HEIGHT: 18px" bgcolor="#ffffff" colspan="2" height="17"><font face="宋体">&nbsp;
											<asp:Label ID="LB_Fmodify_time" runat="server"></asp:Label></font></td>
                            </tr>
                   <%--         <tr>
                                <td sstyle="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee"
                                    height="18"><font face="宋体">&nbsp;备注:</font></td>
                                <td bgcolor="#ffffff" height="18"><font face="宋体">&nbsp;
											<asp:Label ID="LB_Fexplain" runat="server"></asp:Label></font></td>
                                <td style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee" height="18">
                                    <font face="宋体">&nbsp;</font>
                                </td>
                                <td style="WIDTH: 156px; HEIGHT: 18px" bgcolor="#ffffff" colspan="2" height="17">
                                    <font face="宋体">&nbsp;</font>
                                </td>
                            </tr>--%>
                            <tr>
                                <td style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee" height="18">
                                    <font face="宋体">&nbsp;渠道编号：</font>
                                </td>
                                <td style="WIDTH: 136px; HEIGHT: 18px" bgcolor="#ffffff" height="18">
                                    <font face="宋体">&nbsp;<asp:Label ID="Fchannel_idName" runat="server"></asp:Label></font>
                                </td>
                                <td style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee" height="18">
                                    <font face="宋体">&nbsp;交易说明：</font>
                                </td>
                                <td style="WIDTH: 156px; HEIGHT: 18px" bgcolor="#ffffff" colspan="2" height="17">
                                    <font face="宋体">&nbsp;<asp:Label ID="Fmemo" runat="server"></asp:Label></font>
                                </td>
                            </tr>
                            <tr>
                                <td style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee" height="18">
                                    <font face="宋体">&nbsp;退款类型：</font>
                                </td>
                                <td style="WIDTH: 136px; HEIGHT: 18px" bgcolor="#ffffff" height="18">
                                    <font face="宋体">&nbsp;<asp:Label ID="Frefund_typeName" runat="server"></asp:Label></font>
                                </td>
                                <td style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee" height="18">
                                    <font face="宋体">&nbsp;退买家金额：</font>
                                </td>
                                <td style="WIDTH: 156px; HEIGHT: 18px" bgcolor="#ffffff" colspan="2" height="17">
                                    <font face="宋体">&nbsp;<asp:Label ID="FpaybuyName" runat="server"></asp:Label></font>
                                </td>
                            </tr>
                            <tr>
                                <td style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee" height="18">
                                    <font face="宋体">&nbsp;退卖家金额：</font>
                                </td>
                                <td style="WIDTH: 136px; HEIGHT: 18px" bgcolor="#ffffff" height="18">
                                    <font face="宋体">&nbsp;<asp:Label ID="FpaysaleName" runat="server"></asp:Label></font>
                                </td>
                                <td style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee" height="18">
                                    <font face="宋体">&nbsp;请求退款时间：</font>
                                </td>
                                <td style="WIDTH: 156px; HEIGHT: 18px" bgcolor="#ffffff" colspan="2" height="17">
                                    <font face="宋体">&nbsp;<asp:Label ID="Freq_refund_time" runat="server"></asp:Label></font>
                                </td>
                            </tr>
                            <tr>
                                <td style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee" height="18">
                                    <font face="宋体">&nbsp;退款时间：</font>
                                </td>
                                <td style="WIDTH: 136px; HEIGHT: 18px" bgcolor="#ffffff" height="18">
                                    <font face="宋体">&nbsp;<asp:Label ID="Fok_time" runat="server"></asp:Label></font>
                                </td>
                                <td style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee" height="18">
                                    <font face="宋体">&nbsp;退款时间(帐务)：</font>
                                </td>
                                <td style="WIDTH: 156px; HEIGHT: 18px" bgcolor="#ffffff" colspan="2" height="17">
                                    <font face="宋体">&nbsp;<asp:Label ID="Fok_time_acc" runat="server"></asp:Label></font>
                                </td>
                            </tr>
                            <tr>
                                <%--<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee" height="18">
											<FONT face="宋体">&nbsp;申诉标志：</FONT>
										</TD>
										<TD style="WIDTH: 136px; HEIGHT: 18px" bgColor="#ffffff" height="18">
											<FONT face="宋体">&nbsp;<asp:label id="Fappeal_signName" runat="server"></asp:label></FONT>
										</TD>--%>
                                <td style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee" height="18">
                                    <font face="宋体">&nbsp;中介标志：</font>
                                </td>
                                <td style="WIDTH: 156px; HEIGHT: 18px" bgcolor="#ffffff" colspan="4" height="17">
                                    <font face="宋体">&nbsp;<asp:Label ID="Fmedi_signName" runat="server"></asp:Label></font>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <br>
            <table height="35" cellspacing="0" cellpadding="0" width="95%" align="center" border="0">
                <tr>
                    <td bgcolor="#666666">
                        <table height="35" cellspacing="1" cellpadding="0" width="100%" align="center" border="0">
                            <tr bgcolor="#e4e5f7">
                                <td bgcolor="#e4e5f7" height="20">
                                    <div align="left">
                                        <table height="20" cellspacing="0" cellpadding="1" width="100%" background="../IMAGES/Page/bg_bl.gif"
                                            border="0">
                                            <tr>
                                                <td style="WIDTH: 216px" width="216"><font color="#ff0000">&nbsp;<img height="16" src="../IMAGES/Page/post.gif" width="20">
                                                    &nbsp;充值记录 </font>
                                                </td>
                                                <td width="55%">&nbsp;
														<asp:Label ID="Label1_listID" runat="server" Width="246px" Visible="False">Label</asp:Label></td>
                                                <td width="5%">
                                                    <div align="center"><font face="宋体"></font>&nbsp;</div>
                                                </td>
                                                <td width="5%">
                                                    <div class="style2" align="center"><a href="tradeLogQueryNew.aspx?id=<%=LB_Flistid.Text.Trim()%>" target="_blank"><font color="red">全屏</font></a>|</div>
                                                </td>
                                                <td width="4%">
                                                    <div class="style2" align="center"><a href="javascript:window.self.iframe1.location.reload()">刷新</a></div>
                                                </td>
                                                <td width="4%">
                                                    <div align="center">
                                                        <img height="18" src="../IMAGES/Page/down.gif">
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td bgcolor="#ffffff" height="12">
                                    <iframe
                                        name="iframe1" marginwidth="0" marginheight="0"
                                        src="<%=iFramePath_Gathering%>" frameborder="0" width="100%"></iframe>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <br>
            <table height="35" cellspacing="0" cellpadding="0" width="95%" align="center" border="0">
                <tr>
                    <td bgcolor="#666666">
                        <table height="35" cellspacing="1" cellpadding="0" width="100%" align="center" border="0">
                            <tr bgcolor="#e4e5f7">
                                <td height="20">
                                    <div align="center"></div>
                                    <table height="20" cellspacing="0" cellpadding="1" width="100%" background="../IMAGES/Page/bg_bl.gif"
                                        border="0">
                                        <tr>
                                            <td><font color="#ff0000">&nbsp;
                                                <img height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp; 
														资金流水 </font></td>
                                            <td>&nbsp;</td>
                                            <td width="5%">
                                                <div align="center"><font face="宋体"></font>&nbsp;</div>
                                            </td>
                                            <td width="5%">
                                                <div class="style2" align="center"><a href="tradeLogQueryNew.aspx?id=<%=LB_Flistid.Text.Trim()%>" target="_blank"><font color="red"><font color="red">全屏</font></font></a>|</div>
                                            </td>
                                            <td style="WIDTH: 25px" width="26">
                                                <div class="style2" align="center"><a href="javascript:window.self.iframe3.location.reload()">刷新</a></div>
                                            </td>
                                            <td width="4%">
                                                <div align="center">
                                                    <img height="18" src="../IMAGES/Page/down.gif">
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td bgcolor="#ffffff" height="12">
                                    <iframe
                                        name="iframe3" marginwidth="0" marginheight="0"
                                        src="<%=iFramePath_bankrollLog%>" frameborder="0" width="100%"></iframe>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <br>
            <table height="35" cellspacing="0" cellpadding="0" width="95%" align="center" border="0">
                <tr>
                    <td bgcolor="#666666">
                        <table height="35" cellspacing="1" cellpadding="0" width="100%" align="right" border="0">
                            <tr bgcolor="#e4e5f7">
                                <td height="20">
                                    <table height="20" cellspacing="0" cellpadding="1" width="100%" background="../IMAGES/Page/bg_bl.gif"
                                        border="0">
                                        <tr>
                                            <td><font color="#ff0000">&nbsp;<img height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp; 
														交易流水
                                            </font>
                                            </td>
                                            <td>&nbsp;</td>
                                            <td style="WIDTH: 42px" width="42">
                                                <div align="center"><font face="宋体"></font>&nbsp;</div>
                                            </td>
                                            <td width="5%">
                                                <div class="style2" align="center"><a href="tradeLogQueryNew.aspx?id=<%=LB_Flistid.Text.Trim()%>" target="_blank"><font color="red">全屏</font></a>|</div>
                                            </td>
                                            <td width="4%">
                                                <div class="style2" align="center"><a href="javascript:window.self.iframe4.location.reload()">刷新</a></div>
                                            </td>
                                            <td width="4%">
                                                <div align="center">
                                                    <img height="18" src="../IMAGES/Page/down.gif">
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td bgcolor="#ffffff" height="12">
                                    <iframe
                                        name="iframe4" marginwidth="0" marginheight="0"
                                        src="<%=iFramePath_TradeLog%>" frameborder="0" width="100%"></iframe>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <br>
            <br>
            <!-- 不需要的功能暂时隐藏-->
        </div>
    </form>
</body>
</html>
