<%@ Page Language="c#" CodeBehind="logOnUser.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.C2C.KF.KF_Web.BaseAccount.logOnUser" %>

<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>logOnUser</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="C#" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <style type="text/css">
    @import url( ../STYLES/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %> );
    .style2 { FONT-WEIGHT: bold; COLOR: #ff0000 }
	.style3 { COLOR: #000000 }
	.style4 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
    <script type="text/javascript" src="../SCRIPTS/My97DatePicker/WdatePicker.js"></script>    
</head>
<body>
    <form id="Form1" method="post" runat="server">
        <table cellspacing="1" cellpadding="0" width="90%" align="center" bgcolor="#666666" border="0">
            <tr bgcolor="#e4e5f7">
                <td valign="middle" colspan="2" height="20">
                    <table cellspacing="0" cellpadding="1" width="100%" border="0">
                        <tr>
                            <td width="80%" height="18"><font color="#ff0000"><strong><font color="#ff0000">&nbsp;</font></strong><img height="16" src="../IMAGES/Page/post.gif" width="20">
                                用户销户</font>
                                <div align="right"></div>
                            </td>
                            <td width="20%">
                                <asp:Label ID="Label1" runat="server"></asp:Label></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr bgcolor="#ffffff">
                <td>
                    <div align="center"></div>
                    <div align="left">
                        <table cellspacing="0" cellpadding="1" width="100%" border="0">
                            <tr>
                                <td width="19%">&nbsp;</td>
                                <td width="78%"><font face="宋体">输入销户帐号(财付通、手Q):<asp:TextBox ID="TextBox1_QQID" runat="server" BorderStyle="Solid" BorderWidth="1px"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;
											<asp:Label ID="ValidateID" ForeColor="Red" runat="Server"></asp:Label></font></td>
                                <td width="3%">&nbsp;</td>
                            </tr>
                            <tr>
                                <td width="19%">&nbsp;</td>
                                <td width="78%"><font face="宋体">输入销户帐号(微信账号):<asp:TextBox ID="TextBox2_WX" runat="server" BorderStyle="Solid" BorderWidth="1px"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;
                                </font></td>
                                <td width="3%">&nbsp;</td>
                            </tr>
                            <tr>
                                <td width="19%"></td>
                                <td width="78%"><font face="宋体">再次确认帐号:
											<asp:TextBox ID="txbConfirmQ" runat="server" BorderStyle="Solid" BorderWidth="1px"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
											<asp:RequiredFieldValidator ID="Requiredfieldvalidator2" runat="server" Display="Dynamic" ErrorMessage="RequiredFieldValidator"
                                                Width="77px" ControlToValidate="txbConfirmQ">请输入帐号</asp:RequiredFieldValidator></font></td>
                                <td width="3%"></td>
                            </tr>
                            <tr>
                                <td width="19%"></td>
                                <td width="78%"><font face="宋体">输入销户原因:
											<asp:TextBox ID="txtReason" runat="server" BorderStyle="Solid" BorderWidth="1px" Width="286px"
                                                Height="40px" TextMode="MultiLine"></asp:TextBox>&nbsp;
											<asp:RequiredFieldValidator ID="Requiredfieldvalidator3" runat="server" Display="Dynamic" ErrorMessage="RequiredFieldValidator"
                                                Width="63px" ControlToValidate="txtReason">请输入原因</asp:RequiredFieldValidator></font></td>
                                <td width="3%"></td>
                            </tr>
                            <tr>
                                <td width="19%"></td>
                                <td width="78%"><font face="宋体">系统自动销户是否通知用户:<asp:CheckBox ID="EmailCheckBox" runat="server"
                                    />是</font></td>
                                <td width="3%"></td>
                            </tr>
                            <tr>
                                <td width="19%"></td>
                                <td width="78%"><font face="宋体">输入用户邮箱地址:
											<asp:TextBox ID="txtEmail" runat="server" BorderStyle="Solid" BorderWidth="1px" Width="135px">
                                            </asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</font></td>
                                <td width="3%"></td>
                            </tr>
                        </table>
                    </div>
                    <div align="left"></div>
                    <div align="center"><font face="宋体"></font></div>
                </td>
                <td width="25%">
                    <div align="center">
                        &nbsp;
							<asp:Button ID="btLogOn" runat="server" Width="122px" Height="31px" Text="销户申请" OnClick="btLogOn_Click"></asp:Button>
                    </div>
                </td>
            </tr>
        </table>
        <table id="Table1" cellspacing="0" cellpadding="0" width="90%" align="center" border="0"
            runat="server" visible="true">
            <tr>
                <td bgcolor="#666666">
                    <table id="Table2" cellspacing="1" cellpadding="0" width="100%" align="center" border="0">
                        <tr bgcolor="#e4e5f7">
                            <td height="20"><font color="#ff0000">
                                <img height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp;&nbsp;销户历史明细(最新10条)
                            </font>
                            </td>
                        </tr>
                        <tr>
                            <td bgcolor="#ffffff" height="12"><font face="宋体"></font><font face="宋体">
                                <asp:DataGrid ID="dgInfo" runat="server" Width="100%" AutoGenerateColumns="False">
                                    <HeaderStyle BackColor="#EEEEEE"></HeaderStyle>
                                    <Columns>
                                        <asp:BoundColumn DataField="Fid" HeaderText="ID"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="Fqqid" HeaderText="帐号"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="Fquid" HeaderText="内部帐号"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="Freason" HeaderText="销户原因"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="handid" HeaderText="执行人"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="handip" HeaderText="执行人IP"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="FlastModifyTime" HeaderText="最后修改时间"></asp:BoundColumn>
                                    </Columns>
                                </asp:DataGrid></font></td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <p align="center">
            <asp:LinkButton ID="lkHistoryQuery" runat="server" CausesValidation="False" OnClick="lkHistoryQuery_Click">高级查询</asp:LinkButton></p>
        <table id="TABLE3" cellspacing="0" cellpadding="0" width="90%" align="center" border="0"
            runat="server" visible="false">
            <tr>
                <td bgcolor="#666666">
                    <table style="HEIGHT: 119px" cellspacing="1" cellpadding="0" width="100%" align="center"
                        border="0">
                        <tr bgcolor="#e4e5f7">
                            <td colspan="6" height="20">
                                <p align="left"><font color="#ff0000">
                                    <img height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp;&nbsp;销户查询&nbsp;&nbsp;</font></p>
                            </td>
                        </tr>
                        <tr>
                            <td style="WIDTH: 128px; HEIGHT: 33px" bgcolor="#ffffff" height="33">
                                <p align="center"><font face="宋体">开始日期</font></p>
                            </td>
                            <td style="WIDTH: 299px; HEIGHT: 33px" bgcolor="#ffffff" height="33">
                                <font face="宋体">
                                    <input type="text" runat="server" id="TextBoxBeginDate" onclick="WdatePicker()" />
                                    <img onclick="TextBoxBeginDate.click()" src="../SCRIPTS/My97DatePicker/skin/datePicker.gif" width="16" height="22" style="width: 16px; height: 22px; cursor: pointer;" alt="选择日期" />
                                </font>
                            </td>
                            <td style="WIDTH: 139px; HEIGHT: 33px" bgcolor="#ffffff" height="33"><font face="宋体">结束日期</font></td>
                            <td colspan="2" style="HEIGHT: 33px" bgcolor="#ffffff" height="33">
                                <font face="宋体">
                                    <input type="text" runat="server" id="TextBoxEndDate" onclick="WdatePicker()" />
                                    <img onclick="TextBoxEndDate.click()" src="../SCRIPTS/My97DatePicker/skin/datePicker.gif" width="16" height="22" style="width: 16px; height: 22px; cursor: pointer;" alt="选择日期" />
                                </font></td>
                        </tr>
                        <tr>
                            <td style="WIDTH: 128px; HEIGHT: 1px" bgcolor="#ffffff" height="1">
                                <p align="center"><font face="宋体">按销户帐号</font></p>
                            </td>
                            <td style="WIDTH: 299px; HEIGHT: 1px" bgcolor="#ffffff" height="1"><font face="宋体">
                                <asp:TextBox ID="TxbQueryQQ" runat="server" BorderStyle="Groove"></asp:TextBox></font></td>
                            <td style="WIDTH: 139px; HEIGHT: 1px" bgcolor="#ffffff" height="1"><font face="宋体">按操作员</font></td>
                            <td style="HEIGHT: 1px" bgcolor="#ffffff" height="1"><font face="宋体">
                                <asp:TextBox ID="txbHandID" runat="server" BorderStyle="Groove"></asp:TextBox></font></td>
                            <td bgcolor="#ffffff">微信绑定QQ<asp:TextBox ID="tbWxQQ" runat="server" BorderStyle="Groove"></asp:TextBox>&nbsp;&nbsp;微信绑定邮箱<asp:TextBox ID="tbWxEmail" runat="server" BorderStyle="Groove"></asp:TextBox>&nbsp;&nbsp;微信绑定手机<asp:TextBox ID="tbWxPhone" runat="server" BorderStyle="Groove"></asp:TextBox>&nbsp;&nbsp;微信号<asp:TextBox ID="tbWxNo" runat="server" BorderStyle="Groove"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td bgcolor="#ffffff" colspan="5" height="12"><font face="宋体">
                                <asp:Button ID="btQuery" runat="server" Width="108px" Text="查 询" CausesValidation="False" OnClick="btQuery_Click"></asp:Button>
                            </font>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
