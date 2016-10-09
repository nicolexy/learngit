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
                                �û�����</font>
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
                                <td width="78%"><font face="����">���������ʺ�(�Ƹ�ͨ����Q):<asp:TextBox ID="TextBox1_QQID" runat="server" BorderStyle="Solid" BorderWidth="1px"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;
											<asp:Label ID="ValidateID" ForeColor="Red" runat="Server"></asp:Label></font></td>
                                <td width="3%">&nbsp;</td>
                            </tr>
                            <tr>
                                <td width="19%">&nbsp;</td>
                                <td width="78%"><font face="����">���������ʺ�(΢���˺�):<asp:TextBox ID="TextBox2_WX" runat="server" BorderStyle="Solid" BorderWidth="1px"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;
                                </font></td>
                                <td width="3%">&nbsp;</td>
                            </tr>
                            <tr>
                                <td width="19%"></td>
                                <td width="78%"><font face="����">�ٴ�ȷ���ʺ�:
											<asp:TextBox ID="txbConfirmQ" runat="server" BorderStyle="Solid" BorderWidth="1px"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
											<asp:RequiredFieldValidator ID="Requiredfieldvalidator2" runat="server" Display="Dynamic" ErrorMessage="RequiredFieldValidator"
                                                Width="77px" ControlToValidate="txbConfirmQ">�������ʺ�</asp:RequiredFieldValidator></font></td>
                                <td width="3%"></td>
                            </tr>
                            <tr>
                                <td width="19%"></td>
                                <td width="78%"><font face="����">��������ԭ��:
											<asp:TextBox ID="txtReason" runat="server" BorderStyle="Solid" BorderWidth="1px" Width="286px"
                                                Height="40px" TextMode="MultiLine"></asp:TextBox>&nbsp;
											<asp:RequiredFieldValidator ID="Requiredfieldvalidator3" runat="server" Display="Dynamic" ErrorMessage="RequiredFieldValidator"
                                                Width="63px" ControlToValidate="txtReason">������ԭ��</asp:RequiredFieldValidator></font></td>
                                <td width="3%"></td>
                            </tr>
                            <tr>
                                <td width="19%"></td>
                                <td width="78%"><font face="����">ϵͳ�Զ������Ƿ�֪ͨ�û�:<asp:CheckBox ID="EmailCheckBox" runat="server"
                                    />��</font></td>
                                <td width="3%"></td>
                            </tr>
                            <tr>
                                <td width="19%"></td>
                                <td width="78%"><font face="����">�����û������ַ:
											<asp:TextBox ID="txtEmail" runat="server" BorderStyle="Solid" BorderWidth="1px" Width="135px">
                                            </asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</font></td>
                                <td width="3%"></td>
                            </tr>
                        </table>
                    </div>
                    <div align="left"></div>
                    <div align="center"><font face="����"></font></div>
                </td>
                <td width="25%">
                    <div align="center">
                        &nbsp;
							<asp:Button ID="btLogOn" runat="server" Width="122px" Height="31px" Text="��������" OnClick="btLogOn_Click"></asp:Button>
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
                                <img height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp;&nbsp;������ʷ��ϸ(����10��)
                            </font>
                            </td>
                        </tr>
                        <tr>
                            <td bgcolor="#ffffff" height="12"><font face="����"></font><font face="����">
                                <asp:DataGrid ID="dgInfo" runat="server" Width="100%" AutoGenerateColumns="False">
                                    <HeaderStyle BackColor="#EEEEEE"></HeaderStyle>
                                    <Columns>
                                        <asp:BoundColumn DataField="Fid" HeaderText="ID"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="Fqqid" HeaderText="�ʺ�"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="Fquid" HeaderText="�ڲ��ʺ�"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="Freason" HeaderText="����ԭ��"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="handid" HeaderText="ִ����"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="handip" HeaderText="ִ����IP"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="FlastModifyTime" HeaderText="����޸�ʱ��"></asp:BoundColumn>
                                    </Columns>
                                </asp:DataGrid></font></td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <p align="center">
            <asp:LinkButton ID="lkHistoryQuery" runat="server" CausesValidation="False" OnClick="lkHistoryQuery_Click">�߼���ѯ</asp:LinkButton></p>
        <table id="TABLE3" cellspacing="0" cellpadding="0" width="90%" align="center" border="0"
            runat="server" visible="false">
            <tr>
                <td bgcolor="#666666">
                    <table style="HEIGHT: 119px" cellspacing="1" cellpadding="0" width="100%" align="center"
                        border="0">
                        <tr bgcolor="#e4e5f7">
                            <td colspan="6" height="20">
                                <p align="left"><font color="#ff0000">
                                    <img height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp;&nbsp;������ѯ&nbsp;&nbsp;</font></p>
                            </td>
                        </tr>
                        <tr>
                            <td style="WIDTH: 128px; HEIGHT: 33px" bgcolor="#ffffff" height="33">
                                <p align="center"><font face="����">��ʼ����</font></p>
                            </td>
                            <td style="WIDTH: 299px; HEIGHT: 33px" bgcolor="#ffffff" height="33">
                                <font face="����">
                                    <input type="text" runat="server" id="TextBoxBeginDate" onclick="WdatePicker()" />
                                    <img onclick="TextBoxBeginDate.click()" src="../SCRIPTS/My97DatePicker/skin/datePicker.gif" width="16" height="22" style="width: 16px; height: 22px; cursor: pointer;" alt="ѡ������" />
                                </font>
                            </td>
                            <td style="WIDTH: 139px; HEIGHT: 33px" bgcolor="#ffffff" height="33"><font face="����">��������</font></td>
                            <td colspan="2" style="HEIGHT: 33px" bgcolor="#ffffff" height="33">
                                <font face="����">
                                    <input type="text" runat="server" id="TextBoxEndDate" onclick="WdatePicker()" />
                                    <img onclick="TextBoxEndDate.click()" src="../SCRIPTS/My97DatePicker/skin/datePicker.gif" width="16" height="22" style="width: 16px; height: 22px; cursor: pointer;" alt="ѡ������" />
                                </font></td>
                        </tr>
                        <tr>
                            <td style="WIDTH: 128px; HEIGHT: 1px" bgcolor="#ffffff" height="1">
                                <p align="center"><font face="����">�������ʺ�</font></p>
                            </td>
                            <td style="WIDTH: 299px; HEIGHT: 1px" bgcolor="#ffffff" height="1"><font face="����">
                                <asp:TextBox ID="TxbQueryQQ" runat="server" BorderStyle="Groove"></asp:TextBox></font></td>
                            <td style="WIDTH: 139px; HEIGHT: 1px" bgcolor="#ffffff" height="1"><font face="����">������Ա</font></td>
                            <td style="HEIGHT: 1px" bgcolor="#ffffff" height="1"><font face="����">
                                <asp:TextBox ID="txbHandID" runat="server" BorderStyle="Groove"></asp:TextBox></font></td>
                            <td bgcolor="#ffffff">΢�Ű�QQ<asp:TextBox ID="tbWxQQ" runat="server" BorderStyle="Groove"></asp:TextBox>&nbsp;&nbsp;΢�Ű�����<asp:TextBox ID="tbWxEmail" runat="server" BorderStyle="Groove"></asp:TextBox>&nbsp;&nbsp;΢�Ű��ֻ�<asp:TextBox ID="tbWxPhone" runat="server" BorderStyle="Groove"></asp:TextBox>&nbsp;&nbsp;΢�ź�<asp:TextBox ID="tbWxNo" runat="server" BorderStyle="Groove"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td bgcolor="#ffffff" colspan="5" height="12"><font face="����">
                                <asp:Button ID="btQuery" runat="server" Width="108px" Text="�� ѯ" CausesValidation="False" OnClick="btQuery_Click"></asp:Button>
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
