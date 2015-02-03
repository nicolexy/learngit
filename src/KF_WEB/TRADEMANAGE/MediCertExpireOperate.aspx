<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Page language="c#" Codebehind="MediCertExpireOperate.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.TradeManage.MediCertExpireOperate" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>MediCertExpireOperate</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); UNKNOWN { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
        <TABLE  style="POSITION: absolute; TOP: 150px;left:200px" cellSpacing="1" cellPadding="1" width="600" border="1">
                 <TR>
					<TD align="right" width="70px"><FONT face="宋体">商户号：</FONT>
					</TD>
					<TD align="left"><asp:Label id="tbSpid" runat="server"></asp:Label></TD>
				</TR>
                <TR>
					<TD align="right" width="70px"><FONT face="宋体">备注：</FONT>
					</TD>
					<TD align="left"><asp:textbox id="tbmemo" runat="server" Width="500px" Height="120px" TextMode="MultiLine"></asp:textbox><asp:requiredfieldvalidator id="Requiredfieldvalidator1" runat="server" ErrorMessage="请输入" ControlToValidate="tbmemo"></asp:requiredfieldvalidator></TD>
				</TR>
                 <TR>
                      <TD colspan="2" align="center"><asp:button id="btnSave" runat="server" Width="80px" Text="保存" onclick="btnSave_Click"></asp:button></TD>
                </TR>
			</TABLE>
		</form>
	</body>
</HTML>
