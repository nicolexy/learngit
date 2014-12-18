<%@ Page language="c#" Codebehind="ComplainBussinessDetail.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.TradeManage.ComplainBussinessDetail" %>
<%@ Import Namespace="TENCENT.OSS.CFT.KF.KF_Web.classLibrary" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>SysBulletinManage_Detail</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); UNKNOWN { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
		<script src="../SCRIPTS/Local.js"></script>
		
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE id="Table1" style="Z-INDEX: 101; LEFT: 15%; WIDTH: 70%; POSITION: absolute; TOP: 8%"
				cellSpacing="1" cellPadding="1" border="1">
				<TR>
					<TD colSpan="2" style="HEIGHT: 25px"><IMG height="16" src="../IMAGES/Page/post.gif" width="15">&nbsp;
						<asp:Label id="labTitle" runat="server" ForeColor="Red">新增商户</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:label id="Label1" runat="server" ForeColor="Red" Width="73px"></asp:label></TD>
				</TR>
				
				<TR>
					<TD align="left" width="15%">
						<asp:Label id="Label2" runat="server">商户号码：</asp:Label></TD>
					<TD align="left">
						<asp:TextBox id="bussNumber" runat="server" Width="300px"></asp:TextBox>
						<asp:RequiredFieldValidator id="RequiredFieldValidator2" runat="server" ErrorMessage="请输入" ControlToValidate="bussNumber"></asp:RequiredFieldValidator></TD>
				</TR>
				<TR>
					<TD align="left" width="15%">
						<asp:Label id="Label3" runat="server">商户名称：</asp:Label></TD>
					<TD align="left">
						<asp:TextBox id="bussName" runat="server" Width="300px"></asp:TextBox>
						<asp:RequiredFieldValidator id="RequiredFieldValidator3" runat="server" ErrorMessage="请输入" ControlToValidate="bussName"></asp:RequiredFieldValidator></TD>
				</TR>
				<TR>
					<TD align="left" width="15%">
						<asp:Label id="Label4" runat="server">通知邮箱：</asp:Label></TD>
					<TD align="left">
						<asp:TextBox id="bussEmail" runat="server" Width="300px"></asp:TextBox>
						<asp:RequiredFieldValidator id="RequiredFieldValidator4" runat="server" ErrorMessage="请输入" ControlToValidate="bussEmail"></asp:RequiredFieldValidator></TD>
				</TR>
				<TR>
					<TD align="center" colspan="2">
						<asp:Button id="btnSave" runat="server" Width="80px" Text="提交" onclick="btnSave_Click"></asp:Button>
						&nbsp;<input type="button" name="btn_back" value="返回" style="WIDTH: 80px; HEIGHT: 22px" onclick="history.go(-1)" />
					</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
