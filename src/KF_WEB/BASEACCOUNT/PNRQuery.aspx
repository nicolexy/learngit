<%@ Page language="c#" Codebehind="PNRQuery.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.BaseAccount.PNRQuery" %>
<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>PNRQuery</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<style type="text/css">@import url( ../STYLES/ossstyle.css );
UNKNOWN {
	COLOR: #000000
}
.style3 {
	COLOR: #ff0000
}
BODY {
	BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif)
}
		</style>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE border="1" cellSpacing="1" cellPadding="1" width="820">
				<TR>
					<TD style="WIDTH: 100%" bgColor="#e4e5f7" colSpan="5"><FONT face="宋体"><FONT color="red"><IMG src="../IMAGES/Page/post.gif" width="20" height="16">&nbsp;&nbsp;签约关系查询</FONT>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						</FONT>操作员代码: </FONT><SPAN class="style3"><asp:label id="Label1" runat="server" Width="73px" ForeColor="Red"></asp:label></SPAN></TD>
				</TR>
				<TR>
					<TD align="right"><asp:label id="Label7" runat="server">财付通帐号</asp:label></TD>
					<TD><asp:textbox id="txtQQ" runat="server"></asp:textbox></TD>
					<TD colSpan="2" align="center"><asp:button id="Button2" runat="server" Width="80px" Text="查 询" onclick="Button2_Click"></asp:button></TD>
				</TR>
			</TABLE>
			<table id="Table1" border="0" cellSpacing="0" cellPadding="0" width="820" runat="server">
				<TR>
					<TD vAlign="top" align="center"><asp:checkboxlist id="cblSpid" Runat="server">
							<asp:ListItem Value="*">所有</asp:ListItem>
							<asp:ListItem Value="1205733301">中国国际航空股份有限公司</asp:ListItem>
							<asp:ListItem Value="1201187901">中国南方航空股份有限公司</asp:ListItem>
							<asp:ListItem Value="1204311001">海南航空股份有限公司(B2B)</asp:ListItem>
							<asp:ListItem Value="1203652401">海南航空股份有限公司(B2C)</asp:ListItem>
							<asp:ListItem Value="1204760501">中国东方航空股份有限公司</asp:ListItem>
							<asp:ListItem Value="1204996901">四川航空股份有限公司(B2B)</asp:ListItem>
							<asp:ListItem Value="1204997001">四川航空股份有限公司(B2C)</asp:ListItem>
							<asp:ListItem Value="1203292301">深圳航空有限责任公司</asp:ListItem>
							<asp:ListItem Value="1203998801">山东航空股份有限公司</asp:ListItem>
							<asp:ListItem Value="1202405701">春秋航空有限公司</asp:ListItem>
							<asp:ListItem Value="1203891701">厦门航空有限公司</asp:ListItem>
							<asp:ListItem Value="1204986801">西部航空有限责任公司</asp:ListItem>
							<asp:ListItem Value="1204706901">金鹿航空有限公司</asp:ListItem>
						</asp:checkboxlist></TD>
				</TR>
			</table>
		</form>
	</body>
</HTML>
