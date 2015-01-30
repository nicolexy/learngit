<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Page language="c#" Codebehind="QueryFreeFlow.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.NewQueryInfoPages.QueryFreeFlow" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>ComplainBussinessInput</title>
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
            
			<TABLE style="LEFT: 5%; POSITION: absolute; TOP: 5%" cellSpacing="1" cellPadding="1" width="820"
				border="1">
				<TR>
					<TD style="WIDTH: 100%" bgColor="#e4e5f7"><FONT face="宋体"><FONT color="red"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp;&nbsp;免费流量信息查询</FONT>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						</FONT>操作员代码: </FONT><SPAN class="style3"><asp:label id="Label1" runat="server" ForeColor="Red" Width="73px"></asp:label></SPAN></TD>
				</TR>
				<TR>
                    <TD align="left" colspan="4"><asp:label id="Label2" runat="server">账号：</asp:label>
                    <asp:textbox id="cftNo" style="WIDTH: 180px;" runat="server"></asp:textbox>
                    <asp:button id="btnQuery" runat="server" Width="80px" Text="查 询" onclick="btnQuery_Click"></asp:button>
                    </TD>
				</TR>
			</TABLE>
			
            <TABLE id="detailTB" cellSpacing="1" cellPadding="1" style="Z-INDEX: 102; LEFT: 5.02%; WIDTH: 820px; POSITION: absolute; TOP: 104px; "  border="1" runat="server">
				<TR>
					<TD><FONT face="宋体">&nbsp;账号：</FONT>&nbsp;&nbsp;<asp:label id="lb_c1" runat="server"></asp:label></TD>
				</TR>
                <TR>
					<TD><FONT face="宋体">&nbsp;是否为QQ会员：</FONT>&nbsp;&nbsp;<asp:label id="lb_c2" runat="server"></asp:label></FONT></TD>
				</TR>
                <TR>
					<TD><FONT face="宋体">&nbsp;会员过期时间：</FONT>&nbsp;&nbsp;<asp:label id="lb_c3" runat="server"></asp:label></FONT></TD>
				</TR>
                <TR>
					<TD><FONT face="宋体">&nbsp;是否为实名认证用户：</FONT>&nbsp;&nbsp;<asp:label id="lb_c4" runat="server"></asp:label></FONT></TD>
				</TR>
                <TR>
					<TD><FONT face="宋体">&nbsp;是否为转账白名单：</FONT>&nbsp;&nbsp;<asp:label id="lb_c5" runat="server"></asp:label></FONT></TD>
				</TR>
                <TR>
					<TD><FONT face="宋体">&nbsp;免费流量：</FONT>&nbsp;&nbsp;<asp:label id="lb_c6" runat="server"></asp:label></FONT></TD>
				</TR>
                <TR>
					<TD><FONT face="宋体">&nbsp;是否为提现白名单：</FONT>&nbsp;&nbsp;<asp:label id="lb_c7" runat="server"></asp:label></FONT></TD>
				</TR>
            </TABLE>
            </form>
	</body>
</HTML>
