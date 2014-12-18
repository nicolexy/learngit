<%@ Page language="c#" Codebehind="OrderMigration.aspx.cs" AutoEventWireup="false" Inherits="TENCENT.OSS.CFT.KF.KF_Web.TradeManage.OrderMigration" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
    <HEAD>
        <title>OrderMigration</title>
        <meta name="vs_defaultClientScript" content="JavaScript">
        <style type="text/css">@import url( ../STYLES/ossstyle.css ); .style2 { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
	.style4 { COLOR: #ff0000 }
	</style>
    </HEAD>
    <body>
        <form id="Form1" method="post" runat="server">
            <table border="0" cellSpacing="1" cellPadding="0" width="95%" bgColor="#666666" align="center">
                <tr bgColor="#e4e5f7" background="../IMAGES/Page/bg_bl.gif">
                    <td height="20" vAlign="middle" colSpan="2">
                        <table border="0" cellSpacing="0" cellPadding="1" width="100%" height="90%">
                            <tr>
                                <td height="18" background="../IMAGES/Page/bg_bl.gif" width="80%"><font color="#ff0000"><STRONG><FONT color="#ff0000">&nbsp;</FONT></STRONG><IMG src="../IMAGES/Page/post.gif" width="20" height="16">
                                        历史订单迁移</font>
                                    <div align="right"><FONT color="#ff0000" face="Tahoma"></FONT></div>
                                </td>
                                <td background="../IMAGES/Page/bg_bl.gif" width="20%">操作员代码:
                                    <span class="style3">
                                        <asp:label id="lblUId" runat="server">Label</asp:label>
                                    </span></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr bgColor="#ffffff">
                    <td>
                        <table border="0" cellSpacing="0" cellPadding="1" width="100%" height="100%">
                            <tr>
                                <td width="78%" height="37">
                                    <P align="left">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                                        订单号：&nbsp;
                                        <asp:textbox id="txtOrderId" runat="server" Width="200px" BorderWidth="1px" BorderStyle="Solid"></asp:textbox>
                                        <asp:regularexpressionvalidator id="rfvNum" runat="server" Display="Dynamic" ValidationExpression="^\d{0,32}$"
                                            ControlToValidate="txtOrderId" ErrorMessage="RegularExpressionValidator" Enabled="True">订单号有误，订单号必须为32位内数字!</asp:regularexpressionvalidator><br>
                                    </P>
                                <TD width="3%" height="40">&nbsp;</TD>
                            </tr>
                        </table>
                    </td>
                    <td width="25%">
                        <div align="center"><asp:button id="btMigration" runat="server" BorderStyle="Groove" Width="66px" Text="迁移" Height="23px"></asp:button>&nbsp;
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:label id="labErrMsg" runat="server" ForeColor="Red"></asp:label>
                    </td>
                </tr>
            </table>
        </form>
    </body>
</HTML>
